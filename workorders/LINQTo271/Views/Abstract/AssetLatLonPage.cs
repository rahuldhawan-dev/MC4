using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC.Authentication;
using StructureMap;
using WorkOrders.Library.Controls;
using WorkOrders.Model;
using IOperatingCenterRepository = WorkOrders.Model.IOperatingCenterRepository;
using OperatingCenter = WorkOrders.Model.OperatingCenter;

namespace LINQTo271.Views.Abstract
{
    public abstract class AssetLatLonPage : WorkOrdersMvpPage
    {
        private OperatingCenter _operatingCenter;

        public virtual string MapId
        {
            get
            {
                return (OperatingCenter != null) ? 
                    OperatingCenter.MapId :
                    DependencyResolver.Current.GetService<IGISLayerUpdateRepository>().GetCurrent().MapId;
            }
        }

        public virtual OperatingCenter OperatingCenter
        {
            get
            {
                if (_operatingCenter == null)
                {
                    var authService = DependencyResolver.Current.GetService<IAuthenticationService<User>>();
                    if (authService.CurrentUserIsAuthenticated)
                    {
                        //dafuq?
                        // problem here is where we're getting the operating center from WorkOrders.OperatingCenter or MapCall.Common.Data. This gets one from the other
                        _operatingCenter = DependencyResolver.Current.GetService<IOperatingCenterRepository>().GetAll271OperatingCenters().First(x => x.OperatingCenterID == authService?.CurrentUser?.DefaultOperatingCenter.Id);
                    }
                }
                return _operatingCenter;
            }
            set { _operatingCenter = value; }
        }
    }
}