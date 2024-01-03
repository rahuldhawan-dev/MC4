using System;
using System.Linq;
using System.Web.Mvc;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Metadata;
using StructureMap;

namespace MMSINC.Utilities
{
    public interface IFileUploadService
    {
        void StoreFileAndSetKey(AjaxFileUpload file);
        AjaxFileUpload GetFile(Guid key);
        bool HasFile(Guid key);
    }

    public class FileUploadService : IFileUploadService
    {
        #region Fields

        private bool _expiredFilesChecked;
        private readonly TempDataDictionary _tempData;
        private readonly IDateTimeProvider _dateTimeProvider;
        public static readonly TimeSpan UPLOAD_EXPIRATION_TIMEOUT = new TimeSpan(0, 10, 0); // 10 minutes

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new FileUploadService instance that uses the provided TempDataDictionary as its backing storage.
        /// </summary>
        /// <param name="tempData"></param>
        public FileUploadService(IDateTimeProvider dateTimeProvider, TempDataDictionary tempData)
        {
            tempData.ThrowIfNull("tempData");
            _tempData = tempData;
            _dateTimeProvider = dateTimeProvider;
        }

        #endregion

        #region Private Methods

        private void ClearExpiredUploads()
        {
            // We only wanna do this check once. This method's called from all the
            // publically accessed methods, we don't wanna hit a race condition where
            // HasFile returns true and then a second later GetFile throws cause the
            // file expired and no longer exists.

            if (_expiredFilesChecked)
            {
                return;
            }

            var now = _dateTimeProvider.GetCurrentDate();
            var keys = _tempData.Keys.ToArray();
            foreach (var key in keys)
            {
                var possibleItem = _tempData.Peek(key) as FileUploadServiceItem;
                if (possibleItem != null)
                {
                    var timeSinceUpload = now - possibleItem.UploadedAt;
                    if (timeSinceUpload >= UPLOAD_EXPIRATION_TIMEOUT)
                    {
                        _tempData.Remove(key);
                    }
                }
            }

            _expiredFilesChecked = true;
        }

        #endregion

        #region Public Methods

        public void StoreFileAndSetKey(AjaxFileUpload file)
        {
            ClearExpiredUploads();
            file.ThrowIfNull("file");
            if (file.Key != Guid.Empty)
            {
                throw new InvalidOperationException("This file has already been stored with a key.");
            }

            file.Key = Guid.NewGuid();

            var item = new FileUploadServiceItem {
                FileUpload = file,
                UploadedAt = _dateTimeProvider.GetCurrentDate()
            };

            _tempData[file.Key.ToString()] = item;
        }

        public bool HasFile(Guid key)
        {
            ClearExpiredUploads();
            try
            {
                var file = GetFile(key);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public AjaxFileUpload GetFile(Guid key)
        {
            ClearExpiredUploads();
            // Use Peek because we don't wanna mark the key as dirty
            // until we're done. 
            var possibleFile = _tempData.Peek(key.ToString());
            if (possibleFile == null)
            {
                throw new InvalidOperationException("There is no uploaded file matching the key: " + key);
            }

            if (!(possibleFile is FileUploadServiceItem))
            {
                throw new InvalidOperationException(
                    "The uploaded file key does not have an FileUploadServiceItem instance. Instead it has a " +
                    possibleFile.GetType().FullName);
            }

            var item = (FileUploadServiceItem)possibleFile;
            return item.FileUpload;
        }

        #endregion
    }

    public class FileUploadServiceItem
    {
        public AjaxFileUpload FileUpload { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}
