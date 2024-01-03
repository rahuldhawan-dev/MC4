using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MMSINC.Utilities
{
    /// <inheritdoc />
    /// <remarks>
    /// This implementation simply wraps a <see cref="HttpClient"/>.
    /// </remarks>
    public class HttpClientWrapper : IHttpClient
    {
        #region Private Members

        private readonly HttpClient _innerClient;

        #endregion

        #region Properties

        /// <inheritdoc />
        public TimeSpan Timeout
        {
            get => _innerClient.Timeout;
            set => _innerClient.Timeout = value;
        }

        /// <inheritdoc />
        public Uri BaseAddress
        {
            get => _innerClient.BaseAddress;
            set => _innerClient.BaseAddress = value;
        }

        /// <inheritdoc />
        public HttpRequestHeaders DefaultRequestHeaders => _innerClient.DefaultRequestHeaders;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for the <see cref="HttpClientWrapper"/> class.  Use this constructor to provide an
        /// existing <see cref="HttpClient"/> instance, or use the empty constructor to generate a new one.
        /// </summary>
        public HttpClientWrapper(HttpClient client)
        {
            _innerClient = client;
        }

        /// <summary>
        /// Constructor for the <see cref="HttpClientWrapper"/> class.  Use this constructor to generate a
        /// new <see cref="HttpClient"/> instance, or use the other constructor to provide an existing one.
        /// </summary>
        public HttpClientWrapper() : this(new HttpClient()) {}

        #endregion

        #region Exposed Methods

        /// <inheritdoc />
        public Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
        {
            return _innerClient.PostAsync(requestUri, content);
        }

        /// <inheritdoc />
        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            return _innerClient.SendAsync(request);
        }

        /// <inheritdoc cref="HttpClient.Dispose" />
        public void Dispose()
        {
            _innerClient.Dispose();
        }

        #endregion
    }
}
