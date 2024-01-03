using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MapCallScheduler.JobHelpers.SapMaterial;
using MapCallScheduler.Library.Common;
using MapCallScheduler.Tests.Library.JobHelpers.Sap;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using StructureMap;

namespace MapCallScheduler.Tests.JobHelpers.SapMaterial
{
    [TestClass]
    public class SapMaterialFileParserTest : SapFileParserTestBase<SapMaterialFileParser, SapMaterialFileRecord>
    {
        #region Constants

        public const string SAMPLE_FILE = "TestData/sap_material_sample.txt";

        public static readonly SapMaterialFileRecord[] SAMPLE_FILE_RECORDS = {
            new SapMaterialFileRecord {
                PartNumber = "1404267",
                Plant = "D205",
                Description = "OBS*,CORP,BALL,TAPERXMIP,1(USE 1411875)",
                UnitOfMeasure = "EA",
                CreatedDate = new DateTime(2012, 07, 27),
                ChangedDate = new DateTime(2014, 08, 04),
                Cost = 17.58m,
                DeletionFlag = true
            },
            new SapMaterialFileRecord {
                PartNumber = "1404270",
                Plant = "D205",
                Description = "OBS*,CORP,BALL,TAPERXMIP,2(USE 1411876",
                UnitOfMeasure = "EA",
                CreatedDate = new DateTime(2012, 07, 27),
                ChangedDate = new DateTime(2014, 10, 21),
                Cost = 78.12m,
                DeletionFlag = true
            },
            new SapMaterialFileRecord {
                PartNumber = "1404273",
                Plant = "D205",
                Description = "OBS*,CORP,BAL,TAPERXMIP,3/4(USE 1411878)",
                UnitOfMeasure = "EA",
                CreatedDate = new DateTime(2012, 07, 27),
                ChangedDate = new DateTime(2014, 08, 04),
                Cost = 9.78m,
                DeletionFlag = true
            },
            new SapMaterialFileRecord {
                PartNumber = "1404276",
                Plant = "D205",
                Description = "CORP,BALL,NL,TAPERXMIP,1 1/2",
                UnitOfMeasure = "EA",
                CreatedDate = new DateTime(2012, 07, 27),
                ChangedDate = new DateTime(2014, 10, 01),
                Cost = 34.81m,
                DeletionFlag = false
            },
            new SapMaterialFileRecord {
                PartNumber = "1404277",
                Plant = "D205",
                Description = "OBS*,CORP,BALL,TAPERXCF,1(USE 1411850)",
                UnitOfMeasure = "EA",
                CreatedDate = new DateTime(2012, 07, 27),
                ChangedDate = new DateTime(2014, 10, 21),
                Cost = 22.36m,
                DeletionFlag = true
            },
            new SapMaterialFileRecord {
                PartNumber = "1404291",
                Plant = "D205",
                Description = "CORP,NL,BALL,TAPERXCC,1",
                UnitOfMeasure = "EA",
                CreatedDate = new DateTime(2012, 07, 27),
                ChangedDate = new DateTime(2015, 06, 16),
                Cost = 37.60m,
                DeletionFlag = false
            },
            new SapMaterialFileRecord {
                PartNumber = "1404322",
                Plant = "D205",
                Description = "VLV,NL,BALL,FIPXMF,LW,CHK,2",
                UnitOfMeasure = "EA",
                CreatedDate = new DateTime(2012, 07, 27),
                ChangedDate = new DateTime(2014, 09, 18),
                Cost = 85.56m,
                DeletionFlag = false
            },
            new SapMaterialFileRecord {
                PartNumber = "1404333",
                Plant = "D205",
                Description = "OBS*CURB STOP,BALL,,1 (USE 1411859)",
                UnitOfMeasure = "EA",
                CreatedDate = new DateTime(2012, 07, 27),
                ChangedDate = new DateTime(2014, 05, 16),
                Cost = 21.08m,
                DeletionFlag = true
            },
            new SapMaterialFileRecord {
                PartNumber = "1404336",
                Plant = "D205",
                Description = "OBS*,CURB STOP,BALL,3/4(USE 1411860)",
                UnitOfMeasure = "EA",
                CreatedDate = new DateTime(2012, 07, 27),
                ChangedDate = new DateTime(2014, 05, 16),
                Cost = 19.68m,
                DeletionFlag = true
            },
            new SapMaterialFileRecord {
                PartNumber = "1404348",
                Plant = "D205",
                Description = "OBS*,CURB STOP,BALL,CF,1(USE 1411865)",
                UnitOfMeasure = "EA",
                CreatedDate = new DateTime(2012, 07, 27),
                ChangedDate = new DateTime(2014, 08, 04),
                Cost = 33.69m,
                DeletionFlag = true
            },
            new SapMaterialFileRecord {
                PartNumber = "1404352",
                Plant = "D205",
                Description = "OBS*,CURB STOP,BALL,CF,3/4 (USE 1411868)",
                UnitOfMeasure = "EA",
                CreatedDate = new DateTime(2012, 07, 27),
                ChangedDate = new DateTime(2014, 12, 11),
                Cost = 18.77m,
                DeletionFlag = true
            },
            new SapMaterialFileRecord {
                PartNumber = "1404375",
                Plant = "D205",
                Description = "OBS*,CURB STOP,FIP,3/4 (USE 1411902)",
                UnitOfMeasure = "EA",
                CreatedDate = new DateTime(2012, 07, 27),
                ChangedDate = new DateTime(2014, 12, 11),
                Cost = 11.11m,
                DeletionFlag = true
            },
            new SapMaterialFileRecord {
                PartNumber = "1404382",
                Plant = "D205",
                Description = "OBS*CURB STOP,BALL,1(USE 1411861)",
                UnitOfMeasure = "EA",
                CreatedDate = new DateTime(2012, 07, 27),
                ChangedDate = new DateTime(2014, 09, 18),
                Cost = 33.13m,
                DeletionFlag = true
            },
            new SapMaterialFileRecord {
                PartNumber = "1404386",
                Plant = "D205",
                Description = "OBS*,CURB STOP,BALL,3/4 (USE 1411864)",
                UnitOfMeasure = "EA",
                CreatedDate = new DateTime(2012, 07, 27),
                ChangedDate = new DateTime(2014, 09, 18),
                Cost = 12.27m,
                DeletionFlag = true
            }
        };

