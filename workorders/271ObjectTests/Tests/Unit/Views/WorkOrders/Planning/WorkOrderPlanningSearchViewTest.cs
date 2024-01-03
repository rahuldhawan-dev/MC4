using System;
using System.Collections.Generic;
using System.Linq;
using LINQTo271.Common;
using LINQTo271.Controls.WorkOrders;
using LINQTo271.Views.WorkOrders.Planning;
using MMSINC.Controls;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using WorkOrders.Library.Permissions;
using WorkOrders.Model;
using _271ObjectTests.Tests.Unit.Model;

namespace _271ObjectTests.Tests.Unit.Views.WorkOrders.Planning
{
    /// <summary>
    /// Summary description for WorkOrderPlanningSearchViewTest
    /// </summary>
    [TestClass]
    public class WorkOrderPlanningSearchViewTest : EventFiringTestClass
    {
        #region Private Members

        private TestWorkOrderPlanningSearchView _target;
        private IListBox lstDrivenBy;
        private IDropDownList ddlRequestedBy,
                              ddlAcousticMonitoringType,
                              ddlMarkoutRequirement,
                              ddlSOPRequirement,
                              ddlPriority,
                              ddlOfficeAssignment,
                              ddlStreetOpeningPermitRequested,
                              ddlStreetOpeningPermitIssued;
        private ICheckBox chkMarkoutToBeCalled;
        private IDateRange drDateReceived;
        private IBaseWorkOrderSearch baseSearch;
        private ISecurityService _securityService;
        private IPanel pnlAssignedTo;
        private ITextBox txtWBSCharged, txtNotes;


        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
                .DynamicMock(out baseSearch)
                .DynamicMock(out drDateReceived)
                .DynamicMock(out ddlRequestedBy)
                .DynamicMock(out ddlAcousticMonitoringType)
                .DynamicMock(out ddlMarkoutRequirement)
                .DynamicMock(out ddlSOPRequirement)
                .DynamicMock(out ddlPriority)
                .DynamicMock(out lstDrivenBy)
                .DynamicMock(out ddlOfficeAssignment)
                .DynamicMock(out _securityService)
                .DynamicMock(out pnlAssignedTo)
                .DynamicMock(out chkMarkoutToBeCalled)
                .DynamicMock(out txtWBSCharged)
                .DynamicMock(out txtNotes)
                .DynamicMock(out ddlStreetOpeningPermitRequested)
                .DynamicMock(out ddlStreetOpeningPermitIssued);

            _target = new TestWorkOrderPlanningSearchViewBuilder()
                .WithBaseSearchControl(baseSearch)
                .WithDRDateReceived(drDateReceived)
                .WithDDLRequestedBy(ddlRequestedBy)
                .WithDDLAcousticMonitoringTypeID(ddlAcousticMonitoringType)
                .WithDDLMarkoutRequirement(ddlMarkoutRequirement)
                .WithDDLSOPRequirement(ddlSOPRequirement)
                .WithDDLPriority(ddlPriority)
                .WithLSTDrivenBy(lstDrivenBy)
                .WithDDLOfficeAssignment(ddlOfficeAssignment)
                .WithSecurityService(_securityService)
                .WithPnlAssignedTo(pnlAssignedTo)
                .WithChkMarkoutToBeCalled(chkMarkoutToBeCalled)
                .WithTXTWBSCharged(txtWBSCharged)
                .WithTXTNotes(txtNotes)
                .WithStreetOpeningPermitRequested(ddlStreetOpeningPermitRequested)
                .WithStreetOpeningPermitIssued(ddlStreetOpeningPermitIssued);
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        #region Property Tests

        [TestMethod]
        public void TestDateReceivedReturnsValueOfDRDateReceived()
        {
            var expected = new DateTime(2000, 1, 1);

            using (_mocks.Record())
            {
                SetupResult.For(drDateReceived.Date).Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.DateReceived);
            }
        }

        [TestMethod]
        public void TestSopRequestedAndIssuedPropertyReturnsBooleanValue()
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

        [TestMethod]
        public void TestPhasePropertyDenotesGeneral()
        {
            Assert.AreEqual(WorkOrderPhase.Planning, _target.Phase);

            _mocks.ReplayAll();
        }

