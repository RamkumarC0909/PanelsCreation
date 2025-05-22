using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
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

                using (Transaction tr = AcadCommunication.NewWorkingDocTransaction)
                {

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

                    List<List<string>> list = [];
                    list.Add(["S.No", "Panel Name", "Count", "Width - Height"]);
                    int i = 1;
                    foreach (var text in texts)
                    {
                        list.Add([i.ToString(), text.Item1, text.Item3.ToString(),
                        $"{Math.Round(texts[i - 1].Item2.Width, 2)} - {Math.Round(texts[i - 1].Item2.Height, 2)}"]);
                        i++;
                    }
                    TableHandler.DrawTable(list, new(0, 0), tr, "0");

                    tr.Commit();
                }
            }
        }
    }
}
