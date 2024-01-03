using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using MMSINC.Data.Linq;
using Permits.Data.Client.Entities;
using Permits.Data.Client.Repositories;
using StructureMap;
using WorkOrders;
using WorkOrders.Model;
using AjaxFileUpload = Permits.Data.Client.Entities.AjaxFileUpload;

namespace LINQTo271.Views.Permits
{
    /// <summary>
    /// Summary description for Drawings
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [ScriptService]
    public class Drawings : WebService
    {
        public IRepository<WorkOrder> WorkOrderRepository
        {
            get
            {
                return
                    DependencyResolver.Current.GetService<IRepository<WorkOrder>>();
            }
        }


        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Create()
        {
            var permitId = Context.Request["permitId"];
            var qqFileName = Context.Request["qqFileName"];
            var workOrderId = Context.Request["workOrderId"];
            var repo = new DrawingRepository(WorkOrderRepository.Get(workOrderId).OperatingCenter.PermitsUserName);

            byte[] binaryData;

            // When IE posts the file, it properly ends up in Context.Request.Files. It's maybe the one
            // thing that IE actually does right. Otherwise, using InputStream will result in extra data
            // being considered part of the file, which in turn leads to corrupt files(try uploading an image
            // and reading it from InputStream for IE, it doesn't work).
            var inputStream = (Context.Request.Files.AllKeys.Any() ? Context.Request.Files[0].InputStream : Context.Request.InputStream);
            using (var reader = new BinaryReader(inputStream))
            {
                binaryData = reader.ReadBytes((int)inputStream.Length);
            }
        
            var drawing = repo.Save(
                new Drawing {
                    FileUpload = new AjaxFileUpload {
                        BinaryData = binaryData,
                        FileName = qqFileName
                    },
                    PermitId = Convert.ToInt32(permitId)
                });

            var ret = new JavaScriptSerializer().Serialize(new SuccessJson {
                success = (drawing != null)
            });

            Context.Response.Write(ret);
        }
    }

    public class SuccessJson
    {
        public bool success { get; set; }
    }
}
