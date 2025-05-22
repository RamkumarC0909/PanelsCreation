using Autodesk.AutoCAD.DatabaseServices;
using PanelsCreation.Geometry;

namespace PanelsCreation.AcadComponent
{
    public static class TableHandler
    {
        public static ObjectId DrawTable(List<List<string>> texts, Point location,
           Transaction? trOptional = null, string? layer = null)
        {
            Transaction tr = trOptional ?? AcadCommunication.NewWorkingDocTransaction;

            Table table = new Table();
            table.SetSize(texts.Count, 4);
            table.Position = AcadGeometry.ToPoint3d(location).Value;
            table.SetColumnWidth(3);

            for (int i = 0; i < texts.Count; i++)
            {
                List<string> text = texts[i];
                for (int j = 0; j < text.Count; j++)
                {
                    table.SetTextString(i, j, text[j]);
                }
            }

            ObjectId id = AcadCommunication.AppendEntity(table, tr, "0");

            if (trOptional == null) { tr.Commit(); tr.Dispose(); }
            return id;
        }
    }
}
