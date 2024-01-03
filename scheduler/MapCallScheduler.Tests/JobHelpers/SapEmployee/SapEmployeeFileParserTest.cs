using System;
using System.Collections.Generic;
using System.Linq;
using MapCallScheduler.JobHelpers.SapEmployee;
using MapCallScheduler.Library.Common;
using MapCallScheduler.Tests.Library.JobHelpers.Sap;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallScheduler.Tests.JobHelpers.SapEmployee
{
    [TestClass]
    public class SapEmployeeFileParserTest : SapFileParserTestBase<SapEmployeeFileParser, SapEmployeeFileRecord>
    {
        #region Constants

        public const string SAMPLE_FILE = "TestData/sap_employee_sample.csv";

        public static readonly SapEmployeeFileRecord[] SAMPLE_FILE_RECORDS = {
            new SapEmployeeFileRecord {
                Address = "3032 Fifth Street",
                Address2 = "Addy 2",
                //Birthdate = DateTime.Today,
                City = "City town",
                State = "NJ",
                DateHired = new DateTime(1984,6,16),
                EmailAddress = "some@address.com",
                EmployeeId = "3000002",
                FirstName = "Chicken",
                LastName = "McChicken",
                PersonnelAreaDescription = "PAD",
                PersonnelAreaId = 1234,
                PhoneHome = "9999999999",
                PositionGroupBusinessUnit = "P1-30MVA2-01-CM",
                PositionGroupBusinessUnitDescription = "PGBUDesc",
                PositionGroupCompanyCode = "PGCC",
                PositionGroupGroup = "45",
                PositionGroupKey = "11223344",
                PositionGroupPositionDescription = "PGPD",
                PositionGroupState = "NJ",
                ReportsTo = "54321",
                Status = "Active", 
                ZipCode = "07720"
            },
        };

        #endregion

        protected override string SampleFile => SAMPLE_FILE;
        protected override IEnumerable<SapEmployeeFileRecord> SampleRecords => SAMPLE_FILE_RECORDS;

        protected override void CompareResult(SapEmployeeFileRecord sampleRecord, SapEmployeeFileRecord resultRecord)
        {
            MyAssert.AreEqual(sampleRecord, resultRecord, x => x.Address);
            MyAssert.AreEqual(sampleRecord, resultRecord, x => x.Address2);
            MyAssert.AreEqual(sampleRecord, resultRecord, x => x.City);
            MyAssert.AreEqual(sampleRecord, resultRecord, x => x.DateHired);
            MyAssert.AreEqual(sampleRecord, resultRecord, x => x.EmailAddress);
            MyAssert.AreEqual(sampleRecord, resultRecord, x => x.EmployeeId);
            MyAssert.AreEqual(sampleRecord, resultRecord, x => x.FirstName);
            MyAssert.AreEqual(sampleRecord, resultRecord, x => x.LastName);
            MyAssert.AreEqual(sampleRecord, resultRecord, x => x.PersonnelAreaDescription);
            MyAssert.AreEqual(sampleRecord, resultRecord, x => x.PersonnelAreaId);
            MyAssert.AreEqual(sampleRecord, resultRecord, x => x.PhoneHome);
            MyAssert.AreEqual(sampleRecord, resultRecord, x => x.PositionGroupBusinessUnit);
            MyAssert.AreEqual(sampleRecord, resultRecord, x => x.PositionGroupBusinessUnitDescription);
            MyAssert.AreEqual(sampleRecord, resultRecord, x => x.PositionGroupCompanyCode);
            MyAssert.AreEqual(sampleRecord, resultRecord, x => x.PositionGroupGroup);
            MyAssert.AreEqual(sampleRecord, resultRecord, x => x.PositionGroupPositionDescription);
            MyAssert.AreEqual(sampleRecord, resultRecord, x => x.PositionGroupState);
            MyAssert.AreEqual(sampleRecord, resultRecord, x => x.PositionGroupKey);
            MyAssert.AreEqual(sampleRecord, resultRecord, x => x.ReportsTo);
            MyAssert.AreEqual(sampleRecord, resultRecord, x => x.State);
            MyAssert.AreEqual(sampleRecord, resultRecord, x => x.Status);
            MyAssert.AreEqual(sampleRecord, resultRecord, x => x.ZipCode);
        }

        [TestMethod]
        public void TestParserCanParseAllTheThings()
        {
            var input =
                @"""CompanyCode"",""CostCenter"",""CostCenterDesc"",""PersNo"",""Lastname"",""Firstname"",""POSITION_ID"",""Position"",""PSgroup"",""PositionGroupState"",""OriginalHireDate"",""EmploymentStatus"",""PersonnelAreaId"",""PersonnelAreaDesc"",""ManagerID"",""AWHireDate"",""AddressLine1"",""AddressLine2"",""City_Country"",""State"",""ZIPCode"",""Telephone"",""EmailId"",""ReportingLocation"",""LocalEmployeeRelationsBusinessPartner"",""LocalEmployeeRelationsBusinessPartnerCell"",""EmployeeCell""
""PGCC"",""P1-30MVA2-01-CM"",""PGBUDesc"",""03000002"",""McChicken"",""Chicken"",""11223344"",""PGPD"",""45"",""NJ"",""06/16/1984"",""Active"",""1234"",""PAD"",""0054321"",""06/16/1984"",""3032 Fifth Street"",""Addy 2"",""City town"",""NJ"",""07720"",""9999999999"",""some@address.com"","""",""012345"",""9999999999"",""8888888888""";
            var result = _target.Parse(new FileData(SAMPLE_FILE, input)).First();
            Assert.AreEqual("PGCC", result.PositionGroupCompanyCode);
            Assert.AreEqual("P1-30MVA2-01-CM", result.PositionGroupBusinessUnit);
            Assert.AreEqual("PGBUDesc", result.PositionGroupBusinessUnitDescription);
            Assert.AreEqual("3000002", result.EmployeeId);
            Assert.AreEqual("McChicken", result.LastName);
            Assert.AreEqual("Chicken", result.FirstName);
            Assert.AreEqual("11223344", result.PositionGroupKey);
            Assert.AreEqual("PGPD", result.PositionGroupPositionDescription);
            Assert.AreEqual("45", result.PositionGroupGroup);
            Assert.AreEqual("NJ", result.PositionGroupState);
            Assert.AreEqual(new DateTime(1984,6,16), result.DateHired);
            Assert.AreEqual("Active", result.Status);
            Assert.AreEqual(1234, result.PersonnelAreaId);
            Assert.AreEqual("PAD", result.PersonnelAreaDescription);
            Assert.AreEqual("54321", result.ReportsTo, "Padded zeros should be stripped from this");
            Assert.AreEqual("3032 Fifth Street", result.Address);
            Assert.AreEqual("Addy 2", result.Address2);
            Assert.AreEqual("City town", result.City);
            Assert.AreEqual("NJ", result.State);
            Assert.AreEqual("07720", result.ZipCode);
            Assert.AreEqual("9999999999", result.PhoneHome);
            Assert.AreEqual("some@address.com", result.EmailAddress);
            Assert.AreEqual(string.Empty, result.ReportingLocation);
            Assert.AreEqual("12345", result.LocalEmployeeRelationsBusinessPartner);
            Assert.AreEqual("9999999999", result.LocalEmployeeRelationsBusinessPartnerCell);
            Assert.AreEqual("8888888888", result.PhoneCellular);
        }
    }
}