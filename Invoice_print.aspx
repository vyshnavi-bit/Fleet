﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" EnableEventValidation="false"
    CodeFile="Invoice_print.aspx.cs" Inherits="Invoice_print" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Css/VyshnaviStyles.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
        function CallPrint(strid) {
            //            var prtContent = document.getElementById(strid);
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
    <section class="content-header">
        <h1>
            Invoice<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Reports</a></li>
            <li><a href="#">Invoice</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Invoice Details
                </h3>
            </div>
            <div class="box-body">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table>
                            <tr>
                              <td>
                                        <asp:Label ID="Label4" runat="server" Text="Label">Type</asp:Label>&nbsp;
                                        <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                                            <asp:ListItem>All</asp:ListItem>
                                            <asp:ListItem>Puff</asp:ListItem>
                                            <asp:ListItem>Tanker</asp:ListItem>
                                            <asp:ListItem>Truck</asp:ListItem>
                                            <asp:ListItem>Bus</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                     
                                    <td style="width:6px;"></td>
                                <td>
                                    <asp:Label ID="Label6" runat="server" Text="Label">Vehicle No</asp:Label>&nbsp;
                                    <asp:DropDownList ID="ddlVehicleNo" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 5px;">
                                </td>
                                   <td>
                                        <asp:Panel ID="hideroutes" runat="server" Visible="false">
                                            <asp:Label ID="Label5" runat="server" Text="Label">Route</asp:Label>&nbsp;
                                            <asp:DropDownList ID="ddlroutes" runat="server" CssClass="form-control">
                                            </asp:DropDownList>
                                        </asp:Panel>
                                    </td>
                                <td style="width: 5px;">
                                </td>
                                <td>
                                    <asp:Label ID="Label3" runat="server" Text="Label">Vendor Name</asp:Label>&nbsp;
                                    <asp:DropDownList ID="ddlVendorname" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 5px;">
                                </td>
                                <td>
                                    <asp:Label ID="Label1" runat="server" Text="Label">From Date</asp:Label>&nbsp;
                                    <asp:TextBox ID="dtp_FromDate" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="enddate_CalendarExtender" runat="server" Enabled="True"
                                        TargetControlID="dtp_FromDate" Format="dd-MM-yyyy HH:mm">
                                    </asp:CalendarExtender>
                                </td>
                                <td style="width: 6px;">
                                </td>
                                <td>
                                    <asp:Label ID="Label2" runat="server" Text="Label">To Date</asp:Label>&nbsp;
                                    <asp:TextBox ID="dtp_Todate" runat="server" CssClass="form-control">
                                    </asp:TextBox>
                                    <asp:CalendarExtender ID="enddate_CalendarExtender2" runat="server" Enabled="True"
                                        TargetControlID="dtp_Todate" Format="dd-MM-yyyy HH:mm">
                                    </asp:CalendarExtender>
                                </td>
                                        <td style="width: 5px;">
                                </td>
                                <td>
                                    <asp:Button ID="btnGenerate" Text="Generate" runat="server" OnClientClick="OrderValidate();"
                                        CssClass="btn btn-primary" OnClick="btnGenerate_Click" />
                                </td>
                            </tr>
                        </table>
                        <asp:Panel ID="PanelHide" runat="server" Visible="false">
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
                                    <div style="width: 100%;" align="center">
                                        <span style="font-size: 16px; font-weight: bold; padding-left: 25%; text-decoration: underline;">
                                            <asp:Label ID="lblType" runat="server" Text=""></asp:Label></span>
                                        <br />
                                        <span style="font-size: 16px; font-weight: bold; text-decoration: underline;">
                                            Tax Invoice</span><br />
                                    </div>
                                     <table border="2" style="width: 100%">
                                        <tr>
                                            <td>
                                           <span style="font-size: 16px; font-weight: bold; "> Bill From ,</span>
                                                <br />
                                               <label style="font-size: 12px;font-weight: bold !important;"> Name:</label>  <asp:Label ID="lbl_client" runat="server" Text=""></asp:Label>,
                                                <br />
                                             <label style="font-size: 12px;font-weight: bold !important;"> Address:</label>   <asp:Label ID="lbl_client_address1" runat="server" Text=""></asp:Label>,
                                                <br />
                                                <asp:Label ID="lbl_client_address2" runat="server" Text=""></asp:Label>,
                                                <br />
                                                <asp:Label ID="lbl_client_address3" runat="server" Text=""></asp:Label>,
                                                <br />
                                              <label style="font-size: 12px;font-weight: bold !important;"> GSTIN: </label> <asp:Label ID="lbl_client_gstno" runat="server" Text=""></asp:Label>
                                                <br />
                                             <label style="font-size: 12px;font-weight: bold !important;"> State Name: </label> <asp:Label ID="lbl_client_statename" runat="server" Text=""></asp:Label>
                                                <br />
                                              <label style="font-size: 12px;font-weight: bold !important;"> State Code: </label> <asp:Label ID="lbl_client_statecode" runat="server" Text=""></asp:Label>
                                            </td>
                                            <td>
                                            <span style="font-weight: bold;">Date:</span>
                                                <asp:Label ID="lbldate" runat="server" Text=""></asp:Label>
                                                <br />

                                                <span style="font-weight: bold;">Invoice no: </span>
                                                <asp:Label ID="lblinvoiceno" runat="server" Text=""></asp:Label>
                                                <br />
                                                <br />

                                                <span style="font-weight: bold;">Peroid From: </span>
                                                <asp:Label ID="lbl_peroidFrom" runat="server" Text="" ></asp:Label>
                                                <br />
                                                <span style="font-weight: bold;">To:</span>
                                                <asp:Label ID="lbl_peroidto" runat="server" Text=""></asp:Label>
                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                          <span style="font-size: 16px; font-weight: bold; ">  Bill To,</span>
                                                <br />
                                              <label style="font-size: 12px;font-weight: bold !important;"> Name:</label>  <asp:Label ID="lbl_Vendor_tile" runat="server" Text=""></asp:Label>
                                                <br />
                                               <label style="font-size: 12px;font-weight: bold !important;"> Address:</label>   <asp:Label ID="lbl_Vendor_address" runat="server" Text=""></asp:Label>
                                                <br />
                                            <label style="font-size: 12px;font-weight: bold !important;"> GSTIN: </label>    <asp:Label ID="lbl_Vendor_gstno" runat="server" Text=""></asp:Label>
                                              <br />
                                             <label style="font-size: 12px;font-weight: bold !important;"> State Name: </label> <asp:Label ID="lbl_Vendor_statename" runat="server" Text=""></asp:Label>
                                                <br />
                                              <label style="font-size: 12px;font-weight: bold !important;"> State Code: </label> <asp:Label ID="lbl_Vendor_statecode" runat="server" Text=""></asp:Label>
                                            </td>
                                            <td>
                                                
                                                <br />
                                                <span style="font-weight: bold;font-size:18px;">Vehicle No:</span>
                                                <asp:Label ID="lbl_tankerNo" runat="server" Text="" Font-Size="18px" Font-Bold="true"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <br />
                                <div style="text-align: center; width: 100%">
                                    <asp:GridView ID="grdReports" runat="server" CellPadding="5" CellSpacing="5" CssClass="gridcls"
                                        ForeColor="White" GridLines="Both" Font-Size="Medium" Width="100%" OnDataBinding="gvMenu_DataBinding">
                                        <EditRowStyle BackColor="#999999" />
                                        <FooterStyle BackColor="Gray" Font-Bold="False" ForeColor="White" />
                                        <HeaderStyle BackColor="#f4f4f4" Font-Bold="true" Font-Italic="False" Font-Names="Raavi"
                                            Font-Size="13px" ForeColor="Black" />
                                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                        <RowStyle BackColor="#ffffff" ForeColor="#333333" HorizontalAlign="Center" />
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                    </asp:GridView>
                                </div>
                                <br />
                                <table border="2" align="center" style="width: 100%;">
                                    <tr>
                                        <td style="width: 49%;">
                                            SGST
                                        </td>
                                        <td>
                                         <asp:Label ID="lbl_sgst" runat="server" Text="" Font-Size="18px" Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 49%;">
                                            CGST
                                        </td>
                                           <td>
                                         <asp:Label ID="lbl_cgst" runat="server" Text="" Font-Size="18px" Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                      <tr>
                                        <td style="width: 49%;">
                                            IGST
                                        </td>
                                           <td>
                                         <asp:Label ID="lbl_igst" runat="server" Text="" Font-Size="18px" Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 49%;">
                                            Grand Total
                                        </td>
                                           <td>
                                         <asp:Label ID="lbl_grandtotal" runat="server" Text="" Font-Size="20px" Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <table align="center" style="width: 100%;">
                                    <tr>
                                        <td style="width: 49%;">
                                            Amount Chargeable ( IN WORDS)
                                            <br />
                                            INR:
                                            <asp:Label ID="lblamountinwords" runat="server" Text="" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <table align="center" style="width: 100%;">
                                    <tr>
                                        <td style="width: 33%;">
                                            <span style="font-weight: bold; font-size: 13px;">Prepared by </span>
                                        </td>
                                        <td style="width: 33%;">
                                            <span style="font-weight: bold; font-size: 13px;">Receivers Signature </span>
                                        </td>
                                        <td style="width: 33%;">
                                            <span style="font-weight: bold; font-size: 13px;">Authorised Signatory </span>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <br />
                            <br />
                        </asp:Panel>
                        <br />
                                    <asp:Button ID="Button1" Text="Save" runat="server" OnClientClick="OrderValidate();"
                                        CssClass="btn btn-success" OnClick="btnsave_Click" />
                                        <br />
                                        <br />
                        <button type="button" class="btn btn-primary" style="margin-right: 5px;" onclick="javascript:CallPrint('divPrint');">
                            <i class="fa fa-print"></i>Print
                        </button>
                        <asp:Label ID="lblmsg" runat="server" Text="" ForeColor="Red" Font-Size="20px"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </section>
</asp:Content>
