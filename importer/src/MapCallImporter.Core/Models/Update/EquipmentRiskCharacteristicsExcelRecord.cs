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
    public class EquipmentRiskCharacteristicsExcelRecord : ExcelRecordBase<Equipment, MyEditEquipment, EquipmentRiskCharacteristicsExcelRecord>
    {
        #region Properties

        public int Equipment { get; set; }
        public object Description { get; set; }
        public object Planningplant { get; set; }
        public string EquipmentCondition { get; set; }
        public string EquipmentPerformance { get; set; }
        public string EquipmentConsequenceofFailure { get; set; }
        public string EquipmentStaticDynamicType { get; set; }

        #endregion

        #region Private Methods

        protected override MyEditEquipment MapExtra(MyEditEquipment viewModel, IUnitOfWork uow, int index, ExcelRecordItemHelperBase<Equipment> helper)
        {
            viewModel = base.MapExtra(viewModel, uow, index, helper);

            viewModel.Condition =
                StringToEntityLookup<EquipmentCondition>(uow, index, helper, nameof(EquipmentCondition),
                    EquipmentCondition);
            viewModel.Performance =
                StringToEntityLookup<EquipmentPerformanceRating>(uow, index, helper, nameof(EquipmentPerformance),
                    EquipmentPerformance);
            viewModel.ConsequenceOfFailure =
                StringToEntityLookup<EquipmentConsequencesOfFailureRating>(uow, index, helper, nameof(EquipmentConsequenceofFailure),
                    EquipmentConsequenceofFailure);
            viewModel.StaticDynamicType =
                StringToEntityLookup<EquipmentStaticDynamicType>(uow, index, helper, nameof(EquipmentStaticDynamicType),
                    EquipmentStaticDynamicType);

            return viewModel;
        }

        #endregion

        #region Exposed Methods

        public override Equipment MapToEntity(IUnitOfWork uow, int index, ExcelRecordItemHelperBase<Equipment> helper)
        {
            if (Equipment == 0)
            {
                helper.AddFailure($"Row {index}: no value has been provided for {nameof(Equipment)} to map to MapCall's SAPEquipmentId");
                return null;
            }

            var entities = uow.GetRepository<Equipment>().Where(e => e.SAPEquipmentId == Equipment);
            var count = entities.Count();

            if (count < 1)
            {
                helper.AddFailure($"Row {index}: Could not find equipment by SAPEquipmentId {Equipment}.");
                return null;
            }
            if (count > 1)
            {
                helper.AddFailure($"Row {index}: Found more than one piece of equipment with SAPEquipmentId {Equipment}.");
                return null;
            }

            var entity = entities.Single();

            entity.ProductionPrerequisites.GetEnumerator();
            entity.Characteristics.GetEnumerator();
            uow.Evict(entity);
            entity.SAPErrorCode = "RETRY::";
            return MapToEntity(uow, index, helper, entity);
        }

        public override Equipment MapAndValidate(IUnitOfWork uow, int index, ExcelRecordItemValidationHelper<Equipment, MyEditEquipment, EquipmentRiskCharacteristicsExcelRecord> helper)
        {
            var entity = MapToEntity(uow, index, helper);

            return entity;
        }

        public override void InsertEntity(IUnitOfWork uow, Equipment entity)
        {
            if (entity.EquipmentModel != null)
            {
                // for some reason it doesn't always like the value it had from the old session
                entity.EquipmentModel = uow.Find<EquipmentModel>(entity.EquipmentModel.Id);
            }

            if (entity.ProductionPrerequisites.Any())
            {
                // nor does it like these much either.  this was resulting in:
                // NHibernate.NonUniqueObjectException: a different object with the same identifier value was already associated with the session: 1, of entity: MapCall.Common.Model.Entities.ProductionPrerequisite
                entity.ProductionPrerequisites =
                    uow.Where<ProductionPrerequisite>(pp => pp.Equipment.Any(e => e.Id == entity.Id)).ToList();
            }

            uow.Update(entity);
        }

        #endregion
    }
}
