using System;
using System.Linq.Expressions;
using MMSINC.Common;
using MMSINC.View;
using WorkOrders;
using WorkOrders.Model;
using WorkOrders.Views.RestorationAccountingCodes;

namespace LINQTo271.Views.RestorationAccountingCodes
{
    public partial class RestorationAccountingCodeSearchView : SearchView<RestorationAccountingCode>, IRestorationAccountingCodeSearchView
    {
        public override Expression<Func<RestorationAccountingCode, bool>> GenerateExpression()
        {
            return new ExpressionBuilder<RestorationAccountingCode>(BaseExpression);
        }
    }
}