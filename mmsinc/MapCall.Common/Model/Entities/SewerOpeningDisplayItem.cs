using System;
using MMSINC.ClassExtensions.IQueryableExtensions;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class SewerOpeningDisplayItem : DisplayItem<SewerOpening>
    {
        private string _display;

        public string OpeningNumber { get; set; }

        [SelectDynamic("Description")]
        public string Status { get; set; }

        public override string Display
        {
            get
            {
                if (_display == null)
                {
                    _display = $"{OpeningNumber} - {Status}";
                }

                return _display;
            }
        }
    }
}
