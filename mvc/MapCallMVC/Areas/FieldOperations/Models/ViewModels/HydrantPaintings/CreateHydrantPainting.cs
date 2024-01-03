using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.HydrantPaintings
{
    public class CreateHydrantPainting : HydrantPaintingViewModel
    {
        #region Properties

        [Required, EntityMap]
        public override int? Hydrant { get; set; }

        [RequiredWhen(nameof(PaintedToday), false)]
        public override DateTime? PaintedAt { get; set; }

        [DoesNotAutoMap]
        public bool PaintedToday { get; set; }

        #endregion

        #region Constructors

        public CreateHydrantPainting(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override HydrantPainting MapToEntity(HydrantPainting entity)
        {
            entity = base.MapToEntity(entity);

            entity.CreatedAt = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            entity.CreatedBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser;

            if (PaintedToday)
            {
                entity.PaintedAt = entity.CreatedAt;
            }

            return entity;
        }

        #endregion
    }
}
