using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC.ClassExtensions;

namespace MMSINC.Controls
{
    [Themeable(true)]
    public class MvpGridView : GridView, IGridView
    {
        #region Private Members

        private GridViewRow _headerRow, _footerRow;
        private MvpGridViewClientHandler _clientHandler;
        private bool _showEmptyTable = true;
        private SortDirection _sortDirection;
        private IGridViewRowCollection _iRows;
        private IGridViewRow _iFooterRow, _iHeaderRow;

        #endregion

        #region Properties

        [Obsolete("Use the wrapped (testable) IHeaderRow instead.")]
        public override GridViewRow HeaderRow
        {
            get { return _headerRow ?? base.HeaderRow; }
        }

        [Obsolete("Use the wrapped (testable) IFooterRow instead.")]
        public override GridViewRow FooterRow
        {
            get { return _footerRow ?? base.FooterRow; }
        }

        new public object SelectedDataKey
        {
            get { return base.SelectedDataKey; }
        }

        public bool ShowEmptyTable
        {
            get { return _showEmptyTable; }
            set { _showEmptyTable = value; }
        }

        public bool UseTHead { get; set; }
        public bool UseTFoot { get; set; }

        public string OnClientSelect { get; set; }

        public string OnClientDelete { get; set; }

        public string OnClientUpdate { get; set; }

        internal MvpGridViewClientHandler ClientHandler
        {
            get
            {
                if (_clientHandler == null)
                    _clientHandler = new MvpGridViewClientHandler(this);
                return _clientHandler;
            }
        }

        public IGridViewRowCollection IRows
        {
            get
            {
                if (_iRows == null)
                    _iRows = new GridViewRowCollectionWrapper(Rows);
                return _iRows;
            }
        }

        public IGridViewRow IFooterRow
        {
            get
            {
                if (_iFooterRow == null)
                    _iFooterRow = (FooterRow == null)
                        ? null
                        : new GridViewRowWrapper(FooterRow);
                return _iFooterRow;
            }
        }

        public IGridViewRow IHeaderRow
        {
            get
            {
                if (_iHeaderRow == null)
                    _iHeaderRow = (HeaderRow == null)
                        ? null
                        : new GridViewRowWrapper(HeaderRow);
                return _iHeaderRow;
            }
        }

        #endregion

        #region Private Methods

        protected override int CreateChildControls(IEnumerable dataSource, bool dataBinding)
        {
            var numRows = base.CreateChildControls(dataSource, dataBinding);

            if (numRows == 0 && ShowEmptyTable)
            {
                var table = new Table {
                    ID = ID
                };

                var fields = new DataControlField[Columns.Count];
                Columns.CopyTo(fields, 0);

                if (ShowHeader)
                {
                    _headerRow = CreateRow(-1, -1, DataControlRowType.Header,
                        DataControlRowState.Normal);

                    InitializeRow(_headerRow, fields);
                    table.Rows.Add(_headerRow);
                }

                var emptyRow = new GridViewRow(-1, -1,
                    DataControlRowType.EmptyDataRow, DataControlRowState.Normal);

                var cell = new TableCell {
                    ColumnSpan = Columns.Count,
                    Width = Unit.Percentage(100)
                };
                cell.Controls.Add(new LiteralControl(EmptyDataText ?? "&nbsp;"));

                if (EmptyDataTemplate != null)
                    EmptyDataTemplate.InstantiateIn(cell);

                emptyRow.Cells.Add(cell);
                table.Rows.Add(emptyRow);

                if (ShowFooter)
                {
                    _footerRow = CreateRow(-1, -1, DataControlRowType.Footer,
                        DataControlRowState.Normal);

                    InitializeRow(_footerRow, fields);
                    table.Rows.Add(_footerRow);
                }

                Controls.Clear();
                Controls.Add(table);
            }

            return numRows;
        }

        protected override void OnRowDataBound(GridViewRowEventArgs e)
        {
            base.OnRowDataBound(e);
            ClientHandler.AttachHandlersToRow(e);
        }

        private void MakeAccessible()
        {
            if (UseTHead && HeaderRow != null)
                HeaderRow.TableSection = TableRowSection.TableHeader;

            if (UseTFoot && FooterRow != null)
                FooterRow.TableSection = TableRowSection.TableFooter;
        }

        #endregion

        #region Event Handlers

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (UseAccessibleHeader)
            {
                MakeAccessible();
            }
        }

        public override int SelectedIndex
        {
            get { return base.SelectedIndex; }
            set
            {
                switch (SortDirection)
                {
                    case SortDirection.Descending:
                        base.SelectedIndex = Math.Abs(Rows.Count - 1 - value);
                        break;
                    default:
                        base.SelectedIndex = value;
                        break;
                }
            }
        }

        public override SortDirection SortDirection
        {
            get { return _sortDirection; }
        }

        #endregion

        #region Exposed Methods

        public void SetSortDirection(SortDirection direction)
        {
            _sortDirection = direction;
        }

        public TControl FindControl<TControl>(string id) where TControl : Control
        {
            return
                ControlExtensions.FindControl
                    <TControl>(this, id);
        }

        public TIControl FindIControl<TIControl>(string id) where TIControl : IControl
        {
            return
                ControlExtensions.FindIControl
                    <TIControl>((Control)this, id);
        }

        #endregion
    }
}
