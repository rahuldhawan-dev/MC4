using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using MMSINC.Data.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MMSINC.CoreTest.Data.WebApi
{
    [TestClass]
    public class RepositoryBaseTest
    {
        #region Private Members

        private TestRepository _target;
        private Mock<IHttpClient> _client;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _client = new Mock<IHttpClient>();
            _target = new TestRepository(new Uri("http://somesite.com"));
            _target.SetClient(_client.Object);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _client.VerifyAll();
        }

        private void SetBaseClientExpectations()
        {
            _client
               .Setup(x => x.AcceptMimeType("application/json"));
            _client
               .Setup(x => x.SetAuthorizationToken(_target.Token));
            _client
               .Setup(x => x.Dispose());
        }

        #endregion

        #region Url

        [TestMethod]
        public void TestUrlReturnsCorrectUrl()
        {
            Assert.AreEqual("api/fooclass", _target.Url);
        }

        #endregion

        #region Token

        [TestMethod]
        public void TestTokenReturnsTokenFromTheConfigFileWithTheProperKey()
        {
            Assert.AreEqual(
                ConfigurationManager.AppSettings[Repository<Object>.RSA_TOKEN_CONFIG_KEY],
                _target.Token);
        }

        #endregion

        #region New(StringDictionary)

        [TestMethod]
        public void TestNewNewsTheParamsAndReturnsTheResult()
        {
            // new news is newd news
            var parms = new NameValueCollection {
                {"Foo", "chicken"},
                {"Bar", "meatball"},
                {"Baz", "veal"}
            };
            var theContent = "this is the content";

            SetBaseClientExpectations();
            _client
               .Setup(x => x.New(_target.Url + "/new", parms))
               .Returns(theContent);

            Assert.AreEqual(theContent, _target.New(parms));
        }

        #endregion

        #region Save(TEntity)

        [TestMethod]
        public void TestSavePostsNewObject()
        {
            var obj = new FooClass();

            SetBaseClientExpectations();
            _client
               .Setup(x => x.Post(_target.Url, obj))
               .Returns(obj);

            Assert.AreSame(obj, _target.Save(obj));
        }

        #endregion

        #region Save(NameValueCollection)

        [TestMethod]
        public void TestSaveConvertsNameValueCollectionToTEntityAndPostsIt()
        {
            var parms = new NameValueCollection {
                {"Foo", "chicken"},
                {"Bar", "meatball"},
                {"Baz", "veal"}
            };

            SetBaseClientExpectations();
            _client
               .Setup(x => x.Post(_target.Url, It
                   .Is<FooClass>(
                        fc => fc.Foo == parms["Foo"] && fc.Bar == parms["Bar"] && fc.Bar == parms["Bar"])));

            _target.Save(parms);
        }

        #endregion

        #region Find(int)

        [TestMethod]
        public void TestFindFindsTheObjectWithTheGivenId()
        {
            var id = 123;
            var obj = new FooClass();

            SetBaseClientExpectations();
            _client
               .Setup(x => x.Get<FooClass>(_target.Url, id))
               .Returns(obj);

            Assert.AreSame(obj, _target.Find(id));
        }

        #endregion

        #region Search(StringDictionary)

        [TestMethod]
        public void TestSearchReturnsObject()
        {
            var obj = new FooClass();
            var param = new NameValueCollection {{"foo", "bar"}};
            SetBaseClientExpectations();
            _client
               .Setup(x => x.Search<FooClass>(_target.Url, param))
               .Returns(new[] {obj});

            Assert.AreSame(obj, ((object[])_target.Search(param))[0]);
        }

        #endregion

        #region Delete(int)

        [TestMethod]
        public void TestDeleteDeletesWithProvidedId()
        {
            var id = 123;

            SetBaseClientExpectations();
            _client
               .Setup(x => x.Delete(_target.Url, id));

            _target.Delete(id);
        }

        #endregion
    }

    [TestClass]
    public class RepositoryTest
    {
        #region Private Members

        private Repository<Object> _target;

        #endregion

        #region BaseAddress

        [TestMethod]
        public void TestBaseAddressReturnsValueProvidedToConstructor()
        {
            var baseAddress = new Uri("http://somesite.com");
            _target = new Repository<object>("foo", baseAddress);

            Assert.AreSame(baseAddress, _target.BaseAddress);
        }

        #endregion
    }

    public class TestRepository : Repository<FooClass>
    {
        #region Private Members

        private IHttpClient _client;

        #endregion

        #region Constructors

        public TestRepository(Uri baseAddress) : base("foo", baseAddress) { }

        #endregion

        #region Private Methods

        protected override IHttpClient GetClient()
        {
            return _client ?? base.GetClient();
        }

        #endregion

        #region Exposed Methods

        public void SetClient(IHttpClient client)
        {
            _client = client;
        }

        #endregion
    }

    public class FooClass
    {
        #region Properties

        public string Foo { get; set; }
        public string Bar { get; set; }
        public string Baz { get; set; }

        #endregion
    }
}
