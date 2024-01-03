using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Utility.AssetUploads;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class CreateAssetUpload : ViewModel<AssetUpload>
    {
        [DoesNotAutoMap]
        [Required, FileUpload(FileTypes.Xlsx)]
        public AjaxFileUpload FileUpload { get; set; }

        public CreateAssetUpload(IContainer container) : base(container) { }

        public override AssetUpload MapToEntity(AssetUpload entity)
        {
            entity = base.MapToEntity(entity);

            entity.FileName = FileUpload.FileName;
            entity.FileGuid = Guid.NewGuid();
            entity.Status = new AssetUploadStatus {Id = AssetUploadStatus.Indices.PENDING};

            try
            {
                _container.GetInstance<IAssetUploadFileService>().SaveFile(entity.FileGuid, FileUpload.BinaryData);
            }
            catch (Exception e)
            {
                entity.Status = new AssetUploadStatus {Id = AssetUploadStatus.Indices.ERROR};
                entity.ErrorText = e.ToString();
            }

            return entity;
        }
    }

    public class SearchAssetUpload : SearchSet<AssetUpload>
    {
        [DropDown, EntityMustExist(typeof(User))]
        public int? CreatedBy { get; set; }

        public DateRange CreatedAt { get; set; }
        public SearchString FileName { get; set; }

        [DropDown, EntityMustExist(typeof(AssetUploadStatus))]
        public int? Status { get; set; }
    }
}