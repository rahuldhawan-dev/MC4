using System;
using System.Collections.Specialized;
using System.Net.Mail;
using System.Web;
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
    /// Summary description for ErrorMessageGeneratorTest.
    /// </summary>
    [TestClass, DoNotParallelize]
    public class ErrorMessageGeneratorTest : EventFiringTestClass
    {
        #region Private Members

        private TestErrorMessageGenerator _target;
        private IParameterCollectionFormatter _parameterCollectionFormatter;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void ErrorMessageGeneratorTestInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks.DynamicMock(out _parameterCollectionFormatter);

            _target = new TestErrorMessageGeneratorBuilder()
               .WithParameterFormatter(_parameterCollectionFormatter);
        }

        [TestCleanup]
        public void ErrorMessageGeneratorTestCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        [TestMethod]
        public void TestConstructorCreatesParameterFormatter()
        {
            _target = new TestErrorMessageGenerator();

            Assert.IsInstanceOfType(_target.ParameterFormatter, typeof(ParameterCollectionFormatter));

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestGenerateMessageBodyGeneratesMessageForExceptionWithHttpContext()
        {
            var message = "This is the message";
            var currentPage = "SomePage.aspx";
            var currentUserName = "someUser";
            var parameters = "These are the parameters";
            string generatedMessage;
            Exception thrownException;
            var ctx = _mocks.DynamicMock<IHttpContext>();
            var server = _mocks.DynamicMock<IServer>();
            var user = _mocks.DynamicMock<IUser>();
            var request = _mocks.DynamicMock<IRequest>();

            using (_mocks.Record())
            {
                SetupResult.For(ctx.Server).Return(server);
                SetupResult.For(ctx.User).Return(user);
                SetupResult.For(ctx.Request).Return(request);
                SetupResult.For(user.Name).Return(currentUserName);
                SetupResult.For(request.RawUrl).Return(currentPage);
                SetupResult.For(
                    _parameterCollectionFormatter.FormatParameters(request,
                        ErrorMessageGenerator.INDENT_STRING,
                        ErrorMessageGenerator.LINE_SEPARATOR,
                        ErrorMessageGenerator.KEYS_TO_SKIP)).Return(parameters);

                try
                {
                    throw new Exception(message);
                }
                catch (Exception ex)
                {
                    SetupResult.For(server.GetLastError()).Return(ex);
                    thrownException = ex;
                }
            }

            using (_mocks.Playback())
            {
                generatedMessage = _target.GenerateMessageBody(ctx);
            }

            Assert.AreEqual(
                String.Format(
                    ErrorMessageGenerator.FormatStrings.FIRST_EXCEPTION_HTTP,
                    currentUserName, thrownException.GetType(),
                    thrownException.Message, currentPage, parameters,
                    thrownException.StackTrace.Replace("\n", "<br/>")),
                generatedMessage);
        }

        [TestMethod]
        public void TestGenerateMessageBodyGeneratesMessageForExceptionWithHttpContextAndInnerException()
        {
            var outerMessage = "This is the outer message";
            var innerMessage = "This is the inner message";
            var currentPage = "SomePage.aspx";
            var currentUserName = "someUser";
            var parameters = "these are the parameters";
            string generatedMessage;
            Exception outerException, innerException;
            var ctx = _mocks.DynamicMock<IHttpContext>();
            var server = _mocks.DynamicMock<IServer>();
            var user = _mocks.DynamicMock<IUser>();
            var request = _mocks.DynamicMock<IRequest>();

            using (_mocks.Record())
            {
                SetupResult.For(ctx.Server).Return(server);
                SetupResult.For(ctx.User).Return(user);
                SetupResult.For(ctx.Request).Return(request);
                SetupResult.For(user.Name).Return(currentUserName);
                SetupResult.For(request.RawUrl).Return(currentPage);
                SetupResult.For(
                    _parameterCollectionFormatter.FormatParameters(request,
                        ErrorMessageGenerator.INDENT_STRING,
                        ErrorMessageGenerator.LINE_SEPARATOR,
                        ErrorMessageGenerator.KEYS_TO_SKIP)).Return(parameters);

                try
                {
                    throw new Exception(outerMessage,
                        new Exception(innerMessage));
                }
                catch (Exception ex)
                {
                    SetupResult.For(server.GetLastError()).Return(ex);
                    outerException = ex;
                    innerException = ex.InnerException;
                }
            }

            using (_mocks.Playback())
            {
                generatedMessage = _target.GenerateMessageBody(ctx);
            }

            Assert.AreEqual(
                String.Format(
                    ErrorMessageGenerator.FormatStrings.FIRST_EXCEPTION_HTTP,
                    currentUserName,
                    outerException.GetType(), outerException.Message,
                    currentPage, parameters,
                    outerException.StackTrace.Replace("\n", "<br/>")) +
                String.Format(
                    ErrorMessageGenerator.FormatStrings.INNER_EXCEPTION,
                    innerException.GetType(), innerException.Message,
                    (innerException.StackTrace ?? String.Empty).Replace("\n",
                        "<br/>")),
                generatedMessage);
        }

        [TestMethod]
        public void TestGenerateMessageBodyGeneratesMessageForExceptionWithHttpContextAndMultipleInnerExceptions()
        {
            var firstMessage = "This is the first message";
            var secondMessage = "This is the second message";
            var thirdMessage = "This is the third message";
            var currentPage = "SomePage.aspx";
            var currentUserName = "someUser";
            var parameters = "these are the parameters";
            string generatedMessage;
            Exception firstException, secondException, thirdException;
            var ctx = _mocks.DynamicMock<IHttpContext>();
            var server = _mocks.DynamicMock<IServer>();
            var user = _mocks.DynamicMock<IUser>();
            var request = _mocks.DynamicMock<IRequest>();

            using (_mocks.Record())
            {
                SetupResult.For(ctx.Server).Return(server);
                SetupResult.For(ctx.User).Return(user);
                SetupResult.For(ctx.Request).Return(request);
                SetupResult.For(user.Name).Return(currentUserName);
                SetupResult.For(request.RawUrl).Return(currentPage);
                SetupResult.For(
                    _parameterCollectionFormatter.FormatParameters(request,
                        ErrorMessageGenerator.INDENT_STRING,
                        ErrorMessageGenerator.LINE_SEPARATOR,
                        ErrorMessageGenerator.KEYS_TO_SKIP)).Return(parameters);

                try
                {
                    throw new Exception(firstMessage,
                        new Exception(secondMessage,
                            new Exception(thirdMessage)));
                }
                catch (Exception ex)
                {
                    SetupResult.For(server.GetLastError()).Return(ex);
                    firstException = ex;
                    secondException = ex.InnerException;
                    thirdException = secondException.InnerException;
                }
            }

            using (_mocks.Playback())
            {
                generatedMessage = _target.GenerateMessageBody(ctx);
            }

            Assert.AreEqual(
                String.Format(
                    ErrorMessageGenerator.FormatStrings.FIRST_EXCEPTION_HTTP,
                    currentUserName,
                    firstException.GetType(), firstException.Message,
                    currentPage, parameters,
                    firstException.StackTrace.Replace("\n", "<br/>")) +
                String.Format(
                    ErrorMessageGenerator.FormatStrings.INNER_EXCEPTION,
                    secondException.GetType(), secondException.Message,
                    (secondException.StackTrace ?? String.Empty).Replace("\n",
                        "<br/>")) +
                String.Format(
                    ErrorMessageGenerator.FormatStrings.INNER_EXCEPTION,
                    thirdException.GetType(), thirdException.Message,
                    (thirdException.StackTrace ?? String.Empty).Replace(
                        "\n", "<br/>")),
                generatedMessage);
        }

        [TestMethod]
        public void TestGenerateMessageBodyGeneratesMessageForException()
        {
            var message = "This is the message";
            string generatedMessage;
            Exception thrownException;

            try
            {
                throw new Exception(message);
            }
            catch (Exception e)
            {
                thrownException = e;
            }

            using (_mocks.Record()) { }

            using (_mocks.Playback())
            {
                generatedMessage = _target.GenerateMessageBody(thrownException);
            }

            Assert.AreEqual(
                String.Format(
                    ErrorMessageGenerator.FormatStrings.FIRST_EXCEPTION,
                    thrownException.GetType(),
                    thrownException.Message, thrownException.StackTrace.Replace("\n", "<br/>")),
                generatedMessage);
        }

        [TestMethod]
        public void TestGenerateMessageBodyGeneratesMessageForExceptionWithInnerException()
        {
            var outerMessage = "This is the outer message";
            var innerMessage = "This is the inner message";
            string generatedMessage;
            Exception outerException, innerException;

            try
            {
                throw new Exception(outerMessage,
                    new Exception(innerMessage));
            }
            catch (Exception ex)
            {
                outerException = ex;
                innerException = ex.InnerException;
            }

            using (_mocks.Record()) { }

            using (_mocks.Playback())
            {
                generatedMessage = _target.GenerateMessageBody(outerException);
            }

            Assert.AreEqual(
                String.Format(
                    ErrorMessageGenerator.FormatStrings.FIRST_EXCEPTION,
                    outerException.GetType(),
                    outerException.Message, outerException.StackTrace.Replace("\n", "<br/>")) +
                String.Format(
                    ErrorMessageGenerator.FormatStrings.INNER_EXCEPTION,
                    innerException.GetType(), innerException.Message,
                    (innerException.StackTrace ?? String.Empty).Replace("\n",
                        "<br/>")),
                generatedMessage);
        }

        [TestMethod]
        public void
            TestGenerateMessageBodyGeneratesMessageForExceptionWithInerrExceptionAndSkipsHttpUnhandledException()
        {
            var outerMessage = "This is the outer message";
            var innerMessage = "This is the inner message";
            var currentPage = "SomePage.aspx";
            var currentUserName = "someUser";
            var parameters = "these are the parameters";
            string generatedMessage;
            HttpUnhandledException outerException;
            Exception innerException;
            var ctx = _mocks.DynamicMock<IHttpContext>();
            var server = _mocks.DynamicMock<IServer>();
            var user = _mocks.DynamicMock<IUser>();
            var request = _mocks.DynamicMock<IRequest>();

            using (_mocks.Record())
            {
                SetupResult.For(ctx.Server).Return(server);
                SetupResult.For(ctx.User).Return(user);
                SetupResult.For(ctx.Request).Return(request);
                SetupResult.For(user.Name).Return(currentUserName);
                SetupResult.For(request.RawUrl).Return(currentPage);
                SetupResult.For(
                    _parameterCollectionFormatter.FormatParameters(request,
                        ErrorMessageGenerator.INDENT_STRING,
                        ErrorMessageGenerator.LINE_SEPARATOR,
                        ErrorMessageGenerator.KEYS_TO_SKIP)).Return(parameters);

                try
                {
                    throw new HttpUnhandledException(outerMessage,
                        new Exception(innerMessage));
                }
                catch (HttpUnhandledException ex)
                {
                    SetupResult.For(server.GetLastError()).Return(ex);
                    outerException = ex;
                    innerException = ex.InnerException;
                }
            }

            using (_mocks.Playback())
            {
                generatedMessage = _target.GenerateMessageBody(ctx);
            }

            var expected = String.Format(
                ErrorMessageGenerator.FormatStrings.FIRST_EXCEPTION_HTTP,
                currentUserName,
                innerException.GetType(), innerException.Message,
                currentPage, parameters,
                (innerException.StackTrace ?? String.Empty).Replace("\n",
                    "<br/>"));

            Assert.AreEqual(expected, generatedMessage);
        }

        [TestMethod]
        public void TestGenerateMailMessageCreatesMailMessageObjectFromArguments()
        {
            var message = "This is the message";
            var currentPage = "SomePage.aspx";
            var currentUserName = "someUser";
            var parameters = "These are the parameters";
            var serverName = "www.someserver.com";
            var serverVariables = new NameValueCollection {
                {"SERVER_NAME", serverName}
            };
            var shortName = serverName.Replace("www.", "");
            string generatedMessage;
            IMailMessage mailMessage;
            var ctx = _mocks.DynamicMock<IHttpContext>();
            var server = _mocks.DynamicMock<IServer>();
            var user = _mocks.DynamicMock<IUser>();
            var request = _mocks.DynamicMock<IRequest>();

            using (_mocks.Record())
            {
                SetupResult.For(ctx.Server).Return(server);
                SetupResult.For(ctx.User).Return(user);
                SetupResult.For(ctx.Request).Return(request);
                SetupResult.For(user.Name).Return(currentUserName);
                SetupResult.For(request.RawUrl).Return(currentPage);
                SetupResult.For(request.ServerVariables).Return(serverVariables);
                SetupResult.For(
                    _parameterCollectionFormatter.FormatParameters(request,
                        ErrorMessageGenerator.INDENT_STRING,
                        ErrorMessageGenerator.LINE_SEPARATOR,
                        ErrorMessageGenerator.KEYS_TO_SKIP)).Return(parameters);

                try
                {
                    throw new Exception(message);
                }
                catch (Exception ex)
                {
                    SetupResult.For(server.GetLastError()).Return(ex);
                }
            }

            using (_mocks.Playback())
            {
                generatedMessage = _target.GenerateMessageBody(ctx);
                mailMessage = _target.GenerateMailMessage(ctx);
            }

            Assert.AreEqual(
                String.Format(ErrorMessageGenerator.FormatStrings.SENDER,
                    shortName), mailMessage.From.ToString());
            Assert.AreEqual(ErrorMessageGenerator.RECIPIENT,
                mailMessage.To.ToString());
            Assert.IsTrue(mailMessage.IsBodyHtml);
            Assert.AreEqual(
                String.Format(ErrorMessageGenerator.FormatStrings.SUBJECT,
                    shortName), mailMessage.Subject);
            Assert.AreEqual(MailPriority.High, mailMessage.Priority);
            Assert.AreEqual(generatedMessage, mailMessage.Body);
        }
    }

    internal class TestErrorMessageGeneratorBuilder : TestDataBuilder<TestErrorMessageGenerator>
    {
        #region Private Members

        private IParameterCollectionFormatter _parameterFormatter;

        #endregion

        #region Exposed Methods

        public override TestErrorMessageGenerator Build()
        {
            var obj = new TestErrorMessageGenerator();
            if (_parameterFormatter != null)
                obj.ParameterFormatter = _parameterFormatter;
            return obj;
        }

        public TestErrorMessageGeneratorBuilder WithParameterFormatter(
            IParameterCollectionFormatter parameterCollectionFormatter)
        {
            _parameterFormatter = parameterCollectionFormatter;
            return this;
        }

        #endregion
    }

    internal class TestErrorMessageGenerator : ErrorMessageGenerator
    {
        #region Properties

        public IParameterCollectionFormatter ParameterFormatter
        {
            get { return _parameterFormatter; }
            set { _parameterFormatter = value; }
        }

        #endregion
    }
}
