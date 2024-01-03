<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PositionPosting.ascx.cs" Inherits="MapCall.Controls.HR.PositionPosting" %>
<%@ Register Src="~/Controls/HR/dropdownlists/ddlPositions.ascx" TagName="ddlPositions" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/HR/dropdownlists/ddlEmployees.ascx" TagName="ddlEmployees" TagPrefix="mmsi" %>
<%@ Register assembly="MMSINC" namespace="MMSINC" tagPrefix="mmsinc" %>

<script type="text/javascript"> 
function PosPos_CheckEffective(objEffectiveDate, objChk1, objChk2, objChk3, objChk4)
{
    //alert('ok');
    if (document.getElementById(objEffectiveDate).value.length>0) 
    {
        document.getElementById(objChk1).checked=false;
        document.getElementById(objChk2).checked=false;
        document.getElementById(objChk3).checked=false;
        document.getElementById(objChk4).checked=false;
        document.getElementById(objChk1).disabled=true;
        document.getElementById(objChk2).disabled=true;
        document.getElementById(objChk3).disabled=true;
        document.getElementById(objChk4).disabled=true;
    }
    else
    {
        document.getElementById(objChk1).disabled=false;
        document.getElementById(objChk2).disabled=false;
        document.getElementById(objChk3).disabled=false;
        document.getElementById(objChk4).disabled=false;
    }
}
</script>
<asp:DetailsView ID="DetailsView1" runat="server" AutoGenerateRows="False" DataKeyNames="Position_Posting_ID" DataSourceID="SqlDataSource1"
    Width="100%"
    OnDataBound="DetailsView1_DataBound" 
    FieldHeaderStyle-Width="100px" 
