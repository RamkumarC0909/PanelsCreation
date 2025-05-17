namespace PanelsCreation.Geometry
{
    public class GLine
    {
        public Point StartPoint { get; }
        public Point EndPoint { get; }
        public Point MidPoint { get; }
        public bool IsHorizontal { get { return StartPoint.Y == EndPoint.Y; } }
        public bool IsVertical { get { return StartPoint.X == EndPoint.X; } }
        public Point DisplacementVector { get { return StartPoint.GetVectorTo(EndPoint); } }
        public double Angle { get { return DisplacementVector.Angle; } }
        public double Length { get { return Math.Round(DisplacementVector.Length, 3); } }
        public Bounds GLineBounds { get { return new Bounds(StartPoint, 0, 0).ExtendBy(EndPoint); } }

        public GLine(Point startPoint, Point endPoint)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
            MidPoint = new Point((StartPoint.X + EndPoint.X) / 2, (StartPoint.Y + EndPoint.Y) / 2, (StartPoint.Z + EndPoint.Z) / 2);
        }

        public bool IsInLine(GLine gLine)
        {
            Point d = DisplacementVector;
            Point n = new Point(d.Y, -d.X);
            Point Point = gLine.StartPoint.GetVectorTo(StartPoint);
            double dot = DisplacementVector.UnitVector.Dot(gLine.DisplacementVector.UnitVector);
            double s = Math.Abs(n.Dot(Point)) / n.Length;
            return (dot == -1 || dot == 1) && s == 0;
        }

        public bool IntersectsXY(GLine other)
        {
            return IntersectionXY(other, true) != null;
        }

        public Point IntersectionXY(GLine other, bool parallelLinesQualify)
        {
            //Let startPoint = A, endPoint = B, otherStartPoint = C, otherEndPoint = D
            //First immediately disqualify all edges whose square bounding boxes don't overlap
            Bounds boxAB = GLineBounds;
            Bounds boxCD = other.GLineBounds;
            if (boxAB.MinX > boxCD.MaxX || boxAB.MaxX < boxCD.MinX ||
                boxAB.MinY > boxCD.MaxY || boxAB.MaxY < boxCD.MinY) return null;

            //We are solving for the coefficients m,n in: A + m AB = C + n CD (m,n must be between 0 and 1)
            //Solution: m = [AC (cross) CD].Z / [AB (cross) CD].Z,  n = [AC (cross) AB].Z / [AB (cross) CD].Z
            Point A = StartPoint;
            Point B = EndPoint;
            Point C = other.StartPoint;
            Point D = other.EndPoint;

            Point AB = A.GetVectorTo(B);
            Point CD = C.GetVectorTo(D);
            Point AC = A.GetVectorTo(C);
            Point AD = A.GetVectorTo(D);
            double ABxCD = AB.CrossZ(CD);
            double ACxAB = AC.CrossZ(AB);

            if (ABxCD == 0) //lines are parallel, case treated separately
            {
                if (ACxAB != 0) return null; //lines don't overlap
                if (!parallelLinesQualify) return null;

                double AB2 = AB.Dot(AB);
                double mC = AC.Dot(AB) / AB2;
                double mD = AD.Dot(AB) / AB2;

                if (mC < 0 && mD < 0) return null; //both C and D are before AB line starts
                if (mC > 1 && mD > 1) return null; //both C and D are after AB line ends

                //lines intersect, return smallest coefficient larger than 0
                //(ensures closest to A than B, but also clamped onto AB)
                return A.Add(AB.MultiplyBy(Math.Max(Math.Min(mC, mD), 0)));
            }

            //lines not parallel, intersection point must fall within both line segments (coefficient between [0,1])
            double ACxCD = AC.CrossZ(CD);
            double m = ACxCD / ABxCD;
            double n = ACxAB / ABxCD;

            if (m < 0 || m > 1 || n < 0 || n > 1) return null; //intersection not on line segments

            return A.Add(AB.MultiplyBy(m));
        }

    }
}
