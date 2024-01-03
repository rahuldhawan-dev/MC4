using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.Environmental.Models.ViewModels.EnvironmentalPermits
{
    public class CreateEnvironmentalPermitRequirement : ViewModel<EnvironmentalPermit>
    {
        #region Properties

        [DoesNotAutoMap]
        public int[] OperatingCenters { get; set; }

        [DoesNotAutoMap]
        public int? State { get; set; }

        [DropDown, Required, DoesNotAutoMap]
        public int? RequirementType { get; set; }

        [Required, DoesNotAutoMap, StringLength(EnvironmentalPermitRequirement.StringLengths.REQUIREMENT)]
        public string Requirement { get; set; }

        [DropDown, Required, DoesNotAutoMap]
        public int? ValueUnit { get; set; }

        [DropDown, Required, DoesNotAutoMap]
        public int? ValueDefinition { get; set; }

        [DropDown, Required, DoesNotAutoMap]
        public int? TrackingFrequency { get; set; }

        [DropDown, Required, DoesNotAutoMap]
        public int? ReportingFrequency { get; set; }

        [Multiline, DoesNotAutoMap]
        public string ReportingFrequencyDetails { get; set; }

        [DropDown("", "Employee", "ByOperatingCentersOrState", DependsOn = "State,OperatingCenters", DependentsRequired = DependentRequirement.One)]
        [DoesNotAutoMap]
        public int? ProcessOwner { get; set; }

        [DropDown("", "Employee", "ByOperatingCentersOrState", DependsOn = "State,OperatingCenters", DependentsRequired = DependentRequirement.One)]
        [Required, DoesNotAutoMap]
        public int? ReportingOwner { get; set; }

        [DoesNotAutoMap, StringLength(EnvironmentalPermitRequirement.StringLengths.REPORT_DATA_STORAGE_LOCATION)]
        public string ReportDataStorageLocation { get; set; }

        [DoesNotAutoMap, StringLength(EnvironmentalPermitRequirement.StringLengths.REPORT_CREATION_INSTRUCTIONS)]
        public string ReportCreationInstructions { get; set; }

        [Multiline, DoesNotAutoMap]
        [RequiredWhen("RequiresRequirements", ComparisonType.EqualTo, true)]
        public string ReportSendTo { get; set; }

        [DoesNotAutoMap]
        [DropDown, RequiredWhen("ReportSendTo", ComparisonType.NotEqualTo, "")]
        public int? CommunicationType { get; set; }

        [Multiline, View("Notes"), DoesNotAutoMap]
        public string RequirementNotes { get; set; }

        [DoesNotAutoMap]
        [RequiredWhen("CommunicationType", ComparisonType.EqualTo, MapCall.Common.Model.Entities.CommunicationType.Indices.EMAIL)]
        [StringLength(EnvironmentalPermitRequirement.StringLengths.COMMUNICATION_EMAIL)]
        public string CommunicationEmail { get; set; }

        [DoesNotAutoMap]
        [RequiredWhen("CommunicationType", ComparisonType.EqualTo, MapCall.Common.Model.Entities.CommunicationType.Indices.AGENCY_SUBMITTAL_FORM)]
        [StringLength(EnvironmentalPermitRequirement.StringLengths.COMMUNICATION_LINK)]
        public string CommunicationLink { get; set; }

        [DoesNotAutoMap]
        public virtual bool? RequiresRequirements { get; set; }

        #endregion

        #region Constructors

        public CreateEnvironmentalPermitRequirement(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override void Map(EnvironmentalPermit entity)
        {
            base.Map(entity);
            OperatingCenters = entity.OperatingCenters.Select(oc => oc.Id).ToArray();
            State = entity.State?.Id;
            RequiresRequirements = entity.RequiresRequirements;
        }

        public override EnvironmentalPermit MapToEntity(EnvironmentalPermit entity)
        {
            // NOTE: Do not call base.MapToEntity. This is mapped entirely manually.
            var type = _container.GetInstance<IRepository<EnvironmentalPermitRequirementType>>().Find(RequirementType.Value);
            var valueUnit = _container.GetInstance<IRepository<EnvironmentalPermitRequirementValueUnit>>().Find(ValueUnit.Value);
            var valueDefinition = _container.GetInstance<IRepository<EnvironmentalPermitRequirementValueDefinition>>().Find(ValueDefinition.Value);
            var trackingFrequency = _container.GetInstance<IRepository<EnvironmentalPermitRequirementTrackingFrequency>>().Find(TrackingFrequency.Value);
            var reportingFrequency = _container.GetInstance<IRepository<EnvironmentalPermitRequirementReportingFrequency>>().Find(ReportingFrequency.Value);
            var reportingOwner = _container.GetInstance<IRepository<Employee>>().Find(ReportingOwner.Value);
            var communicationType = (CommunicationType.HasValue) ? _container.GetInstance<IRepository<CommunicationType>>().Find(CommunicationType.Value) : null;
            Employee processOwner = null;

            if (ProcessOwner.HasValue)
            {
                processOwner = _container.GetInstance<IRepository<Employee>>().Find(ProcessOwner.Value);
            }

            entity.Requirements.Add(new EnvironmentalPermitRequirement {
                EnvironmentalPermit = entity,
                RequirementType = type,
                Requirement = Requirement,
                ValueUnit = valueUnit,
                ValueDefinition = valueDefinition,
                TrackingFrequency = trackingFrequency,
                ReportingFrequency = reportingFrequency,
                ReportingFrequencyDetails = ReportingFrequencyDetails,
                ProcessOwner = processOwner,
                ReportingOwner = reportingOwner,
                ReportDataStorageLocation = ReportDataStorageLocation,
                ReportCreationInstructions = ReportCreationInstructions,
                ReportSendTo = ReportSendTo,
                CommunicationType = communicationType,
                Notes = RequirementNotes,
                CommunicationEmail = CommunicationEmail,
                CommunicationLink = CommunicationLink
            });

            return entity;
        }

        #endregion
    }
}