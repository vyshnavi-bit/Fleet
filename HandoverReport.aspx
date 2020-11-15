<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" CodeFile="HandoverReport.aspx.cs" Inherits="HandoverReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script type="text/javascript">
    $(function () {
        
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
    function get_handover_details() {
        var type = document.getElementById('slct_type').value;
        var fromdate = document.getElementById('txt_frdate').value;
        var todate = document.getElementById('txt_todate').value;
        var data = { 'op': 'get_handover_details', 'type': type, 'fromdate': fromdate, 'todate': todate };
        var s = function (msg) {
            if (msg) {
                if (msg.length > 0) {
                    fillhandoverdetails(msg);
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
    function fillhandoverdetails(msg) {
        var results = '<div  style="overflow:auto;"><table class="table table-bordered table-VehicleNo dataTable no-footer" role="grid" aria-describedby="example2_info">';
        results += '<thead><tr><th scope="col"></th><th scope="col">Sno</th><th scope="col">Position</th><th scope="col">InspectionDate</th><th scope="col">Km Reading</th><th scope="col">ACHour Meter</th><th scope="col">InspectedBy</th></tr></thead></tbody>';
        for (var i = 0; i < msg.length; i++) {
            results += '<tr><td><input id="btn_poplate" type="button" onclick="viewhandoverprint(this)" name="submit" class="btn btn-primary" value="View"/></td>';
            results += '<td data-title="regno" class="1">' + msg[i].sno + '</td>';
            results += '<td data-title="position" class="2" >' + msg[i].vehno + '</td>';
            results += '<td data-title="vtype" class="3" >' + msg[i].inspecteddate + '</td>';
            results += '<td data-title="totalkms" class="4" >' + msg[i].kmreading + '</td>';
            results += '<td data-title="vtype" class="5" >' + msg[i].achrmeter + '</td>';
            results += '<td data-title="kms" class="6">' + msg[i].inspectedby + '</td></tr>';
        }
        results += '</table></div>';
        $("#div_handover").html(results);
    }
    function viewhandoverprint(thisid) {
        var handoversno = $(thisid).parent().parent().children('.1').html();
        var data = { 'op': 'get_handover_sno', 'handoversno': handoversno };
        var s = function (msg) {
            if (msg) {
                if (msg.length > 0) {
                    window.location.href = "viewHandOverprint.aspx";
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
  </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<section class="content-header">
        <h1>
            Handover Report<small>Preview</small>
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
                                <td>
                                    <label>
                                       Type<span style="color: red;">*</span></label>
                                    <select id="slct_type" class="form-control" style="min-width: 150px;">
                                        <option selected disabled value="Select Tyre">Select Type</option>
                                         <option  value="Handover">Handover</option>
                                          <option value="Receiver">Receiver</option>
                                         </td>
                                <td style="width: 5px;">
                                </td>
                                  <td style="height: 50px;">
                                  <label>
                                      Fromdate</label>
                                        <input id="txt_frdate" class="form-control"  type="date">
                                        </td>
                                         <td style="width: 5px;">
                                </td>
                                  <td style="height: 50px;">
                                  <label>
                                     Todate</label>
                                        <input id="txt_todate" class="form-control"  type="date">
                                        </td>
                            </tr>
                        </table>
                        <br />
                        <table  align="center">
                          <tr>
                                    <td>
                                        <input id='btn_handover' type="button" class="btn btn-primary" name="submit" value='Generate'
                                            onclick="get_handover_details()" />
                                    </td>
                                </tr>
                            </table>
                    </div>
                    <div id="div_handover"></div>
                        </div>
                    </div>
                </div>
    </section>

</asp:Content>

