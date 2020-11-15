<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="AddBranches.aspx.cs" Inherits="AddBranches" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%--<style type="text/css">
        .dispalynone
        {
            display: none;
        }
        .inputstable
        {
            width: 100%;
        }
        .inputBox
        {
            border-radius: 3px;
            border: 1px solid #c6c6c6;
            box-shadow: 2px 2px 3px #eee inset;
            cursor: text;
            width: 250px;
            height: 30px;
        }
        
        .inputBox.focused
        {
            border: 1px solid orange !important;
        }
        
        .inputBox.focused input
        {
            box-shadow: 0px 0px 0px #fff;
        }
        
        .inputFieldWidget input
        {
            padding: 0px;
            width: 100%; /*background: transparent;*/
        }
        th
        {
            text-align: center;
        }
    </style>--%>
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
            retrivedata();
            $('.hiddenrow').hide();
//            $('#add_branch').click(function () {
//                $('.hiddenrow').hide();
//                $('#branch_fillform').css('display', 'block');
//                $('#branch_showlogs').css('display', 'none');
//                $('#div_branchname_table').css('display', 'none');
//                $("#lbl_branch_Name_error_msg").hide();
//                $("#lbl_invst").hide();
//                $("#lbl_outst").hide();
//                $("#lbl_workst").hide();
//                $('#lbl_email_error_msg').hide();
//                clearall();
//            });
//            $('#close_branch').click(function () {
//                $('.hiddenrow').hide();
//                $('#branch_fillform').css('display', 'none');
//                $('#branch_showlogs').css('display', 'block');
//                $('#div_branchname_table').css('display', 'block');
//                $("#lbl_branch_Name_error_msg").hide();
//                $("#lbl_invst").hide();
//                $("#lbl_outst").hide();
//                $("#lbl_workst").hide();
//                $('#lbl_email_error_msg').hide();
//            });
        });
        function addbranchdetails() {
            $('.hiddenrow').hide();
            $('#branch_fillform').css('display', 'block');
            $('#branch_showlogs').css('display', 'none');
            $('#div_branchname_table').css('display', 'none');
            $("#lbl_branch_Name_error_msg").hide();
            $("#lbl_invst").hide();
            $("#lbl_outst").hide();
            $("#lbl_workst").hide();
            $('#lbl_email_error_msg').hide();
            clearall();
        }
        function closebranches() {
            $('.hiddenrow').hide();
            $('#branch_fillform').css('display', 'none');
            $('#branch_showlogs').css('display', 'block');
            $('#div_branchname_table').css('display', 'block');
            $("#lbl_branch_Name_error_msg").hide();
            $("#lbl_invst").hide();
            $("#lbl_outst").hide();
            $("#lbl_workst").hide();
            $('#lbl_email_error_msg').hide();
        }
        function save_branchname() {
            var branchname = document.getElementById('txt_branchname').value;
            var address = document.getElementById('txt_address').value;
            var mobile = document.getElementById('txt_mobileno').value;
            var status = document.getElementById('cmb_branchstatus').value;
            var email = document.getElementById('txt_email').value;
            var sno = document.getElementById('txt_sno').value;
            var btnval = document.getElementById('save_branch').innerHTML;
            var inwardno = document.getElementById('txt_inwardstart').value;
            var outwardno = document.getElementById('txt_outwardstart').value;
            var workorderno = document.getElementById('txt_workorderstart').value;
            var flag = false;
            if (branchname == "Select Part Group") {
                $("#lbl_branch_Name_error_msg").show();
                $("#txt_branchname").focus();
                flag = true;
            }
            if (inwardno == "") {
                $("#lbl_invst").show();
                $("#txt_inwardstart").focus();
                flag = true;
            }
            if (outwardno == "") {
                $("#lbl_outst").show();
                $("#txt_outwardstart").focus();
                flag = true;
            }
            if (workorderno == "") {
                $("#lbl_workst").show();
                $("#txt_workorderstart").focus();
                flag = true;
            }
            if (!validateEmail(email)) {
                $("#lbl_email_error_msg").show();
                $("#txt_email").show();
                flag = true;
            }
            if (flag) {
                return;
            }
            var data = { 'op': 'save_Branch_data', 'branchname': branchname, 'address': address, 'mobile': mobile
            , 'status': status, 'email': email, 'sno': sno, 'btnval': btnval, 'inwardno': inwardno, 'outwardno': outwardno, 'workorderno': workorderno
            };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        if (msg == "OK") {
                            alert("New Branch Name Successfully Added");
                            $('#branch_fillform').css('display', 'none');
                            $('#branch_showlogs').css('display', 'block');
                            $('#div_branchname_table').css('display', 'block');
                            //$('#save_branch').val("Save");
                            document.getElementById('save_branch').innerHTML = "Save";
                            clearall();
                            retrivedata();
                            $("#lbl_branch_Name_error_msg").hide();
                            $("#lbl_invst").hide();
                            $("#lbl_outst").hide();
                            $("#lbl_workst").hide();
                            $('#lbl_email_error_msg').hide();
                        }
                        else if (msg == "UPDATE") {
                            alert("Branch Name Successfully Updated");
                            $('#branch_fillform').css('display', 'none');
                            $('#branch_showlogs').css('display', 'block');
                            $('#div_branchname_table').css('display', 'block');
                            //$('#save_branch').val("Save");
                            document.getElementById('save_branch').innerHTML = "Save";
                            retrivedata();
                            clearall();
                            $("#lbl_branch_Name_error_msg").hide();
                            $("#lbl_invst").hide();
                            $("#lbl_outst").hide();
                            $("#lbl_workst").hide();
                            $('#lbl_email_error_msg').hide();
                        }
                        else {
                            alert(msg);
                        }
                    }
                    else {

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

        function retrivedata() {
            var table = document.getElementById("tbl_branchname");
            for (var i = table.rows.length - 1; i > 0; i--) {
                table.deleteRow(i);
            }
            var data = { 'op': 'get_all_BranchName_data' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillbranchdata(msg);
                        $('.hiddenrow').show();
                    }
                    else {

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
        function fillbranchdata(results) {
            var k = 0;
            var colorue = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            var table = document.getElementById("tbl_branchname");
            for (var i = 0; i < results.length; i++) {
                if (results[i].branchname != null) {
                    var branchname = results[i].branchname;
                    var brnch_address = results[i].brnch_address;
                    var brnch_mobno = results[i].brnch_mobno;
                    var brnch_email = results[i].brnch_email;
                    var brnch_inward_start = results[i].brnch_inward_start;
                    var brnch_outward_start = results[i].brnch_outward_start;
                    var brnch_workorder_start = results[i].brnch_workorder_start;
                    var brnch_status = results[i].brnch_status;
                    var status = "";
                    if (brnch_status == "1") {
                        status = "Enabled";
                    }
                    else {
                        status = "Disabled";
                    }
                    var brnch_sno = results[i].brnch_sno;
                    var tablerowcnt = document.getElementById("tbl_branchname").rows.length;
                    $('#tbl_branchname').append('<tr style="background-color:' + colorue[k] + '">' +
                    //'<td scope="row" style="font-weight: 600;" id="txt_branchname" class="0">' + branchname + '</td>' + 
                    '<td style="font-weight: 600;"><i class="" aria-hidden="true"></i>&nbsp;<span id="0">' + branchname + '</span></td>' +   
                    '<td data-title="Address" style="display: none;" class="1">' + brnch_address + '</td>' +
                    '<td ><i class="fa fa-phone" aria-hidden="true"></i>&nbsp;<span id="2">' + brnch_mobno + '</span></td>' +
                    '<td ><i class="fa fa-envelope" aria-hidden="true"></i>&nbsp;<span id="3">' + brnch_email + '</span></td>' +    
                    '<td data-title="Inward Start No" class="4">' + brnch_inward_start + '</td>' +   
                    '<td data-title="Outward Start No"  class="5">' + brnch_outward_start + '</td>' +   
                    '<td data-title="Workorder Start No"  class="6">' + brnch_workorder_start + '</td>' +  
                    '<td data-title="Status" class="7" >' + status + '</td>' + 
                    '<td data-title="sno" style="display:none;" class="8" >' + brnch_sno + '</td>' + 
                    //'<td><input type="button" class="btn btn-primary" name="Update" value ="Modify" onclick="updateclick(this);"/></td></tr>');
                    '<td><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls" name="Update" value ="Modify"  onclick="updateclick(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>');
                    k = k + 1;
                    if (k == 4) {
                        k = 0;
                    }
                }
            }
        }

        function updateclick(thisid) {
            var row = $(thisid).parents('tr');
            var branchname = $(thisid).parent().parent().find('#0').text();
            var brnch_address = $(thisid).parent().parent().children('.1').html();
            var brnch_mobno = $(thisid).parent().parent().find('#2').html();
            var brnch_email = $(thisid).parent().parent().find('#3').html();
            var brnch_inward_start = $(thisid).parent().parent().children('.4').html();
            var brnch_outward_start = $(thisid).parent().parent().children('.5').html();
            var brnch_workorder_start = $(thisid).parent().parent().children('.6').html();
            var brnch_status = $(thisid).parent().parent().children('.7').html();
            var sno = $(thisid).parent().parent().children('.8').html();
            var status = "";
            if (brnch_status == "Enabled") {
                status = "1";
            }
            else {
                status = "0";
            }
            document.getElementById('txt_branchname').value = branchname;
            document.getElementById('txt_address').value = brnch_address;
            document.getElementById('txt_mobileno').value = brnch_mobno;
            document.getElementById('cmb_branchstatus').value = status;
            document.getElementById('txt_email').value = brnch_email;
            document.getElementById('txt_sno').value = sno;
            document.getElementById('txt_inwardstart').value = brnch_inward_start;
            document.getElementById('txt_outwardstart').value = brnch_outward_start;
            document.getElementById('txt_workorderstart').value = brnch_workorder_start;
            $('#branch_fillform').css('display', 'block');
            $('#branch_showlogs').css('display', 'none');
            $('#div_branchname_table').css('display', 'none');
            //$('#save_branch').val("Modify");
            document.getElementById('save_branch').innerHTML = "Modify";
        }

        function clearall() {
            document.getElementById('txt_branchname').value = "";
            document.getElementById('txt_address').value = "";
            document.getElementById('txt_mobileno').value = "";
            document.getElementById('cmb_branchstatus').value = "1";
            document.getElementById('txt_email').value = "";
            document.getElementById('txt_sno').value = "";
            document.getElementById('save_branch').innerHTML = "Save";
            document.getElementById('txt_inwardstart').value = "";
            document.getElementById('txt_outwardstart').value = "";
            document.getElementById('txt_workorderstart').value = "";
        }
        function validateEmail(email) {
            var reg = /^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/
            if (reg.test(email)) {
                return true;
            }
            else {
                return false;
            }
        } 
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Branch Master<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Branch Master</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Branch Master Details
                </h3>
            </div>
            <div class="box-body">
                <div id="branch_showlogs" >
                    <%--<input id="add_branch" type="button" name="submit" value='Add Branch' class="btn btn-primary" />--%>
                    <div class="input-group" style="width: 100px;padding-left: 960px;">
                                <div class="input-group-addon">
                                <span  class="glyphicon glyphicon-plus-sign" onclick="addbranchdetails()"></span><span onclick="addbranchdetails()">Add Branch</span>
                          </div>
                          </div>
                    <br />
                </div>
                <div id='branch_fillform' style="display: none;padding-left: 20%;">
                    <div>
                        <table>
                            <tr>
                                <td style="height: 40px;">
                                <label>
                                    Branch Name</label><span style="color: red;">*</span>
                                  </td>
                                <td>
                                    <input id="txt_branchname" type="text" name="vendorcode" class="form-control" placeholder="Branch Name">
                                    <label id="lbl_branch_Name_error_msg" class="errormessage">
                                        * Please Enter Branch Name</label>
                                </td>
                            <td style="width: 5px;">
                             </td>
                                <td style="height: 40px;">
                                    <label> Address </label>
                                      </td>
                                <td>
                                    <input type="text" id="txt_address" class="form-control" name="vendorcode" placeholder="Address">
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 40px;">
                                   <label> Phone Number </label>
                                     </td>
                                <td>
                                    <input type="text" id="txt_mobileno" maxlength="45" class="form-control" name="vendorcode"
                                        placeholder="Phone Number">
                                </td>
                           <td style="width: 5px;">
                             </td>
                                <td style="height: 40px;">
                                   <label> Email</label>
                                     </td>
                                <td>
                                    <input type="text" id="txt_email" maxlength="45" class="form-control" name="vendorcode"
                                        placeholder="Email">
                                    <label id="lbl_email_error_msg" class="errormessage">
                                        * Please Enter Proper Email ID</label>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 40px;">
                                  <label>  Inward Start Number</label><span style="color: red;">*</span>
                                    </td>
                                <td>
                                    <input type="text" id="txt_inwardstart" maxlength="45" class="form-control" name="vendorcode"
                                        placeholder="Inward Start Number">
                                    <label id="lbl_invst" class="errormessage">
                                        * Please Enter Inward Start Number</label>
                                </td>
                            <td style="width: 5px;">
                             </td>
                                <td style="height: 40px;">
                                   <label> Outward Start Number </label><span style="color: red;">*</span>
                                     </td>
                                <td>
                                    <input type="text" id="txt_outwardstart" maxlength="45" class="form-control" name="vendorcode"
                                        placeholder="Outward Start Number">
                                    <label id="lbl_outst" class="errormessage">
                                        * Please Enter Outward Start Number</label>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 40px;">
                                    <label>Workorder Start Number </label><span style="color: red;">*</span>
                                      </td>
                                <td>
                                    <input type="text" id="txt_workorderstart" maxlength="45" class="form-control" name="vendorcode"
                                        placeholder="Workorder Start Number">
                                    <label id="lbl_workst" class="errormessage">
                                        * Please Enter Work Order Number</label>
                                </td>
                            </tr>
                            <tr style="display: none;" class="hiddenrow">
                                <td>
                                   <label> Status</label>
                                     </td>
                                <td>
                                    <select id="cmb_branchstatus" class="form-control">
                                        <option value="1">Enabled</option>
                                        <option value="0">Disabled</option>
                                    </select>
                                </td>
                            </tr>
                            <tr style="display: none;">
                                <td>
                                    Sno
                                </td>
                                <td>
                                    <input type="text" id="txt_sno" maxlength="45" name="vendorcode" placeholder="sno">
                                </td>
                            </tr>
                            <tr>
                             </tr>
                        </table>
                               <%-- <td colspan="2" align="center" style="height: 40px;">
                                    <input id='save_branch' type="button" name="submit" value='Save' onclick="save_branchname()"
                                        class="btn btn-primary" />
                                    <input id='close_branch' type="button" name="Close" value='Close' class="btn btn-primary" />
                                </td>--%>
                                <div style="padding-left: 270px;padding-top: 10px;">
                                <table>
                                <tr>
                                <td>
                                </td>
                                <td>
                                    <div class="input-group">
                                        <div class="input-group-addon">
                                        <span class="glyphicon glyphicon-ok" id="save_branch1" onclick="save_branchname()"></span><span id="save_branch" onclick="save_branchname()">Save</span>
                                  </div>
                                  </div>
                                    </td>
                                    <td style="width:10px;"></td>
                                    <td>
                                     <div class="input-group">
                                        <div class="input-group-close">
                                        <span class="glyphicon glyphicon-remove" id='close_branch1' onclick="closebranches()"></span><span id='close_branch' onclick="closebranches()">Close</span>
                                  </div>
                                  </div>
                                    </td>
                                    </tr>
                                    </table>
                               </div>
                    </div>
                </div>
                <div>
                    <div id="div_branchname_table">
                        <table id="tbl_branchname" class="table table-bordered table-striped">
                            <thead>
                                <tr style="background:#5aa4d0; color: white; font-weight: bold;">
                                    <th scope="col">
                                        Branch Name
                                    </th>
                                    <th scope="col" style="display: none;">
                                        Address
                                    </th>
                                    <th scope="col">
                                        Phone Number
                                    </th>
                                    <th scope="col">
                                        Email
                                    </th>
                                    <th scope="col">
                                        Inward Start Number
                                    </th>
                                    <th scope="col">
                                        Outward Start Number
                                    </th>
                                    <th scope="col">
                                        Workorder Start Number
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
        </div>
    </section>
</asp:Content>
