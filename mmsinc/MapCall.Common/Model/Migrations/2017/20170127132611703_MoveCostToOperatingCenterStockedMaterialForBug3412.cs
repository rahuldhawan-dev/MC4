using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170127132611703), Tags("Production")]
    public class MoveCostToOperatingCenterStockedMaterialForBug3412 : Migration
    {
        #region Private Methods

        private void MakeChange(string from, string to)
        {
            Create.Column("Cost").OnTable(to).AsCurrency().Nullable();

            Execute.Sql($"UPDATE {to} SET Cost = m.Cost FROM {@from} m WHERE {to}.MaterialId = m.MaterialId");

            Delete.Column("Cost").FromTable(@from);
        }

        #endregion

        #region Exposed Methods

        public override void Up()
        {
            MakeChange("Materials", "OperatingCenterStockedMaterials");
        }

        public override void Down()
        {
            MakeChange("OperatingCenterStockedMaterials", "Materials");
        }

        #endregion
    }
}
