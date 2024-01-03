using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Mappings;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class BacterialWaterSample : IEntity, IThingWithNotes, IThingWithDocuments, IThingWithCoordinate
    {
        #region Constants

        public struct StringLengths
        {
            #region Constants

            public const int COLLECTOR = 50,
                             LOCATION = 50,
                             SAMPLE_NUMBER = 25,
                             ADDRESS = 255,
                             SAP_WORK_ORDER_ID = 50;

            #endregion
        }

        public struct Validation
        {
            // NOTE: These are all strings due to the Range attribute requiring strings when
            // dealing with typeof(decimal)
            public const string MIN_CL2FREE = "0.000",
                                MAX_CL2FREE = "99.999",
                                MIN_CL2TOTAL = "0.000",
                                MAX_CL2TOTAL = "99.999",
                                MIN_FLUSH_TIME_MINUTES = "0.00",
                                MAX_FLUSH_TIME_MINUTES = "9999.99",
                                MIN_NITRATE = "0.001",
                                MAX_NITRATE = "999999999999999.999",
                                MIN_NITRITE = "0.001",
                                MAX_NITRITE = "999999999999999.999",
                                MIN_MONOCHLORAMINE = "0.001",
                                MAX_MONOCHLORAMINE = "99.999",
                                MIN_FREE_AMMONIA = "0.001",
                                MAX_FREE_AMONIA = "99.999",
                                MIN_PH = "0.001",
                                MAX_PH = "99.999",
                                MIN_TEMPERATURE = "0.1",
                                MAX_TEMPERATURE = "9999.9",
                                MIN_IRON = "0.001",
                                MAX_IRON = "999.999",
                                MIN_MANGANESE = "0.001",
                                MAX_MANGANESE = "999.999",
                                MIN_TURBIDITY = "0.001",
                                MAX_TURBIDITY = "99.999",
                                MIN_ORTHOPHOSPHATEASP = "0.001",
                                MAX_ORTHOPHOSPHATEASP = "999.999",
                                MIN_ORTHOPHOSPHATEASPO4 = "0.001",
                                MAX_ORTHOPHOSPHATEASPO4 = "999.999",
                                MIN_CONDUCTIVITY = "0.001",
                                MAX_CONDUCTIVITY = "9999.99",
                                MIN_ALKALINITY = "0.001",
                                MAX_ALKALINITY = "99.999";
        }

        #endregion

        #region Properties

        // BEGIN HUGE NOTE
        //
        // The properties are in the same order as they are on the Show view for these records. This is so they
        // export to excel in the same order. 
        //
        // END HUGE NOTE

        #region Section 1: Collection Information

        public virtual int Id { get; set; }
        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual SampleSite SampleSite { get; set; }

        [View(MMSINC.Utilities.FormatStyle.DateTime24WithoutSeconds)]
        public virtual DateTime? SampleCollectionDTM { get; set; }

        public virtual User CollectedBy { get; set; }
        public virtual BacterialSampleType BacterialSampleType { get; set; }
        public virtual BacterialWaterSampleRepeatLocationType RepeatLocationType { get; set; }

        [View("Original Bacterial Water Sample ID")]
        public virtual BacterialWaterSample OriginalBacterialWaterSample { get; set; }

        [View("Capital Main Project")]
        public virtual EstimatingProject EstimatingProject { get; set; }

        public virtual string SAPWorkOrderId { get; set; }
        public virtual bool ComplianceSample { get; set; }
        public virtual string Address { get; set; }
        public virtual Town Town { get; set; }
        public virtual string Location { get; set; }

        [View("DTM Date Entered")]
        public virtual DateTime? DataEntered { get; set; }

        #endregion

        #region Section 2: Field Values

        [View("pH")]
        public virtual decimal? Ph { get; set; }

        [View("Temperature (C)")]
        public virtual decimal? Temperature { get; set; }

        [View("Cl2 Free")]
        public virtual decimal? Cl2Free { get; set; }

        [View("Cl2 Total")]
        public virtual decimal? Cl2Total { get; set; }

        [View("Flush Time (min)")]
        public virtual decimal? FlushTimeMinutes { get; set; }

        public virtual decimal? Monochloramine { get; set; }
        public virtual decimal? FreeAmmonia { get; set; }
        public virtual decimal? Nitrite { get; set; }
        public virtual decimal? Nitrate { get; set; }
        public virtual decimal? Alkalinity { get; set; }
        public virtual decimal? Iron { get; set; }
        public virtual decimal? Manganese { get; set; }
        public virtual decimal? Turbidity { get; set; }

        public virtual decimal? Conductivity { get; set; }

        [View("Orthophosphate - as P")]
        public virtual decimal? OrthophosphateAsP { get; set; }

        [View("Orthophosphate - as PO4")]
        public virtual decimal? OrthophosphateAsPO4 { get; set; }

        #endregion

        #region Section 3: Bacterial Results

        public virtual string SampleNumber { get; set; }
        public virtual DateTime? ReceivedByLabDTM { get; set; }
        public virtual bool IsInvalid { get; set; }
        public virtual BacterialWaterSampleReasonForInvalidation ReasonForInvalidation { get; set; }

        [BoolFormat("Present", "Absent")] // NOTE: This format is manually added in the BactiInputTrigger.cshtml too.
        public virtual bool ColiformConfirm { get; set; }

        public virtual BacterialWaterSampleConfirmMethod ColiformConfirmMethod { get; set; }
        public virtual int? NonSheenColonyCount { get; set; }
        public virtual NonSheenColonyCountOperator NonSheenColonyCountOperator { get; set; }
        public virtual int? SheenColonyCount { get; set; }
        public virtual SheenColonyCountOperator SheenColonyCountOperator { get; set; }

        [BoolFormat("Present", "Absent")]
        public virtual bool? EColiConfirm { get; set; }

        public virtual BacterialWaterSampleConfirmMethod EColiConfirmMethod { get; set; }

        public virtual BacterialWaterSampleAnalyst ColiformSetupAnalyst { get; set; }

        [View(MMSINC.Utilities.FormatStyle.DateTime24WithoutSeconds)]
        public virtual DateTime? ColiformSetupDTM { get; set; }

        public virtual BacterialWaterSampleAnalyst ColiformReadAnalyst { get; set; }

        [View(MMSINC.Utilities.FormatStyle.DateTime24WithoutSeconds)]
        public virtual DateTime? ColiformReadDTM { get; set; }

        /// <summary>
        /// HPC is short for Heterotrophic Plate Count in case you were wondering.
        /// </summary>
        public virtual decimal? FinalHPC { get; set; }

        public virtual BacterialWaterSampleConfirmMethod HPCConfirmMethod { get; set; }
        public virtual bool IsSpreader { get; set; }

        public virtual BacterialWaterSampleAnalyst HPCSetupAnalyst { get; set; }

        [View(MMSINC.Utilities.FormatStyle.DateTime24WithoutSeconds)]
        public virtual DateTime? HPCSetupDTM { get; set; }

        public virtual BacterialWaterSampleAnalyst HPCReadAnalyst { get; set; }

        [View(MMSINC.Utilities.FormatStyle.DateTime24WithoutSeconds)]
        public virtual DateTime? HPCReadDTM { get; set; }

        #endregion

        #region LIMS

        public virtual LIMSStatus LIMSStatus { get; set; }

        /// <summary>
        /// Gets the raw JSON response returned from the LIMS API when this has been submitted.
        /// This is for debugging purposes and can probably go away after awhile.
        /// </summary>
        [DoesNotExport, Multiline]
        public virtual string LIMSResponse { get; set; }

        [DoesNotExport]
        public virtual DateTime? SubmittedToLIMSAt { get; set; }

        #endregion

        public virtual string Collector { get; set; }

        [View("Town")]
        [DoesNotExport]
        public virtual Town SampleTown { get; set; }

        [DoesNotExport]
        public virtual MapIcon Icon => Coordinate != null ? Coordinate.Icon : null;

        [DoesNotExport]
        public virtual Coordinate Coordinate
        {
            get
            {
                if (SampleSite != null && SampleSite.Coordinate != null)
                    return SampleSite.Coordinate;
                if (OriginalBacterialWaterSample != null)
                    return OriginalBacterialWaterSample.Coordinate;

                return SampleCoordinate;
            }
            set { }
        }

        [DoesNotExport]
        public virtual Coordinate SampleCoordinate { get; set; }

        public virtual IList<BacterialWaterSample> LinkedBacterialWaterSamples { get; set; }

        [DoesNotExport]
        public virtual string TableName => nameof(BacterialWaterSample) + "s";

        public virtual decimal? Latitude => Coordinate?.Latitude;
        public virtual decimal? Longitude => Coordinate?.Longitude;

        #region Documents

        public virtual IList<Document<BacterialWaterSample>> Documents { get; set; }

        public virtual IList<IDocumentLink> LinkedDocuments => Documents.Cast<IDocumentLink>().ToList();

        #endregion

        #region Notes

        public virtual IList<Note<BacterialWaterSample>> Notes { get; set; }

        public virtual IList<INoteLink> LinkedNotes => Notes.Cast<INoteLink>().ToList();

        #endregion

        #region Formula Properties

        // What are these actually used for? -Ross 10/27/2017
        public virtual int Month { get; set; }
        public virtual int Year { get; set; }

        #endregion

        #endregion

        #region Constructors

        public BacterialWaterSample()
        {
            Documents = new List<Document<BacterialWaterSample>>();
            Notes = new List<Note<BacterialWaterSample>>();
            LinkedBacterialWaterSamples = new List<BacterialWaterSample>();
        }

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return Id.ToString();
        }

        public virtual IEnumerable<ValidationResult> ValidateForLIMS()
        {
            // NOTE: Ensure that this validation does not throw exceptions. The validation
            // results are used to display purposes and we want to display as many of them as possible.

            if (ReceivedByLabDTM == null)
            {
                yield return new ValidationResult($"BacterialWaterSample#{Id} must have a ReceivedByLabDTM value.");
            }

            if (ColiformConfirmMethod == null)
            {
                yield return
                    new ValidationResult($"BacterialWaterSample#{Id} must have a ColiformConfirmMethod value.");
            }

            if (ColiformSetupDTM == null)
            {
                yield return new ValidationResult($"BacterialWaterSample#{Id} must have a ColiformSetupDTM value.");
            }

            if (SampleCollectionDTM == null)
            {
                yield return new ValidationResult($"BacterialWaterSample#{Id} must have a SampleCollectionDTM value.");
            }

            if (CollectedBy == null)
            {
                yield return new ValidationResult($"BacterialWaterSample#{Id} must have a CollectedBy value.");
            }

            if (SampleSite == null)
            {
                yield return new ValidationResult($"BacterialWaterSample#{Id} must have a SampleSite value.");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(SampleSite.CommonSiteName))
                {
                    yield return new ValidationResult($"BacterialWaterSample#{Id} has a SampleSite#{SampleSite.Id} without a CommonSiteName.");
                }
                if (SampleSite.PublicWaterSupply == null)
                {
                    yield return new ValidationResult(
                        $"BacterialWaterSample#{Id} has a SampleSite#{SampleSite.Id} without a PublicWaterSupply.");
                }
                else
                {
                    if (SampleSite.PublicWaterSupply.LocalCertifiedStateId == null)
                    {
                        yield return new ValidationResult(
                            $"BacterialWaterSample#{Id} has a SampleSite#{SampleSite.Id} with a PublicWaterSupply#{SampleSite.PublicWaterSupply.Id} without a LocalCertifiedStateId.");
                    }

                    if (SampleSite.PublicWaterSupply.LIMSProfileNumber == null)
                    {
                        yield return new ValidationResult(
                            $"BacterialWaterSample#{Id} has a SampleSite#{SampleSite.Id} with a PublicWaterSupply#{SampleSite.PublicWaterSupply.Id} without a LIMSProfileNumber.");
                    }
                }
            }
        }

        #endregion
    }

    [Serializable]
    public class BacterialSampleType : ReadOnlyEntityLookup
    {
        #region Constants

        public struct Indices
        {
            #region Constants

            public const int ROUTINE = 1,
                             PROCESS_CONTROL = 2,
                             NEW_MAIN = 3,
                             REPEAT = 4,
                             SYSTEM_REPAIR = 5,
                             CONFIRMATION = 19,
                             SPECIAL = 20,
                             DUPLICATE = 21,
                             SPLIT = 22,
                             SHIPPING_BLANK = 23,
                             FIELD_BLANK = 24,
                             BATCH_BLANK = 25,
                             SPLIT_BLANK = 26,
                             PERFORMANCE_EVALUATION = 27,
                             MAX_RESIDENCE_TIME = 28;

            #endregion
        }

        #endregion
    }

    [Serializable]
    public class NonSheenColonyCountOperator : EntityLookup { }

    [Serializable]
    public class SheenColonyCountOperator : EntityLookup { }

    [Serializable]
    public class BacterialWaterSampleRepeatLocationType : ReadOnlyEntityLookup { }

    [Serializable]
    public class BacterialWaterSampleConfirmMethod : EntityLookup { }

    #region Report View Models

    #region Bacti Sampling Requirements

    public class BacterialWaterSampleRequirementViewModel
    {
        #region Properties

        public virtual PublicWaterSupply PublicWaterSupply { get; set; }
        public virtual BacterialSampleType BacterialSampleType { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int Total { get; set; }

        #endregion
    }

    public class BacterialWaterSampleRequirementReportViewModel : MonthlyReportViewModel
    {
        #region Properties

        public virtual PublicWaterSupply PublicWaterSupply { get; set; }
        public virtual string BacterialSampleType { get; set; }

        #endregion
    }

    public interface
        ISearchBacterialWaterSampleRequirementViewModel : ISearchSet<BacterialWaterSampleRequirementViewModel>
    {
        #region Abstract Properties

        bool? OnlyWithSamples { get; set; }
        int[] Year { get; set; }
        int[] BacterialSampleType { get; set; }

        #endregion
    }

    #endregion

    #region Bacterial Water Sample Chlorine High/Low

    public class BactiSamplesChlorineHighLowViewModel // : ViewModel<BactiSample>
    {
        #region Properties

        public virtual string PublicWaterSupply { get; set; }

        public virtual string Town { get; set; }

        // Sample Date Month
        public int Month { get; set; }

        // Sample Date Year
        public int Year { get; set; }

        public decimal Cl2FreeMax { get; set; }
        public decimal Cl2FreeMin { get; set; }
        public decimal Cl2TotalMax { get; set; }
        public decimal Cl2TotalMin { get; set; }

        #endregion
    }

    public class BactiSamplesChlorineHighLowReportViewModel
    {
        #region Properties

        [DisplayName("PWSID")]
        public virtual string PublicWaterSupply { get; set; }

        public virtual string OperatingCenter { get; set; }
        public virtual string Town { get; set; }
        public virtual string Type { get; set; }
        public virtual int Year { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public virtual decimal? Jan { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public virtual decimal? Feb { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public virtual decimal? Mar { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public virtual decimal? Apr { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public virtual decimal? May { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public virtual decimal? Jun { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public virtual decimal? Jul { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public virtual decimal? Aug { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public virtual decimal? Sep { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public virtual decimal? Oct { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public virtual decimal? Nov { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public virtual decimal? Dec { get; set; }

        #endregion
    }

    #endregion

    #endregion
}
