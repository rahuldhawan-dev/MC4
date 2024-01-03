<%@ Page Title="" Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="PipeDataLookupValues.aspx.cs" Inherits="MapCall.Modules.Engineering.PipeDataLookupValues" %>
<%@ Register TagPrefix="mapcall" TagName="DetailsViewDataPageTemplate" Src="~/Controls/DetailsViewDataPageTemplate.ascx" %>
<%@ Register TagPrefix="mapcall" Namespace="MapCall.Controls.DropDowns" Assembly="MapCall" %>
<%@ Register TagPrefix="mapcall" Namespace="MapCall.Controls" Assembly="MapCall" %>
<%@ Register TagPrefix="search" Namespace="MapCall.Controls.SearchFields" Assembly="MapCall" %>
<asp:Content ID="Content3" ContentPlaceHolderID="cphHeader" runat="server">
    Pipe Data Lookup Values
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="cphInstructions">
    Setting a new or existing value to default will reset the default flag for all the other values of the same type.
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphMain" runat="server">
<mapcall:DetailsViewDataPageTemplate ID="template" runat="server"
    DataElementTableName="PipeDataLookupValues" 
    DataElementPrimaryFieldName="PipeDataLookupValueID"
    Label="Pipe Data Lookup Values">
    <SearchBox>
        <Fields>
            <search:DropDownSearchField
                Label="Lookup Type"
                DataFieldName="v.PipeDataLookupTypeID"
                TableName="PipeDataLookupTypes"
                TextFieldName="Description" 
                ValueFieldName="PipeDataLookupTypeID"  />                
        </Fields>
    </SearchBox>
    <ResultsGridView>
        <Columns>
            <mapcall:BoundField HeaderText="PipeDataLookupValueID" DataField="PipeDataLookupValueID" ReadOnly="True" InsertVisible="False"/>
            <mapcall:BoundField DataField="PipeDataLookupType"/>
            <mapcall:BoundField DataField="Description"/>
            <mapcall:BoundField DataField="VariableScore"/>
            <mapcall:BoundField DataField="PriorityWeightedScore"/>
            <asp:CheckBoxField DataField="IsEnabled" HeaderText="Enabled" ReadOnly="True"/>
            <asp:CheckBoxField DataField="IsDefault" HeaderText="Default" ReadOnly="True"/>
        </Columns>
    </ResultsGridView>
    
    <ResultsDataSource SelectCommand="
        SELECT 
            v.*,
            t.Description as PipeDataLookupType
        FROM 
            [PipeDataLookupValues] v
        JOIN
            [PipeDataLookupTypes] t on t.PipeDataLookupTypeID = v.PipeDataLookupTypeID
        " />
    
    <DetailsView>
        <Fields>
            <mapcall:BoundField HeaderText="PipeDataLookupValueID" DataField="PipeDataLookupValueID" ReadOnly="True" InsertVisible="False"/>
            <mapcall:TemplateBoundField HeaderText="Lookup Type">
                <ItemTemplate><%#Eval("PipeDataLookupType") %></ItemTemplate>
                <EditItemTemplate>
                    <mapcall:DataSourceDropDownList ID="ddlInsertPipeDataLookupType" runat="server" 
                        TableName="PipeDataLookupTypes"
                        TextFieldName="Description" 
                        ValueFieldName="PipeDataLookupTypeID" 
                        SelectedValue='<%#Bind("PipeDataLookupTypeID") %>' />
                    <asp:RequiredFieldValidator runat="server" 
                        ValidationGroup="InsertPipeDataLookupValue"
                        ControlToValidate="ddlInsertPipeDataLookupType" Text="Required" />
                </EditItemTemplate>
            </mapcall:TemplateBoundField>
            <mapcall:BoundField DataField="Description" Required="True" />
            <mapcall:BoundField DataField="VariableScore" DataType="Decimal" Required="True" />
            <mapcall:BoundField DataField="PriorityWeightedScore" DataType="Decimal" Required="True" />
            <asp:CheckBoxField DataField="IsEnabled" HeaderText="Enabled" />
            <asp:CheckBoxField DataField="IsDefault" HeaderText="Default" />
        </Fields>
    </DetailsView>
    
    <DetailsDataSource CancelSelectOnNullParameter="true" 
                DeleteCommand="DELETE FROM [PipeDataLookupValues] WHERE [PipeDataLookupValueID] = @PipeDataLookupValueID" 
                InsertCommand="
                    IF (@IsDefault = 1)
                        UPDATE [PipeDataLookupValues] SET IsDefault = 0 WHERE [PipeDataLookupTypeID] = @PipeDataLookupTypeID;
                    INSERT INTO [PipeDataLookupValues] ([PipeDataLookupTypeID], [Description], [VariableScore], [PriorityWeightedScore], [IsEnabled], [IsDefault]) 
                        VALUES (@PipeDataLookupTypeID, @Description, @VariableScore, @PriorityWeightedScore, @IsEnabled, @IsDefault); 
                    SELECT @PipeDataLookupValueID = (SELECT @@IDENTITY)" 
                SelectCommand="
                    SELECT 
                        v.*,
                        t.Description as PipeDataLookupType
                    FROM 
                        [PipeDataLookupValues] v
                    JOIN
                        [PipeDataLookupTypes] t on t.PipeDataLookupTypeID = v.PipeDataLookupTypeID
                    WHERE 
                        PipeDataLookupValueID = @PipeDataLookupValueID" 
                UpdateCommand="
                    IF (@IsDefault = 1)
                        UPDATE [PipeDataLookupValues] SET IsDefault = 0 WHERE [PipeDataLookupTypeID] = @PipeDataLookupTypeID;
                    UPDATE 
                        [PipeDataLookupValues] 
                    SET 
                        [PipeDataLookupTypeID] = @PipeDataLookupTypeID,
                        [Description] = @Description,
                        [VariableScore] = @VariableScore,
                        [PriorityWeightedScore] = @PriorityWeightedScore,
                        [IsEnabled] = @IsEnabled,
                        [IsDefault] = @IsDefault
                    WHERE 
                        [PipeDataLookupValueID] = @PipeDataLookupValueID">
        <SelectParameters>
            <asp:Parameter Name="PipeDataLookupValueID" Type="Int32" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="PipeDataLookupValueID" Type="Int32" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="PipeDataLookupTypeID" Type="Int32" />
            <asp:Parameter Name="Description" Type="String" />
            <asp:Parameter Name="VariableScore" Type="Decimal" />
            <asp:Parameter Name="PriorityWeightedScore" Type="Decimal" />
            <asp:Parameter Name="IsEnabled" Type="Boolean" />
            <asp:Parameter Name="IsDefault" Type="Boolean" />
            <asp:Parameter Name="PipeDataLookupValueID" Type="Int32" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="PipeDataLookupValueID" Type="Int32" Direction="Output" />
            <asp:Parameter Name="PipeDataLookupTypeID" Type="Int32" />
            <asp:Parameter Name="Description" Type="String" />
            <asp:Parameter Name="VariableScore" Type="Decimal" />
            <asp:Parameter Name="PriorityWeightedScore" Type="Decimal" />
            <asp:Parameter Name="IsEnabled" Type="Boolean" />
            <asp:Parameter Name="IsDefault" Type="Boolean" />
        </InsertParameters>
    </DetailsDataSource>
    
</mapcall:DetailsViewDataPageTemplate>

</asp:Content>
