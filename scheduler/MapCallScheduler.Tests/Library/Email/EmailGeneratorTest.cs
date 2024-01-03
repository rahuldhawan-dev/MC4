using System.Net.Mail;
using MapCallScheduler.Library.Email;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StructureMap;

namespace MapCallScheduler.Tests.Library.Email
{
    [TestClass]
    public class EmailGeneratorTest
    {
        #region Private Members

        private Mock<IMapCallSchedulerConfiguration> _config;
        private IContainer _container;
        private EmailGenerator _target;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();
            _container.Inject((_config = new Mock<IMapCallSchedulerConfiguration>()).Object);

            _target = _container.GetInstance<EmailGenerator>();
        }

        #endregion

        [TestMethod]
        public void TestGenerateGeneratesMessageWithSpecifiedArguments()
        {
            var from = "foo@meh.com";
            var to = "bar@meh.com";
            var subject = "hey";
            var message = "sup";
            var useHtml = false;

            var result = _target.Generate(from, to, subject, message, useHtml);

            Assert.AreEqual(new MailAddress(from), result.From);
            Assert.AreEqual(new MailAddress(to), result.To);
            Assert.AreEqual(subject, result.Subject);
            Assert.AreEqual(message, result.Body);
            Assert.AreEqual(useHtml, result.IsBodyHtml);
        }

        [TestMethod]
        public void TestGenerateOverridesToEmailAddressWhenConfiguredToDoSo()
        {
            var from = "foo@meh.com";
            var to = "bar@meh.com";
            var subject = "hey";
            var message = "sup";
            var expected = "foo@bar.baz";
            var useHtml = true;

            _config.SetupGet(x => x.AllEmailsGoTo).Returns(expected);
            
            var result = _target.Generate(from, to, subject, message, useHtml);

            Assert.AreEqual(new MailAddress(from), result.From);
            Assert.AreEqual(new MailAddress(expected), result.To);
            Assert.AreEqual(subject, result.Subject);
            Assert.AreEqual(message, result.Body);
            Assert.AreEqual(useHtml, result.IsBodyHtml);
        }

        [TestMethod]
        public void TestGenerateGeneratesMessageForException()
        {
            
        }
    }
}