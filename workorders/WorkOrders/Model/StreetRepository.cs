using System.Collections.Generic;
using System.Linq;

namespace WorkOrders.Model
{
    /// <summary>
    /// Repository for retrieving Street objects from persistence.
    /// </summary>
    public class StreetRepository : WorkOrdersRepository<Street>
    {
        #region Exposed Methods

        /// <summary>
        /// Retrieves all Street objects from persistence.
        /// </summary>
        /// <returns>
        /// A list of all Street objects available.
        /// </returns>
        public static new List<Street> SelectAllAsList()
        {
            return SelectAllAsList(0);
        }

        /// <summary>
        /// Retrieves all Street objects from persistence, limited to a count.
        /// </summary>
        /// <param name="count">
        /// The maximum number of Street objects to bring back.  If zero or
        /// less, all available Street objects will be returned.
        /// </param>
        /// <returns>
        /// A list of Street objects.
        /// </returns>
        public static List<Street> SelectAllAsList(int count)
        {
            if (count <= 0)
                return
                    (from s in DataTable orderby s.StreetName select s).ToList();
            return
                (from s in DataTable orderby s.StreetName select s).Take(count).
                    ToList();
        }

        /// <summary>
        /// Retrieves the Street object from persistence which has the given
        /// name and belongs to the given town (searched by its name).
        /// </summary>
        /// <param name="streetName">
        /// The name of the street to search for.
        /// </param>
        /// <param name="townName">
        /// The name of the town to which the desired street belongs.
        /// </param>
        /// <returns>
        /// The street object with the given name which belongs to the given
        /// town, or null if none found.
        /// </returns>
        public static Street GetStreetByNameAndTownID(string streetName, int townID)
        {
            return (from s in DataTable
                    where (s.TownID == townID
                           && s.StreetName == streetName)
                    select s).FirstOrDefault();

        }

        /// <summary>
        /// Retrieves all of the Street objects from persistence which belong
        /// to the given town.
        /// </summary>
        /// <param name="townName">
        /// Name of the town within which all searched Street objects should
        /// belong.
        /// </param>
        /// <returns>A List of all the Street objects found.</returns>
        public static List<Street> SelectByTownID(int townID)
        {
            return (from s in DataTable
                    orderby s.StreetName
                    where s.TownID == townID
                    select s).ToList();
        }

        #endregion
    }
}