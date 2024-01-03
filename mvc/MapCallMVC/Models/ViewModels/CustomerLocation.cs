using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class CustomerLocationViewModel : ViewModel<CustomerLocation>
    {
        #region Properties

        [AutoMap(MapDirections.ToPrimary)]
        public virtual string PremiseNumber { get; set; }
        public virtual string Address { get; set; }
        public virtual string City { get; set; }
        public virtual string State { get; set; }
        public virtual string Zip { get; set; }

        #endregion

        #region Constructors

        public CustomerLocationViewModel(IContainer container) : base(container) {}

        #endregion
    }

    public class UpdateCustomerLocation : CustomerLocationViewModel
    {
        #region Private Members

        private float? _suggestedLatitude, _suggestedLongitude;

        #endregion

        #region Properties

        [Required]
        public virtual float Latitude { get; set; }

        [Required]
        public virtual float Longitude { get; set; }

        public virtual IList<CustomerCoordinate> CustomerCoordinates { get; set; }

        [DoesNotAutoMap]
        public virtual string ReturnUrl { get; set; }

        [DoesNotAutoMap("Used by view.")]
        public float? SuggestedLatitude
        {
            get { return _suggestedLatitude; }
        }

        [DoesNotAutoMap("Used by view.")]
        public float? SuggestedLongitude
        {
            get { return _suggestedLongitude; }
        }

        #endregion

        #region Constructors

        public UpdateCustomerLocation(IContainer container) : base(container) {}

        #endregion

        #region Private Methods

        protected float? FindCenter(Func<CustomerCoordinate, float> getVal)
        {
            if (CustomerCoordinates.Count == 0)
                return null;
            var min = CustomerCoordinates.Min(getVal);
            var max = CustomerCoordinates.Max(getVal);
            return (min + max)/2;
        }

        #endregion

        #region Exposed Methods

        public override void Map(CustomerLocation entity)
        {
            base.Map(entity);

            if (CustomerCoordinates != null)
            {
                _suggestedLatitude = FindCenter(c => c.Latitude);
                _suggestedLongitude = FindCenter(c => c.Longitude);
            }
        }

        #endregion
    }

    public class SearchCustomerLocation : SearchSet<CustomerLocation>, ISearchCustomerLocation
    {
        #region Properties

        public virtual bool? HasVerifiedCoordinate { get; set; }

        [DropDown]
        public virtual string State { get; set; }

        [DropDown("CustomerLocation", "CitiesByState", DependsOn = "State", PromptText = "Please select a state above")]
        public virtual string City { get; set; }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public void EnsureSearchValues(SearchMappableArgs args)
        {
            args.Properties.Remove("HasVerifiedCoordinate");
        }

        #endregion
    }
}