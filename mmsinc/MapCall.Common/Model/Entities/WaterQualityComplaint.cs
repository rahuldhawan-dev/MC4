using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Mappings;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class WaterQualityComplaint : IEntity, IValidatableObject, IThingWithDocuments, IThingWithNotes,
        IThingWithCoordinate
    {
        #region Constants

        public struct StringLengths
        {
            public const int ORCOM_ORDER_NUMBER = 25,
                             NOTIFICATION_COMPLETED_BY = 55,
                             CUSTOMER_NAME = 50,
                             HOME_PHONE_NUMBER = 15,
                             EXTENSION = 10,
                             STREET_NUMBER = 10,
                             STREET_NAME = 50,
                             APARTMENT_NUMBER = 15,
                             ZIP_CODE = 25,
                             PREMISE_NUMBER = 10,
                             SERVICE_NUMBER = 10,
                             ACCOUNT_NUMBER = 15,
                             SITE_VISIT_BY = 55,
                             MATERIAL_OF_SERVICE = 20,
                             NOTIFICATION_CREATED_BY = 55;
        }
        
        public struct DisplayNames
        {
            public const string NOTIFICATION_COMPLETED_BY = "Notification Completed By",
                                WORK_ORDER_ID = "WorkOrderId";
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }

        public virtual MapIcon Icon => Coordinate?.Icon;

        public virtual OrcomOrderType OrcomOrderType { get; set; }
        public virtual PublicWaterSupply PublicWaterSupply { get; set; }
        public virtual WaterQualityComplaintType Type { get; set; }
        public virtual State State { get; set; }
        public virtual Town Town { get; set; }
        public virtual TownSection TownSection { get; set; }
        public virtual Coordinate Coordinate { get; set; }
        public virtual WaterQualityComplaintProblemArea ProblemArea { get; set; }
        public virtual WaterQualityComplaintSource Source { get; set; }
        public virtual WaterQualityComplaintProbableCause ProbableCause { get; set; }
        public virtual WaterQualityComplaintActionsWhichCanBeTaken ActionTaken { get; set; }
        public virtual WaterQualityComplaintCustomerExpectation CustomerExpectation { get; set; }
        public virtual WaterQualityComplaintCustomerSatisfaction CustomerSatisfaction { get; set; }
        public virtual WaterQualityComplaintRootCause RootCause { get; set; }
        public virtual MainSize MainSize { get; set; }
        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual Employee InitialLocalContact { get; set; }
        public virtual WaterQualityComplaintLocalResponseType InitialLocalResponseType { get; set; }
        public virtual IList<WaterQualityComplaintSampleResult> SampleResults { get; set; } = new List<WaterQualityComplaintSampleResult>();
        public virtual IList<WaterQualityComplaintDocument> ComplaintDocuments { get; set; } = new List<WaterQualityComplaintDocument>();
        public virtual IList<WaterQualityComplaintNote> ComplaintNotes { get; set; } = new List<WaterQualityComplaintNote>();
        [DisplayName("SAP Notification #")]
        public virtual string OrcomOrderNumber { get; set; }

        public virtual DateTime? DateComplaintReceived { get; set; }

        [View(FormatStyle.DateTimeWithoutSeconds)]
        public virtual DateTime? InitialLocalResponseDate { get; set; }

        public virtual string NotificationCreatedBy { get; set; }

        [View(FormatStyle.DateTimeWithoutSeconds)]
        public virtual DateTime? NotificationCompletionDate { get; set; }

        public virtual Employee NotificationCompletedBy { get; set; }

        public virtual string CustomerName { get; set; }

        public virtual string HomePhoneNumber { get; set; }

        public virtual string Ext { get; set; }

        public virtual string StreetNumber { get; set; }

        public virtual string StreetName { get; set; }

        public virtual string ApartmentNumber { get; set; }

        public virtual string ZipCode { get; set; }

        public virtual string PremiseNumber { get; set; }

        public virtual string ServiceNumber { get; set; }

        public virtual string AccountNumber { get; set; }

        [Multiline]
        public virtual string ComplaintDescription { get; set; }

        public virtual DateTime? ComplaintStartDate { get; set; }

        [Required]
        public virtual bool SiteVisitRequired { get; set; }

        public virtual string SiteVisitBy { get; set; }

        [Multiline]
        public virtual string SiteComments { get; set; }

        [Required]
        public virtual bool WaterFilterOnComplaintSource { get; set; }

        [Required]
        public virtual bool CrossConnectionDetected { get; set; }

        public virtual string MaterialOfService { get; set; }

        public virtual DateTime? CustomerAnticipatedFollowupDate { get; set; }
        public virtual DateTime? ActualCustomerFollowupDate { get; set; }

        [Required]
        public virtual bool CustomerSatisfactionFollowupLetter { get; set; }

        [Required]
        public virtual bool CustomerSatisfactionFollowupCall { get; set; }

        public virtual string CustomerSatisfactionFollowupComments { get; set; }

        [Required]
        public virtual bool RootCauseIdentified { get; set; }

        public virtual bool? FollowUpPostCardSent { get; set; }

        public virtual bool IsClosed { get; set; }

        public virtual bool Imported { get; set; }

        public virtual bool HasCoordinate { get; set; }

        public virtual IList<IDocumentLink> LinkedDocuments => ComplaintDocuments.Map(n => (IDocumentLink)n);

        public virtual string TableName => WaterQualityComplaintMap.TABLE_NAME;

        public virtual IList<INoteLink> LinkedNotes => ComplaintNotes.Map(n => (INoteLink)n);

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }

    [Serializable]
    public class WaterQualityComplaintByStateForYearReportItem
    {
        public virtual string OperatingCenterCode { get; set; }
        public virtual string ComplaintType { get; set; }
        public virtual int Jan { get; set; }
        public virtual int Feb { get; set; }
        public virtual int Mar { get; set; }
        public virtual int Apr { get; set; }
        public virtual int May { get; set; }
        public virtual int Jun { get; set; }
        public virtual int Jul { get; set; }
        public virtual int Aug { get; set; }
        public virtual int Sep { get; set; }
        public virtual int Oct { get; set; }
        public virtual int Nov { get; set; }
        public virtual int Dec { get; set; }
    }

    public interface ISearchWaterQualityComplaintByStateForYear
    {
        int Year { get; }
        string State { get; }
    }
}
