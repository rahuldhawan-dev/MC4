using System.Web;
using System.Collections.Specialized;

// ReSharper disable CheckNamespace
namespace AuthorizeNet
    // ReSharper restore CheckNamespace
{
    public static class CheckoutFormReaders
    {
        /// <summary>
        /// This helper method will read the form post and load the values into the request, if present. This method
        /// should be used with the form builder, and expects
        /// </summary>
        public static IGatewayRequest BuildAuthAndCaptureFromPost(NameValueCollection post)
        {
            //validate the request first
            var request = new AuthorizationRequest(post);

            SerializeForm(request, post);
            return request;
        }

        /// <summary>
        /// This override will use HttpContext.Current - not ideal for testing
        /// </summary>
        public static IGatewayRequest BuildAuthAndCaptureFromPost()
        {
            return BuildAuthAndCaptureFromPost(HttpContext.Current.Request.Form);
        }

        /// <summary>
        /// Loops the form request and pushes the data into the request, if preset.
        /// </summary>
        static void SerializeForm(IGatewayRequest request, NameValueCollection collection)
        {
            var api = new ApiFields();
            foreach (string item in collection.Keys)
            {
                //always send the keys to the API - this allows for Merchant Custom Keys
                request.Queue(item, collection[item]);
            }
        }
    }
}
