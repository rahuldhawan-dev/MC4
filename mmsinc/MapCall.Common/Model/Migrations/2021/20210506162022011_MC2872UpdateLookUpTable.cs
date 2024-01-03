using FluentMigrator;
using FluentMigrator.Expressions;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210506162022011), Tags("Production")]
    public class MC2872UpdateLookUpTable : Migration
    {
        public override void Up()
        {
            Execute.Sql($@"UPDATE HazardTypes 
            SET Description = 'Telephone'
            WHERE Description like 'Telephone to Start' ");
        }

        public override void Down()
        {
            Execute.Sql($@"UPDATE HazardTypes 
            SET Description = 'Telephone to Start'
            WHERE Description like 'Telephone' ");
        }
    }
}