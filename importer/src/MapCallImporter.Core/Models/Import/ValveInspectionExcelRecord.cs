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
    public class ValveInspectionExcelRecord : ExcelRecordBase<ValveInspection, MyCreateValveInspection, ValveInspectionExcelRecord>
    {
        #region Private Members

        private static readonly ConcurrentDictionary<(string, int?), int?> _valveCache =
            new ConcurrentDictionary<(string, int?), int?>();

        private static readonly ConcurrentDictionary<string, User> _inspectorCache =
            new ConcurrentDictionary<string, User>();

        #endregion

        #region Properties

        public int? OperatingCenterId { get; set; }
        public DateTime? DateInspected { get; set; }
        public string Inspected { get; set; }
        public int? PositionFound { get; set; }
        public int? PositionLeft { get; set; }
        [AutoMap(SecondaryPropertyName = "Turns")]
        public decimal? NumberOfTurnsCompleted { get; set; }

        [AutoMap(SecondaryPropertyName = "TurnsNotCompleted")]
        public bool? AcceptevenifMinReqTurnsnotcompleted { get; set; }

        public string InspectedBy { get; set; }
        [AutoMap(SecondaryPropertyName = "SAPNotificationNumber")]
        public string Notification { get; set; }

        [AutoMap(SecondaryPropertyName = "Remarks")]
        public string Notes { get; set; }

        public string SAPEquipmentNumber { get; set; }

        #endregion

        #region Private Methods

        protected override MyCreateValveInspection MapExtra(MyCreateValveInspection viewModel, IUnitOfWork uow, int index,
            ExcelRecordItemHelperBase<ValveInspection> helper)
        {
            viewModel = base.MapExtra(viewModel, uow, index, helper);

            viewModel.Valve = LookupValve(uow, index, helper);
            viewModel.Inspected = Inspected?.ToLower() == "yes";

            return viewModel;
        }

        private int? LookupValve(IUnitOfWork uow, int index, ExcelRecordItemHelperBase<ValveInspection> helper)
        {
            if (string.IsNullOrWhiteSpace(
                helper.RequiredStringValue(SAPEquipmentNumber, index, nameof(SAPEquipmentNumber))))
            {
                return null;
            }

            if (_valveCache.ContainsKey((SAPEquipmentNumber, OperatingCenterId)))
            {
                return _valveCache[(SAPEquipmentNumber, OperatingCenterId)];
            }

            var result = uow
                .SqlQuery(
                    "SELECT ID FROM Valves WHERE SAPEquipmentID = :sapEquipmentId AND OperatingCenterId = :operatingCenterId;")
                .SetString("sapEquipmentId", SAPEquipmentNumber)
                .SetInt32("operatingCenterId",
                    helper.RequiredIntValue(OperatingCenterId, index, nameof(OperatingCenterId)).Value)
                .SafeUniqueIntResult();

            if (!result.HasValue)
            {
                helper.AddFailure(
                    $"ValveInspection at row {index} has {nameof(SAPEquipmentNumber)} '{SAPEquipmentNumber}' and {nameof(OperatingCenterId)} '{OperatingCenterId}' but no matching Valve was found in the database.");
            }

            return _valveCache[(SAPEquipmentNumber, OperatingCenterId)] = result;
        }

        private User LookupInspector(IUnitOfWork uow, int index, ExcelRecordItemHelperBase<ValveInspection> helper)
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
                helper.AddFailure($"ValveInspection at row {index} has {nameof(InspectedBy)} '{InspectedBy}' but no record with that EmployeeNumber was found in the database.");
                return null;
            }

            return _inspectorCache[InspectedBy] = new User {
                Id = result.UserId, Employee = new Employee {
                    Id = result.EmployeeId
                }
            };
        }

        #endregion

        #region Exposed Methods

        public override ValveInspection MapToEntity(IUnitOfWork uow, int index, ExcelRecordItemHelperBase<ValveInspection> helper)
        {
            var entity = base.MapToEntity(uow, index, helper);

            if (entity != null)
            {
                entity.InspectedBy = LookupInspector(uow, index, helper);
                entity.SAPErrorCode = SAP_RETRY_ERROR_CODE;
            }

            return entity;
        }

        public override ValveInspection MapAndValidate(IUnitOfWork uow, int index,
            ExcelRecordItemValidationHelper<ValveInspection, MyCreateValveInspection, ValveInspectionExcelRecord>
                helper)
        {
            var entity = MapToEntity(uow, index, helper);

            return entity;
        }

        #endregion
    }
}
