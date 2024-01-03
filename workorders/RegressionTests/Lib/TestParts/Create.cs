using MapCall.Common.Testing.Selenium;
using MMSINC.Testing.Selenium;
using System;
using System.Text.RegularExpressions;

namespace RegressionTests.Lib.TestParts.Create
{
    public static class Markout
    {
        public struct NecessaryIDs
        {
            public static readonly string TAB_MARKOUTS = "//a[@id='markoutsTab']/span";
            public static readonly string
                DDL_INSERT_MARKOUT_TYPE = "content_cphMain_cphMain_woprv_wodvWorkOrder_fvWorkOrder_ctl02_gvMarkouts_ddlMarkoutType",
                LNK_INSERT = "content_cphMain_cphMain_woprv_wodvWorkOrder_fvWorkOrder_ctl02_gvMarkouts_lbInsert",
                TXT_INSERT_MARKOUT_NUMBER = "content_cphMain_cphMain_woprv_wodvWorkOrder_fvWorkOrder_ctl02_gvMarkouts_txtMarkoutNumber",
                TXT_INSERT_DATE_REQUEST = "content_cphMain_cphMain_woprv_wodvWorkOrder_fvWorkOrder_ctl02_gvMarkouts_ccDateOfRequest",
                TXT_INSERT_EXPIRATION_DATE = "content_cphMain_cphMain_woprv_wodvWorkOrder_fvWorkOrder_ctl02_gvMarkouts_ccMarkoutExpirationDate",
                TXT_INSERT_READY_DATE = "content_cphMain_cphMain_woprv_wodvWorkOrder_fvWorkOrder_ctl02_gvMarkouts_ccMarkoutReadyDate",

                DDL_EDIT_MARKOUT_TYPE = "content_cphMain_cphMain_woprv_wodvWorkOrder_fvWorkOrder_ctl02_gvMarkouts_ddlMarkoutTypeEdit_0",
                LNK_DELETE = "content_cphMain_cphMain_woprv_wodvWorkOrder_fvWorkOrder_ctl02_gvMarkouts_lbDelete_0",
                LNK_EDIT = "content_cphMain_cphMain_woprv_wodvWorkOrder_fvWorkOrder_ctl02_gvMarkouts_lbEdit_0",
                LNK_UPDATE = "content_cphMain_cphMain_woprv_wodvWorkOrder_fvWorkOrder_ctl02_gvMarkouts_lbSave_0",
                LNK_CANCEL = "content_cphMain_cphMain_woprv_wodvWorkOrder_fvWorkOrder_ctl02_gvMarkouts_lbCancel_0",
                TXT_EDIT_MARKOUT_NUMBER = "content_cphMain_cphMain_woprv_wodvWorkOrder_fvWorkOrder_ctl02_gvMarkouts_txtMarkoutNumberEdit_0",
                TXT_EDIT_DATE_REQUEST = "content_cphMain_cphMain_woprv_wodvWorkOrder_fvWorkOrder_ctl02_gvMarkouts_ccDateOfRequestEdit_0",
                TXT_EDIT_EXPIRATION_DATE = "content_cphMain_cphMain_woprv_wodvWorkOrder_fvWorkOrder_ctl02_gvMarkouts_ccMarkoutReadyDateEdit_0",
                TXT_EDIT_READY_DATE = "content_cphMain_cphMain_woprv_wodvWorkOrder_fvWorkOrder_ctl02_gvMarkouts_ccMarkoutExpirationDateEdit_0";
        }

        public static Types.Markout WithDateOfRequest(IExtendedSelenium selenium, Types.WorkOrder workOrder, string dateOfRequest)
        {
            var markout = new Types.Markout
            {
                MarkoutNumber = "1234",
                MarkoutType = "C TO C",
                DateOfRequest = dateOfRequest
            };

            DoCreateMarkout(selenium, workOrder, markout);
            return markout;
        }
        
