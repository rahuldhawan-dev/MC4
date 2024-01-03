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
using System;
using System.Linq.Expressions;

namespace MapCallMVC.Tests.Areas.Environmental.Models.ViewModels
{
    [TestClass]
    public class EditWaterSampleComplianceFormTest : MapCallMvcInMemoryDatabaseTestBase<BacterialWaterSample>
    {
        #region Fields

        private ViewModelTester<EditWaterSampleComplianceForm, WaterSampleComplianceForm> _vmTester;
        private Mock<IDateTimeProvider> _dateTimeProvider;
        private EditWaterSampleComplianceForm _viewModel;
        private WaterSampleComplianceForm _entity;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;
        private Mock<IUserRepository> _userRepo;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _userRepo = new Mock<IUserRepository>();
            _user = GetFactory<UserFactory>().Create(new { IsAdmin = true });
            _authServ = new Mock<IAuthenticationService<User>>();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _viewModel = new EditWaterSampleComplianceForm(_container);
            _entity = new WaterSampleComplianceForm();
            _vmTester = new ViewModelTester<EditWaterSampleComplianceForm, WaterSampleComplianceForm>(_viewModel, _entity);

            _container.Inject(_dateTimeProvider.Object);
            _container.Inject(_authServ.Object);
            _container.Inject(_userRepo.Object);
        }

        #endregion

        #region Tests

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

            void doTest(Action<EditWaterSampleComplianceForm, int?> answerSetter,
                Expression<Func<EditWaterSampleComplianceForm, string>> noteGetter,
                Action<EditWaterSampleComplianceForm, string> noteSetter)
            {
                var target = new EditWaterSampleComplianceForm(_container);

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

        [TestMethod]
        public void TestMapToEntityDoesNotOverwriteDateCertified()
        {
            var expected = new DateTime(2018, 3, 12);
            _entity.DateCertified = expected;
            _viewModel.DateCertified = null;

            _vmTester.MapToEntity();
            Assert.AreEqual(expected, _entity.DateCertified);
        }

        [TestMethod]
        public void TestMapToEntityDoesNotOverwriteCertifedBy()
        {
            var expected = new User();
            _entity.CertifiedBy = expected;
            _viewModel.CertifiedBy = "bleep bloop";

            _vmTester.MapToEntity();

            Assert.AreSame(expected, _entity.CertifiedBy);
        }

        [TestMethod]
        public void TestMapSetsCertifedByToUserFullName()
        {
            var expected = "My full name";
            _entity.CertifiedBy = new User {
                FullName = expected
            };
            _viewModel.CertifiedBy = null;

            _vmTester.MapToViewModel();

            Assert.AreEqual(expected, _viewModel.CertifiedBy);
        }

        [TestMethod]
        public void TestMapSetsDateCertified()
        {
            var expected = new DateTime(2932, 1, 12);
            _entity.DateCertified = expected;

            _vmTester.MapToViewModel();
            Assert.AreEqual(expected, _viewModel.DateCertified);
        }

        #endregion
    }
}
