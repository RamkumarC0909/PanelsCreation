using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using PanelsCreation.AcadComponent;
using PanelsCreation.PanelFactory;
using PanelsCreation.UserInterface;
using System.IO;


namespace PanelsCreation
{
    /// <summary>
    /// PanelCreationCommand entry point for panel creation commands
    /// </summary>
    public class PanelCreationCommand
    {
        public static bool IsLoggedIn { get; set; } = false;

        [CommandMethod("DrawPanelDesign", CommandFlags.Session)]
        public static void DrawIncomingFullDesign()
        {
            if (!IsLoggedIn)
            {
                LoginUI login = new LoginUI();
                login.ShowDialog();
                return;
            }

            // Initialize document and transaction
            AcadCommunication.WorkingDocument = AcadCommunication.ActiveDocument;

            // Create a panel designer to handle the design process
            var panelDesigner = new PanelDesigner();
            panelDesigner.CreatePanelDesign();

            // Save the output file
            string filePath = AcadCommunication.WorkingDocument.Name;
            string outputFilePath = Path.Combine(Path.GetDirectoryName(filePath), "E001 - Sample Output.dwg");
            if (File.Exists(outputFilePath)) File.Delete(outputFilePath);
            
            // Save the current drawing as a new file
            AcadCommunication.WorkingDatabase.SaveAs(outputFilePath, DwgVersion.Current);
        }
    }
}
