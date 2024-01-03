using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories.Users;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Environmental.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using MMSINC.Utilities;
using Moq;
using StructureMap;
using System;
using System.Linq.Expressions;

namespace MapCallMVC.Tests.Areas.Environmental.Models.ViewModels
{
    [TestClass]
    public class CreateWaterSampleComplianceFormTest : MapCallMvcInMemoryDatabaseTestBase<BacterialWaterSample>
    {
        #region Fields

        private ViewModelTester<CreateWaterSampleComplianceForm, WaterSampleComplianceForm> _vmTester;
        private Mock<IDateTimeProvider> _dateTimeProvider;
        private CreateWaterSampleComplianceForm _viewModel;
        private WaterSampleComplianceForm _entity;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;
        private Mock<IUserRepository> _userRepo;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);

            e.For<IDateTimeProvider>().Use((_dateTimeProvider = new Mock<IDateTimeProvider>()).Object);
            e.For<IAuthenticationService<User>>().Use((_authServ = new Mock<IAuthenticationService<User>>()).Object);
            e.For<IUserRepository>().Use((_userRepo = new Mock<IUserRepository>()).Object);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetFactory<UserFactory>().Create(new { IsAdmin = true });
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
            _viewModel = new CreateWaterSampleComplianceForm(_container);
            _entity = new WaterSampleComplianceForm();
            _vmTester = new ViewModelTester<CreateWaterSampleComplianceForm, WaterSampleComplianceForm>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        #region SetDefaults

        [TestMethod]
        public void TestSetDefaultsSetsCertificationMonthYearDisplayForCurrentMonth()
        {
            var expectedDate = new DateTime(2018, 2, 15);
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(expectedDate);

            var target = new CreateWaterSampleComplianceForm(_container);
            target.SetDefaults();

            Assert.AreEqual("02/2018", target.CertifiedMonthYearDisplay);
        }

        [TestMethod]
        public void TestSetDefaultsSetsCertifiedBy()
        {
            var expected = "This is my name";
            _user.FullName = expected;

            var target = new CreateWaterSampleComplianceForm(_container);
            target.SetDefaults();

            Assert.AreEqual(expected, target.CertifiedBy);
        }

        #endregion

        #region Mapping

        [TestMethod]
        public void TestMapToEntitySetsCertifiedByToCurrentUser()
        {
            var expected = _user;
            Assert.IsNull(_entity.CertifiedBy, "Sanity");
            _vmTester.MapToEntity();
            Assert.AreSame(expected, _entity.CertifiedBy);
        }

        [TestMethod]
        public void TestMapToEntitySetsCertifiedAtToCurrentDateTime()
        {
            var expected = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(expected);
            Assert.AreNotEqual(expected, _entity.DateCertified, "Sanity");
            _vmTester.MapToEntity();
            Assert.AreEqual(expected, _entity.DateCertified);
        }

        [TestMethod]
        public void TestMapToEntitySetsCertfiedMonthAndCertifiedYearToExpectedValues()
        {
            DateTime expected;

            Action<DateTime> setExpectedDate = (expectedDateTime) => {
                expected = expectedDateTime;
                _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(expected);

                // Reset these to zeroes to ensure this fails for weird changes in MapToEntity.
                // Similar results are expected for many dates, so we don't want there to
                // be a weird failure that passes just because of the previous test results.
                _entity.CertifiedMonth = 0;
                _entity.CertifiedYear = 0;

                _vmTester.MapToEntity();
                Assert.AreEqual(expected, _entity.DateCertified, "Sanity.");
            };

            // Test that 2/1/2018 leads to 1/2018
            setExpectedDate(new DateTime(2018, 2, 1));
            Assert.AreEqual(1, _entity.CertifiedMonth, "Prior to the 11th of the month, certification is for the month before.");
            Assert.AreEqual(2018, _entity.CertifiedYear);

            // Test that 2/10/2018 leads to 1/2018
            setExpectedDate(new DateTime(2018, 2, 10));
            Assert.AreEqual(1, _entity.CertifiedMonth, "Prior to the 11th of the month, certification is for the month before.");
            Assert.AreEqual(2018, _entity.CertifiedYear);

            // Test that 2/11/2018 leads to 2/2018
            setExpectedDate(new DateTime(2018, 2, 11));
            Assert.AreEqual(2, _entity.CertifiedMonth, "Anytime after the 10th of the currenth month means certification is for the same month.");
            Assert.AreEqual(2018, _entity.CertifiedYear);

            // Test that 3/10/2018 leads to 2/2018
            setExpectedDate(new DateTime(2018, 3, 10));
            Assert.AreEqual(2, _entity.CertifiedMonth, "Prior to the 11th of the month, certification is for the month before.");
            Assert.AreEqual(2018, _entity.CertifiedYear);

            // Test month/year when doing 1/1/2018 because that should then be 12/2017
            setExpectedDate(new DateTime(2018, 1, 1));
            Assert.AreEqual(12, _entity.CertifiedMonth, "Prior to the 11th of the month, certification is for the month before.");
            Assert.AreEqual(2017, _entity.CertifiedYear, "The month was also last year so it should be last year in this case.");
        }

