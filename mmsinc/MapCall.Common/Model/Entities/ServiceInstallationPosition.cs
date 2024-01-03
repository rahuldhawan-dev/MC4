using System;
using System.Text;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ServiceInstallationPosition : ReadOnlyEntityLookup
    {
        public virtual string CodeGroup { get; set; }
        public virtual string SAPCode { get; set; }

        public virtual string Display =>
            new ServiceInstallationPositionDisplayItem {
                SAPCode = SAPCode,
                Description = Description
            }.ToString();

        public override string ToString()
        {
            return Display;
        }
    }

    [Serializable]
    public class ServiceInstallationPositionDisplayItem : DisplayItem<ServiceInstallationPosition>
    {
        private string _display;

        public string Description { get; set; }
        public string SAPCode { get; set; }

        public override string Display
        {
            get
            {
                if (_display == null)
                {
                    var sb = new StringBuilder(SAPCode);
                    if (!string.IsNullOrWhiteSpace(Description))
                    {
                        sb.Append(" " + Description);
                    }

                    _display = sb.ToString();
                }

                return _display;
            }
        }
    }
}
