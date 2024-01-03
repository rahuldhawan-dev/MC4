using System;
using System.Text.RegularExpressions;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Mappings;
using MapCallImporter.Common;
using MapCallImporter.Library.ClassExtensions;
using MapCallImporter.Validation;
using MapCallImporter.ViewModels;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Data.V2;
using MMSINC.Utilities.ObjectMapping;

namespace MapCallImporter.Models.Import
{
    public class ValveExcelRecord : ExcelRecordBase<Valve, MyCreateValve, ValveExcelRecord>
    {
        #region Properties

        public bool BPUKPI { get; set; }
        public bool Critical { get; set; }
        public string CriticalNotes { get; set; }
        public DateTime? DateRetired { get; set; }
        public DateTime? DateTested { get; set; }
        public decimal? Elevation { get; set; }
        public int? InspectionFrequency { get; set; }
        public string MapPage { get; set; }

        public int? ObjectID { get; set; }

        // NOTE: this column is ignored
        public string Id { get; set; }

        public string SketchNumber { get; set; }
        public string StreetNumber { get; set; }

        [AutoMap(SecondaryPropertyName = "Street")]
        public int StreetId { get; set; }

        public int Town { get; set; }
        public bool Traffic { get; set; }
        public decimal? Turns { get; set; }
        public string ValveLocation { get; set; }
        public string ValveNumber { get; set; }
        public int ValveSuffix { get; set; }
        public string WorkOrderNumber { get; set; }

        public DateTime? DateAdded { get; set; }

        // NOTE: this field is commented out in Valve.cs
        public int? ImageActionID { get; set; }

        public DateTime? DateInstalled { get; set; }
        public DateTime? LastUpdated { get; set; }
        [AutoMap(SecondaryPropertyName = "SAPEquipmentId")]
        public int? SAPEquipmentID { get; set; }

        [AutoMap(SecondaryPropertyName = "ValveBilling")]
        public int ValveBillingId { get; set; }

        [AutoMap(SecondaryPropertyName = "CrossStreet")]
        public int? CrossStreetId { get; set; }

        [AutoMap(SecondaryPropertyName = "InspectionFrequencyUnit")]
        public int? InspectionFrequencyUnitId { get; set; }

        [AutoMap(SecondaryPropertyName = "NormalPosition")]
        public int? NormalPositionId { get; set; }

        [AutoMap(SecondaryPropertyName = "OperatingCenter")]
        public int OperatingCenterId { get; set; }

        [AutoMap(SecondaryPropertyName = "Opens")]
        public int? OpensId { get; set; }

        [AutoMap(SecondaryPropertyName = "TownSection")]
        public int? TownSectionId { get; set; }

        [AutoMap(SecondaryPropertyName = "MainType")]
        public int? MainTypeId { get; set; }

        [AutoMap(SecondaryPropertyName = "ValveControls")]
        public int ValveControlsId { get; set; }

        [AutoMap(SecondaryPropertyName = "ValveMake")]
        public int? ValveMakeId { get; set; }

        [AutoMap(SecondaryPropertyName = "ValveType")]
        public int? ValveTypeId { get; set; }

        [AutoMap(SecondaryPropertyName = "ValveSize")]
        public int? ValveSizeId { get; set; }

        [AutoMap(SecondaryPropertyName = "Status")]
        public int ValveStatusId { get; set; }

        [AutoMap(SecondaryPropertyName = "ValveZone")]
        public int ValveZoneId { get; set; }

        [AutoMap(SecondaryPropertyName = "WaterSystem")]
        public int WaterSystemId { get; set; }

        [AutoMap(SecondaryPropertyName = "FunctionalLocation")]
        public int FunctionalLocationId { get; set; }

        [AutoMap(SecondaryPropertyName = "Initiator")]
        public int? InitiatorId { get; set; }

        // NOTE: only here to prevent errors; MapToEntity creates a new Coordinate using Lat and Lng
        public int? CoordinateId { get; set; }

        public int Route { get; set; }
        public decimal Stop { get; set; }

        [AutoMap(SecondaryPropertyName = "Facility")]
        public int? FacilityId { get; set; }

        [AutoMap(SecondaryPropertyName = "GISUID")]
        public string GeoEFunctionalLocation { get; set; }

        // NOTE: this column is ignored
        public string SAPerrorCode { get; set; }

