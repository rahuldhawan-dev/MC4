using System;

namespace MMSINC.Utilities.APIM
{
    public interface IAPIMClientConfiguration
    {
        string Scheme { get; }
        string Host { get; }
        int Port { get; }
        string Path { get; }
        Uri ApiUri { get; }
        string ApiKey { get; }
        int TimeoutInMinutes { get; }
    }
}
