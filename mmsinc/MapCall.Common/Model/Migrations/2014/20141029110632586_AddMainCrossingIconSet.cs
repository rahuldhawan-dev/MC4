using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20141029110632586), Tags("Production")]
    public class AddMainCrossingIconSet : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
    SET IDENTITY_INSERT [IconSets] ON
    insert into IconSets(Id, Name, DefaultIconId) Values(10, 'MainCrossings', 8) 
    SET IDENTITY_INSERT [IconSets] OFF

    insert into MapIconIconSets(IconId, IconSetId) values(4, 10); -- pin_red
    insert into MapIconIconSets(IconId, IconSetId) values(8, 10); -- pin_blue
            ");
        }

        public override void Down()
        {
            Execute.Sql(@"
        delete from MapIconIconSets where IconSetId = 10
        delete from IconSets where Id = 10
          ");
        }
    }
}
