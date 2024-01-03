using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using MapCall.Common.Controllers;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Models.ViewModels;
using MMSINC;
using MMSINC.ClassExtensions;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Results;

namespace MapCallMVC.Controllers
{
    public class CustomerLocationController : ControllerBaseWithPersistence<ICustomerLocationRepository, CustomerLocation, User>
    {
        #region Constants

        public const int PAGE_SIZE = 7;

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            if (action == ControllerAction.Search)
            {
                this.AddDropDownData<ICustomerLocationRepository, CustomerLocation>("State", r => r.GetDistinctStates(),
                    cl => cl.State, cl => cl.State);
            }
        }
        
        [HttpGet, RequiresRole(RoleModules.FieldServicesImages)]
        public ActionResult CitiesByState(string state)
        {
            return new CascadingActionResult(Repository.GetDistinctCitiesByState(state), "City", "City");
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(RoleModules.FieldServicesImages)]
        public ActionResult Search(SearchCustomerLocation search)
        {
            SetLookupData(ControllerAction.Search);

            return this.RespondTo(f => {
                f.View(() =>
                       View(search));
                f.Fragment(() =>
                           View("_Search", search));
            });
        }

        [HttpGet, NoCache, RequiresRole(RoleModules.FieldServicesImages)]
        public ActionResult Index(SearchCustomerLocation search)
        {
            search.PageSize = PAGE_SIZE;
            
            return this.RespondTo(f => {
                f.View(() => {
                    // TODO: Shouldn't this use f.Map() and MapResult instead?
                    var urlHelper = new UrlHelper(Request.RequestContext);
                    ViewData[MapController.MAP_CONFIGURATION] = new {
                        iconDir = urlHelper.Content("~/Content/Images/"),
                        icons =
                            MapController.SerializeIcons(_container.GetInstance<IRepository<MapIcon>>(), urlHelper)
                    };

                    return ActionHelper.DoIndex(search, new MMSINC.Utilities.ActionHelperDoIndexArgs
                    {
                        SearchOverrideCallback = () => Repository.SearchWithHasVerifiedCoordinate(search)
                    });
                });
                f.Excel(() => {
                    var results = Repository
                        .SearchWithHasVerifiedCoordinate(search)
                        .Select(x => new {
                            x.Id,
                            x.PremiseNumber,
                            x.Address,
                            x.Latitude,
                            x.Longitude
                        });
                    return this.Excel(results);
                });
                f.Fragment(() => ActionHelper.DoIndex(search, new MMSINC.Utilities.ActionHelperDoIndexArgs
                {
                    ViewName = "_Index",
                    // What is this ext needed for? -Ross 12/17/2019
                    RouteValues = new
                    {
                        ext = ResponseFormatter.KnownExtensions.FRAGMENT
                    }
                }));
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(RoleModules.FieldServicesImages)] // Why does this not use the Edit RoleAction?
        public ActionResult Edit(int id)
        {
            return this.RespondTo(f => {
                f.View(() => ActionHelper.DoEdit<UpdateCustomerLocation>(id));
                f.Fragment(() => ActionHelper.DoEdit(id, new MMSINC.Utilities.ActionHelperDoEditArgs<CustomerLocation, UpdateCustomerLocation>{
                    // Is there a reason we're not setting IsPartial = true here? The views for this seem odd.
                    ViewName = "_Edit"
                }));
            });
        }

        [HttpPost, RequiresRole(RoleModules.FieldServicesImages)] // Why does this not use the Edit RoleAction?
        public ActionResult Update(UpdateCustomerLocation location)
        {
            var original = Repository.Find(location.Id);
            if (original == null)
            {
                return HttpNotFound();
            }
            var coordinate = original.HasVerifiedCoordinate
                                 ? original.CustomerCoordinates.Where(cc => cc.Verified).Single()
                                 : new CustomerCoordinate {
                                     CustomerLocation = Repository.Find(location.Id),
                                     Verified = true,
                                     Source = (int)CoordinateSource.MapCall
                                 };
            coordinate.Latitude = location.Latitude;
            coordinate.Longitude = location.Longitude;

            _container.GetInstance<ICustomerCoordinateRepository>().Save(coordinate);

            //TODO: This is a hack. 
            var routeValues = (location.ReturnUrl != null)
                                  ? location.ReturnUrl.ToRouteValues()
                                  : new RouteValueDictionary();
            routeValues.Add("ext", "frag");
            return RedirectToAction("Index", routeValues);
        }

        #endregion

        public CustomerLocationController(ControllerBaseWithPersistenceArguments<ICustomerLocationRepository, CustomerLocation, User> args) : base(args) {}
    }
}