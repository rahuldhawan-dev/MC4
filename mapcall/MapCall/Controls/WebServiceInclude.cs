using System;
using System.Web.UI;

namespace MapCall.Controls
{
    /// <summary>
    /// This is a simple control that adds a script include to a page for the javascript associated with a given web service.
    /// Just place the control on a page and set its Service property to whichever service. 
    /// 
    /// There can be multiple instances throughout a page, but only one will actually render, so there's no worry of collision.
    /// </summary>
    public sealed class WebServiceInclude : Control
    {

        #region Enums

        public enum Services
        {
            None = 0, // Default value, no rendering is done if this is set.
            Premises,
            MeterRecorders,
            Contacts
        }

        #endregion

        #region Properties

        public Services Service { get; set; }

        #endregion

        #region Virtual Methods

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            EnableViewState = false; // This control doesn't render, no need for ViewState at all.
            var pageType = Page.GetType();
            Action<string, string, string, string> regScript = (string scriptKey, string scriptUrl, string scriptObjName, string serviceUrl) =>
                                                   {
                                                       var cs = Page.ClientScript;
                                                       cs.RegisterClientScriptInclude(pageType, 
                                                           "BaseServiceInclude",
                                                           Page.ResolveClientUrl("~/scripts/services.js"));

                                                       cs.RegisterClientScriptInclude(pageType, 
                                                           scriptKey,
                                                           Page.ResolveClientUrl(scriptUrl));

                                                       var initScript = string.Format("{0}.initialize(\"{1}\");",
                                                                                      scriptObjName,
                                                                                      this.Page.ResolveClientUrl(
                                                                                          serviceUrl));

                                                       cs.RegisterClientScriptBlock(pageType, scriptObjName + "Initialize", initScript,true);
                                                   };


            switch (Service)
            {
                default: // For any unsupported Services enums, or Services.None.
                    // This does nothing
                    Visible = false;
                    break;

                case Services.Premises:
                    regScript("PremisesServiceInclude", 
                        "~/Modules/Data/Premises/Premises.js",
                        "PremiseService",
                        "~/Modules/Data/Premises/Premises.asmx");
                    break;

                case Services.MeterRecorders:
                    regScript("MeterRecorderServiceInclude",
                        "~/Modules/Data/MeterRecorders/MeterRecorders.js",
                        "MeterRecorderService",
                        "~/Modules/Data/MeterRecorders/MeterRecorders.asmx");
                    break;

                case Services.Contacts:
                    regScript("ContactsServiceInclude", 
                        "~/Modules/Data/Contacts/ContactsService.js",
                        "ContactsService",
                        "~/Modules/Data/Contacts/ContactsService.asmx");
                    break;
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            // This control doesn't actually render.
            //base.Render(writer);
        }

        #endregion


    }
}