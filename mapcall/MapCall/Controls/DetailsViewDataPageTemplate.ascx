<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DetailsViewDataPageTemplate.ascx.cs" Inherits="MapCall.Controls.DetailsViewDataPageTemplate" %>
<%@ Register Assembly="MMSINC.Core.WebForms" Namespace="MMSINC.Controls" TagPrefix="mmsi" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls" TagPrefix="mapcall" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls.Data" TagPrefix="mapcall" %>
<%@ Register Src="~/Controls/Data/Notes.ascx" TagName="Notes" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Documents/Documents.ascx" TagName="Documents" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/HR/employees.ascx" TagName="Employees" TagPrefix="mmsi" %>

<mmsi:MvpPlaceHolder ID="placeHome" runat="server">
    <div class="container boxContainer bc-2col">
        <div class="bc-bigLink">
            <a class="bc-box-search" href="<%= PathHelper.GetNewSearchUrl() %>">
                <span>Search <%= this.Label %>s</span><%--This is a pluralization hack until we actually need real pluralization--%>
                <p>Find <%=this.Label%>s based on criteria.</p>
            </a>
        </div>
        <%--TODO: Hide this when users can't add records!--%>
        <div class="bc-bigLink">
            <a class="newDoc" href="<%= PathHelper.GetCreateNewRecordUrl() %>">
                <span>Add a new <%= this.Label %></span>
                <p>Create a new <%=this.Label%> record if the one you're looking for doesn't already exist.</p>
            </a>
        </div>
    </div>
    <mmsi:MvpPlaceHolder ID="phHome" runat="server" />
</mmsi:MvpPlaceHolder>

<mmsi:MvpPanel ID="pnlSearch" runat="server" ClientIDMode="Static" EnableViewState="false">
    <div class="container">
        <div class="dualCol buttonSplit">
            <div class="right">
                <asp:PlaceHolder ID="phSearchButtonsRight" runat="server" />
                <%= RenderHelper.RenderCreateNewRecordButton(String.Format("Add New {0}", this.Label))%>
            </div>
        </div>
    </div>
    
    <%-- This is dumb and I hate it and it needs to die. --%>
    <asp:PlaceHolder ID="phSearchHelp" runat="server" />

    <mapcall:SearchBox ID="searchBox" runat="server" CssClass="prettyTable noBorders centered searchBox" />

    <div class="buttonContainer">
        <mmsi:MvpButton ID="btnSearch" runat="server" Text="Search" CausesValidation="true" ClientIDMode="Static" />
        <%= RenderHelper.RenderLinkButton(GetBaseUrl(), "Reset") %>
    </div>
</mmsi:MvpPanel>

<mmsi:MvpPanel ID="pnlResults" runat="server" ClientIDMode="Static">
    <div class="container">
        <div class="dualCol buttonSplit">
            <div class="left">
                <%= RenderHelper.RenderExportToExcelButton() %>
                <%= RenderMapLinkButton() %> <%--Renders when ShowMapButton = true--%>
                <%= RenderHelper.RenderNewSearchButton()%>
                <asp:PlaceHolder ID="phResultsButtons" runat="server" />            
            </div>
            <div class="right">
                <%= RenderHelper.RenderCreateNewRecordButton(String.Format("Add New {0}", this.Label))%>
            </div>
        </div>
    </div>
    <% if (resultsGrid.Visible)
       {
           %> 
           <span>Total Records: <%= ResultCount %></span>
           <%
       } %>
    
    <mmsi:MvpGridView ID="resultsGrid" runat="server" 
            AllowSorting="true"
            AutoGenerateColumns="false" 
            DataSourceID="resDataSource"
            EmptyDataText="There are no records to display"
            EnableViewState="false" 
            ShowEmptyTable="false"
            UseAccessibleHeader="true" 
             />
            
    <mapcall:McProdDataSource ID="resDataSource" runat="server" />
    
    <asp:PlaceHolder ID="phResultsPlaceHolder" runat="server" />
    
</mmsi:MvpPanel>

<mmsi:MvpPanel ID="pnlDetails" runat="server" ClientIDMode="Static">
    <div class="container">
        <div class="dualCol buttonSplit">
            <div class="left">
                <%= RenderHelper.RenderNewSearchButton()%>
                <%= RenderHelper.RenderBackToResultsButton() %>
                <asp:PlaceHolder ID="phDetailsButtonsLeft" runat="server" />
            </div>
            <div class="right">
                <%= RenderHelper.RenderCreateNewRecordButton(String.Format("Add New {0}", this.Label))%>
            </div>
        </div>
        <div class="error">
            <asp:Label runat="server" ID="lblErrorMessage"></asp:Label>
        </div>
    </div>

    <mapcall:TabView ID="tabView" runat="server">
        <mapcall:Tab ID="details" runat="server" Selected="true" VisibleDuringInsert="true" VisibleDuringUpdate="true">
            <asp:PlaceHolder ID="phDetailsView" runat="server" />
            <%--Do not rename this from 'detailView'. It is required by some ControlParameter instances.--%>
            <mmsi:MvpDetailsView ID="detailView" runat="server" 
                CssClass="prettyTable" 
                DataSourceID="detailDataSource" 
                AutoGenerateRows="false">
                <EmptyDataTemplate>
                    Record not found. 
                </EmptyDataTemplate>
                </mmsi:MvpDetailsView>
                
                <%--The Insert/Update/Delete buttons are created in
                DetailsViewDataPageBase automatically.--%>
                
            <mapcall:McProdDataSource ID="detailDataSource"
                                      runat="server" 
                                      CancelSelectOnNullParameter="true" />
        </mapcall:Tab>
        <%--Uh?--%>
        <mapcall:Tab runat="server" ID="employees" Label="Employees" VisibleDuringInsert="false" VisibleDuringUpdate="false">
            <mmsi:Employees ID="Employee1" runat="server" />
        </mapcall:Tab>
        <mapcall:Tab runat="server" ID="notes" Label="Notes" VisibleDuringInsert="false" VisibleDuringUpdate="false">
            <mmsi:Notes ID="Notes1" runat="server" />
        </mapcall:Tab>
        <mapcall:Tab runat="server" ID="documents" Label="Documents"  VisibleDuringInsert="false" VisibleDuringUpdate="false">
            <mmsi:Documents ID="Documents1" runat="server" />
        </mapcall:Tab>
    </mapcall:TabView>
    
    
</mmsi:MvpPanel>