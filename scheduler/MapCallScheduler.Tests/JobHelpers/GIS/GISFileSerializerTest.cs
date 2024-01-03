using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using MapCall.Common.Model.Entities;
using MapCallScheduler.JobHelpers.GIS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.MSTest.TestExtensions;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using AsBuiltImage = MapCall.Common.Model.Entities.AsBuiltImage;
using AssetStatus = MapCall.Common.Model.Entities.AssetStatus;
using County = MapCall.Common.Model.Entities.County;
using Hydrant = MapCall.Common.Model.Entities.Hydrant;
using HydrantBilling = MapCall.Common.Model.Entities.HydrantBilling;
using HydrantManufacturer = MapCall.Common.Model.Entities.HydrantManufacturer;
using OperatingCenter = MapCall.Common.Model.Entities.OperatingCenter;
using Service = MapCall.Common.Model.Entities.Service;
using ServiceMaterial = MapCall.Common.Model.Entities.ServiceMaterial;
using ServiceUtilityType = MapCall.Common.Model.Entities.ServiceUtilityType;
using SewerOpening = MapCall.Common.Model.Entities.SewerOpening;
using SewerOpeningMaterial = MapCall.Common.Model.Entities.SewerOpeningMaterial;
using SewerOpeningType = MapCall.Common.Model.Entities.SewerOpeningType;
using State = MapCall.Common.Model.Entities.State;
using Town = MapCall.Common.Model.Entities.Town;
using User = MapCall.Common.Model.Entities.Users.User;
using Valve = MapCall.Common.Model.Entities.Valve;
using ValveControl = MapCall.Common.Model.Entities.ValveControl;
using ValveNormalPosition = MapCall.Common.Model.Entities.ValveNormalPosition;
using ValveOpenDirection = MapCall.Common.Model.Entities.ValveOpenDirection;
using ValveSize = MapCall.Common.Model.Entities.ValveSize;
using ValveType = MapCall.Common.Model.Entities.ValveType;

namespace MapCallScheduler.Tests.JobHelpers.GIS
{
    [TestClass]
    public class GISFileSerializerTest
    {
        #region Constants

        #region Hydrants

        public static readonly Hydrant[] HYDRANT_SAMPLE_INPUT = {
            new Hydrant {
                Id = 1,
                HydrantBilling = new HydrantBilling {Id = 2, Description = "billing"},
                HydrantNumber = "hydrant",
                DateInstalled = new DateTime(2013, 5, 19),
                Status = new AssetStatus {Id = 3, Description = "status"},
                HydrantManufacturer = new HydrantManufacturer {Id = 4, Description = "manufacturer"},
                FireDistrict = new FireDistrict {Id = 5, PremiseNumber = "premise"},
                Route = 6,
                SAPEquipmentId = 7,
                Stop = 8.8m,
                UpdatedAt = new DateTime(2020, 8, 21),
                UpdatedBy = new User {
                    Id = 9,
                    FullName = "user",
                    UserName = "user"
                },
                WorkOrders = new List<WorkOrder> {
                    new WorkOrder {
                        Id = 10,
                        CreatedAt = new DateTime(2020, 8, 21),
                        WorkDescription = new WorkDescription {
                            Id = (int)WorkDescription.Indices.HYDRANT_INSTALLATION,
                            Description = "HYDRANT INSTALLATION",
                        },
                        AccountCharged = "account"
                    }
                },
                OperatingCenter = new OperatingCenter {
                    Id = 10,
                    OperatingCenterCode = "NJ7",
                    OperatingCenterName = "Shrewsbury"
                },
                Town = new Town {
                    Id = 57,
                    FullName = "CITY OF LONG BRANCH",
                    State = new State {
                        Id = 15,
                        Abbreviation = "NJ"
                    }
                },
                FunctionalLocation = new FunctionalLocation { Description = "NJMM-LB-HYDRT" }
            }
        };

