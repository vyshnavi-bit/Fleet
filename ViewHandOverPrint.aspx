<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="ViewHandOverPrint.aspx.cs" Inherits="ViewHandOverPrint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
  <script type="text/javascript">
      function CallPrint(strid) {
          var divToPrint = document.getElementById(strid);
          var newWin = window.open('', 'Print-Window', 'width=400,height=400,top=100,left=100');
          newWin.document.open();
          newWin.document.write('<html><body   onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
          newWin.document.close();
      }
    </script>
<script type="text/javascript">
    $(function () {
        get_handover_sno();
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
    function get_handover_sno() {
        document.getElementById('txt_handoversno').value = '<%= Session["handoversno"] %>';
    }
    function get_handover_print_details() {
        $('#Button2').css('display','block');
        var handoversno = document.getElementById('txt_handoversno').value;
        var type = document.getElementById('slct_type').value;
        if(type=="" || type=="Select Type")
        {
        alert("Please Select Type");
        return;
        }
        var data = { 'op': 'get_handover_print_details', 'type': type, 'handoversno': handoversno };
        var s = function (msg) {
            if (msg) {
                if (msg.length > 0) {
                    $('#divPrint').css('display', 'block');
                    document.getElementById('Spnhandoversno').innerHTML = msg[0].handoversno;
                    document.getElementById('spnAddress').innerHTML = msg[0].address;
                    document.getElementById('spndate').innerHTML = msg[0].date; 
                    document.getElementById('spnvehno').innerHTML = msg[0].vehno;
                    document.getElementById('spanvtype').innerHTML = msg[0].vehtype;
                    document.getElementById('lbldriver').innerHTML = msg[0].empname;
                    document.getElementById('lbllicense').innerHTML = msg[0].licenseno;
                    document.getElementById('spnacmeter').innerHTML = msg[0].achrmeter;
                    document.getElementById('spnrecordchk').innerHTML = msg[0].recordscheck;
                    document.getElementById('spnremarks').innerHTML = msg[0].remarks;
                    document.getElementById('spntime').innerHTML = msg[0].time;
                    document.getElementById('spnmake').innerHTML = msg[0].vehmake;
                    document.getElementById('spncapacity').innerHTML = msg[0].capacity;
                    document.getElementById('spnmobno').innerHTML = msg[0].phoneno;
                    document.getElementById('spnkmreading').innerHTML = msg[0].kmreading;
                    document.getElementById('Spnbodycondition').innerHTML = msg[0].bodycondition;
                    get_vehicle_tools();
                    get_vehicle_tyres();
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
    function get_vehicle_tools() {
        var handoversno = document.getElementById('txt_handoversno').value;
        var data = { 'op': 'get_vehicle_tools', 'handoversno': handoversno };
        var s = function (msg) {
            if (msg) {
                if (msg.length > 0) {
                    filltoolsdetails(msg);
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
    function filltoolsdetails(msg) {
        var results = '<div  style="overflow:auto;"><table class="table table-bordered table-VehicleNo dataTable no-footer" role="grid" aria-describedby="example2_info">';
        results += '<thead><tr><th scope="col">Tools</th></tr></thead></tbody>';
        for (var i = 0; i < msg.length; i++) {
            results += '<tr>';
            results += '<td data-title="regno" class="1">' + msg[i].toolname + '</td></tr>';
        }
        results += '</table></div>';
        $("#div_vehtools").html(results);
    }
    function get_vehicle_tyres() {
        var handoversno = document.getElementById('txt_handoversno').value;
        var data = { 'op': 'get_vehicle_tyres', 'handoversno': handoversno };
        var s = function (msg) {
            if (msg) {
                if (msg.length > 0) {
                    filltyresdetails(msg);
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
    function filltyresdetails(msg) {
        var results = '<div  style="overflow:auto;"><table class="table table-bordered table-VehicleNo dataTable no-footer" role="grid" aria-describedby="example2_info">';
        results += '<thead><tr><th scope="col">TyreNo</th><th scope="col">SVDS No</th><th scope="col">Tyre Type</th><th scope="col">Brand</th><th scope="col">Grove</th></tr></thead></tbody>';
        for (var i = 0; i < msg.length; i++) {
            results += '<tr>';
            results += '<td data-title="regno" class="1">' + msg[i].tyresno + '</td>';
            results += '<td data-title="position" class="2" >' + msg[i].svdsno + '</td>';
            results += '<td data-title="vtype" class="3" >' + msg[i].tyretype + '</td>';
            results += '<td data-title="totalkms" class="4" >' + msg[i].brand + '</td>';
            results += '<td data-title="kms" class="6">' + msg[i].grove + '</td></tr>';
        }
        results += '</table></div>';
        $("#div_vehtyres").html(results);
    }
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<section class="content-header">
        <h1>
          HandOver Report<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Reports</a></li>
            <li><a href="#">HandOver Report</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>HandOver Details
                </h3>
            </div>
            <div class="box-body">
                <div style="padding: 20px;">
                    <div id="tyre_div">
                        <table align="center">
                            <tr>
                             </td>
                                  <td style="height: 50px;">
                                  <label>
                                      Handoversno</label>
                                        <input id="txt_handoversno" class="form-control"  type="text">
                                        </td>
                                         <td style="width: 5px;">
                                </td>
                                 <td style="width: 5px;">
                                <td>
                                    <label>
                                       Type<span style="color: red;">*</span></label>
                                    <select id="slct_type" class="form-control" style="min-width: 150px;">
                                        <option selected disabled value="Select Type">Select Type</option>
                                         <option  value="Handover">Handover</option>
                                          <option value="Receiver">Receiver</option>
                                         </td>
                            </tr>
                        </table>
                        <br />
                        <table  align="center">
                          <tr>
                                    <td>
                                        <input id='btn_handoverview' type="button" class="btn btn-primary" name="submit" value='Generate'
                                            onclick="get_handover_print_details()" />
                                    </td>
                                </tr>
                            </table>
                    </div>
                   
                        </div>
                           <div id="divPrint" style="display: none;">
                <div>
                    <div style="width: 13%; float: right;">
                        <img src="Images/Vyshnavilogo.png" alt="Vyshnavi" width="100px" height="72px" />
                        <br />
                    </div>
                    <div>
                        <div style="font-family: Arial; font-size: 14pt; font-weight: bold; color: Black;">
                            <span>Sri Vyshnavi Dairy Specialities (P) Ltd </span>
                            <br />
                        </div>
                        <div style="width:33%;">
                        <span id="spnAddress" style="font-size: 14px;"></span>
                        </div>
                    </div>
                    <div align="center" style="border-bottom: 1px solid gray; border-top: 1px solid gray;">
                        <span style="font-size: 18px; font-weight: bold;">Vehicle HandOver Report</span>
                    </div>
                    <div style="width: 100%;">
                    <label style="font-size: 15px;font-weight:bold">
                                        Sno :</label>
                                    <span id="Spnhandoversno"></span>
                        <table style="width: 100%;">
                            <tr>
                                <td style="width: 49%; float: left;">
                                 <label style="font-size: 12px;">
                                        Date :</label>
                                    <span id="spndate"></span>
                                    <br />
                                    <label style="font-size: 12px;">
                                        Vehicle No :</label>
                                    <span id="spnvehno" style="font-size: 14px;"></span>
                                    <br />
                                    <label style="font-size: 12px;">
                                        Vehicle Type :</label>
                                    <span id="spanvtype" style="font-size: 14px;"></span>
                                    <br />
                                    <label style="font-size: 12px;">
                                        Driver Name :</label>
                                    <span id="lbldriver" style="font-size: 14px;"></span>
                                    <br />
                                    <label style="font-size: 12px;">
                                        License Details :</label>
                                    <span id="lbllicense" style="font-size: 14px;"></span>
                                    <br />
                                     <br />
                                    <label style="font-size: 12px;">
                                       A/C Hr Meter :</label>
                                    <span id="spnacmeter" style="font-size: 14px;"></span>
                                    <br />
                                    <label style="font-size: 12px;">
                                        Record Check :</label>
                                    <span id="spnrecordchk" style="font-size: 14px;"></span>
                                    <br />
                                    <label style="font-size: 12px;">
                                        Remarks :</label>
                                    <span id="spnremarks" style="font-size: 14px;"></span>
                                    <br />
                                </td>
                                <td style="width: 49%; float: right;">
                                    <label style="font-size: 12px;">
                                       Time :</label>
                                    <span id="spntime" style="font-size: 14px;"></span>
                                    <br />
                                    <label style="font-size: 12px;">
                                       Make :</label>
                                    <span id="spnmake" style="font-size: 14px;"></span>
                                    <br />
                                    <label style="font-size: 12px;">
                                        Capacity :</label>
                                    <span id="spncapacity" style="font-size: 14px;"></span>
                                    <br />
                                    <label style="font-size: 12px;">
                                        Mob No. :</label>
                                    <span id="spnmobno" style="font-size: 14px;"></span>
                                    <br />
                                    <label style="font-size: 12px;">
                                        KM Reading:</label>
                                    <span id="spnkmreading" style="font-size: 14px;"></span>
                                    <br />
                                    <label style="font-size: 12px;">
                                       Body Condition:</label>
                                    <span id="Spnbodycondition" style="font-size: 14px;"></span>
                                    <br />
                                </td>
                            </tr>
                        </table>
                    </div>
                     <div id="div_vehtools"></div>
                     <br />
                      <div id="div_vehtyres"></div>
            </div>
            <br />
                <table style="width: 100%;">
                        <tr>
                            <td style="width: 25%;">
                                <span style="font-weight: bold; font-size: 15px;">Incharge Signature</span>
                            </td>
                            <td style="width: 25%;">
                                <span style="font-weight: bold; font-size: 15px;">Driver Signature</span>
                            </td>
                            <td style="width: 25%;">
                                <span style="font-weight: bold; font-size: 15px;">Authorised Signature</span>
                            </td>
                        </tr>
                    </table>
        </div>
        <br />
        <input id="Button2" type="button" class="btn btn-primary" name="submit" style="display:none;" value='Print'
                    onclick="javascript:CallPrint('divPrint');" />
    </div>
    </div>
                    
    </section>

</asp:Content>
