using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.HealthAndSafety.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Testing;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.HealthAndSafety.Models.ViewModels
{
    [TestClass]
    public abstract class IncidentInvestigationViewModelTest<T> : MapCallMvcInMemoryDatabaseTestBase<IncidentInvestigation> where T : IncidentInvestigationViewModel
    {
        #region Fields

        protected ViewModelTester<T, IncidentInvestigation> _vmTester;
        protected T _viewModel;
        protected IncidentInvestigation _entity;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            var user = GetEntityFactory<User>().Create();
            _entity = GetEntityFactory<IncidentInvestigation>().Create();
            _entity.RootCauseFindingPerformedByUsers.Add(user);
            _viewModel = _viewModelFactory.Build<T, IncidentInvestigation>(_entity);
            _vmTester = new ViewModelTester<T, IncidentInvestigation>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.Incident, GetEntityFactory<Incident>().Create());
            _vmTester.CanMapBothWays(x => x.IncidentInvestigationRootCauseFindingType, GetEntityFactory<IncidentInvestigationRootCauseFindingType>().Create());
            _vmTester.CanMapBothWays(x => x.IncidentInvestigationRootCauseLevel1Type, GetEntityFactory<IncidentInvestigationRootCauseLevel1Type>().Create());
            _vmTester.CanMapBothWays(x => x.IncidentInvestigationRootCauseLevel2Type, GetEntityFactory<IncidentInvestigationRootCauseLevel2Type>().Create());
            _vmTester.CanMapBothWays(x => x.IncidentInvestigationRootCauseLevel3Type, GetEntityFactory<IncidentInvestigationRootCauseLevel3Type>().Create());
        }

        [TestMethod]
        public void TestRootCauseFindingPerformedByUsersCanMapBothWays()
        {
            _entity.RootCauseFindingPerformedByUsers.Clear();
            _viewModel.RootCauseFindingPerformedByUsers = null;

            var user = GetEntityFactory<User>().Create();
            _entity.RootCauseFindingPerformedByUsers.Add(user);
            _vmTester.MapToViewModel();
            Assert.AreEqual(user.Id, _viewModel.RootCauseFindingPerformedByUsers.Single());

            _entity.RootCauseFindingPerformedByUsers.Clear();
            _vmTester.MapToEntity();
            Assert.IsTrue(_entity.RootCauseFindingPerformedByUsers.Contains(user));
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Incident);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.IncidentInvestigationRootCauseFindingType);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.IncidentInvestigationRootCauseLevel1Type);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.IncidentInvestigationRootCauseLevel2Type);
            // No, Level3 is not required.
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.RootCauseFindingPerformedByUsers);
        }

        [TestMethod]
        public void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.Incident, GetEntityFactory<Incident>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.IncidentInvestigationRootCauseFindingType, GetEntityFactory<IncidentInvestigationRootCauseFindingType>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.IncidentInvestigationRootCauseLevel1Type, GetEntityFactory<IncidentInvestigationRootCauseLevel1Type>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.IncidentInvestigationRootCauseLevel2Type, GetEntityFactory<IncidentInvestigationRootCauseLevel2Type>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.IncidentInvestigationRootCauseLevel3Type, GetEntityFactory<IncidentInvestigationRootCauseLevel3Type>().Create());
        }

        [TestMethod]
        public void TestEntityMustExistForRootCauseFindingPerformedByUsers()
        {
            const string expectedError = "RootCauseFindingPerformedByUsers's value does not match an existing object.";
            var user = GetEntityFactory<User>().Create(0);

            // Test single invalid entity fails
            _viewModel.RootCauseFindingPerformedByUsers = new[] {-1};
            ValidationAssert.ModelStateHasError(_viewModel, x => x.RootCauseFindingPerformedByUsers, expectedError);

            // Test multiple invalid entity fails
            _viewModel.RootCauseFindingPerformedByUsers = new[] { -1, -2 };
            ValidationAssert.ModelStateHasError(_viewModel, x => x.RootCauseFindingPerformedByUsers, expectedError);

            // Test mix of valid and invalid entity fails
            _viewModel.RootCauseFindingPerformedByUsers = new[] { -1, user.Id };
            ValidationAssert.ModelStateHasError(_viewModel, x => x.RootCauseFindingPerformedByUsers, expectedError);

            _viewModel.RootCauseFindingPerformedByUsers = new[] { user.Id };
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.RootCauseFindingPerformedByUsers);
        }

        [TestMethod]
        public void TestRootcauseFindingPerformedByUsersRequiresAtLeastOneValue()
        {
            // These tests are going to fail once the ValidationAssert fix branch gets pushed. 
            // Replace these with the commented out lines.

            _viewModel.RootCauseFindingPerformedByUsers = null;
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.RootCauseFindingPerformedByUsers);
            // ValidationAssert.PropertyIsRequired(_viewModel, x => x.RootCauseFindingPerformedByUsers, validationNotDoneByAttribute: true);

            _viewModel.RootCauseFindingPerformedByUsers = new int[0];
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.RootCauseFindingPerformedByUsers);
            // ValidationAssert.PropertyIsRequired(_viewModel, x => x.RootCauseFindingPerformedByUsers, validationNotDoneByAttribute: true);

            var user = GetEntityFactory<User>().Create(0);
            _viewModel.RootCauseFindingPerformedByUsers = new[] { user.Id };
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.RootCauseFindingPerformedByUsers);
        }

        #endregion
    }

    [TestClass]
    public class CreateIncidentInvestigationTest : IncidentInvestigationViewModelTest<CreateIncidentInvestigation>
    {
        #region Tests

        [TestMethod]
        public void TestValidationFailsIfIncidentIsOSHARecordableIsFalse()
        {
            _entity.Incident.IsOSHARecordable = false;
            ValidationAssert.ModelStateHasNonPropertySpecificError(_viewModel, "Incident investigations can not be created for an incident that is not OSHA recordable.");

            _entity.Incident.IsOSHARecordable = true;
            ValidationAssert.ModelStateIsValid(_viewModel);
        }

        #endregion
    }

    [TestClass]
    public class EditIncidentInvestigationTest : IncidentInvestigationViewModelTest<EditIncidentInvestigation>
    {
        // This test just runs all the base tests. Nothing special added.
    }
}
