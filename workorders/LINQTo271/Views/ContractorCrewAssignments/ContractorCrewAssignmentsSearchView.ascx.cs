using System;
using System.Linq.Expressions;
using LINQTo271.Views.Abstract;
using LINQTo271.Views.CrewAssignments;
using MMSINC.Controls;
using MMSINC.Common;
using WorkOrders.Library.Permissions;
using WorkOrders.Model;
using WorkOrders.Views.CrewAssignments;
using SecurityServiceClass = WorkOrders.Library.Permissions.SecurityService;

namespace LINQTo271.Views.ContractorCrewAssignments
{
    /// <summary>
    /// Identical to CrewAssignmentsSearchView. Only difference is showing contractor crews.
    /// </summary>
    public partial class ContractorCrewAssignmentsSearchView : WorkOrdersSearchView<CrewAssignment>, ICrewAssignmentsSearchView
    {
        #region Constants

        public const string DATE_FORMAT = "MM-dd-yyyy";

        #endregion
        
        #region Control Declarations

        protected ITextBox ccDate;
        protected IDropDownList ddlCrew, ddlContractor;

        #endregion

        #region Private Members

        protected ISecurityService _securityService;

        #endregion

        #region Properties

        public DateTime Date
        {
            get
            {
                var date = ccDate.TryGetDateTimeValue();
                return (date == null) ? DateTime.Today : date.Value;
            }
            set { ccDate.Text = value.ToString(DATE_FORMAT); }
        }

        public int? CrewID
        {
            get { return ddlCrew.GetSelectedValue(); }
        }

        public Crew Crew
        {
            get { return CrewID == null ? null : CrewRepository.GetEntity(CrewID); }
        }

        protected ISecurityService SecurityService
        {
            get
            {
                if (_securityService == null)
                    _securityService = SecurityServiceClass.Instance;
                return _securityService;
            }
        }

        public override Expression<Func<CrewAssignment, bool>> BaseExpression
        {
            get
            {
                if (_baseExpression == null)
                    _baseExpression = GetBaseExpression();
                return _baseExpression;
            }
        }

        #endregion

        #region Private Methods

        private Expression<Func<CrewAssignment, bool>> GetBaseExpression()
        {
            // don't use base here because base does the opposite.
            var expr = PredicateBuilder.True<CrewAssignment>();
            expr =
                expr.And(
                    ca => ca.Crew.ContractorID != null);
            return expr;
        }

        #endregion

        #region Event Handlers

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (String.IsNullOrEmpty(ccDate.Text))
                ccDate.Text = Date.ToString(DATE_FORMAT);

            if (CrewID != null)
            {
                DataBindCrew(Crew);
            }
        }
        
        protected void cabmCrewAssignments_SelectedDateChanged(object sender, DateTimeEventArgs e)
        {
            Date = e.Date;

            if (CrewID != null)
            {
                DataBindCrew(Crew);
            }

            cabmCrewAssignments.VisibleDate = e.Date;
            btnSearch_Click(sender, e);
        }
        
        public void DataBindCrew(Crew crew)
        {
            cabmCrewAssignments.DataBind(crew, Date);
        }

        protected void ccDate_TextChanged(object sender, EventArgs e)
        {
            DateTime date;

            if (DateTime.TryParse(ccDate.Text, out date))
            {
                cabmCrewAssignments.SelectedDate = date;
                btnSearch_Click(sender, e);
            }
            else
            {
                Date = cabmCrewAssignments.SelectedDate;
            }
        }

        #endregion

        #region Exposed Methods

        public override Expression<Func<CrewAssignment, bool>>
            GenerateExpression()
        {
            var builder = new ExpressionBuilder<CrewAssignment>(BaseExpression);
            builder.And(ca => ca.AssignedFor.Date.Date == Date.Date);
            builder.And(ca => ca.CrewID == CrewID);
            return builder;
        }

        #endregion
    }
}