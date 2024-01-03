using System.Web.UI.WebControls;

namespace MMSINC.Controls
{
    public class EntityBulletedList : BulletedList
    {
        #region Constants

        private const string BULLETED_LIST_STYLE = "padding:0px;margin:0px;padding-left:24px;";

        #endregion

        public override void DataBind()
        {
            Attributes.Add("Style", BULLETED_LIST_STYLE);
            base.DataBind();
        }
    }
}
