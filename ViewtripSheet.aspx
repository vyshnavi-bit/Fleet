<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" EnableEventValidation="false"
    CodeFile="ViewtripSheet.aspx.cs" Inherits="ViewtripSheet" %>

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
    <div id="second_div" style="padding: 20px;">
        <div class="tab-content">
            <div>
                <asp:UpdateProgress ID="updateProgress1" runat="server">
                    <progresstemplate>
                        <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0;
                            right: 0; left: 0; z-index: 9999; background-color: #FFFFFF; opacity: 0.7;">
                            <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="thumbnails/loading.gif"
                                Style="padding: 10px; position: absolute; top: 40%; left: 40%; z-index: 99999;" />
                        </div>
                    </progresstemplate>
                </asp:UpdateProgress>
            </div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <contenttemplate>
                    <table id="tbltrip">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_tripid" runat="server">Trip Ref No:</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTripRefNo" runat="server" CssClass="txtsize"></asp:TextBox>
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
                                    TRIP SHEET</span><br />
                               <span style="font-size: 16px; font-weight: bold; padding-left: 10%; color: #0252aa">
                                    Trip Ref No:</span>
                                <asp:Label ID="lblTripRefNo" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
                                  <span style="font-size: 16px; font-weight: bold; padding-left: 25%; color: #0252aa">
                                    Trip Sheet No:</span>
                                <asp:Label ID="lblTripsheetNo" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
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
                                <tr>
                                    <td>
                                        Vehicle No
                                    </td>
                                    <td>
                                        <asp:Label ID="lblVehicleNo" runat="server" Text="" ForeColor="Red"></asp:Label>
                                    </td>
                                    <td>
                                        Make
                                    </td>
                                    <td>
                                        <asp:Label ID="lblMake" runat="server" Text="" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    
                                    <td>
                                        Vehicle Type
                                    </td>
                                    <td>
                                        <asp:Label ID="lblVehicleType" runat="server" Text="" ForeColor="Red"></asp:Label>
                                    </td>
                                    <td>
                                        Assign Route
                                    </td>
                                    <td>
                                        <asp:Label ID="lblAssignRoute" runat="server" Text="" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Type Of Load
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTypeOfLoad" runat="server" Text="" ForeColor="Red"></asp:Label>
                                    </td>
                                     <td>
                                        Driver Name
                                    </td>
                                    <td>
                                        <asp:Label ID="lblDriverName" runat="server" Text="" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                   <td>
                                        Mob No:
                                    </td>
                                    <td>
                                        <asp:Label ID="lblPhoneNo" runat="server" Text="" ForeColor="Red"></asp:Label>
                                    </td>
                                    <td>
                                        Licencedetails:DL.NO:
                                    </td>
                                    <td>
                                        <asp:Label ID="lblLicenceNo" runat="server" Text="" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <asp:GridView ID="grdReports" runat="server" CellPadding="5" CellSpacing="5" CssClass="gridcls"
                            ForeColor="White" GridLines="Both" Height="400px" Width="100%" Font-Size="Small">
                            <EditRowStyle BackColor="#999999" />
                            <FooterStyle BackColor="Gray" Font-Bold="False" ForeColor="White" />
                            <HeaderStyle BackColor="#f4f4f4" Font-Bold="true" Font-Italic="False" Font-Names="Raavi"
                                Font-Size="13px" ForeColor="Black" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#ffffff" ForeColor="#333333" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        </asp:GridView>
                        <br />
                        <%--  <table style="width: 80%">
                                <tr>
                                    <td>
                                        Total Expenses
                                    </td>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text="" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                </table>--%>
                                               <table style="width: 100%;">
                            <tr>
                             <td style="width: 25%;">
                                    <span style="font-weight: bold; font-size: 12px;">PREPARED BY</span>
                                </td>
                                <td style="width: 25%;">
                                    <span style="font-weight: bold; font-size: 12px;">INCHARGE SIGNATURE</span>
                                </td>
                                <td style="width: 25%;">
                                    <span style="font-weight: bold; font-size: 12px;">DRIVER SIGNATURE</span>
                                </td>
                                <td style="width: 25%;">
                                    <span style="font-weight: bold; font-size: 12px;">AUTHORISED SIGNATURE</span>
                                </td>
                            </tr>
                        </table>
                    </div>
                     <asp:Label ID="lblmsg" runat="server" Text="" ForeColor="Red" Font-Size="20px"></asp:Label>
                </contenttemplate>
            </asp:UpdatePanel>
            <br />
            <br />
            <asp:Button ID="btnPrint" runat="Server" CssClass="btn btn-primary" OnClientClick="javascript:CallPrint('divPrint');"
                Text="Print" />
        </div>
    </div>
</asp:Content>
