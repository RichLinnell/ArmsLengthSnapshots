using System.Collections.Generic;
using SnapshotTestingGround.Snapshot;

namespace SnapshotTestingGround.ModelClasses
{
    [Snapshot]
    public partial class ExternalCompany
    {
        private readonly Dictionary<long, User> _users;
        public ExternalCompany()
        {
            _users = new Dictionary<long, User>();
        }

        private long _id;
        public string Name { get; set; }
        public Dictionary<long, User> Users => _users;
        public ExternalCompany ParentCompany { get; set; }

        public string IAmAMethod()
        {
            return "Banzai";
        }
    }
}