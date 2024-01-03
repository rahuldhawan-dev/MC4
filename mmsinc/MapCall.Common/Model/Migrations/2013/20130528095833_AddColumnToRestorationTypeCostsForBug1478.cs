using System;
using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20130528095833), Tags("Production")]
    public class AddColumnToRestorationTypeCostsForBug1478 : Migration
    {
        #region Constants

        public struct Columns
        {
            public const string FINAL_COST = "FinalCost";
        }

        public struct Tables
        {
            public const string RESTORATION_TYPE_COSTS = "RestorationTypeCosts";
        }

        #endregion

        public override void Up()
        {
            Alter.Table(Tables.RESTORATION_TYPE_COSTS)
                 .AddColumn(Columns.FINAL_COST)
                 .AsInt32()
                 .Nullable();
            Execute.Sql(String.Format("UPDATE [{0}] SET [{1}] = {2}", Tables.RESTORATION_TYPE_COSTS, Columns.FINAL_COST,
                6));
            Alter.Column(Columns.FINAL_COST)
                 .OnTable(Tables.RESTORATION_TYPE_COSTS)
                 .AsInt32()
                 .NotNullable();
        }

        public override void Down()
        {
            Delete.Column(Columns.FINAL_COST)
                  .FromTable(Tables.RESTORATION_TYPE_COSTS);
        }
    }
}
