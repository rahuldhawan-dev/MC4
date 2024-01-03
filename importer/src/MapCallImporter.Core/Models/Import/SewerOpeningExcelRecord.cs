using System;
using MapCall.Common.Model.Entities;
using MapCallImporter.Common;
using MapCallImporter.Library.ClassExtensions;
using MapCallImporter.Validation;
using MapCallImporter.ViewModels;
using MMSINC.Data.V2;
using MMSINC.Utilities.ObjectMapping;

namespace MapCallImporter.Models.Import
{
    public class SewerOpeningExcelRecord : ExcelRecordBase<SewerOpening, MyCreateSewerOpening, SewerOpeningExcelRecord>
    {
        [AutoMap("SAPEquipmentId")]
        public int? SAPEquipmentID { get; set; }
        [AutoMap("OperatingCenter")]
        public int OperatingCenterID { get; set; }
        [AutoMap("Town")]
        public int TownID { get; set; }
        [AutoMap("WasteWaterSystem")]
        public int? PDESSystem { get; set; }
        [AutoMap("SewerOpeningType")]
        public int? SewerOpeningTypeID { get; set; }
        public int? TownSection { get; set; }
        public string OpeningNumber { get; set; }
        public int OpeningSuffix { get; set; }
        public string OldNumber { get; set; }
        public string StreetNumber { get; set; }
        [AutoMap("Street")]
        public int StreetID { get; set; }
        [AutoMap("IntersectingStreet")]
        public int IntersectingStreetID { get; set; }
        [AutoMap("Status")]
        public int? AssetStatusID { get; set; }
        public decimal? DepthToInvert { get; set; }
        public decimal? RimElevation { get; set; }
        [AutoMap("SewerOpeningMaterial")]
        public int? SewerOpeningMaterialID { get; set; }
        public DateTime? DateInstalled { get; set; }
        public DateTime? DateRetired { get; set; }
        public string MapPage { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string DistanceFromCrossStreet { get; set; }
        public bool? IsEpoxyCoated { get; set; }
        public string GeoEFunctionalLocation { get; set; }
        [AutoMap("FunctionalLocation")]
        public int? FunctionalLocationID { get; set; }
        public bool? IsDoghouseOpening { get; set; }
        public int? Route { get; set; }
        public int? Stop { get; set; }
        public string TaskNumber { get; set; }
        public bool? Critical { get; set; }
        public string CriticalNotes { get; set; }
        public string Notes { get; set; }
        public string SAPErrorCode { get; set; }

        public int? InspectionFrequency { get; set; }

        [AutoMap(SecondaryPropertyName = "InspectionFrequencyUnit")]
        public int? InspectionFrequencyUnitId { get; set; }

        protected override MyCreateSewerOpening MapExtra(MyCreateSewerOpening viewModel, IUnitOfWork uow, int index, ExcelRecordItemHelperBase<SewerOpening> helper)
        {
            viewModel = base.MapExtra(viewModel, uow, index, helper);

            // just to prevent the validation error
            viewModel.Coordinate = 0;
            helper.RequiredEntityRef<AssetStatus>(uow, AssetStatusID, index, nameof(AssetStatusID));

            return viewModel;
        }

        public override SewerOpening MapToEntity(IUnitOfWork uow, int index, ExcelRecordItemHelperBase<SewerOpening> helper)
        {
            var entity = base.MapToEntity(uow, index, helper);

            if (entity != null)
            {
                entity.Coordinate = new Coordinate {
                    Latitude = helper.RequiredDecimalValue(Latitude, index, nameof(Latitude)).Value,
                    Longitude = helper.RequiredDecimalValue(Longitude, index, nameof(Longitude)).Value
                };
            }

            return entity;
        }

        public override SewerOpening MapAndValidate(IUnitOfWork uow, int index, ExcelRecordItemValidationHelper<SewerOpening, MyCreateSewerOpening, SewerOpeningExcelRecord> helper)
        {
            var entity = MapToEntity(uow, index, helper);

            return entity;
        }


        public override void InsertEntity(IUnitOfWork uow, SewerOpening entity)
        {
            uow.Insert(entity.Coordinate);
            base.InsertEntity(uow, entity);
        }
    }
}