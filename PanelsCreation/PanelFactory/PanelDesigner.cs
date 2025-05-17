using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using PanelsCreation.AcadComponent;
using PanelsCreation.DrawSettings;
using PanelsCreation.Geometry;
using PanelsCreation.Interfaces;

namespace PanelsCreation.PanelFactory
{
    /// <summary>
    /// PanelCreationCommand class for designing panels based on elevation boundaries
    /// </summary>
    public class PanelDesigner : IPanelDesigner
    {
        private readonly IGeometryProcessor _geometryProcessor;
        private readonly IDrawingRenderer _drawingRenderer;

        public PanelDesigner()
        {
            _geometryProcessor = new GeometryProcessor();
            _drawingRenderer = new DrawingRenderer();
        }

        public void CreatePanelDesign()
        {
            using (DocumentLock docLock = AcadCommunication.NewWorkingDocLock)
            {
                // Initialize line collections
                var lineCollector = new LineCollector();

                // Get elevation boundaries and collect lines
                ObjectIdCollection elevationObjects = SelectionSetsHandler.SelectObjectInLayer
                    (LayerSettings.ElevationBoundary);
                foreach (ObjectId objectId in elevationObjects)
                {
                    using (Transaction tr = AcadCommunication.NewWorkingDocTransaction)
                    {
                        Entity entity = (Entity)tr.GetObject(objectId, OpenMode.ForRead);
                        if (entity is Polyline polyline)
                        {
                            // Get bounds and process inner objects
                            Bounds elevationBounds = EntityTransformer.GetBounds(polyline.ObjectId, tr);
                            ProcessInnerObjects(elevationBounds, lineCollector, tr);

                            // Merge lines and find intersections
                            lineCollector.MergeLinesFromOpenings();
                            lineCollector.SortLines();

                            // Find intersection points and create bounding boxes
                            List<Point> intersectionPoints = _geometryProcessor.FindIntersectionPoints(
                                lineCollector.HorizontalLines, lineCollector.VerticalLines);

                            List<Bounds> panels = _geometryProcessor.CreateBoundingBoxes(
                                intersectionPoints, lineCollector.OpeningBounds);

                            // Render the panels
                            _drawingRenderer.RenderPanels(panels, tr);
                        }

                        tr.Commit();
                    }
                }
            }
        }

        private void ProcessInnerObjects(Bounds bounds, LineCollector lineCollector, Transaction tr)
        {
            ObjectIdCollection innerObjects = SelectionSetsHandler.SelectObjectWithInBounds(bounds, tr);
            foreach (ObjectId innerObjectId in innerObjects)
            {
                Entity innerEntity = (Entity)tr.GetObject(innerObjectId, OpenMode.ForWrite);
                if (innerEntity is Line line)
                    lineCollector.AddLine(line);
                else if (innerEntity is Polyline poly)
                    ProcessPolyline(poly, lineCollector, tr);
            }
        }

        private void ProcessPolyline(Polyline polyline, LineCollector lineCollector, Transaction transaction)
        {
            (List<GLine> horizontalLines, List<GLine> verticalLines) = _geometryProcessor.ExtractLinesFromPolyline(polyline);

            if (polyline.Layer == LayerSettings.Opening)
            {
                lineCollector.OpeningVerticalLines.AddRange(verticalLines);
                lineCollector.OpeningHorizontalLines.AddRange(horizontalLines);
                lineCollector.OpeningBounds = EntityTransformer.GetBounds(polyline.ObjectId, transaction);
            }
            else if (polyline.Layer == LayerSettings.Border)
            {
                lineCollector.HorizontalLines.AddRange(horizontalLines);
                lineCollector.VerticalLines.AddRange(verticalLines);
            }
        }
    }
}
