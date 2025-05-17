using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using PanelsCreation.Geometry;

namespace PanelsCreation.AcadComponent
{
    internal class AcadGeometry
    {
        public static Point2d? ToPoint2d(Point vector)
        {
            if (vector == null) return null;

            return new Point2d(vector.X, vector.Y);
        }

        public static Point3d? ToPoint3d(Point vector)
        {
            if (vector == null) return null;

            return new Point3d(vector.X, vector.Y, vector.Z);
        }

        public static Vector2d? ToVector2d(Point vector)
        {
            if (vector == null) return null;

            return new Vector2d(vector.X, vector.Y);
        }

        public static Vector3d? ToVector3d(Point vector)
        {
            if (vector == null) return null;

            return new Vector3d(vector.X, vector.Y, vector.Z);
        }

        public static Point ToPoint(Point2d Point)
        {
            return new Point(Point.X, Point.Y);
        }

        public static Point ToPoint(Point3d Point)
        {
            return new Point(Point.X, Point.Y, Point.Z);
        }

        public static Point ToPoint(Vector2d vector)
        {
            return new Point(vector.X, vector.Y);
        }

        public static Point ToPoint(Vector3d vector)
        {
            return new Point(vector.X, vector.Y, vector.Z);
        }

        public static Extents2d? ToExtents2d(Bounds b)
        {
            if (b == null) return null;

            return new Extents2d(new Point2d(b.MinX, b.MinY), new Point2d(b.MaxX, b.MaxY));
        }

        public static Bounds ToBounds(Extents3d ex)
        {
            return new Bounds(ex.MinPoint.X, ex.MinPoint.Y, ex.MaxPoint.X, ex.MaxPoint.Y);
        }

        public static GLine ToGLine2D(Line line)
        {
            if (line == null) return null;
            return new GLine(new Point(line.StartPoint.X, line.StartPoint.Y),
                new Point(line.EndPoint.X, line.EndPoint.Y));
        }

    }
}
