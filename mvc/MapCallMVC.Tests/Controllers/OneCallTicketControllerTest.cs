using System.Collections.Generic;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class OneCallTicketControllerTest : MapCallMvcControllerTestBase<OneCallTicketController, OneCallTicket, OneCallTicketRepository>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IStateRepository>().Use<StateRepository>();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestByRequestNumberReturnsJsonFormattedTicket()
        {
            var town = GetFactory<TownFactory>().Create();
            var ocTicket = GetEntityFactory<OneCallTicket>().Create(new
            {
                RequestNumber = "000000000",
                State = town.County.State.Abbreviation,
                County = town.County.Name.ToUpper(),
                Town = town.ShortName.ToUpper(),
                Street = "A street",
                NearestCrossStreet = "B street",
                Excavator = "Some guy",
                ExcavatorAddress = "Ugh",
                ExcavatorPhone = "It's a phone"
            });

            var result = (JsonResult)_target.ByRequestNumber("000000000");

            var json = (Dictionary<string, object>)result.Data;
            Assert.AreEqual(town.County.State.Id, json["stateId"]);
            Assert.AreEqual(town.County.Id, json["countyId"]);
            Assert.AreEqual(town.Id, json["townId"]);
            Assert.AreEqual(ocTicket.Street, json["street"]);
            Assert.AreEqual(ocTicket.NearestCrossStreet, json["nearestCrossStreet"]);
            Assert.AreEqual(ocTicket.Excavator, json["excavator"]);
            Assert.AreEqual(ocTicket.ExcavatorAddress, json["excavatorAddress"]);
            Assert.AreEqual(ocTicket.ExcavatorPhone, json["excavatorPhone"]);
        }

        [TestMethod]
        public void TestByRequestNumberDoesNotChokeAndDieBecauseATownHasANullShortNameForSomeReasonThatIDoNotUnderstand()
        {
            var town = GetFactory<TownFactory>().Create();
            town.ShortName = null;
            Session.Save(town);
            var ocTicket = GetEntityFactory<OneCallTicket>().Create(new
            {
                RequestNumber = "000000000",
                State = town.County.State.Abbreviation,
                County = town.County.Name.ToUpper(),
                Town = "WHO CARES",
                Street = "A street",
                NearestCrossStreet = "B street",
                Excavator = "Some guy",
                ExcavatorAddress = "Ugh",
                ExcavatorPhone = "It's a phone"
            });

            var result = (JsonResult)_target.ByRequestNumber("000000000");

            var json = (Dictionary<string, object>)result.Data;
            Assert.AreEqual(town.County.State.Id, json["stateId"]);
            Assert.AreEqual(town.County.Id, json["countyId"]);
            Assert.IsFalse(json.ContainsKey("townId"));
            Assert.AreEqual(ocTicket.Street, json["street"]);
            Assert.AreEqual(ocTicket.NearestCrossStreet, json["nearestCrossStreet"]);
            Assert.AreEqual(ocTicket.Excavator, json["excavator"]);
            Assert.AreEqual(ocTicket.ExcavatorAddress, json["excavatorAddress"]);
            Assert.AreEqual(ocTicket.ExcavatorPhone, json["excavatorPhone"]);
        }
        
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/OneCallTicket/ByRequestNumber/");
            });
        }

        [TestMethod]
        public void TestByRequestNumberDoesNotRequireSecureForm()
        {
            DoesNotRequireSecureForm("~/OneCallTicket/ByRequestNumber/");
        }
        

        #endregion
    }
}
