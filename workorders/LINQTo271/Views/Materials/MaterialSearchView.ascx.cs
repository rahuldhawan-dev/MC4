using System;
using System.Linq.Expressions;
using MMSINC.Common;
using MMSINC.Controls;
using MMSINC.View;
using WorkOrders;
using WorkOrders.Model;
using WorkOrders.Views.Materials;

namespace LINQTo271.Views.Materials
{
    public partial class MaterialSearchView : SearchView<Material>, IMaterialSearchView
    {
        #region Control Declarations

        protected ITextBox txtPartNumber;
        protected ITextBox txtDescription;
        protected IDropDownList ddlActive;
        protected IDropDownList ddlDoNotOrder;

        #endregion

        #region Properties

        public string PartNumber { get { return txtPartNumber.Text; }}
        public string Description { get { return txtDescription.Text; }}
        public bool? Active
        {
            get {
                return String.IsNullOrWhiteSpace(ddlActive.SelectedValue)
                           ? (bool?)null : bool.Parse(ddlActive.SelectedValue);
            }
        }

        public bool? DoNotOrder
        {
            get
            {
                return String.IsNullOrWhiteSpace(ddlDoNotOrder.SelectedValue)
                           ? (bool?)null : bool.Parse(ddlDoNotOrder.SelectedValue);
            }
        }

        #endregion

        #region Exposed Methods

        public override Expression<Func<Material, bool>> GenerateExpression()
        {
            var builder = new ExpressionBuilder<Material>(BaseExpression);

            if (!String.IsNullOrWhiteSpace(PartNumber))
                builder.And(m => m.PartNumber.Contains(PartNumber));
            if (!String.IsNullOrWhiteSpace(Description))
                builder.And(m => m.Description.ToLower().Contains(Description.ToLower()));
            if (Active.HasValue)
                builder.And(m => m.IsActive == Active);
            if (DoNotOrder.HasValue)
                builder.And(m => m.DoNotOrder == DoNotOrder);
            return builder;
        }

        #endregion

    }
}