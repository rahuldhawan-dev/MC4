using System.Web.UI.WebControls;
using MMSINC.ClassExtensions.IOrderedDictionaryExtensions;
using MMSINC.Controls;

namespace LINQTo271.Common
{
    public class WorkOrdersFormView : MvpFormView
    {
        #region Private Methods

        protected override void OnItemInserting(FormViewInsertEventArgs e)
        {
            e.Values.CleanValues();
            base.OnItemInserting(e);
        }

        protected override void OnItemUpdating(FormViewUpdateEventArgs e)
        {
            e.NewValues.CleanValues();
            base.OnItemUpdating(e);
        }

        #endregion
    }
}
