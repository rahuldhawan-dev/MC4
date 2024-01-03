<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MaterialPicker.ascx.cs" Inherits="MapCall.Controls.MaterialPicker" %>
<%@ Register TagPrefix="mapcall" Namespace="MMSINC.Controls" Assembly="MMSINC.Core.WebForms" %>

<div>Search:
    <asp:TextBox runat="server" ID="txtSearch" 
        ClientIDMode="Static" 
        onkeyup="MaterialPicker.txtSearch_Keyup(this);"/>
    <br/>
</div>

<div>
    Material: <br/>
    <mapcall:MvpListBox runat="server" ID="lbMaterials" Rows="6" 
        ClientIDMode="Static"
        onchange="MaterialPicker.lbMaterials_Change(this);"
        style="width:350px;"></mapcall:MvpListBox>
    <br/>
</div>

<asp:HiddenField runat="server" ID="hidMaterialID" ClientIDMode="Static" />
<asp:HiddenField runat="server" ID="hidOperatingCenterID" ClientIDMode="Static" />
<asp:SqlDataSource runat="server" ID="dsMaterials" 
    ConnectionString="<%$ ConnectionStrings:MCProd %>"
    SelectCommand="Select MaterialID, PartNumber + ' - ' + cast(Description as varchar) as PartNumber from Materials order by PartNumber" />
<asp:RequiredFieldValidator ID="rfvMaterials" ControlToValidate="lbMaterials" InitialValue="" 
    Text="Required" runat="server" Display="Dynamic" />
    
<script type="text/javascript">
    Sys.Application.add_load(function () {
      MaterialPicker.txtSearch_Keyup($('#txtSearch')[0]);
    });
</script>