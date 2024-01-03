using System.Collections.Generic;
using MapCall.Common.Metadata;

namespace MapCall.Common.Model.ViewModels
{
    public class ForbiddenRoleAccessModel
    {
        #region Properties

        public List<RequiresRoleAttribute> RequiredRoles { get; private set; }

        #endregion

        #region Constructor

        public ForbiddenRoleAccessModel()
        {
            RequiredRoles = new List<RequiresRoleAttribute>();
        }

        #endregion
    }
}
