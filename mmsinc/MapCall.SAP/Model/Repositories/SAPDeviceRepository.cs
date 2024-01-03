using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using log4net;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.ViewModels;
using MapCall.SAP.Model.ViewModels.SAPDeviceDetail;
using MMSINC.Utilities;
using MMSINC.Utilities.APIM;
using Newtonsoft.Json;

namespace MapCall.SAP.Model.Repositories
{
    public class SAPDeviceRepository : ISAPDeviceRepository
    {
        #region Constants

        public const string MIU_DETAILS_WEB_METHOD = "miu-details",
                            METER_DETAILS_WEB_METHOD = "meter-details";

        #endregion

        #region Private members

        protected readonly ILog _log;

        #endregion

        #region Constructors
        
        public SAPDeviceRepository(ILog log)
        {
            _log = log;
        }

        #endregion

        #region Exposed Methods

        public virtual SAPDeviceCollection Search(SAPDeviceDetailRequest request)
        {
            return GetData(request);
        }

        private SAPDeviceCollection GetData(SAPDeviceDetailRequest request)
        {
            var _httpClient = new HttpClient();

            try
            {
                var method = request.DeviceType == "Z" ? METER_DETAILS_WEB_METHOD : MIU_DETAILS_WEB_METHOD;
                
                var queryParameters = $"?equipmentID=&deviceType={request.DeviceType}&deviceLocation={request.DeviceLocation}&actionCode={request.ActionCode}&MSN={request.MeterSerialNumber}";
                
                _httpClient.BaseAddress = new Uri(ConfigurationManager.AppSettings["AW-API-URL"]);

                _httpClient.DefaultRequestHeaders?.Add("APIKey", ConfigurationManager.AppSettings["AW-API-KEY"]);

                var requestUrl = new Uri(_httpClient.BaseAddress, method + queryParameters).ToString();

                using (HttpResponseMessage response = _httpClient.GetAsync(requestUrl).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var sapResponse = response.Content.ReadAsStringAsync().Result;
                        return JsonConvert.DeserializeObject<SAPDeviceCollection>(sapResponse);
                    }

                    return new SAPDeviceCollection();
                }
            }
            catch (Exception e)
            {
                _log.Error(e.ToString());

                return new SAPDeviceCollection();
            }
            finally
            {
                _httpClient.Dispose();
            }
        }

        #endregion
    }

    public interface ISAPDeviceRepository
    {
        #region Abstract Methods

        SAPDeviceCollection Search(SAPDeviceDetailRequest entity);

        #endregion
    }
}