        private static void DoCreateMarkout(IExtendedSelenium selenium, Types.WorkOrder order, Types.Markout markout)
        {
            // if !MarkoutEditable
            // verify no ready date control in foooter
            // verify no expiration date control in footer

            selenium.ClickAndWaitForPageToLoad(Verify.NecessaryIDs.Planning.LNK_SELECT);
            selenium.Click(NecessaryIDs.TAB_MARKOUTS);

            // MarkoutNumber - fillout markout insert footer 
            selenium.Type(NecessaryIDs.TXT_INSERT_MARKOUT_NUMBER, markout.MarkoutNumber);
            // MarkoutType - fillout markout insert footer 
            selenium.SelectLabel(NecessaryIDs.DDL_INSERT_MARKOUT_TYPE, markout.MarkoutType);
            // DateOfRequest - fillout markout insert footer 
            selenium.RunScript("$('#{0}').val('{1}');", NecessaryIDs.TXT_INSERT_DATE_REQUEST, markout.DateOfRequest);
            // ExpirationDate
            if (!string.IsNullOrWhiteSpace(markout.ExpirationDate)) selenium.RunScript("$('#{0}').val('{1}');", NecessaryIDs.TXT_INSERT_EXPIRATION_DATE, markout.ExpirationDate);
            // ReadyDate
            if (!string.IsNullOrWhiteSpace(markout.ReadyDate)) selenium.RunScript("$('#{0}').val('{1}');", NecessaryIDs.TXT_INSERT_READY_DATE, markout.ReadyDate);
            // insert
            selenium.ClickAndWait("content_cphMain_cphMain_woprv_wodvWorkOrder_fvWorkOrder_ctl02_gvMarkouts_lbInsert", 10);
            
            // verify
            selenium.Click(NecessaryIDs.TAB_MARKOUTS);
            selenium.AssertTextPresent(markout.MarkoutNumber);
            selenium.AssertTextPresent(markout.MarkoutType);
            selenium.AssertTextPresent(markout.DateOfRequest);
            // verify no ready date control in foooter
            selenium.AssertElementIsNotPresent(NecessaryIDs.TXT_INSERT_READY_DATE);
            // verify no expiration date control in footer
            selenium.AssertElementIsNotPresent(NecessaryIDs.TXT_INSERT_EXPIRATION_DATE);

            // click edit markout
            selenium.ClickAndWait(NecessaryIDs.LNK_EDIT,5);
            // verify no ready date control in foooter
            selenium.AssertElementIsNotPresent(NecessaryIDs.TXT_INSERT_READY_DATE);
            // verify no expiration date control in footer
            selenium.AssertElementIsNotPresent(NecessaryIDs.TXT_INSERT_EXPIRATION_DATE);
            // verify no ready date control in edit row
            selenium.AssertElementIsNotPresent(NecessaryIDs.TXT_EDIT_READY_DATE);
            // verify no expiration date control in edit row
            selenium.AssertElementIsNotPresent(NecessaryIDs.TXT_EDIT_EXPIRATION_DATE);

            // MarkoutNumber - fillout markout insert footer 
            selenium.Type(NecessaryIDs.TXT_EDIT_MARKOUT_NUMBER, markout.MarkoutNumber + "1");
            // MarkoutType - fillout markout insert footer 
            selenium.SelectLabel(NecessaryIDs.DDL_EDIT_MARKOUT_TYPE, markout.MarkoutType + ", 25FT RADIUS OF HYDRANT");
            // DateOfRequest - fillout markout insert footer 
            selenium.RunScript("$('#{0}').val('{1}');", NecessaryIDs.TXT_EDIT_DATE_REQUEST, "1/1/2017");
            // Update/Save
            selenium.ClickAndWait(NecessaryIDs.LNK_UPDATE, 6);

            // verify values
            selenium.AssertTextPresent(markout.MarkoutNumber + "1");
            selenium.AssertTextPresent(markout.MarkoutType + ", 25FT RADIUS OF HYDRANT");
            selenium.AssertTextPresent("1/1/2017");

            // verify no ready date control in foooter
            selenium.AssertElementIsNotPresent(NecessaryIDs.TXT_INSERT_READY_DATE);
            // verify no expiration date control in footer
            selenium.AssertElementIsNotPresent(NecessaryIDs.TXT_INSERT_EXPIRATION_DATE);

            // click edit markout
            selenium.ClickAndWait(NecessaryIDs.LNK_EDIT, 3);
            // verify no ready date control in foooter
            selenium.AssertElementIsNotPresent(NecessaryIDs.TXT_INSERT_READY_DATE);
            // verify no expiration date control in footer
            selenium.AssertElementIsNotPresent(NecessaryIDs.TXT_INSERT_EXPIRATION_DATE);
            // verify no ready date control in edit row
            selenium.AssertElementIsNotPresent(NecessaryIDs.TXT_EDIT_READY_DATE);
            // verify no expiration date control in edit row
            selenium.AssertElementIsNotPresent(NecessaryIDs.TXT_EDIT_EXPIRATION_DATE);
            selenium.ClickAndWait(NecessaryIDs.LNK_CANCEL, 3);

            // delete
            //selenium.ChooseOkOnNextConfirmation();
            selenium.ClickAndWait(NecessaryIDs.LNK_DELETE, 3);
            selenium.AssertConfirmation("Are you sure?");

            // verify no ready date control in foooter
            selenium.AssertElementIsNotPresent(NecessaryIDs.TXT_INSERT_READY_DATE);
            // verify no expiration date control in footer
            selenium.AssertElementIsNotPresent(NecessaryIDs.TXT_INSERT_EXPIRATION_DATE);

            //Shelbyville - Automatically generating and overriding the entered dates for Expiration/Ready
            //https://qa.mapcall.info/Modules/WorkOrders/Views/WorkOrders/General/WorkOrderGeneralResourceRPCPage.aspx?cmd=update&arg=292815
            //PA - 
            //https://qa.mapcall.info/Modules/WorkOrders/Views/WorkOrders/General/WorkOrderGeneralResourceRPCPage.aspx?cmd=update&arg=281165
        }


        private static void DoCreateMarkoutWithExpirationAndReady(IExtendedSelenium selenium, Types.WorkOrder order, Types.Markout markout)
        {
            // if !MarkoutEditable
            // verify no ready date control in foooter
            // verify no expiration date control in footer

            selenium.ClickAndWaitForPageToLoad(Verify.NecessaryIDs.Planning.LNK_SELECT);
            selenium.Click(NecessaryIDs.TAB_MARKOUTS);

            // MarkoutNumber - fillout markout insert footer 
            selenium.Type(NecessaryIDs.TXT_INSERT_MARKOUT_NUMBER, markout.MarkoutNumber);
            // MarkoutType - fillout markout insert footer 
            selenium.SelectLabel(NecessaryIDs.DDL_INSERT_MARKOUT_TYPE, markout.MarkoutType);

            // test date alerts
            selenium.RunScript("$('#{0}').val('{1}');", NecessaryIDs.TXT_INSERT_DATE_REQUEST, DateTime.Now.ToShortDateString());
            selenium.RunScript("$('#{0}').val('{1}');", NecessaryIDs.TXT_INSERT_EXPIRATION_DATE, DateTime.Now.ToShortDateString());
            selenium.RunScript("$('#{0}').val('{1}');", NecessaryIDs.TXT_INSERT_READY_DATE, DateTime.Now.AddDays(-1).ToShortDateString());
            selenium.ClickAndWait("content_cphMain_cphMain_woprv_wodvWorkOrder_fvWorkOrder_ctl02_gvMarkouts_lbInsert", 10);
            selenium.AssertAlert("Request Date cannot be after the Ready Date.");

            selenium.RunScript("$('#{0}').val('{1}');", NecessaryIDs.TXT_INSERT_DATE_REQUEST, DateTime.Now.ToShortDateString());
            selenium.RunScript("$('#{0}').val('{1}');", NecessaryIDs.TXT_INSERT_EXPIRATION_DATE, DateTime.Now.AddDays(-1).ToShortDateString());
            selenium.RunScript("$('#{0}').val('{1}');", NecessaryIDs.TXT_INSERT_READY_DATE, DateTime.Now.ToShortDateString());
            selenium.ClickAndWait("content_cphMain_cphMain_woprv_wodvWorkOrder_fvWorkOrder_ctl02_gvMarkouts_lbInsert", 10);
            selenium.AssertAlert("Ready Date cannot be after the Expiration Date.");

            // DateOfRequest - fillout markout insert footer 
            selenium.RunScript("$('#{0}').val('{1}');", NecessaryIDs.TXT_INSERT_DATE_REQUEST, markout.DateOfRequest);
            // ExpirationDate
            selenium.RunScript("$('#{0}').val('{1}');", NecessaryIDs.TXT_INSERT_EXPIRATION_DATE, markout.ExpirationDate);
            // ReadyDate
            selenium.RunScript("$('#{0}').val('{1}');", NecessaryIDs.TXT_INSERT_READY_DATE, markout.ReadyDate);
            // insert
            selenium.ClickAndWait("content_cphMain_cphMain_woprv_wodvWorkOrder_fvWorkOrder_ctl02_gvMarkouts_lbInsert", 10);

            // verify
            selenium.Click(NecessaryIDs.TAB_MARKOUTS);
            selenium.AssertTextPresent(markout.MarkoutNumber);
            selenium.AssertTextPresent(markout.MarkoutType);
            selenium.AssertTextPresent(markout.DateOfRequest);
            selenium.AssertTextPresent(markout.ExpirationDate);
            selenium.AssertTextPresent(markout.ReadyDate);
            // verify no ready date control in foooter
            selenium.AssertElementPresent(NecessaryIDs.TXT_INSERT_READY_DATE);
            // verify no expiration date control in footer
            selenium.AssertElementPresent(NecessaryIDs.TXT_INSERT_EXPIRATION_DATE);

            // click edit markout
            selenium.ClickAndWait(NecessaryIDs.LNK_EDIT, 3);
            // verify no ready date control in foooter
            selenium.AssertElementPresent(NecessaryIDs.TXT_INSERT_READY_DATE);
            // verify no expiration date control in footer
            selenium.AssertElementPresent(NecessaryIDs.TXT_INSERT_EXPIRATION_DATE);
            // verify no ready date control in edit row
            selenium.AssertElementPresent(NecessaryIDs.TXT_EDIT_READY_DATE);
            // verify no expiration date control in edit row
            selenium.AssertElementPresent(NecessaryIDs.TXT_EDIT_EXPIRATION_DATE);

            // MarkoutNumber - fillout markout insert footer 
            selenium.Type(NecessaryIDs.TXT_EDIT_MARKOUT_NUMBER, markout.MarkoutNumber + "1");
            // MarkoutType - fillout markout insert footer 
            selenium.SelectLabel(NecessaryIDs.DDL_EDIT_MARKOUT_TYPE, markout.MarkoutType + ", 25FT RADIUS OF HYDRANT");
            // DateOfRequest - fillout markout insert footer 
            selenium.RunScript("$('#{0}').val('{1}');", NecessaryIDs.TXT_EDIT_DATE_REQUEST, "1/1/2017");
            // Update/Save
            selenium.ClickAndWait(NecessaryIDs.LNK_UPDATE, 3);

            // verify values
            selenium.AssertTextPresent(markout.MarkoutNumber + "1");
            selenium.AssertTextPresent(markout.MarkoutType + ", 25FT RADIUS OF HYDRANT");
            selenium.AssertTextPresent("1/1/2017");

            // verify no ready date control in foooter
            selenium.AssertElementPresent(NecessaryIDs.TXT_INSERT_READY_DATE);
            // verify no expiration date control in footer
            selenium.AssertElementPresent(NecessaryIDs.TXT_INSERT_EXPIRATION_DATE);

            // click edit markout
            selenium.ClickAndWait(NecessaryIDs.LNK_EDIT, 3);
            // verify no ready date control in foooter
            selenium.AssertElementPresent(NecessaryIDs.TXT_INSERT_READY_DATE);
            // verify no expiration date control in footer
            selenium.AssertElementPresent(NecessaryIDs.TXT_INSERT_EXPIRATION_DATE);
            // verify no ready date control in edit row
            selenium.AssertElementPresent(NecessaryIDs.TXT_EDIT_READY_DATE);
            // verify no expiration date control in edit row
            selenium.AssertElementPresent(NecessaryIDs.TXT_EDIT_EXPIRATION_DATE);
            selenium.ClickAndWait(NecessaryIDs.LNK_CANCEL, 3);

            // delete
            selenium.ClickAndWait(NecessaryIDs.LNK_DELETE, 3);
            selenium.AssertConfirmation("Are you sure?");

            // verify no ready date control in foooter
            selenium.AssertElementPresent(NecessaryIDs.TXT_INSERT_READY_DATE);
            // verify no expiration date control in footer
            selenium.AssertElementPresent(NecessaryIDs.TXT_INSERT_EXPIRATION_DATE);
        }

