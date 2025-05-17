using Autodesk.AutoCAD.DatabaseServices;
using PanelsCreation.AcadComponent;
using PanelsCreation.Geometry;

namespace PanelsCreation.PanelFactory
{
    /// <summary>
    /// Class for collecting and managing lines from different sources
    /// </summary>
    public class LineCollector
    {
        public List<GLine> VerticalLines { get; private set; } = new List<GLine>();
        public List<GLine> HorizontalLines { get; private set; } = new List<GLine>();
        public List<GLine> OpeningVerticalLines { get; private set; } = new List<GLine>();
        public List<GLine> OpeningHorizontalLines { get; private set; } = new List<GLine>();
        public Bounds? OpeningBounds { get; set; }

        public void AddLine(Line line)
        {
            GLine gLine = AcadGeometry.ToGLine2D(line);
            if (gLine.IsHorizontal)
                HorizontalLines.Add(gLine);
            else if (gLine.IsVertical)
                VerticalLines.Add(gLine);
        }

        public void MergeLinesFromOpenings()
        {
            // Merge horizontal lines from openings if they don't overlap with existing lines
            foreach (var openingLine in OpeningHorizontalLines)
            {
                if (!HorizontalLines.Any(line => openingLine.IsInLine(line)))
                {
                    HorizontalLines.Add(openingLine);
                }
            }

            // Merge vertical lines from openings if they don't overlap with existing lines
            foreach (var openingLine in OpeningVerticalLines)
            {
                if (!VerticalLines.Any(line => openingLine.IsInLine(line)))
                {
                    VerticalLines.Add(openingLine);
                }
            }
        }

        public void SortLines()
        {
            // Sort horizontal lines by Y coordinate
            HorizontalLines.Sort((a, b) => a.StartPoint.Y.CompareTo(b.StartPoint.Y));

            // Sort vertical lines by X coordinate
            VerticalLines.Sort((a, b) => a.StartPoint.X.CompareTo(b.StartPoint.X));
        }
    }
}
