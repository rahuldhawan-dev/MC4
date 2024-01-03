using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMSINC.Utilities.Pdf
{
    /// <summary>
    /// Base class for doing stuff with EvoPdf.
    /// </summary>
    public abstract class BasePdfConverter
    {
        #region Properties

        /// <summary>
        /// Gets the EvoPdf license key that needs to be set on things.
        /// </summary>
        internal string License
        {
            get
            {
                // We only need the one license, I see no reason to move this out to individual app/web.configs.
                // -Ross 1/17/2014
                return "6Wd0ZnN2ZnR+d2ZxaHZmdXdod3Rof39/f2Z2";
            }
        }

        #endregion
    }
}
