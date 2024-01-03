using System;
using System.Collections;
using System.Collections.Generic;

namespace MapCallImporter.Common
{
    public class ExcelFileValidationAndMappingResult
    {
        #region Properties

        public ExcelFileProcessingResult Result { get; }
        public IEnumerable<string> Issues { get; }
        public IEnumerable Entities { get; }
        public Type RecordType { get; }

        #endregion

        #region Constructors

        public ExcelFileValidationAndMappingResult(ExcelFileProcessingResult result, Type recordType, IEnumerable<string> issues = null, IEnumerable entities = null)
        {
            Result = result;
            Issues = issues ?? new string[0];
            Entities = entities;
            RecordType = recordType;
        }

        #endregion
    }
}