        #endregion

        protected override string SampleFile => SAMPLE_FILE;
        protected override IEnumerable<SapMaterialFileRecord> SampleRecords => SAMPLE_FILE_RECORDS;

        protected override void CompareResult(SapMaterialFileRecord sampleRecord, SapMaterialFileRecord resultRecord)
        {
            MyAssert.AreEqual(sampleRecord, resultRecord, x => x.PartNumber);
            MyAssert.AreEqual(sampleRecord, resultRecord, x => x.Plant);
            MyAssert.AreEqual(sampleRecord, resultRecord, x => x.Description);
            MyAssert.AreEqual(sampleRecord, resultRecord, x => x.UnitOfMeasure);
            MyAssert.AreEqual(sampleRecord, resultRecord, x => x.CreatedDate);
            MyAssert.AreEqual(sampleRecord, resultRecord, x => x.ChangedDate);
            MyAssert.AreEqual(sampleRecord, resultRecord, x => x.Cost);
            MyAssert.AreEqual(sampleRecord, resultRecord, x => x.DeletionFlag);
        }

        [TestMethod]
        public void TestParserDoesNotChokeOnEmptyCostOrMissingDeletionFlagColumn()
        {
            var input =
                @"PartNumber        	Plant	Description                             	UnitOfMeasure	CreatedDate	ChangedDate	Cost         
1414780           	D205 	Changed Material desc                   	EA           	20161025   	20161104   	             
";

            var result = _target.Parse(new FileData(SAMPLE_FILE, input)).First();
            Assert.AreEqual("1414780", result.PartNumber);
            Assert.AreEqual("D205", result.Plant);
            Assert.AreEqual("Changed Material desc", result.Description);
            Assert.AreEqual("EA", result.UnitOfMeasure);
            Assert.AreEqual(new DateTime(2016, 10, 25), result.CreatedDate);
            Assert.AreEqual(new DateTime(2016, 11, 04), result.ChangedDate);
            Assert.AreEqual(null, result.Cost);
            Assert.AreEqual(false, result.DeletionFlag);
        }

        [TestMethod]
        public void TestParserDoesNotChokeOnAllZeroesForChangedDate()
        {
            var input =
                @"PartNumber        	Plant	Description                             	UnitOfMeasure	CreatedDate	ChangedDate	Cost         	DeletionFlag
1404267           	D205 	OBS*,CORP,BALL,TAPERXMIP,1(USE 1411875) 	EA           	20120727   	00000000   	17.58        	X
";

            var result = _target.Parse(new FileData(SAMPLE_FILE, input)).First();
            Assert.AreEqual("1404267", result.PartNumber);
            Assert.AreEqual("D205", result.Plant);
            Assert.AreEqual("OBS*,CORP,BALL,TAPERXMIP,1(USE 1411875)", result.Description);
            Assert.AreEqual("EA", result.UnitOfMeasure);
            Assert.AreEqual(new DateTime(2012, 07, 27), result.CreatedDate);
            Assert.AreEqual(new DateTime(2012, 07, 27), result.ChangedDate);
            Assert.AreEqual(17.58m, result.Cost);
            Assert.AreEqual(true, result.DeletionFlag);
        }
    }
}