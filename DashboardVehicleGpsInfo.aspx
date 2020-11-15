<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" CodeFile="DashboardVehicleGpsInfo.aspx.cs" Inherits="DashboardVehicleGpsInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
   
    <script type="text/javascript">
       
        $(function () {
            get_gps_plantname();
        });
        var k = 0;
        var colorue = ["#b3ffe6", "AntiqueWhite", "#daffff", "MistyRose", "Bisque"];

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

        function get_gps_plantname() {
            var data = { 'op': 'get_gps_plantname' };
            var s = function (msg) {
                if (msg) {
                    fillgpsplantname(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }

        function fillgpsplantname(msg) {
            var data = document.getElementById('ddl_Plantname');
            var length = data.options.length;
            document.getElementById('ddl_Plantname').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Plantname";
            opt.value = "Select Plantname";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            data.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].VID != null & msg[i].VID.length>0) {
                    
                    if (i == 1) {
                        var option = document.createElement('option');
                        option.innerHTML = "All";
                        option.value = "All";
                        data.appendChild(option);
                    }
                    var option = document.createElement('option');
                    option.innerHTML = msg[i].VID;
                    option.value = msg[i].Sno;
                    data.appendChild(option);
                }
            }
        }


        var Allvehicles = [];
        function get_VehicleGps_Informations() {
            var ddlPlantname = document.getElementById('ddl_Plantname').value;
            if (ddlPlantname == "") {
                alert("Please, Select the Plantname");
                return false;
            }
            var data = { 'op': 'GetGPsVehicleInformations', 'ddlPlantname': ddlPlantname };
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
            var table = document.getElementById("tbl_vehicleGps_Informations");
            for (var i = table.rows.length - 1; i > 0; i--) {
                table.deleteRow(i);
            }
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].VID != "" || msg[i].VID != null) {
                    var KS = msg[i].Status;
                    var Type = msg[i].Capacity;
                    if (KS == "Red") {
                        
                        $("#tbl_vehicleGps_Informations").append('<tr style="background-color:' + colorue[k] + '"><td data-title="Sno" ><input name="Sno" type="text" class="form-control" style="max-width: 25px;font-size:12px;font-weight:bold;padding: 0px 5px;height:30px;" disabled /></td>' +
                             '<td data-title="VehicleRegistrationNo"><input name="VehicleRegistrationNo" type="text" class="form-control" style="max-width: 100px;font-size:12px;font-weight:bold;padding: 0px 5px;height:30px;" disabled /></td>' +
                             '<td data-title="Status"><span class="badge bg-green" id="2">' + Type + '</span></td>' +
                             '<td data-title="LogDate"><input name="LogDate"  type="text" class="form-control" style="width:80px;font-size:12px;font-weight:bold;padding: 0px 5px;height:30px;" disabled/></td>' +
                             '<td data-title="Device"><input name="Device"  type="text" class="form-control" style="width:70px;font-size:12px;font-weight:bold;padding: 0px 5px;height:30px;" placeholder="Device" /></td>' +
                             '<td data-title="Sim"><input name="Sim"  type="text" class="form-control" style="width:70px;font-size:12px;font-weight:bold;padding: 0px 5px;height:30px;" placeholder="Sim" /></td>' +
                             '<td data-title="Wire"><input name="Wire"  type="text" class="form-control" style="width:70px;font-size:12px;font-weight:bold;padding: 0px 5px;height:30px;" placeholder="Wire" /></td>' +
                             '<td data-title="Vehicle"><input name="Vehicle"  type="text" class="form-control" style="width:70px;font-size:12px;font-weight:bold;padding: 0px 5px;height:30px;" placeholder="Vehicle" /></td>' +
                             '<td data-title="Status"><span class="badge bg-red">' + " Not update " + '</span></td>' +
                             '<td data-title="BtnSave"><input name="BtnSave"  type="button" class="btn btn-primary" style="width:70px;font-size:12px;font-weight:bold;padding: 0px 5px;height:30px;" value="Update" text="Update" onclick="row_Update(this)"  /></td>' +

                             '<td data-title="ID"><input name="ID" type="text" class="form-control" style="display:none" style="max-width: 10px;font-size:12px;font-weight:bold;padding: 0px 5px;height:30px;"/></td></tr>');
                    }
                    else {
                        $("#tbl_vehicleGps_Informations").append('<tr style="background-color:' + colorue[k] + '"><td data-title="Sno" ><input name="Sno" type="text" class="form-control" style="max-width: 25px;font-size:12px;font-weight:bold;padding: 0px 5px;height:30px;" disabled /></td>' +
                            '<td data-title="VehicleRegistrationNo"><input name="VehicleRegistrationNo" type="text" class="form-control" style="max-width: 100px;font-size:12px;font-weight:bold;padding: 0px 5px;height:30px;" disabled /></td>' +
                            '<td data-title="Status"><span class="badge bg-red" id="2">' + Type + '</span></td>' +
                            '<td data-title="LogDate"><input name="LogDate"  type="text" class="form-control" style="width:80px;font-size:12px;font-weight:bold;padding: 0px 5px;height:30px;" disabled/></td>' +
                            '<td data-title="Device"><input name="Device"  type="text" class="form-control" style="width:70px;font-size:12px;font-weight:bold;padding: 0px 5px;height:30px;" placeholder="Device" /></td>' +
                            '<td data-title="Sim"><input name="Sim"  type="text" class="form-control" style="width:70px;font-size:12px;font-weight:bold;padding: 0px 5px;height:30px;" placeholder="Sim" /></td>' +
                            '<td data-title="Wire"><input name="Wire"  type="text" class="form-control" style="width:70px;font-size:12px;font-weight:bold;padding: 0px 5px;height:30px;" placeholder="Wire" /></td>' +
                            '<td data-title="Vehicle"><input name="Vehicle"  type="text" class="form-control" style="width:70px;font-size:12px;font-weight:bold;padding: 0px 5px;height:30px;" placeholder="Vehicle" /></td>' +
                            '<td data-title="Status"><span class="badge bg-green">' + "  In Update " + '</span></td>' +
                            '<td data-title="BtnSave"><input name="BtnSave"  type="button" class="btn btn-primary" style="width:70px;font-size:12px;font-weight:bold;padding: 0px 5px;height:30px;" value="Update" text="Update" onclick="row_Update(this)"  /></td>' +

                            '<td data-title="ID"><input name="ID" type="text" class="form-control" style="display:none" style="max-width: 10px;font-size:12px;font-weight:bold;padding: 0px 5px;height:30px;"/></td></tr>');
                    }
                    k = k + 1;
                    if (k == 4) {
                        k = 0;
                    }
                }
            }
            Fill_Data();
        }

        function Fill_Data() {
            var msginddex = 0;
            $('#tbl_vehicleGps_Informations> tbody > tr').each(function () {
                $(this).find('[name="Sno"]').val(Allvehicles[msginddex].Sno);
                $(this).find('[name="VehicleRegistrationNo"]').val(Allvehicles[msginddex].VehicleRegistrationNo);
                $(this).find('[name="LogDate"]').val(Allvehicles[msginddex].Logdate);
                $(this).find('[name="Device"]').val();
                $(this).find('[name="Sim"]').val();
                $(this).find('[name="Wire"]').val();
                $(this).find('[name="Vehicle"]').val();
              //  $(this).find('[name="Status"]').val();               
                msginddex++;
            });
        }

        function row_Update(thisid) {
            var sno = $(thisid).closest("tr").find('[name="Sno"]').val();
            var VehicleRegistrationNo = $(thisid).closest("tr").find('[name="VehicleRegistrationNo"]').val();
            var LogDate = $(thisid).closest("tr").find('[name="LogDate"]').val();
            var Device = $(thisid).closest("tr").find('[name="Device"]').val();
            var Sim = $(thisid).closest("tr").find('[name="Sim"]').val();
            var Wire = $(thisid).closest("tr").find('[name="Wire"]').val();
            var Vehicle = $(thisid).closest("tr").find('[name="Vehicle"]').val();

            //
            var data = { 'op': 'btnUpdateVehicleGpsInformation', 'VehicleRegistrationNo': VehicleRegistrationNo, 'LogDate': LogDate, 'Device': Device, 'Sim': Sim, 'Wire': Wire, 'Vehicle': Vehicle };
            var s = function (msg) {
                if (msg) {
                    //alert("Data Update Successfully...");
                    alert(msg);
                    get_VehicleGps_Informations();
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
            VehicleGps Informations<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">VehicleGps Informations</a></li>
        </ol>
    </section>
       <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>VehicleGps Informations
                </h3>
            </div>
            <div class="box-body">
                <div id="VehicleGps_showlogs" >                    
                   <table>
                       <tr>
                           <td>
                               <label>PlantName</label>
                           </td>
                           <td>
                               <select id="ddl_Plantname" class="form-control">                                  
                               </select>
                           </td>
                           <td style="width:5px;">
                           </td>
                           <td>
                               <input type="button" id="btn_vehiclelist" class="btn btn-primary" value="Get Vehicles" onclick="get_VehicleGps_Informations()" />
                           </td>
                       </tr>
                   </table>
                </div>
                <div style="overflow-x: scroll;">
                        <table id="tbl_vehicleGps_Informations" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">
                            <thead>
                                <tr>
                                    <th scope="col" >Sno
                                    </th>
                                    <th scope="col">VehicleNo
                                    </th>
                                    <th scope="col">Type
                                    </th>
                                    <th scope="col">LogDate
                                    </th>
                                    <th scope="col">Device
                                    </th>
                                    <th scope="col">Sim
                                    </th>
                                    <th scope="col">Wire
                                    </th>
                                    <th scope="col">Vehicle
                                    </th>   
                                    <th scope="col">Status
                                    </th>                                                          
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

