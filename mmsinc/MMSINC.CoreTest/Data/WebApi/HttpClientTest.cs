using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Routing;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.WebApi;
using HttpClient = MMSINC.Data.WebApi.HttpClient;
using SystemHttpClient = System.Net.Http.HttpClient;

namespace MMSINC.CoreTest.Data.WebApi
{
    [TestClass]
    public class HttpClientTest
    {
        #region Private Members

        private HttpClient _target;
        private HttpServer _server;
        private HttpConfiguration _config;
        private const string _url = "http://somesite.com";

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _config = BuildServerConfig();
            _server = new HttpServer(_config);
            _target = new TestHttpClient(new SystemHttpClient(_server)) {
                BaseAddress = new Uri(_url),
                IsSiteRunning = true
            };
        }

        [TestCleanup]
        public void TestCleanup()
        {
            FooController.ResetObjectCache();
            _server.Dispose();
        }

        private HttpConfiguration BuildServerConfig()
        {
            var config = new HttpConfiguration();

            config.Routes
                  .Add("New", new HttpRoute("{controller}/new",
                       new HttpRouteValueDictionary {{"action", "New"}},
                       new HttpRouteValueDictionary {{"method", new HttpMethodConstraint(HttpMethod.Get)}}));
            config.Routes
                  .Add("Post", new HttpRoute("{controller}/{id}",
                       new HttpRouteValueDictionary {{"action", "Post"}, {"id", RouteParameter.Optional}},
                       new HttpRouteValueDictionary {{"method", new HttpMethodConstraint(HttpMethod.Post)}}));
            config.Routes
                  .Add("Default", new HttpRoute("{controller}/{id}",
                       new HttpRouteValueDictionary {{"id", RouteParameter.Optional}}));
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            return config;
        }

        #endregion

        #region InnerClient

        [TestMethod]
        public void TestBaseAddressReflectsSameOnInnerCLient()
        {
            var address = new Uri(_url);

            _target.BaseAddress = address;

            Assert.AreSame(address, _target.BaseAddress);
            Assert.AreSame(address, _target.InnerClient.BaseAddress);
        }

        #endregion

        #region Properties

