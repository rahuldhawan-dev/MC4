using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallImporter.Common;
using MapCallImporter.Library;
using MapCallImporter.Validation;
using MapCallImporter.ViewModels;
using MMSINC.Data.V2;
using MMSINC.Utilities.ObjectMapping;

namespace MapCallImporter.Models.Import
{
    public class LockoutDeviceExcelRecord : ExcelRecordBase<LockoutDevice, MyCreateLockoutDevice, LockoutDeviceExcelRecord>
    {
        #region Properties

        public string OperatingCenter { get; set; }

        [AutoMap(SecondaryPropertyName = "User")]
        public string Person { get; set; }
        public string SerialNumber { get; set; }
        public string Description { get; set; }
        public string LockoutDeviceColor { get; set; }

        #endregion

        #region Private Methods

        protected override MyCreateLockoutDevice MapExtra(MyCreateLockoutDevice viewModel, IUnitOfWork uow, int index,
            ExcelRecordItemHelperBase<LockoutDevice> helper)
        {
            viewModel = base.MapExtra(viewModel, uow, index, helper);

            viewModel.OperatingCenter = CommonModelMethods.FindOperatingCenterByName(OperatingCenter,
                nameof(OperatingCenter),
                uow,
                index,
                helper);

            viewModel.Person = StringToEntity<User>(uow,
                index,
                helper,
                nameof(Person),
                Person,
                x => x.FullName == Person);

            viewModel.LockoutDeviceColor = StringToEntityLookup<LockoutDeviceColor>(uow,
                index,
                helper,
                nameof(LockoutDeviceColor),
                LockoutDeviceColor);

            return viewModel;
        }

        #endregion

        #region Exposed Methods

        public override LockoutDevice MapAndValidate(IUnitOfWork uow, int index, ExcelRecordItemValidationHelper<LockoutDevice, MyCreateLockoutDevice, LockoutDeviceExcelRecord> helper)
        {
            var entity = MapToEntity(uow, index, helper);

            if (entity == null)
            {
                return null;
            }

            return entity;
        }

        #endregion
    }
}
