using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

// ReSharper disable once CheckNamespace
namespace MMSINC.ClassExtensions.IQueryableExtensions
{
    public static class LambdaExpressionExtensions
    {
        #region Exposed Methods

        public static MemberExpression GetMemberExpression(this LambdaExpression expr)
        {
            return (MemberExpression)(expr.Body is MemberExpression ? expr.Body : ((UnaryExpression)expr.Body).Operand);
        }

        #endregion
    }

    public static class IQueryableExtensions
    {
        #region Private Methods

        private static Dictionary<string, PropertyInfo> ToPropDictionary<TEntity>(
            this Expression<Func<TEntity, object>>[] fields)
        {
            var ret = new Dictionary<string, PropertyInfo>();
            var bodies = fields.Select(f => {
                switch (f.Body)
                {
                    case MemberExpression e:
                        return e;
                    case UnaryExpression e:
                        return e.Operand;
                    case MethodCallExpression e:
                        throw new ArgumentException(
                            $"Each field specification must be a public property \"get\" mapped directly to sql as a column or formula.  '{f.Body}' is a method call.");
                    default:
                        throw new ArgumentException(
                            $"Not sure how to handle expression of type '{f.Body.GetType()}'");
                }
            });

            foreach (MemberExpression expression in bodies)
            {
                if (!ret.ContainsKey(expression.Member.Name))
                {
                    ret.Add(expression.Member.Name, expression.Member as PropertyInfo);
                }
            }

            return ret;
        }

        private static Expression BuildSourcePropertyExpression(string propertyName, ParameterExpression sourceItem)
        {
            Expression body = sourceItem;

            foreach (var member in propertyName.Split('.'))
            {
                body = Expression.Property(body, member);
            }

            return body;
        }

        private static MemberBinding ToMemberBinding(this KeyValuePair<string, MemberInfo> pi,
            ParameterExpression sourceItem,
            Dictionary<string, PropertyInfo> sourceProperties)
        {
            var attr =
                pi.Value.GetCustomAttributes().SingleOrDefault(a => a is SelectDynamicAttribute) as
                    SelectDynamicAttribute;

            return Expression.Bind(pi.Value, attr == null
                ? Expression.Property(sourceItem, sourceProperties[pi.Value.Name])
                : BuildSourcePropertyExpression($"{attr.Field ?? pi.Value.Name}.{attr.Dereference}", sourceItem));
        }

        private static DynamicSelectResult SelectDynamic(this IQueryable source, Type outputType,
            Dictionary<string, PropertyInfo> sourceProperties, Dictionary<string, MemberInfo> outputMembers)
        {
            var sourceItem = Expression.Parameter(source.ElementType, "t");
            var bindings = outputMembers.Select(pi => pi.ToMemberBinding(sourceItem, sourceProperties));

            Expression selector = Expression.Lambda(Expression.MemberInit(
                Expression.New(outputType.GetConstructor(Type.EmptyTypes)), bindings), sourceItem);

            var result = source.Provider.CreateQuery(Expression.Call(typeof(Queryable), "Select",
                new Type[] {source.ElementType, outputType},
                Expression.Constant(source), selector));

            return new DynamicSelectResult(outputType, outputMembers, result);
        }

        #endregion

        #region Exposed Methods

        private static Dictionary<string, PropertyInfo> ToPropDictionary(this string[] fields, Type elementType)
        {
            return fields.ToDictionary(name => name, elementType.GetProperty);
        }

        public static DynamicSelectResult SelectDynamic<TEntity>(this IQueryable<TEntity> source,
            params Expression<Func<TEntity, object>>[] fields)
        {
            return source.SelectDynamic(fields.ToPropDictionary());
        }

        public static DynamicSelectResult SelectDynamic<TEntity, TDisplay>(this IQueryable<TEntity> source)
        {
            return source.SelectDynamic<TDisplay>(typeof(TEntity)
                                                 .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                                 .Where(pi => pi.CanRead).ToDictionary(pi => pi.Name));
        }

