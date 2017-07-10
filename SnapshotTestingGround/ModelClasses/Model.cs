using System.Collections.Generic;
using SnapshotTestingGround.Snapshot;

namespace SnapshotTestingGround.ModelClasses
{
    [Snapshot]
    public partial class Model
    {
        public Model()
        {
            ExternalCompanies = new List<ExternalCompany>();
        }
        public List<ExternalCompany> ExternalCompanies { get; }
        
    }
}