using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20141030121751917), Tags("Production")]
    public class AdditionalHydInspFieldChangesForBug2175 : Migration
    {
        public const string TABLE_NAME = "tblNJAWHydInspData";

        public override void Up()
        {
            Execute.Sql(
                @"UPDATE tblNJAWHydInspData SET [Chlorine] = NULL WHERE isNumeric([Chlorine]) = 0 AND [Chlorine] IS NOT NULL
                          UPDATE tblNJAWHydInspData SET Chlorine = replace(chlorine, ',', '.') WHERE charindex(',', chlorine) > 0
                          UPDATE tblNJAWHydInspData SET Chlorine = null WHERE chlorine = '-'
                          UPDATE tblNJAWHydInspData SET Chlorine = null WHERE chlorine = '.'
                          UPDATE tblNJAWHydInspData SET Chlorine = null WHERE isNumeric(Chlorine) = 0 and chlorine is not null;
                          update tblNJAWHydInspData set pressStatic = rtrim(ltrim(pressStatic)) where isnumeric(pressStatic) = 1 and pressStatic is not null
                          update tblNJAWHydInspData set pressStatic = null where rtrim(ltrim(pressStatic)) in('.','')
                          update tblNJAWHydInspData set pressStatic = 0 where pressStatic like '00%'
                          update tblNJAWHydInspdata set pressStatic = '0.' + substring(pressStatic, 2, 10) where left(pressStatic, 1) = '0' and len(pressStatic) > 1 and charindex('.', pressStatic) = 0
                          update tblNJAWHydInspData set pressStatic = null where  pressStatic is not null and pressStatic <> '0' and cast(pressStatic as decimal) > 300.0 
                          ALTER TABLE [dbo].[tblNJAWHydInspData] DROP CONSTRAINT [DF_tblNJAWHydInspData_PressStatic]");
            Alter.Table(TABLE_NAME).AlterColumn("Chlorine").AsDecimal(3, 2).Nullable()
                 .AlterColumn("TotalChlorine").AsDecimal(3, 2).Nullable()
                 .AlterColumn("PressStatic").AsDecimal(5, 2).Nullable();
            Execute.Sql(
                "ALTER TABLE [dbo].[tblNJAWHydInspData] ADD  CONSTRAINT [DF_tblNJAWHydInspData_PressStatic]  DEFAULT (0) FOR [PressStatic]");
        }

        public override void Down()
        {
            Execute.Sql("ALTER TABLE [dbo].[tblNJAWHydInspData] DROP CONSTRAINT [DF_tblNJAWHydInspData_PressStatic]");
            Alter.Table(TABLE_NAME).AlterColumn("Chlorine").AsAnsiString(10).Nullable()
                 .AlterColumn("TotalChlorine").AsDecimal(5, 4).Nullable()
                 .AlterColumn("PressStatic").AsAnsiString(10).Nullable();
            Execute.Sql(
                "ALTER TABLE [dbo].[tblNJAWHydInspData] ADD  CONSTRAINT [DF_tblNJAWHydInspData_PressStatic]  DEFAULT (0) FOR [PressStatic]");
        }
    }
}
