using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160426152718147), Tags("Production")]
    public class AddThingsToScadaThingsForBug2896 : Migration
    {
        public override void Up()
        {
            Alter.Table("ScadaTagNames")
                 .AddForeignKeyColumn("ScadaSignalId", "ScadaSignals");

            Execute.Sql(
                "UPDATE ScadaTagNames SET ScadaSignalId = s.Id FROM ScadaSignals s WHERE ScadaTagNames.TagName = s.TagName");
            Execute.Sql(
                "DELETE FROM ScadaReadings WHERE EXISTS (SELECT 1 FROM ScadaTagNames WHERE LTRIM(RTRIM(TagName)) = '' AND ScadaReadings.TagNameId = ScadaTagNames.Id)");
            Execute.Sql("DELETE FROM ScadaTagNames WHERE LTRIM(RTRIM(TagName)) = ''");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("ScadaTagNames", "ScadaSignalId", "ScadaSignals");
        }
    }
}
