using System;
using System.Linq;
using System.Linq.Expressions;
using Historian.Data.Client.Entities;
using Historian.Data.Client.Repositories;
using log4net;
using MapCall.Common.Model.Entities;
using MapCallScheduler.JobHelpers.ScadaData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data.NHibernate;
using Moq;
using StructureMap;

namespace MapCallScheduler.Tests.JobHelpers.ScadaData
{
    [TestClass]
    public class ScadaTagNameServiceTest
    {
        #region Private Members

        private Mock<IRepository<ScadaTagName>> _localRepo;
        private Mock<ITagNameRepository> _remoteRepo;
        private Mock<ILog> _log;
        private ScadaTagNameService _target;
        private IContainer _container;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();

            _container.Inject((_log = new Mock<ILog>()).Object);
            _container.Inject((_remoteRepo = new Mock<ITagNameRepository>()).Object);
            _container.Inject((_localRepo = new Mock<IRepository<ScadaTagName>>()).Object);

            _target = _container.GetInstance<ScadaTagNameService>();
        }

        #endregion

        [TestMethod]
        public void TestProcessDownloadsAllRemoteTagNamesAndSavesThemLocallyIfTheyDoNotExist()
        {
            var remoteTagNames = new[] {
                new TagName {Name = "1234", Description = "foo", Units = "stuff"},
                new TagName {Name = "4321", Description = "bar", Units = "things"}
            };

            _remoteRepo.Setup(x => x.GetAll()).Returns(remoteTagNames);

            foreach (var tagName in remoteTagNames)
            {
                _localRepo.Setup(
                    x =>
                        x.Where(
                            It.Is<Expression<Func<ScadaTagName, bool>>>(
                                f => f.Compile().Invoke(new ScadaTagName {TagName = tagName.Name}))))
                    .Returns(Enumerable.Empty<ScadaTagName>().AsQueryable());
            }

            _target.Process();

            foreach (var tagName in remoteTagNames)
            {
                _localRepo.Verify(r => r.Save(It.Is<ScadaTagName>(t => t.TagName == tagName.Name && t.Description == tagName.Description && t.Units == tagName.Units)));
            }
        }

        [TestMethod]
        public void TestProcessUpdatesExistingTagNameRecords()
        {
            var remoteTagNames = new[] {
                new TagName {Name = "1234", Description = "foo", Units = "stuff"},
                new TagName {Name = "4321", Description = "bar", Units = "things"}
            };

            _remoteRepo.Setup(x => x.GetAll()).Returns(remoteTagNames);
            var i = 0;
            _localRepo.Setup(x => x.GetAll()).Returns(remoteTagNames.Map(t => new ScadaTagName {
                Id = ++i,
                TagName = t.Name,
                Description = t.Description,
                Units = t.Units
            }).AsQueryable());

            _target.Process();

            foreach (var tagName in remoteTagNames)
            {
                _localRepo.Verify(
                    r =>
                        r.Save(
                            It.Is<ScadaTagName>(
                                t =>
                                    t.Id > 0 && t.TagName == tagName.Name && t.Description == tagName.Description &&
                                    t.Units == tagName.Units && !t.Inactive)));
            }
        }

        [TestMethod]
        public void TestProcessSetsAnyLocalRecordsWhichDoNotHaveCorrespondingRemoteRecordsToInactive()
        {
            var localTagNames = new[] {
                new ScadaTagName {TagName = "1234", Description = "foo", Units = "stuff"},
                new ScadaTagName {TagName = "4321", Description = "bar", Units = "things"}
            };

            _remoteRepo.Setup(x => x.GetAll()).Returns(Enumerable.Empty<TagName>());
            _localRepo.Setup(x => x.GetAll()).Returns(localTagNames.AsQueryable());

            foreach (var tagName in localTagNames)
            {
                _localRepo.Setup(
                    r =>
                        r.Save(
                            It.Is<ScadaTagName>(
                                t =>
                                    t.TagName == tagName.TagName && t.Description == tagName.Description &&
                                    t.Units == tagName.Units && t.Inactive)));
            }

            _target.Process();

            _localRepo.VerifyAll();
        }
    }
}
