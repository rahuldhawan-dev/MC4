using System;
using System.Collections.Generic;
using System.Linq;
using MapCallScheduler.JobHelpers.SapChemical;
using MapCallScheduler.Library.Common;
using MapCallScheduler.Tests.Library.JobHelpers.Sap;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallScheduler.Tests.JobHelpers.SapChemical
{
    [TestClass]
    public class SapChemicalFileParserTest : SapFileParserTestBase<SapChemicalFileParser, SapChemicalFileRecord>
    {
        #region Constants

        public const string SAMPLE_FILE = "TestData/sap_chemical_sample.txt";

        public static readonly SapChemicalFileRecord[] SAMPLE_FILE_RECORDS = {
            new SapChemicalFileRecord {
                PartNumber = "1200552",
                Plant = "P204",
                Name = "CHM,ALUM,SULFAT LIQUID,50%,BULK-Testing1",
                UnitOfMeasure = "LB",
                CreatedDate = new DateTime(2012, 07, 27),
                ChangedDate = new DateTime(2023, 01, 03),
                Cost = 0.11m,
                DeletionFlag = false
            },
            new SapChemicalFileRecord {
                PartNumber = "1200566",
                Plant = "P218",
                Name = "CHM,AMMONIA,AQUA,19%,BULK-Testing4567",
                UnitOfMeasure = "LB",
                CreatedDate = new DateTime(2012, 07, 27),
                ChangedDate = new DateTime(2023, 01, 02),
                Cost = 0.14m,
                DeletionFlag = false
            },
            new SapChemicalFileRecord {
                PartNumber = "1200631",
                Plant = "P219",
                Name = "CHM,LIME,HYDRATED CAO,72%,BULK-Testing1",
                UnitOfMeasure = "LB",
                CreatedDate = new DateTime(2012, 07, 27),
                ChangedDate = new DateTime(2023, 01, 02),
                Cost = 0.10m,
                DeletionFlag = false
            },
            new SapChemicalFileRecord {
                PartNumber = "1203071",
                Plant = "P203",
                Name = "CHM,ZINC,SULFATE,1:1,55GA DRUM-Testing11",
                UnitOfMeasure = "LB",
                CreatedDate = new DateTime(2021, 07, 14),
                ChangedDate = new DateTime(2023, 03, 06),
                Cost = 50.01m,
                DeletionFlag = false
            },
            new SapChemicalFileRecord {
                PartNumber = "1203221",
                Plant = "P203",
                Name = "CHM,ZINC,SULFATE,1:1,55GA DRUM",
                UnitOfMeasure = "LB",
                CreatedDate = new DateTime(2023, 02, 27),
                ChangedDate = null,
                Cost = null,
                DeletionFlag = false
            },
            new SapChemicalFileRecord {
                PartNumber = "1203231",
                Plant = "P203",
                Name = "Testing new material",
                UnitOfMeasure = "LB",
                CreatedDate = new DateTime(2023, 03, 06),
                ChangedDate = null,
                Cost = null,
                DeletionFlag = false
            }
        };

        #endregion

        protected override string SampleFile => SAMPLE_FILE;
        protected override IEnumerable<SapChemicalFileRecord> SampleRecords => SAMPLE_FILE_RECORDS;

        protected override void CompareResult(SapChemicalFileRecord sampleRecord, SapChemicalFileRecord resultRecord)
        {
            MyAssert.AreEqual(sampleRecord, resultRecord, x => x.PartNumber);
            MyAssert.AreEqual(sampleRecord, resultRecord, x => x.Plant);
            MyAssert.AreEqual(sampleRecord, resultRecord, x => x.Name);
            MyAssert.AreEqual(sampleRecord, resultRecord, x => x.UnitOfMeasure);
            MyAssert.AreEqual(sampleRecord, resultRecord, x => x.CreatedDate);
            MyAssert.AreEqual(sampleRecord, resultRecord, x => x.ChangedDate);
            MyAssert.AreEqual(sampleRecord, resultRecord, x => x.Cost);
            MyAssert.AreEqual(sampleRecord, resultRecord, x => x.DeletionFlag);
        }

        [TestMethod]
        public void TestParserDoesNotChokeOnEmptyCostOrMissingDeletionFlagColumn()
        {
            var input = @"PartNumber        	Plant	Name                   	UnitOfMeasure	CreatedDate	ChangedDate	Cost
1200542         	D205	Changed Chemical name	Pounds      	20230207  	20230209	";

            var result = _target.Parse(new FileData(SAMPLE_FILE, input)).First();
            Assert.AreEqual("1200542", result.PartNumber);
            Assert.AreEqual("D205", result.Plant);
            Assert.AreEqual("Changed Chemical name", result.Name);
            Assert.AreEqual("Pounds", result.UnitOfMeasure);
            Assert.AreEqual(new DateTime(2023, 02, 07), result.CreatedDate);
            Assert.AreEqual(new DateTime(2023, 02, 09), result.ChangedDate);
            Assert.AreEqual(null, result.Cost);
            Assert.AreEqual(false, result.DeletionFlag);
        }

        [TestMethod]
        public void TestParserDoesNotChokeOnAllZeroesForChangedDate()
        {
            var input = @"PartNumber        	Plant	Name                    	UnitOfMeasure	CreatedDate	ChangedDate	Cost
1200544         	D205	CHM,ALGICIDE,CUTRINE,55GA	Gallons      	20230207  	20230209	10.50";

            var result = _target.Parse(new FileData(SAMPLE_FILE, input)).First();
            Assert.AreEqual("1200544", result.PartNumber);
            Assert.AreEqual("D205", result.Plant);
            Assert.AreEqual("CHM,ALGICIDE,CUTRINE,55GA", result.Name);
            Assert.AreEqual("Gallons", result.UnitOfMeasure);
            Assert.AreEqual(new DateTime(2023, 02, 07), result.CreatedDate);
            Assert.AreEqual(new DateTime(2023, 02, 09), result.ChangedDate);
            Assert.AreEqual(10.50m, result.Cost);
            Assert.AreEqual(false, result.DeletionFlag);
        }

        [TestMethod]
        public void TestParserDoesNotChokeOnEmptyChangedDateAndDefaultsToCreatedDate()
        {
            var input = @"PartNumber        	Plant	Name                   	UnitOfMeasure	CreatedDate	ChangedDate	Cost
1200542           	D205 	Changed Chemical name  	Pounds       	20230207   	           	";

            var result = _target.Parse(new FileData(SAMPLE_FILE, input)).First();
            Assert.AreEqual("1200542", result.PartNumber);
            Assert.AreEqual("D205", result.Plant);
            Assert.AreEqual("Changed Chemical name", result.Name);
            Assert.AreEqual("Pounds", result.UnitOfMeasure);
            Assert.AreEqual(new DateTime(2023, 02, 07), result.CreatedDate);
            Assert.AreEqual(new DateTime(2023, 02, 07), result.ChangedDate);
            Assert.AreEqual(null, result.Cost);
            Assert.AreEqual(false, result.DeletionFlag);
        }
    }
}
