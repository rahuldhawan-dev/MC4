using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140919171901909), Tags("Production")]
    public class CleanAsBuiltImagesTablebug2090 : Migration
    {
        private const string AS_BUILT_IMAGES = "AsBuiltImages";

        private const int MAX_FLD_LENGTH = 50,
                          MAX_FILELIST_LENGTH = 255;

        private void CleanUpBadStringData()
        {
            Execute.Sql(@"
update [AsBuiltImages] set StreetPrefix = null where StreetPrefix = ''
update [AsBuiltImages] set Street = null where Street = ''
update [AsBuiltImages] set StreetSuffix = null where StreetSuffix = ''
update [AsBuiltImages] set XStreetPrefix = null where XStreetPrefix = ''
update [AsBuiltImages] set CrossStreet = null where CrossStreet = ''
update [AsBuiltImages] set XStreetSuffix = null where XStreetSuffix = ''
update [AsBuiltImages] set TownSection = null where TownSection = ''
");
        }

        private void CleanUpOperatingCenterData()
        {
            Execute.Sql(@"
                update [AsBuiltImages] set OperatingCenterId = oct.OperatingCenterId
                from [AsBuiltImages]
                left join [OperatingCentersTowns] oct on oct.TownID = AsBuiltImages.TownID 
                where [AsBuiltImages].OperatingCenterId is null
                ");
        }

        public override void Up()
        {
            Alter.Column("fld").OnTable(AS_BUILT_IMAGES)
                 .AsString(MAX_FLD_LENGTH)
                 .NotNullable();

            Alter.Column("FileList").OnTable(AS_BUILT_IMAGES)
                 .AsString(MAX_FILELIST_LENGTH)
                 .NotNullable();

            Alter.Column("TownId").OnTable(AS_BUILT_IMAGES)
                 .AsInt32()
                 .NotNullable();

            // Can get through town.
            Delete.Column("DistrictID").FromTable(AS_BUILT_IMAGES);
            Delete.Column("OldAsBuiltId").FromTable(AS_BUILT_IMAGES);

            CleanUpBadStringData();
            CleanUpOperatingCenterData();
        }

        public override void Down()
        {
            Alter.Column("fld").OnTable(AS_BUILT_IMAGES)
                 .AsString(MAX_FLD_LENGTH)
                 .Nullable();

            Alter.Column("FileList").OnTable(AS_BUILT_IMAGES)
                 .AsString(MAX_FILELIST_LENGTH)
                 .Nullable();

            Alter.Column("TownId").OnTable(AS_BUILT_IMAGES)
                 .AsInt32()
                 .Nullable();

            Alter.Table(AS_BUILT_IMAGES)
                 .AddColumn("DistrictID").AsInt32().Nullable()
                 .AddColumn("OldAsBuiltId").AsInt32().Nullable();
        }
    }
}
