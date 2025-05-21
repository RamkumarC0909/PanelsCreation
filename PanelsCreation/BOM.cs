using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using PanelsCreation.AcadComponent;
using PanelsCreation.DrawSettings;
using PanelsCreation.Geometry;
using PanelsCreation.PanelFactory;

namespace PanelsCreation
{
    public class BOM
    {
        [CommandMethod("BOM", CommandFlags.Session)]
        public static void Bom()
        {
            ObjectIdCollection collection = SelectionSetsHandler.SelectObjectInLayer(LayerSettings.Panel);
            using (DocumentLock docLock = AcadCommunication.NewWorkingDocLock)
            {
                Transaction tr = AcadCommunication.NewWorkingDocTransaction;

                List<(string, PanelDimensions, int)> texts = [];
                foreach (ObjectId id in collection)
                {
                    Entity entity = (Entity)tr.GetObject(id, OpenMode.ForRead);
                    if (entity != null && entity is Polyline poly)
                    {
                        Bounds bounds = EntityTransformer.GetBounds(id);
                        PanelDimensions panelDimensions = new(bounds.Width, bounds.Height);
                        ObjectIdCollection objectIds = SelectionSetsHandler.SelectObjectWithInBounds(bounds, tr);
                        if (objectIds.Count == 2) //one for panel one for text
                        {
                            Entity ent = (Entity)tr.GetObject(objectIds[0], OpenMode.ForRead);
                            Entity ent1 = (Entity)tr.GetObject(objectIds[1], OpenMode.ForRead);
                            DBText? txt = null;
                            if (ent is DBText) txt = (DBText)ent;
                            else if (ent1 is DBText) txt = (DBText)ent1;
                            if (txt != null)
                            {
                                string text = txt.TextString;
                                if (texts.Any(x => x.Item1 == text))
                                {
                                    int a = texts.IndexOf(texts.Find(x => x.Item1 == text));
                                    texts[a] = (texts[a].Item1, texts[a].Item2, texts[a].Item3 + 1);
                                }
                                else
                                {
                                    texts.Add((text, panelDimensions, 1));
                                }
                            }
                        }
                    }
                }


                Table table = new Table();
                table.SetSize(texts.Count + 1, 4);

                table.SetTextString(0, 0, "S.No");
                table.SetTextString(0, 1, "Panel Name");
                table.SetTextString(0, 2, "Count");
                table.SetTextString(0, 3, "Width-Height");
                table.Position = new Point3d(0, 0, 0);
                table.SetColumnWidth(3);
                for (int i = 1; i < texts.Count + 1; i++)
                {

                    table.SetTextString(i, 0, i.ToString());
                    table.SetTextString(i, 1, texts[i - 1].Item1);
                    table.SetTextString(i, 2, texts[i - 1].Item3.ToString());
                    table.SetTextString(i, 3, $"{Math.Round(texts[i - 1].Item2.Width, 2)} - {Math.Round(texts[i - 1].Item2.Height, 2)}");
                }

                AcadCommunication.AppendEntity(table, tr, "0");
                tr.Commit();
                tr.Dispose();
            }
        }
    }
}
