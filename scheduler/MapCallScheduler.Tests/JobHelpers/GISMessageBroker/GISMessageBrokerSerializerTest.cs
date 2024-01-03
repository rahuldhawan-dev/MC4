using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using MapCallScheduler.JobHelpers.GISMessageBroker;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using StructureMap;

namespace MapCallScheduler.Tests.JobHelpers.GISMessageBroker
{
    [TestClass]
    // ReSharper disable once InconsistentNaming
    public class GISMessageBrokerSerializerTest
    {
        #region Constants

        public static readonly SampleSite SAMPLE_SITE_INPUT = new SampleSite {
            // Coordinates (Lat/Lng)
            Coordinate = new Coordinate {
                Latitude = 1.1m,
                Longitude = 2.2m
            },
            // Derive street address (Street Number, Street, Town, State, Zip) in GIS and populate MapCall based on what is selected for Lat/Long, Premise or Facility
            StreetNumber = "streetNumber",
            Street = new Street {
                Id = 3,
                Prefix = new StreetPrefix {
                    Id = 4,
                    Description = "streetPrefix"
                },
                Suffix = new StreetSuffix {
                    Id = 5,
                    Description = "streetSuffix"
                },
                Name = "name"
            },
            Town = new Town {
                Id = 6,
                FullName = "fullName"
            },
            ZipCode = "zip",
            // Common Site Name
            CommonSiteName = "commonSiteName",
            // Location Name Description
            LocationNameDescription = "locationNameDescription",
            // MapCall Sample Site Id
            Id = 7,
            // Premise/Facility
            Premise = new Premise {
                PremiseNumber = "premiseNumber"
            },
            Facility = new Facility {
                Id = 8,
                FacilityName = "facilityName"
            },
            // PWSID (need to investigate if we can leverage rest api to populate PWSID from GIS)
            PublicWaterSupply = new PublicWaterSupply {
                Id = 9,
                Identifier = "identifier",
                // Active/Inactive PWSID (derive from status of linked PWSID)
                Status = new PublicWaterSupplyStatus {
                    Id = 10,
                    Description = "publicWaterSupplyStatus"
                }
            },
            // Operating Center/District/State [auto populate based on selection of 1 and/or 2]
            OperatingCenter = new OperatingCenter {
                Id = 11,
                OperatingCenterCode = "operatingCenterCode",
                OperatingCenterName = "operatingCenterName"
            },
            // District ?
            State = new State {
                Id = 12,
                Abbreviation = "abbreviation"
            },
            // Pipe Material (utility side) (Service Material)
            // Pipe Material(customer side)(Customer Side Material)
            CustomerPlumbingMaterial = new ServiceMaterial {
                Id = 13,
                Description = "customerPlumbingMaterial"
            },
            // Lead & Copper Sample Site
            LeadCopperSite = true,
            // Active / Inactive(Status)
            Status = new SampleSiteStatus {
                Id = 14,
                Description = "sampleSiteStatus"
            },
            // Availability
            Availability = new SampleSiteAvailability {
                Id = 15,
                Description = "availability"
            },
            // Sample Site Collection Type(added by MC - 2768)
            CollectionType = new SampleSiteCollectionType {
                Id = 16,
                Description = "collectionType"
            },
            // Sample Site Location Type(added by MC - 2768)
            LocationType = new SampleSiteLocationType {
                Id = 17,
                Description = "locationType"
            },
            ParentSite = null,
            // Primary Station Code(Agency Id)(relabeled from DEP ID by MC - 2768)
            AgencyId = "agencyId",
            LimsFacilityId = "limsFacilityId",
            LimsPrimaryStationCode = "limsPrimaryStationCode",
            LimsSequenceNumber = null,
            LimsSiteId = "limsSiteId",
            SampleSiteProfile = new SampleSiteProfile {
                Id = 1,
                Number = 200,
                Name = "profile-200",
                SampleSiteProfileAnalysisType = new SampleSiteProfileAnalysisType {
                    Id = 1,
                    Description = "CHEM"
                },
                PublicWaterSupply = new PublicWaterSupply {
                    Id = 9,
                    Identifier = "identifier",
                    Status = new PublicWaterSupplyStatus {
                        Id = 10,
                        Description = "publicWaterSupplyStatus"
                    }
                }
            },
            // Special CAT Alert(added by MC - 2768)
            // Analyte List(ex.BacT, L & C, etc)(added by MC - 2768)
            // LIMS Sequence Number(added by MC - 2768)(1:many relationship sample site/ sequence number)
            // Customer Participation Confirmed?
            CustomerParticipationConfirmed = true,
            // Is Alternate Site?
            IsAlternateSite = true,
            // Open Text) Contact Name:
            CustomerName = "customerName",
            // Contact email
            CustomerEmail = "customerEmail",
            // Contact phone
            CustomerHomePhone = "customerHomePhone",
            // Contact Notes
            //Send to LIMS(added by MC - 2768)
            IsLimsLocation = true,
            BactiSite = true,
            // Validation Status(added by MC - 2768)
            ValidatedAt = new DateTime(2021, 1, 15, 10, 33, 0, DateTimeKind.Utc),
            // Validation Status(added by MC - 2768)
            ValidatedBy = new Employee {
                Id = 2476,
                EmployeeId = "12345678",
                FirstName = "FirstName",
                MiddleName = "RM",
                LastName = "LastName",
                EmailAddress = "xyz@AMWATER.COM"
            },
            SampleSiteValidationStatus = new SampleSiteValidationStatus {
                Id = 2,
                Description = "Not Validated"
            },
            SampleSiteInactivationReason = new SampleSiteInactivationReason {
                Id = 1,
                Description = "Customer Declined Program"
            }
        };

