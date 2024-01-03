using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using MMSINC.Extensions;

namespace MMSINC
{
    public sealed class ManyToManyField : System.Web.UI.WebControls.BoundField
    {
        #region Constants

        private struct ControlIDs
        {
            public const string LB_LEFT = "lbLeft";
            public const string LB_RIGHT = "lbRight";
            public const string BTN_MOVELEFT = "btnMoveLeft";
            public const string BTN_MOVERIGHT = "btnMoveRight";
            public const string BTN_MOVEALLLEFT = "btnMoveAllLeft";
            public const string BTN_MOVEALLRIGHT = "btnMoveAllRight";
        }

        private struct SizeUnits
        {
            public static readonly Unit BOX_HEIGHT = new Unit(84);
            public static readonly Unit BOX_WIDTH = new Unit(120);
            public static readonly Unit BUTTON_WIDTH = new Unit(25);
        }

        private struct FunCallFormatStrings
        {
            public const string MOVE_ALL = "DualSelectBox.moveAll($get('{0}'),$get('{1}'));";
            public const string MOVE_SELECTED = "DualSelectBox.moveSelected($get('{0}'),$get('{1}'));";
        }

        private const string SCRIPT_PATH = "~/includes/dualSelectBox.js";

        #endregion

        #region Private Members

        private static int instances = 0;

        #endregion

        #region Control IDs

        private string InstanceCount { get; set; }

        private string lbLeftID
        {
            get { return ControlIDs.LB_LEFT + InstanceCount; }
        }

        private string lbRightID
        {
            get { return ControlIDs.LB_RIGHT + InstanceCount; }
        }

        private string btnMoveLeftID
        {
            get { return ControlIDs.BTN_MOVELEFT + InstanceCount; }
        }

        private string btnMoveAllLeftID
        {
            get { return ControlIDs.BTN_MOVEALLLEFT + InstanceCount; }
        }

        private string btnMoveRightID
        {
            get { return ControlIDs.BTN_MOVERIGHT + InstanceCount; }
        }

        private string btnMoveAllRightID
        {
            get { return ControlIDs.BTN_MOVEALLRIGHT + InstanceCount; }
        }

        #endregion

        #region Control Declarations

        ListBox lbLeft, lbRight;
        HtmlInputButton btnMoveLeft, btnMoveRight, btnMoveAllLeft, btnMoveAllRight;

        #endregion

        #region Constructors

        public ManyToManyField()
        {
            instances++;
            InstanceCount = instances.ToString();
        }

        public ManyToManyField(IDataElementField thing)
            : this()
        {
            this.HeaderText = thing.HeaderName;
        }

        ~ManyToManyField()
        {
            instances--;
        }

        #endregion

        #region Bound Field Methods

        public override void InitializeCell(DataControlFieldCell cell, DataControlCellType cellType, DataControlRowState rowState, int rowIndex)
        {
            if (cellType != DataControlCellType.DataCell)
            {
                base.InitializeCell(cell, cellType, rowState, rowIndex);
                return;
            }

            if (Control.Page != null)
            {
                ScriptManager mgr = ScriptManager.GetCurrent(Control.Page);
                mgr.Scripts.Add(new ScriptReference(SCRIPT_PATH));
            }

            if (cell != null)
            {
                HtmlTable tbl = new HtmlTable();
                HtmlTableRow tr = new HtmlTableRow();
                HtmlTableCell td = new HtmlTableCell();
                lbLeft = CreateListBox(lbLeftID);
                td.Controls.Add(lbLeft);
                tr.Cells.Add(td);

                td = new HtmlTableCell();
                CreateAndAddButtons(ref td);
                tr.Cells.Add(td);

                td = new HtmlTableCell();
                lbRight = CreateListBox(lbRightID);
                td.Controls.Add(lbRight);
                tr.Cells.Add(td);

                tbl.Rows.Add(tr);
                cell.Controls.Add(tbl);

                cell.Load += (object sender, EventArgs e) =>
                    AssignClientEventHandlers();
            }
        }

        #endregion

        #region Private Methods

        private ListBox CreateListBox(string id)
        {
            return new ListBox()
            {
                SelectionMode = ListSelectionMode.Multiple,
                ID = id,
                Height = SizeUnits.BOX_HEIGHT,
                Width = SizeUnits.BOX_WIDTH
            };
        }

        private HtmlInputButton CreateButton(string id, string label)
        {
            HtmlInputButton btn = new HtmlInputButton()
            {
                ID = id,
                Value = label
            };
            btn.Style.Add("width", SizeUnits.BUTTON_WIDTH.ToString());
            return btn;
        }

        private static LiteralControl CreateHtmlBreak()
        {
            return new LiteralControl("<br />");
        }

        private void CreateAndAddButtons(ref HtmlTableCell cell)
        {
            btnMoveLeft = CreateButton(btnMoveLeftID, "<");
            cell.Controls.Add(btnMoveLeft);
            cell.Controls.Add(CreateHtmlBreak());

            btnMoveRight = CreateButton(btnMoveRightID, ">");
            cell.Controls.Add(btnMoveRight);
            cell.Controls.Add(CreateHtmlBreak());

            btnMoveAllLeft = CreateButton(btnMoveAllLeftID, "<<");
            cell.Controls.Add(btnMoveAllLeft);
            cell.Controls.Add(CreateHtmlBreak());

            btnMoveAllRight = CreateButton(btnMoveAllRightID, ">>");
            cell.Controls.Add(btnMoveAllRight);
        }

        private void AssignClientEventHandlers()
        {
            btnMoveLeft.SetClientClickHandler(String.Format(FunCallFormatStrings.MOVE_SELECTED,
                lbRight.ClientID, lbLeft.ClientID));
            btnMoveRight.SetClientClickHandler(String.Format(FunCallFormatStrings.MOVE_SELECTED,
                lbLeft.ClientID, lbRight.ClientID));
            btnMoveAllLeft.SetClientClickHandler(String.Format(FunCallFormatStrings.MOVE_ALL,
                lbRight.ClientID, lbLeft.ClientID));
            btnMoveAllRight.SetClientClickHandler(String.Format(FunCallFormatStrings.MOVE_ALL,
                lbLeft.ClientID, lbRight.ClientID));
        }

        #endregion
    }
}
