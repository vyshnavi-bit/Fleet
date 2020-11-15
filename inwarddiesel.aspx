<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" CodeFile="inwarddiesel.aspx.cs" Inherits="inwarddiesel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<%-- <script src="js/date.format.js" type="text/javascript"></script>--%>
 <%--<script src="js/JTemplate.js?v=1001" type="text/javascript"></script>--%>
    <%--<script src="js/jquery.blockUI.js" type="text/javascript"></script>--%>
    <script type="text/javascript">
        $(function () {
            GetDieselQty();
            
        });
        $(document).ready(function () {
            $("#txtInwardDiesel,#txt_cpl").keydown(function (event) {
                // Allow: backspace, delete, tab, escape, and enter
                if (event.keyCode == 46 || event.keyCode == 110 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 || event.keyCode == 190 ||
                // Allow: Ctrl+A
            (event.keyCode == 65 && event.ctrlKey === true) ||
                // Allow: home, end, left, right
            (event.keyCode >= 35 && event.keyCode <= 39)) {
                    // let it happen, don't do anything
                    return;
                }
                else {
                    // Ensure that it is a number and stop the keypress
                    if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
                        event.preventDefault();
                    }
                }
            });

        });
        function GetDieselQty() {
            var data = { 'op': 'GetStockQty' };
            var s = function (msg) {
                if (msg) {
                    document.getElementById("txtOppQty").innerHTML = msg.fuel;
                    //document.getElementById("spn_cspl").innerHTML = msg.costperltr;
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function btnstockclosing() {
            var txt_date = document.getElementById("txt_date").value;
            if (txt_date == "") {
                alert("Please Select  Date");
                return false;
                $('#txt_date').focus();
            }
            var Qty = document.getElementById("txtClosingStock").value;
            var data = { 'op': 'btnstockclosing', 'Qty': Qty, 'date': txt_date };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    document.getElementById("txtClosingStock").value = "";
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function btnsaveInwardstock() {
            var Qty = document.getElementById("txtInwardDiesel").value;
            var txtOppQty = document.getElementById("txtOppQty").innerHTML;
            var litercost = document.getElementById("txt_cpl").value;
            if (Qty == "") {
                alert("Enter Inward Diesel");
                $('#txtInwardDiesel').focus();
                return false;
            }
            var data = { 'op': 'btnsaveInwardstock', 'Qty': Qty, 'txtOppQty': txtOppQty, 'litercost': litercost };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    document.getElementById("txtInwardDiesel").value = "";
                    GetDieselQty();
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function closeinward() {
            document.getElementById("txtInwardDiesel").value = "";
            document.getElementById('txt_cpl').value = "";
        }
        function callHandler(d, s, e) {
            $.ajax({
                url: 'FleetManagementHandler.axd',
                data: d,
                type: 'GET',
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                async: true,
                cache: true,
                success: s,
                error: e
            });
        }
        function divInwardDieselClick() {
            $('#divInward').css('display', 'block');
            $('#divclosing').css('display', 'none');
        }
        function divClosingStockClick() {
            $('#divInward').css('display', 'none');
            $('#divclosing').css('display', 'block');
            var today = new Date();
            var dd = today.getDate();
            var mm = today.getMonth() + 1; //January is 0!
            var yyyy = today.getFullYear();
            if (dd < 10) {
                dd = '0' + dd
            }
            if (mm < 10) {
                mm = '0' + mm
            }
            var hrs = today.getHours();
            var mnts = today.getMinutes();
            $('#txt_date').val(yyyy + '-' + mm + '-' + dd);
            var data = { 'op': 'GetStockQty' };
            var s = function (msg) {
                if (msg) {
                    document.getElementById("txtClosingStock").value = msg.fuel;
//                    document.getElementById("spn_cspl").innerHTML = msg.costperltr;
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
 <section class="content-header">
        <h1>
            Diesel Details<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Diesel Details</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
           
            <div class="box-body">
 <div style="width: 100%; background-color: #fff">
            <div role="tabpanel">
                <!-- Nav tabs -->
                <ul class="nav nav-tabs" role="tablist">
                    <li role="presentation" class="active"><a  onclick="divInwardDieselClick();" on aria-controls="TripStart"
                        role="tab" data-toggle="tab">Inward Diesel </a></li>
                    <li role="presentation"><a onclick="divClosingStockClick();" aria-controls="TripEnd" role="tab" data-toggle="tab">
                       Closing Stock</a></li>
                </ul>
    </div>
    <div id="divInward">
        <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Inward Diesel Details
                </h3>
            </div>
        <table align="center" >
            <tr>
                <td>
                   <label> stock Qty </label>
                </td>
                <td style="height:40px;">
                    <span id="txtOppQty" class="form-control" style="font-size: 18px; color: Red; font-weight: bold;">
                    </span>
                </td>
            </tr>
            <tr>
                <td>
                   <label> Inward Diesel <span style="color: red;">*</span> </label>
                </td>
                <td  style="height:40px;">
                    <input type="text" id="txtInwardDiesel" class="form-control" placeholder="Enter Inward Diesel" />
                </td>
            </tr>
            <tr>
                <td>
                   <label> Cost Pet Ltr  <span style="color: red;">*</span></label>
                </td>
                <td  style="height:40px;"> 
                    <input type="text" id="txt_cpl" class="form-control" placeholder="Enter Cost Per Liter" />
                </td>
            </tr>
            <tr>
            <td>
                </td>
                <td  style="height:40px;">
                    <%--<input type="button" id="btnSave" value="Save" onclick="btnsaveInwardstock();" class="btn btn-primary"  />--%>
                    </td>
            </tr>
        </table>
        <table>
        <tr>
            <td style="padding-left: 430px;">
                <div class="input-group">
                    <div class="input-group-addon">
                    <span class="glyphicon glyphicon-ok" id="btnSave1" onclick="btnsaveInwardstock()"></span><span id="btnSave" onclick="btnsaveInwardstock()">Save</span>
                </div>
                </div>
                </td>
                <td style="width:10px;"></td>
                <td>
                    <div class="input-group">
                    <div class="input-group-close">
                    <span class="glyphicon glyphicon-remove" id='close_inward1' onclick="closeinward()"></span><span id='close_inward' onclick="closeinward()">Clear</span>
                </div>
                </div>
            </td>
            </tr>
        </table>
    </div>
    <div id="divclosing" style="display:none;"  >
      <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Closing Stock Details
                </h3>
            </div>
        <table align="center">
        <tr>
         <td>
                <label> Closing Date </label>
                </td>
                <td  style="height:40px;">
                  <input type="date" id="txt_date" class="form-control"  />
                </td>
        </tr>
            <tr>
                <td>
                 <label>   stock Qty </label>
                </td>
                <td  style="height:40px;">
                <input type="text" id="txtClosingStock" class="form-control" placeholder="Enter Inward Diesel" />
                </td>
            </tr>
             <%--<tr>
                <td>
                </td>
                <td  style="height:40px;">
                    <input type="button" id="Button1" value="Stock Closing" onclick="btnstockclosing();" class="btn btn-primary" />
                </td>
            </tr>--%>
        </table>
        <table>
            <tr>
            <td style="padding-left: 440px;height: 50px;">
            <div class="input-group">
                <div class="input-group-addon">
                <span class="glyphicon glyphicon-ok" id="Button11" onclick="btnstockclosing()"></span><span id="Button1" onclick="btnstockclosing()">Stock Closing</span>
            </div>
            </div>
            </td>
            </tr>
        </table>
    </div>
    </div>
    </div>
    </div>
    </section>
</asp:Content>

