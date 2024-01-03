using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Data;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150511101337060), Tags("Production")]
    public class AlterProjectsDVForBug2203 : Migration
    {
        public override void Up()
        {
            Execute.Sql("DELETE FROM ProjectsDV where OpCode IS NULL OR BU IS NULL;");

            //Op Code- Make required
            Alter.Table("ProjectsDV")
                 .AlterColumn("OpCode").AsInt32().NotNullable();

            //Project Category - Remove this field
            this.DeleteLookupTableLookup("ProjectsDV", "ProjectCategory", "ProjectCategory");

            //Change "Project Number" to  "Developer Services ID" 
            Rename.Column("ProjectNumber").OnTable("ProjectsDV").To("DeveloperServicesID");

            //Change "PP work order" to "WBS Number"
            Rename.Column("PPWorkOrder").OnTable("ProjectsDV").To("WBSNumber");

            //Category (make required)
            this.ExtractLookupTableLookup("ProjectsDV", "Category", "DevelopmentProjectCategories", 50, "Category",
                deleteOldForeignKey: true, deleteSafely: true, deleteLookupValues: false);
            Alter.Table("ProjectsDV")
                 .AlterColumn("Category").AsInt32().NotNullable();

            //Asset Category - Remove this field
            this.DeleteLookupTableLookup("ProjectsDV", "AssetCategory", "AssetCategory");

            //Project Status - Remove this field
            this.DeleteLookupTableLookup("ProjectsDV", "ProjectStatus", "ProjectStatus");

            //Project Obstacles - Remove this field
            Delete.Column("ProjectObstacles").FromTable("ProjectsDV");

            //Project Risk- Remove this field
            Delete.Column("ProjectRisks").FromTable("ProjectsDV");

            //Project Approach -- Remove this field
            Delete.Column("ProjectApproach").FromTable("ProjectsDV");

            //Project Duration Months- Remove this field
            Delete.Column("ProjectDurationMonths").FromTable("ProjectsDV");

            //Estimated Project Cost - Remove this field
            Delete.Column("EstProjectCost").FromTable("ProjectsDV");

            //Phase - Remove this field
            this.DeleteLookupTableLookup("ProjectsDV", "Phase", "Phase");

            //MIS Dates - Remove this field
            Delete.Column("MISDates").FromTable("ProjectsDV");

            //COE - Remove this field
            Delete.Column("COE").FromTable("ProjectsDV");

            //BU (make required)
            Alter.Table("ProjectsDV")
                 .AlterColumn("BU").AsInt32().NotNullable();

            //Project Description (make required)
            Alter.Column("ProjectDescription")
                 .OnTable("ProjectsDV")
                 .AsCustom("text").NotNullable();

            Rename.Table("ProjectsDV")
                  .To("DevelopmentProjects");

            Alter.Table("DevelopmentProjects")
                 .AddColumn("DomesticWaterServices").AsInt32().Nullable();
            Alter.Table("DevelopmentProjects")
                 .AddColumn("FireServices").AsInt32().Nullable();
            Alter.Table("DevelopmentProjects")
                 .AddColumn("DomesticSanitaryServices").AsInt32().Nullable();

            Execute.Sql(
                "UPDATE DataType SET Table_Name = 'DevelopmentProjects', Data_Type = 'Development Project' WHERE Table_Name = 'ProjectsDV'");
            Execute.Sql(
                "INSERT INTO DocumentType (Document_Type, DataTypeId) SELECT 'Development Project Document', da.DataTypeId FROM DataType da WHERE da.Table_Name = 'DevelopmentProjects'");

            Execute.Sql(
                "UPDATE DevelopmentProjects SET CreatedBy = (SELECT RecID FROM tblPermissions WHERE username = CreatedBy)");
            Rename.Column("CreatedBy").OnTable("DevelopmentProjects").To("CreatedById");
            Alter.Column("CreatedById").OnTable("DevelopmentProjects")
                 .AsInt32().NotNullable().ForeignKey("tblPermissions", "RecId");

            this.CreateNotificationPurpose("Field Services", "Projects", "Development Project Created");
        }

        public override void Down()
        {
            this.DeleteNotificationPurpose("Field Services", "Projects", "Development Project Created");
            Execute.Sql(
                @"IF EXISTS (Select 1 from sysobjects where name = 'FK_DevelopmentProjects_CreatedById_tblPermissions_RecId') 
                            ALTER TABLE DevelopmentProjects DROP CONSTRAINT FK_DevelopmentProjects_CreatedById_tblPermissions_RecId");
            Alter.Column("CreatedById").OnTable("DevelopmentProjects")
                 .AsString(50).NotNullable();
            Rename.Column("CreatedById").OnTable("DevelopmentProjects").To("CreatedBy");
            Execute.Sql(
                "UPDATE DevelopmentProjects SET CreatedBy = (SELECT username FROM tblPermissions WHERE RecId = CreatedBy)");

            Execute.Sql(
                "UPDATE DataType SET Table_Name = 'ProjectsDV', Data_Type = 'Project DV' WHERE Table_Name = 'DevelopmentProjects'");
            Execute.Sql("DELETE FROM DocumentType WHERE Document_Type = 'Development Project Document'");

            Delete.Column("DomesticWaterServices").FromTable("DevelopmentProjects");
            Delete.Column("FireServices").FromTable("DevelopmentProjects");
            Delete.Column("DomesticSanitaryServices").FromTable("DevelopmentProjects");

            Rename.Table("DevelopmentProjects")
                  .To("ProjectsDV");

            //Op Code- Make required
            Alter.Table("ProjectsDV")
                 .AlterColumn("OpCode").AsInt32().Nullable();

            //Project Category - Remove this field
            Alter.Table("ProjectsDV")
                 .AddForeignKeyColumn("ProjectCategory", "Lookup", "LookupId");

            //Change "Project Number" to  "Developer Services ID" 
            Rename.Column("DeveloperServicesID").OnTable("ProjectsDV").To("ProjectNumber");

            //Change "PP work order" to "WBS Number"
            Rename.Column("WBSNumber").OnTable("ProjectsDV").To("PPWorkOrder");

            //Category (make required)
            this.ReplaceLookupTableLookup("ProjectsDV", "Category", "DevelopmentProjectCategories", 50, "Category");
            Alter.Table("ProjectsDV")
                 .AlterForeignKeyColumn("Category", "Lookup", "LookupId");

            //Asset Category - Remove this field
            Alter.Table("ProjectsDV")
                 .AddForeignKeyColumn("AssetCategory", "Lookup", "LookupId");

            //Project Status - Remove this field
            Alter.Table("ProjectsDV")
                 .AddForeignKeyColumn("ProjectStatus", "Lookup", "LookupId");

            //Project Obstacles - Remove this field
            Alter.Table("ProjectsDV").AddColumn("ProjectObstacles").AsCustom("text").Nullable();

            //Project Risk- Remove this field
            Alter.Table("ProjectsDV").AddColumn("ProjectRisks").AsCustom("text").Nullable();

            //Project Approach -- Remove this field
            Alter.Table("ProjectsDV").AddColumn("ProjectApproach").AsCustom("text").Nullable();

            //Project Duration Months- Remove this field
            Alter.Table("ProjectsDV").AddColumn("ProjectDurationMonths").AsInt16().Nullable();

            //Estimated Project Cost - Remove this field
            Alter.Table("ProjectsDV").AddColumn("EstProjectCost").AsDecimal(18, 0).Nullable();

            //Phase - Remove this field
            Alter.Table("ProjectsDV")
                 .AddForeignKeyColumn("Phase", "Lookup", "LookupId");

            //MIS Dates - Remove this field
            Alter.Table("ProjectsDV").AddColumn("MISDates").AsBoolean().Nullable();

            //COE - Remove this field
            Alter.Table("ProjectsDV").AddColumn("COE").AsBoolean().Nullable();

            //BU (make required)
            Alter.Table("ProjectsDV")
                 .AlterColumn("BU").AsInt32().Nullable();

            //Project Description (make required)
            Alter.Table("ProjectsDV").AlterColumn("ProjectDescription").AsCustom("text").Nullable();

            //PWSID (make required)
            Alter.Table("ProjectsDV")
                 .AlterColumn("PWSID").AsInt32().Nullable();
        }
    }
}
