using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCallImporter.Common;
using MapCallImporter.Validation;
using MapCallImporter.ViewModels;
using MMSINC.Data.V2;
using MMSINC.Utilities.ObjectMapping;

namespace MapCallImporter.Models.Import
{
    public class ArcFlashStudyExcelRecord : ExcelRecordBase<ArcFlashStudy, MyCreateArcFlashStudy, ArcFlashStudyExcelRecord>
    {
        #region Properties

        #region Core Fields

        [AutoMap("Facility")]
        public virtual int FacilityId { get; set; }

        [DoesNotAutoMap("FacilityName is for the Business to include in their template, but isn't used for mapping.")]
        public virtual string FacilityName { get; set; }

        public virtual string UtilityCompany { get; set; }

        public virtual string ArcFlashStatus { get; set; }

        public virtual string FacilitySize { get; set; }

        public virtual string TypeOfArcFlashAnalysis { get; set; }

        [AutoMap("ArcFlashLabelType")]
        public virtual string LabelType { get; set; }

        [AutoMap("TransformerKVARating")]
        public virtual string TransformerKVARating { get; set; }

        [AutoMap("Voltage")]
        public virtual string SecondaryVoltage { get; set; }

        public virtual string PowerPhase { get; set; }

        [AutoMap("FacilityTransformerWiringType")]
        public virtual string FacilityTransformerWiringConfiguration { get; set; }

        public virtual string Priority { get; set; }

        [AutoMap("PowerCompanyDataReceived")]
        public virtual bool UtilityCompanyDataReceived { get; set; }

        public virtual DateTime? UtilityCompanyDataReceivedDate { get; set; }

        public virtual bool AFHAAnalysisPerformed { get; set; }

        public virtual string UtilityCompanyOther { get; set; }

        public virtual string UtilityAccountNumber { get; set; }

        public virtual string UtilityMeterNumber { get; set; }

        public virtual string UtilityPoleNumber { get; set; }

        public virtual decimal? PrimaryVoltageKV { get; set; }

        public virtual bool TransformerKVAFieldConfirmed { get; set; }

        public virtual decimal? TransformerResistancePercentage { get; set; }

        [AutoMap("TransformerReactancePercentage")]
        public virtual decimal? TransformerImpedancePercentage { get; set; }

        public virtual decimal? PrimaryFuseSize { get; set; }

        public virtual string PrimaryFuseType { get; set; }

        public virtual string PrimaryFuseManufacturer { get; set; }

        public virtual decimal? LineToLineFaultAmps { get; set; }

        public virtual decimal? LineToLineNeutralFaultAmps { get; set; }

        public virtual string ArcFlashNotes { get; set; }

        [AutoMap("DateLabelsApplied")]
        public virtual DateTime? DateLabelIsApplied { get; set; }

        [AutoMap("ArcFlashContractor")]
        public virtual string ArcFlashSiteDataCollectionParty { get; set; }

        public virtual string ArcFlashHazardAnalysisStudyParty { get; set; }

        public virtual decimal? CostToComplete { get; set; }

        #endregion

        #endregion

        #region Exposed Methods

        protected override MyCreateArcFlashStudy MapExtra(MyCreateArcFlashStudy viewModel, IUnitOfWork uow, int index, ExcelRecordItemHelperBase<ArcFlashStudy> helper)
        {
            viewModel.UtilityCompany = StringToEntityLookup<UtilityCompany>(uow,
                index,
                helper,
                nameof(UtilityCompany),
                UtilityCompany);

            viewModel.ArcFlashStatus = StringToEntityLookup<ArcFlashStatus>(uow,
                index,
                helper,
                nameof(ArcFlashStatus),
                ArcFlashStatus);

            viewModel.FacilitySize = StringToEntityLookup<FacilitySize>(uow,
                index,
                helper,
                nameof(FacilitySize),
                FacilitySize);

            viewModel.TypeOfArcFlashAnalysis = StringToEntityLookup<ArcFlashAnalysisType>(uow,
                index,
                helper,
                nameof(ArcFlashAnalysisType),
                TypeOfArcFlashAnalysis);

            viewModel.ArcFlashLabelType = StringToEntityLookup<ArcFlashLabelType>(uow,
                index,
                helper,
                nameof(LabelType),
                LabelType);

            viewModel.TransformerKVARating = StringToEntityLookup<UtilityTransformerKVARating>(uow,
                index,
                helper,
                nameof(TransformerKVARating),
                TransformerKVARating);

            viewModel.Voltage = StringToEntityLookup<Voltage>(uow,
                index,
                helper,
                nameof(SecondaryVoltage),
                SecondaryVoltage);

            viewModel.PowerPhase = StringToEntityLookup<PowerPhase>(uow,
                index,
                helper,
                nameof(PowerPhase),
                PowerPhase);

            viewModel.FacilityTransformerWiringType = StringToEntityLookup<FacilityTransformerWiringType>(uow,
                index,
                helper,
                nameof(FacilityTransformerWiringConfiguration),
                FacilityTransformerWiringConfiguration);

            return viewModel;
        }

        public override ArcFlashStudy MapAndValidate(IUnitOfWork uow, int index, ExcelRecordItemValidationHelper<ArcFlashStudy, MyCreateArcFlashStudy, ArcFlashStudyExcelRecord> helper)
        {
            return MapToEntity(uow, index, helper);
        }

        public override ArcFlashStudy MapToEntity(IUnitOfWork uow, int index, ExcelRecordItemHelperBase<ArcFlashStudy> helper)
        {
            EnsureUtilityCompanyDataReceivedDateIsNotNullWhenUtilityCompanyDataReceivedIsTrue(index, helper);
            EnsureConditionallyRequiredFieldsAreProvidedWhenLabelTypeIsNotSetToStandardLabel(uow, index, helper);

            return base.MapToEntity(uow, index, helper);
        }

        private void EnsureUtilityCompanyDataReceivedDateIsNotNullWhenUtilityCompanyDataReceivedIsTrue(int index, ExcelRecordItemHelperBase<ArcFlashStudy> helper)
        {
            if (UtilityCompanyDataReceived && UtilityCompanyDataReceivedDate is null)
            {
                helper.AddFailure($"Row {index}: The field '{nameof(UtilityCompanyDataReceivedDate)}' must have a value since {nameof(UtilityCompanyDataReceived)} is set to TRUE.");
            }
        }

        private void EnsureConditionallyRequiredFieldsAreProvidedWhenLabelTypeIsNotSetToStandardLabel(IUnitOfWork uow, int index, ExcelRecordItemHelperBase<ArcFlashStudy> helper)
        {
            var standardLabelType = uow.GetRepository<ArcFlashLabelType>()
                                       .Where(x => x.Id == ArcFlashLabelType.Indices.STANDARDLABEL)
                                       .Single();

            if (string.Equals(LabelType, standardLabelType.Description, StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            if (string.IsNullOrEmpty(SecondaryVoltage))
            {
                helper.AddFailure($"Row {index}: The field '{nameof(SecondaryVoltage)}' must have a value since {nameof(LabelType)} is not '{standardLabelType.Description}'");
            }

            if (string.IsNullOrEmpty(PowerPhase))
            {
                helper.AddFailure($"Row {index}: The field '{nameof(PowerPhase)}' must have a value since {nameof(LabelType)} is not '{standardLabelType.Description}'");
            }

            if (string.IsNullOrEmpty(TransformerKVARating))
            {
                helper.AddFailure($"Row {index}: The field '{nameof(TransformerKVARating)}' must have a value since {nameof(LabelType)} is not '{standardLabelType.Description}'");
            }
        }   

        #endregion
    }
}
