using System;
using System.Linq.Expressions;
using LINQTo271.Views.Abstract;
using MMSINC.Common;
using MMSINC.Controls;
using WorkOrders.Model;
using WorkOrders.Views.SpoilFinalProcessingLocations;

namespace LINQTo271.Views.SpoilFinalProcessingLocations
{
    public partial class SpoilFinalProcessingLocationSearchView : WorkOrdersSearchView<SpoilFinalProcessingLocation>, ISpoilFinalProcessingLocationSearchView
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

        public override Expression<Func<SpoilFinalProcessingLocation, bool>> GenerateExpression()
        {
            return new ExpressionBuilder<SpoilFinalProcessingLocation>(BaseExpression)
                .And(sl => sl.OperatingCenterID == OperatingCenterID);
        }

        #endregion
    }
}