        /// <summary>
        /// This is the test that a work order is basically in the planning phase
        /// TODO: This needs a better home, but linq limits us on linq expressions
        /// against logical fields.
        /// </summary>
        [TestMethod]
        public void TestGetBaseExpression()
        {
            Func<WorkOrder, bool> expr;

            // no markout required, no sop required
            WorkOrder wo1 = new TestWorkOrderBuilder()
                .BuildForPlanning()
                .WithDateCompleted(DateTime.Now);

            // markout required, no markouts, no sop required
            WorkOrder wo2 = new TestWorkOrderBuilder()
                .BuildForPlanning()
                .WithSOPRequired(false);

            // markout required, w/markout, no sop required
            WorkOrder wo3 = new TestWorkOrderBuilder()
                .BuildForPlanning()
                .WithSOPRequired(false);
            wo3.Markouts.Add(new Markout {
                ExpirationDate = DateTime.Now.AddDays(1)
            });

            // no mo required, sop required, no sop
            WorkOrder wo4 = new TestWorkOrderBuilder()
                .BuildForPlanning()
                .WithMarkoutRequirement(TestMarkoutRequirementBuilder.None)
                .WithSOPRequired(true);
            
            // no mo req, sop req, w/sop
            WorkOrder wo5 = new TestWorkOrderBuilder()
                .BuildForPlanning()
                .WithMarkoutRequirement(TestMarkoutRequirementBuilder.None)
                .WithSOPRequired(true);
            wo5.StreetOpeningPermits.Add(new StreetOpeningPermit {
                DateIssued = DateTime.Now, 
                ExpirationDate = DateTime.Now.AddDays(1)
            });

            // mo req, w/mo, sop req, no sop
            WorkOrder wo6 = new TestWorkOrderBuilder()
                .BuildForPlanning()
                .WithSOPRequired(true);
            wo6.Markouts.Add(new Markout {
                ExpirationDate = DateTime.Now.AddDays(1)
            });

            // mo req, no mo, sop req, w/sop
            WorkOrder wo7 = new TestWorkOrderBuilder()
                .BuildForPlanning()
                .WithSOPRequired(true);
            wo7.StreetOpeningPermits.Add(new StreetOpeningPermit {
                DateIssued = DateTime.Now,
                ExpirationDate = DateTime.Now.AddDays(1)
            });

            // mo req, w/mo, sop req, w/sop
            WorkOrder wo8 = new TestWorkOrderBuilder()
                .BuildForPlanning()
                .WithSOPRequired(true);
            wo8.Markouts.Add(new Markout {
                ExpirationDate = DateTime.Now.AddDays(1)
            });
            wo8.StreetOpeningPermits.Add(new StreetOpeningPermit {
                DateIssued = DateTime.Now,
                ExpirationDate = DateTime.Now.AddDays(1)
            });

            WorkOrder wo9 = new TestWorkOrderBuilder()
                .BuildForPlanning()
                .WithMarkoutRequirement(TestMarkoutRequirementBuilder.Emergency)
                .WithSOPRequired(true)
                .WithPriority(new WorkOrderPriority { WorkOrderPriorityID = WorkOrderPriorityRepository.Indices.ROUTINE });


            using (_mocks.Record())
            {
                SetupResult.For(_securityService.IsAdmin).Return(true);
            }

            using (_mocks.Playback())
            {
                expr = _target.GenerateExpression().Compile();
                Assert.IsFalse(expr(wo1));
                Assert.IsTrue(expr(wo2));
                Assert.IsFalse(expr(wo3));
                Assert.IsTrue(expr(wo4));
                Assert.IsFalse(expr(wo5));
                Assert.IsTrue(expr(wo6));    
                Assert.IsTrue(expr(wo7));     // TODO: FIX
                Assert.IsFalse(expr(wo8));
                Assert.IsTrue(expr(wo9));
            }
        }

