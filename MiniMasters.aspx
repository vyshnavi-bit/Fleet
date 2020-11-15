<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="MiniMasters.aspx.cs" Inherits="MiniMasters" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
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
        function CallHandlerUsingJson(d, s, e) {
            d = JSON.stringify(d);
            d = d.replace(/&/g, '\uFF06');
            d = d.replace(/#/g, '\uFF03');
            d = d.replace(/\+/g, '\uFF0B');
            d = d.replace(/\=/g, '\uFF1D');
            $.ajax({
                type: "GET",
                url: "FleetManagementHandler.axd?json=",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: d,
                async: true,
                cache: true,
                success: s,
                error: e
            });
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
        $(function () {
            $('#btn_mastersub').click(function () {
                $('#masterstable_div').css('display', 'none');
                $('#masterinfo_div').css('display', 'block');
            });
            $('#btn_addmaster').click(function () {
                $('#masterstable_div').css('display', 'none');
                $('#masterinfo_div').css('display', 'block');
                $('#div_tblmasterdata').css('display', 'none');
                forclearall();
            });
            $('#close_minmaster').click(function () {
                $('#masterstable_div').css('display', 'none');
                $('#masterinfo_div').css('display', 'none');
                $('#div_tblmasterdata').show();
                $('#save_minmaster').val("Save");
                getmasterdata();
                forclearall();
            });
        });
        function getmasterdata() {
            var table = document.getElementById("tbl_mstrdata");
            for (var i = table.rows.length - 1; i > 0; i--) {
                table.deleteRow(i);
            }
            var minimaster = document.getElementById("slct_choosemaster").value;
            document.getElementById("h3_header").innerHTML = "Add " + minimaster;
            document.getElementById("lbl_minmstrname").innerHTML = minimaster + " Name";
            $("#txt_namemstr").attr('placeholder', minimaster + " Name");
            if (minimaster != "") {
                var data = { 'op': 'get_Mini_Master_data', 'minimaster': minimaster };
                var s = function (msg) {
                    if (msg) {
                        if (msg.length > 0) {
                            filldata(msg);
                            $('#masterstable_div').hide();
                            $('#masterinfo_div').css('display', 'none');
                            $('#div_tblmasterdata').show();
                            $('#btn_addmaster').val(minimaster + " Save");
                        }
                        else {
                            $('#masterstable_div').show();
                            $('#masterinfo_div').css('display', 'none');
                            $('#div_tblmasterdata').hide();
                        }
                    }
                    else {
                    }
                };
                var e = function (x, h, e) {
                }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
                callHandler(data, s, e);
            }
        }
        function filldata(results) {
            var table = document.getElementById("tbl_mstrdata");
            for (var i = 0; i < results.length; i++) {
                if (results[i].mm_type != null) {
                    var mm_type = results[i].mm_type;
                    var mm_name = results[i].mm_name;
                    var statuscode = results[i].mm_status;
                    var status = "";
                    if (statuscode == "1") {
                        status = "Enabled";
                    }
                    else {
                        status = "Disabled";
                    }
                    var sno = results[i].sno;
                    var tablerowcnt = document.getElementById("tbl_mstrdata").rows.length;
                    $('#tbl_mstrdata').append('<tr><th scope="row">' + mm_type + '</th><td data-title="Name">' + mm_name + '</td><td data-title="Status" >' + status + '</td><td data-title="sno" style="display:none;">' + sno + '</td><td><input type="button" class="btn btn-primary" name="Update" value ="Modify" onclick="updateclick(this);"/></td></tr>');
                }
            }
        }
        function updateclick(thisid) {
            var row = $(thisid).parents('tr');
            var sno = row[0].cells[3].innerHTML;
            var mm_type = row[0].cells[0].innerHTML;
            var mm_name = row[0].cells[1].innerHTML;
            var statuscode = row[0].cells[2].innerHTML;
            var status = "";
            if (statuscode == "Enabled") {
                status = "1";
            }
            else {
                status = "0";
            }
            document.getElementById("h3_header").innerHTML = "Add " + mm_type;
            document.getElementById("lbl_minmstrname").innerHTML = mm_type + " Name";
            $("#txt_namemstr").attr('placeholder', mm_type + " Name");
            document.getElementById('txt_namemstr').value = mm_name;
            document.getElementById('cmb_masterstatus').value = status;
            document.getElementById('txt_sno').value = sno;
            $('#masterstable_div').css('display', 'none');
            $('#masterinfo_div').css('display', 'block');
            $('#div_tblmasterdata').css('display', 'none');
            $('#save_minmaster').val("Modify");
        }

        function save_minmastr() {
            var master_type = document.getElementById('slct_choosemaster').value;
            var master_name = document.getElementById('txt_namemstr').value;
            var status = document.getElementById('cmb_masterstatus').value;
            var sno = document.getElementById('txt_sno').value;
            var btnval = document.getElementById('save_minmaster').value;
            var flag = false;
            if (master_name == "") {
                $("#lbl_part_Group_error_msg").show();
                flag = true;
            }
            if (flag) {
                return;
            }
            var data = { 'op': 'for_save_edit_MinMaster', 'master_type': master_type, 'master_name': master_name, 'status': status, 'sno': sno, 'btnval': btnval };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        if (msg == "OK") {
                            alert("New " + master_type + " Name Successfully Created");
                            forclearall();
                            getmasterdata();
                            $('#masterstable_div').css('display', 'none');
                            $('#masterinfo_div').css('display', 'none');
                            $('#div_tblmasterdata').show();
                            $('#save_minmaster').val("Save");
                            $('#btn_addmaster').val(master_type + " Save");
                        }
                        else if (msg == "UPDATE") {
                            alert(master_type + "Successfully Modified");
                            forclearall();
                            getmasterdata();
                            $('#masterstable_div').css('display', 'none');
                            $('#masterinfo_div').css('display', 'none');
                            $('#div_tblmasterdata').show();
                            $('#save_minmaster').val("Save");
                            $('#btn_addmaster').val(master_type + " Save");
                        }
                        else {
                            alert(msg);
                        }
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }

        function forclearall() {
            // document.getElementById('slct_choosemaster').value = "";
            document.getElementById('txt_namemstr').value = "";
            document.getElementById('cmb_masterstatus').value = "1";
            document.getElementById('txt_sno').value = "";
            document.getElementById('save_minmaster').value = "Save";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="minimasters_showlogs" style="text-align: center;">
        <div style="width: 100%; font-size: 20px;" align="center">
            <%--<div style="color:#fff;">Choose A Master</div><br />--%>
            <select id="slct_choosemaster" style="background: #fff; font-weight: bold; width: 75%;"
                onchange="getmasterdata()">
                <option value="" selected disabled style="display: none;">Choose A Master</option>
                <option value="PartGroup">PartGroup</option>
                <option value="RakeInfo">RakeInfo</option>
                <%--<option value="Category">Category</option>--%>
                <option value="CostCenterType">CostCenterType</option>
                <option value="Units">Units</option>
                <option value="Departments">Departments</option>
                <option value="VehicleType">VehicleType</option>
                <option value="VehicleMake">VehicleMake</option>
                <option value="Inspections">Inspections</option>
                <option value="Repairs">Repairs</option>
                <option value="TyreBrand">TyreBrand</option>
                <option value="TyreModel">TyreModel</option>
                <option value="TyreSize">TyreSize</option>
                <option value="TyreType">TyreType</option>
                <option value="JobCards">JobCards</option>
                <option value="VehicleTools">VehicleTools</option>
                <option value="BatteryMake">BatteryMake</option>
                <option value="" disabled></option>
            </select>
        </div>
        <br />
        <div id="masterstable_div" style="background: #fff; display: none;">
            <br />
            <div id="nocontent_div" align="center" style="color: Red;">
                ****No Content Found For This Master****</div>
            <div id="addsome_div" align="center">
                <input id="btn_mastersub" type="button" value="Click Here To Add" class="btn btn-primary" />
            </div>
            <br />
        </div>
        <div id='masterinfo_div' style="display: none; background: #ffffff; padding: 20px;">
            <div style="border: 1px solid #d5d5d5; margin-left: 18px; margin-top: 10px; margin-right: 5px;">
                <div style="padding: 8px 10px 5px 10px; background-color: #eee;">
                    Add Mini Master</div>
                <div align="center">
                    <h3>
                        <span class="label label-default" id="h3_header">New</span></h3>
                </div>
                <div style=" padding: 20px; text-align: center;">
                    <div class="row">
                        <div class="form-group">
                            <label id="lbl_minmstrname">
                            </label>
                            <span style="color: red;">*</span>
                            <input id="txt_namemstr" class="form-control" type="text" name="vendorcode">
                            <label id="lbl_part_Group_error_msg" class="errormessage">
                                * Please Enter The Name</label>
                        </div>
                        <div class="form-group">
                            <label>
                                Status</label>
                            <select id="cmb_masterstatus" class="allinputs" style="min-width: 200px;">
                                <option value="1" selected>Enabled</option>
                                <option value="0">Disabled</option>
                            </select>
                            <input type="text" id="txt_sno" maxlength="45" style="display: none;" placeholder="sno">
                        </div>
                    </div>
                    <div class="row">
                     <div class="form-group">
                            <input id='save_minmaster' type="button" class="btn btn-primary" name="submit" value='Save'
                                onclick="save_minmastr()" />
                            <input id='close_minmaster' type="button" class="btn btn-primary" name="Close" value='Close' />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div >
            <div id="div_tblmasterdata" class='divcontainer' style="display: none;">
                <div id="add_master" align="center">
                    <input id='btn_addmaster' type="button" class="btn btn-primary" />
                </div>
                <br />
                <table id="tbl_mstrdata" class="responsive-table">
                    <thead>
                        <tr>
                            <th scope="col">
                                Master Type
                            </th>
                            <th scope="col">
                                Name
                            </th>
                            <th scope="col">
                                Status
                            </th>
                            <th scope="col" style="display: none;">
                                sno
                            </th>
                            <th scope="col">
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
