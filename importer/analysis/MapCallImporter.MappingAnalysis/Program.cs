using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Glob;
using Newtonsoft.Json;

namespace MapCallImporter.MappingAnalysis
{
    class Program
    {
        #region Private Methods

        static void Main(string[] args)
        {
            var thisPath = Path.GetFullPath(".");
            var solutionPath = Path.GetFullPath(Path.Combine("..", "..", "..", "..", ".."));
            var basePath = Path.Combine(solutionPath, "tests", "MapCallImporter.Tests");
            var modelsPath = Path.Combine(basePath, "Tests", "Models");
            var equipmentBaseMappings = new ExcelRecordTestParser().Parse(new FileInfo(Path.Combine(basePath, "Library", "Testing", "EquipmentExcelRecordTestBase.cs")));
            var mappings = new List<object>();

            foreach (var mapping in new DirectoryInfo(Path.Combine(modelsPath, "Import", "Equipment"))
                .GlobFiles("*ExcelRecordTest.cs", SearchOption.TopDirectoryOnly)
                .Select(file => new ExcelRecordTestParser().Parse(file)).OrderBy(m => m.ExcelModelName))
            {
                mappings.Add(mapping);
            }

            foreach (var mapping in new DirectoryInfo(Path.GetFullPath(Path.Combine(modelsPath, "Import", "Equipment")))
                .GlobFiles("*ExcelRecordTest.cs").Select(file =>
                    new ExcelRecordTestParser().ParseEquipment(equipmentBaseMappings, file)).OrderBy(m => m.ExcelModelName))
            {
                mappings.Add(mapping);
            }

            var output = JsonConvert.SerializeObject(new {
                Mappings = mappings
            }, Formatting.Indented);
            File.WriteAllText(Path.Combine(solutionPath, "release", "mappings.json"), output);

            Console.Write(output);
        }

        #endregion
    }
}
