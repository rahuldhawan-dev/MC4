using System;
using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190221095919070), Tags("Production")]
    public class MC872AddIsActiveToMeterChangeOutContracts : Migration
    {
        public override void Up()
        {
            Create.Column("IsActive").OnTable("MeterChangeOutContracts")
                  .AsBoolean().NotNullable().WithDefaultValue(false);

            Action<string> activate = (desc) => {
                Update.Table("MeterChangeOutContracts").Set(new {IsActive = true}).Where(new {Description = desc});
            };

            activate("MMSI-NJ4-Lakewood-2019-A.xlsx");
            activate("MMSI-NJ4-2018-A.xlsx");
            activate("MMSI-NJ4-2018-TEMP .xlsx"); // Yes, there's a space in the file name.
            activate("MMSI-NJ7S-2018-A.xlsx");
            activate("MMSI-NJ7S-2019-A.xlsx");
            activate("LPH-NJ3-2018-A.xlsx");
            activate("LPH-NJ3-2019-A.xlsx");
            activate("NM-NY1-2019-A.xlsx");
            activate("AV-NJ7-2018-A.xlsx");
            activate("AV-NJ7-2018-B.xlsx");
            activate("AV-NJ7-2018-D.xlsx");
            activate("AV-NJ7-2019-A.xlsx");
        }

        public override void Down()
        {
            Delete.Column("IsActive").FromTable("MeterChangeOutContracts");
        }
    }
}
