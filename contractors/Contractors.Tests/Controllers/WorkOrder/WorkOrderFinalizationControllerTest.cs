using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Contractors.Controllers.WorkOrder;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MMSINC.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing;
using StructureMap;
using IWorkDescriptionRepository = Contractors.Data.Models.Repositories.IWorkDescriptionRepository;
using MapCall.Common.Model.Entities;

namespace Contractors.Tests.Controllers.WorkOrder
{
    [TestClass]
    public class WorkOrderFinalizationControllerTest : ContractorControllerTestBase<WorkOrderFinalizationController, MapCall.Common.Model.Entities.WorkOrder>
    {
        #region Setup/Teardown

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateValidEntity = () => {
                var order = GetFactory<FinalizationWorkOrderFactory>()
                   .Create(new { AssignedContractor = _currentUser.Contractor, DateCompleted = DateTime.Now });
                Session.Flush();
                return order;
            };
            options.InitializeSearchTester = (tester) => {
                // For reasons unknown to me, the default factories are not creating usable values in 
                // the contractors tests. So some of them need to be created with proper values manually.
                tester.TestPropertyValues[nameof(WorkOrderFinalizationSearch.Street)] =
                    GetFactory<StreetFactory>().Create(new { Name = "Street Name" }).Id;
            };
            options.UpdateRedirectsToRouteOnSuccessArgs = (vm) => new { action = "ShowCalendar", controller = "CrewAssignment" };
        }

