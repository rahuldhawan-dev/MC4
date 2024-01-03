using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.ClassExtensions.StringExtensions;

namespace MMSINC.Utilities
{
    /// <summary>
    /// Static helper functions for working with Expression objects.
    /// </summary>
    public static class Expressions
    {
        #region Consts

        private const BindingFlags FIND_EVERYTHING_FLAG =
            (BindingFlags.FlattenHierarchy | BindingFlags.Public |
             BindingFlags.NonPublic | BindingFlags.Instance |
             BindingFlags.Static);

        private const string LAMBDA_PARAMETER_NAME = "x";

        private static readonly Type _emptyTwoParamFuncType = typeof(Func<,>),
                                     _emptyTwoParamActionType = typeof(Action<,>);

        #endregion

        #region Private methods

        private static Expression BuildLambdaExpression(Expression propertyExpression, Type classType,
            Type propertyType, Type genericType, params ParameterExpression[] lambaParamExpr)
        {
            var genericFunc = genericType.MakeGenericType(classType, propertyType);
            return Expression.Lambda(genericFunc, propertyExpression, lambaParamExpr);
        }

        /// <summary>
        /// In an expression like (x => x.Some.Property), this is returning the "x.Some.Property" part. 
        /// By itself it's not really useful.
        /// </summary>
        private static Expression BuildRawGetterExpression(Type classType, string propertyExpression,
            ParameterExpression lambdaParamExpr)
        {
            propertyExpression.ThrowIfNullOrWhiteSpace("propertyExpression");
            var props = propertyExpression.Split('.');
            var propertyType = classType;
            var exprArg = lambdaParamExpr;
            Expression finalExpression = exprArg;
            foreach (var propName in props)
            {
                var pi = FindProperty(propertyType, propName, propertyExpression);
                finalExpression = Expression.Property(finalExpression, pi);
                propertyType = pi.PropertyType;
            }

            return finalExpression;
        }

        public static Expression BuildSetterExpression(Type classType, Type propertyType, string propertyExpression,
            ParameterExpression lambaParamExpr)
        {
            var props = propertyExpression.Split('.');
            Expression expr = lambaParamExpr;
            var curPropType = classType;
            foreach (var prop in props.Take(props.Length - 1))
            {
                // use reflection (not ComponentModel) to mirror LINQ 
                var pi = FindProperty(curPropType, prop, propertyExpression);
                expr = Expression.Property(expr, pi);
                curPropType = pi.PropertyType;
            }

            // final property set...
            var valArg = Expression.Parameter(propertyType, "val");
            var finalProp = FindProperty(curPropType, props.Last(), propertyExpression);

            Expression setterExpressionValArg = valArg;
            if (finalProp.PropertyType != propertyType)
            {
                // We only wanna cast if the final property type is different from the propertyType argument.
                // This way we can create an expression that will accept something like the object type without
                // throwing an error, but will still throw an error if the compiled form tries to set a value
                // with the wrong type.
                setterExpressionValArg = Expression.Convert(valArg, finalProp.PropertyType);
            }

            expr = Expression.Call(expr, finalProp.GetSetMethod(), setterExpressionValArg);
            return BuildLambdaExpression(expr, classType, propertyType, _emptyTwoParamActionType, lambaParamExpr,
                valArg);
        }

        private static PropertyInfo FindProperty(Type type, string propertyName, string fullExpressionPath)
        {
            var prop = type.GetProperty(propertyName);
            if (prop != null)
            {
                return prop;
            }

            throw ExceptionHelper.Format<InvalidOperationException>(
                "Unable to find property '{0}' used by the expression '{1}' from type '{2}'", propertyName,
                fullExpressionPath, type);
        }

