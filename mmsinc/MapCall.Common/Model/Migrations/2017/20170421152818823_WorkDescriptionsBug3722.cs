using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170421152818823), Tags("Production")]
    public class WorkDescriptionsBug3722 : Migration
    {
        public override void Up()
        {
            Execute.Sql(
                @"UPDATE [WorkDescriptions] SET description = 'RSTRN-RESTORATION INQUIRY - Main' WHERE WorkDescriptionID = 165

UPDATE [WorkDescriptions] SET description = 'RSTRN-RESTORATION INQUIRY - Service' WHERE WorkDescriptionID = 166

UPDATE [WorkDescriptions] SET description = 'RSTRN-RESTORATION INQUIRY - Sewer Lateral' WHERE WorkDescriptionID = 167

UPDATE [WorkDescriptions] SET description = 'RSTRN-RESTORATION INQUIRY - Sewer Main' WHERE WorkDescriptionID = 168

UPDATE [WorkDescriptions] SET description = 'NO ISSUE FOUND - Hydrant' WHERE WorkDescriptionID = 242

UPDATE [WorkDescriptions] SET description = 'NO ISSUE FOUND - Main' WHERE WorkDescriptionID = 243

UPDATE [WorkDescriptions] SET description = 'NO ISSUE FOUND - Main Crossing' WHERE WorkDescriptionID = 244

UPDATE [WorkDescriptions] SET description = 'NO ISSUE FOUND - Service' WHERE WorkDescriptionID = 245

UPDATE [WorkDescriptions] SET description = 'NO ISSUE FOUND - Sewer Lateral' WHERE WorkDescriptionID = 246

UPDATE [WorkDescriptions] SET description = 'NO ISSUE FOUND - Sewer Main' WHERE WorkDescriptionID = 247

UPDATE [WorkDescriptions] SET description = 'NO ISSUE FOUND - Sewer Manhole' WHERE WorkDescriptionID = 248

UPDATE [WorkDescriptions] SET description = 'NO ISSUE FOUND - Storm/Catch' WHERE WorkDescriptionID = 249

UPDATE [WorkDescriptions] SET description = 'NO ISSUE FOUND - Valve' WHERE WorkDescriptionID = 250

UPDATE [WorkDescriptions] SET description = 'ISSUE FOUND-OWNED BY OTHERS - Main' WHERE WorkDescriptionID = 251

UPDATE [WorkDescriptions] SET description = 'ISSUE FOUND-OWNED BY OTHERS - Main Crossing' WHERE WorkDescriptionID = 252

UPDATE [WorkDescriptions] SET description = 'ISSUE FOUND-OWNED BY OTHERS - Sewer Main' WHERE WorkDescriptionID = 253

UPDATE [WorkDescriptions] SET description = 'ISSUE FOUND-OWNED BY OTHERS - Storm/Catch' WHERE WorkDescriptionID = 254");
        }

        public override void Down() { }
    }
}
