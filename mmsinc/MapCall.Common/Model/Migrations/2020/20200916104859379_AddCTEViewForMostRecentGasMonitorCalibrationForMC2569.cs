using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200916104859379), Tags("Production")]
    public class AddCTEViewForMostRecentGasMonitorCalibrationForMC2569 : Migration
    {
        public const string CREATE_VIEW = @"
CREATE VIEW 
    MostRecentGasMonitorCalibration
AS 
WITH MostRecentGasCalibrations AS (
    SELECT  
        GasMonitorId,
        Max(CalibrationDate) AS CalibrationDate
    FROM
        GasMonitorCalibrations
    WHERE
        CalibrationPassed = 1
    GROUP BY 
        GasMonitorId
)
SELECT
    Id,
    MRGC.CalibrationDate,
    CASE WHEN (MRGC.GasMonitorId IS NULL) THEN 1 WHEN DateDiff(""day"", MRGC.CalibrationDate, getDate()) > GM.CalibrationFrequencyDays THEN 1 ELSE 0 END as DueCalibration,
    DateAdd(""day"", CalibrationFrequencyDays,  MRGC.CalibrationDate) as NextDueDate
FROM
    GasMonitors GM
LEFT JOIN
    MostRecentGasCalibrations MRGC
ON 
    GM.Id = MRGC.GasMonitorId;",
                            DROP_VIEW = "DROP VIEW MostRecentGasMonitorCalibration";

        public override void Up()
        {
            Execute.Sql(CREATE_VIEW);
        }

        public override void Down()
        {
            Execute.Sql(DROP_VIEW);
        }
    }
}
