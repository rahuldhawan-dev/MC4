using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160722140855150), Tags("Production")]
    public class NormalizeOutPrimaryDriverForProposalAndAddCostModelNeededForBug3046 : Migration
    {
        public override void Up()
        {
            Execute.WithConnection((conn, tran) => {
                var badDrivers = new List<string[]>();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tran;
                    cmd.CommandText =
                        "SELECT DISTINCT PrimaryDriverForProposal FROM UnionContractProposals WHERE PrimaryDriverForProposal IS NOT NULL;";

                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var current = rdr.GetString(0);
                            var regexes = new[] {new Regex("^( ?- ?)"), new Regex("( ?[,;] ?)$")};
                            foreach (var rgx in regexes)
                            {
                                if (rgx.IsMatch(current))
                                {
                                    var newValues = new[] {current, rgx.Replace(current, "")};

                                    Console.WriteLine(
                                        $"Found bad value, replacing '{newValues[0]}' with '{newValues[1]}'.");

                                    badDrivers.Add(newValues);
                                }
                            }
                        }
                    }

                    foreach (var pair in badDrivers)
                    {
                        cmd.CommandText =
                            $"UPDATE UnionContractProposals SET PrimaryDriverForProposal = '{pair[1]}' WHERE PrimaryDriverForProposal = '{pair[0]}';";

                        Console.WriteLine($"Executing query {cmd.CommandText}");

                        cmd.ExecuteNonQuery();
                    }
                }
            });

            this.ExtractNonLookupTableLookup("UnionContractProposals", "PrimaryDriverForProposal",
                "PrimaryDriversForProposals", 123);

            Create.Column("CostModelNeeded").OnTable("UnionContractProposals").AsBoolean().Nullable();
        }

        public override void Down()
        {
            this.ReplaceNonLookupTableLookup("UnionContractProposals", "PrimaryDriverForProposal",
                "PrimaryDriversForProposals", 123);

            Delete.Column("CostModelNeeded").FromTable("UnionContractProposals");
        }
    }
}
