namespace MapCall.Common.Model.Migrations.SeedData
{
    public abstract class SeedBase
    {
        #region Private Members

        private SeedData _seedData;

        #endregion

        #region Properties

        public SeedData SeedData
        {
            get { return _seedData; }
        }

        #endregion

        #region Private Methods

        internal void EnableIdentityInsert(string tableName)
        {
            _seedData.Execute.Sql(string.Format("SET IDENTITY_INSERT [{0}] ON", tableName));
        }

        internal void DisableIdentityInsert(string tableName)
        {
            _seedData.Execute.Sql(string.Format("SET IDENTITY_INSERT [{0}] OFF", tableName));
        }

        #endregion

        #region Constructors

        public SeedBase(SeedData seedData)
        {
            _seedData = seedData;
        }

        #endregion

        #region Exposed Methods

        public abstract void Up();

        #endregion
    }
}
