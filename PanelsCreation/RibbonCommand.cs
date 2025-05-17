using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.Windows;
using PanelsCreation.AcadComponent;
using PanelsCreation.RibbonCreatorHelper;
using System.Windows;
using Exception = System.Exception;

namespace PanelsCreation
{
    public class RibbonCommand
    {
        [CommandMethod("CreateRibbon")]
        [CommandMethod("CR")]
        public void CreateRibbon()
        {
            try
            {
                Ribbon();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Panel Design Ribbon creation failed. Turn on Ribbon first and" +
                    " then restart AutoCAD application...", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Document doc = AcadCommunication.WorkingDocument;
                doc.SendStringToExecute("RIBBON ", true, false, false);
            }
        }

        private void Ribbon()
        {
            RibbonControl ribbon = ComponentManager.Ribbon;

            RibbonTab ribbonTab = RibbonCreator.GetRibbonTab("Panel Design", "PanelDesign");
            ribbon.Tabs.Add(ribbonTab);

            // User Panel
            RibbonPanelSource panelSource = RibbonCreator.GetRibbonPanelSource("User", "User-Panel");

            RibbonButton button = RibbonCreator.GetRibbonButton("Log in", "Log in ",
                "LogIn", "Resources\\login.png", "Resources\\login.png");
            panelSource.Items.Add(button);

            button = RibbonCreator.GetRibbonButton("Log out", " Log out ",
                "LogOut", "Resources\\logout.png", "Resources\\logout.png");
            panelSource.Items.Add(button);

            RibbonPanel panel = RibbonCreator.GetRibbonPanel(panelSource);
            ribbonTab.Panels.Add(panel);

            // Panel Creation
            panelSource = RibbonCreator.GetRibbonPanelSource("Panel Creation", "PanelCreation");

            button = RibbonCreator.GetRibbonButton("Draw Panel Design", "DrawPanelDesign",
                "DrawPanel-Design", "Resources\\validate.png", "Resources\\validate.png");
            panelSource.Items.Add(button);

            panel = RibbonCreator.GetRibbonPanel(panelSource);
            ribbonTab.Panels.Add(panel);

        }
    }
}
