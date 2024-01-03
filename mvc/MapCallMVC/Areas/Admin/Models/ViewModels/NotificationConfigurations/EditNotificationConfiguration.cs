using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.Admin.Models.ViewModels.NotificationConfigurations
{
    public class EditNotificationConfiguration : ViewModel<NotificationConfiguration>
    {
        #region Constructors

        public EditNotificationConfiguration(IContainer container) : base(container) { }

        #endregion

        #region Properties

        [Required, 
         DropDown, 
         EntityMap, 
         EntityMustExist(typeof(Contact))]
        public int? Contact { get; set; }

        [View(Description = "When checked, this notification will be sent for *all* operating centers.")]
        public bool AppliesToAllOperatingCenters { get; set; }

        [RequiredWhen(nameof(AppliesToAllOperatingCenters), false), 
         DropDown, 
         EntityMap, 
         EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        [Required, 
         DropDown, 
         EntityMustExist(typeof(Application)), 
         DoesNotAutoMap]
        public int? Application { get; set; }

        [Required, 
         EntityMustExist(typeof(Module)), 
         DoesNotAutoMap,
         DropDown("Admin", "Module", "ByApplication", DependsOn = nameof(Application), PromptText = "Please select an Application.")]
        public int? Module { get; set; }

        [Required, 
         EntityMap, 
         EntityMustExist(typeof(NotificationPurpose)),
         MultiSelect("Admin", "NotificationPurpose", "ByModule", DependsOn = nameof(Module), PromptText = "Please select a Module.")]
        public int[] NotificationPurposes { get; set; }

        #endregion

        #region Exposed Methods

        public override void Map(NotificationConfiguration entity)
        {
            base.Map(entity);
            Application = entity.NotificationPurposes?.FirstOrDefault()?.Module.Application.Id;
            Module = entity.NotificationPurposes?.FirstOrDefault()?.Module.Id;
        }

        public override NotificationConfiguration MapToEntity(NotificationConfiguration entity)
        {
            entity = base.MapToEntity(entity);

            if (AppliesToAllOperatingCenters)
            {
                entity.OperatingCenter = null;
            }

            return entity;
        }

        #endregion
    }
}