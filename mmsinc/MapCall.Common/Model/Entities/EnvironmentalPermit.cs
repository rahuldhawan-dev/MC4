using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Mappings;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class EnvironmentalPermit : IEntity, IValidatableObject, IThingWithDocuments, IThingWithNotes
    {
        #region Constants

        public struct StringLengths
        {
            #region Constants

            public const int PERMIT_NAME = 50,
                             PERMIT_NUMBER = 50,
                             PROGRAM_INTEREST_NUMBER = 50,
                             PERMIT_CROSS_REFERENCE_NUMBER = 50,
                             DESCRIPTION = 255;

            #endregion
        }

        #endregion

        #region Properties

        #region Table

        [View("Permit ID")]
        public virtual int Id { get; set; }

        [View("Permit Type")]
        public virtual EnvironmentalPermitType EnvironmentalPermitType { get; set; }

        [View("Permit Status")]
        public virtual EnvironmentalPermitStatus EnvironmentalPermitStatus { get; set; }

        [View("PWSID")]
        public virtual PublicWaterSupply PublicWaterSupply { get; set; }

        [View("WWSID")]
        public virtual WasteWaterSystem WasteWaterSystem { get; set; }

        public virtual State State { get; set; }

        // TODO: This property or this entity should be renamed. 
        // Probably to "FacilityWaterType".
        public virtual WaterType FacilityType { get; set; }
        public virtual string PermitNumber { get; set; }
        public virtual string PermitName { get; set; }

        [View("Program Interest #")]
        public virtual string ProgramInterestNumber { get; set; }

        [View("Cross Reference #")]
        public virtual string PermitCrossReferenceNumber { get; set; }

        [View("Effective Date", MMSINC.Utilities.FormatStyle.Date)]
        public virtual DateTime? PermitEffectiveDate { get; set; }

        [View("Renewal Date", MMSINC.Utilities.FormatStyle.Date)]
        public virtual DateTime? PermitRenewalDate { get; set; }

        [View("Expiration Date", MMSINC.Utilities.FormatStyle.Date)]
        public virtual DateTime? PermitExpirationDate { get; set; }

        public virtual string Description { get; set; }
        public virtual bool RequiresFees { get; set; }
        public virtual bool? ReportingRequired { get; set; }

        [View("Does this permit have Requirements?")]
        public virtual bool RequiresRequirements { get; set; }

        public virtual IList<EnvironmentalPermitRequirement> Requirements { get; set; }
        public virtual IList<AllocationPermit> AllocationPermits { get; set; }
        public virtual IList<OperatingCenter> OperatingCenters { get; set; }
        public virtual IList<Equipment> Equipment { get; set; }
        public virtual IList<Facility> Facilities { get; set; }
        public virtual IList<EnvironmentalPermitFee> Fees { get; set; }
        public virtual Facility FirstFacility { get; set; }
        public virtual Equipment FirstEquipment { get; set; }

        public virtual IEnumerable<AllocationPermitWithdrawalNode> AllocationPermitWithdrawalNodes
        {
            get { return AllocationPermits.SelectMany(x => x.AllocationPermitWithdrawalNodes); }
        }

        #endregion

        #region Logical Fields

        public virtual bool IsLinkedToFacilityOrEquipment { get; set; }

        public virtual IList<IDocumentLink> LinkedDocuments => Documents.Cast<IDocumentLink>().ToList();

        public virtual IList<Document<EnvironmentalPermit>> Documents { get; set; }

        public virtual IList<INoteLink> LinkedNotes => Notes.Cast<INoteLink>().ToList();

        public virtual IList<Note<EnvironmentalPermit>> Notes { get; set; }

        [DoesNotExport]
        public virtual string TableName => nameof(EnvironmentalPermit) + "s";

        [DoesNotExport]
        public virtual string RecordUrl { get; set; }

        #endregion

        #endregion

        #region Constructors

        public EnvironmentalPermit()
        {
            Documents = new List<Document<EnvironmentalPermit>>();
            Notes = new List<Note<EnvironmentalPermit>>();
            OperatingCenters = new List<OperatingCenter>();
            Facilities = new List<Facility>();
            Equipment = new List<Equipment>();
            Fees = new List<EnvironmentalPermitFee>();
            Requirements = new List<EnvironmentalPermitRequirement>();
        }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public override string ToString()
        {
            return PermitNumber;
        }

        #endregion
    }
}
