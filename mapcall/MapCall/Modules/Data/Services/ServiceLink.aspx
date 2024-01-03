<%@ Page Title="" Language="C#" MasterPageFile="~/MapCallHIB.Master" AutoEventWireup="true" CodeBehind="ServiceLink.aspx.cs" Inherits="MapCall.Modules.Data.Services.ServiceLink" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    Service Images
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphInstructions" runat="server">
    Click the link below to view the service images or service record. 
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
        Linked Service Images
    </div>
    <div class="results">
        <asp:GridView runat="server" ID="gvLinkedImages"
            AutoGenerateColumns="False"
            DataSourceID="dsLinkedImages"
            EmptyDataText="No matches were found for this Service"
            SkinID="OldSchoolLines"
            HeaderStyle-Font-Size="Smaller">
            <Columns>
                <asp:HyperLinkField HeaderText="Service"
                    Text="View Service"
                    DataNavigateUrlFields="Id"
                    Target="_top"
                    DataNavigateUrlFormatString="~/Modules/Mvc/FieldOperations/Service/Show/{0}"
                />
                <asp:HyperLinkField HeaderText="Tap Image"
                    Text="View Tap Image Record"
                    DataNavigateUrlFields="TapImageID"
                    DataNavigateUrlFormatString="~/Modules/Mvc/FieldOperations/TapImage/Show/{0}"
                />
                <asp:HyperLinkField HeaderText="Tap Image PDF"
                    Text="View Tap Image PDF"
                    DataNavigateUrlFields="TapImageID"
                    DataNavigateUrlFormatString="~/Modules/Mvc/FieldOperations/TapImage/Show/{0}.pdf"
                />
                <asp:BoundField DataField="PremiseNumber" HeaderText="Premise Number" />
                <asp:BoundField DataField="ServiceNumber" HeaderText="Service Number" />
                <asp:BoundField DataField="Town" HeaderText="Town" />
                <asp:BoundField DataField="TownSection" HeaderText="Town Section" />
                <asp:BoundField DataField="Street" HeaderText="Street" />
                <asp:BoundField DataField="DateInstalled" DataFormatString="{0:d}" HeaderText="Date Installed" />
                <asp:BoundField DataField="IsDefault" HeaderText="Is Default Image" />
                <asp:BoundField DataField="DateCompleted" HeaderText="Date Completed" DataFormatString="{0:d}"/>
                <asp:BoundField DataField="ServiceType" HeaderText="Service Type"/>
            </Columns>        
        </asp:GridView>
        <asp:SqlDataSource runat="server" ID="dsLinkedImages"
            CancelSelectOnNullParameter="false"
            ConnectionString="<%$ ConnectionStrings:MCProd %>"
            SelectCommand="
                    if (@recID is null AND @service is null) 
	                    select 
		                    isNull(cast(s.PremiseNumber as varchar),'') + ' / ' + isNull(cast(s.ServiceNumber as varchar),'') as premNum,
		                    S.PremiseNumber, S.ServiceNumber,
                            TI.TapImageID, TI.Fld, TI.FileList, TI.ServiceID, st.FullStName as Street,TI.IsDefault,
		                    T.StateID, T.Town, 
                            ts.Name as [TownSection],
                            S.DateInstalled,
                            oc.OperatingCenterCode as OperatingCenter, s.Id,
                            ti.DateCompleted,ti.ServiceType
	                    from 
		                    Services s 
	                    inner join 
		                    TapImages ti on s.Id = ti.ServiceId 
                        left join OperatingCenters oc on oc.OperatingCenterId = S.OperatingCenterID
                        left join Towns T on T.TownID = S.TownID
	                    left join TownSections TS on TS.TownSectionID = S.TownSectionId
                        left join Streets St on ST.StreetID = S.StreetID
	                    where s.PremiseNumber = @premise

                    else
                    begin
                        if (@recID is null) select @recID = (select top 1 Id from Services S where str(S.ServiceNumber) = isNull(@service, rtrim(ltrim(str(S.ServiceNumber)))) and PremiseNumber = @premise)
                        select 
                            serv.PremiseNumber + ' / ' + serv.ServiceNumber as PremServ, 
                            TI.*,
                            T.StateID, 
                            T.Town,
                            [TownSection],
                            serv.DateInstalled,
                            oc.OperatingCenterCode as OperatingCenter
                        from 
                            TapImages TI 
                        join OperatingCenters oc on oc.OperatingCenterId = TI.OperatingCenterID
                        join Towns T on T.TownID = TI.TownID
                        left join Services serv on serv.Id = @RecID
                        where 
                            ServiceID = @RecID
                    end
                ">
            <SelectParameters>
                <asp:QueryStringParameter QueryStringField="RecID" Name="RecID" />
                <asp:QueryStringParameter QueryStringField="premise" Name="premise" />
                <asp:QueryStringParameter QueryStringField="premiseNumber" Name="premiseNumber" />
                <asp:QueryStringParameter QueryStringField="service" Name="service" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>

    <div class="header">
        Unlinked Images with the same Service/Premise #
    </div>
    <div class="results">
        <asp:GridView runat="server" ID="gvNumbers"
            AutoGenerateColumns="False"
            DataSourceID="dsNumbers"
            EmptyDataText="No matches were found for this Service"
            SkinID="OldSchoolLines"
            HeaderStyle-Font-Size="Smaller">
            <Columns>
                <asp:HyperLinkField HeaderText="Tap Image"
                    Text="View Tap Image Record"
                    DataNavigateUrlFields="TapImageID"
                    DataNavigateUrlFormatString="~/Modules/Mvc/FieldOperations/TapImage/Show/{0}"
                />
                <asp:HyperLinkField HeaderText="Tap Image PDF"
                    Text="View Tap Image PDF"
                    DataNavigateUrlFields="TapImageID"
                    DataNavigateUrlFormatString="~/Modules/Mvc/FieldOperations/TapImage/Show/{0}.pdf"
                />
                <asp:BoundField DataField="PremiseNumber" HeaderText="Premise Number" />
                <asp:BoundField DataField="ServiceNumber" HeaderText="Service Number" />
                <asp:BoundField DataField="Town" HeaderText="Town" />
                <asp:BoundField DataField="TownSection" HeaderText="Town Section" />
                <asp:BoundField DataField="StreetPrefix" HeaderText="StreetPrefix" />
                <asp:BoundField DataField="Street" HeaderText="Street" />
                <asp:BoundField DataField="StreetSuffix" HeaderText="StreetSuffix" />
                <asp:BoundField DataField="DateCompleted" HeaderText="Date Completed" DataFormatString="{0:d}"/>
                <asp:BoundField DataField="ServiceType" HeaderText="Service Type"/>
            </Columns>        
        </asp:GridView>
        <asp:SqlDataSource runat="server" ID="dsNumbers"
            CancelSelectOnNullParameter="false"
            ConnectionString="<%$ ConnectionStrings:MCProd %>"
            SelectCommand="
                if (@recID is not null)
                begin
                    select @service = (select ServiceNumber from Services where Id = @RecID)
                    select @premise = (select PremiseNumber from Services where Id = @RecID)
                end
                select
	                premiseNumber, serviceNumber, S.Abbreviation as [State], oc.OperatingCenterCode as OperatingCenter, *
                from
	                TapImages TI
                join Towns town on town.TownId = TI.TownId
                join Counties c on c.CountyId = town.CountyId
                join states S on S.StateId = c.StateId
            join OperatingCenters oc on oc.OperatingCenterId = TI.OperatingCenterId
                where 
	                TI.PremiseNumber = @premise
                and
                    (TI.serviceNumber = isNull(@service, serviceNumber) or TI.ServiceNumber is null)
                and ServiceID is null
                ">
            <SelectParameters>
                <asp:QueryStringParameter QueryStringField="RecID" Name="RecID" />
                <asp:QueryStringParameter QueryStringField="premise" Name="premise" />
                <asp:QueryStringParameter QueryStringField="service" Name="service" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
    
    <div class="header">
        Services
    </div>
    <div class="results">
        <asp:GridView runat="server" ID="gvServices" AutoGenerateColumns="False"
            DataSourceID="dsServices" SkinID="OldSchoolLines"
            EmptyDataText="No matches were found">
            <Columns>
                 <asp:HyperLinkField HeaderText="Action"
                    Text="View"
                    DataNavigateUrlFields="RecID"
                    DataNavigateUrlFormatString="~/Modules/Mvc/FieldOperations/Service/Show/{0}"
                />
                <asp:BoundField DataField="OpCntr" HeaderText="OpCntr" />
                <asp:BoundField DataField="PremiseNumber" HeaderText="Premise Number" />
                <asp:BoundField DataField="ServiceNumber" HeaderText="Service Number" />
                <asp:BoundField DataField="CatOfService" HeaderText="Category of Service" />
                <asp:BoundField DataField="Town" HeaderText="Town" />
                <asp:BoundField DataField="TownSection" HeaderText="Town Section" />
                <asp:BoundField DataField="Street" HeaderText="Street" />
                
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource runat="server" ID="dsServices"
            CancelSelectOnNullParameter="false"
            ConnectionString="<%$ ConnectionStrings:MCProd %>"
            SelectCommand="
                select
	                se.Id as RecID, 
                    oc.OperatingCenterCode as OpCntr, 
	                PremiseNumber, 
	                ServiceNumber,
	                T.Town, 
	                ts.Name as TownSection,
	                St.FullStName as Street,
                    sc.Description as CatOfService
                from
	                Services se
                left join
                    Towns t
                on
                    se.TownId = t.TownID
                left join
                    Streets st
                on
                    st.StreetID = se.StreetId
                left join
                    OperatingCenters oc on oc.OperatingCenterID = se.OperatingCenterID
                left join
                    TownSections ts on ts.TownSectionID = se.TownSectionID
                left join
                    ServiceCategories sc on sc.ServiceCategoryID = se.ServiceCategoryId
                where
	                PremiseNumber = @premise
            ">
            <SelectParameters>
                <asp:QueryStringParameter QueryStringField="premise" Name="premise" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>

</asp:Content>