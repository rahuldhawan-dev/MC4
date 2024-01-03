using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Migrations;
using MMSINC.Data;
using MMSINC.Validation;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ContractorLaborCost : IEntityLookup
    {
        #region Properties

        public virtual int Id { get; set; }

        [Required, StringLength(CreateContractorLaborCostsTable.StringLengths.STOCK_NUMBER)]
        public virtual string StockNumber { get; set; }

        [Required, StringLength(CreateContractorLaborCostsTable.StringLengths.UNIT)]
        public virtual string Unit { get; set; }

        [Required, StringLength(AddContractorLaborCostPercentagesForBug2273.StringLengths.JOB_DESCRIPTION)]
        public virtual string JobDescription { get; set; }

        [StringLength(CreateContractorLaborCostsTable.StringLengths.SUB_DESCRIPTION)]
        public virtual string SubDescription { get; set; }

        [DisplayFormat(DataFormatString = "{0:c}"), RequiredWhen("Percentage", ComparisonType.EqualTo, null)]
        public virtual decimal? Cost { get; set; }

        [DisplayFormat(DataFormatString = "{0:p}"), RequiredWhen("Cost", ComparisonType.EqualTo, null)]
        public virtual int? Percentage { get; set; }

        public virtual IList<OperatingCenter> OperatingCenters { get; set; }
        public virtual IList<ContractorOverrideLaborCost> OverrideLaborCosts { get; set; }

        public virtual string Description => new ContractorLaborCostDisplayItem {
            StockNumber = StockNumber,
            Unit = Unit,
            JobDescription = JobDescription
        }.Display;

        #endregion

        #region Constructors

        public ContractorLaborCost()
        {
            OperatingCenters = new List<OperatingCenter>();
            OverrideLaborCosts = new List<ContractorOverrideLaborCost>();
        }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public override string ToString()
        {
            return Description;
        }

        #endregion
    }

    public class ContractorLaborCostDisplayItem : DisplayItem<ContractorLaborCost>
    {
        public string StockNumber { get; set; }
        public string Unit { get; set; }
        public string JobDescription { get; set; }

        public override string Display => $"{StockNumber} - {Unit} - {JobDescription}";
    }
}
