using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.HydrantPaintings
{
    public abstract class HydrantPaintingViewModel : ViewModel<HydrantPainting>
    {
        #region Abstract Properties

        [EntityMustExist(typeof(Hydrant))]
        public abstract int? Hydrant { get; set; }

        [DateTimePicker]
        public abstract DateTime? PaintedAt { get; set; }

        #endregion

        #region Constructors

        protected HydrantPaintingViewModel(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override HydrantPainting MapToEntity(HydrantPainting entity)
        {
            entity = base.MapToEntity(entity);

            entity.UpdatedAt = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            entity.UpdatedBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser;

            return entity;
        }

        #endregion
    }
}
