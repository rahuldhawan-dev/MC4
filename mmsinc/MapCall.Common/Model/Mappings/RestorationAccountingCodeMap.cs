using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class RestorationAccountingCodeMap : ClassMap<RestorationAccountingCode>
    {
        public RestorationAccountingCodeMap()
        {
            Id(x => x.Id, "RestorationAccountingCodeID");

            Map(x => x.Code).Length(RestorationAccountingCode.StringLengths.CODE).Not.Nullable();
            Map(x => x.SubCode).Length(RestorationAccountingCode.StringLengths.SUBCODE).Nullable();
        }
    }
}
