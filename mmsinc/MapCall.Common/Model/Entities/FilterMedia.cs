using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Mappings;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data;
using StringLengths = MapCall.Common.Model.Migrations.CreateTablesForBug1510.StringLengths.FilterMedia;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class FilterMedia : IEntity, IValidatableObject, IThingWithDocuments, IThingWithNotes
    {
        #region Properties

        public virtual int Id { get; set; }

        public virtual IList<FilterMediaNote> FilterMediaNotes { get; set; }
        public virtual IList<FilterMediaDocument> FilterMediaDocuments { get; set; }

        public virtual IList<INoteLink> LinkedNotes => FilterMediaNotes.Map(n => (INoteLink)n);
        public virtual IList<IDocumentLink> LinkedDocuments => FilterMediaDocuments.Map(n => (IDocumentLink)n);

        public virtual string TableName => FilterMediaMap.TABLE_NAME;

        public virtual Facility Facility => Equipment?.Facility;
        public virtual FilterMediaFilterType FilterType { get; set; }
        public virtual FilterMediaLevelControlMethod LevelControlMethod { get; set; }
        public virtual Equipment Equipment { get; set; }
        public virtual FilterMediaWashType WashType { get; set; }
        public virtual FilterMediaType MediaType { get; set; }
        public virtual FilterMediaLocation Location { get; set; }

        [Required]
        public virtual int FilterNumber { get; set; }

        [Required]
        [StringLength(StringLengths.EQUIPMENT_IDENTIFIER)]
        public virtual string EquipmentIdentifier { get; set; }

        public virtual int? YearInService { get; set; }
        public virtual int? EstimatedMediaLifecycle { get; set; }
        public virtual float? CapacityMGD { get; set; }

        [StringLength(StringLengths.COEFFICIENT)]
        public virtual string Coefficient { get; set; }

        [StringLength(StringLengths.FILTER_DIMENSIONS)]
        public virtual string FilterDimensions { get; set; }

        public virtual int? MediaArea { get; set; }
        public virtual int? MediaVolume { get; set; }
        public virtual bool? GravelSupportMedia { get; set; }
        public virtual decimal? MonthlyMediaExpense { get; set; }
        public virtual decimal? AnnualInspectionCosts { get; set; }
        public virtual decimal? AnnualAnalysisCosts { get; set; }
        public virtual decimal? AnnualCompanyLaborCosts { get; set; }
        public virtual int? EquipmentCriticalRating { get; set; }
        public virtual int? YearLastPainted { get; set; }
        public virtual bool? ServedByStandbyPower { get; set; }

        [StringLength(StringLengths.TURBIDIMETER_MODEL)]
        public virtual string TurbidimeterModel { get; set; }

        [StringLength(StringLengths.NOTES)]
        public virtual string Notes { get; set; }

        [StringLength(StringLengths.PRODUCT_CODE)]
        public virtual string ProductCode { get; set; }

        public virtual int? AnthraciteDepth { get; set; }
        public virtual int? GACDepth { get; set; }
        public virtual int? SandDepth { get; set; }
        public virtual int? GravelDepth { get; set; }
        public virtual DateTime? LastTimeChanged { get; set; }
        public virtual DateTime? LastTimeCleaned { get; set; }
        public virtual bool? AirScouring { get; set; }
        public virtual bool? Recycling { get; set; }

        [StringLength(StringLengths.COMMENT)]
        public virtual string Comment { get; set; }

        #endregion

        #region Constructors

        public FilterMedia()
        {
            FilterMediaNotes = new List<FilterMediaNote>();
            FilterMediaDocuments = new List<FilterMediaDocument>();
        }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }
}
