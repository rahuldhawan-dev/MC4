using System;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Data.ChangeTracking;

namespace MapCall.Common.Model.Entities
{
    [Obsolete(
        "This only enables querying, to set the values automatically you need " +
        nameof(IEntityWithUpdateTracking<User>) + ".")]
    public interface IThingWithShadow : IEntityWithUpdateTracking<User> { }
}
