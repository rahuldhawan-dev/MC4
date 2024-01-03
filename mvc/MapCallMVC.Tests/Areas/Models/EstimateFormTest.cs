using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Areas.ProjectManagement.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.Models
{
    [TestClass]
    public class EstimateFormTest : MapCallMvcInMemoryDatabaseTestBase<EstimatingProject>
    {
        #region Init/Cleanup

        private DateTime _now;
        private EstimateForm _target;

        [TestInitialize]
        public void TestInitialize()
        {
            _now = DateTime.Now;
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(_now);
            _container.Inject(dateTimeProvider.Object);

            var target = GetEntityFactory<EstimatingProject>().Create(new
            {
                ContingencyPercentage = 10,
                OverheadPercentage = 11,
                LumpSum = 12m
            });

            GetEntityFactory<EstimatingProjectMaterial>().CreateList(2, new
            {
                EstimatingProject = target,
                Quantity = 1,
            });

            GetEntityFactory<EstimatingProjectCompanyLaborCost>().CreateList(2, new
            {
                EstimatingProject = target,
                Quantity = 2
            });

            GetEntityFactory<EstimatingProjectContractorLaborCost>().CreateList(2, new
            {
                EstimatingProject = target,
                Quantity = 3
            });

            GetEntityFactory<EstimatingProjectPermit>().CreateList(2, new
            {
                EstimatingProject = target,
                Quantity = 4,
                Cost = 6.66m
            });

            GetEntityFactory<EstimatingProjectOtherCost>().CreateList(2, new
            {
                Cost = 6.66m,
                Quantity = 5,
                Description = "vOv",
                EstimatingProject = target
            });

            Session.Clear();

            _target = _viewModelFactory.Build<EstimateForm, EstimatingProject>(
                Session.Load<EstimatingProject>(target.Id));
        }

        #endregion

        #region Summary Fields

        [TestMethod]
        public void Test_AdminInstallCost_IsTheSumOf_ContractorLaborCosts_And_Permits_And_OtherCosts_And_CompanyLabor_And_LumpSum()
        {
            var expected = _target.Project.TotalContractorLaborCost + _target.Project.TotalPermitCost +
                           _target.Project.TotalOtherCost + _target.Project.LumpSum +
                           _target.Project.TotalCompanyLaborCost;

            Assert.AreNotEqual(0, expected, "sanity check");
            Assert.AreEqual(expected, _target.AdminInstallCost);
        }

        [TestMethod]
        public void Test_AdminInstallCost_DoesNotHaveAFreakOutBecauseLumpSumIsNull()
        {
            // NOTE: You can delete this test if LumpSum stops being nullable.
            _target.Project.LumpSum = null;
            var expected = _target.Project.TotalContractorLaborCost + _target.Project.TotalPermitCost +
                           _target.Project.TotalOtherCost + _target.Project.TotalCompanyLaborCost;

            Assert.AreNotEqual(0, expected, "sanity check");
            Assert.AreEqual(expected, _target.AdminInstallCost);
        }

        [TestMethod]
        public void Test_AdminInstallAndMaterialCost_IsSumOf_AdminInstallCost_And_TotalMaterialCost()
        {
            var expected = _target.AdminInstallCost + _target.Project.TotalMaterialCost;
            Assert.AreNotEqual(0, expected, "sanity check");
            Assert.AreEqual(expected, _target.AdminInstallAndMaterialCost);
        }

        [TestMethod]
        public void Test_OmmissionsAndContingenciesReturnsEquals_ContingencyPercentageAsDecimal_MultipliedBy_AdminInstallAndMaterialCost()
        {
            _target.Project.ContingencyPercentage = 15;
            var expected = _target.AdminInstallAndMaterialCost*_target.Project.ContingencyPercentageAsDecimal;
            Assert.AreEqual(expected, _target.OmmissionsAndContingencies);
        }

        //[TestMethod]
        //public void Test_OmmissionsAndContingencies_ReturnsZeroIfContingencyPercentageIsNull()
        //{
        //    // NOTE: You can delete this test if ContingencyPercentage stops being nullable.
        //    _target.Project.ContingencyPercentage = null;

        //    Assert.AreEqual(0, _target.OmmissionsAndContingencies);
        //}

        [TestMethod]
        public void Test_TotalDirectCost_IsSumOf_AdminInstallAndMaterialCost_And_OmmissionsAndContingencies()
        {
            var expected = _target.AdminInstallAndMaterialCost + _target.OmmissionsAndContingencies;
            Assert.AreEqual(expected, _target.TotalDirectCost);
        }

        [TestMethod]
        public void Test_ConstructionOverheadCost_Equals_TotalDirectCost_MultipliedBy_OverheadPercentageAsDecimal()
        {
            _target.Project.OverheadPercentage = 15;
            var expected = _target.TotalDirectCost*_target.Project.OverheadPercentageAsDecimal;
            Assert.AreEqual(expected, _target.ConstructionOverheadCost);
        }

        [TestMethod]
        public void Test_SubTotal_IsSumOf_TotalDirectCost_And_ConstructionOverheadCost()
        {
            var expected = _target.TotalDirectCost + _target.ConstructionOverheadCost;
            Assert.AreEqual(expected, _target.SubTotal);
        }

        [TestMethod]
        public void Test_TotalEstimatedCost_Equals_SubTotal()
        {
            Assert.AreEqual(_target.SubTotal, _target.TotalEstimatedCost);
        }
        

        #endregion
    }
}
