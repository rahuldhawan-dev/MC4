using System.Linq;
using Apache.NMS.ActiveMQ.Commands;
using Newtonsoft.Json.Linq;

namespace MMSINC.Utilities.ActiveMQ
{
    public static class ApacheExtensions
    {
        #region Exposed Methods

        public static string ToJSON(this ActiveMQMapMessage that)
        {
            var obj = new JObject();

            foreach (var key in that.Body.Keys)
            {
                if (key == null)
                {
                    continue;
                }

                obj.Add(new JProperty(key.ToString(), that.Body[key.ToString()]));
            }

            return obj.ToString();
        }

        #endregion
    }
}
