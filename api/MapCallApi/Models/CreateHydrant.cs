using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallApi.Models
{
    public class CreateHydrant : ViewModel<Hydrant>
    {
        #region Constants

        public const string SAP_ERROR_CODE = "RETRY::INITIAL RECORD CREATED";

        #endregion

        #region Properties
        
        [Required, EntityMap("LateralValve"), EntityMustExist(typeof(Valve))]
        public int? Valve { get; set; }
        [Required, EntityMap, EntityMustExist(typeof(FireDistrict))]
        public int? FireDistrict { get; set; }
        [Required, AutoMap("WorkOrderNumber")]
        public string WbsNumber { get; set; }
        [Required]
        public decimal? Latitude { get; set; }
        [Required]
        public decimal? Longitude { get; set; }
        [EntityMap]
        public int? Coordinate { get; set; }

        #endregion

        #region Constructors

        public CreateHydrant(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override Hydrant MapToEntity(Hydrant entity)
        {
            entity = base.MapToEntity(entity);

            var billingPublic = _container.GetInstance<IRepository<HydrantBilling>>().Find(HydrantBilling.Indices.PUBLIC);
            var statusNsiPending = _container.GetInstance<IRepository<AssetStatus>>().Find(AssetStatus.Indices.NSI_PENDING);

            entity.HydrantBilling = billingPublic;
            entity.Status = statusNsiPending;

            entity.Initiator = _container.GetInstance<IAuthenticationService<User>>().CurrentUser;

            var valveRepository = _container.GetInstance<IRepository<Valve>>();
            var valve = valveRepository.Find(this.Valve.Value);
            entity.OperatingCenter = valve.OperatingCenter;
            entity.Town = valve.Town;
            entity.Street = valve.Street;
            entity.CrossStreet = valve.CrossStreet;

            var hydrantRepository = _container.GetInstance<IRepository<Hydrant>>();

            var hydrantNumber = hydrantRepository.GenerateNextHydrantNumber(_container.GetInstance<IAbbreviationTypeRepository>(),
                _container.GetInstance<RepositoryBase<Hydrant>>(),
                entity.OperatingCenter, entity.Town, entity.TownSection, null);

            if (!HydrantNumberIsUniqueToOperatingCenter(hydrantRepository, entity.OperatingCenter, hydrantNumber))
            {
                throw ExceptionHelper.Format<InvalidOperationException>("The generated Hydrant number '{0}' is not unique to the operating center '{1}'", hydrantNumber.FormattedNumber, entity.OperatingCenter);
            }

            entity.HydrantNumber = hydrantNumber.FormattedNumber;
            entity.HydrantSuffix = hydrantNumber.Suffix;
            entity.SAPErrorCode = SAP_ERROR_CODE;

            return entity;
        }

        #endregion

        #region Private Methods

        protected virtual bool HydrantNumberIsUniqueToOperatingCenter(IRepository<Hydrant> HydrantRepository, OperatingCenter operatingCenter, HydrantNumber number)
        {
            return !HydrantRepository.FindByOperatingCenterAndHydrantNumber(operatingCenter, number.FormattedNumber).Any();
        }

        #endregion
    }
}
