using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.Text;
using System.Web;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Utility;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using Newtonsoft.Json;

namespace MapCall.CommonTest.Utility
{
    [TestClass]
    public class ArcCollectorLinkGeneratorTest
    {
        #region Constants

        public const string BASE_ESRI_URL =
            "https://utility.arcgis.com/usrsvcs/servers/efbd2ffeadff48cfb3ef76a3da02668b/rest/services/National/AW_National_OneMap_StagingEdit_SDE/FeatureServer/{0}";

        #endregion

        [TestMethod]
        public void TestArcCollectorLinkReturnsArcCollectorLinkForNameValueCollection()
        {
            var workOrder = new WorkOrder {
                OperatingCenter = new OperatingCenter(),
                Latitude = (decimal?)40.247169,
                Longitude = (decimal?)-73.992704
            };
            var nvc = new NameValueCollection {
                {"referenceContext", "center"},
                {"itemID", "15fdc279b4234fcb85f455ee70897a9e"},
                {"center", workOrder.Latitude?.ToString() + "," + workOrder.Longitude?.ToString()},
                {"scale", "3000"}
            };

            var actual = ArcCollectorLinkGenerator.ArcCollectorLink(nvc, workOrder);

            Assert.AreEqual(ArcCollectorLinkGenerator.ARCGIS_COLLECTOR_URL + "?referenceContext=center&itemID=15fdc279b4234fcb85f455ee70897a9e&center=40.247169%2c-73.992704&scale=3000",
                actual);

            workOrder.OperatingCenter.DataCollectionMapUrl = "arcgis-collector://";

            Assert.AreEqual("arcgis-collector://?referenceContext=center&itemID=15fdc279b4234fcb85f455ee70897a9e&center=40.247169%2c-73.992704&scale=3000", ArcCollectorLinkGenerator.ArcCollectorLink(nvc, workOrder));
        }

        [TestMethod]
        public void TestArcCollectorHtmlStringReturnsArcCollectorHtmlStringForAHydrantWithNoPropertiesSet()
        {
            var hydrant = new Hydrant {
                OperatingCenter = new OperatingCenter { ArcMobileMapId = "15fdc279b4234fcb85f455ee70897a9e"},
                Id = 138443,
                HydrantNumber = "HOC-652",
                HydrantBilling = new HydrantBilling {Id = 1},
                DateInstalled = new DateTime(2016, 9, 3),
                Status = new AssetStatus {Id = 6},
                HydrantManufacturer = new HydrantManufacturer {Id = 18},
                FireDistrict = new FireDistrict {PremiseNumber = "9180661205"}, //PremiseNumber
                Route = 50,
                SAPEquipmentId = 20304406,
                UpdatedAt = new DateTime(2020, 9, 1),
                UpdatedBy = new User {FullName = "Elroy Patashnik"}
            };
            var assetType = new AssetType {
                Id = AssetType.Indices.HYDRANT,
                Description = "Hydrant",
                OneMapFeatureSource = string.Format(BASE_ESRI_URL, "3")
            };

            var jsonObj = new Dictionary<string, object> {
                {"MapCall_ID", hydrant.Id.ToString()},
                {"HydrantID", hydrant.HydrantNumber},
                {"BillingType", hydrant.HydrantBilling.Id.ToString()},
                {"InstallDate", TryGetEpoch(hydrant.DateInstalled)},
                {"LifeCycleStatus", hydrant.Status.Id.ToString()},
                {"Manufacturer", hydrant.HydrantManufacturer.Id.ToString()},
                {"Premise_ID", hydrant.FireDistrict.PremiseNumber},
                {"RouteID", hydrant.Route.Value.ToString()},
                {"Sequence", (string)null},
                {"SAPID", $"0000000000{hydrant.SAPEquipmentId.Value.ToString()}"},
                {"WBS_Number", (string)null},
                {"WorkorderID", (string)null}
            };
            
            string url = $"{ArcCollectorLinkGenerator.ARCGIS_COLLECTOR_URL}" +
                         $"?itemID=15fdc279b4234fcb85f455ee70897a9e" +
                         $"&referenceContext=addFeature" +
                         $"&featureSourceURL=https://utility.arcgis.com/usrsvcs/servers/efbd2ffeadff48cfb3ef76a3da02668b/rest/services/National/AW_National_OneMap_StagingEdit_SDE/FeatureServer/3" +
                         $"&featureAttributes={ArcCollectorLinkGenerator.GetFeatureAttributes(jsonObj)}";
            string expected = $"<a href=\"{url}\" target=\"_blank\" class=\"arcgis-collector\"></a>";
            Assert.AreEqual(
                expected,
                ArcCollectorLinkGenerator.ArcCollectorHydrantHtmlString(hydrant, assetType).ToString()
            );
        }

