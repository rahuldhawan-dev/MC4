using MapCallImporter.Common;
using StructureMap;
using System;

namespace MapCallImporter.Validation
{
    /// <summary>
    /// Service which enumerates ExcelFileValidatorBase implementations and returns an instance
    /// of the correct validator based on the column headers in the file.
    /// </summary>
    public class ExcelFileValidatorFinderService : ExcelFileHandlerFinderServiceBase
    {
        #region Constants

        public const string VALIDATOR_NAMESPACE = "MapCallImporter.Validation.Validators";

        #endregion

        #region Properties

        public override Type HandlerType => typeof(ExcelFileValidator<,,>);

        #endregion

        #region Constructors

        public ExcelFileValidatorFinderService(IContainer container) : base(container) { }

        #endregion
    }
}