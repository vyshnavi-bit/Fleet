<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="PrintTripSheet.aspx.cs" Inherits="PrintTripSheet" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
 <link href="css/VyshnaviStyles.css?v=1801" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function CallPrint(strid) {
            var divToPrint = document.getElementById(strid);
            var newWin = window.open('', 'Print-Window', 'width=300,height=300,top=100,left=100');
            newWin.document.open();
            newWin.document.write('<html><body onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
            newWin.document.write('<link rel="stylesheet" type="text/css" href="css/print.css" />');
            newWin.document.close();
        }
    </script>
     <style type="text/css">
        .mylbl
        {
            font-size: 10px;
        }
        .mylbl1
        {
            font-size: 10px; /*font-weight: bold;
            
            color:#0252aa;
            font-family: ravvi;*/
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="second_div" style="padding: 20px;">
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
                                <asp:Label ID="lbl_tripid" runat="server">Trip Ref No:</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTripRefNo" runat="server" CssClass="form-control"></asp:TextBox>
                            </td>
                            <td style="width: 6px;">
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                                    <asp:ListItem>With Logo</asp:ListItem>
                                    <asp:ListItem>Without Logo</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 6px;">
                            </td>
                            <td>
                                <asp:Button ID="btnGenerate" runat="server" CssClass="btn btn-primary" OnClick="btnGenerate_Click"
                                    Text="Generate" />
                            </td>
                        </tr>
                    </table>
                    <div id="divPrint">
                        <div style="width: 100%;">
                            <asp:Panel ID="hidePanel" runat="server">
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
                                <span style="font-size: 16px; font-weight: bold; padding-left: 25%; text-decoration: underline;">
                                    TRIP SHEET</span><br />
                            </asp:Panel>
                            <div>
                            </div>
                            <div style="width: 100%;">
                                
                                <span style="font-size: 16px; font-weight: bold; padding-left: 10%; color: #0252aa">
                                    Trip Ref No:</span>
                                <asp:Label ID="lblTripRefNo" runat="server" Text="" CssClass="mylbl"></asp:Label>
                                <span style="font-size: 16px; font-weight: bold; padding-left: 25%; color: #0252aa">
                                    Trip Sheet No:</span>
                                <asp:Label ID="lblTripsheetNo" runat="server" Text="" CssClass="mylbl"></asp:Label>
                                <br />
                            </div>
                            <table style="width: 100%">
                                <tr>
                                    <td>
                                       <span class="mylbl"> Date</span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblDate" runat="server" Text="" CssClass="mylbl"></asp:Label>
                                    </td>
                                    <td>
                                      <span class="mylbl">  Time:</span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTime" runat="server" Text="" CssClass="mylbl"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                       <span class="mylbl"> Vehicle No</span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblVehicleNo" runat="server" Text="" CssClass="mylbl"></asp:Label>
                                    </td>
                                    <td>
                                     <span class="mylbl">   Make</span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblMake" runat="server" Text="" CssClass="mylbl"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                       <span class="mylbl"> Vehicle Type</span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblVehicleType" runat="server" Text="" CssClass="mylbl"></asp:Label>
                                    </td>
                                    <td>
                                       <span class="mylbl"> Assign Route</span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblAssignRoute" runat="server" Text="" CssClass="mylbl"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                       <span class="mylbl"> Type Of Load</span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTypeOfLoad" runat="server" Text="" CssClass="mylbl"></asp:Label>
                                    </td>
                                    <td>
                                       <span class="mylbl"> Driver Name</span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblDriverName" runat="server" Text="" CssClass="mylbl"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                       <span class="mylbl"> Mob No:</span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblPhoneNo" runat="server" Text="" CssClass="mylbl"></asp:Label>
                                    </td>
                                    <td>
                                       <span class="mylbl"> Licencedetails:DL.NO:</span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblLicenceNo" runat="server" Text="" CssClass="mylbl"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <asp:GridView ID="grdReports" runat="server" CellPadding="5" CellSpacing="5" CssClass="gridcls"
                            ForeColor="White" GridLines="Both"  Width="100%" Font-Size="Small">
                            <EditRowStyle BackColor="#999999" />
                            <FooterStyle BackColor="Gray" Font-Bold="False" ForeColor="White" />
                            <HeaderStyle BackColor="#f4f4f4" Font-Bold="true" Font-Italic="False" Font-Names="Raavi"
                                Font-Size="10px" ForeColor="Black" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#ffffff" ForeColor="#333333" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        </asp:GridView>
                    </div>
                    <asp:Label ID="lblmsg" runat="server" Text="" ForeColor="Red" Font-Size="20px"></asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>
            <br />
            <br />
            <button type="button" class="btn btn-primary" style="margin-right: 5px;" onclick="javascript:CallPrint('divPrint');">
                <i class="fa fa-print"></i>Print
            </button>
        </div>
    </div>
</asp:Content>
