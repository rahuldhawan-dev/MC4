using System;
using MMSINC.ClassExtensions.ReflectionExtensions;
using MMSINC.Data;
using StructureMap;

namespace MMSINC.Testing.NHibernate
{
    public class EntityLookupTestDataFactory<TEntity> : TestDataFactory<TEntity>
        where TEntity : class, IEntityLookup, new()
    {
        private int _count;

        public EntityLookupTestDataFactory(IContainer container) : base(container)
        {
            Func<String> descriptionFunc = () => String.Format("{0}{1}", typeof(TEntity).Name, _count++);
            var registration =
                GetType(); //= callingType.IsSubclassOfRawGeneric(typeof(EntityLookupTestDataFactory<>)) ? callingType : GetType();

            if (!DefaultValues.ContainsKey(registration))
            {
                DefaultValues.Add(registration, new {
                    Description = descriptionFunc
                });
            }
        }
    }
}
