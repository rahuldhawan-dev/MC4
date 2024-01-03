using System;
using System.Collections.Generic;
using FluentMigrator;
using FluentMigrator.Builders.Alter.Column;
using FluentMigrator.Builders.Create.Table;
using MMSINC.ClassExtensions.FluentMigratorExtensions;
using MMSINC.ClassExtensions.StringExtensions;

namespace MapCall.Common.ClassExtensions
{
    public static class MigrationExtensions
    {
        /// <summary>
        /// Runs the GRANT ALL ON TABLE TO USER command. 
        /// </summary
        public static void Grant(this Migration migration, string tableName, string userName)
        {
            migration.Execute.Sql(string.Format("GRANT ALL ON [{0}] TO {1}", tableName, userName));
        }

        /// <param name="deleteSafely">Wheither or not we should use parentTableName to delete existing lookup values instead of just going by lookup type name.  Only matters if deleteLookupValues is true.</param
        /// <param name="createForeignKey">Whether or not we should create a new foreign key from the parent table to the new lookup table</param>
        /// <param name="lookupIsTableSpecific">Whether or not the lookup table has a value for 'TableName' for the given lookup.</param>
        public static void ExtractLookupTableLookup(this Migration that, string parentTableName,
            string parentColumnName,
            string newTableName, int descriptionLength, string lookupTypeName, bool createForeignKey = true,
            bool deleteLookupValues = true, bool nullable = true, bool deleteOldForeignKey = false,
            bool deleteSafely = false, bool lookupIsTableSpecific = true)
        {
            that.Create.LookupTable(newTableName, descriptionLength);

            if (lookupIsTableSpecific)
            {
                that.Execute.Sql(
                    "INSERT INTO {0} (Description) SELECT DISTINCT LookupValue FROM Lookup WHERE LookupType = '{1}' and TableName = '{2}'",
                    newTableName, lookupTypeName, parentTableName);
            }
            else
            {
                that.Execute.Sql(
                    "INSERT INTO {0} (Description) SELECT DISTINCT LookupValue FROM Lookup WHERE LookupType = '{1}'",
                    newTableName, lookupTypeName);
            }

            if (deleteOldForeignKey)
            {
                that.TryDeleteForeignKey(parentTableName, parentColumnName, "Lookup");
            }

            that.Execute.Sql(
                "UPDATE {0} SET {1} = {2}.Id FROM {2} INNER JOIN Lookup ON Lookup.LookupValue = {2}.Description WHERE {0}.{1} = Lookup.LookupId AND Lookup.LookupType = '{3}';",
                parentTableName, parentColumnName, newTableName, lookupTypeName);

            if (deleteLookupValues)
            {
                that.Execute.Sql("DELETE FROM Lookup WHERE LookupType = '{0}'{1};", lookupTypeName,
                    deleteSafely ? String.Format(" AND TableName = '{0}'", parentTableName) : String.Empty);
            }

            if (!createForeignKey)
            {
                return;
            }

            Action<IAlterColumnOptionOrForeignKeyCascadeSyntax> nullabilityFn;

            if (nullable)
            {
                nullabilityFn = stmt => stmt.Nullable();
            }
            else
            {
                nullabilityFn = stmt => stmt.NotNullable();
            }

            nullabilityFn(
                that.Alter.Column(parentColumnName)
                    .OnTable(parentTableName)
                    .AsInt32()
                    .ForeignKey(String.Format("FK_{0}_{1}_{2}", parentTableName, newTableName, parentColumnName),
                         newTableName, "Id"));
        }

