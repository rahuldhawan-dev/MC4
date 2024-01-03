using System;
using System.Web.UI;

namespace MapCall.Modules.Maps
{
    /// <summary>
    /// This page redirect the user to the correct IFrame from our Realtime Operations Map.
    /// This is used in order to cut down the amount of data needed to be stored in the marker arrays
    /// </summary>
    public partial class ifm : Page
    {
        #region Constants

        public struct URLS
        {
            internal const string HYDRANT = "~/modules/maps/hydrant_mvc.aspx?recID={0}",
                VALVE = "~/modules/maps/valve_mvc.aspx?recID={0}",
                WORKORDER = "WorkOrder.aspx?ID={0}",
                COMPLAINT = "iWQComplaint.aspx?recordID={0}",
                BACTI = "iSampleSite.aspx?recordID={0}",
                LEAD = "iSampleResult.aspx?recordId={0}",
                EVENT = "iEvent.aspx?recordID={0}",
                VEHICLE = "iVehicle.aspx?ID={0}",
                OVERFLOWS = "iSewerOverflow.aspx?ID={0}",
                FLUSHING = "iFlushingSchedule.aspx?ID={0}",
                ONECALL_TICKET = "iOneCallTicket.aspx?ID={0}";
             
        }
        #endregion

        #region Properties

        public string RequestID
        {
            get
            {
                return Request.QueryString["ID"];
            }
        }

        #endregion

        #region Event Handlers

        protected void Page_Load(object sender, EventArgs e)
        {
            switch (Request.QueryString["Type"].ToLower())
            {
                case "hyd":
                    //dv =
                    //    (DataView)
                    //    dsHydrant.Select(DataSourceSelectArguments.Empty);
                    //if (dv != null && dv.Table.Rows.Count > 0)
                        Response.Redirect(string.Format(URLS.HYDRANT, RequestID));
                    break;
                case "val":
                    //dv = (DataView)dsValve.Select(DataSourceSelectArguments.Empty);
                    //if (dv != null && dv.Table.Rows.Count > 0)
                        Response.Redirect(string.Format(URLS.VALVE, RequestID));
                    break;
                case "wo":
                case "lek":
                    Response.Redirect(string.Format(URLS.WORKORDER, RequestID));
                    break;
                case "com":
                    Response.Redirect(string.Format(URLS.COMPLAINT, RequestID));
                    break;
                case "bac":
                    Response.Redirect(string.Format(URLS.BACTI, RequestID));
                    break;
                case "led":
                    Response.Redirect(string.Format(URLS.LEAD, RequestID));
                    break;
                case "evt":
                    Response.Redirect(string.Format(URLS.EVENT, RequestID));
                    break;
                case "veh":
                    Response.Redirect(string.Format(URLS.VEHICLE, RequestID));
                    break;
                case "ovr":
                    Response.Redirect(string.Format(URLS.OVERFLOWS, RequestID));
                    break;
                case "flu":
                    Response.Redirect(string.Format(URLS.FLUSHING, RequestID));
                    break;
                case "oct":
                    Response.Redirect(string.Format(URLS.ONECALL_TICKET, RequestID));
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}
