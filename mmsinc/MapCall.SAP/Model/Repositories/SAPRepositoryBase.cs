namespace MapCall.SAP.Model.Repositories
{
    public abstract class SAPRepositoryBase
    {
        #region Private Members

        protected readonly ISAPHttpClient _sapHttpClient;

        #endregion

        #region Properties

        public virtual ISAPHttpClient SAPHttpClient => _sapHttpClient;

        #endregion

        #region Constructors

        protected SAPRepositoryBase(ISAPHttpClient sapHttpClient)
        {
            _sapHttpClient = sapHttpClient;
        }

        #endregion
    }
}
