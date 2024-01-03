<%@ Page Language="C#" AutoEventWireup="true" Theme="bender" CodeBehind="RptStreets.aspx.cs" Inherits="MapCall.Reports.Data.RptStreets" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Panel runat="server" ID="pnlMap" Width="100%" Height="100%" BackColor="White">
            <table class="" style="margin:0px;padding:0px;width:100%;height:100%;" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td colspan="4" class="RptHeader">
                        Streets
                    </td>
                </tr>
                <tr>
                    <td class="RptTDData" style="height:90%;padding:0px;">
                        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt"
                            Height="100%" Width="100%">
                            <LocalReport ReportPath="Reports\Data\RptStreets.rdlc"><DataSources>
                            <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSet1_rptStreets"></rsweb:ReportDataSource>
                            </DataSources>
                            </LocalReport>
                            </rsweb:ReportViewer>
                        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetData"
                            TypeName="MapCall.DataSet1TableAdapters.rptStreetsTableAdapter">
                        </asp:ObjectDataSource> 
                    </td>
                </tr>
            </table>
        </asp:Panel>   
    </div>
    </form>
</body>
</html>
