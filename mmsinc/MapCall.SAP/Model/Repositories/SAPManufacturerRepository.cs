using MapCall.SAP.Model.Entities;

namespace MapCall.SAP.Model.Repositories
{
    public class SAPManufacturerRepository : SAPRepositoryBase, ISAPManufacturerRepository
    {
        #region Constants

        public const string ERROR_NO_SITE_CONNECTION =
                                "RETRY::CONNECTION FAILURE: No connection could be made to the site:",
                            SAP_NAMESPACE = "ManufacturerLookup",
                            SAP_INTERFACE = "http://amwater.com/EAM/0024/MAPCALL/ManufacturerLookup";

        #endregion

        #region Constructors

        public SAPManufacturerRepository(ISAPHttpClient sapHttpClient) : base(sapHttpClient) { }

        #endregion

        #region Exposed Methods

        public virtual SAPManufacturerCollection Search(SAPManufacturer entity)
        {
            SAPHttpClient.SAPInterface = SAP_NAMESPACE;
            SAPHttpClient.SAPInterfaceNamespace = SAP_INTERFACE;

            if (!SAPHttpClient.IsSiteRunning)
            {
                SAPManufacturer sapManufacturer = new SAPManufacturer();
                sapManufacturer.SAPErrorCode = ERROR_NO_SITE_CONNECTION;
                var sapManufacturerCollection = new SAPManufacturerCollection();
                sapManufacturerCollection.Items.Add(sapManufacturer);
                return sapManufacturerCollection;
            }

            return SAPHttpClient.GetManufacturer(entity);
        }

        #endregion
    }

    public interface ISAPManufacturerRepository
    {
        #region Abstract Methods

        SAPManufacturerCollection Search(SAPManufacturer entity);

        #endregion
    }
}
