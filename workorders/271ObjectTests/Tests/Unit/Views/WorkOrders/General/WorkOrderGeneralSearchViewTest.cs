using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using LINQTo271.Common;
using LINQTo271.Controls.WorkOrders;
using LINQTo271.Views.WorkOrders.General;
using MMSINC.Controls;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using WorkOrders.Model;
using _271ObjectTests.Tests.Unit.Model;

namespace _271ObjectTests.Tests.Unit.Views.WorkOrders.General
{
    /// <summary>
    /// Summary description for WorkOrderGeneralSearchViewTest.
    /// </summary>
    [TestClass]
    public class WorkOrderGeneralSearchViewTest : EventFiringTestClass
    {
        #region Private Members

        private IListBox lstDescriptionOfWork, lstDocumentType, lstDrivenBy;

        private IDropDownList ddlAssetType,
            ddlPriority,
            ddlRequestedBy,
            ddlMarkoutRequirement,
            ddlSOPRequirement,
            ddlCreatedBy,
            ddlContractor,
            ddlIsAssignedToContractor,
            ddlRequiresInvoice,
            ddlDateType,
            ddlLastCrewAssigned,
            ddlCompleted,
            ddlCancelled,
            ddlHasInvoice,
            ddlAcousticMonitoringType,
            ddlStreetOpeningPermitRequested,
            ddlStreetOpeningPermitIssued;
        private IBaseWorkOrderSearch baseSearch;

        private ITextBox txtAssetID,
            txtSAPNotificationNumber,
            txtSAPWorkOrderNumber,
            txtWBSCharged;
        private IDateRange drDateToSearch;
        private TestWorkOrderGeneralSearchView _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
                .DynamicMock(out baseSearch)
                .DynamicMock(out txtAssetID)
                .DynamicMock(out ddlAssetType)
                .DynamicMock(out ddlDateType)
                .DynamicMock(out lstDescriptionOfWork)
                .DynamicMock(out lstDocumentType)
                .DynamicMock(out drDateToSearch)
                .DynamicMock(out lstDrivenBy)
                .DynamicMock(out ddlPriority)
                .DynamicMock(out ddlRequestedBy)
                .DynamicMock(out ddlMarkoutRequirement)
                .DynamicMock(out ddlSOPRequirement)
                .DynamicMock(out ddlCreatedBy)
                .DynamicMock(out ddlContractor)
                .DynamicMock(out ddlIsAssignedToContractor)
                .DynamicMock(out ddlRequiresInvoice)
                .DynamicMock(out ddlHasInvoice)
                .DynamicMock(out ddlLastCrewAssigned)
                .DynamicMock(out ddlCompleted)
                .DynamicMock(out ddlCancelled)
                .DynamicMock(out txtSAPNotificationNumber)
                .DynamicMock(out txtSAPWorkOrderNumber)
                .DynamicMock(out txtWBSCharged)
                .DynamicMock(out ddlAcousticMonitoringType)
                .DynamicMock(out ddlStreetOpeningPermitRequested)
                .DynamicMock(out ddlStreetOpeningPermitIssued);

            _target = new TestWorkOrderGeneralSearchViewBuilder()
                .WithBaseSearchControl(baseSearch)
                .WithTXTAssetID(txtAssetID)
                .WithDDLAssetType(ddlAssetType)
                .WithLSTDescriptionOfWork(lstDescriptionOfWork)
                .WithLSTDocumentType(lstDocumentType)
                .WithDRDateReceived(drDateToSearch)
                .WithDDLDateType(ddlDateType)
                .WithLSTDrivenBy(lstDrivenBy)
                .WithDDLPriority(ddlPriority)
                .WithDDLRequestedBy(ddlRequestedBy)
                .WithDDLMarkoutRequirement(ddlMarkoutRequirement)
                .WithDDLSOPRequirement(ddlSOPRequirement)
                .WithDDLCreatedBy(ddlCreatedBy)
                .WithDDLContractor(ddlContractor)
                .WithDDLIsAssignedToContractor(ddlIsAssignedToContractor)
                .WithDDLLastCrewAssigned(ddlLastCrewAssigned)
                .WithDDLCompleted(ddlCompleted)
                .WithDDLCancelled(ddlCancelled)
                .WithTXTSAPNotificationNumber(txtSAPNotificationNumber)
                .WithTXTSAPWorkOrderNumber(txtSAPWorkOrderNumber)
                .WithDDLRequiresInvoice(ddlRequiresInvoice)
                .WithDDLHasInvoice(ddlHasInvoice)
                .WithTXTWBSCharged(txtWBSCharged)
                .WithDDLAcousticMonitoringType(ddlAcousticMonitoringType)
                .WithDDLStreetOpeningPermitRequested(ddlStreetOpeningPermitRequested)
                .WithDDLStreetOpeningPermitIssued(ddlStreetOpeningPermitIssued);
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        #region Property Tests

        [TestMethod]
        public void TestPhasePropertyDenotesGeneral()
        {
            Assert.AreEqual(WorkOrderPhase.General, _target.Phase);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestAssetIDPropertyReturnsValueOfTXTAssetID()
        {
            var expected = "123";

            using (_mocks.Record())
            {
                SetupResult.For(txtAssetID.Text).Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.AssetID);
            }
        }

        [TestMethod]
        public void TestAssetTypeIDPropertyReturnsValueOfDDLAssetType()
        {
            var expected = 123;

            using (_mocks.Record())
            {
                SetupResult.For(ddlAssetType.GetSelectedValue()).Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.AssetTypeID);
            }
        }

