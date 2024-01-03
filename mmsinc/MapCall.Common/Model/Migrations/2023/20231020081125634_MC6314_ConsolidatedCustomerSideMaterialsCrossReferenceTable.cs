using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20231020081125634), Tags("Production")]
    public class MC6314_ConsolidatedCustomerSideMaterials : Migration
    {
        public struct Tables
        {
            public const string CONSOLIDATED_MATERIALS = "ConsolidatedCustomerSideMaterials";
            public const string EPA_CODES = "EPACodes";
        }

        public struct Columns
        {
            public const string DISPLAY_CODE = "ConsolidatedEPACodeId",
                                CUSTOMER_CODE = "CustomerSideEPACodeId",
                                W1V_CODE = "CustomerSideExternalEPACodeId";
        }
        
        public string ADD_INITIAL_RECORDS = $@"Insert into {Tables.CONSOLIDATED_MATERIALS} ({Columns.CUSTOMER_CODE}, {Columns.W1V_CODE}, {Columns.DISPLAY_CODE}) Values (1,1,1);
Insert into {Tables.CONSOLIDATED_MATERIALS} ({Columns.CUSTOMER_CODE}, {Columns.W1V_CODE}, {Columns.DISPLAY_CODE}) Values (1,2,1);
Insert into {Tables.CONSOLIDATED_MATERIALS} ({Columns.CUSTOMER_CODE}, {Columns.W1V_CODE}, {Columns.DISPLAY_CODE}) Values (1,3,1);
Insert into {Tables.CONSOLIDATED_MATERIALS} ({Columns.CUSTOMER_CODE}, {Columns.W1V_CODE}, {Columns.DISPLAY_CODE}) Values (1,4,1);
Insert into {Tables.CONSOLIDATED_MATERIALS} ({Columns.CUSTOMER_CODE}, {Columns.W1V_CODE}, {Columns.DISPLAY_CODE}) Values (2,1,1);
Insert into {Tables.CONSOLIDATED_MATERIALS} ({Columns.CUSTOMER_CODE}, {Columns.W1V_CODE}, {Columns.DISPLAY_CODE}) Values (2,2,2);
Insert into {Tables.CONSOLIDATED_MATERIALS} ({Columns.CUSTOMER_CODE}, {Columns.W1V_CODE}, {Columns.DISPLAY_CODE}) Values (2,3,2);
Insert into {Tables.CONSOLIDATED_MATERIALS} ({Columns.CUSTOMER_CODE}, {Columns.W1V_CODE}, {Columns.DISPLAY_CODE}) Values (2,4,2);
Insert into {Tables.CONSOLIDATED_MATERIALS} ({Columns.CUSTOMER_CODE}, {Columns.W1V_CODE}, {Columns.DISPLAY_CODE}) Values (3,1,1);
Insert into {Tables.CONSOLIDATED_MATERIALS} ({Columns.CUSTOMER_CODE}, {Columns.W1V_CODE}, {Columns.DISPLAY_CODE}) Values (3,2,2);
Insert into {Tables.CONSOLIDATED_MATERIALS} ({Columns.CUSTOMER_CODE}, {Columns.W1V_CODE}, {Columns.DISPLAY_CODE}) Values (3,3,3);
Insert into {Tables.CONSOLIDATED_MATERIALS} ({Columns.CUSTOMER_CODE}, {Columns.W1V_CODE}, {Columns.DISPLAY_CODE}) Values (3,4,4);
Insert into {Tables.CONSOLIDATED_MATERIALS} ({Columns.CUSTOMER_CODE}, {Columns.W1V_CODE}, {Columns.DISPLAY_CODE}) Values (4,1,1);
Insert into {Tables.CONSOLIDATED_MATERIALS} ({Columns.CUSTOMER_CODE}, {Columns.W1V_CODE}, {Columns.DISPLAY_CODE}) Values (4,2,2);
Insert into {Tables.CONSOLIDATED_MATERIALS} ({Columns.CUSTOMER_CODE}, {Columns.W1V_CODE}, {Columns.DISPLAY_CODE}) Values (4,3,4);
Insert into {Tables.CONSOLIDATED_MATERIALS} ({Columns.CUSTOMER_CODE}, {Columns.W1V_CODE}, {Columns.DISPLAY_CODE}) Values (4,4,4);
Insert into {Tables.CONSOLIDATED_MATERIALS} ({Columns.CUSTOMER_CODE}, {Columns.DISPLAY_CODE}) Values (1,3);
Insert into {Tables.CONSOLIDATED_MATERIALS} ({Columns.CUSTOMER_CODE}, {Columns.DISPLAY_CODE}) Values (2,3);
Insert into {Tables.CONSOLIDATED_MATERIALS} ({Columns.CUSTOMER_CODE}, {Columns.DISPLAY_CODE}) Values (3,3);
Insert into {Tables.CONSOLIDATED_MATERIALS} ({Columns.CUSTOMER_CODE}, {Columns.DISPLAY_CODE}) Values (4,3);
Insert into {Tables.CONSOLIDATED_MATERIALS} ({Columns.W1V_CODE}, {Columns.DISPLAY_CODE}) Values (1,3);
Insert into {Tables.CONSOLIDATED_MATERIALS} ({Columns.W1V_CODE}, {Columns.DISPLAY_CODE}) Values (2,3);
Insert into {Tables.CONSOLIDATED_MATERIALS} ({Columns.W1V_CODE}, {Columns.DISPLAY_CODE}) Values (3,3);
Insert into {Tables.CONSOLIDATED_MATERIALS} ({Columns.W1V_CODE}, {Columns.DISPLAY_CODE}) Values (4,3);
Insert into {Tables.CONSOLIDATED_MATERIALS} ({Columns.DISPLAY_CODE}) Values (3);";
        
        public override void Up()
        {
            Create.Table(Tables.CONSOLIDATED_MATERIALS)
                  .WithIdentityColumn()
                  .WithForeignKeyColumn(Columns.DISPLAY_CODE, Tables.EPA_CODES).Nullable()
                  .WithForeignKeyColumn(Columns.CUSTOMER_CODE, Tables.EPA_CODES).Nullable()
                  .WithForeignKeyColumn(Columns.W1V_CODE, Tables.EPA_CODES).Nullable();
            
            Execute.Sql(ADD_INITIAL_RECORDS);
        }

        public override void Down()
        {
            Delete.Table(Tables.CONSOLIDATED_MATERIALS);
        }
    }
}

