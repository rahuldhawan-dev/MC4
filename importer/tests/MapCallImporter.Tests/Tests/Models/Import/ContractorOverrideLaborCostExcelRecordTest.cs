using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import;
using MapCallImporter.SampleValues;
using MapCallImporter.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MapCallImporter.Tests.Models.Import
{
    [TestClass]
    public class ContractorOverrideLaborCostExcelRecordTest : ExcelRecordTestBase<ContractorOverrideLaborCost, MyCreateContractorOverrideLaborCost, ContractorOverrideLaborCostExcelRecord>
    {
        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            TestDataHelper.CreateStuffForContractorOverrideLaborCostInAberdeenNJ(_container);
        }

        #endregion

        protected override ContractorOverrideLaborCostExcelRecord CreateTarget()
        {
            return new ContractorOverrideLaborCostExcelRecord {
                Contractor = "Dave",
                StockNumber = "AQ100",
                Unit = "Thing1",
                Description = "Look after Thing2",
                OperatingCenterCode = OperatingCenters.NJ7.CODE,
                Cost = (decimal)100.00,
                EffectiveDate = DateTime.Parse("10/1/2023"),
                Percentage = 50,
                ContractorId = 1,
                ContractorLaborCostId = 7,
                OperatingCenterId = OperatingCenters.NJ7.ID
            };
        }

        protected override void InnerTestMappings(ExcelRecordMappingTester<ContractorOverrideLaborCost, MyCreateContractorOverrideLaborCost, ContractorOverrideLaborCostExcelRecord> test)
        {
            test.EntityRef(x => x.ContractorId, x => x.Contractor);
            test.EntityRef(x => x.ContractorLaborCostId, x => x.ContractorLaborCost);
            test.EntityRef(x => x.OperatingCenterId, x => x.OperatingCenter);
            
            test.Int(x => x.Percentage, x => x.Percentage);

            test.Decimal(x => x.Cost, x => x.Cost);

            test.DateTime(x => x.EffectiveDate, x => x.EffectiveDate);

            test.NotMapped(x => x.Contractor);
            test.NotMapped(x => x.StockNumber);
            test.NotMapped(x => x.Unit);
            test.NotMapped(x => x.Description);
            test.NotMapped(x => x.OperatingCenterCode);
        }

        [TestMethod]
        public void TestContractorMapsToContractorIfIdIsNull()
        {
            _target.ContractorId = null;
            WithUnitOfWork(uow => {
                var result = _target.MapToEntity(uow, 1, MappingHelper).Contractor;

                Assert.AreEqual(_target.Contractor, result.Name);
            });
        }

        [TestMethod]
        public void TestOperatingCenterCodeMapsToOperatingCenterContractorIfIdIsNull()
        {
            _target.OperatingCenterId = null;
            WithUnitOfWork(uow => {
                var result = _target.MapToEntity(uow, 1, MappingHelper).OperatingCenter;

                Assert.AreEqual(_target.OperatingCenterCode, result.OperatingCenterCode);
            });
        }

        [TestMethod]
        public void TestStockNumberUnitDescriptionMapsToContractorLaborCostIfIdIsNull()
        {
            _target.ContractorLaborCostId = null;
            WithUnitOfWork(uow => {
                var result = _target.MapToEntity(uow, 1, MappingHelper).ContractorLaborCost;

                Assert.AreEqual(_target.Description, result.JobDescription);
            });
        }
    }
}
