using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230921104005524), Tags("Production")]
    public class MC6213_AddPremiseIdToWorkOrders : Migration
    {
        public override void Up()
        {
            Alter.Table("WorkOrders")
                 .AddForeignKeyColumn("PremiseId", "Premises")
                 .Nullable();
            Execute.Sql(@"Update w
                            Set w.PremiseId = p.Id
                            From Premises p
                            Inner Join WorkOrders w
                            On p.PremiseNumber = w.PremiseNumber
                            And p.DeviceLocation = cast(w.DeviceLocation as nvarchar)
                            And p.Installation = cast(w.Installation as nvarchar)");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("WorkOrders", "PremiseId", "Premises");
        }
    }
}

