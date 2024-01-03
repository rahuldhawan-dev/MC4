using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Utilities;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class CreateNpdesRegulatorInspection : NpdesRegulatorInspectionViewModel
    {
        #region Constructors

        public CreateNpdesRegulatorInspection(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override void SetDefaults()
        {
            base.SetDefaults();
            ArrivalDateTime = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
        }

        public override NpdesRegulatorInspection MapToEntity(NpdesRegulatorInspection entity)
        {
            entity = base.MapToEntity(entity);
            entity.DepartureDateTime = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            entity.InspectedBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser;

            return entity;
        }

        #endregion
    }
}
