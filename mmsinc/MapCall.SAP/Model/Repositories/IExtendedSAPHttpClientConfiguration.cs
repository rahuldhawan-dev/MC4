using System;

namespace MapCall.SAP.Model.Repositories
{
    public interface IExtendedSAPHttpClientConfiguration
    {
        string UserName { get; }
        string Password { get; }
        Uri BaseAddress { get; }
    }
}
