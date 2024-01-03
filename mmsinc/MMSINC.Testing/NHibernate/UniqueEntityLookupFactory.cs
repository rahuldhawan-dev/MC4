using System.Linq;
using MMSINC.Data;
using NHibernate.Linq;
using StructureMap;

namespace MMSINC.Testing.NHibernate
{
    /// <summary>
    /// Base factory for entities that inherit from EntityLookup and are required to have unique 
    /// descriptions.
    /// For when the Id and/or description values don't have specific meaning,
    /// if they do, use StaticListEntityLookupFactory.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class UniqueEntityLookupFactory<T> : TestDataFactory<T> where T : ReadOnlyEntityLookup, new()
    {
        private static int _number = 0;

        protected UniqueEntityLookupFactory(IContainer container) : base(container) { }

        public override T Build(object overrides = null)
        {
            var model = base.Build(overrides);
            if (model.Description == null)
            {
                _number++;
                model.Description = this.GetType().Name + " " + _number;
            }

            return model;
        }

        protected override T Save(T entity)
        {
            var existing = Session.Query<T>().SingleOrDefault(x => x.Description == entity.Description);
            return existing ?? base.Save(entity);
        }
    }
}
