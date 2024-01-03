using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations._2017;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class UnitOfWaterSampleMeasureMap : EntityLookupMap<UnitOfWaterSampleMeasure>
    {
        public const string TABLE_NAME = AddUnitOfMeasureToWaterSamplesForBug3670.TABLE_NAME;

        public UnitOfWaterSampleMeasureMap()
        {
            Table(TABLE_NAME);
        }
    }
}
