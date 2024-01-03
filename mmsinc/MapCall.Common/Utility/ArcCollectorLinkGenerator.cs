using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using MapCall.Common.Model.Entities;
using MMSINC.ClassExtensions.NameValueCollectionExtensions;
using MMSINC.ClassExtensions.DateTimeExtensions;
using Newtonsoft.Json;

namespace MapCall.Common.Utility
{
    /// <summary>
    /// The ArcCollector feature provides links for mobile devices to open ESRI'S FieldMaps
    /// application, directly into editing a specific asset on the map.
    /// ESRI doesn't like to use standard libraries, as a result this class keeps needing updates.
    /// Instead of a standard HTML Decode library for the feature attributes parameter, they are
    /// doing what this is currently mimicking. 
    /// </summary>
    public class ArcCollectorLinkGenerator
    {
        #region Constants

        public const string ARCGIS_COLLECTOR_HTML_FORMAT_STRING =
            "<a href=\"{0}\" target=\"_blank\" class=\"arcgis-collector\"></a>";

        public const string ARCGIS_COLLECTOR_URL = "https://fieldmaps.arcgis.app";

        public const string ARCGIS = "{0}?itemID={1}&referenceContext=addFeature&featureSourceURL={2}&featureAttributes={3}";

        #endregion

        #region Private Methods

        private static double? TryGetEpoch(DateTime? dateTime)
        {
            return dateTime?.MillisecondsSinceEpoch();
        }

        #endregion

        #region Exposed Methods

        /// <summary>
        /// Returns href, used by 271/workorders
        /// </summary>
        public static string ArcCollectorLink(NameValueCollection nvc, dynamic workOrder = null)
        {
            if (workOrder != null && !string.IsNullOrWhiteSpace(workOrder.OperatingCenter.DataCollectionMapUrl))
            {
                return workOrder.OperatingCenter.DataCollectionMapUrl + nvc.ToQueryString();
            }

            return ARCGIS_COLLECTOR_URL + nvc.ToQueryString();
        }

        /// <summary>
        /// Returns html string for a hydrant for MVC
        /// </summary>
        /// <param name="hydrant">A MapCall.Common hydrant</param>
        /// <returns>html string with link</returns>
        public static string ArcCollectorHydrantLink(dynamic hydrant, dynamic assetType, dynamic workOrder = null)
        {
            var nvc = new Dictionary<string, object> {
                {"MapCall_ID", hydrant is Hydrant ? hydrant.Id.ToString() : hydrant.HydrantID.ToString()},
                {"HydrantID", hydrant.HydrantNumber},
                {"BillingType", hydrant.HydrantBilling?.Id.ToString()},
                {"InstallDate", TryGetEpoch(hydrant.DateInstalled)},
                {"LifeCycleStatus", hydrant is Hydrant ? hydrant.Status?.Id.ToString() : hydrant.AssetStatus?.AssetStatusID.ToString()},
                {"Manufacturer", hydrant.HydrantManufacturer?.Id.ToString()},
                {"Premise_ID", hydrant.PremiseNumber},
                {"RouteID", hydrant.Route?.ToString()},
                {"Sequence", hydrant.Stop?.ToString()},
                {"SAPID", hydrant is Hydrant ? hydrant.SAPEquipmentNumber : hydrant.SAPEquipmentID?.ToString()},
                {"WBS_Number", workOrder?.AccountCharged},
                {"WorkorderID", workOrder?.WorkOrderID.ToString()},
            };

            return string.Format(ARCGIS, 
                !string.IsNullOrWhiteSpace(hydrant.OperatingCenter.DataCollectionMapUrl) ? hydrant.OperatingCenter.DataCollectionMapUrl : ARCGIS_COLLECTOR_URL, 
                hydrant.OperatingCenter.ArcMobileMapId, 
                assetType.OneMapFeatureSource, 
                GetFeatureAttributes(nvc));
        }

        public static HtmlString ArcCollectorHydrantHtmlString(dynamic hydrant, dynamic assetType,
            dynamic workOrder = null)
        {
            return new HtmlString(string.Format(ARCGIS_COLLECTOR_HTML_FORMAT_STRING,
                ArcCollectorHydrantLink(hydrant, assetType, workOrder)));
        }

