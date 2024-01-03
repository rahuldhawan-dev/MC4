using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Exceptions;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.NHibernate;
using Moq;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.Linq;
using NHibernate.Metadata;
using NHibernate.SqlCommand;
using StructureMap;

namespace MMSINC.CoreTest.Data.NHibernate
{
    /// <summary>
    /// Summary description for RepositoryBaseTest
    /// </summary>
    [TestClass]
    public class RepositoryBaseTest
    {
        #region Private Members

        private TestRepository _target;
        private Mock<ISession> _sessionMock;
        private Mock<ISessionFactory> _sessionFactoryMock;
        private Mock<IClassMetadata> _classMetadataMock;
        private Mock<ICriteria> _criteriaMock;
        private Mock<ISearchMapper> _searchMapper;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void RepositoryBaseTestInitialize()
        {
            _classMetadataMock = new Mock<IClassMetadata>();
            _sessionFactoryMock = new Mock<ISessionFactory>();
            _sessionMock = new Mock<ISession>();
            _sessionMock.Setup(x => x.SessionFactory).Returns(_sessionFactoryMock.Object);
            _criteriaMock = new Mock<ICriteria>();
            _searchMapper = new Mock<ISearchMapper>();
            _target = new TestRepository(_sessionMock.Object, new Container());
            _target.TestSearchMapper = _searchMapper.Object;
        }

        #endregion

        #region Linq

        [TestMethod]
        public void TestLinqPropertyReturnsTypedQueryObjectFromSession()
        {
            var queryObj = _target.GetLinqProperty();

            Assert.AreSame(typeof(TestUser), queryObj.ElementType);
            Assert.IsInstanceOfType(queryObj, typeof(EnumerableQuery<TestUser>));
        }

        #endregion

        #region GetAll()

        [TestMethod]
        public void TestGetAllReturnsLinqProperty()
        {
            Assert.IsInstanceOfType(_target.GetAll(),
                typeof(EnumerableQuery<TestUser>));
        }

        #endregion

        #region GetAllSorted

        [TestMethod]
        public void TestGetAllSortedReturnsIQueryable()
        {
            // NOTE: This test doesn't test for any actual sorting because RepositoryBase.GetAllSorted
            // doesn't sort anything by default.

            var expected = new TestUser();
            var queryable = new List<TestUser>(new[] {expected}).AsQueryable();
            _target.SetLinqProperty(queryable);

            var result = _target.GetAllSorted();
            Assert.IsInstanceOfType(result, typeof(IQueryable<TestUser>));
            Assert.AreSame(expected, result.Single());
        }

        [TestMethod]
        public void TestGetAllSortedWithSortExpressionReturnsIQueryableInTheOrderExpected()
        {
            var expectedOne = new TestUser {Email = "b"};
            var expectedTwo = new TestUser {Email = "a"};
            var queryable = new List<TestUser>(new[] {expectedOne, expectedTwo}).AsQueryable();
            _target.SetLinqProperty(queryable);

            var result = _target.GetAllSorted(x => x.Email);

            Assert.IsInstanceOfType(result, typeof(IQueryable<TestUser>));

            Assert.AreSame(expectedTwo, result.First());
            Assert.AreSame(expectedOne, result.Last());
        }

        #endregion

        #region GetAllAs

        [TestMethod]
        public void TestGetAllAsGetsAllAs()
        {
            var expected = new TestUser {Email = "email"};
            _target.SetLinqProperty(new[] {expected}.AsQueryable());

            var result = _target.GetAllAs(x => new ADifferentModelOfTestUser {Email = x.Email});
            Assert.AreEqual("email", result.Single().Email);
        }

        #endregion

        #region Criteria

        [TestMethod]
        public void TestCriteriaPropertyReturnsTypedCriteriaObjectFromSession()
        {
            var criteriaObj = new Mock<ICriteria>();
            _sessionMock
               .Setup(x => x.CreateCriteria<TestUser>("testuser"))
               .Returns(criteriaObj.Object);

            Assert.AreSame(criteriaObj.Object, _target.GetCriteriaObj());

            _sessionMock.Verify(x => x.CreateCriteria<TestUser>("testuser"));
        }

        #endregion

        #region Save(TEntity)

        [TestMethod]
        public void TestSaveSavesItemToAndFlushesSession()
        {
            var obj = new TestUser();
            _target.Save(obj);
            _sessionMock.Verify(x => x.Save(obj));
            _sessionMock.Verify(x => x.Flush());
        }

        #endregion

        #region Save(IEnumerable<TEntity>)

        [TestMethod]
        public void TestSaveSavesAllItemsAndOnlyFlushesTheSessionOnce()
        {
            var obj1 = new TestUser();
            var obj2 = new TestUser();
            _target.Save(new[] {obj1, obj2});
            _sessionMock.Verify(x => x.Save(obj1));
            _sessionMock.Verify(x => x.Save(obj2));
            _sessionMock.Verify(x => x.Flush(), Times.Once());
        }

        #endregion

        #region Find(int)

        [TestMethod]
        public void TestFindFindsItemByPrimaryKey()
        {
            var id = 666;
            var expected = new TestUser();
            _sessionMock.Setup(x => x.Get<TestUser>(id)).Returns(expected);

            var result = _target.Find(id);

            Assert.AreSame(expected, result);
            _criteriaMock.VerifyAll();
        }

