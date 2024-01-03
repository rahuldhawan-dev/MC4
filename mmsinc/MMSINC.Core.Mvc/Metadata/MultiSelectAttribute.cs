using JetBrains.Annotations;

namespace MMSINC.Metadata
{
    /// <summary>
    /// Represents a field that should be a multi select list. 
    /// </summary>
    public class MultiSelectAttribute : SelectAttribute
    {
        #region Constructors

        /// <summary>
        /// Default constructor for if you just wanna use named parameters.
        /// </summary>
        public MultiSelectAttribute() : base(SelectType.MultiSelect) { }

        public MultiSelectAttribute(string controllerViewDataKey) :
            base(SelectType.MultiSelect, controllerViewDataKey) { }

        /// <summary>
        /// Creates a new cascading MultiSelectAttribute instance with the given controller and action names. 
        /// If an Area is not set, the default area will be used.
        /// </summary>
        public MultiSelectAttribute([AspMvcController] string controllerName, [AspMvcAction] string actionName) : base(
            SelectType.MultiSelect, controllerName, actionName) { }

        /// <summary>
        /// Creates a new cascading MultiSelectAttribute instance with the given area, controller, and action names.
        /// </summary>
        public MultiSelectAttribute([AspMvcArea] string areaName, [AspMvcController] string controllerName,
            [AspMvcAction] string actionName)
            : base(SelectType.MultiSelect, areaName, controllerName, actionName) { }

        #endregion
    }
}
