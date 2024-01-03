using System;
using System.Web.Mvc;
using LINQTo271.Views.Abstract;
using MMSINC.Utilities.Documents;
using MMSINC.Controls;
using MMSINC.Interface;
using StructureMap;
using WorkOrders.Model;

namespace LINQTo271.Views.Documents
{
    public partial class DocumentDetailView : WorkOrdersDetailView<Document>
    {
        #region Properties

        public override IDetailControl DetailControl
        {
            get { throw new NotImplementedException(); }
        }

        public override IObjectContainerDataSource DataSource
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region Exposed Methods

        public override void ShowEntity(Document instance)
        {
            //if (instance.DocumentsWorkOrders.Count == 0)
            //{
            //    throw new DomainLogicException("Cannot open documents that are not attached to work orders in this scope.");
            //}

            var docServ = DependencyResolver.Current.GetService<IDocumentService>();
            var file = docServ.Open(instance.DocumentData.Hash);

            var encodedFileName = IServer.UrlEncode(instance.FileName);
            IResponse.AddHeader("Content-Disposition",
                String.Format("attachment;filename={0}", encodedFileName));
            IResponse.ContentType = "application/octet-stream";
            IResponse.BinaryWrite(file);
            IResponse.End();
        }

        #endregion
    }
}