using System;
using FluentMigrator.Builders;
using FluentMigrator.Infrastructure;

namespace MMSINC.Data.FluentMigrator
{
    public static class IColumnOptionSyntaxExtensions
    {
        /// <summary>
        /// Create a foreign key named FK_LocalTable_ForeignTable_LocalColumnName (ex. FK_Users_Groups_MainGroupId).
        /// Assumes that the foreign primary key is called "Id", do not use this method if that is not the case.
        /// </summary>
        /// <param name="foreignTableName">Name of the foreign table being referenced.</param>
        public static TNextFk ForeignKey<TNext, TNextFk>(this IColumnOptionSyntax<TNext, TNextFk> that,
            string foreignTableName)
            where TNext : IFluentSyntax
            where TNextFk : IFluentSyntax
        {
            dynamic blah = that;
            var tableName = blah.TableName;
            var columnName = blah.Column.Name;

            return that.ForeignKey(String.Format("FK_{0}_{1}_{2}", tableName, foreignTableName, columnName),
                foreignTableName, "Id");
        }
    }
}
