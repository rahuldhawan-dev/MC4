using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using MapCallScheduler.Library.Common;

namespace MapCallScheduler.Library.JobHelpers.Sap
{
    public abstract class SapDelimitedFileParserBase<TFileData> : IFileParser<TFileData> where TFileData : new()
    {
        protected virtual CsvConfiguration CsvConfiguration => new CsvConfiguration(CultureInfo.CurrentCulture) {
            HasHeaderRecord = true,
            BadDataFound = null
        };

        #region Abstract Methods

        protected abstract Action<TFileData, string>[] GetConversionTable();

        #endregion

        #region Exposed Methods

        public IEnumerable<TFileData> Parse(FileData file)
        {
            var conversionTable = GetConversionTable();

            var reader = new CsvParser(new StringReader(file.Content), CsvConfiguration);
            string[] currentLine;

            if (CsvConfiguration.HasHeaderRecord)
            {
                reader.Read();
            }

            while (reader.Read())
            {
                currentLine = reader.Record;
                var ret = new TFileData();

                for (var i = 0; i < currentLine.Length; ++i)
                {
                    conversionTable[i](ret, (currentLine[i] ?? "").Trim());
                }

                yield return ret;
            }
        }

        #endregion
    }
}
