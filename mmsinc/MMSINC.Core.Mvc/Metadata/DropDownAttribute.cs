using JetBrains.Annotations;

namespace MMSINC.Metadata
{
    /// <summary>
    /// Represents a field that should be a dropdown. 
    /// </summary>
    public class DropDownAttribute : SelectAttribute
    {
        #region Constructors

        /// <summary>
        /// Default constructor for if you just wanna use named parameters.
        /// </summary>
        public DropDownAttribute() : base(SelectType.DropDown) { }

        public DropDownAttribute(string controllerViewDataKey) : base(SelectType.DropDown, controllerViewDataKey) { }

        /// <summary>
        /// Creates a new cascading DropDownAttribute instance with the given controller and action names. 
        /// If an Area is not set, the default area will be used.
        /// </summary>
        public DropDownAttribute([AspMvcController] string controllerName, [AspMvcAction] string actionName) : base(
            SelectType.DropDown, controllerName, actionName) { }

        /// <summary>
        /// Creates a new cascading DropDownAttribute instance with the given area, controller, and action names.
        /// </summary>
        public DropDownAttribute([AspMvcArea] string areaName, [AspMvcController] string controllerName,
            [AspMvcAction] string actionName)
            : base(SelectType.DropDown, areaName, controllerName, actionName) { }

        #endregion
    }
}
