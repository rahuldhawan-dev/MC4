using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using MapCall.Common.Model.Mappings;
using MapCall.Common.Model.ViewModels;

namespace MapCallScheduler.JobHelpers.SystemDeliveryEntry.DumpTasks
{
    public class SystemDeliveryEntryFileSerializer : ISystemDeliveryEntryFileSerializer
    {
        public string Serialize(IQueryable<SystemDeliveryEntryFileDumpViewModel> coll)
        {
            var csv = new StringBuilder();

            using (var textWriter = new StringWriter(csv))
            {
                using (var csvWriter = new CsvWriter(textWriter, CultureInfo.CurrentCulture))
                {
                    foreach (var item in coll)
                    {
                        csvWriter.WriteField(item.Year);
                        csvWriter.WriteField(item.Month);
                        csvWriter.WriteField(item.BusinessUnit);
                        csvWriter.WriteField(item.FacilityName);
                        csvWriter.WriteField(item.EntryDescription);
                        csvWriter.WriteField(item.AsPostedDescription);
                        csvWriter.WriteField(item.SystemDeliveryDescription);
                        csvWriter.WriteField(item.TotalValue);
                        csvWriter.NextRecord();
                    }
                }
            }

            return csv.ToString();
        }
    }
}
