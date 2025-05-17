using Autodesk.AutoCAD.DatabaseServices;

namespace PanelsCreation.AcadComponent
{
    public class AttributeHandler
    {
        public static Dictionary<string, string> GetBlockAttributeTags(BlockReference? block)
        {
            try
            {
                if (block == null) return [];
                Dictionary<string, string> attributes = [];
                using (Transaction tr = AcadCommunication.NewWorkingDocTransaction)
                {
                    AttributeCollection atts = block.AttributeCollection;
                    for (int i = 0; i < atts.Count; i++)
                    {
                        var att = (AttributeReference)tr.GetObject(atts[i], OpenMode.ForRead);
                        attributes[att.Tag] = att.TextString;
                    }
                    return attributes;
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException("Error loading attributes: " + e.Message);
            }
        }
    }
}
