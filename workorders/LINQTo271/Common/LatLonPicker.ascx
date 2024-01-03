<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LatLonPicker.ascx.cs" Inherits="LINQTo271.Common.LatLonPicker" %>

<%-- these will not render --%>
<link href="../Includes/jqModal.css" rel="stylesheet" type="text/css" runat="server" visible="false" />
<link href="../App_Themes/WorkOrders/WorkOrders.css" type="text/css" runat="server" visible="false" />

<%-- inline style here so that its size will match the default size of a select --%>
<mmsinc:MvpImageButton runat="server" ID="imgShowPicker" Style="height: 16px; width: 20px;" />
<asp:HiddenField runat="server" ID="hidAssetID" Value='<%# AssetID %>' />
<asp:HiddenField runat="server" ID="hidAssetTypeID" Value='<%# AssetTypeID %>' />
<mmsinc:MvpHiddenField runat="server" ID="hidLatitude" />
<mmsinc:MvpHiddenField runat="server" ID="hidLongitude" />
<mmsinc:MvpHiddenField runat="server" ID="hidState" />
<asp:HiddenField runat="server" ID="hidAddress" />

<!-- actual picker -->
<asp:Panel runat="server" ID="modalWindow" CssClass="jqmWindow">
    <div id="jqmTitle">
        <button class="jqmClose" id="btnClose">
            Close X
        </button>
    </div>
    <div id="jqmWrapper">
        <iframe runat="server" id="jqmContent" class="jqmContent" src=""></iframe>
    </div>
</asp:Panel>

<mmsinc:CssInclude runat="server" CssFileName="jqModal.css" />
<mmsinc:ScriptInclude runat="server" ScriptFileName="jqmodal.js" />
<mmsinc:ScriptInclude runat="server" ScriptFileName="LatLonPicker.js" />

<script type="text/javascript">
    $(document).ready(function() {
        LatLonPicker.Picker.initialize($('#<%= modalWindow.ClientID %>'),
            $('#<%= imgShowPicker.ClientID %>'),
            $('#<%= jqmContent.ClientID %>'),
            $('#<%= hidAssetID.ClientID %>'),
            $('#<%= hidAssetTypeID.ClientID %>'),
            $('#<%= hidLatitude.ClientID %>'),
            $('#<%= hidLongitude.ClientID %>'),
            $('#<%= hidAddress.ClientID %>'),
            eval('<%= ClientClickHandler %>'));
    });

    LatLonPicker.Picker.url = '<%= ResolveUrl("~/Views/Assets/AssetLatLonPickerView.aspx") %>';

    function _getFieldVal(id) {
        return $('#' + id).val();
    }

    function _setFieldVal(id, val) {
        return $('#' + id).val(val);
    }

    function getLat() {
        return _getFieldVal('<%= hidLatitude.ClientID %>');
    }

    function setLat(val) {
        _setFieldVal('<%= hidLatitude.ClientID %>', val);
        LatLonPicker.ImageButton.setIcon($('#<%= imgShowPicker.ClientID %>'),
            $('#<%= hidLatitude.ClientID %>'),
            $('#<%= hidLongitude.ClientID %>'));
    }

    function getLon() {
        return _getFieldVal('<%= hidLongitude.ClientID %>');
    }

    function setLon(val) {
        _setFieldVal('<%= hidLongitude.ClientID %>', val);
        LatLonPicker.ImageButton.setIcon($('#<%= imgShowPicker.ClientID %>'),
            $('#<%= hidLatitude.ClientID %>'),
            $('#<%= hidLongitude.ClientID %>'));
    }
</script>
