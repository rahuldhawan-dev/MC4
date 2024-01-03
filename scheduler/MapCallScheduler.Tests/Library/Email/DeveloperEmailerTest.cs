using MapCallScheduler.Library.Email;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.NameValueCollectionExtensions;
using MMSINC.Interface;
using MMSINC.Utilities.ErrorHandling;
using Moq;
using StructureMap;
using System;
using System.Configuration;

namespace MapCallScheduler.Tests.Library.Email
{
    [TestClass]
    public class DeveloperEmailerTest
    {
        #region Private Members

        private Mock<IEmailGenerator> _generator;
        private Mock<ISmtpClient> _smtpClient;
        private Mock<IErrorMessageGenerator> _errorMessageGenerator;
        private IContainer _container;
        private DeveloperEmailer _target;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();
            _container.Inject((_generator = new Mock<IEmailGenerator>()).Object);
            _container.Inject((_smtpClient = new Mock<ISmtpClient>()).Object);
            _container.Inject((_errorMessageGenerator = new Mock<IErrorMessageGenerator>()).Object);

            _target = _container.GetInstance<DeveloperEmailer>();
        }

        #endregion

        [TestMethod]
        public void TestSendMessageSendsMessageToDevelopers()
        {
            var subject = "hey";
            var message = "sup";
            var useHtml = false;
            var email = new Mock<IMailMessage>();
            _generator
                .Setup(x => x.Generate(ConfigurationManager.AppSettings.EnsureValue("noreply_address"), DeveloperEmailer.EXTERMINATOR, subject, message, useHtml))
                .Returns(email.Object);

            _target.SendMessage(subject, message, useHtml);

            _smtpClient.Verify(x => x.Send(email.Object));
            _smtpClient.Verify(x => x.Dispose(), Times.Never());
            email.Verify(x => x.Dispose());
        }

        [TestMethod]
        public void TestSendErrorMessageSendsMessageToDevelopers()
        {
            var subject = "hey";
            var message = "sup";
            var exception = new Exception();
            var email = new Mock<IMailMessage>();
            _generator
                .Setup(x => x.Generate(ConfigurationManager.AppSettings.EnsureValue("noreply_address"), DeveloperEmailer.EXTERMINATOR, subject, message, true))
                .Returns(email.Object);
            _errorMessageGenerator.Setup(x => x.GenerateMessageBody(exception)).Returns(message);

            _target.SendErrorMessage(subject, exception);

            _smtpClient.Verify(x => x.Send(email.Object));
            _smtpClient.Verify(x => x.Dispose(), Times.Never());
            email.Verify(x => x.Dispose());
        }

        [TestMethod]
        public void TestDisposeDisposesOfSmtpClient()
        {
            _target.Dispose();

            _smtpClient.Verify(x => x.Dispose());
        }
    }
}
