using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Engineering.Models.ViewModels.AwiaCompliance;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.Engineering.Models.ViewModels
{
    [TestClass]
    public class CreateAwiaComplianceTest : ViewModelTestBase<AwiaCompliance, CreateAwiaCompliance>
    {
        #region Private Members

        private Mock<IAuthenticationService<User>> _authenticationService;

        #endregion

        #region Private Methods

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authenticationService = e.For<IAuthenticationService<User>>().Mock();
        }

        #endregion

        #region Validations

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(x => x.State, GetFactory<StateFactory>().Create())
                            .EntityMustExist(x => x.OperatingCenter, GetFactory<OperatingCenterFactory>().Create())
                            .EntityMustExist(x => x.CertificationType, GetFactory<AwiaComplianceCertificationTypeFactory>().Create())
                            .EntityMustExist(x => x.CertifiedBy, GetFactory<UserFactory>().Create());
        }

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.State);
            _vmTester.CanMapBothWays(x => x.OperatingCenter);
            _vmTester.CanMapBothWays(x => x.CertificationType);
            _vmTester.CanMapBothWays(x => x.CertifiedBy);
            _vmTester.CanMapBothWays(x => x.DateAccepted);
            _vmTester.CanMapBothWays(x => x.DateSubmitted);
            _vmTester.CanMapBothWays(x => x.RecertificationDue);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(x => x.State)
                            .PropertyIsRequired(x => x.OperatingCenter)
                            .PropertyIsRequired(x => x.PublicWaterSupplies)
                            .PropertyIsRequired(x => x.CertificationType)
                            .PropertyIsRequired(x => x.CertifiedBy)
                            .PropertyIsRequired(x => x.DateSubmitted)
                            .PropertyIsRequired(x => x.DateAccepted)
                            .PropertyIsRequired(x => x.RecertificationDue);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            // noop: No properties with string length validation.
        }

        #endregion
    }
}
