using System;
using System.Net.Http;
using System.Text;
using System.Web.Mvc;
using DeleporterCore.Client;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Testing.NHibernate;
using MMSINC.Testing.SpecFlow.Library;
using Moq;
using NUnit.Framework;
using StructureMap;

namespace FunctionalTests
{
    public abstract class BaseApiTestClass
    {
        #region Constants

        public const string BASE_URL = "http://localhost:8888/";

        #endregion

        #region Private Members

        private HttpClient _client;
        private HttpRequestMessage _requestMessage;

        #endregion

        #region Properties

        public HttpClient Client => _client;

        /// <summary>
        /// This defaults to GET
        /// </summary>
        public HttpRequestMessage RequestMessage => _requestMessage;

        #endregion

        private string Base64Encode(string toEncode)
        {
            var bytes = Encoding.GetEncoding(28591).GetBytes(toEncode);
            return Convert.ToBase64String(bytes);
        }

        private void AddAuthorizationHeader(string username, string password)
        {
            RequestMessage.Headers.Add("Authorization", "Basic " + Base64Encode($"{username}:{password}"));
        }

        [SetUp]
        public virtual void SetUp()
        {
            Deleporter.Run(() => {
                TestHelperProxy.CreateSystemWideSession();
                TestHelperProxy.ResetNHibernateSessionMessages();
                TestHelperProxy.EnableRequestProcessing();
            });

            _client = new HttpClient();
            _requestMessage = new HttpRequestMessage {
                Method = HttpMethod.Get
            };
        }

        [TearDown]
        public void TearDown()
        {
            // Ideally, when we rollback, the site should not be responding to anymore requests.
            // That's most likely what's causing all the various nhibernate flukes. 
            try
            {
                Deleporter.Run(() => {
                    TestHelperProxy.DisableRequestProcessing();
                    TestHelperProxy.DestroySystemWideSession();
#if DEBUG
                    TestHelperProxy.ClearAllRegressionTestFlags();
#endif
                });
            }
            finally { }
        }

        private void AddAuthorizedUserForTesting()
        {
            var un = "thisisauser";
            var pd = "abcdefg";
            CreateAdminUser(un, pd);
            AddAuthorizationHeader(un, pd);
        }

        public TEntity CreateEntity<TEntity>(object overrides = null)
            where TEntity : class, IEntity, new()
        {
            TEntity entity = null;

            Deleporter.Run(() => {
                var container = DependencyResolver.Current.GetService<IContainer>();
                entity = container.With(typeof(ActionFactory).Assembly).GetInstance<TestDataFactoryService>()
                                  .GetEntityFactory<TEntity>().Create(overrides);
            });

            return entity;
        }

        /// <summary>
        /// We don't need to test Authorization again here, or even Authentication, that's done in the main test project.
        /// We are skipping Authentication using a mock for IMemberHelper
        /// We are skipping Auhotirization by making the user.IsAdmin true
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User CreateAdminUser(string username, string password)
        {
            var userEntity = CreateEntity<User>(new UserEntity { UserName = username, IsAdmin = true });

            Deleporter.Run(() => {
                var container = DependencyResolver.Current.GetService<IContainer>();
                var mockMembershipProvider = new Mock<IMembershipHelper>();
                mockMembershipProvider.Setup(x => x.ValidateUser(username, password)).Returns(true);
                container.Inject(mockMembershipProvider.Object);
            });

            return userEntity;
        }

        public void TestAsyncResponse(string url, Action<HttpResponseMessage> fn)
        {
            RequestMessage.RequestUri = new Uri(BASE_URL + url);
            using (var response = Client.SendAsync(RequestMessage).Result)
            {
                fn(response);
            }
        }

        public void TestAuthorizedAsyncResponse(string url, Action<HttpResponseMessage> fn)
        {
            AddAuthorizedUserForTesting();
            TestAsyncResponse(url, fn);
        }

        public void TestAsyncResponseWithBody(string url, object body, Action<HttpResponseMessage> fn)
        {
            RequestMessage.RequestUri = new Uri(BASE_URL + url);
            RequestMessage.Method = HttpMethod.Post;
            RequestMessage.Content = new StringContent(body.ToString(), Encoding.UTF8, "application/json");

            using (var response = Client.SendAsync(RequestMessage).Result)
            {
                fn(response);
            }
        }

        public void TestAuthorizedAsyncResponseWithBody(string url, object body, Action<HttpResponseMessage> fn)
        {
            AddAuthorizedUserForTesting();
            TestAsyncResponseWithBody(url, body, fn);
        }

        [Serializable]
        public class UserEntity
        {
            public string UserName { get; set; }
            public bool IsAdmin { get; set; }
        }
    }
}
