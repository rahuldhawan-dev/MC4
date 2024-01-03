using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace MMSINC.ClassExtensions.ExpressionExtensions
{
    public static class ExpressionExtensions
    {
        public static PropertyInfo GetProperty<TObj, TVal>(this Expression<Func<TObj, TVal>> memberFn)
        {
            return InnerGetProperty(memberFn.Body);
        }

        private static PropertyInfo InnerGetProperty(Expression ex)
        {
            switch (ex)
            {
                case MemberExpression e:
                    return (PropertyInfo)e.Member;
                case UnaryExpression e:
                    return InnerGetProperty((MemberExpression)e.Operand);
                default:
                    throw new ArgumentException($"Not sure how to handle type '{ex.GetType().Name}'");
            }
        }

        public static PropertyInfo SetProperty<TObj, TVal>(this Expression<Func<TObj, TVal>> memberFn, TObj target,
            TVal value)
        {
            var prop = memberFn.GetProperty();
            prop.SetValue(target, value);
            return prop;
        }
    }
}