>
    <Fields>
        <asp:BoundField DataField="Position_Posting_ID" HeaderText="Position_Posting_ID" InsertVisible="False" ReadOnly="True" SortExpression="Position_Posting_ID" />
        <asp:BoundField DataField="Position_Control_Number" HeaderText="Position_Control_Number" SortExpression="Position_Control_Number" />
        <asp:TemplateField HeaderText="Position" SortExpression="Position">
            <EditItemTemplate>
                <mmsi:ddlPositions ID="ddlPositions" runat="server" SelectedValue='<%# Bind("Position_ID") %>' />
            </EditItemTemplate>
            <InsertItemTemplate>
                <mmsi:ddlPositions ID="ddlPositions" runat="server" SelectedValue='<%# Bind("Position_ID") %>' />
            </InsertItemTemplate>
            <ItemTemplate>
                <asp:Label ID="lblPosition" runat="server" Text='<%# Bind("Position") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <mmsinc:BoundField SqlDataType="DateTime" DataFormatString="{0:d}" HtmlEncode="False" DataField="Date_of_Posting" HeaderText="Date_of_Posting" SortExpression="Date_of_Posting" />
        <asp:TemplateField HeaderText="OpCode" SortExpression="opCode">
            <EditItemTemplate>
                <asp:Label ID="lblOpCode" runat="server" Text='<%# Eval("opCode") %>'></asp:Label>
            </EditItemTemplate>
            <InsertItemTemplate>
                <asp:Label runat="Server" ID="labelopcode" Text='Defined by position'></asp:Label>
            </InsertItemTemplate>
            <ItemTemplate>
                <asp:Label ID="lblOpCode" runat="server" Text='<%# Bind("opCode") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Local" SortExpression="Local">
            <EditItemTemplate>
                <asp:Label ID="lblLocal" runat="server" Text='<%# Eval("Local") %>'></asp:Label>
            </EditItemTemplate>
            <InsertItemTemplate>
                <asp:Label ID="lblLocal" runat="server" Text='Defined by position'></asp:Label>
            </InsertItemTemplate>
            <ItemTemplate>
                <asp:Label ID="Label3" runat="server" Text='<%# Bind("Local") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Position Awarded To">
            <ItemTemplate><asp:Label ID="lblAwardedTo" runat="server" Text='<%# Bind("fullname") %>'></asp:Label></ItemTemplate>
            <EditItemTemplate><mmsi:ddlEmployees runat="server" id="ddlEmployees" SelectedValue='<%# Bind("tblEmployeeID") %>' /></EditItemTemplate>
           <InsertItemTemplate><mmsi:ddlEmployees runat="server" id="ddlEmployees" SelectedValue='<%# Bind("tblEmployeeID") %>'/></InsertItemTemplate>
        </asp:TemplateField>
        <asp:CheckBoxField DataField="No_Qualified_Bidders" HeaderText="No_Qualified_Bidders" SortExpression="No_Qualified_Bidders" />
        <asp:CheckBoxField DataField="No_Bidders" HeaderText="No_Bidders" SortExpression="No_Bidders" />
        <asp:TemplateField HeaderText="Internal_Posting" SortExpression="Internal_Posting">
            <EditItemTemplate><asp:CheckBox ID="chkInternal_Posting" runat="server" Checked='<%# Bind("Internal_Posting") %>' /></EditItemTemplate>
            <InsertItemTemplate><asp:CheckBox ID="chkInternal_Posting" runat="server" Checked='<%# Bind("Internal_Posting") %>' /></InsertItemTemplate>
            <ItemTemplate><asp:CheckBox ID="chkInternal_Posting" runat="server" Checked='<%# Bind("Internal_Posting") %>' Enabled="false" /></ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="External_Recruitment" SortExpression="External_Recruitment">
            <EditItemTemplate><asp:CheckBox ID="chkExternal_Recruitment" runat="server" Checked='<%# Bind("External_Recruitment") %>' /></EditItemTemplate>
            <InsertItemTemplate><asp:CheckBox ID="chkExternal_Recruitment" runat="server" Checked='<%# Bind("External_Recruitment") %>' /></InsertItemTemplate>
            <ItemTemplate><asp:CheckBox ID="chkExternal_Recruitment" runat="server" Checked='<%# Bind("External_Recruitment") %>' Enabled="false" /></ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Interviewing" SortExpression="Interviewing">
            <EditItemTemplate><asp:CheckBox ID="chkInterviewing" runat="server" Checked='<%# Bind("Interviewing") %>' /></EditItemTemplate>
            <InsertItemTemplate><asp:CheckBox ID="chkInterviewing" runat="server" Checked='<%# Bind("Interviewing") %>' /></InsertItemTemplate>
            <ItemTemplate><asp:CheckBox ID="chkInterviewing" runat="server" Checked='<%# Bind("Interviewing") %>' Enabled="false" /></ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Candidate_Selected" SortExpression="Candidate_Selected">
            <EditItemTemplate><asp:CheckBox ID="chkCandidate_Selected" runat="server" Checked='<%# Bind("Candidate_Selected") %>' /></EditItemTemplate>
            <InsertItemTemplate><asp:CheckBox ID="chkCandidate_Selected" runat="server" Checked='<%# Bind("Candidate_Selected") %>' /></InsertItemTemplate>
            <ItemTemplate><asp:CheckBox ID="chkCandidate_Selected" runat="server" Checked='<%# Bind("Candidate_Selected") %>' Enabled="false" /></ItemTemplate>
        </asp:TemplateField>
        <mmsinc:BoundField SqlDataType="DateTime" DataFormatString="{0:d}" HtmlEncode="False" DataField="Anticipated_Start_Date" HeaderText="Anticipated_Start_Date" SortExpression="Anticipated_Start_Date" />
        <mmsinc:BoundField SqlDataType="DateTime" DataFormatString="{0:d}" HtmlEncode="False" DataField="Effective_Date" HeaderText="Effective_Date" SortExpression="Effective_Date" />
        <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes" ><ControlStyle Width="98%" /></asp:BoundField>
        
        
        <asp:TemplateField ShowHeader="False">
            <EditItemTemplate>
                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update" Text="Update"></asp:LinkButton>
                <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel"></asp:LinkButton>
            </EditItemTemplate>
            <InsertItemTemplate>
                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Insert" Text="Insert"></asp:LinkButton>
                <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel"></asp:LinkButton>
            </InsertItemTemplate>
            <ItemTemplate>
                <asp:LinkButton ID="btnEdit" runat="server" CausesValidation="False" CommandName="Edit" Text="Edit"></asp:LinkButton>
                <asp:LinkButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete" OnClientClick="return confirm('Are you sure?');"></asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
    </Fields>
    <FieldHeaderStyle Width="100px" />
</asp:DetailsView>
<asp:SqlDataSource runat="server" ID="dsPositions"
    ConnectionString="<%$ ConnectionStrings:MCProd %>"
    SelectCommand="SELECT distinct positionID, cast(positionID as varchar(15)) + '-' + isNull(opCode,'') + '-' + isNull(position, '') as [PositionText], opcode, position from tblPositions_Classifications order by position"
    >
