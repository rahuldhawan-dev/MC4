using System;
using System.ComponentModel.DataAnnotations;
using MMSINC.ClassExtensions.IQueryableExtensions;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ClassLocation : EntityLookup
    {
        #region Private Members

        private ClassLocationDisplayItem _display;

        #endregion

        #region Properties

        public virtual OperatingCenter OperatingCenter { get; set; }

        [Required, StringLength(35), Display(Name = "Description")]
        public virtual string Name { get; set; }

        public override string Description
        {
            get => (_display ?? (_display = new ClassLocationDisplayItem {
                OperatingCenter = OperatingCenter.OperatingCenterCode,
                Name = Name
            })).Display;
            set => Name = value;
        }

        #endregion
    }

    [Serializable]
    public class ClassLocationDisplayItem : DisplayItem<ClassLocation>
    {
        public string Name { get; set; }

        [SelectDynamic("OperatingCenterCode")]
        public string OperatingCenter { get; set; }

        public override string Display => $"{OperatingCenter} - {Name}";
    }
}
