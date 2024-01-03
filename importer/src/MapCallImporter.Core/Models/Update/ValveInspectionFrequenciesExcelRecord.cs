using MapCall.Common.Model.Entities;
using MapCallImporter.Common;
using MapCallImporter.Library.ClassExtensions;
using MapCallImporter.Validation;
using MapCallImporter.ViewModels;
using MMSINC.Data.V2;
using MMSINC.Data.V2.NHibernate;
using MMSINC.Utilities.ObjectMapping;

namespace MapCallImporter.Models.Update
{
    public class ValveInspectionFrequenciesExcelRecord : ExcelRecordBase<Valve, MyEditValve, ValveInspectionFrequenciesExcelRecord>
    {
        #region Properties

        public int Id { get; set; }
        public int? InspectionFrequency { get; set; }

        [AutoMap(SecondaryPropertyName = "InspectionFrequencyUnit")]
        public int? FrequencyUnitId { get; set; }
        public int? ValveZone { get; set; }

        #endregion

        #region Exposed Methods

        public override Valve MapToEntity(IUnitOfWork uow, int index, ExcelRecordItemHelperBase<Valve> helper)
        {
            var entity = uow.GetRepository<Valve>().Find(Id);

            if (entity == null)
            {
                helper.AddFailure($"Row {index}: Could not find Valve by id {Id}.");
                return null;
            }

            uow.Evict(entity);
            return MapToEntity(uow, index, helper, entity);
        }

        public override Valve MapAndValidate(IUnitOfWork uow, int index, ExcelRecordItemValidationHelper<Valve, MyEditValve, ValveInspectionFrequenciesExcelRecord> helper)
        {
            return MapToEntity(uow, index, helper);
        }

        public override void InsertEntity(IUnitOfWork uow, Valve entity)
        {
            uow.Update(entity);
        }

        #endregion
    }
}
