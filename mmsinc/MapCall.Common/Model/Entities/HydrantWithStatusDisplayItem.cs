using System;
using MMSINC.ClassExtensions.IQueryableExtensions;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class HydrantDisplayItem : DisplayItem<Hydrant>
    {
        private string _display;

        public string HydrantNumber { get; set; }

        [SelectDynamic("Description")]
        public string Status { get; set; }

        public override string Display
        {
            get
            {
                if (_display == null)
                {
                    _display = $"{HydrantNumber} - {Status}";
                }

                return _display;
            }
        }
    }
}
