using MapCall.Common.Model.Entities;
using MapCallImporter.Common;
using MapCallImporter.Validation;
using MapCallImporter.ViewModels;
using MMSINC.Data.V2;
using MMSINC.Utilities.ObjectMapping;
using System;
using System.Web.UI.WebControls;

namespace MapCallImporter.Models.Import
{
    public class ContractorOverrideLaborCostExcelRecord : ExcelRecordBase<ContractorOverrideLaborCost, MyCreateContractorOverrideLaborCost, ContractorOverrideLaborCostExcelRecord>
    {
        #region Properties

        [DoesNotAutoMap]
        public string Contractor { get; set; }

        [DoesNotAutoMap]
        public string StockNumber { get; set; }

        [DoesNotAutoMap]
        public string Unit { get; set; }

        [DoesNotAutoMap]
        public string Description { get; set; }

        [DoesNotAutoMap]
        public string OperatingCenterCode { get; set; }

        public decimal? Cost { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public int? Percentage { get; set; }

        [AutoMap(SecondaryPropertyName = "Contractor")]
        public int? ContractorId { get; set; }

        [AutoMap(SecondaryPropertyName = "ContractorLaborCost")]
        public int? ContractorLaborCostId { get; set; }

        [AutoMap(SecondaryPropertyName = "OperatingCenter")]
        public int? OperatingCenterId { get; set; }

        #endregion

        #region Exposed Methods

        protected override MyCreateContractorOverrideLaborCost MapExtra(MyCreateContractorOverrideLaborCost viewModel, IUnitOfWork uow, int index, ExcelRecordItemHelperBase<ContractorOverrideLaborCost> helper)
        {
            viewModel = base.MapExtra(viewModel, uow, index, helper);

            viewModel.Contractor = viewModel.Contractor != null
                ? viewModel.Contractor
                : StringToEntity<Contractor>(uow, index, helper,
                    nameof(Contractor), Contractor,
                    x => x.Name == Contractor);

            viewModel.ContractorLaborCost = viewModel.ContractorLaborCost != null
                ? viewModel.ContractorLaborCost
                : StringToEntity<ContractorLaborCost>(uow, index, helper,
                    nameof(ContractorLaborCost), Description,
                    x => x.JobDescription != null && x.StockNumber == StockNumber &&
                         x.Unit == Unit && x.JobDescription == Description);

            viewModel.OperatingCenter = viewModel.OperatingCenter != null
                ? viewModel.OperatingCenter
                : StringToEntity<OperatingCenter>(uow, index, helper,
                    nameof(OperatingCenter), OperatingCenterCode,
                    x => x.OperatingCenterCode == OperatingCenterCode);

            return viewModel;
        }

        public override ContractorOverrideLaborCost MapAndValidate(IUnitOfWork uow, int index, ExcelRecordItemValidationHelper<ContractorOverrideLaborCost, MyCreateContractorOverrideLaborCost, ContractorOverrideLaborCostExcelRecord> helper)
        {
            var entity = MapToEntity(uow, index, helper);

            return entity;
        }

        #endregion
    }
}
