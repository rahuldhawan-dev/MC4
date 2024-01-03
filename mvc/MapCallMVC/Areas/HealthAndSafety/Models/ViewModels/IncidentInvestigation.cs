using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.HealthAndSafety.Models.ViewModels
{
    public class IncidentInvestigationViewModel : ViewModel<IncidentInvestigation>
    {
        #region Properties

        [Secured, Required, EntityMap, EntityMustExist(typeof(Incident))]
        public int? Incident { get; set; }

        [DropDown]
        [Required, EntityMap, EntityMustExist(typeof(IncidentInvestigationRootCauseFindingType))]
        public int? IncidentInvestigationRootCauseFindingType { get; set; }

        [DropDown]
        [Required, EntityMap, EntityMustExist(typeof(IncidentInvestigationRootCauseLevel1Type))]
        public int? IncidentInvestigationRootCauseLevel1Type { get; set; }

        [DropDown("HealthAndSafety", "IncidentInvestigationRootCauseLevel2Type", "ByLevel1", DependsOn = nameof(IncidentInvestigationRootCauseLevel1Type))]
        [Required, EntityMap, EntityMustExist(typeof(IncidentInvestigationRootCauseLevel2Type))]
        public int? IncidentInvestigationRootCauseLevel2Type { get; set; }

        // Level3 is not required because not all Level2s have a Level3.
        [DropDown("HealthAndSafety", "IncidentInvestigationRootCauseLevel3Type", "ByLevel2", DependsOn = nameof(IncidentInvestigationRootCauseLevel2Type))]
        [EntityMap, EntityMustExist(typeof(IncidentInvestigationRootCauseLevel3Type))]
        public int? IncidentInvestigationRootCauseLevel3Type { get; set; }

        // Incident is secured, so this value should never end up being null.
        [DoesNotAutoMap("Needed only for cascading on the server-side")]
        public int? State => _container.GetInstance<IRepository<Incident>>().Find(Incident.Value).OperatingCenter.State.Id;

        [MultiSelect("", "User", "GetActiveUsersByStateId", DependsOn = nameof(State))] 
        [RequiredCollection, EntityMap, EntityMustExist(typeof(User))]
        public int[] RootCauseFindingPerformedByUsers { get; set; }

        #endregion

        #region Constructors

        public IncidentInvestigationViewModel(IContainer container) : base(container) { }

        #endregion
    }

    public class CreateIncidentInvestigation : IncidentInvestigationViewModel
    {
        #region Constructor

        public CreateIncidentInvestigation(IContainer container) : base(container) { }

        #endregion

        #region Validation

        private IEnumerable<ValidationResult> ValidateIsOSHARecordable()
        {
            var incident = _container.GetInstance<IRepository<Incident>>().Find(Incident.GetValueOrDefault());

            if (incident == null)
            {
                yield break; // cut out early, Required validation covers this already.
            }

            if (!incident.IsOSHARecordable)
            {
                yield return new ValidationResult("Incident investigations can not be created for an incident that is not OSHA recordable.");
            }
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext).Concat(ValidateIsOSHARecordable());
        }

        #endregion
    }

    public class EditIncidentInvestigation : IncidentInvestigationViewModel
    {
        #region Constructor

        public EditIncidentInvestigation(IContainer container) : base(container) { }

        #endregion
    }
}