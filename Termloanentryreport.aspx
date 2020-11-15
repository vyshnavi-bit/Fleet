<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" EnableEventValidation="false"
    CodeFile="Termloanentryreport.aspx.cs" Inherits="Termloanentryreport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .container
        {
            max-width: 100%;
        }
    </style>
    <%--<script src="js/date.format.js" type="text/javascript"></script>--%>
    <script type="text/javascript">
        function CallPrint(strid) {
            var divToPrint = document.getElementById(strid);
            var newWin = window.open('', 'Print-Window', 'width=400,height=400,top=100,left=100');
            newWin.document.open();
            newWin.document.write('<html><body   onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
            newWin.document.close();
        }
        
    </script>
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
            Term Loanentry Report<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Trip Reports</a></li>
            <li><a href="#">Term Loanentry Report</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Term Loanentry Details
                </h3>
            </div>
            <div class="box-body">
                <asp:UpdatePanel ID="updPanel" runat="server">
                    <ContentTemplate>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="Label5" runat="server" Text="Label">Date</asp:Label>&nbsp;
                                    <asp:TextBox ID="dtp_FromDate" runat="server" CssClass="txtClass">
                                    </asp:TextBox>
                                    <asp:CalendarExtender ID="enddate_CalendarExtender2" runat="server" Enabled="True"
                                        TargetControlID="dtp_FromDate" Format="dd-MM-yyyy HH:mm">
                                    </asp:CalendarExtender>
                                </td>
                                <td style="width: 5px;">
                                </td>
                                <td>
                                    <asp:Button ID="Button2" runat="server" Text="Generate" CssClass="btn btn-primary"
                                        OnClick="btn_Generate_Click" /><br />
                                </td>
                                <td style="width: 5px;">
                                </td>
                                <td>
                                    <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/exporttoxl.aspx">Export to XL</asp:HyperLink>
                                </td>
                            </tr>
                        </table>
                        <div id="divPrint">
                            <div style="width: 100%;">
                                <div style="width: 13%; float: left;">
                                    <img src="Images/Vyshnavilogo.png" alt="Vyshnavi" width="120px" height="82px" />
                                </div>
                                <div align="center">
                                    <asp:Label ID="lblTitle" runat="server" Font-Bold="true" Font-Size="20px" ForeColor="#0252aa"
                                        Text=""></asp:Label>
                                    <br />
                                    <asp:Label ID="lblAddress" runat="server" Font-Bold="true" Font-Size="12px" ForeColor="#0252aa"
                                        Text=""></asp:Label>
                                    <br />
                                </div>
                                <div align="center">
                                    <span style="font-size: 18px; font-weight: bold; color: #0252aa;">Vehicle Term Loans
                                        Statement</span><br />
                                    <div>
                                    </div>
                                </div>
                                <table align="center">
                                    <tr>
                                        <td>
                                            Month
                                        </td>
                                        <td>
                                            <asp:Label ID="lblMonth" runat="server" Text="" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <div>
                                    <asp:GridView ID="grdReports" runat="server" ForeColor="White" Width="100%" CssClass="EU_DataTable"
                                        GridLines="Both" Font-Bold="true" OnDataBinding="gvMenu_DataBinding" OnRowDataBound="grdReports_RowDataBound">
                                        <EditRowStyle BackColor="#999999" />
                                        <FooterStyle BackColor="Gray" Font-Bold="False" ForeColor="White" />
                                        <HeaderStyle BackColor="#f4f4f4" Font-Bold="False" ForeColor="Black" Font-Italic="False"
                                            Font-Names="Raavi" Font-Size="Small" />
                                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                        <RowStyle BackColor="#ffffff" ForeColor="#333333" HorizontalAlign="Center" />
                                        <AlternatingRowStyle HorizontalAlign="Center" />
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                    </asp:GridView>
                                </div>
                                <br />
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 25%;">
                                            <span style="font-weight: bold; font-size: 12px;">INCHARGE SIGNATURE</span>
                                        </td>
                                        <td style="width: 25%;">
                                            <span style="font-weight: bold; font-size: 12px;">ACCOUNTS DEPARTMENT</span>
                                        </td>
                                        <td style="width: 25%;">
                                            <span style="font-weight: bold; font-size: 12px;">AUTHORISED SIGNATURE</span>
                                        </td>
                                        <td style="width: 25%;">
                                            <span style="font-weight: bold; font-size: 12px;">PREPARED BY</span>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:Label ID="lblmsg" runat="server" Text="" ForeColor="Red" Font-Size="20px"></asp:Label>
                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/exporttoxl.aspx">Export to XL</asp:HyperLink>
                <br />
                <asp:Button ID="btnPrint" runat="Server" CssClass="btn btn-primary" OnClientClick="javascript:CallPrint('divPrint');"
                    Text="Print" />
            </div>
        </div>
    </section>
</asp:Content>
