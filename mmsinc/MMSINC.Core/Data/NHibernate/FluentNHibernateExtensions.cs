using System;
using System.Configuration;
using System.Linq;
using FluentNHibernate.Cfg;
using FluentNHibernate.Mapping;
using MMSINC.ClassExtensions.StringExtensions;

namespace MMSINC.Data.NHibernate
{
    public static class FluentNHibernateExtensions
    {
        public const string DATABASE_TYPE_CONFIGURATION_KEY = "DatabaseType";

        public static bool IsSqlite()
        {
            return ConfigurationManager.AppSettings[DATABASE_TYPE_CONFIGURATION_KEY] == "sqlite";
        }

        public static ManyToOnePart<TOther> DbSpecificFormula<TOther>(this ManyToOnePart<TOther> that,
            string sqlServerFormula, string sqliteFormula)
        {
            return that.Formula(IsSqlite() ? sqliteFormula : sqlServerFormula);
        }

        public static PropertyPart DbSpecificFormula(this PropertyPart that, string sqlServerFormula,
            string sqliteFormula)
        {
            return that.Formula(IsSqlite() ? sqliteFormula : sqlServerFormula);
        }

        public static PropertyPart DbSpecificFormula(this PropertyPart that, string sqlServerFormula,
            Func<string, string> fn)
        {
            return DbSpecificFormula(that, sqlServerFormula, fn(sqlServerFormula));
        }

        public static PropertyPart DbSpecificFormula(this PropertyPart that, string sqlServerFormula)
        {
            return DbSpecificFormula(that, sqlServerFormula, sqlServerFormula.ToSqlite());
        }

        public static OneToManyPart<T> DbSpecificSubselect<T>(this OneToManyPart<T> that, string sqlServerFormula,
            string sqliteFormula)
        {
            return that.Subselect(IsSqlite() ? sqliteFormula : sqlServerFormula);
        }

        /// <summary>
        /// Maps a property as an nvarchar(max) column. text/ntext is deprecated in newer versions of SQL Server.
        /// </summary>
        /// <param name="that"></param>
        /// <returns></returns>
        public static PropertyPart AsTextField(this PropertyPart that)
        {
            var customType = IsSqlite() ? "text" : "nvarchar(max)";
            return that.CustomType("StringClob").CustomSqlType(customType);
        }

        public static FluentMappingsContainer AddDynamicMapping<TAssemblyOf>(this FluentMappingsContainer mappings)
        {
            var iDynamicMapTypeProviderType = typeof(IDynamicMapTypeProvider);
            var assembly = typeof(TAssemblyOf).Assembly;
            var dynamicMappers =
                assembly.GetTypes().Where(x => x.GetInterfaces().Contains(iDynamicMapTypeProviderType));
            foreach (var type in dynamicMappers)
            {
                var instance = (IDynamicMapTypeProvider)Activator.CreateInstance(type);
                foreach (var dynamicallyMappedType in instance.GetMapTypesForAssembly(assembly))
                {
                    mappings.Add(dynamicallyMappedType);
                }
            }

            return mappings;
        }
    }
}