        public const string SAMPLE_SITE_OUTPUT =
            @"{
  ""SchemaVersion"": ""1.1.0"",
  ""DataType"": ""SAMPLE_SITE_DATA"",
  ""SourceSystem"": ""MAPCALL"",
  ""SampleSite"": {
    ""Id"": 7,
    ""Coordinate"": {
      ""Latitude"": 1.1,
      ""Longitude"": 2.2
    },
    ""Street"": {
      ""Id"": 3,
      ""Prefix"": {
        ""Id"": 4,
        ""Description"": ""streetPrefix""
      },
      ""Suffix"": {
        ""Id"": 5,
        ""Description"": ""streetSuffix""
      },
      ""Name"": ""name""
    },
    ""Town"": {
      ""Id"": 6,
      ""FullName"": ""fullName""
    },
    ""Facility"": {
      ""Id"": 8,
      ""FacilityName"": ""facilityName""
    },
    ""PublicWaterSupply"": {
      ""Id"": 9,
      ""Identifier"": ""identifier"",
      ""Status"": {
        ""Id"": 10,
        ""Description"": ""publicWaterSupplyStatus""
      }
    },
    ""OperatingCenter"": {
      ""Id"": 11,
      ""OperatingCenterCode"": ""operatingCenterCode"",
      ""OperatingCenterName"": ""operatingCenterName""
    },
    ""State"": {
      ""Id"": 12,
      ""Abbreviation"": ""abbreviation""
    },
    ""CustomerPlumbingMaterial"": {
      ""Id"": 13,
      ""Description"": ""customerPlumbingMaterial""
    },
    ""Status"": {
      ""Id"": 14,
      ""Description"": ""sampleSiteStatus""
    },
    ""Availability"": {
      ""Id"": 15,
      ""Description"": ""availability""
    },
    ""CollectionType"": {
      ""Id"": 16,
      ""Description"": ""collectionType""
    },
    ""LocationType"": {
      ""Id"": 17,
      ""Description"": ""locationType""
    },
    ""ParentSiteId"": null,
    ""StreetNumber"": ""streetNumber"",
    ""Zip"": ""zip"",
    ""CommonSiteName"": ""commonSiteName"",
    ""LocationNameDescription"": ""locationNameDescription"",
    ""PremiseNumber"": ""premiseNumber"",
    ""LeadCopperSite"": true,
    ""AgencyId"": ""agencyId"",
    ""LimsFacilityId"": ""limsFacilityId"",
    ""LimsSiteId"": ""limsSiteId"",
    ""LimsPrimaryStationCode"": ""limsPrimaryStationCode"",
    ""LimsSequenceNumber"": null,
    ""LimsProfile"": {
      ""Id"": 1,
      ""Name"": ""profile-200"",
      ""Number"": 200,
      ""SampleSiteProfileAnalysisType"": {
        ""Id"": 1,
        ""Description"": ""CHEM""
      },
      ""PublicWaterSupply"": {
        ""Id"": 9,
        ""Identifier"": ""identifier"",
        ""Status"": {
          ""Id"": 10,
          ""Description"": ""publicWaterSupplyStatus""
        }
      }
    },
    ""CustomerParticipationConfirmed"": true,
    ""IsAlternateSite"": true,
    ""IsComplianceSampleSite"": null,
    ""CustomerName"": ""customerName"",
    ""CustomerEmail"": ""customerEmail"",
    ""CustomerHomePhone"": ""customerHomePhone"",
    ""IsLimsLocation"": true,
    ""BactiSite"": true,
    ""ValidatedAt"": ""2021-01-15T10:33:00.0000000Z"",
    ""ValidatedBy"": {
      ""Id"": 2476,
      ""FirstName"": ""FirstName"",
      ""LastName"": ""LastName"",
      ""MiddleName"": ""RM"",
      ""EmployeeId"": ""12345678"",
      ""EmailAddress"": ""xyz@AMWATER.COM""
    },
    ""ValidationStatus"": {
      ""Id"": 2,
      ""Description"": ""Not Validated""
    },
    ""InactivationReason"": {
      ""Id"": 1,
      ""Description"": ""Customer Declined Program""
    }
  }
}";

