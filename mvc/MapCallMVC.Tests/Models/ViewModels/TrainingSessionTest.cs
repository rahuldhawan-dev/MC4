using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class EditTrainingSessionTest : MapCallMvcInMemoryDatabaseTestBase<TrainingSession>
    {
        #region Fields

        private ViewModelTester<EditTrainingSession, TrainingSession> _vmTester;
        private EditTrainingSession _viewModel;
        private TrainingSession _entity;
        
        #endregion
        
        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = new EditTrainingSession(_container);
            _entity = new TrainingSession();
            _vmTester = new ViewModelTester<EditTrainingSession, TrainingSession>(_viewModel, _entity);
        }

        #endregion

        [TestMethod]
        public void TestRequiredFieldsAreRequired()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.EndDateTime);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.StartDateTime);
        }
    }
}