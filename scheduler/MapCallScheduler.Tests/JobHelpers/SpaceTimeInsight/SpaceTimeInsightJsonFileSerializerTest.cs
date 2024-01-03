using System;
using System.Collections.Generic;
using Historian.Data.Client.Entities;
using MapCall.Common.Model.Entities;
using MapCallScheduler.JobHelpers.SpaceTimeInsight;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace MapCallScheduler.Tests.JobHelpers.SpaceTimeInsight
{
    [TestClass]
    public class SpaceTimeInsightJsonFileSerializerTest
    {
        #region Constants

        #region WorkOrders

        public static readonly WorkOrder[] WORK_ORDER_SAMPLE_INPUT = {
            new WorkOrder {
                Id = 194061,
                WorkDescription = new WorkDescription {Description = "LEAK IN METER BOX, OUTLET"},
                DateCompleted = new DateTime(2014, 06, 02),
                SAPWorkOrderNumber = 90154580,
                SAPNotificationNumber = 12564778,
                LostWater = 200,
                Longitude = -90.3262208380223m,
                Latitude = 38.5664494943542m
            },
            new WorkOrder {
                Id = 196853,
                WorkDescription = new WorkDescription {Description = "SERVICE LINE RETIRE"},
                DateCompleted = new DateTime(2014, 08, 13),
                SAPWorkOrderNumber = 90244325,
                SAPNotificationNumber = 306113203,
                LostWater = 380,
                Longitude = -90.3229060313326m,
                Latitude = 38.6386819224579m
            },
        };

        public const string WORK_ORDER_SAMPLE_OUTPUT = @"{
  ""DataType"": ""WORKORDER_DATA"",
  ""SourceSystem"": ""MAPCALL"",
  ""WorkOrders"": [
    {
      ""ID"": ""194061"",
      ""Description"": ""LEAK IN METER BOX, OUTLET"",
      ""DateCompleted"": ""2014-06-02"",
      ""SAPWorkorderNum"": 90154580,
      ""SAPNotifNum"": 12564778,
      ""LostWater"": 200,
      ""LostWaterUnitOfMeasure"": ""CGL"",
      ""Geometry"": ""POINT(-90.3262208380223 38.5664494943542)""
    },
    {
      ""ID"": ""196853"",
      ""Description"": ""SERVICE LINE RETIRE"",
      ""DateCompleted"": ""2014-08-13"",
      ""SAPWorkorderNum"": 90244325,
      ""SAPNotifNum"": 306113203,
      ""LostWater"": 380,
      ""LostWaterUnitOfMeasure"": ""CGL"",
      ""Geometry"": ""POINT(-90.3229060313326 38.6386819224579)""
    }
  ]
}";

        #endregion

        #region MainBreaks

        public static readonly MainBreak[] MAIN_BREAK_SAMPLE_INPUT = {
            new MainBreak {
                Id = 1,
                MainFailureType = new MainFailureType {Description = "BURST"},
                WorkOrder = new WorkOrder {
                    CreatedAt = new DateTime(2000, 03, 01),
                    DateCompleted = new DateTime(2000, 02, 28),
                    EstimatedCustomerImpact = new CustomerImpactRange {Description = "100", Id = 2},
                    Longitude = -90.2587003731367m,
                    Latitude = 38.7294452111913m
                }
            },
            new MainBreak {
                Id = 2,
                MainFailureType = new MainFailureType {Description = "BURST"},
                WorkOrder = new WorkOrder {
                    CreatedAt = new DateTime(2000, 04, 01),
                    DateCompleted = new DateTime(2000, 03, 01),
                    EstimatedCustomerImpact = new CustomerImpactRange {Description = "50", Id = 1},
                    Longitude = -90.4039043556992m,
                    Latitude = 38.5584695905716m
                }
            },
        };

        public const string MAIN_BREAK_SAMPLE_OUTPUT = @"{
  ""DataType"": ""MAINBREAK_DATA"",
  ""SourceSystem"": ""MAPCALL"",
  ""MainBreaks"": [
    {
      ""ID"": ""0001"",
      ""MainBreakType"": ""BURST"",
      ""CreatedDate"": ""2000-03-01"",
      ""RepairDate"": ""2000-02-28"",
      ""AcquiredYear"": """",
      ""LeakDate"": ""2000-03-01"",
      ""CustomerImpacted"": 2,
      ""Geometry"": ""POINT(-90.2587003731367 38.7294452111913)""
    },
    {
      ""ID"": ""0002"",
      ""MainBreakType"": ""BURST"",
      ""CreatedDate"": ""2000-04-01"",
      ""RepairDate"": ""2000-03-01"",
      ""AcquiredYear"": """",
      ""LeakDate"": ""2000-04-01"",
      ""CustomerImpacted"": 1,
      ""Geometry"": ""POINT(-90.4039043556992 38.5584695905716)""
    }
  ]
}";

        #endregion

        #region HydrantInspections

        public static readonly HydrantInspection[] HYDRANT_INSPECTION_SAMPLE_INPUT = {
            new HydrantInspection {
                Hydrant = new Hydrant {
                    HydrantNumber = "HUB-101"
                },
                HydrantInspectionType = new HydrantInspectionType {
                    Description = "INSPECT"
                },
                DateInspected = new DateTime(2015, 09, 24),
                GallonsFlowed = 200,
                MinutesFlowed = 15,
                GPM = 50
            },
            new HydrantInspection {
                Hydrant = new Hydrant {
                    HydrantNumber = "HUB-205"
                },
                DateInspected = new DateTime(2015, 09, 24),
                GallonsFlowed = 200,
                MinutesFlowed = 10,
                GPM = 70
            }
        };

        public const string HYDRANT_INSPECTION_SAMPLE_OUTPUT = @"{
  ""DataType"": ""INSPECTION_DATA"",
  ""SourceSystem"": ""MAPCALL"",
  ""InspectionData"": [
    {
      ""HydrantNumber"": ""HUB-101"",
      ""InspectionType"": ""INSPECT"",
      ""DateInspected"": ""2015-09-24"",
      ""GallonsFlowed"": 200,
      ""MinutesFlowed"": 15.0,
      ""GPM"": 50.0
    },
    {
      ""HydrantNumber"": ""HUB-205"",
      ""InspectionType"": """",
      ""DateInspected"": ""2015-09-24"",
      ""GallonsFlowed"": 200,
      ""MinutesFlowed"": 10.0,
      ""GPM"": 70.0
    }
  ]
}";

        #endregion

        #region Interconnects

        public static readonly RawData[] INTERCONNECT_SAMPLE_INPUT = {
            new RawData {TagName = "7443", TimeStamp = new DateTime(2015, 9, 22, 10, 30, 6), Value = 30.4m},
            new RawData {TagName = "7443", TimeStamp = new DateTime(2015, 9, 23, 10, 30, 6), Value = 27m},
            new RawData {TagName = "7443", TimeStamp = new DateTime(2015, 9, 24, 10, 30, 6), Value = 12m},
            new RawData {TagName = "7443", TimeStamp = new DateTime(2015, 9, 25, 10, 30, 6), Value = 20m},
            new RawData {TagName = "7443", TimeStamp = new DateTime(2015, 9, 26, 10, 30, 6), Value = 16m},
            new RawData {TagName = "7443", TimeStamp = new DateTime(2015, 9, 27, 10, 30, 6), Value = 15m},
            new RawData {TagName = "7443", TimeStamp = new DateTime(2015, 9, 28, 10, 30, 6), Value = 19m},
            new RawData {TagName = "7443", TimeStamp = new DateTime(2015, 9, 29, 10, 30, 6), Value = 25m},
            new RawData {TagName = "7443", TimeStamp = new DateTime(2015, 9, 30, 10, 30, 6), Value = 27m}
        };

        public const string INTERCONNECT_SAMPLE_OUTPUT = @"{
  ""DataType"": ""INTERCONNECT_DATA"",
  ""SourceSystem"": ""MAPCALL"",
  ""ScadaReadings"": [
    {
      ""Tagname"": ""7443"",
      ""Timestamp"": ""2015-09-22T10:30:06.000Z"",
      ""Value"": 30.4
    },
    {
      ""Tagname"": ""7443"",
      ""Timestamp"": ""2015-09-23T10:30:06.000Z"",
      ""Value"": 27.0
    },
    {
      ""Tagname"": ""7443"",
      ""Timestamp"": ""2015-09-24T10:30:06.000Z"",
      ""Value"": 12.0
    },
    {
      ""Tagname"": ""7443"",
      ""Timestamp"": ""2015-09-25T10:30:06.000Z"",
      ""Value"": 20.0
    },
    {
      ""Tagname"": ""7443"",
      ""Timestamp"": ""2015-09-26T10:30:06.000Z"",
      ""Value"": 16.0
    },
    {
      ""Tagname"": ""7443"",
      ""Timestamp"": ""2015-09-27T10:30:06.000Z"",
      ""Value"": 15.0
    },
    {
      ""Tagname"": ""7443"",
      ""Timestamp"": ""2015-09-28T10:30:06.000Z"",
      ""Value"": 19.0
    },
    {
      ""Tagname"": ""7443"",
      ""Timestamp"": ""2015-09-29T10:30:06.000Z"",
      ""Value"": 25.0
    },
    {
      ""Tagname"": ""7443"",
      ""Timestamp"": ""2015-09-30T10:30:06.000Z"",
      ""Value"": 27.0
    }
  ]
}";

        #endregion

        #region TankLevels (TanksLevel?  TanksLevels?)

        public static readonly RawData[] TANK_LEVEL_SAMPLE_INPUT = {
            new RawData {TagName = "SWIMRVR1.SWR_UB.F_CV", TimeStamp = new DateTime(2015, 9, 22, 10, 30, 6), Value = 97},
            new RawData {TagName = "SWIMRVR1.SWR_UB.F_CV", TimeStamp = new DateTime(2015, 9, 22, 10, 31, 6), Value = 97},
            new RawData {TagName = "SWIMRVR1.SWR_UB.F_CV", TimeStamp = new DateTime(2015, 9, 22, 10, 32, 6), Value = 97},
            new RawData {TagName = "SWIMRVR1.SWR_UB.F_CV", TimeStamp = new DateTime(2015, 9, 22, 10, 33, 6), Value = 97},
            new RawData {TagName = "SWIMRVR1.SWR_UB.F_CV", TimeStamp = new DateTime(2015, 9, 22, 10, 34, 6), Value = 97},
            new RawData {TagName = "SWIMRVR1.SWR_UB.F_CV", TimeStamp = new DateTime(2015, 9, 22, 10, 35, 6), Value = 97},
            new RawData {TagName = "SWIMRVR1.SWR_UB.F_CV", TimeStamp = new DateTime(2015, 9, 22, 10, 36, 6), Value = 97},
            new RawData {TagName = "SWIMRVR1.SWR_UB.F_CV", TimeStamp = new DateTime(2015, 9, 22, 10, 37, 6), Value = 97},
            new RawData {TagName = "SWIMRVR1.SWR_UB.F_CV", TimeStamp = new DateTime(2015, 9, 22, 10, 38, 6), Value = 97},
        };

        public const string TANK_LEVEL_SAMPLE_OUTPUT = @"{
  ""DataType"": ""TANK_LEVEL_DATA"",
  ""SourceSystem"": ""MAPCALL"",
  ""ScadaReadings"": [
    {
      ""Tagname"": ""SWIMRVR1.SWR_UB.F_CV"",
      ""Timestamp"": ""2015-09-22T10:30:06.000Z"",
      ""Value"": 97.0
    },
    {
      ""Tagname"": ""SWIMRVR1.SWR_UB.F_CV"",
      ""Timestamp"": ""2015-09-22T10:31:06.000Z"",
      ""Value"": 97.0
    },
    {
      ""Tagname"": ""SWIMRVR1.SWR_UB.F_CV"",
      ""Timestamp"": ""2015-09-22T10:32:06.000Z"",
      ""Value"": 97.0
    },
    {
      ""Tagname"": ""SWIMRVR1.SWR_UB.F_CV"",
      ""Timestamp"": ""2015-09-22T10:33:06.000Z"",
      ""Value"": 97.0
    },
    {
      ""Tagname"": ""SWIMRVR1.SWR_UB.F_CV"",
      ""Timestamp"": ""2015-09-22T10:34:06.000Z"",
      ""Value"": 97.0
    },
    {
      ""Tagname"": ""SWIMRVR1.SWR_UB.F_CV"",
      ""Timestamp"": ""2015-09-22T10:35:06.000Z"",
      ""Value"": 97.0
    },
    {
      ""Tagname"": ""SWIMRVR1.SWR_UB.F_CV"",
      ""Timestamp"": ""2015-09-22T10:36:06.000Z"",
      ""Value"": 97.0
    },
    {
      ""Tagname"": ""SWIMRVR1.SWR_UB.F_CV"",
      ""Timestamp"": ""2015-09-22T10:37:06.000Z"",
      ""Value"": 97.0
    },
    {
      ""Tagname"": ""SWIMRVR1.SWR_UB.F_CV"",
      ""Timestamp"": ""2015-09-22T10:38:06.000Z"",
      ""Value"": 97.0
    }
  ]
}";

        #endregion

        #region Schema

        public const string SCHEMA = @"{
	""$schema"": ""http://json-schema.org/draft-04/schema#"",
	""description"": ""Non Revenue Water JSON schema to exchang data from MapCall to STI"",
	""type"": ""object"",
	""properties"": {
		""DataType"": {
			""description"": ""Indicates the type of Message such as INTERCONNECT/TANK_LEVEL etc."",
			""type"": ""string"",
			""pattern"": ""[A-Z_]"",
			""enum"": [
				""INTERCONNECT_DATA"",
				""TANK_LEVEL_DATA"",
				""INSPECTION_DATA"",
				""WORKORDER_DATA"",
				""MAINBREAK_DATA""
			]
		},
		""SourceSystem"": {
			""description"": ""Indicates source system ie. MapCall etc."",
			""type"": ""string"",
			""pattern"": ""[a_zA-Z]"",
			""enum"": [
				""MAPCALL"",
				""TELOG""
			]
		},
		""ScadaReadings"": {
			""$ref"": ""#/definitions/ScadaReadingType""
		},
		""InspectionData"": {
			""$ref"": ""#/definitions/InspectionType""
		},
		""MainBreaks"": {
			""$ref"": ""#/definitions/MainBreakType""
		},
		""WorkOrders"": {
			""$ref"": ""#/definitions/WorkOrderType""
		}
	},
	""required"": [
		""DataType"",
		""SourceSystem""
	],
	""additionalProperties"": false,
	""definitions"": {
		""ScadaReadingType"": {
			""description"": ""Element to represent SCADA readings or interconnects, tank level etc."",
			""type"": ""array"",
			""items"": {
				""type"": ""object"",
				""properties"": {
					""Tagname"": {
						""description"": ""PI/SCADA tagname"",
						""type"": ""string""
					},
					""Timestamp"": {
						""description"": ""Indicates timestamp of the reading"",
						""type"": ""string"",
						""format"": ""date-time""
					},
					""Value"": {
						""description"": ""Value - can be text/numeric"",
						""type"": [
							""string"",
							""number""
						]
					}
				},
				""required"": [
					""Tagname"",
					""Timestamp"",
					""Value""
				],
				""additionalProperties"": false
			}
		},
		""InspectionType"": {
			""description"": ""Element to represent Hydrant Inspection Data"",
			""type"": ""array"",
			""items"": {
				""type"": ""object"",
				""properties"": {
					""HydrantNumber"": {
						""description"": ""Indicates Hydrant Id"",
						""type"": ""string""
					},
					""InspectionType"": {
						""description"": ""Indicates the type of Inspection done"",
						""type"": ""string""
					},
					""DateInspected"": {
						""description"": ""Indicates the date when Inspection was done"",
						""type"": ""string"",
						""format"": ""date""
					},
					""GallonsFlowed"": {
						""description"": ""Indicates # of Gallons Flowed"",
						""type"": ""number""
					},
					""MinutesFlowed"": {
						""description"": ""Indicates Number of Minutes  flowed"",
						""type"": ""number""
					},
					""GPM"": {
						""description"": ""Indicates Gallons per Minute"",
						""type"": ""number""
					}
				},
				""required"": [
					""HydrantNumber"",
					""InspectionType"",
					""DateInspected"",
					""GallonsFlowed"",
					""MinutesFlowed"",
					""GPM""
				],
				""additionalProperties"": false
			}
		},
		""MainBreakType"": {
			""description"": ""Element to represent MainBreak Data"",
			""type"": ""array"",
			""items"": {
				""type"": ""object"",
				""properties"": {
					""ID"": {
						""description"": ""Indicates Mainbreak ID"",
						""type"": ""string""
					},
					""MainBreakType"": {
						""description"": ""Indicates Type of the Mainbreak"",
						""type"": ""string""
					},
					""CreatedDate"": {
						""description"": ""Indicates the date when Mainbreak record created in the system"",
						""type"": ""string"",
						""format"": ""date""
					},
					""RepairDate"": {
						""description"": ""Indicates repair date of Mainbreak"",
						""type"": ""string"",
						""format"": ""date""
					},
					""AcquiredYear"": {
						""description"": ""Indicates acquired year"",
						""type"": ""string""
					},
					""LeakDate"": {
						""description"": ""Indicates Leakage date of Mainbreak"",
						""type"": ""string"",
						""format"": ""date""
					},
					""CustomerType"": {
						""description"": ""Indicates type of the customer Residential/Commercial etc."",
						""type"": ""string""
					},
					""CustomerImpacted"": {
						""description"": ""Indicates # of customers impacted"",
						""type"": ""integer""
					},
					""Geometry"": {
						""description"": ""Indicates Lat/Long coordinates for Mainbreak"",
						""type"": ""string""
					}
				},
				""required"": [
					""ID"",
					""MainBreakType"",
					""CreatedDate"",
					""RepairDate"",
					""AcquiredYear"",
					""LeakDate"",
					""CustomerImpacted"",
					""Geometry""
				],
				""additionalProperties"": false
			}
		},
		""WorkOrderType"": {
			""description"": ""Element to represent Workorder Data"",
			""type"": ""array"",
			""items"": {
				""type"": ""object"",
				""properties"": {
					""ID"": {
						""description"": ""Indicates WorkOrder ID"",
						""type"": ""string""
					},
					""Description"": {
						""description"": ""Indicates work order description"",
						""type"": ""string""
					},
					""DateCompleted"": {
						""description"": ""Indicates date when work order is completed"",
						""type"": ""string"",
						""format"": ""date""
					},
					""SAPWorkorderNum"": {
						""description"": ""Indicates SAP Workorder number"",
						""type"": ""number""
					},
					""SAPNotifNum"": {
						""description"": ""Indicates SAP Notification Number"",
						""type"": ""number""
					},
					""LostWater"": {
						""type"": ""number""
					},
					""LostWaterUnitOfMeasure"": {
						""type"": ""string"",
						""enum"": [
							""CGL"",
							""DCG""
						]
					},
					""Geometry"": {
						""type"": ""string""
					}
				},
				""required"": [
					""ID"",
					""Description"",
					""DateCompleted"",
					""SAPWorkorderNum"",
					""SAPNotifNum"",
					""LostWater"",
					""LostWaterUnitOfMeasure""
				],
				""additionalProperties"": false
			}
		}
	}
}";

        #endregion

        #endregion

        private SpaceTimeInsightJsonFileSerializer _target;

        [TestInitialize]
        public void TestInitialize()
        {
            _target = new SpaceTimeInsightJsonFileSerializer();
        }

        private void TestSample<TEntity>(string sampleOutput, IEnumerable<TEntity> sampleInput, Func<IEnumerable<TEntity>, Formatting, string> fn)
        {
            IList<string> invalidReasons;
            var result = fn(sampleInput, Formatting.Indented);

            MyAssert.StringsAreEqual(sampleOutput, result);

            Assert.IsTrue(JObject.Parse(result).IsValid(JSchema.Parse(SCHEMA), out invalidReasons),
                $"JSON is not valid as per the schema for the following reasons:{Environment.NewLine}{string.Join(Environment.NewLine, invalidReasons)}");
        }

        [TestMethod]
        public void TestSerializeWorkOrdersSerializesWorkOrdersProperly()
        {
            TestSample(WORK_ORDER_SAMPLE_OUTPUT, WORK_ORDER_SAMPLE_INPUT, _target.SerializeWorkOrders);
        }

        [TestMethod]
        public void TestSerializeMainBreaksSerializesMainBreaksProperly()
        {
            TestSample(MAIN_BREAK_SAMPLE_OUTPUT, MAIN_BREAK_SAMPLE_INPUT, _target.SerializeMainBreaks);
        }

        [TestMethod]
        public void TestSerializeHydrantInspectionsSerializesHydrantInspectionsProperly()
        {
            TestSample(HYDRANT_INSPECTION_SAMPLE_OUTPUT, HYDRANT_INSPECTION_SAMPLE_INPUT,
                _target.SerializeHydrantInspections);
        }

        [TestMethod]
        public void TestSerializeInterconnectDataSerializesInterconnectDataProperly()
        {
            TestSample(INTERCONNECT_SAMPLE_OUTPUT, INTERCONNECT_SAMPLE_INPUT, _target.SerializeInterconnectData);
        }

        [TestMethod]
        public void TestSerializeTankLevelDataSerializesTankLevelDataProperly()
        {
            TestSample(TANK_LEVEL_SAMPLE_OUTPUT, TANK_LEVEL_SAMPLE_INPUT,
                _target.SerializeTankLevelData);
        }
    }
}