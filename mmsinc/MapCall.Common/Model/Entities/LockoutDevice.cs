using System;
using MapCall.Common.Model.Entities.Users;
using MMSINC.ClassExtensions.IQueryableExtensions;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class LockoutDevice : ReadOnlyEntityLookup
    {
        public static string FORMAT_STRING = "{0} - {1} - {2}";

        #region Private Members

        private LockoutDeviceDisplayItem _display;

        #endregion

        #region Properties

        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual User Person { get; set; }
        public virtual LockoutDeviceColor LockoutDeviceColor { get; set; }
        public virtual string SerialNumber { get; set; }

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return (_display ?? (_display = new LockoutDeviceDisplayItem {
                LockoutDeviceColor = LockoutDeviceColor?.Description,
                SerialNumber = SerialNumber,
                Description = Description
            })).Display;
        }

        #endregion
    }

    [Serializable]
    public class LockoutDeviceDisplayItem : DisplayItem<LockoutDevice>
    {
        [SelectDynamic("Description")]
        public string LockoutDeviceColor { get; set; }

        public string SerialNumber { get; set; }
        public string Description { get; set; }
        public override string Display => $"{LockoutDeviceColor} - {SerialNumber} - {Description}";
    }
}
