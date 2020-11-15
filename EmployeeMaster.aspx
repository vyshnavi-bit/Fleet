<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="EmployeeMaster.aspx.cs" Inherits="EmployeeMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="autocomplete/jquery-ui.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .dispalynone
        {
            display: none;
        }
    </style>
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
        //Function for only no
        $(document).ready(function () {
            $("#txt_experience").keydown(function (event) {
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
        $(function () {
            var Emptype = '<%# Session["Emp_Type"] %>';
            if (Emptype == "MainAdmin") {
                var department = document.getElementById('slct_emptype');
                var opt = document.createElement('option');
                opt.innerHTML = "Admin";
                opt.value = "Admin";
                department.add(opt, 1);
                $("#logins_div").show();
            }
            $('.hiddenrow').hide();
            retrivealldata();
            getdepartment();
//            $('#add_employee').click(function () {
//                $('#employee_fillform').css('display', 'block');
//                $('#employee_showlogs').css('display', 'none');
//                $('#div_employeetable').css('display', 'none');
//                errormags();
//                $('.hiddenrow').hide();
//                forclearall();
//            });
//            $('#close_employee').click(function () {
//                $('#employee_fillform').css('display', 'none');
//                $('#employee_showlogs').css('display', 'block');
//                $('#div_employeetable').css('display', 'block');
//                errormags();
//                $('.hiddenrow').hide();
//                forclearall();
//            });
        });
        function addemployeedetails() {
            $('#employee_fillform').css('display', 'block');
            $('#employee_showlogs').css('display', 'none');
            $('#div_employeetable').css('display', 'none');
            errormags();
            $('.hiddenrow').hide();
            forclearall();
        }
        function closeemployee() {
            $('#employee_fillform').css('display', 'none');
            $('#employee_showlogs').css('display', 'block');
            $('#div_employeetable').css('display', 'block');
            errormags();
            $('.hiddenrow').hide();
            forclearall();
        }
        function getdepartment() {
            var minimaster = "Departments";
            var data = { 'op': 'get_Mini_Master_data', 'minimaster': minimaster };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        filldepartments(msg);
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
        function filldepartments(departmentmsg) {
            var department = document.getElementById('slct_department');
            var length = department.options.length;
            document.getElementById('slct_department').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Department";
            opt.value = "Select Department";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            department.appendChild(opt);
            for (var i = 0; i < departmentmsg.length; i++) {
                if (departmentmsg[i].mm_name != null && departmentmsg[i].mm_status != "0") {
                    var option = document.createElement('option');
                    option.innerHTML = departmentmsg[i].mm_name;
                    option.value = departmentmsg[i].sno;
                    department.appendChild(option);
                }
            }
        }
        function save_employee_click() {
            var dataURL = document.getElementById('main_img').src;
            var imagename = document.getElementById("yourBtn").value;
            var emp_name = document.getElementById('txt_emloyeename').value;
            var emp_id = document.getElementById('txt_empid').value;
            var emp_sno = document.getElementById('txt_sno').value;
            var emp_department = document.getElementById('slct_department').value;
            var emp_dob = document.getElementById('txt_dob').value;
            var emp_licence = document.getElementById('txt_licencenumb').value;
            var emp_type = document.getElementById('slct_emptype').value;
            //var emp_branch = document.getElementById('slct_branchname').value;
            var emp_login = document.getElementById('txt_emplogin').value;
            var emp_pass = document.getElementById('txt_emppass').value;
            var status = document.getElementById('cmb_status').value;
            var licenceexp = document.getElementById('txt_licenceexpire').value;
            var bloodgroup = document.getElementById('txt_bloodgroup').value;
            var address = document.getElementById('txt_address').value;
            var doj = document.getElementById('txt_doj').value;
            var experience = document.getElementById('txt_experience').value;
            var mobileno = document.getElementById('txt_mobno').value;
            var btnval = document.getElementById('save_employee').innerHTML;
            var sno = document.getElementById('txt_sno').value;
            var gender = document.getElementById('slct_gender').value;
            var fathersnme = document.getElementById('txt_fathersnme').value;
            var eduqquali = document.getElementById('txt_eduqualification').value;
            var techquali = document.getElementById('txt_techqualifi').value;
            var banckac = document.getElementById('txt_bankac').value;
            var maritial = document.getElementById('slct_maritial').value;
            var nationality = document.getElementById('txt_nationality').value;
            var flag = false;
            var div_text = $('#yourBtn').text().trim();
            var blob = dataURItoBlob(dataURL);
            if (emp_type == "Select Employee Type") {
                $("#lbl_axil_name_error_msg").show();
                flag = true;
            }
            if (emp_department == "Select Department") {
                $("#lbl_axil_name_error_msg").show();
                flag = true;
            }
            if (doj == "") {
                alert("Please Enter Date of Joining");
                $('#txt_doj').focus();
                flag = true;
            }
            if (emp_name == "") {
                $("#lbl_emp_Name_error_msg").show();
                $('#txt_emloyeename').focus();
                flag = true;
            }
            if (flag) {
                return;
            }
            var data = { 'op': 'for_save_edit_employee', 'emp_name': emp_name, 'emp_id': emp_id, 'emp_department': emp_department, 'emp_dob': emp_dob,
                'emp_licence': emp_licence, 'emp_type': emp_type, 'emp_login': emp_login, 'emp_pass': emp_pass, 'status': status, 'btnval': btnval, 'sno': sno, 'licenceexp': licenceexp,
                'bloodgroup': bloodgroup, 'address': address, 'doj': doj, 'experience': experience, 'mobileno': mobileno, 'gender': gender, 'fathersnme': fathersnme, 'eduqquali': eduqquali, 'techquali': techquali, 'banckac': banckac, 'maritial': maritial, 'nationality': nationality
            };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        if (msg == "OK") {
                            alert("New Employee Successfully Added");
                            retrivealldata();
                            forclearall();
                            errormags();
                            $('#employee_fillform').css('display', 'none');
                            $('#employee_showlogs').css('display', 'block');
                            $('#div_employeetable').css('display', 'block');
                            //$('#save_employee').val("Save");
                            document.getElementById('txt_address').innerHTML = "Save";
                        }
                        else if (msg == "UPDATE") {
                            alert("Employee Successfully Modified");
                            retrivealldata();
                            forclearall();
                            errormags();
                            $('#employee_fillform').css('display', 'none');
                            $('#employee_showlogs').css('display', 'block');
                            $('#div_employeetable').css('display', 'block');
                            //$('#save_employee').val("Save");
                            document.getElementById('txt_address').innerHTML = "Save";
                        }
                        else if (msg == "EXISTED") {
                            alert("Employee Already Existed");
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
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }

        function errormags() {
            $("#lbl_axil_name_error_msg").hide();
            $("#lbl_axil_name_error_msg").hide();
            $("#lbl_emp_Name_error_msg").hide();
            $("#lbl_emplogin_error_msg").hide();
            $("#lbl_emppass_error_msg").hide();
        }

        var empdetails = [];
        function retrivealldata() {
            var table = document.getElementById("tbl_employee");
            for (var i = table.rows.length - 1; i > 0; i--) {
                table.deleteRow(i);
            }
            var data = { 'op': 'get_all_employee_data' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        filldata(msg);
                        fillempname(msg);
                        empdetails = msg;
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
        var empnameList = [];
        function fillempname(msg) {
            for (var i = 0; i < msg.length; i++) {
                var empname = msg[i].employname;
                empnameList.push(empname);
            }
            $('#txt_empname1').autocomplete({
                source: empnameList,
                change: empnamechange,
                autoFocus: true
            });
        }
        function empnamechange() {
            var results = empdetails;
            var empname = document.getElementById("txt_empname1").value;
            if (empname == "" || empname == null || empname == undefined) {
                retrivealldata();
            }
            var table = document.getElementById("tbl_employee");
            for (var i = table.rows.length - 1; i > 0; i--) {
                table.deleteRow(i);
            }
            var k = 0;
            var colorue = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            for (var i = 0; i < results.length; i++) {
                if (empname == results[i].employname) {
                    if (results[i].employname != null) {
                        var employname = results[i].employname;
                        var employid = results[i].employid;
                        //var sno = results[i].sno;
                        var dept_id = results[i].dept_id;
                        var emp_type = results[i].emp_type;
                        var emp_login_id = results[i].emp_login_id;
                        var emp_pwd = results[i].emp_pwd;
                        var emp_dob = results[i].emp_dob;
                        var emp_licencenum = results[i].emp_licencenum;
                        var emp_licenceexpire = results[i].emp_licenceexpire;
                        var emp_bloodgrp = results[i].emp_bloodgrp;
                        var emp_address = results[i].emp_address;
                        var emp_doj = results[i].emp_doj;
                        var emp_experience = results[i].emp_experience;
                        var Phoneno = results[i].Phoneno;
                        var imagename = results[i].imagename;
                        var ftplocation = results[i].ftplocation;
                        var gender = results[i].gender;
                        var fathernme = results[i].fathernme;
                        var eduqual = results[i].eduqual;
                        var techqual = results[i].techqual;
                        var bankac = results[i].bankac;
                        var marital = results[i].marital;
                        var nationality = results[i].nationality;
                        var branch_id = results[i].branch_id;
                        var statuscode = results[i].emp_status;
                        var imagepath = results[i].imagepath;
                        var status = "";
                        if (statuscode == "1") {
                            status = "Enabled";
                        }
                        else {
                            status = "Disabled";
                        }
                        var emp_sno = results[i].emp_sno;
                        var tablerowcnt = document.getElementById("tbl_employee").rows.length;
                        $('#tbl_employee').append('<tr  style="background-color:' + colorue[k] + '">' +
                    '<td style="font-weight: 600;"><i class="fa fa-user" aria-hidden="true"></i>&nbsp;<span id="1">' + employname + '</span></td>' +
                    '<td scope="row" style="display:none;">' + employname + '</td>' +
                    '<td data-title="ID">' + employid + '</td>' +
                    '<td data-title="Department" style="display:none;">' + dept_id + '</td>' +
                    '<td data-title="Employee Type" >' + emp_type + '</td>' +
                    '<td ><i class="" aria-hidden="true"></i>&nbsp;<span id="1">' + emp_login_id + '</span></td>' +
                    '<td data-title="Emp Login ID"  style="display:none;">' + emp_login_id + '</td>' +
                    '<td data-title="Emp Password" style="display:none;">' + emp_pwd + '</td>' +
                    '<td ><i class="fa fa-calendar-times-o" aria-hidden="true"></i>&nbsp;<span id="1">' + emp_dob + '</span></td>' +
                    '<td data-title="Date OF Birth" style="display:none;" >' + emp_dob + '</td>' +
                    '<td data-title="Licence Number" >' + emp_licencenum + '</td>' +
                        //'<td data-title="Licence Expire" class="11" >' + emp_licenceexpire + '</td>' +
                    '<td ><i class="fa fa-calendar-times-o" aria-hidden="true"></i>&nbsp;<span id="11">' + emp_licenceexpire + '</span></td>' +
                    '<td data-title="Status" >' + status + '</td>' +
                    '<td data-title="Branch Name" style="display:none;">' + branch_id + '</td>' +
                    '<td data-title="emp_sno" style="display:none;">' + emp_sno + '</td>' +
                    '<td data-title="emp_bloodgrp" style="display:none;">' + emp_bloodgrp + '</td>' +
                    '<td data-title="emp_address" style="display:none;">' + emp_address + '</td>' +
                    '<td data-title="emp_doj" style="display:none;">' + emp_doj + '</td>' +
                    '<td data-title="emp_experience" style="display:none;">' + emp_experience + '</td>' +
                    '<td data-title="Phoneno" style="display:none;">' + Phoneno + '</td>' +
                    '<td data-title="gender" style="display:none;">' + gender + '</td>' +
                    '<td data-title="fathernme" style="display:none;">' + fathernme + '</td>' +
                    '<td data-title="eduqual" style="display:none;">' + eduqual + '</td>' +
                    '<td data-title="techqual" style="display:none;">' + techqual + '</td>' +
                    '<td data-title="bankac" style="display:none;">' + bankac + '</td>' +
                    '<td data-title="marital" style="display:none;">' + marital + '</td>' +
                    '<td data-title="nationality" style="display:none;">' + nationality + '</td>' +
                    '<td style="display:none" class="9">' + ftplocation + '</td>' +
                    '<td style="display:none" class="8">' + imagename + '</td>' +
                    '<td style="display:none" >' + imagepath + '</td>' +
                        //'<td><input type="button" class="btn btn-primary" name="Update" value ="Modify" onclick="updateclick(this);"/></td></tr>');
                    '<td><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls" name="Update" value ="Modify" onclick="updateclick(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>');
                        k = k + 1;
                        if (k == 4) {
                            k = 0;
                        }
                    }
                }
            }
        }
        function filldata(results) {
            var table = document.getElementById("tbl_employee");
            for (var i = table.rows.length - 1; i > 0; i--) {
                table.deleteRow(i);
            }
            var k = 0;
            var colorue = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            for (var i = 0; i < results.length; i++) {
                if (results[i].employname != null) {
                    var employname = results[i].employname;
                    var employid = results[i].employid;
                       //var sno = results[i].sno;
                    var dept_id = results[i].dept_id;
                    var emp_type = results[i].emp_type;
                    var emp_login_id = results[i].emp_login_id;
                    var emp_pwd = results[i].emp_pwd;
                    var emp_dob = results[i].emp_dob;
                    var emp_licencenum = results[i].emp_licencenum;
                    var emp_licenceexpire = results[i].emp_licenceexpire;
                    var emp_bloodgrp = results[i].emp_bloodgrp;
                    var emp_address = results[i].emp_address;
                    var emp_doj = results[i].emp_doj;
                    var emp_experience = results[i].emp_experience;
                    var Phoneno = results[i].Phoneno;
                    var imagename = results[i].imagename;
                    var ftplocation = results[i].ftplocation;
                    var gender = results[i].gender;
                    var fathernme = results[i].fathernme;
                    var eduqual = results[i].eduqual;
                    var techqual = results[i].techqual;
                    var bankac = results[i].bankac;
                    var marital = results[i].marital;
                    var nationality = results[i].nationality;
                    var branch_id = results[i].branch_id;
                    var statuscode = results[i].emp_status;
                    var imagepath = results[i].imagepath;
                    var status = "";
                    if (statuscode == "1") {
                        status = "Enabled";
                    }
                    else {
                        status = "Disabled";
                    }
                    var emp_sno = results[i].emp_sno;
                    var tablerowcnt = document.getElementById("tbl_employee").rows.length;
                    $('#tbl_employee').append('<tr  style="background-color:' + colorue[k] + '">' + 
                    '<td style="font-weight: 600;"><i class="fa fa-user" aria-hidden="true"></i>&nbsp;<span id="1">' + employname + '</span></td>' +   
                    '<td scope="row" style="display:none;">' + employname + '</td>' +   
                    '<td data-title="ID">' + employid + '</td>' +   
                    '<td data-title="Department" style="display:none;">' + dept_id + '</td>' +  
                    '<td data-title="Employee Type" >' + emp_type + '</td>' +  
                    '<td ><i class="" aria-hidden="true"></i>&nbsp;<span id="2">' + emp_login_id + '</span></td>' +  
                    '<td data-title="Emp Login ID"  style="display:none;">' + emp_login_id + '</td>' +   
                    '<td data-title="Emp Password" style="display:none;">' + emp_pwd + '</td>' +  
                    '<td ><i class="fa fa-calendar-times-o" aria-hidden="true"></i>&nbsp;<span id="3">' + emp_dob + '</span></td>' +   
                    '<td data-title="Date OF Birth" style="display:none;" >' + emp_dob + '</td>' +   
                    '<td data-title="Licence Number" >' + emp_licencenum + '</td>' +
                    //'<td data-title="Licence Expire" class="11">' + emp_licenceexpire + '</td>' +   
                    '<td ><i class="fa fa-calendar-times-o" aria-hidden="true"></i>&nbsp;<span id="11">' + emp_licenceexpire + '</span></td>' +
                    '<td data-title="Status" >' + status + '</td>' +   
                    '<td data-title="Branch Name" style="display:none;">' + branch_id + '</td>' +   
                    '<td data-title="emp_sno" style="display:none;">' + emp_sno + '</td>' +   
                    '<td data-title="emp_bloodgrp" style="display:none;">' + emp_bloodgrp + '</td>' +   
                    '<td data-title="emp_address" style="display:none;">' + emp_address + '</td>' +   
                    '<td data-title="emp_doj" style="display:none;">' + emp_doj + '</td>' + 
                    '<td data-title="emp_experience" style="display:none;">' + emp_experience + '</td>' +   
                    '<td data-title="Phoneno" style="display:none;">' + Phoneno + '</td>' +   
                    '<td data-title="gender" style="display:none;">' + gender + '</td>' +   
                    '<td data-title="fathernme" style="display:none;">' + fathernme + '</td>' +  
                    '<td data-title="eduqual" style="display:none;">' + eduqual + '</td>' +   
                    '<td data-title="techqual" style="display:none;">' + techqual + '</td>' +   
                    '<td data-title="bankac" style="display:none;">' + bankac + '</td>' +  
                    '<td data-title="marital" style="display:none;">' + marital + '</td>' + 
                    '<td data-title="nationality" style="display:none;">' + nationality + '</td>' +  
                    '<td style="display:none" class="9">' +ftplocation + '</td>'+  
                    '<td style="display:none" class="8">' + imagename + '</td>' + 
                    '<td style="display:none" >' + imagepath + '</td>' +  
                    //'<td><input type="button" class="btn btn-primary" name="Update" value ="Modify" onclick="updateclick(this);"/></td></tr>');
                    '<td><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls" name="Update" value ="Modify"  onclick="updateclick(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>');
                    k = k + 1;
                    if (k == 4) {
                        k = 0;
                    }
                }
            }
        }
        var emp_sno;
        var employid;
        function updateclick(thisid) {
            var row = $(thisid).parents('tr');
//                       var Empsno = selectedrow[0].cells[1].innerHTML;
            var employname = row[0].cells[1].innerHTML;
            var employid = row[0].cells[2].innerHTML;
            var dept_id = row[0].cells[3].innerHTML;
            var emp_type = row[0].cells[4].innerHTML;
            var emp_login_id = row[0].cells[6].innerHTML;
            var emp_pwd = row[0].cells[7].innerHTML;
            var emp_dob = row[0].cells[9].innerHTML;
            var emp_licencenum = row[0].cells[10].innerHTML;
            var emp_licenceexp = $(thisid).parent().parent().find('#11').html();
            var statuscode = row[0].cells[12].innerHTML;
            var branch_id = row[0].cells[13].innerHTML;
            var emp_sno = row[0].cells[14].innerHTML;
            var emp_bloodgrp = row[0].cells[15].innerHTML;
            var emp_address = row[0].cells[16].innerHTML;
            var emp_doj = row[0].cells[17].innerHTML;
            var emp_experience = row[0].cells[18].innerHTML;
            var Phoneno = row[0].cells[19].innerHTML;
            var imagename = row[0].cells[28].innerHTML;
            var imagepath = row[0].cells[29].innerHTML;
            var ftplocation = row[0].cells[27].innerHTML;
            var gender = row[0].cells[20].innerHTML;
            var fathernme = row[0].cells[21].innerHTML;
            var eduqual = row[0].cells[22].innerHTML;
            var techqual = row[0].cells[23].innerHTML;
            var bankac = row[0].cells[24].innerHTML;
            var marital = row[0].cells[25].innerHTML;
            var nationality = row[0].cells[26].innerHTML;
           
//            emp_sno = results[i].emp_sno;
//            employid = results[i].employid;
            var status = "";
            if (statuscode == "Enabled") {
                status = "1";
            }
            else {
                status = "0";
            }
//            var emp_sno = results[i].emp_sno;
            document.getElementById('lbl_topempname').innerHTML = employname;
            document.getElementById('lbl_topemployeeid').innerHTML = employid;
            document.getElementById('lbl_topempemailid').innerHTML = emp_login_id;
             document.getElementById('lbl_topempmobno').innerHTML = Phoneno;
            $("select#slct_department option").each(function () { this.selected = (this.text == dept_id); });
            document.getElementById('txt_emloyeename').value = employname;
            document.getElementById('txt_empid').value = employid;
            document.getElementById('txt_dob').value = emp_dob;
            document.getElementById('txt_licencenumb').value = emp_licencenum;
            $("select#slct_emptype option").each(function () { this.selected = (this.text == emp_type); });
            document.getElementById('txt_emplogin').value = emp_login_id;
            document.getElementById('txt_emppass').value = emp_pwd;
            document.getElementById('cmb_status').value = status;
            document.getElementById('txt_licenceexpire').value = emp_licenceexp;
            document.getElementById('txt_sno').value = emp_sno;
            document.getElementById('txt_bloodgroup').value = emp_bloodgrp;
            document.getElementById('txt_address').value = emp_address;
            document.getElementById('txt_doj').value = emp_doj;
            document.getElementById('txt_experience').value = emp_experience;
            document.getElementById('txt_mobno').value = Phoneno;
            document.getElementById('slct_gender').value = gender;
            document.getElementById('txt_fathersnme').value = fathernme;
            document.getElementById('txt_eduqualification').value = eduqual;
            document.getElementById('txt_techqualifi').value = techqual;
            document.getElementById('txt_bankac').value = bankac;
            document.getElementById('slct_maritial').value = marital;
            document.getElementById('txt_nationality').value = nationality;
            $('.hiddenrow').show();
            $('#employee_fillform').css('display', 'block');
            $('#employee_showlogs').css('display', 'none');
            $('#div_employeetable').css('display', 'none');
            //$('#save_employee').val("Modify");
            document.getElementById('save_employee').innerHTML = "Modify";
            errormags();
            var rndmnum = Math.floor((Math.random() * 10) + 1);
            img_url = ftplocation + imagepath + '?v=' + rndmnum;
            if (imagepath != "") {
                $('#main_img').attr('src', img_url).width(200).height(200);
            }
            else {
                $('#main_img').attr('src', 'Images/Employeeimg.jpg').width(200).height(200);
            }
            document.getElementById('btn_upload_profilepic').disabled = false;
        }
        function forclearall() {
            document.getElementById('txt_emloyeename').value = "";
            document.getElementById('txt_empid').value = "";
            document.getElementById('slct_department').value = "Select Department";
            document.getElementById('txt_dob').value = "";
            document.getElementById('txt_licencenumb').value = "";
            document.getElementById('txt_licenceexpire').value = "";
            document.getElementById('slct_emptype').value = "Select Employee Type";
            document.getElementById('txt_emplogin').value = "";
            document.getElementById('txt_emppass').value = "";
            document.getElementById('cmb_status').value = "1";
            document.getElementById('txt_sno').value = "";
            document.getElementById('txt_bloodgroup').value = "";
            document.getElementById('txt_address').value = "";
            document.getElementById('txt_doj').value = "";
            document.getElementById('txt_experience').value = "";
            document.getElementById('txt_mobno').value = "";
            document.getElementById('slct_gender').value = "Male";
            document.getElementById('txt_fathersnme').value = "";
            document.getElementById('txt_eduqualification').value = "";
            document.getElementById('txt_techqualifi').value = "";
            document.getElementById('txt_bankac').value = "";
            document.getElementById('slct_maritial').value = "Married";
            document.getElementById('txt_nationality').value = "";
            //$('#save_employee').val("Save");
            document.getElementById('save_employee').innerHTML = "Save";
                  
        }
        function change_Documents() {
            $("li").removeClass("active");
            $("li").addClass("");
            $("#id_tab_documents").removeClass("");
            $("#id_tab_documents").addClass("active");
            $("#div_basic_details").css("display", "none");
            $("#btn_modify").css("display", "none");
            $("#div_Documents").css("display", "block");
            getemployee_Uploaded_Documents(emp_sno);
        }
        function change_Personal() {
            $("li").removeClass("active");
            $("li").addClass("");
            $("#id_tab_documents").removeClass("");
            $("#id_tab_documents").addClass("active");
            $("#div_basic_details").css("display", "block");
            $("#btn_modify").css("display", "block");
            $("#div_Documents").css("display", "none");
        }

        function callHandler_nojson_post(d, s, e) {
            $.ajax({
                url: 'FleetManagementHandler.axd',
                type: "POST",
                // dataType: "json",
                contentType: false,
                processData: false,
                data: d,
                success: s,
                error: e
            });
        }
        function readURL(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();

                reader.onload = function (e) {
                    $('#main_img,#img_1').attr('src', e.target.result).width(200).height(200);
                    //                    $('#img_1').css('display', 'inline');
                };
                reader.readAsDataURL(input.files[0]);
            }
        }
        //image uploding.......
        function getFile() {
            document.getElementById("file").click();
        }
        //----------------> convert base 64 to file
        function dataURItoBlob(dataURI) {
            // convert base64/URLEncoded data component to raw binary data held in a string
            var byteString;
            if (dataURI.split(',')[0].indexOf('base64') >= 0)
                byteString = atob(dataURI.split(',')[1]);
            else
                byteString = unescape(dataURI.split(',')[1]);
            // separate out the mime component
            var mimeString = dataURI.split(',')[0].split(':')[1].split(';')[0];
            // write the bytes of the string to a typed array
            var ia = new Uint8Array(byteString.length);
            for (var i = 0; i < byteString.length; i++) {
                ia[i] = byteString.charCodeAt(i);
            }
            return new Blob([ia], { type: 'image/jpeg' });
        }
        function upload_profile_pic() {
            var dataURL = document.getElementById('main_img').src;
            var div_text = $('#yourBtn').text().trim();
            var blob = dataURItoBlob(dataURL);
            var emp_sno = document.getElementById("txt_sno").value;
            var employid = document.getElementById('txt_empid').value 
            var Data = new FormData();
            Data.append("op", "emp_profile_pic_files_upload");
            Data.append("emp_sno", emp_sno);
            Data.append("employid", employid);
            Data.append("canvasImage", blob);
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    document.getElementById('btn_upload_profilepic').disabled = true;
                }
                else {
                    document.location = "Default.aspx";
                }
            };
            var e = function (x, h, e) {
            };
            callHandler_nojson_post(Data, s, e);
        }

        //document uploding......
        function getFile_doc() {
            document.getElementById("FileUpload1").click();
        }
        function upload_Employee_Document_Info(thisid) {
            var documentid = document.getElementById('ddl_documenttype').value;
            var documentname = document.getElementById('ddl_documenttype').selectedOptions[0].innerText;
            if (documentid == null || documentid == "" || documentid == "Select Document Type") {
                document.getElementById("ddl_documenttype").focus();
                alert("Please select Document Type");
                return false;
            }
//            var row = $(thisid).parents('tr');
            var emp_sno = document.getElementById("txt_sno").value;
           var employid =  document.getElementById('txt_empid').value 
            var documentExists = 0;
            $('#tbl_documents tr').each(function () {
                var selectedrow = $(this);
                var document_manager_id = selectedrow[0].cells[0].innerHTML;
                if (document_manager_id == documentid) {
                    alert(documentname + "  Already Exist For This Employee");
                    documentExists = 1;
                    return false;
                }

            });
            if (documentExists == 1) {
                return false;
            }
            var Data = new FormData();
            Data.append("op", "save_employeeDocument");
            Data.append("emp_sno", emp_sno);
            Data.append("employid", employid);
            Data.append("documentname", documentname);
            Data.append("documentid", documentid);
            var fileUpload = $("#FileUpload1").get(0);
            var files = fileUpload.files;
            for (var i = 0; i < files.length; i++) {
                Data.append(files[i].name, files[i]);
            }
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    getemployee_Uploaded_Documents(emp_sno);
                }
            };
            var e = function (x, h, e) {
                alert(e.toString());
            };
            callHandler_nojson_post(Data, s, e);
        }
        function readURL_doc(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.readAsDataURL(input.files[0]);
                document.getElementById("FileUpload_div").innerHTML = input.files[0].name;
            }
        }
        function getemployee_Uploaded_Documents(emp_sno) {
            var emp_sno = document.getElementById("txt_sno").value;
            var data = { 'op': 'getemployee_Uploaded_Documents', 'emp_sno': emp_sno };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillemployee_Uploaded_Documents(msg);
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
        function fillemployee_Uploaded_Documents(msg) {
            var results = '<div class="divcontainer" style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer">';
            results += '<thead><tr><th scope="col">Sno</th><th scope="col" style="text-align:center">Document Name</th><th scope="col">Photo</th><th scope="col">Download</th></tr></thead></tbody>';
            var k = 1;
            for (var i = 0; i < msg.length; i++) {
                results += '<tr><td>' + k++ + '</td>';
                var path = img_url = msg[i].ftplocation + msg[i].photo;
                var documentname = "";
                if (msg[i].documentid == "1") {
                    documentname = " DrivingLicence";
                }
                if (msg[i].documentid == "2") {
                    documentname = " Adarcard";
                }
                if (msg[i].documentid == "3") {
                    documentname = " voterid";
                }
                results += '<th scope="row" class="1" style="text-align:center;">' + documentname + '</th>';
                //results += '<td data-title="Code" class="2">' + msg[i].Status + '</td>';
                results += '<td data-title="brandstatus" class="2"><img src=' + path + '  style="cursor:pointer;height:200px;width:200px;border-radius: 5px;"/></td>';
                results += '<th scope="row" class="1" ><a  target="_blank" href=' + path + '><i class="fa fa-download" aria-hidden="true"></i> Download</a></th>';
                results += '</tr>';
            }
            results += '</table></div>';
            $("#div_documents_table").html(results);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Employee Master<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Employee Master</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Employee Master Details
                </h3>
            </div>
            <div class="box-body">
                <div id="employee_showlogs" style="text-align: center;">
                    <table>
                        <tr>
                            <td>
                                <input id="txt_empname1" type="text" style="height: 28px; opacity: 1.0; width: 180px;"
                                    class="form-control" placeholder="Search Employee Name" />
                            </td>
                            <td style="width: 5px;">
                            </td>
                            <td>
                                <td>
                               <div class="input-group">
                                <div class="input-group-addon">
                                <span  class="fa fa-search"  onclick="empnamechange();"></span>
                             </div>
                             </div>
                            </td>
                            </td>
                            <td style="width: 500px">
                            </td>
                            <td>
                                <%--<input id="add_employee" type="button" name="submit" value='Add Employee' class="btn btn-primary" />--%>
                            <div class="input-group" style="padding-left: 692px;">
                                <div class="input-group-addon">
                                <span  class="glyphicon glyphicon-plus-sign" onclick="addemployeedetails()"></span><span onclick="addemployeedetails()">Add Employee</span>
                          </div>
                          </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id='employee_fillform' style="display: none;">
                 
                <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="well panel panel-default" style="padding: 0px;">
                                        <div class="panel-body">
                                            <div class="row">
                                                <div class="col-sm-4" style="width: 100%;">
                                                    <div class="row">
                                                        <div class="col-xs-12 col-sm-3 text-center">
                                                            <div class="pictureArea1">
                                                                <img class="center-block img-circle img-thumbnail img-responsive profile-img" id="main_img"
                                                                    src="Images/Employeeimg.jpg" alt="your image" style="border-radius: 5px; width: 200px;
                                                                    height: 200px; border-radius: 50%;" />
                                                                <%--<img id="prw_img" class="center-block img-circle img-thumbnail img-responsive profile-img" src="Images/Employeeimg.jpg" alt="your image" style="width: 150px; height: 150px;">--%>
                                                                <div class="photo-edit-admin">
                                                                    <a onclick="getFile();" class="photo-edit-icon-admin" href="/employee/emp-master/emp-photo?eid=3"
                                                                        title="Change Profile Picture" data-toggle="modal" data-target="#photoup"><i class="fa fa-pencil">
                                                                        </i></a>
                                                                </div>
                                                                <div id="yourBtn" class="img_btn" onclick="getFile();" style="margin-top: 5px; display: none;">
                                                                    Click to Choose Image
                                                                </div>
                                                                <div style="height: 0px; width: 0px; overflow: hidden;">
                                                                    <input id="file" type="file" name="files[]" onchange="readURL(this);">
                                                                </div>
                                                                <div  style="width: 0px;padding-left: 40px;">
                                                                    <%--<input type="button" id="btn_upload_profilepic" class="btn btn-primary" onclick="upload_profile_pic();"
                                                                        style="margin-top: 5px;" value="Upload Profile Pic">--%>
                                                                         <div class="input-group">
                                                                    <div class="input-group-addon">
                                                                    <span class="glyphicon glyphicon-upload" id="btn_upload_profilepic1" onclick="upload_profile_pic()"></span> <span id="btn_upload_profilepic" onclick="upload_profile_pic()">Upload Employee Pic</span>
                                                                </div>
                                                               </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-xs-12 col-sm-9">
                                                            <h2 class="text-primary">
                                                                <b><span class="glyphicon glyphicon-user"></span>
                                                                    <label id="lbl_topempname">
                                                                    </label>
                                                                </b>
                                                            </h2>
                                                            <p>
                                                                <strong>Employee ID : <span style="color: Red;">*</span></strong>
                                                                <label style="padding-left: 20px; font-weight: 700;" id="lbl_topemployeeid">
                                                                </label>
                                                            </p>
                                                            <p>
                                                                <strong>Email ID : <span style="color: Red;">*</span></strong>
                                                                <label id="lbl_topempemailid">
                                                                </label>
                                                            </p>
                                                            <p>
                                                                <strong>Mobile No :<span style="color: Red;">*</span> </strong>
                                                                <label id="lbl_topempmobno">
                                                                </label>
                                                            </p>
                                                        </div>
                                                        <!--/col-->
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                   <ul class="nav nav-tabs">
                                    <li id="id_tab_Personal" class="active"><a data-toggle="tab" href="#" onclick="change_Personal()">
                                        <i class="fa fa-street-view"></i>&nbsp;&nbsp;Basic Details</a></li>
                                    <li id="id_tab_documents" class=""><a data-toggle="tab" href="#" onclick="change_Documents()">
                                        <i class="fa fa-file-text"></i>&nbsp;&nbsp;Documents</a></li>
                                </ul>
                    <div id="div_basic_details">
                                <table align="center">
                        <tr>
                            <td>
                                <label>
                                    Employee Name<span style="color: red;">*</span></label>
                                <input id="txt_emloyeename" type="text" class="form-control" name="vendorcode" placeholder="Employee Name"><label
                                    id="lbl_emp_Name_error_msg" class="errormessage">* Please Enter Employee Name</label>
                            </td>
                            <td style="width: 5px;">
                            </td>
                            <td>
                                <label>
                                    ID<span style="color: red;">*</span></label>
                                <input type="text" id="txt_empid" maxlength="45" class="form-control" name="vendorcode"
                                    placeholder="ID">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Mobile Number<span style="color: red;">*</span></label>
                                <input type="text" id="txt_mobno" maxlength="45" class="form-control" name="vendorcode"
                                    placeholder="Mobile No">
                            </td>
                            <td style="width: 5px;">
                            </td>
                            <td>
                                <label>
                                    Department<span style="color: red;">*</span>
                                </label>
                                <select id="slct_department" class="form-control" style="min-width: 195px;">
                                </select>
                                <label id="lbl_depart_error_msg" class="errormessage">
                                    * Please Select Department Name</label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Employee Type<span style="color: red;">*</span></label>
                                <select id="slct_emptype" class="form-control" style="min-width: 195px;">
                                    <option value="Select Employee Type" selected disabled>Select Employee Type</option>
                                    <option value="Driver">Driver</option>
                                    <option value="Helper">Helper</option>
                                    <option value="Operations">Operations</option>
                                </select><label id="lbl_emptype_error_msg" class="errormessage">* Please Select Employee
                                    Type</label>
                            </td>
                            <td style="width: 5px;">
                            </td>
                            <td>
                                <label>
                                    DOJ</label>
                                <input type="date" style="min-width: 195px;" id="txt_doj" class="form-control" style="min-width: 195px;" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Gender</label>
                                <select id="slct_gender" class="form-control" style="min-width: 195px;">
                                    <option value="Male">Male</option>
                                    <option value="Female">Female</option>
                                    <option value="Other">Other</option>
                                </select>
                            </td>
                            <td style="width: 5px;">
                            </td>
                            <td>
                                <label>
                                    Experience(In Years)</label>
                                <input type="text" id="txt_experience" placeholder="Experience" class="form-control" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Date OF Birth</label>
                                <input type="date" id="txt_dob" style="min-width: 195px;" maxlength="45" name="vendorcode"
                                    placeholder="DOB" class="form-control" />
                            </td>
                            <td style="width: 5px;">
                            </td>
                            <td>
                                <label>
                                    Licence Number</label>
                                <input type="text" id="txt_licencenumb" maxlength="45" name="vendorcode" placeholder="Licence Number"
                                    class="form-control" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Licence Expire</label>
                                <input type="date" id="txt_licenceexpire" style="min-width: 195px;" maxlength="45"
                                    name="vendorcode" placeholder="Licence Expire Date" class="form-control" />
                            </td>
                            <td style="width: 5px;">
                            </td>
                            <td>
                                <label>
                                    Blood Group</label>
                                <input type="text" id="txt_bloodgroup" placeholder=" Blood Group" class="form-control" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Fathers Name</label>
                                <input type="text" id="txt_fathersnme" maxlength="45" name="vendorcode" placeholder="Father Name"
                                    class="form-control" />
                            </td>
                            <td style="width: 5px;">
                            </td>
                            <td>
                                <label>
                                    Educational Qualification</label>
                                <input type="text" id="txt_eduqualification" maxlength="45" name="vendorcode" placeholder="Educartional Qualification"
                                    class="form-control" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Technical/Other Qualification</label>
                                <input type="text" id="txt_techqualifi" placeholder="Other Qualification" class="form-control" />
                            </td>
                            <td style="width: 5px;">
                            </td>
                            <td>
                            <label>
                                    Status</label>
                                <select id="cmb_status" class="allinputs" class="form-control" style="min-width: 195px;">
                                    <option value="1">Enabled</option>
                                    <option value="0">Disabled</option>
                                </select>
                               
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Maritial Status</label>
                                <select id="slct_maritial" class="form-control" style="min-width: 195px;">
                                    <option value="Married">Married</option>
                                    <option value="Single">Single</option>
                                </select>
                            </td>
                            <td style="width: 5px;">
                            </td>
                            <td>
                                <label>
                                    Nationality</label>
                                <input type="text" id="txt_nationality" maxlength="45" name="vendorcode" placeholder="Nationality"
                                    class="form-control" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Bank A/C Number</label>
                                <input type="text" id="txt_bankac" placeholder="Bank A/C Number" class="form-control" />
                            </td>
                            <td style="width: 5px;">
                            </td>
                            <td>
                                <label>
                                    Adhar No</label>
                                <input type="text" id="txt_adharno" placeholder="Adhar No" class="form-control" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Emp Login ID</label>
                                <input type="text" id="txt_emplogin" maxlength="45" name="vendorcode" placeholder="Give Employee A Login ID"
                                    class="form-control" /><label id="lbl_emplogin_error_msg" class="errormessage">* Please
                                        Enter Employee Logn ID</label>
                            </td>
                            <td style="width: 5px;">
                            </td>
                            <td>
                                <label>
                                    Emp Password</label>
                                <input type="text" id="txt_emppass" maxlength="45" name="vendorcode" placeholder="Give Employee A Password"
                                    class="form-control" /><label id="lbl_emppass_error_msg" class="errormessage">* Please
                                        Eneter Employee Password</label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                 <label>
                                    Address</label>
                                <textarea type="text" id="txt_address" placeholder="Address" class="form-control"
                                    rows="2" cols="40"></textarea>
                                <input type="text" id="txt_sno" maxlength="45" name="vendorcode" style="display: none;"
                                    placeholder="sno" class="form-control" />
                            </td>
                            <td style="width: 5px;">
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td style="height:10px;">
                            </td >
                        </tr>
                        <tr>
                           <%-- <td>
                                <input id='save_employee' type="button" class="btn btn-primary" name="submit" value='Save'
                                    onclick="save_employee_click()" />
                            </td>
                            <td style="width: 5px;">
                            </td>
                            <td>
                                <input id='close_employee' type="button" class="btn btn-danger" name="Close" value='Close' />
                            </td>
                            <td>
                            </td>--%>
                            <td>
                            <div style="padding-left: 100px;">
                                <table>
                                   <tr>
                                    <td>
                                    <div class="input-group">
                                        <div class="input-group-addon">
                                        <span class="glyphicon glyphicon-ok" id="save_employee1" onclick="save_employee_click()"></span> <span id="save_employee" onclick="save_employee_click()">Save</span>
                                  </div>
                                  </div>
                                    </td>
                                    <td style="width:10px;"></td>
                                    <td>
                                     
                                  </div>
                                    </td>
                                    </tr>
                               </table>
                            </div>
                            </td>
                        </tr>
                    </table>
                    </div>
                    <div id="div_Documents" class="box box-danger" style="display: none;">
                                <div class="box-header with-border">
                                    <h3 class="box-title">
                                        <i style="padding-right: 5px;" class="fa fa-cog"></i>Documents Upload</h3>
                                </div>
                                <div class="box-body">
                                    <div class="row">
                                        <div>
                                            <br>
                                            <div class="box-body">
                                                <div class="row">
                                                    <div class="col-sm-4">
                                                        <label class="control-label">
                                                            Document Type</label>
                                                        <select id="ddl_documenttype" class="form-control">
                                                            <option>Select Document Type</option>
                                                            <option value="1">DrivingLicence</option>
                                                                <option value="2">Adarcard</option>
                                                            <option value="3">voterid</option>
                                                        </select>
                                                    </div>
                                                    <div class="col-sm-4">
                                                        <table class="table table-bordered table-striped">
                                                            <tbody>
                                                                <tr>
                                                                    <td>
                                                                        <div id="FileUpload_div" class="img_btn" onclick="getFile_doc()" style="height: 50px;
                                                                            width: 100%">
                                                                            Choose Document To Upload
                                                                        </div>
                                                                        <div style="height: 0px; width: 0px; overflow: hidden;">
                                                                            <input id="FileUpload1" type="file" name="files[]" onchange="readURL_doc(this);">
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </div>
                                                    <div style="width: 0px;">
                                                        <%--<input id="btn_upload_document" type="button" class="btn btn-primary" name="submit"
                                                            value="UPLOAD" onclick="upload_Employee_Document_Info();" style="width: 120px;
                                                            margin-top: 25px;">--%>
                                                            <div class="input-group">
                                                            <div class="input-group-addon">
                                                            <span class="glyphicon glyphicon-upload" id="btn_upload_document1" onclick="upload_Employee_Document_Info()"></span> <span id="btn_upload_document" onclick="upload_Employee_Document_Info()">UPLOAD</span>
                                                        </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div id="div_documents_table">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                    <div>
                        <div class="row" id="logins_div" style="display: none;">
                        </div>
                    </div>
                </div>
            </div>
            <div>
                <div id="div_employeetable">
                    <table id="tbl_employee" class="table table-bordered table-hover dataTable no-footer"
                        role="grid" aria-describedby="example2_info">
                        <thead>
                            <tr style="background:#5aa4d0; color: white; font-weight: bold;">
                                <th scope="col">
                                    Employee Name
                                </th>
                                <th scope="col">
                                    ID
                                </th>
                                <th scope="col" style="display: none;">
                                    Department
                                </th>
                                <th scope="col">
                                    Employee Type
                                </th>
                                <th scope="col">
                                    Login ID
                                </th>
                                <th scope="col" style="display: none;">
                                    Password
                                </th>
                                <th scope="col">
                                    Date Of Birth
                                </th>
                                <th scope="col">
                                    Licence Number
                                </th>
                                <th scope="col">
                                   LicenceExpireDate
                                </th>
                                <th scope="col">
                                    Status
                                </th>
                                <th scope="col" style="display: none;">
                                    Branch Name
                                </th>
                                <th scope="col" style="display: none;">
                                    sno
                                </th>
                                <th scope="col">
                                </th>
                                <th scope="col" style="display: none;">
                                </th>
                                <th scope="col" style="display: none;">
                                </th>
                                <th scope="col" style="display: none;">
                                </th>
                                <th scope="col" style="display: none;">
                                </th>
                                <th scope="col" style="display: none;">
                                </th>
                                <th scope="col" style="display: none;">
                                </th>
                                <th scope="col" style="display: none;">
                                </th>
                                <th scope="col" style="display: none;">
                                </th>
                                <th scope="col" style="display: none;">
                                </th>
                                <th scope="col" style="display: none;">
                                </th>
                                <th scope="col" style="display: none;">
                                </th>
                                <th scope="col" style="display: none;">
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                    
                </div>
            </div>
    </section>
</asp:Content>
