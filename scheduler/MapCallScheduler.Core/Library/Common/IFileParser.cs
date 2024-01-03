using System.Collections.Generic;

namespace MapCallScheduler.Library.Common
{
    public interface IFileParser<out TFileRecord>
        where TFileRecord : new()
    {
        #region Abstract Methods

        IEnumerable<TFileRecord> Parse(FileData data);

        #endregion
    }
}