<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderMapForm.ascx.cs" Inherits="LINQTo271.Controls.WorkOrders.WorkOrderMapForm" %>

<div>
    <asp:HiddenField runat="server" ID="hidMapAssetTypeID" Value='<%# Eval("AssetTypeID") %>' />
    <asp:HiddenField runat="server" ID="hidMapAssetID" Value='<%# Eval("Asset.AssetKey") %>' />
    <asp:HiddenField runat="server" ID="hidWorkOrderLatitude" Value='<%# Eval("Latitude") %>' />
    <asp:HiddenField runat="server" ID="hidWorkOrderLongitude" Value='<%# Eval("Longitude") %>' />
    <asp:HiddenField runat="server" ID="hidOperatingCenter" Value='<%# Eval("OperatingCenter.OperatingCenterID") %>'/>
    <div id="mapWrapper"></div>
</div>