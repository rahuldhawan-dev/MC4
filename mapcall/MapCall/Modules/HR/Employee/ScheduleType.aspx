<%@ Page Title="Schedule Types" Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="ScheduleType.aspx.cs" Inherits="MapCall.Modules.HR.Employee.ScheduleType" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls" TagPrefix="mapcall" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls.SearchFields" TagPrefix="search" %>
<%@ Register Src="~/Controls/DetailsViewDataPageTemplate.ascx" TagPrefix="mapcall" TagName="DetailsViewDataPageTemplate" %>
<asp:Content ID="Content3" ContentPlaceHolderID="cphHeader" runat="server">
Schedule Types
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphMain" runat="server">
     <mapcall:DetailsViewDataPageTemplate ID="template" runat="server" 
        DataElementTableName="ScheduleType" 
        DataElementPrimaryFieldName="ScheduleTypeID"
        Label="Schedule Type">
        
        <SearchBox>
            <Fields>
                <search:TextSearchField DataFieldName="ScheduleType" />
            </Fields>
        </SearchBox>

        <ResultsGridView>
            <Columns>
                <mapcall:BoundField DataField="ScheduleTypeID" />
                <mapcall:BoundField DataField="ScheduleType" />
            </Columns>
        </ResultsGridView>
        
        <ResultsDataSource SelectCommand="select * from ScheduleType" />

        <DetailsView>
            <Fields>
                <mapcall:BoundField DataField="ScheduleTypeID" ReadOnly="true" InsertVisible="false" />
                <mapcall:BoundField DataField="ScheduleType" MaxLength="50" />
            </Fields>
        </DetailsView>

        <DetailsDataSource
            InsertCommand="insert into [ScheduleType] ([ScheduleType]) Values (@ScheduleType); select @ScheduleTypeID = (Select @@IDENTITY)"
            UpdateCommand="update [ScheduleType] set [ScheduleType] = @ScheduleType where [ScheduleTypeID] = @ScheduleTypeID"
            DeleteCommand="delete from [ScheduleType] where ScheduleTypeID = @ScheduleTypeID"
            SelectCommand="select * from [ScheduleType] where ScheduleTypeID = @ScheduleTypeID"
            >
            <SelectParameters>
                <asp:Parameter Name="ScheduleTypeID" Type="Int32" />
            </SelectParameters>
            <DeleteParameters>
                <asp:Parameter Name="ScheduleTypeID" Type="Int32" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="ScheduleTypeID" Type="Int32" Direction="Output" />
                <asp:Parameter Name="ScheduleType" Type="String" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="ScheduleTypeID" Type="Int32" />
                <asp:Parameter Name="ScheduleType" Type="String" />
            </UpdateParameters>
        </DetailsDataSource>

    </mapcall:DetailsViewDataPageTemplate>
</asp:Content>
