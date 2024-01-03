using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Script.Serialization;
using System.Web.UI;
using MMSINC.DataPages;

namespace MapCall.Controls.Data
{
    
    /// <summary>
    /// Ajax autocomplete control for searching for PremiseNumbers.
    /// </summary>
    public partial class PremiseNumberSearch : UserControl , IDataField
    {

        #region Properties

        /// <summary>
        /// Property used for binding so that DBNull is handled properly.
        /// 
        /// Ross does not remember why this is a string field. It's silly, it should be an integer.
        /// </summary>
        public string BindingSelectedPremiseId
        {
            get
            {
                var selected = SelectedPremiseId;
                // return string.empty if it's DBNull.
                return ((selected <= 0) ? string.Empty : selected.ToString());
            }
            set {
                SelectedPremiseId = (String.IsNullOrEmpty(value) ? 0 : Convert.ToInt32(value));
            }
        }

        /// <summary>
        /// Gets or sets whether this is a required field.
        /// </summary>
        public bool IsRequiredField { get; set; }

        /// <summary>
        /// Gets or sets the selected PremiseID for this instance.  
        /// </summary>
        public int SelectedPremiseId
        {
            get
            {
                var hf = hidPremiseID;
                if (hf != null)
                {
                    var result = 0;
                    if (int.TryParse(hidPremiseID.Value, out result))
                    {
                        // Only return the result if it's non-negative.
                        if (result >= 0)
                        {
                            return result;
                        }
                    }
                }

                // Return 0 if the hidden field hasn't been initialized yet,
                // or if the value in the hidden field is invalidly formatted.
                return 0;
            }
            set
            {
                hidPremiseID.Value = value.ToString();
            }
        }

        /// <summary>
        /// Gets or sets the selected PremiseNumber for this instance.
        /// </summary>
        public string SelectedPremiseNumber
        {
            get
            {
                return ((hidPremiseNumber != null) ? hidPremiseNumber.Value.Trim() : string.Empty);
            }
            set
            {
                value = ((value != null) ? value.Trim() : String.Empty);
                txtPremiseNumber.Text = value;
                lblPremiseNumber.Text = value;
                hidPremiseNumber.Value = value;
            }
        }

        #endregion

        #region Protected Overrides

        // This is so the premise number gets persisted on postbacks. I'm using
        // ControlState so if ViewState gets disabled, it doesn't break this.

        protected override void LoadControlState(object savedState)
        {
            base.LoadControlState(savedState);
            if (savedState != null)
            {
                var state = (Dictionary<string, object>) savedState;
                SelectedPremiseId = (int) state["SelectedPremiseId"];
                SelectedPremiseNumber = (string) state["SelectedPremiseNumber"];
            }
        }

        protected override object SaveControlState()
        {
            // This is needed to persist the value to the label and textbox. Otherwise
            // it keeps showing up a postback too late.
            SelectedPremiseNumber = SelectedPremiseNumber;

            var state = new Dictionary<string, object>();
            state["SelectedPremiseId"] = SelectedPremiseId;
            state["SelectedPremiseNumber"] = SelectedPremiseNumber;

            return state;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Page.RegisterRequiresControlState(this);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            rfvPremiseID.Visible = (IsRequiredField);

            var clientId = ClientID;
            var thisType = GetType();
            var pageType = Page.GetType();
            var csm = Page.ClientScript;
         
            csm.RegisterClientScriptInclude(pageType, "PremiseNumberSearchScript", ResolveClientUrl("~/Controls/Data/PremiseNumberSearch.js"));

            // This is so the service path is always correct, regardless of what page this control is loaded from. 
            const string servicePathFormat = "PremiseNumberSearch.servicePath = \"{0}\";";
            var servicePath = ResolveClientUrl("~/Modules/Data/Premises/Premises.asmx");
            csm.RegisterStartupScript(thisType,
                                          "PremiseNumberSearchStaticValues", 
                                          string.Format(servicePathFormat, servicePath),
                                          true);
           
            // This constructs a new JS PremiseNumberSearch object. 
           
            var scriptInitDict = new Dictionary<String, Object>();
            scriptInitDict["txtPremiseNumber"] = txtPremiseNumber.ClientID;
            scriptInitDict["hidPremiseID"] = hidPremiseID.ClientID;
            scriptInitDict["hidPremiseNumber"] = hidPremiseNumber.ClientID;
            scriptInitDict["listPremiseNumber"] = listPremiseNumber.ClientID;
            scriptInitDict["lblPremiseNumber"] = lblPremiseNumber.ClientID;
            scriptInitDict["rfvPremiseID"] = rfvPremiseID.ClientID;
            scriptInitDict["lnkRemoveSelected"] = lnkRemoveSelected.ClientID;
            scriptInitDict["isRequiredField"] = IsRequiredField;
         
            const string scriptInitFormat = "var {0} = new PremiseNumberSearch({1});";

            var jsonSerializer = new JavaScriptSerializer();

            var script = string.Format(scriptInitFormat,
                                          ClientID,
                                          jsonSerializer.Serialize(scriptInitDict));

            // Using clientId as the key since it shouldn't be used anywhere else that would overwrite it.
            csm.RegisterStartupScript(thisType, clientId, script, true);

        }

        #endregion

        #region Exposed Methods

        public string FilterExpression()
        {
            return ((SelectedPremiseId > 0)
                        ? String.Format(" AND [{0}] = {1}", "PremiseID", SelectedPremiseId)
                        : string.Empty);

        }

        public void FilterExpression(IFilterBuilder builder)
        {
            if (SelectedPremiseId > 0)
            {
                var exp = new FilterBuilderExpression();
                exp.AddParameter("PremiseID", DbType.Int32, SelectedPremiseId);
                builder.AddExpression(exp);
            }
        }

        #endregion
    }

}