using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Utilities;
using MMSINC.Utilities.Pdf;
using NHibernate;
using StructureMap;

namespace Contractors.Data.Models.Repositories
{
    public class TapImageRepository : BaseAssetImageRepository<TapImage>, ITapImageRepository
    {
        #region Properties

        // NOTE: These repositories aren't supposed to be secured apparently. 

        protected override string RelativeFileDirectory
        {
            get
            {
                var path = ConfigurationManager.AppSettings["ImageUploadTapDirectory"];
                if (string.IsNullOrWhiteSpace(path))
                {
                    throw new InvalidOperationException("The ImageUploadTapDirectory app setting must have a value.");
                }
                return path;
            }
        }

        #endregion

        #region Constructors

        public TapImageRepository(ISession session, IAuthenticationService<ContractorUser> authenticationService, IContainer container, IImageToPdfConverter imageToPdfConverter) : base(session, authenticationService, container, imageToPdfConverter) { }

        #endregion

        #region Private Methods

        private void SaveFile(IAssetImage entity)
        {
            // BIG STUPID NOTE:
            // This can ONLY be used for new records. Updating a record and changing
            // its town/state/whatever will not move the file.
            //
            // BIG STUPID NOTE 2:
            // The way file names work is an atrocious mess so we can't just rename
            // the file if there's one with the same name already. 
            //
            // BIG STUPID NOTE 3:
            // Related to note 2, the Directory value MUST end with a / or else the
            // image viewing pages load the wrong file location.

            entity.FileName.ThrowIfNullOrWhiteSpace("FileName");
            var path = new PathHelper(entity.FileName, RelativeFileDirectory, entity.Town);
            // This is for backwards compatibility with the way the data currently 
            // is in the database. 
            entity.Directory = path.RelativeDirectoryPath.Replace('\\', '/');

            FileIO.WriteAllBytes(path.AbsoluteFilePath, entity.ImageData);
        }

        #endregion

        #region Exposed Methods

        public override TapImage Save(TapImage entity)
        {
            // Only new records can have their images saved
            if (entity.Id == 0)
            {
                SaveFile(entity);
            }
            return base.Save(entity);
        }

        public IEnumerable<TapImage> GetTapImagesForWorkOrder(WorkOrder entity)
        {
            return from s in Linq
                   where s.PremiseNumber == entity.PremiseNumber && s.ServiceNumber == entity.ServiceNumber
                   select s;
        }

        #endregion
    }
}
