<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" CodeFile="TrailEdit.aspx.cs" Inherits="TrailEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="js/utility.js" type="text/javascript"></script>

<script type="text/javascript">


    $(function () {
        fill_tripsheet_no();
    });

    function fill_tripsheet_no() {
        var data = { 'op': 'get_all_tripsheets' };
        var s = function (msg) {
            if (msg) {
                if (msg.length > 0) {
                    fill_trip_sheet_no(msg);
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
    function fill_trip_sheet_no(msg) {
        assignedtrips = msg;
        var data = document.getElementById('cmb_Tripsheets');
        var length = data.options.length;
        document.getElementById('cmb_Tripsheets').options.length = null;

        var opt = document.createElement('option');
        opt.innerHTML = "Select Trip";
        opt.value = "Select Trip";
        opt.setAttribute("selected", "selected");
        opt.setAttribute("disabled", "disabled");
        opt.setAttribute("class", "dispalynone");
        data.appendChild(opt);

        for (var i = 0; i < msg.length; i++) {
            if (msg[i].sno != null) {
                var option = document.createElement('option');
                option.innerHTML = msg[i].tripsheetno;
                option.value = msg[i].sno;
                data.appendChild(option);

            }

        }
    }

    function get_tollgates() {
        var tripid = document.getElementById('cmb_Tripsheets').value;
        var data = { 'op': 'get_tollgates', 'tripid': tripid };
        var s = function (msg) {
            if (msg) {
                if (msg.length > 0) {
                    
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

<div style="width: 100%; height: 100%;">
        <div id="second_div" style=" padding: 20px;">
        
            <div class="row">
                <div class="form-group">
                    <label>
                        TripSheet</label>
                    <select id="cmb_Tripsheets" class="form-control" style="min-width: 200px;" onchange="get_tollgates()">
                    </select>
                </div>
            </div>


</div>
</div>

</asp:Content>

