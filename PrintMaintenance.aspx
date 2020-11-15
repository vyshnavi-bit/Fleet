<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" EnableEventValidation="false"
    CodeFile="PrintMaintenance.aspx.cs" Inherits="PrintMaintenance" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Css/VyshnaviStyles.css" rel="stylesheet" type="text/css" />
    <script src="Js/JTemplate.js?v=3000" type="text/javascript"></script>
    <script src="JSF/jquery.blockUI.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="Css/VyshnaviStyles.css" />
    <script src="js/utility.js" type="text/javascript"></script>
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
        $(function () {
            window.history.forward(1);
        });
        function PopupOpen(MaintenanceID) {
            var data = { 'op': 'GetSubmaintenses', 'MaintenanceID': MaintenanceID };
            var s = function (msg) {
                if (msg) {
                    $('#divHead').setTemplateURL('SubPayable.htm');
                    $('#divHead').processTemplate(msg);
                    var TotRate = 0.0;
                    $('.AmountClass').each(function (i, obj) {
                        if ($(this).text() == "") {
                        }
                        else {
                            TotRate += parseFloat($(this).text());
                        }
                    });
                    TotRate = parseFloat(TotRate).toFixed(2);
                    document.getElementById("txt_total").innerHTML = TotRate;
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
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
                                <asp:Label ID="lbl_tripid" runat="server">Maintenance Code:</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtmaintenancecode" runat="server" CssClass="form-control"></asp:TextBox>
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
                                <img src="Images/Vyshnavilogo.png" alt="Vyshnavi" width="90px" height="62px" />
                            </div>
                            <div align="center">
                                <asp:Label ID="lblTitle" runat="server" Font-Bold="true" Font-Size="20px" ForeColor="#0252aa"
                                    Text=""></asp:Label>
                                <br />
                                <asp:Label ID="lblAddress" runat="server" Font-Bold="true" Font-Size="12px" ForeColor="#0252aa"
                                    Text=""></asp:Label>
                                    <br />
                                    <span style="font-size:12px;font-weight:bold;">Maintenance Code:</span>    
                                        <asp:Label ID="lblMaintenanceCode" runat="server" Font-Bold="true" Font-Size="12px" Text="" ForeColor="#0252aa"></asp:Label>
                                <br />
                                <br />
                            </div>
                        </div>
                        <div>
                        </div>
                        <div>
                            <table style="width:100%;">
                                <tr>
                                    <td >
                                        Date
                                    </td>
                                    <td >
                                        <asp:Label ID="lblDate" runat="server" Text="" ForeColor="Red"></asp:Label>
                                    </td>
                                    <td>
                                        Time:
                                    </td>
                                    <td >
                                        <asp:Label ID="lblTime" runat="server" Text="" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                  
                                    <td >
                                        Vehicle No
                                    </td>
                                      <td >
                                        <asp:Label ID="lblVehicleNo" runat="server" Text="" ForeColor="Red"></asp:Label>
                                    </td>
                                       <td >
                                        Make
                                    </td>
                                    <td >
                                        <asp:Label ID="lblMake" runat="server" Text="" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                   
                                       <td >
                                        Vehicle Type
                                    </td>
                                       <td >
                                        <asp:Label ID="lblVehicleType" runat="server" Text="" ForeColor="Red"></asp:Label>
                                    </td>
                                    <td >
                                        Name
                                    </td>
                                   <td >
                                        <asp:Label ID="lblName" runat="server" Text="" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                   
                                       <td >
                                        Incharge Name
                                    </td>
                                      <td >
                                        <asp:Label ID="lblIncharge" runat="server" Text="" ForeColor="Red"></asp:Label>
                                    </td>
                                      <td >
                                        Model
                                    </td>
                                      <td >
                                        <asp:Label ID="lblModel" runat="server" Text="" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" rowspan="1" style="width: 100%;">
                                        <div id="divHead">
                                        </div>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                <td style="width: 25%; float: left;">
                                    Towards
                                </td>
                                <td colspan="2">
                                    <asp:Label ID="lblTowards" runat="server" ForeColor="Red" Text=""></asp:Label>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 25%; float: left;">
                                    Received Rs.
                                </td>
                                <td  colspan="2" >
                                    <asp:Label ID="lblReceived" runat="server" ForeColor="Red" Text="" ></asp:Label>
                                </td>
                            </tr>
                            </table>
                            <br />
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
                    </div>
                    <asp:Label ID="lblmsg" runat="server" Text="" ForeColor="Red" Font-Size="20px"></asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>
            <br />
            <br />
            <input type="button" class="btn btn-primary" value="Print" onclick="javascript:CallPrint('divPrint');"
        </div>
    </div>
</asp:Content>
