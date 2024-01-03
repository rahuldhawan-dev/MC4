namespace MapCallScheduler.JobHelpers.SapPremise
{
    public class SapPremiseFileRecord
    {
        public virtual string OperatingCentre { get; set; } = "";
        public virtual string PremiseNumber { get; set; } = "";
        public virtual string ConnectionObject { get; set; } = "";
        public virtual string DeviceLocation { get; set; } = "";
        public virtual string DeviceCategory { get; set; } = "";
        public virtual string Equipment { get; set; } = "";
        public virtual string NextMeterReadingdate { get; set; } = "";
        public virtual string DeviceSerialNumber { get; set; } = "";
        public virtual string Latitude { get; set; } = "";
        public virtual string Longitude { get; set; } = "";
        public virtual string ServiceAddressHouseNumber { get; set; } = "";
        public virtual string ServiceAddressFraction { get; set; } = "";
        public virtual string ServiceAddressApartment { get; set; } = "";
        public virtual string ServiceAddressStreet { get; set; } = "";
        public virtual string ServiceCity { get; set; } = "";
        public virtual string ServiceState { get; set; } = "";
        public virtual string ServiceZip { get; set; } = "";
        public virtual string ServiceDistrict { get; set; } = "";
        public virtual string ServiceDistrictDescription { get; set; } = "";
        public virtual string AreaCode { get; set; } = "";
        public virtual string AreaCodeDescription { get; set; } = "";
        public virtual string RegionCode { get; set; } = "";
        public virtual string RegionCodeDescription { get; set; } = "";
        public virtual string RouteNumber { get; set; } = "";
        public virtual string RouteNumberDescription { get; set; } = "";
        public virtual string StatusCode { get; set; } = "";
        public virtual string ServiceUtilityType { get; set; } = "";
        public virtual string MeterSize { get; set; } = "";
        public virtual string MeterLocation { get; set; } = "";
        public virtual string MeterLocationFreeText { get; set; } = "";
        public virtual string Installation { get; set; } = "";
        public virtual string MeterSerialNumber { get; set; } = "";
        public virtual string CriticalCareType { get; set; } = "";
        public virtual bool IsMajorAccount { get; set; }
        public virtual string MajorAccountManager { get; set; } = "";
        public virtual string AccountManagerContactNumber { get; set; } = "";
        public virtual string AccountManagerEmail { get; set; } = "";
        public virtual string PremiseType { get; set; } = "";
        public virtual string Pwsid { get; set; } = "";
    }
}
