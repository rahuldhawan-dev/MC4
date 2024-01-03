using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.ProjectManagement.Controllers;
using MapCallMVC.Areas.ProjectManagement.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing.MSTest.TestExtensions;
using System;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class BidControllerTest : MapCallMvcControllerTestBase<BidController, EstimatingProject>
    {
        #region Init

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeCreateViewModel = (vm) => {
                // This is just a PDF with user input. None of these values are important.
                var model = (CreateBidForm)vm;
                model.ProjectBidTitle = "Some title";
                model.Employee = GetEntityFactory<Employee>().Create(new {
                    PhoneWork = "123",
                    EmailAddress = "some@email.com"
                }).Id;
                model.DueDateTime = DateTime.Now;
            };
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresRole("~/ProjectManagement/Bid/New/", BidController.ROLE);
                a.RequiresRole("~/ProjectManagement/Bid/Create/", BidController.ROLE);
            });
        }

        #region Create

        [TestMethod]
        public override void TestCreateRedirectsToTheRecordsShowPageAfterSuccessfullySaving()
        {
            // noop this returns a pdf.
        }

        [TestMethod]
        public override void TestCreateSavesNewRecordWhenModelStateIsValid()
        {
            // noop nothing is saved in the bd.
        }

        [TestMethod]
        public void TestCreateDoesNotError()
        {
            _authenticationService.SetupGet(x => x.CurrentUser.IsAdmin).Returns(true);

            var project = GetEntityFactory<EstimatingProject>().Create();
            var employee = GetEntityFactory<Employee>().Create();

            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] = ResponseFormatter.KnownExtensions.PDF;

            MyAssert.DoesNotThrow(() => (PdfResult)_target.Create(new CreateBidForm(_container)
            {
                Id = project.Id,
                Employee = employee.Id,
                ProjectBidTitle = "meh"
            }));
        }

        #endregion

        #region New

        [TestMethod]
        public override void TestNewReturnsNewViewWithNewViewModel()
        {
            Assert.Inconclusive("test me");
        }

        #endregion
    }
}
