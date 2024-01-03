using System;
using System.Linq.Expressions;
using MMSINC.Common;
using MMSINC.Controls;
using MMSINC.View;
using WorkOrders;
using WorkOrders.Model;
using WorkOrders.Views.SpoilRemovals;

namespace LINQTo271.Views.SpoilRemovals
{
    public partial class SpoilRemovalSearchView : SearchView<SpoilRemoval>, ISpoilRemovalSearchView
    {
        #region Control Declarations

        protected IDropDownList ddlOperatingCenter;

        #endregion

        #region Properties

        public int OperatingCenterID
        {
            get
            {
                if (ddlOperatingCenter.Items.Count == 0)
                    ddlOperatingCenter.DataBind();

                return int.Parse(ddlOperatingCenter.SelectedValue);
            }
        }

        #endregion

        #region Exposed Methods

        public override Expression<Func<SpoilRemoval, bool>> GenerateExpression()
        {
            var builder = new ExpressionBuilder<SpoilRemoval>(BaseExpression);
            builder.And(sr => sr.RemovedFrom.OperatingCenterID == OperatingCenterID);
            return builder;
        }

        #endregion
    }
}