using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Data;
using MMSINC.Testing.MSTest.TestExtensions;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class
        MeterChangeOutRepositoryTest : MapCallMvcInMemoryDatabaseTestBase<MeterChangeOut, MeterChangeOutRepository>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IMeterChangeOutStatusRepository>().Use<MeterChangeOutStatusRepository>();
        }

        [TestInitialize]
        public void InitializeTest()
        {
        }

        #endregion

        #region Tests

        [TestMethod]
        public void
            TestIsNewSerialNumberUniqueReturnsTrueIfThereAreNoOtherRecordsThatUseTheSameNewSerialNumberAsThePassedInMeterChangeOutRecord()
        {
            var expected = "12345";
            var entity = GetFactory<MeterChangeOutFactory>().Create(new {NewSerialNumber = expected});
            Session.Clear();
            var entityWithoutMatch =
                GetFactory<MeterChangeOutFactory>().Create(new {NewSerialNumber = "something else"});
            Assert.IsTrue(Repository.IsNewSerialNumberUnique(entity.Id, expected));
        }

        [TestMethod]
        public void
            TestIsNewSerialNumberUniqueReturnsFalseIfThereArOtherRecordsThatUseTheSameNewSerialNumberAsThePassedInMeterChangeOutRecord()
        {
            var expected = "12345";
            var entity = GetFactory<MeterChangeOutFactory>().Create(new {NewSerialNumber = expected});
            Session.Clear();
            var entityWithMatch = GetFactory<MeterChangeOutFactory>().Create(new {NewSerialNumber = expected});
            Assert.IsFalse(Repository.IsNewSerialNumberUnique(entity.Id, expected));

            // Also test that it's trimming the string.
            Assert.IsFalse(Repository.IsNewSerialNumberUnique(entity.Id, expected + "    "));
        }

        [TestMethod]
        public void TestGetCompletionsReport()
        {
            var now = DateTime.Now;
            var statuses = GetEntityFactory<MeterChangeOutStatus>().CreateList(10);
            var crew = GetEntityFactory<ContractorMeterCrew>().Create();
            for (var i = 0; i < 3; ++i)
            {
                GetEntityFactory<MeterChangeOut>().Create(new {
                    MeterChangeOutStatus = statuses[MeterChangeOutStatus.Indices.CHANGED - 1],
                    DateStatusChanged = now,
                    CalledInByContractorMeterCrew = crew
                });
                Session.Clear();
            }

            var results = Repository.GetCompletionsReport(new TestSearchMeterChangeOutCompletions()).ToList();

            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(crew.Id, results[0].ContractorMeterCrew.Id);
            Assert.AreEqual(3, results[0].Changed);
            // we want daily values, not time specific for the report
            MyAssert.AreClose(now.BeginningOfDay(), results[0].CompletionDate);
        }

        #region GetActiveMeterChangeOutsWithOutOfDateNewSerialNumber

        private ActiveMeterChangeOut CreateValidMeterChangeOutForGetActiveMeterChangeOutTest()
        {
            // See the repo method for documentation on what all the expected criteria are.

            // Create some meter change out statuses. The first three must be ignored, the last one
            // can be used for getting a valid match from the query.
            // There are only three invalid statuses, they're Id'ed 1-3, so we need four total.
            var mcoStatuses = GetFactory<MeterChangeOutStatusFactory>().CreateList(4);
            var serviceUtilityTypes = GetFactory<ServiceUtilityTypeFactory>().CreateList(30);
            var expectedPremiseNumber = "12345";
            var meterSize = GetFactory<ServiceSizeFactory>().Create(new {Size = (decimal)2.0});
            var premise = GetFactory<PremiseFactory>().Create(new {
                PremiseNumber = expectedPremiseNumber,
                MeterSize = meterSize,
                MeterSerialNumber = "Not empty or null",
                ServiceUtilityType = serviceUtilityTypes.Single(x => x.Id == ServiceUtilityType.Indices.DOMESTIC_WATER)
            });
            var contract = GetFactory<MeterChangeOutContractFactory>().Create(new {IsActive = true});
            var mco = GetFactory<MeterChangeOutFactory>().Create(new {
                PremiseNumber = expectedPremiseNumber,
                Contract = contract,
                NewSerialNumber = "12345",
                MeterChangeOutStatus =
                    mcoStatuses.Single(x =>
                        x.Id == 4) // 4 is nothing in particular and is a magic number for this test.
            });

            return new ActiveMeterChangeOut {
                MeterChangeOut = mco,
                Premise = premise,
                Statuses = mcoStatuses,
                ServiceUtilityTypes = serviceUtilityTypes
            };
        }

        private void AssertNoMatchingMeterChangeOut()
        {
            var result = Repository.GetActiveMeterChangeOutsWithOutOfDateNewSerialNumber();
            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public void
            TestGetActiveMeterChangeOutsWithOutOfDateNewSerialNumberReturnsMeterChangeOutWhenAllExpectedCriteriaAreMet()
        {
            var mco = CreateValidMeterChangeOutForGetActiveMeterChangeOutTest();
            var result = Repository.GetActiveMeterChangeOutsWithOutOfDateNewSerialNumber().Single();
            Assert.AreSame(mco.MeterChangeOut, result);
        }

        [TestMethod]
        public void TestGetActiveMeterChangeOutsWithOutOfDateNewSerialNumber_DoesNotReturnWhenContractIsNotActive()
        {
            var mco = CreateValidMeterChangeOutForGetActiveMeterChangeOutTest();
            mco.MeterChangeOut.Contract.IsActive = false;
            Session.Save(mco.MeterChangeOut.Contract);
            Session.Flush();
            AssertNoMatchingMeterChangeOut();
        }

        [TestMethod]
        public void
            TestGetActiveMeterChangeOutsWithOutOfDateNewSerialNumber_DoesNotReturnWhenAMatchingPremiseNumberIsNotFound()
        {
            var mco = CreateValidMeterChangeOutForGetActiveMeterChangeOutTest();
            mco.Premise.PremiseNumber = "Something else";
            Session.Save(mco.Premise);
            Session.Flush();
            AssertNoMatchingMeterChangeOut();
        }

        [TestMethod]
        public void
            TestGetActiveMeterChangeOutsWithOutOfDateNewSerialNumber_DoesNotReturnWhenPremiseMeterSizeIsGreaterThan2()
        {
            var mco = CreateValidMeterChangeOutForGetActiveMeterChangeOutTest();
            mco.Premise.MeterSize.Size = (decimal)2.0001;
            Session.Save(mco.Premise.MeterSize);
            Session.Flush();
            AssertNoMatchingMeterChangeOut();
        }

        [TestMethod]
        public void TestGetActiveMeterChangeOutsWithOutOfDateNewSerialNumber_DoesNotReturnWhenPremiseHasNullMeterSize()
        {
            var mco = CreateValidMeterChangeOutForGetActiveMeterChangeOutTest();
            mco.Premise.MeterSize = null;
            Session.Save(mco.Premise);
            Session.Flush();
            AssertNoMatchingMeterChangeOut();
        }

        [TestMethod]
        public void
            TestGetActiveMeterChangeOutsWithOutOfDateNewSerialNumber_DoesNotReturnWhenPremiseMeterSerialNumberIsNull()
        {
            var mco = CreateValidMeterChangeOutForGetActiveMeterChangeOutTest();
            mco.Premise.MeterSerialNumber = null;
            Session.Save(mco.Premise);
            Session.Flush();
            AssertNoMatchingMeterChangeOut();
        }

        [TestMethod]
        public void
            TestGetActiveMeterChangeOutsWithOutOfDateNewSerialNumber_DoesNotReturnWhenPremiseMeterSerialNumberIsEmptyString()
        {
            var mco = CreateValidMeterChangeOutForGetActiveMeterChangeOutTest();
            mco.Premise.MeterSerialNumber = string.Empty;
            Session.Save(mco.Premise);
            Session.Flush();
            AssertNoMatchingMeterChangeOut();
        }

        [TestMethod]
        public void
            TestGetActiveMeterChangeOutsWithOutOfDateNewSerialNumber_DoesNotReturnWhenMeterChangeOutNewSerialNumberIsNull()
        {
            var mco = CreateValidMeterChangeOutForGetActiveMeterChangeOutTest();
            mco.Premise.MeterSerialNumber = "anything";
            mco.MeterChangeOut.NewSerialNumber = null;
            Session.Save(mco.Premise);
            Session.Save(mco.MeterChangeOut);
            Session.Flush();
            AssertNoMatchingMeterChangeOut();
        }

        [TestMethod]
        public void
            TestGetActiveMeterChangeOutsWithOutOfDateNewSerialNumber_DoesNotReturnWhenMeterChangeOutNewSerialNumberIsEmptyString()
        {
            var mco = CreateValidMeterChangeOutForGetActiveMeterChangeOutTest();
            mco.Premise.MeterSerialNumber = "anything";
            mco.MeterChangeOut.NewSerialNumber = string.Empty;
            Session.Save(mco.Premise);
            Session.Save(mco.MeterChangeOut);
            Session.Flush();
            AssertNoMatchingMeterChangeOut();
        }

        [TestMethod]
        public void
            TestGetActiveMeterChangeOutsWithOutOfDateNewSerialNumber_DoesNotReturnWhenPremiseMeterSerialNumberIsTheSameAsMeterChangeOutNewSerialNumber()
        {
            var mco = CreateValidMeterChangeOutForGetActiveMeterChangeOutTest();
            mco.Premise.MeterSerialNumber = mco.MeterChangeOut.NewSerialNumber;
            Session.Save(mco.Premise);
            Session.Flush();
            AssertNoMatchingMeterChangeOut();
        }

        [TestMethod]
        public void
            TestGetActiveMeterChangeOutsWithOutOfDateNewSerialNumber_DoesNotReturnForIgnorableMeterChangeOutStatuses()
        {
            var mco = CreateValidMeterChangeOutForGetActiveMeterChangeOutTest();
            var invalidStatusIds = new[] {
                MeterChangeOutStatus.Indices.ALREADY_CHANGED,
                MeterChangeOutStatus.Indices.AW_TO_COMPLETE,
                MeterChangeOutStatus.Indices.CHANGED
            };
            var invalidStatuses = mco.Statuses.Where(x => invalidStatusIds.Contains(x.Id));
            Assert.AreEqual(3, invalidStatuses.Count(), "Sanity");
            foreach (var status in invalidStatuses)
            {
                mco.MeterChangeOut.MeterChangeOutStatus = status;
                Session.Save(mco.MeterChangeOut);
                Session.Flush();
                AssertNoMatchingMeterChangeOut();
            }

            var validStatuses = mco.Statuses.Except(invalidStatuses);
            Assert.IsTrue(validStatuses.Count() > 0);
            foreach (var status in validStatuses)
            {
                mco.MeterChangeOut.MeterChangeOutStatus = status;
                Session.Save(mco.MeterChangeOut);
                Session.Flush();
                var result = Repository.GetActiveMeterChangeOutsWithOutOfDateNewSerialNumber().Single();
                Assert.AreSame(mco.MeterChangeOut, result);
            }
        }

        [TestMethod]
        public void
            TestGetActiveMeterChangeOutsWithOutOfDateNewSerialNumber_DoesNotReturnWhenServiceTypeIsNotWaterOrFire()
        {
            var validServiceUtilityTypes = new[] {
                ServiceUtilityType.Indices.BULK_WATER,
                ServiceUtilityType.Indices.BULK_WATER_MASTER,
                ServiceUtilityType.Indices.DOMESTIC_WATER,
                ServiceUtilityType.Indices.PRIVATE_FIRE_SERVICE,
                ServiceUtilityType.Indices.PUBLIC_FIRE_SERVICE,
            };

            var mco = CreateValidMeterChangeOutForGetActiveMeterChangeOutTest();
            var validTypes = mco.ServiceUtilityTypes.Where(x => validServiceUtilityTypes.Contains(x.Id));
            Assert.AreEqual(validServiceUtilityTypes.Count(), validTypes.Count(), "Sanity");

            foreach (var type in validTypes)
            {
                mco.Premise.ServiceUtilityType = type;
                Session.Save(mco.Premise);
                Session.Flush();
                var result = Repository.GetActiveMeterChangeOutsWithOutOfDateNewSerialNumber().Single();
                Assert.AreSame(mco.MeterChangeOut, result);
            }

            var invalidTypes = mco.ServiceUtilityTypes.Except(validTypes);
            foreach (var type in invalidTypes)
            {
                mco.Premise.ServiceUtilityType = type;
                Session.Save(mco.Premise);
                Session.Flush();
                AssertNoMatchingMeterChangeOut();
            }
        }

        #endregion

        #endregion

        #region Helper classes

        private class TestSearchMeterChangeOutCompletions : SearchSet<MeterChangeOutCompletionReportItem>,
            ISearchMeterChangeOutCompletions
        {
            public int? CalledInByContractorMeterCrew { get; set; }
            public DateRange DateStatusChanged { get; set; }
            public int[] MeterChangeOutStatus { get; set; }
        }

        private class ActiveMeterChangeOut
        {
            public MeterChangeOut MeterChangeOut { get; set; }
            public Premise Premise { get; set; }
            public IEnumerable<MeterChangeOutStatus> Statuses { get; set; }
            public IEnumerable<ServiceUtilityType> ServiceUtilityTypes { get; set; }
        }

        #endregion
    }
}