        public const string HYDRANT_SAMPLE_OUTPUT = @"{
  ""SchemaVersion"": ""1.0.5"",
  ""DataType"": ""HYDRANT_DATA"",
  ""SourceSystem"": ""MAPCALL"",
  ""Hydrants"": [
    {
      ""Id"": 1,
      ""HydrantBilling"": {
        ""Id"": 2,
        ""Description"": ""billing""
      },
      ""HydrantNumber"": ""hydrant"",
      ""DateInstalled"": ""2013-05-19T04:00:00.0000000Z"",
      ""Status"": {
        ""Id"": 3,
        ""Description"": ""status""
      },
      ""HydrantManufacturer"": {
        ""Id"": 4,
        ""Description"": ""manufacturer""
      },
      ""PremiseNumber"": ""premise"",
      ""Route"": 6,
      ""SAPEquipmentId"": 7,
      ""Stop"": 8.8,
      ""LastUpdated"": ""2020-08-21T04:00:00.0000000Z"",
      ""LastUpdatedBy"": {
        ""Id"": 9,
        ""FullName"": ""user"",
        ""UserName"": ""user""
      },
      ""WBSNumber"": ""account"",
      ""WorkOrderId"": 10,
      ""State"": {
        ""Id"": 15,
        ""Abbreviation"": ""NJ""
      },
      ""Town"": {
        ""Id"": 57,
        ""FullName"": ""CITY OF LONG BRANCH""
      },
      ""OperatingCenter"": {
        ""Id"": 10,
        ""OperatingCenterCode"": ""NJ7"",
        ""OperatingCenterName"": ""Shrewsbury""
      },
      ""FunctionalLocation"": ""NJMM-LB-HYDRT""
    }
  ]
}";

        #endregion

        #region Valves

        public static readonly Valve[] VALVE_SAMPLE_INPUT = {
            new Valve {
                Id = 1,
                ValveControls = new ValveControl {Id = 2, Description = "controls"},
                ValveSize = new ValveSize {Size = 3.3m},
                DateInstalled = new DateTime(2013, 5, 19),
                Status = new AssetStatus {Id = 4, Description = "status"},
                ValveMake = new ValveManufacturer {Id = 5, Description = "manufacturer"},
                NormalPosition = new ValveNormalPosition {Id = 6, Description = "position"},
                OpenDirection = new ValveOpenDirection {Id = 7, Description = "direction"},
                Route = 8,
                SAPEquipmentId = 9,
                Stop = 10.1m,
                Turns = 11.11m,
                ValveNumber = "1234",
                ValveType = new ValveType {Id = 12, Description = "type"},
                UpdatedAt = new DateTime(2020, 8, 21),
                UpdatedBy = new User {Id = 13, FullName = "user", UserName = "user"},
                WorkOrders = new List<WorkOrder> {
                    new WorkOrder {
                        Id = 14,
                        CreatedAt = new DateTime(2020, 8, 21),
                        WorkDescription = new WorkDescription {
                            Id = (int)WorkDescription.Indices.VALVE_INSTALLATION,
                            Description = "VALVE INSTALLATION",
                        },
                        AccountCharged = "account"
                    }
                },
                OperatingCenter = new OperatingCenter {
                    Id = 10,
                    OperatingCenterCode = "NJ7",
                    OperatingCenterName = "Shrewsbury"
                },
                Town = new Town {
                    Id = 57,
                    FullName = "CITY OF LONG BRANCH",
                    State = new State {
                        Id = 15,
                        Abbreviation = "NJ"
                    }
                },
                FunctionalLocation = new FunctionalLocation { Description = "NJMM-LB-HYDRT" }
            }
        };

