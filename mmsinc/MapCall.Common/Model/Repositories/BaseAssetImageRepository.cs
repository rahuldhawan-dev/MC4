using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using MMSINC.Utilities.Pdf;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public interface IAssetImageRepository<TEntity> : IRepository<TEntity> where TEntity : class, IAssetImage
    {
        bool FileExists(string fileName, Town town);
        bool FileExists(TEntity entity);
        byte[] GetImageDataAsPdf(TEntity entity);
    }

    public abstract class BaseAssetImageRepository<TEntity> : RepositoryBase<TEntity>, IAssetImageRepository<TEntity>
        where TEntity : class, IAssetImage
    {
        #region Fields

        private readonly IImageToPdfConverter _imageToPdfConverter;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the relative directory for saving the file to.
        /// </summary>
        protected abstract string RelativeFileDirectory { get; }

        #endregion

        #region Constructor

        protected BaseAssetImageRepository(ISession session, IContainer container,
            IImageToPdfConverter imageToPdfConverter) : base(session, container)
        {
            _imageToPdfConverter = imageToPdfConverter;
        }

        #endregion

        #region Private Methods

        private static string GetBaseImageDirectory()
        {
            var rootDir = ConfigurationManager.AppSettings["ImageUploadRootDirectory"];
            if (string.IsNullOrWhiteSpace(rootDir))
            {
                throw new InvalidOperationException(
                    "The ImageUploadRootDirectory application setting must have a value.");
            }

            return rootDir;
        }

        private void SaveFile(IAssetImage entity)
        {
            // BIG STUPID NOTE:
            // This can ONLY be used for new records. Updating a record and changing
            // its town/state/whatever will not move the file.
            //
            // BIG STUPID NOTE 2:
            // The way file names work is an atrocious mess so we can't just rename
            // the file if there's one with the same name already. 
            //
            // BIG STUPID NOTE 3:
            // Related to note 2, the Directory value MUST end with a / or else the
            // image viewing pages load the wrong file location.

            entity.FileName.ThrowIfNullOrWhiteSpace("FileName");
            var path = new PathHelper(entity.FileName, RelativeFileDirectory, entity.Town);
            // This is for backwards compatibility with the way the data currently 
            // is in the database. 
            entity.Directory = path.RelativeDirectoryPath.Replace('\\', '/');

            FileIO.WriteAllBytes(path.AbsoluteFilePath, entity.ImageData);
        }

        private string GetAbsoluteFilePathForNewRecord(string fileName, Town town)
        {
            return new PathHelper(fileName, RelativeFileDirectory, town).AbsoluteFilePath;
        }

        private string GetAbsoluteFilePathForEntity(TEntity entity)
        {
            return new PathHelper(entity).AbsoluteFilePath;
        }

        #endregion

        #region Public Methods

        public bool FileExists(string fileName, Town town)
        {
            return File.Exists(GetAbsoluteFilePathForNewRecord(fileName, town));
        }

        public bool FileExists(TEntity entity)
        {
            var path = new PathHelper(entity).AbsoluteFilePath;
            return File.Exists(path);
        }

        /// <summary>
        /// Gets the actual file name(not the path) for an entity. This is
        /// here because MapImages are awful.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected virtual string GetActualFileName(TEntity entity)
        {
            // The correct/default behavior for everything but MapImages
            return entity.FileName;
        }

        /// <summary>
        /// Returns the image data as a pdf. If the image is an image, the image
        /// is converted to a pdf.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public byte[] GetImageDataAsPdf(TEntity entity)
        {
            const StringComparison caseCompare = StringComparison.InvariantCultureIgnoreCase;

            var actualFileName = GetActualFileName(entity);

            Func<string, byte[]> readFile = (fileName) => {
                // Path.Combine can apparently flip the Directory slashes, so that's nice of it.
                var path = Path.Combine(GetBaseImageDirectory(), entity.Directory, fileName);
                try
                {
                    return File.ReadAllBytes(path);
                }
                catch (DirectoryNotFoundException ex)
                {
                    throw new AssetImageException(AssetImageExceptionType.FileNotFound, "Directory not found.", ex);
                }
                catch (FileNotFoundException ex)
                {
                    throw new AssetImageException(AssetImageExceptionType.FileNotFound, "File not found.", ex);
                }
            };

            if (actualFileName.EndsWith(".pdf", caseCompare))
            {
                return readFile(actualFileName);
            }

            var imageData = new List<byte[]>();

            if (actualFileName.EndsWith(".tif", caseCompare) || actualFileName.EndsWith(".tiff", caseCompare))
            {
                imageData.Add(readFile(actualFileName));
            }
            else if (actualFileName.Contains("."))
            {
                throw new NotSupportedException("Unable to use file type. " + actualFileName);
            }
            else
            {
                // For whatever reason, ancient data inserted hundreds of thousands of records
                // where there are multiple files, but their file names have been combined into
                // a single long string. It's assumed these are tifs. All the actual file names
                // are 8 characters(without extension).

                foreach (var file in actualFileName.SplitEvery(8))
                {
                    imageData.Add(readFile(file + ".tif"));
                }
            }

            try
            {
                return _imageToPdfConverter.Render(imageData);
            }
            catch (ImageToPdfConverterException ex)
            {
                throw new AssetImageException(AssetImageExceptionType.InvalidImageData, "Invalid image data.", ex);
            }
        }

        public override void Save(IEnumerable<TEntity> entities)
        {
            // The base Save(IEnumerable) will not work here
            // because it only calls Session.Save.
            throw new NotImplementedException();
        }

        public override TEntity Save(TEntity entity)
        {
            // Only new records can have their images saved
            if (entity.Id == 0)
            {
                SaveFile(entity);
            }

            return base.Save(entity);
        }

        public override void Delete(TEntity entity)
        {
            base.Delete(entity);
            // Assuming this has flushed and is actually definitely deleted
            // from the database then we can delete the file.
            // If we ever get transactions, we'll want to make sure the
            // transaction goes through, then delete, then commit the transaction.
            //
            // Also there are older records that have duplicate images which
            // is JUST SO MUCH FUN!
            var possiblyOldRecordsUsingTheSameFile =
                Linq.Any(x => x.Directory == entity.Directory && x.FileName == entity.FileName);
            if (!possiblyOldRecordsUsingTheSameFile)
            {
                var filePath = GetAbsoluteFilePathForEntity(entity);
                FileIO.DeleteIfFileExists(filePath);
            }
        }

        #endregion

        #region Helper classes

        private sealed class PathHelper
        {
            public string AbsoluteFilePath { get; private set; }
            public string AbsoluteDirectoryPath { get; private set; }
            public string RelativeDirectoryPath { get; private set; }

            /// <summary>
            /// Use this constructor if you have a NEW record with a file that is NOT saved.
            /// </summary>
            /// <param name="fileName"></param>
            /// <param name="relativeFileDir"></param>
            /// <param name="town"></param>
            public PathHelper(string fileName, string relativeFileDir, Town town)
            {
                fileName = fileName.Trim();
                // DistrictID is nullable, so use 0.
                var districtId = town.DistrictId ?? 0;
                RelativeDirectoryPath =
                    Path.Combine(town.County.State.Abbreviation, relativeFileDir, districtId.ToString()) + "\\";
                AbsoluteDirectoryPath = Path.Combine(GetBaseImageDirectory(), RelativeDirectoryPath);
                AbsoluteFilePath = Path.Combine(AbsoluteDirectoryPath, fileName);
            }

            /// <summary>
            /// Use this contructor if you have an entity with a file THAT ALREADY EXISTS ON THE DRIVE.
            /// </summary>
            /// <param name="entity"></param>
            public PathHelper(TEntity entity)
            {
                RelativeDirectoryPath = entity.Directory.Replace('/', '\\');
                AbsoluteDirectoryPath = Path.Combine(GetBaseImageDirectory(), RelativeDirectoryPath);
                AbsoluteFilePath = Path.Combine(AbsoluteDirectoryPath, entity.FileName);
            }
        }

        #endregion
    }

    public class AssetImageException : Exception
    {
        public AssetImageExceptionType AssetExceptionType { get; private set; }

        public AssetImageException(AssetImageExceptionType type, string message, Exception innerException = null)
            : base(message, innerException)
        {
            AssetExceptionType = type;
        }
    }

    public enum AssetImageExceptionType
    {
        FileNotFound,
        InvalidImageData
    }
}
