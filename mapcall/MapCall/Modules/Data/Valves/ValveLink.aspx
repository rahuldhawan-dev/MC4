<%@ Page Title="Valve Image Search" Theme="bender" Language="C#" MasterPageFile="~/MapCallHIB.Master" AutoEventWireup="true" CodeBehind="ValveLink.aspx.cs" Inherits="MapCall.Modules.Data.Valves.ValveLink" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    Valve Images
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphInstructions" runat="server">
    Click the link below to view the valve images or valve record. 
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
    <style type="text/css">
    .header 
    {
        margin-top: 15px;
        font-size:larger;
    }    
    </style>

    <div class="header">
        Linked Valve Images
    </div>
    <div class="results">
        <asp:GridView runat="server" ID="GridView1" 
        AutoGenerateColumns="False"
        DataSourceID="dsValveID"
        EmptyDataText="No matches were found for this Id"
        SkinID="OldSchoolLines"
        HeaderStyle-Font-Size="Smaller"
    >
        <Columns>
            <asp:BoundField DataField="OperatingCenterCode" HeaderText="OpCntr"/>
            <asp:HyperLinkField HeaderText="Detail"
                Text="Detail"
                DataNavigateUrlFields="Id"
                Target="_top"
                DataNavigateUrlFormatString="~/Modules/mvc/FieldOperations/Valve/Show/{0}"
            />
            <asp:HyperLinkField HeaderText="View Image Detail"
                Text="Image Detail"
                DataNavigateUrlFields="ValveImageID"
                Target="_top"
                DataNavigateUrlFormatString="~/Modules/Mvc/FieldOperations/ValveImage/Show/{0}"
            />
            <asp:HyperLinkField HeaderText=""
                Text="View PDF"
                DataNavigateUrlFields="ValveImageID"
                Target="_top"
                DataNavigateUrlFormatString="~/Modules/Mvc/FieldOperations/ValveImage/Show/{0}.pdf"
            />
            <asp:BoundField DataField="Town" HeaderText="Town" />
            <asp:BoundField DataField="Town Section" HeaderText="Town Section" />
            <asp:BoundField DataField="Street" HeaderText="Street" />
            <asp:BoundField DataField="Cross Street" HeaderText="Cross Street" />
            <asp:BoundField DataField="DateInstalled" DataFormatString="{0:d}" HeaderText="Date Installed" />
            <asp:BoundField DataField="IsDefault" HeaderText="Is Default Image" />
            <asp:BoundField DataField="CreatedAt" DataFormatString="{0:d}" HeaderText="Date Added" />
        </Columns>
    </asp:GridView>
        <asp:SqlDataSource runat="server" ID="dsValveID"
        CancelSelectOnNullParameter="false"
        ConnectionString="<%$ ConnectionStrings:MCProd %>"
        SelectCommand="
        IF (@ValveNumber is not null)
        BEGIN 
            select 
		                oc.OperatingCenterCode,
                        vi.ValveImageID,
		                vi.ValveID as Id,
                        case when (vi.IsDefault = 1) then 'Yes' else 'No' end as IsDefault,
		                @ValveNumber as ValveNumber, 
		                t.StateID, 
		                oc.OperatingCenterCode, 
		                isNull(vi.fld,'') as fld, 
		                vi.filelist,
		                t.Town as [Town], 
		                vi.TownSection as [Town Section], 
		                isNull(' ' + vi.StreetPrefix, '') + isNull(' ' + vi.Street,'') + isNull(' ' + vi.StreetSuffix,'') as [Street], 
		                xs.FullStName as [Cross Street], 
		                vi.MainSize as [Main Size], 
		                vi.DateCompleted,
                        valves.DateInstalled as DateInstalled,
                        vi.CreatedAt
	                from 
		                ValveImages vi
	                left join
		                Towns t
	                on
		                vi.TownID = t.TownID		
	                left join
		                OperatingCentersTowns oct
	                on
		                oct.TownID = t.TownID
	                left join
		                OperatingCenters oc
	                on
		                oc.OperatingCenterID = oct.OperatingCenterID
                    left join
                        Valves valves
                    on
                        valves.Id = vi.ValveID
                    left join
                        streets xs on valves.CrossStreetId = xs.StreetId
	                where 
		                vi.ValveNumber = @ValveNumber
                    and
                        vi.ValveId is not null
        END

        ELSE
            BEGIN
                declare @town int
                declare @Id int
                declare @state varchar(4)
                --declare @ValveNumber varchar(50)

                select @town = (select town from Valves where objectID = @objectID)
                select @Id = (select id from Valves where objectID = @objectID)
                select @ValveNumber = (select ValveNumber from Valves where objectID = @objectID)
                select @state = (select state from Towns where TownID = @town)

                if (@Id is null)
                BEGIN
                    select @Id = @qsRecID
                    select @town = (select town from Valves where Id = @Id)
                    select @ValveNumber = (select ValveNumber from Valves where Id = @Id)
                    select @state = (select state from Towns where TownID = @town)
                END
	                select 
		                VI.ValveImageID,
                        case when (vi.IsDefault = 1) then 'Yes' else 'No' end as IsDefault,
		                @Id as Id,
		                @ValveNumber as ValveNumber, 
		                T.StateID, 
		                oc.OperatingCenterCode, 
		                isNull(VI.fld,'') as fld, 
		                VI.filelist,
		                T.Town as [Town], 
		                '' as [Town Section], 
		                S.FullStName as [Street], 
		                xs.FullStName as [Cross Street], 
		                vs.size as ValveSize,
		                VI.DateCompleted,
                        V.DateInstalled as DateInstalled,
                        vi.CreatedAt
	                from 
		                ValveImages VI
	                join
		                Valves V
	                on	
		                V.Id = VI.ValveID
	                join
		                Towns T
	                on
		                T.TownID = V.Town
	                join
		                streets S
	                on
		                S.StreetID = V.StreetId
                    join
		                streets xs
	                on
		                S.StreetID = V.CrossStreetId
                    left join
                        OperatingCenters oc on oc.OperatingCenterId = V.OperatingCenterId
                    left join 
                        valvesizes vs on vs.Id = V.ValveSizeId
	                where 
		                V.Id = @Id
            END

        ">
        <SelectParameters>
            <asp:QueryStringParameter Name="ObjectID" Type="Int32" 
                QueryStringField="ObjectID" ConvertEmptyStringToNull="true" />
            <asp:QueryStringParameter Name="qsRecID" Type="Int32"
                QueryStringField="recID" ConvertEmptyStringToNull="true" DefaultValue="0" />
            <asp:QueryStringParameter Name="ValveNumber" Type="String" 
                QueryStringField="ValNum" ConvertEmptyStringToNull="true" />

        </SelectParameters>
    </asp:SqlDataSource>
    </div>

    <div class="header">
        Unlinked Valves Images with the same Valve Number
    </div>
    <div class="results">
    <asp:GridView runat="server" ID="gvSearchResults" 
        AutoGenerateColumns="False"
        DataSourceID="dsSearchResults"
        EmptyDataText="No matches were found for this ObjectID"
        SkinID="OldSchoolLines"
        HeaderStyle-Font-Size="Smaller"
    >
        <Columns>
            <asp:BoundField DataField="OperatingCenterCode" HeaderText="OpCntr"/>
            <asp:HyperLinkField HeaderText="Detail"
                Text="Detail"
                DataNavigateUrlFields="ValveImageID"
                Target="_top"
                DataNavigateUrlFormatString="~/Modules/Mvc/FieldOperations/ValveImage/Show/{0}"
            />
            <asp:HyperLinkField HeaderText=""
                Text="View PDF"
                DataNavigateUrlFields="ValveImageID"
                Target="_top"
                DataNavigateUrlFormatString="~/Modules/Mvc/FieldOperations/ValveImage/Show/{0}.pdf"
            />
            <asp:BoundField DataField="Town" HeaderText="Town" />
            <asp:BoundField DataField="Town Section" HeaderText="Town Section" />
            <asp:BoundField DataField="Street" HeaderText="Street" />
            <asp:BoundField DataField="Cross Street" HeaderText="Cross Street" />
            <asp:BoundField DataField="CreatedAt" DataFormatString="{0:d}" HeaderText="Date Added" />
        </Columns>
    </asp:GridView>
        
    <asp:SqlDataSource runat="server" ID="dsSearchResults"
        CancelSelectOnNullParameter="false"
        SelectCommand="

                -- DISPLAY ANY IMAGES WITH THE SAME OPCNTR AND VALVE NUMBER
	                select 
		                OperatingCenterCode,
                        v.ValveImageID,
		                @ValveNumber as ValveNumber, 
		                t.StateID, 
		                oc.OperatingCenterCode, 
		                isNull(v.fld,'') as fld, 
		                v.filelist,
		                t.Town as [Town], 
		                v.TownSection as [Town Section], 
		                isNull(' ' + v.StreetPrefix, '') + isNull(' ' + v.Street,'') + isNull(' ' + v.StreetSuffix,'') as [Street], 
		                isNull(CrossStreet,'') + isNull(' ' + XStreetPrefix,'') as [Cross Street], 
		                v.MainSize as [Main Size], 
		                v.DateCompleted,
                        v.CreatedAt
	                from 
		                ValveImages v
	                left join
		                Towns t
	                on
		                v.TownID = t.TownID		
	                left join
		                OperatingCentersTowns oct
	                on
		                oct.TownID = t.TownID
	                left join
		                OperatingCenters oc
	                on
		                oc.OperatingCenterID = oct.OperatingCenterID
	                where 
                        v.ValveNumber = @ValveNumber
                    and
                        V.ValveId is null
        "
        ConnectionString="<%$ ConnectionStrings:MCProd %>"
    >
        <SelectParameters>
            <asp:QueryStringParameter Name="ValveNumber" Type="String" QueryStringField="ValNum" ConvertEmptyStringToNull="true" />
        </SelectParameters>
    </asp:SqlDataSource>
    </div>
    
    <div class="header">
        Valves
    </div>
    <div class="results">
        <asp:GridView runat="server" ID="gvValves" AutoGenerateColumns="False" DataSourceID="dsValves"
            EmptyDataText="No matches were found" SkinID="OldSchoolLines">
            <Columns>
                <asp:BoundField DataField="OpCntr" HeaderText="OpCntr"/>
                <asp:HyperLinkField HeaderText="Valve Number"
                    DataTextField="ValveNumber"
                    DataNavigateUrlFields="Id"
                    DataNavigateUrlFormatString="~/Modules/Mvc/FieldOperations/Valve/Show/{0}"
                />
                <asp:BoundField DataField="Town" HeaderText="Town"/>
                <asp:BoundField DataField="TownSection" HeaderText="Town Section"/>
                <asp:BoundField DataField="Street" HeaderText="Street"/>
                <asp:BoundField DataField="CrossStreet" HeaderText="Cross Street"/>
                <asp:BoundField DataField="CreatedAt" DataFormatString="{0:d}" HeaderText="Date Added" />
            </Columns>
        </asp:GridView>

        <asp:SqlDataSource runat="server" CancelSelectOnNullParameter="false" ID="dsValves"
            ConnectionString="<%$ ConnectionStrings:MCProd %>"
            SelectCommand="
            IF (@ValveNumber is not null)
            BEGIN 
                select 
		            oc.OperatingCenterCode as OpCntr, T.Town, ValveNumber, V.Id, s.FullStName as Street, xs.FullStName as CrossStreet, ts.Name as TownSection, v.CreatedAt
	            from 
		            Valves V
	            left join
		            Towns t
	            on
		            v.Town = t.TownID
                left join
                    Streets s
                on
                    s.StreetID = v.StreetId
                left join
                    Streets xs
                on
                    xs.StreetID = v.CrossStreetId
                left join
                    OperatingCenters oc
                on
                    oc.OperatingCenterID = V.OperatingCenterId
                left join
                    TownSections ts on ts.TownSectionId = V.TownSectionId
                where 
		            v.ValveNumber = @ValveNumber
            END"
        >
            <SelectParameters>
                <asp:QueryStringParameter Name="ValveNumber" Type="String" QueryStringField="ValNum" ConvertEmptyStringToNull="true" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>

</asp:Content>