        public bool? ControlsCrossing { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        [AutoMap(SecondaryPropertyName = "LegacyId")]
        public string LegacyID { get; set; }

        public string VlvTopValveNutDepth { get; set; }

        #endregion

        #region Private Methods

        private bool MatchingValvesExist(IUnitOfWork uow)
        {
            var result = uow.SqlQuery(
                    $"SELECT COUNT(1) FROM Valves v INNER JOIN AssetStatuses vs ON v.AssetStatusId = vs.AssetStatusId WHERE v.ValveNumber = :valveNumber AND v.ValveSuffix = :valveSuffix AND v.OperatingCenterId = :operatingCenter AND DateRetired IS NULL AND vs.Description IN (:valveStatuses)")
                .SetString("valveNumber", ValveNumber)
                .SetInt32("valveSuffix", ValveSuffix)
                .SetInt32("operatingCenter", OperatingCenterId)
                .SetParameterList("valveStatuses", new[] {AssetStatus.Indices.ACTIVE, AssetStatus.Indices.INSTALLED})
                .SafeUniqueIntResult();

            return result.HasValue && result.Value > 0;
        }

        #endregion

        #region Exposed Methods

        protected override MyCreateValve MapExtra(MyCreateValve viewModel, IUnitOfWork uow, int index, ExcelRecordItemHelperBase<Valve> helper)
        {
            viewModel = base.MapExtra(viewModel, uow, index, helper);
            viewModel.IsFoundValve = true;
            TryMapVlvTopValveNutDepth(viewModel);
            ValidateTownAndOperatingCenter(uow, index, helper);
            return viewModel;
        }

        private void ValidateTownAndOperatingCenter(IUnitOfWork uow, int index, ExcelRecordItemHelperBase<Valve> helper)
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

        private void TryMapVlvTopValveNutDepth(MyCreateValve viewModel)
        {
            if (string.IsNullOrWhiteSpace(VlvTopValveNutDepth))
            {
                return;
            }

            var match = new Regex(@"^((\d+)FT ?)?((\d+)IN)?$").Match(VlvTopValveNutDepth);

            void TryMap(int group, Action<int> setFn)
            {
                if (match.Groups[group].Success)
                {
                    setFn(int.Parse(match.Groups[group].Value));
                }
            }

            TryMap(2, feet => viewModel.DepthFeet = feet);
            TryMap(4, inches => viewModel.DepthInches = inches);
        }

        public override Valve MapToEntity(IUnitOfWork uow, int index, ExcelRecordItemHelperBase<Valve> helper)
        {
            Valve valve = null;
            try
            {
                valve = base.MapToEntity(uow, index, helper);
            }
            catch (InvalidOperationException e) when (e.Message.IsMatch("The generated valve number '[^']+' is not unique to the operating center '[^']+'"))
            {
                helper.AddFailure($"Row {index}: {e.Message.Replace("generated ", string.Empty)}");
            }

            if (valve != null)
            {
                valve.Coordinate = new Coordinate {Latitude = Latitude, Longitude = Longitude};
                valve.OpenDirection =
                    helper.RequiredEntityRef<ValveOpenDirection>(uow, OpensId, index, nameof(OpensId));
                valve.Initiator = helper.RequiredEntityRef<User>(uow, InitiatorId, index, nameof(InitiatorId));
                valve.CreatedAt = DateAdded ?? valve.CreatedAt;
                valve.ValveNumber = ValveNumber;
                valve.SAPErrorCode = SAP_RETRY_ERROR_CODE;
            }

            return valve;
        }

        public override Valve MapAndValidate(IUnitOfWork uow, int index,
            ExcelRecordItemValidationHelper<Valve, MyCreateValve, ValveExcelRecord> helper)
        {
            var entity = MapToEntity(uow, index, helper);

            if (entity?.Town == null)
            {
                return null;
            }

            if (MatchingValvesExist(uow))
            {
                helper.AddFailure(
                    $"Valve at row {index} has the same valve number, valve suffix, and operating center as an installed or active valve already in the database.");
            }

            if (entity.Street != null && entity.Street.Town.Id != entity.Town.Id)
            {
                helper.AddFailure(
                    $"Valve at row {index} has StreetId {StreetId} and TownId {Town}, but according to the database that street is in TownId {entity.Street.Town.Id}");
            }

            if (entity.CrossStreet != null && entity.CrossStreet.Town.Id != entity.Town.Id)
            {
                helper.AddFailure(
                    $"Valve at row {index} has CrossStreetId {CrossStreetId} and TownId {Town}, but according to the database that street is in TownId {entity.CrossStreet.Town.Id}");
            }

            if (entity.TownSection != null && entity.TownSection.Town.Id != entity.Town.Id)
            {
                helper.AddFailure(
                    $"Valve at row {index} has TownSectionId {TownSectionId} and TownId {Town}, but according to the database that town section is in TownId {entity.TownSection.Town.Id}");
            }

            return entity;
        }

        public override void InsertEntity(IUnitOfWork uow, Valve entity)
        {
            uow.Insert(entity.Coordinate);
            base.InsertEntity(uow, entity);
        }

        #endregion
    }
}
