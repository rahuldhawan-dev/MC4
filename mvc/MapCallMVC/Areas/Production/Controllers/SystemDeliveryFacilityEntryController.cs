using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using Fasterflect;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Production.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.ClassExtensions.BooleanExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Helpers;
using MMSINC.Metadata;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.Production.Controllers
{
    [DisplayName("System Delivery / System Flows Entries")]
    public class SystemDeliveryFacilityEntryController : ControllerBaseWithPersistence<IRepository<SystemDeliveryFacilityEntry>, SystemDeliveryFacilityEntry, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.ProductionSystemDeliveryEntry;

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search(SearchSystemDeliveryFacilityEntry search)
        {
            return RedirectToAction("Search", "SystemDeliveryEntry");
        }
        
        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchSystemDeliveryFacilityEntry search)
        {
            search.EnablePaging = false;
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => {
                     search.EnablePaging = false;
                    var results = Repository
                                 .Search(search)
                                 .Select(x => new {
                                      x.SystemDeliveryEntry.Id,
                                      Date = x.EntryDate,
                                      x.Facility.OperatingCenter,
                                      x.SystemDeliveryType,
                                      Facility = x.Facility.FacilityIdWithFacilityName,
                                      LegacyIdSd = x.Facility.RegionalPlanningArea,
                                      x.Facility.PublicWaterSupply,
                                      x.Facility.FacilitySystemDeliveryEntryTypes.FirstOrDefault(f => f.SystemDeliveryEntryType.Id == x.SystemDeliveryEntryType.Id)?.BusinessUnit,
                                      PurchaseSupplier = x.PurchaseSupplierName,
                                      x.SystemDeliveryEntryType,
                                      Adjustment = x.HasBeenAdjusted.ToString("yn"),
                                      OriginalEntry = x.EntryValue,
                                      Value = x.EntryValue,
                                      IsValidated = x.SystemDeliveryEntry.IsValidatedNotNull.ToString("yn"),
                                      IsInjection = x.IsInjection.ToString("yn"),
                                      Employee = x.EnteredBy,
                                      Comment = x.AdjustmentComment,
                                      IsHyperionFileCreated = x.SystemDeliveryEntry.IsHyperionFileCreated.ToString("yn")
                                 }).ToList();
                                return this.Excel(results);
                    });
                });
        }

        #endregion

        public SystemDeliveryFacilityEntryController(ControllerBaseWithPersistenceArguments<IRepository<SystemDeliveryFacilityEntry>, SystemDeliveryFacilityEntry, User> args) : base(args) { }
    }
}
