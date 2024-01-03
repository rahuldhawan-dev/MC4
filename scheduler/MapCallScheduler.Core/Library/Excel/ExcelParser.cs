using System;
using System.Collections.Generic;
using OfficeOpenXml;

namespace MapCallScheduler.Library.Excel
{
    public abstract class ExcelParserBase<TOutput> : IExcelParser<TOutput>
    {
        #region Properties

        public int SkipRows { get; set; }

        #endregion

        #region Constructors

        protected ExcelParserBase(int skipRows)
        {
            SkipRows = skipRows;
        }

        #endregion

        #region Abstract Methods

        public abstract IEnumerable<TOutput> Parse(ExcelWorksheet worksheet);

        #endregion
    }

    public abstract class ByColumnIndexExcelParser<TOutput> : ExcelParserBase<TOutput>
    {
        #region Constants

        public const int DEFAULT_SKIP_ROWS = 1;

        #endregion

        #region Abstract Properties

        protected abstract Action<string, TOutput>[] Setters { get; }

        #endregion

        #region Constructors

        public ByColumnIndexExcelParser() : base(DEFAULT_SKIP_ROWS) {}

        #endregion

        #region Exposed Methods

        public override IEnumerable<TOutput> Parse(ExcelWorksheet worksheet)
        {
            var start = worksheet.Dimension.Start;
            var end = worksheet.Dimension.End;

            for (int row = start.Row + SkipRows; row <= end.Row; ++row)
            {
                var record = Activator.CreateInstance<TOutput>();

                if (RowIsEmpty(worksheet, row, Setters.Length))
                {
                    continue;
                }

                for (int col = start.Column; col <= Setters.Length; ++col)
                {
                    Setters[col - 1](worksheet.Cells[row, col].Text, record);
                }

                yield return record;
            }
        }

        private bool RowIsEmpty(ExcelWorksheet worksheet, int row, int columns)
        {
            for (int col = 1; col <= columns; ++col)
            {
                if (!string.IsNullOrWhiteSpace(worksheet.Cells[row, col].Text))
                {
                    return false;
                }
            }
            return true;
        }

        #endregion
    }

    public interface IExcelParser<out TOutput>
    {
        #region Abstract Properties

        int SkipRows { get; set; }

        #endregion

        #region Abstract Methods

        IEnumerable<TOutput> Parse(ExcelWorksheet worksheet);

        #endregion
    }
}