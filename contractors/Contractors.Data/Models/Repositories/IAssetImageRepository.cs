using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace Contractors.Data.Models.Repositories {
    public interface IAssetImageRepository<TEntity> : IRepository<TEntity> where TEntity : class, IAssetImage
    {
        bool FileExists(string fileName, Town town);
        bool FileExists(TEntity entity);
        byte[] GetImageDataAsPdf(TEntity entity);
    }
}