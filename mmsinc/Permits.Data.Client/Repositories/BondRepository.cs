using System;
using System.Diagnostics.CodeAnalysis;
using Permits.Data.Client.Entities;

namespace Permits.Data.Client.Repositories
{
    [Obsolete("There's no functional benefit to this class over the generic. Delete this when 271 is deleted.")]
    [ExcludeFromCodeCoverage]
    public class BondRepository : PermitsDataClientRepository<Bond>
    {
        public BondRepository(string userName) : base(userName) { }
    }
}
