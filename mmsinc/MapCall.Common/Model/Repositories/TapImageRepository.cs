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
    public interface ITapImageRepository : IAssetImageRepository<TapImage>
    {
        IEnumerable<TapImageLinkReportItem> GetTapImageLinks(ISearchSet<TapImageLinkReportItem> search);
    }

    public class TapImageRepository : BaseAssetImageRepository<TapImage>, ITapImageRepository
    {
        #region Properties

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

        #region Constructor

        public TapImageRepository(ISession session, IContainer container, IImageToPdfConverter imageToPdfConverter) :
            base(session, container, imageToPdfConverter) { }

        #endregion

        #region Public Methods

        public override TapImage Save(TapImage entity)
        {
            if (entity.IsDefaultImageForService && entity.Service != null)
            {
                foreach (var image in entity.Service.TapImages)
                {
                    if (image != entity)
                    {
                        image.IsDefaultImageForService = false;
                        // No need to call base.Save since calling Session.Flush
                        // automatically saves these changes.
                    }
                }
            }

            return base.Save(entity);
        }

        #endregion

        public IEnumerable<TapImageLinkReportItem> GetTapImageLinks(ISearchSet<TapImageLinkReportItem> search)
        {
            TapImageLinkReportItem result = null;
            OperatingCenter opc = null;

            var query = Session.QueryOver<TapImage>();
            query.JoinAlias(x => x.OperatingCenter, () => opc);
            query.Where(
                Restrictions.And(
                    Restrictions.Eq(
                        Projections.SqlFunction("length", NHibernateUtil.String,
                            Projections.Property<TapImage>(l => l.PremiseNumber)),
                        10),
                    !Restrictions.Eq("PremiseNumber", "0000000000")
                ));
            query.SelectList(x => x
                                 .Select(y => y.OperatingCenter).WithAlias(() => result.OperatingCenter)
                                 .Select(y => y.PremiseNumber).WithAlias(() => result.PremiseNumber)
                                 .Select(y => y.Id).WithAlias(() => result.Id));
            query
               .OrderBy(() => opc.OperatingCenterCode).Asc()
               .OrderBy(x => x.PremiseNumber).Asc();
            query.TransformUsing(Transformers.AliasToBean<TapImageLinkReportItem>());

            return Search(search, query);
        }
    }
}
