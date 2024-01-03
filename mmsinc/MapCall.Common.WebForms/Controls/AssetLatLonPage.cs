using System.Web.Mvc;
using System.Web.UI;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC.Authentication;
using StructureMap;

namespace MapCall.Common.Controls
{
    public abstract class AssetLatLonPage : Page
    {
        private OperatingCenter _operatingCenter;
        private string _defaultLatitude, _defaultLongitude;

        public string DefaultLatitude
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_defaultLatitude))
                {
                    if (OperatingCenter != null && OperatingCenter.Coordinate != null)
                        _defaultLatitude = OperatingCenter.Coordinate.Latitude.ToString();
                }

                return _defaultLatitude;
            }
            set { _defaultLatitude = value; }
        }

        public string DefaultLongitude
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_defaultLongitude))
                {
                    if (OperatingCenter != null && OperatingCenter.Coordinate != null)
                        _defaultLongitude = OperatingCenter.Coordinate.Longitude.ToString();
                }

                return _defaultLongitude;
            }
            set { _defaultLongitude = value; }
        }

        public string MapId
        {
            get
            {
                return (OperatingCenter != null)
                    ? OperatingCenter.MapId
                    : DependencyResolver.Current.GetService<IGISLayerUpdateRepository>().GetCurrent().MapId;
            }
        }

        public OperatingCenter OperatingCenter
        {
            get
            {
                if (_operatingCenter == null)
                {
                    var authService = DependencyResolver.Current.GetService<IAuthenticationService<User>>();
                    _operatingCenter = authService?.CurrentUser?.DefaultOperatingCenter;
                }

                return _operatingCenter;
            }
            set { _operatingCenter = value; }
        }
    }
}
