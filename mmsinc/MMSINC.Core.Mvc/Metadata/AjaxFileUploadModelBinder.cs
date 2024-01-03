using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MMSINC.Utilities;
using StructureMap;
using ControllerBase = MMSINC.Controllers.ControllerBase;

namespace MMSINC.Metadata
{
    /// <summary>
    /// ModelBinder that handles the different ajax file uploading possibilities
    /// that come from fileuploader.js. 
    /// </summary>
    public class AjaxFileUploadModelBinder : IModelBinder
    {
        #region Constants

        public static readonly string FILE_NAME_KEY =
                                          Utilities.Expressions.GetMember((AjaxFileUpload x) => x.FileName).Name,
                                      BINARY_DATA_KEY = Utilities.Expressions
                                                                 .GetMember((AjaxFileUpload x) => x.BinaryData).Name,
                                      KEY_KEY = Utilities.Expressions.GetMember((AjaxFileUpload x) => x.Key).Name;

        public struct ContentTypes
        {
            #region Constants

            public const string MULTIPART_FORMDATA = "multipart/form-data",
                                APPLICATION_OCTET_STREAM = "application/octet-stream";

            #endregion
        }

        #endregion

        #region Fields

        private static readonly Action<ControllerContext, ModelBindingContext, AjaxFileUpload>[] _modelProviders;

        #endregion

        #region Constructors

        static AjaxFileUploadModelBinder()
        {
            _modelProviders = new Action<ControllerContext, ModelBindingContext, AjaxFileUpload>[] {
                TryBindingModelNormally,
                TryBindingAsMultiPartFormData,
                TryBindingFromOctetStream
            };
        }

        #endregion

        #region Private Methods

        #region Utility

        private static bool IsContentType(ControllerContext controllerContext, string contentType)
        {
            var requestCT = controllerContext.HttpContext.Request.ContentType;

            // Apparently this can return null if a request has a poorly formatted content type header or none at all.
            if (requestCT == null)
            {
                return false;
            }

            return requestCT.StartsWith(contentType);
        }

        private static byte[] GetByteArrayFromStream(Stream stream)
        {
            // We need to use the BinaryReader to properly decode file uploads,
            // but disposing it also disposes the stream which is annoying. 
            // This *shouldn't* pose any memory leaks because the streams
            // get disposed later on. In release mode, this BinaryReader
            // should get garbage collected fairly quickly.

            var reader = new BinaryReader(stream);
            return reader.ReadBytes((int)stream.Length);
        }

        private static T TryGetValue<T>(string key, ModelBindingContext bindingContext)
        {
            var vpResult = bindingContext.ValueProvider.GetValue(key);
            if (vpResult == null)
            {
                return default(T);
            }

            var convertedResult = vpResult.ConvertTo(typeof(T));
            if (convertedResult != null)
            {
                return (T)convertedResult;
            }
            else
            {
                return default(T);
            }
        }

        #endregion

        private static HttpPostedFileBase TryGetPostedFileForMultiPartFormData(ControllerContext controllerContext,
            ModelBindingContext bindingContext)
        {
            // In practice, before model binding even starts, Request.Files *should* already contain the file
            // all nicely parsed as an HttpPostedFileBase. The key should also match bindingContext.ModelName.
            // If, for some reason, that doesn't work then we can attempt normal model binding.

            var files = controllerContext.HttpContext.Request.Files;
            if (files.AllKeys.Contains(bindingContext.ModelName))
            {
                return files[bindingContext.ModelName];
            }
            else
            {
                return TryGetValue<HttpPostedFileBase>(bindingContext.ModelName, bindingContext);
            }
        }

        private static string ParseFileName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                // Ensure this is null.
                return null;
            }

