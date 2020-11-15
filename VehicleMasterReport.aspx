<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="VehicleMasterReport.aspx.cs" Inherits="VehicleMasterReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .container
        {
            max-width: 100%;
        }
        .fixedColumn .fixedTable td
        {
            color: #FFFFFF;
            background-color: #5097d1;
            font-size: 14px;
            font-weight: normal;
            border-collapse: collapse;
        }
        
        .fixedHead td, .fixedFoot td
        {
            color: #FFFFFF;
            background-color: #5097d1;
            font-size: 14px;
            font-weight: normal;
            padding: 5px;
            border: 1px solid #187BAF;
        }
        .fixedTable td
        {
            font-size: 10pt;
            background-color: #FFFFFF;
            padding: 5px;
            text-align: left;
            border: 1px solid #CEE7FF;
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

        
    </script>
    <script type="text/javascript">
        $(document).ready(function () {

            $(".tableDiv").each(function () {
                var Id = $(this).get(0).id;
                var maintbheight = 455;
                var maintbwidth = 760;

                $("#" + Id + " .FixedTables").fixedTable({
                    width: maintbwidth,
                    height: maintbheight,
                    fixedColumns: 1,
                    // header style
                    classHeader: "fixedHead",
                    // footer style        
                    classFooter: "fixedFoot",
                    // fixed column on the left        
                    classColumn: "fixedColumn",
                    // the width of fixed column on the left      
                    fixedColumnWidth: 100,
                    // table's parent div's id           
                    outerId: Id,
                    // tds' in content area default background color                     
                    Contentbackcolor: "#FFFFFF",
                    // tds' in content area background color while hover.     
                    Contenthovercolor: "#99CCFF",
                    // tds' in fixed column default background color   
                    fixedColumnbackcolor: "#5097d1",
                    // tds' in fixed column background color while hover. 
                    fixedColumnhovercolor: "#99CCFF"
                });
            });
            $(".fixedTable table").attr('rules', 'All');
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
            Vehicle Master<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Vehicle Master</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Vehicle Master Details
                </h3>
            </div>
            <div class="box-body">
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
                                    <span style="font-size: 18px; font-weight: bold; color: #0252aa;">Vehicle Master Report</span><br />
                                    <br />
                                    <div>
                                    </div>
                                </div>
                                <div style="overflow: auto;">
                                    <asp:GridView ID="grdReports" runat="server" ForeColor="White" AutoGenerateColumns="false"
                                        Width="100%" GridLines="Both">
                                        <EditRowStyle BackColor="#999999" Font-Size="12px" />
                                        <FooterStyle BackColor="Gray" Font-Bold="False" ForeColor="White" />
                                        <HeaderStyle BackColor="#f4f4f4" Font-Bold="False" ForeColor="Black" Font-Italic="False"
                                            Font-Names="Raavi" Font-Size="Small" />
                                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                        <RowStyle BackColor="#ffffff" ForeColor="#333333" HorizontalAlign="Center" />
                                        <AlternatingRowStyle HorizontalAlign="Center" />
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        <Columns>
                                            <asp:BoundField HeaderText="Sno" DataField="Sno" />
                                            <asp:BoundField HeaderText="VehicleNo" DataField="VehicleNo" />
                                            <asp:BoundField HeaderText="Capacity" DataField="Capacity" />
                                            <asp:BoundField HeaderText="Type" DataField="Type" />
                                            <asp:BoundField HeaderText="Make" DataField="Make" />
                                            <asp:BoundField HeaderText="Owner" DataField="Owner" />
                                            <asp:BoundField HeaderText="Permitt" DataField="Permitt" />
                                            <asp:BoundField HeaderText="PermitExpDate" DataField="PermitExpDate" />
                                            <asp:BoundField HeaderText="PolExpDate" DataField="PolExpDate" />
                                            <asp:BoundField HeaderText="Insurance" DataField="Insurance" />
                                            <asp:BoundField HeaderText="InsExpDate" DataField="InsExpDate" />
                                            <asp:BoundField HeaderText="Fitness" DataField="Fitness" />
                                            <asp:BoundField HeaderText="FitExpDate" DataField="FitExpDate" />
                                            <asp:BoundField HeaderText="RoadTax" DataField="RoadTax" />
                                            <asp:BoundField HeaderText="RoadTaxExpDate" DataField="RoadTaxExpDate" />
                                            <asp:BoundField HeaderText="ModelNo" DataField="ModelNo" />
                                            <asp:BoundField HeaderText="EngineNo" DataField="EngineNo" />
                                            <asp:BoundField HeaderText="ChasisNo" DataField="ChasisNo" />
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
    </section>
</asp:Content>
