using System;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels
{
    [TestClass]
    public class CrewAssignmentIndexTableRowViewTest
    {
        private TestDateTimeProvider _dateTimeProvider;
        private DateTime _now;
        private CrewAssignmentIndexTableRowView _target;

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            var container = new Container(e =>
                e.For<IDateTimeProvider>()
                 .Use(_dateTimeProvider = new TestDateTimeProvider(_now = DateTime.Now)));

            _target = container.GetInstance<CrewAssignmentIndexTableRowView>();
            _target.CrewAssignment = new CrewAssignment {
                AssignedFor = _now.AddHours(-1),
                WorkOrder = new WorkOrder {
                    CancelledAt = null,
                    MarkoutRequirement = new MarkoutRequirement {
                        Id = (int)MarkoutRequirement.Indices.EMERGENCY
                    }
                }
            };
        }

        #endregion

        [TestMethod]
        public void TestCanSetStartTimeReturnsTrueForAssignmentForPastWithUncancelledWorkOrderAndEmergencyMarkoutRequirement()
        {
            // this is the default case given the setup in TestInitialize
            Assert.IsTrue(_target.CanSetStartTime);
        }

        [TestMethod]
        public void TestCanSetStartTimeReturnsTrueForAssignmentForPastWithUncancelledWorkOrderAndNoMarkoutRequirement()
        {
            _target.CrewAssignment.WorkOrder.MarkoutRequirement.Id = (int)MarkoutRequirement.Indices.NONE;

            Assert.IsTrue(_target.CanSetStartTime);
        }

        [TestMethod]
        public void TestCanSetStartTimeReturnsTrueForAssignmentForPastWithUncancelledWorkOrderAndCurrentReadyAndUnexpiredMarkout()
        {
            _target.CrewAssignment.WorkOrder.MarkoutRequirement.Id = (int)MarkoutRequirement.Indices.ROUTINE;
            _target.CrewAssignment.WorkOrder.Markouts.Add(new Markout {
                ReadyDate = _now.AddDays(-1),
                ExpirationDate = _now.AddDays(1)
            });

            Assert.IsTrue(_target.CanSetStartTime);
        }

        [TestMethod]
        public void TestCanSetStartTimeDoesNotReturnFalseWhenAssignmentIsInFuture()
        {
            _target.CrewAssignment.AssignedFor = _now.AddDays(1);

            Assert.IsTrue(_target.CanSetStartTime);
        }

        [TestMethod]
        public void TestCanSetStartTimeReturnsFalseWhenAssignmentOrderHasBeenCancelled()
        {
            _target.CrewAssignment.WorkOrder.CancelledAt = _now;

            Assert.IsFalse(_target.CanSetStartTime);
        }

        [TestMethod]
        public void TestCanSetStartTimeReturnsFalseWhenAssignmentOrderCurrentMarkoutNotYetReady()
        {
            _target.CrewAssignment.WorkOrder.MarkoutRequirement.Id = (int)MarkoutRequirement.Indices.ROUTINE;
            _target.CrewAssignment.WorkOrder.Markouts.Add(new Markout {
                ReadyDate = _now.AddDays(1),
                ExpirationDate = _now.AddDays(2)
            });

            Assert.IsFalse(_target.CanSetStartTime);
        }

        [TestMethod]
        public void TestCanSetStartTimeReturnsFalseWhenAssignmentOrderCurrentMarkoutExpired()
        {
            _target.CrewAssignment.WorkOrder.MarkoutRequirement.Id = (int)MarkoutRequirement.Indices.ROUTINE;
            _target.CrewAssignment.WorkOrder.Markouts.Add(new Markout {
                ReadyDate = _now.AddDays(-2),
                ExpirationDate = _now.AddDays(-1)
            });

            Assert.IsFalse(_target.CanSetStartTime);
        }

        [TestMethod]
        public void TestCanSetStartTimeReturnsTrueWhenAssignmentOrderHasValidOverlappingMarkouts()
        {
            _dateTimeProvider.SetNow(_now = new DateTime(2022, 11, 29, 12, 47, 00));
            _target.CrewAssignment.WorkOrder.MarkoutRequirement.Id = (int)MarkoutRequirement.Indices.ROUTINE;
            _target.CrewAssignment.WorkOrder.Markouts.Add(new Markout {
                DateOfRequest = new DateTime(2022, 11, 1),
                ReadyDate = new DateTime(2022, 11, 1),
                ExpirationDate = new DateTime(2022, 11, 30)
            });
            _target.CrewAssignment.WorkOrder.Markouts.Add(new Markout {
                DateOfRequest = new DateTime(2022, 11, 29),
                ReadyDate = new DateTime(2022, 11, 30),
                ExpirationDate = new DateTime(2022, 12, 8)
            });

            Assert.IsTrue(_target.CanSetStartTime);
        }

        [TestMethod]
        public void TestCanSetStartTimeReturnsFalseWhenAssignmentOrderMarkoutRequirementIsEmergencyButStartTimeAlreadyHasValue()
        {
            _target.CrewAssignment.DateStarted = _now;

            Assert.IsFalse(_target.CanSetStartTime);
        }

        [TestMethod]
        public void TestCanSetStartTimeReturnsFalseWhenAssignmentOrderMarkoutRequirementIsNoneButStartTimeAlreadyHasValue()
        {
            _target.CrewAssignment.WorkOrder.MarkoutRequirement.Id = (int)MarkoutRequirement.Indices.NONE;
            _target.CrewAssignment.DateStarted = _now;

            Assert.IsFalse(_target.CanSetStartTime);
        }

        [TestMethod]
        public void TestCanSetStartTimeReturnsFalseWhenAssignmentOrderMarkoutRequirementIsSatisfiedButStartTimeAlreadyHasValue()
        {
            _target.CrewAssignment.WorkOrder.MarkoutRequirement.Id = (int)MarkoutRequirement.Indices.ROUTINE;
            _target.CrewAssignment.WorkOrder.Markouts.Add(new Markout {
                ReadyDate = _now.AddDays(-1),
                ExpirationDate = _now.AddDays(1)
            });
            _target.CrewAssignment.DateStarted = _now;

            Assert.IsFalse(_target.CanSetStartTime);
        }
    }
}
