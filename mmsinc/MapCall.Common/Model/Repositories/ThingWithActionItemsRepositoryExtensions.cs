using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;

namespace MapCall.Common.Model.Repositories
{
    public static class ThingWithActionItemsRepositoryExtensions
    {
        public static void DeleteAllActionItems<TThing, TRepository>(this TRepository that, ISession session,
            TThing entity)
            where TThing : IThingWithActionItems
            where TRepository : IRepository<TThing>
        {
            foreach (var actionItem in entity.LinkedActionItems)
            {
                session.Delete(actionItem.ActionItem);
            }
        }
    }
}