        public static object WithAllDates(IExtendedSelenium selenium, Types.WorkOrder order, DateTime dateOfRequest, DateTime readyDate, DateTime expirationDate)
        {
            var markout = new Types.Markout
            {
                MarkoutNumber = "AA1234",
                MarkoutType = "C TO C",
                DateOfRequest = dateOfRequest.ToString(),
                ReadyDate = readyDate.ToShortDateString(),
                ExpirationDate = expirationDate.ToShortDateString()
            };

            DoCreateMarkoutWithExpirationAndReady(selenium, order, markout);
            return markout;
        }
    }

    public static class WorkOrder
    {
        #region Constants

        public struct NecessaryIDs
        {
            public static readonly string DDL_OP_CENTER =
                Global.CONTROL_BASE_ID +
                "woirvParaDesWorkOrdersOI_wodvWorkOrder_fvWorkOrder_fvWorkOrder_ddlOperatingCenter",
                DDL_TOWN =
                    Global.CONTROL_BASE_ID +
                    "woirvParaDesWorkOrdersOI_wodvWorkOrder_fvWorkOrder_fvWorkOrder_ddlTown",
                DDL_TOWN_SECTION =
                    Global.CONTROL_BASE_ID +
                    "woirvParaDesWorkOrdersOI_wodvWorkOrder_fvWorkOrder_fvWorkOrder_ddlTownSection",
                DDL_STREET =
                    Global.CONTROL_BASE_ID +
                    "woirvParaDesWorkOrdersOI_wodvWorkOrder_fvWorkOrder_fvWorkOrder_ddlStreet",
                DDL_NEAREST_CROSS_STREET =
                    Global.CONTROL_BASE_ID +
                    "woirvParaDesWorkOrdersOI_wodvWorkOrder_fvWorkOrder_fvWorkOrder_ddlNearestCrossStreet",
                DDL_ASSET_TYPE =
                    Global.CONTROL_BASE_ID +
                    "woirvParaDesWorkOrdersOI_wodvWorkOrder_fvWorkOrder_fvWorkOrder_ddlAssetType",
                DDL_REQUESTED_BY =
                    Global.CONTROL_BASE_ID +
                    "woirvParaDesWorkOrdersOI_wodvWorkOrder_fvWorkOrder_fvWorkOrder_ddlRequestedBy",
                DDL_DRIVEN_BY =
                    Global.CONTROL_BASE_ID +
                    "woirvParaDesWorkOrdersOI_wodvWorkOrder_fvWorkOrder_fvWorkOrder_ddlDrivenBy",
                DDL_PRIORITY =
                    Global.CONTROL_BASE_ID +
                    "woirvParaDesWorkOrdersOI_wodvWorkOrder_fvWorkOrder_fvWorkOrder_ddlPriority",
                DDL_DESCRIPTION_OF_WORK =
                    Global.CONTROL_BASE_ID +
                    "woirvParaDesWorkOrdersOI_wodvWorkOrder_fvWorkOrder_fvWorkOrder_ddlDescriptionOfWork",
                DDL_MARKOUT_REQUIREMENT =
                    Global.CONTROL_BASE_ID +
                    "woirvParaDesWorkOrdersOI_wodvWorkOrder_fvWorkOrder_fvWorkOrder_ddlMarkoutRequirement",
                DDL_VALVE =
                    Global.CONTROL_BASE_ID +
                    "woirvParaDesWorkOrdersOI_wodvWorkOrder_fvWorkOrder_fvWorkOrder_ddlValve",
                DDL_HYDRANT =
                    Global.CONTROL_BASE_ID +
                    "woirvParaDesWorkOrdersOI_wodvWorkOrder_fvWorkOrder_fvWorkOrder_ddlHydrant",
                DDL_EQUIPMENT =
                    Global.CONTROL_BASE_ID +
                    "woirvParaDesWorkOrdersOI_wodvWorkOrder_fvWorkOrder_fvWorkOrder_ddlEquipment",
                DDL_SEWER_OPENING =
                    Global.CONTROL_BASE_ID +
                    "woirvParaDesWorkOrdersOI_wodvWorkOrder_fvWorkOrder_fvWorkOrder_ddlSewerOpening",
                DDL_CUSTOMER_IMPACT_RANGE =
                    Global.CONTROL_BASE_ID +
                    "woirvParaDesWorkOrdersOI_wodvWorkOrder_fvWorkOrder_fvWorkOrder_ddlCustomerImpactRange",
                DDL_REPAIR_TIME_RANGE =
                    Global.CONTROL_BASE_ID +
                    "woirvParaDesWorkOrdersOI_wodvWorkOrder_fvWorkOrder_fvWorkOrder_ddlRepairTimeRange",
                DDL_ALERT_ISSUED =
                    Global.CONTROL_BASE_ID +
                    "woirvParaDesWorkOrdersOI_wodvWorkOrder_fvWorkOrder_fvWorkOrder_ddlAlertIssued",
                DDL_SIGNIFICANT_TRAFFIC_IMPACT =
                    Global.CONTROL_BASE_ID +
                    "woirvParaDesWorkOrdersOI_wodvWorkOrder_fvWorkOrder_fvWorkOrder_ddlSignificantTrafficImpact";

