using MMSINC.Utilities.APIM;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace MMSINC.Testing.Utilities
{
    /// <summary>
    /// A factory that generates an HttpClient that always returns async calls with the given mocked response.
    /// </summary>
    /// <typeparam name="T">The type of the response to be mocked.</typeparam>
    /// <seealso cref="MMSINC.Utilities.APIM.IAPIMClientFactory" />
    public class APIMClientMockFactory<T> : IAPIMClientFactory
    {
        #region Private Fields

        private readonly T _sendAsyncResponseMock;

        #endregion  

        #region Constructors

        public APIMClientMockFactory(T mockedApiResponse)
        {
            _sendAsyncResponseMock = mockedApiResponse;
        }

        #endregion  

        #region Exposed Methods

        public HttpClient Build(IAPIMClientConfiguration configuration)
        {
            var responseMessage = new HttpResponseMessage {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(_sendAsyncResponseMock))
            };

            responseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var messageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            messageHandlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
               .ReturnsAsync(responseMessage)
               .Verifiable();

            return new HttpClient(messageHandlerMock.Object) {
                BaseAddress = configuration.ApiUri
            };
        }

        #endregion
    }
}
