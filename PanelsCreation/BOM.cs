using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using PanelsCreation.AcadComponent;
using PanelsCreation.DrawSettings;

namespace PanelsCreation
{
    public class BOM
    {
        [CommandMethod("BOM", CommandFlags.Session)]
        public static void Bom()
        {
            ObjectIdCollection collection = SelectionSetsHandler.SelectObjectInLayer(LayerSettings.Text);
            using (DocumentLock docLock = AcadCommunication.NewWorkingDocLock)
            {
                Transaction tr = AcadCommunication.NewWorkingDocTransaction;

                Dictionary<string, int> texts = [];
                foreach (ObjectId id in collection)
                {
                    Entity entity = (Entity)tr.GetObject(id, OpenMode.ForRead);
                    if (entity != null && entity is DBText txt)
                    {
                        string text = txt.TextString;
                        if (texts.ContainsKey(text))
                        {
                            texts[text] = texts[text] + 1;
                        }
                        else
                        {
                            texts.Add(text, 1);
                        }
                    }
                }


                Table table = new Table();
                table.SetSize(texts.Count + 1, 3);

                table.SetTextString(0, 0, "S.No");
                table.SetTextString(0, 1, "Panel Name");
                table.SetTextString(0, 2, "Count");
                table.Position = new Point3d(0, 0, 0);
                for (int i = 1; i < texts.Count + 1; i++)
                {

                    table.SetTextString(i, 0, i.ToString());
                    table.SetTextString(i, 1, texts.ElementAt(i - 1).Key);
                    table.SetTextString(i, 2, texts.ElementAt(i - 1).Value.ToString());
                }

                AcadCommunication.AppendEntity(table, tr, "0");
                tr.Commit();
                tr.Dispose();
            }
        }
    }
}
