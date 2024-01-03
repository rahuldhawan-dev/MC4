using System;
using System.ComponentModel;
using System.Linq;
using System.Web.Script.Services;
using System.Web.Services;
using MapCall.Common.Utility;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using WorkOrders.Model;

namespace LINQTo271.Views.WorkOrders
{
    /// <summary>
    /// Summary description for WorkOrdersServiceView
    /// </summary>
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1),
    ToolboxItem(false), ScriptService]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WorkOrdersServiceView : WebService
    {
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetWorkOrderById(int id)
        {
            var order = WorkOrderRepository.GetEntity(id);
            return new {
                operatingCenterID = order.OperatingCenterID,
                townID = order.TownID,
                townSectionID = order.TownSectionID,
                streetNumber = order.StreetNumber,
                streetID = order.StreetID,
                crossStreetID = order.NearestCrossStreetID,
                zipCode = order.ZipCode,
                assetTypeID = order.AssetTypeID,
                requestedByID = order.RequesterID,
                customerName = order.CustomerName,
                phoneNumber = order.PhoneNumber,
                secondaryPhoneNumber = order.SecondaryPhoneNumber,
                requestingEmployeeID = order.RequestingEmployeeID,
                purposeID = order.PurposeID,
                priorityID = order.PriorityID,
                workDescriptionID = order.WorkDescriptionID,
                markoutRequirementID = order.MarkoutRequirementID,
                trafficControlRequired = order.TrafficControlRequired,
                streetOpeningPermitRequired = order.StreetOpeningPermitRequired,
                valveID = order.ValveID,
                hydrantID = order.HydrantID,
                sewerOpeningID = order.SewerOpeningID,
                premiseNumber = order.PremiseNumber,
                serviceNumber = order.ServiceNumber,
                stormCatchID = order.StormCatchID, 
                latitude = order.Latitude, 
                longitude = order.Longitude
            };
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public bool ValidateMarkoutReadyAndUnexpired(int id, DateTime date)
        {
            var order = WorkOrderRepository.GetEntity(id);

            return
                new[] { MarkoutRequirementEnum.Emergency, MarkoutRequirementEnum.None }
                   .Contains(order.MarkoutRequirement.RequirementEnum) ||
                order.Markouts.Any(mo => mo.ReadyDate <= date && mo.ExpirationDate > date);
        }
    }
}
