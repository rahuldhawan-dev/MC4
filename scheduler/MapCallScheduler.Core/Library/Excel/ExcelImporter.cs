using System;
using System.Collections.Generic;
using System.IO;
using MMSINC.Utilities;
using OfficeOpenXml;

namespace MapCallScheduler.Library.Excel
{
    public abstract class ExcelImporterBase<TOutput, TParser> : IExcelImporter<TOutput>
        where TParser : IExcelParser<TOutput>
    {
        #region Private Members

        protected readonly TParser _parser;

        #endregion

        #region Constructors

        protected ExcelImporterBase(TParser parser)
        {
            _parser = parser;
        }

        #endregion

        #region Private Methods

        protected IEnumerable<TOutput> ParsePackage(FileInfo excelFile, Func<ExcelPackage, ExcelWorksheet> getWorksheetFn)
        {
            return _parser.Parse(getWorksheetFn(new ExcelPackage(excelFile)));
        }

        #endregion

        #region Exposed Methods

        public IEnumerable<TOutput> Import(FileInfo excelFile)
        {
            return Import(excelFile, 1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="excelFile"></param>
        /// <param name="sheetIndex">1-based index of the worksheet to load.</param>
        /// <returns></returns>
        public IEnumerable<TOutput> Import(FileInfo excelFile, int sheetIndex)
        {
            return ParsePackage(excelFile, p => p.Workbook.Worksheets[sheetIndex]);
        }

        public IEnumerable<TOutput> Import(FileInfo excelFile, string sheetName)
        {
            return ParsePackage(excelFile, p => p.Workbook.Worksheets[sheetName]);
        }

        #endregion
    }

    public class ExcelImporter<TOutput> : ExcelImporterBase<TOutput, ByColumnIndexExcelParser<TOutput>>
    {
        #region Constructors

        public ExcelImporter(ByColumnIndexExcelParser<TOutput> parser) : base(parser) {}

        #endregion
    }

    public interface IExcelImporter<out TOutput>
    {
        #region Abstract Methods

        IEnumerable<TOutput> Import(FileInfo excelFile);
        IEnumerable<TOutput> Import(FileInfo excelFile, int sheetIndex);
        IEnumerable<TOutput> Import(FileInfo excelFile, string sheetName);

        #endregion
    }

    public static class ExcelImporterExtensions
    {
        #region Exposed Methods

        public static IEnumerable<TOutput> Import<TOutput>(this IExcelImporter<TOutput> that, Stream excelStream)
        {
            return TemporaryFile.WithTemporaryFile(excelStream, f => that.Import(f.FileInfo));
        }

        public static IEnumerable<TOutput> Import<TOutput>(this IExcelImporter<TOutput> that, Stream excelStream, int sheetIndex)
        {
            return TemporaryFile.WithTemporaryFile(excelStream, f => that.Import(f.FileInfo, sheetIndex));
        }

        public static IEnumerable<TOutput> Import<TOutput>(this IExcelImporter<TOutput> that, Stream excelStream, string sheetName)
        {
            return TemporaryFile.WithTemporaryFile(excelStream, f => that.Import(f.FileInfo, sheetName));
        }

        #endregion
    }
}