            public static readonly string TXT_STREET_NUMBER =
                Global.CONTROL_BASE_ID +
                "woirvParaDesWorkOrdersOI_wodvWorkOrder_fvWorkOrder_fvWorkOrder_txtStreetNumber",
                TXT_SUPPLEMENT_NO = Global.CONTROL_BASE_ID + "woirvParaDesWorkOrdersOI_wodvWorkOrder_fvWorkOrder_fvWorkOrder_txtApartmentAddtl",
                TXT_ZIP_CODE =
                    Global.CONTROL_BASE_ID +
                    "woirvParaDesWorkOrdersOI_wodvWorkOrder_fvWorkOrder_fvWorkOrder_txtZipCode",
                TXT_LOCATION = "txtLocation",
                TXT_NOTES =
                    Global.CONTROL_BASE_ID +
                    "woirvParaDesWorkOrdersOI_wodvWorkOrder_fvWorkOrder_fvWorkOrder_txtNotes",
                TXT_PREMISE_NUMBER =
                    Global.CONTROL_BASE_ID +
                    "woirvParaDesWorkOrdersOI_wodvWorkOrder_fvWorkOrder_fvWorkOrder_txtPremiseNumber",
                TXT_SERVICE_NUMBER =
                    Global.CONTROL_BASE_ID +
                    "woirvParaDesWorkOrdersOI_wodvWorkOrder_fvWorkOrder_fvWorkOrder_txtServiceNumber",
                TXT_ORIGINAL_ORDER_NUMBER =
                    Global.CONTROL_BASE_ID +
                    "woirvParaDesWorkOrdersOI_wodvWorkOrder_fvWorkOrder_fvWorkOrder_txtOriginalOrderNumber",
                TXT_SAP_NOTIFICATION_NUMBER =
                    Global.CONTROL_BASE_ID +
                    "woirvParaDesWorkOrdersOI_wodvWorkOrder_fvWorkOrder_fvWorkOrder_txtSAPNotificationNumber",
                TXT_SAP_WORK_ORDER_NUMBER = 
                    Global.CONTROL_BASE_ID +
                    "woirvParaDesWorkOrdersOI_wodvWorkOrder_fvWorkOrder_fvWorkOrder_txtSAPWorkOrderNumber";


            public static readonly string IMG_SHOW_PICKER =
                Global.CONTROL_BASE_ID + "woirvParaDesWorkOrdersOI_wodvWorkOrder_fvWorkOrder_fvWorkOrder_llpAsset_imgShowPicker";

            public static readonly string BTN_GEO_CODE = "btnGeoCode",
                                          BTN_SAVE_COORDINATES = "btnSave",
                                          BTN_SAVE =
                                              Global.CONTROL_BASE_ID + "woirvParaDesWorkOrdersOI_wodvWorkOrder_fvWorkOrder_btnSave",
                                          BTN_CLOSE_PICKER = "btnClose";

            public static readonly string LBL_DATE_RECEIVED =
                                              Global.CONTROL_BASE_ID + "woirvParaDesWorkOrdersOI_wodvWorkOrder_fvWorkOrder_fvWorkOrder_lblDateReceived",
                                          LBL_WORK_ORDER_ID =
                                              Global.CONTROL_BASE_ID + "woirvParaDesWorkOrdersOI_wodvWorkOrder_fvWorkOrder_fvWorkOrder_lblWorkOrderID";

            public static readonly string RDO_REVISIT_REVISIT =
                Global.CONTROL_BASE_ID + "woirvParaDesWorkOrdersOI_wodvWorkOrder_fvWorkOrder_fvWorkOrder_rdoRevisitRevisit";

            public const string CHK_SOP_REQUIRED =
                Global.CONTROL_BASE_ID + "woirvParaDesWorkOrdersOI_wodvWorkOrder_fvWorkOrder_fvWorkOrder_chkStreetOpeningPermitRequired";

        }

        #endregion

        #region Public Static Methods

        public static Types.WorkOrder WithMainAsset(IExtendedSelenium selenium, string createdBy)
        {
            var order = new Types.WorkOrder {
                OperatingCenter = "NJ7 - Shrewsbury",
                OperatingCenterName = "Shrewsbury",
                Town = "OCEAN",
                TownSection = "WANAMASSA",
                StreetNumber = "16",
                ApartmentAddtl = "Apt A",
                Street = "ABBEY LN",
                NearestCrossStreet = "OVERHILL RD",
                ZipCode = "07712",
                AssetType = "Main",
                RequestedBy = "Local Government",
                Purpose = "Customer",
                Priority = "Routine",
                DescriptionOfWork = "WATER MAIN BLEEDERS",
                MarkoutRequirement = "None",
                CreatedBy = createdBy,
                Notes = "Test Notes"
            };
            DoCreateOrder(selenium, order);
            VerifyCreatedOrder(selenium, ref order);
            return order;
        }

