using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20151007092309585), Tags("Production")]
    public class AddFieldsToOneCallTicketsForBug2644 : Migration
    {
        public const string TABLE_NAME = CreateMarkoutTicketsTablesAndSuchForBug2630.TableNames.MARKOUT_TICKETS;

        public struct Regexes
        {
            public static readonly Regex TYPE_OF_WORK =
                                             new Regex(@"^Type\s+of\s+Work:\s+([^\r\n]+)\s*$", RegexOptions.Multiline),
                                         WORKING_FOR = new Regex(@"^Working\s+For:\s+([^\r\n]+)\s*$",
                                             RegexOptions.Multiline),
                                         EXCAVATOR = new Regex(@"^Excavator:\s+([^\r\n]+)\s*$", RegexOptions.Multiline),
                                         CDC_CODE = new Regex(@"CDC\s+=\s+([^\s]+)");
        }

        public struct ColumnNames
        {
            public const string TYPE_OF_WORK = "TypeOfWork",
                                WORKING_FOR = "WorkingFor",
                                EXCAVATOR = "Excavator",
                                CDC_CODE = "CDCCode";
        }

        public struct StringLengths
        {
            public const int TYPE_OF_WORK = 75,
                             WORKING_FOR = 50,
                             EXCAVATOR = 50,
                             CDC_CODE = 20;
        }

        public override void Up()
        {
            Create.Column(ColumnNames.TYPE_OF_WORK).OnTable(TABLE_NAME).AsString(StringLengths.TYPE_OF_WORK).Nullable();
            Create.Column(ColumnNames.WORKING_FOR).OnTable(TABLE_NAME).AsString(StringLengths.WORKING_FOR).Nullable();
            Create.Column(ColumnNames.EXCAVATOR).OnTable(TABLE_NAME).AsString(StringLengths.EXCAVATOR).Nullable();
            Create.Column(ColumnNames.CDC_CODE).OnTable(TABLE_NAME).AsString(StringLengths.CDC_CODE).Nullable();

            var updates = new List<string>();

            Execute.Command(String.Format("SELECT Id, FullText FROM {0}", TABLE_NAME), cmd => {
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        var id = rdr.GetInt32(0);
                        var fullText = rdr.GetString(1);

                        if (!Regexes.TYPE_OF_WORK.IsMatch(fullText))
                        {
                            throw new InvalidOperationException();
                        }

                        var typeOfWork = ParseRegex(fullText, Regexes.TYPE_OF_WORK);
                        var workingFor = ParseRegex(fullText, Regexes.WORKING_FOR);
                        var excavator = ParseRegex(fullText, Regexes.EXCAVATOR);
                        var cdcCode = ParseRegex(fullText, Regexes.CDC_CODE);

                        var query = String.Format(
                            "UPDATE {0} SET TypeOfWork = '{1}', WorkingFor = '{2}', Excavator = '{3}', CDCCode = '{4}' WHERE Id = {5}",
                            TABLE_NAME, typeOfWork, workingFor, excavator, cdcCode, id);

                        updates.Add(query);
                    }
                }

                foreach (var update in updates)
                {
                    cmd.CommandText = update;
                    cmd.ExecuteNonQuery();
                }
            });

            Alter.Column(ColumnNames.TYPE_OF_WORK).OnTable(TABLE_NAME).AsString(StringLengths.TYPE_OF_WORK)
                 .NotNullable();
            Alter.Column(ColumnNames.WORKING_FOR).OnTable(TABLE_NAME).AsString(StringLengths.WORKING_FOR).NotNullable();
            Alter.Column(ColumnNames.EXCAVATOR).OnTable(TABLE_NAME).AsString(StringLengths.EXCAVATOR).NotNullable();
            Alter.Column(ColumnNames.CDC_CODE).OnTable(TABLE_NAME).AsString(StringLengths.CDC_CODE).NotNullable();
        }

        public override void Down()
        {
            Delete.Column(ColumnNames.TYPE_OF_WORK).FromTable(TABLE_NAME);
            Delete.Column(ColumnNames.WORKING_FOR).FromTable(TABLE_NAME);
            Delete.Column(ColumnNames.EXCAVATOR).FromTable(TABLE_NAME);
            Delete.Column(ColumnNames.CDC_CODE).FromTable(TABLE_NAME);
        }

        private string ParseRegex(string input, Regex regex)
        {
            return regex.Match(input).Groups[1].Value.Trim().Replace("'", "''");
        }
    }
}
