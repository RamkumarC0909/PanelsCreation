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
    /// Interface for drawing operations
    /// </summary>
    public interface IDrawingRenderer
    {
        void RenderPanels(List<Bounds> panels, Transaction transaction);
    }
}
