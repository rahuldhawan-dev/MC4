using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class PrimaryDriverForProposalMap : EntityLookupMap<PrimaryDriverForProposal>
    {
        protected override int DescriptionLength => 123;

        public PrimaryDriverForProposalMap()
        {
            Table("PrimaryDriversForProposals");
        }
    }
}
