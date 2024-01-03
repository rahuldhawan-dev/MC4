using System;
using System.Globalization;
using CsvHelper.Configuration;
using MapCallScheduler.Library.JobHelpers.Sap;

namespace MapCallScheduler.JobHelpers.LeakAlert
{
    public class LeakAlertFileParser : SapDelimitedFileParserBase<LeakAlertFileRecord>, ILeakAlertFileParser
    {
        protected override CsvConfiguration CsvConfiguration => new CsvConfiguration(CultureInfo.CurrentCulture) { HasHeaderRecord = false };

        protected override Action<LeakAlertFileRecord, string>[] GetConversionTable()
        {
            return new Action<LeakAlertFileRecord, string>[] {
                (r, s) => r.PersistedCorrelatedNoiseId = s,
                (r, s) => r.DatePCNCreated = s,
                (r, s) => r.POIStatusId = s,
                (r, s) => r.POIStatus = s,
                (r, s) => r.Latitude = s,
                (r, s) => r.Longitude = s,
                (r, s) => r.SocketID1 = s,
                (r, s) => r.SocketID2 = s,
                (r, s) => r.DistanceFrom1 = s,
                (r, s) => r.DistanceFrom2 = s,
                (r, s) => r.Note = s,
                (r, s) => r.SiteName = s,
                (r, s) => r.FieldInvestigationRecommendedOn = s
            };
        }
    }
}
