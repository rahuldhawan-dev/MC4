using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class RestorationMethodMap : EntityLookupMap<RestorationMethod>
    {
        #region Properties

        protected override string IdName => "RestorationMethodID";

        #endregion

        #region Constructors

        public RestorationMethodMap()
        {
            HasManyToMany(x => x.RestorationTypes)
               .Cascade.SaveUpdate()
               .Table("RestorationMethodsRestorationTypes")
               .ParentKeyColumn("RestorationMethodID")
               .ChildKeyColumn("RestorationTypeID");
        }

        #endregion
    }
}
