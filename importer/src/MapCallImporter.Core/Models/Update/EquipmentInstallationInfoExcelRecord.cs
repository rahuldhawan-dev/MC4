using System;
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
    public class EquipmentInstallationInfoExcelRecord : ExcelRecordBase<Equipment, MyEditEquipment, EquipmentInstallationInfoExcelRecord>
    {
        #region Properties

        public int SAPEquipmentId { get; set; }
        public object Description { get; set; }
        public DateTime DateInstalled { get; set; }
        public DateTime DateRetired { get; set; }
        public string ScadaTagName { get; set; }

        #endregion

        protected override MyEditEquipment MapExtra(MyEditEquipment viewModel, IUnitOfWork uow, int index, ExcelRecordItemHelperBase<Equipment> helper)
        {
            viewModel = base.MapExtra(viewModel, uow, index, helper);

            viewModel.ScadaTagName = StringToEntity<ScadaTagName>(uow, index, helper, nameof(ScadaTagName),
                ScadaTagName, x => x.TagName == ScadaTagName);

            return viewModel;
        }

        #region Exposed Methods

        public override Equipment MapToEntity(IUnitOfWork uow, int index, ExcelRecordItemHelperBase<Equipment> helper)
        {
            var entity = uow.GetRepository<Equipment>().Where(e => e.SAPEquipmentId == SAPEquipmentId).SingleOrDefault();

            if (entity == null)
            {
                helper.AddFailure($"Row {index}: Could not find equipment by SAPEquipmentId {SAPEquipmentId}.");
                return null;
            }

            entity.ProductionPrerequisites.GetEnumerator();
            entity.Characteristics.GetEnumerator();
            entity.WorkOrders.GetEnumerator();
            uow.Evict(entity);
            entity.SAPErrorCode = "RETRY::";
            return MapToEntity(uow, index, helper, entity);
        }

        public override Equipment MapAndValidate(IUnitOfWork uow, int index, ExcelRecordItemValidationHelper<Equipment, MyEditEquipment, EquipmentInstallationInfoExcelRecord> helper)
        {
            return MapToEntity(uow, index, helper);
        }

        public override void InsertEntity(IUnitOfWork uow, Equipment entity)
        {
            uow.Update(entity);
        }

        #endregion
    }
}