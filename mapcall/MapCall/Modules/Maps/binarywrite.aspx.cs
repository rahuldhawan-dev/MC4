using System;
using System.Data;
using System.Web.Mvc;
using System.Web.UI;
using MapCall.Common.Model.Repositories;
using MMSINC.Utilities.Documents;
using StructureMap;

namespace MapCall.Modules.Maps
{
    public partial class binarywrite : Page
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            var canAccess = false;
            var reqId = Request.QueryString["id"];
            var tmpDocID = Int32.Parse(reqId);

            var dv = (DataView)dsUserNameImages.Select(DataSourceSelectArguments.Empty);
            if (dv.Table.Rows.Count > 0)
            {
                foreach (DataRow dr in dv.Table.Rows)
                {
                    if (dr[0].ToString() == reqId)
                    {
                        BinaryWriteImage(tmpDocID);
                        canAccess = true;
                        break;
                    }
                }
            }

            if (!canAccess)
            {
                Response.Write("Access Denied");
            }
        }
        private void BinaryWriteImage(int documentID)
        {
            var docRepo = DependencyResolver.Current.GetService<IDocumentRepository>();
            var doc = docRepo.Find(documentID);
            var file = DependencyResolver.Current.GetService<IDocumentService>().Open(doc.DocumentData.Hash);

            Response.AddHeader("Content-disposition", String.Format("inline;filename={0}", Server.UrlEncode(doc.FileName)));
            Response.ContentType = "application/octet-stream";
            Response.BinaryWrite(file);
        }
    }
}
