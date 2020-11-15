<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" CodeFile="RptGpsAlerts.aspx.cs" Inherits="RptGpsAlerts" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     <style type="text/css">
        .container
        {
            max-width: 100%;
        }
    </style>
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
    <script type="text/javascript">
        function exportfn() {
            window.location = "exporttoxl.aspx";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
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
            Gps Alert Report<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Gps Alert Report</a></li>
            <li><a href="#">Gps Alert Report</a></li>
        </ol>
    </section>
    <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Gps Alert Report
                </h3>
            </div>
        <div class="box-body">
             <asp:UpdatePanel ID="updPanel" runat="server">
                    <ContentTemplate>
                      <asp:Panel ID="PanelHide" runat="server" Visible="true">
                            <div id="divPrint">
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
                            <div style="width: 100%;" align="center">
                                <span style="font-size: 18px; font-weight: bold; color: #0252aa;">Gps Alert Report</span><br />
                                <br />
                                <div>
                                </div>
                            </div>
                             <div align="center">
                                <div>
                                    <asp:GridView ID="grdReports" runat="server" ForeColor="White" Width="100%" CssClass="EU_DataTable" OnRowDataBound="grdReports_RowDataBound"  OnDataBinding="grdReports_DataBinding"
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
                                </div>
                            </div>
                          </div>   
                            <br />
                           <br />
                          <div style="float:left;">
            <asp:Button ID="Button1"  Text="Generate" runat="server" CssClass="btn btn-primary" OnClick="btnsave_Click" />            
            </div>
            <div style="float:right;">
            <button type="button" class="btn btn-primary" style="margin-right: 5px;" onclick="javascript:CallPrint('divPrint');"> <i class="fa fa-print"></i>Print</button>
             </div>                         
                        </asp:Panel>
                            </ContentTemplate>
                </asp:UpdatePanel>
           
            
             <asp:Label ID="lblmsg" runat="server" Text="" ForeColor="Red" Font-Size="20px"></asp:Label>
        </div>
      </div>
</asp:Content>

