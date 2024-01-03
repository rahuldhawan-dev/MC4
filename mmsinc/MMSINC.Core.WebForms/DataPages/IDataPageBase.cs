using System;
using MMSINC.DataPages.Permissions;
using MMSINC.Interface;

namespace MMSINC.DataPages
{
    public interface IDataPageBase : IPage
    {
        #region Properties

        Guid CachedFilterKey { get; }
        bool IsReadOnlyPage { get; }
        IDataPagePermissions Permissions { get; }
        IDataPagePath PathHelper { get; }
        IDataPageRenderHelper RenderHelper { get; }
        int ResultCount { get; }

        #endregion
    }
}
