using Autodesk.AutoCAD.DatabaseServices;
using PanelsCreation.AcadComponent;
using PanelsCreation.DrawSettings;
using PanelsCreation.Geometry;
using PanelsCreation.Interfaces;

namespace PanelsCreation.PanelFactory
{
    /// <summary>
    /// Class for rendering drawings
    /// </summary>
    public class DrawingRenderer : IDrawingRenderer
    {
        public void RenderPanels(List<Bounds> panels, Transaction tr)
        {
            if (panels == null || panels.Count == 0) return;

            Dictionary<PanelDimensions, int> panelTypeMap = CreatePanelTypeMap(panels);
            double textHeight = panels.Max(panel => panel.Height) * 0.08;

            BlockReference? blockReference = SelectionSetsHandler.SelectObjectInLayer1().Find(x => x.Name == FileSettings.DetTitle);
            AttributeHandler.GetBlockAttributeTags(blockReference).TryGetValue("DET", out string? value);
            if (string.IsNullOrEmpty(value)) value = "0";

            foreach (var panel in panels)
            {
                // Draw the panel rectangle
                PolylineHandler.DrawRectangle(panel, 0.01, LayerSettings.Panel, tr);

                // Add panel numbering
                int b = GetPanelTypeNumber(panel, panelTypeMap);
                TextHandler.DrawTextBox($"{value}-{b}", textHeight,
                    panel.Center, AttachmentPoint.MiddleCenter, tr, LayerSettings.Text);
            }

        }

        private Dictionary<PanelDimensions, int> CreatePanelTypeMap(List<Bounds> panels)
        {
            Dictionary<PanelDimensions, int> panelTypeMap = new Dictionary<PanelDimensions, int>();
            int typeNumber = 1;

            foreach (var panel in panels)
            {
                var dimensions = new PanelDimensions(panel.Width, panel.Height);

                if (!panelTypeMap.ContainsKey(dimensions))
                {
                    panelTypeMap.Add(dimensions, typeNumber++);
                }
            }

            return panelTypeMap;
        }

        private int GetPanelTypeNumber(Bounds panel, Dictionary<PanelDimensions, int> panelTypeMap)
        {
            var dimensions = new PanelDimensions(panel.Width, panel.Height);
            return panelTypeMap.TryGetValue(dimensions, out int typeNumber) ? typeNumber : 0;
        }
    }
}
