using MapCall.SAP.ManufacturerLookupWS;

namespace MapCall.SAP.Model.Entities
{
    public class SAPManufacturer
    {
        #region properties

        public virtual string Class { get; set; }
        public virtual string Manufacturer { get; set; }
        public virtual string SAPErrorCode { get; set; }

        #endregion

        #region Exposed methods

        public SAPManufacturer() { }

        public SAPManufacturer(string ManufacturerRecord)
        {
            Manufacturer = ManufacturerRecord;
            SAPErrorCode = "Successful";
        }

        public Manufacturer_Query ManufacturerRequest(SAPManufacturer sapManufacturer)
        {
            Manufacturer_Query ManufacturerRequest = new Manufacturer_Query();
            ManufacturerRequest.Class = sapManufacturer.Class;
            ManufacturerRequest.Manufacturer = sapManufacturer.Manufacturer;

            return ManufacturerRequest;
        }

        #endregion
    }
}