        [TestMethod]
        public void TestDescriptionOfWorkIDsPropertyReturnsSelectedValuesOfLSTDescriptionOfWork()
        {
            var expected = new List<int> {
                1, 2, 3
            };

            using (_mocks.Record())
            {
                SetupResult.For(lstDescriptionOfWork.GetSelectedValues())
                    .Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.DescriptionOfWorkIDs);
            }
        }

        [TestMethod]
        public void TestDateReceivedReturnsValueOfDRDateReceived()
        {
            var expected = new DateTime(2000, 1, 1);

            using (_mocks.Record())
            {
                SetupResult.For(drDateToSearch.Date).Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.DateToSearch);
            }
        }

        [TestMethod]
        public void TestDateReceivedStartReturnsValueOfDRDateReceivedStart()
        {
            var expected = new DateTime(2000, 1, 1);

            using (_mocks.Record())
            {
                SetupResult.For(drDateToSearch.StartDate).Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.DateToSearchStart);
            }
        }

        [TestMethod]
        public void TestDateReceivedEndReturnsValueOfDRDateReceivedEnd()
        {
            var expected = new DateTime(2000, 1, 1);

            using (_mocks.Record())
            {
                SetupResult.For(drDateToSearch.EndDate).Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.DateToSearchEnd);
            }
        }

        [TestMethod]
        public void TestContractorIDPropertyReturnsValueOfDdlContractor()
        {
            var expected = 919;

            using (_mocks.Record())
            {
                SetupResult.For(ddlContractor.GetSelectedValue()).Return(
                    expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.ContractorID);
            }
        }

        [TestMethod]
        public void TestIsAssignedToContractorReturnsValuesDdlIsAssignedToContractor()
        {
            var expected = false;

            using (_mocks.Record())
            {
                SetupResult.For(ddlIsAssignedToContractor.GetBooleanValue()).
                    Return(expected);
            }
            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.IsAssignedToContractor);
            }
        }

        [TestMethod]
        public void TestSAPNotificationNumberReturnsValueOfTXTSapNotificationNumber()
        {
            var expected = 999999999;

            using (_mocks.Record())
            {
                SetupResult.For(txtSAPNotificationNumber.Text).Return(expected.ToString());
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.SAPNotificationNumber);
            }
        }

        [TestMethod]
        public void TestStreetOpeningPermitRequestedAndStreetOpeningPermitIssuedPropertyReturnsBooleanValue()
        {
            var expected = true;

            using (_mocks.Record())
            {
                SetupResult.For(ddlStreetOpeningPermitRequested.GetBooleanValue()).
                            Return(expected);
                SetupResult.For(ddlStreetOpeningPermitIssued.GetBooleanValue()).
                            Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.StreetOpeningPermitRequested);
                Assert.AreEqual(expected, _target.StreetOpeningPermitIssued);
            }
        }

        #endregion

        #region Search Expression Tests
        [TestMethod]
        public void TestDateToSearchFiltersAgainstDateCompletedWhenChosen()
        {
            Func<WorkOrder, bool> expr;
            WorkOrder today = new TestWorkOrderBuilder()
                .WithDateCompleted(DateTime.Today);
            WorkOrder yesterday = new TestWorkOrderBuilder()
                .WithDateCompleted(DateTime.Today.AddDays(-1));
            WorkOrder tomorrow = new TestWorkOrderBuilder()
                .WithDateCompleted(DateTime.Today.AddDays(1));
            
            // =
            using (_mocks.Record())
            {
                SetupResult.For(ddlDateType.SelectedValue).Return("DateCompleted");
                SetupResult.For(drDateToSearch.SelectedOperator).Return("=");
                SetupResult.For(drDateToSearch.Date).Return(DateTime.Today);
            }
            using (_mocks.Playback())
            {
                expr = _target.GenerateExpression().Compile();

                Assert.IsTrue(expr(today));
                Assert.IsFalse(expr(yesterday));
                Assert.IsFalse(expr(tomorrow));
            }

            _mocks.VerifyAll();
            _mocks.BackToRecordAll();

            // >=
            using (_mocks.Record())
            {
                SetupResult.For(ddlDateType.SelectedValue).Return("DateCompleted");
                SetupResult.For(drDateToSearch.SelectedOperator).Return(">=");
                SetupResult.For(drDateToSearch.Date).Return(DateTime.Today);
            }
            using (_mocks.Playback())
            {
                expr = _target.GenerateExpression().Compile();

                Assert.IsTrue(expr(today));
                Assert.IsFalse(expr(yesterday));
                Assert.IsTrue(expr(tomorrow));
            }
            
            _mocks.VerifyAll();
            _mocks.BackToRecordAll();

            // >
            using (_mocks.Record())
            {
                SetupResult.For(ddlDateType.SelectedValue).Return("DateCompleted");
                SetupResult.For(drDateToSearch.SelectedOperator).Return(">");
                SetupResult.For(drDateToSearch.Date).Return(DateTime.Today);
            }
            using (_mocks.Playback())
            {
                expr = _target.GenerateExpression().Compile();

                Assert.IsFalse(expr(today));
                Assert.IsFalse(expr(yesterday));
                Assert.IsTrue(expr(tomorrow));
            }

            _mocks.VerifyAll();
            _mocks.BackToRecordAll();

            // <=
            using (_mocks.Record())
            {
                SetupResult.For(ddlDateType.SelectedValue).Return("DateCompleted");
                SetupResult.For(drDateToSearch.SelectedOperator).Return("<=");
                SetupResult.For(drDateToSearch.Date).Return(DateTime.Today);
            }
            using (_mocks.Playback())
            {
                expr = _target.GenerateExpression().Compile();

                Assert.IsTrue(expr(today));
                Assert.IsFalse(expr(tomorrow));
                Assert.IsTrue(expr(yesterday));
            }

            _mocks.VerifyAll();
            _mocks.BackToRecordAll();
            
            // <
            using (_mocks.Record())
            {
                SetupResult.For(ddlDateType.SelectedValue).Return("DateCompleted");
                SetupResult.For(drDateToSearch.SelectedOperator).Return("<");
                SetupResult.For(drDateToSearch.Date).Return(DateTime.Today);
            }
            using (_mocks.Playback())
            {
                expr = _target.GenerateExpression().Compile();

                Assert.IsFalse(expr(today));
                Assert.IsTrue(expr(yesterday));
                Assert.IsFalse(expr(tomorrow));
            }

            _mocks.VerifyAll();
            _mocks.BackToRecordAll();

            //between
            using (_mocks.Record())
            {
                SetupResult.For(ddlDateType.SelectedValue).Return("DateCompleted");
                SetupResult.For(drDateToSearch.StartDate).Return(DateTime.Today.AddDays(-1));
                SetupResult.For(drDateToSearch.EndDate).Return(DateTime.Today);
            }
            using (_mocks.Playback())
            {
                expr = _target.GenerateExpression().Compile();

                Assert.IsTrue(expr(yesterday));
                Assert.IsTrue(expr(today));
                Assert.IsFalse(expr(tomorrow));
            }

        }

        [TestMethod]
        public void TestDateToSearchFiltersAgainstDateReceivedWhenChosen()
        {
            Func<WorkOrder, bool> expr;
            WorkOrder today = new TestWorkOrderBuilder()
                .WithDateReceived(DateTime.Today);
            WorkOrder yesterday = new TestWorkOrderBuilder()
                .WithDateReceived(DateTime.Today.AddDays(-1));
            WorkOrder tomorrow = new TestWorkOrderBuilder()
                .WithDateReceived(DateTime.Today.AddDays(1));

            // =
            using (_mocks.Record())
            {
                SetupResult.For(ddlDateType.SelectedValue).Return("DateReceived");
                SetupResult.For(drDateToSearch.SelectedOperator).Return("=");
                SetupResult.For(drDateToSearch.Date).Return(DateTime.Today);
            }
            using (_mocks.Playback())
            {
                expr = _target.GenerateExpression().Compile();

                Assert.IsTrue(expr(today));
                Assert.IsFalse(expr(yesterday));
                Assert.IsFalse(expr(tomorrow));
            }

            _mocks.VerifyAll();
            _mocks.BackToRecordAll();

            // >=
            using (_mocks.Record())
            {
                SetupResult.For(ddlDateType.SelectedValue).Return("DateReceived");
                SetupResult.For(drDateToSearch.SelectedOperator).Return(">=");
                SetupResult.For(drDateToSearch.Date).Return(DateTime.Today);
            }
            using (_mocks.Playback())
            {
                expr = _target.GenerateExpression().Compile();

                Assert.IsTrue(expr(today));
                Assert.IsFalse(expr(yesterday));
                Assert.IsTrue(expr(tomorrow));
            }

            _mocks.VerifyAll();
            _mocks.BackToRecordAll();

            // >
            using (_mocks.Record())
            {
                SetupResult.For(ddlDateType.SelectedValue).Return("DateReceived");
                SetupResult.For(drDateToSearch.SelectedOperator).Return(">");
                SetupResult.For(drDateToSearch.Date).Return(DateTime.Today);
            }
            using (_mocks.Playback())
            {
                expr = _target.GenerateExpression().Compile();

                Assert.IsFalse(expr(today));
                Assert.IsFalse(expr(yesterday));
                Assert.IsTrue(expr(tomorrow));
            }

            _mocks.VerifyAll();
            _mocks.BackToRecordAll();

            // <=
            using (_mocks.Record())
            {
                SetupResult.For(ddlDateType.SelectedValue).Return("DateReceived");
                SetupResult.For(drDateToSearch.SelectedOperator).Return("<=");
                SetupResult.For(drDateToSearch.Date).Return(DateTime.Today);
            }
            using (_mocks.Playback())
            {
                expr = _target.GenerateExpression().Compile();

                Assert.IsTrue(expr(today));
                Assert.IsFalse(expr(tomorrow));
                Assert.IsTrue(expr(yesterday));
            }

            _mocks.VerifyAll();
            _mocks.BackToRecordAll();

            // <
            using (_mocks.Record())
            {
                SetupResult.For(ddlDateType.SelectedValue).Return("DateReceived");
                SetupResult.For(drDateToSearch.SelectedOperator).Return("<");
                SetupResult.For(drDateToSearch.Date).Return(DateTime.Today);
            }
            using (_mocks.Playback())
            {
                expr = _target.GenerateExpression().Compile();

                Assert.IsFalse(expr(today));
                Assert.IsTrue(expr(yesterday));
                Assert.IsFalse(expr(tomorrow));
            }

            _mocks.VerifyAll();
            _mocks.BackToRecordAll();

            //between
            using (_mocks.Record())
            {
                SetupResult.For(ddlDateType.SelectedValue).Return("DateReceived");
                SetupResult.For(drDateToSearch.StartDate).Return(DateTime.Today.AddDays(-1));
                SetupResult.For(drDateToSearch.EndDate).Return(DateTime.Today);
            }
            using (_mocks.Playback())
            {
                expr = _target.GenerateExpression().Compile();

                Assert.IsTrue(expr(yesterday));
                Assert.IsTrue(expr(today));
                Assert.IsFalse(expr(tomorrow));
            }

        }

        [TestMethod]
        public void TestGeneratedExpressionFiltersByAssetIDWhenAssetIDValueEntered()
        {
            Func<WorkOrder, bool> expr;
            AssetType valveType = new TestAssetTypeBuilder<Valve>(),
                      hydrantType = new TestAssetTypeBuilder<Hydrant>(),
                      mainType = new TestAssetTypeBuilder().WithTypeName("Main"),
                      serviceType =
                          new TestAssetTypeBuilder().WithTypeName("Service"),
                      sewerOpeningType =
                          new TestAssetTypeBuilder<SewerOpening>(),
                      stormCatchType = 
                        new TestAssetTypeBuilder<StormCatch>(),
                      sewerLateralType =
                          new TestAssetTypeBuilder().WithTypeName(
                              "SewerLateral"),
                      sewerMainType =
                          new TestAssetTypeBuilder().WithTypeName("SewerMain"),
                      equipmentPurpose = 
                          new TestAssetTypeBuilder<Equipment>().WithTypeName("Equipment");

            const int equipmentID = 1234, invalidEquipmentID = 4321;
            const string valveID = "123",
                         hydrantID = "456",
                         premiseNum = "12345678",
                         sewerOpeningID = "321",
                         sewerLateralNum = "12345678",
                         stormCatchNumber = "9123",
                         invalidValveID = "124",
                         invalidHydrantID = "455",
                         invalidPremiseNum = "12345677",
                         invalidSewerOpeningID = "654",
                         invalidSewerLateralNum = "87654321",
                         invalidStormCatchNumber = "99999";

            WorkOrder assetIsValve = new TestWorkOrderBuilder()
                          .WithAssetType(valveType)
                          .WithValve(new TestValveBuilder().WithAssetID(valveID)),
                      assetIsInvalidValve = new TestWorkOrderBuilder()
                          .WithAssetType(valveType)
                          .WithValve(new TestValveBuilder().WithAssetID(invalidValveID)),
                      assetIsHydrant = new TestWorkOrderBuilder()
                          .WithAssetType(hydrantType)
                          .WithHydrant(
                          new TestHydrantBuilder().WithAssetID(hydrantID)),
                      assetIsInvalidHydrant = new TestWorkOrderBuilder()
                          .WithAssetType(hydrantType)
                          .WithHydrant(
                          new TestHydrantBuilder().WithAssetID(invalidHydrantID)),
                      assetIsSewerOpening = new TestWorkOrderBuilder()
                          .WithAssetType(sewerOpeningType)
                          .WithSewerOpening(new TestSewerOpeningBuilder().WithAssetID(sewerOpeningID)),
                      assetIsInvalidSewerOpening = new TestWorkOrderBuilder()
                          .WithAssetType(sewerOpeningType)
                          .WithSewerOpening(new TestSewerOpeningBuilder().WithAssetID(invalidSewerOpeningID)), 
                      assetIsStormCatch = new TestWorkOrderBuilder()
                          .WithAssetType(stormCatchType)
                          .WithStormCatch(new TestStormCatchBuilder().WithAssetNumber(stormCatchNumber)),
                      assetIsInvalidStormCatch = new TestWorkOrderBuilder()
                          .WithAssetType(stormCatchType)
                          .WithStormCatch(new TestStormCatchBuilder().WithAssetID(invalidStormCatchNumber)),
                      assetIsEquipment = new TestWorkOrderBuilder()
                          .WithAssetType(equipmentPurpose)
                          .WithEquipment(new TestEquipmentBuilder().WithEquipmentID(equipmentID)),
                      assetIsInvalidEquipment = new TestWorkOrderBuilder()
                        .WithAssetType(equipmentPurpose)
                        .WithEquipment(new TestEquipmentBuilder().WithEquipmentID(invalidEquipmentID)),
                      assetIsMain = new TestWorkOrderBuilder()
                          .WithAssetType(mainType),
                      assetIsSewerMain = new TestWorkOrderBuilder()
                          .WithAssetType(sewerMainType),
                      assetIsService = new TestWorkOrderBuilder()
                          .WithAssetType(serviceType)
                          .WithPremiseNumber(premiseNum),
                      assetIsInvalidService = new TestWorkOrderBuilder()
                          .WithAssetType(serviceType)
                          .WithPremiseNumber(invalidPremiseNum),
                      assetIsSewerLateral = new TestWorkOrderBuilder()
                          .WithAssetType(sewerLateralType)
                          .WithPremiseNumber(sewerLateralNum),
                      assetIsInvalidSewerLateral = new TestWorkOrderBuilder()
                          .WithAssetType(sewerLateralType)
                          .WithPremiseNumber(invalidSewerLateralNum);

            using (_mocks.Record())
            {
                SetupResult.For(txtAssetID.Text).Return(valveID);
                SetupResult.For(ddlAssetType.GetSelectedValue()).Return(valveType.AssetTypeID);
                SetupResult.For(ddlDateType.SelectedValue).Return(null);
                SetupResult.For(lstDocumentType.GetSelectedValues()).Return(null);
                SetupResult.For(lstDrivenBy.GetSelectedValues()).Return(null);
            }

            using (_mocks.Playback())
            {
                expr = _target.GenerateExpression().Compile();

                Assert.IsTrue(expr(assetIsValve));
                Assert.IsFalse(expr(assetIsInvalidValve));
                Assert.IsFalse(expr(assetIsHydrant));
                Assert.IsFalse(expr(assetIsMain));
                Assert.IsFalse(expr(assetIsService));
                Assert.IsFalse(expr(assetIsSewerOpening));
                Assert.IsFalse(expr(assetIsSewerMain));
                Assert.IsFalse(expr(assetIsSewerLateral));
                Assert.IsFalse(expr(assetIsStormCatch));
                Assert.IsFalse(expr(assetIsEquipment));
            }

            _mocks.VerifyAll();
            _mocks.BackToRecordAll();

            using (_mocks.Record())
            {
                SetupResult.For(txtAssetID.Text).Return(hydrantID);
                SetupResult.For(ddlAssetType.GetSelectedValue()).Return(hydrantType.AssetTypeID);
            }

            using (_mocks.Playback())
            {
                expr = _target.GenerateExpression().Compile();

                Assert.IsFalse(expr(assetIsValve));
                Assert.IsTrue(expr(assetIsHydrant));
                Assert.IsFalse(expr(assetIsInvalidHydrant));
                Assert.IsFalse(expr(assetIsMain));
                Assert.IsFalse(expr(assetIsService));
                Assert.IsFalse(expr(assetIsSewerOpening));
                Assert.IsFalse(expr(assetIsSewerMain));
                Assert.IsFalse(expr(assetIsSewerLateral));
                Assert.IsFalse(expr(assetIsStormCatch));
                Assert.IsFalse(expr(assetIsEquipment));
            }

            _mocks.VerifyAll();
            _mocks.BackToRecordAll();

            using (_mocks.Record())
            {
                SetupResult.For(txtAssetID.Text).Return(sewerOpeningID);
                SetupResult.For(ddlAssetType.GetSelectedValue()).Return(
                    sewerOpeningType.AssetTypeID);
            }
            using (_mocks.Playback())
            {
                expr = _target.GenerateExpression().Compile();

                Assert.IsFalse(expr(assetIsValve));
                Assert.IsFalse(expr(assetIsHydrant));
                Assert.IsFalse(expr(assetIsInvalidHydrant));
                Assert.IsFalse(expr(assetIsMain));
                Assert.IsFalse(expr(assetIsService));
                Assert.IsTrue(expr(assetIsSewerOpening));
                Assert.IsFalse(expr(assetIsInvalidSewerOpening));
                Assert.IsFalse(expr(assetIsSewerMain));
                Assert.IsFalse(expr(assetIsSewerLateral));
                Assert.IsFalse(expr(assetIsStormCatch));
                Assert.IsFalse(expr(assetIsEquipment));
            }

            _mocks.VerifyAll();
            _mocks.BackToRecordAll();

            using (_mocks.Record())
            {
                SetupResult.For(txtAssetID.Text).Return(premiseNum);
                SetupResult.For(ddlAssetType.GetSelectedValue()).Return(serviceType.AssetTypeID);
            }

            using (_mocks.Playback())
            {
                expr = _target.GenerateExpression().Compile();

                Assert.IsFalse(expr(assetIsValve));
                Assert.IsFalse(expr(assetIsHydrant));
                Assert.IsFalse(expr(assetIsMain));
                Assert.IsTrue(expr(assetIsService));
                Assert.IsFalse(expr(assetIsInvalidService));
                Assert.IsFalse(expr(assetIsSewerOpening));
                Assert.IsFalse(expr(assetIsSewerMain));
                Assert.IsFalse(expr(assetIsSewerLateral));
                Assert.IsFalse(expr(assetIsStormCatch));
                Assert.IsFalse(expr(assetIsEquipment));
            }

            _mocks.VerifyAll();
            _mocks.BackToRecordAll();

            using (_mocks.Record())
            {
                SetupResult.For(txtAssetID.Text).Return(sewerLateralNum);
                SetupResult.For(ddlAssetType.GetSelectedValue()).Return(sewerLateralType.AssetTypeID);
            }

            using (_mocks.Playback())
            {
                expr = _target.GenerateExpression().Compile();

                Assert.IsFalse(expr(assetIsValve));
                Assert.IsFalse(expr(assetIsHydrant));
                Assert.IsFalse(expr(assetIsMain));
                Assert.IsFalse(expr(assetIsService));
                Assert.IsFalse(expr(assetIsSewerOpening));
                Assert.IsFalse(expr(assetIsSewerMain));
                Assert.IsTrue(expr(assetIsSewerLateral));
                Assert.IsFalse(expr(assetIsInvalidSewerLateral));
                Assert.IsFalse(expr(assetIsStormCatch));
                Assert.IsFalse(expr(assetIsEquipment));
            }

            _mocks.VerifyAll();
            _mocks.BackToRecordAll();

            using (_mocks.Record())
            {
                SetupResult.For(txtAssetID.Text).Return(equipmentID.ToString());
                SetupResult.For(ddlAssetType.GetSelectedValue()).Return(equipmentPurpose.AssetTypeID);
            }

            using (_mocks.Playback())
            {
                expr = _target.GenerateExpression().Compile();

                Assert.IsFalse(expr(assetIsValve));
                Assert.IsFalse(expr(assetIsHydrant));
                Assert.IsFalse(expr(assetIsMain));
                Assert.IsFalse(expr(assetIsService));
                Assert.IsFalse(expr(assetIsSewerOpening));
                Assert.IsFalse(expr(assetIsSewerMain));
                Assert.IsFalse(expr(assetIsSewerLateral));
                Assert.IsFalse(expr(assetIsInvalidSewerLateral));
                Assert.IsFalse(expr(assetIsStormCatch));
                Assert.IsTrue(expr(assetIsEquipment));
            }

            _mocks.VerifyAll();
            _mocks.BackToRecordAll();

            using (_mocks.Record())
            {
                SetupResult.For(txtAssetID.Text).Return(stormCatchNumber);
                SetupResult.For(ddlAssetType.GetSelectedValue()).Return(stormCatchType.AssetTypeID);
            }

            using (_mocks.Playback())
            {
                expr = _target.GenerateExpression().Compile();

                Assert.IsFalse(expr(assetIsValve));
                Assert.IsFalse(expr(assetIsHydrant));
                Assert.IsFalse(expr(assetIsMain));
                Assert.IsFalse(expr(assetIsService));
                Assert.IsFalse(expr(assetIsSewerOpening));
                Assert.IsFalse(expr(assetIsSewerMain));
                Assert.IsFalse(expr(assetIsSewerLateral));
                Assert.IsFalse(expr(assetIsInvalidSewerLateral));
                Assert.IsTrue(expr(assetIsStormCatch));
                Assert.IsFalse(expr(assetIsInvalidStormCatch));
                Assert.IsFalse(expr(assetIsEquipment));
            }
        }

        [TestMethod]
        public void TestGeneratedExpressionFiltersByContractorIDWhenContractorIDValueEntered()
        {
            Func<WorkOrder, bool> expr;
            const int contractorID = 2;
            WorkOrder woContractor = new TestWorkOrderBuilder(),
                      woNoContractor = new TestWorkOrderBuilder();
            woContractor.AssignedContractorID = contractorID;

            using (_mocks.Record())
            {
                SetupResult.For(ddlContractor.GetSelectedValue()).Return(contractorID);
            }

            using (_mocks.Playback())
            {
                expr = _target.GenerateExpression().Compile();

                Assert.IsTrue(expr(woContractor));
                Assert.IsFalse(expr(woNoContractor));
            }
        }

        [TestMethod]
        public void TestGeneratedExpressionFiltersIsAssignedToContractorWhenIsAssignedToContractorIsTrue()
        {
            Func<WorkOrder, bool> expr;
            const int contractorID = 2;
            WorkOrder woContractor = new TestWorkOrderBuilder(),
                      woNoContractor = new TestWorkOrderBuilder();
            woContractor.AssignedContractorID = contractorID;

            using (_mocks.Record())
            {
                SetupResult.For(ddlIsAssignedToContractor.GetBooleanValue()).Return(true);
            }

            using (_mocks.Playback())
            {
                expr = _target.GenerateExpression().Compile();

                Assert.IsTrue(expr(woContractor));
                Assert.IsFalse(expr(woNoContractor));
            }
        }

        [TestMethod]
        public void TestGeneratedExpressionFiltersIsAssignedToContractorWhenIsAssignedToContractorIsFalse()
        {
            Func<WorkOrder, bool> expr;
            const int contractorID = 2;
            WorkOrder woContractor = new TestWorkOrderBuilder(),
                      woNoContractor = new TestWorkOrderBuilder();
            woContractor.AssignedContractorID = contractorID;

            using (_mocks.Record())
            {
                SetupResult.For(ddlIsAssignedToContractor.GetBooleanValue()).Return(false);
            }

            using (_mocks.Playback())
            {
                expr = _target.GenerateExpression().Compile();

                Assert.IsFalse(expr(woContractor));
                Assert.IsTrue(expr(woNoContractor));
            }
        }

        [TestMethod]
        public void TestGeneratedExpressionFiltersRequiresInvoiceWhenRequiresInvoiceIsSelected()
        {
            Func<WorkOrder, bool> expr;
            WorkOrder woRequiresInvoice = new TestWorkOrderBuilder();
            woRequiresInvoice.RequiresInvoice = true;
            WorkOrder woDoesNotRequireInvoice = new TestWorkOrderBuilder();
            woDoesNotRequireInvoice.RequiresInvoice = false;

            using (_mocks.Record())
            {
                SetupResult.For(ddlRequiresInvoice.GetBooleanValue())
                    .Return(true);
            }

            using (_mocks.Playback())
            {
                expr = _target.GenerateExpression().Compile();

                Assert.IsTrue(expr(woRequiresInvoice));
                Assert.IsFalse(expr(woDoesNotRequireInvoice));
            }
        }

        [TestMethod]
        public void TestGeneratedExpressionFiltersRequiresInvoiceWhenRequiresInvoiceIsSelectedForFalse()
        {
            Func<WorkOrder, bool> expr;
            WorkOrder woRequiresInvoice = new TestWorkOrderBuilder();
            woRequiresInvoice.RequiresInvoice = true;
            WorkOrder woDoesNotRequireInvoice = new TestWorkOrderBuilder();
            woDoesNotRequireInvoice.RequiresInvoice = false;


            using (_mocks.Record())
            {
                SetupResult.For(ddlRequiresInvoice.GetBooleanValue()).Return(false);
            }

            using (_mocks.Playback())
            {
                expr = _target.GenerateExpression().Compile();

                Assert.IsFalse(expr(woRequiresInvoice));
                Assert.IsTrue(expr(woDoesNotRequireInvoice));
            }
        }

        [TestMethod]
        public void TestBaseExpressionIncludesSAPWorkOrdersWithOutSAPWorkOrderNumbers()
        {
            _mocks.ReplayAll();
            var operatingCenter = new OperatingCenter { SAPEnabled = true, SAPWorkOrdersEnabled = true };
            WorkOrder woInvalidSapOrder = new TestWorkOrderBuilder().BuildIncompleteOrder()
                .WithOperatingCenter(operatingCenter);

            IEnumerable<WorkOrder> orders = new[] { woInvalidSapOrder };

            var result = orders.Where(_target.BaseExpression.Compile()).ToList();

            Assert.IsTrue(result.Contains(woInvalidSapOrder));
        }

        [TestMethod]
        public void TestSearchFiltersAgainstStreetOpeningPermitRequestedChosenAndStreetOpeningPermitIssuedNotChosen()
        {
            WorkOrder wo = new TestWorkOrderBuilder().Build();
            wo.StreetOpeningPermits.Add(new StreetOpeningPermit {
                DateIssued = DateTime.Now,
                DateRequested = DateTime.Now.AddDays(1)
            });

            WorkOrder wo2 = new TestWorkOrderBuilder().Build();

            using (_mocks.Record())
            {
                SetupResult.For(ddlStreetOpeningPermitRequested.GetBooleanValue()).Return(true);
                SetupResult.For(ddlStreetOpeningPermitIssued.GetBooleanValue()).Return(null);
            }

            using (_mocks.Playback())
            {
                var expr = _target.GenerateExpression().Compile();
                Assert.IsTrue(expr(wo));
                Assert.IsFalse(expr(wo2));
            }
        }

        [TestMethod]
        public void TestSearchFiltersAgainstStreetOpeningPermitRequestedAndStreetOpeningPermitIssuedNotChosen()
        {
            WorkOrder wo = new TestWorkOrderBuilder().Build();
            wo.StreetOpeningPermits.Add(new StreetOpeningPermit {
                DateIssued = DateTime.Now,
                DateRequested = DateTime.Now.AddDays(1)
            });

            WorkOrder wo2 = new TestWorkOrderBuilder().Build();

            using (_mocks.Record())
            {
                SetupResult.For(ddlStreetOpeningPermitRequested.GetBooleanValue()).Return(null);
                SetupResult.For(ddlStreetOpeningPermitIssued.GetBooleanValue()).Return(null);
            }

            using (_mocks.Playback())
            {
                var expr = _target.GenerateExpression().Compile();
                Assert.IsTrue(expr(wo));
                Assert.IsTrue(expr(wo2));
            }
        }

        #endregion
    }

    internal class TestWorkOrderGeneralSearchViewBuilder : TestDataBuilder<TestWorkOrderGeneralSearchView>
    {
        #region Private Members

        private IListBox _lstDescriptionOfWork, _lstDrivenBy;

        private IDropDownList _ddlAssetType,
            _ddlDateType,
            _ddlPriority,
            _ddlRequestedBy,
            _ddlMarkoutRequirement,
            _ddlSOPRequirement,
            _ddlCreatedBy,
            _ddlContractor,
            _ddlIsAssignedToContractor,
            _ddlLastCrewAssigned,
            _ddlCompleted,
            _ddlCancelled,
            _ddlRequiresInvoice,
            _ddlHasInvoice,_ddlAcousticMonitoringType,
            _ddlStreetOpeningPermitRequested, _ddlStreetOpeningPermitIssued;

        private ITextBox _txtAssetID,
            _txtSapNotificationNumber,
            _txtSapWorkOrderNumber,
            _txtWBSCharged;
        private IBaseWorkOrderSearch _baseSearch;
        private IDateRange _drDateReceived;
        private Label _lblError;
        private bool _postBack;
        private IListBox _lstDocumentType;

        #endregion

        #region Exposed Methods

        public override TestWorkOrderGeneralSearchView Build()
        {
            var obj = new TestWorkOrderGeneralSearchView();
            if (_baseSearch != null)
                obj.SetBaseSearch(_baseSearch);
            if (_txtAssetID != null)
                obj.SetTXTAssetID(_txtAssetID);
            if (_ddlAssetType != null)
                obj.SetDDLAssetType(_ddlAssetType);
            if (_ddlDateType != null)
                obj.SetDDLDateType(_ddlDateType);
            if (_lstDrivenBy != null)
                obj.SetLSTDrivenBy(_lstDrivenBy);
            if (_ddlPriority != null)
                obj.SetDDLPriority(_ddlPriority);
            if (_lstDescriptionOfWork != null)
                obj.SetLSTDescriptionOfWork(_lstDescriptionOfWork);
            if (_lstDocumentType != null)
                obj.SetLSTDocumentType(_lstDocumentType);
            if (_drDateReceived != null)
                obj.SetDRDateReceived(_drDateReceived);
            if (_ddlRequestedBy != null)
                obj.SetDDLRequestedBy(_ddlRequestedBy);
            if (_ddlMarkoutRequirement != null)
                obj.SetDDLMarkoutRequirement(_ddlMarkoutRequirement);
            if (_ddlSOPRequirement != null)
                obj.SetDDLSOPRequirement(_ddlSOPRequirement);
            if (_ddlCreatedBy != null)
                obj.SetDDLCreatedBy(_ddlCreatedBy);
            if (_ddlContractor != null)
                obj.SetDDLContractor(_ddlContractor);
            if (_ddlIsAssignedToContractor != null)
                obj.SetDDLIsAssignedToContractor(_ddlIsAssignedToContractor);
            if (_ddlLastCrewAssigned != null)
                obj.SetDDLLastCrewAssigned(_ddlLastCrewAssigned);
            if (_ddlCompleted != null)
                obj.SetDDLCompleted(_ddlCompleted);
            if (_ddlCancelled != null)
                obj.SetDDLCancelled(_ddlCancelled);
            if (_txtSapNotificationNumber != null)
                obj.SetTXTSAPNotificationNumber(_txtSapNotificationNumber);
            if (_txtSapWorkOrderNumber != null)
                obj.SetTXTSAPWorkOrderNumber(_txtSapWorkOrderNumber);
            if (_ddlRequiresInvoice != null)
                obj.SetDDLRequiresInvoice(_ddlRequiresInvoice);
            if (_ddlHasInvoice != null)
                obj.SetDDLHasInvoice(_ddlHasInvoice);
            if (_txtWBSCharged != null)
                obj.SetTxtAccountCharged(_txtWBSCharged);
            if (_ddlAcousticMonitoringType != null)
                obj.SetDDLAcousticMonitoringType(_ddlAcousticMonitoringType);
            if (_ddlStreetOpeningPermitRequested != null)
                obj.SetDDLStreetOpeningPermitRequested(_ddlStreetOpeningPermitRequested);
            if (_ddlStreetOpeningPermitIssued != null)
                obj.SetDDLStreetOpeningPermitIssued(_ddlStreetOpeningPermitIssued);
            obj.SetPostBack(_postBack);
            return obj;
        }

        public TestWorkOrderGeneralSearchViewBuilder WithBaseSearchControl(IBaseWorkOrderSearch ctrl)
        {
            _baseSearch = ctrl;
            return this;
        }

        public TestWorkOrderGeneralSearchViewBuilder WithTXTAssetID(ITextBox txt)
        {
            _txtAssetID = txt;
            return this;
        }

        public TestWorkOrderGeneralSearchViewBuilder WithDDLAssetType(IDropDownList list)
        {
            _ddlAssetType = list;
            return this;
        }

        public TestWorkOrderGeneralSearchViewBuilder WithDDLDateType(IDropDownList list)
        {
            _ddlDateType = list;
            return this;
        }

        public TestWorkOrderGeneralSearchViewBuilder WithLSTDrivenBy(IListBox list)
        {
            _lstDrivenBy = list;
            return this;
        }

        public TestWorkOrderGeneralSearchViewBuilder WithDDLPriority(IDropDownList list)
        {
            _ddlPriority = list;
            return this;
        }

        public TestWorkOrderGeneralSearchViewBuilder WithDDLRequestedBy(IDropDownList list)
        {
            _ddlRequestedBy = list;
            return this;
        }

        public TestWorkOrderGeneralSearchViewBuilder WithDDLMarkoutRequirement(IDropDownList list)
        {
            _ddlMarkoutRequirement = list;
            return this;
        }

        public TestWorkOrderGeneralSearchViewBuilder WithDDLSOPRequirement(IDropDownList list)
        {
            _ddlSOPRequirement = list;
            return this;
        }

        public TestWorkOrderGeneralSearchViewBuilder WithDDLStreetOpeningPermitRequested(IDropDownList list)
        {
            _ddlStreetOpeningPermitRequested = list;
            return this;
        }

        public TestWorkOrderGeneralSearchViewBuilder WithDDLStreetOpeningPermitIssued(IDropDownList list)
        {
            _ddlStreetOpeningPermitIssued = list;
            return this;
        }

        public TestWorkOrderGeneralSearchViewBuilder WithLSTDescriptionOfWork(IListBox work)
        {
            _lstDescriptionOfWork = work;
            return this;
        }

        public TestWorkOrderGeneralSearchViewBuilder WithDRDateReceived(IDateRange dateReceived)
        {
            _drDateReceived = dateReceived;
            return this;
        }

        public TestWorkOrderGeneralSearchViewBuilder WithLSTDocumentType(IListBox type)
        {
            _lstDocumentType = type;
            return this;
        }

        public TestWorkOrderGeneralSearchViewBuilder WithDDLCreatedBy(IDropDownList createdBy)
        {
            _ddlCreatedBy = createdBy;
            return this;
        }

        public TestWorkOrderGeneralSearchViewBuilder WithDDLContractor(IDropDownList contractor)
        {
            _ddlContractor = contractor;
            return this;
        }

        public TestWorkOrderGeneralSearchViewBuilder WithDDLIsAssignedToContractor(IDropDownList isAssignedToContractor)
        {
            _ddlIsAssignedToContractor = isAssignedToContractor;
            return this;
        }

        public TestWorkOrderGeneralSearchViewBuilder WithDDLLastCrewAssigned(IDropDownList ddlLastCrewAssigned)
        {
            _ddlLastCrewAssigned = ddlLastCrewAssigned;
            return this;
        }

        public TestWorkOrderGeneralSearchViewBuilder WithDDLCompleted(IDropDownList ddlCompleted)
        {
            _ddlCompleted = ddlCompleted;
            return this;
        }

        public TestWorkOrderGeneralSearchViewBuilder WithDDLCancelled(IDropDownList ddlCancelled)
        {
            _ddlCancelled = ddlCancelled;
            return this;
        }

        public TestWorkOrderGeneralSearchViewBuilder WithTXTSAPNotificationNumber(ITextBox txtSapNotificationNumber)
        {
            _txtSapNotificationNumber = txtSapNotificationNumber;
            return this;
        }

        public TestWorkOrderGeneralSearchViewBuilder WithTXTSAPWorkOrderNumber(ITextBox txtSapWorkOrderNumber)
        {
            _txtSapWorkOrderNumber = txtSapWorkOrderNumber;
            return this;
        }

        public TestWorkOrderGeneralSearchViewBuilder WithDDLRequiresInvoice(IDropDownList ddlRequiresInvoice)
        {
            _ddlRequiresInvoice = ddlRequiresInvoice;
            return this;
        }

        public TestWorkOrderGeneralSearchViewBuilder WithDDLHasInvoice(IDropDownList ddlHasInvoice)
        {
            _ddlHasInvoice = ddlHasInvoice;
            return this;
        }

        public TestWorkOrderGeneralSearchViewBuilder WithTXTWBSCharged(ITextBox txtWbsCharged)
        {
            _txtWBSCharged = txtWbsCharged;
            return this;
        }

        public TestWorkOrderGeneralSearchViewBuilder
            WithDDLAcousticMonitoringType(IDropDownList ddl)
        {
            _ddlAcousticMonitoringType = ddl;
            return this;
        }

        #endregion
    }

    internal class TestWorkOrderGeneralSearchView : WorkOrderGeneralSearchView
    {
        #region Exposed Methods

        public void SetPostBack(bool postBack)
        {
            _isMvpPostBack = postBack;
        }

        public void SetBaseSearch(IBaseWorkOrderSearch ctrl)
        {
            baseSearch = ctrl;
        }

        public void SetTXTAssetID(ITextBox txt)
        {
            txtAssetID = txt;
        }

        public void SetDDLAssetType(IDropDownList ddl)
        {
            ddlAssetType = ddl;
        }

        public void SetDDLDateType(IDropDownList ddl)
        {
            ddlDateType = ddl;
        }

        public void SetLSTDrivenBy(IListBox lb)
        {
            lstDrivenBy = lb;
        }

        public void SetDDLPriority(IDropDownList ddl)
        {
            ddlPriority = ddl;
        }

        public void SetDDLRequestedBy(IDropDownList ddl)
        {
            ddlRequestedBy = ddl;
        }

        public void SetDDLMarkoutRequirement(IDropDownList ddl)
        {
            ddlMarkoutRequirement = ddl;
        }

        public void SetDDLSOPRequirement(IDropDownList ddl)
        {
            ddlSOPRequirement = ddl;
        }

        public void SetLSTDescriptionOfWork(IListBox work)
        {
            lstDescriptionOfWork = work;
        }

        public void SetDRDateReceived(IDateRange dateReceived)
        {
            drDateToSearch = dateReceived;
        }

        public void SetLSTDocumentType(IListBox type)
        {
            lstDocumentType = type;
        }

        public void SetDDLCreatedBy(IDropDownList ddl)
        {
            ddlCreatedBy = ddl;
        }

        public void SetDDLContractor(IDropDownList dropDownList)
        {
            ddlContractor = dropDownList;
        }

        public void SetDDLIsAssignedToContractor(IDropDownList dropDownList)
        {
            ddlIsAssignedToContractor = dropDownList;
        }

        public void SetDDLLastCrewAssigned(IDropDownList dropDownList)
        {
            ddlLastCrewAssigned = dropDownList;
        }

        public void SetDDLCompleted(IDropDownList dropDownList)
        {
            ddlCompleted = dropDownList;
        }

        public void SetDDLCancelled(IDropDownList dropDownList)
        {
            ddlCancelled = dropDownList;
        }

        public void SetTXTSAPNotificationNumber(ITextBox txtSapNotificationNumber)
        {
            txtSAPNotificationNumber = txtSapNotificationNumber;
        }

        public void SetTXTSAPWorkOrderNumber(ITextBox txtSapWorkOrderNumber)
        {
            txtSAPWorkOrderNumber = txtSapWorkOrderNumber;
        }

        public void SetDDLRequiresInvoice(IDropDownList ddl)
        {
            ddlRequiresInvoice = ddl;
        }

        public void SetDDLHasInvoice(IDropDownList ddl)
        {
            ddlHasInvoice = ddl;
        }

        public void SetTxtAccountCharged(ITextBox txt)
        {
            txtWBSCharged = txt;
        }

        public void SetDDLAcousticMonitoringType(IDropDownList ddl)
        {
            ddlAcousticMonitoringType = ddl;
        }

        internal void SetDDLStreetOpeningPermitRequested(IDropDownList dropDownList)
        {
            ddlStreetOpeningPermitRequested = dropDownList;
        }

        internal void SetDDLStreetOpeningPermitIssued(IDropDownList dropDownList)
        {
            ddlStreetOpeningPermitIssued = dropDownList;
        }

        #endregion
    }
}
