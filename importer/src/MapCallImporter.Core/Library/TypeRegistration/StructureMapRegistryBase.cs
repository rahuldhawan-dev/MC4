using System.Web.Mvc;
using MapCall.Common.Model.Entities.Users;
using MapCallImporter.Library.Excel;
using MMSINC.Authentication;
using MMSINC.Data.V2;
using MMSINC.Data.V2.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using StructureMap;

namespace MapCallImporter.Library.TypeRegistration
{
    public abstract class StructureMapRegistryBase : Registry
    {
        #region Constructors

        public StructureMapRegistryBase()
        {
            For(typeof(IRepository<>)).Use(typeof(Repository<>));
            For(typeof(MMSINC.Data.NHibernate.IRepository<>)).Use(typeof(MMSINC.Data.NHibernate.RepositoryBase<>));
            For<IObjectMapperFactory>().Use<ObjectMapperFactory>();
            For<ModelMetadataProvider>().Use<CustomModelMetadataProvider>();
            For<IAuthenticationService<User>>().Use<DangerousAuthenticationService>();
            For<IAuthenticationService>().Use(ctx => ctx.GetInstance<IAuthenticationService>());
            For<IExcelImportFactory>().Use<ExcelImportFactory>();
        }

        #endregion
    }
}
