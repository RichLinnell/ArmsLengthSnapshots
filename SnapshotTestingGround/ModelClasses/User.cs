using SnapshotTestingGround.Snapshot;

namespace SnapshotTestingGround.ModelClasses
{
    [Snapshot]
    public partial class User
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public ExternalCompany Company { get; set; }

    }
}