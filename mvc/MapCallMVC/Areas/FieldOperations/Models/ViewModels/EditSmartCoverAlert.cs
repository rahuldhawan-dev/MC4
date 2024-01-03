using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class EditSmartCoverAlert : ViewModel<SmartCoverAlert>
    {
        #region Constructors

        public EditSmartCoverAlert(IContainer container) : base(container) { }

        #endregion

        #region Properties

        [DoesNotAutoMap]
        public bool Acknowledged { get; set; }

        [DateTimePicker]
        public DateTime? AcknowledgedOn { get; set; }

        [AutoMap(MapDirections.None)]
        public string IndexSearch { get; set; }

        [AutoMap(MapDirections.None)]
        public User User { get; set; }

        #endregion

        #region Public Methods

        public override SmartCoverAlert MapToEntity(SmartCoverAlert entity)
        {
            base.MapToEntity(entity);
            entity.Acknowledged = true;
            entity.AcknowledgedBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser;
            entity.NeedsToSync = true;
            if (!AcknowledgedOn.HasValue)
            {
                entity.AcknowledgedOn = DateTime.Now;
            }
            return entity;
        }

        public override void Map(SmartCoverAlert entity)
        {
            base.Map(entity);
            Acknowledged = true;
            User = _container.GetInstance<IAuthenticationService<User>>().CurrentUser;
        }

        #endregion
    }
}