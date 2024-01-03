using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150609102003472), Tags("Production")]
    public class UpdateRoutesFieldsForBug2423 : Migration
    {
        public override void Up()
        {
            Alter.Table("Valves").AddColumn("RouteNew").AsAnsiString(55).Nullable();
            Execute.Sql("UPDATE Valves set RouteNew = convert(varchar(55), route, 128)");
            Delete.Column("Route").FromTable("Valves");
            Alter.Table("Valves").AddColumn("Route").AsInt32().Nullable();
            Alter.Table("Valves").AddColumn("Stop").AsInt32().Nullable();

            Alter.Table("Hydrants").AddColumn("RouteNew").AsAnsiString(55).Nullable();
            Execute.Sql("Update Hydrants set RouteNew = convert(varchar(55), route, 128)");
            Delete.Column("Route").FromTable("Hydrants");
            Alter.Table("Hydrants").AddColumn("Route").AsInt32().Nullable();
            Alter.Table("Hydrants").AddColumn("Stop").AsInt32().Nullable();

            Execute.Sql("update valves set routeNew = '0' where routeNew = '0.0E0' or charindex('E', routeNew) > 0");
            Execute.Sql("update valves set route = routeNew where charindex('.', routeNew) = 0");
            Execute.Sql(
                "update valves set route = cast(left(routeNew, charindex('.', routeNew)-1) as int) where charindex('.', routeNew) > 0 ");
            Execute.Sql(
                "update valves set stop = cast(substring(routeNew, charindex('.', routeNew)+1, 50) as int) where charindex('.', routeNew) > 0 ");

            Execute.Sql("update hydrants set routeNew = '0' where routeNew = '0.0E0' or charindex('E', routeNew) > 0");
            Execute.Sql("update hydrants set route = routeNew where charindex('.', routeNew) = 0");
            Execute.Sql(
                "update hydrants set route = cast(left(routeNew, charindex('.', routeNew)-1) as int) where charindex('.', routeNew) > 0 ");
            Execute.Sql("update hydrants set routenew = '3.007' where routenew = '3.00699996948242'");
            Execute.Sql(
                "update hydrants set stop = cast(substring(routeNew, charindex('.', routeNew)+1, 50) as int) where charindex('.', routeNew) > 0 ");

            Delete.Column("RouteNew").FromTable("Valves");
            Delete.Column("RouteNew").FromTable("Hydrants");
        }

        public override void Down()
        {
            Alter.Column("Route").OnTable("Valves").AsFloat().Nullable();
            Alter.Column("Route").OnTable("Hydrants").AsFloat().Nullable();
            Execute.Sql(
                "update valves set route = cast(route as varchar) + '.' + right('00000' + cast(stop as varchar), 5) where stop is not null");
            Execute.Sql(
                "update hydrants set route = cast(route as varchar) + '.' + right('00000' + cast(stop as varchar), 5) where stop is not null");
            Delete.Column("Stop").FromTable("Valves");
            Delete.Column("Stop").FromTable("Hydrants");
        }
    }
}