        private static MemberInfo FromMemberExpression(MemberExpression exp)
        {
            var mem = exp.Member;

            // Can't do a test on type if the expression is null, which will
            // happen if you're attemping to get a static member as it won't
            // require passing an argument for the expression ie: () => {} instead of (arg) => {}
            if (exp.Expression == null)
            {
                return mem;
            }

            // Means we aren't dealing with a derived type using a base property(either virtual or not),
            // so we can return the exp.Member without further reflection.
            if (mem.ReflectedType == exp.Expression.Type)
            {
                return mem;
            }

            // I don't know if this will work with overloads properly.
            return exp.Expression.Type.GetMember(mem.Name, mem.MemberType, FIND_EVERYTHING_FLAG).Single();
        }

        private static MethodInfo FromMethodCallExpression(MethodCallExpression exp)
        {
            var meth = exp.Method;
            if (exp.Object == null)
            {
                return meth;
            }

            if (meth.ReflectedType == exp.Object.Type)
            {
                // Means we aren't dealing with a derived type using a base method(either virtual or not),
                // so we can return the exp.Member without further reflection.
                return meth;
            }

            var paramTypes = meth.GetParameters().Select(p => p.ParameterType).ToArray();
            return exp.Object.Type.GetMethod(meth.Name, FIND_EVERYTHING_FLAG, null, paramTypes, null);
        }

        #endregion

        #region Public methods

        #region BuildGetterExpression

        /// <summary>
        /// Creates an expression for retrieving a property of unknown type based on the string representation of the accessor.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyExpression"></param>
        /// <returns></returns>
        public static Expression<Func<T, object>> BuildGetterExpression<T>(string propertyExpression)
        {
            return (Expression<Func<T, object>>)BuildGetterExpression(typeof(T), typeof(object), propertyExpression);
        }

        /// <summary>
        /// Creates an expression for retrieving a property value based on the string representation of the accessor.
        /// </summary>
        /// <typeparam name="T">The type that owns the property(or in the case of a nested property, the first property in the path)</typeparam>
        /// <typeparam name="TValue">The type returned by the property</typeparam>
        /// <param name="propertyExpression">The name of the property, or if nested, The.Path.To.Property</param>
        public static Expression<Func<T, TValue>> BuildGetterExpression<T, TValue>(string propertyExpression)
        {
            return (Expression<Func<T, TValue>>)BuildGetterExpression(typeof(T), propertyExpression);
        }

        /// <summary>
        /// Creates an expression for retrieving a property value based on the string representation of the accessor.
        /// The Expression returned is the equivalent of Func(T, TPropertyType)(x => x.Prop)
        /// </summary>
        /// <param name="classType">The type that owns the property(or in the case of a nested property, the first property in the path)</param>
        /// <param name="propertyExpression">The name of the property, or if nested, The.Path.To.Property</param>
        /// <returns></returns>
        public static Expression BuildGetterExpression(Type classType, string propertyExpression)
        {
            // Not using the other BuildGetterExpression method so we're not adding the
            // additional cost of casting to the mix.
            var paramExpr = Expression.Parameter(classType, LAMBDA_PARAMETER_NAME);
            var expr = (MemberExpression)BuildRawGetterExpression(classType, propertyExpression, paramExpr);
            return BuildLambdaExpression(expr, classType, expr.Type, _emptyTwoParamFuncType, paramExpr);
        }

        /// <summary>
        /// Creates an expression for retrieving a property value based on the string representation of the accessor. The type
        /// The Expression returned is the equivalent of Func(T, CastedPropertyType)(x => (CastedPropertyType)x.Prop).
        /// </summary>
        /// <param name="classType">The type that owns the property(or in the case of a nested property, the first property in the path)</param>
        /// <param name="propertyType">The type </param>
        /// <param name="propertyExpression">The name of the property, or if nested, The.Path.To.Property</param>
        /// <returns></returns>
        public static Expression BuildGetterExpression(Type classType, Type propertyType, string propertyExpression)
        {
            var paramExpr = Expression.Parameter(classType, LAMBDA_PARAMETER_NAME);
            var expr = BuildRawGetterExpression(classType, propertyExpression, paramExpr);
            expr = Expression.Convert(expr, propertyType);
            return BuildLambdaExpression(expr, classType, propertyType, _emptyTwoParamFuncType, paramExpr);
        }

