using System.Linq;
using MapCall.Common.Model.Entities;
using Newtonsoft.Json;

namespace MapCallScheduler.JobHelpers.GIS
{
    public interface IGISFileSerializer
    {
        string Serialize(IQueryable<Hydrant> coll, Formatting formatting = Formatting.None);
        string Serialize(IQueryable<Valve> coll, Formatting formatting = Formatting.None);
        string Serialize(IQueryable<SewerOpening> coll, Formatting formatting = Formatting.None);
        string Serialize(IQueryable<MostRecentlyInstalledService> coll, Formatting formatting = Formatting.None);
        string Serialize(IQueryable<AsBuiltImage> coll, Formatting formatting = Formatting.None);
    }
}