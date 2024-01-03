using System;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing.ClassExtensions;
using StructureMap;

namespace Contractors.Tests.Models.ViewModels
{
    // Needs ServiceRepository because Service.cs is using one
    [TestClass]
    public class EditServiceTest : ViewModelTestBase<Service, EditService>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IAuthenticationService<ContractorUser>>().Mock();
            e.For<IAuthenticationService<User>>().Mock();
            e.For<ITapImageRepository>().Mock();
            e.For<IServiceRepository>().Use<ServiceRepository>();
        }

        protected override Service CreateEntity()
        {
            return GetEntityFactory<Service>().Create(new {
                DateInstalled = (DateTime?)null
            });
        }

        #endregion

        [TestMethod]
        public void TestDisplayServiceReturnsOriginalService()
        {
            var entity = GetFactory<ServiceFactory>().Create();
            var viewModel = _viewModelFactory.Build<EditService, Service>(entity);

            Assert.AreSame(entity, viewModel.Display);
        }

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.CustomerSideMaterial);
            _vmTester.CanMapBothWays(x => x.CustomerSideReplacementDate);
            _vmTester.CanMapBothWays(x => x.CustomerSideSLReplacedBy);
            _vmTester.CanMapBothWays(x => x.CustomerSideSLReplacement);
            _vmTester.CanMapBothWays(x => x.CustomerSideSLReplacementContractor);
            _vmTester.CanMapBothWays(x => x.CustomerSideSize);
            _vmTester.CanMapBothWays(x => x.DateInstalled);
            _vmTester.CanMapBothWays(x => x.DateIssuedToField);
            _vmTester.CanMapBothWays(x => x.DepthMainFeet);
            _vmTester.CanMapBothWays(x => x.DepthMainInches);
            _vmTester.CanMapBothWays(x => x.FlushingOfCustomerPlumbing);
            _vmTester.CanMapBothWays(x => x.LeadAndCopperCommunicationProvided);
            _vmTester.CanMapBothWays(x => x.LengthOfCustomerSideSLReplaced);
            _vmTester.CanMapBothWays(x => x.LengthOfService);
            _vmTester.CanMapBothWays(x => x.MainSize);
            _vmTester.CanMapBothWays(x => x.MainType);
            _vmTester.CanMapBothWays(x => x.MeterSettingRequirement);
            _vmTester.CanMapBothWays(x => x.MeterSettingSize);
            _vmTester.CanMapBothWays(x => x.OriginalInstallationDate);
            _vmTester.CanMapBothWays(x => x.PitInstalled);
            _vmTester.CanMapBothWays(x => x.PreviousServiceCustomerMaterial);
            _vmTester.CanMapBothWays(x => x.PreviousServiceCustomerSize);
            _vmTester.CanMapBothWays(x => x.PreviousServiceMaterial);
            _vmTester.CanMapBothWays(x => x.PreviousServiceSize);
            _vmTester.CanMapBothWays(x => x.RetireMeterSet);
            _vmTester.CanMapBothWays(x => x.RetiredAccountNumber);
            _vmTester.CanMapBothWays(x => x.RetiredDate);
            _vmTester.CanMapBothWays(x => x.ServiceMaterial);
            _vmTester.CanMapBothWays(x => x.ServicePriority);
            _vmTester.CanMapBothWays(x => x.ServiceSideType);
            _vmTester.CanMapBothWays(x => x.ServiceSize);
            _vmTester.CanMapBothWays(x => x.StreetMaterial);
            _vmTester.CanMapBothWays(x => x.WorkIssuedTo);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            // noop, it's all RequiredWhens
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert
               .EntityMustExist<Contractor>(x => x.CustomerSideSLReplacementContractor)
               .EntityMustExist<CustomerSideSLReplacementOfferStatus>(x => x.CustomerSideSLReplacement)
               .EntityMustExist<CustomerSideSLReplacer>(x => x.CustomerSideSLReplacedBy)
               .EntityMustExist<FlushingOfCustomerPlumbingInstructions>(x => x.FlushingOfCustomerPlumbing)
               .EntityMustExist<MainType>(x => x.MainType)
               .EntityMustExist<ServiceMaterial>(x => x.CustomerSideMaterial)
               .EntityMustExist<ServiceMaterial>(x => x.PreviousServiceCustomerMaterial)
               .EntityMustExist<ServiceMaterial>(x => x.PreviousServiceMaterial)
               .EntityMustExist<ServiceMaterial>(x => x.ServiceMaterial)
               .EntityMustExist<ServicePriority>(x => x.ServicePriority)
               .EntityMustExist<ServiceRestorationContractor>(x => x.WorkIssuedTo)
               .EntityMustExist<ServiceSideType>(x => x.ServiceSideType)
               .EntityMustExist<ServiceSize>(x => x.CustomerSideSize)
               .EntityMustExist<ServiceSize>(x => x.MainSize)
               .EntityMustExist<ServiceSize>(x => x.MeterSettingSize)
               .EntityMustExist<ServiceSize>(x => x.PreviousServiceCustomerSize)
               .EntityMustExist<ServiceSize>(x => x.PreviousServiceSize)
               .EntityMustExist<ServiceSize>(x => x.ServiceSize)
               .EntityMustExist<StreetMaterial>(x => x.StreetMaterial);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert
               .PropertyHasMaxStringLength(x => x.RetiredAccountNumber,
                    Service.StringLengths.RETIRED_ACCOUNT_NUMBER)
               .PropertyHasMaxStringLength(x => x.RetireMeterSet,
                    Service.StringLengths.RETIRE_METER_SET);
        }

        [TestMethod]
        public void TestRequiredWhenForMainSizeWhenDateInstalledNotNull()
        {
            _viewModel.MainSize = null;
            _viewModel.DateInstalled = null;

            ValidationAssert.ModelStateIsValid();

            _viewModel.DateInstalled = DateTime.Now;
            ValidationAssert.PropertyIsRequired(x => x.MainSize);
        }

        [TestMethod]
        public void TestRequiredWhenForMeterSettingRequirementWhenRetiredDateNotNull()
        {
            _viewModel.MeterSettingRequirement = null;
            _viewModel.RetiredDate = null;

            ValidationAssert.ModelStateIsValid();

            _viewModel.RetiredDate = DateTime.Now;
            ValidationAssert.PropertyIsRequired(x => x.MeterSettingRequirement);
        }

        [TestMethod]
        public void TestRequiredWhenForMeterSettingRequirementWhenDateInstalledNotNull()
        {
            _viewModel.MeterSettingRequirement = null;
            _viewModel.DateInstalled = null;

            ValidationAssert.ModelStateIsValid();

            _viewModel.DateInstalled = DateTime.Now;
            ValidationAssert.PropertyIsRequired(x => x.MeterSettingRequirement);
        }

        [TestMethod]
        public void TestRequiredWhenForMeterSettingSizeWhenDateInstalledNotNull()
        {
            _viewModel.MeterSettingSize = null;
            _viewModel.DateInstalled = null;

            ValidationAssert.ModelStateIsValid();

            _viewModel.DateInstalled = DateTime.Now;
            ValidationAssert.PropertyIsRequired(x => x.MeterSettingSize);
        }

        [TestMethod]
        public void TestRequiredWhenForServiceMaterialWhenDateInstalledNotNull()
        {
            _viewModel.ServiceMaterial = null;
            _viewModel.DateInstalled = null;

            ValidationAssert.ModelStateIsValid();

            _viewModel.DateInstalled = DateTime.Now;
            ValidationAssert.PropertyIsRequired(x => x.ServiceMaterial);
        }

        [TestMethod]
        public void TestRequiredWhenForServiceSizeWhenDateInstalledNotNull()
        {
            _viewModel.ServiceSize = null;
            _viewModel.DateInstalled = null;

            ValidationAssert.ModelStateIsValid();

            _viewModel.DateInstalled = DateTime.Now;
            ValidationAssert.PropertyIsRequired(x => x.ServiceSize);
        }

        [TestMethod]
        public void TestRequiredWhenForCustomerSideSLReplacementContractorWhenDateInstalledNotNull()
        {
            var replacedByContractor = GetEntityFactory<CustomerSideSLReplacer>().CreateList(2);
            _viewModel.CustomerSideSLReplacementContractor = null;
            _viewModel.CustomerSideSLReplacedBy = replacedByContractor[0].Id;

            ValidationAssert.ModelStateIsValid();

            _viewModel.CustomerSideSLReplacedBy = replacedByContractor[1].Id;
            ValidationAssert.PropertyIsRequired(x => x.CustomerSideSLReplacementContractor);
        }

        [TestMethod]
        public void TestRequiredWhenForDateIssuedToFieldWhenRetiredDateNotNull()
        {
            _viewModel.DateIssuedToField = null;
            _viewModel.RetiredDate = null;

            ValidationAssert.ModelStateIsValid();

            _viewModel.RetiredDate = DateTime.Now;
            ValidationAssert.PropertyIsRequired(x => x.DateIssuedToField);
        }

        [TestMethod]
        public void TestRequiredWhenForDateIssuedToFieldWhenDateInstalledNotNull()
        {
            _viewModel.DateIssuedToField = null;
            _viewModel.DateInstalled = null;

            ValidationAssert.ModelStateIsValid();

            _viewModel.DateInstalled = DateTime.Now;
            ValidationAssert.PropertyIsRequired(x => x.DateIssuedToField);
        }
        
        [TestMethod]
        public void TestRequiredWhenForWorkIssuedToWhenRetiredDateNotNull()
        {
            _viewModel.WorkIssuedTo = null;
            _viewModel.RetiredDate = null;

            ValidationAssert.ModelStateIsValid();

            _viewModel.RetiredDate = DateTime.Now;
            ValidationAssert.PropertyIsRequired(x => x.WorkIssuedTo);
        }

        [TestMethod]
        public void TestRequiredWhenForWorkIssuedToWhenDateInstalledNotNull()
        {
            _viewModel.WorkIssuedTo = null;
            _viewModel.DateInstalled = null;

            ValidationAssert.ModelStateIsValid();

            _viewModel.DateInstalled = DateTime.Now;
            ValidationAssert.PropertyIsRequired(x => x.WorkIssuedTo);
        }

        [TestMethod]
        public void TestRequiredWhenForServicePriorityWhenRetiredDateNotNull()
        {
            _viewModel.ServicePriority = null;
            _viewModel.RetiredDate = null;

            ValidationAssert.ModelStateIsValid();

            _viewModel.RetiredDate = DateTime.Now;
            ValidationAssert.PropertyIsRequired(x => x.ServicePriority);
        }

        [TestMethod]
        public void TestRequiredWhenForServicePriorityWhenDateInstalledNotNull()
        {
            _viewModel.ServicePriority = null;
            _viewModel.DateInstalled = null;

            ValidationAssert.ModelStateIsValid();

            _viewModel.DateInstalled = DateTime.Now;
            ValidationAssert.PropertyIsRequired(x => x.ServicePriority);
        }

        [TestMethod]
        public void TestRequiredWhenForLengthOfServiceWhenDateInstalledNotNull()
        {
            _viewModel.LengthOfService = null;
            _viewModel.DateInstalled = null;

            ValidationAssert.ModelStateIsValid();

            _viewModel.DateInstalled = DateTime.Now;
            ValidationAssert.PropertyIsRequired(x => x.LengthOfService);
        }

        [TestMethod]
        public void TestRequiredWhenForDateInstalledWhenRetireDateNotNull()
        {
            _viewModel.DateInstalled = null;
            _viewModel.RetiredDate = null;

            ValidationAssert.ModelStateIsValid();

            _viewModel.RetiredDate = DateTime.Now;
            ValidationAssert.PropertyIsRequired(x => x.DateInstalled);
        }

        [TestMethod]
        public void TestRequiredWhenForPreviousServiceMaterialWhenRetiredDateNotNull()
        {
            _viewModel.PreviousServiceMaterial = null;
            _viewModel.RetiredDate = null;

            ValidationAssert.ModelStateIsValid();

            _viewModel.RetiredDate = DateTime.Now;
            ValidationAssert.PropertyIsRequired(x => x.PreviousServiceMaterial);
        }
        
        [TestMethod]
        public void TestRequiredWhenForPreviousServiceMaterialWhenServiceCategory()
        {
            _viewModel.PreviousServiceCustomerMaterial = null;
            _viewModel.ServiceCategory = ServiceCategory.Indices.FIRE_RETIRE_SERVICE_ONLY;

            ValidationAssert.ModelStateIsValid();

            _viewModel.ServiceCategory = ServiceCategory.Indices.WATER_SERVICE_RENEWAL_CUST_SIDE;
            ValidationAssert.PropertyIsRequired(x => x.PreviousServiceCustomerMaterial);
        }

        [TestMethod]
        public void TestRequiredWhenForOriginalInstallationDateWhenRetiredDateNotNull()
        {
            _viewModel.OriginalInstallationDate = null;
            _viewModel.RetiredDate = null;

            ValidationAssert.ModelStateIsValid();

            _viewModel.RetiredDate = DateTime.Now;
            ValidationAssert.PropertyIsRequired(x => x.OriginalInstallationDate);
        }

        [TestMethod]
        public void TestRequiredWhenForPreviousServiceSizeDateWhenRetiredDateNotNull()
        {
            _viewModel.PreviousServiceSize = null;
            _viewModel.RetiredDate = null;

            ValidationAssert.ModelStateIsValid();

            _viewModel.RetiredDate = DateTime.Now;
            ValidationAssert.PropertyIsRequired(x => x.PreviousServiceSize);
        }

        [TestMethod]
        public void TestRequiredWhenForPreviousServiceCustomerSizeWhenServiceCategoryWaterServiceRenewalCustomerSide()
        {
            _viewModel.PreviousServiceCustomerSize = null;
            _viewModel.ServiceCategory = ServiceCategory.Indices.FIRE_RETIRE_SERVICE_ONLY;

            ValidationAssert.ModelStateIsValid();

            _viewModel.ServiceCategory = ServiceCategory.Indices.WATER_SERVICE_RENEWAL_CUST_SIDE;
            ValidationAssert.PropertyIsRequired(x => x.PreviousServiceCustomerSize);
        }
    }
}
