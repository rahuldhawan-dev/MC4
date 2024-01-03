 <%@ Page Title="Tap Image Fixes" Language="C#" MasterPageFile="~/MapCallSite.Master" Theme="bender" AutoEventWireup="true" CodeBehind="Mismatches.aspx.cs" Inherits="MapCall.Modules.Data.Services.Mismatches" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls" TagPrefix="mapcall" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls.Data" TagPrefix="mapcall" %>
<%@ Register Src="~/Controls/Data/OpCntrDataField.ascx" TagPrefix="mapcall" TagName="OpCntrDataField" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphHeader" runat="server">
    Tap Image Fixes
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphInstructions" runat="server">
    Use this page to correct the links between the images and asset data.
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
        <h3>Service Record</h3>
        <asp:DetailsView runat="server" ID="detailsView"
            AutoGenerateRows="false" DataSourceID="dsService" 
            DataKeyNames="RecID" DefaultMode="Edit" >
            <Fields>
                <mapcall:BoundField DataField="RecID" ReadOnly="true" />
                <mapcall:BoundField DataField="OpCntr" ReadOnly="true" />
                <mapcall:BoundField DataField="Town" ReadOnly="true" />
                <mapcall:BoundField DataField="TownID" ReadOnly="true" />
                <mapcall:BoundField DataField="StNum" HeaderText="Street Number" ReadOnly="true" />
                <mapcall:BoundField DataField="FullStName" HeaderText="Street Name" ReadOnly="true" />
                <mapcall:BoundField DataField="ServNum" HeaderText="Service #" ReadOnly="true" />
                <mapcall:BoundField DataField="PremNum" HeaderText="Premise #" ReadOnly="true" />
                <mapcall:BoundField DataField="TwnSection" HeaderText="Town Section" ReadOnly="true" />
                <mapcall:BoundField DataField="Block" ReadOnly="true" />
                <mapcall:BoundField DataField="Lot" ReadOnly="true" />
                <mapcall:BoundField DataField="ServiceCategory" HeaderText="Cat of Service" ReadOnly="true" />
                <mapcall:BoundField DataField="DateInstalled" ReadOnly="true" />
                <mapcall:BoundField DataField="SizeOfService" ReadOnly="true" />
                <mapcall:BoundField DataField="SizeOfMain" ReadOnly="true" />
                <mapcall:BoundField DataField="TaskNum1" ReadOnly="true" />
                <mapcall:BoundField DataField="TaskNum2" ReadOnly="true" />
                <mapcall:BoundField DataField="Name" ReadOnly="true" />
                <asp:TemplateField HeaderText="Actions">
                    <ItemTemplate>
                        <asp:DropDownList runat="server" ID="ddlActions" SelectedValue='<%#Bind("ImageActionID") %>'>
                            <asp:ListItem Value="0" Text="-- Select Action --" />
                            <asp:ListItem Value="1" Text="Ignore" />
                            <asp:ListItem Value="2" Text="Further Review" />
                        </asp:DropDownList>
                        <asp:Button runat="server" ID="btnNext" Text="Next" OnClick="OnBtnNextClicked" 
                            OnClientClick="this.style.display='none';return true;return confirm('Are you sure you want to ignore this Service?');" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <a href='../../../njaw/system/Existing/LookUp2.asp?pid=<%# Eval("RecID") %>' target="_new">Asset Record</a>
                    </ItemTemplate>
                </asp:TemplateField>
            </Fields>
        </asp:DetailsView>
        <mapcall:McProdDataSource runat="server" ID="dsService"
            CancelSelectOnNullParameter="false"
            SelectCommand="
                Select Top 1 
                    T.Town, SC.[Description] as [ServiceCategory], 
                    SS.SizeServ as SizeOfService,
                    SM.SizeServ as SizeOfMain,
                    S.RecID, opc.OperatingCenterCode as OpCntr, T.Town, T.TownID,
                    S.StNum, ST.FullStName, S.ServNum, S.PremNum, S.TwnSection, S.Block, S.Lot,
                    S.DateInstalled, S.TaskNum1, S.TaskNum2, S.Name, ImageActionID
                from 
                    tblNJAWService S
                left join 
                    Towns T on T.TownID = S.Town
                left join 
                    Streets ST on ST.StreetID = S.stName
                left join 
                    OperatingCenters opc on opc.OperatingCenterID = S.OpCntr
                left join 
                    ServiceCategories SC on SC.ServiceCategoryID = S.CatofService
                left join 
                    tblNJAWSizeServ SS on SS.RecID = S.SizeofService
				left join 
                    tblNJAWSizeServ SM on SM.RecID = S.SizeofMain
                where 
                    SC.Description &lt;&gt; 'Replace Meter Set'
                and
                    SC.Description &lt;&gt; 'Water Retire Meter Set Only'
                and
                    SC.Description &lt;&gt; 'Water Retire Service Only'
                and
                    not exists (select 1 from TapImages where ServiceID = S.RecID)
                and
                    isNull(@OpCntr, opc.OperatingCenterID) = opc.OperatingCenterID
                and
                    isNull(@TownID, t.townID) = t.townID
                and
                    ImageActionID = @ImageActionID
                order by T.Town, StName
                "
            UpdateCommand="Update tblNJAWService
                           Set ImageActionID = @ImageActionID 
                           Where RecID = @RecID">
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
                <asp:ControlParameter ControlID="detailsView" Name="RecID" />
            </UpdateParameters>
        </mapcall:McProdDataSource>
    </div>

    <div>
        <h3>Tap Images For Address</h3>
        <asp:GridView runat="server" ID="gvImagesByAddress" DataKeyNames="TapImageID"
            EmptyDataText="There are no records for this street address"
            DataSourceID="dsImagesByAddress" AutoGenerateColumns="false">
            <Columns>
                <asp:TemplateField InsertVisible="false">
                    <ItemTemplate>
                        <a href='../../Mvc/FieldOperations/TapImage/Show/<%#Eval("TapImageID")%>.pdf' target="_blank">View Image</a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField InsertVisible="false">
                    <ItemTemplate>
                        <asp:CheckBox ID="chkMate" runat="server"  />
                    </ItemTemplate>
                </asp:TemplateField>
                <mapcall:BoundField DataField="OperatingCenter" />
                <mapcall:BoundField DataField="Town" />
                <mapcall:BoundField DataField="StreetPrefix" />
                <mapcall:BoundField DataField="StreetNumber" />
                <mapcall:BoundField DataField="Street" />
                <mapcall:BoundField DataField="StreetSuffix" />
                <mapcall:BoundField DataField="PremiseNumber" />
                <mapcall:BoundField DataField="ServiceNumber" />
                <mapcall:BoundField DataField="Block" />
                <mapcall:BoundField DataField="Lot" />
                <mapcall:BoundField DataField="DateCompleted" />
            </Columns>
        </asp:GridView>
        <mapcall:McProdDataSource runat="server" ID="dsImagesByAddress"
            SelectCommand="
                select 
	                *, oc.OperatingCenterCode as [OperatingCenter] 
                from 
	                TapImages T
                left join OperatingCenters oc on oc.OperatingCenterId = t.OperatingCenterID
                left join Towns town on town.TownId = t.TownID
                left join Counties c on c.CountyId = town.CountyId
                left join States state on state.StateId = c.StateId
                left join
                    tblNJAWService S
                on
	                T.Street = (Select StreetName from Streets where StreetID = S.StName and Streets.TownID = S.Town)
                AND 
	                T.TownID = S.Town
                AND 
	                T.StreetNumber = S.StNum
                AND
	                ServiceID is null
                WHERE
                    S.RecID = @RecID
                ORDER BY
                    PremiseNumber, ServiceNumber">
            <SelectParameters>
                <asp:ControlParameter ControlID="detailsView" Name="RecID" PropertyName="SelectedValue" />
            </SelectParameters>
            <UpdateParameters>
                <asp:ControlParameter ControlID="detailsView" Name="RecID" PropertyName="SelectedValue" />
                <asp:Parameter Name="TapImageID"  />
            </UpdateParameters>
        </mapcall:McProdDataSource>
    </div>

    <div>
        <h3>Tap Images for Premise/Service #</h3>
        <asp:GridView runat="server" ID="gvImagesByPremServ" DataKeyNames="TapImageID"
            EmptyDataText="There are no records for this Premise/Service #"
            DataSourceID="dsImagesByPremServ" AutoGenerateColumns="false">
        <Columns>
            <asp:TemplateField InsertVisible="false">
                <ItemTemplate>
                    <a href='../../Mvc/FieldOperations/TapImage/Show/<%#Eval("TapImageID")%>.pdf' target="_blank">View Image</a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField InsertVisible="false">
                <ItemTemplate>
                    <asp:CheckBox ID="chkMate" runat="server"  />
                </ItemTemplate>
            </asp:TemplateField>
            <mapcall:BoundField DataField="OperatingCenter" />
            <mapcall:BoundField DataField="Town" />
            <mapcall:BoundField DataField="StreetPrefix" />
            <mapcall:BoundField DataField="StreetNumber" />
            <mapcall:BoundField DataField="Street" />
            <mapcall:BoundField DataField="StreetSuffix" />
            <mapcall:BoundField DataField="PremiseNumber" />
            <mapcall:BoundField DataField="ServiceNumber" />
            <mapcall:BoundField DataField="Block" />
            <mapcall:BoundField DataField="Lot" />
            <mapcall:BoundField DataField="DateCompleted" />
        </Columns>
        </asp:GridView>
        <mapcall:McProdDataSource runat="server" ID="dsImagesByPremServ"
            SelectCommand="
                select 
                    *, oc.OperatingCenterCode as [OperatingCenter]
                from 
                    TapImages T
                left join OperatingCenters oc on oc.OperatingCenterId = t.OperatingCenterID
                left join Towns town on town.TownId = t.TownID
                left join Counties c on c.CountyId = town.CountyId
                left join States state on state.StateId = c.StateId
                left join
                    tblNJAWService S
                on
                    cast(T.PremiseNumber as varchar(20)) = cast(S.PremNum as varchar(20))
                AND 
                    cast(T.ServiceNumber as varchar(20))= cast(S.ServNum as varchar(20))
                AND
                    ServiceID is null
                WHERE
                    S.RecID = @recID
                ORDER BY
                    PremiseNumber, ServiceNumber">
            <SelectParameters>
                <asp:ControlParameter ControlID="detailsView" Name="RecID" PropertyName="SelectedValue" />
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

    <mapcall:McProdDataSource ID="dsImageUpdate" runat="server" 
        UpdateCommand="
            update
                TapImages
            set
                ServiceID = @recID
            where
                TapImageID = @TapImageID">
        <UpdateParameters>
            <asp:ControlParameter ControlID="detailsView" Name="RecID" PropertyName="SelectedValue" />
            <asp:Parameter Name="TapImageID"  />
        </UpdateParameters>
    </mapcall:McProdDataSource>

</asp:Content>
