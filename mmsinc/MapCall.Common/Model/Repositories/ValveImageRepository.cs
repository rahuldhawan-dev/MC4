using System;
using System.Collections.Generic;
using System.Configuration;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Utilities.Pdf;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public interface IValveImageRepository : IAssetImageRepository<ValveImage>
    {
        IEnumerable<ValveImageLinkReportItem> GetValveImageLinks(ISearchSet<ValveImageLinkReportItem> search);
    }

    public class ValveImageRepository : BaseAssetImageRepository<ValveImage>, IValveImageRepository
    {
        #region Properties

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

        #region Constructor

        public ValveImageRepository(ISession session, IContainer container, IImageToPdfConverter imageToPdfConverter) :
            base(session, container, imageToPdfConverter) { }

        #endregion

        #region Public Methods

        public override ValveImage Save(ValveImage entity)
        {
            if (entity.IsDefaultImageForValve && entity.Valve != null)
            {
                foreach (var image in entity.Valve.ValveImages)
                {
                    if (image != entity)
                    {
                        image.IsDefaultImageForValve = false;
                        // No need to call base.Save here since 
                        // the single save below will save all this.
                    }
                }
            }

            return base.Save(entity);
        }

        public IEnumerable<ValveImageLinkReportItem> GetValveImageLinks(ISearchSet<ValveImageLinkReportItem> search)
        {
            ValveImageLinkReportItem result = null;
            OperatingCenter opc = null;
            Valve valve = null;

            var query = Session.QueryOver<ValveImage>();
            query
               .JoinAlias(x => x.OperatingCenter, () => opc)
               .JoinAlias(x => x.Valve, () => valve);
            query
               .Where(x => x.OperatingCenter != null)
               .Where(x => x.Valve != null)
               .Where(x => valve.SAPEquipmentId != null)
               .Where(x => valve.SAPEquipmentId != 0);
            query
               .SelectList(x => x
                               .Select(y => y.OperatingCenter).WithAlias(() => result.OperatingCenter)
                               .Select(y => y.Valve).WithAlias(() => result.Valve)
                               .Select(y => valve.SAPEquipmentId).WithAlias(() => result.SAPEquipmentId)
                               .Select(y => y.Id).WithAlias(() => result.ValveImageId)
                );
            query
               .OrderBy(() => opc.OperatingCenterCode).Asc()
               .OrderBy(() => valve.ValveSuffix).Asc();
            query
               .TransformUsing(Transformers.AliasToBean<ValveImageLinkReportItem>());

            return Search(search, query);
        }

        #endregion
    }
}