        [TestMethod]
        public void TestFindDoesNotHitDatabaseIfPrimaryKeyValueEqualsZero()
        {
            // We wanna verify that the Criteria is never used.
            var criteriaMock = new Mock<ICriteria>(MockBehavior.Strict);
            _target.SetCriteriaObj(criteriaMock.Object);
            _target.Find(0);
            criteriaMock.VerifyAll();
        }

        #endregion

        #region FindManyByIds

        // BIG NOTE: I'm not comfortable with these tests. Because they're entirely mocked
        // it's hard to know if they actually function properly on the query end of things.

        [TestMethod]
        public void TestFindManyByIdsReturnsExpectedResultsAsDictionary()
        {
            var entity1 = new TestUser(); // With Id == 1
            var entity2 = new TestUser(); // With Id == 2
            _sessionFactoryMock.Setup(x => x.GetClassMetadata(typeof(TestUser))).Returns(_classMetadataMock.Object);
            _classMetadataMock.Setup(x => x.GetIdentifier(entity1)).Returns(1);
            _classMetadataMock.Setup(x => x.GetIdentifier(entity2)).Returns(2);

            var results = new List<TestUser>();
            results.Add(entity1);
            results.Add(entity2);

            var someIds = new[] {1, 2};
            InExpression inRestriction = null;
            _criteriaMock.Setup(x => x.Add(It.IsAny<ICriterion>()))
                         .Callback<ICriterion>((x) => inRestriction = (InExpression)x)
                         .Returns(_criteriaMock.Object);
            _target.SetCriteriaObj(_criteriaMock.Object);
            _criteriaMock
               .Setup(x => x.List<TestUser>())
               .Returns(results);

            var resultDict = _target.FindManyByIds(someIds);

            Assert.AreSame(entity1, resultDict[1]);
            Assert.AreSame(entity2, resultDict[2]);

            Assert.AreEqual(2, inRestriction.Values.Count());
            Assert.IsTrue(inRestriction.Values.Contains(1));
            Assert.IsTrue(inRestriction.Values.Contains(2));

            _classMetadataMock.Verify(x => x.GetIdentifier(entity1));
            _classMetadataMock.Verify(x => x.GetIdentifier(entity2));
        }

        [TestMethod]
        public void
            TestFindManyByIdsReturnsNullValuesWhenTheIdsDoNotMatchAnExistingRecordOrOtherwiseAreNotReturnedByTheQuery()
        {
            // NOTE: No way of knowing if this test is passing because the query didn't return something
            //       or because the mock wasn't setup to return a value.
            var entity1 = new TestUser(); // With Id == 1
            var entity2 = new TestUser(); // With Id == 2
            _sessionFactoryMock.Setup(x => x.GetClassMetadata(typeof(TestUser))).Returns(_classMetadataMock.Object);
            _classMetadataMock.Setup(x => x.GetIdentifier(entity1)).Returns(1);
            _classMetadataMock.Setup(x => x.GetIdentifier(entity2)).Returns(2);

            var results = new List<TestUser>();
            results.Add(entity1);

            var someIds = new[] {1, 2};

            _criteriaMock.Setup(x => x.Add(It.IsAny<ICriterion>())).Returns(_criteriaMock.Object);
            _target.SetCriteriaObj(_criteriaMock.Object);
            _criteriaMock
               .Setup(x => x.List<TestUser>())
               .Returns(results);

            var resultDict = _target.FindManyByIds(someIds);

            Assert.AreSame(entity1, resultDict[1]);
            Assert.IsNull(resultDict[2]);

            _classMetadataMock.Verify(x => x.GetIdentifier(entity1));
        }

        [TestMethod]
        public void TestFindManyByIdsRemovesDuplicateIds()
        {
            var entity1 = new TestUser(); // With Id == 1
            _sessionFactoryMock.Setup(x => x.GetClassMetadata(typeof(TestUser))).Returns(_classMetadataMock.Object);
            _classMetadataMock.Setup(x => x.GetIdentifier(entity1)).Returns(1);

            var results = new List<TestUser>();
            results.Add(entity1);

            var someIds = new[] {1, 1};
            InExpression inRestriction = null;
            _criteriaMock.Setup(x => x.Add(It.IsAny<ICriterion>()))
                         .Callback<ICriterion>((x) => inRestriction = (InExpression)x)
                         .Returns(_criteriaMock.Object);
            _target.SetCriteriaObj(_criteriaMock.Object);
            _criteriaMock
               .Setup(x => x.List<TestUser>())
               .Returns(results);

            var resultDict = _target.FindManyByIds(someIds);

            Assert.AreSame(entity1, resultDict[1]);

            Assert.AreEqual(1, inRestriction.Values.Count());
            Assert.IsTrue(inRestriction.Values.Contains(1));

            _classMetadataMock.Verify(x => x.GetIdentifier(entity1));
        }

        #endregion

        #region Exists(int)

        [TestMethod]
        public void TestExistsReturnsTrueIfObjectWithTheSpecifiedIdIsFound()
        {
            var id = 666;
            var expected = new TestUser();
            _sessionMock.Setup(x => x.Get<TestUser>(id)).Returns(expected);

            Assert.IsTrue(_target.Exists(id));
            _criteriaMock.VerifyAll();
        }

