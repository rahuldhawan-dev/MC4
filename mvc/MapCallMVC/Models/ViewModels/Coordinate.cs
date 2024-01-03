using MMSINC.Data;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Utilities.ObjectMapping;
using StructureMap;

// TODO: IconId should be getting validated.

namespace MapCallMVC.Models.ViewModels
{
    public class CreateCoordinate : ViewModel<Coordinate>
    {
        #region Properties

        /// <summary>
        /// Prepopulates the address textbox in the view.
        /// </summary>
        [DoesNotAutoMap("Used for the view only.")]
        public virtual string Address { get; set; }
        public virtual decimal Longitude { get; set; }
        public virtual decimal Latitude { get; set; }

        [EntityMap("Icon")]
        public virtual int IconId { get; set; }

        [DoesNotAutoMap("Used by view")]
        public virtual string ValueFor { get; set; }

        [DoesNotAutoMap("Used by view")]
        public virtual int? IconSet { get; set; }

        [DoesNotAutoMap("Used by view")]
        public virtual IconSets CastedIconSet
        {
            get
            {
                return (IconSets)(IconSet ?? 0);
            }
        }

        #endregion

        #region Constructors

        public CreateCoordinate(IContainer container) : base(container) {}

        #endregion

        #region Exposed Methods

        public override void SetDefaults()
        {
            base.SetDefaults();
            var currentUser = _container.GetInstance<IAuthenticationService<User>>().CurrentUser;
            var defaultOperatingCenterCoordinate = currentUser?.DefaultOperatingCenter?.Coordinate;
            if (defaultOperatingCenterCoordinate != null && Latitude == 0)
            {
                Latitude = defaultOperatingCenterCoordinate.Latitude;
                Longitude = defaultOperatingCenterCoordinate.Longitude;
            }
        }

        #endregion
    }
}