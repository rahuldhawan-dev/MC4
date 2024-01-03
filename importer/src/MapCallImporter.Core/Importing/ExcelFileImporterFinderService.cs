using MapCallImporter.Common;
using StructureMap;
using System;

namespace MapCallImporter.Importing
{
    public class ExcelFileImporterFinderService : ExcelFileHandlerFinderServiceBase
    {
        #region Constants

        public const string IMPORTER_NAMESPACE = "MapCallImporter.Importing.Importers";

        #endregion

        #region Properties

        public override Type HandlerType => typeof(ExcelFileImporter<,,>);

        #endregion

        #region Constructors

        public ExcelFileImporterFinderService(IContainer container) : base(container) { }

        #endregion
    }
}