        #endregion

        #region Validation

        [TestMethod]
        public void TestValidationFailsIfPWSIDIsCertifiedForTheCurrentMonthAlready()
        {
            var expectedDate = new DateTime(2018, 1, 15);
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(expectedDate);
            var pwsid = GetFactory<PublicWaterSupplyFactory>().Create();
            _viewModel.PublicWaterSupply = pwsid.Id;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.PublicWaterSupply);

            var existingForm = new WaterSampleComplianceForm();
            // Do not set the CertifiedDate, that property should not be used to check 
            // for matching month/year.
            existingForm.CertifiedMonth = 1;
            existingForm.CertifiedYear = 2018;
            pwsid.WaterSampleComplianceForms.Add(existingForm);

            ValidationAssert.ModelStateHasNonPropertySpecificError(_viewModel, "This public water supply is already certified for the current month.");
        }

        [TestMethod]
        public void TestValidationFailsIfPWSIDHasNullOrFalseAWOwnedValue()
        {
            var pwsid = GetFactory<PublicWaterSupplyFactory>().Create();
            _viewModel.PublicWaterSupply = pwsid.Id;

            pwsid.AWOwned = null;
            ValidationAssert.ModelStateHasNonPropertySpecificError(_viewModel, "Compliance forms can only be entered for American Water owned public water supplies.");

            pwsid.AWOwned = false;
            ValidationAssert.ModelStateHasNonPropertySpecificError(_viewModel, "Compliance forms can only be entered for American Water owned public water supplies.");

            pwsid.AWOwned = true;
            ValidationAssert.ModelStateIsValid(_viewModel);
        }

        [TestMethod]
        public void TestEntityMustExistProperties()
        {
            var entity = GetFactory<YesWaterSampleComplianceFormAnswerTypeFactory>().Create();
            ValidationAssert.EntityMustExist(_viewModel, x => x.CentralLabSamplesHaveBeenCollected, entity);
            ValidationAssert.EntityMustExist(_viewModel, x => x.ContractedLabsSamplesHaveBeenCollected, entity);
            ValidationAssert.EntityMustExist(_viewModel, x => x.InternalLabsSamplesHaveBeenCollected, entity);
            ValidationAssert.EntityMustExist(_viewModel, x => x.BactiSamplesHaveBeenCollected, entity);
            ValidationAssert.EntityMustExist(_viewModel, x => x.LeadAndCopperSamplesHaveBeenCollected, entity);
            ValidationAssert.EntityMustExist(_viewModel, x => x.WQPSamplesHaveBeenCollected, entity);
            ValidationAssert.EntityMustExist(_viewModel, x => x.SurfaceWaterPlantSamplesHaveBeenCollected, entity);
            ValidationAssert.EntityMustExist(_viewModel, x => x.ChlorineResidualsHaveBeenCollected, entity);

            ValidationAssert.EntityMustExist(_viewModel, x => x.CentralLabSamplesHaveBeenReported, entity);
            ValidationAssert.EntityMustExist(_viewModel, x => x.ContractedLabsSamplesHaveBeenReported, entity);
            ValidationAssert.EntityMustExist(_viewModel, x => x.InternalLabsSamplesHaveBeenReported, entity);
            ValidationAssert.EntityMustExist(_viewModel, x => x.BactiSamplesHaveBeenReported, entity);
            ValidationAssert.EntityMustExist(_viewModel, x => x.LeadAndCopperSamplesHaveBeenReported, entity);
            ValidationAssert.EntityMustExist(_viewModel, x => x.WQPSamplesHaveBeenReported, entity);
            ValidationAssert.EntityMustExist(_viewModel, x => x.SurfaceWaterPlantSamplesHaveBeenReported, entity);
            ValidationAssert.EntityMustExist(_viewModel, x => x.ChlorineResidualsHaveBeenReported, entity);
        }

        [TestMethod]
        public void TestVariousReasonFieldsAreRequiredWhenTheirMatchingAnswerFieldsAreNOandOnlyNO()
        {
            var noAnswer = GetFactory<NoWaterSampleComplianceFormAnswerTypeFactory>().Create();
            var notAvailableAnswer = GetFactory<NotAvailableWaterSampleComplianceFormAnswerTypeFactory>().Create();
            var yesAnswer = GetFactory<YesWaterSampleComplianceFormAnswerTypeFactory>().Create();

            void doTest(Action<CreateWaterSampleComplianceForm, int?> answerSetter, 
                Expression<Func<CreateWaterSampleComplianceForm, string>> noteGetter, 
                Action<CreateWaterSampleComplianceForm, string> noteSetter)
            {
                var target = new CreateWaterSampleComplianceForm(_container);

                noteSetter(target, null);
                // Test everything that should be valid for a null NoteText.
                answerSetter(target, null);
                ValidationAssert.ModelStateIsValid(target, noteGetter);
                answerSetter(target, notAvailableAnswer.Id);
                ValidationAssert.ModelStateIsValid(target, noteGetter);
                answerSetter(target, yesAnswer.Id);
                ValidationAssert.ModelStateIsValid(target, noteGetter);

                // Test everything for a NO value.
                answerSetter(target, noAnswer.Id);
                // Bypassing the attribute check is okay here because we are literally testing every possible
                // variation. PropertyIsRequiredWhen doesn't work well, here.
                ValidationAssert.PropertyIsRequired(target, noteGetter, validationNotDoneByAttribute: true);
                noteSetter(target, "some note");
                ValidationAssert.ModelStateIsValid(target, noteGetter);
            }

            doTest((targ, answer) => targ.CentralLabSamplesHaveBeenCollected = answer, 
                (x) => x.CentralLabSamplesReason,
                (x, note) => x.CentralLabSamplesReason = note);

            doTest((targ, answer) => targ.ContractedLabsSamplesHaveBeenCollected = answer,
                (x) => x.ContractedLabsSamplesReason,
                (x, note) => x.ContractedLabsSamplesReason = note);

            doTest((targ, answer) => targ.InternalLabsSamplesHaveBeenCollected = answer,
                (x) => x.InternalLabSamplesReason,
                (x, note) => x.InternalLabSamplesReason = note);

            doTest((targ, answer) => targ.BactiSamplesHaveBeenCollected = answer,
                (x) => x.BactiSamplesReason,
                (x, note) => x.BactiSamplesReason = note);

            doTest((targ, answer) => targ.LeadAndCopperSamplesHaveBeenCollected = answer,
                (x) => x.LeadAndCopperSamplesReason,
                (x, note) => x.LeadAndCopperSamplesReason = note);

            doTest((targ, answer) => targ.WQPSamplesHaveBeenCollected = answer, 
                (x) => x.WQPSamplesReason,
                (x, note) => x.WQPSamplesReason = note);

            doTest((targ, answer) => targ.SurfaceWaterPlantSamplesHaveBeenCollected = answer,
                (x) => x.SurfaceWaterPlantSamplesReason,
                (x, note) => x.SurfaceWaterPlantSamplesReason = note);

            doTest((targ, answer) => targ.ChlorineResidualsHaveBeenCollected = answer,
                (x) => x.ChlorineResidualsReason,
                (x, note) => x.ChlorineResidualsReason = note);

            doTest((targ, answer) => targ.CentralLabSamplesHaveBeenReported = answer, 
                (x) => x.CentralLabSamplesReason,
                (x, note) => x.CentralLabSamplesReason = note);

            doTest((targ, answer) => targ.ContractedLabsSamplesHaveBeenReported = answer,
                (x) => x.ContractedLabsSamplesReason,
                (x, note) => x.ContractedLabsSamplesReason = note);

            doTest((targ, answer) => targ.InternalLabsSamplesHaveBeenReported = answer,
                (x) => x.InternalLabSamplesReason,
                (x, note) => x.InternalLabSamplesReason = note);

            doTest((targ, answer) => targ.BactiSamplesHaveBeenReported = answer,
                (x) => x.BactiSamplesReason,
                (x, note) => x.BactiSamplesReason = note);

            doTest((targ, answer) => targ.LeadAndCopperSamplesHaveBeenReported = answer,
                (x) => x.LeadAndCopperSamplesReason,
                (x, note) => x.LeadAndCopperSamplesReason = note);

            doTest((targ, answer) => targ.WQPSamplesHaveBeenReported = answer, 
                (x) => x.WQPSamplesReason,
                (x, note) => x.WQPSamplesReason = note);

            doTest((targ, answer) => targ.SurfaceWaterPlantSamplesHaveBeenReported = answer,
                (x) => x.SurfaceWaterPlantSamplesReason,
                (x, note) => x.SurfaceWaterPlantSamplesReason = note);

            doTest((targ, answer) => targ.ChlorineResidualsHaveBeenReported = answer,
                (x) => x.ChlorineResidualsReason,
                (x, note) => x.ChlorineResidualsReason = note);
        }

        #endregion

        #endregion
    }
}
