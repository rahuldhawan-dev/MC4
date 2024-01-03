using System.Web.Mvc;
using Contractors.Controllers;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Contractors.Tests.Controllers
{
    [TestClass]
    public class CrewControllerTest : ContractorControllerTestBase<CrewController, Crew>
    {
        #region Setup/Teardown

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateValidEntity = () => GetFactory<CrewFactory>().Create(new { _currentUser.Contractor });
        }

        #endregion

        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresSiteAdminUser("~/Crew/Create");
                a.RequiresSiteAdminUser("~/Crew/Edit");
                a.RequiresSiteAdminUser("~/Crew/Index");
                a.RequiresSiteAdminUser("~/Crew/New");
                a.RequiresSiteAdminUser("~/Crew/Show/1");
                a.RequiresSiteAdminUser("~/Crew/Update");
            });
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var expected = GetFactory<CrewFactory>().Create(new {
                _currentUser.Contractor
            });
            var model = new EditCrew(_container) {
                Availability = 666, Description = "new description", Id = expected.Id
            };

            var result = _target.Update(model);
            expected = Session.Get<Crew>(expected.Id);

            Assert.AreEqual(model.Availability, expected.Availability);
            Assert.AreEqual(model.Description, expected.Description);
        }

        [TestMethod]
        public void TestEdit()
        {
            var expected = GetFactory<CrewFactory>().Create(new {
                _currentUser.Contractor
            });

            var result = (ViewResult)_target.Edit(expected.Id);
            var actual = (EditCrew)result.Model;

            Assert.IsNotNull(result.Model);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Availability, actual.Availability);
            Assert.AreEqual(expected.Description, actual.Description);
        }

        #endregion
    }
}
