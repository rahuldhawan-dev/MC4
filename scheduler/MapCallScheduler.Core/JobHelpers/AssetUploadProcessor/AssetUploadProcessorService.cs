using log4net;
using MapCall.Common.Model.Entities;
using MapCall.Common.Utility.AssetUploads;
using MapCallImporter.Common;
using MMSINC.ClassExtensions.NameValueCollectionExtensions;
using MMSINC.Common;
using MMSINC.Data.V2;
using MMSINC.Interface;
using MMSINC.Utilities.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace MapCallScheduler.JobHelpers.AssetUploadProcessor
{
    public class AssetUploadProcessorService : IAssetUploadProcessorService
    {
        #region Private Members

        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IAssetUploadFileService _fileService;
        private readonly IAssetUploadFileHandler _fileHandler;
        private readonly ISmtpClient _smtpClient;
        private readonly ILog _log;
        private const string NO_REPLY_KEY = "noreply_address";

        #endregion

        #region Constructors

        public AssetUploadProcessorService(IUnitOfWorkFactory unitOfWorkFactory, IAssetUploadFileService fileService,
            IAssetUploadFileHandler fileHandler, ISmtpClientFactory smtpClientFactory, ILog log)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _fileService = fileService;
            _fileHandler = fileHandler;
            _smtpClient = smtpClientFactory.Build();
            _log = log;
        }

        #endregion

        #region Private methods

        protected void SendEmail(string to, string subject, string body)
        {
            using (var mail = new MailMessageWrapper(ConfigurationManager.AppSettings.EnsureValue(NO_REPLY_KEY), to) {
                IsBodyHtml = false, Subject = subject, Body = body
            })
            {
                try
                {
                    _smtpClient.Send(mail);
                }
                catch (Exception) { }
            }
        }

        protected IList<AssetUploadRecord> ProcessUploads(IList<AssetUploadRecord> uploads)
        {
            _log.Info($"Found {uploads.Count} files to process");

            string DescribeFile(AssetUploadRecord file)
            {
                return $"{file.Id} - {file.FileName} - {file.FileGuid}";
            }

            foreach (var upload in uploads)
            {
                _log.Info($"Processing file {DescribeFile(upload)}...");
                var result = _fileHandler.Handle(_fileService.GetFilePath(upload.FileGuid));

                if (result.Result == ExcelFileProcessingResult.FileValid)
                {
                    upload.Status = new AssetUploadStatus {Id = AssetUploadStatus.Indices.SUCCESS};
                    _log.Info($"Successfully processed {DescribeFile(upload)}");
                    SendEmail(upload.CreatedBy,
                        $"MapCallImporter: Successfully Imported File {upload.FileName}",
                        $"MapCallImporter: Successfully Imported File {upload.FileName}");
                }
                else
                {
                    var issues = string.Join(Environment.NewLine, result.Issues);
                    upload.Status = new AssetUploadStatus {Id = AssetUploadStatus.Indices.ERROR};
                    var recipients = new List<string> {upload.CreatedBy};
                    switch (result.Result)
                    {
                        case ExcelFileProcessingResult.CouldNotDetermineContentType:
                            upload.ErrorText =
                                $"Could not determine type of file '{upload.FileName}' by its column headers.  Please adjust the columns to match an expected file type, or choose another file.";
                            break;
                        case ExcelFileProcessingResult.InvalidFileType:
                            upload.ErrorText =
                                $"The file '{upload.FileName}' does not appear to be a valid Microsoft Excel Open XML (Excel 2007+) document.  Please choose another file.";
                            break;
                        case ExcelFileProcessingResult.InvalidFileContents:
                            upload.ErrorText =
                                $"The file '{upload.FileName}' has invalid contents.{Environment.NewLine}{issues}";
                            break;
                        case ExcelFileProcessingResult.OtherError:
                            upload.ErrorText =
                                $"Error processing file '{upload.FileName}'.{Environment.NewLine}{issues}";
                            recipients.Add(ErrorMessageGenerator.RECIPIENT);
                            break;
                    }

                    _log.Info($"Error processing {DescribeFile(upload)}");
                    _log.Info(upload.ErrorText);

                    foreach (var recipient in recipients)
                    {
                        SendEmail(recipient, $"MapCallImporter: Error Importing File {upload.FileName}",
                            $"https://mapcall.amwater.com/modules/mvc/AssetUpload/Show/{upload.Id}{Environment.NewLine}{upload.ErrorText}");
                    }
                }
            }

            return uploads;
        }

        #endregion

        #region Exposed Methods

        public void Process()
        {
            IList<AssetUploadRecord> originalRecords;
            using (var uow = _unitOfWorkFactory.Build())
            {
                originalRecords = uow.GetRepository<AssetUpload>()
                             .Where(au => au.Status.Id == AssetUploadStatus.Indices.PENDING)
                             .OrderBy(au => au.CreatedAt)
                             .Select(au => new AssetUploadRecord(au))
                             .ToList();
            }

            var updatedRecords = ProcessUploads(originalRecords);

            using (var uow = _unitOfWorkFactory.Build())
            {
                var ids = updatedRecords.Select(r => r.Id).ToArray();
                var repo = uow.GetRepository<AssetUpload>();
                var existingRecords = repo.Where(au => ids.Contains(au.Id));

                foreach (var updated in updatedRecords)
                {
                    var existing = existingRecords.Single(au => au.Id == updated.Id);
                    existing.Status = updated.Status;
                    existing.ErrorText = updated.ErrorText;
                    repo.Update(existing);
                }

                uow.Commit();
            }
        }

        #endregion

        public class AssetUploadRecord
        {
            public int Id { get; }
            public string FileName { get; }
            public Guid FileGuid { get; }
            public string CreatedBy { get; }
            public AssetUploadStatus Status { get; set; }
            public string ErrorText { get; set; }

            public AssetUploadRecord(AssetUpload upload)
            {
                Id = upload.Id;
                FileName = upload.FileName;
                FileGuid = upload.FileGuid;
                CreatedBy = upload.CreatedBy.Email;
            }
        }
    }
}