        public static Types.WorkOrder WithMainAssetAsEmergency(IExtendedSelenium selenium, string createdBy)
        {
            var order = new Types.WorkOrder {
                OperatingCenter = "NJ7 - Shrewsbury",
                Town = "OCEAN",
                TownSection = "WANAMASSA",
                StreetNumber = "16",
                ApartmentAddtl = "Apt A",
                Street = "ABBEY LN",
                NearestCrossStreet = "OVERHILL RD",
                ZipCode = "07712",
                AssetType = "Main",
                RequestedBy = "Local Government",
                Purpose = "Customer",
                Priority = "Emergency",
                DescriptionOfWork = "GROUND WATER-MAIN",
                MarkoutRequirement = "None",
                CreatedBy = createdBy
            };
            DoCreateOrder(selenium, order);
            VerifyCreatedOrder(selenium, ref order);
            return order;
        }

        public static Types.WorkOrder WithMainAssetForMainBreak(IExtendedSelenium selenium, string createdBy, bool alertIssued = true)
        {
            var order = new Types.WorkOrder {
                OperatingCenter = "NJ7 - Shrewsbury",
                OperatingCenterName = "Shrewsbury",
                Town = "OCEAN",
                TownSection = "WANAMASSA",
                StreetNumber = "16",
                ApartmentAddtl = "Apt A",
                Street = "ABBEY LN",
                NearestCrossStreet = "OVERHILL RD",
                ZipCode = "07712",
                AssetType = "Main",
                RequestedBy = "Local Government",
                Purpose = "Customer",
                Priority = "Routine",
                DescriptionOfWork = "WATER MAIN BREAK REPAIR",
                CustomerImpactRange = "0-50",
                RepairTimeRange = "10-12",
                AlertIssued = (alertIssued) ? "Yes" : "No",
                SignificantTrafficImpact = "No",
                MarkoutRequirement = "None",
                CreatedBy = createdBy,
                SAPWorkOrderNumber = "123123123",
                SAPNotificationNumber = "123321321",
                Notes = "these are initial notes"
            };
            DoCreateOrder(selenium, order);
            VerifyCreatedOrder(selenium, ref order);
            return order;
        }

        public static Types.WorkOrder WithValveAsset(IExtendedSelenium selenium, string createdBy)
        {
            var order = new Types.WorkOrder {
                OperatingCenter = "NJ7 - Shrewsbury",
                OperatingCenterName = "Shrewsbury",
                Town = "OCEAN",
                TownSection = "WANAMASSA",
                StreetNumber = "16",
                ApartmentAddtl = "Apt A",
                Street = "ABBEY LN",
                NearestCrossStreet = "OVERHILL RD",
                ZipCode = "07712",
                AssetType = "Valve",
                ValveID = "870 - ACTIVE",
                RequestedBy = "Local Government",
                Purpose = "Customer",
                Priority = "Routine",
                DescriptionOfWork = "VALVE BLOW OFF REPLACEMENT",
                MarkoutRequirement = "None",
                CreatedBy = createdBy
            };
            DoCreateOrder(selenium, order);
            VerifyCreatedOrder(selenium, ref order);
            return order;
        }

        public static Types.WorkOrder WithHydrantAsset(IExtendedSelenium selenium, string createdBy)
        {
            var order = new Types.WorkOrder
            {
                OperatingCenter = "NJ7 - Shrewsbury",
                OperatingCenterName = "Shrewsbury",
                Town = "OCEAN",
                TownSection = "WANAMASSA",
                StreetNumber = "16",
                ApartmentAddtl = "Apt A",
                Street = "ABBEY LN",
                NearestCrossStreet = "OVERHILL RD",
                ZipCode = "07712",
                AssetType = "Hydrant",
                HydrantID = "301 - ACTIVE",
                RequestedBy = "Local Government",
                Purpose = "Customer",
                Priority = "Routine",
                DescriptionOfWork = "HYDRANT FROZEN",
                MarkoutRequirement = "None",
                CreatedBy = createdBy
            };
            DoCreateOrder(selenium, order);
            VerifyCreatedOrder(selenium, ref order);
            return order;
        }

        public static Types.WorkOrder WithSewerOpeningAsset(IExtendedSelenium selenium, string createdBy)
        {
            var order = new Types.WorkOrder {
                OperatingCenter = "NJ4 - Howell",
                Town = "HOWELL TWP",
                TownSection = "FREEWOOD ACRES",
                StreetNumber = "123",
                ApartmentAddtl = "Apt A",
                Street = "CITATION ST",
                NearestCrossStreet = "1ST ST",
                AssetType = "Sewer Opening",
                SewerOpeningID = "MHW-1 - ACTIVE",
                RequestedBy = "Local Government",
                Purpose = "Customer",
                Priority = "Routine",
                DescriptionOfWork = "SEWER OPENING INSTALLATION",
                MarkoutRequirement = "None",
                CreatedBy = createdBy
            };
            DoCreateOrder(selenium, order);
            VerifyCreatedOrder(selenium, ref order);
            return order;
        }

        public static Types.WorkOrder WithEquipmentAsset(IExtendedSelenium selenium, string createdBy)
        {
            var order = new Types.WorkOrder {
                OperatingCenter = "NJ4 - Howell",
                Town = "LAKEWOOD",
                StreetNumber = "123",
                ApartmentAddtl = "Apt A",
                Street = "10TH ST",
                NearestCrossStreet = "1ST ST",
                AssetType = "Equipment",
                EquipmentID = "NJLK-7-SBLG-1",
                RequestedBy = "Local Government",
                Purpose = "Customer",
                Priority = "Routine",
                DescriptionOfWork = "Misc repair",
                MarkoutRequirement = "None",
                CreatedBy = createdBy
            };
            //DoCreateOrder(selenium, order);
            //VerifyCreatedOrder(selenium,ref order);

            // Create the order with raw sql and set work order id 
            order.WorkOrderID = Data.CreateOrderForEquipment(
                operatingCenter: 14, town: 189, townSection: 137,
                streetNumber: "123", street: 30883, crossStreet: 28920,
                assetType: 9, equipment: 202, requestedBy: 3, purpose: 1,
                priority: 4, workdescription: 207, orcom: "0123456789",
                markoutRequirement: 1, createdBy: 402);

            return order;
        }