        [TestMethod]
        public void TestExistsReturnsFalseIfObjectWithTheSpecifiedIdIsNotFound()
        {
            var id = 666;
            _sessionMock.Setup(x => x.Get<TestUser>(id)).Returns((TestUser)null);

            Assert.IsFalse(_target.Exists(id));
            _criteriaMock.VerifyAll();
        }

        #endregion

        #region Search(ICriterion)

        [TestMethod]
        public void TestSearchRunsQueryUsingGivenCriterion()
        {
            var count = 100;
            var results = new Mock<IList>();
            var criterionMock = new Mock<ICriterion>();

            _target.SetCriteriaObj(_criteriaMock.Object);
            _criteriaMock
               .Setup(x => x.Add(criterionMock.Object))
               .Returns(_criteriaMock.Object);
            _criteriaMock
               .Setup(x => x.List())
               .Returns(results.Object);

            _target.Search(criterionMock.Object);
        }

        [TestMethod]
        public void TestSearchRunsQueryUsingGivenCriterionAndAliases()
        {
            var count = 108;
            var results = new Mock<IList>();
            var criterionMock = new Mock<ICriterion>();
            var aliases = new Dictionary<string, string> {{"k", "val"}};

            _target.SetCriteriaObj(_criteriaMock.Object);
            _criteriaMock.Setup(x => x.Add(criterionMock.Object)).Returns(_criteriaMock.Object);
            _criteriaMock.Setup(x => x.List()).Returns(results.Object);
            _criteriaMock.Setup(x => x.CreateAlias(aliases.First().Value, aliases.First().Key, JoinType.InnerJoin))
                         .Returns(_criteriaMock.Object);

            _target.Search(criterionMock.Object, aliases);
        }

        #endregion

        #region GetCount

        [TestMethod]
        public void TestGetCountReturnsCorrectResultCount()
        {
            var expectedCount = 100;
            var expectedMapCriterion = new Mock<ICriterion>();
            var expectedClonedCriteriaForGetCount = new Mock<ICriteria>();
            var expectedDictionary = new Dictionary<string, string>();
            _searchMapper.Setup(x => x.Map()).Returns(expectedMapCriterion.Object);
            _searchMapper.Setup(x => x.GetAliases()).Returns(expectedDictionary);
            _target.SetCriteriaObj(_criteriaMock.Object);
            _criteriaMock.Setup(x => x.Clone()).Returns(expectedClonedCriteriaForGetCount.Object);
            expectedClonedCriteriaForGetCount.Setup(x => x.SetProjection(It.IsAny<IProjection>()))
                                             .Returns(expectedClonedCriteriaForGetCount.Object);
            _criteriaMock.Setup(x => x.SetFirstResult(It.IsAny<int>())).Returns(_criteriaMock.Object);

            expectedClonedCriteriaForGetCount.Setup(x => x.UniqueResult<int>()).Returns(expectedCount);

            var args = new TestSearchSet();
            args.EnablePaging = true;
            args.Count = 0;
            _target.GetCount(args, _criteriaMock.Object);
            Assert.AreEqual(expectedCount, args.Count);

            expectedClonedCriteriaForGetCount.Verify(x => x.ClearOrders(),
                "ClearOrders must be called when the count is being queried for.");
        }

        #endregion

        #region Search(ISearchSet, ICriteria, Action, int?)

        [TestMethod]
        public void TestSearchWithPagingEnabledQueriesForTotalRecordCount()
        {
            var expectedMapCriterion = new Mock<ICriterion>();
            var expectedClonedCriteriaForGetCount = new Mock<ICriteria>();
            var expectedDictionary = new Dictionary<string, string>();
            _searchMapper.Setup(x => x.Map()).Returns(expectedMapCriterion.Object);
            _searchMapper.Setup(x => x.GetAliases()).Returns(expectedDictionary);
            _target.SetCriteriaObj(_criteriaMock.Object);
            _criteriaMock.Setup(x => x.Clone()).Returns(expectedClonedCriteriaForGetCount.Object);
            expectedClonedCriteriaForGetCount.Setup(x => x.SetProjection(It.IsAny<IProjection>()))
                                             .Returns(expectedClonedCriteriaForGetCount.Object);
            _criteriaMock.Setup(x => x.SetFirstResult(It.IsAny<int>())).Returns(_criteriaMock.Object);

            var expectedCount = 3242;
            expectedClonedCriteriaForGetCount.Setup(x => x.UniqueResult<int>()).Returns(expectedCount);

            var args = new TestSearchSet();
            args.EnablePaging = true;
            args.Count = 0;
            _target.Search(args);
            Assert.AreEqual(expectedCount, args.Count);

            expectedClonedCriteriaForGetCount.Verify(x => x.ClearOrders(),
                "ClearOrders must be called when the count is being queried for.");
        }

