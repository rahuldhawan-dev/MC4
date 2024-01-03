using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities.Customers;

namespace MapCall.Common.Model.Mappings.Customers
{
    public class H2OSurveyMap : ClassMap<H2OSurvey>
    {
        #region Constructors

        public H2OSurveyMap()
        {
            Id(x => x.H2OSurveyID, "H20SurveyId");

            Map(x => x.AuditPerformedDate);
            Map(x => x.CustomerReceivedDate);
            Map(x => x.CustomerWithHighWaterUsage);
            Map(x => x.DoesCustomerWantToParticpate);
            Map(x => x.EnrollmentDate);
            Map(x => x.H2OSurveyContactStatusTypeID);
            Map(x => x.H2OSurveyReceivedThroughTypeID);
            Map(x => x.H2OSurveyAuditPerformedByTypeID);
            Map(x => x.QualifiesForKit);
            Map(x => x.QualifiesForKitDate);
            Map(x => x.IsDuplicate);
            Map(x => x.IsHomeOwner);
            Map(x => x.LastContactDate);
            Map(x => x.SiteVisitDate);
            Map(x => x.WaterSavingKitProvided);
            Map(x => x.WaterSavingKitProvidedDate);
        }

        #endregion
    }
}
