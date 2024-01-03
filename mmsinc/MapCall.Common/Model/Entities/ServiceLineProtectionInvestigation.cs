using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ServiceLineProtectionInvestigation
        : IValidatableObject,
            IThingWithCoordinate,
            IThingWithNotes,
            IThingWithDocuments
    {
        #region Constants

        public struct StringLengths
        {
            public const int CUSTOMER_NAME = 50,
                             CUSTOMER_ADDRESS = 120,
                             STREET_NUMBER = 10,
                             CUSTOMER_ZIP = 10,
                             PREMISE_NUMBER = 10,
                             CUSTOMER_PHONE = 10,
                             ACCOUNT_NUMBER = 20,
                             NOTES = 255;
        }

        #endregion

        #region Properties

        #region Table Properties

        public virtual int Id { get; set; }

        public virtual MapIcon Icon => Coordinate.Icon;
        public virtual string CustomerName { get; set; }
        public virtual string StreetNumber { get; set; }
        public virtual Street Street { get; set; }
        public virtual string CustomerAddress2 { get; set; }
        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual Town CustomerCity { get; set; }
        public virtual string CustomerZip { get; set; }
        public virtual string PremiseNumber { get; set; }
        public virtual string AccountNumber { get; set; }
        public virtual string CustomerPhone { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? DateInstalled { get; set; }

        public virtual ServiceMaterial CustomerServiceMaterial { get; set; }
        public virtual ServiceSize CustomerServiceSize { get; set; }
        public virtual ServiceLineProtectionWorkType WorkType { get; set; }
        public virtual Coordinate Coordinate { get; set; }
        public virtual Contractor Contractor { get; set; }

        [DisplayName("Notes")]
        public virtual string TheNotes { get; set; }

        public virtual Service Service { get; set; }
        public virtual ServiceMaterial CompanyServiceMaterial { get; set; }
        public virtual ServiceSize CompanyServiceSize { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? RenewalCompleted { get; set; }

        #endregion

        #region Logical Fields

        #region Formula

        //TODO: make this an entity lookup?
        public virtual ServiceLineProtectionInvestigationStatus Status { get; set; }

        #endregion

        #region Logical Properties

        #region Documents

        public virtual IList<ServiceLineProtectionInvestigationDocument> ServiceLineProtectionInvestigationDocuments
        {
            get;
            set;
        }

        public virtual IList<IDocumentLink> LinkedDocuments
        {
            get { return ServiceLineProtectionInvestigationDocuments.Map(epd => (IDocumentLink)epd); }
        }

        public virtual IList<Document> Documents
        {
            get { return ServiceLineProtectionInvestigationDocuments.Map(epd => epd.Document); }
        }

        #endregion

        #region Notes

        public virtual IList<ServiceLineProtectionInvestigationNote> ServiceLineProtectionInvestigationNotes
        {
            get;
            set;
        }

        public virtual IList<INoteLink> LinkedNotes
        {
            get { return ServiceLineProtectionInvestigationNotes.Map(n => (INoteLink)n); }
        }

        public virtual IList<Note> Notes
        {
            get { return ServiceLineProtectionInvestigationNotes.Map(n => n.Note); }
        }

        #endregion

        public virtual string TableName => nameof(ServiceLineProtectionInvestigation) + "s";

        [DoesNotExport]
        public virtual string RecordUrl { get; set; }

        #endregion

        #endregion

        #endregion

        #region Constructors

        public ServiceLineProtectionInvestigation()
        {
            ServiceLineProtectionInvestigationDocuments = new List<ServiceLineProtectionInvestigationDocument>();
            ServiceLineProtectionInvestigationNotes = new List<ServiceLineProtectionInvestigationNote>();
        }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }

    [Serializable]
    public class ServiceLineProtectionWorkType : EntityLookup { }

    [Serializable]
    public class ServiceLineProtectionInvestigationStatus : EntityLookup { }
}
