using MapCall.SAP.Model.Entities;

namespace MapCall.SAP.Model.Repositories
{
    public class SAPEquipmentRepository : SAPRepositoryBase, ISAPEquipmentRepository
    {
        #region Constants

        public const string ERROR_NO_SITE_CONNECTION =
                                "RETRY::CONNECTION FAILURE: No connection could be made to the site:",
                            SAP_NAMESPACE = "CreateUpdate_Equipments",
                            SAP_INTERFACE = "http://amwater.com/EAM/0006/MAPCALL/CreateUpdate_Equipment";

        #endregion

        #region Constructors

        public SAPEquipmentRepository(ISAPHttpClient sapHttpClient) : base(sapHttpClient) { }

        #endregion

        #region Exposed Methods

        public virtual SAPEquipment Save(SAPEquipment entity)
        {
            SAPHttpClient.SAPInterface = SAP_NAMESPACE;
            SAPHttpClient.SAPInterfaceNamespace = SAP_INTERFACE;

            if (!SAPHttpClient.IsSiteRunning)
            {
                entity.SAPErrorCode = ERROR_NO_SITE_CONNECTION;
                return entity;
            }

            return SAPHttpClient.GetEquipmentResponse(entity);
        }

        #endregion
    }

    public interface ISAPEquipmentRepository
    {
        #region Abstract Methods

        SAPEquipment Save(SAPEquipment entity);

        #endregion
    }
}
