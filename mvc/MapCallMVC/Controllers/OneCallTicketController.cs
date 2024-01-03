using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC.Controllers;
using MMSINC.Metadata;
using StructureMap;

namespace MapCallMVC.Controllers
{
    public class OneCallTicketController : ControllerBaseWithPersistence<IOneCallTicketRepository, OneCallTicket, User>
    {
        #region ByRequestNumber

        [HttpPost, RequiresSecureForm(false)]
        public ActionResult ByRequestNumber([Required]string requestNumber)
        {
            if (string.IsNullOrWhiteSpace(requestNumber))
            {
                return Json(null);
            }

            requestNumber = requestNumber.Trim();
            var ticket = Repository.FindByRequestNumber(requestNumber);

            if (ticket == null)
            {
                return Json(null);
            }

            var model = new Dictionary<string, object>();
            model.Add("street", ticket.Street);
            model.Add("nearestCrossStreet", ticket.NearestCrossStreet);
            model.Add("excavator", ticket.Excavator);
            model.Add("excavatorAddress", ticket.ExcavatorAddress);
            model.Add("excavatorPhone", ticket.ExcavatorPhone);

            var state = _container.GetInstance<IStateRepository>().FindByAbbreviation(ticket.State.Trim());
            if (state != null)
            {
                model.Add("stateId", state.Id);
                var county = state.Counties.SingleOrDefault(x => x.Name.Equals(ticket.County, StringComparison.InvariantCultureIgnoreCase));

                if (county != null)
                {
                    model.Add("countyId", county.Id);

                    // Apparently we have towns in the database where the ShortName is not set. That's incredibly useful.
                    var town =
                        county.Towns.SingleOrDefault(
                            x => (x.ShortName ?? String.Empty).Equals(ticket.Town, StringComparison.InvariantCultureIgnoreCase));

                    if (town != null)
                    {
                        model.Add("townId", town.Id);
                    }
                }
            }

            return Json(model);
        }

        #endregion

        public OneCallTicketController(ControllerBaseWithPersistenceArguments<IOneCallTicketRepository, OneCallTicket, User> args) : base(args) {}
    }
}