        [TestMethod]
        public void TestArcCollectorHtmlStringReturnsArcCollectorHtmlStringForASewerOpening()
        {
            var sewerOpening = new SewerOpening {
                OperatingCenter = new OperatingCenter { ArcMobileMapId = "15fdc279b4234fcb85f455ee70897a9e"},
                Id = 8443,
                OpeningNumber = "MAD-1",
                DepthToInvert = 12.5m,
                DateInstalled = new DateTime(2016, 9, 3),
                Status = new AssetStatus {Id = 1},
                SewerOpeningMaterial = new SewerOpeningMaterial {Id = 3},
                RimElevation = 3.14m,
                Route = 66,
                SAPEquipmentId = 20304406,
                Stop = 1
            };
            var assetType = new AssetType {
                Id = AssetType.Indices.SEWER_OPENING,
                Description = "Sewer Opening",
                OneMapFeatureSource = string.Format(BASE_ESRI_URL, "13")
            };

            var jsonObj = new Dictionary<string, object> {
                {"MapCall_ID", sewerOpening.Id.ToString()},
                {"StructureID", sewerOpening.OpeningNumber},
                {"Depth", sewerOpening.DepthToInvert.ToString()},
                {"Diameter", ""},
                {"InstallDate", TryGetEpoch(sewerOpening.DateInstalled)},
                {"LifeCycleStatus", sewerOpening.Status.Id.ToString()},
                {"Material", sewerOpening.SewerOpeningMaterial.Id.ToString()},
                {"RimElevation", sewerOpening.RimElevation.ToString()},
                {"Route", sewerOpening.Route.ToString()},
                {"SAPID", $"0000000000{sewerOpening.SAPEquipmentId.Value.ToString()}"},
                {"Sequence", sewerOpening.Stop.ToString()},
                {"WBS_Number", (string)null},
                {"WorkorderID", (string)null}
            };
            
            string url = $"{ArcCollectorLinkGenerator.ARCGIS_COLLECTOR_URL}" +
                         $"?itemID=15fdc279b4234fcb85f455ee70897a9e" +
                         $"&referenceContext=addFeature" +
                         $"&featureSourceURL=https://utility.arcgis.com/usrsvcs/servers/efbd2ffeadff48cfb3ef76a3da02668b/rest/services/National/AW_National_OneMap_StagingEdit_SDE/FeatureServer/13" +
                         $"&featureAttributes={ArcCollectorLinkGenerator.GetFeatureAttributes(jsonObj)}";
            string expected = $"<a href=\"{url}\" target=\"_blank\" class=\"arcgis-collector\"></a>";
            MyAssert.StringsAreEqual(
                expected, ArcCollectorLinkGenerator.ArcCollectorSewerOpeningHtmlString(sewerOpening, assetType).ToString());
        }

        [TestMethod]
        public void TestArcCollectorLinkReturnsArcCollectorLinkForAValve()
        {
            var valve = new Valve {
                OperatingCenter = new OperatingCenter { ArcMobileMapId = "15fdc279b4234fcb85f455ee70897a9e"},
                Id = 8443,
                ValveNumber = "VAN-6",
                ValveControls = new ValveControl {Id = 14},
                ValveSize = new ValveSize {Size = 1.5m},
                DateInstalled = new DateTime(2016, 9, 3),
                Status = new AssetStatus {Id = 1},
                ValveMake = new ValveManufacturer {Id = 18},
                NormalPosition = new ValveNormalPosition {Id = 1},
                OpenDirection = new ValveOpenDirection {Id = 1},
                Route = 66,
                SAPEquipmentId = 20304406,
                Stop = 1,
                Turns = 21,
                ValveType = new ValveType {Id = 5}
            };
            var assetType = new AssetType {
                Id = AssetType.Indices.VALVE,
                Description = "Valve",
                OneMapFeatureSource = string.Format(BASE_ESRI_URL, "4")
            };

            var jsonObj = new Dictionary<string, object> {
                {"MapCall_ID", valve.Id.ToString()},
                {"ValveID", valve.ValveNumber},
                {"Application", valve.ValveControls.Id.ToString()},
                {"Diameter", valve.ValveSize.Size.ToString()},
                {"InstallDate", TryGetEpoch(valve.DateInstalled)},
                {"LifeCycleStatus", valve.Status.Id.ToString()},
                {"Manufacturer", valve.ValveMake.Id.ToString()},
                {"NormalPosition", valve.NormalPosition.Id.ToString()},
                {"OpenDirection", valve.OpenDirection.Id.ToString()},
                {"RouteID", valve.Route.ToString()},
                {"SAPID", $"0000000000{valve.SAPEquipmentId.Value.ToString()}"},
                {"Sequence", valve.Stop.ToString()},
                {"TurnsToClose", valve.Turns.Value.ToString()},
                {"ValveType", valve.ValveType.Id.ToString()},
                {"WBS_Number", null},
                {"WorkorderID", null}
            };
            
            string url = $"{ArcCollectorLinkGenerator.ARCGIS_COLLECTOR_URL}" +
                         $"?itemID=15fdc279b4234fcb85f455ee70897a9e" +
                         $"&referenceContext=addFeature" +
                         $"&featureSourceURL=https://utility.arcgis.com/usrsvcs/servers/efbd2ffeadff48cfb3ef76a3da02668b/rest/services/National/AW_National_OneMap_StagingEdit_SDE/FeatureServer/4" +
                         $"&featureAttributes={ArcCollectorLinkGenerator.GetFeatureAttributes(jsonObj)}";
            string expected = $"<a href=\"{url}\" target=\"_blank\" class=\"arcgis-collector\"></a>";

            MyAssert.StringsAreEqual(expected, ArcCollectorLinkGenerator.ArcCollectorValveHtmlString(valve, assetType).ToString()
            );
        }

