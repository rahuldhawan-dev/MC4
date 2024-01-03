using System;
using System.Linq;
using FluentNHibernate.Visitors;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing;
using MMSINC.Testing.NHibernate;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;
using MMSINC.Utilities.Pdf;
using NHibernate.Criterion;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    [DeploymentItem(@"x64\SQLite.Interop.dll", "x64")]
    public class ServiceRepositoryTest : MapCallMvcSecuredRepositoryTestBase<Service, ServiceRepository, User>
    {
        #region Constants

        public const RoleModules ROLE_MODULE = RoleModules.FieldServicesAssets;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<ITapImageRepository>().Use<TapImageRepository>();
            e.For<IImageToPdfConverter>().Mock();
            e.For<IIconSetRepository>().Use<IconSetRepository>();
            e.For<IDateTimeProvider>().Mock();
        }

        protected override User CreateUser()
        {
            return GetFactory<UserFactory>().Create(new {IsAdmin = true});
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestGetServicesRenewedReturnsAggregateOfData()
        {
            //each services collection (1,2,3...) below is setup to be in order so result testing is simpler
            //i.e. switching cat1 and 2's descriptions will result in a test failure
            var opCntr = GetFactory<OperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>().Create();
            var serviceCategory1 = GetFactory<ServiceCategoryFactory>().Create(new {Description = "A"});
            var serviceCategory2 = GetFactory<ServiceCategoryFactory>().Create(new {Description = "B"});
            var serviceSize1 = GetFactory<ServiceSizeFactory>().Create(new {ServiceSizeDescription = "2", Size = 2m});
            var serviceSize2 = GetFactory<ServiceSizeFactory>().Create(new {ServiceSizeDescription = "15", Size = 15m});

            var services1 = GetFactory<ServiceFactory>().CreateList(6, new {
                OperatingCenter = opCntr,
                Town = town,
                ServiceCategory = serviceCategory1,
                ServiceSize = serviceSize1
            });
            var services2 = GetFactory<ServiceFactory>().CreateList(5, new {
                OperatingCenter = opCntr,
                Town = town,
                ServiceCategory = serviceCategory1,
                ServiceSize = serviceSize2
            });
            var services3 = GetFactory<ServiceFactory>().CreateList(4, new {
                OperatingCenter = opCntr,
                Town = town,
                ServiceCategory = serviceCategory2,
                ServiceSize = serviceSize1
            });

            // ENSURE THAT NHIBERNATE CAN ACTUALLY QUERY FOR THIS.
            Session.Clear();
            var search = new TestGetServicesRenewed();
            var results = Repository.GetServicesRenewed(search).ToList();

            Assert.AreEqual(3, results.Count());
            Assert.AreEqual(results[0].ServiceCount, services1.Count);
            Assert.AreEqual(serviceSize1.ServiceSizeDescription, results[0].ServiceSize.ServiceSizeDescription);

            Assert.AreEqual(results[1].ServiceCount, services2.Count);
            Assert.AreEqual(serviceSize2.ServiceSizeDescription, results[1].ServiceSize.ServiceSizeDescription);

            Assert.AreEqual(results[2].ServiceCount, services3.Count);
            Assert.AreEqual(serviceSize1.ServiceSizeDescription, results[2].ServiceSize.ServiceSizeDescription);
        }

        [TestMethod]
        public void TestGetServicesRenewedReturnsFilteredAggregateOfData()
        {
            //each services collection (1,2,3...) below is setup to be in order so result testing is simpler
            //i.e. switching cat1 and 2's descriptions will result in a test failure
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opCntr2 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>().Create();
            var town2 = GetFactory<TownFactory>().Create();
            var serviceCategory1 = GetFactory<ServiceCategoryFactory>().Create(new {Description = "A"});
            var serviceCategory2 = GetFactory<ServiceCategoryFactory>().Create(new {Description = "B"});
            var serviceSize1 = GetFactory<ServiceSizeFactory>().Create(new {ServiceSizeDescription = "2", Size = 2m});
            var serviceSize2 = GetFactory<ServiceSizeFactory>().Create(new {ServiceSizeDescription = "15", Size = 15m});

            var services1 = GetFactory<ServiceFactory>().CreateList(6, new {
                OperatingCenter = opCntr,
                Town = town,
                ServiceCategory = serviceCategory1,
                ServiceSize = serviceSize1
            });
            var services2 = GetFactory<ServiceFactory>().CreateList(5, new {
                OperatingCenter = opCntr2,
                Town = town2,
                ServiceCategory = serviceCategory1,
                ServiceSize = serviceSize2
            });
            var services3 = GetFactory<ServiceFactory>().CreateList(4, new {
                OperatingCenter = opCntr,
                Town = town,
                ServiceCategory = serviceCategory2,
                ServiceSize = serviceSize1
            });

            // ENSURE THAT NHIBERNATE CAN ACTUALLY QUERY FOR THIS.
            Session.Clear();

            var search = new TestGetServicesRenewed();
            search.OperatingCenter = opCntr2.Id;
            search.Town = town2.Id;
            var results = Repository.GetServicesRenewed(search).ToList();

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(results[0].ServiceCount, services2.Count);
            Assert.AreEqual(serviceSize2.Size, results[0].ServiceSize.Size);
        }

        [TestMethod]
        public void TestFindByServiceNumberAndPremiseNumberReturnsMatch()
        {
            var goodServ = GetFactory<ServiceFactory>().Create(new {
                ServiceNumber = (long?)11111,
                PremiseNumber = "99999"
            });
            // Make sure it doesn't match only on ServiceNumber
            var badServ = GetFactory<ServiceFactory>().Create(new {
                ServiceNumber = (long?)11111,
                PremiseNumber = "22222"
            });

            // Make sure it doesn't match only on PremiseNumber
            var anotherBadServ = GetFactory<ServiceFactory>().Create(new {
                ServiceNumber = (long?)22222,
                PremiseNumber = "99999"
            });

            var result = Repository.FindByPremiseNumberAndServiceNumber("11111", "99999");
            Assert.AreSame(goodServ, result);
        }

        [TestMethod]
        public void TestFindByServiceNumberAndPremiseNumberReturnsNullIfNoMatch()
        {
            var unexpected = GetFactory<ServiceFactory>().Create(new {
                ServiceNumber = (long?)11111,
                PremiseNumber = "99999"
            });

            var result = Repository.FindByPremiseNumberAndServiceNumber("23525", "23523");
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestFindByServiceNumberAndPremiseNumberReturnsMostRecentMatchAKATheHighestIdValue()
        {
            var first = GetFactory<ServiceFactory>().Create(new {
                ServiceNumber = (long?)11111,
                PremiseNumber = "99999"
            });
            // Make sure it doesn't match only on ServiceNumber
            var second = GetFactory<ServiceFactory>().Create(new {
                ServiceNumber = (long?)11111,
                PremiseNumber = "99999"
            });

            var result = Repository.FindByPremiseNumberAndServiceNumber("11111", "99999");
            Assert.AreSame(second, result);
        }

        [TestMethod]
        public void
            TestFindByServiceNumberAndPremiseNumberReturnsMostRecentMatchAKATheHighestIdValueWhenServiceNumberNull()
        {
            var first = GetFactory<ServiceFactory>().Create(new {
                ServiceNumber = (long?)null,
                PremiseNumber = "99999"
            });
            // Make sure it doesn't match only on ServiceNumber
            var second = GetFactory<ServiceFactory>().Create(new {
                ServiceNumber = (long?)null,
                PremiseNumber = "99999"
            });

            var result = Repository.FindByPremiseNumber("99999");
            Assert.AreSame(second, result);
        }

        [TestMethod]
        public void TestFindByStreetIdReturnsServicesWithMatchingStreetIds()
        {
            var street = GetFactory<StreetFactory>().Create();
            var goodServ = GetFactory<ServiceFactory>().Create(new {
                Street = street
            });
            var badServ = GetFactory<ServiceFactory>().Create(new {
                Street = typeof(StreetFactory)
            });

            var result = Repository.FindByStreetId(street.Id);
            Assert.AreSame(goodServ, result.Single());
        }

        [TestMethod]
        public void TestGetNextServiceNumberGetsNextServiceNumberForOperatingCenterCorrectly()
        {
            var operatingCenterNJ3 = GetEntityFactory<OperatingCenter>().Create(new {OperatingCenterCode = "NJ3"});
            var operatingCenterNJ4 = GetEntityFactory<OperatingCenter>().Create(new {OperatingCenterCode = "NJ4"});

            var nj3service =
                GetEntityFactory<Service>()
                   .Create(new {OperatingCenter = operatingCenterNJ3, ServiceNumber = (long?)0});
            var nj4service =
                GetEntityFactory<Service>().Create(new
                    {OperatingCenter = operatingCenterNJ4, ServiceNumber = (long?)25111111});

            var expected = Repository.GetNextServiceNumber(operatingCenterNJ3.OperatingCenterCode);
            Assert.AreEqual(1, expected);

            expected = Repository.GetNextServiceNumber(operatingCenterNJ4.OperatingCenterCode);
            Assert.AreEqual(25111112, expected);
        }

        [TestMethod]
        public void TestGetNextServiceNumberIgnoringNulls()
        {
            var operatingCenterNJ3 = GetEntityFactory<OperatingCenter>().Create(new {OperatingCenterCode = "NJ3"});

            var nj3service =
                GetEntityFactory<Service>()
                   .Create(new {OperatingCenter = operatingCenterNJ3, ServiceNumber = (long?)4});
            var nj3service2 =
                GetEntityFactory<Service>().Create(new
                    {OperatingCenter = operatingCenterNJ3, ServiceNumber = (long?)null});
            var nj3service3 =
                GetEntityFactory<Service>()
                   .Create(new {OperatingCenter = operatingCenterNJ3, ServiceNumber = (long?)3});
            var nj3service4 =
                GetEntityFactory<Service>().Create(new
                    {OperatingCenter = operatingCenterNJ3, ServiceNumber = (long?)null});

            var expected = Repository.GetNextServiceNumber(operatingCenterNJ3.OperatingCenterCode);
            Assert.AreEqual(5, expected);
        }

        [TestMethod]
        public void TestSaveRefreshesFormulaColumnsOnEntity()
        {
            var street = GetEntityFactory<Street>().Create();
            var target = GetFactory<ServiceFactory>()
               .BuildWithConcreteDependencies(new {StreetNumber = "12", Street = street});

            Session.Save(target);
            Assert.IsNull(target.StreetAddress);
            Assert.IsNotNull(target.Street);
            Repository.Save(target);
            Assert.IsNotNull(target.StreetAddress);
            Assert.IsNotNull(target.Street);
        }

        #region FindManyByWorkOrder

        [TestMethod]
        public void TestFindManyByWorkOrderReturnsServicesWhenWorkOrderHasAServiceNumber()
        {
            var premiseNumber = "000000000";
            var operatingCenter = GetFactory<OperatingCenterFactory>().Create();
            var town = GetEntityFactory<Town>().Create();
            var workOrder = GetEntityFactory<WorkOrder>().Create(new {
                PremiseNumber = premiseNumber,
                ServiceNumber = "123",
                OperatingCenter = operatingCenter,
                Town = town
            });
            var service = GetEntityFactory<Service>().Create(new {
                ServiceNumber = 123L,
                PremiseNumber = premiseNumber,
                OperatingCenter = operatingCenter,
                Town = town
            });

            var result = Repository.FindManyByWorkOrder(workOrder);
            
            Assert.AreEqual(result.FirstOrDefault().Id, service.Id);
        }

        [TestMethod]
        public void TestFindManyByWorkOrderReturnsServicesWorkOrderDoesNotHaveAServiceNumber()
        {
            var premiseNumber = "000000000";
            var operatingCenter = GetFactory<OperatingCenterFactory>().Create();
            var town = GetEntityFactory<Town>().Create();
            var workOrder = GetEntityFactory<WorkOrder>().Create(new {
                PremiseNumber = premiseNumber,
                OperatingCenter = operatingCenter,
                Town = town
            });
            var service = GetEntityFactory<Service>().Create(new {
                ServiceNumber = 123L,
                PremiseNumber = premiseNumber,
                OperatingCenter = operatingCenter,
                Town = town
            });

            var result = Repository.FindManyByWorkOrder(workOrder);

            Assert.AreEqual(result.FirstOrDefault().Id, service.Id);
        }

        [TestMethod]
        public void TestFindManyByWorkOrderReturnsEmptyWhenNoServiceExistsForWorkOrder()
        {
            var premiseNumber = "000000000";
            var operatingCenter = GetFactory<OperatingCenterFactory>().Create();
            var town = GetEntityFactory<Town>().Create();
            var workOrder = GetEntityFactory<WorkOrder>().Create(new {
                PremiseNumber = premiseNumber,
                ServiceNumber = "123",
                OperatingCenter = operatingCenter,
                Town = town
            });

            var result = Repository.FindManyByWorkOrder(workOrder);

            MyAssert.IsEmpty(result);
        }

        #endregion

        #endregion

        #region Reports

        #region Monthly Services Installed By Category

        [TestMethod]
        public void TestGetMonthlyServicesInstalledByCategoryReportWorksCorrectlyWithAllItsMadness()
        {
            var operatingCenter1 =
                GetFactory<UniqueOperatingCenterFactory>()
                   .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var operatingCenter2 =
                GetFactory<UniqueOperatingCenterFactory>()
                   .Create(new {OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury"});
            var serviceCategory1 =
                GetEntityFactory<ServiceCategory>().Create(new {Description = "Fire Service Retire Only"});
            var serviceCategory2 =
                GetEntityFactory<ServiceCategory>().Create(new {Description = "Fire Service Installation"});

            // the good
            var service1 =
                GetEntityFactory<Service>()
                   .Create(
                        new {
                            OperatingCenter = operatingCenter1,
                            ServiceCategory = serviceCategory2,
                            DateInstalled = new DateTime(2016, 1, 1)
                        });
            var service2 =
                GetEntityFactory<Service>()
                   .Create(
                        new {
                            OperatingCenter = operatingCenter1,
                            ServiceCategory = serviceCategory2,
                            DateInstalled = new DateTime(2016, 2, 2)
                        });
            var service21 =
                GetEntityFactory<Service>()
                   .Create(
                        new {
                            OperatingCenter = operatingCenter2,
                            ServiceCategory = serviceCategory2,
                            DateInstalled = new DateTime(2016, 1, 1)
                        });
            var service22 =
                GetEntityFactory<Service>()
                   .Create(
                        new {
                            OperatingCenter = operatingCenter2,
                            ServiceCategory = serviceCategory2,
                            DateInstalled = new DateTime(2016, 2, 2)
                        });
            // the bad
            var service3 =
                GetEntityFactory<Service>()
                   .Create(
                        new {
                            OperatingCenter = operatingCenter1,
                            ServiceCategory = serviceCategory1,
                            DateInstalled = new DateTime(2016, 1, 1)
                        });
            var service4 =
                GetEntityFactory<Service>()
                   .Create(
                        new {
                            OperatingCenter = operatingCenter1,
                            ServiceCategory = serviceCategory2,
                            DateInstalled = new DateTime(2015, 1, 1)
                        });
            var service5 =
                GetEntityFactory<Service>()
                   .Create(
                        new {
                            OperatingCenter = operatingCenter1,
                            ServiceCategory = serviceCategory2,
                            DateInstalled = new DateTime(2015, 1, 1)
                        });

            var results =
                Repository.GetMonthlyServicesInstalledByCategoryReport(
                    new TestSearchMonthlyServicesInstalledByCategory {Year = 2016});

            Assert.AreEqual(4, results.Count());

            // Assert.Fail("This'll never work");
        }

        #endregion

        #region BPU Report for Services

        [TestMethod]
        public void TestBPUReportForServices()
        {
            var operatingCenter1 = GetFactory<UniqueOperatingCenterFactory>().Create(new {
                OperatingCenterCode = "NJ4",
                OperatingCenterName = "Lakewood"
            });
            var operatingCenter2 = GetFactory<UniqueOperatingCenterFactory>().Create(new {
                OperatingCenterCode = "NJ7",
                OperatingCenterName = "Shrewsbury"
            });
            var serviceCategory1 = GetEntityFactory<ServiceCategory>().Create(new {
                Description = "Fire Service Retire Only"
            });

            var serviceSize = GetEntityFactory<ServiceSize>().Create();
            var serviceMaterial = GetEntityFactory<ServiceMaterial>().Create();
            var categoryOfServiceGroups = GetEntityFactory<CategoryOfServiceGroup>().CreateList(3);

            var serviceType1 =
                GetEntityFactory<ServiceType>()
                   .Create(
                        new {
                            Description = "Foo",
                            OperatingCenter = operatingCenter1,
                            ServiceCategory = serviceCategory1,
                            CategoryOfServiceGroup = categoryOfServiceGroups[0]
                        });
            var serviceType2 =
                GetEntityFactory<ServiceType>()
                   .Create(
                        new {
                            Description = "Foo",
                            OperatingCenter = operatingCenter2,
                            ServiceCategory = serviceCategory1,
                            CategoryOfServiceGroup = categoryOfServiceGroups[1]
                        });

            Session.Flush();
            Session.Clear();

            // the good
            var service1 = GetEntityFactory<Service>().Create(new {
                OperatingCenter = operatingCenter1,
                ServiceCategory = serviceCategory1,
                DateInstalled = new DateTime(2016, 1, 1),
                ServiceMaterial = serviceMaterial,
                ServiceSize = serviceSize
            });
            var service2 = GetEntityFactory<Service>().Create(new {
                OperatingCenter = operatingCenter1,
                ServiceCategory = serviceCategory1,
                DateInstalled = new DateTime(2016, 2, 2),
                ServiceMaterial = serviceMaterial,
                ServiceSize = serviceSize
            });
            var service3 = GetEntityFactory<Service>().Create(new {
                OperatingCenter = operatingCenter1,
                ServiceCategory = serviceCategory1,
                DateInstalled = new DateTime(2016, 1, 1),
                ServiceMaterial = serviceMaterial,
                ServiceSize = serviceSize
            });
            var service4 = GetEntityFactory<Service>().Create(new {
                OperatingCenter = operatingCenter2,
                ServiceCategory = serviceCategory1,
                DateInstalled = new DateTime(2016, 2, 2),
                ServiceMaterial = serviceMaterial,
                ServiceSize = serviceSize
            });
            var search = new TestSearchBPUReportForServices {Year = 2016};

            var results = Repository.GetBPUReportForServices(search).ToList();

            Assert.AreEqual(2, results.Count());
            Assert.AreEqual(3, results[0].InstalledNew);
            Assert.AreEqual(0, results[0].Replaced);
            Assert.AreEqual(0, results[1].InstalledNew);
            Assert.AreEqual(1, results[1].Replaced);
        }

        #endregion

        #region Services Retired

        [TestMethod]
        public void TestGetServicesRetiredReturnsServicesRetired()
        {
            var operatingCenter1 =
                GetFactory<UniqueOperatingCenterFactory>()
                   .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var town = GetEntityFactory<Town>().Create();
            var serviceSize = GetEntityFactory<ServiceSize>()
               .Create(new {ServiceSizeDescription = "3/5", Size = 0.75m});
            var serviceCategory1 =
                GetEntityFactory<ServiceCategory>().Create(new {Description = "Fire Service Retire Only"});
            var service1 =
                GetEntityFactory<Service>()
                   .Create(
                        new {
                            ServiceCategory = serviceCategory1,
                            OperatingCenter = operatingCenter1,
                            Town = town,
                            PreviousServiceSize = serviceSize
                        });
            var service2 =
                GetEntityFactory<Service>()
                   .Create(
                        new {
                            ServiceCategory = serviceCategory1,
                            OperatingCenter = operatingCenter1,
                            Town = town,
                            PreviousServiceSize = serviceSize
                        });

            var search = new TestSearchServicesRetired();

            var results = Repository.GetServicesRetired(search).ToList();

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(2, results[0].Total);
        }

        #endregion

        #region Services Renewed

        [TestMethod]
        public void TestGetServicesRenewedReturnsGroupedServicesRenewed()
        {
            var installed = new DateTime(2016, 3, 14);
            var operatingCenter1 = GetFactory<UniqueOperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var town = GetEntityFactory<Town>().Create();
            var serviceSize = GetEntityFactory<ServiceSize>()
               .Create(new {ServiceSizeDescription = "3/5", Size = 0.75m});
            var serviceCategories = GetEntityFactory<ServiceCategory>().CreateList(23);
            var service1 = GetEntityFactory<Service>().Create(new {
                ServiceNumber = 12312412414,
                ServiceCategory = serviceCategories[22],
                OperatingCenter = operatingCenter1,
                Town = town,
                PreviousServiceSize = serviceSize,
                DateInstalled = installed
            });
            var service2 = GetEntityFactory<Service>().Create(new {
                ServiceNumber = 7777777777,
                ServiceCategory = serviceCategories[2],
                OperatingCenter = operatingCenter1,
                Town = town,
                PreviousServiceSize = serviceSize,
                DateInstalled = installed
            });
            var service3 = GetEntityFactory<Service>().Create(new {
                ServiceCategory = serviceCategories[2],
                OperatingCenter = operatingCenter1,
                Town = town,
                PreviousServiceSize = serviceSize,
                DateInstalled = installed.AddYears(-1)
            });
            var service4 = GetEntityFactory<Service>().Create(new {
                ServiceCategory = serviceCategories[2],
                OperatingCenter = operatingCenter1,
                Town = town,
                PreviousServiceSize = serviceSize,
                DateInstalled = installed.AddYears(-2)
            });

            var search = new TestSearchServicesRenewedSummary();
            var results = Repository.GetServicesRenewedSummary(search).ToList();

            Assert.AreEqual(3, results.Count());
            Assert.AreEqual(1, results[0].Total);
            Assert.AreEqual(1, results[1].Total);
            Assert.AreEqual(2, results[2].Total);
        }

        #endregion

        #region ServicesCompletedByCategory

        [TestMethod]
        public void TestServicesCompletedByCategoryReturns()
        {
            var installed = new DateTime(2016, 3, 14);
            var operatingCenter1 = GetFactory<UniqueOperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var town = GetEntityFactory<Town>().Create();
            var serviceSize = GetEntityFactory<ServiceSize>()
               .Create(new {ServiceSizeDescription = "3/5", Size = 0.75m});
            var serviceCategories = GetEntityFactory<ServiceCategory>().CreateList(23);
            var service1 = GetEntityFactory<Service>().Create(new {
                ServiceCategory = serviceCategories[22],
                OperatingCenter = operatingCenter1,
                Town = town,
                PreviousServiceSize = serviceSize,
                DateInstalled = installed
            });
            var service2 = GetEntityFactory<Service>().Create(new {
                ServiceCategory = serviceCategories[1],
                OperatingCenter = operatingCenter1,
                Town = town,
                PreviousServiceSize = serviceSize,
                DateInstalled = installed
            });
            var service3 = GetEntityFactory<Service>().Create(new {
                ServiceCategory = serviceCategories[2],
                OperatingCenter = operatingCenter1,
                Town = town,
                PreviousServiceSize = serviceSize,
                DateInstalled = installed.AddYears(-1)
            });
            var service4 = GetEntityFactory<Service>().Create(new {
                ServiceCategory = serviceCategories[2],
                OperatingCenter = operatingCenter1,
                Town = town,
                PreviousServiceSize = serviceSize,
                DateInstalled = installed.AddYears(-2)
            });

            var search = new TestSearchServicesCompletedByCategory();
            var results = Repository.GetServicesCompletedByCategory(search).ToList();

            Assert.AreEqual(3, results.Count());
            Assert.AreEqual(1, results[0].Total);
            Assert.AreEqual(2, results[1].Total);
            Assert.AreEqual(1, results[2].Total);
        }

        #endregion

        #region T/D Pending Services KPI

        [TestMethod]
        public void TestTDPendingServicesKPIReportReturnsResults()
        {
            var now = DateTime.Now;
            var operatingCenter1 = GetFactory<UniqueOperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var town = GetEntityFactory<Town>().Create();
            var serviceSize = GetEntityFactory<ServiceSize>()
               .Create(new {ServiceSizeDescription = "3/5", Size = 0.75m});
            var serviceCategories = GetEntityFactory<ServiceCategory>().CreateList(23);
            var serviceStatuses = GetEntityFactory<ServiceStatus>().CreateList(2);
            var serviceInstallationPurpose = GetEntityFactory<ServiceInstallationPurpose>()
               .Create(new {Description = "Not Main Replacement"});
            var service1 = GetEntityFactory<Service>().Create(new {
                DateInstalled = (DateTime?)null,
                ServiceCategory =
                    serviceCategories.FirstOrDefault(x => x.Id == ServiceCategory.Indices.WATER_SERVICE_RENEWAL),
                OperatingCenter = operatingCenter1,
                Town = town,
                PreviousServiceSize = serviceSize,
                PermitSentDate = now,
                ServiceInstallationPurpose = serviceInstallationPurpose,
                IsActive = true
            });

            var service2 = GetEntityFactory<Service>().Create(new {
                DateInstalled = (DateTime?)null,
                ServiceCategory =
                    serviceCategories.FirstOrDefault(x => x.Id == ServiceCategory.Indices.WATER_SERVICE_RENEWAL),
                OperatingCenter = operatingCenter1,
                Town = town,
                PreviousServiceSize = serviceSize,
                DateIssuedToField = now,
                ServiceInstallationPurpose = serviceInstallationPurpose,
                IsActive = true
            });

            var service3 = GetEntityFactory<Service>().Create(new {
                DateInstalled = (DateTime?)null,
                ServiceCategory =
                    serviceCategories.FirstOrDefault(x => x.Id == ServiceCategory.Indices.WATER_SERVICE_NEW_DOMESTIC),
                OperatingCenter = operatingCenter1,
                ServiceInstallationPurpose = serviceInstallationPurpose,
                IsActive = true,
                ApplicationApprovedOn = now,
                ServiceStatus = serviceStatuses[0],
                Town = town,
                PreviousServiceSize = serviceSize
            });

            var service4 = GetEntityFactory<Service>().Create(new {
                DateInstalled = (DateTime?)null,
                ServiceCategory =
                    serviceCategories.FirstOrDefault(x => x.Id == ServiceCategory.Indices.WATER_SERVICE_NEW_DOMESTIC),
                OperatingCenter = operatingCenter1,
                ServiceInstallationPurpose = serviceInstallationPurpose,
                IsActive = true,
                PermitSentDate = now,
                DeveloperServicesDriven = false,
                ServiceStatus = serviceStatuses[0],
                Town = town,
                PreviousServiceSize = serviceSize
            });

            var service5 = GetEntityFactory<Service>().Create(new {
                DateInstalled = (DateTime?)null,
                ServiceCategory =
                    serviceCategories.FirstOrDefault(x => x.Id == ServiceCategory.Indices.WATER_SERVICE_NEW_DOMESTIC),
                OperatingCenter = operatingCenter1,
                ServiceInstallationPurpose = serviceInstallationPurpose,
                IsActive = true,
                DateIssuedToField = now,
                DeveloperServicesDriven = false,
                ServiceStatus = serviceStatuses[0],
                Town = town,
                PreviousServiceSize = serviceSize
            });

            var service6 = GetEntityFactory<Service>().Create(new {
                DateInstalled = (DateTime?)null,
                ServiceCategory =
                    serviceCategories.FirstOrDefault(x => x.Id == ServiceCategory.Indices.WATER_SERVICE_NEW_DOMESTIC),
                OperatingCenter = operatingCenter1,
                ServiceInstallationPurpose = serviceInstallationPurpose,
                IsActive = true,
                DateIssuedToField = now,
                DeveloperServicesDriven = false,
                ServiceStatus = serviceStatuses[1],
                Town = town,
                PreviousServiceSize = serviceSize
            });

            var service7 = GetEntityFactory<Service>().Create(new {
                DateInstalled = (DateTime?)null,
                ServiceCategory =
                    serviceCategories.FirstOrDefault(x => x.Id == ServiceCategory.Indices.SEWER_SERVICE_NEW),
                OperatingCenter = operatingCenter1,
                ServiceInstallationPurpose = serviceInstallationPurpose,
                IsActive = true,
                DateIssuedToField = now,
                DeveloperServicesDriven = false,
                Town = town,
            });

            var contractor1 = GetEntityFactory<ServiceRestorationContractor>()
               .Create(new {Contractor = "MMSI", OperatingCenter = operatingCenter1});
            var service8 = GetEntityFactory<Service>().Create(new {
                DateInstalled = (DateTime?)null,
                ServiceCategory =
                    serviceCategories.FirstOrDefault(x => x.Id == ServiceCategory.Indices.WATER_SERVICE_NEW_COMMERCIAL),
                WorkIssuedTo = contractor1,
                ServiceInstallationPurpose = serviceInstallationPurpose,
                OperatingCenter = operatingCenter1,
                DateIssuedToField = now,
                DeveloperServicesDriven = false
            });
            var contractor2 = GetEntityFactory<ServiceRestorationContractor>()
               .Create(new {Contractor = "Xymetrex", OperatingCenter = operatingCenter1});
            var service9 = GetEntityFactory<Service>().Create(new {
                DateInstalled = (DateTime?)null,
                ServiceCategory =
                    serviceCategories.FirstOrDefault(x => x.Id == ServiceCategory.Indices.WATER_SERVICE_NEW_DOMESTIC),
                WorkIssuedTo = contractor2,
                ServiceInstallationPurpose = serviceInstallationPurpose,
                OperatingCenter = operatingCenter1,
                DateIssuedToField = now,
                DeveloperServicesDriven = false
            });

            var search = new TestTDPendingServicesKPI {OperatingCenter = operatingCenter1.Id};

            var results = Repository.GetTDPendingServicesKPI(search).OrderByDescending(x => x.Section).ToList();

            Assert.AreEqual(9, results.Count());
            // 0
            Assert.AreEqual(TDPendingServicesKPIReportItem.SECTION_SERVICE, results[0].Section);
            Assert.AreEqual(TDPendingServicesKPIReportItem.Category.WATER_SERVICE_RENEWAL_PENDING_PERMITS,
                results[0].ServicesContractor);
            Assert.AreEqual(1, results[0].Total,
                TDPendingServicesKPIReportItem.Category.WATER_SERVICE_RENEWAL_PENDING_PERMITS);
            // 1
            Assert.AreEqual(TDPendingServicesKPIReportItem.SECTION_SERVICE, results[1].Section);
            Assert.AreEqual(TDPendingServicesKPIReportItem.Category.WATER_SERVICE_ISSUED_TO_FIELD,
                results[1].ServicesContractor);
            Assert.AreEqual(1, results[1].Total, TDPendingServicesKPIReportItem.Category.WATER_SERVICE_ISSUED_TO_FIELD);
            // 2
            Assert.AreEqual(TDPendingServicesKPIReportItem.SECTION_SERVICE, results[2].Section);
            Assert.AreEqual(TDPendingServicesKPIReportItem.Category.NEW_WATER_SERVICES_APPROVED_APPLICATION,
                results[2].ServicesContractor);
            Assert.AreEqual(1, results[2].Total,
                TDPendingServicesKPIReportItem.Category.NEW_WATER_SERVICES_APPROVED_APPLICATION);
            // 3
            Assert.AreEqual(TDPendingServicesKPIReportItem.SECTION_SERVICE, results[3].Section);
            Assert.AreEqual(TDPendingServicesKPIReportItem.Category.NEW_WATER_SERVICES_PERMITS_PENDING,
                results[3].ServicesContractor);
            Assert.AreEqual(1, results[3].Total,
                TDPendingServicesKPIReportItem.Category.NEW_WATER_SERVICES_PERMITS_PENDING);
            // 4
            Assert.AreEqual(TDPendingServicesKPIReportItem.SECTION_SERVICE, results[4].Section);
            Assert.AreEqual(TDPendingServicesKPIReportItem.Category.NEW_WATER_SERVICES_ISSUED_TO_FIELD,
                results[4].ServicesContractor);
            Assert.AreEqual(1, results[4].Total,
                TDPendingServicesKPIReportItem.Category.NEW_WATER_SERVICES_ISSUED_TO_FIELD);
            // 5
            Assert.AreEqual(TDPendingServicesKPIReportItem.SECTION_SERVICE, results[5].Section);
            Assert.AreEqual(TDPendingServicesKPIReportItem.Category.NEW_WATER_SERVICES_SITE_NOT_READY,
                results[5].ServicesContractor);
            Assert.AreEqual(1, results[5].Total,
                TDPendingServicesKPIReportItem.Category.NEW_WATER_SERVICES_SITE_NOT_READY);
            // 6
            Assert.AreEqual(TDPendingServicesKPIReportItem.SECTION_SERVICE, results[6].Section);
            Assert.AreEqual(TDPendingServicesKPIReportItem.Category.SEWER_SERVICES, results[6].ServicesContractor);
            Assert.AreEqual(1, results[6].Total, TDPendingServicesKPIReportItem.Category.SEWER_SERVICES);
            // Contractor
            // 7
            Assert.AreEqual(TDPendingServicesKPIReportItem.SECTION_CONTRACTOR, results[7].Section);
            Assert.AreEqual(contractor1.Contractor, results[7].ServicesContractor);
            Assert.AreEqual(1, results[7].Total, "Contractor 1");
            // 8
            Assert.AreEqual(TDPendingServicesKPIReportItem.SECTION_CONTRACTOR, results[8].Section);
            Assert.AreEqual(contractor2.Contractor, results[8].ServicesContractor);
            Assert.AreEqual(1, results[8].Total, "Contractor 2");
        }

        #endregion

        #endregion

        #region Test classes

        private class TestGetServicesRenewed : SearchSet<Service>
        {
            public int? OperatingCenter { get; set; }
            public int? Town { get; set; }
        }

        #endregion
    }

    #region Test Search Classes

    public class TestSearchMonthlyServicesInstalledByCategory : SearchSet<MonthlyServicesInstalledByCategoryViewModel>,
        ISearchMonthlyServicesInstalledByCategory
    {
        public virtual int[] OperatingCenter { get; set; }

        public virtual int Year { get; set; }

        // StoredProcedure limited to these: where Description in 
        // ('Fire Service Installation', 'Irrigation New', 'Sewer Service New', 
        // 'Water Service New Commercial', 'Water Service New Domestic')
        public virtual int[] ServiceCategory
        {
            get
            {
                return new[] {
                    Common.Model.Entities.ServiceCategory.Indices.FIRE_SERVICE_INSTALLATION,
                    Common.Model.Entities.ServiceCategory.Indices.IRRIGATION_NEW,
                    Common.Model.Entities.ServiceCategory.Indices.SEWER_SERVICE_NEW,
                    Common.Model.Entities.ServiceCategory.Indices.WATER_SERVICE_NEW_COMMERCIAL,
                    Common.Model.Entities.ServiceCategory.Indices.WATER_SERVICE_NEW_DOMESTIC
                };
            }
        }
    }

    public class TestSearchBPUReportForServices : SearchSet<BPUReportForServiceReportItem>, ISearchBPUReportForServices
    {
        public int? OperatingCenter { get; set; }
        public int Year { get; set; }
    }

    public class TestSearchServicesRetired : SearchSet<ServicesRetiredReportItem>, ISearchServicesRetired
    {
        public int? OperatingCenter { get; set; }
        public int? Town { get; set; }
        public int? YearRetired { get; set; }
    }

    public class TestSearchServicesRenewedSummary : SearchSet<ServicesRenewedSummaryReportItem>,
        ISearchServicesRenewedSummary
    {
        public int? OperatingCenter { get; set; }
        public IntRange Year { get; set; }
    }

    public class TestSearchServicesCompletedByCategory : SearchSet<ServicesCompletedByCategoryReportItem>,
        ISearchServicesCompletedByCategory
    {
        public int? OperatingCenter { get; set; }
        public DateRange DateInstalled { get; set; }
        public bool? DeveloperServicesDriven { get; set; }
    }

    public class TestTDPendingServicesKPI : SearchSet<TDPendingServicesKPIReportItem>, ISearchTDPendingServicesKPI
    {
        public int? OperatingCenter { get; set; }
    }

    #endregion
}
