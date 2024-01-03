<%@ Page Title="Schedule Type Details" Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="ScheduleTypeDetails.aspx.cs" Inherits="MapCall.Modules.HR.Employee.ScheduleTypeDetails" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls" TagPrefix="mapcall" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls.DropDowns" TagPrefix="mapcall" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls.SearchFields" TagPrefix="search" %>
<%@ Register Src="~/Controls/DetailsViewDataPageTemplate.ascx" TagPrefix="mapcall" TagName="DetailsViewDataPageTemplate" %>
<asp:Content ID="Content3" ContentPlaceHolderID="cphHeader" runat="server">
Schedule Type Descriptions
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphMain" runat="server">
<mapcall:DetailsViewDataPageTemplate ID="template" runat="server" 
        DataElementTableName="ScheduleTypeDetails" 
        DataElementPrimaryFieldName="ScheduleTypeDetailsID"
        Label="Schedule Type">
        
        <SearchBox>
            <Fields>
               <search:DropDownSearchField 
                   DataFieldName="ScheduleTypeDetails.ScheduleTypeID"
                   TableName="ScheduleType"
                   TextFieldName="ScheduleType"
                   ValueFieldName="ScheduleTypeID" />
               <search:NumericSearchField DataFieldName="Day" />
               <search:TextSearchField DataFieldName="StartTime" />
               <search:TextSearchField DataFieldName="EndTime" />
            </Fields>
        </SearchBox>

        <ResultsGridView>
            <Columns>
                <mapcall:BoundField DataField="ScheduleTypeDescription" />
                <mapcall:BoundField DataField="Day" />
                <mapcall:BoundField DataField="StartTime" />
                <mapcall:BoundField DataField="EndTime" />
            </Columns>
        </ResultsGridView>
        
        <ResultsDataSource 
            SelectCommand="select 
                                [ScheduleTypeDetails].*,
                                [ScheduleType].[ScheduleType] as [ScheduleTypeDescription]
                           from ScheduleTypeDetails
                           left join [ScheduleType] on [ScheduleType].[ScheduleTypeID] = [ScheduleTypeDetails].[ScheduleTypeID]" />

        <DetailsView>
            <Fields>
                <mapcall:BoundField DataField="ScheduleTypeDetailsID" ReadOnly="true" InsertVisible="false" />
                <mapcall:TemplateBoundField HeaderText="Schedule Type">
                    <ItemTemplate><%#Eval("ScheduleTypeDescription") %></ItemTemplate>
                    <EditItemTemplate>
                       <mapcall:DataSourceDropDownList ID="ddlScheduleTypes" runat="server"
                            TableName="ScheduleType"
                            TextFieldName="ScheduleType"
                            ValueFieldName="ScheduleTypeID"
                            SelectedValue='<%#Bind("ScheduleTypeID") %>' />
                    </EditItemTemplate>
                </mapcall:TemplateBoundField>
                <mapcall:BoundField DataField="Day" DataType="Int" />
                <mapcall:BoundField DataField="StartTime" MaxLength="10" />
                <mapcall:BoundField DataField="EndTime" MaxLength="10" />
            </Fields>
        </DetailsView>

        <DetailsDataSource
            InsertCommand="insert into [ScheduleTypeDetails] ([ScheduleTypeID], [Day], [StartTime], [EndTime]) Values (@ScheduleTypeID, @Day, @StartTime, @EndTime); select @ScheduleTypeDetailsID = (Select @@IDENTITY)"
            UpdateCommand="update [ScheduleTypeDetails] set [ScheduleTypeID] = @ScheduleTypeID, [Day] = @Day, [StartTime] = @StartTime, [EndTime] = @EndTime where [ScheduleTypeDetailsID] = @ScheduleTypeDetailsID"
            DeleteCommand="delete from [ScheduleTypeDetails] where ScheduleTypeDetailsID = @ScheduleTypeDetailsID"
            SelectCommand="select 
                                [ScheduleTypeDetails].*,
                                [ScheduleType].[ScheduleType] as [ScheduleTypeDescription]
                           from ScheduleTypeDetails
                           left join [ScheduleType] on [ScheduleType].[ScheduleTypeID] = [ScheduleTypeDetails].[ScheduleTypeID]
                           where ScheduleTypeDetailsID = @ScheduleTypeDetailsID"
            >
            <SelectParameters>
                <asp:Parameter Name="ScheduleTypeDetailsID" Type="Int32" />
            </SelectParameters>
            <DeleteParameters>
                <asp:Parameter Name="ScheduleTypeDetailsID" Type="Int32" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="ScheduleTypeDetailsID" Type="Int32" Direction="Output" />
                <asp:Parameter Name="ScheduleTypeID" Type="Int32" />
                <asp:Parameter Name="Day" Type="Int32" />
                <asp:Parameter Name="StartTime" Type="String" />
                <asp:Parameter Name="EndTime" Type="String" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="ScheduleTypeDetailsID" Type="Int32" />
                <asp:Parameter Name="ScheduleTypeID" Type="Int32" />
                <asp:Parameter Name="Day" Type="Int32" />
                <asp:Parameter Name="StartTime" Type="String" />
                <asp:Parameter Name="EndTime" Type="String" />
            </UpdateParameters>
        </DetailsDataSource>

    </mapcall:DetailsViewDataPageTemplate>
</asp:Content>