        #endregion

        #region BuildSetterExpression

        /// <summary>
        /// Creates an expression for setting a property value based on the string representation of the getter. The type
        /// returned is an Action(T, Object), but internally will cast the Object value to the type required by the property.
        /// </summary>
        /// <typeparam name="T">The type that owns the property(or in the case of a nested property, the first property in the path)</typeparam>
        /// <param name="propertyExpression">The name of the property, or if nested, The.Path.To.Property</param>
        /// <returns></returns>
        public static Expression<Action<T, object>> BuildSet<T>(string propertyExpression)
        {
            // var paramExpr = Expression.Parameter(typeof(T), LAMBDA_PARAMETER_NAME);
            return (Expression<Action<T, object>>)BuildSetterExpression(typeof(T), typeof(object), propertyExpression);
        }

        /// <summary>
        /// Creates an expression for setting a property value based on the string representation of the getter. The type
        /// returned is an Action(T, TPropertyType).
        /// </summary>
        /// <typeparam name="T">The type that owns the property(or in the case of a nested property, the first property in the path)</typeparam>
        /// <typeparam name="TValue">The type of the property value.</typeparam>
        /// <param name="propertyExpression">The name of the property, or if nested, The.Path.To.Property</param>
        /// <returns></returns>
        public static Expression<Action<T, TValue>> BuildSet<T, TValue>(string propertyExpression)
        {
            //  var paramExpr = Expression.Parameter(typeof(T), LAMBDA_PARAMETER_NAME);
            return (Expression<Action<T, TValue>>)BuildSetterExpression(typeof(T), typeof(TValue), propertyExpression);
        }

        /// <summary>
        /// Creates an expression for setting a property value based on the string representation of the getter. The type
        /// returned is an Action(T, Object), but internally will cast the Object value to the type required by the property.
        /// </summary>
        /// <param name="classType">The type that owns the property(or in the case of a nested property, the first property in the path)</param>
        /// <param name="propertyType"></param>
        /// <param name="propertyExpression">The name of the property, or if nested, The.Path.To.Property</param>
        /// <returns></returns>
        public static Expression BuildSetterExpression(Type classType, Type propertyType, string propertyExpression)
        {
            var paramExpr = Expression.Parameter(classType, LAMBDA_PARAMETER_NAME);
            return BuildSetterExpression(classType, propertyType, propertyExpression, paramExpr);
            //return BuildSetterExpression(classType, propertyType, propertyExpression, 
            //var props = propertyExpression.Split('.');
            //Expression expr = lambaParamExpr;
            //var curPropType = classType;
            //foreach (var prop in props.Take(props.Length - 1))
            //{
            //    // use reflection (not ComponentModel) to mirror LINQ 
            //    var pi = FindProperty(curPropType, prop, propertyExpression);
            //    expr = Expression.Property(expr, pi);
            //    curPropType = pi.PropertyType;
            //}
            //// final property set...
            //var valArg = Expression.Parameter(propertyType, "val");
            //var finalProp = FindProperty(curPropType, props.Last(), propertyExpression);

            //Expression setterExpressionValArg = valArg;
            //if (finalProp.PropertyType != propertyType)
            //{     
            //    // We only wanna cast if the final property type is different from the propertyType argument.
            //    // This way we can create an expression that will accept something like the object type without
            //    // throwing an error, but will still throw an error if the compiled form tries to set a value
            //    // with the wrong type.
            //    setterExpressionValArg = Expression.Convert(valArg, finalProp.PropertyType);
            //}
            //expr = Expression.Call(expr, finalProp.GetSetMethod(), setterExpressionValArg);  
            //return BuildLambdaExpression(expr, classType, propertyType, _emptyTwoParamActionType, lambaParamExpr, valArg);
        }

        #endregion

        #region GetMember

