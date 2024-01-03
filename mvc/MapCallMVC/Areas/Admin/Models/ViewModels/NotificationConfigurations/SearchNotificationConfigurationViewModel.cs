using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;

namespace MapCallMVC.Areas.Admin.Models.ViewModels.NotificationConfigurations
{
    public class SearchNotificationConfigurationViewModel : SearchSet<NotificationConfiguration>
    {
        [DropDown, 
         EntityMap, 
         EntityMustExist(typeof(Contact))]
        public int? Contact { get; set; }

        [DropDown, 
         EntityMap, 
         EntityMustExist(typeof(State)),
         SearchAlias("criteriaOperatingCenter.State", "criteriaState", "Id")]
        public int? State { get; set; }

        [EntityMap, 
         EntityMustExist(typeof(OperatingCenter)),
         SearchAlias("OperatingCenter", "criteriaOperatingCenter", "Id"),
         DropDown("", "OperatingCenter", "ByStateId", DependsOn = nameof(State), PromptText = "Please select a State.")]
        public int? OperatingCenter { get; set; }

        [DropDown, 
         DoesNotAutoMap, 
         EntityMustExist(typeof(Application)),
         SearchAlias("criteriaModule.Application", "criteriaApplication", "Id")]
        public int? Application { get; set; }

        [DoesNotAutoMap, 
         EntityMustExist(typeof(Module)),
         SearchAlias("criteriaNotificationPurposes.Module", "criteriaModule", "Id"),
         DropDown("Admin", "Module", "ByApplication", DependsOn = nameof(Application), PromptText = "Please select an Application.")]
        public int? Module { get; set; }

        [EntityMap, 
         EntityMustExist(typeof(NotificationPurpose)),
         SearchAlias("NotificationPurposes", "criteriaNotificationPurposes", "Id"),
         MultiSelect("Admin", "NotificationPurpose", "ByModule", DependsOn = nameof(Module), PromptText = "Please select a Module.")]
        public int[] NotificationPurposes { get; set; }
    }
}