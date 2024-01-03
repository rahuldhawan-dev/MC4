using System;
using System.Configuration;
using MapCall.Common.Model.Entities;
using MMSINC.Authentication;
using MMSINC.Utilities.Pdf;
using NHibernate;
using StructureMap;

namespace Contractors.Data.Models.Repositories
{
    public class ValveImageRepository : BaseAssetImageRepository<ValveImage>, IValveImageRepository
    {
        #region Properties

        // NOTE: These repositories aren't supposed to be secured apparently. 

        protected override string RelativeFileDirectory
        {
            get
            {
                var path = ConfigurationManager.AppSettings["ImageUploadValveDirectory"];
                if (string.IsNullOrWhiteSpace(path))
                {
                    throw new InvalidOperationException("The ImageUploadValveDirectory app setting must have a value.");
                }
                return path;
            }
        }

        #endregion

        #region Constructors

        #region Constructor

        public ValveImageRepository(ISession session, IAuthenticationService<ContractorUser> authenticationService, IContainer container, IImageToPdfConverter imageToPdfConverter) : base(session, authenticationService, container, imageToPdfConverter) { }

        #endregion

        #endregion

        #region Exposed Methods

        public override ValveImage Save(ValveImage entity)
        {
            throw new NotSupportedException("Contractors can't delete images.");
        }

        #endregion
    }
}
