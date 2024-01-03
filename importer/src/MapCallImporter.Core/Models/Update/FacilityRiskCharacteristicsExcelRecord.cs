using System.Linq;
using MapCall.Common.Model.Entities;
using MapCallImporter.Common;
using MapCallImporter.Library.ClassExtensions;
using MapCallImporter.Validation;
using MapCallImporter.ViewModels;
using MMSINC.Data.V2;
using MMSINC.Data.V2.NHibernate;

namespace MapCallImporter.Models.Update
{
    public class FacilityRiskCharacteristicsExcelRecord : ExcelRecordBase<Facility, MyEditFacility, FacilityRiskCharacteristicsExcelRecord>
    {
        public int Id { get; set; }
        public object OperatingCenter { get; set; }
        public object FacilityName { get; set; }
        public string FacilityConditions { get; set; }
        public string FacilityPerformance { get; set; }
        public string FacilityConsequencesOfFailure { get; set; }

        protected override MyEditFacility MapExtra(MyEditFacility viewModel, IUnitOfWork uow, int index, ExcelRecordItemHelperBase<Facility> helper)
        {
            viewModel = base.MapExtra(viewModel, uow, index, helper);

            viewModel.Condition =
                StringToEntityLookup<FacilityCondition>(uow, index, helper, nameof(FacilityConditions),
                    FacilityConditions);
            viewModel.Performance = StringToEntityLookup<FacilityPerformance>(uow, index, helper,
                nameof(FacilityPerformance), FacilityPerformance);
            viewModel.ConsequenceOfFailure = StringToEntityLookup<FacilityConsequenceOfFailure>(uow, index, helper,
                nameof(FacilityConsequencesOfFailure), FacilityConsequencesOfFailure);

            return viewModel;
        }

        #region Exposed Methods

        public override Facility MapToEntity(IUnitOfWork uow, int index, ExcelRecordItemHelperBase<Facility> helper)
        {
            var entity = uow.GetRepository<Facility>().Find(Id);

            if (entity == null)
            {
                helper.AddFailure($"Row {index}: Could not find Facility by id {Id}.");
                return null;
            }

            entity.Equipment.GetEnumerator();
            uow.Evict(entity);
            return MapToEntity(uow, index, helper, entity);
        }

        public override Facility MapAndValidate(IUnitOfWork uow, int index,
            ExcelRecordItemValidationHelper<Facility, MyEditFacility, FacilityRiskCharacteristicsExcelRecord> helper)
        {
            var entity = MapToEntity(uow, index, helper);

            return entity;
        }

        public override void InsertEntity(IUnitOfWork uow, Facility entity)
        {
            // need to reload these to prevent an issue with EquipmentModel
            // potentially being referenced from out of session
            foreach (var equipment in entity.Equipment)
            {
                if (equipment.EquipmentModel != null)
                {
                    equipment.EquipmentModel = uow.Where<EquipmentModel>(em => em.Id == equipment.EquipmentModel.Id).Single();
                }
            }

            // need to reload this due to a similar issue
            if (entity.Coordinate != null)
            {
                entity.Coordinate = uow.Find<Coordinate>(entity.Coordinate.Id);
            }

            uow.Update(entity);
        }

        #endregion
    }
}