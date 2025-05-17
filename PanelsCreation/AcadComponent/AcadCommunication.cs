using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;


namespace PanelsCreation.AcadComponent
{
    internal class AcadCommunication
    {
        public static Document WorkingDocument { get; set; } = ActiveDocument;
        public static Database WorkingDatabase { get { return (WorkingDocument?.Database); } }
        public static Editor WorkingEditor { get { return (WorkingDocument?.Editor); } }
        public static DocumentLock NewWorkingDocLock { get { return (WorkingDocument?.LockDocument()); } }
        public static Transaction NewWorkingDocTransaction { get { return (WorkingDatabase?.TransactionManager.StartTransaction()); } }

        public static Document ActiveDocument { get { return Application.DocumentManager.MdiActiveDocument; } }


        public static BlockTable GetBlockTable(Transaction tr, bool writeAccess)
        {
            OpenMode mode = (writeAccess ? OpenMode.ForWrite : OpenMode.ForRead);
            return (BlockTable)tr.GetObject(WorkingDatabase.BlockTableId, mode);
        }


        public static BlockTableRecord GetModelSpace(Transaction tr, bool writeAccess = true)
        {
            return GetModelSpace(tr, GetBlockTable(tr, false), writeAccess);
        }

        public static BlockTableRecord GetModelSpace(Transaction tr, BlockTable bt, bool writeAccess)
        {
            OpenMode mode = (writeAccess ? OpenMode.ForWrite : OpenMode.ForRead);
            return (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], mode);
        }


        public static ObjectId AppendEntity(Entity e, Transaction tr, string? layer)
        {
            BlockTable bt = (BlockTable)tr.GetObject(WorkingDatabase.BlockTableId, OpenMode.ForRead);
            return AppendEntity(e, tr, bt, layer);
        }

        public static ObjectId AppendEntity(Entity e, Transaction tr, BlockTable bt, string? layer)
        {
            e.Layer = layer == null ? "0" : layer;

            BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
            btr.AppendEntity(e);
            tr.AddNewlyCreatedDBObject(e, true);

            return e.ObjectId;
        }



        /// <summary>
        /// Writes editor message in working document.
        /// Appears in Forge output, but only in full if message ends with a line break.
        /// For this reason, avoid and use <c>LogTraceLine</c> instead.
        /// </summary>
        private static void LogTrace(string format, params object[] args) { WorkingDocument.Editor.WriteMessage(format, args); }

        /// <summary> Writes editor message on working document (appears on Design Automation output). </summary>
        public static void LogTraceLine(string format, params object[] args) { LogTrace(format + "\n", args); }

    }
}
