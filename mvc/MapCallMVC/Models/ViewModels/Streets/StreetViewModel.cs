using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Models.ViewModels.Streets
{
    public abstract class StreetViewModel : ViewModel<Street>
    {
        #region Properties

        [DropDown, EntityMap, EntityMustExist(typeof(StreetPrefix))]
        public int? Prefix { get; set; }

        [Required, StringLength(Street.StringLengths.NAME)]
        public string Name { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(StreetSuffix))]
        public int? Suffix { get; set; }
        public bool IsActive { get; set; }

        #endregion

        #region Constructors

        protected StreetViewModel(IContainer container) : base(container) { }

        #endregion

        private static string GetPreparedFullStreetName(StreetPrefix prefix, string name, StreetSuffix suffix)
        {
            return $"{prefix} {name} {suffix}".Trim();
        }

        public override Street MapToEntity(Street entity)
        {
            base.MapToEntity(entity);
            entity.FullStName = GetPreparedFullStreetName(entity.Prefix, entity.Name, entity.Suffix);
            return entity;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            IsActive = true;
        }

        #region Validation

        protected abstract Town TryGetTownForStreetNameValidation();

        private IEnumerable<ValidationResult> ValidateStreetNameIsUnique()
        {
            var town = TryGetTownForStreetNameValidation();
            if (town == null)
            {
                // This validation should never actually be seen, but best to play it safe.
                yield return new ValidationResult("A town is required.");
            }
            else
            {
                var prefix = _container.GetInstance<IRepository<StreetPrefix>>().Find(Prefix.GetValueOrDefault());
                var suffix = _container.GetInstance<IRepository<StreetSuffix>>().Find(Suffix.GetValueOrDefault());
                var preparedName = GetPreparedFullStreetName(prefix, Name, suffix);

                // Compare against FullStName because nothing's stopping someone from setting 
                // just the Name of the street to something like "N South St" instead of setting
                // that via the prefix/suffix dropdowns.
                var matches = town.Streets.Where(x => x.FullStName.ToLowerInvariant() == preparedName.ToLowerInvariant()).ToArray();
                if (matches.Any(x => x.Id != Id))
                {
                    yield return new ValidationResult("A record already exists for this street for this town.");
                }
            }
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext).Concat(ValidateStreetNameIsUnique());
        }

        #endregion 
    }
}