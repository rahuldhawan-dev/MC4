using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Mappings;
using MapCallImporter.Common;
using MapCallImporter.Library;
using MapCallImporter.Validation;
using MapCallImporter.ViewModels;
using MMSINC.Data.V2;

namespace MapCallImporter.Models.Import
{
    public class FacilityExcelRecord : ExcelRecordBase<Facility, MyCreateFacility, FacilityExcelRecord>
    {
        #region Properties
        
        #region Core Fields

        public string FacilityOwnership { get; set; }

        public string FacilityStatus { get; set; }

        public string CompanySubsidiary { get; set; }

        public string Department { get; set; }

        public string OperatingCenter { get; set; }

        public string FunctionalLocation { get; set; }

        public string PlanningPlant { get; set; }

        public string PWSID { get; set; }

        public string PublicWaterSupplyPressureZone { get; set; }

        public string WasteWaterSystemId { get; set; }

        public string WasteWaterSystemBasin { get; set; }

        public string SystemDeliveryType { get; set; }

        public string FacilityName { get; set; }

        public string StreetNumber { get; set; }

        public string Street { get; set; }

        public string Town { get; set; }

        public string ZipCode { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public decimal? FacilityTotalCapacityMGD { get; set; }

        public decimal? FacilityReliableCapacityMGD { get; set; }

        public decimal? FacilityOperatingCapacityMGD { get; set; }

        public decimal? FacilityRatedCapacityMGD { get; set; }

        public decimal? FacilityAuxPowerCapacityMGD { get; set; }

        #endregion

        #region Attributes

        public bool PropertyOnly { get; set; }

        public bool Administration { get; set; }

        public bool EmergencyPower { get; set; }

        public bool GroundWaterSupply { get; set; }

        public bool BasicGroundWaterSupply { get; set; }

        public bool SurfaceWaterSupply { get; set; }

        public bool Reservoir { get; set; }

        public bool Dam { get; set; }

        public bool Interconnection { get; set; }

        public bool PointOfEntry { get; set; }

        public bool WaterTreatmentFacility { get; set; }

        public bool Radionuclides { get; set; }

        public bool ChemicalFeed { get; set; }

        public string ChemicalStorageLocation { get; set; }

        public bool DPCC { get; set; }

        public bool PSM { get; set; }

        public bool Filtration { get; set; }

        public bool ResidualsGeneration { get; set; }

        public bool TReport { get; set; }

        public bool DistributivePumping { get; set; }

        public bool BoosterStation { get; set; }

        public bool PressureReducing { get; set; }

        public bool GroundStorage { get; set; }

        public bool ElevatedStorage { get; set; }

        public bool OnSiteAnalyticalInstruments { get; set; }

        public bool SewerLiftStation { get; set; }

        public bool WasteWaterTreatmentFacility { get; set; }

        public bool FieldOperations { get; set; }

        public bool SpoilsStaging { get; set; }

        public bool CellularAntenna { get; set; }

        public bool SCADAIntrusionAlarm { get; set; }

        public bool RMP { get; set; }

        public bool RawWaterPumpStation { get; set; }

        public bool SWMStation { get; set; }

        public bool WellProd { get; set; }

        public bool WellMonitoring { get; set; }

        public bool ClearWell { get; set; }

        public bool RawWaterIntake { get; set; }

        public bool CommunityRightToKnow { get; set; }

        public bool HasConfinedSpaceRequirement { get; set; }

        public bool SampleStation { get; set; }

        public bool? UsedInProductionCapacityCalculation { get; set; }

        public bool? IgnitionEnterprisePortal { get; set; }

        public bool? ArcFlashLabelRequired { get; set; }

        #endregion

        #endregion

        #region Private Methods

        protected override MyCreateFacility MapExtra(MyCreateFacility viewModel, IUnitOfWork uow, int index,
            ExcelRecordItemHelperBase<Facility> helper)
        {
            viewModel = base.MapExtra(viewModel, uow, index, helper);

            viewModel.FacilityOwner = StringToEntityLookup<FacilityOwner>(uow,
                index,
                helper,
                nameof(FacilityOwnership),
                FacilityOwnership);

            viewModel.Department = StringToEntity<Department>(uow,
                index,
                helper,
                nameof(Department),
                Department,
                x => x.Description == Department);

            viewModel.OperatingCenter = CommonModelMethods.FindOperatingCenter(OperatingCenter,
                nameof(OperatingCenter),
                uow,
                index,
                helper);

            viewModel.PlanningPlant = CommonModelMethods.LookupPlanningPlant(uow, index, helper, nameof(PlanningPlant), PlanningPlant);

            viewModel.Town = CommonModelMethods.LookupTown(viewModel.OperatingCenter,
                OperatingCenter,
                Town,
                nameof(Town),
                uow,
                index,
                helper);

            viewModel.Street = CommonModelMethods.LookupStreet(viewModel.Town,
                Town,
                Street,
                nameof(Street),
                uow,
                index,
                helper);

            viewModel.PublicWaterSupply = StringToEntity<PublicWaterSupply>(uow,
                index,
                helper,
                nameof(PublicWaterSupply),
                PWSID,
                x => x.Identifier == PWSID);

            viewModel.FacilityStatus = StringToEntity<FacilityStatus>(uow,
                index,
                helper,
                nameof(FacilityStatus),
                FacilityStatus,
                fs => fs.Description == FacilityStatus);

            viewModel.CompanySubsidiary = StringToEntityLookup<CompanySubsidiary>(uow,
                index,
                helper,
                nameof(CompanySubsidiary),
                CompanySubsidiary);

            viewModel.PublicWaterSupplyPressureZone = StringToEntity<PublicWaterSupplyPressureZone>(uow,
                index,
                helper,
                nameof(PublicWaterSupplyPressureZone),
                PublicWaterSupplyPressureZone,
                x => x.HydraulicModelName == PublicWaterSupplyPressureZone);

            // WasteWaterSystemId = xxx...ETDDDD where ET is Entity Type and DDDD is Id zero filled in. Have to match both of those parts. Start of field is irrelevant
            // We have to use Substring and TrimStart in this way because it must translate into SQL and we cannot use String Formatting (so Id.ToString() works but not Id.ToString("0000")
            // We have tried many ways to get around this but have failed to do so; use it this way unless you have a breakthrough idea
            viewModel.WasteWaterSystem = StringToEntity<WasteWaterSystem>(uow,
                index,
                helper,
                nameof(WasteWaterSystemId),
                WasteWaterSystemId,
                x => WasteWaterSystemId.Substring(WasteWaterSystemId.Length - 6, 2) == "WW" &&
                     WasteWaterSystemId.Substring(WasteWaterSystemId.Length - 4).TrimStart('0') == x.Id.ToString());

            viewModel.WasteWaterSystemBasin = StringToEntity<WasteWaterSystemBasin>(uow,
                index,
                helper,
                nameof(WasteWaterSystemBasin),
                WasteWaterSystemBasin,
                x => x.BasinName == WasteWaterSystemBasin);

            viewModel.SystemDeliveryType = StringToEntityLookup<SystemDeliveryType>(uow,
                index,
                helper,
                nameof(SystemDeliveryType),
                SystemDeliveryType);

            viewModel.ChemicalStorageLocation = StringToEntity<ChemicalStorageLocation>(uow,
                index,
                helper,
                nameof(ChemicalStorageLocation),
                ChemicalStorageLocation,
                x => x.StorageLocationDescription == ChemicalStorageLocation);

            return viewModel;
        }

        private void EnsureFunctionalLocationUnique(IUnitOfWork uow, int index, ExcelRecordItemHelperBase<Facility> helper)
        {
            var result = uow
                        .SqlQuery(
                             $"SELECT COUNT(1) FROM {FacilityMap.TABLE_NAME} WHERE FunctionalLocationId = :functionalLocation")
                        .SetString("functionalLocation", FunctionalLocation)
                        .SafeUniqueIntResult();

            if (result.HasValue && result.Value > 0)
            {
                helper.AddFailure($"Row {index}: at least one Facility with {nameof(FunctionalLocation)} '{FunctionalLocation}' already exists in the database.");
            }
        }

        private void EnsureBothPublicWaterSupplyAndWasteWaterSystemAreNotSpecified(int index, ExcelRecordItemHelperBase<Facility> helper)
        {
            if (!string.IsNullOrWhiteSpace(WasteWaterSystemId) && !string.IsNullOrWhiteSpace(PWSID))
            {
                helper.AddFailure($"Row {index}: Cannot specify both {nameof(WasteWaterSystemId)} and {nameof(PWSID)}. Only one or the other are allowed.");
            }
        }

        private void EnsureAtLeastOneOfPublicWaterSupplyOrWasteWaterSystemAreSpecified(int index, ExcelRecordItemHelperBase<Facility> helper)
        {
            if (string.IsNullOrWhiteSpace(WasteWaterSystemId) && string.IsNullOrWhiteSpace(PWSID))
            {
                helper.AddFailure($"Row {index}: Must specify either {nameof(WasteWaterSystemId)} or {nameof(PWSID)}.");
            }
        }

        private void PublicWaterSupplyRequiredWhenPublicWaterSupplyPressureZoneIsNotNull(int index, ExcelRecordItemHelperBase<Facility> helper)
        {
            if (!string.IsNullOrWhiteSpace(PublicWaterSupplyPressureZone) && string.IsNullOrWhiteSpace(PWSID))
            {
                helper.AddFailure($"Row {index}: {nameof(PWSID)} is required when {nameof(PublicWaterSupplyPressureZone)} has been specified.");
            }
        }

        private void WasteWaterSystemIdRequiredWhenWasteWaterSystemBasinIsNotNull(int index, ExcelRecordItemHelperBase<Facility> helper)
        {
            if (!string.IsNullOrWhiteSpace(WasteWaterSystemBasin) && string.IsNullOrWhiteSpace(WasteWaterSystemId))
            {
                helper.AddFailure($"Row {index}: {nameof(WasteWaterSystemId)} is required when {nameof(WasteWaterSystemBasin)} has been specified.");
            }
        }

        public void ChemicalStorageLocationRequiredWhenChemicalFeedIsTrue(int index, ExcelRecordItemHelperBase<Facility> helper)
        {
            if (ChemicalFeed == true && string.IsNullOrWhiteSpace(ChemicalStorageLocation))
            {
                helper.AddFailure($"Row {index}: {nameof(ChemicalStorageLocation)} is required when {nameof(ChemicalFeed)} is true.");
            }
        }

        #endregion

        #region Exposed Methods

        public override Facility MapToEntity(IUnitOfWork uow, int index, ExcelRecordItemHelperBase<Facility> helper)
        {
            EnsureFunctionalLocationUnique(uow, index, helper);
            EnsureBothPublicWaterSupplyAndWasteWaterSystemAreNotSpecified(index, helper);
            EnsureAtLeastOneOfPublicWaterSupplyOrWasteWaterSystemAreSpecified(index, helper);
            PublicWaterSupplyRequiredWhenPublicWaterSupplyPressureZoneIsNotNull(index, helper);
            WasteWaterSystemIdRequiredWhenWasteWaterSystemBasinIsNotNull(index, helper);
            ChemicalStorageLocationRequiredWhenChemicalFeedIsTrue(index, helper);

            var facility = base.MapToEntity(uow, index, helper);

            if (facility != null)
            {
                if ((!Latitude.HasValue && Longitude.HasValue) || (Latitude.HasValue && !Longitude.HasValue))
                {
                    helper.AddFailure($"Row {index}: Cannot create a coordinate without a value for both {nameof(Latitude)} and {nameof(Longitude)}");
                }
                else if (Latitude.HasValue && Longitude.HasValue)
                {
                    facility.Coordinate = new Coordinate { Latitude = Latitude.Value, Longitude = Longitude.Value };
                }
            }

            return facility;
        }

        public override Facility MapAndValidate(IUnitOfWork uow, int index,
            ExcelRecordItemValidationHelper<Facility, MyCreateFacility, FacilityExcelRecord> helper)
        {
            var entity = MapToEntity(uow, index, helper);

            return entity;
        }

        #endregion
    }
}