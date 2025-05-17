using Autodesk.AutoCAD.DatabaseServices;
using PanelsCreation.Geometry;

namespace PanelsCreation.AcadComponent
{
    internal class PolylineHandler
    {
        internal static ObjectId DrawRectangle(Bounds outline, double thickness,
            string? layer = null, Transaction? trOptional = null, string? layout = null)
        {
            Transaction tr = trOptional ?? AcadCommunication.NewWorkingDocTransaction;

            using (Polyline poly = new Polyline(4) { Closed = true })
            {
                outline = outline.AddMargins(0.01);
                poly.AddVertexAt(0, AcadGeometry.ToPoint2d(outline.TopLeft).Value, 0, 0, 0);
                poly.AddVertexAt(1, AcadGeometry.ToPoint2d(outline.TopRight).Value, 0, 0, 0);
                poly.AddVertexAt(2, AcadGeometry.ToPoint2d(outline.BottomRight).Value, 0, 0, 0);
                poly.AddVertexAt(3, AcadGeometry.ToPoint2d(outline.BottomLeft).Value, 0, 0, 0);
                poly.ConstantWidth = thickness;

                ObjectId id = AcadCommunication.AppendEntity(poly, tr, layer);

                if (trOptional == null) { tr.Commit(); tr.Dispose(); }
                return poly.ObjectId;
            }
        }
    }
}