        [TestMethod]
        public void TestArcCollectorHydrantLinkDoesNotErrorWhenSAPEquipmentIDIsNull()
        {
            dynamic hydrant = new ExpandoObject();
            dynamic assetType = new ExpandoObject();
            dynamic operatingCenter = new ExpandoObject();

            hydrant.HydrantID = 1;
            hydrant.HydrantNumber = "hydrantNumber";
            hydrant.HydrantBilling = null;
            hydrant.DateInstalled = null;
            hydrant.UpdatedAt = null;
            hydrant.UpdatedBy = null;
            hydrant.AssetStatus = null;
            hydrant.HydrantManufacturer = null;
            hydrant.PremiseNumber = null;
            hydrant.Route = null;
            hydrant.Stop = null;
            hydrant.SAPEquipmentID = null;
            hydrant.OperatingCenter = operatingCenter;
            operatingCenter.ArcMobileMapId = "mapId";
            assetType.OneMapFeatureSource = "oneMapFeatureSource";
            operatingCenter.DataCollectionMapUrl = "arcgis-collector://";

            MyAssert.DoesNotThrow<RuntimeBinderException>(() =>
                ArcCollectorLinkGenerator.ArcCollectorHydrantLink(hydrant, assetType));
        }

        [TestMethod]
        public void TestArcCollectorValveLinkDoesNotErrorWhenSAPEquipmentIDIsNull()
        {
            dynamic valve = new ExpandoObject();
            dynamic assetType = new ExpandoObject();
            dynamic operatingCenter = new ExpandoObject();

            valve.ValveID = 817;
            valve.UpdatedAt = null;
            valve.UpdatedBy = null;
            valve.AssetStatus = null;
            valve.ValveNumber = "VAN-6";
            valve.ValveControls = null;
            valve.ValveSize = null;
            valve.DateInstalled = null;
            valve.Status = null;
            valve.ValveMake = null;
            valve.NormalPosition = null;
            valve.OpenDirection = null;
            valve.Route = null;
            valve.Stop = null;
            valve.SAPEquipmentID = null;
            valve.Turns = null;
            valve.ValveType = null;
            operatingCenter.ArcMobileMapId = "mapId";
            valve.OperatingCenter = operatingCenter;
            assetType.OneMapFeatureSource = "oneMapFeatureSource";
            operatingCenter.DataCollectionMapUrl = "arcgis-collector://";

            MyAssert.DoesNotThrow<RuntimeBinderException>(() =>
                ArcCollectorLinkGenerator.ArcCollectorValveLink(valve, assetType));
        }

        [TestMethod]
        public void TestArcCollectorSewerOpeningLinkDoesNotErrorWhenSAPEquipmentIDIsNull()
        {
            dynamic sewerOpening = new ExpandoObject();
            dynamic assetType = new ExpandoObject();
            dynamic operatingCenter = new ExpandoObject();

            sewerOpening.Id = 817;
            sewerOpening.OpeningNumber = "MAN-6";
            sewerOpening.Depth = null;
            sewerOpening.DepthToInvert = null;
            sewerOpening.DateInstalled = null;
            sewerOpening.UpdatedAt = null;
            sewerOpening.UpdatedBy = null;
            sewerOpening.Status = null;
            sewerOpening.AssetStatus = null;
            sewerOpening.SewerOpeningMaterial = null;
            sewerOpening.RimElevation = null;
            sewerOpening.Route = null;
            sewerOpening.SAPEquipmentID = null;
            sewerOpening.Stop = null;
            sewerOpening.AccountCharged = null;
            sewerOpening.WorkOrderID = null;
            sewerOpening.ValveType = null;
            operatingCenter.ArcMobileMapId = "mapId";
            sewerOpening.OperatingCenter = operatingCenter;
            assetType.OneMapFeatureSource = "oneMapFeatureSource";
            operatingCenter.DataCollectionMapUrl = "arcgis-collector://";

            MyAssert.DoesNotThrow<RuntimeBinderException>(() =>
                ArcCollectorLinkGenerator.ArcCollectorSewerOpeningLink(sewerOpening, assetType));
        }
        
        private double? TryGetEpoch(DateTime? dateTime)
        {
            return dateTime?.MillisecondsSinceEpoch();
        }
    }
}

