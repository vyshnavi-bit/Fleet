<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" CodeFile="ChangePassword.aspx.cs" Inherits="ChangePassword" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" Runat="Server">
 <style type="text/css">
           .txtinputCss 
    {
    display:block;
    width:80%;
    height:30px;
    padding:6px 12px;
    font-size:14px;
    line-height:1.428571429;
    color:#555;
    vertical-align:middle;
    background-color:#fff;
    background-image:none;
    border:1px solid #ccc;
    border-radius:4px;
    -webkit-box-shadow:inset 0 1px 1px rgba(0,0,0,0.075);
    box-shadow:inset 0 1px 1px rgba(0,0,0,0.075);
    -webkit-transition:border-color ease-in-out .15s,box-shadow ease-in-out .15s;
    transition:border-color ease-in-out .15s,box-shadow ease-in-out .15s
    }
    </style>
        <script type="text/javascript">
            $(function () {
                window.history.forward(1);

            });
        </script>
        
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div style="width: 100%; background-color: #fff">
 <br />
            <br /> <br />
            
    <asp:UpdateProgress ID="updateProgress1" runat="server">
        <ProgressTemplate>
            <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0;
                right: 0; left: 0; z-index: 9999999; background-color: #FFFFFF; opacity: 0.7;">
                <br />
                <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="thumbnails/loading.gif"
                    AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: absolute;
                    top: 35%; left: 40%;" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="updPanel" runat="server">
        <ContentTemplate>
            <div  align="center">
                  <div>
            <span style="color: Red; font-size: 22px; font-weight: bold;">Change Password</span>
           
        </div>
                <br />
                <table align="center" style="border: 1px solid gray;">
                    <tr>
                        <td>
                            <asp:Label ID="lblOldPassWord" runat="server" Text="Current Password"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtOldPassWord" TextMode="Password" runat="server" placeholder="Enter Current Password" CssClass="txtinputCss"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtOldPassWord"
                                ForeColor="Red" ErrorMessage="*">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblNewPassWord" runat="server" Text="New Password"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNewPassWord" TextMode="Password" runat="server" placeholder="Enter New Password" CssClass="txtinputCss"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtNewPassWord"
                                ForeColor="Red" ErrorMessage="*">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblConformPassWord" runat="server" Text="Conform Password"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtConformPassWord" TextMode="Password" runat="server" placeholder="Enter Confirm Password" CssClass="txtinputCss"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtConformPassWord"
                                ForeColor="Red" ErrorMessage="*">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <a href="LogOut.aspx">Back To Login Page</a>
                        </td>
                        <td>
                            <asp:Button ID="btnSubmitt" runat="server" Text="Submitt" OnClick="btnSubmitt_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Label ID="lblError" ForeColor="Red" runat="server" Text=""></asp:Label>
                            <asp:Label ID="lblMessage" ForeColor="Red" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </div>
</asp:Content>

