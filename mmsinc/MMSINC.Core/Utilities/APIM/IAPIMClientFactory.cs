using System.Net.Http;

namespace MMSINC.Utilities.APIM
{
    public interface IAPIMClientFactory
    {
        HttpClient Build(IAPIMClientConfiguration configuration);
    }
}
