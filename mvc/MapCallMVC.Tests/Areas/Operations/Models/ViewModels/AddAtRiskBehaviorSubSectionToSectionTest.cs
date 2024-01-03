using System;
using System.Linq;
using System.Security.Policy;
using FluentNHibernate.Utils;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Operations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.Operations.Models.ViewModels
{
    [TestClass]
    public class AddAtRiskBehaviorSubSectionToSectionTest : MapCallMvcInMemoryDatabaseTestBase<AtRiskBehaviorSubSection>
    {
        #region Fields

        private ViewModelTester<AddAtRiskBehaviorSubSectionToSection, AtRiskBehaviorSection> _vmTester;
        private AddAtRiskBehaviorSubSectionToSection _viewModel;
        private AtRiskBehaviorSection _entity;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = new AddAtRiskBehaviorSubSectionToSection(_container);
            _entity = new AtRiskBehaviorSection();
            _vmTester = new ViewModelTester<AddAtRiskBehaviorSubSectionToSection, AtRiskBehaviorSection>(_viewModel, _entity);
        }

        #endregion

        #region Tests


        [TestMethod]
        public void TestMapSetsSectionFromEntityId()
        {
            _entity.Id = 32;
            _viewModel.Map(_entity);
            Assert.AreEqual(32, _viewModel.Section);
        }

        [TestMethod]
        public void TestMapToEntityCreatesNewSubSectionAndSetsPropertiesFromViewModel()
        {
            _viewModel.Description = "Expected description";
            _viewModel.SubSectionNumber = 0.1m;

            _viewModel.MapToEntity(_entity);

            var result = _entity.SubSections.Single();

            Assert.AreSame(_entity, result.Section);
            Assert.AreEqual(0.1m, result.SubSectionNumber);
            Assert.AreEqual("Expected description", result.Description);
        }

        [TestMethod]
        public void TestMapToEntityDoesNotOverwriteDescriptionOnParentSectionEntity()
        {
            _entity.Description = "Yo";
            _viewModel.Description = "Sup";
            _viewModel.SubSectionNumber = 0.1m; // Needed or the test throws an exception.

            _viewModel.MapToEntity(_entity);

            Assert.AreEqual("Yo", _entity.Description, "This value should not have changed.");
        }

        #endregion
    }

}
