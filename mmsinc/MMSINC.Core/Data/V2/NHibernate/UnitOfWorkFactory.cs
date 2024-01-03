using System.Web.Mvc;
using MMSINC.DesignPatterns;
using MMSINC.Utilities.StructureMap;
using MMSINC.Validation;
using NHibernate;
using StructureMap;

namespace MMSINC.Data.V2.NHibernate
{
    public class UnitOfWorkFactory : FactoryBase, IUnitOfWorkFactory
    {
        public const string CONTAINER_PROFILE = nameof(UnitOfWork);

        #region Constructors

        public UnitOfWorkFactory(IContainer container) : base(container) { }

        #endregion

        #region Private Methods

        protected virtual IUnitOfWork BuildInstance<TInstance>()
            where TInstance : UnitOfWork
        {
            var container = _container.GetNestedContainer(CONTAINER_PROFILE);
            var uow = container.GetInstance<TInstance>();

            DataAnnotationsModelValidatorProvider.RegisterAdapterFactory(typeof(EntityMustExistAttribute),
                (metadata, context, attribute) =>
                    container.With(metadata).With(context).With(attribute as EntityMustExistAttribute)
                             .GetInstance<EntityMustExistAttributeAdapter>());
            DependencyResolver.SetResolver(new StructureMapDependencyResolver(container));

            return uow;
        }

        #endregion

        #region Exposed Methods

        public virtual IUnitOfWork Build()
        {
            return BuildInstance<UnitOfWork>();
        }

        public virtual IUnitOfWork BuildMemoized()
        {
            return BuildInstance<MemoizedUnitOfWork>();
        }

        #endregion
    }
}
