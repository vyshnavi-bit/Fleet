<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" CodeFile="VehicleRatemaster.aspx.cs" Inherits="VehicleRatemaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(function () {
            Ratemode();
        });

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
        

        var options = "";
        function Ratemode() {
            var Ratemodes = ['PerMonth', 'PerDay', 'PerKm', 'PerKg', 'PerTrip', 'PerKmEmpty'];
            for (var i = 0; i < Ratemodes.length; i++) {
                if (Ratemodes[i] != null) {
                     options += "<option value=" + Ratemodes[i] + ">" + Ratemodes[i] + "</option>";                    
                }
            }
        }

        var Allvehicles = [];
        function get_Vehicle_details() {
            var ddlvehicletype = document.getElementById('ddl_Vehicletype').value;
            if (ddlvehicletype == "") {
                alert("Please, Select the Vehicletype");
                return false;
            }
            var data = { 'op': 'GetVehicleModuleConfig', 'ddlvehicletype': ddlvehicletype };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        Allvehicles = [];
                        Allvehicles = msg;
                        FillVehiclesDetail(msg);
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

        function FillVehiclesDetail(msg) {
            var table = document.getElementById("tbl_vehicle_Details");
            for (var i = table.rows.length - 1; i > 0; i--) {
                table.deleteRow(i);
            }
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].VID != "" || msg[i].VID != null) {
                    $("#tbl_vehicle_Details").append('<tr><td data-title="Sno"><input name="Sno" type="text" class="form-control" style="max-width: 25px;font-size:12px;padding: 0px 5px;height:30px;" disabled /></td>' +
                         '<td data-title="VehicleRegistrationNo"><input name="VehicleRegistrationNo" type="text" class="form-control" style="max-width: 100px;font-size:12px;padding: 0px 5px;height:30px;" disabled /></td>' +
                         '<td data-title="Capacity"><input name="Capacity"  type="text" class="form-control" style="width:70px;font-size:12px;padding: 0px 5px;height:30px;" disabled/></td>' +
                         '<td data-title="PerMonth"><input name="PerMonth"  type="text" class="form-control" style="width:70px;font-size:12px;padding: 0px 5px;height:30px;" placeholder="PerMonth" /></td>' +
                         '<td data-title="PerDay"><input name="PerDay"  type="text" class="form-control" style="width:70px;font-size:12px;padding: 0px 5px;height:30px;" placeholder="PerDay" /></td>' +
                         '<td data-title="PerKm"><input name="PerKm"  type="text" class="form-control" style="width:70px;font-size:12px;padding: 0px 5px;height:30px;" placeholder="PerKm" /></td>' +
                         '<td data-title="PerKg"><input name="PerKg"  type="text" class="form-control" style="width:70px;font-size:12px;padding: 0px 5px;height:30px;" placeholder="PerKg" /></td>' +
                         '<td data-title="PerTrip"><input name="PerTrip"  type="text" class="form-control" style="width:70px;font-size:12px;padding: 0px 5px;height:30px;" placeholder="PerTrip" /></td>' +
                         '<td data-title="PerKmEmpty"><input name="PerKmEmpty"  type="text" class="form-control" style="width:80px;font-size:12px;padding: 0px 5px;height:30px;" placeholder="PerKmEmpty" /></td>' +
                         '<td data-title="PresentDefaultMode"><select class="form-control" name="Present_DefaultMode" style="width: 100px;font-size:12px;padding: 0px 5px;height:30px;" ><option selected disabled value="DefaultMode">DefaultMode</option>' + options + '</select></td>' +
                         '<td data-title="BtnSave"><input name="BtnSave"  type="button" class="btn btn-primary" style="width:70px;font-size:12px;padding: 0px 5px;height:30px;" value="Update" text="Update" onclick="row_Update(this)"  /></td>' +
                         '<td data-title="Minus"><span><img src="images/minus.png" onclick="removerow(this)" style="cursor:pointer"/></span></td>' +
                         '<td data-title="ID"><input name="ID" type="text" class="form-control" style="display:none" style="max-width: 10px;font-size:12px;padding: 0px 5px;height:30px;"/></td></tr>');

                }
            }
            Fill_Data();
        }

        function row_Update(thisid) {

            var sno = $(thisid).closest("tr").find('[name="Sno"]').val();
            var VehicleRegistrationNo = $(thisid).closest("tr").find('[name="VehicleRegistrationNo"]').val();
            var Capacity = $(thisid).closest("tr").find('[name="Capacity"]').val();
            var PerMonth = $(thisid).closest("tr").find('[name="PerMonth"]').val();
            var PerDay = $(thisid).closest("tr").find('[name="PerDay"]').val();
            var PerKm = $(thisid).closest("tr").find('[name="PerKm"]').val();
            var PerKg = $(thisid).closest("tr").find('[name="PerKg"]').val();
            var PerTrip = $(thisid).closest("tr").find('[name="PerTrip"]').val();
            var PerKmEmpty = $(thisid).closest("tr").find('[name="PerKmEmpty"]').val();
            var PresentDefaultMode = $(thisid).closest("tr").find('select[name*="Present_DefaultMode"] :selected').val();

            var ID = $(thisid).closest("tr").find('[name="ID"]').val();

            var flag = false;
            if (PresentDefaultMode == "DefaultMode") {
                alert("Please,Select Vehicle :  " + VehicleRegistrationNo + "   Rate Mode Type...");
                flag = true;
                return false;
            }
            else {
                if (PresentDefaultMode == "PerMonth") {

                    if (PerMonth != "" && PerMonth != 0 && PerMonth != null) {

                    }
                    else {
                        alert("Please, Enter the  " + PresentDefaultMode + "   Value");
                        flag = true;
                        return false;
                    }

                }
                else if (PresentDefaultMode == "PerDay") {
                    if (PerDay != "" && PerDay != 0 && PerDay != null) {

                    }
                    else {
                        alert("Please, Enter the  " + PresentDefaultMode + "   Value");
                        flag = true;
                        return false;
                    }
                }
                else if (PresentDefaultMode == "PerKm") {
                    if (PerKm != "" && PerKm != 0 && PerKm != null) {

                    }
                    else {
                        alert("Please, Enter the  " + PresentDefaultMode + "   Value");
                        flag = true;
                        return false;
                    }
                }
                else if (PresentDefaultMode == "PerKg") {
                    if (PerKg != "" && PerKg != 0 && PerKg != null) {

                    }
                    else {
                        alert("Please, Enter the  " + PresentDefaultMode + "   Value");
                        flag = true;
                        return false;
                    }
                }
                else if (PresentDefaultMode == "PerTrip") {
                    if (PerTrip != "" && PerTrip != 0 && PerTrip != null) {

                    }
                    else {
                        alert("Please, Enter the  " + PresentDefaultMode + "   Value");
                        flag = true;
                        return false;
                    }
                }
                else if (PresentDefaultMode == "PerKmEmpty") {
                    if (PerKmEmpty != "" && PerKmEmpty != 0 && PerKmEmpty != null) {

                    }
                    else {
                        alert("Please, Enter the  " + PresentDefaultMode + "   Value");
                        flag = true;
                        return false;
                    }
                }
                else {
                    flag = true;
                    return false;
                }
            }
            if (flag) {
                return;
            }
            //
            var data = { 'op': 'btnUpdateVehicleRate', 'ID': ID, 'PerMonth': PerMonth, 'PerDay': PerDay, 'PerKm': PerKm, 'PerKg': PerKg, 'PerTrip': PerTrip, 'PerKmEmpty': PerKmEmpty, 'PresentDefaultMode': PresentDefaultMode };
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

        function removerow(thisid) {
            $(thisid).parents('tr').remove();
        }

        function Fill_Data() {
            var msginddex = 0;
            $('#tbl_vehicle_Details> tbody > tr').each(function () {
                $(this).find('[name="Sno"]').val(Allvehicles[msginddex].Sno);
                $(this).find('[name="VehicleRegistrationNo"]').val(Allvehicles[msginddex].VehicleRegistrationNo);
                $(this).find('[name="Capacity"]').val(Allvehicles[msginddex].Capacity);
                $(this).find('[name="PerMonth"]').val(Allvehicles[msginddex].PerMonth);
                $(this).find('[name="PerDay"]').val(Allvehicles[msginddex].PerDay);
                $(this).find('[name="PerKm"]').val(Allvehicles[msginddex].PerKm);
                $(this).find('[name="PerKg"]').val(Allvehicles[msginddex].PerKg);
                $(this).find('[name="PerTrip"]').val(Allvehicles[msginddex].PerTrip);
                $(this).find('[name="PerKmEmpty"]').val(Allvehicles[msginddex].PerKmEmpty);
                $(this).find('[name="PresentDefaultMode"] option[value=' + Allvehicles[msginddex].PresentDefaultMode + ']').attr("selected", "selected");
                $(this).find('[name="ID"]').val(Allvehicles[msginddex].ID);
                msginddex++;
            });
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            VehicleRate Master<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">VehicleRate Master</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Vehicle Ratemaster Details
                </h3>
            </div>
            <div class="box-body">
                 <div id="Ratemaster_showlogs" >                    
                   <table>
                       <tr>
                           <td>
                               <label>VehicleType</label>
                           </td>
                           <td>
                               <select id="ddl_Vehicletype" class="form-control">
                                   <option value="7">Puff</option>
                                   <option value="22">Tanker</option>
                               </select>
                           </td>
                           <td style="width:5px;">
                           </td>
                           <td>
                               <input type="button" id="btn_vehiclelist" class="btn btn-primary" value="Get Vehicles" onclick="get_Vehicle_details()" />
                           </td>
                       </tr>
                   </table>
                </div>
                  <div style="overflow-x: scroll;">
                        <table id="tbl_vehicle_Details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">
                            <thead>
                                <tr>
                                    <th scope="col" >Sno
                                    </th>
                                    <th scope="col">VehicleNo
                                    </th>
                                    <th scope="col">Capacity
                                    </th>
                                    <th scope="col">PerMonth
                                    </th>
                                    <th scope="col">PerDay
                                    </th>
                                    <th scope="col">PerKm
                                    </th>
                                    <th scope="col">PerKg
                                    </th>
                                    <th scope="col">PerTrip
                                    </th>
                                    <th scope="col">PerKmEmpty
                                    </th>
                                    <th scope="col">PresentDMode
                                    </th>                         
                                    <th></th>
                                    <th></th>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                    </div>
            </div>
            </div>
    </section>
</asp:Content>

