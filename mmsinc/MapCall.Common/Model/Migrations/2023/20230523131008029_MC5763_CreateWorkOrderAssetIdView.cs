using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230523131008029), Tags("Production")]
    public class MC5763_CreateWorkOrderAssetIdView : Migration
    {
        public const string VIEW_NAME = "WorkOrderAssetIdView";

        public const string CREATE_VIEW_SQL = "CREATE VIEW [" + VIEW_NAME + @"] AS
   SELECT
          wo.WorkOrderID AS Id,
          CASE
              WHEN AssetTypeID = 1 THEN v.ValveNumber
              WHEN AssetTypeID = 2 THEN h.HydrantNumber
              WHEN AssetTypeID IN (4, 6, 7)
                  THEN ISNULL('p#:' + wo.PremiseNumber, 'p#:') + ', ' +
                       ISNULL('s#:' + wo.ServiceNumber, 's#:')
              WHEN AssetTypeID = 5 THEN so.OpeningNumber
              WHEN AssetTypeID = 8 THEN sw.AssetNumber
              WHEN AssetTypeID = 9 THEN CAST(wo.EquipmentID AS varchar)
              WHEN AssetTypeID = 12 THEN 'CR' + CAST(wo.MainCrossingId AS varchar)
              ELSE ''
          END AS AssetId
     FROM WorkOrders wo
LEFT JOIN Valves v ON wo.ValveID = v.Id
LEFT JOIN Hydrants h ON wo.HydrantID = h.Id
LEFT JOIN SewerOpenings so ON wo.SewerOpeningId = so.Id
LEFT JOIN StormWaterAssets sw ON wo.StormCatchID = sw.StormWaterAssetID";

        public override void Up()
        {
            Execute.Sql(CREATE_VIEW_SQL);
        }

        public override void Down()
        {
            Execute.Sql($"DROP VIEW [{VIEW_NAME}];");
        }
    }
}

