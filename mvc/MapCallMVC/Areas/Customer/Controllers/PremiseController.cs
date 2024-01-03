using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using Antlr.Runtime.Misc;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Configuration;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Customer.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using Microsoft.Ajax.Utilities;
using MMSINC;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;

namespace MapCallMVC.Areas.Customer.Controllers
{
    //Users of Work Management need to be able to view these
    //Not limited by OperatingCenter
    public class PremiseController : ControllerBaseWithPersistence<IPremiseRepository, Premise, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesWorkManagement;

        public const string SAMPLE_SITE_WARNING =
            "This Premise Record is Linked to a Service with a Sample Site. Contact WQ before making any changes.";

        #endregion

        #region Private Members

        private ActionResult GetMapResult(SearchPremiseForMap search)
        {
            if (Repository.GetCountForSearchSet(search) > SearchPremiseForMap.MAX_MAP_RESULT_COUNT)
            {
                return null;
            }
            
            return _container
                  .GetInstance<IMapResultFactory>()
                  .Build(ModelState, () => Repository.SearchForMap(search));
        }
        
        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchPremise search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return this.RespondTo(f => {
                f.View(() => {
                    Action<Premise> onModelFound = (entity) => {
                        if (entity.Services.Any(s => s.Premise.SampleSites.Any()))
                        {
                            DisplayErrorMessage(SAMPLE_SITE_WARNING);
                        }
                    };

                    return ActionHelper.DoShow(id,onModelFound: onModelFound);
                });
                f.Fragment(() => ActionHelper.DoShow(id, new MMSINC.Utilities.ActionHelperDoShowArgs {
                    ViewName = "_ShowPopup",
                    IsPartial = true
                }));
            });
        }

        /// <summary>
        /// This is typically loaded in a modal.
        /// </summary>
        /// <param name="operatingCenterId"></param>
        /// <param name="partialView">
        /// This will default to returning a Partial. Set to false for testing purposes otherwise
        /// </param>
        [RequiresRole(ROLE, RoleActions.Read), HttpGet]
        [OutputCache(Location = OutputCacheLocation.None, Duration = 0, NoStore = true)]
        public ActionResult Find(int? operatingCenterId, bool partialView = true)
        {
            this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE, RoleActions.Edit);
            if (operatingCenterId.HasValue)
            {
                var regionCodes =
                    Repository.Where(x => x.RegionCode != null &&
                                          x.OperatingCenter.Id == operatingCenterId.Value)
                              .Select(x => x.RegionCode)
                              .Distinct()
                              .ToList();
                if (regionCodes.Any())
                {
                    this.AddDropDownData<RegionCode>(
                        "RegionCode",
                        x => regionCodes.OrderBy(rc => rc.Description).AsQueryable(),
                        x => x.Id,
                        x => x.Description);
                }
            }
            else
            {
                this.AddDropDownData<RegionCode>();
            }
            this.AddDropDownData<ServiceUtilityType>();
            return partialView ? PartialView("_Find") : (ActionResult)View("_Find");
        }
        
        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchPremise search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
                formatter.Map(() => GetMapResult(search));
                formatter.Json(() => {
                    search.EnablePaging = true;
                    search.PageSize = 100;
                    var results = Repository.Search(search);
                    return Json(new {
                            // "results" here is an IQueryable that still has a query that's open to be
                            // built, any new filter/sorting/selection calls (.Where, .OrderBy, .Select) or
                            // things along those lines alter the query which will eventually be passed to
                            // SQL (rather than applying the filter/sort/selection in-memory on a list after
                            // the query has returned).  In order to achieve this, the Func<> passed to
                            // .Select must be a System.Expression<Func<>>, which has certain restrictions
                            // on what you can do (no function calls, and the newer c# features like
                            // null-safe or coalesce operators are forbidden).
                            Data = results.Select(p => new {
                                p.Id,
                                p.PremiseNumber,
                                p.FullStreetAddress,
                                FullStreetNumber = !string.IsNullOrWhiteSpace(p.FullStreetNumber)
                                    ? p.FullStreetNumber
                                    : string.Empty,
                                ApartmentNumber = !string.IsNullOrWhiteSpace(p.ServiceAddressApartment)
                                    ? p.ServiceAddressApartment
                                    : string.Empty,
                                ConnectionObject = !string.IsNullOrWhiteSpace(p.ConnectionObject)
                                    ? p.ConnectionObject
                                    : string.Empty,
                                DeviceLocation = !string.IsNullOrWhiteSpace(p.DeviceLocation)
                                    ? p.DeviceLocation
                                    : string.Empty,
                                InstallationType = (p.ServiceUtilityType != null)
                                    ? p.ServiceUtilityType.ToString()
                                    : string.Empty,
                                Equipment = !string.IsNullOrWhiteSpace(p.Equipment)
                                    ? p.Equipment.TrimStart('0')
                                    : string.Empty,
                                RegionCode = (p.RegionCode != null)
                                    ? p.RegionCode.Description
                                    : string.Empty,
                                DeviceSerialNumber = !string.IsNullOrWhiteSpace(p.DeviceSerialNumber)
                                    ? p.DeviceSerialNumber.TrimStart('0')
                                    : string.Empty,
                                MeterSerialNumber = !string.IsNullOrWhiteSpace(p.MeterSerialNumber)
                                    ? p.MeterSerialNumber.TrimStart('0')
                                    : string.Empty,
                                Latitude = (p.Coordinate != null)
                                    ? p.Coordinate.Latitude.ToString()
                                    : string.Empty,
                                Longitude = (p.Coordinate != null)
                                    ? p.Coordinate.Longitude.ToString()
                                    : string.Empty,
                                Installation = !string.IsNullOrWhiteSpace(p.Installation)
                                    ? p.Installation
                                    : string.Empty,
                                ZipCode = !string.IsNullOrWhiteSpace(p.ServiceZip)
                                    ? p.ServiceZip
                                    : string.Empty,
                                CoordinateId = (p.Coordinate != null)
                                    ? p.Coordinate.Id.ToString()
                                    : string.Empty,
                                StreetNumber = (!string.IsNullOrWhiteSpace(p.ServiceAddressHouseNumber))
                                    ? p.ServiceAddressHouseNumber
                                    : string.Empty,
                                Street = !string.IsNullOrWhiteSpace(p.ServiceAddressStreet)
                                    ? p.ServiceAddressStreet
                                    : string.Empty,
                                MeterLocationFreeText = !string.IsNullOrWhiteSpace(p.MeterLocationFreeText)
                                    ? p.MeterLocationFreeText
                                    : string.Empty,
                                IsMajorAccount = p.IsMajorAccount.ToString(),
                                ServiceCity = (p.ServiceCity != null)
                                    ? p.ServiceCity.ToString()
                                    : string.Empty
                            })
                        },
                        JsonRequestBehavior.AllowGet);
                });
            });
        }

        [HttpGet]
        public ActionResult RegionCodesByOperatingCenterId(int operatingCenterId)
        {
            var regionCodes =
                Repository.Where(x => x.OperatingCenter.Id == operatingCenterId &&
                                      x.RegionCode != null)
                          .Select(x => new RegionCode {
                               Id = x.RegionCode.Id,
                               Description = x.RegionCode.Description
                           })
                          .OrderBy(x => x.Description)
                          .Distinct()
                          .ToList();
            return new CascadingActionResult(regionCodes, "Description", "Id");
        }

        #endregion

        #region Constructors

        public PremiseController(
            ControllerBaseWithPersistenceArguments<IPremiseRepository, Premise, User> args)
            : base(args) {}
        
        #endregion
    }
}
