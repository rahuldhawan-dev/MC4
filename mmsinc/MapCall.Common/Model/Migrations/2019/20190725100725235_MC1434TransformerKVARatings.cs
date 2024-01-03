using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190725100725235), Tags("Production")]
    public class MC1434TransformerKVARatings : Migration
    {
        private void AddVoltageRating(string voltage, params string[] kvaRatings)
        {
            foreach (var kvr in kvaRatings)
            {
                Execute.Sql(
                    $@"insert into UtilityTransformerKVARatingsVoltages (VoltageId, UtilityTransformerKVARatingId) VALUES((select Id from Voltages where Description = '{voltage}'), (select Id from UtilityTransformerKVARatings where Description = '{kvr}'))");
            }
        }

        private void CleanupExistingData()
        {
            // This information is from Michael Mantione
            // 357 - It's three phase, so the current value of 35 is invalid. It should be nulled out.
            // 573, 580, and 924 are all invalid and can be nulled.
            // 353, 366 can be nulled because the Voltage value is null.
            Update.Table("tblFacilities").Set(new {TransformerKVARating = (object)null}).Where(new {RecordID = 357});
            Update.Table("tblFacilities").Set(new {TransformerKVARating = (object)null}).Where(new {RecordID = 353});
            Update.Table("tblFacilities").Set(new {TransformerKVARating = (object)null}).Where(new {RecordID = 366});
            Update.Table("tblFacilities").Set(new {TransformerKVARating = (object)null}).Where(new {RecordID = 573});
            Update.Table("tblFacilities").Set(new {TransformerKVARating = (object)null}).Where(new {RecordID = 580});
            Update.Table("tblFacilities").Set(new {TransformerKVARating = (object)null}).Where(new {RecordID = 924});
        }

        public override void Up()
        {
            CleanupExistingData();

            // Insert new voltage
            Insert.IntoTable("Voltages").Row(new {Description = "480"});

            // Create UtilityTransformerKVARatings table
            // Insert all of the new values to UtilityTransformerKVARatings table

            this.CreateLookupTableWithValues("UtilityTransformerKVARatings", "1", "1.5", "2", "3", "5", "6", "7.5",
                "9", "10", "15", "25", "30", "37.5", "45", "50",
                "75", "100", "112.5", "125", "150", "167", "200", "225",
                "250", "300", "333", "500", "750", "1000",
                "3000", "5000", "Other");

            // Create ManyToMany table
            Create.ManyToManyTable("UtilityTransformerKVARatings", "Voltages");

            // Add Voltages/KVARatings to ManyToMany table
            AddVoltageRating("120/240", "1", "1.5", "2", "3", "5", "6", "7.5",
                "9", "10", "15", "25", "30", "37.5", "45", "50",
                "75", "100", "112.5", "150", "167", "200", "225",
                "250", "300", "333", "500", "750", "1000", "Other");

            AddVoltageRating("120/208", "3", "6", "9", "15", "25", "30", "45", "75", "100",
                "112.5", "150", "225", "300", "500", "750", "1000", "3000", "Other");

            AddVoltageRating("240", "3", "5", "6", "9", "10", "15", "30", "45", "75",
                "112.5", "150", "225", "300", "500", "750", "1000", "Other");

            AddVoltageRating("277/480", "3", "6", "9", "15", "25", "30", "45", "50", "75", "100",
                "112.5", "125", "150", "225", "300", "500", "750", "1000", "3000", "5000", "Other");

            AddVoltageRating("480", "3", "6", "9", "15", "30", "45", "75", "112.5",
                "150", "225", "300", "500", "750", "1000", "3000", "5000", "Other");

            // Create new column on tblFacilities that links to UtilityTransformerKVARatings
            Create.Column("UtilityTransformerKVARatingId").OnTable("tblFacilities")
                  .AsInt32().Nullable()
                  .ForeignKey("FK_tblFacilities_UtilityTransformerKVARatings_UtilityTransformerKVARatingId",
                       "UtilityTransformerKVARatings", "Id");

            // Import data from old column to new column
            Execute.Sql(@"
update tblFacilities 
set UtilityTransformerKVARatingId = (select Id from UtilityTransformerKVARatings ut
	where ut.Description = tblFacilities.TransformerKVARating 
	and exists(select * from UtilityTransformerKVARatingsVoltages utr where utr.VoltageId = tblFacilities.VoltageId and utr.UtilityTransformerKVARatingId = ut.Id))
");

            Delete.Column("TransformerKVARating").FromTable("tblFacilities");
        }

        public override void Down()
        {
            // Set the original data back.
            Create.Column("TransformerKVARating").OnTable("tblFacilities")
                  .AsString(6).Nullable();
            Execute.Sql(
                "update tblFacilities set TransformerKVARating = (select Description from UtilityTransformerKVARatings u where u.Id = UtilityTransformerKVARatingId)");

            Delete.ForeignKey("FK_tblFacilities_UtilityTransformerKVARatings_UtilityTransformerKVARatingId")
                  .OnTable("tblFacilities");
            Delete.Column("UtilityTransformerKVARatingId").FromTable("tblFacilities");

            Delete.Table("UtilityTransformerKVARatingsVoltages");
            Delete.Table("UtilityTransformerKVARatings");
        }
    }
}