        [TestMethod]
        public void TestSearchFiltersAgainstPurposeWhenChosen()
        {
            const int EXPECTED_ID = 4;
            const int UNEXPECTED_ID = 2;

            var expected = new WorkOrderPurpose
            {
                WorkOrderPurposeID = EXPECTED_ID
            };
            var unexpected = new WorkOrderPurpose
            {
                WorkOrderPurposeID = UNEXPECTED_ID
            };

            Func<WorkOrder, bool> expr;
            WorkOrder wo = new TestWorkOrderBuilder()
                .BuildForPlanning()
                .WithPurpose(expected);
            WorkOrder wo2 = new TestWorkOrderBuilder()
                .BuildForPlanning()
                .WithPurpose(unexpected);

            using (_mocks.Record())
            {
                SetupResult.For(lstDrivenBy.GetSelectedValues()).Return(new List<int> { EXPECTED_ID });
                SetupResult.For(_securityService.IsAdmin).Return(true);
            }

            using (_mocks.Playback())
            {
                expr = _target.GenerateExpression().Compile();
                Assert.IsTrue(expr(wo));
                Assert.IsFalse(expr(wo2));
            }
        }

        [TestMethod]
        public void TestSearchFiltersAgainstSOPRequiredWhenChosen()
        {
            Func<WorkOrder, bool> expr;
            // 
            WorkOrder wo = new TestWorkOrderBuilder()
                .BuildForPlanning()
                .WithSOPRequired(true);

            WorkOrder wo2 = new TestWorkOrderBuilder()
                .BuildForPlanning()
                .WithSOPRequired(false);

            using (_mocks.Record())
            {
                SetupResult.For(ddlSOPRequirement.GetBooleanValue()).Return(true);
                SetupResult.For(_securityService.IsAdmin).Return(true);
            }

            using (_mocks.Playback())
            {
                expr = _target.GenerateExpression().Compile();
                Assert.IsTrue(expr(wo));
                Assert.IsFalse(expr(wo2));
            }
        }

        [TestMethod]
        public void TestSearchFiltersAgainstPriorityWhenChosen()
        {
            const int EXPECTED_ID = 4;
            const int UNEXPECTED_ID = 1;
            var expected = new WorkOrderPriority { WorkOrderPriorityID = EXPECTED_ID };
            var unexpected = new WorkOrderPriority { WorkOrderPriorityID = UNEXPECTED_ID };

            Func<WorkOrder, bool> expr;
            WorkOrder wo = new TestWorkOrderBuilder()
                .BuildForPlanning()
                .WithPriority(expected);
            WorkOrder wo2 = new TestWorkOrderBuilder()
                .BuildForPlanning()
                .WithPriority(unexpected);

            using (_mocks.Record())
            {
                SetupResult.For(ddlPriority.GetSelectedValue()).Return(EXPECTED_ID);
                SetupResult.For(_securityService.IsAdmin).Return(true);
            }

            using (_mocks.Playback())
            {
                expr = _target.GenerateExpression().Compile();
                Assert.IsTrue(expr(wo));
                Assert.IsFalse(expr(wo2));
            }
        }

        [TestMethod]
        public void TestSearchFiltersAgainstMarkoutRequirementWhenChosen()
        {
            const int EXPECTED_MARKOUT_ID = 4;
            const int UNEXPECTED_MARKOUT_ID = 1;
            var expectedMarkout = new MarkoutRequirement { MarkoutRequirementID = EXPECTED_MARKOUT_ID };
            var unexpectedMarkout = new MarkoutRequirement { MarkoutRequirementID = UNEXPECTED_MARKOUT_ID };

            Func<WorkOrder, bool> expr;
            WorkOrder wo = new TestWorkOrderBuilder()
                .BuildForPlanning()
                .WithMarkoutRequirement(expectedMarkout);
            WorkOrder wo2 = new TestWorkOrderBuilder()
                .BuildForPlanning()
                .WithMarkoutRequirement(unexpectedMarkout);

            using (_mocks.Record())
            {
                SetupResult.For(ddlMarkoutRequirement.GetSelectedValue()).Return(EXPECTED_MARKOUT_ID);
                SetupResult.For(_securityService.IsAdmin).Return(true);
            }

            using (_mocks.Playback())
            {
                expr = _target.GenerateExpression().Compile();
                Assert.IsTrue(expr(wo));
                Assert.IsFalse(expr(wo2));
            }
        }

