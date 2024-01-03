using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160909112433707), Tags("Production")]
    public class RestorationMethodsBug2784 : Migration
    {
        private void AddRestMethod(string methodName, bool isInitial, bool isFinal, params int[] types)
        {
            //Insert.IntoTable("RestorationMethods").Row(new { Description = methodName });

            Execute.Sql(@"
IF NOT EXISTS (SELECT * FROM [RestorationMethods] 
                   WHERE Description = '{0}')
   BEGIN
       INSERT INTO [RestorationMethods] (Description) VALUES ('{0}')
   END
", methodName);

            foreach (var type in types)
            {
                Execute.Sql(@"
        declare @methodId int;
        set @methodId = (select RestorationMethodId from RestorationMethods where Description = '{0}')

IF NOT EXISTS (SELECT * FROM [RestorationMethodsRestorationTypes] WHERE RestorationMethodID = @methodId and RestorationTypeID = {1})
BEGIN
        insert into [RestorationMethodsRestorationTypes] (RestorationMethodID, RestorationTypeID, InitialMethod, FinalMethod)
        values (@methodId, {1}, '{2}', '{3}')
END
", methodName, type, isInitial, isFinal);
            }
        }

        public override void Up()
        {
            /* RestorationTypeID	Description
1	ASPHALT-STREET
2	ASPHALT-DRIVEWAY
3	CONCRETE STREET
4	CURB RESTORATION
5	CURB/GUTTER RESTORATION
6	DRIVEWAY APRON RESTORATION
7	GROUND RESTORATION
8	SIDEWALK RESTORATION
9	ASPHALT - ALLEY
          */

            AddRestMethod("6\" Stab Base", true, false, 1, 2, 9);
            AddRestMethod("10\" Stab Base", true, false, 1, 2, 9);
            AddRestMethod("Cold Patch", true, false, 1, 2, 9);
            AddRestMethod("Black Mulch", false, true, 7);
        }

        public override void Down() { }
    }
}
