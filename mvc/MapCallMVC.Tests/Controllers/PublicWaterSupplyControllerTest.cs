using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Configuration;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class PublicWaterSupplyControllerTest : MapCallMvcControllerTestBase<PublicWaterSupplyController, PublicWaterSupply>
    {
        #region Private Members

        private Mock<INotificationService> _notifier;
        private Mock<IRoleService> _roleService;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            base.InitializeObjectFactory(i);
            i.For<IPublicWaterSupplyRepository>().Use<PublicWaterSupplyRepository>();
            _notifier = i.For<INotificationService>().Mock();
        }

        #endregion

        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = PublicWaterSupplyController.ROLE;

            Authorization.Assert(a => {
                a.RequiresRole("~/PublicWaterSupply/Search/", role);
                a.RequiresRole("~/PublicWaterSupply/Show/", role);
                a.RequiresRole("~/PublicWaterSupply/Index/", role);
                a.RequiresRole("~/PublicWaterSupply/New/", role, RoleActions.Add);
                a.RequiresRole("~/PublicWaterSupply/Create/", role, RoleActions.Add);
                a.RequiresRole("~/PublicWaterSupply/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/PublicWaterSupply/Update/", role, RoleActions.Edit);
                a.RequiresLoggedInUserOnly("~/PublicWaterSupply/ByOperatingCenterId/");
                a.RequiresLoggedInUserOnly("~/PublicWaterSupply/ByOperatingCenterOrState/");
                a.RequiresLoggedInUserOnly("~/PublicWaterSupply/ByOperatingCenterIdsAndAWOwned/");
                a.RequiresLoggedInUserOnly("~/PublicWaterSupply/ActiveByOperatingCenterId/");
                a.RequiresLoggedInUserOnly("~/PublicWaterSupply/ActiveOrPendingByOperatingCenterId/");
                a.RequiresLoggedInUserOnly("~/PublicWaterSupply/ByStateId/");
                a.RequiresLoggedInUserOnly("~/PublicWaterSupply/ByPartialPWSIDMatch/");
                a.RequiresLoggedInUserOnly("~/PublicWaterSupply/GetSystemNameByOperatingCenter/");
                a.RequiresLoggedInUserOnly("~/PublicWaterSupply/ActiveByStateIdOrOperatingCenterId/");
            });
        }

        #endregion

        #region Show

        [TestMethod]
        public void TestShowSetsUpAListOfOperatingCentersAsDropDownData()
        {
            var expected = GetFactory<OperatingCenterFactory>().CreateList(3);
            var pws = GetEntityFactory<PublicWaterSupply>().Create();

            _target.Show(pws.Id);

            _target.AssertHasDropDownData(expected, oc => oc.Id, oc => oc.ToString());
        }
        
        [TestMethod]
        public void TestShowSetsUpAListOfPlanningPlantsAsDropDownData()
        {
            var expected = GetFactory<PlanningPlantFactory>().CreateList(3);
            var pws = GetEntityFactory<PublicWaterSupply>().Create();

            _target.Show(pws.Id);

            _target.AssertHasDropDownData(expected, pp => pp.Id, pp => pp.ToString());
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<PublicWaterSupply>().Create(new {Identifier = "description 0"});
            var entity1 = GetEntityFactory<PublicWaterSupply>().Create(new {Identifier = "description 1"});
            var search = new SearchPublicWaterSupply();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.Identifier, "PWSID");
                helper.AreEqual(entity1.Identifier, "PWSID", 1);
            }
        }

        #endregion

        #region Create

        [TestMethod]
        public void TestCreateSendsNotificationPWSIDTicketMade()
        {
            var entity =
                _viewModelFactory.Build<CreatePublicWaterSupply, PublicWaterSupply>(
                    GetEntityFactory<PublicWaterSupply>().Build());
            //act
            var result = _target.Create(entity);
            //assert
            _notifier.Verify(i => i.Notify(It.Is<NotifierArgs>(na => na.Module == PublicWaterSupplyController.ROLE)), Times.Once);
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<PublicWaterSupply>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditPublicWaterSupply, PublicWaterSupply>(
                eq, new {
                    Identifier = expected
                }));

            Assert.AreEqual(expected, Session.Get<PublicWaterSupply>(eq.Id).Identifier);
        }

        [TestMethod]
        public void TestUpdatePublicWaterSupplyWithDifferentStatusSendsNotification()
        {
            var entity = GetEntityFactory<PublicWaterSupply>().Create();
            var model = _viewModelFactory.BuildWithOverrides<EditPublicWaterSupply, PublicWaterSupply>(entity, new {
                Status = GetFactory<PendingPublicWaterSupplyStatusFactory>().Create().Id
            });  
            
            //Act
            var result = _target.Update(model);
           
            //Assert-the 'i' from the top a
            _notifier.Verify(i => i.Notify(It.Is<NotifierArgs>(na => na.Module == PublicWaterSupplyController.ROLE)), Times.Once);
        }

        [TestMethod]
        public void TestUpdatePublicWaterSupplyWithSameStatusDoesNotSendNotification()
        {
            var entity = GetEntityFactory<PublicWaterSupply>().Create();
            var model = _viewModelFactory.BuildWithOverrides<EditPublicWaterSupply, PublicWaterSupply>(entity, new {
                Status = GetFactory<ActivePublicWaterSupplyStatusFactory>().Create().Id
            });

            //Act
            var result = _target.Update(model);

            //Assert-the 'i' from the top 
            _notifier.Verify(i => i.Notify(It.IsAny<NotifierArgs>()), Times.Never);
        }

        #endregion

        #region Cascading Endpoints

        [TestMethod]
        public void TestGetSystemNameByOperatingCenter()
        {
            var opc = GetFactory<OperatingCenterFactory>().Create();
            var pws = GetEntityFactory<PublicWaterSupply>().Create();
            var operatingCenterPublicWaterSupply = GetEntityFactory<OperatingCenterPublicWaterSupply>().Create(new { OperatingCenter = opc, PublicWaterSupply = pws });
            var result = (CascadingActionResult)_target.GetSystemNameByOperatingCenter(opc.Id);
            var data = (IEnumerable<PublicWaterSupplyDisplayItemForNearMiss>)result.Data;
            Assert.AreEqual(operatingCenterPublicWaterSupply.Id, data.Single().Id);
        }

        [TestMethod]
        public void TestActiveOrPendingByOperatingCenterIdForTheGivenOperatingCenterIds()
        {
            var operatingCenter1 = GetFactory<UniqueOperatingCenterFactory>().Create();

            var activePublicWaterSupply = GetEntityFactory<PublicWaterSupply>().Create();
            var pendingPublicWaterSupply = GetEntityFactory<PublicWaterSupply>().Create(new { Status = GetFactory<PendingPublicWaterSupplyStatusFactory>().Create() });
            var pendingMergerPublicWaterSupply = GetEntityFactory<PublicWaterSupply>().Create(new { Status = GetFactory<PendingMergerPublicWaterSupplyStatusFactory>().Create() });
            var inactivePublicWaterSupply = GetEntityFactory<PublicWaterSupply>().Create(new { Status = GetFactory<InactivePublicWaterSupplyStatusFactory>().Create() });

            GetEntityFactory<OperatingCenterPublicWaterSupply>().Create(new { OperatingCenter = operatingCenter1, PublicWaterSupply = activePublicWaterSupply });
            GetEntityFactory<OperatingCenterPublicWaterSupply>().Create(new { OperatingCenter = operatingCenter1, PublicWaterSupply = pendingPublicWaterSupply });
            GetEntityFactory<OperatingCenterPublicWaterSupply>().Create(new { OperatingCenter = operatingCenter1, PublicWaterSupply = pendingMergerPublicWaterSupply });
            GetEntityFactory<OperatingCenterPublicWaterSupply>().Create(new { OperatingCenter = operatingCenter1, PublicWaterSupply = inactivePublicWaterSupply });

            var actionResult = (CascadingActionResult)_target.ActiveOrPendingByOperatingCenterId(operatingCenter1.Id);
            var publicWaterSupplyDisplayItems = ((IEnumerable<PublicWaterSupplyDisplayItem>)actionResult.Data).ToList();

            Assert.AreEqual(3, publicWaterSupplyDisplayItems.Count);
            Assert.AreEqual(1, publicWaterSupplyDisplayItems.Count(x => x.Id == activePublicWaterSupply.Id));
            Assert.AreEqual(1, publicWaterSupplyDisplayItems.Count(x => x.Id == pendingPublicWaterSupply.Id));
            Assert.AreEqual(1, publicWaterSupplyDisplayItems.Count(x => x.Id == pendingMergerPublicWaterSupply.Id));
            Assert.AreEqual(0, publicWaterSupplyDisplayItems.Count(x => x.Id == inactivePublicWaterSupply.Id));
        }

        [TestMethod]
        public void TestActiveOrPendingByOperatingCenterIdWhenNoOperatingCenterIdsAreGiven()
        {
            var operatingCenter1 = GetFactory<UniqueOperatingCenterFactory>().Create();

            var activePublicWaterSupply = GetEntityFactory<PublicWaterSupply>().Create();
            var pendingPublicWaterSupply = GetEntityFactory<PublicWaterSupply>().Create(new { Status = GetFactory<PendingPublicWaterSupplyStatusFactory>().Create() });
            var pendingMergerPublicWaterSupply = GetEntityFactory<PublicWaterSupply>().Create(new { Status = GetFactory<PendingMergerPublicWaterSupplyStatusFactory>().Create() });
            var inactivePublicWaterSupply = GetEntityFactory<PublicWaterSupply>().Create(new { Status = GetFactory<InactivePublicWaterSupplyStatusFactory>().Create() });

            GetEntityFactory<OperatingCenterPublicWaterSupply>().Create(new { OperatingCenter = operatingCenter1, PublicWaterSupply = activePublicWaterSupply });
            GetEntityFactory<OperatingCenterPublicWaterSupply>().Create(new { OperatingCenter = operatingCenter1, PublicWaterSupply = pendingPublicWaterSupply });
            GetEntityFactory<OperatingCenterPublicWaterSupply>().Create(new { OperatingCenter = operatingCenter1, PublicWaterSupply = pendingMergerPublicWaterSupply });
            GetEntityFactory<OperatingCenterPublicWaterSupply>().Create(new { OperatingCenter = operatingCenter1, PublicWaterSupply = inactivePublicWaterSupply });

            var actionResult = (CascadingActionResult)_target.ActiveOrPendingByOperatingCenterId();
            var publicWaterSupplyDisplayItems = ((IEnumerable<PublicWaterSupplyDisplayItem>)actionResult.Data).ToList();

            Assert.AreEqual(3, publicWaterSupplyDisplayItems.Count);
            Assert.AreEqual(1, publicWaterSupplyDisplayItems.Count(x => x.Id == activePublicWaterSupply.Id));
            Assert.AreEqual(1, publicWaterSupplyDisplayItems.Count(x => x.Id == pendingPublicWaterSupply.Id));
            Assert.AreEqual(1, publicWaterSupplyDisplayItems.Count(x => x.Id == pendingMergerPublicWaterSupply.Id));
            Assert.AreEqual(0, publicWaterSupplyDisplayItems.Count(x => x.Id == inactivePublicWaterSupply.Id));
        }

        [TestMethod]
        public void TestActiveByOperatingCenterIdForTheGivenOperatingCenterIds()
        {
            var operatingCenter1 = GetFactory<UniqueOperatingCenterFactory>().Create();

            var activePublicWaterSupply = GetEntityFactory<PublicWaterSupply>().Create();
            var pendingPublicWaterSupply = GetEntityFactory<PublicWaterSupply>().Create(new { Status = GetFactory<PendingPublicWaterSupplyStatusFactory>().Create() });
            var pendingMergerPublicWaterSupply = GetEntityFactory<PublicWaterSupply>().Create(new { Status = GetFactory<PendingMergerPublicWaterSupplyStatusFactory>().Create() });
            var inactivePublicWaterSupply = GetEntityFactory<PublicWaterSupply>().Create(new { Status = GetFactory<InactivePublicWaterSupplyStatusFactory>().Create() });

            GetEntityFactory<OperatingCenterPublicWaterSupply>().Create(new { OperatingCenter = operatingCenter1, PublicWaterSupply = activePublicWaterSupply });
            GetEntityFactory<OperatingCenterPublicWaterSupply>().Create(new { OperatingCenter = operatingCenter1, PublicWaterSupply = pendingPublicWaterSupply });
            GetEntityFactory<OperatingCenterPublicWaterSupply>().Create(new { OperatingCenter = operatingCenter1, PublicWaterSupply = pendingMergerPublicWaterSupply });
            GetEntityFactory<OperatingCenterPublicWaterSupply>().Create(new { OperatingCenter = operatingCenter1, PublicWaterSupply = inactivePublicWaterSupply });

            var actionResult = (CascadingActionResult)_target.ActiveByOperatingCenterId(operatingCenter1.Id);
            var publicWaterSupplyDisplayItems = ((IEnumerable<PublicWaterSupplyDisplayItem>)actionResult.Data).ToList();

            Assert.AreEqual(1, publicWaterSupplyDisplayItems.Count);
            Assert.AreEqual(1, publicWaterSupplyDisplayItems.Count(x => x.Id == activePublicWaterSupply.Id));
            Assert.AreEqual(0, publicWaterSupplyDisplayItems.Count(x => x.Id == pendingPublicWaterSupply.Id));
            Assert.AreEqual(0, publicWaterSupplyDisplayItems.Count(x => x.Id == pendingMergerPublicWaterSupply.Id));
            Assert.AreEqual(0, publicWaterSupplyDisplayItems.Count(x => x.Id == inactivePublicWaterSupply.Id));
        }

        [TestMethod]
        public void TestActiveByOperatingCenterIdWhenNoOperatingCenterIdsAreGiven()
        {
            var operatingCenter1 = GetFactory<UniqueOperatingCenterFactory>().Create();

            var activePublicWaterSupply = GetEntityFactory<PublicWaterSupply>().Create();
            var pendingPublicWaterSupply = GetEntityFactory<PublicWaterSupply>().Create(new { Status = GetFactory<PendingPublicWaterSupplyStatusFactory>().Create() });
            var pendingMergerPublicWaterSupply = GetEntityFactory<PublicWaterSupply>().Create(new { Status = GetFactory<PendingMergerPublicWaterSupplyStatusFactory>().Create() });
            var inactivePublicWaterSupply = GetEntityFactory<PublicWaterSupply>().Create(new { Status = GetFactory<InactivePublicWaterSupplyStatusFactory>().Create() });

            GetEntityFactory<OperatingCenterPublicWaterSupply>().Create(new { OperatingCenter = operatingCenter1, PublicWaterSupply = activePublicWaterSupply });
            GetEntityFactory<OperatingCenterPublicWaterSupply>().Create(new { OperatingCenter = operatingCenter1, PublicWaterSupply = pendingPublicWaterSupply });
            GetEntityFactory<OperatingCenterPublicWaterSupply>().Create(new { OperatingCenter = operatingCenter1, PublicWaterSupply = pendingMergerPublicWaterSupply });
            GetEntityFactory<OperatingCenterPublicWaterSupply>().Create(new { OperatingCenter = operatingCenter1, PublicWaterSupply = inactivePublicWaterSupply });

            var actionResult = (CascadingActionResult)_target.ActiveByOperatingCenterId();
            var publicWaterSupplyDisplayItems = ((IEnumerable<PublicWaterSupplyDisplayItem>)actionResult.Data).ToList();

            Assert.AreEqual(1, publicWaterSupplyDisplayItems.Count);
            Assert.AreEqual(1, publicWaterSupplyDisplayItems.Count(x => x.Id == activePublicWaterSupply.Id));
            Assert.AreEqual(0, publicWaterSupplyDisplayItems.Count(x => x.Id == pendingPublicWaterSupply.Id));
            Assert.AreEqual(0, publicWaterSupplyDisplayItems.Count(x => x.Id == pendingMergerPublicWaterSupply.Id));
            Assert.AreEqual(0, publicWaterSupplyDisplayItems.Count(x => x.Id == inactivePublicWaterSupply.Id));
        }

        [TestMethod]
        public void Test_ByOperatingCenterOrState_FiltersByOperatingCentersIfSent()
        {
            var expected = GetEntityFactory<OperatingCenter>().CreateList();
            var extra = GetEntityFactory<OperatingCenter>().CreateList();
            var all = expected.Concat(extra);

            foreach (var operatingCenter in all)
            {
                GetEntityFactory<OperatingCenterPublicWaterSupply>().Create(new {
                    OperatingCenter = operatingCenter
                });
            }

            var actionResult = (CascadingActionResult)_target
               .ByOperatingCenterOrState(
                    // these will be ignored
                    all.Select(x => x.State.Id).ToArray(),
                    expected.Select(x => x.Id).ToArray());
            var publicWaterSupplyDisplayItems =
                ((IEnumerable<PublicWaterSupplyDisplayItem>)actionResult.Data).ToList();

            foreach (var result in publicWaterSupplyDisplayItems)
            {
                var pws = Session.Load<PublicWaterSupply>(result.Id);
                
                Assert.IsTrue(
                    expected.Contains(pws.OperatingCenterPublicWaterSupplies.Single().OperatingCenter));
            }
        }

        [TestMethod]
        public void Test_ByOperatingCenterOrState_FiltersByStatesIfSent()
        {
            var expected = GetEntityFactory<State>().CreateList();
            var extra = GetEntityFactory<State>().CreateList();
            var all = expected.Concat(extra);

            foreach (var state in all)
            {
                GetEntityFactory<PublicWaterSupply>().Create(new { State = state });
            }

            var actionResult = (CascadingActionResult)_target
               .ByOperatingCenterOrState(expected.Select(x => x.Id).ToArray(), null);
            var publicWaterSupplyDisplayItems =
                ((IEnumerable<PublicWaterSupplyDisplayItem>)actionResult.Data).ToList();

            foreach (var result in publicWaterSupplyDisplayItems)
            {
                var pws = Session.Load<PublicWaterSupply>(result.Id);
                
                Assert.IsTrue(expected.Contains(pws.State));
            }
        }

        [TestMethod]
        public void Test_ByOperatingCenterOrState_ReturnsEmptySet_IfNoArgumentsProvided()
        {
            var extra = GetEntityFactory<OperatingCenter>().CreateList();

            foreach (var operatingCenter in extra)
            {
                GetEntityFactory<OperatingCenterPublicWaterSupply>().Create(new {
                    OperatingCenter = operatingCenter
                });
            }
            
            var actionResult = (CascadingActionResult)_target.ByOperatingCenterOrState(null, null);
            var publicWaterSupplyDisplayItems =
                ((IEnumerable<PublicWaterSupplyDisplayItem>)actionResult.Data).ToList();

            Assert.IsTrue(!publicWaterSupplyDisplayItems.Any());
        }

        [TestMethod]
        public void TestActiveByStateIdOrOperatingCenterId()
        {
            var state1 = GetFactory<StateFactory>().Create();
            var operatingCenter1 = GetFactory<UniqueOperatingCenterFactory>().Create(new {State = state1});

            var activePublicWaterSupply = GetEntityFactory<PublicWaterSupply>().Create( new {State = state1});
            var pendingPublicWaterSupply = GetEntityFactory<PublicWaterSupply>().Create(new { Status = GetFactory<PendingPublicWaterSupplyStatusFactory>().Create(), State = state1 });
            var pendingMergerPublicWaterSupply = GetEntityFactory<PublicWaterSupply>().Create(new { Status = GetFactory<PendingMergerPublicWaterSupplyStatusFactory>().Create(), State = state1 });
            var inactivePublicWaterSupply = GetEntityFactory<PublicWaterSupply>().Create(new { Status = GetFactory<InactivePublicWaterSupplyStatusFactory>().Create(), State = state1 });

            GetEntityFactory<OperatingCenterPublicWaterSupply>().Create(new { OperatingCenter = operatingCenter1, PublicWaterSupply = activePublicWaterSupply });
            GetEntityFactory<OperatingCenterPublicWaterSupply>().Create(new { OperatingCenter = operatingCenter1, PublicWaterSupply = pendingPublicWaterSupply });
            GetEntityFactory<OperatingCenterPublicWaterSupply>().Create(new { OperatingCenter = operatingCenter1, PublicWaterSupply = pendingMergerPublicWaterSupply });
            GetEntityFactory<OperatingCenterPublicWaterSupply>().Create(new { OperatingCenter = operatingCenter1, PublicWaterSupply = inactivePublicWaterSupply });

            /* Without state or operating center */

            var actionResult = (CascadingActionResult)_target.ActiveByStateIdOrOperatingCenterId(null, null);
            var publicWaterSupplyDisplayItems = ((IEnumerable<PublicWaterSupplyDisplayItem>)actionResult.Data).ToList();

            Assert.AreEqual(1, publicWaterSupplyDisplayItems.Count);
            Assert.AreEqual(1, publicWaterSupplyDisplayItems.Count(x => x.Id == activePublicWaterSupply.Id));
            Assert.AreEqual(0, publicWaterSupplyDisplayItems.Count(x => x.Id == pendingPublicWaterSupply.Id));
            Assert.AreEqual(0, publicWaterSupplyDisplayItems.Count(x => x.Id == pendingMergerPublicWaterSupply.Id));
            Assert.AreEqual(0, publicWaterSupplyDisplayItems.Count(x => x.Id == inactivePublicWaterSupply.Id));

            /* With state no operating center */

            var actionResult2 = (CascadingActionResult)_target.ActiveByStateIdOrOperatingCenterId(new[] {state1.Id}, null);
            var publicWaterSupplyDisplayItems2 = ((IEnumerable<PublicWaterSupplyDisplayItem>)actionResult2.Data).ToList();

            Assert.AreEqual(1, publicWaterSupplyDisplayItems2.Count);
            Assert.AreEqual(1, publicWaterSupplyDisplayItems2.Count(x => x.Id == activePublicWaterSupply.Id));
            Assert.AreEqual(0, publicWaterSupplyDisplayItems2.Count(x => x.Id == pendingPublicWaterSupply.Id));
            Assert.AreEqual(0, publicWaterSupplyDisplayItems2.Count(x => x.Id == pendingMergerPublicWaterSupply.Id));
            Assert.AreEqual(0, publicWaterSupplyDisplayItems2.Count(x => x.Id == inactivePublicWaterSupply.Id));

            /* With state and operating center */

            var actionResult3 = (CascadingActionResult)_target.ActiveByStateIdOrOperatingCenterId(new[] { state1.Id }, new[] {operatingCenter1.Id});
            var publicWaterSupplyDisplayItems3 = ((IEnumerable<PublicWaterSupplyDisplayItem>)actionResult3.Data).ToList();

            Assert.AreEqual(1, publicWaterSupplyDisplayItems2.Count);
            Assert.AreEqual(1, publicWaterSupplyDisplayItems2.Count(x => x.Id == activePublicWaterSupply.Id));
            Assert.AreEqual(0, publicWaterSupplyDisplayItems2.Count(x => x.Id == pendingPublicWaterSupply.Id));
            Assert.AreEqual(0, publicWaterSupplyDisplayItems2.Count(x => x.Id == pendingMergerPublicWaterSupply.Id));
            Assert.AreEqual(0, publicWaterSupplyDisplayItems2.Count(x => x.Id == inactivePublicWaterSupply.Id));
        }
        
        #endregion
    }
}
