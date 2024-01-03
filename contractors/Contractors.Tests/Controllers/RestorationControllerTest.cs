using System.Web.Mvc;
using Contractors.Controllers;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using StructureMap;
using RestorationRepository = Contractors.Data.Models.Repositories.RestorationRepository;

namespace Contractors.Tests.Controllers
{
    [TestClass]
    public class RestorationControllerTest : ContractorControllerTestBase<RestorationController, Restoration, RestorationRepository>
    {
        #region Setup/Teardown

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>().Mock();
            e.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateValidEntity = CreateValidRestoration;
            options.InitializeCreateViewModel = (vm) => {
                var wo = GetFactory<WorkOrderFactory>().Create(new { AssignedContractor = _currentUser.Contractor });
                var model = (CreateRestoration)vm;
                model.WorkOrder = wo.Id;
                model.EstimatedRestorationFootage = 1m;
            };
            options.InitializeSearchTester = (tester) => {
                tester.TestPropertyValues[nameof(SearchRestoration.OperatingCenter)] = GetFactory<UniqueOperatingCenterFactory>().Create().Id;
            };
        }

        private Restoration CreateValidRestoration()
        {
            var workOrder = GetFactory<WorkOrderFactory>().Create(new { AssignedContractor = _currentUser.Contractor });
            return GetFactory<RestorationFactory>().Create(new { WorkOrder = workOrder, AssignedContractor = _currentUser.Contractor });
        }

        #endregion

        #region New/Create

        [TestMethod]
        public override void TestNewReturnsNewViewWithNewViewModel()
        {
            // override due to parameter on New action
            var workorder = GetFactory<WorkOrderFactory>().Create(new {
                AssignedContractor = _currentUser.Contractor 
            });
            var result = (ViewResult)_target.New(workorder.Id);
            MyAssert.IsInstanceOfType<CreateRestoration>(result.Model);
        }

        #endregion

        #region Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var expected = 42;
            var restoration = CreateValidRestoration();
            restoration.OperatingCenter = restoration.WorkOrder.OperatingCenter;

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditRestoration, Restoration>(restoration, new {
                FinalPavingSquareFootage = expected,
            }));

            Assert.AreEqual(expected, Session.Get<Restoration>(restoration.Id).FinalPavingSquareFootage);
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/Restoration/Create");
                a.RequiresLoggedInUserOnly("~/Restoration/New");
                a.RequiresLoggedInUserOnly("~/Restoration/Edit");
                a.RequiresLoggedInUserOnly("~/Restoration/Update");
                a.RequiresLoggedInUserOnly("~/Restoration/Show");
                a.RequiresLoggedInUserOnly("~/Restoration/Index");
                a.RequiresLoggedInUserOnly("~/Restoration/Search");
            });
        }
    }
}
