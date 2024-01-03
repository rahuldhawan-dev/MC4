using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration.Attributes;

namespace MapCallScheduler.JobHelpers.W1V
{
    public class W1VFileParser : IW1VFileParser
    {
        public class ParsedCustomerMaterial
        {
            [Name("work_order_number")]
            public int WorkOrderNumber { get; set; }
            
            [Name("premise_id")]
            public string PremiseId { get; set; }
            
            [Name("meter_size")]
            public string MeterSize { get; set; }
            
            [Name("customersidematerial")]
            public string CustomerSideMaterial { get; set; }
            
            [Name("readingdevicepositionallocation")]
            public string ReadingDevicePositionalLocation { get; set; }
            
            [Name("assignment_start")]
            public DateTime? AssignmentStart { get; set; }
            
            [Name("technicalinspectedon")]
            public string TechnicalInspectedOn { get; set; }

            [Name("installation")]
            public string Installation { get; set; }

            [Name("functional_location")]
            public string FunctionalLocation { get; set; }
        }

        private static IEnumerable<TRecord> ParseRecords<TRecord>(string csv)
        {
            using (var reader = new StringReader(csv))
            using (var csvReader = new CsvReader(reader, CultureInfo.CurrentCulture))
            {
                return csvReader.GetRecords<TRecord>().ToList();
            }
        }

        public IEnumerable<ParsedCustomerMaterial> ParseCustomerMaterial(string csv)
        {
            return ParseRecords<ParsedCustomerMaterial>(csv);
        }
    }

    public interface IW1VFileParser
    {
        IEnumerable<W1VFileParser.ParsedCustomerMaterial> ParseCustomerMaterial(string csv);
    }
}
