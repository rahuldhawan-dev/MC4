using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using MapCall.Common.Model.ViewModels;

namespace MapCallScheduler.JobHelpers.NonRevenueWater
{
    public class NonRevenueWaterEntryFileSerializer : INonRevenueWaterEntryFileSerializer
    {
        #region Exposed Methods

        public string Serialize(IQueryable<NonRevenueWaterEntryFileDumpViewModel> viewModels)
        {
            var csv = new StringBuilder();

            using (var textWriter = new StringWriter(csv))
            {
                using (var csvWriter = new CsvWriter(textWriter, CultureInfo.CurrentCulture))
                {
                    foreach (var viewModel in viewModels)
                    {
                        csvWriter.WriteField(viewModel.Year);
                        csvWriter.WriteField(viewModel.Month);
                        csvWriter.WriteField(viewModel.BusinessUnit);
                        csvWriter.WriteField(viewModel.OperatingCenter);
                        csvWriter.WriteField(viewModel.Value);
                        csvWriter.NextRecord();
                    }
                }
            }

            return csv.ToString();
        }

        #endregion
    }
}
