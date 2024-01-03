using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20201103135557839), Tags("Production")]
    public class AddReadingCaptureTimesForConfinedSpaceFormsForMC2665 : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("ConfinedSpaceFormReadingCaptureTimes", "Pre-Entry", "During Entry",
                "Post Entry");
            Alter.Table("ConfinedSpaceFormAtmosphericTests")
                 .AddForeignKeyColumn("ConfinedSpaceFormReadingCaptureTimeId", "ConfinedSpaceFormReadingCaptureTimes");
            Execute.Sql("UPDATE [ConfinedSpaceFormAtmosphericTests] SET ConfinedSpaceFormReadingCaptureTimeId = 1");
            Alter.Column("ConfinedSpaceFormReadingCaptureTimeId").OnTable("ConfinedSpaceFormAtmosphericTests").AsInt32()
                 .NotNullable();
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("ConfinedSpaceFormAtmosphericTests", "ConfinedSpaceFormReadingCaptureTimeId",
                "ConfinedSpaceFormReadingCaptureTimes");
            Delete.Table("ConfinedSpaceFormReadingCaptureTimes");
        }
    }
}
