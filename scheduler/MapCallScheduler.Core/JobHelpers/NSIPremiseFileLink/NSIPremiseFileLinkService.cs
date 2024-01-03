using System;
using System.Linq;
using System.Web.UI.WebControls;
using log4net;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCallScheduler.Library.Common;
using MMSINC.Data.NHibernate;
using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using StructureMap;
using DataType = MapCall.Common.Model.Entities.DataType;

namespace MapCallScheduler.JobHelpers.NSIPremiseFileLink
{
    public class NSIPremiseFileLinkService : INSIPremiseFileLinkService
    {
        #region Constants

        private const string PREMISE_DOCUMENT = "Premise";
        private const string PREMISE_TABLE = "Premises";
        private const int PREMISE_DOCUMENT_TYPE = DocumentType.Indices.PREMISE;

        #endregion

        #region Private Members

        private readonly NSIPremiseFileDownloadService _fileDownloadService;
        private readonly ILog _log;
        private readonly IContainer _container;
        protected readonly IRepository<Premise> _repository;

        #endregion

        #region Constructors

        public NSIPremiseFileLinkService(IContainer container,
            NSIPremiseFileDownloadService fileDownloadService, 
            ILog log,
            IRepository<Premise> repository)
        {
            _container = container;
            _fileDownloadService = fileDownloadService;
            _log = log;
            _repository = repository;
        }

        #endregion

        #region Private Methods

        private void SaveDocument(int id, string fileName, byte[] bytes)
        {
            var docDataRepo = _container.GetInstance<IDocumentDataRepository>();
            var docData = docDataRepo.FindByBinaryData(bytes);

            DataType datatype = _container.GetInstance<IDataTypeRepository>().GetByTableName(PREMISE_TABLE).SingleOrDefault();
            if (docData == null)
            {
                var documentData = new DocumentData { BinaryData = bytes };
                var document = new Document {
                    DocumentData = documentData,
                    DocumentType = datatype.DocumentTypes.Single(dt => dt.Name == PREMISE_DOCUMENT),
                    FileName = fileName
                };
                _container.GetInstance<IDocumentRepository>().Save(document);
                var documentLink = new DocumentLink {
                    DataType = datatype,
                    Document = document,
                    DocumentType = datatype.DocumentTypes.Single(dt => dt.Name == PREMISE_DOCUMENT),
                    LinkedId = id
                };
                _container.GetInstance<IRepository<DocumentLink>>().Save(documentLink);
            }
            else
            {
                var document = new Document {
                    DocumentData = docData,
                    DocumentType = datatype.DocumentTypes.Single(dt => dt.Name == PREMISE_DOCUMENT),
                    FileName = fileName
                };
                var documentLink = new DocumentLink {
                    DataType = datatype,
                    Document = document,
                    DocumentType = new DocumentType { Id = PREMISE_DOCUMENT_TYPE },
                    LinkedId = id
                };
                _container.GetInstance<IDocumentRepository>().Save(document);
                _container.GetInstance<IRepository<DocumentLink>>().Save(documentLink);
            }
        }

        #endregion

        #region Exposed Methods

        public void Process()
        {
            var files = _fileDownloadService.GetAllFiles();
            
            _log.Info($"Found {files.Count()} NSI files...");
            foreach (var file in files)
            {
                _log.Info($"Processing NSI file '{file.Filename}'...");

                //File name structure looks like the following:
                //OperatingCenterID.PremiseNumber.InstallationNumber.Devicelocation.connection.PDF

                var justFileName = file.Filename.Split('\\').Last();
                var premiseNumber = justFileName.Split('.')[1];
                var installation = justFileName.Split('.')[2];
                var deviceLocation = justFileName.Split('.')[3];
                var connectionObject = justFileName.Split('.')[4];

                var foundPremiseIds = _repository.Where(
                    p =>
                        p.PremiseNumber == premiseNumber &&
                        p.DeviceLocation == deviceLocation &&
                        p.ConnectionObject == connectionObject &&
                        p.Installation == installation).Select(p => p.Id);

                int? premiseId = foundPremiseIds.SingleOrDefault();
                
                if (premiseId.HasValue && premiseId.Value > 0)
                {
                    _log.Info($"Saving file '{file.Filename}'...");
                    
                    SaveDocument(premiseId.Value, justFileName, file.Bytes);
                    _fileDownloadService.DeleteFile(file.Filename);
                }
                else
                {
                    _log.Info($"File '{file.Filename}' could not be matched to a Premise record!");
                }
            }
        }
        #endregion
    }

    public interface INSIPremiseFileLinkService : IProcessableService { }
}