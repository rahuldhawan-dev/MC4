using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class AbbreviationTypeMap : EntityLookupMap<AbbreviationType>
    {
        #region Properties

        protected override string IdName => "AbbreviationTypeID";

        #endregion
    }
}
