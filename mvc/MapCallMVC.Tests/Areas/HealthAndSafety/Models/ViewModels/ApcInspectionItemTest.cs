using System;
using System.Linq;
using MapCall.Common.Model.Entities;
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
    public class CreateApcInspectionItemTest : MapCallMvcInMemoryDatabaseTestBase<ApcInspectionItem>
    {
        #region Fields

        private ViewModelTester<CreateApcInspectionItem, ApcInspectionItem> _vmTester;
        private CreateApcInspectionItem _viewModel;
        private CreateApcFormAnswer _createViewModel;
        private ApcInspectionItem _entity;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = new CreateApcInspectionItem(_container);
            _createViewModel = new CreateApcFormAnswer(_container);
            _entity = new ApcInspectionItem();
            _vmTester = new ViewModelTester<CreateApcInspectionItem, ApcInspectionItem>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.Area);
            _vmTester.CanMapBothWays(x => x.Description);
            _vmTester.CanMapBothWays(x => x.DateReported);
            _vmTester.CanMapBothWays(x => x.DateInspected);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.OperatingCenter);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Facility);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Area);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Description);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Type);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.DateReported);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.AssignedTo);
        }

        [TestMethod]
        public void TestMapToEntityMapsFacilityInspectionFormAnswers()
        {
            var category = GetFactory<FacilityInspectionFormQuestionCategoriesFactory>().Create();
            var question = GetEntityFactory<FacilityInspectionFormQuestion>().Create(new {
                Category = category,
                DisplayOrder = 1,
                Weightage = 3,
                Question = "Would you like to play a game of questions?"
            });

            _entity.FacilityInspectionFormAnswers.Add(new FacilityInspectionFormAnswer {
                ApcInspectionItem = _entity,
                FacilityInspectionFormQuestion = question,
                IsPictureTaken = true,
                Comments = "test",
                IsSafe = false
            });
            _viewModel.MapToEntity(_entity);

            var firstAnswer = _entity.FacilityInspectionFormAnswers.First();
            Assert.AreEqual(question, firstAnswer.FacilityInspectionFormQuestion); 
            Assert.AreEqual(question, firstAnswer.FacilityInspectionFormQuestion);
            Assert.AreEqual(_entity, firstAnswer.ApcInspectionItem);
            Assert.AreEqual("test", firstAnswer.Comments);
        }

        #endregion
    }

    [TestClass]
    public class EditApcInspectionItemTest : MapCallMvcInMemoryDatabaseTestBase<ApcInspectionItem>
    {
        #region Fields

        private ViewModelTester<EditApcInspectionItem, ApcInspectionItem> _vmTester;
        private EditApcInspectionItem _viewModel;
        private EditApcFormAnswer _editViewModel;
        private ApcInspectionItem _entity;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = new EditApcInspectionItem(_container);
            _editViewModel = new EditApcFormAnswer(_container);
            _entity = new ApcInspectionItem();
            _vmTester = new ViewModelTester<EditApcInspectionItem, ApcInspectionItem>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.Area);
            _vmTester.CanMapBothWays(x => x.Description);
            _vmTester.CanMapBothWays(x => x.DateReported);
            _vmTester.CanMapBothWays(x => x.DateInspected);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.OperatingCenter);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Facility);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Area);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Description);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Type);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.DateReported);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.AssignedTo);
        }

        [TestMethod]
        public void TestFacilityInspectionQuestionFormSetDefaults()
        {
            var questionCategories = GetFactory<FacilityInspectionFormQuestionCategoriesFactory>().CreateAll();

            foreach (var category in questionCategories)
            {
                GetEntityFactory<FacilityInspectionFormQuestion>().Create(new {
                    Category = category,
                    DisplayOrder = 1,
                    Weightage = 3,
                    Question = "Would you like to play a game of questions?"
                });
            }
            // we want to make it null in this case to see that we're only setting it when it's not set
            _viewModel.SetDefaults();
            Assert.AreEqual(4, _viewModel.CreateApcFormAnswers.Count);
            foreach (var category in questionCategories)
            {
                Assert.IsTrue(_viewModel.CreateApcFormAnswers.Any(x => x.Category == category.Id));
            }
        }

        [TestMethod]
        public void TestMapToEntityMapsAnswersToEntity()
        {
            var category = GetFactory<FacilityInspectionFormQuestionCategoriesFactory>().Create();
            var question = GetEntityFactory<FacilityInspectionFormQuestion>().Create(new {
                Category = category,
                DisplayOrder = 1,
                Weightage = 3,
                Question = "Would you like to play a game of questions?"
            });

            _entity.FacilityInspectionFormAnswers.Add(new FacilityInspectionFormAnswer {
                ApcInspectionItem = _entity,
                FacilityInspectionFormQuestion = question,
                IsPictureTaken = true,
                Comments = "test",
                IsSafe = false
            });
            Session.Clear();

            _viewModel.Map(_entity);

            var firstAnswer = _viewModel.EditApcFormAnswers.First();
            Assert.AreEqual(question.Id, firstAnswer.FacilityInspectionFormQuestion);
            Assert.AreEqual(category.Id, firstAnswer.Category);
            Assert.IsFalse(firstAnswer.IsSafe.Value);
            Assert.AreEqual("test", firstAnswer.Comments);
        }

        #endregion
    }
}
