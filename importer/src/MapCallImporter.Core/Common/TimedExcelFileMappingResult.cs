using System;
using System.Collections;
using System.Collections.Generic;

namespace MapCallImporter.Common
{
    public class TimedExcelFileMappingResult : ExcelFileValidationAndMappingResult
    {
        #region Properties

        public double ElapsedMiliseconds { get; }

        #endregion

        #region Constructors

        public TimedExcelFileMappingResult(ExcelFileProcessingResult result, Type recordType, double elapsedMiliseconds, IEnumerable<string> issues = null, IEnumerable entities = null) : base(result, recordType, issues, entities)
        {
            ElapsedMiliseconds = elapsedMiliseconds;
        }

        public TimedExcelFileMappingResult(ExcelFileValidationAndMappingResult result, double elapsedMiliseconds) : this(result.Result, result.RecordType, elapsedMiliseconds, result.Issues, result.Entities) {}

        #endregion
    }
}