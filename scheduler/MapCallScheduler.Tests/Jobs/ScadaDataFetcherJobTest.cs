using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallScheduler.Tests.Jobs
{
    [TestClass]
    public class ScadaDataFetcherJobTest
    {
        //#region Private Members

        //private ScadaTagFetcherJob _target;
        //private Mock<ILog> _log;
        //private Mock<IScadaTagNameService> _messageService;
        //private IContainer _container;

        //#endregion

        //#region Setup/Teardown

        //[TestInitialize]
        //public void TestInitialize()
        //{
        //    _container = new Container();
        //    _container.Inject((_log = new Mock<ILog>()).Object);
        //    _container.Inject((_messageService = new Mock<IScadaTagNameService>()).Object);
        //    _container.Inject(new Mock<IDeveloperEmailer>().Object);

        //    _target = _container.GetInstance<ScadaTagFetcherJob>();
        //}

        //#endregion

        //[TestMethod]
        //public void TestExecuteProcessesTheMessageService()
        //{
        //    _target.Execute(null);

        //    _messageService.Verify(x => x.Process());
        //}

        //[TestMethod]
        //public void TestExecuteLogsExceptionifThrownByMessageService()
        //{
        //    var e = new Exception();
        //    _messageService.Setup(x => x.Process()).Throws(e);

        //    _target.Execute(null);

        //    _log.Verify(x => x.Error("Error in scada tag name service", e));
        //}
    }
}
