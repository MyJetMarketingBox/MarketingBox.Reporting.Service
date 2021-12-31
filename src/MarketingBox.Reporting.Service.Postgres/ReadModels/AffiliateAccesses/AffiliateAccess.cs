using System.Collections.Generic;
using MarketingBox.Reporting.Service.Postgres.ReadModels.Deposits;

namespace MarketingBox.Reporting.Service.Postgres.ReadModels.AffiliateAccesses
{
    public class AffiliateAccess
    {
        public long Id { get; set; }
        public long MasterAffiliateId { get; set; }
        public long AffiliateId { get; set; }

        //public IList<Deposit> Deposits { get; set; }

        //public IList<Registration> Registrations { get; set; }
    }
}
