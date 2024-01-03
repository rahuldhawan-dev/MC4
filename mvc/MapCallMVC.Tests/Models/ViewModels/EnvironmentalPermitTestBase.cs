using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Environmental.Models.ViewModels.EnvironmentalPermits;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.NHibernate;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Models.ViewModels
{
    public abstract class EnvironmentalPermitTestBase<TViewModel> : ViewModelTestBase<EnvironmentalPermit, TViewModel> where TViewModel : EnvironmentalPermitViewModel
    {
        #region Fields

        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = e.For<IAuthenticationService<User>>().Mock();
            e.For<IPublicWaterSupplyRepository>().Use<PublicWaterSupplyRepository>();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<IFacilityRepository>().Use<FacilityRepository>();
            e.For<IEquipmentRepository>().Use<EquipmentRepository>();
            e.For<ISensorRepository>().Use<SensorRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetFactory<UserFactory>().Create(new {IsAdmin = true});
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.EnvironmentalPermitType)
                     .CanMapBothWays(x => x.EnvironmentalPermitStatus)
                     .CanMapBothWays(x => x.PublicWaterSupply)
                     .CanMapBothWays(x => x.WasteWaterSystem)
                     .CanMapBothWays(x => x.State)
                     .CanMapBothWays(x => x.FacilityType)
                     .CanMapBothWays(x => x.PermitNumber)
                     .CanMapBothWays(x => x.PermitName)
                     .CanMapBothWays(x => x.ProgramInterestNumber)
                     .CanMapBothWays(x => x.PermitCrossReferenceNumber)
                     .CanMapBothWays(x => x.PermitEffectiveDate)
                     .CanMapBothWays(x => x.PermitRenewalDate)
                     .CanMapBothWays(x => x.PermitEffectiveDate)
                     .CanMapBothWays(x => x.Description)
                     .CanMapBothWays(x => x.RequiresFees)
                     .CanMapBothWays(x => x.ReportingRequired)
                     .CanMapBothWays(x => x.RequiresRequirements);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(x => x.State)
                            .PropertyIsRequired(x => x.FacilityType)
                            .PropertyIsRequired(x => x.EnvironmentalPermitType)
                            .PropertyIsRequired(x => x.PermitNumber)
                            .PropertyIsRequired(x => x.PermitEffectiveDate)
                            .PropertyIsRequired(x => x.ReportingRequired);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert
               .EntityMustExist<State>(x => x.State)
               .EntityMustExist<WaterType>(x => x.FacilityType)
               .EntityMustExist<EnvironmentalPermitType>(x => x.EnvironmentalPermitType)
               .EntityMustExist<EnvironmentalPermitStatus>(x => x.EnvironmentalPermitStatus)
               .EntityMustExist<PublicWaterSupply>(x => x.PublicWaterSupply)
               .EntityMustExist<WasteWaterSystem>(x => x.WasteWaterSystem);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(x => x.PermitName, EnvironmentalPermit.StringLengths.PERMIT_NAME);
            ValidationAssert.PropertyHasMaxStringLength(x => x.PermitNumber, EnvironmentalPermit.StringLengths.PERMIT_NUMBER);
            ValidationAssert.PropertyHasMaxStringLength(x => x.ProgramInterestNumber, EnvironmentalPermit.StringLengths.PROGRAM_INTEREST_NUMBER);
            ValidationAssert.PropertyHasMaxStringLength(x => x.PermitCrossReferenceNumber, EnvironmentalPermit.StringLengths.PERMIT_CROSS_REFERENCE_NUMBER);
            ValidationAssert.PropertyHasMaxStringLength(x => x.Description, EnvironmentalPermit.StringLengths.DESCRIPTION);
        }

        #endregion
    }
}
