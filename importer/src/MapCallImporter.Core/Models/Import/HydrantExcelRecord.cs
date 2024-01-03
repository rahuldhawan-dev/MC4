using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Mappings;
using MapCallImporter.Common;
using MapCallImporter.Library.ClassExtensions;
using MapCallImporter.Validation;
using MapCallImporter.ViewModels;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Data.V2;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;

namespace MapCallImporter.Models.Import
{
    public class HydrantExcelRecord : ExcelRecordBase<Hydrant, MyCreateHydrant, HydrantExcelRecord>
    {
        #region Properties

        public bool IsNonBPUKPI { get; set; }
        public bool Critical { get; set; }
        public string CriticalNotes { get; set; }
        public DateTime? DateInstalled { get; set; }
        public DateTime? DateRetired { get; set; }
        public DateTime? DateTested { get; set; }
        public bool IsDeadEndMain { get; set; }
        public decimal? Elevation { get; set; }
        public string HydrantNumber { get; set; }
        public int HydrantSuffix { get; set; }
        public int? InspectionFrequency { get; set; }

        public string LateralValveId { get; set; }

        // ignored
        public string LateralValveRemove { get; set; }

        public string Location { get; set; }
        public string MapPage { get; set; }
        public int? Id { get; set; }

        [AutoMap(SecondaryPropertyName = "Street")]
        public int? StreetId { get; set; }

        public string StreetNumber { get; set; }
        public int Town { get; set; }
        public string ValveLocation { get; set; }
        public string WorkOrderNumber { get; set; }
        public DateTime? DateAdded { get; set; }

        [AutoMap(SecondaryPropertyName = "FireDistrict")]
        public int FireDistrictID { get; set; }

        public int? YearManufactured { get; set; }
        public int Zone { get; set; }
        public string ClowTagged { get; set; }
        public int? ObjectID { get; set; }

        [AutoMap(SecondaryPropertyName = "HydrantTagStatus")]
        public int? HydrantTagStatusID { get; set; }

        public DateTime? BillingDate { get; set; }

        [AutoMap(SecondaryPropertyName = "HydrantManufacturer")]
        public int? ManufacturerID { get; set; }

        [AutoMap(SecondaryPropertyName = "HydrantModel")]
        public int? HydrantModelID { get; set; }