        #endregion

        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/WorkOrderFinalization/Index");
                a.RequiresLoggedInUserOnly("~/WorkOrderFinalization/Search");
                a.RequiresLoggedInUserOnly("~/WorkOrderFinalization/Edit");
                a.RequiresLoggedInUserOnly("~/WorkOrderFinalization/Update");
            });
        }

        #endregion

        #region Edit

        [TestMethod]
        public void TestEditSetsViewData()
        {
            var order = GetFactory<FinalizationWorkOrderFactory>()
                .Create(new {AssignedContractor = _currentUser.Contractor});
            var expected = _container
                .GetInstance<IWorkDescriptionRepository>()
                .GetByAssetTypeId(order.AssetType.Id).ToArray();
            Session.Flush();
            var result = (ViewResult)_target.Edit(order.Id);
            var ddi = result.ViewData["WorkDescription"];
            var actual = ((IEnumerable<SelectListItem>)ddi).ToArray();

            MyAssert.AreEqual(expected[0].Id.ToString(), actual[0].Value);
        }

        [TestMethod]
        public void TestEditReturns404IfWorkOrderIsInPlanningPhase()
        {
            var order = GetFactory<PlanningWorkOrderFactory>()
                .Create(new { AssignedContractor = _currentUser.Contractor });
            
            MvcAssert.IsStatusCode(404, 
                string.Format(WorkOrderFinalizationController.NO_SUCH_WORK_ORDER, order.Id),
                _target.Edit(order.Id));
        }

        [TestMethod]
        public void TestEditDefaultsMeterLocationToPremiseMeterLocationForServiceAssetType()
        {
            var eq = GetFactory<FinalizationWorkOrderFactory>().Create(new {
                AssignedContractor = _currentUser.Contractor,
                PremiseNumber = "A13243546",
                AssetType = GetFactory<ServiceAssetTypeFactory>().Create(),
                Installation = Convert.ToInt64(9876)
            });
            GetFactory<OpenCrewAssignmentFactory>().Create(new { WorkOrder = eq });
            var meterLocation = GetEntityFactory<MeterLocation>().Create(new { SAPCode = "c1" });
            GetFactory<PremiseFactory>().Create(new { PremiseNumber = "A13243546", MeterLocation = meterLocation, Installation = "9876" });
            GetFactory<PremiseFactory>().Create(new { PremiseNumber = "A13243546" });

            var result = _target.Edit(eq.Id);

            MvcAssert.IsViewNamed(result, "Edit");
            Assert.AreEqual(meterLocation.Id, ((result as ViewResult).Model as WorkOrderFinalizationDetails).WorkOrder.MeterLocation.Id);

            // Test for WorkOrder.Installation = null
            eq = GetFactory<FinalizationWorkOrderFactory>().Create(new {
                AssignedContractor = _currentUser.Contractor,
                PremiseNumber = "B13243546",
                AssetType = GetFactory<ServiceAssetTypeFactory>().Create()
            });
            GetFactory<OpenCrewAssignmentFactory>().Create(new { WorkOrder = eq });
            meterLocation = GetEntityFactory<MeterLocation>().Create(new { SAPCode = "c1" });
            GetFactory<PremiseFactory>().Create(new { PremiseNumber = "B13243546", MeterLocation = meterLocation, Installation = "9876" });
            GetFactory<PremiseFactory>().Create(new { PremiseNumber = "B13243546" });

            result = _target.Edit(eq.Id);

            MvcAssert.IsViewNamed(result, "Edit");
            Assert.AreEqual(meterLocation.Id, ((result as ViewResult).Model as WorkOrderFinalizationDetails).WorkOrder.MeterLocation.Id);
        }

        [TestMethod]
        public void TestEditDoesNotDefaultMeterLocationToPremiseMeterLocationForNonServiceAssetTypes()
        {
            var eq = GetFactory<FinalizationWorkOrderFactory>().Create(new { AssignedContractor = _currentUser.Contractor });
            GetFactory<OpenCrewAssignmentFactory>().Create(new { WorkOrder = eq });
            var meterLocation = GetEntityFactory<MeterLocation>().Create(new { SAPCode = "c1" });
            GetFactory<PremiseFactory>().Create(new { PremiseNumber = "13243546", MeterLocation = meterLocation });

            var result = _target.Edit(eq.Id);

            MvcAssert.IsViewNamed(result, "Edit");
            Assert.IsNull(((result as ViewResult).Model as WorkOrderFinalizationDetails).WorkOrder.MeterLocation);
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexRendersViewWithOrders()
        {
            // Leaving this test rather than overriding the automatic test due to the extra checks.
            var expectedTown = GetFactory<TownFactory>().Create();
            var expected = GetFactory<FinalizationWorkOrderFactory>().CreateArray(2, new {
                AssignedContractor = _currentUser.Contractor, Town = expectedTown
            });

            // extra because of contractor
            GetFactory<FinalizationWorkOrderFactory>().CreateList(2, new {
                AssignedContractor = GetFactory<ContractorFactory>().Create(),
                Town = expectedTown
            });

            // extra because of town
            GetFactory<FinalizationWorkOrderFactory>().CreateList(2, new {
                AssignedContractor = _currentUser.Contractor,
                Town = GetFactory<TownFactory>().Create()
            });
            Session.Flush();
            var search = new WorkOrderFinalizationSearch {
                Town = expectedTown.Id
            };

            var result = (ViewResult)_target.Index(search);
            Assert.AreSame(search, result.Model);
            Assert.AreEqual(expected.Length, search.Results.Count());
            foreach (var order in search.Results)
            {
                Assert.IsTrue(expected.Contains(order));
            }
        }

        #endregion

        #region Search

        [TestMethod]
        public override void TestSearchReturnsSearchViewWithModel()
        {
            // override needed because inherited Search action does not
            // return a model intance.
            var result = (ViewResult)_target.Search();
            MvcAssert.IsViewNamed(result, "Search");
            Assert.IsNull(result.Model);
        }

        #endregion

        #region Update

        [TestMethod]
        public override void TestUpdateReturnsEditViewWithModelWhenThereAreModelStateErrors()
        {
            // override because this redirects to Edit instead. 
            var order = GetFactory<FinalizationWorkOrderFactory>()
                .Create(new {AssignedContractor = _currentUser.Contractor});
            _target.ModelState.AddModelError("nope", "nuh uh");

            var result = (RedirectToRouteResult)_target.Update(_viewModelFactory.Build<WorkOrderFinalizationDetails, MapCall.Common.Model.Entities.WorkOrder>(order));

            Assert.AreEqual("Edit", result.RouteValues["action"]);
            Assert.AreEqual(order.Id, result.RouteValues["id"]);
        }

        [TestMethod]
        public void TestUpdateRedirectsToCrewAssignmentCalendarAfterSuccessfulFinalizationOfOrder()
        {
            var order = GetFactory<FinalizationWorkOrderFactory>()
                .Create(new {AssignedContractor = _currentUser.Contractor});
            var args = _viewModelFactory.Build<WorkOrderFinalizationDetails, MapCall.Common.Model.Entities.WorkOrder>(order);
            args.DateCompleted = DateTime.Today;

            var result = (RedirectToRouteResult)_target.Update(args);

            Assert.IsNotNull(result);
            Assert.AreEqual("ShowCalendar", result.RouteValues["action"]);
            Assert.AreEqual("CrewAssignment", result.RouteValues["controller"]);
        }

        [TestMethod]
        public void TestUpdateAddsMainBreakErrorIfMainBreakHasNotBeenEntered()
        {
            // TODO: This should be a view model test, not a controller test.
            var assetType = GetFactory<MainAssetTypeFactory>().Create();
            var wd = GetFactory<MainBreakRepairWorkDescriptionFactory>().Create();
            var order = GetFactory<FinalizationWorkOrderFactory>()
               .Create(new {
                    AssignedContractor = _currentUser.Contractor,
                    AssetType = assetType,
                    WorkDescription = wd
                });

            var model =
                _viewModelFactory.Build<WorkOrderFinalizationDetails, MapCall.Common.Model.Entities.WorkOrder>(order);
            model.DateCompleted = DateTime.Now;
            _target.RunModelValidation(model);
            var result = (RedirectToRouteResult)_target.Update(model);

            Assert.AreEqual("Edit", result.RouteValues["action"]);
            Assert.AreEqual(order.Id, result.RouteValues["id"]);
            Assert.AreEqual(WorkOrderFinalizationDetails.NO_MAIN_BREAK_INFO,
                _target.ModelState.Values.First().Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void TestUpdateAddsCrewAssignmentErrorIfOpenCrewAssignmentExists()
        {
            // TODO: This should be a view model test, not a controller test.
            var order = GetFactory<FinalizationWorkOrderFactory>()
               .Create(new { AssignedContractor = _currentUser.Contractor, DateCompleted = DateTime.Now });
            var ca = GetFactory<CrewAssignmentFactory>().Create(
                new { DateStarted = DateTime.Today, WorkOrder = order });

            var model =
                _viewModelFactory.Build<WorkOrderFinalizationDetails, MapCall.Common.Model.Entities.WorkOrder>(order);
            _target.RunModelValidation(model);
            var result = (RedirectToRouteResult)_target.Update(model);

            Assert.AreEqual("Edit", result.RouteValues["action"]);
            Assert.AreEqual(order.Id, result.RouteValues["id"]);
            Assert.AreEqual(WorkOrderFinalizationDetails.OPEN_CREW_ASSIGNMENTS,
                _target.ModelState.Values.First().Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void TestUpdateUpdatesMeterLocationInServiceInstallationForServiceAssetType()
        {
            var eq = GetFactory<FinalizationWorkOrderFactory>().Create(new {
                DigitalAsBuiltRequired = true,
                AssetType = GetFactory<ServiceAssetTypeFactory>().Create(),
                AssignedContractor = _currentUser.Contractor
            });

            var i = GetEntityFactory<ServiceInstallation>().Create(new {
                WorkOrder = eq
            });

            Session.Refresh(eq);
            GetEntityFactory<MeterLocation>().Create(new { SAPCode = "c0" });
            var meterLocation = GetEntityFactory<MeterLocation>().Create(new { SAPCode = "c1" });
            GetEntityFactory<MeterSupplementalLocation>().Create();
            var now = DateTime.Now;

            var model = new WorkOrderFinalizationDetails(_container) {
                Id = eq.Id,
                DateCompleted = now,
                MeterLocation = meterLocation.Id
            };

            _target.Update(model);

            var installation = Session.Query<ServiceInstallation>().FirstOrDefault(x => x.WorkOrder.Id == eq.Id);
            Assert.AreEqual(meterLocation.Id, installation?.MeterLocation.Id);
        }

        #endregion
    }
}
