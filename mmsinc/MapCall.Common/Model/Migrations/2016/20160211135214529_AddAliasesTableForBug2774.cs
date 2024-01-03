using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160211135214529), Tags("Production")]
    public class AddAliasesTableForBug2774 : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("AmericanWaterAliases", "N J AMERICAN WATER CO",
                "NEW JERSEY AMERCAN WATER COMP", "NEW JERSEY AMERICAN WATER", "NEW JERSEY AMERICAN WATER CO",
                "NEW JERSEY AMERICAN WATER CO.", "NEW JERSEY AMERICAN WATER COM", "NEW JERSEY AMERICAN WATER COMP",
                "NEW JERSEY AMERICAN WATER COMPANY", "NEW JERSEY AMERICAN WATRE", "NJ AMERICAN WATER",
                "NJ AMERICAN WATER CO", "NJ AMERICAN WATER CO.", "NJ AMERICAN WATER COMP", "NJ AMERICAN WATER COMPANY",
                "NJ AMERICAN WTR", "NJ AMERICAN WTR CO", "NJ AMERICAN WTR CO.", "NJAM WATER", "NEW JERSEY AMERICAN",
                "NEW JERSEY AMERICAN WATER'", "NEW JERSEY AMERICAN WATERQ", "NJ  AMERICAN WATER COMPANY",
                "NJ AMERICA WATER", "NJ AMERICA WATER CO", "NJ AMERICA WATER COMPANY", "NJ AMERICAN",
                "NJ AMERICAN ATER",
                "NJ AMERICAN WAER COMPANY", "NJ AMERICAN WATE COMPANY", "NJ AMERICAN WATER ACO",
                "NJ AMERICAN WATER COM",
                "NJ AMERICAN WATER COMAPNY", "NJ AMERICAN WATER COMMPANY", "NJ AMERICAN WATER COMPNAY",
                "NJ AMERICAN WATER.", "NJ AMERICAN WTR COMPANY", "NJ AMREICAN WATER", "NJ ANERICAN WATER", "NJ AW",
                "NJ JERSEY AMERICAN WATER", "NJ MERICAN WATER", "NJ WATER COMPANY", "NJAMERICAN WATER",
                "NJAMERICAN WATER CO");
        }

        public override void Down()
        {
            Delete.Table("AmericanWaterAliases");
        }
    }
}
