using System;
using System.Configuration;
using MapCall.Common.Model.Entities;
using MMSINC.Authentication;
using MMSINC.Utilities.Pdf;
using NHibernate;
using StructureMap;

namespace Contractors.Data.Models.Repositories
{
    public class AsBuiltImageRepository : BaseAssetImageRepository<AsBuiltImage>, IAsBuiltImageRepository
    {
        #region Properties

        // NOTE: These repositories aren't supposed to be secured apparently. 

        protected override string RelativeFileDirectory
        {
            get
            {
                var path = ConfigurationManager.AppSettings["ImageUploadAsBuiltDirectory"];
                if (string.IsNullOrWhiteSpace(path))
                {
                    throw new InvalidOperationException("The ImageUploadAsBuiltDirectory app setting must have a value.");
                }
                return path;
            }
        }

        #endregion

        #region Constructors

        public AsBuiltImageRepository(ISession session, IAuthenticationService<ContractorUser> authenticationService, IContainer container, IImageToPdfConverter imageToPdfConverter) : base(session, authenticationService, container, imageToPdfConverter) { }

        #endregion

        #region Exposed Methods

        public override AsBuiltImage Save(AsBuiltImage entities)
        {
            throw new NotSupportedException("Contractors can't delete images.");
        }

        #endregion
    }
}