        [TestMethod]
        public void TestSearchingWithPagingEnabledSetsPageNumberToOneIfPageNumberIsLessThanOrEqualToZero()
        {
            var expectedMapCriterion = new Mock<ICriterion>();
            var expectedClonedCriteria = new Mock<ICriteria>();
            var expectedDictionary = new Dictionary<string, string>();
            _searchMapper.Setup(x => x.Map()).Returns(expectedMapCriterion.Object);
            _searchMapper.Setup(x => x.GetAliases()).Returns(expectedDictionary);
            _target.SetCriteriaObj(_criteriaMock.Object);
            _criteriaMock.Setup(x => x.Clone()).Returns(expectedClonedCriteria.Object);
            expectedClonedCriteria.Setup(x => x.SetProjection(It.IsAny<IProjection>()))
                                  .Returns(expectedClonedCriteria.Object);
            _criteriaMock.Setup(x => x.SetFirstResult(It.IsAny<int>())).Returns(_criteriaMock.Object);

            var args = new TestSearchSet();
            args.EnablePaging = true;
            args.PageNumber = 0;
            _target.Search(args);
            Assert.AreEqual(1, args.PageNumber);

            args.PageNumber = -1;
            _target.Search(args);
            Assert.AreEqual(1, args.PageNumber);
        }

        [TestMethod]
        public void TestSearchingWithPagingEnabledSetsPageSizeToDefaultValueIfPageSizeIsLessThanOrEqualToZero()
        {
            var expectedMapCriterion = new Mock<ICriterion>();
            var expectedClonedCriteria = new Mock<ICriteria>();
            var expectedDictionary = new Dictionary<string, string>();
            _searchMapper.Setup(x => x.Map()).Returns(expectedMapCriterion.Object);
            _searchMapper.Setup(x => x.GetAliases()).Returns(expectedDictionary);
            _target.SetCriteriaObj(_criteriaMock.Object);
            _criteriaMock.Setup(x => x.Clone()).Returns(expectedClonedCriteria.Object);
            expectedClonedCriteria.Setup(x => x.SetProjection(It.IsAny<IProjection>()))
                                  .Returns(expectedClonedCriteria.Object);
            _criteriaMock.Setup(x => x.SetFirstResult(It.IsAny<int>())).Returns(_criteriaMock.Object);

            var args = new TestSearchSet();
            args.EnablePaging = true;
            args.PageSize = 0;
            _target.Search(args);
            Assert.AreEqual(RepositoryBase<TestUser>.DEFAULT_PAGE_SIZE, args.PageSize);
            args.PageSize = -1;
            _target.Search(args);
            Assert.AreEqual(RepositoryBase<TestUser>.DEFAULT_PAGE_SIZE, args.PageSize);
            args.PageSize = 1;
            _target.Search(args);
            Assert.AreEqual(1, args.PageSize);
        }

        [TestMethod]
        public void TestSearchingWithPagingEnabledSetsTheActualPagingStuffOnICriteria()
        {
            var expectedMapCriterion = new Mock<ICriterion>();
            var expectedClonedCriteria = new Mock<ICriteria>();
            var expectedDictionary = new Dictionary<string, string>();
            _searchMapper.Setup(x => x.Map()).Returns(expectedMapCriterion.Object);
            _searchMapper.Setup(x => x.GetAliases()).Returns(expectedDictionary);
            _target.SetCriteriaObj(_criteriaMock.Object);
            _criteriaMock.Setup(x => x.Clone()).Returns(expectedClonedCriteria.Object);
            expectedClonedCriteria.Setup(x => x.SetProjection(It.IsAny<IProjection>()))
                                  .Returns(expectedClonedCriteria.Object);
            _criteriaMock.Setup(x => x.SetFirstResult(It.IsAny<int>())).Returns(_criteriaMock.Object);
            var expectedCount = 500;
            expectedClonedCriteria.Setup(x => x.UniqueResult<int>()).Returns(expectedCount);

            var args = new TestSearchSet();
            args.EnablePaging = true;
            args.PageNumber = 1;
            args.PageSize = 25;
            _target.Search(args);
            _criteriaMock.Verify(x => x.SetFirstResult(0));
            _criteriaMock.Verify(x => x.SetMaxResults(25));

            args.PageNumber = 2;
            _target.Search(args);
            _criteriaMock.Verify(x => x.SetFirstResult(25));
            _criteriaMock.Verify(x => x.SetMaxResults(25));
        }

        [TestMethod]
        public void TestSearchWithPagingDisabledSetsTheCountFromTheResultsCount()
        {
            var expectedMapCriterion = new Mock<ICriterion>();
            var expectedDictionary = new Dictionary<string, string>();
            _searchMapper.Setup(x => x.Map()).Returns(expectedMapCriterion.Object);
            _searchMapper.Setup(x => x.GetAliases()).Returns(expectedDictionary);
            _target.SetCriteriaObj(_criteriaMock.Object);
            _criteriaMock.Setup(x => x.List(It.IsAny<IList>())).Callback((IList list) => {
                list.Add(new TestUser());
                list.Add(new TestUser());
                list.Add(new TestUser());
            });

            var args = new TestSearchSet();
            args.EnablePaging = false;
            _target.Search(args);
            Assert.AreEqual(3, args.Count);
        }

        [TestMethod]
        public void TestSearchWithPagingDisabledSetsPageNumberAndPageCountToOne()
        {
            var expectedMapCriterion = new Mock<ICriterion>();
            var expectedDictionary = new Dictionary<string, string>();
            _searchMapper.Setup(x => x.Map()).Returns(expectedMapCriterion.Object);
            _searchMapper.Setup(x => x.GetAliases()).Returns(expectedDictionary);
            _target.SetCriteriaObj(_criteriaMock.Object);

            var args = new TestSearchSet();
            args.EnablePaging = false;
            args.PageNumber = 32;
            args.PageCount = 32;
            _target.Search(args);
            Assert.AreEqual(1, args.PageNumber);
            Assert.AreEqual(1, args.PageCount);
        }