        public string SEWER_MAIN_CLEANING_OUTPUT = @"{
  ""SchemaVersion"": ""1.1.0"",
  ""DataType"": ""SEWER_MAIN_CLEANING_DATA"",
  ""SourceSystem"": ""MAPCALL"",
  ""SewerMainCleaning"": {
    ""Id"": 10,
    ""NextCleanLineDate"": ""2021-11-01T10:10:00.0000000"",
    ""CompletedDate"": ""2021-11-02T12:15:00.0000000"",
    ""Opening1"": {
      ""Id"": 2,
      ""OpeningNumber"": ""123456""
    },
    ""Opening2"": {
      ""Id"": 6,
      ""OpeningNumber"": ""456789""
    },
    ""State"": {
      ""Id"": 12,
      ""Abbreviation"": ""abbreviation""
    }
  }
}";
        
        public string W1V_SERVICE_RECORD_OUTPUT = @"{
  ""schemaVersion"": ""1.1.0"",
  ""dataType"": ""W1V_SERVICE_DATA"",
  ""sourceSystem"": ""MAPCALL"",
  ""w1VServiceRecord"": {
    ""premiseId"": ""1233456"",
    ""fsrId"": ""123"",
    ""companySideMaterial"": ""COMPANY_LEAD"",
    ""customerSideMaterial"": ""W1V_CUSTOMER_MATERIAL"",
    ""customerSideMaterialExternal"": ""MAPCALL_CUSTOMER_LEAD"",
    ""epaConvertedCompanySideMaterial"": ""COMPANY_LEAD_EPA"",
    ""epaConvertedCustomerSideMaterial"": ""W1V_CUSTOMER_MATERIAL_EPA"",
    ""epaConvertedCustomerSideMaterialExternal"": ""MAPCALL_CUSTOMER_LEAD_EPA"",
    ""epaConvertedConsolidatedCustomerSideMaterial"": ""LEAD"",
    ""submitDate"": ""1/1/2023"",
    ""equipmentId"": ""EQ_123123"",
    ""deviceLocationId"": ""DL_123123"",
    ""installationId"": ""INS_123123"",
    ""installationType"": ""UT""
  }
}";

        #endregion

        #region Private Members

        private Container _container;
        private GISMessageBrokerSerializer _target;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container(InitializeContainer);

            _target = _container.GetInstance<GISMessageBrokerSerializer>();
        }

        #endregion

        #region Private Methods

        private void InitializeContainer(ConfigurationExpression e) { }

        private string ReadSchema()
        {
            var assembly = GetType().Assembly;
            var resourceName = "MapCallScheduler.Tests.JobHelpers.GISMessageBroker.GISMessageBrokerSchema.json";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
        
        private static IRepository<ConsolidatedCustomerSideMaterial> CreateMockedConsolidatedMaterialRepository()
        {
            var mock = new Mock<IRepository<ConsolidatedCustomerSideMaterial>>();

            var mockData = new List<ConsolidatedCustomerSideMaterial> {
                new ConsolidatedCustomerSideMaterial {
                    CustomerSideEPACode = new EPACode { Id = 2, Description = "MAPCALL_CUSTOMER_LEAD_EPA" },
                    ConsolidatedEPACode = new EPACode { Id = 1, Description = "LEAD" },
                    CustomerSideExternalEPACode = new EPACode { Id = 2, Description = "W1V_CUSTOMER_MATERIAL" }
                },
            };

            mock.Setup(repo => repo.Where(It.IsAny<Expression<Func<ConsolidatedCustomerSideMaterial, bool>>>()))
                .Returns((Expression<Func<ConsolidatedCustomerSideMaterial, bool>> predicate) => mockData.AsQueryable().Where(predicate));

            return mock.Object;
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestSerializeSerializesSampleSites()
        {
            IList<string> invalidReasons;

            var serialized = _target.Serialize(SAMPLE_SITE_INPUT, Formatting.Indented);

            Assert.AreEqual(SAMPLE_SITE_OUTPUT, serialized);

            var schema = ReadSchema();

            Assert.IsTrue(JObject.Parse(serialized).IsValid(JSchema.Parse(schema), out invalidReasons),
                $"JSON is not valid as per the schema for the following reasons:{Environment.NewLine}{string.Join(Environment.NewLine, invalidReasons)}");
        }

        [TestMethod]
        public void TestSerializeSerializesSewerMainCleaning()
        {
            var sewerMainCleaning = new SewerMainCleaning {
                Id = 10,
                InspectedDate = new DateTime(2021, 11, 2, 12, 15, 0),
                Opening1 = new SewerOpening { Id = 2, OpeningNumber = "123456" },
                Opening2 = new SewerOpening { Id = 6, OpeningNumber = "456789" },
                Date = new DateTime(2021, 10, 31, 10, 10, 0),
                OperatingCenter = new OperatingCenter {
                    Id = 11,
                    State = new State {
                        Id = 12,
                        Abbreviation = "abbreviation"
                    }
                }
            };

            var sewerOpeningConnection = new SewerOpeningConnection {
                UpstreamOpening = sewerMainCleaning.Opening1,
                DownstreamOpening = sewerMainCleaning.Opening2,
                InspectionFrequencyUnit = new RecurringFrequencyUnit {
                    Id = RecurringFrequencyUnit.Indices.DAY
                },
                InspectionFrequency = 1
            };

            sewerMainCleaning.Opening1.UpstreamSewerOpeningConnections.Add(sewerOpeningConnection);

            IList<string> invalidReasons;

            var serialized = _target.Serialize(sewerMainCleaning, Formatting.Indented);

            Assert.AreEqual(SEWER_MAIN_CLEANING_OUTPUT, serialized);

            var schema = ReadSchema();

            Assert.IsTrue(JObject.Parse(serialized).IsValid(JSchema.Parse(schema), out invalidReasons),
                $"JSON is not valid as per the schema for the following reasons:{Environment.NewLine}{string.Join(Environment.NewLine, invalidReasons)}");
        }
        
        [TestMethod]
        public void TestSerializeSerializesServiceRecord()
        {
            var service = new Service {
                PremiseNumber = "1233456",
                UpdatedAt = DateTime.Parse("1/1/2023"),
                DeviceLocation = "DL_123123",
                Installation = "INS_123123",
                UpdatedBy = new User {
                    Employee = new Employee { EmployeeId = "123" }
                },
                ServiceMaterial = new ServiceMaterial {
                    Description = "COMPANY_LEAD",
                    CompanyEPACode = new EPACode {
                        Description = "COMPANY_LEAD_EPA"
                    }
                },
                CustomerSideMaterial = new ServiceMaterial {
                    Description = "MAPCALL_CUSTOMER_LEAD",
                    CustomerEPACode = new EPACode {
                        Description = "MAPCALL_CUSTOMER_LEAD_EPA"
                    }
                },
                Premise = new Premise {
                    ConsolidatedCustomerConvertedMaterialsRepository = CreateMockedConsolidatedMaterialRepository(),
                    MostRecentService = new MostRecentlyInstalledService {
                        ServiceMaterial = new ServiceMaterial {
                            Id = 2,
                            Description = "serviceMaterial",
                            CompanyEPACode = new EPACode { Id = 1, Description = "COMPANY_LEAD" },
                            CustomerEPACode = new EPACode { Id = 2, Description = "W1V_CUSTOMER_MATERIAL" },
                        },
                        CustomerSideMaterial = new ServiceMaterial {
                            Id = 3,
                            Description = "customerSideMaterial",
                            CompanyEPACode = new EPACode { Id = 1, Description = "COMPANY_LEAD" },
                            CustomerEPACode = new EPACode { Id = 2, Description = "W1V_CUSTOMER_MATERIAL" },
                        }
                    },
                    Equipment = "EQ_123123",
                    DeviceLocation = "DL_123123",
                    Installation = "INS_123123",
                    ServiceUtilityType = new ServiceUtilityType {
                        Division = "UT"
                    },
                    OperatingCenter = new OperatingCenter { State = new State { Id = 1, Name = "NJ" } },
                    ShortCycleCustomerMaterials = new List<ShortCycleCustomerMaterial> {
                        new ShortCycleCustomerMaterial {
                            CustomerSideMaterial = new ServiceMaterial {
                                Description = "W1V_CUSTOMER_MATERIAL",
                                CustomerEPACode = new EPACode {
                                    Id = 2,
                                    Description = "W1V_CUSTOMER_MATERIAL_EPA"
                                }
                            }
                        }
                    }
                }
            };
            
            IList<string> invalidReasons;

            var serialized = _target.Serialize(service, Formatting.Indented);

            Assert.AreEqual(W1V_SERVICE_RECORD_OUTPUT, serialized);

            var schema = ReadSchema();

            Assert.IsTrue(JObject.Parse(serialized.ToLower()).IsValid(JSchema.Parse(schema.ToLower()), out invalidReasons),
                $"JSON is not valid as per the schema for the following reasons:{Environment.NewLine}{string.Join(Environment.NewLine, invalidReasons)}");
        }

        #endregion
    }
}
