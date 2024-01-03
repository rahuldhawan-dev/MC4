using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;
using MMSINC.ClassExtensions.IEnumerableExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20151009103824820), Tags("Production")]
    public class AlterConstraintsOnOneCallMarkoutTicketsForBug2647 : Migration
    {
        #region Constants

        public const string TABLE_NAME = CreateMarkoutTicketsTablesAndSuchForBug2630.TableNames.MARKOUT_TICKETS;
        public const string KEY_NAME = "RequestNumberAndCDCCode";

        #endregion

        #region Private Methods

        public static string GetConstraintName(string table)
        {
            return String.Format("UQ_{0}_RequestNumberCDCCode", table);
        }

        private void UpConstraints(string table, string extraCol = null)
        {
            var columns = new List<string> {"RequestNumber", "CDCCode"};

            if (extraCol != null)
            {
                columns.Add(extraCol);
            }

            Execute.Sql(@"DROP INDEX [IX_{0}_RequestNumber] ON [dbo].[{0}]", table);
            Execute.Sql(@"CREATE UNIQUE NONCLUSTERED INDEX [IX_{0}_{1}] ON [dbo].[{0}]
(
	{2}
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];",
                table, String.Join("", columns),
                String.Join(", ", columns.Map<string, string>(c => string.Format("[{0}] ASC", c))));
            Create.UniqueConstraint(GetConstraintName(table))
                  .OnTable(table)
                  .Columns(columns.ToArray());
        }

        private void DownConstraints(string table, string extraCol = null)
        {
            var columns = new List<string> {"RequestNumber", "CDCCode"};

            if (extraCol != null)
            {
                columns.Add(extraCol);
            }

            Delete.UniqueConstraint(GetConstraintName(table)).FromTable(table);
            Execute.Sql(@"DROP INDEX [IX_{0}_{1}] ON [dbo].[{0}]", table, String.Join("", columns));
            Execute.Sql(@"CREATE UNIQUE NONCLUSTERED INDEX [IX_{0}_RequestNumber] ON [dbo].[{0}]
(
	[RequestNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];",
                table);
        }

        #endregion

        #region Exposed Methods

        public override void Up()
        {
            UpConstraints(TABLE_NAME);

            Create.Column("CDCCode")
                  .OnTable(CreateMarkoutTicketsTablesAndSuchForBug2630.TableNames.AUDIT_TICKET_NUMBERS)
                  .AsString(AddFieldsToOneCallTicketsForBug2644.StringLengths.CDC_CODE)
                  .Nullable();

            var updates = new List<string>();

            Execute.WithConnection((conn, tran) => {
                using (var cmd = conn.CreateCommand())
                {
                    var receivingTerminal = new Regex(@"Receiving\s+Terminal:\s+([^\s]+)");
                    cmd.Transaction = tran;

                    cmd.CommandText = String.Format("SELECT Id, FullText FROM {0}",
                        CreateMarkoutTicketsTablesAndSuchForBug2630.TableNames.AUDITS);
                    var i = 0;

                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var id = rdr.GetInt32(0);
                            var fullText = rdr.GetString(1);

                            if (!receivingTerminal.IsMatch(fullText))
                            {
                                throw new InvalidOperationException(String.Format(
                                    "Processed {0} messages so far. String was not found in full text of audit {1}:{2}{3}",
                                    i, id, Environment.NewLine, fullText));
                            }

                            var query = String.Format(
                                "UPDATE {0} SET CDCCode = '{1}' WHERE AuditId = {2}",
                                CreateMarkoutTicketsTablesAndSuchForBug2630.TableNames.AUDIT_TICKET_NUMBERS,
                                receivingTerminal.Match(fullText).Groups[1].Value.Trim().Replace("'", "''"), id);

                            updates.Add(query);
                            i++;
                        }
                    }

                    foreach (var update in updates)
                    {
                        cmd.CommandText = update;
                        cmd.ExecuteNonQuery();
                    }
                }
            });

            Alter.Column("CDCCode")
                 .OnTable(CreateMarkoutTicketsTablesAndSuchForBug2630.TableNames.AUDIT_TICKET_NUMBERS)
                 .AsString(AddFieldsToOneCallTicketsForBug2644.StringLengths.CDC_CODE)
                 .NotNullable();

            UpConstraints(CreateMarkoutTicketsTablesAndSuchForBug2630.TableNames.AUDIT_TICKET_NUMBERS, "AuditId");
        }

        public override void Down()
        {
            Execute.Sql(
                "delete from OneCallMarkoutResponses where OneCallMarkoutTicketId in (select Id from OneCallMarkoutTickets where RequestNumber in (select RequestNumber from OneCallMarkoutTickets group by RequestNumber having count(1) > 1));" +
                "delete from OneCallMarkoutAuditTicketNumbers where RequestNumber in (select RequestNumber from OneCallMarkoutTickets group by RequestNumber having count(1) > 1);" +
                "delete from OneCallMarkoutAuditTicketNumbers where RequestNumber in (select RequestNumber from OneCallMarkoutAuditTicketNumbers group by RequestNumber having count(1) > 1);" +
                "delete from OneCallMarkoutTickets where RequestNumber in (select RequestNumber from OneCallMarkoutTickets group by RequestNumber having count(1) > 1);");

            DownConstraints(TABLE_NAME);
            DownConstraints(CreateMarkoutTicketsTablesAndSuchForBug2630.TableNames.AUDIT_TICKET_NUMBERS, "AuditId");
            Delete.Column("CDCCode")
                  .FromTable(CreateMarkoutTicketsTablesAndSuchForBug2630.TableNames.AUDIT_TICKET_NUMBERS);
        }

        #endregion
    }
}
