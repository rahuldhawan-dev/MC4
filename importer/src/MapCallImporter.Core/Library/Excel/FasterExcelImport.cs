using MMSINC.Utilities.Excel;
using OfficeOpenXml;
using StructureMap;

namespace MapCallImporter.Library.Excel
{
    public class FasterExcelImport<T> : ExcelImport<T>
    {
        #region Constants

        public const int MAX_ROW_SCAN = 1;

        #endregion

        #region Private Members

        protected IContainer _container;

        #endregion

        #region Properties

        public override IContainer Container => _container;
        protected override int MaxRowScan => MAX_ROW_SCAN;

        #endregion

        #region Constructors

        public FasterExcelImport(ExcelPackage package, IContainer container) : base(container, null)
        {
            Package = package;
            _container = container;
        }

        #endregion

        #region Private Methods

        protected override T GetInstance()
        {
            return _container.GetInstance<T>();
        }

        #endregion

        #region Exposed Methods

        public override void Dispose() { }

        #endregion
    }
}
