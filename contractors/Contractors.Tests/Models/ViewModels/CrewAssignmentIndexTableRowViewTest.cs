using System;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MapCall.Common.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace Contractors.Tests.Models.ViewModels
{
    [TestClass]
    public class CrewAssignmentIndexTableRowViewTest
    {
        #region Fields

        private CrewAssignmentIndexTableRowView _target;
        private CrewAssignment _crewAss;
        private readonly DateTime _testDate = DateTime.Now;
        private IContainer _container;

        #endregion

        #region Setup/cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _container = new Container(e =>
                e.For<IDateTimeProvider>().Use<DateTimeProvider>());
            _crewAss = new CrewAssignment {
                // Setting up properties that should never be null.
                WorkOrder = new WorkOrder {
                    DateTimeProvider = _container.GetInstance<IDateTimeProvider>(),
                    WorkDescription = new WorkDescription(),
                    Priority = new WorkOrderPriority()
                }
            };
            _target = _container.With(_crewAss).GetInstance<CrewAssignmentIndexTableRowView>();
        }

        #endregion

        #region Tests

        #region Constructor

        [TestMethod]
        public void TestConstructorThrowsForNullArgument()
        {
            MyAssert.Throws<ArgumentNullException>(() => new CrewAssignmentIndexTableRowView(null, null));
        }

        [TestMethod]
        public void TestConstructorSetsCrewAssignmentProperty()
        {
            Assert.IsNotNull(_target.CrewAssignment);
            Assert.AreSame(_crewAss, _target.CrewAssignment);
        }
        
        #endregion

        #region AssignedFor

        [TestMethod]
        public void TestAssignedForReturnsCrewAssignmentsAssignedForValue()
        {
            _crewAss.AssignedFor = _testDate;
            Assert.AreEqual(_testDate, _target.AssignedFor);
        }

        #endregion

        #region AssignedOn

        [TestMethod]
        public void TestAssignedOnReturnsCrewAssignmentsAssignedOnValue()
        {
            _crewAss.AssignedOn = _testDate;
            Assert.AreEqual(_testDate, _target.AssignedOn);
        }

        #endregion

        #region AssignedTo

        [TestMethod]
        public void TestAssignedToReturnsCrewDescription()
        {
            _crewAss.Crew = new Crew {Description = "I am a crew"};
            Assert.AreEqual("I am a crew", _target.AssignedTo);
        }
        
        #endregion

        #region CanSetEndTime

        [TestMethod]
        public void TestCanSetEndTimeReturnsTrueIfEndTimeDoesNotHaveValueAndStartTimeHasValue()
        {
            _crewAss.DateStarted = _testDate;
            _crewAss.DateEnded = null;
            Assert.IsTrue(_target.CanSetEndTime);
        }

        [TestMethod]
        public void TestCanSetEndTimeReturnsFalseIfEndTimeHasValue()
        {
            _crewAss.DateEnded = _testDate;
            Assert.IsFalse(_target.CanSetEndTime);
        }

        [TestMethod]
        public void TestCanSetEndTimeReturnsFalesIfStartTimeDoesNotHaveValue()
        {
            _crewAss.DateStarted = null;
            Assert.IsFalse(_target.CanSetEndTime);
        }

        [TestMethod]
        public void TestCanSetEndTimeReturnsFalseIfCancelled()
        {
            _crewAss.DateStarted = _testDate;
            _crewAss.DateEnded = null;
            _crewAss.WorkOrder.CancelledAt = DateTime.Now;

            Assert.IsFalse(_target.CanSetEndTime);
        }

        #endregion

        #region CanSetStartTime

        #region CanStart

        [TestMethod]
        public void TestCanStartReturnsFalseIfScheduledInTheFutureAndMarkoutRequiredAndDoesNotExist()
        {
            var contractor = new Contractor();
            contractor.SetPropertyValueByName("Id", 1);
            var markoutRequirement = new MarkoutRequirement();
            markoutRequirement.SetPropertyValueByName("Id", (int)MarkoutRequirementEnum.Routine);
            var workorder = new WorkOrder { AssignedContractor = contractor, MarkoutRequirement = markoutRequirement };
            var crew = new Crew { Contractor = contractor };
            var ca = new CrewAssignment { Crew = crew, WorkOrder = workorder, AssignedFor = DateTime.Now.AddDays(2) };

            var target = _container.With(ca).GetInstance<CrewAssignmentIndexTableRowView>();

            Assert.IsFalse(target.CanSetStartTime);
        }

        [TestMethod]
        public void TestCanStartReturnsFalseIfScheduledInTheFutureAndMarkoutIsNotReady()
        {
            var contractor = new Contractor();
            contractor.SetPropertyValueByName("Id", 1);
            var markoutRequirement = new MarkoutRequirement();
            markoutRequirement.SetPropertyValueByName("Id", (int)MarkoutRequirementEnum.Routine);
            var workorder = new WorkOrder {
                AssignedContractor = contractor,
                MarkoutRequirement = markoutRequirement,
                DateTimeProvider = _container.GetInstance<IDateTimeProvider>()
            };
            workorder.Markouts.Add(new Markout { ReadyDate = DateTime.Now.AddDays(2) });
            var crew = new Crew { Contractor = contractor };
            var ca = new CrewAssignment { Crew = crew, WorkOrder = workorder, AssignedFor = DateTime.Now.AddDays(2) };

            var target = _container.With(ca).GetInstance<CrewAssignmentIndexTableRowView>();

            Assert.IsFalse(target.CanSetStartTime);
        }

        [TestMethod]
        public void TestCanStartReturnsFalseIfStarted()
        {
            var contractor = new Contractor();
            contractor.SetPropertyValueByName("Id", 1);
            var markoutRequirement = new MarkoutRequirement();
            markoutRequirement.SetPropertyValueByName("Id", (int)MarkoutRequirementEnum.None);
            var workorder = new WorkOrder { AssignedContractor = contractor, MarkoutRequirement = markoutRequirement };
            var crew = new Crew { Contractor = contractor };
            var ca = new CrewAssignment
            {
                Crew = crew,
                WorkOrder = workorder,
                AssignedFor = DateTime.Now,
                DateStarted = DateTime.Now
            };

            var target = _container.With(ca).GetInstance<CrewAssignmentIndexTableRowView>();

            Assert.IsFalse(target.CanSetStartTime);
        }

        [TestMethod]
        public void TestCanStartReturnsTrueIfNotStartedAndNotScheduledInTheFutureWithMarkoutRequirementNone()
        {
            var contractor = new Contractor();
            contractor.SetPropertyValueByName("Id", 1);
            var markoutRequirement = new MarkoutRequirement();
            markoutRequirement.SetPropertyValueByName("Id", (int)MarkoutRequirementEnum.None);
            var workorder = new WorkOrder { AssignedContractor = contractor, MarkoutRequirement = markoutRequirement };
            var crew = new Crew { Contractor = contractor };
            var ca = new CrewAssignment
            {
                Crew = crew,
                WorkOrder = workorder,
                AssignedFor = DateTime.Now
            };

            var target = _container.With(ca).GetInstance<CrewAssignmentIndexTableRowView>();

            Assert.IsTrue(target.CanSetStartTime);
        }

        [TestMethod]
        public void TestCanStartReturnsTrueIfNotStartedAndNotScheduledInTheFutureWithMarkoutRequirementRoutine()
        {
            var contractor = new Contractor();
            contractor.SetPropertyValueByName("Id", 1);
            var markoutRequirement = new MarkoutRequirement();
            markoutRequirement.SetPropertyValueByName("Id", (int)MarkoutRequirementEnum.Routine);
            var workorder = new WorkOrder {
                AssignedContractor = contractor,
                MarkoutRequirement = markoutRequirement,
                DateTimeProvider = _container.GetInstance<IDateTimeProvider>()
            };
            workorder.Markouts.Add(new Markout { ReadyDate = DateTime.Today, ExpirationDate = DateTime.Now.AddDays(7)});
            var crew = new Crew { Contractor = contractor };
            var ca = new CrewAssignment {
                Crew = crew,
                WorkOrder = workorder,
                AssignedFor = DateTime.Now
            };

            var target = _container.With(ca).GetInstance<CrewAssignmentIndexTableRowView>();

            Assert.IsTrue(target.CanSetStartTime);
        }

        [TestMethod]
        public void TestCanStartIfMarkoutRequirementEmergency()
        {
            var contractor = new Contractor();
            contractor.SetPropertyValueByName("Id", 1);
            var markoutRequirement = new MarkoutRequirement();
            markoutRequirement.SetPropertyValueByName("Id", (int)MarkoutRequirementEnum.Emergency);
            var workorder = new WorkOrder { AssignedContractor = contractor, MarkoutRequirement = markoutRequirement };
            var crew = new Crew { Contractor = contractor };
            var ca = new CrewAssignment
            {
                Crew = crew,
                WorkOrder = workorder,
                AssignedFor = DateTime.Now
            };

            var target = _container.With(ca).GetInstance<CrewAssignmentIndexTableRowView>();

            Assert.IsTrue(target.CanSetStartTime);
        }

        [TestMethod]
        public void TestCanSetStartTimeReturnFalseIfWorkOrderCancelled()
        {
            var contractor = new Contractor();
            contractor.SetPropertyValueByName("Id", 1);
            var markoutRequirement = new MarkoutRequirement();
            markoutRequirement.SetPropertyValueByName("Id", (int)MarkoutRequirementEnum.Emergency);
            var workorder = new WorkOrder { AssignedContractor = contractor, MarkoutRequirement = markoutRequirement, CancelledAt = DateTime.Now };
            var crew = new Crew { Contractor = contractor };
            var ca = new CrewAssignment
            {
                Crew = crew,
                WorkOrder = workorder,
                AssignedFor = DateTime.Now
            };

            var target = _container.With(ca).GetInstance<CrewAssignmentIndexTableRowView>();
            
            Assert.IsFalse(target.CanSetStartTime);
        }


        #endregion

        #endregion

        #region CrewAssignmentPriority

        [TestMethod]
        public void TestCrewAssignmentPriorityReturnsCrewAssignmentsPriorityValue()
        {
            _crewAss.Priority = 9000;
            Assert.AreEqual(9000, _target.CrewAssignmentPriority);
        }
        
        #endregion

        #region EmployeesOnJob

        [TestMethod]
        public void TestEmployeesOnJobReturnsCrewAssignmentsEmployeesOnJobValue()
        {
            _crewAss.EmployeesOnJob = 13513;
            Assert.AreEqual(13513, _target.EmployeesOnJob);
        }

        #endregion

        #region EndTime

        [TestMethod]
        public void TestEndTimeReturnsCrewAssignmentsDateEndedValue()
        {
            _crewAss.DateEnded = _testDate;
            Assert.AreEqual(_testDate, _target.EndTime);
        }

        #endregion

        #region IsCompleted

        [TestMethod]
        public void TestIsCompletedReturnsTrueIfWorkOrdersDateCompletedHasValue()
        {
            _crewAss.WorkOrder.DateCompleted = DateTime.Now;
            Assert.IsTrue(_target.IsCompleted);
        }

        [TestMethod]
        public void TestIsCompletedReturnsFalseIfWorkOrderDateCompletedIsNull()
        {
            _crewAss.WorkOrder.DateCompleted = null;
            Assert.IsFalse(_target.IsCompleted);
        }
        

        #endregion

        #region MarkoutExpirationDate

        [TestMethod]
        public void TestMarkoutExpirationDateReturnsNullIfWorkOrdersCurrentMarkoutIsNull()
        {
            Assert.IsNull(_target.MarkoutExpirationDate);
        }

        [TestMethod]
        public void TestMarkoutExpirationDateReturnsWorkOrdersCurrentMarkoutValuesExpirationDateIfCurrentMarkoutIsNotNull()
        {
            var m = new CurrentMarkout();
            m.ExpirationDate = _testDate;
            _crewAss.WorkOrder.CurrentMarkout = m;
            Assert.AreEqual(_testDate, _target.MarkoutExpirationDate);
        }
        

        #endregion

        #region NearestCrossStreetName

        [TestMethod]
        public void TestNearestCrossStreetNameReturnsNullIfWorkOrdersCrossStreetIsNull()
        {
            Assert.IsNull(_target.NearestCrossStreetName);
        }

        [TestMethod]
        public void TestNearestCrossStreetNameReturnsWorkOrdersCrossStreetsFullStName()
        {
            var street = new Street();
            street.FullStName = "HELL NO ROAD";
            _crewAss.WorkOrder.NearestCrossStreet = street;
            Assert.AreEqual("HELL NO ROAD", _target.NearestCrossStreetName);
        }
        

        #endregion

        #region Notes
        
        [TestMethod]
        public void TestNotesReturnsNoNotesEnteredIfNoNotesAreEntered()
        {
            _crewAss.WorkOrder.Notes = null;
            Assert.AreEqual(CrewAssignmentIndexTableRowView.NO_NOTES, _target.Notes);
            _crewAss.WorkOrder.Notes = string.Empty;
            Assert.AreEqual(CrewAssignmentIndexTableRowView.NO_NOTES, _target.Notes);
            _crewAss.WorkOrder.Notes = "         ";
            Assert.AreEqual(CrewAssignmentIndexTableRowView.NO_NOTES, _target.Notes);
        }

        [TestMethod]
        public void TestNotesReturnsWorkOrderNotes()
        {
            var expected = "REAGAN SLEEEEEPY";
            _crewAss.WorkOrder.Notes = expected;
            Assert.AreEqual(expected, _target.Notes);
        }

        #endregion

        #region NotesTitle

        [TestMethod]
        public void TestNotesTitleReturnsWorkOrdersWorkDescriptionsDescription()
        {
            _crewAss.WorkOrder.WorkDescription.Description = "Hi";
            Assert.AreEqual("Hi", _target.NotesTitle);
        }
        
        #endregion

        #region StartTime

        [TestMethod]
        public void TestStartTimeReturnsCrewAssignmentsDateStarted()
        {
            _crewAss.DateStarted = _testDate;
            Assert.AreEqual(_testDate, _target.StartTime);
        }

        #endregion

        #region StreetName

        [TestMethod]
        public void TestStreetNameReturnsNullIfWorkOrdersStreetIsNull()
        {
            Assert.IsNull(_target.StreetName);
        }

        [TestMethod]
        public void TestStreetNameReturnsWorkOrdersStreetsFullStName()
        {
            var street = new Street();
            street.FullStName = "HELL NO ROAD";
            _crewAss.WorkOrder.Street = street;
            Assert.AreEqual("HELL NO ROAD", _target.StreetName);
        }

        #endregion

        #region StreetNumber

        [TestMethod]
        public void TestStreetNumberReturnsWorkOrdersStreetNumber()
        {
            _crewAss.WorkOrder.StreetNumber = "51A";
            Assert.AreEqual("51A", _target.StreetNumber);
        }
        
        #endregion

        #region TimeToCompletion

        [TestMethod]
        public void TestTimeToCompletionReturnsWorkOrderDescriptionsTimeToCompletion()
        {
            var expected = (decimal) 12.5;
            _crewAss.WorkOrder.WorkDescription.TimeToComplete = expected;
            Assert.AreEqual(expected, _target.TimeToCompletion);
        }

        #endregion

        #region Town

        [TestMethod]
        public void TestTownReturnsNullIfWorkOrdersTownIsNull()
        {
            _crewAss.WorkOrder.Town = null;
            Assert.IsNull(_target.Town);
        }

        [TestMethod]
        public void TestTownReturnsWorkOrdersTownsShortNameValue()
        {
            _crewAss.WorkOrder.Town = new Town { ShortName = "Towny"};
            Assert.AreEqual("Towny", _target.Town);
        }

        #endregion

        #region TownSection

        [TestMethod]
        public void TestTownSectionReturnsNullIfWorkOrdersTownSectionIsNull()
        {
            _crewAss.WorkOrder.Town = null;
            Assert.IsNull(_target.Town);
        }

        [TestMethod]
        public void TestTownSectionReturnsWorkOrdersTownSectionsName()
        {
            _crewAss.WorkOrder.TownSection = new TownSection { Name = "TownSectiony" };
            Assert.AreEqual("TownSectiony", _target.TownSection);
        }

        #endregion

        #region WorkOrderID

        [TestMethod]
        public void TestWorkOrderIDReturnsCrewAssignmentsWorkOrderIDValue()
        {
            _crewAss.WorkOrder.Id = 534;
            Assert.AreEqual(534, _target.WorkOrder);
        }
        
        #endregion

        #region WorkOrderPriority

        [TestMethod]
        public void TestWorkOrderPriorityReturnsWorkOrdersPrioritysDescriptionValue()
        {
            _crewAss.WorkOrder.Priority.Description = "Derp";
            Assert.AreEqual("Derp", _target.WorkOrderPriority);
        }

        #endregion

        #region ContractorsMatch

        [TestMethod]
        public void TestContractorsMatchReturnsTrueIfTheyMatch()
        {
            var contractor = new Contractor();
            contractor.SetPropertyValueByName("Id", 1);
            var workOrder = new WorkOrder {AssignedContractor = contractor};
            var crew = new Crew { Contractor = contractor};
            var ca = new CrewAssignment {Crew = crew, WorkOrder = workOrder};
            var target = _container.With(ca).GetInstance<CrewAssignmentIndexTableRowView>();

            Assert.IsTrue(target.ContractorsMatch);
        }

        [TestMethod]
        public void TestContractorsMatchReturnsFalseIfTheyDoNotMatch()
        {
            var contractor = new Contractor {Id = 1};
            var otherContractor = new Contractor {Id = 2};
            var workOrder = new WorkOrder { AssignedContractor = contractor };
            var crew = new Crew { Contractor = otherContractor };
            var ca = new CrewAssignment { Crew = crew, WorkOrder = workOrder };
            var target = _container.With(ca).GetInstance<CrewAssignmentIndexTableRowView>();

            Assert.IsFalse(target.ContractorsMatch);
        }

        #endregion

        #endregion
    }
}
