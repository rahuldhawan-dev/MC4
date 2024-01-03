<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChartWithSettings.ascx.cs" Inherits="MapCall.Controls.ChartWithSettings" %>
<%@ Register TagPrefix="dotnet" Namespace="dotnetCHARTING" Assembly="dotnetCHARTING" %>

<style type="text/css">
    .nowrap
    {
        float: left;
        width: 120px;
    }
    .cell
    {
        float: left;
        width: auto;
        white-space: nowrap;
        padding: 2px;
    }
    #ChartSettings
    {
        border: 1px solid #aaaaaa;
        background-color: #FFFFDB;
        float: left;
        clear: none;
        width: auto;
        height: auto;
        padding: 4px;
    }
    .error
    {       
    	color: #FF0000;
    }
    div.chart
    {
    	clear: both;
    }
</style>

<script type="text/javascript">    
    $(document).ready(function() {
        //Chart Settings slider.
        $("#ToggleChartSettings").click(function(){
            $("#ChartSettings").toggle("normal");
            return false;         
        });
       
        //NOTE: the Validation jQuery plugin requires the NAME of the inputs to work.
        //Validation
        $("#<%=Page.Form.Name%>").validate({                      
            rules: { 
                '<%=txtChartHeight.ClientID.Replace("_", "$")%>': { digits: true },
                '<%=txtChartWidth.ClientID.Replace("_", "$")%>': { digits: true }
            }
        });      
    });     
</script>
    <div id="ChartyMcDivChart">
        <asp:Panel runat="server" ID="pnlChartSettings" >
            <a href="#" id="ToggleChartSettings">Chart Settings</a>
        </asp:Panel>
        <div id="ChartSettings" style="display: none;">
            <div class="cell">
                <div>
                    <span class="nowrap">Chart Type</span>
                    <asp:DropDownList runat="server" ID="ddlChartType" />
                </div>
                <div>
                    <span class="nowrap">Scale </span>
                    <asp:DropDownList runat="server" ID="ddlScale" />
                </div>
                <div>
                    <span class="nowrap">Series Type </span>
                    <asp:DropDownList runat="server" ID="ddlSeriesType" />
                </div>
                <div>
                    <span class="nowrap">Show Markers </span>
                    <asp:CheckBox runat="server" ID="cbShowMarkers" />
                </div>
                <div>
                    <span class="nowrap">Use 3D </span>
                    <asp:CheckBox ID="cbUse3d" runat="server" />
                </div>
            </div>
            <div class="cell">
                <div>
                    <span class="nowrap">Chart Title </span>
                    <asp:TextBox runat="server" ID="txtChartTitle" />
                </div>
                <div>
                    <span class="nowrap">Title Box Position</span>
                    <asp:DropDownList runat="server" ID="ddlTitleBoxPosition" />
                </div>
                <div>
                    <span class="nowrap">X Axis Label </span>
                    <asp:TextBox runat="server" ID="txtXAxisLabel"></asp:TextBox></div>
                <div>
                    <span class="nowrap">Y Axis Label </span>
                    <asp:TextBox runat="server" ID="txtYAxisLabel"></asp:TextBox></div>
                <div>
                    <span class="nowrap">Chart Height </span>
                    <asp:TextBox runat="server" ID="txtChartHeight"></asp:TextBox>
                </div>
                <div>
                    <span class="nowrap">Chart Width </span>
                    <asp:TextBox runat="server" ID="txtChartWidth"></asp:TextBox></div>
                <div style="text-align: right;">
                    <span class="nowrap"></span>
                    <asp:Button runat="server" ID="btnChartRefresh" Text="Refresh" 
                        OnClick="btnChartRefresh_Click" />
                </div>
            </div>
        </div>
        <br />
        <div class="chart">
            <dotnet:Chart ID="Chart" runat="server" />
        </div>
    </div>