        /// <param name="recreateOldForeignKey">Set this to false if the column never had a foreign key in the first place. Otherwise
        /// the migration will fail when you do migrate -> rollback -> migrate -> rollback.</param>
        public static void ReplaceLookupTableLookup(this Migration that, string parentTableName,
            string parentColumnName,
            string newTableName, int descriptionLength, string lookupTypeName, bool createdForeignKey = true,
            bool deletedLookupValues = true, bool lookupIsTableSpecific = true, bool recreateOldForeignKey = true)
        {
            if (createdForeignKey)
            {
                that.Delete.ForeignKey(String.Format("FK_{0}_{1}_{2}", parentTableName, newTableName, parentColumnName))
                    .OnTable(parentTableName);
            }

            if (deletedLookupValues)
            {
                if (lookupIsTableSpecific)
                {
                    that.Execute.Sql(
                        "INSERT INTO Lookup (LookupValue, LookupType, TableName) SELECT Description, '{0}', '{1}' FROM {2};",
                        lookupTypeName, parentTableName, newTableName);
                }
                else
                {
                    that.Execute.Sql(
                        "INSERT INTO Lookup (LookupValue, LookupType) SELECT Description, '{0}' FROM {1};",
                        lookupTypeName, newTableName);
                }
            }

            that.Execute.Sql(
                "UPDATE {0} SET {1} = Lookup.LookupId FROM Lookup INNER JOIN {2} ON {2}.Description = Lookup.LookupValue WHERE Lookup.LookupType = '{3}' AND {2}.Id = {0}.{1};",
                parentTableName, parentColumnName, newTableName, lookupTypeName);

            that.Delete.Table(newTableName);

            var column = that.Alter.Column(parentColumnName)
                             .OnTable(parentTableName)
                             .AsInt32()
                             .Nullable();
            if (recreateOldForeignKey)
            {
                column = column.ForeignKey($"FK_{parentTableName}_Lookup_{parentColumnName}", "Lookup", "LookupId");
            }
        }

        public static void DeleteLookupTableLookup(this Migration that, string parentTableName, string parentColumnName,
            string lookupTypeName)
        {
            that.TryDeleteForeignKey(parentTableName, parentColumnName, "Lookup");

            that.Delete.Column(parentColumnName).FromTable(parentTableName);

            that.Execute.Sql("DELETE FROM Lookup WHERE LookupType = '{0}' AND TableName = '{1}'", lookupTypeName,
                parentTableName);
        }

        public static void TryDeleteForeignKey(this Migration that, string parentTableName, string parentColumnName,
            string foreignTableName)
        {
            // Does this try catch even do anything? The actual execution on sql server comes way after the delete call.
            // -Ross 10/8/2015
            try
            {
                that.Delete.ForeignKey(
                         Utilities.CreateForeignKeyName(parentTableName, foreignTableName,
                             parentColumnName))
                    .OnTable(parentTableName);
            }
            catch { }
        }

        /// <summary>
        /// Removes a column as well as the foreign key it's associated with.
        /// </summary>
        /// <param name="that"></param>
        /// <param name="parentTableName"></param>
        /// <param name="parentColumnName"></param>
        /// <param name="foreignTableName"></param>
        public static void DeleteForeignKeyColumn(this Migration that, string parentTableName, string parentColumnName,
            string foreignTableName)
        {
            TryDeleteForeignKey(that, parentTableName, parentColumnName, foreignTableName);
            that.Delete.Column(parentColumnName).FromTable(parentTableName);
        }

        /// <summary>
        /// This will create a lookup table with the specified values for Id, Description
        /// Feel Free to use this. If for any reason you find yourself changing this. DON'T
        /// YOU WILL BREAK THE OLD MIGRATIONS. 
        /// If the table format for entity Lookups changes then 
        /// you will need to write a new extension method for that format.
        /// </summary>
        public static void CreateLookupTableWithValues(
            this MigrationBase that,
            string table,
            params string[] descriptions)
        {
            CreateLookupTableWithValues(that, table, 50, descriptions);
        }

        /// <inheritdoc cref="CreateLookupTableWithValues(FluentMigrator.MigrationBase,string,string[])"/>
        public static void CreateLookupTableWithValues(
            this MigrationBase that,
            string table,
            int descriptionLength,
            params string[] descriptions)
        {
            that.Create.LookupTable(table, descriptionLength);

            foreach (var d in descriptions)
            {
                that.Insert.IntoTable(table).Row(new {Description = d});
            }
        }