        [TestMethod]
        public void TestSearchCallsActionCallbackIfOneIsSet()
        {
            var expectedMapCriterion = new Mock<ICriterion>();
            var expectedDictionary = new Dictionary<string, string>();
            _searchMapper.Setup(x => x.Map()).Returns(expectedMapCriterion.Object);
            _searchMapper.Setup(x => x.GetAliases()).Returns(expectedDictionary);
            _target.SetCriteriaObj(_criteriaMock.Object);

            var args = new TestSearchSet();
            args.EnablePaging = false;
            var called = false;
            Action<ISearchMapper> expectedAction = (map) => { called = true; };
            _target.Search(args, (ICriteria)null, expectedAction);
            Assert.IsTrue(called);
        }

        [TestMethod]
        public void TestSearchAddsAliasesFromMapper()
        {
            var expectedMapCriterion = new Mock<ICriterion>();
            var expectedDictionary = new Dictionary<string, string>();
            expectedDictionary.Add("neat", "okay");
            _searchMapper.Setup(x => x.Map()).Returns(expectedMapCriterion.Object);
            _searchMapper.Setup(x => x.GetAliases()).Returns(expectedDictionary);
            _target.SetCriteriaObj(_criteriaMock.Object);

            var args = new TestSearchSet();
            args.EnablePaging = false;
            _target.Search(args);

            _criteriaMock.Verify(x => x.CreateAlias("okay", "neat", JoinType.LeftOuterJoin));
        }

        [TestMethod]
        public void TestSearchReturnsSearchSetResultsCollection()
        {
            var expectedMapCriterion = new Mock<ICriterion>();
            var expectedDictionary = new Dictionary<string, string>();
            var mockIQueryOver = new Mock<IQueryOver>();
            mockIQueryOver.Setup(x => x.UnderlyingCriteria).Returns(_criteriaMock.Object);
            _searchMapper.Setup(x => x.Map()).Returns(expectedMapCriterion.Object);
            _searchMapper.Setup(x => x.GetAliases()).Returns(expectedDictionary);
            _target.SetCriteriaObj(_criteriaMock.Object);

            _criteriaMock.Setup(x => x.List(It.IsAny<IList>())).Callback((IList list) => { list.Add(new TestUser()); });
            var args = new TestSearchSet();
            var expected = args.Results; // The Results collection instance should never change.
            args.EnablePaging = false;

            Assert.AreSame(expected, _target.Search(args));
            Assert.AreSame(expected, _target.Search(args, _criteriaMock.Object));
            Assert.AreSame(expected, _target.Search(args, mockIQueryOver.Object));
        }

        [TestMethod]
        public void TestSearchWillNotExceedMaxResultsAndInsteadReturnsNull()
        {
            var clonedCriteria = new Mock<ICriteria>();
            var expectedMapCriterion = new Mock<ICriterion>();
            var expectedDictionary = new Dictionary<string, string>();
            _searchMapper.Setup(x => x.Map()).Returns(expectedMapCriterion.Object);
            _searchMapper.Setup(x => x.GetAliases()).Returns(expectedDictionary);
            _criteriaMock.Setup(x => x.Clone()).Returns(clonedCriteria.Object);
            clonedCriteria.Setup(x => x.SetProjection(It.IsAny<CountProjection>())).Returns(clonedCriteria.Object);
            clonedCriteria.Setup(x => x.UniqueResult<int>()).Returns(2);
            _target.SetCriteriaObj(_criteriaMock.Object);

            var args = new TestSearchSet();
            var expected = args.Results; // The Results collection instance should never change.
            args.EnablePaging = false;

            var result = _target.Search(args, null, maxResults: 1);

            Assert.IsNull(result);
            Assert.AreEqual(2, args.Count);
            // criteria.List(IList) is where the query would actually execute, so if that never happens the
            // query has never actually executed.
            _criteriaMock.Verify(x => x.List(It.IsAny<IList>()), Times.Never);
        }

        [TestMethod]
        public void TestSearchWillStillReturnResultsIfMaxResultsNotExceeded()
        {
            var clonedCriteria = new Mock<ICriteria>();
            var expectedMapCriterion = new Mock<ICriterion>();
            var expectedDictionary = new Dictionary<string, string>();
            _searchMapper.Setup(x => x.Map()).Returns(expectedMapCriterion.Object);
            _searchMapper.Setup(x => x.GetAliases()).Returns(expectedDictionary);
            _criteriaMock.Setup(x => x.Clone()).Returns(clonedCriteria.Object);
            clonedCriteria.Setup(x => x.SetProjection(It.IsAny<CountProjection>())).Returns(clonedCriteria.Object);
            clonedCriteria.Setup(x => x.UniqueResult<int>()).Returns(1);
            _target.SetCriteriaObj(_criteriaMock.Object);

            var args = new TestSearchSet();
            var expected = args.Results; // The Results collection instance should never change.
            args.EnablePaging = false;

            _target.Search(args, null, maxResults: 1);

            _criteriaMock.Verify(x => x.List(It.IsAny<IList>()), Times.Once);
        }

        [TestMethod]
        public void TestSearchSorting()
        {
            Assert.Inconclusive("TODO");
        }

        #endregion