        public static Types.WorkOrder WithServiceAsset(IExtendedSelenium selenium, string createdBy, string descriptionOfWork = "CURB BOX REPAIR")
        {
            var order = new Types.WorkOrder {
                OperatingCenter = "NJ7 - Shrewsbury",
                OperatingCenterName = "Shrewsbury",
                Town = "OCEAN",
                TownSection = "WANAMASSA",
                StreetNumber = "16",
                ApartmentAddtl = "Apt A",
                Street = "ABBEY LN",
                NearestCrossStreet = "OVERHILL RD",
                ZipCode = "07712",
                AssetType = "Service",
                PremiseNumber = "9180458651",
                ServiceNumber = "12345678",
                RequestedBy = "Local Government",
                Purpose = "Customer",
                Priority = "Routine",
                DescriptionOfWork = descriptionOfWork,
                MarkoutRequirement = "None",
                CreatedBy = createdBy
            };
            DoCreateOrder(selenium, order);
            VerifyCreatedOrder(selenium, ref order);
            return order;
        }
        public static Types.WorkOrder WithServiceAssetWithPremiseLinkedToSampleSite(IExtendedSelenium selenium, string createdBy, string descriptionOfWork = "CURB BOX REPAIR")
        {
            var order = new Types.WorkOrder {
                OperatingCenter = "NJ7 - Shrewsbury",
                OperatingCenterName = "Shrewsbury",
                Town = "OCEAN",
                TownSection = "WANAMASSA",
                StreetNumber = "16",
                ApartmentAddtl = "Apt A",
                Street = "ABBEY LN",
                NearestCrossStreet = "OVERHILL RD",
                ZipCode = "07712",
                AssetType = "Service",
                PremiseNumber = "9180620291",
                ServiceNumber = "12345678",
                RequestedBy = "Local Government",
                Purpose = "Customer",
                Priority = "Routine",
                DescriptionOfWork = descriptionOfWork,
                MarkoutRequirement = "None",
                CreatedBy = createdBy
            };
            DoCreateOrder(selenium, order);
            VerifyCreatedOrder(selenium, ref order);
            return order;
        }

        public static Types.WorkOrder WithServiceAssetWithoutTownSection(IExtendedSelenium selenium, string createdBy)
        {
            var order = new Types.WorkOrder
            {
                OperatingCenter = "NJ7 - Shrewsbury",
                Town = "OCEAN",
                StreetNumber = "16",
                ApartmentAddtl = "Apt A",
                Street = "ABBEY LN",
                NearestCrossStreet = "OVERHILL RD",
                ZipCode = "07712",
                AssetType = "Service",
                PremiseNumber = "9180458651",
                ServiceNumber = "12345678",
                RequestedBy = "Local Government",
                Purpose = "Customer",
                Priority = "Routine",
                DescriptionOfWork = "CURB BOX REPAIR",
                MarkoutRequirement = "None",
                CreatedBy = createdBy
            };
            DoCreateOrder(selenium, order);
            VerifyCreatedOrder(selenium, ref order);
            return order;
        }

        public static Types.WorkOrder WithServiceAssetInTownWithNoTownSections(IExtendedSelenium selenium, string createdBy)
        {
            var order = new Types.WorkOrder {
                OperatingCenter = "NJ7 - Shrewsbury",
                Town = "LOCH ARBOUR",
                StreetNumber = "148",
                ApartmentAddtl = "Apt A",
                Street = "EDGEMONT DR",
                NearestCrossStreet = "PAGE AVE",
                ZipCode = "07701",
                AssetType = "Service",
                PremiseNumber = "9180458651",
                ServiceNumber = "12345678",
                RequestedBy = "Local Government",
                Purpose = "Customer",
                Priority = "Routine",
                DescriptionOfWork = "CURB BOX REPAIR",
                MarkoutRequirement = "None",
                CreatedBy = createdBy
            };
            DoCreateOrder(selenium, order);
            VerifyCreatedOrder(selenium, ref order);
            return order;
        }

        public static Types.WorkOrder ForRevisitWithServiceAssetWithoutTownSection(IExtendedSelenium selenium, string @by, Types.WorkOrder order)
        {
            order.DescriptionOfWork = "SERVICE LANDSCAPING";

            DoRevisitOrder(selenium, order);
            VerifyCreatedOrder(selenium, ref order);
            return order;
        }

        public static Types.WorkOrder ForRevisitWithServiceAssetInTownWithNoTownSections(IExtendedSelenium selenium, string @by, Types.WorkOrder order)
        {
            order.DescriptionOfWork = "SERVICE LANDSCAPING";

            DoRevisitOrder(selenium, order);
            VerifyCreatedOrder(selenium, ref order);
            return order;
        }

        public static Types.WorkOrder ForRevisitWithHydrantAssetWithTownSection(IExtendedSelenium selenium, string @by, Types.WorkOrder order)
        {
            order.DescriptionOfWork = "HYDRANT LANDSCAPING";

            DoRevisitOrder(selenium, order);
            VerifyCreatedOrder(selenium, ref order);
            return order;
        }

        public static Types.WorkOrder WithEmergencyMarkoutRequirement(IExtendedSelenium selenium, string createdBy)
        {
            var order = new Types.WorkOrder {
                OperatingCenter = "NJ7 - Shrewsbury",
                Town = "LOCH ARBOUR",
                StreetNumber = "148",
                ApartmentAddtl = "Apt A",
                Street = "EDGEMONT DR",
                NearestCrossStreet = "PAGE AVE",
                ZipCode = "07701",
                AssetType = "Service",
                PremiseNumber = "9180458651",
                ServiceNumber = "12345678",
                RequestedBy = "Local Government",
                Purpose = "Customer",
                Priority = "Emergency",
                DescriptionOfWork = "CURB BOX REPAIR",
                MarkoutRequirement = "Emergency",
                CreatedBy = createdBy
            };
            DoCreateOrder(selenium, order);
            VerifyCreatedOrder(selenium, ref order);
            return order;
        }

        public static Types.WorkOrder WithNoMarkoutRequirement(IExtendedSelenium selenium, string createdBy)
        {
            var order = new Types.WorkOrder {
                OperatingCenter = "NJ7 - Shrewsbury",
                Town = "LOCH ARBOUR",
                StreetNumber = "148",
                ApartmentAddtl = "Apt A",
                Street = "EDGEMONT DR",
                NearestCrossStreet = "PAGE AVE",
                ZipCode = "07701",
                AssetType = "Service",
                PremiseNumber = "9180458651",
                ServiceNumber = "12345678",
                RequestedBy = "Local Government",
                Purpose = "Customer",
                Priority = "Emergency",
                DescriptionOfWork = "CURB BOX REPAIR",
                MarkoutRequirement = "None",
                CreatedBy = createdBy
            };
            DoCreateOrder(selenium, order);
            VerifyCreatedOrder(selenium, ref order);
            return order;
        }

        public static Types.WorkOrder WithNoMarkoutRequirementButSOPRequired(IExtendedSelenium selenium, string createdBy)
        {
            var order = new Types.WorkOrder {
                OperatingCenter = "NJ7 - Shrewsbury",
                Town = "LOCH ARBOUR",
                StreetNumber = "148",
                ApartmentAddtl = "Apt A",
                Street = "EDGEMONT DR",
                NearestCrossStreet = "PAGE AVE",
                ZipCode = "07701",
                AssetType = "Service",
                PremiseNumber = "9180458651",
                ServiceNumber = "12345678",
                RequestedBy = "Local Government",
                Purpose = "Customer",
                Priority = "Emergency",
                DescriptionOfWork = "CURB BOX REPAIR",
                MarkoutRequirement = "None",
                CreatedBy = createdBy,
                StreetOpeningPermitRequired = "True"
            };
            DoCreateOrder(selenium, order);
            VerifyCreatedOrder(selenium, ref order);
            return order;
        }