</asp:SqlDataSource>
<asp:Label runat="server" ID="lblResults"></asp:Label>
<asp:SqlDataSource 
    ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:MCProd %>" 
    ProviderName="<%$ ConnectionStrings:MCProd.ProviderName %>" 
    DeleteCommand="DELETE FROM [tblPosition_Postings] WHERE [Position_Posting_ID] = @Position_Posting_ID"
    InsertCommand="
        INSERT INTO [tblPosition_Postings] 
            ([Position_ID], [Position_Control_Number], [Date_of_Posting], tblEmployeeID, [Effective_Date], [Notes], [No_Qualified_Bidders], [No_Bidders],[Internal_Posting],[External_Recruitment],[Interviewing],[Candidate_Selected],[Anticipated_Start_Date]) 
            VALUES (@Position_ID, @Position_Control_Number, @Date_of_Posting, @tblEmployeeID, @Effective_Date, @Notes, @No_Qualified_Bidders, @No_Bidders,@Internal_Posting,@External_Recruitment,@Interviewing,@Candidate_Selected,@Anticipated_Start_Date);
        Select @Position_Posting_ID=@@IDENTITY
        "
    SelectCommand="
        SELECT 
			replace(isNull(First_Name, '') + ' ' +  isNull(Middle_Name,'') + ' ' + isNull(Last_Name,''), '  ', ' ') as [fullname],
			replace(isNull(Last_Name, '') + ', ' +  isNull(First_Name,'') + ' ' + isNull(Middle_Name,''), '  ', ' ') as [empname],
			tblPosition_Postings.tblEmployeeID, Position_Control_Number,
            [Position_Posting_ID], [Position_ID], [tblPositions_classifications].[Position], [Date_of_Posting], [Effective_Date], [Notes], [No_Qualified_Bidders], [No_Bidders] 
            ,LocalBargainingUnits.Name as Local, LocalBargainingUnits.Id as LocalId, tblPositions_classifications.opcode
            ,[Internal_Posting],[External_Recruitment],[Interviewing],[Candidate_Selected],[Anticipated_Start_Date]

        FROM [tblPosition_Postings] 
    	LEFT JOIN [tblPositions_classifications] on [tblPositions_classifications].positionID = [tblPosition_Postings].position_id
    	LEFT JOIN [LocalBargainingUnits] on tblPositions_classifications.localID = [LocalBargainingUnits].Id
    	LEFT JOIN tblEmployee on tblEmployee.tblEmployeeID = [tblPosition_Postings].tblEmployeeID
	    WHERE [Position_Posting_ID] = @Position_Posting_ID"
    UpdateCommand="
        UPDATE [tblPosition_Postings] 
            SET [Position_ID] = @Position_ID, Position_Control_Number=@Position_Control_Number, [Date_of_Posting] = @Date_of_Posting, [tblEmployeeID] = @tblEmployeeID, [Effective_Date] = @Effective_Date, [Notes] = @Notes, [No_Qualified_Bidders] = @No_Qualified_Bidders, [No_Bidders] = @No_Bidders, [Internal_Posting]=@Internal_Posting, [External_Recruitment] = @External_Recruitment, [Interviewing] = @Interviewing, [Candidate_Selected] = @Candidate_Selected, [Anticipated_Start_Date] = @Anticipated_Start_Date
            WHERE [Position_Posting_ID] = @Position_Posting_ID
    "
    OnInserted="SqlDataSource1_Inserted"
    OnDeleted="SqlDataSource1_Deleted"
    OnUpdated="SqlDataSource1_Updated"
    >
    <InsertParameters>
        <asp:Parameter Name="Position_ID" Type="Int32" />
        <asp:Parameter Name="Position_Control_Number" Type="Decimal" />
        <asp:Parameter Name="Date_of_Posting" Type="DateTime" />
        <asp:Parameter Name="tblEmployeeID" Type="String" />
        <asp:Parameter Name="Effective_Date" Type="DateTime" />
        <asp:Parameter Name="Notes" Type="String" />
        <asp:Parameter Name="No_Qualified_Bidders" Type="Boolean" />
        <asp:Parameter Name="No_Bidders" Type="Boolean" />
        <asp:Parameter Name="Internal_Posting" Type="Boolean" />
        <asp:Parameter Name="External_Recruitment" Type="Boolean" />
        <asp:Parameter Name="Interviewing" Type="Boolean" />
        <asp:Parameter Name="Candidate_Selected" Type="Boolean" />
        <asp:Parameter Name="Anticipated_Start_Date" Type="DateTime" />
        <asp:Parameter Name="Position_Posting_ID" Type="Int32" Direction="Output" />
    </InsertParameters>
    <UpdateParameters>
        <asp:Parameter Name="Position_ID" Type="Int32" />
        <asp:Parameter Name="Position_Control_Number" Type="Decimal" />
        <asp:Parameter Name="Date_of_Posting" Type="DateTime" />
        <asp:Parameter Name="tblEmployeeID" Type="String" />
        <asp:Parameter Name="Effective_Date" Type="DateTime" />
        <asp:Parameter Name="Notes" Type="String" />
        <asp:Parameter Name="No_Qualified_Bidders" Type="Boolean" />
        <asp:Parameter Name="No_Bidders" Type="Boolean" />
        <asp:Parameter Name="Position_Posting_ID" Type="Int32" />
        <asp:Parameter Name="Internal_Posting" Type="Boolean" />
        <asp:Parameter Name="External_Recruitment" Type="Boolean" />
        <asp:Parameter Name="Interviewing" Type="Boolean" />
        <asp:Parameter Name="Candidate_Selected" Type="Boolean" />
        <asp:Parameter Name="Anticipated_Start_Date" Type="DateTime" />
    </UpdateParameters>
    <DeleteParameters>
        <asp:Parameter Name="Position_Posting_ID" Type="Int32" />
    </DeleteParameters>
    <SelectParameters>
        <asp:Parameter Name="Position_Posting_ID" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>

