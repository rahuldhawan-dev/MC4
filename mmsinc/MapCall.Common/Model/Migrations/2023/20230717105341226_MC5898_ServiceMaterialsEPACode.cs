using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230717105341226), Tags("Production")]
    public class MC5898_ServiceMaterialsEPACode : Migration
    {
        public override void Up()
        {
            // add table epa codes
            Create.Table("EPACodes")
                  .WithIdentityColumn()
                  .WithColumn("Description").AsString(500).NotNullable();

            // update service materials
            Alter.Table("ServiceMaterials")
                 .AddColumn("IsEditEnabled")
                 .AsBoolean()
                 .WithDefaultValue(false)
                 .NotNullable();

            Alter.Table("ServiceMaterials")
                 .AddForeignKeyColumn("CompanyEPACodeId", "EPACodes")
                 .AddForeignKeyColumn("CustomerEPACodeId", "EPACodes");

            // add table service material epa code overrides
            Create.Table("ServiceMaterialEPACodeOverrides")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("ServiceMaterialId", "ServiceMaterials", "ServiceMaterialId").NotNullable()
                  .WithForeignKeyColumn("StateId", "States", "StateId").NotNullable()
                  .WithForeignKeyColumn("CompanyEPACodeId", "EPACodes").NotNullable()
                  .WithForeignKeyColumn("CustomerEPACodeId", "EPACodes").NotNullable();

            // add new service materials
            var insertServiceMaterial =
                @"SET IDENTITY_INSERT [ServiceMaterials] ON;

                IF NOT EXISTS (SELECT * FROM ServiceMaterials WHERE [Description] = 'ORANGEBURG') 
                INSERT INTO ServiceMaterials ([ServiceMaterialId],[Description],[Code]) VALUES (26,'ORANGEBURG', NULL); 
                
                IF NOT EXISTS (SELECT * FROM ServiceMaterials WHERE [Description] = 'GALVANIZED REQUIRING REPLACEMENT') 
                INSERT INTO ServiceMaterials ([ServiceMaterialId],[Description],[Code]) VALUES (27,'GALVANIZED REQUIRING REPLACEMENT', NULL); 
                
                SET IDENTITY_INSERT [ServiceMaterials] OFF;";
            Execute.Sql(insertServiceMaterial);

            // update copper, ductile and plastic to be edit enabled
            var updateIsEditEnabledFlag =
                @"UPDATE ServiceMaterials SET [IsEditEnabled] = 1 WHERE [Description] IN ('COPPER', 'DUCTILE', 'PLASTIC')";
            Execute.Sql(updateIsEditEnabledFlag);

            // add epa codes
            var insertEpaCodes =
                @"INSERT INTO [EPACodes] ([Description]) VALUES 
                ('LEAD'), 
                ('GALVANIZED REQUIRING REPLACEMENT'), 
                ('LEAD STATUS UNKNOWN'), 
                ('NOT LEAD')";
            Execute.Sql(insertEpaCodes);

            // update epa code
            var setCompanyEpaCodes =
                @"UPDATE ServiceMaterials SET 
                CompanyEPACodeId = CASE 
                WHEN [Description] IN ('LEAD') THEN 1 
                WHEN [Description] IN ('GALVANIZED REQUIRING REPLACEMENT') THEN 2 
                WHEN [Description] IN ('UNKNOWN', 
                'CUSTOMER LINE NOT INSTALLED', 
                'UNCLASSIFIED - NO TAP INFO',  
                'UNCLASSIFIED - POTENTIAL LEAD', 
                'UNCLASSIFIED - UNLIKELY LEAD', 
                'UNCLASSIFIED - RESEARCH NEEDED', 
                'UNKNOWN NOT VISIBLE', 
                'UNKNOWN PIPE - HEAVILY PAINTED', 
                'UNKNOWN-VIA TEST', 
                'NULL') THEN 3 
                WHEN [Description] IN ('AC', 
                'CAST IRON', 
                'COPPER', 
                'DUCTILE', 
                'GALVANIZED', 
                'PLASTIC', 
                'GALVANIZED WITH LEAD GOOSENECK', 
                'OTHER WITH LEAD GOOSENECK', 
                'NOT LEAD – MATERIAL UNDETERMINED', 
                'BRASS') THEN 4 
                ELSE NULL 
                END";
            Execute.Sql(setCompanyEpaCodes);

            // customer epa codes are same as customer epa codes
            var setCustomerEpaCodes =
                "UPDATE ServiceMaterials SET CustomerEPACodeId = CompanyEPACodeId";
            Execute.Sql(setCustomerEpaCodes);

            // add overrides to 'galvanize' and 'galvanized requiring replacement' materials for NJ state (id - 1)
            var insertEpaCodeOverrides =
                @"INSERT INTO [ServiceMaterialEPACodeOverrides] (ServiceMaterialId, StateId, CompanyEPACodeId, CustomerEPACodeId) Values 
                ((Select ServiceMaterialId from ServiceMaterials where [Description] = 'GALVANIZED'), 1, 1, 1), 
                ((Select ServiceMaterialId from ServiceMaterials where [Description] = 'GALVANIZED REQUIRING REPLACEMENT'), 1, 1, 1)";
            Execute.Sql(insertEpaCodeOverrides);
        }

        public override void Down()
        {
            Delete.Column("IsEditEnabled").FromTable("ServiceMaterials");
            Delete.ForeignKeyColumn("ServiceMaterials", "CompanyEPACodeId", "EPACodes");
            Delete.ForeignKeyColumn("ServiceMaterials", "CustomerEPACodeId", "EPACodes");
            Delete.Table("ServiceMaterialEPACodeOverrides");
            Delete.Table("EPACodes");
        }
    }
}
