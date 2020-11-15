<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="PrintTyreRethread.aspx.cs" Inherits="PrintTyreRethread" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function CallPrint(strid) {
            var divToPrint = document.getElementById(strid);
            var newWin = window.open('', 'Print-Window', 'width=400,height=400,top=100,left=100');
            newWin.document.open();
            newWin.document.write('<html><body   onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
            newWin.document.close();
        }
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="second_div" style=" padding: 20px;">
        <div class="tab-content">
            <div>
                <asp:UpdateProgress ID="updateProgress1" runat="server">
                    <ProgressTemplate>
                        <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0;
                            right: 0; left: 0; z-index: 9999; background-color: #FFFFFF; opacity: 0.7;">
                            <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="thumbnails/loading.gif"
                                Style="padding: 10px; position: absolute; top: 40%; left: 40%; z-index: 99999;" />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table id="tbltrip">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_tripid" runat="server">Tyre Rethread Sno:</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRethreadSno" runat="server" CssClass="form-control"></asp:TextBox>
                            </td>
                            <td>
                            </td>
                            <td>
                                <asp:Button ID="btnGenerate" runat="server" CssClass="btn btn-primary" OnClick="btnGenerate_Click"
                                    Text="Generate" />
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
                            <div>
                            </div>
                            <div style="width: 100%;">
                                <span style="font-size: 16px; font-weight: bold; padding-left: 25%; text-decoration: underline;">
                                    DELIVERY CHALLAN</span><br />
                                <span style="font-size: 16px; font-weight: bold; padding-left: 25%; color: #0252aa">
                                    DC No:</span>
                                <asp:Label ID="lblTripNo" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
                                <br />
                                <br />
                            </div>
                            <table style="width: 80%">
                                <tr>
                                    <td>
                                        Date
                                    </td>
                                    <td>
                                        <asp:Label ID="lblDate" runat="server" Text="" ForeColor="Red"></asp:Label>
                                    </td>
                                    <td>
                                        Time:
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTime" runat="server" Text="" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <asp:GridView ID="grdReports" runat="server" ForeColor="White" Width="100%" CssClass="EU_DataTable"
                            GridLines="Both" Font-Bold="true">
                            <EditRowStyle BackColor="#999999" />
                            <FooterStyle BackColor="Gray" Font-Bold="False" ForeColor="White" />
                            <HeaderStyle BackColor="#f4f4f4" Font-Bold="False" ForeColor="Black" Font-Italic="False"
                                Font-Names="Raavi" Font-Size="Small" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#ffffff" ForeColor="#333333" HorizontalAlign="Center" />
                            <AlternatingRowStyle HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        </asp:GridView>
                        <br />
                        <table style="width: 100%;">
                            <tr>
                                <td style="width: 50%;">
                                    <span style="font-weight: bold; font-size: 12px;">Prepared by</span>
                                </td>
                                <td style="width: 50%; float: right;">
                                    <span style="font-weight: bold; font-size: 12px;">Authorised Signature</span>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <asp:Label ID="lblmsg" runat="server" Text="" ForeColor="Red" Font-Size="20px"></asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>
            <br />
            <br />
            <asp:Button ID="btnPrint" runat="Server" CssClass="btn btn-primary" OnClientClick="javascript:CallPrint('divPrint');"
                Text="Print" />
        </div>
    </div>
</asp:Content>
