using System.Collections.Generic;

namespace MMSINC.Utilities.Permissions
{
    public interface ILookupCache
    {
        IDictionary<int, IRoleAction> Actions { get; }

        IDictionary<int, IRoleApplication> Applications { get; }
        IDictionary<string, IRoleApplication> ApplicationsByName { get; }

        IDictionary<int, IRoleModule> Modules { get; }

        IDictionary<int, IOperatingCenter> OperatingCenters { get; }
        IDictionary<string, IOperatingCenter> OperatingCentersByName { get; }
        void Reinitialize();
    }
}
