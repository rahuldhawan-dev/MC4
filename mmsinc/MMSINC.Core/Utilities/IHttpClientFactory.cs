namespace MMSINC.Utilities
{
    /// <summary>
    /// Factory for creating <see cref="IHttpClient"/> instances, with or without settings.
    /// </summary>
    public interface IHttpClientFactory
    {
        #region Abstract Methods

        /// <summary>
        /// Build and return an <see cref="IHttpClient"/> instance with no settings.
        /// </summary>
        IHttpClient Build();
        
        /// <summary>
        /// Build and return an <see cref="IHttpClient"/> with the specified <paramref name="settings"/>.
        /// </summary>
        IHttpClient Build(HttpClientSettings settings);

        #endregion
    }
}
