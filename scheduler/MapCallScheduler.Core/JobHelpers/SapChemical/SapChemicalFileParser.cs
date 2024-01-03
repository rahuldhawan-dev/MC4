using System;
using System.Globalization;
using System.Text.RegularExpressions;
using CsvHelper.Configuration;
using MapCallScheduler.Library.JobHelpers.Sap;

namespace MapCallScheduler.JobHelpers.SapChemical
{
    public class SapChemicalFileParser : SapDelimitedFileParserBase<SapChemicalFileRecord>, ISapChemicalFileParser
    {
        protected override CsvConfiguration CsvConfiguration
        {
            get
            {
                var config = base.CsvConfiguration;
                config.Delimiter = "\t";
                return config;
            }
        }

        #region Private Methods

        protected override Action<SapChemicalFileRecord, string>[] GetConversionTable()
        {
            return new Action<SapChemicalFileRecord, string>[] {
                (r, s) => r.PartNumber = s,
                (r, s) => r.Plant = s,
                (r, s) => r.Name = s,
                (r, s) => r.UnitOfMeasure = s,
                (r, s) => r.CreatedDate = DateTime.ParseExact(s, "yyyyMMdd", CultureInfo.InvariantCulture),
                (r, s) => r.ChangedDate = IsValidDate(s) ? DateTime.ParseExact(s, "yyyyMMdd", CultureInfo.InvariantCulture) : r.CreatedDate,
                (r, s) => r.Cost = string.IsNullOrWhiteSpace(s) ? (decimal?)null : decimal.Parse(s),
                (r, s) => r.DeletionFlag = s == "X",
            };
        }

        private bool IsValidDate(string s)
        {
            return !string.IsNullOrWhiteSpace(s) && s.Length == 8 && new Regex("^\\d{8}$").IsMatch(s) &&
                   !new Regex("^0\\d{7}$").IsMatch(s);
        }

        #endregion
    }
}
