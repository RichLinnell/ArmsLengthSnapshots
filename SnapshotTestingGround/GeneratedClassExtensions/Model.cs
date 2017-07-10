using System.Collections.Generic;
using System.Linq;
using SnapshotTestingGround.Snapshot;

namespace SnapshotTestingGround.ModelClasses
{
    public partial class Model : ISnapshotIdentity
    {
        public long SnapshotIdentity { get; set; }
        public long SnapshotObjectId { get; set; }

        public void Snapshot(Writer writer)
        {
            if (SnapshotIdentity == writer.SnapshotIdentity) return;
            SnapshotIdentity = writer.SnapshotIdentity;
            SnapshotObjectId = writer.GetNextObjectId();

            //Collections
            foreach (var company in ExternalCompanies)
            {
                company.Snapshot(writer);
            }

            //Referenced Snapshot Items

            //Snapshot Object
            writer.WriteClassStart("SnapshotTestingGround.ModelClasses.Model");
            writer.WriteProperty("SnapshotObjectId", SnapshotObjectId);
            writer.WriteArray("ExternalCompanies", ExternalCompanies.Select(e => e.SnapshotObjectId));
            writer.WriteClassEnd();
        }

        public void Hydrate(Dictionary<string, string[]> values, Reader reader)
        {
            ExternalCompanies.AddRange(values["ExternalCompanies"].Select(i => reader.GetOrCreate<ExternalCompany>(i)));
        }
    }
}