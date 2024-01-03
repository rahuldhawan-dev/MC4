using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class RestorationProductCodeMap : EntityLookupMap<RestorationProductCode>
    {
        protected override string DescriptionName => "Code";

        protected override int DescriptionLength => RestorationProductCode.CODE_DESCRIPTION_LENGTH;

        protected override string IdName => "RestorationProductCodeID";
    }
}
