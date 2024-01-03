using System.Data.Linq;
using System.Linq;

namespace WorkOrders.Model
{
    /// <summary>
    /// Static class of special extension methods for functionality that could
    /// be included in a particular class in the model.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Finds and retrieves the most recent Markout object from a typed
        /// EntitySet of Markout objects.
        /// </summary>
        /// <param name="markouts">EntitySet of Markout objects to parse.
        /// </param>
        /// <returns>The most recent Markout, or null if the EntitySet is
        /// empty.</returns>
        public static Markout GetCurrent(this EntitySet<Markout> markouts)
        {
            return (markouts.Count > 0) ?
                markouts[markouts.Count - 1] :
                null;
        }

        public static Markout GetLast(this EntitySet<Markout> markouts)
        {
            return (markouts.Count > 1) ?
                markouts[markouts.Count - 2] :
                null;
        }

        public static StreetOpeningPermit GetCurrent(this EntitySet<StreetOpeningPermit> set)
        {
            return (from m in set 
                    orderby m.DateRequested descending
                    select m).FirstOrDefault();
        }
    }
}
