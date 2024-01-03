using MapCall.Common.Model.Entities;
using MapCallImporter.Common;
using MapCallImporter.Library.ClassExtensions;
using MapCallImporter.Models.Import;
using MapCallImporter.Validation;
using MapCallImporter.ViewModels;
using MMSINC.Data;
using MMSINC.Data.V2.NHibernate;
using MMSINC.Utilities.ObjectMapping;
using IUnitOfWork = MMSINC.Data.V2.IUnitOfWork;

namespace MapCallImporter.Models.Update
{
    public class FacilityBooleanAttributesExcelRecord : ExcelRecordBase<Facility, MyEditFacility, FacilityBooleanAttributesExcelRecord>
    {
        public int Id { get; set; }
        public string FacilityName { get; set; }
        public bool PropertyOnly { get; set; }
        public bool Administration { get; set; }
        public bool EmergencyPower { get; set; }
        public bool GroundWaterSupply { get; set; }
        public bool SurfaceWaterSupply { get; set; }
        public bool Reservoir { get; set; }
        public bool Dam { get; set; }
        public bool Interconnection { get; set; }
        public bool PointOfEntry { get; set; }
        public bool WaterTreatmentFacility { get; set; }
        public bool ChemicalFeed { get; set; }
        public string ChemicalStorageLocation { get; set; }
        public bool DPCC { get; set; }
        public bool PSM { get; set; }
        public bool Filtration { get; set; }
        public bool ResidualsGeneration { get; set; }
        public bool TReport { get; set; }
        public bool DistributivePumping { get; set; }
        [AutoMap(SecondaryPropertyName = "BoosterStation")]
        public bool BoosterPumping { get; set; }
        public bool PressureReducing { get; set; }
        public bool GroundStorage { get; set; }
        public bool ElevatedStorage { get; set; }
        public bool OnSiteAnalyticalInstruments { get; set; }
        public bool SampleStation { get; set; }
        [AutoMap(SecondaryPropertyName = "SewerLiftStation")]
        public bool SewerLift { get; set; }
        [AutoMap(SecondaryPropertyName = "WasteWaterTreatmentFacility")]
        public bool SewerTreatment { get; set; }
        public bool FieldOperations { get; set; }
        public bool SpoilsStaging { get; set; }
        public bool CellularAntenna { get; set; }
        public bool SCADAIntrusionAlarm { get; set; }
        public bool RMP { get; set; }
        public bool? UsedInProductionCapacityCalculation { get; set; }
        public bool? IgnitionEnterprisePortal { get; set; }
        public bool? ArcFlashLabelRequired { get; set; }

        #region Private Methods

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
            ChemicalStorageLocationRequiredWhenChemicalFeedIsTrue(index, helper);

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

        public override Facility MapAndValidate(IUnitOfWork uow, int index, ExcelRecordItemValidationHelper<Facility, MyEditFacility, FacilityBooleanAttributesExcelRecord> helper)
        {
            return MapToEntity(uow, index, helper);
        }

        public override void InsertEntity(IUnitOfWork uow, Facility entity)
        {
            uow.Update(entity);
        }

        #endregion
    }
}
