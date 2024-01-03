using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MapCallMVC.Models.ViewModels
{
    public class TownViewModel : ViewModel<Town>
    {
        #region Properties

        [Required, Display(Name = "Township Name"), StringLength(Town.StringLengths.TOWN)]
        public virtual string ShortName { get; set; }
        [StringLength(Town.StringLengths.ADDRESS)]
        public virtual string Address { get; set; }
        [StringLength(Town.StringLengths.CONTACT_NAME)]
        public virtual string ContactName { get; set; }
        public virtual float? DistrictId { get; set; }
        [StringLength(Town.StringLengths.EMERGENCY_CONTACT)]
        public virtual string EmergencyContact { get; set; }
        [StringLength(Town.StringLengths.EMERGENCY_FAX)]
        public virtual string EmergencyFax { get; set; }
        [StringLength(Town.StringLengths.EMERGENCY_PHONE)]
        public virtual string EmergencyPhone { get; set; }
        [StringLength(Town.StringLengths.FAX)]
        public virtual string Fax { get; set; }
        [StringLength(Town.StringLengths.FD1_CONTACT), Display(Name = "FD Emergency Contact")]
        public virtual string FD1Contact { get; set; }
        [StringLength(Town.StringLengths.FD1_FAX), Display(Name = "FD Emergency Fax")]
        public virtual string FD1Fax { get; set; }
        [StringLength(Town.StringLengths.FD1_PHONE), Display(Name = "FD Emergency Phone")]
        public virtual string FD1Phone { get; set; }
        [StringLength(Town.StringLengths.PHONE)]
        public virtual string Phone { get; set; }
        [Required, StringLength(Town.StringLengths.TOWN_NAME)]
        public virtual string FullName { get; set; }
        [StringLength(Town.StringLengths.ZIP)]
        public virtual string Zip { get; set; }
        [StringLength(Town.StringLengths.LAT)]
        public virtual string Lat { get; set; }
        [StringLength(Town.StringLengths.LON)]
        public virtual string Lon { get; set; }
        [StringLength(Town.StringLengths.DBA)]
        public virtual string DBA { get; set; }
        [StringLength(Town.StringLengths.CRITICAL_MAIN_BREAK_NOTES)]
        public virtual string CriticalMainBreakNotes { get; set; }

        [Required, DropDown]
        [EntityMustExist(typeof(AbbreviationType))]
        [EntityMap]
        public virtual int? AbbreviationType { get; set; }

        [Required, DropDown]
        [EntityMustExist(typeof(State))]
        [EntityMap]
        public virtual int? State { get; set; }

        [Required, DropDown("County", "ByStateId", DependsOn = "State", PromptText = "Please select a state above")]
        [EntityMustExist(typeof(County))]
        [EntityMap]
        public virtual int? County { get; set; }

        #endregion

        #region Constructors

        public TownViewModel(IContainer container) : base(container) { }

        #endregion
    }

    public class EditTown : TownViewModel
    {
        #region Properties


        #endregion

        #region Constructors

        public EditTown(IContainer container) : base(container) { }

        #endregion
    }

    public class CreateTown : TownViewModel
    {
        #region Constructors

        public CreateTown(IContainer container) : base(container) { }

        #endregion
    }

    public class AlterPublicWaterSuppliesTown : ViewModel<Town>
    {
        #region Properties

        [DoesNotAutoMap("Manually mapped")]
        [EntityMustExist(typeof(PublicWaterSupply))]
        public virtual int PublicWaterSupplyId { get; set; }

        #endregion

        #region Constructors

        public AlterPublicWaterSuppliesTown(IContainer container) : base(container) { }

        #endregion
    }

    public class AddPublicWaterSupplyTown : AlterPublicWaterSuppliesTown
    {
        #region Properties

        [DoesNotAutoMap("Display only")]
        public PublicWaterSupply NewPublicWaterSupply
        {
            get { return _container.GetInstance<IPublicWaterSupplyRepository>().Find(PublicWaterSupplyId); }
        }

        #endregion

        #region Constructors

        public AddPublicWaterSupplyTown(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override Town MapToEntity(Town entity)
        {
            // Don't call base.MapToEntity, there's nothing to automap.
            entity.PublicWaterSupplies.Add(NewPublicWaterSupply);
            return entity;
        }

        #endregion
    }

    public class RemovePublicWaterSupplyTown : AlterPublicWaterSuppliesTown
    {
        #region Constructors

        public RemovePublicWaterSupplyTown(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override Town MapToEntity(Town entity)
        {
            var toRemove = entity.PublicWaterSupplies.Where(pws => pws.Id == PublicWaterSupplyId).ToList();
            foreach (var pws in toRemove)
            {
                entity.PublicWaterSupplies.Remove(pws);
            }

            // Don't call base.MapToEntity, there's nothing to automap.
            return entity;
        }

        #endregion
    }

    public class AlterWasteWaterSystemsTown : ViewModel<Town>
    {
        #region Properties

        [DoesNotAutoMap("Manually mapped"), EntityMustExist(typeof(WasteWaterSystem))]
        public virtual int WasteWaterSystemId { get; set; }

        #endregion

        #region Constructors

        public AlterWasteWaterSystemsTown(IContainer container) : base(container) { }

        #endregion
    }

    public class AddWasteWaterSystemTown : AlterWasteWaterSystemsTown
    {
        #region Properties

        [DoesNotAutoMap("Manually mapped.")]
        public WasteWaterSystem NewWasteWaterSystem
        {
            get { return _container.GetInstance<MMSINC.Data.NHibernate.IRepository<WasteWaterSystem>>().Find(WasteWaterSystemId); }
        }

        #endregion

        #region Constructors

        public AddWasteWaterSystemTown(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override Town MapToEntity(Town entity)
        {
            // Don't call base.MapToEntity.
            var newWWS = NewWasteWaterSystem;
            if (!entity.WasteWaterSystems.Contains(newWWS))
            {
                entity.WasteWaterSystems.Add(newWWS);
            }
            return entity;
        }

        #endregion
    }

    public class RemoveWasteWaterSystemTown : AlterWasteWaterSystemsTown
    {
        #region Constructors

        public RemoveWasteWaterSystemTown(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override Town MapToEntity(Town entity)
        {
            // Don't call base.MapToEntity.

            var toRemove = entity.WasteWaterSystems.Where(x => x.Id == WasteWaterSystemId).ToList();
            foreach (var wws in toRemove)
            {
                entity.WasteWaterSystems.Remove(wws);
            }

            return entity;
        }

        #endregion
    }
    
    public class AddTownGradient : ViewModel<Town>
    {
        #region Properties

        [DropDown, EntityMap]
        [Required, EntityMustExist(typeof(Gradient))]
        public virtual int? Gradient { get; set; }

        #endregion

        #region Constructors

        public AddTownGradient(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override Town MapToEntity(Town entity)
        {
            if (!entity.Gradients.Any(x => x.Id == Gradient.Value))
            {
                var gradient = _container.GetInstance<IGradientRepository>().Find(Gradient.Value);
                entity.Gradients.Add(gradient);
            }

            return entity;
        }

        #endregion
    }
    public class RemoveTownGradient : ViewModel<Town>
    {
        #region Properties

        [Required, EntityMustExist(typeof(Gradient))]
        public virtual int? Gradient { get; set; }

        #endregion

        #region Constructors

        public RemoveTownGradient(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override Town MapToEntity(Town entity)
        {
            var gradient = entity.Gradients.SingleOrDefault(x => x.Id == Gradient.Value);
            if (gradient != null)
            {
                entity.Gradients.Remove(gradient);
            }

            return entity;
        }

        #endregion
    }

    public class SearchTown : SearchSet<Town>
    {
        #region Properties

        [DropDown]
        public virtual int? State { get; set; }

        [DropDown("County", "ByStateId", DependsOn = "State", PromptText = "Please select a state above")]
        public virtual int? County { get; set; }

        [Display(Name = "Town"), DropDown("Town", "ByCountyId", DependsOn = "County", PromptText = "Please select a county above")]
        public virtual int? Id { get; set; }

        [Display(Name = "Town Name")]
        public virtual string FullName { get; set; }

        #endregion
    }

    public class CreateTownContact : ViewModel<Town>
    {
        #region Properties

        [EntityMustExist(typeof(Contact))]
        [Required]
        [AutoComplete("Contact", "ByPartialNameMatch")]
        [AutoMap(MapDirections.None)]
        public int? Contact { get; set; }

        [EntityMustExist(typeof(ContactType))]
        [Required]
        [DropDown]
        [AutoMap(MapDirections.None)]
        public int? ContactType { get; set; }

        #endregion

        #region Constructor

        public CreateTownContact(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override Town MapToEntity(Town entity)
        {
            var tc = new TownContact
            {
                Town = entity,
                Contact = _container.GetInstance<IContactRepository>().Find(Contact.GetValueOrDefault()),
                ContactType =
                    _container.GetInstance<MMSINC.Data.NHibernate.IRepository<ContactType>>().Find(ContactType.GetValueOrDefault())
            };
            entity.TownContacts.Add(tc);

            // Not calling base method. There's no actual mapping that needs to occur.
            return entity;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext).Union(Validate());
        }

        private IEnumerable<ValidationResult> Validate()
        {
            // The Town existence is done by ActionHelper before it gets here.
            // If the Town is null, just ignore the rest of the validation since
            // this method gets called twice all silly-like.

            var town = _container.GetInstance<ITownRepository>().Find(Id);
            if (town == null)
            {
                yield break;
            }

            if (town.TownContacts.Any(x => x.ContactType.Id == ContactType && x.Contact.Id == Contact))
            {
                const string errFormat = "A contact already exists for this contact and contact type for {0}.";
                var err = string.Format(errFormat, town.ShortName);
                yield return new ValidationResult(err, new[] { "ContactId" });
            }
        }

        #endregion
    }

    public class DestroyTownContact : ViewModel<Town>
    {
        #region Properties

        [Required, DoesNotAutoMap("Manually mapped")]
        [EntityMustExist(typeof(TownContact))]
        public int? TownContactId { get; set; }

        #endregion

        #region Constructor

        public DestroyTownContact(IContainer container) : base(container) { }

        #endregion

        #region Public methods

        public override Town MapToEntity(Town entity)
        {
            var actualTownContact = entity.TownContacts.Single(x => x.Id == TownContactId);
            entity.TownContacts.Remove(actualTownContact);

            // No need to call base method since no other mapping should occur.
            return entity;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext).Union(Validate());
        }

        private IEnumerable<ValidationResult> Validate()
        {
            // The Town existence is done by ActionHelper before it gets here.
            // If the Town is null, just ignore the rest of the validation since
            // this method gets called twice all silly-like.
            var town = _container.GetInstance<ITownRepository>().Find(Id);
            if (town == null)
            {
                yield break;
            }

            var townContact = town.TownContacts.SingleOrDefault(x => x.Id == TownContactId);
            if (townContact == null)
            {
                yield return new ValidationResult("Contact does not exist for this town.", new[] { "TownContactId" });
            }
        }

        #endregion
    }
}