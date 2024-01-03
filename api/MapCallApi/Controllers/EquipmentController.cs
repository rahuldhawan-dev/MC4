using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Migrations;
using MapCall.Common.Model.Repositories;
using MapCallApi.Models;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using Newtonsoft.Json;

namespace MapCallApi.Controllers
{
    public class EquipmentController : ControllerBaseWithPersistence<IEquipmentRepository, Equipment, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.ProductionFacilities;

        #endregion

        #region Constructors

        public EquipmentController(ControllerBaseWithPersistenceArguments<IEquipmentRepository, Equipment, User> args) : base(args) { }

        #endregion
        
        private int? TryGetId(string identifier)
        {
            var matches = Regex.Match(identifier, @"^([a-z]+)(\d+)-(\d+)-([a-z]+)-(\d+)$", RegexOptions.IgnoreCase);
            if (matches.Groups.Count == 6)
            {
                return int.Parse(matches.Groups[5].Value);
            }
            return null;
        }

        public static object EquipmentToAnonymous(Equipment equipment)
        {
            return new {
                equipment.Id,
                equipment.Identifier,
                equipment.Description,
                DateInstalled = equipment.DateInstalled?.ToString(),
                Manufacturer = equipment.EquipmentManufacturer?.ToString(),
                EquipmentModel = equipment.EquipmentModel?.ToString(),
                equipment.SerialNumber,
                Status = equipment.EquipmentStatus?.ToString(),
                EquipmentPurpose = equipment.EquipmentPurpose?.ToString(),
                EquipmentSubCategory = equipment.EquipmentPurpose?.EquipmentSubCategory?.ToString(),
                EquipmentLifespan = equipment.EquipmentPurpose?.EquipmentLifespan?.ToString(),
                EquipmentTypeDescription = equipment.EquipmentType?.Description,
                equipment.FunctionalLocation,
                FacilityArea = equipment.FacilityFacilityArea?.FacilityArea.Description,
                FacilitySubArea = equipment.FacilityFacilityArea?.FacilitySubArea?.Description,
                Links = equipment.Links.Select(x => new {x.Url, LinkType = x.LinkType?.Description}),
                LastUpdated = equipment.UpdatedAt
            };
        }
        
        [HttpGet, RequiresRole(ROLE)]
        [Route("Equipment/Show/{id:int}")]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id, new ActionHelperDoShowArgs<Equipment> {
                // Changes made here should also be made in the FacilityController
                OnSuccess = equipment =>
                    Content(JsonConvert.SerializeObject(EquipmentToAnonymous(equipment)), "application/json")
            });
        }
        
        // Oddly order matters here, if you move this before the other one tests fail.
        [HttpGet, RequiresRole(ROLE)]
        [Route("Equipment/Show/{id}")]
        public ActionResult ShowString(string id)
        {
            var actualId = TryGetId(id);
            if (!actualId.HasValue)
            {
                return new HttpNotFoundResult($"Unable to locate Equipment with the id: {id}");
            }

            return Show(actualId.Value);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchEquipment search)
        {
            search.EnablePaging = false;
            var equipment = _repository.Search(search).Select(x => x.ToJson());
            return Json(equipment, JsonRequestBehavior.AllowGet);
        }
    }
}
