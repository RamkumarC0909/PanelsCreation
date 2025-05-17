using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using PanelsCreation.Geometry;


namespace PanelsCreation.AcadComponent
{
    internal class SelectionSetsHandler
    {
        public static ObjectIdCollection SelectObjectWithInBounds(
            Bounds selectionArea, Transaction tr)
        {
            ObjectIdCollection idCollection = [];
            BlockTableRecord btr = AcadCommunication.GetModelSpace(tr, true);
            foreach (ObjectId objectId in btr)
            {
                Bounds bounds = EntityTransformer.GetBounds(objectId, tr);
                if (selectionArea.Contains(bounds))
                    idCollection.Add(objectId);
            }
            return idCollection;
        }

        public static ObjectIdCollection SelectObjectInLayer(string layerName)
        {
            TypedValue[] filterList = [new TypedValue((int)DxfCode.LayerName, layerName)];
            ObjectIdCollection collection = new();

            PromptSelectionResult selRes = AcadCommunication.WorkingEditor.
                SelectAll(new SelectionFilter(filterList));
            if (selRes.Status != PromptStatus.OK) return collection;
            foreach (SelectedObject selObj in selRes.Value)
                collection.Add(selObj.ObjectId);

            return collection;
        }

        public static List<BlockReference> SelectObjectInLayer1()
        {
            TypedValue[] filterList = [new TypedValue((int)DxfCode.Start, "INSERT")];
            ObjectIdCollection collection = new();
            List<BlockReference> blocks = [];
            PromptSelectionResult selRes = AcadCommunication.WorkingEditor.
                SelectAll(new SelectionFilter(filterList));
            if(selRes.Status != PromptStatus.OK) return blocks;
            foreach (SelectedObject selObj in selRes.Value)
            {
                Entity entity = (Entity)selObj.ObjectId.GetObject(OpenMode.ForWrite);
                if(entity is BlockReference) blocks.Add((BlockReference)entity);
            }
            return blocks;
        }
    }
}
