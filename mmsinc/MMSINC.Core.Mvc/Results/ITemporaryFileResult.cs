using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMSINC.Results
{
    /// <summary>
    /// Interface for results that produce temporary files on disk.
    /// </summary>
    public interface ITemporaryFileResult
    {
        void DeleteTemporaryFiles();
    }
}
