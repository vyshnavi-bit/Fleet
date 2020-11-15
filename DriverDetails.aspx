<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="DriverDetails.aspx.cs" Inherits="DriverDetails" %>

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
           Driver Details Report<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
               <li><a href="#"><i class="fa fa-dashboard"></i>Trip Reports</a></li>
            <li><a href="#"> Driver Details Report</a></li>
        </ol>
    </section>
    <section class="content">
    <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Driver Details
                </h3>
            </div>
            <div class="box-body" >
        <div id='vehmaster_fillform' style="border: 1px solid #d5d5d5;">
    <asp:UpdatePanel ID="updPanel" runat="server">
        <ContentTemplate>
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
                        <span style="font-size: 18px; font-weight: bold; color: #0252aa;">Driver Details Report</span><br />
                        <div>
                        </div>
                    </div>
                    <div>
                         <asp:GridView ID="grdEmployee" runat="server" ForeColor="White" AutoGenerateColumns="false" Width="100%" CssClass="gridcls"
                        GridLines="Both" Font-Bold="true">  
                         <EditRowStyle BackColor="#999999" />
                        <FooterStyle BackColor="Gray" Font-Bold="False" ForeColor="White" />
                        <HeaderStyle BackColor="#f4f4f4" Font-Bold="False" ForeColor="Black" Font-Italic="False"
                            Font-Names="Raavi" Font-Size="Small" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#ffffff" ForeColor="#333333" HorizontalAlign="Center" />
                        <AlternatingRowStyle HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <Columns>  
             <asp:BoundField HeaderText="Sno" DataField="Sno" />  
             <asp:BoundField HeaderText="EMP Sno" DataField="EMPSno" />  
              <asp:BoundField HeaderText="Driver Name" DataField="Driver Name" /> 
              <asp:BoundField HeaderText="Licence No" DataField="Licence No" />  
              <asp:BoundField HeaderText="Licence Exp Date" DataField="Licence Exp Date" />  
              <asp:BoundField HeaderText="Mob No" DataField="Mob No" />  
              <asp:BoundField HeaderText="Address" DataField="Address" />  
              <asp:BoundField HeaderText="Blood Group" DataField="Blood Group" />  
              <asp:BoundField HeaderText="Experience" DataField="Experience" />  
              <asp:BoundField HeaderText="DOJ" DataField="DOJ" />  
            </Columns>  
        </asp:GridView>  
                       
                    </div>
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
        </div>
        </section>
</asp:Content>