        /// <summary>
        /// Adds a lookup value to the given table.
        /// </summary>
        /// <param name="that">The instance of the invocation.</param>
        /// <param name="tableName">The table to insert the lookup value to.</param>
        /// <param name="id">The id of the lookup value.</param>
        /// <param name="description">The description of the lookup value.</param>
        public static void AddLookupValueWithId(this Migration that, string tableName, int id, string description)
        {
            that.AddLookupEntities(tableName, new Dictionary<int, string> { { id, description }});
        }

        /// <summary>
        /// Creates an entity lookup in the given table for each key value pair.
        /// </summary>
        /// <param name="that">the instance of the invocation.</param>
        /// <param name="tableName">the table to insert entities into.</param>
        /// <param name="entities">a dictionary of int/string key value pairs representing the id and description of entities.</param>
        public static void AddLookupEntities(this Migration that, string tableName, IDictionary<int, string> entities)
        {
            that.EnableIdentityInsert(tableName);

            foreach (var kvp in entities)
            {
                that.Insert.IntoTable(tableName).Row(new { Id = kvp.Key, Description = kvp.Value });    
            }
            
            that.DisableIdentityInsert(tableName);
        }

        /// <summary>
        /// Deletes entities with the matching ids from the given entity lookup table.
        /// </summary>
        /// <param name="that">the instance of this invocation.</param>
        /// <param name="tableName">the table to delete entities from.</param>
        /// <param name="entityIds">a collection of entity lookup ids to be deleted.</param>
        public static void DeleteLookupEntities(this Migration that, string tableName, int[] entityIds)
        {
            foreach (var id in entityIds)
            {
                that.Delete.FromTable(tableName).Row(new { Id = id });    
            }
        }

        /// <summary>
        /// This will create a lookup table with the specified values for Id, Description
        /// Feel Free to use this. If for any reason you find yourself changing this. DON'T
        /// YOU WILL BREAK THE OLD MIGRATIONS. 
        /// If the table format for entity Lookups changes then 
        /// you will need to write a new extension method for that format.
        /// </summary>
        /// <param name="that"></param>
        /// <param name="table"></param>
        /// <param name="descriptions"></param>
        public static void CreateSAPLookupTable(this Migration that, string table, int sapCodeLength)
        {
            that.Create.LookupTable(table)
                .WithColumn("SAPCode").AsAnsiString(sapCodeLength).Unique().NotNullable();
        }

        /// <summary>
        /// This will create a lookup table with the provided query
        /// Feel Free to use this. If for any reason you find yourself changing this. DON'T
        /// YOU WILL BREAK THE OLD MIGRATIONS. 
        /// </summary>
        /// <param name="that"></param>
        /// <param name="tableName"></param>
        /// <param name="query">e.g. SELECT DISTINCT Foo from Bar</param>
        public static void CreateLookupTableFromQuery(this Migration that, string tableName, string query)
        {
            that.Create.LookupTable(tableName);
            that.Execute.Sql("INSERT INTO [" + tableName + "] " + query);
        }

        /// <summary>
        /// Adds a new FK KeyId column, copies the id for the old field into this column, removes the old column
        /// </summary>
        /// <param name="that"></param>
        /// <param name="tableName"></param>
        /// <param name="lookupTableName"></param>
        /// <param name="columnName"></param>
        /// <param name="oldColumnName"></param>
        /// <param name="lookupId"></param>
        /// <param name="lookupDescription"></param>
        /// <param name="additionalWhere"></param>
        public static void AddLookupForeignKeyUpdateItThenRemoveOldColumn(this Migration that, string tableName,
            string lookupTableName, string columnName, string oldColumnName, string lookupId = "Id",
            string lookupDescription = "Description", string additionalWhere = "", bool nullable = true)
        {
            that.Alter.Table(tableName).AddForeignKeyColumn(columnName, lookupTableName, lookupId, nullable);
            that.Execute.Sql(
                "UPDATE [{0}] SET [{1}] = (SELECT TOP 1 [{2}] from [{3}] WHERE [{3}].[{4}] = [{0}].[{5}] {6});",
                tableName, columnName, lookupId, lookupTableName, lookupDescription, oldColumnName,
                additionalWhere);
            that.Delete.Column(oldColumnName).FromTable(tableName);
        }

