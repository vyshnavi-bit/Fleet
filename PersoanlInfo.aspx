<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="PersoanlInfo.aspx.cs" Inherits="PersoanlInfo" %>

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
            retrivedata();
            $('.hiddenrow').hide();
            $('#add_person').click(function () {
                $('.hiddenrow').hide();
                $('#person_fillform').css('display', 'block');
                $('#persons_showlogs').css('display', 'none');
                $('#div_perssons_table').css('display', 'none');
                $("#lbl_designation").hide();
                $("#lbl_mobileno").hide();
                $("#lbl_email").hide();
                $("#lbl_person_Name_error_msg").hide();
                clearall();
            });
            $('#close_employee').click(function () {
                $('.hiddenrow').hide();
                $('#person_fillform').css('display', 'none');
                $('#persons_showlogs').css('display', 'block');
                $('#div_perssons_table').css('display', 'block');
                $("#lbl_designation").hide();
                $("#lbl_mobileno").hide();
                $("#lbl_email").hide();
                $("#lbl_person_Name_error_msg").hide();
            });
        });
        function addpersonalinformation() {
            $('.hiddenrow').hide();
            $('#person_fillform').css('display', 'block');
            $('#persons_showlogs').css('display', 'none');
            $('#div_perssons_table').css('display', 'none');
            $("#lbl_designation").hide();
            $("#lbl_mobileno").hide();
            $("#lbl_email").hide();
            $("#lbl_person_Name_error_msg").hide();
            clearall();
        }
        function closebranches() {
            $('.hiddenrow').hide();
            $('#person_fillform').css('display', 'none');
            $('#persons_showlogs').css('display', 'block');
            $('#div_perssons_table').css('display', 'block');
            $("#lbl_designation").hide();
            $("#lbl_mobileno").hide();
            $("#lbl_email").hide();
            $("#lbl_person_Name_error_msg").hide();
        }
        function save_branchname() {
            var name = document.getElementById('txt_name').value;
            var designation = document.getElementById('txt_designation').value;
            var mobileno = document.getElementById('txt_mobileno').value;
            var alerttype = document.getElementById('cmb_alerttype').value;
            var email = document.getElementById('txt_email').value;
            var sno = document.getElementById('txt_sno').value;
            var btnval = document.getElementById('save_employee').innerHTML;
            var flag = false;
            if (name == "") {
                $("#lbl_person_Name_error_msg").show();
                $("#txt_name").focus();
                flag = true;
            }
            if (designation == "") {
                $("#lbl_designation").show();
                $("#txt_designation").focus();
                flag = true;
            }
            if (mobileno == "") {
                $("#lbl_mobileno").show();
                $("#txt_mobileno").focus();
                flag = true;
            }
            if (email == "") {
                $("#lbl_email").show();
                $("#txt_email").focus();
                flag = true;
            }
            if (flag) {
                return;
            }
            var data = { 'op': 'save_personal_info', 'name': name, 'designation': designation, 'mobileno': mobileno
            , 'alerttype': alerttype, 'email': email, 'sno': sno, 'btnval': btnval
            };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        alert(msg);
                        $('#person_fillform').css('display', 'none');
                        $('#persons_showlogs').css('display', 'block');
                        $('#div_perssons_table').css('display', 'block');
                        //$('#save_employee').val("Save");
                        document.getElementById('save_employee').innerHTML = "Save";
                        clearall();
                        retrivedata();
                        $("#lbl_designation").hide();
                        $("#lbl_mobileno").hide();
                        $("#lbl_email").hide();
                        $("#lbl_person_Name_error_msg").hide();
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
            var data = { 'op': 'get_all_personal_data' };
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
                if (results[i].name != null) {
                    var name = results[i].name;
                    var phoneno = results[i].phoneno;
                    var emailid = results[i].emailid;
                    var alert_type = results[i].alert_type;
                    var designation = results[i].designation;
                    var sno = results[i].sno;
                    var tablerowcnt = document.getElementById("tbl_branchname").rows.length;
                    $('#tbl_branchname').append('<tr style="background-color:' + colorue[k] + '">'+
                    //'<td scope="row" style="font-weight: 600;">' + name + '</td>' +
                    '<td style="font-weight: 600;"><i class="fa fa-user" aria-hidden="true"></i>&nbsp;<span id="0">' + name + '</span></td>' +
                    //'<td data-title="Address">' + phoneno + '</td>' +
                    '<td ><i class="fa  fa-phone" aria-hidden="true"></i>&nbsp;<span id="1">' + phoneno + '</span></td>' +
                    //'<td data-title="Phone Number" >' + emailid + '</td>' +
                    '<td ><i class="fa  fa-envelope-o" aria-hidden="true"></i>&nbsp;<span id="2">' + emailid + '</span></td>' + 
                    '<td data-title="Email" >' + alert_type + '</td>' +
                    '<td data-title="Inward Start No" >' + designation + '</td>' +
                    '<td data-title="sno" style="display:none;">' + sno + '</td>' + 
                    //'<td><input type="button" class="btn btn-primary" name="Update" value ="Modify" onclick="updateclick(this);"/></td></tr>');
                    '<td><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls" name="Update" value ="Modify"   onclick="updateclick(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>');
                    k = k + 1;
                    if (k == 4) {
                        k = 0;
                    }
                }
            }
        }
        function updateclick(thisid) {
            var row = $(thisid).parents('tr');
            var name = $(thisid).parent().parent().find('#0').text();
            var mobileno = $(thisid).parent().parent().find('#1').text();
            var email = $(thisid).parent().parent().find('#2').text();
            var alerttype = row[0].cells[3].innerHTML;
            var designation = row[0].cells[4].innerHTML;
            var sno = row[0].cells[5].innerHTML;
            document.getElementById('txt_name').value = name;
            document.getElementById('txt_designation').value = designation;
            document.getElementById('txt_mobileno').value = mobileno;
            document.getElementById('txt_email').value = email;
            document.getElementById('cmb_alerttype').value = alerttype;
            document.getElementById('txt_sno').value = sno;
            $('#person_fillform').css('display', 'block');
            $('#persons_showlogs').css('display', 'none');
            $('#div_perssons_table').css('display', 'none');
            //$('#save_employee').val("Modify");
            document.getElementById('save_employee').innerHTML = "Modify";
        }
        function clearall() {
            document.getElementById('txt_name').value = "";
            document.getElementById('txt_designation').value = "";
            document.getElementById('txt_mobileno').value = "";
            document.getElementById('txt_email').value = "";
            document.getElementById('cmb_alerttype').selectedIndex = 0;
            document.getElementById('txt_sno').value = "";
            document.getElementById('save_employee').innerHTML = "Save";
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
            Personal Info<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Personal Info</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Personal Info Details
                </h3>
            </div>
            <div class="box-body">
                <div id="persons_showlogs" style="text-align: center;">
                    <%--<input id="add_person" type="button" name="submit" value=' Add Personal information'
                        class="btn btn-primary" />--%>
                        <div class="input-group" style="width: 100px;padding-left: 965px;padding-bottom: 6px;">
                                <div class="input-group-addon">
                                <span  class="glyphicon glyphicon-plus-sign" onclick="addpersonalinformation()"></span><span id="add_person" onclick="addpersonalinformation()">Add Details</span>
                          </div>
                          </div>
                </div>
                <div id='person_fillform' style="display: none;">
                    <div style="padding: 20px; align: center;">
                        <table align="center">
                            <tr>
                                <td style="height:40px;">
                                 <label>  Name </label><span style="color: red;">*</span>
                                </td>
                                <td>
                                    <input id="txt_name" type="text" name="vendorcode" class="form-control" placeholder="Name">
                                </td>
                                <td>
                                    <label id="lbl_person_Name_error_msg" class="errormessage">
                                        * Please Enter Name</label>
                                </td>
                            </tr>
                            <tr>
                                <td style="height:40px;"> 
                                 <label>   Designation </label>
                                </td>
                                <td>
                                    <input type="text" id="txt_designation" class="form-control" name="vendorcode" placeholder="Designation">
                                </td>
                                <td>
                                    <label id="lbl_designation" class="errormessage">
                                        * Please Enter Designation</label>
                                </td>
                            </tr>
                            <tr>
                                <td style="height:40px;">
                                <label>    Phone Number </label> <span style="color: red;">*</span>
                                </td>
                                <td>
                                    <input type="text" id="txt_mobileno" maxlength="45" class="form-control" name="vendorcode"
                                        placeholder="Phone Number">
                                </td>
                                <td>
                                    <label id="lbl_mobileno" class="errormessage">
                                        * Please Phone Number</label>
                                </td>
                            </tr>
                            <tr>
                                <td style="height:40px;">
                                 <label>   Email </label> <span style="color: red;">*</span>
                                </td>
                                <td>
                                    <input type="text" id="txt_email" maxlength="45" class="form-control" name="vendorcode"
                                        placeholder="Email">
                                </td>
                                <td>
                                    <label id="lbl_email" class="errormessage">
                                        * Please Enter Proper Email ID</label>
                                </td>
                            </tr>
                            <tr>
                                <td style="height:40px;">
                                 <label>   Status</label>
                                </td>
                                <td>
                                    <select id="cmb_alerttype" class="form-control">
                                        <option>Enable</option>
                                        <option>Disable</option>
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
                           <%-- <td></td>
                                <td align="center" style="height:40px;">
                                    <input id='save_employee' type="button" name="submit" value='Save' onclick="save_branchname()"
                                        class="btn btn-primary" />
                                    <input id='close_employee' type="button" name="Close" value='Close' class="btn btn-danger" />
                                </td>--%>
                            </tr>
                        </table>
                        <div style="padding-left: 409px;padding-top: 10px;">
                        <table>
                        <tr>
                        <td>
                        </td>
                        <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span class="glyphicon glyphicon-ok" id="save_employee1" onclick="save_branchname()"></span><span id="save_employee" onclick="save_branchname()">Save</span>
                            </div>
                            </div>
                            </td>
                            <td style="width:10px;"></td>
                            <td>
                                <div class="input-group">
                                <div class="input-group-close">
                                <span class="glyphicon glyphicon-remove" id='close_employee1' onclick="closebranches()"></span><span id='close_employee' onclick="closebranches()">Close</span>
                            </div>
                            </div>
                            </td>
                            </tr>
                            </table>
                        </div>
                    </div>
                </div>
            <div>
                <div id="div_perssons_table">
                    <table id="tbl_branchname" class="table table-bordered table-striped" role="grid" aria-describedby="example2_info">
                        <thead>
                            <tr style="background:#5aa4d0; color: white; font-weight: bold;">
                                <th scope="col">
                                    Name
                                </th>
                                <th scope="col">
                                    Phone Number
                                </th>
                                <th scope="col">
                                    Email
                                </th>
                                <th scope="col">
                                    Status
                                </th>
                                <th scope="col">
                                    Designation
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
