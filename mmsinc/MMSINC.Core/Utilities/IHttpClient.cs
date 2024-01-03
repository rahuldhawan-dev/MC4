using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MMSINC.Utilities
{
    /// <summary>
    /// Interface for mockable <see cref="HttpClient"/> purposes.
    /// </summary>
    public interface IHttpClient : IDisposable
    {
        #region Abstract Properties
        
        /// <inheritdoc cref="HttpClient.Timeout" />
        TimeSpan Timeout { get; set; }
        
        /// <inheritdoc cref="HttpClient.BaseAddress" />
        Uri BaseAddress { get; set; }
        
        /// <inheritdoc cref="HttpClient.DefaultRequestHeaders" />
        HttpRequestHeaders DefaultRequestHeaders { get; }
        
        #endregion
        
        #region Abstract Methods
        
        /// <inheritdoc cref="HttpClient.PostAsync(string,HttpContent)" />
        Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content);
        /// <inheritdoc cref="HttpClient.SendAsync(HttpRequestMessage)" />
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);
        
        #endregion
    }
}
