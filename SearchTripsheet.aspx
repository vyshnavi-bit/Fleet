<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" EnableEventValidation="false"
    CodeFile="SearchTripsheet.aspx.cs" Inherits="SearchTripsheet" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%--<script src="js/date.format.js" type="text/javascript"></script>--%>
    <style type="text/css">
        .container
        {
            max-width: 100%;
        }
    </style>
    <script type="text/javascript">
        function exportfn() {
            window.location = "exporttoxl.aspx";
        }

        //------------>Prevent Backspace<--------------------//
        $(document).unbind('keydown').bind('keydown', function (event) {
            var doPrevent = false;
            if (event.keyCode === 8) {
                var d = event.srcElement || event.target;
                if ((d.tagName.toUpperCase() === 'INPUT' && (d.type.toUpperCase() === 'TEXT' || d.type.toUpperCase() === 'PASSWORD'))
            || d.tagName.toUpperCase() === 'TEXTAREA') {
                    doPrevent = d.readOnly || d.disabled;
                } else {
                    doPrevent = true;
                }
            }

            if (doPrevent) {
                event.preventDefault();
            }
        });
    </script>
    <style type="text/css">
        .EU_DataTable
    {
    border-collapse: collapse;
    width:100%;
    }
    .EU_DataTable tr th
    {
    background-color: #3c454f;
    color: #ffffff;
    padding: 10px 5px 10px 5px;
    border: 1px solid #cccccc;
    font-family: Arial, Helvetica, sans-serif;
    font-size: 14px;
    font-weight: normal;
    text-transform:capitalize;
     text-align:center;
    }
    .EU_DataTable tr:nth-child(2n+2)
    {
    background-color: #f3f4f5;
    }

    .EU_DataTable tr:nth-child(2n+1) td
    {
    background-color: #d6dadf;
    color: #454545;
    }
    .EU_DataTable tr td
    {
    padding: 5px 10px 5px 10px;
    color: #454545;
    font-family: Arial, Helvetica, sans-serif;
    font-size: 10px;
    border: 1px solid #cccccc;
    vertical-align: middle;
    }
    .EU_DataTable tr td:first-child
    {
    text-align: center;
    }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
    <section class="content-header">
        <h1>
            Search Tripsheet<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Trip Reports</a></li>
            <li><a href="#">Search Tripsheet</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Trip Sheet Details
                </h3>
            </div>
            <div class="box-body">
                <asp:UpdatePanel ID="updPanel" runat="server">
                    <ContentTemplate>
                        <div align="center">
                            <table>
                                <tr>
                                    <td>
                                        <label>Trip Ref No</label>&nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTripRefNo" runat="server" CssClass="txtClass"></asp:TextBox>
                                    </td>
                                    <td style="width:5px;"></td>
                                    <td>
                                        <asp:Button ID="Button2" runat="server" Text="GENERATE" CssClass="btn btn-primary"
                                            OnClick="btn_Generate_Click" /><br />
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel ID="hidePanel" runat="server" Visible="false">
                                <div style="width: 100%;">
                                    <div style="width: 13%; float: left;">
                                        <img src="Images/Vyshnavilogo.png" alt="Vyshnavi" width="120px" height="82px" />
                                    </div>
                                    <div style="width: 86%; float: right; text-align: center;">
                                        <asp:Label ID="lblTitle" runat="server" Font-Bold="true" Font-Size="20px" ForeColor="#0252aa"
                                            Text=""></asp:Label>
                                        <br />
                                        <asp:Label ID="lblAddress" runat="server" Font-Bold="true" Font-Size="12px" ForeColor="#0252aa"
                                            Text=""></asp:Label>
                                        <br />
                                    </div>
                                    <div style="width: 100%; padding-left: 18%;">
                                        <span style="font-size: 18px; font-weight: bold; color: #0252aa;">Trip Sheet Report</span><br />
                                        <br />
                                        <div>
                                        </div>
                                    </div>
                                    <div style="overflow-x: scroll;">
                                        <asp:GridView ID="dataGridView1" runat="server" ForeColor="White" Width="100%" CssClass="EU_DataTable"
                                            GridLines="Both" Font-Bold="true" OnRowCommand="grdReports_RowCommand">
                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="Gray" Font-Bold="False" ForeColor="White" />
                                            <HeaderStyle BackColor="#f4f4f4" Font-Bold="False" ForeColor="Black" Font-Italic="False"
                                                Font-Names="Raavi" Font-Size="Small" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                            <RowStyle BackColor="#ffffff" ForeColor="#333333" HorizontalAlign="Center" />
                                            <AlternatingRowStyle HorizontalAlign="Center" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Button ID="Button1" runat="server" Text="Print trip sheet" CssClass="btn btn-primary"
                                                            CommandArgument='<%#Container.DataItemIndex%>' Style="height: 25px; width: 120px;
                                                            font-size: 16px;" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <asp:Label ID="lblmsg" runat="server" Text="" ForeColor="Red" Font-Size="20px"></asp:Label>
                                </div>
                            </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </section>
</asp:Content>
