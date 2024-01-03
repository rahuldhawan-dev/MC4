using System;
using System.Linq.Expressions;
using MMSINC.Common;
using MMSINC.View;
using WorkOrders;
using WorkOrders.Model;
using WorkOrders.Views.RestorationProductCodes;

namespace LINQTo271.Views.RestorationProductCodes
{
    public partial class RestorationProductCodeSearchView : SearchView<RestorationProductCode>, IRestorationProductCodeSearchView
    {
        public override Expression<Func<RestorationProductCode, bool>> GenerateExpression()
        {
            return new ExpressionBuilder<RestorationProductCode>(BaseExpression);
        }
    }
}