using System;
using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140313131638), Tags("Production")]
    public class AddTablesForEstimatingProjectsMaterialsForBug1775 : Migration
    {
        #region Constants

        public struct TableNames
        {
            public const string ESTIMATING_PROJECT_MATERIALS = "EstimatingProjectsMaterials";
        }

        public struct ColumnNames
        {
            public struct EstimatingProjectsMaterials
            {
                public const string QUANTITY = "Quantity";
            }
        }

        #endregion

        public override void Up()
        {
            Alter.Table(TableNames.ESTIMATING_PROJECT_MATERIALS)
                 .AddColumn(ColumnNames.EstimatingProjectsMaterials.QUANTITY).AsInt32().Nullable();
        }

        public override void Down()
        {
            Delete.Column(ColumnNames.EstimatingProjectsMaterials.QUANTITY)
                  .FromTable(TableNames.ESTIMATING_PROJECT_MATERIALS);
        }
    }
}
