<%@ Page Title="AsBuilt Image Link" Theme="bender" Language="C#" MasterPageFile="~/MapCallSite.Master" CodeBehind="AsBuiltLink.aspx.cs" Inherits="MapCall.Modules.Data.AsBuilts.AsBuiltLink" EnableEventValidation="false" AutoEventWireup="true" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls" TagPrefix="mapcall" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphHeader" runat="server">
    AsBuilt Images
</asp:Content>

<asp:Content ContentPlaceHolderID="cphMain" runat="server">
    <div class="results">
        <asp:GridView runat="server" ID="gvAsbuilt" AutoGenerateColumns="False"
        DataSourceID="dsAsbuilt" EmptyDataText="No matches were found for this Id">
        <Columns>
            <asp:HyperLinkField HeaderText="Actions"
                Text="View PDF" Target="_new"
                DataNavigateUrlFields="AsBuiltImageID"
                DataNavigateUrlFormatString="~/Modules/Mvc/FieldOperations/AsBuiltImage/Show/{0}.pdf"
            />
            <mapcall:BoundField DataField="DateInstalled" />
            <mapcall:BoundField DataField="TownName" />
            <mapcall:BoundField DataField="TownSection" />
            <mapcall:BoundField DataField="StreetPrefix" />
            <mapcall:BoundField DataField="Street" />
            <mapcall:BoundField DataField="StreetSuffix" />
            <mapcall:BoundField DataField="CrossStreet" />
            <mapcall:BoundField DataField="TaskNumber" />
        </Columns>
    </asp:GridView>
        <asp:SqlDataSource runat="server" ID="dsAsBuilt"
            CancelSelectOnNullParameter="false"
            ConnectionString="<%$ ConnectionStrings:MCProd %>"
            SelectCommand="
            select 
                ab.*, 
                town.StateID,
                town.Town as TownName,
                oc.OPeratingCenterCode
            from [AsBuiltImages] ab
            left join [Towns] town on town.TownID = ab.TownID
            left join [Counties] c on c.CountyID = town.CountyID
            left join [OperatingCenters] oc on oc.OperatingCenterID = ab.OperatingCenterID
            where AsBuiltImageID = @AsBuiltImageID">
            <SelectParameters>
                <asp:QueryStringParameter Name="AsBuiltImageID" Type="Int32" 
                    QueryStringField="AsBuiltImageID" ConvertEmptyStringToNull="true" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>

</asp:Content>
