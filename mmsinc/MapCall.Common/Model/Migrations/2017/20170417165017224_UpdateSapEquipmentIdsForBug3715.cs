using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170417165017224), Tags("Production")]
    public class UpdateSapEquipmentIdsForBug3715 : Migration
    {
        public override void Up()
        {
            Update.Table("Hydrants").Set(new {SapEquipmentId = 20304161}).Where(new {Id = 4198});
            Update.Table("Hydrants").Set(new {SapEquipmentId = 20300714}).Where(new {Id = 154029});
            Update.Table("Hydrants").Set(new {SapEquipmentId = 20304222}).Where(new {Id = 12751});
            Update.Table("Hydrants").Set(new {SapEquipmentId = 20289906}).Where(new {Id = 12867});
            Update.Table("Valves").Set(new {SapEquipmentId = 10877608}).Where(new {Id = 315934});
            Update.Table("Valves").Set(new {SapEquipmentId = 10877609}).Where(new {Id = 315935});
        }

        public override void Down() { }
    }
}