            // NOTE: IE posts the entire path of the file being uploaded,
            //       while other browsers only post the file name.
            return Path.GetFileName(name);
        }

        private static void ValidateModel(AjaxFileUpload model, ControllerContext controllerContext,
            ModelBindingContext bindingContext)
        {
            // In order to validate correctly, we need to get ModelMetadata from elsewhere. bindingContext.ModelMetadata
            // has a null model which will cause the validators to all fail. This is how Controller.TryValidateModel works
            // internally, so good enough for MVC is good enough for me!
            //
            // Also, we're using model.GetType() instead of a static ref to typeof(AjaxFileUpload) so that this will correctly
            // validate models that inherit from AjaxFileUpload.
            var metadata = ModelMetadataProviders.Current.GetMetadataForType(() => model, model.GetType());
            var modelValidator = ModelValidator.GetModelValidator(metadata, controllerContext);
            foreach (var validatorResult in modelValidator.Validate(model))
            {
                bindingContext.ModelState.AddModelError(validatorResult.MemberName, validatorResult.Message);
            }
        }

        #region Model Provider methods

        private static void BindFromFileService(IContainer container, TempDataDictionary tempData,
            ModelBindingContext bindingContext, AjaxFileUpload model, Guid key)
        {
            var fileServ = container.With(tempData).GetInstance<FileUploadService>();
            if (fileServ.HasFile(key))
            {
                var file = fileServ.GetFile(key);
                // Potential threading issues, make sure BinaryData gets copied to a new array.
                model.BinaryData = file.BinaryData.ToArray();
                model.FileName = file.FileName;
                model.Key = file.Key;
            }
        }

        /// <summary>
        /// Attempts to create an AjaxFileUpload object just as the DefaultModelBinder does. Can't use
        /// the DefaultModelBinder itself because it will probably cause some endless loop of calling this 
        /// binder and back and I dunno I didn't test that yet.
        /// </summary>
        private static void TryBindingModelNormally(ControllerContext controllerContext,
            ModelBindingContext bindingContext, AjaxFileUpload model)
        {
            if (model.BinaryData == null)
            {
                var possibleBinaryData = TryGetValue<object>(GetBindingBinaryDataKey(bindingContext), bindingContext);
                if (possibleBinaryData != null)
                {
                    if (possibleBinaryData is IEnumerable<byte>)
                    {
                        model.BinaryData = ((IEnumerable<byte>)possibleBinaryData).ToArray();
                    }
                    else if (possibleBinaryData is string)
                    {
                        // If we're parsing an application/json result, we'll most likely have gotten
                        // a Base64 encoded string. That's what the web api serializer does, anyway.
                        model.BinaryData = Convert.FromBase64String((string)possibleBinaryData);
                    }
                }
            }

            if (model.FileName == null)
            {
                model.FileName = TryGetValue<string>(GetBindingFileNameKey(bindingContext), bindingContext);
            }
        }

        /// <summary>
        /// Tries to bind the model as if it were posted as a regular form post back.
        /// </summary>
        private static void TryBindingAsMultiPartFormData(ControllerContext controllerContext,
            ModelBindingContext bindingContext, AjaxFileUpload model)
        {
            if (!IsContentType(controllerContext, ContentTypes.MULTIPART_FORMDATA))
            {
                return;
            }

            if (model.BinaryData == null)
            {
                var file = TryGetPostedFileForMultiPartFormData(controllerContext, bindingContext);
                model.BinaryData = (file != null ? GetByteArrayFromStream(file.InputStream) : null);

                // NOTE: No way to get the FileName if there's not HttpPostedFileBase to use for the binary data.
                model.FileName = (file != null && !string.IsNullOrWhiteSpace(file.FileName) ? file.FileName : null);
            }
        }

        /// <summary>
        /// Tries to bind the model based on an actual ajax file upload, where browsers uses application/octet-stream.
        /// </summary>
        private static void TryBindingFromOctetStream(ControllerContext controllerContext,
            ModelBindingContext bindingContext, AjaxFileUpload model)
        {
            if (!IsContentType(controllerContext, ContentTypes.APPLICATION_OCTET_STREAM))
            {
                return;
            }

            if (model.BinaryData == null)
            {
                var controller = (Controller)controllerContext.Controller;
                model.BinaryData = GetByteArrayFromStream(controller.Request.InputStream);
            }

            if (model.FileName == null)
            {
                model.FileName = TryGetValue<string>(FILE_NAME_KEY, bindingContext);
            }
        }

        #endregion

        #endregion

        #region Exposed Methods

        private static string GetBindingKey(ModelBindingContext bindingContext, string key)
        {
            Console.WriteLine("BindingKey: " + string.Format("{0}.{1}", bindingContext.ModelName, key));
            return string.Format("{0}.{1}", bindingContext.ModelName, key);
        }

        public static string GetBindingBinaryDataKey(ModelBindingContext bindingContext)
        {
            return GetBindingKey(bindingContext, BINARY_DATA_KEY);
        }

        public static string GetBindingFileNameKey(ModelBindingContext bindingContext)
        {
            return GetBindingKey(bindingContext, FILE_NAME_KEY);
        }

        public static string GetBindingKeyKey(ModelBindingContext bindingContext)
        {
            return GetBindingKey(bindingContext, KEY_KEY);
        }

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var model = new AjaxFileUpload();

            // If a key is present, only bind from TempData/FileService. Ignore all other things.
            var key = TryGetValue<Guid>(GetBindingKeyKey(bindingContext), bindingContext);
            if (key != Guid.Empty)
            {
                if (controllerContext.Controller is ControllerBase c)
                {
                    BindFromFileService(c.Container, c.TempData, bindingContext, model, key);
                }
                else
                {
                    throw new InvalidOperationException(
                        "Cannot bind from file service without an MMSINC.Controllers.ControllerBase-based controller.  Controller controller base base controller.");
                }
            }
            else
            {
                foreach (var binder in _modelProviders)
                {
                    binder(controllerContext, bindingContext, model);
                }
            }

            model.FileName = ParseFileName(model.FileName);

            // This needs to return null if there's absolutely no file upload data. That
            // will allow us to put RequiredAttributes on FileUpload props on models.
            if (model.FileName == null && !model.HasBinaryData && model.Key == Guid.Empty)
            {
                return null;
            }

            ValidateModel(model, controllerContext, bindingContext);
            return model;
        }

        #endregion
    }
}
