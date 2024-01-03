using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Mappings;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.Excel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class WaterSampleComplianceForm : IEntity, IThingWithNotes, IThingWithDocuments
    {
        #region Consts

        public struct Display
        {
            public const string CENTRAL_LAB_COLLECTED = "All samples analyzed by the Central Lab have been collected.",
                                CENTRAL_LAB_REPORTED = "All samples analyzed by the Central Lab have been reported.",
                                CONTRACTED_LABS_COLLECTED =
                                    "All samples analyzed by Contracted Labs have been collected.",
                                CONTRACTED_LABS_REPORTED =
                                    "All samples analyzed by Contracted Labs have been reported.",
                                INTERNAL_LABS_COLLECTED = "All samples analyzed by Internal Labs have been collected.",
                                INTERNAL_LABS_REPORTED = "All samples analyzed by Internal Labs have been reported.",
                                BACTI_SAMPLES_COLLECTED = "All Bacti samples have been collected.",
                                BACTI_SAMPLES_REPORTED = "All Bacti samples have been reported.",
                                LEAD_AND_COPPER_COLLECTED = "All Lead and Copper samples have been collected.",
                                LEAD_AND_COPPER_REPORTED = "All Lead and Copper samples have been reported.",
                                WQP_SAMPLES_COLLECTED = "All WQP samples have been collected.",
                                WQP_SAMPLES_REPORTED = "All WQP samples have been reported.",
                                SURFACE_WATER_PLANT_COLLECTED =
                                    "All Surface Water Plant related samples have been collected.",
                                SURFACE_WATER_PLANT_REPORTED =
                                    "All Surface Water Plant related samples have been reported.",
                                CHLORINE_RESIDUALS_COLLECTED =
                                    "All Chlorine Residuals (Surface or Ground water) have been collected.",
                                CHLORINE_RESIDUALS_REPORTED =
                                    "All Chlorine Residuals (Surface or Ground water) have been reported.",
                                NOTE_TEXT = "Notes",
                                REASON_TEXT = "Reason";
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual PublicWaterSupply PublicWaterSupply { get; set; }

        // NOTE: This is being stored, but it is not being displayed anywhere.
        // Nicole says the field doesn't matter too much because multiple people
        // may be answering each question, so the user recorded in CertifiedBy is
        // really only the person who created the record and not necssarily the person
        // who answered/edited the question later on.
        // I think they're going to end up wanting this back because trying to
        // find this info through the audit logs seems like an annoying amount 
        // of work. If you, the reader, come back to this sometime after 1/17/2020
        // and it's still not being used, then feel free to kill this field.
        [DoesNotExport]
        public virtual User CertifiedBy { get; set; }

        #region Certified Date properties

        // The DateCertified and the CertifiedMonth/CertifiedYear are not necessarily
        // in sync. Certification months run from the 10th of the month into the next.
        // ex: January certification is 1/11 through 2/10. Rather than calculate these
        // all the time, we store it once. This also makes life easier if they decide
        // to change the date range for a month. This also makes life easier because
        // they want the CertifiedMonth/Year searchable and we won't need to adjust for
        // dates in formula fields.

        public virtual DateTime DateCertified { get; set; }
        public virtual int CertifiedMonth { get; set; }
        public virtual int CertifiedYear { get; set; }

        [View("Certification for Month/Year")]
        public virtual string CertifiedMonthYearDisplay =>
            WaterSampleComplianceMonthYear.GetFormattedValue(CertifiedMonth, CertifiedYear);

        #endregion

        [Multiline, View(Display.NOTE_TEXT)]
        public virtual string NoteText { get; set; }

        [View(Display.CENTRAL_LAB_COLLECTED)]
        public virtual WaterSampleComplianceFormAnswerType CentralLabSamplesHaveBeenCollected { get; set; }

        [View(Display.CENTRAL_LAB_REPORTED)]
        public virtual WaterSampleComplianceFormAnswerType CentralLabSamplesHaveBeenReported { get; set; }

        [Multiline, View(Display.REASON_TEXT)]
        public virtual string CentralLabSamplesReason { get; set; }

        [View(Display.CONTRACTED_LABS_COLLECTED)]
        public virtual WaterSampleComplianceFormAnswerType ContractedLabsSamplesHaveBeenCollected { get; set; }

        [View(Display.CONTRACTED_LABS_REPORTED)]
        public virtual WaterSampleComplianceFormAnswerType ContractedLabsSamplesHaveBeenReported { get; set; }

        [Multiline, View(Display.REASON_TEXT)]
        public virtual string ContractedLabsSamplesReason { get; set; }

        [View(Display.INTERNAL_LABS_COLLECTED)]
        public virtual WaterSampleComplianceFormAnswerType InternalLabsSamplesHaveBeenCollected { get; set; }

        [View(Display.INTERNAL_LABS_REPORTED)]
        public virtual WaterSampleComplianceFormAnswerType InternalLabsSamplesHaveBeenReported { get; set; }

        [Multiline, View(Display.REASON_TEXT)]
        public virtual string InternalLabSamplesReason { get; set; }

        [View(Display.BACTI_SAMPLES_COLLECTED)]
        public virtual WaterSampleComplianceFormAnswerType BactiSamplesHaveBeenCollected { get; set; }

        [View(Display.BACTI_SAMPLES_REPORTED)]
        public virtual WaterSampleComplianceFormAnswerType BactiSamplesHaveBeenReported { get; set; }

        [Multiline, View(Display.REASON_TEXT)]
        public virtual string BactiSamplesReason { get; set; }

        [View(Display.LEAD_AND_COPPER_COLLECTED)]
        public virtual WaterSampleComplianceFormAnswerType LeadAndCopperSamplesHaveBeenCollected { get; set; }

        [View(Display.LEAD_AND_COPPER_REPORTED)]
        public virtual WaterSampleComplianceFormAnswerType LeadAndCopperSamplesHaveBeenReported { get; set; }

        [Multiline, View(Display.REASON_TEXT)]
        public virtual string LeadAndCopperSamplesReason { get; set; }

        [View(Display.WQP_SAMPLES_COLLECTED)]
        public virtual WaterSampleComplianceFormAnswerType WQPSamplesHaveBeenCollected { get; set; }

        [View(Display.WQP_SAMPLES_REPORTED)]
        public virtual WaterSampleComplianceFormAnswerType WQPSamplesHaveBeenReported { get; set; }

        [Multiline, View(Display.REASON_TEXT)]
        public virtual string WQPSamplesReason { get; set; }

        [View(Display.SURFACE_WATER_PLANT_COLLECTED)]
        public virtual WaterSampleComplianceFormAnswerType SurfaceWaterPlantSamplesHaveBeenCollected { get; set; }

        [View(Display.SURFACE_WATER_PLANT_REPORTED)]
        public virtual WaterSampleComplianceFormAnswerType SurfaceWaterPlantSamplesHaveBeenReported { get; set; }

        [Multiline, View(Display.REASON_TEXT)]
        public virtual string SurfaceWaterPlantSamplesReason { get; set; }

        [View(Display.CHLORINE_RESIDUALS_COLLECTED)]
        public virtual WaterSampleComplianceFormAnswerType ChlorineResidualsHaveBeenCollected { get; set; }

        [View(Display.CHLORINE_RESIDUALS_REPORTED)]
        public virtual WaterSampleComplianceFormAnswerType ChlorineResidualsHaveBeenReported { get; set; }

        [Multiline, View(Display.REASON_TEXT)]
        public virtual string ChlorineResidualsReason { get; set; }

        /// <summary>
        /// This is for the yearly report. See MC-1820 for this logic.
        /// </summary>
        [DoesNotExport]
        public virtual ComplianceResult ComplianceResult
        {
            get
            {
                var reportedAnswers = new[] {
                    CentralLabSamplesHaveBeenCollected,
                    CentralLabSamplesHaveBeenReported,
                    ContractedLabsSamplesHaveBeenCollected,
                    ContractedLabsSamplesHaveBeenReported,
                    InternalLabsSamplesHaveBeenCollected,
                    InternalLabsSamplesHaveBeenReported,
                    BactiSamplesHaveBeenCollected,
                    BactiSamplesHaveBeenReported,
                    LeadAndCopperSamplesHaveBeenCollected,
                    LeadAndCopperSamplesHaveBeenReported,
                    WQPSamplesHaveBeenCollected,
                    WQPSamplesHaveBeenReported,
                    SurfaceWaterPlantSamplesHaveBeenCollected,
                    SurfaceWaterPlantSamplesHaveBeenReported,
                    ChlorineResidualsHaveBeenCollected,
                    ChlorineResidualsHaveBeenReported
                };

                // If any answers aren't set or are set to No then it is not compliant.
                // It's also not compliant if this instance doesn't exist, but you can't really put
                // that in a property now can you.
                if (reportedAnswers.Any(x => x == null))
                {
                    return ComplianceResult.NotCompliant;
                }

                // If all are YES or N/A then it is compliant. Why N/A is complaint, I don't know.
                if (reportedAnswers.All(x => x.Id == WaterSampleComplianceFormAnswerType.Indices.YES ||
                                             x.Id == WaterSampleComplianceFormAnswerType.Indices.NOT_AVAILABLE))
                {
                    return ComplianceResult.EntirelyCompliant;
                }

                // If there's some mix of YES or N/A then it's only kinda compliant.
                return ComplianceResult.PartiallyCompliant;
            }
        }

        #region Notes/Docs/AuditLogs

        [DoesNotExport]
        public virtual string TableName => nameof(WaterSampleComplianceForm) + "s";

        [DoesNotExport]
        public virtual IList<Document<WaterSampleComplianceForm>> Documents { get; set; }

        [DoesNotExport]
        public virtual IList<IDocumentLink> LinkedDocuments => Documents.Cast<IDocumentLink>().ToList();

        [DoesNotExport]
        public virtual IList<Note<WaterSampleComplianceForm>> Notes { get; set; }

        [DoesNotExport]
        public virtual IList<INoteLink> LinkedNotes => Notes.Cast<INoteLink>().ToList();

        #endregion

        #endregion
    }

    public enum ComplianceResult
    {
        NotCompliant,
        PartiallyCompliant,
        EntirelyCompliant
    }

    [Serializable]
    public class WaterSampleComplianceFormAnswerType : ReadOnlyEntityLookup
    {
        #region Structs

        public struct Indices
        {
            public const int NOT_AVAILABLE = 1,
                             YES = 2,
                             NO = 3;
        }

        #endregion
    }

    /// <summary>
    /// Helper class for getting the proper Month and Year values for Water Sample Compliance Forms.
    /// </summary>
    public sealed class WaterSampleComplianceMonthYear
    {
        #region Properties

        public int Month { get; private set; }
        public int Year { get; private set; }

        #endregion

        #region Constructors

        public WaterSampleComplianceMonthYear(DateTime complianceDate)
        {
            // If the current date is past the 10th of the month, then we are certifying
            // for the current month.
            if (complianceDate.Day > 10)
            {
                Month = complianceDate.Month;
                Year = complianceDate.Year;
            }
            else
            {
                // If it's not yet the 10th of the month, then we are certifying
                // for the prior month. 
                if (complianceDate.Month == 1)
                {
                    // If it's January, we need to do December of the prior year.
                    Month = 12;
                    Year = complianceDate.Year - 1;
                }
                else
                {
                    Month = complianceDate.Month - 1;
                    Year = complianceDate.Year;
                }
            }
        }

        #endregion

        #region Public Methods

        public static string GetFormattedValue(int month, int year)
        {
            return $"{month:00}/{year}";
        }

        #endregion
    }
}
