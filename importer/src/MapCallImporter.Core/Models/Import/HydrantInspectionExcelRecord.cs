using System;
using System.Collections.Concurrent;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallImporter.Common;
using MapCallImporter.Library.ClassExtensions;
using MapCallImporter.Validation;
using MapCallImporter.ViewModels;
using MMSINC.Data.V2;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;

namespace MapCallImporter.Models.Import
{
    public class HydrantInspectionExcelRecord : ExcelRecordBase<HydrantInspection, MyCreateHydrantInspection, HydrantInspectionExcelRecord>
    {
        #region Private Members

        private static readonly ConcurrentDictionary<string, int?> _hydrantCache =
            new ConcurrentDictionary<string, int?>();

        private static readonly ConcurrentDictionary<string, User> _inspectorCache =
            new ConcurrentDictionary<string, User>();

        #endregion

        #region Properties

        public DateTime? DateInspected { get; set; }

        [AutoMap(SecondaryPropertyName = "HydrantInspectionType")]
        public int? InspectionType { get; set; }

        public decimal? GPM { get; set; }
        public decimal? MinutesFlowed { get; set; }
        public decimal? StaticPressure { get; set; }

        [AutoMap(SecondaryPropertyName = "Remarks")]
        public string Notes { get; set; }

        public string SAPEquipmentNumber { get; set; }

        [AutoMap(SecondaryPropertyName = "SAPNotificationNumber")]
        public string NotificationNumber { get; set; }

        public string InspectedBy { get; set; }

        #endregion

        #region Private Methods

        protected override MyCreateHydrantInspection MapExtra(MyCreateHydrantInspection viewModel, IUnitOfWork uow, int index, ExcelRecordItemHelperBase<HydrantInspection> helper)
        {
            viewModel = base.MapExtra(viewModel, uow, index, helper);

            viewModel.Hydrant = LookupHydrant(uow, index, helper);
            viewModel.MinutesFlowed = MinutesFlowed.HasValue && MinutesFlowed.Value > 0 ? MinutesFlowed : null;
            // need this to get around these fields being required when Free/Total values
            // aren't set
            viewModel.FreeNoReadReason = NoReadReason.Indices.INSPECT_ONLY;
            viewModel.TotalNoReadReason = NoReadReason.Indices.INSPECT_ONLY;

            return viewModel;
        }

        private int? LookupHydrant(IUnitOfWork uow, int index, ExcelRecordItemHelperBase<HydrantInspection> helper)
        {
            if (string.IsNullOrWhiteSpace(
                helper.RequiredStringValue(SAPEquipmentNumber, index, nameof(SAPEquipmentNumber))))
            {
                return null;
            }

            if (_hydrantCache.ContainsKey(SAPEquipmentNumber))
            {
                return _hydrantCache[SAPEquipmentNumber];
            }

            var result = uow
                .SqlQuery("SELECT ID FROM Hydrants WHERE SAPEquipmentID = :sapEquipmentId")
                .SetString("sapEquipmentId", SAPEquipmentNumber)
                .SafeUniqueIntResult();

            if (!result.HasValue)
            {
                helper.AddFailure(
                    $"Row {index}: HydrantInspection has {nameof(SAPEquipmentNumber)} '{SAPEquipmentNumber}', but no matching Hydrant was found in the database.");
            }

            return _hydrantCache[SAPEquipmentNumber] = result;
        }

        private User LookupInspector(IUnitOfWork uow, int index, ExcelRecordItemHelperBase<HydrantInspection> helper)
        {
            if (string.IsNullOrWhiteSpace(helper.RequiredStringValue(InspectedBy, index, nameof(InspectedBy))))
            {
                return null;
            }

            if (_inspectorCache.ContainsKey(InspectedBy))
            {
                return _inspectorCache[InspectedBy];
            }

            var result = uow.Where<User>(u => u.Employee.EmployeeId == InspectedBy)
                            .Select(u => new {UserId = u.Id, EmployeeId = u.Employee.Id})
                            .SingleOrDefault();

            if (result == null)
            {
                helper.AddFailure(
                    $"HydrantInspection at row {index} has {nameof(InspectedBy)} '{InspectedBy}', but no record with that EmployeeNumber was found in the database.");
                return _inspectorCache[InspectedBy] = null;
            }

            return _inspectorCache[InspectedBy] = new User {
                Id = result.UserId, Employee = new Employee {
                    Id = result.EmployeeId
                }
            };
        }

        #endregion

        #region Exposed Methods

        public override HydrantInspection MapToEntity(IUnitOfWork uow, int index, ExcelRecordItemHelperBase<HydrantInspection> helper)
        {
            var entity = base.MapToEntity(uow, index, helper);

            if (entity != null)
            {
                entity.InspectedBy = LookupInspector(uow, index, helper);
                entity.SAPErrorCode = SAP_RETRY_ERROR_CODE;
            }

            return entity;
        }

        public override HydrantInspection MapAndValidate(IUnitOfWork uow, int index, ExcelRecordItemValidationHelper<HydrantInspection, MyCreateHydrantInspection, HydrantInspectionExcelRecord> helper)
        {
            var entity = MapToEntity(uow, index, helper);

            return entity;
        }

        #endregion
    }
}