        public int? FLRouteNumber { get; set; }
        public int? FLRouteSequence { get; set; }
        public int? BranchLengthFeet { get; set; }
        public int? BranchLengthInches { get; set; }
        public int? DepthBuryFeet { get; set; }
        public int? DepthBuryInches { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? LastUpdated { get; set; }

        [AutoMap(SecondaryPropertyName = "SAPEquipmentId")]
        public int? SAPEquipmentID { get; set; }

        [AutoMap(SecondaryPropertyName = "Status")]
        public int HydrantStatusId { get; set; }

        [AutoMap(SecondaryPropertyName = "LateralSize")]
        public int? LateralSizeId { get; set; }

        [AutoMap(SecondaryPropertyName = "CrossStreet")]
        public int? CrossStreetId { get; set; }

        [AutoMap(SecondaryPropertyName = "OpenDirection")]
        public int? OpensDirectionId { get; set; }

        [AutoMap(SecondaryPropertyName = "Gradient")]
        public int? GradientId { get; set; }

        [AutoMap(SecondaryPropertyName = "HydrantSize")]
        public int? HydrantSizeId { get; set; }

        [AutoMap(SecondaryPropertyName = "Initiator")]
        public int? InitiatorId { get; set; }

        [AutoMap(SecondaryPropertyName = "InspectionFrequencyUnit")]
        public int? InspectionFrequencyUnitId { get; set; }

        [AutoMap(SecondaryPropertyName = "OperatingCenter")]
        public int OperatingCenterId { get; set; }

        [AutoMap(SecondaryPropertyName = "HydrantMainSize")]
        public int? HydrantMainSizeId { get; set; }

        [AutoMap(SecondaryPropertyName = "HydrantThreadType")]
        public int? HydrantThreadTypeId { get; set; }

        [AutoMap(SecondaryPropertyName = "MainType")]
        public int? MainTypeId { get; set; }

        [AutoMap(SecondaryPropertyName = "TownSection")]
        public int? TownSectionId { get; set; }

        [AutoMap(SecondaryPropertyName = "HydrantBilling")]
        public int HydrantBillingId { get; set; }

        public int? CoordinateId { get; set; }

        [AutoMap(SecondaryPropertyName = "WaterSystem")]
        public int WaterSystemId { get; set; }

        [AutoMap(SecondaryPropertyName = "FunctionalLocation")]
        public int FunctionalLocationId { get; set; }

        public int Route { get; set; }
        public decimal? Stop { get; set; }

        [AutoMap(SecondaryPropertyName = "Facility")]
        public int? FacilityId { get; set; }

        [AutoMap(SecondaryPropertyName = "GISUID")]
        public string GeoEFunctionalLocation { get; set; }

        public string SAPerrorCode { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int? LateralValve { get; set; }

        [AutoMap(SecondaryPropertyName = "LegacyId")]
        public string LegacyID { get; set; }

        #endregion

        #region Private Methods

        private Valve LookupLateralValve(IUnitOfWork uow, int index,
            ExcelRecordItemHelperBase<Hydrant> helper)
        {
            if (string.IsNullOrWhiteSpace(LateralValveId))
            {
                return null;
            }

            var result = uow.SqlQuery("SELECT Id FROM Valves WHERE ValveNumber = :valveNumber AND Town = :town")
                .SetString("valveNumber", LateralValveId)
                .SetInt32("town", Town)
                .SafeUniqueIntResult();

            if (result.HasValue)
            {
                return new Valve {Id = result.Value};
            }

            helper.AddFailure(
                $"Hydrant at row {index} has LateralValveId {LateralValveId} but no matching Valve was found in the database.");
            return null;
        }

        protected override MyCreateHydrant MapExtra(MyCreateHydrant viewModel, IUnitOfWork uow, int index, ExcelRecordItemHelperBase<Hydrant> helper)
        {
            viewModel = base.MapExtra(viewModel, uow, index, helper);
            viewModel.IsFoundHydrant = true;
            ValidateTownAndOperatingCenter(uow, index, helper);
            return viewModel;
        }

        private void ValidateTownAndOperatingCenter(IUnitOfWork uow, int index, ExcelRecordItemHelperBase<Hydrant> helper)
        {
            var count = uow
                       .SqlQuery(
                            $"SELECT COUNT(1) FROM {OperatingCenterTownMap.TABLE_NAME} WHERE TownId = :townId AND OperatingCenterId = :operatingCenterId;")
                       .SetInt32("townId", Town)
                       .SetInt32("operatingCenterId", OperatingCenterId)
                       .SafeUniqueIntResult();

            if (count < 1)
            {
                helper.AddFailure(
                    $"Row {index}: Town {Town} does not exist within operating center {OperatingCenterId}.  Please adjust the value in the file or add an OperatingCenterTown record through MapCall.");
            }
        }

        #endregion

        #region Exposed Methods

        public override Hydrant MapToEntity(IUnitOfWork uow, int index,
            ExcelRecordItemHelperBase<Hydrant> helper)
        {
            Hydrant hydrant = null;
            try
            {
                hydrant = base.MapToEntity(uow, index, helper);
            }
            catch (InvalidOperationException e) when (e.Message.IsMatch("The generated hydrant number '[^']+' is not unique to the operating center '[^']+'"))
            {
                helper.AddFailure($"Row {index}: {e.Message.Replace("generated ", string.Empty)}");
            }

            if (hydrant != null)
            {
                hydrant.Coordinate = new Coordinate {Latitude = Latitude, Longitude = Longitude};
                hydrant.Initiator = helper.RequiredEntityRef<User>(uow, InitiatorId, index, nameof(InitiatorId));
                hydrant.LateralValve = LookupLateralValve(uow, index, helper);
                hydrant.CreatedAt = DateAdded ?? hydrant.CreatedAt;
                hydrant.HydrantNumber = HydrantNumber;
                hydrant.SAPErrorCode = SAP_RETRY_ERROR_CODE;
            }

            return hydrant;
        }

        public override Hydrant MapAndValidate(IUnitOfWork uow, int index,
            ExcelRecordItemValidationHelper<Hydrant, MyCreateHydrant, HydrantExcelRecord> helper)
        {
            var entity = MapToEntity(uow, index, helper);

            return entity;
        }

        public override void InsertEntity(IUnitOfWork uow, Hydrant entity)
        {
            uow.Insert(entity.Coordinate);
            base.InsertEntity(uow, entity);
        }

        #endregion
    }
}
