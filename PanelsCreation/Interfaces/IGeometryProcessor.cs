using Autodesk.AutoCAD.DatabaseServices;
using PanelsCreation.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PanelsCreation.Interfaces
{
    /// <summary>
    /// Interface for geometry operations
    /// </summary>
    public interface IGeometryProcessor
    {
        (List<GLine>, List<GLine>) ExtractLinesFromPolyline(Polyline polyline);
        List<Point> FindIntersectionPoints(List<GLine> horizontalLines, List<GLine> verticalLines);
        List<Bounds> CreateBoundingBoxes(List<Point> intersectionPoints, Bounds? openingBounds);
    }
}
