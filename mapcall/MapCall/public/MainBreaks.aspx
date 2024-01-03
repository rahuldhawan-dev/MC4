<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MainBreaks.aspx.cs" Inherits="MapCall.Argh.MainBreaks" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:SqlDataSource runat="server" ID="dsWorkOrders"
            ConnectionString="<%$ ConnectionStrings:MCProd %>"
            SelectCommand="
select 
	WorkOrderID,
	DateReceived,
	DateCompleted,	
	Latitude, 
	Longitude, 
	(Select Description from WorkDescriptions wd where wd.WorkDescriptionID = WorkOrders.WorkDescriptionID) 
from 
	workorders 
where 
	WorkDescriptionID in (74,80)
and
	DateDiff(DD, DateReceived, getDate()) &lt; 30
order by
	DateReceived desc
            "
        />
    </form>
</body>
</html>
