using System.Text;
using MMSINC.Common;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities.ErrorHandling;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace MMSINC.CoreTest.Utilities.ErrorHandling
{
    /// <summary>
    /// Summary description for ErrorEmailerTest.
    /// </summary>
    [TestClass]
    public class ErrorEmailerTest : EventFiringTestClass
    {
        #region Private Members

        private TestErrorEmailer _target;
        private IErrorMessageGenerator _messageGenerator;
        private ISmtpClient _smtpClient;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void ErrorEmailerTestInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
               .DynamicMock(out _messageGenerator)
               .DynamicMock(out _smtpClient);

            _target = new TestErrorEmailerBuilder()
                     .WithMessageGenerator(_messageGenerator)
                     .WithSmtpClient(_smtpClient)
                     .Build();
        }

        [TestCleanup]
        public void ErrorEmailerTestCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        [TestMethod]
        public void TestConstructorSetsErrorMessageGenerator()
        {
            _target = new TestErrorEmailer();

            Assert.IsInstanceOfType(_target.MessageGenerator, typeof(ErrorMessageGenerator));

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestSendEmailSendsGeneratedMessageUsingSmtpClientAndDisposesOfIt()
        {
            var ctx = _mocks.DynamicMock<IHttpContext>();
            var message = _mocks.DynamicMock<IMailMessage>();

            using (_mocks.Record())
            {
                SetupResult.For(message.Body).Return(string.Empty);
                SetupResult
                   .For(_messageGenerator.GenerateMailMessage(ctx))
                   .Return(message);
                _smtpClient.Send(message);
                message.Dispose();
            }

            using (_mocks.Playback())
            {
                _target.SendEmail(ctx);
            }
        }

        [TestMethod]
        public void TestSendEmailDoesNotSendIfExcludedMessageFound()
        {
            var ctx = _mocks.DynamicMock<IHttpContext>();
            var message = _mocks.DynamicMock<IMailMessage>();
            var mailMessage = new StringBuilder();

            using (_mocks.Record())
            {
                foreach (var exception in ErrorEmailer.Exceptions)
                {
                    mailMessage.Append(exception);
                }

                SetupResult.For(message.Body).Return(mailMessage.ToString());
                SetupResult
                   .For(_messageGenerator.GenerateMailMessage(ctx))
                   .Return(message);
                _smtpClient.Expect(x => x.Send(message)).Repeat.Never();
                ;
                message.Dispose();
            }

            using (_mocks.Playback())
            {
                _target.SendEmail(ctx);
            }
        }
    }

    internal class TestErrorEmailerBuilder : TestDataBuilder<TestErrorEmailer>
    {
        #region Private Members

        private IErrorMessageGenerator _messageGenerator;
        private ISmtpClient _smtpClient;

        #endregion

        #region Exposed Methods

        public override TestErrorEmailer Build()
        {
            var obj = new TestErrorEmailer();
            if (_messageGenerator != null)
                obj.MessageGenerator = _messageGenerator;
            if (_smtpClient != null)
                obj.SmtpClient = _smtpClient;
            return obj;
        }

        public TestErrorEmailerBuilder WithMessageGenerator(IErrorMessageGenerator generator)
        {
            _messageGenerator = generator;
            return this;
        }

        public TestErrorEmailerBuilder WithSmtpClient(ISmtpClient client)
        {
            _smtpClient = client;
            return this;
        }

        #endregion
    }

    internal class TestErrorEmailer : ErrorEmailer
    {
        #region Properties

        public IErrorMessageGenerator MessageGenerator
        {
            get { return _messageGenerator; }
            set { _messageGenerator = value; }
        }

        public ISmtpClient SmtpClient { get; set; }

        #endregion

        protected override ISmtpClient CreateSmtpClient()
        {
            return SmtpClient;
        }
    }
}
