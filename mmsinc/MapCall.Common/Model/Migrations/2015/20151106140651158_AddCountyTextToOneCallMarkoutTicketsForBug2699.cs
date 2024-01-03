using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20151106140651158), Tags("Production")]
    public class AddCountyTextToOneCallMarkoutTicketsForBug2699 : Migration
    {
        #region Constants

        public const string TABLE_NAME = CreateMarkoutTicketsTablesAndSuchForBug2630.TableNames.MARKOUT_TICKETS,
                            COUNTY_TEXT = "County",
                            COUNTY_ID = "CountyId";

        public struct Regexes
        {
            #region Constants

            public static readonly Regex COUNTY = new Regex(@"County:\s+([^\r\n]+)\s+Municipality");

            #endregion
        }

        #endregion

        #region Private Methods

        private string ParseRegex(string input, Regex regex)
        {
            return regex.Match(input).Groups[1].Value.Trim().Replace("'", "''");
        }

        #endregion

        #region Exposed Methods

        public override void Up()
        {
            Create.Column(COUNTY_TEXT).OnTable(TABLE_NAME).AsString(50).Nullable();

            var updates = new List<string>();

            Execute.Sql(
                "UPDATE {0} SET {1} = old.County FROM OneCallTickets old WHERE old.RequestNum = {0}.RequestNumber AND old.CDC = {0}.CDCCode",
                TABLE_NAME, COUNTY_TEXT);

            Execute.Command(String.Format("SELECT Id, FullText FROM {0} WHERE {1} IS NULL", TABLE_NAME, COUNTY_TEXT),
                cmd => {
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var id = rdr.GetInt32(0);
                            var fullText = rdr.GetString(1);

                            if (!Regexes.COUNTY.IsMatch(fullText))
                            {
                                throw new InvalidOperationException();
                            }

                            var county = ParseRegex(fullText, Regexes.COUNTY);

                            updates.Add(string.Format("UPDATE {0} SET {1} = '{2}' WHERE Id = {3}", TABLE_NAME,
                                COUNTY_TEXT,
                                county, id));
                        }
                    }

                    foreach (var update in updates)
                    {
                        cmd.CommandText = update;
                        cmd.ExecuteNonQuery();
                    }
                });

            Alter.Column(COUNTY_TEXT).OnTable(TABLE_NAME).AsString(50).NotNullable();
        }

        public override void Down()
        {
            Delete.Column(COUNTY_TEXT).FromTable(TABLE_NAME);
        }

        #endregion
    }
}
