using JetBrains.Annotations;

namespace MMSINC.Metadata
{
    /// <summary>
    /// Represents a field that should be a check box list. 
    /// </summary>
    public class CheckBoxListAttribute : SelectAttribute
    {
        #region Constructors

        /// <summary>
        /// Default constructor for if you just wanna use named parameters.
        /// </summary>
        public CheckBoxListAttribute() : base(SelectType.CheckBoxList) { }

        public CheckBoxListAttribute(string controllerViewDataKey) : base(SelectType.CheckBoxList,
            controllerViewDataKey) { }

        /// <summary>
        /// Creates a new cascading CheckBoxListAttribute instance with the given controller and action names. 
        /// If an Area is not set, the default area will be used.
        /// </summary>
        public CheckBoxListAttribute([AspMvcController] string controllerName, [AspMvcAction] string actionName) : base(
            SelectType.CheckBoxList, controllerName, actionName) { }

        /// <summary>
        /// Creates a new cascading CheckBoxListAttribute instance with the given area, controller, and action names.
        /// </summary>
        public CheckBoxListAttribute([AspMvcArea] string areaName, [AspMvcController] string controllerName,
            [AspMvcAction] string actionName)
            : base(SelectType.CheckBoxList, areaName, controllerName, actionName) { }

        #endregion
    }
}