        #region GetCountForCriteria(ICriteria)

        [TestMethod]
        public void TestGetCountForCriteriaReturnsCountForCriteria()
        {
            var count = 108;
            IProjection[] callbackProjection = null;

            // This would be an actual clone in practice, but for the sake of this
            // test, and that the criteria is not used elsewhere, we can just do this).
            _criteriaMock
               .Setup(x => x.Clone())
               .Returns(_criteriaMock.Object);
            _criteriaMock
               .Setup(x => x.SetProjection(It.IsAny<IProjection>()))
               .Returns(_criteriaMock.Object)
               .Callback<IProjection[]>(x => callbackProjection = x);
            _criteriaMock
               .Setup(x => x.UniqueResult<int>())
               .Returns(count);

            Assert.AreEqual(count, _target.GetCountForCriteria(_criteriaMock.Object));
            Assert.IsNotNull(callbackProjection);
            Assert.AreEqual(1, callbackProjection.Count(), "There should only be one projection being added.");
            Assert.IsInstanceOfType(callbackProjection.Single(), typeof(RowCountProjection));
        }

        #endregion

        #region GetCountForCriterion(ICriterion)

        [TestMethod]
        public void TestGetCountForCriterionReturnsCorrectCount()
        {
            var count = 100;
            var criterionMock = new Mock<ICriterion>();
            IProjection[] callbackProjection = null;

            _target.SetCriteriaObj(_criteriaMock.Object);

            // This would be an actual clone in practice, but for the sake of this
            // test, and that the criteria is not used elsewhere, we can just do this).
            _criteriaMock
               .Setup(x => x.Clone())
               .Returns(_criteriaMock.Object);
            _criteriaMock
               .Setup(x => x.Add(criterionMock.Object))
               .Returns(_criteriaMock.Object);
            _criteriaMock
               .Setup(x => x.SetProjection(It.IsAny<IProjection>()))
               .Returns(_criteriaMock.Object)
               .Callback<IProjection[]>(x => callbackProjection = x);
            _criteriaMock
               .Setup(x => x.UniqueResult<int>())
               .Returns(count);

            Assert.AreEqual(count,
                _target.GetCountForCriterion(criterionMock.Object));
            Assert.IsNotNull(callbackProjection);
            Assert.AreEqual(1, callbackProjection.Count(), "There should only be one projection being added.");
            Assert.IsInstanceOfType(callbackProjection.Single(), typeof(RowCountProjection));
        }

        [TestMethod]
        public void TestGetCountForCriterionAddsAliasesToCriteria()
        {
            var count = 808;
            var criterionMock = new Mock<ICriterion>();
            var aliases = new Dictionary<string, string> {{"k", "val"}};
            IProjection[] callbackProjection = null;

            _target.SetCriteriaObj(_criteriaMock.Object);

            // This would be an actual clone in practice, but for the sake of this
            // test, and that the criteria is not used elsewhere, we can just do this).
            _criteriaMock.Setup(x => x.Clone())
                         .Returns(_criteriaMock.Object);
            _criteriaMock.Setup(x => x.Add(criterionMock.Object))
                         .Returns(_criteriaMock.Object);
            _criteriaMock.Setup(x => x.CreateAlias(aliases.First().Value, aliases.First().Key, JoinType.InnerJoin))
                         .Returns(_criteriaMock.Object);
            _criteriaMock.Setup(x => x.SetProjection(It.IsAny<IProjection>()))
                         .Returns(_criteriaMock.Object)
                         .Callback<IProjection[]>(x => callbackProjection = x);
            _criteriaMock.Setup(x => x.UniqueResult<int>())
                         .Returns(count);

            Assert.AreEqual(count, _target.GetCountForCriterion(criterionMock.Object, aliases));
            Assert.IsNotNull(callbackProjection);
            Assert.AreEqual(1, callbackProjection.Count(), "There should only be one projection being added.");
            Assert.IsInstanceOfType(callbackProjection.Single(), typeof(RowCountProjection));
        }

        [TestMethod]
        public void TestGetCountForCriterionAppliesAdditionalCriterion()
        {
            var count = 100;
            var criterionMock = new Mock<ICriterion>();
            var addCriterionMock = new Mock<ICriterion>();
            IProjection[] callbackProjection = null;

            _target.SetCriteriaObj(_criteriaMock.Object);

            _criteriaMock
               .Setup(x => x.Clone())
               .Returns(_criteriaMock.Object);
            _criteriaMock
               .Setup(x => x.Add(criterionMock.Object))
               .Returns(_criteriaMock.Object);
            _criteriaMock
               .Setup(x => x.Add(addCriterionMock.Object))
               .Returns(_criteriaMock.Object)
               .Verifiable();
            _criteriaMock
               .Setup(x => x.SetProjection(It.IsAny<IProjection>()))
               .Returns(_criteriaMock.Object)
               .Callback<IProjection[]>(x => callbackProjection = x);
            _criteriaMock
               .Setup(x => x.UniqueResult<int>())
               .Returns(count);

            Assert.AreEqual(count, _target.GetCountForCriterion(criterionMock.Object, null, addCriterionMock.Object));

            _criteriaMock.Verify(x => x.Add(addCriterionMock.Object));
        }

        #endregion

        #region Delete(TEntity)

