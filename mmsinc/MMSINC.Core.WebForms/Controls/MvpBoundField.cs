using System.Web.UI;
using System.Web.UI.WebControls;

namespace MMSINC.Controls
{
    public class MvpBoundField : BoundField
    {
        protected override void InitializeDataCell(DataControlFieldCell cell, DataControlRowState rowState)
        {
            Control child = null;
            Control control2 = null;
            if ((((rowState & DataControlRowState.Edit) != DataControlRowState.Normal) && !ReadOnly) ||
                ((rowState & DataControlRowState.Insert) != DataControlRowState.Normal))
            {
                var box = new TextBox {ToolTip = HeaderText};
                child = box;
                //This is what's different from the base method. It allows insert to databind.
                if (DataField.Length != 0 &&
                    (((rowState & DataControlRowState.Edit) !=
                      DataControlRowState.Normal) ||
                     ((rowState & DataControlRowState.Insert) !=
                      DataControlRowState.Normal)))
                {
                    control2 = box;
                }
            }
            else if (DataField.Length != 0)
            {
                control2 = cell;
            }

            if (child != null)
            {
                cell.Controls.Add(child);
            }

            if ((control2 != null) && Visible)
            {
                control2.DataBinding += OnDataBindField;
            }
        }
    }
}
