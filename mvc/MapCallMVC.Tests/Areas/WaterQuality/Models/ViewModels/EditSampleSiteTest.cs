using System;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.WaterQuality.Models.ViewModels.SampleSites;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.WaterQuality.Models.ViewModels
{
    [TestClass]
    public class EditSampleSiteTest : SampleSiteViewModelTest<EditSampleSite>
    {
        #region Tests

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            base.TestPropertiesCanMapBothWays();

            _vmTester.CanMapBothWays(x => x.Street);
            _vmTester.CanMapBothWays(x => x.CrossStreet);
        }

        [TestMethod]
        public void TestMapToEntitySetsCertifiedDateToNowAndCertifiedByToCurrentUser()
        {
            var expectedUser = GetEntityFactory<User>().Create();
            _authenticationService.Setup(x => x.CurrentUser)
                                  .Returns(expectedUser);

            var expectedDate = new DateTime(1984, 4, 24, 4, 0, 4);
            _dateTimeProvider.Setup(x => x.GetCurrentDate())
                             .Returns(expectedDate);

            _viewModel.CertificationAuthorization = true;
            _entity.CertifiedDate = null;
            _entity.CertifiedBy = null;

            _vmTester.MapToEntity();

            Assert.AreEqual(expectedDate, _entity.CertifiedDate);
            Assert.AreSame(expectedUser, _entity.CertifiedBy);
        }

        [TestMethod]
        public void TestValidationFailsIfCertificationAuthorizationIsSetButSampleSiteIsNotReadyForReCertification()
        {
            _dateTimeProvider.Setup(x => x.GetCurrentDate())
                             .Returns(DateTime.Now);

            _viewModel.Id = _entity.Id;
            _viewModel.CertificationAuthorization = true;
            _entity.CertifiedDate = DateTime.Now;

            ValidationAssert.ModelStateHasNonPropertySpecificError($"It is too soon to certify sample site #{_entity.Id}.");
        }

        #endregion
    }
}
