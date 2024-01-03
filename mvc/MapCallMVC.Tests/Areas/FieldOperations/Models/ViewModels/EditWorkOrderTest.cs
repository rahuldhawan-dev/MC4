using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels
{
    [TestClass]
    public class EditWorkOrderTest : WorkOrderViewModelTestBase<EditWorkOrder>
    {
        #region Private Members

        private Coordinate _coordinate;
        private OperatingCenter _operatingCenter;

        #endregion

        #region Private Methods

        protected override EditWorkOrder CreateViewModel()
        {
            _coordinate = GetEntityFactory<Coordinate>().Create();
            _operatingCenter = GetEntityFactory<OperatingCenter>().Create();

            return _viewModelFactory.BuildWithOverrides<EditWorkOrder, WorkOrder>(_entity, new {
                OperatingCenter = _operatingCenter.Id,
                CoordinateId = _coordinate.Id
            });
        }

        #endregion

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            base.TestEntityMustExistValidation();

            ValidationAssert
               .EntityMustExist<User>(x => x.MaterialsApprovedBy)
               .EntityMustExist<WorkDescription>(x => x.WorkDescription)
               .EntityMustExist<Street>(x => x.Street)
               .EntityMustExist<Street>(x => x.NearestCrossStreet);
        }

        [TestMethod]
        public void Test_Map_SetsIsRevisitFalse_WhenWorkDescriptionIsNotForRevisit()
        {
            var workDescription = GetFactory<BallCurbStopReplaceWorkDescriptionFactory>().Create();
            var order = GetEntityFactory<WorkOrder>().Create(new { WorkDescription = workDescription });
            _viewModel = _viewModelFactory.Build<EditWorkOrder, WorkOrder>(order);

            _viewModel.Map(order);

            Assert.IsNotNull(_viewModel.IsRevisit);
            Assert.IsFalse(_viewModel.IsRevisit.Value);
        }

        [TestMethod]
        public void Test_Map_SetsIsRevisit_WhenWorkDescriptionIsForRevisit()
        {
            var workDescription = GetFactory<HydrantLandscapingWorkDescriptionFactory>().Create();
            var order = GetEntityFactory<WorkOrder>().Create(new {WorkDescription = workDescription});
            _viewModel = _viewModelFactory.Build<EditWorkOrder, WorkOrder>(order);

            _viewModel.Map(order);

            Assert.IsNotNull(_viewModel.IsRevisit);
            Assert.IsTrue(_viewModel.IsRevisit.Value);
        }

        [TestMethod]
        public void
            Test_MapToEntity_SetsMaterialsAndSizesToMostRecentlyInstalledValues_ForServiceRenewals()
        {
            // exception will be generated without these
            GetFactory<ServiceLineRenewalWorkDescriptionFactory>().Create();
            GetFactory<ServiceLineRenewalLeadWorkDescriptionFactory>().Create();
            GetFactory<ServiceLineRenewalStormRestorationWorkDescriptionFactory>().Create();

            var installation = 1234567890;

            var premise = GetEntityFactory<Premise>().Create(new {
                Installation = installation.ToString(),
                OperatingCenter = _operatingCenter
            });
            var service = GetEntityFactory<Service>().Create(new {
                Premise = premise
            });

            _viewModel.OperatingCenter = _operatingCenter.Id;
            _viewModel.Installation = installation;
            _viewModel.AssetType = AssetType.Indices.SERVICE;

            _entity.CompanyServiceLineMaterial = null;
            _entity.CompanyServiceLineSize = null;
            _entity.CustomerServiceLineMaterial = null;
            _entity.CustomerServiceLineSize = null;

            foreach (var description in WorkDescription.SERVICE_LINE_RENEWALS)
            {
                _entity.WorkDescription = new WorkDescription {
                    Id = (int)WorkDescription.Indices.SERVICE_INVESTIGATION
                };
                _viewModel.WorkDescription = description;

                var entity = _viewModel.MapToEntity(_entity);
                
                Assert.AreEqual(service.ServiceMaterial, entity.CompanyServiceLineMaterial);
                Assert.AreEqual(service.ServiceSize, entity.CompanyServiceLineSize);
                Assert.AreEqual(service.CustomerSideMaterial, entity.CustomerServiceLineMaterial);
                Assert.AreEqual(service.CustomerSideSize, entity.CustomerServiceLineSize);

                entity.CompanyServiceLineMaterial = null;
                entity.CompanyServiceLineSize = null;
                entity.CustomerServiceLineMaterial = null;
                entity.CustomerServiceLineSize = null;
            }
        }

        [TestMethod]
        public void
            Test_MapToEntity_DoesNotSetMaterialsAndSizesToMostRecentlyInstalledValues_WhenAlreadyARenewal()
        {
            // exception will be generated without these
            GetFactory<ServiceLineRenewalWorkDescriptionFactory>().Create();
            GetFactory<ServiceLineRenewalLeadWorkDescriptionFactory>().Create();
            GetFactory<ServiceLineRenewalStormRestorationWorkDescriptionFactory>().Create();

            var installation = 1234567890;

            var premise = GetEntityFactory<Premise>().Create(new {
                Installation = installation.ToString(),
                OperatingCenter = _operatingCenter
            });
            var service = GetEntityFactory<Service>().Create(new {
                Premise = premise
            });

            _viewModel.OperatingCenter = _operatingCenter.Id;
            _viewModel.Installation = installation;
            _viewModel.AssetType = AssetType.Indices.SERVICE;

            _entity.CompanyServiceLineMaterial = null;
            _entity.CompanyServiceLineSize = null;
            _entity.CustomerServiceLineMaterial = null;
            _entity.CustomerServiceLineSize = null;

            foreach (var entityDescription in WorkDescription.SERVICE_LINE_RENEWALS)
            {
                _entity.WorkDescription = new WorkDescription { Id = entityDescription };
                
                foreach (var viewModelDescription in WorkDescription.SERVICE_LINE_RENEWALS)
                {
                    _viewModel.WorkDescription = viewModelDescription;

                    var entity = _viewModel.MapToEntity(_entity);

                    Assert.IsNull(entity.CompanyServiceLineMaterial);
                    Assert.IsNull(entity.CompanyServiceLineSize);
                    Assert.IsNull(entity.CustomerServiceLineMaterial);
                    Assert.IsNull(entity.CustomerServiceLineSize);
                }
            }
        }

        [TestMethod]
        public void
            Test_MapToEntity_DoesNotSetMaterialsAndSizesToMostRecentlyInstalledValues_WhenAlreadySet()
        {
            // exception will be generated without these
            GetFactory<ServiceLineRenewalWorkDescriptionFactory>().Create();
            GetFactory<ServiceLineRenewalLeadWorkDescriptionFactory>().Create();
            GetFactory<ServiceLineRenewalStormRestorationWorkDescriptionFactory>().Create();

            var installation = 1234567890;

            var premise = GetEntityFactory<Premise>().Create(new {
                Installation = installation.ToString(),
                OperatingCenter = _operatingCenter
            });
            var service = GetEntityFactory<Service>().Create(new {
                Premise = premise
            });

            _viewModel.OperatingCenter = _operatingCenter.Id;
            _viewModel.Installation = installation;
            _viewModel.AssetType = AssetType.Indices.SERVICE;

            _entity.CompanyServiceLineMaterial = GetEntityFactory<ServiceMaterial>().Create();
            _entity.CompanyServiceLineSize = GetEntityFactory<ServiceSize>().Create();
            _entity.CustomerServiceLineMaterial = GetEntityFactory<ServiceMaterial>().Create();
            _entity.CustomerServiceLineSize = GetEntityFactory<ServiceSize>().Create();

            foreach (var description in WorkDescription.SERVICE_LINE_RENEWALS)
            {
                _viewModel.WorkDescription = description;

                var entity = _viewModel.MapToEntity(_entity);

                Assert.AreNotEqual(service.ServiceMaterial, entity.CompanyServiceLineMaterial);
                Assert.AreNotEqual(service.ServiceSize, entity.CompanyServiceLineSize);
                Assert.AreNotEqual(service.CustomerSideMaterial, entity.CustomerServiceLineMaterial);
                Assert.AreNotEqual(service.CustomerSideSize, entity.CustomerServiceLineSize);
            }
        }
        
        [TestMethod]
        public void Test_MapToEntity_SetsMaterialsToNull_ForServiceRenewalsWhenUnknown()
        {
            // exception will be generated without these
            GetFactory<ServiceLineRenewalWorkDescriptionFactory>().Create();
            GetFactory<ServiceLineRenewalLeadWorkDescriptionFactory>().Create();
            GetFactory<ServiceLineRenewalStormRestorationWorkDescriptionFactory>().Create();

            var installation = 1234567890;
            var unknownMaterial = GetEntityFactory<ServiceMaterial>().Create(new {
                Description = "UnKnoWn"
            });
            var premise = GetEntityFactory<Premise>().Create(new {
                Installation = installation.ToString(),
                OperatingCenter = _operatingCenter
            });
            var service = GetEntityFactory<Service>().Create(new {
                Premise = premise,
                CustomerSideMaterial = unknownMaterial,
                ServiceMaterial = unknownMaterial
            });

            _viewModel.OperatingCenter = _operatingCenter.Id;
            _viewModel.Installation = installation;
            _viewModel.AssetType = AssetType.Indices.SERVICE;

            foreach (var description in WorkDescription.SERVICE_LINE_RENEWALS)
            {
                _viewModel.WorkDescription = description;

                var entity = _viewModel.MapToEntity(_entity);

                Assert.IsNull(entity.CompanyServiceLineMaterial);
                Assert.IsNull(entity.CustomerServiceLineMaterial);

                entity.CompanyServiceLineMaterial = null;
                entity.CompanyServiceLineSize = null;
                entity.CustomerServiceLineMaterial = null;
                entity.CustomerServiceLineSize = null;
            }
        }
        
        [TestMethod]
        public void TestMapToEntitySetsPremiseWhenMatchingPremiseExists()
        {
            var premiseNumber = "12345678";
            long deviceLocation = 554785477;
            long installation = 887542541;
            var premise = GetFactory<PremiseFactory>().Create(new {PremiseNumber = premiseNumber, DeviceLocation = deviceLocation.ToString(), Installation = installation.ToString()});

            _viewModel.PremiseNumber = premiseNumber;
            _viewModel.DeviceLocation = deviceLocation;
            _viewModel.Installation = installation;
            
            var result = _viewModel.MapToEntity(_entity);
            
            Assert.IsNotNull(result.Premise);
            Assert.AreEqual(premise.Id, result.Premise.Id);
        }
        
        [TestMethod]
        public void TestMapToEntityDoesNotSetPremiseWhenNoMatchingPremiseExists()
        {
            var premiseNumber = "12345678";
            long deviceLocation = 554785477;
            long installation = 887542541;
            var premise = GetFactory<PremiseFactory>().Create(new {PremiseNumber = premiseNumber, DeviceLocation = deviceLocation.ToString(), Installation = installation.ToString()});

            _viewModel.PremiseNumber = "123456789";
            _viewModel.DeviceLocation = deviceLocation;
            _viewModel.Installation = installation;
            
            var result = _viewModel.MapToEntity(_entity);
            
            Assert.IsNull(result.Premise);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            base.TestRequiredValidation();
            ValidationAssert.PropertyIsRequired(x => x.NearestCrossStreet);
        }

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            base.TestPropertiesCanMapBothWays();
            _vmTester.CanMapBothWays(x => x.NearestCrossStreet);
        }

        [TestMethod]
        public void TestMapToEntityMapsMeterLocation()
        {
            var premiseNumber = "12345678";
            long deviceLocation = 554785477;
            long installation = 887542541;
            var meterLocation = GetFactory<InsideMeterLocationFactory>().Create(new { SAPCode = "c1" });
            var premise = GetFactory<PremiseFactory>().Create(new {
                PremiseNumber = premiseNumber,
                DeviceLocation = deviceLocation.ToString(),
                Installation = installation.ToString(),
                MeterLocation = meterLocation
            });

            _viewModel.PremiseNumber = "123456789";
            _viewModel.DeviceLocation = deviceLocation;
            _viewModel.Installation = installation;

            // Should not set meter location since no matching premise
            var result = _viewModel.MapToEntity(_entity);

            Assert.IsNull(result.MeterLocation);

            _viewModel.PremiseNumber = premiseNumber;

            // Should set meter location as there is matching premise
            result = _viewModel.MapToEntity(_entity);

            Assert.IsNotNull(result.MeterLocation);
            Assert.AreEqual(meterLocation.Id, result.MeterLocation.Id);
            Assert.AreEqual(meterLocation.SAPCode, result.MeterLocation.SAPCode);
        }
    }
}