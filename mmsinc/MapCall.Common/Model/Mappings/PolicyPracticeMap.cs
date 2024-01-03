using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class PolicyPracticeMap : ClassMap<PolicyPractice>
    {
        public PolicyPracticeMap()
        {
            Table("tblPoliciesPractices");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("PP_ID");

            Map(x => x.Description).Column("PP_Description").Length(255);
            Map(x => x.Summary).Column("PP_Summary");

            /* THESE MIGHT MATTER AT SOME POINT:
            References(x => x.FunctionalArea).Column("Functional_Area").Nullable();
            References(x => x.PpStatu).Column("PP_Status").Nullable();
            References(x => x.PpCategory).Column("PP_Category").Nullable();
            References(x => x.PpSubCategory).Column("PP_Sub_Category").Nullable();
            References(x => x.OperatingCenter).Column("OpCode").Nullable();
            References(x => x.Facility).Column("Facility_ID").Nullable();
            References(x => x.CostImpact).Column("CostImpact").Nullable();

            Map(x => x.Flag);
            Map(x => x.RegulationId).Column("Regulation_ID").Precision(10);
            Map(x => x.EquipmentId).Column("Equipment_ID").Precision(10);
            Map(x => x.DateApproved).Column("Date_Approved");
            Map(x => x.DateIssued).Column("Date_Issued");
            Map(x => x.Revision).Length(50);
            Map(x => x.ReviewFrequencyDays).Column("Review_Frequency_Days").Length(2147483647);
            Map(x => x.PsmTcpa).Column("PSM_TCPA");
            Map(x => x.Dpcc).Column("DPCC");
            Map(x => x.Osha).Column("OSHA");
            Map(x => x.Company);
            Map(x => x.Sox).Column("SOX");
            */
        }
    }
}
