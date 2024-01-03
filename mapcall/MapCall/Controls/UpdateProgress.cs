using System.Web.UI;

namespace MapCall.Controls
{
    /// <summary>
    /// This is only needed so we can style the thing with a CssClass. This won't be needed in .Net 4.0
    /// </summary>
    public class UpdateProgress : System.Web.UI.UpdateProgress 
    {

        #region Properties

        public string CssClass { get; set; }

        #endregion

        #region Private Methods

        protected override void Render(HtmlTextWriter writer)
        {
            if (!string.IsNullOrEmpty(CssClass))
            {
                writer.AddAttribute("class", CssClass);
            }
            base.Render(writer);
        }


        #endregion


    }
}