        /// <summary>
        /// Returns string for a sewer opening for MVC
        /// </summary>
        /// <param name="sewerOpening">A dynamic for 271 or MapCall.Common SewerOpening</param>
        /// <returns>string link</returns>
        public static string ArcCollectorSewerOpeningLink(dynamic sewerOpening, dynamic assetType,
            dynamic workOrder = null)
        {
            var nvc = new Dictionary<string, object> {
                {"MapCall_ID", sewerOpening.Id.ToString()},
                {"StructureID", sewerOpening.OpeningNumber},
                {"Depth", sewerOpening.DepthToInvert?.ToString()},
                {"Diameter", string.Empty},
                {"InstallDate", TryGetEpoch(sewerOpening.DateInstalled)},
                {
                    "LifeCycleStatus",
                    sewerOpening is SewerOpening
                        ? sewerOpening.Status?.Id.ToString()
                        : sewerOpening.AssetStatus?.AssetStatusID.ToString()
                },
                {"Material", sewerOpening.SewerOpeningMaterial?.Id.ToString()},
                {"RimElevation", sewerOpening.RimElevation?.ToString()},
                {"Route", sewerOpening.Route?.ToString()}, {
                    "SAPID",
                    sewerOpening is SewerOpening
                        ? sewerOpening.SAPEquipmentNumber
                        : sewerOpening.SAPEquipmentID?.ToString()
                },
                {"Sequence", sewerOpening.Stop?.ToString()},
                {"WBS_Number", workOrder?.AccountCharged},
                {"WorkorderID", workOrder?.WorkOrderID.ToString()}
            };

            return string.Format(ARCGIS,
                !string.IsNullOrWhiteSpace(sewerOpening.OperatingCenter.DataCollectionMapUrl) ? sewerOpening.OperatingCenter.DataCollectionMapUrl : ARCGIS_COLLECTOR_URL,
                sewerOpening.OperatingCenter.ArcMobileMapId, assetType.OneMapFeatureSource,
                GetFeatureAttributes(nvc));
        }

        public static HtmlString ArcCollectorSewerOpeningHtmlString(dynamic sewerOpening, AssetType assetType,
            dynamic workOrder = null)
        {
            return new HtmlString(string.Format(ARCGIS_COLLECTOR_HTML_FORMAT_STRING,
                ArcCollectorSewerOpeningLink(sewerOpening, assetType, workOrder)));
        }

        /// <summary>
        /// Returns html string for a valve for MVC
        /// </summary>
        /// <param name="valve">A MapCall.Common hydrant</param>
        /// <returns>html string with link</returns>
        public static string ArcCollectorValveLink(dynamic valve, dynamic assetType, dynamic workOrder = null)
        {
            var nvc = new Dictionary<string, object> {
                {"MapCall_ID", valve is Valve ? valve.Id.ToString() : valve.ValveID.ToString()},
                {"ValveID", valve.ValveNumber},
                {"Application", valve.ValveControls?.Id.ToString()},
                {"Diameter", valve.ValveSize?.Size.ToString()},
                {"InstallDate", TryGetEpoch(valve.DateInstalled)},
                {"LifeCycleStatus", valve is Valve ? valve.Status?.Id.ToString() : valve.AssetStatus?.AssetStatusID.ToString()},
                {"Manufacturer", valve.ValveMake?.Id.ToString()},
                {"NormalPosition", valve.NormalPosition?.Id.ToString()},
                {"OpenDirection", valve.OpenDirection?.Id.ToString()},
                {"RouteID", valve.Route?.ToString()},
                {"SAPID", valve is Valve ? valve.SAPEquipmentNumber : valve.SAPEquipmentID?.ToString()},
                {"Sequence", valve.Stop?.ToString()},
                {"TurnsToClose", valve.Turns?.ToString()},
                {"ValveType", valve.ValveType?.Id.ToString()},
                {"WBS_Number", workOrder?.AccountCharged},
                {"WorkorderID", workOrder?.WorkOrderID.ToString()}
            };

            return string.Format(ARCGIS,
                !string.IsNullOrWhiteSpace(valve.OperatingCenter.DataCollectionMapUrl) ? valve.OperatingCenter.DataCollectionMapUrl : ARCGIS_COLLECTOR_URL, 
                valve.OperatingCenter.ArcMobileMapId, assetType.OneMapFeatureSource,
                GetFeatureAttributes(nvc));
        }

        public static object GetFeatureAttributes(Dictionary<string, object> nvc)
        {
            var jsonAsString = new StringBuilder();

            foreach (var x in nvc)
            {
                if (x.Value != null)
                {
                    jsonAsString.Append($"%22{x.Key}%22:");
                    jsonAsString.Append(x.Value is string ? $"%22{x.Value}%22," : $"{x.Value},");
                }
            }

            return $"%7B{jsonAsString.ToString().TrimEnd(',')}%7D";
        }

        public static HtmlString ArcCollectorValveHtmlString(dynamic valve, AssetType assetType,
            dynamic workOrder = null)
        {
            return new HtmlString(string.Format(ARCGIS_COLLECTOR_HTML_FORMAT_STRING,
                ArcCollectorValveLink(valve, assetType, workOrder)));
        }

        #endregion
    }
}
