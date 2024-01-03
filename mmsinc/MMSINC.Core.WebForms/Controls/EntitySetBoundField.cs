using System;
using System.Web.UI.WebControls;

namespace MMSINC.Controls
{
    public class EntitySetBoundField : BoundField
    {
        #region Exposed Methods

        public override void InitializeCell(DataControlFieldCell cell, DataControlCellType cellType,
            DataControlRowState rowState, int rowIndex)
        {
            base.InitializeCell(cell, cellType, rowState, rowIndex);
            var lit = new Literal();
            cell.Controls.Add(lit);
        }

        protected override void OnDataBindField(object sender, EventArgs e)
        {
            base.OnDataBindField(sender, e);
            var dcfc = (DataControlFieldCell)sender;
            var cf = dcfc.BindingContainer;

            var x = new GridView {DataSource = GetValue(cf)};
            ((DataControlFieldCell)sender).Controls.Add(x);
            //((DataControlFieldCell)sender).Text
            //var lit = (Literal) sender;
            //var controlContainer = lit.BindingContainer;
            //object val = GetValue(controlContainer);
        }

        #endregion
    }
}
