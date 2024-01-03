using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions;
using MMSINC.Utilities.Documents;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities.StructureMap;
using Rhino.Mocks;
using StructureMap;
using Subtext.TestLibrary;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Model
{
    [TestClass]
    public class DocumentDataRepositoryTest : EventFiringTestClass
    {
        #region Fields

        private TestDocumentDataRepository _target;
        private IDocumentService _docServ;
        private ITable<DocumentData> _dataTable;
        private IContainer _container;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();
            _container = new Container();

            _mocks
                .DynamicMock(out _docServ)
                .DynamicMock(out _dataTable);
            _container.Inject(_docServ);
            _container.Inject<IDataContext>(new WorkOrdersDataContext());

            _target = new TestDocumentDataRepositoryBuilder()
                .WithDataTable(_dataTable);

            DependencyResolver.SetResolver(
                new StructureMapDependencyResolver(_container));
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestSaveOrGetExistingSavesIfExistingDoesNotExist()
        {
            using (var simulator = new HttpSimulator().SimulateRequest())
            {
                var binary = new byte[] { 1, 2, 3 };
                var hash = "somehash";
                var queryResult = new QueryResultWrapper<DocumentData>(new List<DocumentData>().AsQueryable());

                using (_mocks.Record())
                {
                    SetupResult.For(_docServ.GetFileHash(binary))
                        .Return(hash);
                    SetupResult.For(_dataTable.Where(x => x.Hash == hash))
                        .IgnoreArguments()
                        .Return(new QueryResultWrapper<DocumentData>(queryResult));
                    SetupResult.For(_docServ.Save(binary)).Return(hash);
                }

                using (_mocks.Playback())
                {
                    var result = _target.SaveOrGetExisting(binary);
                    Assert.IsNotNull(result);
                }
            }
        }

        [TestMethod]
        public void TestSaveOrGetExistingReturnsExisting()
        {
            using (var simulator = new HttpSimulator().SimulateRequest())
            {
                var binary = new byte[] { 1, 2, 3 };
                var hash = "somehash";

                var docData = new DocumentData() { Hash = hash };
                var fakeTable = new List<DocumentData>();
                fakeTable.Add(docData);

                var queryResult = new QueryResultWrapper<DocumentData>(fakeTable.AsQueryable());

                using (_mocks.Record())
                {
                    SetupResult.For(_docServ.GetFileHash(binary))
                        .Return(hash);
                    SetupResult.For(_dataTable.Where(x => x.Hash == hash))
                        .IgnoreArguments()
                        .Return(queryResult);
                }

                using (_mocks.Playback())
                {
                    var result = _target.SaveOrGetExisting(binary);
                    Assert.AreSame(docData, result);
                }
            }
        }


        #endregion
    }

    internal class TestDocumentDataRepositoryBuilder : TestDataBuilder<TestDocumentDataRepository>
    {
        #region Private Members

        private ITable<DocumentData> _dataTable;

        #endregion

        #region Exposed Methods

        public override TestDocumentDataRepository Build()
        {
            var obj = new TestDocumentDataRepository();
            if (_dataTable != null)
                obj.SetDataTable(_dataTable);
            return obj;
        }

        public TestDocumentDataRepository WithDataTable(ITable<DocumentData> dataTable)
        {
            _dataTable = dataTable;
            return this;
        }

        #endregion
    }

    internal class TestDocumentDataRepository : DocumentDataRepository
    {
        #region Exposed Methods

        public void SetDataTable(ITable<DocumentData> dataTable)
        {
            _dataTable = dataTable;
        }

        #endregion
    }

}
