using System;
using System.Linq;

using Contractors.Data.Models.Repositories;
using Contractors.Data.Models.ViewModels;
using Contractors.Tests.Controllers;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Data;
using MMSINC.Testing.MSTest.TestExtensions;
using StructureMap;

namespace Contractors.Tests.Models.Repositories
{
    [TestClass]
    public class MeterChangeOutRepositoryTest : ContractorsControllerTestBase<MeterChangeOut, MeterChangeOutRepository>
    {
        #region Tests

        [TestMethod]
        public void TestLinqFiltersByContractor()
        {
            var contractor1 = GetFactory<ContractorFactory>().Create();
            var contractor2 = GetFactory<ContractorFactory>().Create();
            var contract1 = GetFactory<MeterChangeOutContractFactory>().Create(new { Contractor = contractor1 });
            var contract2 = GetFactory<MeterChangeOutContractFactory>().Create(new { Contractor = contractor2 });
            var mco1 = GetFactory<MeterChangeOutFactory>().Create(new { Contract = contract1 });
            var mco2 = GetFactory<MeterChangeOutFactory>().Create(new { Contract = contract2 });
            _currentUser.Contractor = contractor1;

            var result = Repository.GetAll().Single();
            Assert.AreSame(mco1, result);
        }

        [TestMethod]
        public void TestCriteriaFiltersByContractor()
        {
            var contractor1 = GetFactory<ContractorFactory>().Create();
            var contractor2 = GetFactory<ContractorFactory>().Create();
            var contract1 = GetFactory<MeterChangeOutContractFactory>().Create(new { Contractor = contractor1 });
            var contract2 = GetFactory<MeterChangeOutContractFactory>().Create(new { Contractor = contractor2 });
            var mco1 = GetFactory<MeterChangeOutFactory>().Create(new { Contract = contract1 });
            var mco2 = GetFactory<MeterChangeOutFactory>().Create(new { Contract = contract2 });
            _currentUser.Contractor = contractor1;

            var result = Repository.Search(new EmptySearchSet<MeterChangeOut>()).Single();
            Assert.AreSame(mco1, result);
        }


        [TestMethod]
        public void TestIsNewSerialNumberUniqueReturnsTrueIfThereAreNoOtherRecordsThatUseTheSameNewSerialNumberAsThePassedInMeterChangeOutRecord()
        {
            var expected = "12345";
            var entity = GetFactory<MeterChangeOutFactory>().Create(new { NewSerialNumber = expected });
            var entityWithoutMatch = GetFactory<MeterChangeOutFactory>().Create(new { NewSerialNumber = "something else" });
            Assert.IsTrue(Repository.IsNewSerialNumberUnique(entity.Id, expected));
        }

        [TestMethod]
        public void TestIsNewSerialNumberUniqueReturnsFalseIfThereArOtherRecordsThatUseTheSameNewSerialNumberAsThePassedInMeterChangeOutRecord()
        {
            var expected = "12345";
            var entity = GetFactory<MeterChangeOutFactory>().Create(new { NewSerialNumber = expected });
            var entityWithMatch = GetFactory<MeterChangeOutFactory>().Create(new { NewSerialNumber = expected });
            Assert.IsFalse(Repository.IsNewSerialNumberUnique(entity.Id, expected));

            // Also test that it's trimming the string.
            Assert.IsFalse(Repository.IsNewSerialNumberUnique(entity.Id, expected + "    "));
        }

        [TestMethod]
        public void TestGetCompletionsReport()
        {
            var now = DateTime.Now;
            var contract = GetEntityFactory<MeterChangeOutContract>().Create(new { Contractor = _currentUser.Contractor });
            var statuses = GetEntityFactory<MeterChangeOutStatus>().CreateList(10);
            var crew = GetEntityFactory<ContractorMeterCrew>().Create();
            var changeouts = GetEntityFactory<MeterChangeOut>().CreateList(3, new
            {
                MeterChangeOutStatus = statuses[MeterChangeOutStatus.Indices.CHANGED - 1],
                DateStatusChanged = now,
                CalledInByContractorMeterCrew = crew,
                Contract = contract 
            });

            var contractForDifferentContractor = GetEntityFactory<MeterChangeOutContract>().Create();
            var changeoutsBad = GetEntityFactory<MeterChangeOut>().CreateList(3, new
            {
                MeterChangeOutStatus = statuses[MeterChangeOutStatus.Indices.CHANGED - 1],
                DateStatusChanged = now,
                CalledInByContractorMeterCrew = crew,
                Contract = contractForDifferentContractor
            });

            var results = Repository.GetCompletionsReport(new TestSearchMeterChangeOutCompletions()).ToList();

            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(crew, results[0].ContractorMeterCrew);
            Assert.AreEqual(3, results[0].Changed);
            // we want daily values, not time specific for the report
            MyAssert.AreClose(now.BeginningOfDay(), results[0].CompletionDate);

        }

        #endregion

        private class TestSearchMeterChangeOutCompletions : SearchSet<MeterChangeOutCompletionReportItem>, ISearchMeterChangeOutCompletions
        {
            public int? CalledInByContractorMeterCrew { get; set; }
            public DateRange DateStatusChanged { get; set; }
            public int[] MeterChangeOutStatus { get; set; }
        }
    }
}