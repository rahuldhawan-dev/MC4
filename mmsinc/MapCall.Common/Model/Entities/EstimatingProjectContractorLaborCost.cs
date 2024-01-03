using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;
using MMSINC.Utilities;
using StructureMap.Attributes;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class EstimatingProjectContractorLaborCost : IEntity, IValidatableObject
    {
        #region Fields

        [NonSerialized] private IDateTimeProvider _dateTimeProvider;

        #endregion

        #region Properties

        #region Table Columns

        public virtual int Id { get; set; }

        [Required]
        public virtual int Quantity { get; set; }

        #endregion

        #region References

        public virtual EstimatingProject EstimatingProject { get; set; }
        public virtual ContractorLaborCost ContractorLaborCost { get; set; }
        public virtual AssetType AssetType { get; set; }

        #endregion

        #region Logical Properties

        public virtual ContractorOverrideLaborCost ContractorOverrideLaborCost
        {
            get
            {
                var contractorOverrideLaborCost = ContractorLaborCost.OverrideLaborCosts.Where(
                                                                          c => c.EffectiveDate <=
                                                                               _dateTimeProvider.GetCurrentDate() &&
                                                                               c.OperatingCenter ==
                                                                               EstimatingProject.OperatingCenter &&
                                                                               c.Contractor ==
                                                                               EstimatingProject.Contractor)
                                                                     .OrderByDescending(c => c.EffectiveDate)
                                                                     .FirstOrDefault();
                return contractorOverrideLaborCost;
            }
        }

        [DisplayFormat(DataFormatString = "{0:c}")]
        public virtual decimal? TotalCost => ActualCost.HasValue ? (decimal?)Quantity * ActualCost.Value : null;

        [DisplayFormat(DataFormatString = "{0:c}"), DisplayName("Cost")]
        public virtual decimal? ActualCost =>
            ContractorOverrideLaborCost == null
                ? Cost
                : ContractorOverrideLaborCost.Cost; // as opposed to "Cost" which is really "DefaultCost"

        public virtual decimal? Cost => ContractorLaborCost.Cost;

        public virtual int? Percentage => ContractorLaborCost.Percentage;

        #endregion

        #region Injected Properties

        [SetterProperty]
        public virtual IDateTimeProvider DateTimeProvider
        {
            set => _dateTimeProvider = value;
        }

        #endregion

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }

    public static class EstimatingProjectContractorLaborCostListExtensions
    {
        public static decimal GetTotal(this IEnumerable<EstimatingProjectContractorLaborCost> list)
        {
            var costs = list.Where(c => c.ActualCost.HasValue);
            var adders = list.Where(c => c.Percentage.HasValue);

            var ret = costs.Sum(c => c.TotalCost.Value);
            foreach (var percentage in adders)
            {
                ret = ret + (ret * (percentage.Percentage.Value / 100));
            }

            return ret;
        }
    }
}
