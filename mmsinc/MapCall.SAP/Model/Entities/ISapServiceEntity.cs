namespace MapCall.SAP.Model.Entities
{
    /// <summary>
    /// Represents common functionality among all the SAP service entity types
    /// used by an ISapServiceInvoker
    /// </summary>
    public interface ISAPServiceEntity
    {
        #region Properties

        string SAPErrorCode { get; set; }

        #endregion
    }
}
