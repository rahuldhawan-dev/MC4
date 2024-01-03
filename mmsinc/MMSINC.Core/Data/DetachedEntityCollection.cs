using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMSINC.Data
{
    /// <summary>
    /// If you've got an instance of this then the items contained within
    /// are not full entity references and should absolutely not be used 
    /// like they are. 
    /// </summary>
    /// <remarks>
    /// 
    /// This is meant to be returned by repositories to indicate that the
    /// entities being returned are not fully loaded entities. Queries
    /// meant for dropdowns and similar do not need the entire entity just
    /// for display purposes.
    /// 
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    public class DetachedEntityCollection<T>
    {
        #region Properties

        public IList<T> Items { get; }

        #endregion

        #region Constructor

        public DetachedEntityCollection(IEnumerable<T> items)
        {
            // TODO: Could maybe have this throw an exception if it received NHibernate proxies
            Items = items.ToList();
        }

        #endregion
    }
}
