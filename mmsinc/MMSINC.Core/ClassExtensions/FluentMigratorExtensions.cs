using System;
using System.Data;
using FluentMigrator.Builders;
using FluentMigrator.Builders.Alter.Column;
using FluentMigrator.Builders.Alter.Table;
using FluentMigrator.Builders.Create;
using FluentMigrator.Builders.Create.Column;
using FluentMigrator.Builders.Create.Table;
using FluentMigrator.Builders.Delete;
using FluentMigrator.Builders.Execute;
using FluentMigrator.Builders.Insert;
using FluentMigrator.Infrastructure;
using Humanizer;
using JetBrains.Annotations;

// ReSharper disable CheckNamespace
namespace MMSINC.ClassExtensions.FluentMigratorExtensions
    // ReSharper restore CheckNamespace
{
    public class Utilities
    {
        #region Exposed Methods

        public static string CreateForeignKeyName(string table, string foreignTable, string column)
        {
            return String.Format("FK_{0}_{1}_{2}", table, foreignTable, column);
        }

        #endregion
    }

    public static class IExecuteExpressionRootExtensions
    {
        #region Exposed Methods

        [Obsolete("DO NOT USE: If you only have one format parameter, a built-in method on IExecuteExpressionRoot will be used instead of the static method and it doesn't cause a compile-time error.")]
        [StringFormatMethod("format")]
        public static void Sql(this IExecuteExpressionRoot that, string format, params object[] args)
        {
            // ReSharper disable RedundantStringFormatCall
            that.Sql(String.Format(format, args));
            // ReSharper restore RedundantStringFormatCall
        }

        public static void Command(this IExecuteExpressionRoot that, string commandText, Action<IDbCommand> fn)
        {
            that.WithConnection((conn, tran) => {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tran;
                    cmd.CommandText = commandText;

                    fn(cmd);
                }
            });
        }

        #endregion
    }

    public static class IAlterTableAddColumnOrAlterColumnOrSchemaSyntaxExtensions
    {
        #region Exposed Methods

        public static string GetTableName(this IAlterTableAddColumnOrAlterColumnOrSchemaSyntax that)
        {
            return ((AlterTableExpressionBuilder)that).Expression.TableName;
        }

        public static IAlterTableColumnOptionOrAddColumnOrAlterColumnSyntax AddForeignKeyColumn(
            this IAlterTableAddColumnOrAlterColumnOrSchemaSyntax that, string columnName, string foreignTable,
            string foreignId = "Id", bool nullable = true)
        {
            var alter =
                that.AddColumn(columnName)
                    .AsInt32()
                    .ForeignKey(Utilities.CreateForeignKeyName(that.GetTableName(), foreignTable, columnName),
                         foreignTable, foreignId);

            return nullable ? alter.Nullable() : alter.NotNullable();
        }

        public static IAlterTableColumnOptionOrAddColumnOrAlterColumnSyntax AlterForeignKeyColumn(
            this IAlterTableAddColumnOrAlterColumnOrSchemaSyntax that,
            string columnName, string foreignTable, string foreignId = "Id", bool nullable = true)
        {
            var alter = that.AlterColumn(columnName)
                            .AsInt32()
                            .ForeignKey(Utilities.CreateForeignKeyName(that.GetTableName(), foreignTable, columnName),
                                 foreignTable,
                                 foreignId);

            return nullable ? alter.Nullable() : alter.NotNullable();
        }

        #endregion
    }

    public static class IAlterTableColumnOptionOrAddColumnOrAlterColumnSyntaxExtensions
    {
        #region Exposed Methods

        public static string GetTableName(this IAlterTableColumnOptionOrAddColumnOrAlterColumnSyntax that)
        {
            return ((AlterTableExpressionBuilder)that).Expression.TableName;
        }

        public static IAlterTableColumnOptionOrAddColumnOrAlterColumnSyntax AlterForeignKeyColumn(
            this IAlterTableColumnOptionOrAddColumnOrAlterColumnSyntax that,
            string columnName, string foreignTable, string foreignId = "Id", bool nullable = true)
        {
            var alter = that.AlterColumn(columnName)
                            .AsInt32()
                            .ForeignKey(Utilities.CreateForeignKeyName(that.GetTableName(), foreignTable, columnName),
                                 foreignTable,
                                 foreignId);

            return nullable ? alter.Nullable() : alter.NotNullable();
        }

        public static IAlterTableColumnOptionOrAddColumnOrAlterColumnSyntax AddForeignKeyColumn(
            this IAlterTableColumnOptionOrAddColumnOrAlterColumnSyntax that,
            string columnName, string foreignTable, string foreignId = "Id", bool nullable = true)
        {
            var alter = that.AddColumn(columnName).AsInt32()
                            .ForeignKey(Utilities.CreateForeignKeyName(that.GetTableName(), foreignTable, columnName),
                                 foreignTable, foreignId);

            return nullable ? alter.Nullable() : alter.NotNullable();
        }

        #endregion
    }

    public static class IDeleteExpressionRootExtensions
    {
        #region Exposed Methods

        public static void ForeignKeyColumn(this IDeleteExpressionRoot that, string tableName, string columnName,
            string foreignTable, string foreignId = "Id")
        {
            that.ForeignKey(Utilities.CreateForeignKeyName(tableName, foreignTable, columnName)).OnTable(tableName);

            that.Column(columnName).FromTable(tableName);
        }

        public static void ManyToManyTable(this IDeleteExpressionRoot that, string firstTableName,
            string secondTableName)
        {
            that.Table(firstTableName + secondTableName);
        }

        #endregion
    }

    public static class ICreateTableWithColumnOrSchemaSyntaxExtensions
    {
        #region Exposed Methods

        public static string GetTableName(this ICreateTableWithColumnOrSchemaSyntax that)
        {
            return ((CreateTableExpressionBuilder)that).Expression.TableName;
        }

        public static ICreateTableColumnOptionOrWithColumnSyntax WithIdentityColumn(
            this ICreateTableWithColumnOrSchemaSyntax that, string name = "Id")
        {
            return that.WithColumn(name).AsInt32().PrimaryKey().Identity().NotNullable().Unique();
        }

        public static ICreateTableColumnOptionOrWithColumnSyntax WithForeignKeyColumn(
            this ICreateTableWithColumnOrSchemaSyntax that, 
            string columnName, 
            string foreignTable,
            string foreignId = "Id", 
            bool nullable = true, 
            string foreignKeyName = "")
        {
            var keyName = string.IsNullOrEmpty(foreignKeyName)
                ? Utilities.CreateForeignKeyName(that.GetTableName(), foreignTable, columnName)
                : foreignKeyName;

            var with = that.WithColumn(columnName)
                           .AsInt32()
                           .ForeignKey(keyName, foreignTable, foreignId);

            return nullable ? with.Nullable() : with.NotNullable();
        }

        #endregion
    }

    public static class IColumnTypeSyntaxExtensions
    {
        #region Exposed Methods

        public static T AsText<T>(this IColumnTypeSyntax<T> that)
            where T : IFluentSyntax
        {
            return that.AsCustom("text");
        }

        #endregion
    }

    public static class ICreateTableColumnOptionOrWithColumnSyntaxExtensions
    {
        #region Exposed Methods

        public static string GetTableName(this ICreateTableColumnOptionOrWithColumnSyntax that)
        {
            return ((CreateTableExpressionBuilder)that).Expression.TableName;
        }

        public static ICreateTableColumnOptionOrWithColumnSyntax WithIDentityColumn(
            this ICreateTableColumnOptionOrWithColumnSyntax that, string name = "Id")
        {
            return that.WithColumn(name).AsInt32().PrimaryKey().Identity().NotNullable().Unique();
        }

        public static ICreateTableColumnOptionOrWithColumnSyntax WithForeignKeyColumn(
            this ICreateTableColumnOptionOrWithColumnSyntax that, 
            string columnName, 
            string foreignTable,
            string foreignId = "Id", 
            bool nullable = true, 
            string foreignKeyName = "")
        {
            var keyName = string.IsNullOrEmpty(foreignKeyName)
                ? Utilities.CreateForeignKeyName(that.GetTableName(), foreignTable, columnName)
                : foreignKeyName;

            var with =
                that.WithColumn(columnName)
                    .AsInt32()
                    .ForeignKey(keyName, foreignTable, foreignId);

            return nullable ? with.Nullable() : with.NotNullable();
        }

        #endregion
    }

    public static class IAlterColumnAsTypeOrInSchemaSyntaxExtensions
    {
        #region Exposed Methods

        public static string GetTableName(this IAlterColumnAsTypeOrInSchemaSyntax that)
        {
            return ((AlterColumnExpressionBuilder)that).Expression.TableName;
        }

        public static IAlterColumnOptionSyntax AsForeignKey(this IAlterColumnAsTypeOrInSchemaSyntax that,
            string columnName, string foreignTable, string foreignId = "Id", bool nullable = true)
        {
            var foreignKey = that.AsInt32()
                                 .ForeignKey(
                                      Utilities.CreateForeignKeyName(that.GetTableName(), foreignTable, columnName),
                                      foreignTable,
                                      foreignId);

            return nullable ? foreignKey.Nullable() : foreignKey.NotNullable();
        }

        #endregion
    }

    public static class ICreateExpressionRootExtensions
    {
        #region Exposed Methods

        /// This will create a lookup table with the specified values for Id, Description
        /// Feel Free to use this. If for any reason you find yourself changing this. DON'T
        /// YOU WILL BREAK THE OLD MIGRATIONS. 
        /// If the table format for entity Lookups changes then 
        /// you will need to write a new extension method for that format.
        public static ICreateTableColumnOptionOrWithColumnSyntax LookupTable(this ICreateExpressionRoot that,
            string tableName, int descriptionSize = 25)
        {
            return that.Table(tableName)
                       .WithIdentityColumn()
                       .WithColumn("Description").AsString(descriptionSize).NotNullable();
        }

        public static void ManyToManyTable(this ICreateExpressionRoot that, string firstTable, string secondTable,
            string firstTableId = "Id", string secondTableId = "Id")
        {
            that.Table(firstTable + secondTable)
                .WithForeignKeyColumn(firstTable.Singularize() + "Id", firstTable, firstTableId)
                .WithForeignKeyColumn(secondTable.Singularize() + "Id", secondTable, secondTableId);
        }

        /// <summary>
        /// Create a new foreign key column with its name specified by <paramref name="columnName"/> on the
        /// table specified by <paramref name="tableName"/> referencing the (usually primary key) column
        /// specified by <paramref name="foreignId"/> on the table specified by
        /// <paramref name="foreignTable"/>.  This is <paramref name="nullable"/> by default.
        /// </summary>
        public static void ForeignKeyColumn(
            this ICreateExpressionRoot that,
            string tableName,
            string columnName,
            string foreignTable,
            string foreignId = "Id",
            bool nullable = true)
        {
            var column = that
                        .Column(columnName)
                        .OnTable(tableName)
                        .AsInt32();

            column = nullable ? column.Nullable() : column.NotNullable();

            column.ForeignKey(
                Utilities.CreateForeignKeyName(tableName, foreignTable, columnName),
                foreignTable,
                foreignId);
        }

        #endregion
    }

    public static class IInsertDataSyntaxExtensions
    {
        public static IInsertDataSyntax Rows(this IInsertDataSyntax that, params object[] rows)
        {
            foreach (var row in rows)
            {
                that = that.Row(row);
            }

            return that;
        }
    }

    public static class IDeleteDataOrInSchemaSyntaxExtensions
    {
        public static IDeleteDataSyntax Rows(this IDeleteDataSyntax that, params object[] rows)
        {
            foreach (var row in rows)
            {
                that = that.Row(row);
            }

            return that;
        }
    }
}
