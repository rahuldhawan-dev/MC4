using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class DataTableLayoutViewModel : ViewModel<DataTableLayout>
    {
        #region Properties

        // NOTE: This *must* be an array in order to work with ajax json binding.
        // Lists won't bind at all, even with a custom binder. 
        [RequiredCollection]
        public string[] Properties { get; set; }

        [Required, DoesNotAutoMap("This is only used for validation.")]
        public Guid? TypeGuid { get; set; }

        #endregion

        #region Constructors

        public DataTableLayoutViewModel(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override DataTableLayout MapToEntity(DataTableLayout entity)
        {
            base.MapToEntity(entity);

            // Remove properties that were removed
            var allProperties = entity.Properties.ToDictionary(x => x.PropertyName, x => x);
            var removedProperties = allProperties.Keys.Except(Properties);

            foreach (var removedProp in removedProperties)
            {
                entity.Properties.Remove(allProperties[removedProp]);
            }

            // Add properties that were added
            var newProps = Properties.Except(allProperties.Keys);
            foreach (var newProp in newProps)
            {
                entity.Properties.Add(new DataTableLayoutProperty
                {
                    DataTableLayout = entity,
                    PropertyName = newProp
                });
            }

            return entity;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // TODO: Validate that all properties belong to type. Where's that type info coming from?
            //       ControlHelper should know the type it's rendering, could get it from there.
            // TODO: There's RequiredCollection on Properties but we also want to validate that
            //       we aren't wasting time saving a layout that includes all of the type's properties.

            return base.Validate(validationContext).Concat(ValidatePropertyNamesBelongToType());
        }

        private IEnumerable<ValidationResult> ValidatePropertyNamesBelongToType()
        {
            // Break out of this validation early. The model already won't be valid due
            // to the RequiredCollection attribute on the Properties property. Same
            // with the required TypeGuid validation.
            if (Properties == null || !TypeGuid.HasValue)
            {
                yield break;
            }

            if (!TypeCache.TryRetrieveType(TypeGuid.Value, out var type))
            {
                yield return new ValidationResult("Unable to find type.", new[] { nameof(TypeGuid) });
                yield break;
            }

            var publicInstancePropertyNames = type.GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public)
                                                  .Select(x => x.Name).ToList();

            foreach (var prop in Properties)
            {
                if (!publicInstancePropertyNames.Contains(prop))
                {
                    yield return new ValidationResult($"The property '{prop}' does not exist.", new[] { nameof(Properties) });
                }
            }
        }

        #endregion
    }

    public class CreateDataTableLayout : DataTableLayoutViewModel
    {
        #region Properties

        [Required, StringLength(DataTableLayout.StringLengths.LAYOUT_NAME)]
        public string LayoutName { get; set; }

        [Required, StringLength(DataTableLayout.StringLengths.CONTROLLER)]
        public string Area { get; set; }

        [Required, StringLength(DataTableLayout.StringLengths.CONTROLLER)]
        public string Controller { get; set; }

        #endregion

        #region Constructors

        public CreateDataTableLayout(IContainer container) : base(container) { }

        #endregion

        #region Private Methods

        private IEnumerable<ValidationResult> ValidateLayoutNameIsUniqueToAreaAndController()
        {
            var dataTableRepo = _container.GetInstance<IRepository<DataTableLayout>>();
            var existing = dataTableRepo.Where(x => x.Area == Area && x.Controller == Controller && x.LayoutName == LayoutName).Any();
            if (existing)
            {
                yield return new ValidationResult($"A layout already exists with the name \"{LayoutName}\".", new[] { nameof(LayoutName) });
            }
        }

        #endregion

        #region Public Methods

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext).Concat(ValidateLayoutNameIsUniqueToAreaAndController());
        }

        #endregion
    }

    public class EditDataTableLayout : DataTableLayoutViewModel
    {
        #region Constructors

        public EditDataTableLayout(IContainer container) : base(container) { }

        #endregion
    }

    /// <summary>
    /// View model for the Html.DataTable extension method.
    /// </summary>
    public class DataTableLayoutHtmlHelperViewModel
    {
        #region Properties

        /// <summary>
        /// Gets/sets the area neede for finding appropriate layouts. This is set by the Html.DataTable method.
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// Gets/sets the controller neede for finding appropriate layouts. This is set by the Html.DataTable method.
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// Gets/sets the DataTableLayouts that a user can select from. This is set by the Html.DataTable method.
        /// </summary>
        public IEnumerable<DataTableLayout> DataTableLayouts { get; set; }

        /// <summary>
        /// Gets/sets the default properties that will be selected when the table loads.
        /// </summary>
        public IEnumerable<string> ExportableProperties { get; set; }

        /// <summary>
        /// OPTIONAL: Gets/sets the id attribute for the mc-datatable element, *not* the regular html table inside of it.
        /// </summary>
        public string DataTableId { get; set; }

        /// <summary>
        /// Gets/sets the html for the actual table.
        /// </summary>
        public IHtmlString RazorTable { get; set; }

        /// <summary>
        /// Gets/sets the System.Type that represents data items in the table.
        /// </summary>
        public Type ModelType { get; set; }

        public Guid ModelTypeKey
        {
            get { return TypeCache.RegisterType(ModelType); }
        }

        #endregion
    }
}