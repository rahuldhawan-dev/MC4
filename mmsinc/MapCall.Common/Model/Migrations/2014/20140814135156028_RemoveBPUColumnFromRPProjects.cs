using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140814135156028), Tags("Production")]
    public class RemoveBPUColumnFromRPProjects : Migration
    {
        public override void Up() { }

        public override void Down() { }
    }

    public abstract class MapCallMigration : Migration { }
}
