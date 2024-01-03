using MapCall.Common.Configuration;
using MapCall.Common.Utility;
using MapCall.Common.Web;
using MMSINC.Common;
using System.Web.UI;

namespace MapCall.Common.Resources.Controls.Menu
{
    // TODO: Menu's gonna have to work with the new roles stuff. 
    public class Menu : MvpUserControl
    {
        #region Properties

        public override bool EnableViewState
        {
            get { return false; }
            // we need to set this to false here, but Sonar isn't happy unless we use `value`.  thus, a
            // compromise.
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            set { base.EnableViewState = !value && value; }
        }

        #endregion

        #region Private Methods

        protected override void Render(HtmlTextWriter writer)
        {
            MenuRenderHelper.Render<MapCallMenuRenderHelper>(MenuConfiguration.GetMenu(),
                new StringOrTextWriter(writer));
        }

        #endregion
    }
}
