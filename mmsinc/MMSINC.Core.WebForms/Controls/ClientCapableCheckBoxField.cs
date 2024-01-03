using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MMSINC.Controls
{
    public class ClientCapableCheckBoxField : CheckBoxField
    {
        #region Properties

        public string OnClientClick { get; set; }

        #endregion

        #region ExposedMethods

        public override void InitializeCell(DataControlFieldCell cell, DataControlCellType cellType,
            DataControlRowState rowState, int rowIndex)
        {
            base.InitializeCell(cell, cellType, rowState, rowIndex);
            // TODO: Not sure this is ever the case here
            if (cell.Controls.Count <= 0) return;
            if (cell.Controls[0] is CheckBox)
            {
                var chkbox = (CheckBox)cell.Controls[0];
                AddControlID(chkbox, rowIndex);
                if (!string.IsNullOrEmpty(OnClientClick))
                    AddOnClientClick(chkbox);
            }
        }

        #endregion

        #region Private Methods

        private void AddControlID(Control chkbox, int rowIndex)
        {
            chkbox.ID = String.Format("dvChk{0}_{1}", DataField, (rowIndex == -1) ? "0" : rowIndex.ToString());
        }

        private void AddOnClientClick(CheckBox chkbox)
        {
            chkbox.InputAttributes.Add("onclick", OnClientClick);
        }

        #endregion
    }
}