        /// <summary>
        /// Adds the old column without Id, copies the text value in, then removes the fk id column
        /// </summary>
        /// <param name="that"></param>
        /// <param name="tableName"></param>
        /// <param name="lookupTableName"></param>
        /// <param name="columnName"></param>
        /// <param name="oldColumnName"></param>
        /// <param name="stringLength"></param>
        /// <param name="lookupId"></param>
        /// <param name="lookupDescription"></param>
        public static void RemoveLookupAndAdjustColumns(this Migration that, string tableName, string lookupTableName,
            string columnName, string oldColumnName, int stringLength, string lookupId = "Id",
            string lookupDescription = "Description")
        {
            that.Alter.Table(tableName).AddColumn(oldColumnName).AsAnsiString(stringLength).Nullable();
            that.Execute.Sql("UPDATE [{0}] SET [{1}] = (SELECT [{2}] FROM [{3}] WHERE [{3}].[{4}] = [{0}].[{5}])",
                tableName, oldColumnName, lookupDescription, lookupTableName, lookupId, columnName);
            that.Delete.ForeignKeyColumn(tableName, columnName, lookupTableName, lookupId);
        }

        /// <summary>
        /// Deletes and index if it exists.
        /// </summary>
        /// <param name="migration"></param>
        /// <param name="tableName"></param>
        /// <param name="indexName"></param>
        public static void DeleteIndexIfItExists(this Migration migration, string tableName, string indexName)
        {
            const string format =
                "IF EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'[{0}]') AND name = N'{1}') DROP INDEX [{1}] ON [{0}]";
            var cmd = string.Format(format, tableName, indexName);
            migration.Execute.Sql(cmd);
        }

        /// <summary>
        /// Deletes a statistic if it exists.
        /// </summary>
        /// <param name="migration"></param>
        /// <param name="tableName"></param>
        /// <param name="statisticName"></param>
        public static void DeleteStatisticIfItExits(this Migration migration, string tableName, string statisticName)
        {
            const string format =
                "IF EXISTS (SELECT 1 FROM sysindexes where [name] = '{1}') DROP STATISTICS [{0}].[{1}]";
            var cmd = string.Format(format, tableName, statisticName);
            migration.Execute.Sql(cmd);
        }

        public static void DeleteForeignKeyIfItExists(this Migration that, string parentTableName,
            string parentColumnName,
            string foreignTableName)
        {
            const string format =
                @"IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID('{0}') AND parent_object_id = OBJECT_ID('{1}')) ALTER TABLE [{1}] DROP CONSTRAINT [{0}]";

            var fk = Utilities.CreateForeignKeyName(parentTableName, foreignTableName, parentColumnName);
            var cmd = string.Format(format, fk, parentTableName);
            that.Execute.Sql(cmd);
        }

        public static void AddNotificationType(this Migration that, string application, string module, string purpose)
        {
            that.Execute.Sql(
                "INSERT INTO NotificationPurposes SELECT (select ModuleID from Modules M Join Applications A on A.ApplicationID = M.ApplicationID where a.name = '{0}' and m.name = '{1}'), '{2}'",
                application, module, purpose);
        }

        /// <summary>
        /// Reassigns the given notification purpose to a new module.
        /// </summary>
        /// <param name="that">The instance this method is invoked upon.</param>
        /// <param name="currentModuleId">The current module identifier for the given purpose.</param>
        /// <param name="purpose">The purpose whose module will be reassigned.</param>
        /// <param name="reassignedModuleId">The module identifier to be reassigned to the purpose.</param>
        public static void ReassignNotificationPurpose(this Migration that, int currentModuleId, string purpose, int reassignedModuleId)
        {
            that.Execute
                .Sql("update NotificationPurposes set ModuleId = {0} where ModuleId = {1} and purpose = '{2}'",
                     reassignedModuleId,
                     currentModuleId, 
                     purpose);
        }

