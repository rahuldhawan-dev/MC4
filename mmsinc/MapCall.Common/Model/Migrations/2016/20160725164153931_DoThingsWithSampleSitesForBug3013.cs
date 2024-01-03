using System;
using System.Collections.Generic;
using System.Linq;
using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160725164153931), Tags("Production")]
    public class DoThingsWithSampleSitesForBug3013 : Migration
    {
        #region Constants

        public struct Notifications
        {
            #region Constants

            public const string RENEWAL_AT_SERVICE_WITH_SAMPLE_SITE = "Renewal At Service With Sample Site",
                                DEACTIVATED_SERVICE_WITH_SAMPLE_SITE = "Deactivated Service With Sample Site",
                                SAMPLE_SITE_ADDED_TO_SERVICE = "Sample Site Added To Service",
                                SAMPLE_SITE_SERVICE_CHANGED = "Sample Site Service Changed";

            #endregion
        }

        #endregion

        #region Exposed Methods

        public override void Up()
        {
            Execute.Sql($"INSERT INTO SampleSiteStatuses (Description) VALUES ('Alternate');");
            Execute.Sql($"INSERT INTO SampleSiteStatuses (Description) VALUES ('Denied');");
            Execute.Sql(
                "INSERT INTO MapIcon (iconURL, width, height, OffsetId) VALUES ('MapIcons/beaker_black.png', 29, 32, 2)");
            Execute.Sql(
                "INSERT INTO MapIcon (iconURL, width, height, OffsetId) VALUES ('MapIcons/beaker_blue.png', 29, 32, 2)");
            Execute.Sql(
                "INSERT INTO MapIcon (iconURL, width, height, OffsetId) VALUES ('MapIcons/beaker_yellow.png', 29, 32, 2)");
            Execute.Sql(
                "INSERT INTO MapIcon (iconURL, width, height, OffsetId) VALUES ('MapIcons/beaker_green.png', 29, 32, 2)");

            Execute.Sql(
                "INSERT INTO IconSets (Name, DefaultIconId) SELECT 'Beakers', iconID FROM MapIcon WHERE iconURL = 'MapIcons/beaker_green.png'");

            Execute.Sql(
                "insert into MapIconIconSets (IconId, IconSetId) select iconId, 13 from MapIcon where iconUrl like 'MapIcons/beaker_%'");

            Alter.Table("Services").AddForeignKeyColumn("RenewalOfId", "Services");

            var renewalCategories = new[] {
                "Water Reconnect",
                "Water Retire Service Only",
                "Water Service Increase Size",
                "Water Service Renewal",
                "Water Service Split"
            };

            Execute.WithConnection((conn, tran) => {
                var matches = new List<Tuple<string, string>>();
                var toUpdate = new List<List<int>>();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tran;
                    cmd.CommandText =
                        $"SELECT cast(s.ServiceNumber as varchar) + ', ' + s.PremiseNumber, count(cast(s.ServiceNumber as varchar) + ', ' + s.PremiseNumber) FROM Services s INNER JOIN ServiceCategories sc ON s.ServiceCategoryId = sc.ServiceCategoryId WHERE sc.Description IN ('{string.Join("', '", renewalCategories)}') AND s.CreatedOn IS NOT NULL and cast(s.ServiceNumber as varchar) <> '0' AND s.PremiseNumber NOT LIKE '0%' GROUP BY cast(s.ServiceNumber as varchar) + ', ' + s.PremiseNumber HAVING count(cast(s.ServiceNumber as varchar) + ', ' + s.PremiseNumber) > 1";
                    cmd.CommandText =
                        $"SELECT cast(s.ServiceNumber as varchar) + ', ' + s.PremiseNumber, count(cast(s.ServiceNumber as varchar) + ', ' + s.PremiseNumber) FROM Services s INNER JOIN ServiceCategories sc ON s.ServiceCategoryId = sc.ServiceCategoryId WHERE s.CreatedOn IS NOT NULL and cast(s.ServiceNumber as varchar) <> '0' AND s.PremiseNumber NOT LIKE '0%' GROUP BY cast(s.ServiceNumber as varchar) + ', ' + s.PremiseNumber HAVING count(cast(s.ServiceNumber as varchar) + ', ' + s.PremiseNumber) > 1";

                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var current = rdr.GetString(0).Split(',');

                            matches.Add(new Tuple<string, string>(current[0], current[1]));
                        }
                    }

                    foreach (var match in matches)
                    {
                        cmd.CommandText =
                            $"SELECT Id FROM {"Services"} WHERE CreatedOn IS NOT NULL AND ServiceNumber = '{match.Item1}' AND PremiseNumber = '{match.Item2}' ORDER BY CreatedOn DESC";
                        var ids = new List<int>();

                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                ids.Add(rdr.GetInt32(0));
                            }
                        }

                        if (ids.Any())
                        {
                            toUpdate.Add(ids);
                        }
                    }

                    foreach (var set in toUpdate)
                    {
                        for (var i = 0; i < set.Count - 1; ++i)
                        {
                            cmd.CommandText =
                                $"UPDATE {"Services"} SET RenewalOfId = {set[i]} WHERE Id = {set[i + 1]}";

                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            });

            this.AddNotificationType("Field Services", "Assets", Notifications.RENEWAL_AT_SERVICE_WITH_SAMPLE_SITE);
            this.AddNotificationType("Field Services", "Assets", Notifications.DEACTIVATED_SERVICE_WITH_SAMPLE_SITE);
            this.AddNotificationType("Water Quality", "General", Notifications.SAMPLE_SITE_ADDED_TO_SERVICE);
            this.AddNotificationType("Water Quality", "General", Notifications.SAMPLE_SITE_SERVICE_CHANGED);
        }

        public override void Down()
        {
            this.RemoveNotificationType("Field Services", "Assets", Notifications.RENEWAL_AT_SERVICE_WITH_SAMPLE_SITE);
            this.RemoveNotificationType("Field Services", "Assets", Notifications.DEACTIVATED_SERVICE_WITH_SAMPLE_SITE);
            this.RemoveNotificationType("Water Quality", "General", Notifications.SAMPLE_SITE_ADDED_TO_SERVICE);
            this.RemoveNotificationType("Water Quality", "General", Notifications.SAMPLE_SITE_SERVICE_CHANGED);

            Delete.ForeignKeyColumn("Services", "RenewalOfId", "Services");

            Execute.Sql("DELETE FROM MapIconIconSets WHERE IconSetId = 13");

            Execute.Sql(
                $"DELETE FROM SampleSiteStatuses WHERE (Description) IN ('Alternate', 'Denied');");

            Execute.Sql("DELETE FROM IconSets WHERE Name = 'Beakers'");

            Execute.Sql(
                "DELETE FROM MapIcon WHERE iconUrl IN ('MapIcons/beaker_black.png', 'MapIcons/beaker_blue.png', 'MapIcons/beaker_yellow.png', 'MapIcons/beaker_green.png')");
        }

        #endregion
    }
}