        [TestMethod]
        public void TestDeleteDeletesItemAndFlushesSession()
        {
            var obj = new TestUser();
            _target.Delete(obj);
            _sessionMock.Verify(x => x.Delete(obj));
            _sessionMock.Verify(x => x.Flush());
        }

        #endregion

        #region GetIdentifier(TEntity)

        [TestMethod]
        public void TestGetIdentifierGetsIdentifierFromClassMetadata()
        {
            var entity = new TestUser();
            var expectedId = 12;
            _sessionFactoryMock.Setup(x => x.GetClassMetadata(typeof(TestUser))).Returns(_classMetadataMock.Object);
            _classMetadataMock.Setup(x => x.GetIdentifier(entity)).Returns(expectedId);

            var result = _target.GetIdentifier(entity);
            Assert.AreEqual(expectedId, result);
            _classMetadataMock.Verify(x => x.GetIdentifier(entity));
        }

        #endregion

        #region BuildPaginatedQuery(ICriterion filter, int pageIndex, int pageSize, string sort = null, bool sortAsc = true)

        [TestMethod]
        public void TestBuildPaginatedQueryReturnsAllRecordsThatPassTheGivenFilterWithinTheSpecifiedPage()
        {
            ICriterion filter = Restrictions.IsNotNull("Id");
            var page = 3;
            var pageSize = 10;
            var items = new List<TestUser>();
            var criteria = new Mock<ICriteria>();

            _sessionMock.Setup(x => x.CreateCriteria<TestUser>(typeof(TestUser).Name.ToLower()))
                        .Returns(criteria.Object);
            criteria.Setup(x => x.Add(filter)).Returns(criteria.Object);
            criteria.Setup(x => x.SetFirstResult(page * pageSize)).Returns(criteria.Object);
            criteria.Setup(x => x.SetMaxResults(pageSize)).Returns(criteria.Object);
            criteria.Setup(x => x.List<TestUser>()).Returns(items);

            Assert.AreSame(items, _target.BuildPaginatedQuery(page, pageSize, filter));
        }

        [TestMethod]
        public void TestBuildPaginatedQueryReturnsAllRecordsThatPassTheGivenFilterWithinTheSpecifiedPageWithICriteria()
        {
            var page = 3;
            var pageSize = 10;
            var items = new List<TestUser>();
            var criteria = new Mock<ICriteria>();

            criteria.Setup(x => x.SetFirstResult(page * pageSize)).Returns(criteria.Object);
            criteria.Setup(x => x.SetMaxResults(pageSize)).Returns(criteria.Object);
            criteria.Setup(x => x.List<TestUser>()).Returns(items);

            Assert.AreSame(items, _target.BuildPaginatedQuery(page, pageSize, criteria.Object, null, true));
        }

        [TestMethod]
        public void TestBuildPaginatedQueryThrowsExceptionWhenSortIsTooDeep()
        {
            ICriterion filter = Restrictions.IsNotNull("Id");
            var page = 3;
            var pageSize = 10;
            var items = new List<TestUser>();
            var criteria = new Mock<ICriteria>();

            _sessionMock.Setup(x => x.CreateCriteria<TestUser>(typeof(TestUser).Name.ToLower()))
                        .Returns(criteria.Object);
            criteria.Setup(x => x.Add(filter)).Returns(criteria.Object);

            MyAssert.Throws<DomainLogicException>(() =>
                _target.BuildPaginatedQuery(page, pageSize, filter, "Way.Too.Many"));
        }

        [TestMethod]
        public void TestBuildPaginatedQueryThrowsExceptionWhenSortIsTooDeepWithICriteria()
        {
            var page = 3;
            var pageSize = 10;
            var items = new List<TestUser>();
            var criteria = new Mock<ICriteria>();

            MyAssert.Throws<DomainLogicException>(() =>
                _target.BuildPaginatedQuery(page, pageSize, criteria.Object, "Way.Too.Many", true));
        }

        [TestMethod]
        public void TestBuildPaginatedQueryAddsCriteriaForAssociationAndSortsAsc()
        {
            var sort = "MainGroup.Name";
            var order = Order.Asc(Projections.Property(sort));
            ICriterion filter = Restrictions.IsNotNull("Id");
            var page = 3;
            var pageSize = 10;
            var items = new List<TestUser>();
            var criteria = new Mock<ICriteria>();

            _sessionMock.Setup(x => x.CreateCriteria<TestUser>(typeof(TestUser).Name.ToLower()))
                        .Returns(criteria.Object);
            criteria.Setup(x => x.Add(filter)).Returns(criteria.Object);
            criteria.Setup(x => x.SetFirstResult(page * pageSize)).Returns(criteria.Object);
            criteria.Setup(x => x.SetMaxResults(pageSize)).Returns(criteria.Object);
            criteria.Setup(x => x.List<TestUser>()).Returns(items);
            criteria.Setup(x => x.CreateCriteria("MainGroup", "MainGroup", JoinType.LeftOuterJoin))
                    .Returns(criteria.Object);
            criteria.Setup(x => x.AddOrder(order)).Returns(criteria.Object);
            Assert.AreSame(items, _target.BuildPaginatedQuery(page, pageSize, filter, sort));
        }

