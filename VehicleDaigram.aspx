<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" CodeFile="VehicleDaigram.aspx.cs" Inherits="VehicleDaigram" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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
    $(function () {
        vehtype_onchange();
        get_tyresdata();
    });
    var options_val = "";
    var axil_name_check = "";
    var tyres_data = [];
    function get_tyresdata() {
        var data = { 'op': 'get_only_tyre_data' };
        var s = function (msg) {
            if (msg) {
                options_val = "<option value='Select Tyre' selected disabled>Select Tyre</option>";
                if (msg.length > 0) {
                    tyres_data = [];
                    for (var i = 0; i < msg.length; i++) {
                        if (msg[i].tyre_sno != null && msg[i].status != "0") {
                            options_val += "<option value='" + msg[i].sno + "'>" + msg[i].tyre_sno + "</option>";
                            tyres_data.push({ label: msg[i].tyre_sno, id: msg[i].sno });
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
    function vehtype_onchange() {
        $('#div_axilautofill').show();
        var sno = "11"; // document.getElementById('slct_axilmaster').value;
        var data = { 'op': 'get_all_data_Axils', 'sno': sno };
        var s = function (msg) {
            if (msg) {
                if (msg.length > 0) {
                    var table = document.getElementById("tbl_axils");
                    for (var i = table.rows.length - 1; i > 0; i--) {
                        table.deleteRow(i);
                    }
                    var main_results = "";
                    var right = "";
                    var left = "";
                    for (var i = 0; i < msg.length; i++) {
                        //$("#tbl_axils").append('<tr><th scope="row" style="border: 1px solid #1d96b2;">' + (i + 1) + '</th><td style="border: 1px solid #1d96b2;" data-title="Axil Name"><input type="text" placeholder="Axle Name" value="' + msg[i].AxileName + '" class="axilname"/></td><td style="border: 1px solid #1d96b2;" data-title="No.of Tyres"><input step="2" min="0" type="number" onblur="onchange_row(this)" value="' + msg[i].nooftyresperaxle + '" class="tyreno"/></td><td style="display:none;">' + msg[i].veh_typ_axel_sno + '</td></tr><tr class="hide_row" style="display:none;"><th colspan="4" scope="row"><div class="sub_td" style="float:right;"></div></th></tr>');
                        right = "";
                        left = "";
                        if (msg[i].AxileName != "Stephanie") {
                            main_results += '<tr><th scope="row" style="border: 1px solid #1d96b2;">' + (i + 1) + '</th><td style="border: 1px solid #1d96b2;" data-title="Axil Name"><label>' + msg[i].AxileName + '</label></td><td style="border: 1px solid #1d96b2;" data-title="No.of Tyres"><label>' + msg[i].nooftyresperaxle + ' </label></td><td style="display:none;"><label class="axlesno">' + msg[i].veh_typ_axel_sno + '</label></td></tr><tr class="hide_row"><th colspan="4" scope="row"><div class="sub_td" style="float:right;">';
                            // main_results += '<tr><th scope="row" style="border: 1px solid #1d96b2;">' + (i + 1) + '</th><td style="border: 1px solid #1d96b2;" data-title="Axil Name"><input type="text" placeholder="Axle Name" value="' + msg[i].AxileName + '" class="axilname"/></td><td style="border: 1px solid #1d96b2;" data-title="No.of Tyres"><input step="2" min="0" type="number" onblur="onchange_row(this)" value="' + msg[i].nooftyresperaxle + '" class="tyreno"/></td><td style="display:none;"><label class="axlesno">' + msg[i].veh_typ_axel_sno + '</label></td></tr><tr class="hide_row"><th colspan="4" scope="row"><div class="sub_td" style="float:right;">';
                            main_results += '<table id="tbl_' + msg[i].AxileName + '" name="tyre_table"  class="responsive-table"><thead><tr><th scope="col">Right</th><th scope="col">Left</th></tr></thead><tbody>';
                            right += '<td data-title="Right Tyre">';
                            left += '<td data-title="Left Tyre">';
                            for (var j = 0; j < msg[i].tyredata.length; j++) {

                                if (msg[i].tyredata[j].side == "R") {
                                    right += '<div class="right"><label>' + msg[i].tyredata[j].tyre_name + '</label></br><label>' + msg[i].tyredata[j].tyresize + '</label></br><select name="tyresno" class="form-control">' + options_val + '</select><label class="right_tyre_position_sno" style="display:none;">' + msg[i].tyredata[j].tyre_position_sno + '</label></div></br>';
                                }
                                if (msg[i].tyredata[j].side == "L") {
                                    left += '<div class="left"><label>' + msg[i].tyredata[j].tyre_name + '</label></br><label>' + msg[i].tyredata[j].tyresize + '</label></br><select name="tyresno" class="form-control">' + options_val + '</select><label class="left_tyre_position_sno" style="display:none;">' + msg[i].tyredata[j].tyre_position_sno + '</label></div></br>';
                                }
                            }
                            right += '</td>';
                            left += '</td>';
                            main_results += '<tr>' + right + '' + left + '</tr>';
                            main_results += '</tbody></table>';
                            main_results += '</div></th></tr>';
                        }
                        if (msg[i].AxileName == "Stephanie") {
                            main_results += '<tr><th scope="row" style="border: 1px solid #1d96b2;">' + (i + 1) + '</th><td style="border: 1px solid #1d96b2;" data-title="Axil Name"><label>' + msg[i].AxileName + '</label></td><td style="border: 1px solid #1d96b2;" data-title="No.of Tyres"><label>' + msg[i].nooftyresperaxle + ' </label></td><td style="display:none;"><label class="axlesno">' + msg[i].veh_typ_axel_sno + '</label></td></tr><tr class="hide_row"><th colspan="4" scope="row"><div class="sub_td" style="float:right;">';
                            for (var n = 0; n < msg[i].tyredata.length; n++) {
                                if (msg[i].tyredata[n].side == "S") {
                                    main_results += '<div class="stephanie"><input type="text" name="step_tyre_name" value="Stephanie" disabled class="form-control" placeholder="Step Tyre Name" /></br><label>' + msg[i].tyredata[n].tyresize + '</label></br><select name="tyresno" class="form-control">' + options_val + '</select><label class="step_tyre_position_sno" style="display:none;">' + msg[i].tyredata[n].tyre_position_sno + '</label></div></br>';
                                }
                            }
                            main_results += '</div></th></tr>';
                        }
                    }
                    $("#tbl_axils").append(main_results);
                    hiden();
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
    function hiden() {
        $('select[name*="tyresno"]').change(function () {
            $('select[name*="tyresno"] option').attr('disabled', false);
            $('select[name*="tyresno"]').each(function () {
                var $this = $(this);
                $('select[name*="tyresno"]').not($this).find('option').each(function () {
                    if ($(this).attr('value') == $this.val())
                        $(this).attr('disabled', true);
                });
            });
        });
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <div id="div_axilautofill" align="center">
            <table id="tbl_axils" class="responsive-table" style="width: 75%;">
                <thead>
                    <tr>
                        <th scope="col">
                            Axil
                        </th>
                        <th scope="col">
                            Axil Name
                        </th>
                        <th scope="col">
                            No Of Tyres
                        </th>
                        <th scope="col" style="display: none;">
                            Axil Sno
                        </th>
                    </tr>
                </thead>
                <tbody>
                </tbody>
            </table>
        </div>
</asp:Content>

