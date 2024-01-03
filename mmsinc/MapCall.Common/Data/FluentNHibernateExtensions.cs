using System;
using System.Text;
using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.ClassExtensions.StringExtensions;

namespace MapCall.Common.Data
{
    public static class MigrationExtensions
    {
        #region Notifications, Document Types and Data Types OH MY
        
        public static void CreateNotificationPurpose(this Migration that, string application, string module,
            string purpose)
        {
            that.Execute.Sql(@"
                declare @applicationId int
                declare @moduleId int
                set @applicationId = (select ApplicationId from [Applications] where Name = '{0}')
                set @moduleId = (select ModuleID from [Modules] where Name = '{1}' and ApplicationId = @applicationId)
                insert into [NotificationPurposes] ([ModuleID], [Purpose]) VALUES(@moduleId, '{2}')
            ", application, module, purpose);
        }

        [Obsolete("As of MC-3241 this is method's t-sql is no longer compatible with database structure, use RemoveNotificationPurpose instead.")]
        public static void DeleteNotificationPurpose(this Migration that, string application, string module,
            string purpose)
        {
            that.Execute.Sql(@"
                declare @applicationId int
                declare @moduleId int
                declare @purposeId int
                set @applicationId = (select ApplicationId from [Applications] where Name = '{0}')
                set @moduleId = (select ModuleID from [Modules] where Name = '{1}' and [ApplicationId] = @applicationId)
                set @purposeId = (select top 1 NotificationPurposeID from [NotificationPurposes] where [Purpose] = '{2}' AND [ModuleId] = @moduleId)
                
                delete from [NotificationConfigurations] where [NotificationPurposeID] = @purposeId
                delete from [NotificationPurposes] where [NotificationPurposeID] = @purposeId 
            ", application, module, purpose);
        }

        public static void RemoveNotificationPurpose(this Migration that, string application, string module,
            string purpose)
        {
            that.Execute.Sql(@"
                declare @applicationId int
                declare @moduleId int
                declare @purposeId int
                set @applicationId = (select ApplicationId from [Applications] where Name = '{0}')
                set @moduleId = (select ModuleID from [Modules] where Name = '{1}' and [ApplicationId] = @applicationId)
                set @purposeId = (select top 1 Id from [NotificationPurposes] where [Purpose] = '{2}' AND [ModuleId] = @moduleId)
                
                delete from [NotificationConfigurationsNotificationPurposes] where [NotificationPurposeId] = @purposeId
                delete from [NotificationPurposes] where [Id] = @purposeId 
            ", application, module, purpose);
        }

        /// <summary>
        /// Create a document type for the table with the given name, creating the data type if necessary.
        /// </summary>
        /// <param name="tableName">String to use for both Data_Type and Table_name.</param>
        /// <param name="documentTypeName">String to use for Document_Type.</param>
        public static void CreateDocumentType(this Migration that, string tableName, string documentTypeName)
        {
            CreateDocumentType(that, tableName, tableName, documentTypeName.Singularize().ToTitleCase());
        }

        /// <summary>
        /// Create document types for the data type with the given table name and data type name, creating the data type
        /// if it does not already exist.
        /// </summary>
        /// <param name="tableName">String to use for Table_Name</param>
        /// <param name="dataTypeName">String to use for Data_Type</param>
        /// <param name="documentTypeNames">String(s) to use for Document_Type.</param>
        public static void CreateDocumentType(this Migration that, string tableName, string dataTypeName,
            params string[] documentTypeNames)
        {
            if (!documentTypeNames.Any())
            {
                throw new ArgumentException("No document type names supplied.");
            }

            var sb = new StringBuilder(String.Format(@"
declare @dataTypeId int;
if exists (select 1 from DataType where Table_Name = '{0}' and Data_Type = '{1}')
	select @dataTypeId = DataTypeId from DataType where Table_Name = '{0}' and Data_Type = '{1}';
else
begin
	insert into DataType (Table_Name, Data_Type) values ('{0}', '{1}');
	select @dataTypeId = @@IDENTITY;
end{2}", tableName, dataTypeName, Environment.NewLine));

            foreach (var documentTypeName in documentTypeNames)
            {
                sb.AppendLine(
                    String.Format("insert into DocumentType (Document_Type, DataTypeId) values ('{0}', @dataTypeId);",
                        documentTypeName));
            }

            that.Execute.Sql(sb.ToString());
        }

        public static void DeleteDataType(this Migration that, string tableName)
        {
            var fromDocumentType =
                String.Format(
                    "from [DocumentType] where DataTypeID IN (select DataTypeId from [DataType] where Table_Name = '{0}')",
                    tableName);

            that.Execute.Sql("Delete from Document where DocumentTypeId IN (select DocumentTypeId {0});",
                fromDocumentType);
            that.Execute.Sql("delete {0};", fromDocumentType);
            that.Execute.Sql("delete from [DataType] where Table_Name = '{0}';", tableName);
        }

        #endregion

        #region Applications

        public static void CreateApplication(this Migration that, string name, int id)
        {
            that.Execute.Sql(@"
                SET IDENTITY_INSERT Applications ON;
                INSERT INTO Applications ([ApplicationID], [Name]) VALUES ({0}, '{1}');
                SET IDENTITY_INSERT Applications OFF", id, name);
        }

        public static void DeleteApplication(this Migration that, string name)
        {
            that.Execute.Sql(@"
                declare @applicationId int;
                SELECT @applicationId = ApplicationId FROM Applications WHERE Name = '{0}';
                DELETE FROM Roles WHERE ModuleId IN (SELECT ModuleId FROM Modules WHERE ApplicationId = @applicationId);
                DELETE FROM Modules WHERE ApplicationId = @applicationId;
                DELETE FROM Applications WHERE ApplicationId = @applicationId;", name);
        }

        #endregion

        #region Modules

        public static void CreateModule(this Migration that, string name, string applicationName, int id)
        {
            that.Execute.Sql(@"
                SET IDENTITY_INSERT Modules ON;
                INSERT INTO Modules ([ModuleId], [ApplicationId], [Name]) SELECT {0}, ApplicationId, '{1}' FROM Applications where Name = '{2}';
                SET IDENTITY_INSERT Modules OFF", id, name, applicationName);
        }

        /// <summary>
        /// Deletes a single module for an application as well as any associated roles(user or role group).
        /// </summary>
        /// <param name="that"></param>
        /// <param name="applicationName"></param>
        /// <param name="moduleName"></param>
        public static void DeleteModuleAndAssociatedRoles(this Migration that, string applicationName, string moduleName)
        {
            that.Execute.Sql($@"
                declare @applicationId int
                select @applicationId = ApplicationId from Applications where Name = '{applicationName}'
                declare @moduleId int 
                select @moduleId = ModuleId from Modules where ApplicationId = @applicationId and Name = '{moduleName}'
                delete from Roles WHERE ModuleId = @moduleId
                delete from RoleGroupRoles where ModuleId = @moduleId
                delete from Modules where ModuleId = @moduleId");
        }

        /// <summary>
        /// Reassigns the given module to a different application.
        /// </summary>
        /// <param name="that">The <see cref="Migration"/> instance this extension method is invoked upon.</param>
        /// <param name="reassignedApplicationName">The name of the application the module should be reassigned to.</param>
        /// <param name="moduleName">The name of the module to be reassigned.</param>
        public static void ReassignModule(this Migration that, string moduleName, string reassignedApplicationName)
        {
            that.Execute.Sql(@"
                DECLARE @applicationId INT;
                DECLARE @moduleId INT;
                SELECT @applicationId = ApplicationId FROM Applications WHERE Name = '{0}';
                SELECT @moduleId = ModuleId FROM Modules WHERE Name = '{1}';
                UPDATE Modules SET ApplicationId = @applicationId WHERE ModuleID = @moduleId;", reassignedApplicationName, moduleName);
        }

        /// <summary>
        /// Renames a module.
        /// </summary>
        /// <param name="that">The <see cref="Migration"/> instance this extension method is invoked upon.</param>
        /// <param name="currentName">The current name of the module to be renamed.</param>
        /// <param name="newName">The desired new name of the module.</param>
        public static void RenameModule(this Migration that, string currentName, string newName)
        {
            that.Execute.Sql(@"UPDATE Modules SET Name = '{0}' WHERE Name = '{1}'", newName, currentName);
        }

        #endregion

        #region Asset Types

        public static void AddAssetTypeToOperatingCenter(this Migration that, string operatingCenterCode,
            string assetType)
        {
            that.Execute.Sql($@"
DECLARE @opCenterId int, @assetTypeId int;
SELECT @opCenterId = OperatingCenterId FROM OperatingCenters WHERE OperatingCenterCode = '{operatingCenterCode}';
SELECT @assetTypeId = AssetTypeId FROM AssetTypes WHERE Description = '{assetType}';
INSERT INTO OperatingCenterAssetTypes (OperatingCenterId, AssetTypeId) VALUES (@opCenterId, @assetTypeId);");
        }

        public static void RemoveAssetTypeFromOperatingCenter(this Migration that, string operatingCenterCode,
            string assetType)
        {
            that.Execute.Sql($@"
DECLARE @opCenterId int, @assetTypeId int;
SELECT @opCenterId = OperatingCenterId FROM OperatingCenters WHERE OperatingCenterCode = '{operatingCenterCode}';
SELECT @assetTypeId = AssetTypeId FROM AssetTypes WHERE Description = '{assetType}';
DELETE FROM OperatingCenterAssetTypes WHERE OperatingCenterId = @opCenterId AND AssetTypeId = @assetTypeId;");
        }

        #endregion
    }
}