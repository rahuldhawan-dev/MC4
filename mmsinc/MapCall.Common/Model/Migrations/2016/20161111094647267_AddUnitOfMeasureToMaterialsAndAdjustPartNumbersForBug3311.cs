using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20161111094647267), Tags("Production")]
    public class AddUnitOfMeasureToMaterialsAndAdjustPartNumbersForBug3311 : Migration
    {
        public override void Up()
        {
            Create.Column("UnitOfMeasure").OnTable(nameof(Material) + "s").AsString(30).Nullable();

            Execute.Sql($"UPDATE MaterialsUsed SET MaterialId = 3898 WHERE MaterialId = 3899");

            Log($"Moving estimating project materials from 3899 to 3898");

            Execute.Sql($"UPDATE EstimatingProjectsMaterials SET MaterialId = 3898 WHERE MaterialId = 3899");

            Log($"Moving contractor inventory materials from 3899 to 3898");

            Execute.Sql($"UPDATE ContractorInventoriesMaterials SET MaterialId = 3898 WHERE MaterialId = 3899");

            Log($"Deleting 3899");

            Execute.Sql($"DELETE FROM Materials WHERE MaterialId = 3899");

            var duplicates = new List<string>();

            Execute.Command(
                "SELECT PartNumber FROM Materials m WHERE PartNumber <> '0' AND PartNumber <> 'NS' AND EXISTS (SELECT 1 FROM Materials tmp WHERE tmp.PartNumber = m.PartNumber GROUP BY tmp.PartNumber HAVING count(tmp.PartNumber) > 1)",
                cmd => {
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            duplicates.Add(rdr.GetString(0));
                        }
                    }

                    foreach (var partNumber in duplicates)
                    {
                        var materials = new List<Material>();
                        cmd.CommandText =
                            $"SELECT MaterialId, PartNumber, Description, IsActive FROM Materials WHERE PartNumber = '{partNumber}';";

                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                materials.Add(Material.FromReader(rdr));
                            }
                        }

                        if (
                            materials.All(
                                m =>
                                    m.Description.Replace("  ", " ") ==
                                    materials.First().Description.Replace("  ", " ")))
                        {
                            Log($"Materials with part number {partNumber} all share the same description");

                            if (!materials.Any(m => m.IsActive) || materials.Count(m => m.IsActive) == 1)
                            {
                                var toKeep = materials.SingleOrDefault(m => m.IsActive) ?? materials.First();

                                materials.Where(m => m.Id != toKeep.Id).Each(m => {
                                    Log($"Moving materials used from {m.Id} to {toKeep.Id}");

                                    cmd.ExecuteNonQuery(
                                        $"UPDATE MaterialsUsed SET MaterialId = {toKeep.Id} WHERE MaterialId = {m.Id}");

                                    Log($"Moving estimating project materials from {m.Id} to {toKeep.Id}");

                                    cmd.ExecuteNonQuery(
                                        $"UPDATE EstimatingProjectsMaterials SET MaterialId = {toKeep.Id} WHERE MaterialId = {m.Id}");

                                    Log($"Moving contractor inventory materials from {m.Id} to {toKeep.Id}");

                                    cmd.ExecuteNonQuery(
                                        $"UPDATE ContractorInventoriesMaterials SET MaterialId = {toKeep.Id} WHERE MaterialId = {m.Id}");

                                    Log($"Deleting {m}");

                                    cmd.ExecuteNonQuery($"DELETE FROM Materials WHERE MaterialId = {m.Id}");
                                });
                                continue;
                            }

                            Log($"PartNumber {partNumber}");

                            materials.ForEach(m => { Log($"    {m}"); });
                        }
                    }
                });
        }

        private void Log(string s)
        {
            Console.WriteLine(s);
        }

        public override void Down()
        {
            Delete.Column("UnitOfMeasure").FromTable(nameof(Material) + "s");
        }

        private class Material
        {
            public int Id { get; set; }
            public string PartNumber { get; set; }
            public string Description { get; set; }
            public bool IsActive { get; set; }

            public static Material FromReader(IDataReader rdr)
            {
                return new Material {
                    Id = rdr.GetInt32(0),
                    PartNumber = rdr.GetString(1),
                    Description = rdr.GetString(2),
                    IsActive = rdr.GetBoolean(3)
                };
            }

            public override string ToString()
            {
                return $"{Id.ToString().PadLeft(5, ' ')} - {PartNumber} - {Description} - {IsActive}";
            }
        }
    }
}
