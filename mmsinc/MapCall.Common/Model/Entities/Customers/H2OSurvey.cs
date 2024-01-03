using System;

namespace MapCall.Common.Model.Entities.Customers
{
    // This doesn't map to the whole table. Just the fields needed for the monthly report.
    // We can add more later if needed.
    public class H2OSurvey
    {
        #region Properties

        public virtual int H2OSurveyID { get; protected set; }
        public virtual DateTime? AuditPerformedDate { get; set; }
        public virtual DateTime? CustomerReceivedDate { get; set; }
        public virtual bool? CustomerWithHighWaterUsage { get; set; }
        public virtual bool? DoesCustomerWantToParticpate { get; set; }
        public virtual DateTime? EnrollmentDate { get; set; }
        public virtual int? H2OSurveyContactStatusTypeID { get; set; }
        public virtual int? H2OSurveyReceivedThroughTypeID { get; set; }
        public virtual int? H2OSurveyAuditPerformedByTypeID { get; set; }
        public virtual bool? QualifiesForKit { get; set; }
        public virtual DateTime? QualifiesForKitDate { get; set; }
        public virtual bool IsDuplicate { get; set; }
        public virtual bool? IsHomeOwner { get; set; }
        public virtual DateTime? LastContactDate { get; set; }
        public virtual DateTime? SiteVisitDate { get; set; }
        public virtual bool WaterSavingKitProvided { get; set; }
        public virtual DateTime? WaterSavingKitProvidedDate { get; set; }

        #endregion
    }
}
