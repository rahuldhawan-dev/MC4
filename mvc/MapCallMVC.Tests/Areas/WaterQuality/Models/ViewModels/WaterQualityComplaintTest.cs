using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using VM = MapCallMVC.Areas.WaterQuality.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using MMSINC.Utilities.Pdf;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.WaterQuality.Models.ViewModels
{
    public abstract class WaterQualityComplaintTest<TViewModel> : ViewModelTestBase<WaterQualityComplaint, TViewModel>
        where TViewModel : VM.WaterQualityComplaintViewModel
    {
        #region Private Members

        private Mock<IAuthenticationService<User>> _authenticationService;
        protected Mock<IDateTimeProvider> _dateTimeProvider;
        protected DateTime _now;

        #endregion

        #region Private Methods

        [TestInitialize]
        public void TestInitialize()
        {
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(_now);
        }

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IServiceRepository>().Use<ServiceRepository>();
            e.For<ITapImageRepository>().Use<TapImageRepository>();
            e.For<IImageToPdfConverter>().Mock();
            _authenticationService = e.For<IAuthenticationService<User>>().Mock();
            _dateTimeProvider = e.For<IDateTimeProvider>().Mock();
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _container.GetInstance<ITestDataFactoryService>();
            _vmTester.CanMapBothWays(x => x.PublicWaterSupply);
            _vmTester.CanMapBothWays(x => x.Type);
            _vmTester.CanMapBothWays(x => x.DateComplaintReceived);
            _vmTester.CanMapBothWays(x => x.InitialLocalResponseDate);
            _vmTester.CanMapBothWays(x => x.InitialLocalContact);
            _vmTester.CanMapBothWays(x => x.InitialLocalResponseType);
            _vmTester.CanMapBothWays(x => x.NotificationCompletedBy);
            _vmTester.CanMapBothWays(x => x.NotificationCompletionDate);
            _vmTester.CanMapBothWays(x => x.NotificationCreatedBy);
            _vmTester.CanMapBothWays(x => x.CustomerName);
            _vmTester.CanMapBothWays(x => x.HomePhoneNumber);
            _vmTester.CanMapBothWays(x => x.Ext);
            _vmTester.CanMapBothWays(x => x.StreetNumber);
            _vmTester.CanMapBothWays(x => x.StreetName);
            _vmTester.CanMapBothWays(x => x.ApartmentNumber);
            _vmTester.CanMapBothWays(x => x.State);
            _vmTester.CanMapBothWays(x => x.Town);
            _vmTester.CanMapBothWays(x => x.OperatingCenter);
            _vmTester.CanMapBothWays(x => x.TownSection);
            _vmTester.CanMapBothWays(x => x.ZipCode);
            _vmTester.CanMapBothWays(x => x.PremiseNumber);
            _vmTester.CanMapBothWays(x => x.AccountNumber);
            _vmTester.CanMapBothWays(x => x.ComplaintStartDate);
            _vmTester.CanMapBothWays(x => x.ProblemArea);
            _vmTester.CanMapBothWays(x => x.Source);
            _vmTester.CanMapBothWays(x => x.SiteVisitRequired);
            _vmTester.CanMapBothWays(x => x.SiteVisitBy);
            _vmTester.CanMapBothWays(x => x.WaterFilterOnComplaintSource);
            _vmTester.CanMapBothWays(x => x.CrossConnectionDetected);
            _vmTester.CanMapBothWays(x => x.ProbableCause);
            _vmTester.CanMapBothWays(x => x.ActionTaken);
            _vmTester.CanMapBothWays(x => x.CustomerExpectation);
            _vmTester.CanMapBothWays(x => x.CustomerSatisfaction);
            _vmTester.CanMapBothWays(x => x.RootCauseIdentified);
            _vmTester.CanMapBothWays(x => x.RootCause);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.CoordinateId);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.State);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Town);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.OperatingCenter);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.PremiseNumber);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.InitialLocalContact);

            ValidationAssert.PropertyIsRequiredWhen(
                _viewModel,
                x => x.CustomerSatisfaction,
                GetEntityFactory<WaterQualityComplaintCustomerSatisfaction>().Create().Id, 
                x => x.NotificationCompletionDate,
                _now);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.NotificationCreatedBy, WaterQualityComplaint.StringLengths.NOTIFICATION_CREATED_BY);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.CustomerName, WaterQualityComplaint.StringLengths.CUSTOMER_NAME);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.HomePhoneNumber, WaterQualityComplaint.StringLengths.HOME_PHONE_NUMBER);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.Ext, WaterQualityComplaint.StringLengths.EXTENSION);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.StreetNumber, WaterQualityComplaint.StringLengths.STREET_NUMBER);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.StreetName, WaterQualityComplaint.StringLengths.STREET_NAME);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.ApartmentNumber, WaterQualityComplaint.StringLengths.APARTMENT_NUMBER);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.ZipCode, WaterQualityComplaint.StringLengths.ZIP_CODE);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.PremiseNumber, WaterQualityComplaint.StringLengths.PREMISE_NUMBER);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.AccountNumber, WaterQualityComplaint.StringLengths.ACCOUNT_NUMBER);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.SiteVisitBy, WaterQualityComplaint.StringLengths.SITE_VISIT_BY);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.PublicWaterSupply, GetEntityFactory<PublicWaterSupply>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.Type, GetEntityFactory<WaterQualityComplaintType>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.CoordinateId, GetEntityFactory<Coordinate>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.InitialLocalResponseType, GetEntityFactory<WaterQualityComplaintLocalResponseType>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.State, GetEntityFactory<State>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.Town, GetEntityFactory<Town>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.OperatingCenter, GetEntityFactory<OperatingCenter>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.TownSection, GetEntityFactory<TownSection>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.ProblemArea, GetEntityFactory<WaterQualityComplaintProblemArea>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.Source, GetEntityFactory<WaterQualityComplaintSource>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.ProbableCause, GetEntityFactory<WaterQualityComplaintProbableCause>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.ActionTaken, GetEntityFactory<WaterQualityComplaintActionsWhichCanBeTaken>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.CustomerExpectation, GetEntityFactory<WaterQualityComplaintCustomerExpectation>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.CustomerSatisfaction, GetEntityFactory<WaterQualityComplaintCustomerSatisfaction>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.RootCause, GetEntityFactory<WaterQualityComplaintRootCause>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.InitialLocalContact, GetEntityFactory<Employee>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.NotificationCompletedBy, GetEntityFactory<Employee>().Create());
        }

        #endregion
    }
}
