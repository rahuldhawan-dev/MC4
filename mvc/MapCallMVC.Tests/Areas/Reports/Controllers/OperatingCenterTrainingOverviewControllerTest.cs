using System;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Reports.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Reports.Controllers
{
    [TestClass]
    public class OperatingCenterTrainingOverviewControllerTest : MapCallMvcControllerTestBase<OperatingCenterTrainingOverviewController, OperatingCenter, OperatingCenterRepository>
    {
        #region Private Members
        
        private State _state;
        private SAPCompanyCode _sapCompanyCode;
        private PositionGroupCommonName _positionGroupCommonName;
        private PositionGroupCommonName _positionGroupCommonName2;
        private PositionGroup _positionGroup;
        private PositionGroup _positionGroup2;
        private OperatingCenter _opc1;
        private Employee _emp1;
        private Employee _emp2;
        private Employee _emp3;
        private Employee _emp4;
        private Employee _emp5;
        private Employee _emp6;
        private Employee _emp7;
        private TrainingRequirement _trainingRequirement;
        private TrainingModule _activeInitialTrainingModule;
        private TrainingModule _activeRecurringTrainingModule;
        private TrainingRecord _trainingRecord1;
        private TrainingRecord _trainingRecord2;
        private TrainingRecord _trainingRecord3;
        private TrainingRecord _trainingRecord4;
        private TrainingRecord _trainingRecord5;

        #endregion

        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.IndexDisplaysViewWhenNoResults = true;
        }

        private void SetupData()
        {
            _state = GetEntityFactory<State>().Create();
            _sapCompanyCode = GetEntityFactory<SAPCompanyCode>().Create(new { Description = "New Jersey-American Water" });
            _positionGroupCommonName = GetEntityFactory<PositionGroupCommonName>().Create(new { Description = "Production Maintenenace TCPA Non-Supv" });
            _positionGroupCommonName2 = GetEntityFactory<PositionGroupCommonName>().Create(new { Description = "Production Maintenenace TCPB Non-Supv" });
            _positionGroup = GetEntityFactory<PositionGroup>().Create(new
            {
                Group = "MNTMCH4",
                PositionDescription = "",
                BusinessUnit = "181801",
                BusinessUnitDescription = "MN-Production",
                SAPCompanyCode = _sapCompanyCode,
                State = _state,
                CommonName = _positionGroupCommonName
            });
            _positionGroup2 = GetEntityFactory<PositionGroup>().Create(new
            {
                Group = "MNTMCH3",
                PositionDescription = "",
                BusinessUnit = "181802",
                BusinessUnitDescription = "MN-Production",
                SAPCompanyCode = _sapCompanyCode,
                State = _state,
                CommonName = _positionGroupCommonName2
            });

            _opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury" });
            _emp1 = GetFactory<ActiveEmployeeFactory>().Create(new { OperatingCenter = _opc1, PositionGroup = _positionGroup });
            _emp2 = GetFactory<EmployeeFactory>().Create(new { OperatingCenter = _opc1, PositionGroup = _positionGroup });
            _emp3 = GetFactory<ActiveEmployeeFactory>().Create(new { OperatingCenter = _opc1, PositionGroup = _positionGroup });
            _emp4 = GetFactory<ActiveEmployeeFactory>().Create(new { OperatingCenter = _opc1, PositionGroup = _positionGroup });
            _emp5 = GetFactory<ActiveEmployeeFactory>().Create(new { OperatingCenter = _opc1, PositionGroup = _positionGroup2 });
            _emp6 = GetFactory<ActiveEmployeeFactory>().Create(new { OperatingCenter = _opc1, PositionGroup = _positionGroup });
            _emp7 = GetFactory<ActiveEmployeeFactory>().Create(new { OperatingCenter = _opc1, PositionGroup = _positionGroup });

            _opc1 = Repository.Find(_opc1.Id);

            _trainingRequirement = GetEntityFactory<TrainingRequirement>().Create(new { Description = "Arc Flash / Electrical Safety in the Work Place - Awareness", TrainingFrequency = 365, TrainingFrequencyUnit = "D", IsActive = true });
            _trainingRequirement.PositionGroupCommonNames.Add(_positionGroupCommonName);
            Session.Save(_trainingRequirement);
            _trainingRequirement = Session.Load<TrainingRequirement>(_trainingRequirement.Id);

            _activeInitialTrainingModule = GetEntityFactory<TrainingModule>().Create(new { Title = "Electrical Safety in the Workplace (OSHA: 1910.120) - Initial course", TrainingRequirement = _trainingRequirement });
            _activeRecurringTrainingModule = GetEntityFactory<TrainingModule>().Create(new { Title = "Electrical Safety in the Workplace (OSHA: 1910.120) - Refresher course", TrainingRequirement = _trainingRequirement });
            _trainingRequirement.ActiveInitialTrainingModule = _activeInitialTrainingModule;
            _trainingRequirement.ActiveRecurringTrainingModule = _activeRecurringTrainingModule;
            Session.Save(_trainingRequirement);

            _trainingRecord1 = GetEntityFactory<TrainingRecord>().Create(new { HeldOn = DateTime.Now.AddDays(-2), TrainingModule = _activeInitialTrainingModule });
            GetFactory<TrainingRecordAttendedEmployeeFactory>().Create(new { Employee = _emp3, LinkedId = _trainingRecord1.Id });
            _emp3 = Session.Load<Employee>(_emp3.Id);

            _trainingRecord2 = GetEntityFactory<TrainingRecord>().Create(new { HeldOn = DateTime.Now.AddDays(-1), TrainingModule = _activeRecurringTrainingModule });
            GetFactory<TrainingRecordAttendedEmployeeFactory>().Create(new { Employee = _emp4, LinkedId = _trainingRecord2.Id });
            _emp4 = Session.Load<Employee>(_emp4.Id);

            _trainingRecord3 = GetEntityFactory<TrainingRecord>().Create(new { HeldOn = DateTime.Now.AddDays(-731), TrainingModule = _activeInitialTrainingModule });
            GetFactory<TrainingRecordAttendedEmployeeFactory>().Create(new { Employee = _emp6, LinkedId = _trainingRecord3.Id });
            _trainingRecord4 = GetEntityFactory<TrainingRecord>().Create(new { HeldOn = DateTime.Now.AddDays(-366), TrainingModule = _activeRecurringTrainingModule });
            GetFactory<TrainingRecordAttendedEmployeeFactory>().Create(new { Employee = _emp6, LinkedId = _trainingRecord4.Id });
            _trainingRecord5 = GetEntityFactory<TrainingRecord>().Create(new { HeldOn = DateTime.Now.AddDays(-366), TrainingModule = _activeInitialTrainingModule });
            GetFactory<TrainingRecordAttendedEmployeeFactory>().Create(new { Employee = _emp7, LinkedId = _trainingRecord5.Id });

            Session.Evict(_trainingRecord1);
            _trainingRecord1 = Session.Load<TrainingRecord>(_trainingRecord1.Id);
            Session.Evict(_trainingRecord2);
            _trainingRecord2 = Session.Load<TrainingRecord>(_trainingRecord2.Id);
            Session.Evict(_trainingRecord3);
            _trainingRecord3 = Session.Load<TrainingRecord>(_trainingRecord3.Id);
            Session.Evict(_trainingRecord4);
            _trainingRecord4 = Session.Load<TrainingRecord>(_trainingRecord4.Id);
            Session.Evict(_trainingRecord5);
            _trainingRecord5 = Session.Load<TrainingRecord>(_trainingRecord5.Id);

            _emp1 = Session.Load<Employee>(_emp1.Id);
            _emp2 = Session.Load<Employee>(_emp2.Id);
            _emp3 = Session.Load<Employee>(_emp3.Id);
            _emp4 = Session.Load<Employee>(_emp4.Id);
            _emp5 = Session.Load<Employee>(_emp5.Id);
            _emp6 = Session.Load<Employee>(_emp6.Id);

            Session.Flush();
            Session.Clear();
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a =>
            {
                a.RequiresRole("~/Reports/OperatingCenterTrainingOverview/Search", RoleModules.OperationsTrainingRecords);
                a.RequiresRole("~/Reports/OperatingCenterTrainingOverview/Index", RoleModules.OperationsTrainingRecords);
            });
        }

        [TestMethod]
        public void TestIndexReturnsIndexWithZeroResultsViewWithModelFromSpecialRepositoryMethod()
        {
            var search = new SearchOperatingCenterTrainingOverview();

            var result = _target.Index(search);
            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(0, search.Count);
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because of the amount of data needed
            SetupData();
            
            var search = new SearchOperatingCenterTrainingOverview { State = _opc1.State.Id };

            var result = _target.Index(search);
            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(2, search.Count);
        }

        [TestMethod]
        public override void TestIndexRedirectsToSearchIfModelStateIsInvalid()
        {
            Assert.Inconclusive("Implement and test me");
        }

        [TestMethod]
        public void TestIndexReturnsExcelWithModelFromSpecialRepositoryMethodWhenExcel()
        {
            SetupData();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;
            var search = new SearchOperatingCenterTrainingOverview { State = _opc1.State.Id };

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual("NJ7 - Shrewsbury", "OperatingCenter");
                helper.AreEqual(_opc1.ToString(), "OperatingCenter", 1);
                helper.AreEqual(0, "Employees");
                helper.AreEqual(6, "Employees", 1);
            }
        }

        [TestMethod]
        public void TestIndexWithSearchWIthOSHAReturnsExcelWithModelFromSpecialRepositoryMethodWhenExcel()
        {
            SetupData();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;
            var search = new SearchOperatingCenterTrainingOverview { State = _opc1.State.Id, IsOSHARequirement = false };

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true, 2))
            {
                helper.AreEqual("NJ7 - Shrewsbury", "OperatingCenter");
                helper.AreEqual(_opc1.ToString(), "OperatingCenter", 1);
                helper.AreEqual(0, "Employees");
                helper.AreEqual(6, "Employees", 1);
            }
        }
    }
}