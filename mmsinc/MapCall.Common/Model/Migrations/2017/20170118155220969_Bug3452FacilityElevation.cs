using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170118155220969), Tags("Production")]
    public class Bug3452FacilityElevation : Migration
    {
        public override void Up()
        {
            Alter.Column("Elevation").OnTable("tblFacilities")
                 .AsDecimal(18, 9).Nullable();
        }

        public override void Down()
        {
            // NOTE: At the time of making this migration, the column was a float
            // but we had it mapped as an int for some reason.
            Alter.Column("Elevation").OnTable("tblFacilities")
                 .AsFloat().Nullable();
        }
    }
}
