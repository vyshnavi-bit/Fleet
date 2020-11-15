<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="VehicleHandOverDetails.aspx.cs" Inherits="TyreInspection" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="autocomplete/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script src="js/utility.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            get_all_vehicles();
            get_vehicleTools();
            get_driverand_helper();
            retrivehandoverdata();
            $('#add_hovernumber').click(function () {
                $('#div_veh_handover').css('display', 'block');
                $('#handover_showlogs').css('display', 'none');
                $('#div_vehhandover_table').css('display', 'none');
            });
            $('#close_photos').click(function () {
                $('#vehhandover_fillform').css('display', 'none');
                $('#handover_showlogs').css('display', 'block');
                $('#div_veh_handover').css('display', 'none');
                $('#div_vehhandover_table').css('display', 'block');
                clearall();
            });
            $('#close_tyre').click(function () {
                $('#vehhandover_fillform').css('display', 'none');
                $('#handover_showlogs').css('display', 'block');
                $('#div_vehhandover_table').css('display', 'block');
                $('#div_veh_handover').css('display', 'none');
            });
        });
        function addhovernumber() {
            $('#div_veh_handover').css('display', 'block');
            $('#handover_showlogs').css('display', 'none');
            $('#div_vehhandover_table').css('display', 'none');
            $("#vehhandover_fillform").css("display", "block");
            $("#div_photos").css("display", "none");
            get_all_vehicles();
            get_vehicleTools();
            get_driverand_helper();
            retrivehandoverdata();
        }
        function closephotes() {
            $('#vehhandover_fillform').css('display', 'none');
            $('#handover_showlogs').css('display', 'block');
            $('#div_veh_handover').css('display', 'none');
            $('#div_vehhandover_table').css('display', 'block');
            clearall();
        }
        var Regions = "";
        function get_vehicleTools() {
            var minimaster = "VehicleTools";
            var data = { 'op': 'get_Mini_Master_data', 'minimaster': minimaster };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        for (var i = 0; i < msg.length; i++) {
                            if (msg[i].mm_name != null && msg[i].mm_status != "0" && msg[i].mm_type == "VehicleTools") {
                                Regions += msg[i].mm_name + ",";
                            }
                        }
                        Regions = Regions.substring(0, Regions.length - 1);
                        FillRegions();
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
        function FillRegions() {
            document.getElementById('divVehicleTools').innerHTML = "";
            var Region = Regions.split(',');
            var data = "<table style='margin: 12px 25px 12px 25px;'>";
            for (var i = 0; i <= Region.length; i++) {
                if (typeof (Region[i]) != "undefined") {
                    data += "<tr><td><input type='checkbox' name='checkbox' value='checkbox' onchange='ckb_onchange(this);' id = " + i + " class = 'chkclass'/><span for=" + i + ">" + Region[i] + "</span></td></tr>";
                }
            }
            data += "</table>";
            $('#divVehicleTools').append(data);
        }
        function get_driverand_helper() {
            var data = { 'op': 'get_driver_and_helper' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        filldrive_helper(msg);
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

        function filldrive_helper(msg) {
            var data = document.getElementById('txt_HandOver_driver_name');
            var length = data.options.length;
            document.getElementById('txt_HandOver_driver_name').options.length = null;

            var opt = document.createElement('option');
            opt.innerHTML = "Select Driver";
            opt.value = "Select Driver";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            data.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].emp_sno != null && msg[i].emp_type == "Driver") {
                    var option = document.createElement('option');
                    option.innerHTML = msg[i].employname;
                    option.value = msg[i].emp_sno;
                    data.appendChild(option);
                }
            }
            var txt_Receiver_driver_name = document.getElementById('txt_Receiver_driver_name');
            var length = txt_Receiver_driver_name.options.length;
            document.getElementById('txt_Receiver_driver_name').options.length = null;

            var opt = document.createElement('option');
            opt.innerHTML = "Select Driver";
            opt.value = "Select Driver";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            txt_Receiver_driver_name.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].emp_sno != null && msg[i].emp_type == "Driver") {
                    var option = document.createElement('option');
                    option.innerHTML = msg[i].employname;
                    option.value = msg[i].emp_sno;
                    txt_Receiver_driver_name.appendChild(option);
                }
            }
        }
        function get_all_vehicles() {
            var data = { 'op': 'get_only_vehicles_data' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillveh(msg);
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
        function fillveh(getvehicles) {
            var vehicles = document.getElementById('slct_vehicleno');
            var length = vehicles.options.length;
            document.getElementById('slct_vehicleno').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Vehicle";
            opt.value = "Select Vehicle";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            vehicles.appendChild(opt);
            for (i = 0; i < getvehicles.length; i++) {
                var option = document.createElement('option');
                option.innerHTML = getvehicles[i].registration_no;
                option.value = getvehicles[i].vm_sno;
                vehicles.appendChild(option);
            }
        }
        function get_tyres() {
            var table = document.getElementById("tyres_table");
            for (var i = table.rows.length - 1; i > 0; i--) {
                table.deleteRow(i);
            }
            var vehicle_sno = document.getElementById('slct_vehicleno').value;
            var data = { 'op': 'get_tyres_for_vehicle', 'vehicle_sno': vehicle_sno };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        var j = 0;
                        for (var i = 0; i < msg.length; i++) {
                            if (msg[i].TyreName != "Stephanie") {
                                j = j + 1;
                                $("#tyres_table").append('<tr><td>' + j + '</td><td style="display:none;" ><label name="sno">' + msg[i].sno + '</label></td><td><label name="tyre_sno" style="font-weight: normal;">' + msg[i].tyre_sno + '</label></td>' +
            '<td><label name="Brand" style="font-weight: normal;">' + msg[i].brand + '</label></td>' +
            '<td><label name="Type_of_Tyre" style="font-weight: normal;">' + msg[i].tyretype + '</label></td>' +
            '<td><label name="Tube_Type" style="font-weight: normal;">' + msg[i].tubetyre + '</label></td>' +
            '<td><label name="Size" style="font-weight: normal;">' + msg[i].size + '</label></td>' +
            '<td><input name="KMS" type="text" placeholder="KMS" class="form-control" value="' + msg[i].current_KMS + '" /></td>' +
            '<td><input name="grove" type="text" placeholder="grove" class="form-control"/></td></tr>');
                            }
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

        function save_vehicle_handover_click() {
            var vehicleno = document.getElementById('slct_vehicleno').value;
            var inspectiondate = document.getElementById('txt_inspectiondate').value;
            var inspectedby = document.getElementById('txt_inspectedby').value;

            var kmreading = document.getElementById('txt_km_reading').value;
            var achrmeter = document.getElementById('txt_achrmeter').value;
            var bodycondition = document.getElementById('sct_Bodycondition').value;
            var recordscheckup = document.getElementById('sct_recordscheckup').value;
            var remarks = document.getElementById('txt_remarks').value;
            var HandOver_driver_name = document.getElementById('txt_HandOver_driver_name').value;
            var Receiver_driver_name = document.getElementById('txt_Receiver_driver_name').value;

            var tyres = [];
            $('#tyres_table> tbody > tr').each(function () {
                var tyresno = $(this).find('[name=sno]').text();
                var Brand = $(this).find('[name=Brand]').text();
                var Type_of_Tyre = $(this).find('[name=Type_of_Tyre]').text();
                var Tube_Type = $(this).find('[name=Tube_Type]').text();
                var Size = $(this).find('[name=Size]').text();
                var grove = $(this).find('[name=grove]').val();
                var KMS = $(this).find('[name=KMS]').val();
                tyres.push({ 'tyresno': tyresno, 'Brand': Brand, 'Type_of_Tyre': Type_of_Tyre, 'Tube_Type': Tube_Type, 'Size': Size, 'grove': grove, 'KMS': KMS });
            });

            var flag = false;

            if (vehicleno == "Select Vehicle") {
                $("#lbl_error_selectveh").show();
                $('#slct_vehicleno').focus();
                flag = true;
            }
            if (inspectiondate == "") {
                $("#lbl_error_inspectiondate").show();
                $('#txt_inspectiondate').focus();
                flag = true;
            }
            if (inspectedby == "") {
                $("#lbl_error_inspectedby").show();
                $('#txt_inspectedby').focus();
                flag = true;
            }
            if (kmreading == "") {
                $("#lbl_km_reading").show();
                $('#txt_km_reading').focus();
                flag = true;
            }
            if (flag) {
                return;
            }
            var ckdvlsdiv = document.getElementById('divVehicleTools').childNodes;
            var checkedvehicleTools = "";
            for (var i = 0, row; row = ckdvlsdiv[0].rows[i]; i++) {
                if (row.cells[0].childNodes[0].type == 'checkbox' && row.cells[0].childNodes[0].checked == true) {
                    var labelval = row.cells[0].childNodes[1].innerHTML;
                    checkedvehicleTools += labelval + ",";
                }
            }
            var data = { 'op': 'Tyres_save_start' };
            var s = function (msg) {
                if (msg) {
                    for (var i = 0; i < tyres.length; i++) {
                        var Data = { 'op': 'Tyres_save_RowData', 'row_detail': tyres[i], 'end': 'N' };
                        if (i == tyres.length - 1) {
                            Data = { 'op': 'Tyres_save_RowData', 'row_detail': tyres[i], 'end': 'Y' };
                        }
                        var s = function (msg) {
                            if (msg == 'Y') {

                                var Data = { 'op': 'save_edit_TyresInspection', 'vehicleno': vehicleno, 'inspectiondate': inspectiondate, 'inspectedby': inspectedby, 'kmreading': kmreading, 'achrmeter': achrmeter, 'bodycondition': bodycondition, 'recordscheckup': recordscheckup, 'remarks': remarks, 'checkedvehicleTools': checkedvehicleTools, 'HandOver_driver_name': HandOver_driver_name, 'Receiver_driver_name': Receiver_driver_name };
                                var s = function (msg) {
                                    if (msg) {
                                        alert(msg);
                                        hide_error_logs();
                                    }
                                }
                                var e = function (x, h, e) {
                                };
                                $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
                                callHandler(Data, s, e);
                            }

                        }
                        var e = function (x, h, e) {
                        };
                        $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
                        CallHandlerUsingJson(Data, s, e);
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
        function hide_error_logs() {
            $("#lbl_error_selectveh").hide();
            $("#lbl_error_inspectiondate").hide();
            $("#lbl_error_inspectedby").hide();
        }
        function resetall() {
            document.getElementById('slct_vehicleno').value = "Select Vehicle";
            document.getElementById('txt_inspectiondate').value = "";
            document.getElementById('txt_inspectedby').value = "";
            document.getElementById('txt_km_reading').value = "";
            document.getElementById('sct_Bodycondition').innerHTML = 0;
            document.getElementById('sct_recordscheckup').innerHTML = 0;
            document.getElementById('txt_remarks').value = "";
            document.getElementById('txt_achrmeter').value = "";

            document.getElementById('save_tyre').innerHTML = "Save";
            var table = document.getElementById("tyres_table");
            for (var i = table.rows.length - 1; i > 0; i--) {
                table.deleteRow(i);
            }
            hide_error_logs();
            $('#vehhandover_fillform').css('display', 'none');
            $('#handover_showlogs').css('display', 'block');
            $('#div_vehhandover_table').css('display', 'block');
            $('#div_veh_handover').css('display', 'none');
        }
        function change_Documents() {
            $("#vehhandover_fillform").css("display", "none");
            $("#div_photos").css("display", "block");
        }
        function change_Personal() {
            $("#vehhandover_fillform").css("display", "block");
            $("#div_photos").css("display", "none");
        }
        function getFile() {
            document.getElementById("FileUpload1").click();
        }
        function get_single_File() {
            document.getElementById("file").click();
        }
        function readURL(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();

                reader.onload = function (e) {
                    $('#main_img,#img_1').attr('src', e.target.result).width(155).height(200);
                    //                    $('#img_1').css('display', 'inline');
                };
                reader.readAsDataURL(input.files[0]);
            }
        }
        function readURL_photo(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.readAsDataURL(input.files[0]);
                document.getElementById("FileUpload_div").innerHTML = input.files[0].name;
            }
        }
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
        function upload_vehicle_photos() {
            var dataURL = document.getElementById('main_img').src;
            var div_text = $('#yourBtn').text().trim();
            var blob = dataURItoBlob(dataURL);
            //            var empsno = 1;
            var Data = new FormData();
            Data.append("op", "vehicle_handover_pics_upload");
            Data.append("vehiclesno", vehiclesno);
            Data.append("Veh_sno", Veh_sno);
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
        function upload_all_vehicle_photos() {
            var photoid = document.getElementById('ddl_phototype').value;
            var photoname = document.getElementById('ddl_phototype').selectedOptions[0].innerText;
            if (photoid == null || photoid == "" || photoid == "Select Photo Type") {
                document.getElementById("ddl_phototype").focus();
                alert("Please select Photo Type");
                return false;
            }
            var PhotoExists = 0;
            $('#tbl_documents tr').each(function () {
                var selectedrow = $(this);
                var photo_manager_id = selectedrow[0].cells[0].innerHTML;
                if (photo_manager_id == photoid) {
                    alert(photoname + "  Already Exist For This Vehicle");
                    PhotoExists = 1;
                    return false;
                }

            });
            if (PhotoExists == 1) {
                return false;
            }
            var Data = new FormData();
            Data.append("op", "save_Vehicle_handover_Info");
            Data.append("vehiclesno", vehiclesno);
            Data.append("registration_no", reg_no);
            Data.append("photoname", photoname);
            Data.append("photoid", photoid);
            var fileUpload = $("#FileUpload1").get(0);
            var files = fileUpload.files;
            for (var i = 0; i < files.length; i++) {
                Data.append(files[i].name, files[i]);
            }
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    getvehicle_Uploaded_Documents(vehiclesno);
                }
            };
            var e = function (x, h, e) {
                alert(e.toString());
            };
            callHandler_nojson_post(Data, s, e);
        }
        var VehicleDetails = [];
        function retrivehandoverdata() {
            var table = document.getElementById("tbl_vehhandover");
            for (var i = table.rows.length - 1; i > 0; i--) {
                table.deleteRow(i);
            }
            var data = { 'op': 'get_veh_handover_data' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        filldata(msg);
                        fillVehicledetails(msg);
                        VehicleDetails = msg;
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
        var vehicleList = [];
        function fillVehicledetails(msg) {
            for (var i = 0; i < msg.length; i++) {
                var registration_no = msg[i].vehno;
                vehicleList.push(registration_no);
            }
            $('#txt_Vehicle').autocomplete({
                source: vehicleList,
                change: vehiclenochange,
                autoFocus: true
            });
        }
        function vehiclenochange() {
            var results = VehicleDetails;
            var vehicleno = document.getElementById("txt_Vehicle").value;
            if (vehicleno == "") {
                retrivehandoverdata();
            }
            var table = document.getElementById("tbl_vehhandover");
            for (var i = table.rows.length - 1; i > 0; i--) {
                table.deleteRow(i);
            }
            var k = 0;
            var colorue = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            for (var i = 0; i < results.length; i++) {
                if (vehicleno == results[i].vehno) {
                    if (results[i].vehno != null) {
                        var registration_no = results[i].vehno;
                        var kmreading = results[i].kmreading;
                        var receiverid = results[i].receiverid;
                        var receiverdrier = results[i].receiverdrier;
                        var handoverid = results[i].handoverid;
                        var handoverdriver = results[i].handoverdriver;
                        var inspectedby = results[i].inspectedby;
                        var inspecteddate = results[i].inspecteddate;
                        var acmeter = results[i].acmeter;
                        var bodycondition = results[i].bodycondition;
                        var recordscheck = results[i].recordscheck;
                        var remarks = results[i].remarks;
                        var imageloc = results[i].imagename;
                        var ftp = results[i].ftplocation;
                        var rndmnum = Math.floor((Math.random() * 10) + 1);
                        img_url = ftp + imageloc + '?v=' + rndmnum;
                        var vm_sno = results[i].vm_sno;
                        var tablerowcnt = document.getElementById("tbl_vehhandover").rows.length;
                        $('#tbl_vehhandover').append('<tr style="background-color:' + colorue[k] + '">' +
                        //'<th scope="row">' + registration_no + '</th>' +
                        '<td style="font-weight: 600;"><i class="fa fa-truck" aria-hidden="true"></i>&nbsp;<span id="0">' + registration_no + '</span></td>' +
                        //'<td data-title="Vehicle Type">' + inspecteddate + '</td>' +
                        '<td><i class="fa fa-calendar" aria-hidden="true"></i>&nbsp;<span id="1">' + inspecteddate + '</span></td>' +
                        '<td data-title="Door No" >' + inspectedby + '</td>' +
                        '<td data-title="Status" >' + kmreading + '</td>' +
                        '<td data-title="sno" style="display:none;">' + vm_sno + '</td>' +
                        //'<td data-title="Capacity">' + receiverdrier + '</td>' +
                        '<td><i class="fa fa-user" aria-hidden="true"></i>&nbsp;<span id="5">' + receiverdrier + '</span></td>' +
                        //'<td data-title="Vehicle Make">' + handoverdriver + '</td>' +
                        '<td><i class="fa fa-user" aria-hidden="true"></i>&nbsp;<span id="6">' + handoverdriver + '</span></td>' +
                        '<td data-title="Fuel Capacity" style="display:none;">' + acmeter + '</td>' +
                        '<td data-title="AxilName_sno" style="display: none;">' + bodycondition + '</td>' +
                        '<td data-title="AxilName" style="display:none;">' + recordscheck + '</td>' +
                        '<td data-title="act_mileage" style="display:none;">' + remarks + '</td>' +
                        '<td data-title="Fuel Capacity" style="display:none;">' + ftp + '</td>' +
                        //'<td><input type="button" class="btn btn-primary" name="Update" value ="Modify" onclick="updateclick(this);"/></td></tr>');
                        '<td><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls" name="Update" value ="Modify"  onclick="updateclick(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>');
                        k = k + 1;
                        if (k == 4) {
                            k = 0;
                        }
                    }
                }
            }
        }
        var img_url;
        function filldata(results) {
            var k = 0;
            var colorue = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            var table = document.getElementById("tbl_vehhandover");
            for (var i = 0; i < results.length; i++) {
                if (results[i].vehno != null) {
                    var registration_no = results[i].vehno;
                        var kmreading = results[i].kmreading;
                        var receiverid = results[i].receiverid;
                        var receiverdrier = results[i].receiverdrier;
                        var handoverid = results[i].handoverid;
                        var handoverdriver = results[i].handoverdriver;
                        var inspectedby = results[i].inspectedby;
                        var inspecteddate = results[i].inspecteddate;
                        var acmeter = results[i].acmeter;
                        var bodycondition = results[i].bodycondition;
                        var recordscheck = results[i].recordscheck;
                        var remarks = results[i].remarks;
                        var imageloc = results[i].imagename;
                        var ftp = results[i].ftplocation;
                        var rndmnum = Math.floor((Math.random() * 10) + 1);
                        img_url = ftp + imageloc + '?v=' + rndmnum;
                        var vm_sno = results[i].vm_sno;
                        var tablerowcnt = document.getElementById("tbl_vehhandover").rows.length;
                        $('#tbl_vehhandover').append('<tr style="background-color:' + colorue[k] + '">' +
                        //'<th scope="row">' + registration_no + '</th>' +
                        '<td style="font-weight: 600;"><i class="fa fa-truck" aria-hidden="true"></i>&nbsp;<span id="0">' + registration_no + '</span></td>' +
                        //'<td data-title="Vehicle Type">' + inspecteddate + '</td>' +
                        '<td><i class="fa fa-calendar" aria-hidden="true"></i>&nbsp;<span id="1">' + inspecteddate + '</span></td>' +
                        '<td data-title="Door No" >' + inspectedby + '</td>' +
                        '<td data-title="Status" >' + kmreading + '</td>' +
                        '<td data-title="sno" style="display:none;">' + vm_sno + '</td>' +
                        //'<td data-title="Capacity">' + receiverdrier + '</td>' +
                        '<td><i class="fa fa-user" aria-hidden="true"></i>&nbsp;<span id="5">' + receiverdrier + '</span></td>' +
                        //'<td data-title="Vehicle Make">' + handoverdriver + '</td>' +
                        '<td><i class="fa fa-user" aria-hidden="true"></i>&nbsp;<span id="6">' + handoverdriver + '</span></td>' +
                        '<td data-title="Fuel Capacity" style="display:none;">' + acmeter + '</td>' +
                        '<td data-title="AxilName_sno" style="display: none;">' + bodycondition + '</td>' +
                        '<td data-title="AxilName" style="display:none;">' + recordscheck + '</td>' +
                        '<td data-title="act_mileage" style="display:none;">' + remarks + '</td>' +
                        '<td data-title="Fuel Capacity" style="display:none;">' + ftp + '</td>' + 
                        //'<td><input type="button" class="btn btn-primary" name="Update" value ="Modify" onclick="updateclick(this);"/></td></tr>');
                        '<td><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls" name="Update" value ="Modify"  onclick="updateclick(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>');
                        k = k + 1;
                        if (k == 4) {
                            k = 0;
                        }
                    }
                }
            }
            var Veh_sno = "";
            var vehiclesno = "";
            var reg_no = "";
            function updateclick(thisid) {
                var row = $(thisid).parents('tr');
                Veh_sno = row[0].cells[4].innerHTML;
                var registration_no = $(thisid).parent().parent().find('#0').text();
                vehiclesno = Veh_sno;
                reg_no = $(thisid).parent().parent().find('#0').text();
                var kmreading = row[0].cells[3].innerHTML;
                var receiverid = row[0].cells[2].innerHTML;
                var receiverdrier = $(thisid).parent().parent().find('#6').text();
                var handoverid = row[0].cells[4].innerHTML;
                var handoverdriver = $(thisid).parent().parent().find('#5').text();
                var inspectedby = row[0].cells[2].innerHTML;
                var inspecteddate = $(thisid).parent().parent().find('#1').text();
                var acmeter = row[0].cells[7].innerHTML;
                var bodycondition = row[0].cells[8].innerHTML;
                var recordscheck = row[0].cells[9].innerHTML;
                var remarks = row[0].cells[10].innerHTML;
                var imageurl = row[0].cells[11].innerHTML;
                document.getElementById('lbl_topvehname').innerHTML = $(thisid).parent().parent().find('#0').text();
                $("select#slct_vehicleno option").each(function () { this.selected = (this.text == registration_no); });
                $("select#txt_HandOver_driver_name option").each(function () { this.selected = (this.text == handoverdriver); });
                $("select#txt_Receiver_driver_name option").each(function () { this.selected = (this.text == receiverdrier); });
                document.getElementById('txt_inspectiondate').value = inspecteddate;
                document.getElementById('txt_inspectedby').value = inspectedby;
                document.getElementById('txt_km_reading').value = kmreading;
                document.getElementById('sct_Bodycondition').value = bodycondition;
                document.getElementById('sct_recordscheckup').value = recordscheck;
                document.getElementById('txt_remarks').value = remarks;
                document.getElementById('txt_achrmeter').value = acmeter;
                $('#div_vehhandover_table').css('display', 'none');
                $('#handover_showlogs').css('display', 'none');
                $('#div_veh_handover').css('display', 'block');
                $("#vehhandover_fillform").css("display", "block");
                $("#div_photos").css("display", "none");
                //$('#save_tyre').val("Modify");
                document.getElementById('save_tyre').innerHTML = "Modify";
//                $('.hiddenrow').show();
                if (imageurl != "") {
                    $('#main_img').attr('src', imageurl).width(155).height(200);
                }
                else {
                    $('#main_img').attr('src', 'Images/Employeeimg.jpg').width(200).height(200);
                }
                getvehicle_Uploaded_Documents(Veh_sno);
            }

            function getvehicle_Uploaded_Documents(Vehiclesno) {
                var data = { 'op': 'getvehicle_Uploaded_photos', 'Vehiclesno': Vehiclesno };
                var s = function (msg) {
                    if (msg) {
                        fill_Uploaded_Documents(msg);
                    }
                    else {
                    }
                };
                var e = function (x, h, e) {
                }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
                callHandler(data, s, e);
            }
            function fill_Uploaded_Documents(msg) {

                var results = '<div class="divcontainer" style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer">';
                results += '<thead><tr><th scope="col">Sno</th><th scope="col" style="text-align:center">Photo Name</th><th scope="col">Photo</th><th scope="col">Download</th></tr></thead></tbody>';
                var k = 1;
                for (var i = 0; i < msg.length; i++) {
                    results += '<tr><td>' + k++ + '</td>';
                    var path = img_url = msg[i].ftplocation + msg[i].photo;
                    var photoid = msg[i].imageid;
                    var photoname = "";
                    if (photoid == "1") {
                        photoname = "Left";
                    }
                    if (photoid == "2") {
                        photoname = "Right";
                    }
                    if (photoid == "3") {
                        photoname = "Front";
                    }
                    if (photoid == "4") {
                        photoname = "Back";
                    }
                    results += '<th scope="row" class="1" style="text-align:center;">' + photoname + '</th>';
                    results += '<td data-title="brandstatus" class="2"><img src=' + path + '  style="cursor:pointer;height:400px;width:400px;border-radius: 5px;"/></td>';
                    results += '<th scope="row" class="1" ><a  target="_blank" href=' + path + '><i class="fa fa-download" aria-hidden="true"></i> Download</a></th>';
                    results += '</tr>';
                }
                results += '</table></div>';
                $("#div_Photos_table").html(results);
            }
      

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Vehicle HandOver Details<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Vehicle HandOver Details</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Vehicle HandOver Details
                </h3>
            </div>
            <div class="box-body">
             <div id="handover_showlogs" style="text-align: center;">
                    <table>
                        <tr>
                            <td>
                                <input id="txt_Vehicle" type="text" style="height: 28px; opacity: 1.0; width: 180px;"
                                    class="form-control" placeholder="Search vehicle Number" />
                            </td>
                            <td style="width: 5px;">
                            </td>
                           <%-- <td>
                                <i class="fa fa-search" aria-hidden="true">Search</i>
                            </td>--%>
                            <td>
                               <div class="input-group">
                                <div class="input-group-addon">
                                <span  class="fa fa-search"  onclick="vehiclenochange();"></span>
                             </div>
                             </div>
                            </td>
                            <td style="width: 500px">
                            </td>
                            <%--<td>
                                <input id="add_hovernumber" type="button" class="btn btn-primary" name="submit" value='Add Vehicle HandOver' />
                            </td>--%>
                            <td>
                            <div class="input-group" style="padding-left: 665px;padding-bottom: 15px;">
                                <div class="input-group-addon">
                                <span  class="glyphicon glyphicon-plus-sign" onclick="addhovernumber()"></span><span id="add_hovernumber" onclick="addhovernumber()">Add Vehicle HandOver</span>
                          </div>
                          </div>
                            </td>
                        </tr>
                    </table>
            
            </div>
              <div id='div_veh_handover' style="display: none;">
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
                                                            src="Images/Employeeimg.jpg" alt="your image" style="width: 155px; height: 200px;
                                                            border-radius: 50%;" />
                                                        <%--<img id="prw_img" class="center-block img-circle img-thumbnail img-responsive profile-img" src="Images/Employeeimg.jpg" alt="your image" style="width: 150px; height: 150px;">--%>
                                                        <div class="photo-edit-admin">
                                                            <a onclick="get_single_File();" class="photo-edit-icon-admin" href="/employee/emp-master/emp-photo?eid=3"
                                                                title="Change Vehicle Picture" data-toggle="modal" data-target="#photoup"><i class="fa fa-pencil">
                                                                </i></a>
                                                        </div>
                                                        <div id="yourBtn" class="img_btn" onclick="get_single_File();" style="margin-top: 5px; display: none;">
                                                            Click to Choose Image
                                                        </div>
                                                        <div style="height: 0px; width: 0px; overflow: hidden;">
                                                            <input id="file" type="file" name="files[]" onchange="readURL(this);">
                                                        </div>
                                                        <div>
                                                            <input type="button" id="btn_upload_profilepic" class="btn btn-primary" onclick="upload_vehicle_photos();"
                                                                style="margin-top: 5px;" value="Upload Vehicle Pic">
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-xs-12 col-sm-9">
                                                    <h2 class="text-primary">
                                                        <b><span class="fa fa-truck"></span>
                                                            <label id="lbl_topvehname">
                                                            </label>
                                                        </b>
                                                    </h2>
                                                   <%-- <p>
                                                        <strong>Vehicle Type : <span style="color: Red;">*</span></strong>
                                                        <label style="padding-left: 20px; font-weight: 700;" id="lbl_topemployeeid">
                                                        </label>
                                                    </p>
                                                    <p>
                                                        <strong>Vehicle Make : <span style="color: Red;">*</span></strong>
                                                        <label id="lbl_topempemailid">
                                                        </label>
                                                    </p>--%>
                                                </div>
                                                <!--/col-->
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                   <div>
                        <ul class="nav nav-tabs">
                            <li id="id_tab_Personal" class="active"><a data-toggle="tab" href="#" onclick="change_Personal()">
                                <i class="fa fa-street-view"></i>&nbsp;&nbsp;Vehicle Hand Over Details</a></li>
                            <li id="id_tab_documents" class=""><a data-toggle="tab" href="#" onclick="change_Documents()">
                                <i class="fa fa-file-text"></i>&nbsp;&nbsp;Vehicle Photos </a></li>
                        </ul>
                    </div>
                <div id='vehhandover_fillform' style="padding: 20px;">
                    <table align="center">
                        <tr>
                            <td>
                                <label>
                                    Vehicle Number <span style="color: red;">*</span></label>
                                <select id="slct_vehicleno" class="form-control" onchange="get_tyres();">
                                </select>
                                <label id="lbl_error_selectveh" class="errormessage">
                                    * Please select Vehicle</label>
                            </td>
                            <td style="width: 6px;">
                            </td>
                            <td>
                                <label>
                                    HandOver Date <span style="color: red;">*</span></label>
                                <input id="txt_inspectiondate" class="form-control" type="date">
                                <label id="lbl_error_inspectiondate" class="errormessage">
                                    * Please select HandOver Date</label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    HandOver By <span style="color: red;">*</span></label>
                                <input id="txt_inspectedby" class="form-control" type="text" name="vendorcode" placeholder="HandOver By">
                                <label id="lbl_error_inspectedby" class="errormessage">
                                    * Please Enter HandOver By</label>
                            </td>
                            <td style="width: 6px;">
                            </td>
                            <td>
                                <label>
                                    KM Reading <span style="color: red;">*</span></label>
                                <input id="txt_km_reading" class="form-control" type="text" name="vendorcode" placeholder="KM Reading">
                                <label id="lbl_km_reading" class="errormessage">
                                    * Please Enter KM Reading</label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    A/C Hr Meter <span style="color: red;">*</span></label>
                                <input id="txt_achrmeter" class="form-control" type="text" name="vendorcode" placeholder=" A/C Hr Meter">
                            </td>
                            <td style="width: 6px;">
                            </td>
                            <td>
                                <label>
                                    Body Condition <span style="color: red;">*</span></label>
                                <select id="sct_Bodycondition" class="form-control">
                                    <option>Ok</option>
                                    <option>pbm</option>
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Records Checkup <span style="color: red;">*</span></label>
                                <select id="sct_recordscheckup" class="form-control">
                                    <option>Yes</option>
                                    <option>No</option>
                                </select>
                            </td>
                            <td style="width: 6px;">
                            </td>
                            <td>
                                <label>
                                    Remarks<span style="color: red;"></span></label>
                                <textarea id="txt_remarks" class="form-control"></textarea>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    HandOver Driver Name <span style="color: red;">*</span></label>
                                <select id="txt_HandOver_driver_name" class="form-control">
                                </select>
                            </td>
                            <td style="width: 6px;">
                            </td>
                            <td>
                                <label>
                                    Receiver Driver Name <span style="color: red;">*</span></label>
                                <select id="txt_Receiver_driver_name" class="form-control">
                                </select>
                            </td>
                        </tr>
                        <tr>
                        <td>
                        <br />
                        </td>
                        </tr>
                    </table>
                    <div class="row">
                        <div class="form-group">
                             <div class="box box-success">
                            <div class="box-header with-border">
                                <h3 class="box-title">
                                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Vehicle Tools Details
                                </h3>
                            </div>
                            <br />
                            <div id="divVehicleTools" style="height: 130px;  overflow: auto;
                                padding-left: 10%; border-radius: 7px 7px 7px 7px; font-size: 18px">
                            </div>
                            </div>
                        </div>
                    </div>
                    <div class="table-responsive">
                        <table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info"
                            id="tyres_table">
                            <thead>
                                <tr>
                                    <th>
                                        #
                                    </th>
                                    <th style="display: none;">
                                        sno
                                    </th>
                                    <th>
                                        Tyre_Sno
                                    </th>
                                    <th>
                                        Brand
                                    </th>
                                    <th>
                                        Type_of_Tyre
                                    </th>
                                    <th>
                                        Tube_Type
                                    </th>
                                    <th>
                                        Size
                                    </th>
                                    <th>
                                        KMS
                                    </th>
                                    <th>
                                        Grove
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                        <div align="right" id="morerows_div" style="display: none;">
                            <input id='btn_addmore' type="button" class="btn btn-default" value='Add More Tyres'
                                onclick="addmore()" />
                        </div>
                    </div>
                    <div align="center">
                        <%--<input id='save_tyre' type="button" class="btn btn-primary" value='Save' onclick="save_vehicle_handover_click()" />
                        <input id='close_tyre' type="button" class="btn btn-danger" value='Reset' onclick="resetall()" />--%>
                         <table>
                            <tr>
                            <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span class="glyphicon glyphicon-ok" id="save_tyre1" onclick="save_vehicle_handover_click()"></span><span id="save_tyre" onclick="save_vehicle_handover_click()">Save</span>
                            </div>
                            </div>
                            </td>
                            <td style="width:10px;"></td>
                            <td>
                                <div class="input-group">
                                <div class="input-group-close">
                                <span class="glyphicon glyphicon-remove" id="close_tyre1" onclick="resetall()"></span><span id="close_tyre" onclick="resetall()">Reset</span>
                            </div>
                            </div>
                            </td>
                            </tr>
                        </table>
                    </div>
                </div>
                    <div id="div_photos" class="box box-danger" style="display: none;">
                            <div class="box-header with-border">
                                <h3 class="box-title">
                                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Photos Upload</h3>
                            </div>
                            <div class="box-body">
                                <div class="row">
                                    <div>
                                        <br>
                                        <div class="box-body">
                                            <div class="row">
                                                <div class="col-sm-4">
                                                    <label class="control-label">
                                                       Photo Type</label>
                                                    <select id="ddl_phototype" class="form-control">
                                                        <option>Select Photo Type</option>
                                                        <option value="1">Left</option>
                                                        <option value="2">Right</option>
                                                        <option value="3">Front </option>
                                                        <option value="4">Back</option>
                                                    </select>
                                                </div>
                                                <div class="col-sm-4">
                                                    <table class="table table-bordered table-striped">
                                                        <tbody>
                                                            <tr>
                                                                <td>
                                                                    <div id="FileUpload_div" class="img_btn" onclick="getFile()" style="height: 50px;
                                                                        width: 100%">
                                                                        Choose Photo To Upload
                                                                    </div>
                                                                    <div style="height: 0px; width: 0px; overflow: hidden;">
                                                                        <input id="FileUpload1" type="file" name="files[]" onchange="readURL_photo(this);">
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </div>
                                                <div class="col-sm-4">
                                                    <input id="btn_upload_photo" type="button" class="btn btn-primary" name="submit"
                                                        value="UPLOAD" onclick="upload_all_vehicle_photos()" style="width: 120px;
                                                        margin-top: 25px;">
                                                </div>
                                            </div>
                                        </div>
                                        <div class="box-body">
                                            <div id="div_Photos_table">
                                            </div>
                                        </div>
                                        <br />
                                        <div>
                                           <%-- <input id='close_photos' type="button" class="btn btn-danger" name="Close" value='Close' />--%>
                                           <table>
                                           <tr>
                                               <td style="padding-left: 400px;">
                                                <div class="input-group">
                                                <div class="input-group-close">
                                                <span class="glyphicon glyphicon-remove" id="close_photos1" onclick="closephotes()"></span><span id="close_photos" onclick="closephotes()">Close</span>
                                            </div>
                                            </div>
                                            </td>
                                            </tr>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
              <div id="div_vehhandover_table">
                    <table id="tbl_vehhandover" class="table table-bordered table-striped">
                        <thead>
                            <tr  style="background:#5aa4d0; color: white; font-weight: bold;">
                                <th scope="col">
                                    Registration Number
                                </th>
                                <th scope="col">
                                  HandOver Date
                                </th>
                                <th scope="col">
                                   Handover By
                                </th>
                                <th scope="col">
                                    KM Reading
                                </th>
                                <th scope="col" style="display: none;">
                                    sno
                                </th>
                                <th scope="col">
                                    Handover Driver
                                </th>
                                <th scope="col">
                                    Receiver Driver
                                </th>
                                <th scope="col">
                                </th>
                                <%--<th scope="col">
                                    Remarks
                                </th>--%>
                                <%--<th scope="col" style="display: none;">
                                    AxilName_sno
                                </th>
                                <th scope="col">
                                    AxilName
                                </th>
                                <th scope="col">
                                </th>--%>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                </div>
        </div>
    </section>
</asp:Content>
