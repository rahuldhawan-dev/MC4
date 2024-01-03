<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CrewAssignmentsByMonth.ascx.cs" Inherits="LINQTo271.Views.CrewAssignments.CrewAssignmentsByMonth" %>
<%@ Register TagPrefix="mmsi" Namespace="MMSINC.Controls.BetterCalendar" Assembly="MMSINC.Core.WebForms" %>

<mmsi:BetterCalendar ID="wcCrewAssignments" OnSelectedDatesChanged="Calendar_SelectedDatesChanged" runat='server' />
