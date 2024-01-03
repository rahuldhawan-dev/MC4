using MMSINC.Data;

namespace MapCallScheduler.JobHelpers.GIS.Models
{
    public abstract class EntityLookupBase<TThing>
        where TThing : EntityLookupBase<TThing>, new()
    {
        #region Properties

        public int Id { get; set; }
        public string Description { get; set; }

        #endregion

        #region Exposed Methods

        public static TThing FromDbRecord(IEntityLookup record)
        {
            return record == null
                ? null
                : new TThing {
                    Id = record.Id,
                    Description = record.Description
                };
        }

        #endregion
    }
}
