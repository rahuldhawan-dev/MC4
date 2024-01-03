using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.NHibernate;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using MMSINC.Utilities.Pdf;
using StructureMap;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class PremiseTest : InMemoryDatabaseTest<Premise>
    {
        #region Init/Cleanup

        private DateTime _now;
        private TestDateTimeProvider _dateTimeProvider;

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>().Use(_dateTimeProvider = new TestDateTimeProvider());
            e.For<IAuthenticationService<User>>().Mock();
            e.For<IServiceRepository>().Use<ServiceRepository>();
            e.For<ITapImageRepository>().Use<TapImageRepository>();
            e.For<IImageToPdfConverter>().Mock();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _dateTimeProvider.SetNow(_now = DateTime.Now);

            foreach (var id in new[] {
                         MapIcon.Indices.PREMISE_BLUE,
                         MapIcon.Indices.PREMISE_ORANGE,
                         MapIcon.Indices.PREMISE_MABLUE,
                         MapIcon.Indices.PREMISE_BLUE_ORANGE
                     })
            {
                GetEntityFactory<MapIcon>().Create(new {
                    Id = id
                });
            }
        }

        protected override ITestDataFactoryService CreateFactoryService()
        {
            return new TestDataFactoryService(_container, typeof(PremiseFactory).Assembly);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestMostRecentService()
        {
            var target = GetEntityFactory<Premise>().Create();
            var s = GetEntityFactory<Service>().Create();
            var service1 = GetEntityFactory<Service>().Create(new {
                ServiceCategory = GetEntityFactory<ServiceCategory>().Create(),
                DateInstalled = _now.AddDays(-1),
                Premise = target
            });
            var service2 = GetEntityFactory<Service>().Create(new {
                ServiceCategory = GetEntityFactory<ServiceCategory>().Create(),
                DateInstalled = _now,
                Premise = target
            });
            Session.Flush();
            Session.Clear();

            target = Session.Load<Premise>(target.Id);

            Assert.AreEqual(service2.Id, target.MostRecentService.Service.Id);
        }

        [TestMethod]
        public void TestMostRecentCustomerMaterialDoesTheThing()
        {
            var target = GetEntityFactory<Premise>().Create();
            var scwo1 = GetEntityFactory<ShortCycleCustomerMaterial>().Create(new {
                TechnicalInspectedOn = _now,
                CustomerSideMaterial = typeof(ServiceMaterialFactory),
                Premise = target
            });
            var scwo2 = GetEntityFactory<ShortCycleCustomerMaterial>().Create(new {
                TechnicalInspectedOn = _now.AddDays(-1),
                CustomerSideMaterial = typeof(ServiceMaterialFactory),
                Premise = target
            });
            Session.Flush();
            Session.Clear();
        
            target = Session.Load<Premise>(target.Id);
        
            Assert.AreEqual(scwo1.Id, target.MostRecentCustomerMaterial?.Id);
        }

        [TestMethod]
        public void TestStreet()
        {
            var target = GetEntityFactory<Premise>().Create(new {
                ServiceAddressStreet = "address1"
            });
            var serviceCity = GetEntityFactory<Town>().Create();
            var street1 = new Street {
                FullStName = "Address1",
                Town = serviceCity,
                IsActive = true,
                Name = "street1"
            };
            var street2 = new Street {
                FullStName = "Address2",
                Town = serviceCity,
                IsActive = true,
                Name = "street2"
            };
            serviceCity.Streets.Add(street1);
            serviceCity.Streets.Add(street2);
            target.ServiceCity = serviceCity;

            Assert.AreEqual(street1, target.Street);
        }
        
        [TestMethod]
        public void TestServiceAddressStreetCanBeNull()
        {
            var target = GetEntityFactory<Premise>().Create();
            var serviceCity = GetEntityFactory<Town>().Create();
            var street1 = new Street {
                FullStName = "First St",
                Town = serviceCity,
                IsActive = true,
                Name = "First"
            };
            var street2 = new Street {
                FullStName = "Second St",
                Town = serviceCity,
                IsActive = true,
                Name = "Second"
            };
            serviceCity.Streets.Add(street1);
            serviceCity.Streets.Add(street2);
            target.ServiceCity = serviceCity;

            Assert.IsNull(target.Street);
        }

        [TestMethod]
        public void Test_MapIcon_IsBlueOrange_WhenIsMajorAccountAndCriticalCareTypeIsNotNull()
        {
            var premise = GetEntityFactory<Premise>().Create(new {
                IsMajorAccount = true,
                CriticalCareType = GetEntityFactory<PremiseCriticalCareType>().Create()
            });
            Session.Clear();
            premise = Session.Get<Premise>(premise.Id);
            
            Assert.AreEqual(MapIcon.Indices.PREMISE_BLUE_ORANGE, premise.Icon.Id);
        }

        [TestMethod]
        public void Test_MapIcon_IsMaBlue_WhenIsMajorAccountButCriticalCareTypeIsNull()
        {
            var premise = GetEntityFactory<Premise>().Create(new {
                IsMajorAccount = true,
                CriticalCareType = (PremiseCriticalCareType)null
            });
            Session.Clear();
            premise = Session.Get<Premise>(premise.Id);
            
            Assert.AreEqual(MapIcon.Indices.PREMISE_MABLUE, premise.Icon.Id);
        }

        [TestMethod]
        public void Test_MapIcon_IsOrange_WhenCriticalCareTypeIsNotNullButIsMajorAccountIsFalse()
        {
            var premise = GetEntityFactory<Premise>().Create(new {
                IsMajorAccount = false,
                CriticalCareType = GetEntityFactory<PremiseCriticalCareType>().Create()
            });
            Session.Clear();
            premise = Session.Get<Premise>(premise.Id);
            
            Assert.AreEqual(MapIcon.Indices.PREMISE_ORANGE, premise.Icon.Id);
        }

        [TestMethod]
        public void Test_MapIcon_IsBlue_WhenIsMajorAccountIsFalseAndCriticalCareTypeIsNull()
        {
            var premise = GetEntityFactory<Premise>().Create(new {
                IsMajorAccount = false,
                CriticalCareType = (PremiseCriticalCareType)null
            });
            Session.Clear();
            premise = Session.Get<Premise>(premise.Id);
            
            Assert.AreEqual(MapIcon.Indices.PREMISE_BLUE, premise.Icon.Id);
        }

        [TestMethod]
        public void Test_MostRecentServiceCustomerMaterialEPACode_ReturnsMaterialEPACode_IfStateOverrideDoesNotExists()
        {
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var epaCodeLead = GetEntityFactory<EPACode>().Create(new {
                Description = "LEAD"
            });
            var epaCodeNotLead = GetEntityFactory<EPACode>().Create(new {
                Description = "NOT LEAD"
            });
            
            var customerSideMaterial1 = GetEntityFactory<ServiceMaterial>().Create();
            customerSideMaterial1.CustomerEPACode = epaCodeNotLead;
            customerSideMaterial1.CompanyEPACode = epaCodeNotLead;
            
            var customerSideMaterial2 = GetEntityFactory<ServiceMaterial>().Create();

            // service material epa code is overridden for material 2
            var epaCodeOverride = GetEntityFactory<ServiceMaterialEPACodeOverride>().Create(new {
                State = operatingCenter.State,
                ServiceMaterial = customerSideMaterial2, 
                CustomerEPACode = epaCodeLead,
                CompanyEPACode = epaCodeLead
            });
            
            var target = GetEntityFactory<Premise>().Create(new {
                State = operatingCenter.State
            });
            var services = GetEntityFactory<Service>().Create(new {
                ServiceCategory = GetEntityFactory<ServiceCategory>().Create(),
                DateInstalled = _now.AddDays(-1),
                Premise = target,
                CustomerSideMaterial = customerSideMaterial1
            });
            
            Session.Flush();
            Session.Clear();

            target = Session.Load<Premise>(target.Id);
            
            // when not overridden, target should return material epa code
            Assert.AreEqual(customerSideMaterial1.CustomerEPACode.Description, target.MostRecentServiceCustomerMaterialEPACode.Description);
        }
        
        [TestMethod]
        public void Test_MostRecentServiceCustomerMaterialEPACode_ReturnsStateEPACode_IfStateOverrideExists()
        {
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var epaCodeLead = GetEntityFactory<EPACode>().Create(new {
                Description = "LEAD"
            });
            var epaCodeNotLead = GetEntityFactory<EPACode>().Create(new {
                Description = "NOT LEAD"
            });
            var customerSideMaterial = GetEntityFactory<ServiceMaterial>().Create();
            customerSideMaterial.CustomerEPACode = epaCodeNotLead;
            customerSideMaterial.CompanyEPACode = epaCodeNotLead;
            
            // service material epa code is overridden
            var epaCodeOverride = GetEntityFactory<ServiceMaterialEPACodeOverride>().Create(new {
                State = operatingCenter.State,
                ServiceMaterial = customerSideMaterial,
                CustomerEPACode = epaCodeLead,
                CompanyEPACode = epaCodeLead
            });
            
            var target = GetEntityFactory<Premise>().Create(new {
                State = operatingCenter.State
            });
            var services = GetEntityFactory<Service>().Create(new {
                ServiceCategory = GetEntityFactory<ServiceCategory>().Create(),
                DateInstalled = _now.AddDays(-1),
                Premise = target,
                CustomerSideMaterial = customerSideMaterial
            });
            
            Session.Flush();
            Session.Clear();

            target = Session.Load<Premise>(target.Id);
            
            // when overridden, target should return overridden code
            Assert.AreEqual(epaCodeOverride.CustomerEPACode.Description, target.MostRecentServiceCustomerMaterialEPACode.Description);
        }
        
        [TestMethod]
        public void Test_MostRecentCustomerMaterialEPACode_ReturnsMaterialEPACode_IfStateOverrideDoesNotExists()
        {
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var epaCodeLead = GetEntityFactory<EPACode>().Create(new {
                Description = "LEAD"
            });
            var epaCodeNotLead = GetEntityFactory<EPACode>().Create(new {
                Description = "NOT LEAD"
            });
            
            var customerSideMaterial1 = GetEntityFactory<ServiceMaterial>().Create();
            customerSideMaterial1.CustomerEPACode = epaCodeNotLead;
            customerSideMaterial1.CompanyEPACode = epaCodeNotLead;
            
            var customerSideMaterial2 = GetEntityFactory<ServiceMaterial>().Create();

            // service material epa code is overridden for material 2
            var epaCodeOverride = GetEntityFactory<ServiceMaterialEPACodeOverride>().Create(new {
                State = operatingCenter.State,
                ServiceMaterial = customerSideMaterial2, 
                CustomerEPACode = epaCodeLead,
                CompanyEPACode = epaCodeLead
            });
            
            var target = GetEntityFactory<Premise>().Create(new {
                State = operatingCenter.State
            });
            
            var shortCycleCustomerMaterial = GetEntityFactory<ShortCycleCustomerMaterial>().Create(new {
                TechnicalInspectedOn = _now,
                CustomerSideMaterial = customerSideMaterial1,
                Premise = target
            });
            
            Session.Flush();
            Session.Clear();

            target = Session.Load<Premise>(target.Id);
            
            // when not overridden, target should return material epa code
            Assert.AreEqual(customerSideMaterial1.CustomerEPACode.Description, target.MostRecentCustomerMaterialEPACode.Description);
        }

        [TestMethod]
        public void Test_MostRecentCustomerMaterialEPACode_ReturnsStateEPACode_IfStateOverrideExists()
        {
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var epaCodeLead = GetEntityFactory<EPACode>().Create(new {
                Description = "LEAD"
            });
            var epaCodeNotLead = GetEntityFactory<EPACode>().Create(new {
                Description = "NOT LEAD"
            });
            
            var customerSideMaterial1 = GetEntityFactory<ServiceMaterial>().Create();
            customerSideMaterial1.CustomerEPACode = epaCodeNotLead;
            customerSideMaterial1.CompanyEPACode = epaCodeNotLead;
            
            var customerSideMaterial2 = GetEntityFactory<ServiceMaterial>().Create();

            // service material epa code is overridden for material 2
            var epaCodeOverride = GetEntityFactory<ServiceMaterialEPACodeOverride>().Create(new {
                State = operatingCenter.State,
                ServiceMaterial = customerSideMaterial2, 
                CustomerEPACode = epaCodeLead,
                CompanyEPACode = epaCodeLead
            });
            
            var target = GetEntityFactory<Premise>().Create(new {
                State = operatingCenter.State
            });
            
            var shortCycleCustomerMaterial = GetEntityFactory<ShortCycleCustomerMaterial>().Create(new {
                TechnicalInspectedOn = _now,
                CustomerSideMaterial = customerSideMaterial2,
                Premise = target
            });
            
            Session.Flush();
            Session.Clear();

            target = Session.Load<Premise>(target.Id);
            
            // when not overridden, target should return material epa code
            Assert.AreEqual(epaCodeOverride.CustomerEPACode.Description, target.MostRecentCustomerMaterialEPACode.Description);
        }
        
        
        [TestMethod]
        public void Test_MostRecentServiceCompanyMaterialEPACode_ReturnsMaterialEPACode_IfStateOverrideDoesNotExists()
        {
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var epaCodeLead = GetEntityFactory<EPACode>().Create(new {
                Description = "LEAD"
            });
            var epaCodeNotLead = GetEntityFactory<EPACode>().Create(new {
                Description = "NOT LEAD"
            });
            
            var serviceMaterial1 = GetEntityFactory<ServiceMaterial>().Create();
            serviceMaterial1.CustomerEPACode = epaCodeNotLead;
            serviceMaterial1.CompanyEPACode = epaCodeNotLead;
            
            var customerSideMaterial2 = GetEntityFactory<ServiceMaterial>().Create();

            // service material epa code is overridden for material 2
            var epaCodeOverride = GetEntityFactory<ServiceMaterialEPACodeOverride>().Create(new {
                State = operatingCenter.State,
                ServiceMaterial = customerSideMaterial2, 
                CustomerEPACode = epaCodeLead,
                CompanyEPACode = epaCodeLead
            });

            var target = GetEntityFactory<Premise>().Create(new {
                OperatingCenter = operatingCenter
            });
            
            var services = GetEntityFactory<Service>().Create(new {
                ServiceCategory = GetEntityFactory<ServiceCategory>().Create(),
                DateInstalled = _now.AddDays(-1),
                Premise = target,
                ServiceMaterial = serviceMaterial1
            });
            
            Session.Flush();
            Session.Clear();

            target = Session.Load<Premise>(target.Id);
            
            // when not overridden, target should return material epa code
            Assert.AreEqual(serviceMaterial1.CustomerEPACode.Description, target.MostRecentServiceCompanyMaterialEPACode.Description);
        }
        
        [TestMethod]
        public void Test_MostRecentServiceCompanyMaterialEPACode_ReturnsStateEPACode_IfStateOverrideExists()
        {
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var epaCodeLead = GetEntityFactory<EPACode>().Create(new {
                Description = "LEAD"
            });
            var epaCodeNotLead = GetEntityFactory<EPACode>().Create(new {
                Description = "NOT LEAD"
            });
            var serviceMaterial = GetEntityFactory<ServiceMaterial>().Create();
            serviceMaterial.CustomerEPACode = epaCodeNotLead;
            serviceMaterial.CompanyEPACode = epaCodeNotLead;
            
            // service material epa code is overridden
            var epaCodeOverride = GetEntityFactory<ServiceMaterialEPACodeOverride>().Create(new {
                State = operatingCenter.State,
                ServiceMaterial = serviceMaterial,
                CustomerEPACode = epaCodeLead,
                CompanyEPACode = epaCodeLead
            });
            
            var target = GetEntityFactory<Premise>().Create(new {
                State = operatingCenter.State
            });
            var services = GetEntityFactory<Service>().Create(new {
                ServiceCategory = GetEntityFactory<ServiceCategory>().Create(),
                DateInstalled = _now.AddDays(-1),
                Premise = target,
                ServiceMaterial = serviceMaterial
            });
            
            Session.Flush();
            Session.Clear();

            target = Session.Load<Premise>(target.Id);
            
            // when overridden, target should return overridden code
            Assert.AreEqual(epaCodeOverride.CustomerEPACode.Description, target.MostRecentServiceCompanyMaterialEPACode.Description);
        }
        
        #endregion
    }
}
