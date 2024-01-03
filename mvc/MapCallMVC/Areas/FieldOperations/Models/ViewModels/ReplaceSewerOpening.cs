using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.SewerMainCleanings;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class ReplaceSewerOpening : EditSewerOpening
    {
        #region Private Members

        private SewerOpening _retiredSewerOpening;
        protected readonly IViewModelFactory _viewModelFactory;

        #endregion

        #region Constructors

        public ReplaceSewerOpening(IContainer container, IViewModelFactory viewModelFactory, AssetStatusNumberDuplicationValidator numberValidator) : base(container, numberValidator)
        {
            _viewModelFactory = viewModelFactory;
        }

        #endregion

        #region Exposed Methods

        public override void Map(SewerOpening entity)
        {
            base.Map(entity);
            _retiredSewerOpening = entity;
        }

        public override SewerOpening MapToEntity(SewerOpening entity)
        {
            // These need to be set before the base.MapToEntity call because otherwise the
            // SendToSAP setter will fail.
            entity.OperatingCenter = _retiredSewerOpening.OperatingCenter;
            entity.Town = _retiredSewerOpening.Town;

            // NOTE: The entity being passed to this is NOT an existing hydrant record!
            base.MapToEntity(entity);

            // Do not populate the SAP Equipment ID when creating the new pending asset record.  
            // This way when the new pending record is saved, it will automatically create a new record in SAP.
            entity.SAPEquipmentId = null;

            // All replacement hydrants must be set to pending initially.
            entity.Status = _container.GetInstance<IAssetStatusRepository>().GetPendingStatus();

            // Copy the coordinate to a new coordinate record.
            var rc = _retiredSewerOpening.Coordinate;
            if (rc != null)
            {
                entity.Coordinate = new Coordinate {
                    // Not all Coordinate records have an icon set, but it's mapped as not nullable so set it to a default.
                    Icon = rc.Icon ?? _container.GetInstance<IIconSetRepository>().GetDefaultIconSet(_container.GetInstance<IRepository<MapIcon>>()).DefaultIcon,
                    Latitude = rc.Latitude,
                    Longitude = rc.Longitude
                };
            }

            foreach (var cleaning in _retiredSewerOpening.SewerMainCleanings1)
            {
                // NOTE: Important to use the EDIT view model as the create view model will overwrite data.
                // NOTE 2: We're not supposed to copy inspections over to SAP. http://bugzilla.mapcall.info/bugzilla/show_bug.cgi?id=3283#c9
                var model = _viewModelFactory.Build<EditSewerMainCleaning, SewerMainCleaning>(cleaning);
                model.Opening1 = null;
                var inspectCopy = new SewerMainCleaning();
                model.MapToEntity(inspectCopy);
                inspectCopy.Opening1 = entity;
                entity.SewerMainCleanings1.Add(inspectCopy);
            }

            foreach (var cleaning in _retiredSewerOpening.SewerMainCleanings2)
            {
                // NOTE: Important to use the EDIT view model as the create view model will overwrite data.
                // NOTE 2: We're not supposed to copy inspections over to SAP. http://bugzilla.mapcall.info/bugzilla/show_bug.cgi?id=3283#c9
                var model = _viewModelFactory.Build<EditSewerMainCleaning, SewerMainCleaning>(cleaning);
                model.Opening2 = null;
                var inspectCopy = new SewerMainCleaning();
                model.MapToEntity(inspectCopy);
                inspectCopy.Opening2 = entity;
                entity.SewerMainCleanings2.Add(inspectCopy);
            }

            return entity;
        }

        #endregion
    }
}