        public static DynamicSelectResult SelectDynamic(this IQueryable source, params string[] fieldNames)
        {
            return source.SelectDynamic(fieldNames.ToPropDictionary(source.ElementType));
        }

        public static DynamicSelectResult SelectDynamic<TDisplay>(this IQueryable source,
            Dictionary<string, PropertyInfo> sourceProperties)
        {
            var outputType = typeof(TDisplay);
            var outputProps = outputType
                             .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                             .Where(pi => pi.CanWrite)
                             .ToDictionary(pi => pi.Name, pi => (MemberInfo)pi);

            return source.SelectDynamic(outputType, sourceProperties, outputProps);
        }

        public static DynamicSelectResult SelectDynamic(this IQueryable source,
            Dictionary<string, PropertyInfo> sourceProperties)
        {
            var outputType = LinqRuntimeTypeBuilder.GetDynamicType(sourceProperties.Values);
            var outputFields = outputType.GetFields().ToDictionary(fi => fi.Name, fi => (MemberInfo)fi);

            return source.SelectDynamic(outputType, sourceProperties, outputFields);
        }

        #endregion

        #region Nested Type: DynamicSelectResult

        public class DynamicSelectResult
        {
            #region Properties

            public IQueryable Result { get; }

            public Dictionary<string, MemberInfo> ResultProperties { get; }

            public Type ResultType { get; }

            #endregion

            #region Constructors

            public DynamicSelectResult(Type resultType, Dictionary<string, MemberInfo> resultProperties,
                IQueryable result)
            {
                ResultType = resultType;
                ResultProperties = resultProperties;
                Result = result;
            }

            #endregion
        }

        #endregion

        #region Nested Type: LinqRuntimeTypeBuilder

        public static class LinqRuntimeTypeBuilder
        {
            private static readonly AssemblyName assemblyName = new AssemblyName {Name = "DynamicLinqTypes"};
            private static readonly ModuleBuilder moduleBuilder = null;
            private static readonly Dictionary<string, Type> builtTypes = new Dictionary<string, Type>();

            #region Constructors

            static LinqRuntimeTypeBuilder()
            {
                moduleBuilder = Thread.GetDomain().DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run)
                                      .DefineDynamicModule(assemblyName.Name);
            }

            #endregion

            #region Private Methods

            private static string GetTypeKey(Dictionary<string, Type> fields)
            {
                //TODO: optimize the type caching -- if fields are simply reordered, that doesn't mean that they're actually different types, so this needs to be smarter
                var key = string.Empty;
                foreach (var field in fields)
                    key += field.Key + ";" + field.Value.Name + ";";

                return key;
            }

            private static string GetTypeKey(IEnumerable<PropertyInfo> fields)
            {
                return GetTypeKey(fields.ToDictionary(f => f.Name, f => f.PropertyType));
            }

            #endregion

            #region Exposed Methods

            public static Type GetDynamicType(Dictionary<string, Type> fields)
            {
                if (null == fields)
                    throw new ArgumentNullException("fields");
                if (0 == fields.Count)
                    throw new ArgumentOutOfRangeException("fields", "fields must have at least 1 field definition");

                try
                {
                    Monitor.Enter(builtTypes);
                    var className = GetTypeKey(fields);

                    if (builtTypes.ContainsKey(className))
                        return builtTypes[className];

                    var typeBuilder = moduleBuilder.DefineType(className,
                        TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.Serializable);

                    foreach (var field in fields)
                        typeBuilder.DefineField(field.Key, field.Value, FieldAttributes.Public);

                    builtTypes[className] = typeBuilder.CreateType();

                    return builtTypes[className];
                }
                finally
                {
                    Monitor.Exit(builtTypes);
                }
            }

            public static Type GetDynamicType(IEnumerable<PropertyInfo> fields)
            {
                return GetDynamicType(fields.ToDictionary(f => f.Name, f => f.PropertyType));
            }

            #endregion
        }

        #endregion
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class SelectDynamicAttribute : Attribute
    {
        public string Dereference { get; }
        public string Field { get; set; }

        public SelectDynamicAttribute(string dereference)
        {
            Dereference = dereference;
        }
    }
}
