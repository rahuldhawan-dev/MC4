using System;
using System.IO;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.UI;
using LINQTo271.Common;
using MMSINC.Common;
using MMSINC.Controls;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using StructureMap;
using WorkOrders;
using WorkOrders.Library.Controls;
using WorkOrders.Library.Permissions;
using WorkOrders.Model;
using SecurityServiceClass = WorkOrders.Library.Permissions.SecurityService;

namespace LINQTo271.Views.Abstract
{
    public class WorkOrdersReport : WorkOrdersMvpPage
    {
        #region Constants

        public const string RESPONSE_HEADER_NAME = "content-disposition";
        public const string RESPONSE_HEADER_VALUE = "attachment;filename=Data.xls";

        public struct ViewStateKeys
        {
            public const string SORT_EXPRESSION = "_sortExpression",
                                SORT_DIRECTION = "_sortDirection";
        }

        #endregion
        
        #region Control Declarations

        protected IButton btnSearch, btnReturnToSearch;
        protected IPanel pnlSearch, pnlResults;
        protected IGridView gvSearchResults;
        protected ISecurityService _securityService;

        #endregion

        #region Private Members

        protected StringWriter _reportStringWriter;
        protected HtmlTextWriter _reportHtmlTextWriter;
        protected IViewState _iViewState;
        protected IRepository<ReportViewing> _reportViewingRepository;

        #endregion

        #region Properties

        public ISecurityService SecurityService
        {
            get
            {
                if (_securityService == null)
                    _securityService = SecurityServiceClass.Instance;
                return _securityService;
            }
        }

        public virtual IViewState IViewState
        {
            get
            {
                if (_iViewState == null)
                    _iViewState = new ViewStateWrapper(ViewState);
                return _iViewState;
            }
        }

        #if DEBUG && !LOCAL
        // There wont be an actual user unless your logged in. 
        // This is why we need to do this.
        public override IUser IUser
        {
            get
            {
                if (_iUser == null)
                    _iUser = base.IUser ?? new DebuggingUser();
                return _iUser;
            }
        }
        #endif

        public StringWriter ReportStringWriter
        {
            get
            {
                if (_reportStringWriter==null)
                    _reportStringWriter = new StringWriter();
                return _reportStringWriter;
            }
        }

        public HtmlTextWriter ReportHtmlTextWriter
        {
            get
            {
                if(_reportHtmlTextWriter==null)
                    _reportHtmlTextWriter = new HtmlTextWriter(ReportStringWriter);
                return _reportHtmlTextWriter;
            }
        }

        public string SortExpression
        {
            get
            {
                return (IViewState.GetValue(ViewStateKeys.SORT_EXPRESSION) ?? string.Empty).ToString();
            }
            set
            {
                IViewState.SetValue(ViewStateKeys.SORT_EXPRESSION, value);
            }
        }

        public string SortDirection
        {
            get
            {
                return (IViewState.GetValue(ViewStateKeys.SORT_DIRECTION) ?? string.Empty).ToString();
            }
            set
            {
                IViewState.SetValue(ViewStateKeys.SORT_DIRECTION, value);
            }
        }

        public IRepository<ReportViewing> ReportViewingRepository
        {
            get
            {
                if (_reportViewingRepository == null)
                    _reportViewingRepository =
                        DependencyResolver.Current.GetService<IRepository<ReportViewing>>();
                return _reportViewingRepository;
            }
        }

        #endregion

        #region Private Methods

        internal void ApplyCompletedDateFilter(DateRange dr, ExpressionBuilder<WorkOrder> builder)
        {
            switch (dr.SelectedOperator)
            {
                case "=":
                    builder.And(wo => wo.DateCompleted == dr.Date);
                    break;
                case ">":
                    builder.And(
                        wo =>
                        DateTime.Compare(wo.DateCompleted.Value,
                           dr.Date.Value) > 0);
                    break;
                case ">=":
                    builder.And(
                        wo =>
                        DateTime.Compare(wo.DateCompleted.Value,
                            dr.Date.Value) >= 0);
                    break;
                case "<":
                    builder.And(
                        wo =>
                        DateTime.Compare(wo.DateCompleted.Value,
                            dr.Date.Value) < 0);
                    break;
                case "<=":
                    builder.And(
                        wo =>
                        DateTime.Compare(wo.DateCompleted.Value,
                            dr.Date.Value) <= 0);
                    break;
                case "BETWEEN":
                    builder.And(
                        wo =>
                        (DateTime.Compare(wo.DateCompleted.Value, dr.StartDate.Value) >= 0 &&
                         DateTime.Compare(wo.DateCompleted.Value, dr.EndDate.Value) <= 0));
                    break;

            }
        }

        internal virtual string GetSortExpression(string newSortExpression)
        {
            if (SortExpression == newSortExpression)
            {
                SortDirection = SortDirection == "asc" ? "desc" : "asc";
            }
            else
            {
                SortExpression = newSortExpression;
                SortDirection = "asc";
            }
            return String.Format("{0} {1}", SortExpression, SortDirection);
        }

        internal void RecordViewing()
        {
            ReportViewingRepository.InsertNewEntity(new ReportViewing {
                EmployeeID = SecurityService.Employee.EmployeeID,
                DateViewed = DateTime.Now,
                ReportName = GetReportName()
            });
        }

        private string GetReportName()
        {
            return new Regex("/([^/]+)\\..+$").Match(IRequest.Url).Groups[1].ToString();
        }

        protected void ExportToExcel()
        {
            gvSearchResults.AllowSorting = false;
            DataBindResults();
            IResponse.Clear();
            IResponse.AddHeader(RESPONSE_HEADER_NAME, RESPONSE_HEADER_VALUE);

            gvSearchResults.RenderControl(ReportHtmlTextWriter);
            IResponse.Write(ReportStringWriter.ToString());
            IResponse.End();
        }

        /// <summary>
        /// DataBinds gvSearchResults.  Override as necessary to work with
        /// data sources directly.
        /// </summary>
        protected virtual void DataBindResults()
        {
            gvSearchResults.DataBind();
        }

        #endregion

        #region Event Handlers

        protected void Page_Init(object sender, EventArgs e)
        {
            SecurityService.Init(IUser);
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            ExportToExcel();
        }

        protected virtual void btnSearch_Click(object sender, EventArgs e)
        {
            pnlSearch.Visible = false;
            pnlResults.Visible = true;
            DataBindResults();
            RecordViewing();
        }

        protected virtual void btnReturnToSearch_Click(object sender, EventArgs e)
        {
            pnlResults.Visible = false;
            pnlSearch.Visible = true;
        }

        #endregion

        #region Exposed Methods

        public override void VerifyRenderingInServerForm(Control control)
        {
            return;
        }

        #endregion
    }
}