        public static Types.WorkOrder WithRoutineMarkoutRequirement(IExtendedSelenium selenium, string createdBy)
        {
            var order = new Types.WorkOrder {
                OperatingCenter = "NJ7 - Shrewsbury",
                Town = "LOCH ARBOUR",
                StreetNumber = "148",
                ApartmentAddtl = "Apt A",
                Street = "EDGEMONT DR",
                NearestCrossStreet = "PAGE AVE",
                ZipCode = "07701",
                AssetType = "Service",
                PremiseNumber = "9180458651",
                ServiceNumber = "12345678",
                RequestedBy = "Local Government",
                Purpose = "Customer",
                Priority = "Emergency",
                DescriptionOfWork = "CURB BOX REPAIR",
                MarkoutRequirement = "Routine",
                CreatedBy = createdBy
            };
            DoCreateOrder(selenium, order);
            VerifyCreatedOrder(selenium, ref order);
            return order;
        }

        #endregion

        #region Private Static Methods

        private static void DoCreateOrder(IExtendedSelenium selenium, Types.WorkOrder order, bool createInSAP = false)
        {
            selenium.WaitThenSelectLabel(NecessaryIDs.DDL_OP_CENTER, order.OperatingCenter);
            selenium.WaitThenSelectLabel(NecessaryIDs.DDL_TOWN, order.Town);
            if (order.TownSection != null)
            {
                selenium.WaitThenSelectLabel(NecessaryIDs.DDL_TOWN_SECTION,
                    order.TownSection);
            }
            selenium.Type(NecessaryIDs.TXT_STREET_NUMBER, order.StreetNumber);
            selenium.Type(NecessaryIDs.TXT_SUPPLEMENT_NO, order.ApartmentAddtl);
            selenium.WaitThenSelectLabel(NecessaryIDs.DDL_STREET, order.Street);
            selenium.WaitThenSelectLabel(NecessaryIDs.DDL_NEAREST_CROSS_STREET,
                order.NearestCrossStreet);
            selenium.Type(NecessaryIDs.TXT_ZIP_CODE, order.ZipCode);
            EnterOrderAssetInformation(selenium, order);
            EnterOrderNonLocationalInformation(selenium, order);
            if (!string.IsNullOrWhiteSpace(order.SAPNotificationNumber))
            {
                selenium.Type(NecessaryIDs.TXT_SAP_NOTIFICATION_NUMBER, order.SAPNotificationNumber);
            }
            if (!string.IsNullOrWhiteSpace(order.SAPWorkOrderNumber))
            {
                selenium.Type(NecessaryIDs.TXT_SAP_WORK_ORDER_NUMBER, order.SAPWorkOrderNumber);
            }
            SaveOrder(selenium, order);
            //if (createInSAP)
            //{
            //    SaveOrderInGeneralToGetItToSAP();
            //}
        }
        
        private static void DoRevisitOrder(IExtendedSelenium selenium, Types.WorkOrder order)
        {
            selenium.Click(NecessaryIDs.RDO_REVISIT_REVISIT);
            selenium.AssertAlert(
                "Please enter the number of the original order being revisited.");
            selenium.Type(NecessaryIDs.TXT_ORIGINAL_ORDER_NUMBER, order.WorkOrderID);
            selenium.Click(NecessaryIDs.RDO_REVISIT_REVISIT);
            VerifyRevisitInformationLoaded(selenium, order);
            EnterOrderNonLocationalInformation(selenium, order);
            // no clue why this Wait() needs to be here, but it does
            SeleniumExtensions.Wait(1);
            SaveOrder(selenium, order);
        }

        private static void EnterOrderNonLocationalInformation(IExtendedSelenium selenium, Types.WorkOrder order)
        {
            selenium.WaitThenSelectLabel(NecessaryIDs.DDL_REQUESTED_BY,
                order.RequestedBy);
            selenium.WaitThenSelectLabel(NecessaryIDs.DDL_DRIVEN_BY,
                order.Purpose);
            selenium.WaitThenSelectLabel(NecessaryIDs.DDL_PRIORITY,
                order.Priority);
            selenium.WaitThenSelectLabel(NecessaryIDs.DDL_DESCRIPTION_OF_WORK,
                order.DescriptionOfWork);

            switch (order.AssetType)
            {
                case "Main":
                    if (new Regex("^WATER MAIN BREAK REP(AIR|LACE)$").IsMatch(
                            order.DescriptionOfWork))
                    {
                        selenium.WaitThenSelectLabel(
                            NecessaryIDs.DDL_CUSTOMER_IMPACT_RANGE,
                            order.CustomerImpactRange);
                        selenium.WaitThenSelectLabel(
                            NecessaryIDs.DDL_REPAIR_TIME_RANGE,
                            order.RepairTimeRange);
                        selenium.WaitThenSelectLabel(NecessaryIDs.DDL_ALERT_ISSUED, order.AlertIssued);
                        selenium.WaitThenSelectLabel(
                            NecessaryIDs.DDL_SIGNIFICANT_TRAFFIC_IMPACT,
                            order.SignificantTrafficImpact);
                    }
                    break;
            }

            if ((order.StreetOpeningPermitRequired ?? string.Empty).ToLower() == "true")
            {
                selenium.Check(NecessaryIDs.CHK_SOP_REQUIRED);
            }

            selenium.WaitThenSelectLabel(NecessaryIDs.DDL_MARKOUT_REQUIREMENT,
                order.MarkoutRequirement);
            selenium.Type(NecessaryIDs.TXT_NOTES, order.Notes);
        }

