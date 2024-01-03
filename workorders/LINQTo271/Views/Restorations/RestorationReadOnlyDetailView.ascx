<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RestorationReadOnlyDetailView.ascx.cs" Inherits="LINQTo271.Views.Restorations.RestorationReadOnlyDetailView" %>
<%@ Register Assembly="LINQTo271" Namespace="LINQTo271.Common" TagPrefix="wo" %>

<wo:WorkOrdersFormView runat="server" ID="fvRestoration" DataSourceID="odsRestoration"
    DataKeyNames="RestorationID">
    <ItemTemplate>
        <div class="grid_12 font_medium">
            <span class="label">OpCenter:</span>
            <%# Eval("WorkOrder.OperatingCenter") %>
        </div>
        <div class="grid_12 font_medium">
            <span class="label">Restoration Ticket Number:</span>
            <%# Eval("WorkOrderID") %>-<%# Eval("RestorationID") %>
        </div>

        <div class="grid_12">&nbsp;</div>
        <div class="grid_12">&nbsp;</div>

        <div class="grid_12 title font_extra_large">
            New Jersey American Restoration Ticket
        </div>

        <div class="grid_12">&nbsp;</div>

        <div class="grid_12 font_large">
            <span class="label">Date Issued:</span>
            <%# Eval("WorkOrder.ApprovedOn") %>
        </div>

        <div class="grid_12">&nbsp;</div>

        <div class="grid_12 font_large">
            <span class="label">Response Priority:</span>
            <%# Eval("ResponsePriority") %>
        </div>

        <div class="grid_12">&nbsp;</div>

        <div class="grid_12 font_large box">
            <div class="grid_5 alpha label">Type of Restoration:</div>
            <div class="grid_7 omega">
                <%# Eval("RestorationType") %>
            </div>

            <div class="clear"></div>

            <div class="grid_5 alpha label">Estimated Restoration Size:</div>
            <div class="grid_7 omega">
                <%# Eval("PavingSquareFootage") ?? Eval("LinearFeetOfCurb") %>
                <%# Eval("PavingSquareFootage") == null ? "L.F." : "S.F." %>
            </div>

            <div class="clear"></div>

            <div class="grid_12 mu-nu label">Restoration Comments:</div>
            <div class="grid_12 mu-nu" style="font-size:smaller;">
                <%# Eval("RestorationNotes") %>
            </div>

            <div class="grid_5 alpha label">Saw Cutting by Company Forces:</div>
            <div class="grid_7 omega">
                <%# (bool)Eval("SawCutByCompanyForces") ? "yes" : "no" %>
            </div>
        </div>

        <div class="grid_12">&nbsp;</div>

        <div class="grid_12 font_large heading">Location</div>

        <div class="grid_12">&nbsp;</div>

        <div class="grid_12 font_large box">
            <div class="grid_5 alpha label">Town:</div>
            <div class="grid_7 omega"><%# Eval("WorkOrder.Town") %></div>

            <div class="clear"></div>

            <div class="grid_5 alpha label">Street Number:</div>
            <div class="grid_7 omega"><%# Eval("WorkOrder.StreetNumber") %></div>

            <div class="clear"></div>

            <div class="grid_5 alpha label">Street:</div>
            <div class="grid_7 omega"><%# Eval("WorkOrder.Street") %></div>

            <div class="clear"></div>

            <div class="grid_5 alpha label">Cross Street:</div>
            <div class="grid_7 omega"><%# Eval("WorkOrder.NearestCrossStreet") %></div>

            <div class="clear"></div>

            <div class="grid_5 alpha label">Distance from Cross Street:</div>
            <div class="grid_7 omega"><%# Eval("WorkOrder.DistanceFromCrossStreet")%></div>
        </div>

        <div class="grid_12">&nbsp;</div>
        <div class="grid_12 font_large heading">Billing Information</div>
        <div class="grid_12">&nbsp;</div>

        <div class="clear"></div>

        <div class="grid_5 font_large label">Work Order #:</div>
        <div class="grid_7 font_large"><%# Eval("WorkOrderID") %></div>

        <div class="clear"></div>

        <div class="grid_5 font_large label">Job Description:</div>
        <div class="grid_7 font_large"><%# Eval("WorkOrder.WorkDescription") %></div>

        <div class="clear"></div>

        <div class="grid_5 font_large label">Account:</div>
        <div class="grid_5 font_large">
            <%# Eval("WorkOrder.FirstAccountingString") %><%# Eval("WorkOrder.WorkDescription.FirstRestorationAccountingString") %>
        </div>
        <div class="grid_2 font_large"><%# Eval("WorkOrder.WorkDescription.FirstRestorationCostBreakdownString") %></div>
        <div class="clear"></div>
        
        <div class="grid_5">&nbsp;</div>
        <div class="grid_4 font_large">
            <%# Eval("WorkOrder.SecondAccountingString") %><%# Eval("WorkOrder.WorkDescription.SecondRestorationAccountingString")%>
        </div>
        <div class="grid_3 font_large"><%# Eval("WorkOrder.WorkDescription.SecondRestorationCostBreakdownString") %></div>
        <div class="clear"></div>

        <div class="grid_5 font_large label">Approver Email Address:</div>
        <div class="grid_7 font_large"><%# Eval("WorkOrder.ApprovedBy.EMail")%></div>

        <div class="grid_12">&nbsp;</div>
        <div class="grid_12">&nbsp;</div>

        <div class="grid_12 font_large label">
            In the event of any questions regarding this restoration please contact
        </div>
        <div class="grid_12 font_large">
            <%# Eval("WorkOrder.ApprovedBy.FullName") %>
            at
            <%# Eval("WorkOrder.ApprovedBy.PhoneNum")%>
        </div>

        <div class="grid_12">&nbsp;</div>
        <div class="grid_12">&nbsp;</div>

        <div class="grid_12 font_small label">
            <div class="grid_5 alpha">Additional Restoration Approved By:</div>
            <div class="grid_3 underline">&nbsp;</div>
            <div class="grid_1">Date:</div>
            <div class="grid_3 underline omega">&nbsp;</div>
        </div>
    </ItemTemplate>
</wo:WorkOrdersFormView>

<mmsinc:MvpObjectContainerDataSource runat="server" ID="odsRestoration" DataObjectTypeName="WorkOrders.Model.Restoration" />
