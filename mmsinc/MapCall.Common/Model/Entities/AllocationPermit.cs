using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Mappings;
using MapCall.Common.Model.Migrations;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data;
using MMSINC.Data.ChangeTracking;
using MMSINC.Utilities;
using Remotion.Linq.Clauses.StreamedData;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class AllocationPermit
        : IEntityWithCreationTimeTracking, IValidatableObject, IThingWithNotes, IThingWithDocuments
    {
        #region Properties

        #region Table Properties

        [DisplayName("AllocationGroupingID")]
        public virtual int Id { get; set; }

        public virtual PublicWaterSupply PublicWaterSupply { get; set; }
        public virtual EnvironmentalPermit EnvironmentalPermit { get; set; }
        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual DateTime CreatedAt { get; set; }

        public virtual IList<AllocationPermitWithdrawalNode> AllocationPermitWithdrawalNodes { get; set; }

        [StringLength(UpdateEnvironmentalPermitTablesForBug1627.StringLengths.SYSTEM)]
        public virtual string System { get; set; }

        public virtual bool? SurfaceSupply { get; set; }
        public virtual bool? GroundSupply { get; set; }

        [StringLength(UpdateEnvironmentalPermitTablesForBug1627.StringLengths.GEOLOGICAL_FORMATION)]
        public virtual string GeologicalFormation { get; set; }

        public virtual bool? ActivePermit { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public virtual DateTime? EffectiveDateOfPermit { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public virtual DateTime? RenewalApplicationDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public virtual DateTime? ExpirationDate { get; set; }

        [StringLength(UpdateEnvironmentalPermitTablesForBug1627.StringLengths.SUB_ALLOCATION_NUMBER)]
        public virtual string SubAllocationNumber { get; set; }

        [DisplayName("GPD"), DisplayFormat(DataFormatString = CommonStringFormats.DECIMAL_MAX_TWO_DECIMAL_PLACES)]
        public virtual decimal? Gpd { get; set; }

        [DisplayName("MGM"), DisplayFormat(DataFormatString = CommonStringFormats.DECIMAL_MAX_TWO_DECIMAL_PLACES)]
        public virtual decimal? Mgm { get; set; }

        [DisplayName("MGY"), DisplayFormat(DataFormatString = CommonStringFormats.DECIMAL_MAX_TWO_DECIMAL_PLACES)]
        public virtual decimal? Mgy { get; set; }

        [StringLength(UpdateEnvironmentalPermitTablesForBug1627.StringLengths.PERMIT_TYPE)]
        public virtual string PermitType { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.CURRENCY_NO_DECIMAL)]
        public virtual int? PermitFee { get; set; }

        [StringLength(UpdateEnvironmentalPermitTablesForBug1627.StringLengths.SOURCE_DESCRIPTION)]
        public virtual string SourceDescription { get; set; }

        [StringLength(UpdateEnvironmentalPermitTablesForBug1627.StringLengths.SOURCE_RESTRICTIONS)]
        public virtual string SourceRestrictions { get; set; }

        [StringLength(UpdateEnvironmentalPermitTablesForBug1627.StringLengths.NOTES)]
        [DisplayName("Notes")]
        public virtual string PermitNotes { get; set; }

        [DisplayName("GPM"), DisplayFormat(DataFormatString = CommonStringFormats.DECIMAL_MAX_TWO_DECIMAL_PLACES)]
        public virtual decimal? Gpm { get; set; }

        #endregion

        #region Logical Properties

        #region Documents

        public virtual IList<AllocationPermitDocument> AllocationPermitDocuments { get; set; }

        public virtual IList<IDocumentLink> LinkedDocuments
        {
            get { return AllocationPermitDocuments.Map(epd => (IDocumentLink)epd); }
        }

        public virtual IList<Document> Documents
        {
            get { return AllocationPermitDocuments.Map(epd => epd.Document); }
        }

        #endregion

        #region Notes

        public virtual IList<AllocationPermitNote> AllocationPermitNotes { get; set; }

        public virtual IList<INoteLink> LinkedNotes
        {
            get { return AllocationPermitNotes.Map(n => (INoteLink)n); }
        }

        public virtual IList<Note> Notes
        {
            get { return AllocationPermitNotes.Map(n => n.Note); }
        }

        #endregion

        public virtual string TableName => nameof(AllocationPermit) + "s";

        #endregion

        #endregion

        #region Constructors

        public AllocationPermit() { }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public override string ToString()
        {
            return $"{OperatingCenter?.OperatingCenterCode} - {Id} - {PermitType}";
        }

        #endregion
    }

    public class AllocationPermitDisplay : IEntity
    {
        public int Id { get; set; }
        public string OperatingCenter { get; set; }
        public string PermitType { get; set; }

        public string Display => $"{(OperatingCenter == null ? "" : OperatingCenter + " - ")}{Id} - {PermitType}";
    }
}
