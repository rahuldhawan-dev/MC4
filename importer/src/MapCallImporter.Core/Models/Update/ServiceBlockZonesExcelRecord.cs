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
    public class ServiceBlockZonesExcelRecord : ExcelRecordBase<Service, MyEditService, ServiceBlockZonesExcelRecord>
    {
        #region Properties

        public int Id { get; set; }
        public string Block { get; set; }

        #endregion

        #region Exposed Methods

        public override Service MapToEntity(IUnitOfWork uow, int index, ExcelRecordItemHelperBase<Service> helper)
        {
            var entity = uow.GetRepository<Service>().Find(Id);

            if (entity == null)
            {
                helper.AddFailure($"Row {index}: Could not find Service by id {Id}.");
                return null;
            }
           
            uow.Evict(entity);
            entity.SAPErrorCode = "RETRY::";
            return MapToEntity(uow, index, helper, entity);
        }

        public override Service MapAndValidate(IUnitOfWork uow, int index, ExcelRecordItemValidationHelper<Service, MyEditService, ServiceBlockZonesExcelRecord> helper)
        {
            return MapToEntity(uow, index, helper);
        }

        public override void InsertEntity(IUnitOfWork uow, Service entity)
        {
            uow.Update(entity);
        }

        #endregion
    }
}
