using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public abstract class MeterChangeOutContractViewModel : ViewModel<MeterChangeOutContract>
    {
        #region Properties

        [DropDown, Required, EntityMap, EntityMustExist(typeof(Contractor))]
        public int? Contractor { get; set; }
        
        [Required]
        public bool? IsActive { get; set; }

        #endregion

        #region Constructor

        protected MeterChangeOutContractViewModel(IContainer container) : base(container) {}

        #endregion
    }

    public class EditMeterChangeOutContract : MeterChangeOutContractViewModel
    {
        #region Properties

        [Required, StringLength(MeterChangeOutContract.StringLengths.DESCRIPTION)]
        public string Description { get; set; }

        #endregion

        public EditMeterChangeOutContract(IContainer container) : base(container) {}
    }

    public class CreateMeterChangeOutContract : MeterChangeOutContractViewModel
    {
        #region Properties

        [DropDown, Required, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        [DisplayName("Select File"), DoesNotAutoMap]
        [Required(ErrorMessage = "The FileUpload field is required."), FileUpload(FileTypes.Xlsx)]
        public AjaxFileUpload FileUpload { get; set; }

        // Internal set is for test use only.
        [DoesNotAutoMap]
        public List<MeterChangeOutContractExcelRecord> ExcelRecords { get; internal set; }

        /// <summary>
        /// This property exists solely to bypass the validation test in the controller test.
        /// </summary>
        [DoesNotAutoMap]
        public bool SkipValidation { get; set; }

        #endregion

        #region Constructors

        public CreateMeterChangeOutContract(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override void SetDefaults()
        {
            base.SetDefaults();
            // Status is a required field, making life easier for users by setting a default value here.
            IsActive = true;
        }

        public override MeterChangeOutContract MapToEntity(MeterChangeOutContract entity)
        {
            base.MapToEntity(entity);
            entity.Description = FileUpload.FileName;

            MeterChangeOutExcelHelper.ImportExcelRecordsToContracT(_container, entity, ExcelRecords);

            return entity;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (SkipValidation || FileUpload == null || !FileUpload.HasBinaryData)
            {
                // Handled by required attribute.
                yield break;
            }

            // This is here so unit tests can set the ExcelEmployees property rather than
            // having to embed a bunch of different excel files for differen tests.
            if (ExcelRecords == null)
            {
                using (var importer = _container.With(FileUpload.BinaryData).GetInstance<ExcelImport<MeterChangeOutContractExcelRecord>>())
                {
                    ExcelRecords = importer.GetItems().ToList();
                }
            }

            if (!ExcelRecords.Any())
            {
                yield return
                    new ValidationResult("Uploaded file does not contain any data.", new[] { "FileUpload.Key" });
            }

            // Validate that all the towns in the excel file belong to the selected operating center.

            var opc = _container.GetInstance<IOperatingCenterRepository>().Find(OperatingCenter.Value);
            var excelTowns = ExcelRecords.Select(x => x.ServiceCity).Distinct();
            var opcTowns = opc.Towns.Select(x => x.ShortName);
            var unmatchedExcelTowns = excelTowns.Except(opcTowns,StringComparer.InvariantCultureIgnoreCase).ToList();

            if (unmatchedExcelTowns.Any())
            {
                var failTowns = string.Join(", ", unmatchedExcelTowns);
                yield return new ValidationResult("The imported file includes towns that do not belong to the selected operating center: " + failTowns);
            }
        }

        #endregion
    }

    public class AddMeterChangeOutsToContract : ViewModel<MeterChangeOutContract>
    {
        #region Properties

        [DisplayName("Select File"), DoesNotAutoMap]
        [Required(ErrorMessage = "The FileUpload field is required."), FileUpload(FileTypes.Xlsx)]
        public AjaxFileUpload FileUpload { get; set; }

        // Internal set is for test use only.
        [DoesNotAutoMap]
        public List<MeterChangeOutContractExcelRecord> ExcelRecords { get; internal set; }

        #endregion

        #region Constructors

        public AddMeterChangeOutsToContract(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override MeterChangeOutContract MapToEntity(MeterChangeOutContract entity)
        {
            // No base call for this.
            MeterChangeOutExcelHelper.ImportExcelRecordsToContracT(_container, entity, ExcelRecords);
            return entity;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FileUpload == null || !FileUpload.HasBinaryData)
            {
                // Handled by required attribute.
                yield break;
            }

            // This is here so unit tests can set the ExcelEmployees property rather than
            // having to embed a bunch of different excel files for differen tests.
            if (ExcelRecords == null)
            {
                using (var importer = _container.With(FileUpload.BinaryData).GetInstance<ExcelImport<MeterChangeOutContractExcelRecord>>())
                {
                    ExcelRecords = importer.GetItems().ToList();
                }
            }

            if (!ExcelRecords.Any())
            {
                yield return
                    new ValidationResult("Uploaded file does not contain any data.", new[] { "FileUpload.Key" });
            }

            // Validate that all the towns in the excel file belong to the existing contract's operating center.

            var contract = _container.GetInstance<IRepository<MeterChangeOutContract>>().Find(Id);

            var opc = contract.OperatingCenter;
            var excelTowns = ExcelRecords.Select(x => x.ServiceCity).Distinct();
            var opcTowns = opc.Towns.Select(x => x.ShortName);
            var unmatchedExcelTowns = excelTowns.Except(opcTowns, StringComparer.InvariantCultureIgnoreCase).ToList();

            if (unmatchedExcelTowns.Any())
            {
                var failTowns = string.Join(", ", unmatchedExcelTowns);
                yield return new ValidationResult("The imported file includes towns that do not belong to the selected operating center: " + failTowns);
            }
        }
        #endregion
    }

    internal static class MeterChangeOutExcelHelper
    {
        private static Town GetTownFromOperatingCenter(string town, OperatingCenter opc)
        {
            town = town?.Trim();
            return opc.Towns.Where(x => x.ShortName.Equals(town, System.StringComparison.InvariantCultureIgnoreCase)).Single();
        }

        private static ServiceSize GetMeterSize(string meterSize, Dictionary<string, ServiceSize> sizes)
        {
            meterSize = meterSize?.Trim();
            if (sizes.ContainsKey(meterSize))
            {
                return sizes[meterSize];
            }

            var parsedMeterSize = decimal.Parse(meterSize);

            return sizes.Values.Single(x => x.Size == parsedMeterSize);
        }


        public static void ImportExcelRecordsToContracT(IContainer container, MeterChangeOutContract entity,
            IEnumerable<MeterChangeOutContractExcelRecord> excelRecords)
        {
            // caching this to reduce querying
            var meterSizes = container.GetInstance<IRepository<ServiceSize>>().GetAll().Where(x => x.Meter).ToDictionary(x => x.Description, x => x);

            // Add new entities to a new list. If there's an error in the middle of creating this, we don't want
            // the session flushing to save half of them and not the others.
            var newRecords = new List<MeterChangeOut>();

            foreach (var item in excelRecords)
            {
                var mco = new MeterChangeOut();
                mco.Contract = entity;

                // NOTE: ALL strings must be trimmed because knowing NJAW we'll get data with wacky incosistent spacing at either end.
                mco.AccountNumber = item.AccountNumber?.Trim();
                mco.CustomerName = item.AccountName?.Trim();
                mco.EquipmentID = item.EquipmentId?.Trim();
                mco.MeterReadCode1 = item.MeterReadCode1?.Trim();
                mco.MeterReadCode2 = item.MeterReadCode2?.Trim();
                mco.MeterReadCode3 = item.MeterReadCode3?.Trim();
                mco.MeterReadCode4 = item.MeterReadCode4?.Trim();
                mco.MeterReadComment1 = item.MeterReadComment1?.Trim();
                mco.MeterReadComment2 = item.MeterReadComment2?.Trim();
                mco.MeterReadComment3 = item.MeterReadComment3?.Trim();
                mco.MeterReadComment4 = item.MeterReadComment4?.Trim();
                mco.MeterCommentsPremise = item.MeterReadCommentsPremise?.Trim();
                mco.MeterSize = GetMeterSize(item.MeterSize, meterSizes);
                mco.PremiseNumber = item.PremiseNumber?.Trim();
                mco.ProjectYear = item.PrjYr_Job?.Trim();
                mco.RemovedSerialNumber = item.SerialNumber?.Trim();
                mco.NumberOfDials = item.NumberOfDials;
                mco.SAPOrderNumber = item.Order?.Trim();
                mco.ServiceCity = GetTownFromOperatingCenter(item.ServiceCity, entity.OperatingCenter);
                mco.ServicePhone = item.Tel1?.Trim();
                mco.ServiceStreetNumber = item.StreetNumber?.Trim();
                mco.ServiceStreet = item.StreetAddress?.Trim();
                mco.ServiceZip = item.ServiceZip?.Trim();
                mco.StartRead = item.StartRead;
                mco.YearInstalled = item.MeterInsYear;
                mco.RouteNumber = item.RouteNumber;
                newRecords.Add(mco);
            }

            entity.MeterChangeOuts.AddRange(newRecords);
        }
    }

    public class MeterChangeOutContractExcelRecord
    {
        // NOTE: Properties must match those of the column headers in the Excel file.
        //       But without the punctuation and spaces.
        // DOUBLE NOTE: The excel file used to create this had lots of columns with no values,
        //              so none of those columns exist here.

        #region Properties

        // TODO: Figure out if validation can work on this somehow.

        public string AccountName { get; set; }
        public string ServiceCity { get; set; }
        public string ServiceZip { get; set; }
        public string StreetNumber { get; set; }
        public string StreetAddress { get; set; }
        public string Tel1 { get; set; }
        public string MeterSize { get; set; }
        public string SerialNumber { get; set; }
        public string AccountNumber { get; set; }
        public string EquipmentId { get; set; }
        public string MeterReadCode1 { get; set; }
        public string MeterReadComment1 { get; set; }
        public string MeterReadCode2 { get; set; }
        public string MeterReadComment2 { get; set; }
        public string MeterReadCode3 { get; set; }
        public string MeterReadComment3 { get; set; }
        public string MeterReadCode4 { get; set; }
        public string MeterReadComment4 { get; set; }
        public string MeterReadCommentsPremise { get; set; }
        public string PremiseNumber { get; set; }
        public int? StartRead { get; set; }
        public string OpCntr { get; set; }
        public int? MeterInsYear { get; set; }

        public string RouteNumber { get; set; }
        public string Order { get; set; }
        public int? NumberOfDials { get; set; }
        public string PrjYr_Job { get; set; }

        // I think these two can be ignored, we're not splitting them up in the data and ServiceAddress
        // is a combinatino of these two
        //          public string StreetNumber { get; set; }
        //            public string StreetAddress { get; set; }


        #endregion
    }

    public class SearchMeterChangeOutContract : SearchSet<MeterChangeOutContract>
    {
        #region Properties

        [DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(Contractor))]
        public int? Contractor { get; set; }

        public DateRange CreatedAt { get; set; }

        public bool? IsActive { get; set; }

        #endregion
    }
}