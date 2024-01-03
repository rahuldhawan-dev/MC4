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
    public class CreateValve : ViewModel<Valve>
    {
        #region Constants

        public const string SAP_ERROR_CODE = "RETRY::INITIAL RECORD CREATED";

        #endregion

        #region Properties

        [Required, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }
        [Required, EntityMap, EntityMustExist(typeof(Town))]
        public int? Town { get; set; }
        [Required, EntityMap, EntityMustExist(typeof(ValveSize))]
        public int? ValveSize { get; set; }
        [Required, EntityMap, EntityMustExist(typeof(Street))]
        public int? Street { get; set; }
        [Required, EntityMap, EntityMustExist(typeof(Street))]
        public int? CrossStreet { get; set; }
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

        public CreateValve(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override Valve MapToEntity(Valve entity)
        {
            entity = base.MapToEntity(entity);

            var billingPublic = _container.GetInstance<IRepository<ValveBilling>>().Find(ValveBilling.Indices.PUBLIC);
            var statusNsiPending = _container.GetInstance<IRepository<AssetStatus>>().Find(AssetStatus.Indices.NSI_PENDING);
            var controlHydrant = _container.GetInstance<IRepository<ValveControl>>().Find(ValveControl.Indices.HYDRANT);

            entity.ValveBilling = billingPublic;
            entity.Status = statusNsiPending;
            entity.ValveControls = controlHydrant;

            entity.Initiator = _container.GetInstance<IAuthenticationService<User>>().CurrentUser;

            var valRepo = _container.GetInstance<IRepository<Valve>>();

            var valNum = valRepo.GenerateNextValveNumber(_container.GetInstance<RepositoryBase<Valve>>(),
                entity.OperatingCenter, entity.Town, entity.TownSection);

            if (!ValveNumberIsUniqueToOperatingCenter(valRepo, entity.OperatingCenter, valNum))
            {
                throw ExceptionHelper.Format<InvalidOperationException>("The generated valve number '{0}' is not unique to the operating center '{1}'", valNum.FormattedNumber, entity.OperatingCenter);
            }

            entity.ValveNumber = valNum.FormattedNumber;
            entity.ValveSuffix = valNum.Suffix;
            entity.SAPErrorCode = SAP_ERROR_CODE;

            return entity;
        }

        #endregion

        #region Private Methods

        protected virtual bool ValveNumberIsUniqueToOperatingCenter(IRepository<Valve> valveRepository, OperatingCenter operatingCenter, ValveNumber number)
        {
            return !valveRepository.FindByOperatingCenterAndValveNumber(operatingCenter, number.FormattedNumber).Any();
        }
        
        #endregion
    }
}
