<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" CodeFile="Service_Dashboard.aspx.cs" Inherits="Service_Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <script src="js/JTemplate.js" type="text/javascript"></script>
    <script src="js/utility.js" type="text/javascript"></script>
    <link href="bootstrap/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="bootstrap/RouteWiseTimeLine.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(function () {
            GetAssignTripSheets();
        });
        function GetAssignTripSheets() {
            var data = { 'op': 'GetVehicle_service_deatails' };
            var s = function (msg) {
                if (msg) {
                    if (msg == "No data found") {
                    }
                    else {
                        $('#divFillScreen').removeTemplate();
                        $('#divFillScreen').setTemplateURL('VehicleServicedetails.htm');
                        $('#divFillScreen').processTemplate(msg);
//                        blinkFont();
                    }
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Vehicle Service Details
                </h3>
            </div>
            <div class="box-body">
                <div id="divFillScreen" style="height: 8000px;">
                </div>
            </div>
        </div>
    </section>
</asp:Content>

