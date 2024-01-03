using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Admin.Models.ViewModels.NotificationConfigurations
{
    public class CreateNotificationConfigurations : ViewModelSet<NotificationConfiguration>
    {
        #region Constructors

        public CreateNotificationConfigurations(IContainer container) : base(container) { }

        #endregion

        #region Properties

        [Required,
         MultiSelect,
         EntityMap,
         EntityMustExist(typeof(Contact))]
        public int[] Contacts { get; set; }

        // On the frontend, this isn't used to cascade anything.
        // It's only used to automatically select operating center
        // values.
        [View(Description = "Selecting a state will automatically select that state's operating centers."),
         MultiSelect,
         EntityMap,
         EntityMustExist(typeof(State))]
        public int[] States { get; set; }

        [View(Description = "When checked, this notification will be sent for *all* operating centers.")]
        public bool AppliesToAllOperatingCenters { get; set; }

        // RequiredCollection can't work here as it doesn't do
        // RequiredWhen-style validation, and RequiredWhen doesn't work with arrays.
        [RequiredWhen(nameof(AppliesToAllOperatingCenters), false),
         MultiSelect("", "OperatingCenter", "ActiveByStateIdsOrAll", DependsOn = nameof(States), DependentsRequired = DependentRequirement.None),
         EntityMap,
         EntityMustExist(typeof(OperatingCenter))]
        public int[] OperatingCenters { get; set; }

        [Required,
         MultiSelect,
         EntityMustExist(typeof(Application)),
         DoesNotAutoMap("Only used for cascading.")]
        public int[] Application { get; set; }

        [Required,
         EntityMustExist(typeof(Module)), 
         DoesNotAutoMap("Only used for cascading"),
         MultiSelect("Admin", "Module", "ByApplication", DependsOn = nameof(Application), PromptText = "Please select an Application.")]
        public int[] Module { get; set; }

        [Required,
         EntityMap,
         EntityMustExist(typeof(NotificationPurpose)),
         MultiSelect("Admin", "NotificationPurpose", "ByModule", DependsOn = nameof(Module), PromptText = "Please select a Module.")]
        public int[] NotificationPurposes { get; set; }

        public override IEnumerable<NotificationConfiguration> Items
        {
            get
            {
                // TODO: I don't like this setup of using an Items property to return a bunch of new entity
                // instances. I think we should consider changing this to match a little more with how ViewModel<T>
                // works. It could have a MapToEntity function that supplies a Func for generating the new entity
                // instance as needed. 

                // So this needs to create a NotificationConfiguration for every combination of
                // Contacts and OperatingCenters
                // Application and Module don't matter as they're not part of the entity, they
                // only exist for cascading purposes.
                // This is going to be gross because we can't use any of the normal
                // automatic mapping functionality here. 

                var purposeRepo = _container.GetInstance<IRepository<NotificationPurpose>>();
                var contactRepo = _container.GetInstance<IRepository<Contact>>();
                var opcRepo = _container.GetInstance<IRepository<OperatingCenter>>();

                NotificationConfiguration createConfig(int contactId, int? operatingCenterId)
                {
                    var config = new NotificationConfiguration();
                    config.Contact = contactRepo.Load(contactId);
                    if (operatingCenterId.HasValue)
                    {
                        config.OperatingCenter = opcRepo.Load(operatingCenterId.Value);
                    }

                    foreach (var purposeId in NotificationPurposes)
                    {
                        config.NotificationPurposes.Add(purposeRepo.Load(purposeId));
                    }

                    return config;
                }

                foreach (var contactId in Contacts)
                {
                    if (AppliesToAllOperatingCenters)
                    {
                        yield return createConfig(contactId, null);
                    }
                    else
                    {
                        foreach (var opcId in OperatingCenters)
                        {
                            yield return createConfig(contactId, opcId);
                        }
                    }
                }
            }
        }

        #endregion
    }
}