        [TestMethod]
        public void TestSearchFiltersAgainstRequestedByIDWhenChosen()
        {
            const int REQUESTER_ID = 4;
            var requester = new WorkOrderRequester {
                WorkOrderRequesterID = 4
            };

            Func<WorkOrder, bool> expr;
            WorkOrder wo = new TestWorkOrderBuilder()
                .BuildForPlanning()
                .WithRequester(requester);

            using (_mocks.Record())
            {
                SetupResult.For(ddlRequestedBy.GetSelectedValue()).Return(
                    REQUESTER_ID);
                SetupResult.For(_securityService.IsAdmin).Return(true);
            }

            using (_mocks.Playback())
            {
                expr = _target.GenerateExpression().Compile();
                Assert.IsTrue(expr(wo));
            }
        }
        
        [TestMethod]
        public void TestSearchFiltersAgainstDateReceivedWhenChosen()
        {
            Func<WorkOrder, bool> expr;
            WorkOrder today = new TestWorkOrderBuilder()
                .BuildForPlanning()
                .WithDateReceived(DateTime.Today);

            WorkOrder yesterday = new TestWorkOrderBuilder()
                .BuildForPlanning()
                .WithDateReceived(DateTime.Today.AddDays(-1));

            WorkOrder tomorrow = new TestWorkOrderBuilder()
                .BuildForPlanning()
                .WithDateReceived(DateTime.Today.AddDays(1));

            // =
            using (_mocks.Record())
            {
                SetupResult.For(drDateReceived.SelectedOperator).Return("=");
                SetupResult.For(drDateReceived.Date).Return(DateTime.Today);
                SetupResult.For(_securityService.IsAdmin).Return(true);
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
                SetupResult.For(drDateReceived.SelectedOperator).Return(">=");
                SetupResult.For(drDateReceived.Date).Return(DateTime.Today);
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
                SetupResult.For(drDateReceived.SelectedOperator).Return(">");
                SetupResult.For(drDateReceived.Date).Return(DateTime.Today);
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
                SetupResult.For(drDateReceived.SelectedOperator).Return("<=");
                SetupResult.For(drDateReceived.Date).Return(DateTime.Today);
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
                SetupResult.For(drDateReceived.SelectedOperator).Return("<");
                SetupResult.For(drDateReceived.Date).Return(DateTime.Today);
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
                SetupResult.For(drDateReceived.StartDate).Return(DateTime.Today.AddDays(-1));
                SetupResult.For(drDateReceived.EndDate).Return(DateTime.Today);
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
        public void TestBaseExpressionFiltersOfficeAssignmentIDWhenNotAdmin()
        {
            var officeAssignmentID = 42;
            Func<WorkOrder, bool> expr;
            var wo =
                new TestWorkOrderBuilder()
                    .BuildForPlanning()
                    .WithOfficeAssignmentID(officeAssignmentID);
            var wo2 = new TestWorkOrderBuilder()
                    .BuildForPlanning()
                    .WithOfficeAssignmentID(0);

            using (_mocks.Record())
            {
                SetupResult.For(_securityService.IsAdmin).Return(false);
                SetupResult.For(_securityService.GetEmployeeID()).Return(
                    officeAssignmentID);
            }

            using (_mocks.Playback())
            {
                expr = _target.GenerateExpression().Compile();
                Assert.IsTrue(expr(wo));
                Assert.IsFalse(expr(wo2));
            }            
        }

        [TestMethod]
        public void TestSearchFiltersNotesWhenChosen()
        {
            var notes = "foo";
            Func<WorkOrder, bool> expr;
            var wo =
                new TestWorkOrderBuilder()
                    .BuildForPlanning()
                    .WithNotes(notes);
            var wo2 = new TestWorkOrderBuilder()
                .BuildForPlanning()
                .WithNotes("argh"); 

            using (_mocks.Record())
            {
                SetupResult.For(_securityService.IsAdmin).Return(true);
                SetupResult.For(txtNotes.Text).Return(notes);
            }

            using (_mocks.Playback())
            {
                expr = _target.GenerateExpression().Compile();
                Assert.IsTrue(expr(wo));
                Assert.IsFalse(expr(wo2));
            }
        }

        [TestMethod]
        public void TestSearchFiltersOfficeAssignmentIDWhenChosen()
        {
            var officeAssignmentID = 42;
            Func<WorkOrder, bool> expr;
            var wo =
                new TestWorkOrderBuilder()
                    .BuildForPlanning()
                    .WithOfficeAssignmentID(officeAssignmentID);
            var wo2 = new TestWorkOrderBuilder()
                    .BuildForPlanning()
                    .WithOfficeAssignmentID(0);

            using (_mocks.Record())
            {
                SetupResult.For(_securityService.IsAdmin).Return(true);
                SetupResult.For(ddlOfficeAssignment.GetSelectedValue()).Return(
                    officeAssignmentID);
            }

            using (_mocks.Playback())
            {
                expr = _target.GenerateExpression().Compile();
                Assert.IsTrue(expr(wo));
                Assert.IsFalse(expr(wo2));
            } 
        }

        [TestMethod]
        public void TestPagePrerenderSetsOfficeAssignmentToVisibleForAdmin()
        {
            bool admin = true;
            SetupResult.For(_securityService.IsAdmin).Return(admin);
            using (_mocks.Record())
            {
                pnlAssignedTo.Visible = admin;
            }
            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "Page_Prerender");
            }
        }
        