        private static void EnterOrderAssetInformation(IExtendedSelenium selenium, Types.WorkOrder order)
        {
            selenium.WaitThenSelectLabel(NecessaryIDs.DDL_ASSET_TYPE,
                order.AssetType);
            switch (order.AssetType)
            {
                case "Main":
                    // nothing to be done here
                    break;
                case "Valve":
                    selenium.WaitThenSelectLabel(NecessaryIDs.DDL_VALVE,
                        order.ValveID);
                    break;
                case "Hydrant":
                    selenium.WaitThenSelectLabel(NecessaryIDs.DDL_HYDRANT,
                        order.HydrantID);
                    break;
                case "Sewer Opening":
                    selenium.WaitThenSelectLabel(
                        NecessaryIDs.DDL_SEWER_OPENING, order.SewerOpeningID);
                    break;
                case "Service":
                    selenium.Type(NecessaryIDs.TXT_PREMISE_NUMBER,
                        order.PremiseNumber);
                    selenium.Type(NecessaryIDs.TXT_SERVICE_NUMBER,
                        order.ServiceNumber);
                    break;
                case "Equipment":
                    selenium.WaitThenSelectLabel(NecessaryIDs.DDL_EQUIPMENT,
                        order.EquipmentID);
                    break;
            }
            
            selenium.RunScript("getServerElementById('hidLongitude').val('-74.18444101');");
            selenium.RunScript("getServerElementById('hidLatitude').val('40.24209423766002');");
            selenium.RunScript("getServerElementById('imgShowPicker').attr('src', 'http://localhost:4932/mapcall/Modules/WorkOrders/Includes/map-icon-blue.png');");

            /*
            selenium.ClickAndWait(NecessaryIDs.IMG_SHOW_PICKER, 20);

            if (order.AssetType != "Equipment")
            {
                selenium.WaitForElementPresent(NecessaryIDs.BTN_GEO_CODE);
                // TODO: this next line only ever times out
                //            selenium.WaitForElementPresent("//area");
                // txtLocation in the lat/lon picker should have the current address loaded at this point
                if (order.TownSection != null)
                {
                    selenium.AssertValue(NecessaryIDs.TXT_LOCATION,
                        string.Format("{0} {1} {2} {3} NJ", order.StreetNumber,
                            order.Street, order.TownSection, order.Town));
                }
                else
                {
                    selenium.AssertValue(NecessaryIDs.TXT_LOCATION,
                        string.Format("{0} {1} {2} NJ", order.StreetNumber,
                            order.Street, order.Town));
                }

                selenium.ClickAndWait(NecessaryIDs.BTN_GEO_CODE, 20);
                selenium.WaitForNotValue("//input[@id=\"hidLongitude\"]","-74.1481018");
                selenium.WaitForNotValue("//input[@id=\"hidLongitude\"]","0");
                selenium.Click(NecessaryIDs.BTN_SAVE_COORDINATES);
                selenium.Click(NecessaryIDs.BTN_CLOSE_PICKER);
            }
            */
        }

        private static void SaveOrder(IExtendedSelenium selenium, Types.WorkOrder order)
        {
            selenium.ClickAndWaitForPageToLoad(NecessaryIDs.BTN_SAVE);
            try
            {
                switch (order.AssetType)
                {
                    case "Service":
                    case "Sewer Opening":
                    case "Hydrant":
                    case "Valve":
                    case "Equipment":
                        selenium.AssertConfirmation(
                            "There is an existing order for this asset which has not been completed.  Are you sure you want to create a new order?");
                        break;
                }
            }
            catch
            {
                // there may not be a confirmation
            }
        }

        private static void VerifyCreatedOrder(IExtendedSelenium selenium, ref Types.WorkOrder order)
        {
            selenium.DetectYSOD();

            selenium.WaitForTextPresent("Initial Information");

            selenium.AssertTextPresent(order.Town);
            selenium.AssertTextPresent(order.TownSection);
            selenium.AssertTextPresent(order.StreetNumber);
            selenium.AssertTextPresent(order.Street);
            selenium.AssertTextPresent(order.ZipCode);
            VerifyOrderAssetInformation(selenium, order);
            selenium.AssertTextPresent(order.RequestedBy);
            selenium.AssertTextPresent(order.Purpose);
            selenium.AssertTextPresent(order.Priority);
            selenium.AssertTextPresent(order.DescriptionOfWork);
            selenium.AssertTextPresent(order.MarkoutRequirement);
            selenium.AssertTextPresent(order.Notes);
            selenium.AssertTextPresent(order.CreatedBy);
            selenium.AssertTextPresent(order.AccountCharged);
            if (!string.IsNullOrWhiteSpace(order.SAPNotificationNumber))
            {
                selenium.AssertTextPresent(order.SAPNotificationNumber);
            }
            if (!string.IsNullOrWhiteSpace(order.SAPWorkOrderNumber))
            {
                selenium.AssertTextPresent(order.SAPWorkOrderNumber);
            }
            order.DateReceived = selenium.GetText(NecessaryIDs.LBL_DATE_RECEIVED);
            order.WorkOrderID =
                selenium.GetText(NecessaryIDs.LBL_WORK_ORDER_ID);
        }

        private static void VerifyRevisitInformationLoaded(IExtendedSelenium selenium, Types.WorkOrder order)
        {
            selenium.WaitForNotSelectedLabel(NecessaryIDs.DDL_OP_CENTER,
                "--Select Here--");
            selenium.WaitForNotSelectedLabel(NecessaryIDs.DDL_TOWN,
                "--Select Here--");
            selenium.WaitForNotValue(NecessaryIDs.TXT_STREET_NUMBER, "");
            selenium.WaitForNotValue(NecessaryIDs.TXT_ZIP_CODE, "");

            selenium.AssertSelectedLabel(NecessaryIDs.DDL_OP_CENTER,
                order.OperatingCenter);
            selenium.AssertNotEditable(NecessaryIDs.DDL_OP_CENTER);
            selenium.AssertSelectedLabel(NecessaryIDs.DDL_TOWN, order.Town);
            selenium.AssertNotEditable(NecessaryIDs.DDL_TOWN);
            selenium.AssertValue(NecessaryIDs.TXT_STREET_NUMBER, order.StreetNumber);
            selenium.AssertNotEditable(NecessaryIDs.TXT_STREET_NUMBER);
            selenium.AssertValue(NecessaryIDs.TXT_ZIP_CODE, order.ZipCode);
            selenium.AssertNotEditable(NecessaryIDs.TXT_ZIP_CODE);
            VerifyOrderAssetInformation(selenium, order);
        }

        private static void VerifyOrderAssetInformation(IExtendedSelenium selenium, Types.WorkOrder order)
        {
            selenium.AssertTextPresent(order.AssetType);
            switch (order.AssetType)
            {
                case "Main":
                    // nothing further needed
                    break;
                case "Valve":
                    selenium.AssertTextPresent(order.ValveID.Replace(" - ACTIVE", ""));
                    break;
                case "Sewer Opening":
                    selenium.AssertTextPresent(order.SewerOpeningID.Replace(" - ACTIVE", ""));
                    break;
                case "Hydrant":
                    selenium.AssertTextPresent(order.HydrantID.Replace(" - ACTIVE", ""));
                    break;
                case "Service":
                    break;
                case "Equipment":
                    selenium.AssertTextPresent(order.EquipmentID);
                    break;
                default:
                    throw new NotImplementedException(
                        String.Format(
                            "Asset verification for type '{0}' has not been implemented.",
                            order.AssetType));
            }
        }

        #endregion
    }
}
