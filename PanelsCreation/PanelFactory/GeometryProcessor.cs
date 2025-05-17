using Autodesk.AutoCAD.DatabaseServices;
using PanelsCreation.AcadComponent;
using PanelsCreation.Geometry;
using PanelsCreation.Interfaces;

namespace PanelsCreation.PanelFactory
{
    /// <summary>
    /// Class for processing geometry operations
    /// </summary>
    public class GeometryProcessor : IGeometryProcessor
    {
        public (List<GLine>, List<GLine>) ExtractLinesFromPolyline(Polyline polyline)
        {
            List<GLine> horizontalLines = new List<GLine>();
            List<GLine> verticalLines = new List<GLine>();

            int vertexCount = polyline.NumberOfVertices;
            Point previousPoint = AcadGeometry.ToPoint(polyline.GetPoint2dAt(vertexCount - 1));
            for (int i = 0; i < vertexCount; i++)
            {
                Point currentPoint = AcadGeometry.ToPoint(polyline.GetPoint2dAt(i));
                GLine line = new GLine(previousPoint, currentPoint);

                if (line.IsHorizontal)
                    horizontalLines.Add(line); // Check if the line is horizontal and add it to the list
                else if (line.IsVertical)
                    verticalLines.Add(line); // Check if the line is vertical and add it to the list
                previousPoint = currentPoint;
            }
            return (horizontalLines, verticalLines);
        }

        public List<Point> FindIntersectionPoints(List<GLine> horizontalLines, List<GLine> verticalLines)
        {
            List<Point> intersectionPoints = new List<Point>();
            foreach (GLine horizontalLine in horizontalLines)
            {
                foreach (GLine verticalLine in verticalLines)
                {
                    if (horizontalLine.IntersectsXY(verticalLine))
                    {
                        intersectionPoints.Add(horizontalLine.IntersectionXY(verticalLine, true));
                    }
                }
            }
            return intersectionPoints;
        }

        public List<Bounds> CreateBoundingBoxes(List<Point> intersectionPoints, Bounds? openingBounds)
        {
            List<Bounds> boundingBoxes = new List<Bounds>();

            for (int i = 0; i < intersectionPoints.Count; i++)
            {
                Point currentPoint = intersectionPoints[i];
                List<Point> upcomingPoints = intersectionPoints.GetRange(i + 1, intersectionPoints.Count - (i + 1));
                Point? nearestHorizontalPoint = FindNearestHorizontalPoint(currentPoint, upcomingPoints);

                if (nearestHorizontalPoint != null)
                {
                    double horizontalDistance = currentPoint.DistanceSqTo(nearestHorizontalPoint);
                    Point? nearestVerticalPoint = FindNearestVerticalPoint(currentPoint,
                        upcomingPoints, horizontalDistance);

                    if (nearestVerticalPoint != null)
                    {
                        Bounds bounds = new Bounds(currentPoint.X, currentPoint.Y,
                            nearestHorizontalPoint.X, nearestVerticalPoint.Y);

                        // Add the bounding box if it doesn't overlap with an opening
                        if (openingBounds == null || !openingBounds.AreBoundsOverlapping(bounds))
                            boundingBoxes.Add(bounds);
                    }
                }
            }

            return boundingBoxes;
        }

        private Point? FindNearestHorizontalPoint(Point referencePoint, List<Point> points)
        {
            Point? nearestPoint = null;
            double shortestDistance = 0;

            foreach (Point point in points)
            {
                if (point.Y == referencePoint.Y)
                {
                    double distance = referencePoint.DistanceSqTo(point);

                    if (shortestDistance == 0 || distance < shortestDistance)
                    {
                        shortestDistance = distance;
                        nearestPoint = point;
                    }
                }
            }

            return nearestPoint;
        }

        private Point? FindNearestVerticalPoint(Point referencePoint, List<Point> points, double horizontalDistance)
        {
            Point? nearestPoint = null;
            double shortestDistance = 0;

            foreach (Point point in points)
            {
                if (point.X == referencePoint.X)
                {
                    double distance = referencePoint.DistanceSqTo(point);
                    if (shortestDistance == 0 || distance < shortestDistance)
                    {
                        // Check if we can form a complete rectangle
                        if (points.Any(p =>
                            Math.Abs(p.X - (point.X + Math.Sqrt(horizontalDistance))) < 0.001 &&
                            Math.Abs(p.Y - point.Y) < 0.001))
                        {
                            shortestDistance = distance;
                            nearestPoint = point;
                        }
                    }
                }
            }

            return nearestPoint;
        }
    }
}
