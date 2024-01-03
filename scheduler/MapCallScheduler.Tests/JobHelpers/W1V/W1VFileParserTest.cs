using System;
using System.Linq;
using MapCallScheduler.JobHelpers.W1V;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallScheduler.Tests.JobHelpers.W1V
{
    [TestClass]
    public class W1VFileParserTest
    {
        public const string CUSTOMER_MATERIAL_SAMPLE =
            @"work_order_number,status,installation,premise_id,premise_address,meter_serial_number,meterdevicecategory,meter_size,customersidematerial,functional_location,readingdevicedirectionallocation,readingdevicepositionallocation,readingdevicesupplementallocation,metersupplementallocation,meterpositionlocation,meterdirectionallocation,last_updated,assignment_start,assignment_end,technicalinspectedon
523221730,COMP,7001809875,9520063577,""96, W BATTLE RD, Princeton, NJ, 08540"",14340613,1104572,""1"""""",Copper,6003363869,FL-Front Left Side,1A-Cellar/Basement,IS-Inside,IS-Inside,1A-Cellar/Basement,FL-Front Left Side,9/13/2022 13:06,9/13/2022 8:09,9/13/2022 9:06,20220913
523276138,INCM,7001076251,9350245023,""243, Runyon Ave, St Louis, MO, 63125-1155"",84256127,1101512,""5/8"""""",,6001443286,FS-Front Right Side,7A-Wall,OS-Outside,IS-Inside,1A-Cellar/Basement,FS-Front Right Side,9/13/2022 17:04,9/13/2022 11:34,9/13/2022 12:04,20220913
523330746,COMP,7000963410,9350043705,""1120, Basswood Ln, St Louis, MO, 63132-3008"",82978059,1102951,""5/8"""""",,6001054739,FL-Front Left Side,1A-Cellar/Basement,IS-Inside,IS-Inside,1A-Cellar/Basement,FL-Front Left Side,9/13/2022 14:55,9/13/2022 8:59,9/13/2022 9:55,20220913
523333088,INCM,7000999737,9350168540,""2504, Normandy Ave, St Louis, MO, 63121-4703"",84572868,1101512,""5/8"""""",,6001231099,FL-Front Left Side,1A-Cellar/Basement,IS-Inside,IS-Inside,1A-Cellar/Basement,FL-Front Left Side,9/13/2022 18:35,9/13/2022 13:01,9/13/2022 13:35,20220913
523413708,COMP,7002600011,9180787767,""27, Williams St, Lakewood, NJ, 08701"",90201380,1104567,""5/8"""""",,6002369366,FS-Front Right Side,4A-Curb,OS-Outside,OS-Outside,4A-Curb,FS-Front Right Side,9/13/2022 16:40,9/13/2022 12:16,9/13/2022 12:40,20220913";
        
        private W1VFileParser _target;

        [TestInitialize]
        public void TestInitialize()
        {
            _target = new W1VFileParser();
        }

        [TestMethod]
        public void Test_ParseCustomerMaterial_ParsesCustomerMaterialRecords()
        {
            var results = _target.ParseCustomerMaterial(CUSTOMER_MATERIAL_SAMPLE).ToArray();

            // has customer side material
            var result = results[0];
            
            Assert.AreEqual(523221730, result.WorkOrderNumber);
            Assert.AreEqual("9520063577", result.PremiseId);
            Assert.AreEqual("1\"", result.MeterSize);
            Assert.AreEqual("Copper", result.CustomerSideMaterial);
            Assert.AreEqual("1A-Cellar/Basement", result.ReadingDevicePositionalLocation);
            Assert.AreEqual(new DateTime(2022, 9, 13, 8, 9, 0), result.AssignmentStart);
            Assert.AreEqual("20220913", result.TechnicalInspectedOn);
            Assert.AreEqual("7001809875", result.Installation);
            Assert.AreEqual("6003363869", result.FunctionalLocation);

            result = results[3];
            
            Assert.AreEqual(523333088, result.WorkOrderNumber);
            Assert.AreEqual("9350168540", result.PremiseId);
            Assert.AreEqual("5/8\"", result.MeterSize);
            Assert.AreEqual("", result.CustomerSideMaterial);
            Assert.AreEqual("1A-Cellar/Basement", result.ReadingDevicePositionalLocation);
            Assert.AreEqual(new DateTime(2022, 9, 13, 13, 1, 0), result.AssignmentStart);
            Assert.AreEqual("20220913", result.TechnicalInspectedOn);
            Assert.AreEqual("7000999737", result.Installation);
            Assert.AreEqual("6001231099", result.FunctionalLocation);
        }
    }
}
