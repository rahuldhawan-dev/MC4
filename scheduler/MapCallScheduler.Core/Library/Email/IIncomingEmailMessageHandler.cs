using System;
using MMSINC.Interface;

namespace MapCallScheduler.Library.Email
{
    public interface IIncomingEmailMessageHandler : IDisposable
    {
        #region Abstract Properties

        bool MakeChanges { get; }

        #endregion

        #region Abstract Methods

        void Handle(uint uid, string mailbox, IMailMessage message);

        #endregion
    }
}