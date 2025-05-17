namespace PanelsCreation.Geometry
{
    public class Point
    {
        public double X { get; }
        public double Y { get; }
        public double Z { get; }

        public Point Flat { get { return new Point(X, Y, 0); } }
        public bool IsZeroLength { get { return X == 0 && Y == 0 && Z == 0; } }
        public double Length { get { return Math.Sqrt(X * X + Y * Y + Z * Z); } }
        public double Angle { get { return Math.Atan2(Y, X); } }
        public Point UnitVector { get { return IsZeroLength ? this : DivideBy(Length); } }
        public static Point ZeroVector { get { return new Point(0, 0, 0); } }

        public Point(double x, double y, double z = 0)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Point GetVectorTo(Point endPoint)
        {
            if (endPoint == null) return new Point(0, 0);

            return new Point(endPoint.X - X, endPoint.Y - Y, endPoint.Z - Z);
        }

        public double DistanceSqTo(Point other, bool ignoreZ = false)
        {
            Point dist = GetVectorTo(other);

            if (ignoreZ) return dist.X * dist.X + dist.Y * dist.Y;
            return dist.X * dist.X + dist.Y * dist.Y + dist.Z * dist.Z;
        }

        public Point MultiplyBy(double scaling)
        {
            return new Point(X * scaling, Y * scaling, Z * scaling);
        }

        public Point DivideBy(double divisor)
        {
            if (divisor == 0) return this;

            return new Point(X / divisor, Y / divisor, Z / divisor);
        }

        public Point Add(Point vectorToAdd)
        {
            return new Point(X + vectorToAdd.X, Y + vectorToAdd.Y, Z + vectorToAdd.Z);
        }

        public Point Add(double x, double y, double z = 0)
        {
            return new Point(X + x, Y + y, Z + z);
        }

        public Point Subtract(Point vectorToSubtract)
        {
            return new Point(X - vectorToSubtract.X, Y - vectorToSubtract.Y, Z - vectorToSubtract.Z);
        }

        public double Dot(Point other)
        {
            return X * other.X + Y * other.Y + Z * other.Z;
        }

        public double CrossZ(Point other)
        {
            return X * other.Y - Y * other.X;
        }

    }
}
