using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using StructureMap;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.ProjectManagement.Models.ViewModels
{
    public class PipelineProjectCostEstimate : ViewModel<EstimatingProject>
    {
        public static string CONTRACTOR_LABOR_COST_ERROR = "Values are missing for one of the Total Costs in the Contractor Labor Costs.";

        #region Properties

        [DoesNotAutoMap, DisplayName("DVS #:")]
        public string DVSNumber { get; set; }
        public virtual string WBSNumber { get; set; }

        public virtual EstimatingProjectType ProjectType { get; set; }
        public virtual Town Town { get; set; }
        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual Employee Estimator { get; set; }
        public virtual Contractor Contractor { get; set; }
        public virtual string ProjectNumber { get; set; }
        public virtual string ProjectName { get; set; }
        public virtual string Street { get; set; }
        public virtual string Description { get; set; }
        public virtual DateTime? EstimateDate { get; set; }
        public virtual string Remarks { get; set; }

        public virtual IList<EstimatingProjectContractorLaborCost> ContractorLaborCosts { get; set; }
        public virtual IList<EstimatingProjectOtherCost> OtherCosts { get; set; }
        public virtual IList<EstimatingProjectMaterial> Materials { get; set; }
        public virtual IList<EstimatingProjectCompanyLaborCost> CompanyLaborCosts { get; set; }
        public virtual IList<EstimatingProjectPermit> Permits { get; set; }
        public virtual IList<EstimatingProjectDocument> EstimatingProjectDocuments { get; set; }
        public virtual IList<EstimatingProjectNote> EstimatingProjectNotes { get; set; }

        [DisplayFormat(DataFormatString = "{0}%")]
        public virtual int ContingencyPercentage { get; set; }
        [DisplayFormat(DataFormatString = "{0}%")]
        public virtual int OverheadPercentage { get; set; }
        [DisplayFormat(DataFormatString = CommonStringFormats.CURRENCY)]
        public virtual decimal? LumpSum { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.CURRENCY)]
        public virtual decimal TotalOtherCost { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.CURRENCY)]
        public virtual decimal TotalMaterialCost { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.CURRENCY)]
        public virtual decimal TotalCompanyLaborCost { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.CURRENCY)]
        public virtual decimal TotalContractorLaborCost { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.CURRENCY)]
        public virtual decimal TotalPermitCost { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.CURRENCY)]
        public virtual decimal EstimatedConstructionCost { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.CURRENCY)]
        public virtual decimal ContingencyCost { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.CURRENCY)]
        public virtual decimal OverheadCost { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.CURRENCY)]
        public virtual decimal TotalEstimatedCost { get; set; }

        public bool IsNonFramework { get; set; }

        #endregion

        #region Methods

        // These were originally properties, one for each asset type/cost type. That
        // was unsustainable though. So these have been used instead.

        public virtual bool HasRecordsForAssetType(AssetType assetType)
        {
            return
                Materials.Any(x => x.AssetType != null && x.AssetType.Id == assetType.Id) ||
                ContractorLaborCosts.Any(x => x.AssetType != null && x.AssetType.Id == assetType.Id) ||
                CompanyLaborCosts.Any(x => x.AssetType != null && x.AssetType.Id == assetType.Id) ||
                Permits.Any(x => x.AssetType != null && x.AssetType.Id == assetType.Id) ||
                OtherCosts.Any(x => x.AssetType != null && x.AssetType.Id == assetType.Id);
        }

        public virtual decimal GetCostTotalForAssetType(AssetType assetType)
        {
            return
                Materials.Where(x => x.AssetType != null && x.AssetType.Id == assetType.Id).Sum(x => x.TotalCost) +
                ContractorLaborCosts.Where(x => x.AssetType != null && x.AssetType.Id == assetType.Id).Sum(x => x.TotalCost.Value) +
                CompanyLaborCosts.Where(x => x.AssetType != null && x.AssetType.Id == assetType.Id).Sum(x => x.TotalCost) +
                Permits.Where(x => x.AssetType != null && x.AssetType.Id == assetType.Id).Sum(x => x.TotalCost) +
                OtherCosts.Where(x => x.AssetType != null && x.AssetType.Id == assetType.Id).Sum(x => x.TotalCost);
        }

        public virtual decimal GetCostForAssetType(AssetType assetType, EstimatingProject.CostType costType)
        {
            switch (costType)
            {
                case EstimatingProject.CostType.Material:
                    return (Materials == null) ? 0 : Materials.Where(x => x.AssetType != null && x.AssetType.Id == assetType.Id).Sum(x => x.TotalCost);
                case EstimatingProject.CostType.ContractorLabor:
                    return (ContractorLaborCosts == null) ? 0 : ContractorLaborCosts.Where(x => x.AssetType != null && x.AssetType.Id == assetType.Id).GetTotal();
                case EstimatingProject.CostType.CompanyLabor:
                    return (CompanyLaborCosts == null) ? 0 : CompanyLaborCosts.Where(x => x.AssetType != null && x.AssetType.Id == assetType.Id).Sum(x => x.TotalCost);
                case EstimatingProject.CostType.Permit:
                    return (Permits == null) ? 0 : Permits.Where(x => x.AssetType != null && x.AssetType.Id == assetType.Id).Sum(x => x.TotalCost);
                case EstimatingProject.CostType.Other:
                    return (OtherCosts == null) ? 0 : OtherCosts.Where(x => x.AssetType != null && x.AssetType.Id == assetType.Id).Sum(x => x.TotalCost);
                default:
                    throw new InvalidOperationException("A cost type was not provided or matched.");
            }
        }

        public virtual IEnumerable<TCollectionType> GetEnumerablesForAssetTypeAndCostType<TCollectionType>(AssetType assetType, EstimatingProject.CostType costType)
        {
            switch (costType)
            {
                case EstimatingProject.CostType.Material:
                    return (IEnumerable<TCollectionType>)Materials.Where(x => x.AssetType != null && x.AssetType.Id == assetType.Id);
                case EstimatingProject.CostType.ContractorLabor:
                    return (IEnumerable<TCollectionType>)ContractorLaborCosts.Where(x => x.AssetType != null && x.AssetType.Id == assetType.Id);
                case EstimatingProject.CostType.CompanyLabor:
                    return (IEnumerable<TCollectionType>)CompanyLaborCosts.Where(x => x.AssetType != null && x.AssetType.Id == assetType.Id);
                case EstimatingProject.CostType.Permit:
                    return (IEnumerable<TCollectionType>)Permits.Where(x => x.AssetType != null && x.AssetType.Id == assetType.Id);
                case EstimatingProject.CostType.Other:
                    return (IEnumerable<TCollectionType>)OtherCosts.Where(x => x.AssetType != null && x.AssetType.Id == assetType.Id);
                default:
                    throw new InvalidOperationException("A cost type was not provided or matched.");
            }
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext).Concat(ValidateTotalCosts(validationContext));
        }

        private IEnumerable<ValidationResult> ValidateTotalCosts(ValidationContext validationContext)
        {
            var project = _container.GetInstance<IEstimatingProjectRepository>().Find(Id);
            
            if (project.ContractorLaborCosts != null && project.ContractorLaborCosts.Any(x => !x.TotalCost.HasValue))
                yield return new ValidationResult(CONTRACTOR_LABOR_COST_ERROR, new[] {"ContractorLaborCosts"});
        }

        #endregion

        #region Constructors

        public PipelineProjectCostEstimate(IContainer container) : base(container) { }

        #endregion
    }
}