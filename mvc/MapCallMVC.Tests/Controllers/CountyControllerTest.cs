using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;
using StructureMap;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class CountyControllerTest : MapCallMvcControllerTestBase<CountyController, County>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<ICountyRepository>().Use<CountyRepository>();
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a =>
            {
                a.RequiresLoggedInUserOnly("~/County/ByStateId/");
            });
        }

        [TestMethod]
        public void TestByStateIdReturnsCascadingActionResult()
        {
            var stateValid = _container.GetInstance<StateFactory>().Create(new{ Abbreviation = "NJ"});
            var stateInvalid = _container.GetInstance<StateFactory>().Create(new{Abbreviation = "QQ"});
            var countyValid = _container.GetInstance<CountyFactory>().Create(new {State = stateValid});
            var countyInvalid = _container.GetInstance<CountyFactory>().Create(new {State = stateInvalid});

            var results = (CascadingActionResult)_target.ByStateId(stateValid.Id);
            var data = results.GetSelectListItems().ToArray();

            Assert.AreEqual(2, data.Count());
            Assert.AreEqual(countyValid.Name, data.Last().Text);
            Assert.AreEqual(countyValid.Id.ToString(), data.Last().Value);
        }
    }
}
