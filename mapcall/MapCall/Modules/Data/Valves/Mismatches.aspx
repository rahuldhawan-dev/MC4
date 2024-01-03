<%@ Page Title="Valve Image Fixes" Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" Theme="bender" CodeBehind="Mismatches.aspx.cs" Inherits="MapCall.Modules.Data.Valves.Mismatches" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls" TagPrefix="mapcall" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls.Data" TagPrefix="mapcall" %>
<%@ Register Src="~/Controls/Data/OpCntrDataField.ascx" TagPrefix="mapcall" TagName="OpCntrDataField" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphHeader" runat="server">
Valve Image Fixes
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphMain" runat="server">

    <div>
        <h3>Filter/Search</h3>
        <mapcall:OpCntrDataField runat="server" id="ddlOperatingCenter" UseTowns="true" />
        <asp:DropDownList runat="server" ID="ddlActionSearch">
            <asp:ListItem Value="0" Text="--Select Here--" />
            <asp:ListItem Value="1" Text="Ignore" />
            <asp:ListItem Value="2" Text="Further Review" />
        </asp:DropDownList>
        <asp:Button runat="server" ID="btnFilter" Text="Filter" OnClick="onBtnFilterClick" />
    </div>

    <div>
        <h3>Valve Record</h3>
        <asp:DetailsView runat="server" ID="detailsView"
            AutoGenerateRows="false" DataSourceID="dsValve"
            DataKeyNames="Id" DefaultMode="Edit" >
            <Fields>
                <mapcall:BoundField DataField="Id" ReadOnly="true" />
                <mapcall:BoundField DataField="OperatingCenterCode" ReadOnly="true" />
                <mapcall:BoundField DataField="Town" ReadOnly="true" />
                <mapcall:BoundField DataField="TownID" ReadOnly="true" />
                <mapcall:BoundField DataField="StreetNumber" HeaderText="Street Number" ReadOnly="true" />
                <mapcall:BoundField DataField="FullStName" HeaderText="Street Name" ReadOnly="true" />
                <mapcall:BoundField DataField="CrossStreet" ReadOnly="true" />
                <mapcall:BoundField DataField="ValveNumber" HeaderText="Valve #" ReadOnly="true" />
                <mapcall:BoundField DataField="Opens" HeaderText="Opens" ReadOnly="true" />
                <mapcall:BoundField DataField="TownSection" HeaderText="Town Section" ReadOnly="true" />
                <mapcall:BoundField DataField="Turns" ReadOnly="true" />
                <mapcall:BoundField DataField="NormalPosition" HeaderText="Normal Position" ReadOnly="true" />
                <mapcall:BoundField DataField="ValveSize" ReadOnly="true" />
                <mapcall:BoundField DataField="WorkOrderNumber" HeaderText="Work Order #" ReadOnly="true" />
                <asp:TemplateField HeaderText="Actions">
                    <ItemTemplate>
                        <asp:DropDownList runat="server" ID="ddlActions" SelectedValue='<%#Bind("ImageActionID") %>'>
                            <asp:ListItem Value="0" Text="-- Select Action --" />
                            <asp:ListItem Value="1" Text="Ignore" />
                            <asp:ListItem Value="2" Text="Further Review" />
                        </asp:DropDownList>
                        <asp:Button runat="server" ID="btnNext" Text="Next" OnClick="OnBtnNextClicked" 
                            OnClientClick="this.style.display='none';return true;return confirm('Are you sure you want to ignore this Valve?');" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <a href='../../../modules/mvc/fieldoperations/valve/show/<%# Eval("Id") %>' target="_new">Asset Record</a>
                    </ItemTemplate>
                </asp:TemplateField>
            </Fields>
        </asp:DetailsView>
        <mapcall:McProdDataSource runat="server" ID="dsValve"
            CancelSelectOnNullParameter="false"
            SelectCommand="
                Select Top 1 
                    T.Town,
                    T.TownID, 
                    V.*,
                    ST.FullStName,
                    CS.FullStName as CrossStreet,
                    opc.OperatingCenterCode,
                    vd.Description as Opens,
                    ts.Name as TownSection,
                    vnp.Description as NormalPosition,
                    vs.Size as ValveSize
                from 
                    Valves V
                left join Towns                 T on T.TownID = V.Town
                left join Streets               ST on  ST.StreetID = V.StreetId
                left join Streets               CS on CS.StreetId = V.CrossStreetID
                left join OperatingCenters      opc on opc.OperatingCenterId = V.OperatingCenterId
                left join ValveOpenDirections   vd on vd.Id = V.OpensId
                left join TownSections          ts on ts.TownSectionId = v.TownSectionId
                left join ValveNormalPositions  vnp on vnp.Id = v.NormalPositionId
                left join ValveSizes            vs on vs.Id = V.ValveSizeId
                where 
                    not exists (select 1 from ValveImages where ValveID = V.Id)
                and
                    isNull(@OpCntr, opc.OperatingCenterID) = opc.OperatingCenterID
                and
                    isNull(@TownID, t.townID) = t.townID
                and
                    imageActionID = @imageActionID
                order by
                    opc.OperatingCenterCode, T.Town, V.StreetId"
            UpdateCommand="Update Valves Set [ImageActionID] = @ImageActionID Where Id = @Id">
            <SelectParameters>
                <asp:ControlParameter Name="OpCntr" ControlID="ddlOperatingCenter" 
                    ConvertEmptyStringToNull="true" PropertyName="SelectedOperatingCenter" 
                    DbType="String" />
                <asp:ControlParameter Name="TownID" ControlID="ddlOperatingCenter" 
                    ConvertEmptyStringToNull="true" PropertyName="SelectedTown" 
                    DbType="String" />
                <asp:ControlParameter Name="ImageActionID" ControlID="ddlActionSearch"
                    ConvertEmptyStringToNull="false" PropertyName="SelectedValue"
                    DbType="Int32" />
            </SelectParameters>
            <UpdateParameters>
                <asp:ControlParameter ControlID="detailsView" Name="Id" />
                <asp:Parameter Name="ImageActionID" DbType="Int32" />
            </UpdateParameters>
        </mapcall:McProdDataSource>
    </div>

    <div>
        <h3>Valve Images For Address</h3>
        <asp:GridView runat="server" ID="gvImagesByAddress" DataKeyNames="ValveImageID"
            EmptyDataText="There are no records for this street address"
            DataSourceID="dsImagesByAddress" AutoGenerateColumns="false">
            <Columns>
                <asp:TemplateField InsertVisible="false">
                    <ItemTemplate>
                        <a href="/Modules/Mvc/FieldOperations/ValveImage/Show/<%#Eval("ValveImageID")%>.pdf" target="_blank">View Image</a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField InsertVisible="false">
                    <ItemTemplate>
                        <asp:CheckBox ID="chkMate" runat="server"  />
                    </ItemTemplate>
                </asp:TemplateField>
                <mapcall:BoundField DataField="Id" />
                <mapcall:BoundField DataField="OperatingCenter" />
                <mapcall:BoundField DataField="Town" />
                <%--Needs to display ValveImages.ValveNumber, not Valves.ValNum--%>
                <mapcall:BoundField DataField="ValveNumber" />
                <mapcall:BoundField DataField="StreetPrefix" />
                <mapcall:BoundField DataField="StreetNumber" />
                <mapcall:BoundField DataField="Street" />
                <mapcall:BoundField DataField="StreetSuffix" />
                <mapcall:BoundField DataField="CrossStreet" />
                <mapcall:BoundField DataField="MainSize" />
                <mapcall:BoundField DataField="DateCompleted" />
                <mapcall:BoundField DataField="InstallationDate" />
                <mapcall:BoundField DataField="CreatedAt" />
            </Columns>
        </asp:GridView>
        <mapcall:McProdDataSource runat="server" ID="dsImagesByAddress"
            SelectCommand="
                select 
	                *,
                    oc.OperatingCenterCode as [OperatingCenter]
                from 
	                ValveImages VI
                left join OperatingCenters oc on oc.OperatingCenterId = VI.OperatingCenterID
                left join Towns t on t.TownId = VI.TownID
                left join Counties c on c.CountyId = t.CountyId
                left join States s on s.StateId = c.StateId
                left join
                    Valves V
                on
	                VI.Street = (Select StreetName from Streets where StreetID = V.StreetId and Streets.TownID = V.Town)
                AND 
	                V.Town = VI.TownID
                AND
	                ValveID is null
                WHERE
                    V.Id = @Id">
                <SelectParameters>
                    <asp:ControlParameter ControlID="detailsView" Name="Id" PropertyName="SelectedValue" />
                </SelectParameters>
            </mapcall:McProdDataSource>
    </div>

    <div>
        <h3>Valve Images For Valve Number</h3>
        <asp:GridView runat="server" ID="gvImagesByValveNumber" DataKeyNames="ValveImageID"
            EmptyDataText="There are no records for this street address"
            DataSourceID="dsImagesByValveNumber" AutoGenerateColumns="false">
            <Columns>
                <asp:TemplateField InsertVisible="false">
                    <ItemTemplate>
                        <a href="/Modules/Mvc/FieldOperations/ValveImage/Show/<%#Eval("ValveImageID")%>.pdf" target="_blank">View Image</a>
                    </ItemTemplate>
                </asp:TemplateField>
                <mapcall:BoundField DataField="OperatingCenter" />
                <mapcall:BoundField DataField="Town" />
                <%--Needs to display ValveImages.ValveNumber, not Valves.ValNum--%>
                <mapcall:BoundField DataField="ValveNumber" />
                <mapcall:BoundField DataField="StreetPrefix" />
                <mapcall:BoundField DataField="StreetNumber" />
                <mapcall:BoundField DataField="Street" />
                <mapcall:BoundField DataField="StreetSuffix" />
                <mapcall:BoundField DataField="CrossStreet" />
                <mapcall:BoundField DataField="MainSize" />
                <mapcall:BoundField DataField="DateCompleted" />
                <mapcall:BoundField DataField="InstallationDate" />
                <mapcall:BoundField DataField="CreatedAt" />
            </Columns>
        </asp:GridView>
        <mapcall:McProdDataSource runat="server" ID="dsImagesByValveNumber"
            SelectCommand="
                select 
                    *,
                    oc.OperatingCenterCode as [OperatingCenter] 
                from 
                    ValveImages VI
                left join OperatingCenters oc on oc.OperatingCenterId = VI.OperatingCenterID
                left join Towns t on t.TownId = VI.TownID
                left join Counties c on c.CountyId = t.CountyId
                left join States s on s.StateId = c.StateId
                left join
                    Valves V
                on
                    VI.ValveNumber = V.ValveNumber 
                AND
                    ValveID is null
                WHERE
                    V.Id = @Id">
                <SelectParameters>
                <asp:ControlParameter ControlID="detailsView" Name="Id" PropertyName="SelectedValue" />
            </SelectParameters>
        </mapcall:McProdDataSource>
    </div>


    <% if (HasImagesToLink)
       { // Wanna hide this button if there's nothing to link.
    %>
    <div style="padding-top:6px;">
        <asp:Button ID="btnLinkImages" runat="server" Text="Link Selected Images" OnClick="OnBtnLinkImagesClicked" />
    </div>
    <% } %>

    <mapcall:McProdDataSource ID="dsValveImageUpdate" runat="server" 
        UpdateCommand="
            update
                ValveImages
            set
                valveID = @Id
            where
                ValveImageID = @ValveImageID">
        <UpdateParameters>
            <asp:ControlParameter ControlID="detailsView" Name="Id" PropertyName="SelectedValue" />
            <asp:Parameter Name="ValveImageID"  />
        </UpdateParameters>
    </mapcall:McProdDataSource>

</asp:Content>