        /// <summary>
        /// Note that this also removes anyone signed up for the notification type.
        /// </summary>
        /// <param name="that"></param>
        /// <param name="application"></param>
        /// <param name="module"></param>
        /// <param name="purpose"></param>
        [Obsolete("As of MC-3241 this is method's t-sql is no longer compatible with database structure, use RemoveNotificationPurpose instead.")]
        public static void RemoveNotificationType(this Migration that, string application, string module,
            string purpose)
        {
            that.Execute.Sql(
                "DELETE FROM NotificationConfigurations Where NotificationPurposeID = (SELECT NotificationPurposeID from NotificationPurposes where ModuleID = (select ModuleID from Modules M Join Applications A on A.ApplicationID = M.ApplicationID where a.name = '{0}' and m.name = '{1}') and Purpose = '{2}')",
                application, module, purpose);
            that.Execute.Sql(
                "DELETE FROM NotificationPurposes Where ModuleID = (select ModuleID from Modules M Join Applications A on A.ApplicationID = M.ApplicationID where a.name = '{0}' and m.name = '{1}') and Purpose = '{2}'",
                application, module, purpose);
        }

        public static void RemoveNotificationPurpose(this Migration that, string application, string module, string purpose)
        {
            that.Execute.Sql(@"
                declare @notificationConfigurationId int;
                declare @notificationPurposeId int;
                declare @applicationId int;
                declare @moduleId int;
                
                select @applicationId = ApplicationID 
                  from Applications 
                 where Name = '{0}'; 

                select @moduleId = ModuleId 
                  from Modules m
                  join Applications a 
                    on m.ApplicationID = a.ApplicationID                                    
                 where a.ApplicationId = @applicationId
                   and m.Name = '{1}'; 

                select @notificationPurposeId = Id
                  from NotificationPurposes 
                 where Purpose = '{2}'
                   and ModuleId = @moduleId;
                                                
                delete from NotificationConfigurationsNotificationPurposes
                 where NotificationPurposeId = @notificationPurposeId;

                delete from NotificationPurposes 
                 where Id = @notificationPurposeId;
            ", application, module, purpose);
        }

        /// <summary>
        /// Enables identity insert for a table. Remember to call DisableIdentityInsert when you're done!
        /// </summary>
        /// <param name="migration"></param>
        /// <param name="tableName"></param>
        public static void EnableIdentityInsert(this Migration migration, string tableName)
        {
            migration.Execute.Sql($"SET IDENTITY_INSERT [{tableName}] ON");
        }

        public static void DisableIdentityInsert(this Migration migration, string tableName)
        {
            migration.Execute.Sql($"SET IDENTITY_INSERT [{tableName}] OFF");
        }

        /// <summary>
        /// Removes a DataType along with any references to it, including document types and document links.
        /// </summary>
        /// <param name="migration"></param>
        /// <param name="tableName"></param>
        public static void RemoveDataType(this Migration migration, string tableName)
        {
            const string format = @"
                declare @dataTypeId int
                set @dataTypeId = (select DataTypeId from [DataType] where Table_Name = '{0}')

                delete from [DocumentLink] WHERE DataTypeID = @dataTypeId
                delete from [Document] WHERE DocumentTypeId IN (SELECT DocumentTypeID FROM [DocumentType] WHERE DataTypeID = @dataTypeId)
                delete from [DocumentType] where DataTypeId = @dataTypeId
                delete from [DataType] where DataTypeId = @dataTypeId";

            migration.Execute.Sql(string.Format(format, tableName));
        }

        public static void AddDataType(this Migration migration, string tableName, string dataType = null)
        {
            const string format = "INSERT INTO DataType (Data_Type, Table_Name) VALUES ('{0}', '{1}')";

            migration.Execute.Sql(format, dataType ?? tableName, tableName);
        }

        public static void RenameDataType(this Migration migration, string currentTableName, string newTableName, string currentDataType = null, string newDataType = null)
        {
            const string updateQuery = @"
                UPDATE DataType SET Data_Type = '{0}', Table_Name = '{1}'
                 WHERE Data_Type = '{2}' AND Table_Name = '{3}'";

            migration.Execute.Sql(updateQuery, newDataType ?? newTableName, newTableName, currentDataType ?? currentTableName, currentTableName);
        }

        /// <summary>
        /// Inserts a new document type record for the data type that matches the given tableName.
        /// </summary>
        /// <param name="migration"></param>
        /// <param name="documentType"></param>
        /// <param name="tableName"></param>
        public static void AddDocumentType(this Migration migration, string documentType, string tableName)
        {
            // NOTE: This *should* throw an error if there are multiple data types with the same table name.
            //       Indication that something needs to be fixed with one of the data types.
            const string format = @"
                declare @dt int
                set @dt = (select DataTypeId from [DataType] where Table_Name = '{0}')

                insert into [DocumentType] (Document_Type, DataTypeID) VALUES('{1}', @dt)";

            migration.Execute.Sql(format, tableName, documentType);
        }

        /// <summary>
        /// DELETES a document type and ALL DOCUMENTS related to that document type.
        /// </summary>
        /// <param name="migration"></param>
        /// <param name="documentType"></param>
        /// <param name="tableName"></param>
        public static void RemoveDocumentTypeAndAllRelatedDocuments(this Migration migration, string documentType,
            string tableName)
        {
            const string format = @"
                declare @dataType int
                declare @docType int
                set @dataType = (select DataTypeId from [DataType] where Table_Name = '{0}')
                set @docType = (select DocumentTypeId from [DocumentType] where Document_Type = '{1}' and DataTypeId = @dataType)
                
                delete from [Document] WHERE DocumentTypeId = @docType;
                delete from [DocumentType] WHERE DocumentTypeId = @docType;
                ";

            migration.Execute.Sql(format, tableName, documentType);
        }

        /// <summary>
        /// Sets up a column as the identity and primary key column as well as making it not nullable. 
        /// </summary>
        /// <param name="that"></param>
        /// <returns></returns>
        [Obsolete("Use WithIdentityColumn instead because I didn't know it existed when I made this. -Ross")]
        public static ICreateTableColumnOptionOrWithColumnSyntax AsIdColumn(this ICreateTableColumnAsTypeSyntax that)
        {
            return that.AsInt32().Identity().PrimaryKey().NotNullable();
        }

        public static void UpdateBooleanColumn(this Migration migration, string tableName, string columnName,
            bool defaultValue = false)
        {
            var format = @"UPDATE [{0}] SET [{1}] = 0 where [{1}] IS NULL;";
            migration.Execute.Sql(String.Format(format, tableName, columnName));
            migration.Alter.Table(tableName).AlterColumn(columnName).AsBoolean().NotNullable()
                     .WithDefaultValue(defaultValue);
        }

        public static void UpdateBooleanOnColumn(this Migration migration, string tableName, string columnName)
        {
            migration.Execute.Sql("Update {0} SET {1} = CASE WHEN ({1} = 'ON') THEN 1 ELSE 0 END", tableName,
                columnName);
            migration.Alter.Column(columnName).OnTable(tableName).AsBoolean().NotNullable();
        }

        public static void RollbackBooleanOnColumn(this Migration migration, string tableName, string columnName,
            int length = 2)
        {
            migration.Alter.Column(columnName).OnTable(tableName).AsAnsiString(length).Nullable();
            migration.Execute.Sql("Update {0} SET {1} = CASE WHEN ({1} = 1) THEN 'ON' ELSE '' END", tableName,
                columnName);
        }

        public static void NullDateField(this Migration migration, string tableName, string fieldName)
        {
            var format = @"UPDATE [{0}] SET [{1}] = null where [{1}] = '01/01/1900';";
            migration.Execute.Sql(String.Format(format, tableName, fieldName));
        }

        public static void ExtractNonLookupTableLookup(this Migration that, string tableName, string columnName,
            string newTableName, int length, bool nullable = true, string newColumnName = null)
        {
            newColumnName = newColumnName ?? (newTableName.Singularize() + "Id");

            that.Create.Table(newTableName)
                .WithIdentityColumn()
                .WithColumn("Description").AsString(length).NotNullable();

            that.Alter.Table(tableName)
                .AddForeignKeyColumn(newColumnName, newTableName, nullable: nullable);

            that.Execute.Sql(
                $"INSERT INTO [{newTableName}] (Description) SELECT DISTINCT [{columnName}] FROM [{tableName}] WHERE [{columnName}] IS NOT NULL;");
            that.Execute.Sql(
                $"UPDATE [{tableName}] SET [{newColumnName}] = n.Id FROM [{newTableName}] n WHERE n.Description = [{tableName}].[{columnName}];");

            that.Delete.Column(columnName).FromTable(tableName);
        }

        public static void ReplaceNonLookupTableLookup(this Migration that, string tableName, string columnName,
            string newTableName, int length, bool nullable = true, string newColumnName = null)
        {
            newColumnName = newColumnName ?? newTableName.Singularize() + "Id";

            var stmt = that.Alter.Table(tableName).AddColumn(columnName).AsString(length);
            if (nullable)
            {
                stmt.Nullable();
            }
            else
            {
                stmt.NotNullable();
            }

            that.Execute.Sql(
                $"UPDATE [{tableName}] SET [{columnName}] = n.Description FROM [{newTableName}] n WHERE n.Id = [{tableName}].[{newColumnName}]");

            that.Delete.ForeignKeyColumn(tableName, newColumnName, newTableName);

            that.Delete.Table(newTableName);
        }

        public static void NullEmptyStringValues(this Migration that, string table, string column)
        {
            that.Execute.Sql($"UPDATE {table} SET {column} = NULL WHERE LTRIM(RTRIM({column})) = '';");
        }

        public static void NormalizeToExistingTable(this Migration that, string table, string column,
            string foreignTable, bool nullable = true, string newColumn = null, string foreignId = "Id",
            string foreignValue = "Description")
        {
            newColumn = newColumn ?? column + "Id";
            that.Alter.Table(table).AddForeignKeyColumn(newColumn, foreignTable, foreignId, nullable);
            that.Execute.Sql(
                $"UPDATE {table} SET {newColumn} = {foreignTable}.{foreignId} FROM {foreignTable} WHERE {column} = {foreignTable}.{foreignValue};");
            that.Delete.Column(column).FromTable(table);
        }

        public static void DeNormalizeFromExistingTable(this Migration that, string table, string column,
            string foreignTable, int stringLength, bool nullable = true, string newColumn = null,
            string foreignId = "Id", string foreignValue = "Description")
        {
            newColumn = newColumn ?? column + "Id";
            that.Alter.Table(table).AddColumn(column).AsString(stringLength).Nullable();
            that.Execute.Sql(
                $"UPDATE {table} SET {column} = {foreignTable}.{foreignValue} FROM {foreignTable} WHERE {newColumn} = {foreignTable}.{foreignId};");
            that.DeleteForeignKeyColumn(table, newColumn, foreignTable);
            if (!nullable)
            {
                that.Alter.Column(column).OnTable(table).AsString(stringLength).NotNullable();
            }
        }
        
        /// <summary>
        /// This is used to populate change-tracking columns from the audit log table.
        /// </summary>
        public static void UpdateColumnFromAuditLog(
            this Migration that,
            string entityName,
            string destColumn,
            string sourceColumn,
            string entryType,
            string tableName = null,
            string idColumn = "Id")
        {
            tableName = tableName ?? entityName.Pluralize();
            
            that.Execute.Sql($@"
WITH SelectedEntries AS (
      SELECT Timestamp,
             UserId,
             EntityId
        FROM AuditLogEntries 
       WHERE EntityName = '{entityName}'
         AND AuditEntryType = '{entryType}'
), MostRecentEntries AS (
      SELECT recent.Timestamp,
             recent.UserId,
             recent.EntityId
        FROM SelectedEntries recent 
   LEFT JOIN SelectedEntries moreRecent   
          ON moreRecent.EntityId = recent.EntityId
         AND moreRecent.Timestamp > recent.Timestamp
       WHERE moreRecent.Timestamp IS NULL 
)
UPDATE x
SET x.{destColumn} = recent.{sourceColumn}
FROM {tableName} x
INNER JOIN MostRecentEntries recent ON recent.EntityId = x.{idColumn}");
        }
    }
}