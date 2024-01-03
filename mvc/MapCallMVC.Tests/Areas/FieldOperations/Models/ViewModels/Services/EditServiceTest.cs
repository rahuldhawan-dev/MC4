using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels.Services
{
    [TestClass]
    public class EditServiceTest : ServiceViewModelTest<EditService>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<ITownRepository>().Use<TownRepository>();
        }

        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            base.TestPropertiesCanMapBothWays();

            _vmTester
               .CanMapBothWays(x => x.CrossStreet)
               .CanMapBothWays(x => x.IsActive)
               .CanMapBothWays(x => x.ServiceMaterial)
               .CanMapBothWays(x => x.ServiceNumber)
               .CanMapBothWays(x => x.Street);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            base.TestEntityMustExistValidation();

            ValidationAssert
               .EntityMustExist<ServiceMaterial>(x => x.ServiceMaterial)
               .EntityMustExist<Street>(x => x.CrossStreet)
               .EntityMustExist<Street>(x => x.Street);
        }

        [TestMethod]
        public void TestWorkIssuedToCanMapBothWays()
        {
            var opc1 = GetFactory<OperatingCenterFactory>().Create(new { OperatingCenterCode = "FOO" });
            var src = GetEntityFactory<ServiceRestorationContractor>().Create(new {
                OperatingCenter = opc1,
                Contractor = "Buh?",
                FinalRestoration = true,
                PartialRestoration = true
            });

            _entity.WorkIssuedTo = src;

            _vmTester.MapToViewModel();

            Assert.AreEqual(src.Id, _viewModel.WorkIssuedTo);

            _entity.WorkIssuedTo = null;
            _vmTester.MapToEntity();

            Assert.AreSame(src, _entity.WorkIssuedTo);
        }

        #region Mapping

        [TestMethod]
        public void TestMapToEntitySetsLastUpdated()
        {
            var expected = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(expected);

            _vmTester.MapToEntity();
            Assert.AreEqual(expected, _entity.UpdatedAt);
        }
        
        [TestMethod]
        [DataRow(null, null, null, null, false)]
        [DataRow(1, 1, 2, 2, false)]
        [DataRow(1, 2, 3, 4, true)]
        [DataRow(1, 1, 3, 4, true)]
        [DataRow(1, 2, 3, 3, true)]
        public void TestMapToEntitySetsNeedsToSyncToTrueWhenMaterialDiffers(
            int? oldServiceMaterial, int? newServiceMaterial, int? oldCustomerMaterial, int? newCustomerMaterial, bool flag)
        {
            var serviceUtilityType = GetEntityFactory<ServiceUtilityType>().Create();
            var serviceCategory = GetEntityFactory<ServiceCategory>().Create(new { ServiceUtilityType = serviceUtilityType });
            var premise = GetEntityFactory<Premise>().Create(new {
                Installation = "123456789",
                PremiseNumber = "9100327803",
                ServiceUtilityType = serviceUtilityType
            });
            _viewModel.Installation = premise.Installation;
            _viewModel.PremiseNumber = premise.PremiseNumber;
            _viewModel.ServiceCategory = serviceCategory.Id;
            _entity.Premise = premise;
            _entity.ServiceMaterial = (oldServiceMaterial.HasValue)
                ? new ServiceMaterial {
                    Id = oldServiceMaterial.Value
                }
                : null;

            _entity.CustomerSideMaterial = (oldCustomerMaterial.HasValue)
                ? new ServiceMaterial {
                    Id = oldCustomerMaterial.Value
                }
                : null;

            _viewModel.ServiceMaterial = newServiceMaterial;
            _viewModel.CustomerSideMaterial = newCustomerMaterial;

            _vmTester.MapToEntity();

            Assert.AreEqual(flag, _entity.NeedsToSync);
        }

        [TestMethod]
        [DataRow(1, 2, 3, 4, false)]
        [DataRow(1, 1, 3, 4, false)]
        [DataRow(1, 2, 3, 3, false)]
        public void TestMapToEntityDoesNotSetNeedsToSyncToTrueWhenMaterialDiffersAndNoPremiseIsLinked(
            int? oldServiceMaterial, int? newServiceMaterial, int? oldCustomerMaterial, int? newCustomerMaterial, bool flag)
        {
            _entity.ServiceMaterial = (oldServiceMaterial.HasValue)
                ? new ServiceMaterial {
                    Id = oldServiceMaterial.Value
                }
                : null;

            _entity.CustomerSideMaterial = (oldCustomerMaterial.HasValue)
                ? new ServiceMaterial {
                    Id = oldCustomerMaterial.Value
                }
                : null;

            _viewModel.ServiceMaterial = newServiceMaterial;
            _viewModel.CustomerSideMaterial = newCustomerMaterial;
            
            _vmTester.MapToEntity();
            
            Assert.AreEqual(flag, _entity.NeedsToSync);
        }

        [TestMethod]
        public void TestMapToEntitySetsSampleSitesToInactiveWhenRetired()
        {
            GetFactory<InactiveSampleSiteStatusFactory>().Create();
            var activeSampleSiteStatus = GetFactory<ActiveSampleSiteStatusFactory>().Create();

            var premise = GetEntityFactory<Premise>().Create(new {
                PremiseNumber = "xyz",
            });

            var sampleSites = GetEntityFactory<SampleSite>().CreateList(5, new {
                Premise = premise,
                Status = activeSampleSiteStatus
            });
            
            var service = GetEntityFactory<Service>().Create(new {
                Premise = premise
            });

            premise.SampleSites = sampleSites;

            Session.Evict(service); 

            service = Session.Load<Service>(service.Id);
            _viewModel = _viewModelFactory.Build<EditService, Service>(service);

            _viewModel.RetiredDate = DateTime.Now;
            _viewModel.MapToEntity(service);

            foreach (var ss in sampleSites.Select(sampleSite => Session.Load<SampleSite>(sampleSite.Id)))
            {
                Assert.AreEqual(SampleSiteStatus.Indices.INACTIVE, ss.Status.Id);
            }
        }

        [TestMethod]
        public void TestMapToEntitySetsSendServiceWithSampleSiteToTrue()
        {
            GetFactory<InactiveSampleSiteStatusFactory>().Create();
            var activeSampleSiteStatus = GetFactory<ActiveSampleSiteStatusFactory>().Create();

            var premise = GetEntityFactory<Premise>().Create(new {
                PremiseNumber = "xyz",
            });

            var sampleSites = GetEntityFactory<SampleSite>().CreateList(5, new {
                Premise = premise,
                Status = activeSampleSiteStatus
            });
            
            var serviceCategory = GetEntityFactory<ServiceCategory>().Create();
            var service = GetEntityFactory<Service>().Create(new {
                Premise = premise
            });

            premise.SampleSites = sampleSites;

            Session.Evict(service); // because of one to one

            service = Session.Load<Service>(service.Id);
            _viewModel = _viewModelFactory.Build<EditService, Service>(service);

            Assert.IsFalse(_viewModel.SendServiceWithSampleSitesNotificationOnSave);
            service.ServiceCategory = serviceCategory;
            service.Installation = "90001";
            
            _viewModel.MapToEntity(service);
            
            Assert.IsTrue(_viewModel.HasLinkedSampleSiteByInstallation);
            Assert.AreEqual(true, _viewModel.SendServiceWithSampleSitesNotificationOnSave);
        }

        [TestMethod]
        public void TestMapToEntitySetsWBSNumber()
        {
            var customerSideSLReplacementOfferStatuses = GetEntityFactory<CustomerSideSLReplacementOfferStatus>().CreateList(3);
            var wbsNumbers = GetEntityFactory<WBSNumber>().CreateList(3);
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            operatingCenter.DefaultServiceReplacementWBSNumber = wbsNumbers[1];
            var service = GetEntityFactory<Service>().Create(new { OperatingCenter = operatingCenter, CustomerSideSLReplacement = customerSideSLReplacementOfferStatuses[1] });
            _viewModel = _viewModelFactory.Build<EditService, Service>(service);

            _viewModel.MapToEntity(service);

            Assert.AreEqual(wbsNumbers[1].Id, service.CustomerSideReplacementWBSNumber.Id);
        }

        [TestMethod]
        public void TestMapToEntitySetsSendToSAPWhenOperatingCenterEnabled()
        {
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
            var service = GetEntityFactory<Service>().Create(new {
                DateInstalled = (DateTime?)null,
                OperatingCenter = operatingCenter
            });
            _viewModel = _viewModelFactory.Build<EditService, Service>(service);

            _viewModel.MapToEntity(service);

            Assert.IsTrue(_viewModel.SendToSAP);
        }

        [TestMethod]
        public void TestMapToEntitySetsSendToSAPFalseWhenPreviouslyCompleted()
        {
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new {SAPEnabled = true});
            var service = GetEntityFactory<Service>().Create(new {OperatingCenter = operatingCenter, DateInstalled = DateTime.Now});
            _viewModel = _viewModelFactory.Build<EditService, Service>(service);

            _viewModel.MapToEntity(service);

            Assert.IsFalse(_viewModel.SendToSAP);
        }

        [TestMethod]
        public void TestMapToEntitySetsSendToSAPTrueWhenCompletedAndNotPreviouslyCompleted()
        {
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
            var service = GetEntityFactory<Service>().Create(new {
                DateInstalled = (DateTime?)null,
                OperatingCenter = operatingCenter
            });
            _viewModel = _viewModelFactory.BuildWithOverrides<EditService, Service>(service, new { DateInstalled = DateTime.Now });

            _viewModel.MapToEntity(service);

            Assert.IsTrue(_viewModel.SendToSAP);
        }

        #endregion

        #endregion
    }
}