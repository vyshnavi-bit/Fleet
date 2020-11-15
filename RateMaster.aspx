<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="RateMaster.aspx.cs" Inherits="RateMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/JTemplate.js" type="text/javascript"></script>
    <script src="js/utility.js" type="text/javascript"></script>
    <style type="text/css">
        input[type=number]::-webkit-inner-spin-button, input[type=number]::-webkit-outer-spin-button
        {
            -webkit-appearance: none;
            margin: 0;
        }
    </style>
    <script type="text/javascript">
        function get_Vehicle_details() {
            var ddlVehicletype = document.getElementById('ddlVehicletype').value;
            var data = { 'op': 'GetVehiclerents', 'ddlVehicletype': ddlVehicletype };
            var s = function (msg) {
                if (msg) {
                    $('#divFillScreen').removeTemplate();
                    $('#divFillScreen').setTemplateURL('puffratemaster.htm');
                    $('#divFillScreen').processTemplate(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function btnUpdatePuffRent(ID) {
            var VehilceSno = $(ID).closest("tr").find('#hdnVehicleSno').val();
            var Cost = $(ID).closest("tr").find('#txtCost').val();
            var CostPerKm = $(ID).closest("tr").find('#txtCostPerKm').val();
            if (Cost == "") {
                alert("Please Enter Cost");
                return false;
            }
            var data = { 'op': 'btnUpdatePuffRentclick', 'VehilceSno': VehilceSno, 'Cost': Cost, 'CostPerKm': CostPerKm };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    get_Vehicle_details();
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
    <section class="content-header">
        <h1>
            Rate Master<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Rate Master</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Rate Master Details
                </h3>
            </div>
            <div class="box-body">
                <table align="center">
                    <tr>
                        <td>
                            <label>
                                Vehicle Type</label>
                        </td>
                        <td>
                            <select id="ddlVehicletype" class="form-control" >
                            <option value="7">Puff</option>
                            <option value="22">Tanker</option>
                            </select>
                        </td>
                        <td style="width: 5px;">
                        </td>
                        <td>
                            <input id="Button1" type="button" class="btn btn-primary" value="Get Trip Details"
                                onclick="get_Vehicle_details()" />
                        </td>
                    </tr>
                </table>
                <div id="divFillScreen">
                </div>
            </div>
        </div>
    </section>
</asp:Content>
