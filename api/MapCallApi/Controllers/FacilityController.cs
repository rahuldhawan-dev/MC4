using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallApi.Models;
using MMSINC.Controllers;
using MMSINC.Utilities;
using Newtonsoft.Json;

namespace MapCallApi.Controllers
{
    public class FacilityController : ControllerBaseWithPersistence<IFacilityRepository, Facility, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.ProductionFacilities;
        public const int MAX_JSON_LENGTH = 20971520;

        #endregion

        #region Constructors

        public FacilityController(ControllerBaseWithPersistenceArguments<IFacilityRepository, Facility, User> args) :
            base(args) { }

        #endregion

        private int? TryGetId(string identifier)
        {
            var matches = Regex.Match(identifier, @"^([a-z]+)(\d+)-(\d+)$", RegexOptions.IgnoreCase);
            if (matches.Groups.Count == 4)
            {
                return int.Parse(matches.Groups[3].Value);
            }

            return null;
        }

        [HttpGet, RequiresRole(ROLE)]
        [Route("Facility/Show/{id:int}", Name = "Show")]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id, new ActionHelperDoShowArgs<Facility> {
                GetEntityOverride = () => Repository.FindWithEagerJoin(id),
                OnSuccess = facility =>
                    Content(JsonConvert.SerializeObject(new {
                        facility.Id,
                        facility.FacilityId,
                        facility.FacilityName,
                        PublicWaterSupply = facility.PublicWaterSupply?.ToString(),
                        Location = facility.Address,
                        facility.FacilityTotalCapacityMGD,
                        facility.PublicWaterSupply?.CustomerData?.NumberCustomers,
                        facility.PublicWaterSupply?.CustomerData?.PopulationServed,
                        facility.FacilityContactInfo,
                        FacilityProcesses = facility.FacilityProcesses.OrderBy(x => x.Process.Sequence)
                                                    .Select(x => new {
                                                         x.Id,
                                                         x.Process.Sequence,
                                                         ProcessStage = x.Process.ProcessStage.Description,
                                                         Process = x.Process.ToString()
                                                     }),
                        FacilityAreas = facility.FacilityAreas.Select(x => new {
                            x.Id, FacilityArea = x.FacilityArea.Description,
                            FacilitySubArea = x.FacilitySubArea?.Description
                        }),
                        //Changes made to equipment here should also be made in EquipmentController
                        Equipment = facility.Equipment.Select(EquipmentController.EquipmentToAnonymous),
                        Process = facility.Process?.Description,
                        LastUpdated = facility.UpdatedAt
                    }), "application/json")
            });
        }

        // NOTE: The method name has to be different or else errors get thrown about finding duplicate
        // actions. The route itself still knows to call this method, though.
        [HttpGet, RequiresRole(ROLE)]
        [Route("Facility/Show/{id}", Name = "ShowString")]
        public ActionResult ShowString(string id)
        {
            var actualId = TryGetId(id);
            if (!actualId.HasValue)
            {
                return new HttpNotFoundResult($"Unable to locate facility with the id: {id}");
            }

            return Show(actualId.Value);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchFacility search)
        {
            search.EnablePaging = false;
            var query = Repository.Search(search)
                                  .Where(x => (x.FacilityStatus?.Id == FacilityStatus.Indices.ACTIVE ||
                                               x.FacilityStatus?.Id == FacilityStatus.Indices.PENDING) &&
                                              x.FacilityOwner?.Id == FacilityOwner.Indices.AMERICAN_WATER)
                                  .Select(x => x.FacilityToJson());

            return new JsonResult {
                MaxJsonLength = MAX_JSON_LENGTH,
                Data = query,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}
