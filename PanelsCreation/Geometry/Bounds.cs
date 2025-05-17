namespace PanelsCreation.Geometry
{
    public class Bounds
    {
        public double MinX { get; }
        public double MinY { get; }
        public double MaxX { get; }
        public double MaxY { get; }

        public Point TopLeft { get { return new Point(MinX, MaxY); } }
        public Point TopRight { get { return new Point(MaxX, MaxY); } }
        public Point BottomLeft { get { return new Point(MinX, MinY); } }
        public Point BottomRight { get { return new Point(MaxX, MinY); } }

        public Point TopCenter { get { return new Point((MaxX + MinX) / 2, MaxY); } }
        public Point BottomCenter { get { return new Point((MaxX + MinX) / 2, MinY); } }
        public Point LeftCenter { get { return new Point(MinX, (MaxY + MinY) / 2); } }
        public Point RightCenter { get { return new Point(MaxX, (MaxY + MinY) / 2); } }

        public Point Center { get { return new Point((MaxX + MinX) / 2, (MaxY + MinY) / 2); } }
        public double Width { get { return MaxX - MinX; } }
        public double Height { get { return MaxY - MinY; } }

        public Bounds(double minX, double minY, double maxX, double maxY)
        {
            MinX = minX;
            MinY = minY;
            MaxX = maxX;
            MaxY = maxY;
        }

        public Bounds(Point centerPoint, double width, double height)
        {
            MinX = centerPoint.X - width / 2;
            MinY = centerPoint.Y - height / 2;
            MaxX = centerPoint.X + width / 2;
            MaxY = centerPoint.Y + height / 2;
        }

        public static Bounds GetBounds(List<Bounds> boundsList)
        {
            if (boundsList == null) return null;

            List<Bounds> cleanList = new List<Bounds>();
            foreach (Bounds b in boundsList)
            {
                if (b != null) cleanList.Add(b);
            }
            if (cleanList.Count == 0) return null;

            Bounds combinedBounds = cleanList[0];
            foreach (Bounds Point in cleanList) combinedBounds = combinedBounds.ExtendBy(Point);

            return combinedBounds;
        }

        public Bounds ExtendBy(Bounds other)
        {
            if (other == null) return this;

            double minX = Math.Min(MinX, other.MinX);
            double minY = Math.Min(MinY, other.MinY);
            double maxX = Math.Max(MaxX, other.MaxX);
            double maxY = Math.Max(MaxY, other.MaxY);

            return new Bounds(minX, minY, maxX, maxY);
        }

        public Bounds ExtendBy(Point Point)
        {
            if (Point == null) return this;

            double minX = Math.Min(MinX, Point.X);
            double minY = Math.Min(MinY, Point.Y);
            double maxX = Math.Max(MaxX, Point.X);
            double maxY = Math.Max(MaxY, Point.Y);

            return new Bounds(minX, minY, maxX, maxY);
        }

        public bool Overlaps(Bounds other)
        {
            return other != null &&
                MaxX >= other.MinX && MinX <= other.MaxX && MaxY >= other.MinY && MinY <= other.MaxY;
        }

        public bool Contains(Bounds other)
        {
            return other != null &&
                MinX <= other.MinX && MaxX >= other.MaxX && MinY <= other.MinY && MaxY >= other.MaxY;
        }

        public bool Contains(Point Point)
        {
            return Point != null && MaxX >= Point.X && MinX <= Point.X && MaxY >= Point.Y && MinY <= Point.Y;
        }

        public bool AreBoundsOverlapping(Bounds other)
        {
            return !(MaxX <= other.MinX ||
             other.MaxX <= MinX ||
             MaxY <= other.MinY ||
             other.MaxY <= MinY);
        }

        public Bounds AddMargins(double margin)
        {
            double newWidth = Math.Max(Width + 2 * margin, 0);
            double newHeight = Math.Max(Height + 2 * margin, 0);

            return new Bounds(Center, newWidth, newHeight);
        }

    }

}
