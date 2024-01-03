using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230201101403501), Tags("Production")]
    public class MC2780_FixFireDistrictColumnTypesAndNullability : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
UPDATE FireDistricts
SET StateID = s.StateId
FROM States s
WHERE s.Abbreviation = 'IL'
AND FireDistricts.StateID IS NULL
AND FireDistricts.DistrictName IN ('Frankfort', 'Warrenville', 'Sandwich');");

            Alter.Column("StateID")
                 .OnTable("FireDistricts")
                 .AsForeignKey("StateID", "States", "StateId", nullable: false);

            Alter.Column("DistrictName")
                 .OnTable("FireDistricts")
                 .AsString(50).NotNullable();
        }

        public override void Down()
        {
            Alter.Column("DistrictName")
                 .OnTable("FireDistricts")
                 .AsString(50).Nullable();

            Alter.Column("StateID")
                 .OnTable("FireDistricts")
                 .AsInt32().Nullable();
        }
    }
}

