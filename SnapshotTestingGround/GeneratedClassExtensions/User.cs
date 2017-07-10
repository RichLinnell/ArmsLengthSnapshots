using System.Collections.Generic;
using SnapshotTestingGround.Snapshot;

namespace SnapshotTestingGround.ModelClasses
{
    public partial class User : ISnapshotIdentity
    {
        public long SnapshotIdentity { get; set; }
        public long SnapshotObjectId { get; set; }

        public void Snapshot(Writer writer)
        {
            //Check if item already snapshot
            if (SnapshotIdentity == writer.SnapshotIdentity) return;
            SnapshotIdentity = writer.SnapshotIdentity;
            SnapshotObjectId = writer.GetNextObjectId();

            //Collections

            //Referenced Snapshot Items
            Company.Snapshot(writer);

            //Snapshot Object
            writer.WriteClassStart("SnapshotTestingGround.ModelClasses.User");
            writer.WriteProperty("SnapshotObjectId", SnapshotObjectId);
            writer.WriteProperty("CompanyId", Company?.SnapshotObjectId);
            writer.WriteProperty("Id", Id.WriteSnapshotValue());
            writer.WriteProperty("Name", Name.WriteSnapshotValue());
            writer.WriteClassEnd();
        }

        public void Hydrate(Dictionary<string, string[]> values, Reader reader)
        {
            Company = reader.GetOrCreate<ExternalCompany>(values["CompanyId"][0]);
            Id = Id.ReadSnapshotValue(values["Id"][0]);
            Name = Name.ReadSnapshotValue(values["Name"][0]);
        }
    }
}