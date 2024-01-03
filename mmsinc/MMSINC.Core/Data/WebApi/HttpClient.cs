using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Web.Http;
using MMSINC.ClassExtensions.NameValueCollectionExtensions;
using SystemHttpClient = System.Net.Http.HttpClient;

namespace MMSINC.Data.WebApi
{
    [Obsolete("Do not use this. It's specifically designed for use with the Permits API.")]
    public class HttpClient : IHttpClient
    {
        #region Constants

        public const string AUTHORIZATION_TOKEN_KEY = "Authorization-Token",
                            USER_NAME_KEY = "User-Name";

        #endregion

        #region Private Members

        protected SystemHttpClient _innerClient;
        private bool? _isSiteRunning;

        #endregion

        #region Properties

        public SystemHttpClient InnerClient
        {
            get { return _innerClient ?? (_innerClient = new SystemHttpClient()); }
        }

        public Uri BaseAddress
        {
            get { return InnerClient.BaseAddress; }
            set { InnerClient.BaseAddress = value; }
        }

        public bool IsSiteRunning
        {
            get
            {
                if (_isSiteRunning == null)
                    _isSiteRunning = CheckSite();
                return _isSiteRunning.Value;
            }
            set { _isSiteRunning = value; }
        }

        #endregion

        #region Private Methods

        private bool CheckSite()
        {
            var s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                s.Connect(Dns.GetHostAddresses(BaseAddress.DnsSafeHost), BaseAddress.Port);
                return true;
            }
            catch (SocketException)
            {
                return false;
            }
            finally
            {
                s.Dispose();
            }
        }

        private static TResult GetResultOrThrow<TResult>(HttpResponseMessage response)
        {
#if DEBUG
            if (!response.IsSuccessStatusCode)
            {
                // check for ysod
                var rgx = new Regex(@"<!--\s*\[(.*?)]:(\s*.*\s(.*[\n\r]*)*?)\s*(at(.*[\n\r]*)*)-->");
                var str = response.Content.ReadAsStringAsync().Result;
                if (rgx.IsMatch(str))
                {
                    var match = rgx.Match(str);
                    throw new HttpRequestException(
                        String.Format("ExceptionType: {0}, Message: {1}, StackTrace: {2}", match.Groups[1],
                            match.Groups[2],
                            match.Groups[4]));
                }

                // just throw the whole response if no ysod
                throw new HttpRequestException(str);
            }
#else
#endif
            response.EnsureSuccessStatusCode();

            try
            {
                return response.Content.ReadAsAsync<TResult>().Result;
            }
            catch (UnsupportedMediaTypeException e)
            {
                throw new Exception(
                    $"Unable to convert the following response: {response.Content.ReadAsStringAsync().Result}", e);
            }
        }

        #endregion

        #region Exposed Methods

        public void Dispose()
        {
            InnerClient.Dispose();
        }

        public void AcceptMimeType(string mimeType)
        {
            InnerClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mimeType));
        }

        public void SetAuthorizationToken(string token)
        {
            InnerClient.DefaultRequestHeaders.Add(AUTHORIZATION_TOKEN_KEY, token);
        }

        public void SetUserName(string name)
        {
            InnerClient.DefaultRequestHeaders.Add(USER_NAME_KEY, name);
        }

        #region Web Methods

        public TEntity Get<TEntity>(string url, int id)
        {
            if (!IsSiteRunning) return default(TEntity);
            using (var asyncResponse = InnerClient.GetAsync(url + "/" + id))
            using (var response = asyncResponse.Result)
            {
                return GetResultOrThrow<TEntity>(response);
            }
        }

        public TEntity Post<TEntity>(string url, TEntity obj)
        {
            if (!IsSiteRunning) return default(TEntity);
            using (var asyncResponse = InnerClient.PostAsJsonAsync(url, obj))
            using (var response = asyncResponse.Result)
            {
                return GetResultOrThrow<TEntity>(response);
            }
        }

        public IEnumerable<TEntity> Search<TEntity>(string url, NameValueCollection searchParams)
        {
            if (!IsSiteRunning) return null;
            using (var asyncResponse = InnerClient.GetAsync(url + searchParams.ToQueryString()))
            using (var response = asyncResponse.Result)
            {
                // Whoever wrote the Permits API endpoint decided that it should return 
                // a 404 Not Found if there aren't results for a search query. Rather 
                // than an empty result set. It's like Search got confused for Get. So
                // we have to assume that the 404 is an empty result set. It's worth pointing
                // out that if the URL is actually invalid, we're not gonna get a proper 404
                // to throw out. 
                if (!response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.NotFound)
                {
                    return Enumerable.Empty<TEntity>();
                }
                return GetResultOrThrow<IEnumerable<TEntity>>(response);
            }
        }

        public void Delete(string url, int id)
        {
            if (!IsSiteRunning) throw new HttpResponseException(HttpStatusCode.ServiceUnavailable);
            using (var asyncResponse = InnerClient.DeleteAsync(url + "/" + id))
            using (var response = asyncResponse.Result)
            {
                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        throw new IndexOutOfRangeException(String.Format("Cound not find record with id {0}.", id));
                    }

                    throw new Exception("Unable to delete the requested object.");
                }
            }
        }

        public string New(string url, NameValueCollection initialParams = null)
        {
            if (!IsSiteRunning) throw new HttpResponseException(HttpStatusCode.ServiceUnavailable);
            using (var asyncResponse = InnerClient.GetAsync(url + initialParams.ToQueryString()))
            {
                // NOTE: For some reason, this does not call GetResultOrThrow or otherwise
                // check to see if there was an error from the server. This results in the 
                // rendered output getting YSODs or other error html.
                using (var response = asyncResponse.Result)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
            }
        }

        #endregion

        #endregion
    }

    public interface IHttpClient : IDisposable
    {
        #region Abstract Methods

        void AcceptMimeType(string mimeType);
        void SetAuthorizationToken(string token);
        void SetUserName(string userName);
        TEntity Get<TEntity>(string url, int id);
        TEntity Post<TEntity>(string url, TEntity obj);
        IEnumerable<TEntity> Search<TEntity>(string url, NameValueCollection searchParams);
        void Delete(string url, int id);
        string New(string url, NameValueCollection initialParams = null);

        #endregion
    }
}
