using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC.ClassExtensions;
using MMSINC.Interface;

namespace MMSINC.Controls
{
    [Themeable(true)]
    public class MvpDetailsView : DetailsView, IDetailControl, IDetailsView, IDataBoundControl
    {
        #region Properties

        public DetailViewMode CurrentMvpMode
        {
            get { return CurrentMode.ToMVPDetailViewMode(); }
        }

        public override object DataItem
        {
            get
            {
                if (CurrentMode == DetailsViewMode.Insert)
                {
                    var fieldinfo = typeof(DetailsView).GetField("_dataItem",
                        BindingFlags.NonPublic |
                        BindingFlags.Instance |
                        BindingFlags.FlattenHierarchy);
                    return fieldinfo.GetValue(this);
                }

                return base.DataItem;
            }
        }

        #endregion

        #region Exposed Methods

        public void ChangeMvpMode(DetailViewMode newMode)
        {
            ChangeMode(newMode.ToDetailsViewMode());
        }

        public TControl FindControl<TControl>(string id) where TControl : Control
        {
            return ControlExtensions.FindControl<TControl>(this, id);
        }

        public TIControl FindIControl<TIControl>(string id) where TIControl : IControl
        {
            return ((Control)this).FindIControl<TIControl>(id);
        }

        #endregion
    }
}
