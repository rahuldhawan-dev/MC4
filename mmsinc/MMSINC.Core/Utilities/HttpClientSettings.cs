namespace MMSINC.Utilities
{
    /// <summary>
    /// Optional settings to use when creating an <see cref="IHttpClient"/>.
    /// </summary>
    public class HttpClientSettings
    {
        /// <summary>
        /// If set, this will provide the <see cref="IHttpClient.BaseAddress"/>.
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// If set, this will provide the <see cref="IHttpClient.Timeout"/>.
        /// </summary>
        public int? Timeout { get; set; }

        /// <summary>
        /// If set, this will provide a token value as an "authorization" header value for any requests made by the
        /// client by default.
        /// </summary>
        public string AuthorizationToken { get; set; }
    }
}
