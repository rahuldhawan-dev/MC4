using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;
using StructureMap;

namespace MMSINC.Data
{
    /// <summary>
    /// A ViewModel/data object that accepts an Excel file upload and parses out the
    /// rows to the specified T type. Validation is ran for each item.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExcelAjaxFileUpload<T> : IValidatableObject
    {
        #region Fields

        private readonly IContainer _container;

        #endregion

        #region Properties

        [Required, FileUpload(FileTypes.Xlsx)]
        public AjaxFileUpload FileUpload { get; set; }

        // This has as public setter for testing, it shouldn't actually be used in practice.
        public List<T> Items { get; set; }

        /// <summary>
        /// This is currently internal because it's needed for unit testing. There
        /// is currently no way to set this value by the time the model binder validates
        /// everything. If this becomes necessary, ask Ross to fix it.
        /// </summary>
        internal string SheetName { get; set; }

        /// <summary>
        /// This is for testing validation of a parent view model when Items are being
        /// injected in the test in place of using an Excel file.
        /// </summary>
        public bool SkipExcelImporting { get; set; }

        #endregion

        #region Constructors

        public ExcelAjaxFileUpload(IContainer container)
        {
            _container = container;
        }

        #endregion

        #region Exposed Methods

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (SkipExcelImporting)
            {
                yield break;
            }

            Items = new List<T>();

            // TODO: Probably need to do secondary validation that BinaryData exists.

            using (var importer = _container.With(FileUpload.BinaryData).GetInstance<ExcelImport<T>>())
            {
                var importResult = importer.TryGetImport(SheetName);

                foreach (var importLevelValidationResult in importResult.ValidationResults)
                {
                    yield return importLevelValidationResult;
                }

                foreach (var item in importResult.Results)
                {
                    Items.Add(item.ConvertedItem);

                    // There may have been conversation errors, if that's the case then we
                    // want to return those errors and stop processing the current row.
                    if (item.ValidationResults.Any())
                    {
                        foreach (var result in item.ValidationResults)
                        {
                            yield return result;
                        }
                    }
                    else
                    {
                        var valResults = new List<ValidationResult>();

                        // The last parameter must be set to true or else it only validates properties
                        // with a RequiredAttribute.
                        var valContext = new ValidationContext(item.ConvertedItem);
                        if (!Validator.TryValidateObject(item.ConvertedItem, valContext, valResults, true))
                        {
                            foreach (var result in valResults)
                            {
                                yield return new ValidationResult($"Row[{item.Row}]: {result.ErrorMessage}",
                                    result.MemberNames);
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}
