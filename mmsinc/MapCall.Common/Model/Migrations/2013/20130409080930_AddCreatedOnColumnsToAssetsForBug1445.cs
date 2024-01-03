using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20130409080930), Tags("Production")]
    public class AddCreatedOnColumnsToAssetsForBug1445 : Migration
    {
        public struct Tables
        {
            public const string HYDRANTS = "tblNJAWHydrant",
                                VALVES = "tblNJAWValves",
                                SERVICES = "tblNJAWService";
        }

        public struct Columns
        {
            public const string CREATED_ON = "CreatedOn";
        }

        public override void Up()
        {
            Alter.Table(Tables.HYDRANTS)
                 .AddColumn(Columns.CREATED_ON)
                 .AsDateTime().Nullable()
                 .WithDefault(SystemMethods.CurrentDateTime);
            Alter.Table(Tables.VALVES)
                 .AddColumn(Columns.CREATED_ON)
                 .AsDateTime().Nullable()
                 .WithDefault(SystemMethods.CurrentDateTime);
            Alter.Table(Tables.SERVICES)
                 .AddColumn(Columns.CREATED_ON)
                 .AsDateTime().Nullable()
                 .WithDefault(SystemMethods.CurrentDateTime);
        }

        public override void Down()
        {
            Delete.Column(Columns.CREATED_ON)
                  .FromTable(Tables.HYDRANTS);
            Delete.Column(Columns.CREATED_ON)
                  .FromTable(Tables.VALVES);
            Delete.Column(Columns.CREATED_ON)
                  .FromTable(Tables.SERVICES);
        }
    }
}
