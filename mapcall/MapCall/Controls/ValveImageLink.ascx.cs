using System;
using System.Data;
using System.Web.UI;

namespace MapCall.Controls
{
    public partial class ValveImageLink : UserControl
    {
        #region Constants

        private const string NAVIGATE_URL_FORMAT_STRING = @"~/Modules/Mvc/FieldOperations/ValveImage/Show/{0}.pdf";
    
        #endregion
        
        #region Properties

        public string LinkText { get; set; }
        public string ValNum { get; set; }
        public string OpCntr { get; set; }
        public string Fld { get; set; }
        public string FileList { get; set; }
        public int? ValveId { get; set; }

        #endregion

        #region Exposed Methods
        
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnDataBinding(EventArgs e)
        {
            base.OnDataBinding(e);
            GenerateLink();
        }
        #endregion

        #region Private Methods

        private void GenerateLink()
        {
            SetSelectCommand();
            dsNJValveImages.SelectParameters["valnum"].DefaultValue = ValNum;
            dsNJValveImages.SelectParameters["opcntr"].DefaultValue = OpCntr;
           
            if (ValveId.HasValue)
            {
                dsNJValveImages.SelectParameters["valveId"].DefaultValue = ValveId.ToString();
            }
            else
            {
                // Needs to be removed or else our DataView ends up becoming null for some stupid reason.
                dsNJValveImages.SelectParameters.Remove(dsNJValveImages.SelectParameters["valveId"]);
            }
            
            var dv =
                (DataView)
                dsNJValveImages.Select(DataSourceSelectArguments.Empty);
            
            if (dv.Count <= 0) return;

            var valveid = dv[0]["valveimageid"].ToString();
            
            if (String.IsNullOrEmpty(valveid)) return;

            hl1.Text = LinkText;
            hl1.NavigateUrl = String.Format(NAVIGATE_URL_FORMAT_STRING, valveid);
        }

        private void SetSelectCommand()
        {
            //string state;
            //switch (OpCntr.Substring(0, 2))
            //{
            //    case "PA":
            //        state = "PA";
            //        break;
            //    case "NY":
            //        state = "NY";
            //        break;
            //    default:
            //        state = "NJ";
            //        break;
            //}
            //dsNJValveImages.SelectCommand =
            //    String.Format(SQL_SELECT_FORMAT_STRING, state);
            //dsNJValveImages.SelectCommand = SQL_SELECT_FORMAT_STRING;

            if (ValveId.HasValue)
            {
                dsNJValveImages.SelectCommand = @"
                    --declare @valveId int 
                    --select @valveId = 197934
                    declare @valveImageId int 
                    select @valveImageId = (select top 1 ValveImageID from [ValveImages] where [ValveID] = @valveId and [IsDefault] = 'true')
                    if (@valveImageId is null) select @valveImageId = (select top 1 ValveImageID from [ValveImages] where [ValveID] = @valveId)
                    select @valveImageId as ValveImageID";
            }
            else
            {
                // This is the previous version that's been used for who knows how long. -Ross 1/29/2013
                dsNJValveImages.SelectCommand = "select top 1 valveImageID from ValveImages where ValveNumber = @valNum and operatingCenter = @opCntr";
            }

          
        }

        #endregion
    }
}