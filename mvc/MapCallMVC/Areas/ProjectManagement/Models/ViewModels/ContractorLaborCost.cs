using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Models.ViewModels;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.ProjectManagement.Models.ViewModels
{
    public class ContractorLaborCostViewModel : EntityLookupViewModel<ContractorLaborCost>
    {
        #region Properties

        [Required, StringLength(CreateContractorLaborCostsTable.StringLengths.STOCK_NUMBER)]
        public virtual string StockNumber { get; set; } 
        [Required, StringLength(CreateContractorLaborCostsTable.StringLengths.UNIT)]
        public virtual string Unit { get; set; } 
        [Required, StringLength(AddContractorLaborCostPercentagesForBug2273.StringLengths.JOB_DESCRIPTION)]
        public virtual string JobDescription { get; set; } 
        [StringLength(CreateContractorLaborCostsTable.StringLengths.SUB_DESCRIPTION)]
        public virtual string SubDescription { get; set; } 
        [DisplayFormat(DataFormatString = "{0:c}"), RequiredWhen("Percentage", ComparisonType.EqualTo, null)]
        [Range(0, Double.MaxValue, ErrorMessage = "Cost must be greater than or equal to zero.")]
        public virtual decimal? Cost { get; set; }
        [DisplayFormat(DataFormatString = "{0:p}"), RequiredWhen("Cost", ComparisonType.EqualTo, null)]
        [Range(1, 100)]
        public virtual int? Percentage { get; set; }

        public override string Description
        {
            get
            {
                var desc = string.Format("{0} - {1} - {2}", StockNumber, Unit, JobDescription);
                return desc.Substring(0, Math.Min(desc.Length, 50));
            }
        }

        #endregion

        #region Constructors

        public ContractorLaborCostViewModel(IContainer container) : base(container) {}

        #endregion
    }

    public class AlterOperatingCenterContractorLaborCost : ContractorLaborCostViewModel
    {
        #region Properties

        public virtual IEnumerable<OperatingCenter> OperatingCenters { get; set; }

        [DoesNotAutoMap]
        public virtual int OperatingCenterId { get; set; }

        protected virtual IOperatingCenterRepository OperatingCenterRepository
        {
            get { return _container.GetInstance<IOperatingCenterRepository>(); }
        }

        #endregion

        #region Constructors

        public AlterOperatingCenterContractorLaborCost(IContainer container) : base(container) {}

        #endregion

        #region Exposed Methods

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!OperatingCenterRepository.Exists(OperatingCenterId))
            {
                yield return new ValidationResult(String.Format("Operating Center with id {0} does not exist.", OperatingCenterId));
            }
        }

        public override ContractorLaborCost MapToEntity(ContractorLaborCost entity)
        {
            entity = base.MapToEntity(entity);

            if (OperatingCenters != null)
            {
                entity.OperatingCenters = OperatingCenters.ToList();
            }

            return entity;
        }

        #endregion
    }

    public class RemoveOperatingCenterContractorLaborCost : AlterOperatingCenterContractorLaborCost
    {
        #region Constructors

        public RemoveOperatingCenterContractorLaborCost(IContainer container) : base(container) {}

        #endregion

        #region Exposed Methods

        public override ContractorLaborCost MapToEntity(ContractorLaborCost entity)
        {
            OperatingCenters = OperatingCenters.Where(oc => oc.Id != OperatingCenterId);

            return base.MapToEntity(entity);
        }

        #endregion
    }

    public class AddOperatingCenterContractorLaborCost : AlterOperatingCenterContractorLaborCost
    {
        #region Properties

        [DoesNotAutoMap]
        public OperatingCenter NewOperatingCenter
        {
            get { return OperatingCenterRepository.Find(OperatingCenterId); }
        }

        #endregion

        #region Constructors

        public AddOperatingCenterContractorLaborCost(IContainer container) : base(container) {}

        #endregion

        #region Exposed Methods

        public override ContractorLaborCost MapToEntity(ContractorLaborCost entity)
        {
            OperatingCenters = OperatingCenters.Union(new[] {NewOperatingCenter});

            return base.MapToEntity(entity);
        }

        #endregion
    }
}
