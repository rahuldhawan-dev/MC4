using System;
using System.Configuration;
using MapCall.Common.Model.Entities;
using MMSINC.Utilities.Pdf;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public interface IAsBuiltImageRepository : IAssetImageRepository<AsBuiltImage> { }

    public class AsBuiltImageRepository : BaseAssetImageRepository<AsBuiltImage>, IAsBuiltImageRepository
    {
        #region Properties

        protected override string RelativeFileDirectory
        {
            get
            {
                var path = ConfigurationManager.AppSettings["ImageUploadAsBuiltDirectory"];
                if (string.IsNullOrWhiteSpace(path))
                {
                    throw new InvalidOperationException(
                        "The ImageUploadAsBuiltDirectory app setting must have a value.");
                }

                return path;
            }
        }

        #endregion

        #region Constructor

        public AsBuiltImageRepository(ISession session, IContainer container, IImageToPdfConverter imageToPdfConverter)
            : base(session, container, imageToPdfConverter) { }

        #endregion
    }
}
