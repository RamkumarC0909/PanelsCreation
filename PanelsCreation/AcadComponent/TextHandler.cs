using Autodesk.AutoCAD.DatabaseServices;
using PanelsCreation.Geometry;

namespace PanelsCreation.AcadComponent
{
    internal class TextHandler
    {
        internal static ObjectId DrawTextBox(string text, double textHeight, Point location,
           AttachmentPoint anchorPoint = AttachmentPoint.BottomLeft, Transaction? trOptional = null,
           string? layer = null)
        {
            Transaction tr = trOptional ?? AcadCommunication.NewWorkingDocTransaction;

            DBText label = new DBText();
            label.TextString = text;
            label.Height = textHeight;
            label.Justify = anchorPoint; 
            label.AlignmentPoint = AcadGeometry.ToPoint3d(location).Value;
            
            ObjectId id = AcadCommunication.AppendEntity(label, tr, layer);

            if (trOptional == null) { tr.Commit(); tr.Dispose(); }
            return label.ObjectId;
        }
    }
}
