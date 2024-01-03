using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220908111056411), Tags("Production")]
    public class MC1335CreatingInterconnectionInspectionRatingLookupTable : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("InterconnectionInspectionRatings", "Satisfactory", "Unsatisfactory");
            Alter.Table("InterconnectionTests")
                 .AddForeignKeyColumn("InterconnectionInspectionRatingId", "InterconnectionInspectionRatings");
            Execute.Sql("Update InterconnectionTests set InspectionRating = 1 where InspectionRating = 644;" +
                        "Update InterconnectionTests set InspectionRating = 2 where InspectionRating = 645;" +
                        "UPDATE InterconnectionTests SET InterconnectionInspectionRatingId = InspectionRating;");
            this.DeleteForeignKeyColumn("InterconnectionTests", "InspectionRating", "Lookup");
        }

        public override void Down()
        {
            Alter.Table("InterconnectionTests").AddForeignKeyColumn("InspectionRating", "Lookup");
            Execute.Sql("UPDATE InterconnectionTests SET InspectionRating = InterconnectionInspectionRatingId;" +
                        "Update InterconnectionTests set InspectionRating = 644 where InspectionRating = 1;" +
                        "Update InterconnectionTests set InspectionRating = 645 where InspectionRating = 2;");
            this.DeleteForeignKeyColumn("InterconnectionTests", "InterconnectionInspectionRatingId", "InterconnectionInspectionRatings");
            Delete.Table("InterconnectionInspectionRatings");
        }
    }
}

