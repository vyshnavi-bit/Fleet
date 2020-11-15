<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" CodeFile="RouteAssign.aspx.cs" Inherits="RouteAssign" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <script src="js/utility.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            get_route_names();
            getallveh_nos();
        });

        function getallveh_nos() {
            var data = { 'op': 'get_all_vehhilcles' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillvehmasterdata(msg);
                    }
                    else {

                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fillvehmasterdata(msg) {
            var data = document.getElementById('slct_vehicle_no');
            var length = data.options.length;
            document.getElementById('slct_vehicle_no').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Vehicle No";
            opt.value = "Select Vehicle No";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            data.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].registration_no != null) {
                    var option = document.createElement('option');
                    option.innerHTML = msg[i].registration_no;
                    option.value = msg[i].registration_no;
                    data.appendChild(option);
                }
            }
        }
        function get_route_names() {
            var data = { 'op': 'get_route_names' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillroutenames(msg);
                    }
                    else {

                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fillroutenames(msg) {
            var data = document.getElementById('slct_route');
            var length = data.options.length;
            document.getElementById('slct_route').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Route Name";
            opt.value = "Select Route Name";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            data.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].DispName != null) {
                    var option = document.createElement('option');
                    option.innerHTML = msg[i].DispName;
                    option.value = msg[i].DispSno;
                    data.appendChild(option);
                }
            }
        }
        function btnRouteAssignClick() {
            var ddlroute = document.getElementById('slct_route').value;
            var ddlvehicle = document.getElementById('slct_vehicle_no').value;
            if (ddlroute == "Select Route Name" || ddlroute == "") {
                alert("Please Select Route Name");
                return false;
            }
            if (ddlvehicle == "Select Vehicle No") {
                alert("Please Select Vehicle No");
                return false;
            }
            var Data = { 'op': 'btnRouteAssignClick', 'ddlroute': ddlroute, 'ddlvehicle': ddlvehicle };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    document.getElementById('slct_route').selectedIndex = 0;
                    document.getElementById('slct_vehicle_no').selectedIndex = 0;
                }
            }
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(Data, s, e);
        }
        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  <div align="center">
        <span style="color: Red; font-weight: bold; font-size: 20px;">Route Assign</span>
    </div>
    <table align="center">
     <tr>
            <td>
                <label>
                 Route Name <span style="color: red;">*</span></label>
            </td>
            <td>
                <select id="slct_route" class="form-control">
                </select>
                <label id="Label3" class="errormessage">
                    * Please select Route Name</label>
            </td>
        </tr>
        <tr>
            <td>
                <label>
                    Vehicle Number <span style="color: red;">*</span></label>
            </td>
            <td>
                <select id="slct_vehicle_no" class="form-control">
                </select>
                <label id="lbl_error_selectveh" class="errormessage">
                    * Please select Vehicle</label>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <input id="btn_LoanEntry" type="button" value="Route Assign" class="btn btn-primary" onclick="btnRouteAssignClick();" />
            </td>
        </tr>
    </table>

</asp:Content>