        [TestMethod]
        public void TestIsSiteRunningReturnsTrueIfSiteIsRunning()
        {
            var baseAddress = "http://localhost:2020/";
            _target = new TestHttpClient(new SystemHttpClient(_server)) {
                BaseAddress = new Uri(baseAddress)
            };

            var server = new HttpListener();
            server.Prefixes.Add(baseAddress);
            server.Start();

            var actual = _target.IsSiteRunning;
            server.Stop();

            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void TestIsSiteRunningReturnsFalseIfSiteIsNotRunning()
        {
            _target = new TestHttpClient(new SystemHttpClient(_server)) {
                BaseAddress = new Uri("http://invalidLocalserverhost")
            };

            Assert.IsFalse(_target.IsSiteRunning);
        }

        #endregion

        #region AcceptMimeType(string)

        [TestMethod]
        public void TestAcceptMimeTypeAddsMimeTypeToTheListOfAcceptedRequestHeaders()
        {
            var mimeType = "application/json";

            _target.AcceptMimeType(mimeType);

            Assert.IsTrue(_target.InnerClient.DefaultRequestHeaders.Accept
                                 .Contains(new MediaTypeWithQualityHeaderValue(mimeType)));
        }

        #endregion

        #region SetAuthorizationToken(string)

        [TestMethod]
        public void TestSetAuthorizationTokenSetsTheTokenIntheDefaultRequestHeaders()
        {
            var token = "this is the token";
            IEnumerable<string> values;

            _target.SetAuthorizationToken(token);
            _target.InnerClient.DefaultRequestHeaders.TryGetValues(HttpClient.AUTHORIZATION_TOKEN_KEY, out values);

            Assert.AreEqual(token, values.ToArray()[0]);
        }

        #endregion

        #region Get<TEntity>(string, int)

        [TestMethod]
        public void TestGetGetsTheTypedItemAtTheSpecifiedUrlWithTheSpecifiedId()
        {
            var foo = new Foo();
            foo = FooController.ObjectCache.Save(foo);

            var result = _target.Get<Foo>("foo", foo.Id);

            Assert.AreEqual(foo, result);
        }

        [TestMethod]
        public void TestGetReturnsNullWhenSiteIsNotRunning()
        {
            _target.IsSiteRunning = false;

            var result = _target.Get<Foo>("foo", 1);

            Assert.IsNull(result);
        }

        #endregion

        #region Post<TEntity>(string, TEntity)

        [TestMethod]
        public void TestPostPostsTheSpecifiedItemToTheSpecifiedUrl()
        {
            var foo = new Foo();
            FooController.ObjectCache.Save(foo);

            var result = _target.Post("foo", foo);

            Assert.AreEqual(foo, result);
            Assert.AreEqual(FooController.ObjectCache.Count, result.Id);

            var newFoo = new Foo();

            result = _target.Post("foo", newFoo);

            Assert.AreEqual(newFoo, result);
            Assert.AreEqual(FooController.ObjectCache.Count, result.Id);
        }

        [TestMethod]
        public void TestPostReturnsNullWhenSiteIsNotRunning()
        {
            _target.IsSiteRunning = false;

            var result = _target.Post("foo", new Foo());

            Assert.IsNull(result);
        }

        #endregion

        #region Search<TEntity>(string, StringDictionary)

        [TestMethod]
        public void TestSearchSearchesForTheItemSpecifiedByTheSearchHash()
        {
            var foo = new Foo {Name = "foo"};
            FooController.ObjectCache.Save(foo);

            var result = _target.Search<Foo>("foo", new NameValueCollection {{"name", "foo"}});

            Assert.AreEqual(foo.Name, result.Single().Name);
        }

        [TestMethod]
        public void TestSearchReturnsNullWhenSiteIsNotRunning()
        {
            _target.IsSiteRunning = false;

            var result = _target.Search<Foo>("foo", new NameValueCollection());

            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestSearchThrowsExceptionWhenResponseIsUnsuccessful()
        {
            MyAssert.Throws<HttpRequestException>(() =>
                _target.Search<Foo>("foo", new NameValueCollection {{"error", "foo"}}));
        }
        
        #endregion

        #region Delete(string, int)

        [TestMethod]
        public void TestDeleteDeletesTheItemAtTheSpecifiedUrlWithTheSpecifiedId()
        {
            var foo = new Foo();
            foo = FooController.ObjectCache.Save(foo);

            _target.Delete("foo", foo.Id);

            Assert.IsFalse(FooController.ObjectCache.Exists(foo.Id));
        }

        [TestMethod]
        public void TestDeleteThrowsIndexOutOfRangeExceptionIfStatusCodeIs404()
        {
            MyAssert.Throws<IndexOutOfRangeException>(() => _target.Delete("foo", 666));
        }

        [TestMethod]
        public void TestDeleteThrowsHttpResponseExceptionWhenSiteIsNotRunning()
        {
            _target.IsSiteRunning = false;

            MyAssert.Throws<HttpResponseException>(() => _target.Delete("blerg", 666));
        }

        #endregion

        #region New(string, int)

        [TestMethod]
        public void TestNewNewsANewItemAtTheSpecifiedUrlWithTheSpecifiedInitialParameters()
        {
            var foo = _target.New("foo");

            Assert.AreEqual("\"" + FooController.MAGICAL_NEW_RESPONSE_IN_A_CAN + "\"", foo);
        }

        [TestMethod]
        public void TestNewThrowsHttpResponseExceptionWhenSiteIsNotRunning()
        {
            _target.IsSiteRunning = false;

            MyAssert.Throws<HttpResponseException>(() => _target.New("blergh"));
        }

        #endregion
    }

    public class TestHttpClient : HttpClient
    {
        #region Constructors

        public TestHttpClient(SystemHttpClient innerClient)
        {
            _innerClient = innerClient;
        }

        #endregion
    }

    public class Foo
    {
        #region Properties

        public int Id { get; set; }
        public string Name { get; set; }

        #endregion
    }

    public class FooRepository : IRepository<Foo>
    {
        #region Private Members

        private readonly Dictionary<int, Foo> _innerCollection;

        #endregion

        #region Properties

        public int Count
        {
            get { return _innerCollection.Count; }
        }

        #endregion

        #region Constructors

        public FooRepository()
        {
            _innerCollection = new Dictionary<int, Foo>();
        }

        #endregion

        #region Exposed Methods

        public void Delete(int id)
        {
            if (!_innerCollection.ContainsKey(id))
            {
                throw new IndexOutOfRangeException(String.Format("Object with id {0} not found.", id));
            }

            _innerCollection[id] = null;
        }

        public void Delete(Foo entity)
        {
            throw new NotImplementedException();
        }

        public Foo Save(Foo foo)
        {
            if (_innerCollection.ContainsKey(foo.Id))
            {
                _innerCollection[foo.Id] = foo;
            }
            else if (foo.Id == 0)
            {
                foo.Id = _innerCollection.Count + 1;
                _innerCollection[foo.Id] = foo;
            }
            else
            {
                throw new IndexOutOfRangeException(
                    String.Format("Foo with id {0} not found for updating.", foo.Id));
            }

            return foo;
        }

        public void Save(IEnumerable<Foo> entities)
        {
            throw new NotImplementedException();
        }

        public void Update(Foo entity)
        {
            throw new NotImplementedException();
        }

        public Foo Find(int id)
        {
            return _innerCollection[id];
        }

        public bool Exists(int id)
        {
            return _innerCollection.ContainsKey(id) && (_innerCollection[id] != null);
        }

        public int GetIdentifier(Foo entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Foo> Search(NameValueCollection searchParams)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Foo> Search(int[] ids)
        {
            throw new NotImplementedException();
        }

        public Foo[] FindByName(string name)
        {
            foreach (var foo in _innerCollection.Values)
            {
                if (foo.Name == name)
                {
                    return new[] { foo };
                }
            }

            return Enumerable.Empty<Foo>().ToArray();
        }

        public string New(NameValueCollection initialParams = null)
        {
            throw new NotImplementedException();
        }

        public Foo Save(NameValueCollection entity)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    // ApiController has no real benefit to us when we aren't *using* ApiController for anything
    // in the code base otherwise. The only thing it does offer us is an ability to setup what looks
    // like a full-pipeline request to a controller to test against. 
    public class FooController : ApiController
    {
        #region Constants

        public const string MAGICAL_NEW_RESPONSE_IN_A_CAN = "Some magical form for you to fill out.";

        #endregion

        #region Private Members

        private static FooRepository _objectCache;

        #endregion

        #region Static Properties

        public static FooRepository ObjectCache
        {
            get { return _objectCache ?? (_objectCache = new FooRepository()); }
            private set { _objectCache = value; }
        }

        #endregion

        #region Exposed Methods

        public static void ResetObjectCache()
        {
            ObjectCache = new FooRepository();
        }

        public Foo Get(int id)
        {
            return ObjectCache.Find(id);
        }

        public void Delete(int id)
        {
            if (ObjectCache.Exists(id))
            {
                ObjectCache.Delete(id);
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        public Foo Post(Foo foo)
        {
            return ObjectCache.Save(foo);
        }

        // Trying to understand how GetByName is actually called? You might want to read up
        // on how ApiController and the routing of .NET Framework's API stuff works. It's different
        // from MVC.
        // https://learn.microsoft.com/en-us/aspnet/web-api/overview/web-api-routing-and-actions/routing-and-action-selection
        //
        // The short version is that, for these tests, "GetByName", is called because
        // the API is receiving a GET request for the "foo" controller. Rather than 
        // the routes being setup with an action parameter, they look through the parameters
        // to see if something matches in some way. In the case of the Search tests above,
        // we're sending a parameter called "name", which the API sees as a matching parameter
        // and matches it with the GetByName action. ie, it found a match for [HttpMethod]By[Parameter].
        // Since modelbinding is involved somehow, the parameter name of the action has to match the
        // name in the action. ie "GetByName" has to have "name" as a parameter.
        // 
        // We don't use ApiController literally anywhere in the MapCall codebase, so I don't
        // know why it was used for this test. Even the Permitst API, which is the only thing this HttpClient
        // class is ever used with, doesn't use ApiController.
        public IEnumerable<Foo> GetByName(string name)
        {
            return ObjectCache.FindByName(name);
        }

        public IEnumerable<Foo> GetByError(string error)
        {
            throw new Exception("Intentional error.");
        }

        [AcceptVerbs("Get"), HttpGet]
        public String New()
        {
            return MAGICAL_NEW_RESPONSE_IN_A_CAN;
        }

        #endregion
    }
}
