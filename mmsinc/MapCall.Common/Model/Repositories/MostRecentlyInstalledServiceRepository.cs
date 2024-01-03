using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.Transform;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class MostRecentlyInstalledServiceRepository
        : RepositoryBase<MostRecentlyInstalledService>, IMostRecentlyInstalledServiceRepository
    {
        public MostRecentlyInstalledServiceRepository(ISession session, IContainer container)
            : base(session, container) { }
        
        public IEnumerable<CurrentMaterialReportItem> GetCurrentMaterial(ISearchCurrentMaterial search)
        {
            CurrentMaterialReportItem result = null;
            MostRecentlyInstalledService mostRecent = null;
            Service service = null;
            Premise premise = null;
            OperatingCenter operatingCenter = null;
            Town town = null;
            ServiceMaterial material = null;
            ServiceSize size = null;
            ServiceMaterial custMaterial = null;
            ServiceSize custSize = null;
            ServiceCategory category = null;
            ServiceType type = null;
            Street street = null;
            ServiceInstallationPurpose purpose = null;

            var query = Session.QueryOver(() => mostRecent);
            query.Inner.JoinAlias(s => s.Service, () => service);
            query.Inner.JoinAlias(s => s.Premise, () => premise);
            query.Left.JoinAlias(s => s.ServiceMaterial, () => material);
            query.Left.JoinAlias(s => s.ServiceSize, () => size);
            query.Left.JoinAlias(s => s.CustomerSideMaterial, () => custMaterial);
            query.Left.JoinAlias(s => s.CustomerSideSize, () => custSize);
            query.Inner.JoinAlias(_ => service.OperatingCenter, () => operatingCenter);
            query.Inner.JoinAlias(_ => service.Town, () => town);
            query.Left.JoinAlias(_ => service.ServiceCategory, () => category);
            query.Left.JoinAlias(_ => service.ServiceType, () => type);
            query.Left.JoinAlias(_ => service.Street, () => street);
            query.Left.JoinAlias(_ => service.ServiceInstallationPurpose, () => purpose);

            query.SelectList(
                q => q
                    .Select(_ => service.Id).WithAlias(() => result.ServiceId)
                    .Select(_ => premise.Id).WithAlias(() => result.PremiseId)
                    .Select(_ => operatingCenter.OperatingCenterCode)
                    .WithAlias(() => result.OperatingCenterCode)
                    .Select(_ => operatingCenter.OperatingCenterName)
                    .WithAlias(() => result.OperatingCenterName)
                    .Select(_ => town.FullName).WithAlias(() => result.Town)
                    .Select(_ => premise.PremiseNumber).WithAlias(() => result.PremiseNumber)
                    .Select(_ => premise.Installation).WithAlias(() => result.InstallationNumber)
                    .Select(_ => material.Description).WithAlias(() => result.ServiceMaterial)
                    .Select(_ => custMaterial.Description).WithAlias(() => result.CustomerSideMaterial)
                    .Select(_ => size.ServiceSizeDescription).WithAlias(() => result.ServiceSize)
                    .Select(_ => custSize.ServiceSizeDescription).WithAlias(() => result.CustomerSideSize)
                    .Select(s => s.DateInstalled).WithAlias(() => result.InstallDate)
                    .Select(s => s.UpdatedAt).WithAlias(() => result.UpdatedAt)
                    .Select(_ => type.Description).WithAlias(() => result.ServiceType)
                    .Select(_ => category.Description).WithAlias(() => result.ServiceCategory)
                    .Select(_ => purpose.Description).WithAlias(() => result.PurposeOfInstallation)
                    .Select(_ => service.StreetNumber).WithAlias(() => result.StreetNumber)
                    .Select(_ => street.FullStName).WithAlias(() => result.Street));

            query.TransformUsing(Transformers.AliasToBean<CurrentMaterialReportItem>());

            return Search(search, query);
        }

        public IEnumerable<IThingWithCoordinate> SearchForMap(ISearchCurrentMaterialForMap search)
        {
            CurrentMaterialCoordinate result = null; 
            Service service = null;
            Premise premise = null;
            Coordinate coord = null;
            MapIcon icon = null;
            MapIconOffset offset = null;
            
            var query =
                Session
                   .QueryOver<MostRecentlyInstalledService>()
                    // need to join so that search aliases on the search model have this alias to refer to
                   .Inner.JoinAlias(x => x.Service, () => service)
                   .Inner.JoinAlias(x => x.Coordinate, () => coord)
                   .Inner.JoinAlias(x => x.Premise, () => premise)
                   .Left.JoinAlias(_ => coord.Icon, () => icon)
                   .Left.JoinAlias(_ => icon.Offset, () => offset)
                   .SelectList(
                        x =>
                            x.Select(p => p.Id).WithAlias(() => result.Id)
                             .Select(_ => coord.Latitude).WithAlias(() => result.Latitude)
                             .Select(_ => coord.Longitude).WithAlias(() => result.Longitude)
                             .Select(_ => icon.Id).WithAlias(() => result.MapIconId)
                             .Select(_ => icon.FileName).WithAlias(() => result.MapIconFileName)
                             .Select(_ => icon.Height).WithAlias(() => result.MapIconHeight)
                             .Select(_ => icon.Width).WithAlias(() => result.MapIconWidth)
                             .Select(_ => offset.Id).WithAlias(() => result.MapIconOffsetId)
                             .Select(_ => offset.Description).WithAlias(
                                  () => result.MapIconOffsetDescription))
                   .TransformUsing(Transformers.AliasToBean<CurrentMaterialCoordinate>());

            return Search(search, query);
        }
    }

    public interface IMostRecentlyInstalledServiceRepository : IRepository<MostRecentlyInstalledService>
    {
        IEnumerable<CurrentMaterialReportItem> GetCurrentMaterial(ISearchCurrentMaterial search);
        IEnumerable<IThingWithCoordinate> SearchForMap(ISearchCurrentMaterialForMap search);
    }
}
