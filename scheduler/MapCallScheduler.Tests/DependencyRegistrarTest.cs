using System.Collections.Generic;
using MapCall.Common.Model.Entities;
using MapCall.Common.Utility.Notifications;
using MapCall.SAP.Model.Repositories;
using MapCallScheduler.JobHelpers;
using MapCallScheduler.JobHelpers.MarkoutTickets;
using MapCallScheduler.JobHelpers.MarkoutTickets.MessageHandlers;
using MapCallScheduler.JobHelpers.ScadaData;
using MapCallScheduler.Library.Configuration;
using MapCallScheduler.Library.Email;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Common;
using MMSINC.Data.NHibernate;
using MMSINC.Interface;
using MMSINC.Utilities;
using Moq;
using Quartz;
using Quartz.Spi;
using StructureMap;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities.APIM;
using NHibernate;
using StructureMap.Pipeline;
using MapCall.LIMS.Configuration;
using MapCall.LIMS.Client;

namespace MapCallScheduler.Tests
{
    [TestClass]
    public class DependencyRegistrarTest
    {
        private IContainer _container;

        [TestInitialize]
        public void TestInitialize()
        {
            DependencyRegistry.IsInTestMode = true;
            _container = new Container(new DependencyRegistry());
            _container.Configure(e =>
                e.For<ISession>().Singleton().Use(ctx => ctx.GetInstance<ISessionFactory>().OpenSession()));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            DependencyRegistry.IsInTestMode = false;
        }

        [TestMethod]
        public void TestQuartzRegistrations()
        {
            _container.GetInstance<IJobFactory>().ShouldBeOfType<MapCallJobFactory>();
            _container.GetInstance<IScheduler>().ShouldBeOfType<IScheduler>();
        }

        [TestMethod]
        public void TestAPIMRegistrations()
        {
            _container.GetInstance<IAPIMClientFactory>().ShouldBeOfType<APIMClientFactory>();
            _container.GetInstance<ILIMSClientConfiguration>().ShouldBeOfType<LIMSClientConfiguration>();
            _container.GetInstance<ILIMSApiClient>().ShouldBeOfType<LIMSApiClient>();
        }

        [TestMethod]
        public void TestMMSINCLibraryRegistrations()
        {
            _container.GetInstance<IDateTimeProvider>().ShouldBeOfType<DateTimeProvider>();

            // notifier
            _container.GetInstance<ISmtpClient>().ShouldBeOfType<SmtpClientWrapper>();
            _container.GetInstance<INotifier>().ShouldBeOfType<RazorNotifier>();
            _container.GetInstance<INotificationService>().ShouldBeOfType<NotificationService>();
            _container.GetInstance<ISAPHttpClient>().ShouldBeOfType<SAPHttpClient>();
            _container.GetInstance<ISAPEquipmentRepository>().ShouldBeOfType<SAPEquipmentRepository>();
            _container.GetInstance<ISAPInspectionRepository>().ShouldBeOfType<SAPInspectionRepository>();

            // get around some secured repositories
            _container.GetInstance<IRepository<HydrantInspection>>().ShouldBeOfType<RepositoryBase<HydrantInspection>>();
            _container.GetInstance<IRepository<WorkOrder>>().ShouldBeOfType<RepositoryBase<WorkOrder>>();
        }

        [TestMethod]
        public void TestMapCallSchedulerHelperRegistrations()
        {
            // notifier
            _container.GetInstance<INotifierTaskService>().ShouldBeOfType<NotifierTaskService>();

            // markout tickets
            _container.GetInstance<IMarkoutAuditParser>().ShouldBeOfType<MarkoutAuditParser>();
            _container.GetInstance<IMarkoutTicketAuditor>().ShouldBeOfType<MarkoutTicketAuditor>();
            _container.GetInstance<IMarkoutTicketParser>().ShouldBeOfType<MarkoutTicketParser>();
            _container.GetInstance<IOneCallMessageProcessorService>(new ExplicitArguments(new Dictionary<string, object> {
                {"imapClient", new Mock<IWrappedImapClient>().Object}
            })).ShouldBeOfType<OneCallMessageProcessorService>();
            _container.GetInstance<IOneCallMessageService>().ShouldBeOfType<OneCallMessageService>();
            _container.GetInstance<IMarkoutTicketServiceConfiguration>().ShouldBeOfType<MarkoutTicketServiceConfiguration>();
            _container.GetInstance<IOneCallMessageHandlerFactory>().ShouldBeOfType<OneCallMessageHandlerFactory>();
            _container.GetInstance<IAuditFailureEmailer>().ShouldBeOfType<AuditFailureEmailer>();

            // scada data
            _container.GetInstance<IScadaTagNameService>().ShouldBeOfType<ScadaTagNameService>();

            // library
            _container.GetInstance<IIncomingEmailConfigSection>().ShouldBeOfType<IncomingEmailConfigSection>();
            _container.GetInstance<IDeveloperEmailer>().ShouldBeOfType<DeveloperEmailer>();
            _container.GetInstance<IEmailGenerator>().ShouldBeOfType<EmailGenerator>();
            _container.GetInstance<IMapCallSchedulerJobService>().ShouldBeOfType<MapCallSchedulerJobService>();
            _container.GetInstance<IMapCallSchedulerService>().ShouldBeOfType<MapCallSchedulerService>();
            _container.GetInstance<IMapCallSchedulerDateService>().ShouldBeOfType<MapCallSchedulerDateService>();
            
            // this won't work because it'll actually try to connect to the server:
            //_container.GetInstance<IWrappedImapClient>(new ExplicitArguments(new Dictionary<string, object> {
            //    {MarkoutTicketService.ImapClientArgs.SERVER, "foo"},
            //    {MarkoutTicketService.ImapClientArgs.PORT, 1234},
            //    {MarkoutTicketService.ImapClientArgs.USERNAME, "bar"},
            //    {MarkoutTicketService.ImapClientArgs.PASSWORD, "baz"},
            //    {MarkoutTicketService.ImapClientArgs.METHOD, AuthMethod.Auto},
            //    {MarkoutTicketService.ImapClientArgs.SSL, false},
            //    {MarkoutTicketService.ImapClientArgs.VALIDATE, null}
            //})).ShouldBeOfType<WrappedImapClient>();
        }
    }
}
