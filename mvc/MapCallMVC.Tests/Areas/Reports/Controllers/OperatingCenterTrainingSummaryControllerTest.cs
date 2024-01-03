using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Reports.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Reports.Controllers
{
    [TestClass]
    public class OperatingCenterTrainingSummaryControllerTest : MapCallMvcControllerTestBase<OperatingCenterTrainingSummaryController, OperatingCenter, OperatingCenterRepository>
    {
        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.IndexDisplaysViewWhenNoResults = true;
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a =>
            {
                a.RequiresRole("~/Reports/OperatingCenterTrainingSummary/Search", RoleModules.OperationsTrainingRecords);
                a.RequiresRole("~/Reports/OperatingCenterTrainingSummary/Index", RoleModules.OperationsTrainingRecords);
            });
        }

        [TestMethod]
        public void TestIndexReturnsIndexWithZeroResultsViewWithModelFromSpecialRepositoryMethod()
        {
            var search = new SearchOperatingCenterTrainingSummary();

            var result = _target.Index(search);
            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(0, search.Count);
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because of the amount of data needed.
            var state = GetEntityFactory<State>().Create();
            var sapCompanyCode = GetEntityFactory<SAPCompanyCode>().Create(new { Description = "New Jersey-American Water" });
            var positionGroupCommonName = GetEntityFactory<PositionGroupCommonName>().Create(new { Description = "Production Maintenenace TCPA Non-Supv" });
            var positionGroupCommonName2 = GetEntityFactory<PositionGroupCommonName>().Create(new { Description = "Production Maintenenace TCPB Non-Supv" });
            var positionGroup = GetEntityFactory<PositionGroup>().Create(new
            {
                Group = "MNTMCH4",
                PositionDescription = "",
                BusinessUnit = "181801",
                BusinessUnitDescription = "MN-Production",
                SAPCompanyCode = sapCompanyCode,
                State = state,
                CommonName = positionGroupCommonName
            });
            var positionGroup2 = GetEntityFactory<PositionGroup>().Create(new
            {
                Group = "MNTMCH3",
                PositionDescription = "",
                BusinessUnit = "181802",
                BusinessUnitDescription = "MN-Production",
                SAPCompanyCode = sapCompanyCode,
                State = state,
                CommonName = positionGroupCommonName2
            });

            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury" });
            var emp1 = GetFactory<ActiveEmployeeFactory>().Create(new { OperatingCenter = opc1, PositionGroup = positionGroup }); // 
            var emp2 = GetFactory<EmployeeFactory>().Create(new { OperatingCenter = opc1, PositionGroup = positionGroup });         // so we have an inactive employee
            var emp3 = GetFactory<ActiveEmployeeFactory>().Create(new { OperatingCenter = opc1, PositionGroup = positionGroup });   // employee that will have the initial training
            var emp4 = GetFactory<ActiveEmployeeFactory>().Create(new { OperatingCenter = opc1, PositionGroup = positionGroup });   // employee that will have the recurring
            var emp5 = GetFactory<ActiveEmployeeFactory>().Create(new { OperatingCenter = opc1, PositionGroup = positionGroup2 });    // employee in a different position group
            var emp6 = GetFactory<ActiveEmployeeFactory>().Create(new { OperatingCenter = opc1, PositionGroup = positionGroup });   // employee that is due recurring training and has had both initial and recurring
            var emp7 = GetFactory<ActiveEmployeeFactory>().Create(new { OperatingCenter = opc1, PositionGroup = positionGroup });   // employee that is due recurring training and has not had recurring at all

            opc1 = Repository.Find(opc1.Id);

            var trainingRequirement = GetEntityFactory<TrainingRequirement>().Create(new { Description = "Arc Flash / Electrical Safety in the Work Place - Awareness", TrainingFrequency = 365, TrainingFrequencyUnit = "D", IsActive = true });
            trainingRequirement.PositionGroupCommonNames.Add(positionGroupCommonName);
            Session.Save(trainingRequirement);
            trainingRequirement = Session.Load<TrainingRequirement>(trainingRequirement.Id);

            var initialTrainingModule = GetEntityFactory<TrainingModule>().Create(new { Title = "Electrical Safety in the Workplace (OSHA: 1910.120) - Initial course", TrainingRequirement = trainingRequirement });
            var refresherTrainingModule = GetEntityFactory<TrainingModule>().Create(new { Title = "Electrical Safety in the Workplace (OSHA: 1910.120) - Refresher course", TrainingRequirement = trainingRequirement });
            trainingRequirement.ActiveInitialTrainingModule = initialTrainingModule;
            trainingRequirement.ActiveRecurringTrainingModule = refresherTrainingModule;
            Session.Save(trainingRequirement);

            var trainingRecord1 = GetEntityFactory<TrainingRecord>().Create(new { HeldOn = DateTime.Now.AddDays(-2), TrainingModule = initialTrainingModule });
            GetFactory<TrainingRecordAttendedEmployeeFactory>().Create(new { Employee = emp3, LinkedId = trainingRecord1.Id });
            emp3 = Session.Load<Employee>(emp3.Id);

            var trainingRecord2 = GetEntityFactory<TrainingRecord>().Create(new { HeldOn = DateTime.Now.AddDays(-1), TrainingModule = refresherTrainingModule });
            GetFactory<TrainingRecordAttendedEmployeeFactory>().Create(new { Employee = emp4, LinkedId = trainingRecord2.Id });
            emp4 = Session.Load<Employee>(emp4.Id);

            var trainingRecord3 = GetEntityFactory<TrainingRecord>().Create(new { HeldOn = DateTime.Now.AddDays(-731), TrainingModule = initialTrainingModule });
            GetFactory<TrainingRecordAttendedEmployeeFactory>().Create(new { Employee = emp6, LinkedId = trainingRecord3.Id });
            var trainingRecord4 = GetEntityFactory<TrainingRecord>().Create(new { HeldOn = DateTime.Now.AddDays(-366), TrainingModule = refresherTrainingModule });
            GetFactory<TrainingRecordAttendedEmployeeFactory>().Create(new { Employee = emp6, LinkedId = trainingRecord4.Id });
            var trainingRecord5 = GetEntityFactory<TrainingRecord>().Create(new { HeldOn = DateTime.Now.AddDays(-366), TrainingModule = initialTrainingModule });
            GetFactory<TrainingRecordAttendedEmployeeFactory>().Create(new { Employee = emp7, LinkedId = trainingRecord5.Id });

            Session.Evict(trainingRecord1);
            trainingRecord1 = Session.Load<TrainingRecord>(trainingRecord1.Id);
            Session.Evict(trainingRecord2);
            trainingRecord2 = Session.Load<TrainingRecord>(trainingRecord2.Id);
            Session.Evict(trainingRecord3);
            trainingRecord3 = Session.Load<TrainingRecord>(trainingRecord3.Id);
            Session.Evict(trainingRecord4);
            trainingRecord4 = Session.Load<TrainingRecord>(trainingRecord4.Id);
            Session.Evict(trainingRecord5);
            trainingRecord5 = Session.Load<TrainingRecord>(trainingRecord5.Id);

            Assert.AreEqual(0, emp1.AttendedTrainingRecords.Count);
            Assert.AreEqual(1, trainingRecord1.EmployeesAttended.Count);
            Assert.AreEqual(1, trainingRecord2.EmployeesAttended.Count);
            Assert.AreEqual(1, trainingRecord3.EmployeesAttended.Count);
            Assert.AreEqual(1, trainingRecord4.EmployeesAttended.Count);

            emp1 = Session.Load<Employee>(emp1.Id);
            emp2 = Session.Load<Employee>(emp2.Id);
            emp3 = Session.Load<Employee>(emp3.Id);
            emp4 = Session.Load<Employee>(emp4.Id);
            emp5 = Session.Load<Employee>(emp5.Id);
            emp6 = Session.Load<Employee>(emp6.Id);

            Session.Flush();
            Session.Clear();

            var search = new SearchOperatingCenterTrainingSummary { OperatingCenter = new[] { opc1.Id }, TrainingRequirement = trainingRequirement.Id };

            var result = _target.Index(search);
            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(1, search.Count);
        }

        [TestMethod]
        public override void TestIndexRedirectsToSearchIfModelStateIsInvalid()
        {
            Assert.Inconclusive("Implement and test me");
        }

    }
}