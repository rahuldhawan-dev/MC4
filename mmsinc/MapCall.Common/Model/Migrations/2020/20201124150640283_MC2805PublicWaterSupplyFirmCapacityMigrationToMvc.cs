using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20201124150640283), Tags("Production")]
    public class MC2805PublicWaterSupplyFirmCapacityMigrationToMvc : Migration
    {
        public const string UP_TABLE_NAME = "PublicWaterSupplyFirmCapacities";
        public const string UP_FK_PWS_COLUMN_NAME = "PublicWaterSupplyId";
        public const string PWS_TABLE_NAME = "PublicWaterSupplies";

        public const string DOWN_TABLE_NAME = "PWSIDFirmCapacity";
        public const string DOWN_PK_COLUMN_NAME = "PWSIDFirmCapacityID";
        public const string DOWN_FK_PWS_COLUMN_NAME = "PWSID";

        public const string ID_COLUMN_NAME = "Id";

        public override void Up()
        {
            Rename
               .Table(DOWN_TABLE_NAME)
               .To(UP_TABLE_NAME);

            Rename
               .Column(DOWN_PK_COLUMN_NAME)
               .OnTable(UP_TABLE_NAME)
               .To(ID_COLUMN_NAME);

            Rename
               .Column(DOWN_FK_PWS_COLUMN_NAME)
               .OnTable(UP_TABLE_NAME)
               .To(UP_FK_PWS_COLUMN_NAME);

            Rename
               .Column("CurrentSystemPeakDailyDemandYrMth")
               .OnTable(UP_TABLE_NAME)
               .To("CurrentSystemPeakDailyDemandYearMonth");

            Delete
               .ForeignKey($"FK_{DOWN_TABLE_NAME}_tblPWSID_PWSID")
               .OnTable(UP_TABLE_NAME);

            Delete
               .UniqueConstraint("UQ__PWSIDFirmCapacit__68DD7AB4")
               .FromTable(UP_TABLE_NAME);

            Delete
               .PrimaryKey($"PK_{DOWN_TABLE_NAME}")
               .FromTable(UP_TABLE_NAME);

            Create
               .PrimaryKey($"PK_{UP_TABLE_NAME}")
               .OnTable(UP_TABLE_NAME)
               .Column(ID_COLUMN_NAME);

            Create
               .ForeignKey($"FK_{UP_TABLE_NAME}_{PWS_TABLE_NAME}_{UP_FK_PWS_COLUMN_NAME}")
               .FromTable(UP_TABLE_NAME)
               .ForeignColumn(UP_FK_PWS_COLUMN_NAME)
               .ToTable(PWS_TABLE_NAME)
               .PrimaryColumn(ID_COLUMN_NAME);

            Execute.Sql($@"
update DataType
   set Table_Name = '{UP_TABLE_NAME}'
 where DataTypeID = 90
");
        }

        public override void Down()
        {
            Rename
               .Table(UP_TABLE_NAME)
               .To(DOWN_TABLE_NAME);

            Rename
               .Column(ID_COLUMN_NAME)
               .OnTable(DOWN_TABLE_NAME)
               .To(DOWN_PK_COLUMN_NAME);

            Rename
               .Column(UP_FK_PWS_COLUMN_NAME)
               .OnTable(DOWN_TABLE_NAME)
               .To(DOWN_FK_PWS_COLUMN_NAME);

            Rename
               .Column("CurrentSystemPeakDailyDemandYearMonth")
               .OnTable(DOWN_TABLE_NAME)
               .To("CurrentSystemPeakDailyDemandYrMth");

            Delete
               .PrimaryKey($"PK_{UP_TABLE_NAME}")
               .FromTable(DOWN_TABLE_NAME);

            Delete
               .ForeignKey($"FK_{UP_TABLE_NAME}_{PWS_TABLE_NAME}_{UP_FK_PWS_COLUMN_NAME}")
               .OnTable(DOWN_TABLE_NAME);

            Create
               .PrimaryKey($"PK_{DOWN_TABLE_NAME}")
               .OnTable(DOWN_TABLE_NAME)
               .Column(DOWN_PK_COLUMN_NAME);

            Create
               .ForeignKey($"FK_{DOWN_TABLE_NAME}_tblPWSID_PWSID")
               .FromTable(DOWN_TABLE_NAME)
               .ForeignColumn(DOWN_FK_PWS_COLUMN_NAME)
               .ToTable(PWS_TABLE_NAME)
               .PrimaryColumn(ID_COLUMN_NAME);

            Create
               .UniqueConstraint("UQ__PWSIDFirmCapacit__68DD7AB4")
               .OnTable(DOWN_TABLE_NAME)
               .Column(DOWN_PK_COLUMN_NAME);

            Execute.Sql($@"
update DataType
   set Table_Name = '{DOWN_TABLE_NAME}'
 where DataTypeID = 90
");
        }
    }
}
