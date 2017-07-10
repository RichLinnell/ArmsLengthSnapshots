using System.Collections.Generic;

namespace SnapshotTestingGround.Snapshot
{
    public interface ISnapshotIdentity
    {
        long SnapshotIdentity { get; set; }
        long SnapshotObjectId { get; set; }
        void Snapshot(Writer writer);
        void Hydrate(Dictionary<string, string[]> values, Reader reader);
    }
}