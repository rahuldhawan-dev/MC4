using FluentNHibernate.Mapping;

namespace MMSINC.Data.NHibernate
{
    /// <summary>
    /// Basic map models that inherit from EntityLookup. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class EntityLookupMap<T> : ClassMap<T> where T : ReadOnlyEntityLookup
    {
        public const int DEFAULT_DESCRIPTION_MAX_LENGTH = 50;

        public const string DEFAULT_DESCRIPTION_NAME = "Description",
                            DEFAULT_ID_NAME = "Id";

        protected virtual int DescriptionLength => DEFAULT_DESCRIPTION_MAX_LENGTH;

        protected virtual string DescriptionName => DEFAULT_DESCRIPTION_NAME;

        protected virtual string IdName => DEFAULT_ID_NAME;

        /// <summary>
        /// You will probably never need to use this.
        /// </summary>
        protected virtual bool IsDescriptionUnique => true;

        protected EntityLookupMap()
        {
            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            if (IdName != DEFAULT_ID_NAME)
            {
                Id(x => x.Id, IdName).Not.Nullable();
            }
            else
            {
                Id(x => x.Id).Not.Nullable();
            }

            var descPropPart = Map(x => x.Description)
                              .Length(DescriptionLength)
                              .Column(DescriptionName)
                               // ReSharper restore DoNotCallOverridableMethodsInConstructor
                              .Not.Nullable();

            if (IsDescriptionUnique)
            {
                descPropPart.Unique();
            }
        }
    }
}