        [TestMethod]
        public void TestPagePrerenderSetsOfficeAssignmentToNotVisibleForNonAdmin()
        {
            bool admin = false;
            SetupResult.For(_securityService.IsAdmin).Return(admin);
            using (_mocks.Record())
            {
                pnlAssignedTo.Visible = admin;
            }
            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "Page_Prerender");
            }
        }

        [TestMethod]
        public void TestBaseExpressionExcludesSAPWorkOrdersWithOutSAPWorkOrderNumbers()
        {
            _mocks.ReplayAll();
            var operatingCenter = new OperatingCenter { SAPEnabled = true, SAPWorkOrdersEnabled = true };
            WorkOrder woInvalidSapOrder = new TestWorkOrderBuilder().BuildIncompleteOrder()
                .WithOperatingCenter(operatingCenter);

            IEnumerable<WorkOrder> orders = new[] { woInvalidSapOrder };

            var result = orders.Where(_target.BaseExpression.Compile()).ToList();

            Assert.IsFalse(result.Contains(woInvalidSapOrder));
        }

        [TestMethod]
        public void TestSearchFiltersAgainstSopRequestedNotChosenAndSopIssuedChosen()
        {
            WorkOrder wo = new TestWorkOrderBuilder().BuildForPlanning();
            wo.StreetOpeningPermits.Add(new StreetOpeningPermit {
                DateIssued = DateTime.Now,
                DateRequested = DateTime.Now.AddDays(1)
            });

            WorkOrder wo2 = new TestWorkOrderBuilder().BuildForPlanning();

            using (_mocks.Record())
            {
                SetupResult.For(ddlStreetOpeningPermitRequested.GetBooleanValue()).Return(null);
                SetupResult.For(ddlStreetOpeningPermitIssued.GetBooleanValue()).Return(true);
                SetupResult.For(_securityService.IsAdmin).Return(true);
            }

            using (_mocks.Playback())
            {
                var expr = _target.GenerateExpression().Compile();
                Assert.IsTrue(expr(wo));
                Assert.IsFalse(expr(wo2));
            }
        }

        [TestMethod]
        public void TestSearchFiltersAgainstSopRequestedAndIssuedNotChosen()
        {
            WorkOrder wo = new TestWorkOrderBuilder().BuildForPlanning();
            wo.StreetOpeningPermits.Add(new StreetOpeningPermit {
                DateIssued = DateTime.Now,
                DateRequested = DateTime.Now.AddDays(1)
            });

            WorkOrder wo2 = new TestWorkOrderBuilder().BuildForPlanning();

            using (_mocks.Record())
            {
                SetupResult.For(ddlStreetOpeningPermitRequested.GetBooleanValue()).Return(null);
                SetupResult.For(ddlStreetOpeningPermitIssued.GetBooleanValue()).Return(null);
                SetupResult.For(_securityService.IsAdmin).Return(true);
            }

            using (_mocks.Playback())
            {
                var expr = _target.GenerateExpression().Compile();
                Assert.IsTrue(expr(wo));
                Assert.IsTrue(expr(wo2));
            }
        }
    }

    internal class TestWorkOrderPlanningSearchViewBuilder : TestDataBuilder<TestWorkOrderPlanningSearchView>
    {
        private IBaseWorkOrderSearch _baseSearch;
        private IDateRange _drDateReceived;
        private IListBox _lstDrivenBy;
        private IDropDownList _ddlRequestedBy,
                              _ddlAcousticMonitoringTypeID,
                              _ddlMarkoutRequirement,
                              _ddlSOPRequirement,
                              _ddlPriority,
                              _ddlOfficeAssignment,
                              _ddlStreetOpeningPermitRequested,
                              _ddlStreetOpeningPermitIssued;
        private ICheckBox _chkMarkoutToBeCalled;
        private ISecurityService _securityService;
        private IPanel _pnlAssignedTo;
        private ITextBox _txtWBSCharged, _txtNotes;

        #region Overrides of Builder<TestWorkOrderPlanningSearchView>

        public override TestWorkOrderPlanningSearchView Build()
        {
            var obj = new TestWorkOrderPlanningSearchView();

            if (_drDateReceived != null)
                obj.SetDRDateReceived(_drDateReceived);
            if (_baseSearch != null)
                obj.SetBaseSearch(_baseSearch);
            if (_ddlRequestedBy != null)
                obj.SetDDLRequestedBy(_ddlRequestedBy);
            if(_ddlAcousticMonitoringTypeID != null)
                obj.SetDDLAcousticMonitoringTypeID(_ddlAcousticMonitoringTypeID);
            if (_ddlMarkoutRequirement != null)
                obj.SetDDLMarkoutRequirement(_ddlMarkoutRequirement);
            if (_ddlSOPRequirement != null)
                obj.SetDDLSOPRequirement(_ddlSOPRequirement);
            if (_ddlStreetOpeningPermitRequested != null)
                obj.SetDDLStreetOpeningPermitRequested(_ddlStreetOpeningPermitRequested);
            if (_ddlStreetOpeningPermitIssued != null)
                obj.SetDDLStreetOpeningPermitIssued(_ddlStreetOpeningPermitIssued);
            if (_ddlPriority != null)
                obj.SetDDLPriority(_ddlPriority);
            if (_lstDrivenBy != null)
                obj.SetLSTDrivenBy(_lstDrivenBy);
            if (_ddlOfficeAssignment != null)
                obj.SetDDLOfficeAssignment(_ddlOfficeAssignment);
            if (_securityService != null)
                obj.SetSecurityService(_securityService);
            if (_pnlAssignedTo != null)
                obj.SetPnlAssignedTo(_pnlAssignedTo);
            if (_chkMarkoutToBeCalled != null)
                obj.SetChkMarkoutToBeCalled(_chkMarkoutToBeCalled);
            if (_txtWBSCharged != null)
                obj.SetTxtAccountCharged(_txtWBSCharged);
            if (_txtNotes != null)
                obj.SetTXTNotes(_txtNotes);
            return obj;
        }

        #endregion

        public TestWorkOrderPlanningSearchViewBuilder WithDRDateReceived(IDateRange dateReceived)
        {
            _drDateReceived = dateReceived;
            return this;
        }

        public TestWorkOrderPlanningSearchViewBuilder WithBaseSearchControl(IBaseWorkOrderSearch ctrl)
        {
            _baseSearch = ctrl;
            return this;
        }

        public TestWorkOrderPlanningSearchViewBuilder WithDDLRequestedBy(IDropDownList dropDownList)
        {
            _ddlRequestedBy = dropDownList;
            return this;
        }

        public TestWorkOrderPlanningSearchViewBuilder WithDDLAcousticMonitoringTypeID(IDropDownList dropDownList)
        {
            _ddlAcousticMonitoringTypeID = dropDownList;
            return this;
        }

        public TestWorkOrderPlanningSearchViewBuilder WithDDLMarkoutRequirement(IDropDownList dropDownList)
        {
            _ddlMarkoutRequirement = dropDownList;
            return this;
        }

        public TestWorkOrderPlanningSearchViewBuilder WithDDLSOPRequirement(IDropDownList dropDownList)
        {
            _ddlSOPRequirement = dropDownList;
            return this;
        }

        public TestWorkOrderPlanningSearchViewBuilder WithDDLPriority(IDropDownList dropDownList)
        {
            _ddlPriority = dropDownList;
            return this;
        }

        public TestWorkOrderPlanningSearchViewBuilder WithLSTDrivenBy(IListBox lb)
        {
            _lstDrivenBy = lb;
            return this;
        }

        public TestWorkOrderPlanningSearchViewBuilder WithDDLOfficeAssignment(IDropDownList dropDownList)
        {
            _ddlOfficeAssignment = dropDownList;
            return this;
        }

        public TestWorkOrderPlanningSearchViewBuilder WithSecurityService(ISecurityService securityService)
        {
            _securityService = securityService;
            return this;
        }

        public TestWorkOrderPlanningSearchViewBuilder WithPnlAssignedTo(IPanel panel)
        {
            _pnlAssignedTo = panel;
            return this;
        }

        internal TestWorkOrderPlanningSearchViewBuilder WithChkMarkoutToBeCalled(ICheckBox chkMarkoutToBeCalled)
        {
            _chkMarkoutToBeCalled = chkMarkoutToBeCalled;
            return this;
        }

        public TestWorkOrderPlanningSearchViewBuilder WithTXTWBSCharged(ITextBox txtWbsCharged)
        {
            _txtWBSCharged = txtWbsCharged;
            return this;
        }

        public TestWorkOrderPlanningSearchViewBuilder WithTXTNotes(ITextBox txtNotes)
        {
            _txtNotes = txtNotes;
            return this;
        }

        public TestWorkOrderPlanningSearchViewBuilder WithStreetOpeningPermitRequested(IDropDownList sopRequested)
        {
            _ddlStreetOpeningPermitRequested = sopRequested;
            return this;
        }

        public TestWorkOrderPlanningSearchViewBuilder WithStreetOpeningPermitIssued(IDropDownList sopIssued)
        {
            _ddlStreetOpeningPermitIssued = sopIssued;
            return this;
        }
    }

    internal class TestWorkOrderPlanningSearchView : WorkOrderPlanningSearchView
    {
        internal void SetDRDateReceived(IDateRange dateReceived)
        {
            drDateReceived = dateReceived;
        }

        internal void SetBaseSearch(IBaseWorkOrderSearch ctrl)
        {
            baseSearch = ctrl;
        }

        internal void SetDDLRequestedBy(IDropDownList dropDownList)
        {
            ddlRequestedBy = dropDownList;
        }

        internal void SetDDLAcousticMonitoringTypeID(IDropDownList dropDownList)
        {
            ddlAcousticMonitoringType = dropDownList;
        }

        internal void SetDDLMarkoutRequirement(IDropDownList dropDownList)
        {
            ddlMarkoutRequirement = dropDownList;
        }

        internal void SetDDLSOPRequirement(IDropDownList dropDownList)
        {
            ddlSOPRequirement = dropDownList;
        }

        internal void SetDDLStreetOpeningPermitRequested(IDropDownList dropDownList)
        {
            ddlStreetOpeningPermitRequested = dropDownList;
        }

        internal void SetDDLStreetOpeningPermitIssued(IDropDownList dropDownList)
        {
            ddlStreetOpeningPermitIssued = dropDownList;
        }

        internal void SetDDLPriority(IDropDownList dropDownList)
        {
            ddlPriority = dropDownList;
        }

        internal void SetLSTDrivenBy(IListBox listBox)
        {
            lstDrivenBy = listBox;
        }

        internal void SetDDLOfficeAssignment(IDropDownList dropDownList)
        {
            ddlOfficeAssignment = dropDownList;
        }

        internal void SetPnlAssignedTo(IPanel panel)
        {
            pnlAssignedTo = panel;
        }

        internal void SetSecurityService(ISecurityService service)
        {
            _securityService = service;
        }

        public void SetChkMarkoutToBeCalled(ICheckBox checkBox)
        {
            chkMarkoutToBeCalled = checkBox;
        }

        public void SetTxtAccountCharged(ITextBox txt)
        {
            txtWBSCharged = txt;
        }

        public void SetTXTNotes(ITextBox txt)
        {
            txtNotes = txt;
        }
    }
}
