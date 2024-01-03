using System.Reflection;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using WorkOrders.Model;
using WorkOrders.Presenters.Documents;

namespace _271ObjectTests.Tests.Unit.Presenters.Documents
{
    /// <summary>
    /// Summary description for DocumentResourceRPCPresenterTest.
    /// </summary>
    [TestClass]
    public class DocumentResourceRPCPresenterTest : EventFiringTestClass
    {
        #region Private Members

        private TestDocumentResourceRPCPresenter _target;
        private IResourceRPCView<Document> _view;
        private IRepository<Document> _repository;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void DocumentResourceRPCPresenterTestInitialize()
        {
            _mocks
                .DynamicMock(out _view)
                .DynamicMock(out _repository);
            _target = new TestDocumentResourceRPCPresenterBuilder(_view, _repository);
        }

        #endregion

        [TestMethod]
        public void TestProcessCommandAndArgumentGetsDocumentFromRepositoryUsingViewArgumentAndShowsItInView()
        {
            var argument = "1234";
            var document = new Document();

            using (_mocks.Record())
            {
                SetupResult.For(_view.Argument).Return(argument);
                SetupResult.For(_repository.Get(argument)).Return(document);
            }

            using (_mocks.Playback())
            {
                var mi = _target.GetType().GetMethod("ProcessCommandAndArgument",
                    BindingFlags.Instance |
                    BindingFlags.NonPublic);
                mi.Invoke(_target, null);
            }
        }
    }

    internal class TestDocumentResourceRPCPresenterBuilder : TestDataBuilder<TestDocumentResourceRPCPresenter>
    {
        private readonly IResourceRPCView<Document> _view;
        private readonly IRepository<Document> _repository;

        #region Constructors

        public TestDocumentResourceRPCPresenterBuilder(IResourceRPCView<Document> view, IRepository<Document> repository)
        {
            _view = view;
            _repository = repository;
        }

        #endregion

        #region Exposed Methods

        public override TestDocumentResourceRPCPresenter Build()
        {
            var obj = new TestDocumentResourceRPCPresenter(_view, _repository);
            return obj;
        }

        #endregion
    }

    internal class TestDocumentResourceRPCPresenter : DocumentResourceRPCPresenter
    {
        public TestDocumentResourceRPCPresenter(IResourceRPCView<Document> view, IRepository<Document> repository) : base(view, repository)
        {
        }
    }
}
