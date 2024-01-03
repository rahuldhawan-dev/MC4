using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallScheduler.JobHelpers.GISMessageBroker.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using Moq;
using Employee = MapCall.Common.Model.Entities.Employee;
using ServiceMaterial = MapCall.Common.Model.Entities.ServiceMaterial;
using State = MapCall.Common.Model.Entities.State;
using OperatingCenter = MapCall.Common.Model.Entities.OperatingCenter;

namespace MapCallScheduler.Tests.JobHelpers.GISMessageBroker.Models
{
    [TestClass]
    public class W1VServiceRecordTests
    {
        #region Private Members

        private W1VServiceRecord _target;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _target = new W1VServiceRecord();
        }

        #endregion

        #region Private Methods

        private Service GetTestData()
        {
            return new Service {
                PremiseNumber = "1233456",
                DateInstalled = DateTime.Parse("1/1/2023"),
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

        [TestMethod]
        public void Test_FromDBRecord_ReturnsW1VServiceRecordFromService()
        {
            var service = GetTestData();
            var w1VServiceRecord = W1VServiceRecord.FromDbRecord(service);

            Assert.AreEqual(service.PremiseNumber, w1VServiceRecord.PremiseId);
            Assert.AreEqual(service.UpdatedBy.Employee.EmployeeId.ToString(), w1VServiceRecord.FsrId);
            Assert.AreEqual(service.UpdatedAt.ToString("d"), w1VServiceRecord.SubmitDate);
            Assert.AreEqual(service.Premise.Equipment, w1VServiceRecord.EquipmentId);
            Assert.AreEqual(service.DeviceLocation, w1VServiceRecord.DeviceLocationId);
            Assert.AreEqual(service.Installation, w1VServiceRecord.InstallationId);
            Assert.AreEqual(service.Premise.ServiceUtilityType.Division, w1VServiceRecord.InstallationType);

            // Assert Company side materials are mapped correctly
            Assert.AreEqual(service.ServiceMaterial.Description, w1VServiceRecord.CompanySideMaterial);
            Assert.AreEqual(service.ServiceMaterial.CompanyEPACode.Description,
                w1VServiceRecord.EpaConvertedCompanySideMaterial);

            // Assert W1V customer side materials are mapped correctly
            Assert.AreEqual(service.Premise.MostRecentCustomerMaterial.CustomerSideMaterial.Description,
                w1VServiceRecord.CustomerSideMaterial);
            Assert.AreEqual(service.Premise.MostRecentCustomerMaterial.CustomerSideMaterial.CustomerEPACode.Description,
                w1VServiceRecord.EpaConvertedCustomerSideMaterial);

            // Assert Mapcall customer side materials are mapped correctly
            Assert.AreEqual(service.CustomerSideMaterial.Description, w1VServiceRecord.CustomerSideMaterialExternal);
            Assert.AreEqual(service.CustomerSideMaterial.CustomerEPACode.Description,
                w1VServiceRecord.EpaConvertedCustomerSideMaterialExternal);
            
            Assert.AreEqual(service.Premise.ConsolidatedCustomerSideMaterial.Description,
                w1VServiceRecord.EpaConvertedConsolidatedCustomerSideMaterial);
        }

        [TestMethod]
        public void Test_FromDBRecord_GetsStateOverridenEPACodeWhenExists()
        {
            var service = GetTestData();
            var stateEPAOverride = new ServiceMaterialEPACodeOverride {
                State = service.Premise.OperatingCenter.State,
                CompanyEPACode = new EPACode {
                    Description = "STATE_OVERRIDDEN_COMPANY_EPA"
                },
                CustomerEPACode = new EPACode {
                    Description = "STATE_OVERRIDDEN_CUSTOMER_EPA"
                }
            };
            
            service.ServiceMaterial.ServiceMaterialEPACodeOverrides =
                service.CustomerSideMaterial.ServiceMaterialEPACodeOverrides =
                    service.Premise.MostRecentCustomerMaterial.CustomerSideMaterial.ServiceMaterialEPACodeOverrides =
                        new List<ServiceMaterialEPACodeOverride> { stateEPAOverride };

            var w1VServiceRecord = W1VServiceRecord.FromDbRecord(service);

            Assert.AreEqual(stateEPAOverride.CompanyEPACode.Description,
                w1VServiceRecord.EpaConvertedCompanySideMaterial);
            Assert.AreEqual(stateEPAOverride.CustomerEPACode.Description,
                w1VServiceRecord.EpaConvertedCustomerSideMaterial);
            Assert.AreEqual(stateEPAOverride.CustomerEPACode.Description,
                w1VServiceRecord.EpaConvertedCustomerSideMaterialExternal);
        }
    }
}