        public const string VALVE_SAMPLE_OUTPUT = @"{
  ""SchemaVersion"": ""1.0.5"",
  ""DataType"": ""VALVE_DATA"",
  ""SourceSystem"": ""MAPCALL"",
  ""Valves"": [
    {
      ""Id"": 1,
      ""ValveControls"": {
        ""Id"": 2,
        ""Description"": ""controls""
      },
      ""ValveSize"": {
        ""Id"": 0,
        ""Size"": 3.3
      },
      ""DateInstalled"": ""2013-05-19T04:00:00.0000000Z"",
      ""Status"": {
        ""Id"": 4,
        ""Description"": ""status""
      },
      ""ValveMake"": {
        ""Id"": 5,
        ""Description"": ""manufacturer""
      },
      ""NormalPosition"": {
        ""Id"": 6,
        ""Description"": ""position""
      },
      ""OpenDirection"": {
        ""Id"": 7,
        ""Description"": ""direction""
      },
      ""Route"": 8,
      ""SAPEquipmentId"": 9,
      ""Stop"": 10.1,
      ""Turns"": 11.11,
      ""ValveNumber"": ""1234"",
      ""ValveType"": {
        ""Id"": 12,
        ""Description"": ""type""
      },
      ""LastUpdated"": ""2020-08-21T04:00:00.0000000Z"",
      ""LastUpdatedBy"": {
        ""Id"": 13,
        ""FullName"": ""user"",
        ""UserName"": ""user""
      },
      ""WBSNumber"": ""account"",
      ""WorkOrderId"": 14,
      ""State"": {
        ""Id"": 15,
        ""Abbreviation"": ""NJ""
      },
      ""Town"": {
        ""Id"": 57,
        ""FullName"": ""CITY OF LONG BRANCH""
      },
      ""OperatingCenter"": {
        ""Id"": 10,
        ""OperatingCenterCode"": ""NJ7"",
        ""OperatingCenterName"": ""Shrewsbury""
      },
      ""FunctionalLocation"": ""NJMM-LB-HYDRT""
    }
  ]
}";

        #endregion

        #region Sewer Openings

        public static readonly SewerOpening[] SEWER_OPENING_SAMPLE_INPUT = {
            new SewerOpening {
                Id = 1,
                DepthToInvert = 2.2m,
                DateInstalled = new DateTime(2020, 10, 14),
                Status = new AssetStatus {Id = 3, Description = "status"},
                SewerOpeningMaterial = new SewerOpeningMaterial {Id = 4, Description = "material"},
                RimElevation = 5.5m,
                Route = 6,
                SAPEquipmentId = 7,
                Stop = 8,
                OpeningNumber = "1234",
                SewerOpeningType = new SewerOpeningType {Id = 9, Description = "type"},
                UpdatedAt = new DateTime(2020, 10, 14),
                UpdatedBy = new User {Id = 10, FullName = "user", UserName = "user"},
                WorkOrders = new List<WorkOrder> {
                    new WorkOrder {
                        Id = 11,
                        CreatedAt = new DateTime(2020, 10, 14),
                        WorkDescription = new WorkDescription {
                            Id = (int)WorkDescription.Indices.SEWER_OPENING_INSTALLATION,
                            Description = "SEWER OPENING INSTALLATION",
                        },
                        AccountCharged = "account"
                    }
                },
                OperatingCenter = new OperatingCenter {
                    Id = 10,
                    OperatingCenterCode = "NJ7",
                    OperatingCenterName = "Shrewsbury"
                },
                Town = new Town {
                    Id = 57,
                    FullName = "CITY OF LONG BRANCH",
                    State = new State {
                        Id = 15,
                        Abbreviation = "NJ"
                    }
                },
                FunctionalLocation = new FunctionalLocation { Description = "NJMM-LB-HYDRT" }
            }
        };

        public const string SEWER_OPENING_SAMPLE_OUTPUT = @"{
  ""SchemaVersion"": ""1.0.5"",
  ""DataType"": ""SEWER_OPENING_DATA"",
  ""SourceSystem"": ""MAPCALL"",
  ""SewerOpenings"": [
    {
      ""Id"": 1,
      ""DepthToInvert"": 2.2,
      ""DateInstalled"": ""2020-10-14T04:00:00.0000000Z"",
      ""Status"": {
        ""Id"": 3,
        ""Description"": ""status""
      },
      ""SewerOpeningMaterial"": {
        ""Id"": 4,
        ""Description"": ""material""
      },
      ""RimElevation"": 5.5,
      ""Route"": 6,
      ""SAPEquipmentId"": 7,
      ""Stop"": 8,
      ""OpeningNumber"": ""1234"",
      ""SewerOpeningType"": {
        ""Id"": 9,
        ""Description"": ""type""
      },
      ""LastUpdated"": ""2020-10-14T04:00:00.0000000Z"",
      ""LastUpdatedBy"": {
        ""Id"": 10,
        ""FullName"": ""user"",
        ""UserName"": ""user""
      },
      ""WBSNumber"": ""account"",
      ""WorkOrderId"": 11,
      ""State"": {
        ""Id"": 15,
        ""Abbreviation"": ""NJ""
      },
      ""Town"": {
        ""Id"": 57,
        ""FullName"": ""CITY OF LONG BRANCH""
      },
      ""OperatingCenter"": {
        ""Id"": 10,
        ""OperatingCenterCode"": ""NJ7"",
        ""OperatingCenterName"": ""Shrewsbury""
      },
      ""FunctionalLocation"": ""NJMM-LB-HYDRT""
    }
  ]
}";

        #endregion

        #region Services
        
        public static readonly MostRecentlyInstalledService[] SERVICE_SAMPLE_INPUT = {
            new MostRecentlyInstalledService {
                Service = new Service {
                    Id = 1,
                    PremiseNumber = "premiseNumber",
                    Installation = "installation",
                    DateInstalled = new DateTime(2020, 12, 15),
                    UpdatedAt = new DateTime(2020, 12, 15),
                    Town = new Town {
                        Id = 9,
                        FullName = "TOWNSHIP OF MIDDLETOWN",
                        State = new State {
                            Id = 4,
                            Abbreviation = "NJ"
                        }
                    },
                    OperatingCenter = new OperatingCenter {
                        Id = 10,
                        OperatingCenterCode = "NJ7",
                        OperatingCenterName = "Shrewsbury",
                    },
                    ServiceCategory = new ServiceCategory {
                        Id = 12,
                        Description = "Sewer Service New",
                        ServiceUtilityType = new ServiceUtilityType {
                            Id = 6,
                            Description = "Domestic Wastewater"
                        }
                    }
                },
                ServiceMaterial = new ServiceMaterial {
                    Id = 2,
                    Description = "serviceMaterial"
                },
                CustomerSideMaterial = new ServiceMaterial {
                    Id = 3,
                    Description = "customerSideMaterial"
                },
                ServiceMaterialSetBy = new User {
                    Id = 7,
                    FullName = "Bartles",
                    UserName = "bartles"
                },
                CustomerMaterialSetBy = new User {
                    Id = 8,
                    FullName = "Jaymes",
                    UserName = "jaymes"
                },
                Premise = new Premise {
                    ConsolidatedCustomerConvertedMaterialsRepository = CreateMockedConsolidatedMaterialRepository(),
                    PremiseNumber = "premiseNumber",
                    MostRecentService = new MostRecentlyInstalledService {
                        ServiceMaterial = new ServiceMaterial {
                            Id = 2,
                            Description = "serviceMaterial",
                            CompanyEPACode = new EPACode { Id = 1, Description = "LEAD" },
                            CustomerEPACode = new EPACode { Id = 1, Description = "LEAD" },
                        },
                        CustomerSideMaterial = new ServiceMaterial {
                            Id = 3,
                            Description = "customerSideMaterial",
                            CompanyEPACode = new EPACode { Id = 1, Description = "LEAD" },
                            CustomerEPACode = new EPACode { Id = 1, Description = "LEAD" },
                        }
                    },
                    OperatingCenter = new OperatingCenter {
                        OperatingCenterCode = "NJ7",
                        OperatingCenterName = "Shrewsbury",
                        State = new State { Id = 1, Abbreviation = "NJ" }
                    },
                    ShortCycleCustomerMaterials = new List<ShortCycleCustomerMaterial> {
                        new ShortCycleCustomerMaterial {
                            CustomerSideMaterial = new ServiceMaterial {
                                Id = 3,
                                Description = "customerSideMaterial",
                                CompanyEPACode = new EPACode { Id = 1, Description = "LEAD" },
                                CustomerEPACode = new EPACode { Id = 1, Description = "LEAD" }
                            }
                        }
                    }
                }
            }
        };

        public const string SERVICE_SAMPLE_OUTPUT = @"{
  ""SchemaVersion"": ""1.0.5"",
  ""DataType"": ""SERVICE_DATA"",
  ""SourceSystem"": ""MAPCALL"",
  ""Services"": [
    {
      ""Id"": 1,
      ""PremiseNumber"": ""premiseNumber"",
      ""Installation"": ""installation"",
      ""ServiceMaterial"": {
        ""Id"": 2,
        ""Description"": ""serviceMaterial""
      },
      ""CustomerSideMaterial"": {
        ""Id"": 3,
        ""Description"": ""customerSideMaterial""
      },
      ""Work1VCustomerSideMaterial"": {
        ""Id"": 3,
        ""Description"": ""customerSideMaterial""
      },
      ""EpaCompanySideMaterial"": {
        ""EPACode"": {
          ""Id"": 1,
          ""Description"": ""LEAD""
        }
      },
      ""EpaCustomerSideMaterial"": {
        ""EPACode"": {
          ""Id"": 1,
          ""Description"": ""LEAD""
        }
      },
      ""EpaCustomerSideMaterialExternal"": {
        ""EPACode"": {
          ""Id"": 1,
          ""Description"": ""LEAD""
        }
      },
      ""EpaConsolidatedCustomerSideMaterial"": {
        ""EPACode"": {
          ""Id"": 1,
          ""Description"": ""LEAD""
        }
      },
      ""DateInstalled"": ""2020-12-15T05:00:00.0000000Z"",
      ""LastUpdated"": ""2020-12-15T05:00:00.0000000Z"",
      ""Town"": {
        ""Id"": 9,
        ""FullName"": ""TOWNSHIP OF MIDDLETOWN""
      },
      ""OperatingCenter"": {
        ""Id"": 10,
        ""OperatingCenterCode"": ""NJ7"",
        ""OperatingCenterName"": ""Shrewsbury""
      },
      ""State"": {
        ""Id"": 4,
        ""Abbreviation"": ""NJ""
      },
      ""ServiceUtilityType"": {
        ""Id"": 6,
        ""Description"": ""Domestic Wastewater""
      },
      ""ServiceMaterialSetBy"": {
        ""Id"": 7,
        ""FullName"": ""Bartles"",
        ""UserName"": ""bartles""
      },
      ""CustomerMaterialSetBy"": {
        ""Id"": 8,
        ""FullName"": ""Jaymes"",
        ""UserName"": ""jaymes""
      }
    }
  ]
}";

        #endregion

        #region AsBuilt Images

        public static readonly AsBuiltImage[] ASBUILT_IMAGE_SAMPLE_INPUT = {
            new AsBuiltImage {
                Id = 1,
                Coordinate = new Coordinate {
                    Latitude = 40.406821M,
                    Longitude = -74.224427M
                },
                OperatingCenter = new OperatingCenter {
                    OperatingCenterCode = "NJ7",
                    OperatingCenterName = "Shrewsbury"
                },
                Town = new Town {
                    Id = 1,
                    County = new County {
                        Id = 1,
                        Name = "MONMOUTH"
                    },
                    FullName = "TOWNSHIP OF MIDDLETOWN"
                },
                Street = "HYAMILTON",
                CrossStreet = "THOMPSON",
                TaskNumber = "R18-18B1.13-P-0037",
                DateInstalled = new DateTime(2021, 2, 19),
                PhysicalInService = new DateTime(2021, 2, 19),
                Comments = "test"
            }
        };

        public const string ASBUILT_IMAGE_SAMPLE_OUTPUT = @"{
  ""SchemaVersion"": ""1.0.5"",
  ""DataType"": ""AS_BUILT_IMAGE_DATA"",
  ""SourceSystem"": ""MAPCALL"",
  ""AsBuiltImages"": [
    {
      ""Id"": 1,
      ""PdfLink"": ""https://mapcall.awapps.com/modules/mvc/FieldOperations/AsBuiltImage/Show/1.pdf"",
      ""Latitude"": 40.406821,
      ""Longitude"": -74.224427,
      ""County"": {
        ""Id"": 1,
        ""Name"": ""MONMOUTH""
      },
      ""Comments"": ""test"",
      ""DateInstalled"": ""2021-02-19T05:00:00.0000000Z"",
      ""InServiceDate"": ""2021-02-19T05:00:00.0000000Z"",
      ""FullStreet"": ""HYAMILTON"",
      ""CrossStreet"": ""THOMPSON"",
      ""WorkOrderNumber"": ""R18-18B1.13-P-0037"",
      ""Town"": {
        ""Id"": 1,
        ""FullName"": ""TOWNSHIP OF MIDDLETOWN""
      },
      ""OperatingCenter"": {
        ""Id"": 0,
        ""OperatingCenterCode"": ""NJ7"",
        ""OperatingCenterName"": ""Shrewsbury""
      }
    }
  ]
}";

        #endregion

        #endregion

        #region Private Members

        private GISFileSerializer _target;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _target = new GISFileSerializer();
        }

        #endregion

        #region Private Methods

        private string ReadSchema()
        {
            var assembly = GetType().Assembly;
            var resourceName = "MapCallScheduler.Tests.JobHelpers.GIS.GISFileDumpSchema.json";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        private void TestSample<TEntity>(string sampleOutput, IQueryable<TEntity> sampleInput,
            Func<IQueryable<TEntity>, Formatting, string> fn)
        {
            IList<string> invalidReasons;
            var result = fn(sampleInput, Formatting.Indented);

            MyAssert.StringsAreEqual(sampleOutput, result);

            var schema = ReadSchema();

            Assert.IsTrue(JObject.Parse(result).IsValid(JSchema.Parse(schema), out invalidReasons),
                $"JSON is not valid as per the schema for the following reasons:{Environment.NewLine}{string.Join(Environment.NewLine, invalidReasons)}");
        }

        private static IRepository<ConsolidatedCustomerSideMaterial> CreateMockedConsolidatedMaterialRepository()
        {
            var mock = new Mock<IRepository<ConsolidatedCustomerSideMaterial>>();

            var mockData = new List<ConsolidatedCustomerSideMaterial> {
                new ConsolidatedCustomerSideMaterial {
                    CustomerSideEPACode = new EPACode { Id = 1, Description = "LEAD" },
                    ConsolidatedEPACode = new EPACode { Id = 1, Description = "LEAD" },
                    CustomerSideExternalEPACode = new EPACode { Id = 1, Description = "LEAD" }
                }
            };

            mock.Setup(repo => repo.Where(It.IsAny<Expression<Func<ConsolidatedCustomerSideMaterial, bool>>>()))
                .Returns((Expression<Func<ConsolidatedCustomerSideMaterial, bool>> predicate) => mockData.AsQueryable().Where(predicate));

            return mock.Object;
        }
        
        #endregion

        // TO REGENERATE THE SCHEMA:
        // var schema = new JSchemaGenerator {
        //   DefaultRequired = Required.Default,
        //   SchemaReferenceHandling = SchemaReferenceHandling.Objects
        // }.Generate(typeof(MapCallSyncMessage)).ToString();

        [TestMethod]
        public void TestSerializeSerializesHydrantsProperly()
        {
            TestSample(HYDRANT_SAMPLE_OUTPUT, HYDRANT_SAMPLE_INPUT.AsQueryable(), _target.Serialize);
        }

        [TestMethod]
        public void TestSerializeSerializesValvesProperly()
        {
            TestSample(VALVE_SAMPLE_OUTPUT, VALVE_SAMPLE_INPUT.AsQueryable(), _target.Serialize);
        }

        [TestMethod]
        public void TestSerializeSerializesSewerOpeningsProperly()
        {
            TestSample(SEWER_OPENING_SAMPLE_OUTPUT, SEWER_OPENING_SAMPLE_INPUT.AsQueryable(), _target.Serialize);
        }

        [TestMethod]
        public void TestSerializeSerializesServicesProperly()
        {
            TestSample(SERVICE_SAMPLE_OUTPUT, SERVICE_SAMPLE_INPUT.AsQueryable(), _target.Serialize);
        }

        [TestMethod]
        public void TestSerializeSerializesAsBuiltImagesProperly()
        {
            TestSample(ASBUILT_IMAGE_SAMPLE_OUTPUT, ASBUILT_IMAGE_SAMPLE_INPUT.AsQueryable(), _target.Serialize);
        }
    }
}
