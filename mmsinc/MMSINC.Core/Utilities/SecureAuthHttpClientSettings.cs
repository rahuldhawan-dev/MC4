namespace MMSINC.Utilities
{
    public class SecureAuthHttpClientSettings
    {
        public string Url { get; set; }

        // Default Timeout is 100 seconds. Sometimes the initial request takes too long.
        public int? Timeout { get; set; }
    }
}
