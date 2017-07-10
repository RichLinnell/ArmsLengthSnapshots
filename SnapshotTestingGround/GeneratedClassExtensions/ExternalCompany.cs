using System.Collections.Generic;
using System.Linq;
using SnapshotTestingGround.Snapshot;

namespace SnapshotTestingGround.ModelClasses
{
    public partial class ExternalCompany : ISnapshotIdentity
    {
        public long SnapshotIdentity { get; set; }
        public long SnapshotObjectId { get; set; }
        public void Snapshot(Writer writer)
        {
            if (SnapshotIdentity == writer.SnapshotIdentity) return;
            SnapshotIdentity = writer.SnapshotIdentity;
            SnapshotObjectId = writer.GetNextObjectId();

            //Collections
            //Dictionaries
            foreach (var user in _users.Values)
            {
                user.Snapshot(writer);
            }

            //Referenced Snapshot Items
            ParentCompany?.Snapshot(writer);

            //Snapshot Object
            writer.WriteClassStart("SnapshotTestingGround.ModelClasses.ExternalCompany");
            writer.WriteProperty("SnapshotObjectId", SnapshotObjectId);
            writer.WriteProperty("_id", _id.WriteSnapshotValue());
            writer.WriteProperty("Name", Name.WriteSnapshotValue());
            writer.WriteArray("Users",
                Users.Select(kp => new KeyValuePair<string, string>(kp.Key.WriteSnapshotValue(),
                    kp.Value.SnapshotObjectId.WriteSnapshotValue()).WriteSnapshotValue()));
            writer.WriteProperty("ParentCompanyId", ParentCompany?.SnapshotObjectId);
            writer.WriteClassEnd();
        }
        public void Hydrate(Dictionary<string, string[]> values, Reader reader)
        {
            _id = _id.ReadSnapshotValue(values["_id"][0]);
            Name = Name.ReadSnapshotValue(values["Name"][0]);
            foreach (var pair in values["Users"])
            {
                var splitPair = pair.Split('|');
                long key = new long();
                key = key.ReadSnapshotValue(splitPair[0]);
                Users.Add(key, reader.GetOrCreate<User>(splitPair[1]));
            }
            ParentCompany = reader.GetOrCreate<ExternalCompany>(values["ParentCompanyId"][0]);
        }
    }
}