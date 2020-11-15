<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" EnableEventValidation="false"
    CodeFile="Vehiceldocumentsexpreport.aspx.cs" Inherits="Vehiceldocumentsexpreport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="css/font-awesome.css" rel="stylesheet" type="text/css" />
    <script src="js/utility.js" type="text/javascript"></script>
    <style type="text/css">
        div.sub_td:before
        {
            float: right;
        }
        div.sub_td:after
        {
            float: none;
        }
        .row + .row > *
        {
            padding: 0px 0 0 40px;
        }
        
        .container
        {
            max-width: 100%;
        }
        .responsive-table {
  width: 100%;
  margin-bottom: 1.5em;
  border-collapse: collapse;
  border-spacing : 0;
}
@media (min-width: 48em) {
  .responsive-table {
    font-size: .9em;
  }
}
@media (min-width: 62em) {
  .responsive-table {
    font-size: 1em;
  }
}
.responsive-table thead {
  position: absolute;
  clip: rect(1px 1px 1px 1px);
  /* IE6, IE7 */
  clip: rect(1px, 1px, 1px, 1px);
  padding: 0;
  border: 0;
  height: 1px;
  width: 1px;
  overflow: hidden;
}
@media (min-width: 48em) {
  .responsive-table thead {
    position: relative;
    clip: auto;
    height: auto;
    width: auto;
    overflow: auto;
  }
}
.responsive-table thead th {
  background-color: #1d96b2;
  border: 1px solid #1d96b2;
  font-weight: normal;
  text-align: center;
  color: white;
}
.responsive-table thead th:first-of-type {
  text-align: left;
}
.responsive-table tbody{
 background-color: white;
}
.responsive-table tbody,
.responsive-table tr,
.responsive-table th,
.responsive-table td {
  display: block;
  padding: 0;
  text-align: left;
  white-space: normal;
}
@media (min-width: 48em) {
  .responsive-table tr {
    display: table-row;
  }
}
.responsive-table th,
.responsive-table td {
  padding: .5em;
  vertical-align: middle;
}
@media (min-width: 30em) {
  .responsive-table th,
  .responsive-table td {
    padding: .75em .5em;
  }
}
@media (min-width: 48em) {
  .responsive-table th,
  .responsive-table td {
    display: table-cell;
    padding: .5em;
  }
}
@media (min-width: 62em) {
  .responsive-table th,
  .responsive-table td {
    padding: .5em .5em;
  }
}
@media (min-width: 75em) {
  .responsive-table th,
  .responsive-table td {
    padding: .5em;
  }
}
.responsive-table caption {
  margin-bottom: 1em;
  font-size: 1em;
  font-weight: bold;
  text-align: center;
}
@media (min-width: 48em) {
  .responsive-table caption {
    font-size: 1.5em;
  }
}
.responsive-table tfoot {
  font-size: .8em;
  font-style: italic;
}
@media (min-width: 62em) {
  .responsive-table tfoot {
    font-size: .9em;
  }
}
@media (min-width: 48em) {
  .responsive-table tbody {
    display: table-row-group;
  }
}
.responsive-table tbody tr {
  margin-bottom: 1em;
  border: 2px solid #1d96b2;
}
@media (min-width: 48em) {
  .responsive-table tbody tr {
    display: table-row;
    border-width: 1px;
  }
}
.responsive-table tbody tr:last-of-type {
  margin-bottom: 0;
}
@media (min-width: 48em) {
  .responsive-table tbody tr:nth-of-type(even) {
    background-color: rgba(94, 93, 82, 0.1);
  }
}
.responsive-table tbody th[scope="row"] {
  background-color: #1d96b2;
  color: white;
}
@media (min-width: 48em) {
  .responsive-table tbody th[scope="row"] {
    background-color: transparent;
    color: #5e5d52;
    text-align: left;
  }
}
.responsive-table tbody td {
  text-align: right;
}
@media (min-width: 30em) {
  .responsive-table tbody td {
    border-bottom: 1px solid #1d96b2;
  }
}
@media (min-width: 48em) {
  .responsive-table tbody td {
    text-align: center;
  }
}
.responsive-table tbody td[data-type=currency] {
  text-align: right;
}
.responsive-table tbody td[data-title]:before {
  content: attr(data-title);
  float: left;
  font-size: .8em;
  color: rgba(94, 93, 82, 0.75);
}
@media (min-width: 30em) {
  .responsive-table tbody td[data-title]:before {
    font-size: .9em;
  }
}
@media (min-width: 48em) {
  .responsive-table tbody td[data-title]:before {
    content: none;
  }
}
    </style>
    <script type="text/javascript">
        $(function () {
            getVehciledocumentsdata();
        });
        function CallPrint(strid) {
            var divToPrint = document.getElementById(strid);
            var newWin = window.open('', 'Print-Window', 'width=400,height=400,top=100,left=100');
            newWin.document.open();
            newWin.document.write('<html><body   onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
            newWin.document.close();
        }

        function getVehciledocumentsdata() {
            var table = document.getElementById("tbl_review_list_list");
            for (var i = table.rows.length - 1; i > 0; i--) {
                table.deleteRow(i);
            }
            var data = { 'op': 'get_Vehciledocuments_data' };
            var s = function (msg) {
                if (msg) {
                    for (var i = 0; i < msg.length; i++) {
                        var tablerowcnt = document.getElementById("tbl_review_list_list").rows.length;
                        var subrows = "";
                        for (var j = 0; j < msg[i].SubVehiclelist.length; j++) {
                            subrows += '<tr><td data-title="nextcallreq_time">' + msg[i].SubVehiclelist[j].permit_expdate + '</td><td data-title="description">' + msg[i].SubVehiclelist[j].pol_expdate + '</td><td data-title="lead_entry_sno" >' + msg[i].SubVehiclelist[j].ins_expdate + '</td><td data-title="lead_entry_sno" >' + msg[i].SubVehiclelist[j].fitness_expdate + '</td><td data-title="lead_entry_sno" >' + msg[i].SubVehiclelist[j].roadtax_expdate + '</td><td data-title="lead_entry_sno" >' + msg[i].SubVehiclelist[j].state_permit_expdate + '</td><td data-title="lead_entry_sno" >' + msg[i].SubVehiclelist[j].state_roadtax_expdate + '</td></tr>';
                        }
                        $('#tbl_review_list_list').append('<tr><td data-title="categorysno">' + msg[i].vehicleno + '</td><th scope="Category Name">' + msg[i].make + '</th><td data-title="IsTransport">' + msg[i].type + '</td><td data-title="IsTransport">' + msg[i].model + '</td><td data-title="Application Status">' + msg[i].capacity + '</td><td data-title="Application Status">' + msg[i].insurance + '</td><td data-title="Status">' + msg[i].pollution + '</td><td data-title="Status" >' + msg[i].fitness + '</td><td data-title="section_sno">' + msg[i].roadtax + '</td><td data-title="grade_sno" >' + msg[i].permit + '</td><td></td><td><i class="fa fa-plus-circle fa-2x elementlbl" style="width:30px;height:30px" onclick="expandthis(this);"></i></td></tr><tr style="display:none;"><td colspan="15"><table class="responsive-table"><thead><tr><th scope="col" > Permit Exp Date</th><th scope="col" name=note">Pollution Exp Date</th><th scope="col" name=note">Insurance Exp Date</th><th scope="col" name=note">Fiteness Exp Date</th><th scope="col" name=note">RoadTax Exp Date</th><th scope="col" name=note">State Permit</th><th scope="col" name=note">State RoadTax </th></tr></tr></thead><tbody>' + subrows + '</tbody></table></td></tr>');
                    }
                }
                else {
                    document.location = "Default.aspx";
                }
            }
            var e = function (x, h, e) {
                alert(e.toString());
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function expandthis(thisid) {
            if (thisid.className == "fa fa-plus-circle fa-2x elementlbl") {
                thisid.setAttribute("class", "fa fa-minus-circle fa-2x elementlbl");

                $(thisid).closest('tr').next().removeAttr("style");
            }

            else if (thisid.className == "fa fa-minus-circle fa-2x elementlbl") {
                thisid.setAttribute("class", "fa fa-plus-circle fa-2x elementlbl");
                $(thisid).closest('tr').next().css('display', 'none');
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <section class="content-header">
        <h1>
             documents expire Details<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#"> documents expire Details</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i> Vehicle documents expire Details
                </h3>
            </div>
            <div class="box-body">
    <table id="tbl_review_list_list" class="responsive-table">
        <thead>
            <tr>
                <th scope="col">
                    Vehicle No
                </th>
                <th scope="col">
                    Vehicle Type
                </th>
                <th scope="col">
                    Vehicle Make
                </th>
                <th scope="col">
                    Model
                </th>
                <th scope="col">
                    Capacity
                </th>
                <th scope="col">
                    Insurannce Exp Date
                </th>
                <th scope="col">
                    Pollution Exp Date
                </th>
                <th scope="col">
                    Fitness Exp Date
                </th>
                <th scope="col">
                    RoadTax Exp Date
                </th>
                <th scope="col">
                    Permitt Exp Date
                </th>
               <th scope="col"></th>
               <th scope="col"></th>
            </tr>
        </thead>
        <tbody>
        </tbody>
    </table>
    </div>
    </div>
    </section>
</asp:Content>
