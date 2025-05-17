using Autodesk.AutoCAD.DatabaseServices;
using PanelsCreation.Geometry;

namespace PanelsCreation.AcadComponent
{
    internal class EntityTransformer
    {
        public static Bounds GetBounds(ObjectId id, Transaction? trOptional = null)
        {
            return GetBounds(new ObjectIdCollection() { id }, trOptional);
        }

        public static Bounds GetBounds(ObjectIdCollection objectIDs, Transaction? trOptional = null)
        {
            Transaction tr = trOptional ?? AcadCommunication.NewWorkingDocTransaction;

            List<Bounds> boundsList = new List<Bounds>();

            foreach (ObjectId id in objectIDs)
            {
                try
                {
                    Entity e = (Entity)tr.GetObject(id, OpenMode.ForRead);
                    Table t = e as Table;
                    BlockReference br = e as BlockReference;

                    Extents3d? eBounds = e.Bounds;
                    if (eBounds != null) boundsList.Add(AcadGeometry.ToBounds(eBounds.Value));
                }
                catch { } //skip object if bounds retrieval failed
            }

            if (trOptional == null) { tr.Commit(); tr.Dispose(); } //commit tr if created (not provided)

            return Bounds.GetBounds(boundsList); //merged bounds
        }

    }
}
