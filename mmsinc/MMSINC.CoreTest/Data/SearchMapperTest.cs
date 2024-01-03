using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MMSINC.CoreTest.Data
{
    [TestClass]
    public class SearchMapperTest : InMemoryDatabaseTest<TestUser, TestUserRepository>
    {
        #region Private Members

        // these aren't meant to be used, they're just so that the necessary
        // they come from get copied over.  if you have tests that pass in
        // visual studio but fail in TC and from the command line, you might
        // need these in your project also.
#pragma warning disable 169
        private System.Data.SQLite.SQLiteException _doNotUseThisException;
#pragma warning restore 169

        #endregion

        #region Constructor

        [TestMethod]
        public void TestConstructorCallsModifyValuesOnSearchSet()
        {
            var args = new TestSearchSet();
            var wasCalled = false;
            args._modifyValuesCallback = () => { wasCalled = true; };

            Repository.Search(args);
            Assert.IsTrue(wasCalled);
        }

        #endregion

        #region Map

        [TestMethod]
        public void TestMapMethodUsesOverriddenValuesFromMappableProperties()
        {
            var userOne = new TestUser {Email = "user@one.com"};
            Repository.Save(userOne);
            var userTwo = new TestUser {Email = "user@two.com"};
            Repository.Save(userTwo);

            var args = new TestSearchSet {
                Email = userOne.Email,
            };

            ISearchMapper mapperInstance = null;
            var result = Repository.Search(args, (ICriteria)null, (mapper) => {
                mapper.MappedProperties["Email"].Value = userTwo.Email;
                mapperInstance = mapper;
            }).ToArray();

            Assert.IsNotNull(mapperInstance, "If this is null then the callback wasn't called.");
            Assert.IsFalse(result.Contains(userOne));
            Assert.IsTrue(result.Contains(userTwo));
            Assert.AreEqual(userTwo.Email, mapperInstance.MappedProperties["Email"].Value);
        }

        [TestMethod]
        public void TestEntityIdPropertyIsSearchedForAsId()
        {
            var userOne = new TestUser {Email = "user@one.com"};
            Repository.Save(userOne);
            var userTwo = new TestUser {Email = "user@two.com"};
            Repository.Save(userTwo);

            var args = new TestEntityIdToIdSearchSet {
                EntityId = userTwo.Id
            };

            var result = Repository.Search(args).ToArray();
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains(userTwo));
        }

        [TestMethod]
        public void TestGetsSearchAliasAttributesFromInterfaces()
        {
            var group = new TestGroup();
            Session.Save(group);
            var userOne = new TestUser {Email = "user@one.com", MainGroup = group};
            Repository.Save(userOne);
            var userTwo = new TestUser {Email = "user@two.com"};
            Repository.Save(userTwo);

            var callbackCalled = false;
            var args = new TestInterfaceSearch {
                MainGroup = group.Id,
                Callback = (mapper) => {
                    callbackCalled = true;
                    Assert.IsTrue(mapper.MappedProperties["MainGroup"].IsAliased, "SearchAlias was not found.");
                }
            };
            var result = Repository.Search(args).ToArray();
            Assert.IsTrue(callbackCalled);
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains(userOne));
        }

        [TestMethod]
        public void TestMappingGetsSearchAliasAttributeFromConcreteImplementationBeforeItGetsItFromInterfaces()
        {
            var group = new TestGroup();
            Session.Save(group);
            var userOne = new TestUser {Email = "user@one.com", MainGroup = group};
            Repository.Save(userOne);
            var userTwo = new TestUser {Email = "user@two.com"};
            Repository.Save(userTwo);

            var callbackCalled = false;
            var args = new TestInterfaceSearch {
                MainGroup = group.Id,
                Callback = (mapper) => {
                    callbackCalled = true;
                    Assert.IsTrue(mapper.MappedProperties["PropertyWithOverrideSearchAlias"].IsAliased,
                        "SearchAlias was not found.");
                    Assert.AreEqual("MainGroupAlias",
                        mapper.MappedProperties["PropertyWithOverrideSearchAlias"].SearchAlias.Alias);
                }
            };
            var result = Repository.Search(args).ToArray();
            Assert.IsTrue(callbackCalled);
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains(userOne));
        }

        #endregion

        #region Simple Property Searches

        [TestMethod]
        public void TestShouldGetByIdIfRequested()
        {
            var users = new[] {
                new TestUser {Email = "TestUser1@site.com"},
                new TestUser {Email = "TestUser2@site.com"},
                new TestUser {Email = "TestUser3@site.com"}
            };

            Repository.Save(users);

            var searchUser = new TestSearchSet();
            searchUser.EnablePaging = false;
            searchUser.Id = users[0].Id;

            Assert.AreSame(users[0], Repository.Search(searchUser).Single());
        }

        [TestMethod]
        public void TestShouldGetByNameIfRequested()
        {
            var users = new[] {
                new TestUser {Email = "SameGuy@site.com"},
                new TestUser {Email = "SameGuy@site.com"},
                new TestUser {Email = "DifferentGuy@site.com"}
            };

            Repository.Save(users);

            var args = new TestSearchSet {
                Email = users[0].Email
            };

            Repository.Search(args);

            var results = args.Results.ToArray();
            Assert.AreEqual(2, results.Count());
            Assert.IsTrue(results.Contains(users[0]));
            Assert.IsTrue(results.Contains(users[1]));
        }

        [TestMethod]
        public void TestShouldGetByForeignIdIfRequested()
        {
            var users = new[] {
                new TestUser {SomeForeignId = 1},
                new TestUser {SomeForeignId = 2},
                new TestUser {SomeForeignId = 2}
            };

            Repository.Save(users);

            var args = new TestSearchSet {
                SomeForeignId = 2
            };

            var results = Repository.Search(args).ToArray();

            Assert.AreEqual(2, results.Count());
            Assert.IsTrue(results.Contains(users[1]));
            Assert.IsTrue(results.Contains(users[2]));
        }

        [TestMethod]
        public void TestShouldGetByNameAndForeignIdIfRequested()
        {
            var users = new[] {
                new TestUser {Email = "SomeGuy@site.com", SomeForeignId = 1},
                new TestUser {Email = "SomeGuy@site.com", SomeForeignId = 2},
                new TestUser {Email = "DifferentGuy@site.com", SomeForeignId = 2}
            };

            Repository.Save(users);

            var searchUser = new TestSearchSet {
                Email = users[1].Email,
                SomeForeignId = users[1].SomeForeignId
            };

            var actual = Repository.Search(searchUser).ToArray();

            Assert.AreEqual(1, actual.Count());
            Assert.IsTrue(actual.Contains(users[1]));
        }

        [TestMethod]
        public void TestShouldGetByPartOfName()
        {
            var users = new[] {
                new TestUser {Email = "SomeGuy@site.com", SomeForeignId = 1},
                new TestUser {Email = "SomeGuy@site.com", SomeForeignId = 2},
                new TestUser {Email = "DifferentGuy@site.com", SomeForeignId = 2}
            };

            Repository.Save(users);

            var args = new TestSearchSet {
                Email = "omeguy"
            };

            var actual = Repository.Search(args).ToArray();
            Assert.AreEqual(2, actual.Count());
            Assert.IsTrue(actual.Contains(users[0]));
            Assert.IsTrue(actual.Contains(users[1]));
        }

        [TestMethod]
        public void
            TestMappedPropertiesOnlyIncludeThePropertiesDeclaredOnTheModelAndNotAnyInheritedPropertiesFromSearchSet()
        {
            var args = new TestSearchSet();
            var result = new SearchMapper(args, typeof(TestUser), Session).MappedProperties;
            Action<string> AssertHasProperty = (name) => { Assert.IsTrue(result.ContainsKey(name)); };

            Action<string> DoesNotHaveProperty = (name) => { Assert.IsFalse(result.ContainsKey(name)); };

            Assert.AreEqual(12, result.Count);
            AssertHasProperty("Id");
            AssertHasProperty("Email");
            AssertHasProperty("MainGroupId");
            AssertHasProperty("SomeForeignId");
            AssertHasProperty("SomeOrder");
            AssertHasProperty("OtherOrder");
            AssertHasProperty("MainGroup");
            AssertHasProperty("OtherGroup");
            AssertHasProperty("SearchString");
            AssertHasProperty("Criterion");
            AssertHasProperty("Unmappable");
            AssertHasProperty("IsAdmin");

            DoesNotHaveProperty("PageNumber");
            DoesNotHaveProperty("PageSize");
            DoesNotHaveProperty("PageCount");
            DoesNotHaveProperty("Count");
            DoesNotHaveProperty("EnablePaging");
            DoesNotHaveProperty("SortBy");
            DoesNotHaveProperty("SortAscending");
            DoesNotHaveProperty("Results");
        }

        [TestMethod]
        public void TestMappedPropertiesDoesNotIncludeISearchSetProperties()
        {
            var args = new ISearchSetImplementation();
            var result = new SearchMapper(args, typeof(TestUser), Session).MappedProperties;
            Action<string> AssertHasProperty = (name) => { Assert.IsTrue(result.ContainsKey(name)); };

            Action<string> DoesNotHaveProperty = (name) => { Assert.IsFalse(result.ContainsKey(name)); };

            Assert.AreEqual(1, result.Count);
            AssertHasProperty("Id");

            DoesNotHaveProperty("PageNumber");
            DoesNotHaveProperty("PageSize");
            DoesNotHaveProperty("PageCount");
            DoesNotHaveProperty("Count");
            DoesNotHaveProperty("EnablePaging");
            DoesNotHaveProperty("SortBy");
            DoesNotHaveProperty("SortAscending");
            DoesNotHaveProperty("Results");
        }

        #endregion

        #region Wildcard String Searches

        [TestMethod]
        public void TestShouldGetItemsByWildcardsWhenWildcardsExist()
        {
            var user1 = new TestUser {Email = "SomeGuy@site.com"};
            var user2 = new TestUser {Email = "DifferentGuy@site.com"};
            Repository.Save(user1);
            Repository.Save(user2);

            var args = new TestSearchSet {Email = "*site.com"};

            var result = Repository.Search(args).ToArray();

            Assert.IsTrue(result.Contains(user1));
            Assert.IsTrue(result.Contains(user2));

            args.Email = "SomeGuy*";

            result = Repository.Search(args).ToArray();

            Assert.IsTrue(result.Contains(user1));
            Assert.IsFalse(result.Contains(user2));
        }

        [TestMethod]
        public void TestShouldGetExactMatchWhenSearchStringValueIsUsedWithExactMatchValue()
        {
            var user1 = new TestUser {SearchString = "some value"};
            Repository.Save(user1);

            var args = new TestSearchSet {
                SearchString = new SearchString {
                    Value = "some",
                    MatchType = SearchStringMatchType.Exact
                }
            };

            var result = Repository.Search(args).ToArray();
            Assert.IsFalse(result.Contains(user1));

            args.SearchString.Value = "some value";

            result = Repository.Search(args).ToArray();

            Assert.IsTrue(result.Contains(user1));
        }

        [TestMethod]
        public void TestShouldGetWildcardMatchWhenSearchStringValueIsUsedWithWildcardMatchValue()
        {
            var user1 = new TestUser {SearchString = "some value"};
            Repository.Save(user1);

            var args = new TestSearchSet {
                SearchString = new SearchString {
                    Value = "some",
                    MatchType = SearchStringMatchType.Wildcard
                }
            };

            var result = Repository.Search(args).ToArray();
            Assert.IsTrue(result.Contains(user1));

            args.SearchString.Value = "some*";

            result = Repository.Search(args).ToArray();
            Assert.IsTrue(result.Contains(user1));
        }

        [TestMethod]
        public void TestShouldNOTGetWildcardOrExactMatchForSearchStringIfSearchValueIsNullorEmpty()
        {
            var user1 = new TestUser {SearchString = "some value"};
            var user2 = new TestUser {SearchString = "other"};

            Repository.Save(user1);
            Repository.Save(user2);

            var args = new TestSearchSet {
                SearchString = new SearchString {
                    Value = null,
                    MatchType = SearchStringMatchType.Wildcard
                }
            };

            var result = Repository.Search(args).ToArray();

            // Both users should be returned since there was no search value.
            Assert.IsTrue(result.Contains(user1));
            Assert.IsTrue(result.Contains(user2));

            args.SearchString.Value = string.Empty;

            result = Repository.Search(args).ToArray();

            // Both users should be returned
            Assert.IsTrue(result.Contains(user1));
            Assert.IsTrue(result.Contains(user2));
        }

        #endregion

        #region Null Values

        [TestMethod]
        public void TestMapMethodCorrectlyHandlesNullValueSearchingWhenASearchActuallyRequiresANullValue()
        {
            var userOne = new TestUser {Email = null};
            Repository.Save(userOne);
            var userTwo = new TestUser {Email = "user@two.com"};
            Repository.Save(userTwo);

            var args = new TestSearchSet();
            var result = Repository.Search(args, (ICriteria)null,
                (mapper) => { mapper.MappedProperties["Email"].Value = SearchMapperSpecialValues.IsNull; }).ToArray();

            Assert.IsTrue(result.Contains(userOne));
            Assert.IsFalse(result.Contains(userTwo));
        }

        #endregion

        #region IsNotNull

        [TestMethod]
        public void
            TestMapMethodCorrectlyHandlesIsNotNullValueSearchingWhenASearchActuallyRequiresAnythingButANullValue()
        {
            var userOne = new TestUser {Email = null};
            Repository.Save(userOne);
            var userTwo = new TestUser {Email = "user@two.com"};
            Repository.Save(userTwo);

            var args = new TestSearchSet();
            var result = Repository.Search(args, (ICriteria)null,
                (mapper) => { mapper.MappedProperties["Email"].Value = SearchMapperSpecialValues.IsNotNull; });

            Assert.IsFalse(result.Contains(userOne));
            Assert.IsTrue(result.Contains(userTwo));
        }

        #endregion

        #region IsNotNullOrEmpty

        [TestMethod]
        public void
            TestMapMethodCorrectlyHandlesIsNotNullOrEmptyValueSearchingWhenASearchActuallyRequestAnythingButANullOrEmptyValue()
        {
            var userOne = new TestUser {Email = null};
            Repository.Save(userOne);
            var userTwo = new TestUser {Email = ""};
            Repository.Save(userTwo);
            var userThree = new TestUser {Email = "bobs@burgers.com"};
            Repository.Save(userThree);

            var args = new TestSearchSet();

            var result = Repository.Search(args, (ICriteria)null,
                (mapper) => { mapper.MappedProperties["Email"].Value = SearchMapperSpecialValues.IsNotNullOrEmpty; });

            Assert.IsFalse(result.Contains(userOne));
            Assert.IsFalse(result.Contains(userTwo));
            Assert.IsTrue(result.Contains(userThree));
        }

        #endregion

        #region IsNullOrEmpty

        [TestMethod]
        public void
            TestMapMethodCorrectlyHandlesIsNullOrEmptyValueSearchingWhenASearchActuallyRequiresANullOrEmptyValue()
        {
            var userOne = new TestUser {Email = null};
            Repository.Save(userOne);
            var userTwo = new TestUser {Email = ""};
            Repository.Save(userTwo);
            var userThree = new TestUser {Email = "bobs@burgers.com"};
            Repository.Save(userThree);

            var args = new TestSearchSet();

            var result = Repository.Search(args, (ICriteria)null,
                (mapper) => { mapper.MappedProperties["Email"].Value = SearchMapperSpecialValues.IsNullOrEmpty; });

            Assert.IsTrue(result.Contains(userOne));
            Assert.IsTrue(result.Contains(userTwo));
            Assert.IsFalse(result.Contains(userThree));
        }

        #endregion

        #region ISearchCriteria values

        [TestMethod]
        public void TestMapperUsesCriterionReturnedFromValueThatImplementsISearchCriterion()
        {
            var userOne = new TestUser {Email = null};
            Repository.Save(userOne);
            var userTwo = new TestUser {Email = "user@two.com"};
            Repository.Save(userTwo);

            var args = new TestSearchSet();
            args.Criterion = new TestSearchCriterionImpl();
            args.Criterion.ExpectedCriterion = Restrictions.IsNull("Email");
            var result = Repository.Search(args).ToArray();

            Assert.IsTrue(result.Contains(userOne));
            Assert.IsFalse(result.Contains(userTwo));
        }

        #endregion

        #region Aliases

        [TestMethod]
        public void TestGetAliasesReturnsSearchAliases()
        {
            var args = new TestSearchSet();
            var mapper = new SearchMapper(args, typeof(TestUser), Session);

            var foo = mapper.GetAliases();

            Assert.AreEqual(1, foo.Count);
            Assert.AreEqual("TG", foo.First().Key);
            Assert.AreEqual("MainGroup", foo.First().Value);
        }

        #endregion

        #region Foreign Keys

        // TModel/TEntity
        [TestMethod]
        public void TestShouldGetByForeignPropertyIfRequested()
        {
            var testGroup = new TestGroupFactory(_container).Create();

            var users = new[] {
                new TestUserFactory(_container).Create(),
                new TestUserFactory(_container).Create(new {OtherGroup = testGroup})
            };

            var args = new TestSearchSet {OtherGroup = testGroup.Id};
            var actual = Repository.Search(args).ToArray();
            Assert.AreSame(users[1], actual.Single());
        }

        #endregion

        #region Arrays

        [TestMethod]
        public void TestMappingForArrays()
        {
            var userOne = new TestUser {Email = "user@one.com"};
            Repository.Save(userOne);
            var userTwo = new TestUser {Email = "user@two.com"};
            Repository.Save(userTwo);
            var userThree = new TestUser {Email = "user@three.com"};
            Repository.Save(userThree);

            var args = new TestArraySearchSet();
            args.Id = new[] {userTwo.Id, userThree.Id};
            var result = Repository.Search(args).ToArray();

            Assert.IsFalse(result.Contains(userOne));
            Assert.IsTrue(result.Contains(userTwo));
            Assert.IsTrue(result.Contains(userThree));
        }

        #endregion

        #region SearchModifiers

        [TestMethod]
        public void TestSearchingUsesSearchModifiersAddedDuringModifyValues()
        {
            var userOne = new TestUser {Email = "user@one.com"};
            Repository.Save(userOne);
            var userTwo = new TestUser {Email = "user@two.com"};
            Repository.Save(userTwo);

            var args = new TestSearchSet();
            args._addModifierCallback = (mapper) => {
                var modifier = new TestSearchModifier();
                modifier.Callback = (crit, props) => {
                    var emailProp = props["Email"];
                    emailProp.Value = "user@one.com";
                    return emailProp.ToCriterion(crit);
                };
                mapper.SearchModifiers.Add(modifier);
            };
            var result = Repository.Search(args).ToArray();

            Assert.IsTrue(result.Contains(userOne));
            Assert.IsFalse(result.Contains(userTwo));
        }

        [TestMethod]
        public void TestSearchModifiersCanUsePropertiesThatHaveCanMapSetToFalse()
        {
            var userOne = new TestUser {Email = "user@one.com", IsAdmin = true};
            Repository.Save(userOne);
            var userTwo = new TestUser {Email = "user@two.com", IsAdmin = false};
            Repository.Save(userTwo);

            var args = new TestSearchSet();
            args.Unmappable = true;
            args._addModifierCallback = (mapper) => {
                var modifier = new TestSearchModifier();
                modifier.Callback = (crit, props) => {
                    var prop = props["Unmappable"];
                    prop.ActualName = "IsAdmin";
                    Assert.IsFalse(prop.CanMap, "Sanity");
                    return prop.ToCriterion(crit);
                };
                mapper.SearchModifiers.Add(modifier);
            };
            var result = Repository.Search(args).ToArray();

            Assert.IsTrue(result.Contains(userOne));
            Assert.IsFalse(result.Contains(userTwo));
        }

        [TestMethod]
        public void TestSeachingWithOrConstraintSearchModifier()
        {
            var userOne = new TestUser {Email = "user@one.com", IsAdmin = false};
            Repository.Save(userOne);
            var userTwo = new TestUser {Email = "user@two.com", IsAdmin = true};
            Repository.Save(userTwo);
            var userThree = new TestUser {Email = "user@three.com", IsAdmin = false};
            Repository.Save(userThree);

            var args = new TestSearchSet();
            args.Email = "user@one.com";
            args.IsAdmin = true;
            args._addModifierCallback = (mapper) => {
                mapper.SearchModifiers.Add(new OrConstraintSearchModifier("Email", "IsAdmin"));
            };
            var result = Repository.Search(args).ToArray();

            Assert.IsTrue(result.Contains(userOne), "userOne must be returned because it has a matching email.");
            Assert.IsTrue(result.Contains(userTwo), "userTwo must be returned because IsAdmin == true.");
            Assert.IsFalse(result.Contains(userThree),
                "userThree should not be returned because it doesn't match either criteria.");
        }

        #endregion

        [TestMethod]
        public void TestSearchingForTheExistenceOfChildCollections()
        {
            var search = new TestChildCollectionSearchSet();
            var userWithChildren = new TestUser {Email = "i.do@have.children"};
            var child1 = new TestUserChildItem {TestUser = userWithChildren};
            var child2 = new TestUserChildItem {TestUser = userWithChildren};
            userWithChildren.ChildItems.Add(child1);
            userWithChildren.ChildItems.Add(child2);
            var userWithoutChild = new TestUser {Email = "i@have.no.children"};
            Repository.Save(userWithChildren);
            Repository.Save(userWithoutChild);

            // 1. That an entity with a child collection with at least one item can be found
            search.ChildItems = true;
            Assert.AreSame(userWithChildren, Repository.Search(search).Single());

            // 2. That an entity with a child collection with zero items can be found.
            search.ChildItems = false;
            Assert.AreSame(userWithoutChild, Repository.Search(search).Single());

            // 3. That if the search value itself is null that all of the items are returned.
            search.ChildItems = null;
            var result = Repository.Search(search).ToList();
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Contains(userWithChildren));
            Assert.IsTrue(result.Contains(userWithoutChild));
        }

        [TestMethod]
        public void TestMapRemovesPropertiesThatAreNotUsed()
        {
            ISearchMapper mapperInstance = null;

            var result = Repository.Search(new TestArraySearchSet(), (ICriteria)null,
                (mapper) => { mapperInstance = mapper; });

            Assert.AreEqual(0, mapperInstance.MappedProperties.Count);

            result = Repository.Search(new TestSearchSet(), (ICriteria)null, (mapper) => { mapperInstance = mapper; });

            Assert.AreEqual(0, mapperInstance.MappedProperties.Count);

            result = Repository.Search(new TestSearchSet {Email = "foo"}, (ICriteria)null,
                (mapper) => { mapperInstance = mapper; });

            Assert.AreEqual(1, mapperInstance.MappedProperties.Count);
        }

        [TestMethod]
        public void TestClearValuesNullsOutTheValuesOnAllMappedProperties()
        {
            var target = new SearchMapper(new TestSearchSet { Email = "foo" }, typeof(TestUser), Session);
            var property = target.MappedProperties[nameof(TestSearchSet.Email)];
            Assert.AreEqual("foo", property.Value);

            target.ClearValues();
            Assert.IsNull(property.Value);
        }

        #region Test classes

        private class TestSearchCriterionImpl : ISearchCriterion
        {
            public ICriterion ExpectedCriterion { get; set; }

            public ICriterion GetCriterion(ICriterion original, string propertyName)
            {
                return ExpectedCriterion;
            }
        }

        private class TestSearchSet : SearchSet<TestUser>
        {
            #region Fields

            // Thsee aren't properties because otherwise the search mapper tries to map them like normal properties.
            public Action _modifyValuesCallback;
            public Action<ISearchMapper> _addModifierCallback;

            #endregion

            #region Properties

            public int? Id { get; set; }
            public string Email { get; set; }
            public bool? IsAdmin { get; set; }
            public int? MainGroupId { get; set; }

            public int? SomeForeignId { get; set; }

            // for testing sequences:
            public int? SomeOrder { get; set; }
            public int? OtherOrder { get; set; }

            [SearchAlias("MainGroup", "TG", "Id")]
            public TestGroup MainGroup { get; set; }

            public int? OtherGroup { get; set; }

            public SearchString SearchString { get; set; }

            public TestSearchCriterionImpl Criterion { get; set; }

            // All tests will fail if this gets set to true. No explicit test for this.
            [Search(CanMap = false)]
            public bool Unmappable { get; set; }

            #endregion

            public override void ModifyValues(ISearchMapper mapper)
            {
                base.ModifyValues(mapper);
                if (_modifyValuesCallback != null)
                {
                    _modifyValuesCallback();
                }

                _addModifierCallback?.Invoke(mapper);
            }
        }

        private class ISearchSetImplementation : ISearchSet<TestUser>
        {
            public int Id { get; set; }

            public int PageNumber { get; set; }
            public int PageSize { get; set; }
            public int PageCount { get; set; }
            public int Count { get; set; }
            public bool EnablePaging { get; set; }
            public string SortBy { get; set; }
            public bool SortAscending { get; set; }
            public IEnumerable<TestUser> Results { get; set; }

            public string DefaultSortBy { get; private set; }
            public bool DefaultSortAscending { get; private set; }
            public List<string> ExportableProperties { get; set; }

            public void ModifyValues(ISearchMapper mapper)
            {
                // noop
            }
        }

        private interface ITestInterfaceSearch : ISearchSet<TestUser>
        {
            [SearchAlias("MainGroup", "MainGroupAlias", "Id")]
            int? MainGroup { get; set; }

            [SearchAlias("Wrong", "wrong", "WRONG")]
            int? PropertyWithOverrideSearchAlias { get; set; }
        }

        private class TestInterfaceSearch : SearchSet<TestUser>, ITestInterfaceSearch
        {
            // NOTE: No search alias here. It's on interface.
            public int? MainGroup { get; set; }

            [SearchAlias("MainGroup", "MainGroupAlias", "Id")]
            public int? PropertyWithOverrideSearchAlias { get; set; }

            public Action<ISearchMapper> Callback;

            public override void ModifyValues(ISearchMapper mapper)
            {
                base.ModifyValues(mapper);
                Callback(mapper);
            }
        }

        private class TestArraySearchSet : SearchSet<TestUser>
        {
            public int[] Id { get; set; }
        }

        private class TestEntityIdToIdSearchSet : SearchSet<TestUser>
        {
            public int? EntityId { get; set; }
        }

        private class TestChildCollectionSearchSet : SearchSet<TestUser>
        {
            [Search(ChecksExistenceOfChildCollection = true)]
            public bool? ChildItems { get; set; }
        }

        private class TestUserFactory : TestDataFactory<TestUser>
        {
            static TestUserFactory()
            {
                Defaults(new { });
            }

            public TestUserFactory(IContainer session) : base(session) { }
        }

        private class TestGroupFactory : TestDataFactory<TestGroup>
        {
            static TestGroupFactory()
            {
                Defaults(new { });
            }

            public TestGroupFactory(IContainer session) : base(session) { }
        }

        private class TestSearchModifier : ISearchModifier
        {
            public Func<ICriterion, IDictionary<string, SearchMappedProperty>, ICriterion> Callback;

            public ICriterion ToCriterion(ICriterion currentCrit,
                IDictionary<string, SearchMappedProperty> mappedProperties)
            {
                return Callback.Invoke(currentCrit, mappedProperties);
            }
        }

        #endregion
    }
}