        [TestMethod]
        public void TestBuildPaginatedQueryAddsCriteriaForAssociationAndSortsAscWithCriteria()
        {
            var sort = "MainGroup.Name";
            var order = Order.Asc(Projections.Property(sort));
            var page = 3;
            var pageSize = 10;
            var items = new List<TestUser>();
            var criteria = new Mock<ICriteria>();

            criteria.Setup(x => x.SetFirstResult(page * pageSize)).Returns(criteria.Object);
            criteria.Setup(x => x.SetMaxResults(pageSize)).Returns(criteria.Object);
            criteria.Setup(x => x.List<TestUser>()).Returns(items);
            criteria.Setup(x => x.CreateCriteria("MainGroup", "MainGroup", JoinType.LeftOuterJoin))
                    .Returns(criteria.Object);
            criteria.Setup(x => x.AddOrder(order)).Returns(criteria.Object);
            Assert.AreSame(items, _target.BuildPaginatedQuery(page, pageSize, criteria.Object, sort, true));
        }

        [TestMethod]
        public void TestBuildPaginatedQueryAddsCriteriaForAssociationAndSortsDesc()
        {
            var sort = "MainGroup.Name";
            var order = Order.Desc(Projections.Property(sort));
            ICriterion filter = Restrictions.IsNotNull("Id");
            var page = 3;
            var pageSize = 10;
            var items = new List<TestUser>();
            var criteria = new Mock<ICriteria>();

            _sessionMock.Setup(x => x.CreateCriteria<TestUser>(typeof(TestUser).Name.ToLower()))
                        .Returns(criteria.Object);
            criteria.Setup(x => x.Add(filter)).Returns(criteria.Object);
            criteria.Setup(x => x.SetFirstResult(page * pageSize)).Returns(criteria.Object);
            criteria.Setup(x => x.SetMaxResults(pageSize)).Returns(criteria.Object);
            criteria.Setup(x => x.List<TestUser>()).Returns(items);
            criteria.Setup(x => x.CreateCriteria("MainGroup", "MainGroup", JoinType.LeftOuterJoin))
                    .Returns(criteria.Object);
            criteria.Setup(x => x.AddOrder(order)).Returns(criteria.Object);

            Assert.AreSame(items, _target.BuildPaginatedQuery(page, pageSize, filter, sort, false));
        }

        public void TestBuildPaginatedQueryAddsCriteriaForAssociationAndSortsDescWithICriteria()
        {
            var sort = "MainGroup.Name";
            var order = Order.Desc(Projections.Property(sort));
            var page = 3;
            var pageSize = 10;
            var items = new List<TestUser>();
            var criteria = new Mock<ICriteria>();

            criteria.Setup(x => x.SetFirstResult(page * pageSize)).Returns(criteria.Object);
            criteria.Setup(x => x.SetMaxResults(pageSize)).Returns(criteria.Object);
            criteria.Setup(x => x.List<TestUser>()).Returns(items);
            criteria.Setup(x => x.CreateCriteria("MainGroup", "MainGroup", JoinType.LeftOuterJoin))
                    .Returns(criteria.Object);
            criteria.Setup(x => x.AddOrder(order)).Returns(criteria.Object);

            Assert.AreSame(items, _target.BuildPaginatedQuery(page, pageSize, criteria.Object, sort, false));
        }

        [TestMethod]
        public void TestBuildPaginatedQueryDoesNotAddCriteriaForAliasThatAlreadyExists()
        {
            var iSessionImplementor = new Mock<ISessionImplementor>();
            var criteria = new CriteriaImpl("TestUser", iSessionImplementor.Object);
            criteria.CreateAlias("MainGroup", "MainGroup");
            var sort = "MainGroup.Name";
            var page = 3;
            var pageSize = 10;

            MyAssert.DoesNotThrow(() => _target.BuildPaginatedQuery(page, pageSize, criteria, sort, false));
        }

        #endregion

        #region Test classes

        private class TestSearchSet : SearchSet<TestUser> { }

        #endregion
    }

    public class TestRepository : RepositoryBase<TestUser>
    {
        #region Private Members

        private ICriteria _criteriaObj;
        private AbstractCriterion _idMatchesCriterion;
        private IQueryable<TestUser> _linq;

        #endregion

        #region Properties

        public ISearchMapper TestSearchMapper { get; set; }

        public override ICriteria Criteria => _criteriaObj ?? base.Criteria;

        public override IQueryable<TestUser> Linq => _linq ?? base.Linq;

        #endregion

        #region Constructors

        public TestRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion

        #region Exposed Methods

        public IQueryable<Object> GetLinqProperty()
        {
            return Linq;
        }

        public void SetLinqProperty(IQueryable<TestUser> linq)
        {
            _linq = linq;
        }

        public ICriteria GetCriteriaObj()
        {
            return Criteria;
        }

        public void SetCriteriaObj(ICriteria criteria)
        {
            _criteriaObj = criteria;
        }

        public void SetIdMatchesCriterion(AbstractCriterion criterion)
        {
            _idMatchesCriterion = criterion;
        }

        #endregion

        #region Private Methods

        protected override AbstractCriterion GetIdEqCriterion(int id)
        {
            return _idMatchesCriterion ?? base.GetIdEqCriterion(id);
        }

        protected override ISearchMapper CreateSearchMapper(ISearchSet args, Type entityType, ISession session)
        {
            return TestSearchMapper;
        }

        #endregion
    }

    public class ADifferentModelOfTestUser
    {
        public string Email { get; set; }
    }
}
