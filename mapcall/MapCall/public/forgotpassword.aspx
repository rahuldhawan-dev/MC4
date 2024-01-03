<%@ Page Title="Retrieve Password" Language="C#" AutoEventWireup="true" Theme="bender" CodeBehind="forgotpassword.aspx.cs" MasterPageFile="~/Public.master" Inherits="MapCall.public1.forgotpassword" %>

<asp:Content ContentPlaceHolderID="cphHeading" runat="server">
    Retrieve Password
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContent" runat="server">
<form runat="server">
    <div>
        <asp:PasswordRecovery runat="server" 
            BorderPadding="4" 
            SuccessText="Your password has been sent to you. You should receive it within two minutes." 
            UserNameInstructionText="" UserNameTitleText="" 
            QuestionInstructionText="" QuestionTitleText="">
            <UserNameTemplate>
                <table>
                    <thead>
                        <td>Enter username to retrieve password</td>
                        <td></td>
                    </thead>
                    <tr>
                        <td>
                            <asp:TextBox ID="UserName" runat="server" />
                            <div>
                            <asp:RequiredFieldValidator ID="rvUserName" runat="server" ControlToValidate="UserName"
                ErrorMessage="User Name is required." InitialValue="" />
                            </div>
                        </td>
                        <td>
                            <asp:Button ID="SubmitButton" runat="server" CommandName="Submit" Text="Submit" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="FailureText" runat="server" CssClass="error" EnableViewState="False"></asp:Label>
                        </td>
                    </tr>
                </table>
            </UserNameTemplate>
        </asp:PasswordRecovery>
    </div>
</form>
</asp:Content>