        /// <summary>
        /// Expressions.GetMember((SomeClass s) => s.InstanceProperty)
        /// Expressions.GetMember((SomeClass s) => s.InstanceMethod())
        /// </summary>
        public static MemberInfo GetMember<TObject, TMember>(Expression<Func<TObject, TMember>> exp)
        {
            exp.ThrowIfNull("expression");
            return GetMember(exp.Body);
        }

        /// <summary>
        /// Expressions.GetMember(() => new SomeClass().SomeProperty);
        /// Expressions.GetMember(() => SomeClass.StaticProperty)
        /// </summary>
        public static MemberInfo GetMember<TMember>(Expression<Func<TMember>> exp)
        {
            exp.ThrowIfNull("expression");
            return GetMember(exp.Body);
        }

        /// <summary>
        /// Expressions.GetMember(() => SomeClass.SomeStaticVoidWithArgs(null))
        /// </summary>
        public static MemberInfo GetMember(Expression<Action> exp)
        {
            exp.ThrowIfNull("expression");
            return GetMember(exp.Body);
        }

        public static MemberInfo GetMember(Expression exp)
        {
            exp.ThrowIfNull("expression");

            // Do something here that checks that the member ReflectedType
            // is the same as the Expression.Body type. If it's not, we need
            // to reflect from the derived type manually. 

            switch (exp.NodeType)
            {
                case ExpressionType.MemberAccess:
                    return FromMemberExpression((MemberExpression)exp);
                case ExpressionType.Call:
                    return FromMethodCallExpression((MethodCallExpression)exp);

                case ExpressionType.Parameter:

                // This is stuff that I had in my original VB class, but
                // I'm commenting it out since we probably will never use it.
                // -Ross 11/30/11
                case ExpressionType.Convert:
                    return GetMemberName((UnaryExpression)exp);

                //  case ExpressionType.Parameter:
                //  case ExpressionType.Constant:
                //      return string.Empty;

                default:
                    throw new NotSupportedException(string.Format("The expression NodeType '{0}' is not supported.",
                        exp.NodeType));
            }
        }

        #endregion

        public static MemberInfo GetMemberName(UnaryExpression exp)
        {
            return ((MemberExpression)exp.Operand).Member;
        }

        #region GetSortExpression

        public static Expression<Func<T, object>> GetSortExpression<T>(string sortBy)
        {
            if (string.IsNullOrWhiteSpace(sortBy))
                return null;

            var type = typeof(T);
            if (type.GetProperty(sortBy) == null) // property doesn't exist, fail gracefully
                return null;

            var parameter = Expression.Parameter(type, sortBy);
            var property = Expression.Property(parameter, sortBy);
            var orderByExpression = Expression.Lambda<Func<T, object>>(
                Expression.Convert(
                    property,
                    typeof(object)),
                parameter);

            return orderByExpression;
        }

        #endregion

        #region IsSupportedExpression

        // There should be one of these for each GetMember overload.

        /// <summary>
        /// Returns true if GetMember should return a result without throwing an exception.
        /// </summary>
        public static bool IsSupportedExpression<TObject, TMember>(Expression<Func<TObject, TMember>> exp)
        {
            exp.ThrowIfNull("expression");
            return IsSupportedExpression(exp.Body);
        }

        /// <summary>
        /// Returns true if GetMember should return a result without throwing an exception.
        /// </summary>
        public static bool IsSupportedExpression<TMember>(Expression<Func<TMember>> exp)
        {
            exp.ThrowIfNull("expression");
            return IsSupportedExpression(exp.Body);
        }

        /// <summary>
        /// Returns true if GetMember should return a result without throwing an exception.
        /// </summary>
        public static bool IsSupportedExpression(Expression<Action> exp)
        {
            exp.ThrowIfNull("expression");
            return IsSupportedExpression(exp.Body);
        }

        /// <summary>
        /// Returns true if GetMember should return a result without throwing an exception.
        /// </summary>
        public static bool IsSupportedExpression(Expression exp)
        {
            exp.ThrowIfNull("expression");

            try
            {
                GetMember(exp);
                return true;
            }
            catch (NotSupportedException)
            {
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #endregion
    }
}
