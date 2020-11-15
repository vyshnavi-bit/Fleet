<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" CodeFile="EmployInformation.aspx.cs" Inherits="EmployInformation" %>

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
        get_emp_names();
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
    function get_emp_names() {
        var data = { 'op': 'get_all_employee_data' }; 
        var s = function (msg) {
            if (msg) {
                fillemployees(msg);
            }
            else {
            }
        };
        var e = function (x, h, e) {
        }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
        callHandler(data, s, e);
    }

    function fillemployees(msg) {
        var data = document.getElementById('slct_empname');
        var length = data.options.length;
        document.getElementById('slct_empname').options.length = null;
        var opt = document.createElement('option');
        opt.innerHTML = "Select Employee";
        opt.value = "Select Employee";
        opt.setAttribute("selected", "selected");
        opt.setAttribute("disabled", "disabled");
        opt.setAttribute("class", "dispalynone");
        data.appendChild(opt);
        for (var i = 0; i < msg.length; i++) {
            if (msg[i].employname != null) {
                var option = document.createElement('option');
                option.innerHTML = msg[i].employname;
                option.value = msg[i].emp_sno;
                data.appendChild(option);
            }
        }
    }
    function get_emp_information() {
        $('#Button2').css('display', 'block');
        var empname = document.getElementById('slct_empname').value;
        if (empname == "" || empname == "Select Employee") {
            alert("Please Select Employee");
            return;
        }
        var data = { 'op': 'get_emp_information', 'empname': empname };
        var s = function (msg) {
            if (msg) {
                if (msg.length > 0) {
                    $('#divPrint').css('display', 'block');
                    document.getElementById('spnemp').innerHTML = msg[0].DriverName;
                    document.getElementById('spncorporate').innerHTML = msg[0].EmpID;
                    document.getElementById('spnfather').innerHTML = msg[0].FathersName;
                    document.getElementById('spnqualif').innerHTML = msg[0].Qualification;
                    document.getElementById('spngender').innerHTML = msg[0].Gender;
                    document.getElementById('spndob').innerHTML = msg[0].DOB;
                    document.getElementById('spnmrgstatus').innerHTML = msg[0].Status;
                    document.getElementById('spnnationality').innerHTML = msg[0].Nationality;
                    document.getElementById('spnmobileno').innerHTML = msg[0].MobNo;
                    document.getElementById('spndoj').innerHTML = msg[0].Doj;
                    document.getElementById('spntype').innerHTML = msg[0].emptype;
                    document.getElementById('spnaddress').innerHTML = msg[0].Address;
                    document.getElementById('spnbloodgrp').innerHTML = msg[0].bloodgroup;
                    document.getElementById('spnlicense').innerHTML = msg[0].LicenceNo;
                    document.getElementById('spnlicenseexp').innerHTML = msg[0].LicenceExpdate;
                    document.getElementById('spnexp').innerHTML = msg[0].Experience;
                    document.getElementById('spnbankname').innerHTML = msg[0].BankName;
                    document.getElementById('spnacno').innerHTML = msg[0].AccountNo;
                    document.getElementById('spntitle').innerHTML = msg[0].title;
                    document.getElementById('spnadd').innerHTML = msg[0].addr;
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
         Employee Information<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Reports</a></li>
            <li><a href="#">Employee information </a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Employee Information Report
                </h3>
            </div>
            <div class="box-body">
                <div style="padding: 20px;">
                    <div id="tyre_div">
                        <table align="center">
                            <tr>
                                 <td style="width: 5px;">
                                <td>
                                    <label>
                                       Employee Name<span style="color: red;">*</span></label>
                                    <select id="slct_empname" class="form-control" style="min-width: 150px;">
                                         </td>
                                         <td style="width: 5px;">
                                    <td>
                                        <input id='btn_empinfo' type="button" class="btn btn-primary" name="submit" value='Generate'
                                            onclick="get_emp_information()" />
                                    </td>
                                </tr>
                            </table>
                    </div>
                   
                        </div>
                           <div id="divPrint" style="display: none;">
                <div>
                    <div style="width: 13%; float: left;">
                        <img src="Images/Vyshnavilogo.png" alt="Vyshnavi" width="100px" height="72px" />
                        <br />
                    </div>
                    <div>
                        <div align="center">
                                    <span id="spntitle"  style="font-size: 20px;font-weight:bold;color:#0252aa"></span>
                                    <br />
                                    <span id="spnadd" style="font-size: 12px;font-weight:bold;color:#0252aa"></span>  
                                    <br />
                                </div>
                        <div style="width:33%;">
                        <span id="spnAddress" style="font-size: 14px;"></span>
                        </div>
                    </div>
                    <div align="center" style="text-decoration: underline;">
                        <span style="font-size: 16px; font-weight: bold;">PERSONAL INFORMATION</span>
                    </div>
                    <div style="width: 100%;">
                    <div align="center">
                    <label style="font-size: 15px;font-weight:bold;color:#0252aa">
                                        Employee Name :</label>
                                    <span id="spnemp" style="color:red;font-weight:bold"></span>
                                </div>
                                <br />
                        <table style="width: 100%;">
                            <tr>
                                <td>
                                <label style="font-size: 12px;">
                                        Corporate ID :</label>
                                        </td>
                                        <td>
                                    <span id="spncorporate" style="color:red"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                 <label style="font-size: 12px;">
                                       Fathers Name :</label>
                                    </td>
                                    <td>
                                    <span id="spnfather" style="color:red"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    <label style="font-size: 12px;">
                                        Qualification:</label>
                                        </td>
                                        <td>
                                    <span id="spnqualif" style="font-size: 14px;color:red"></span>
                                   </td>
                                </tr>
                                <tr>
                                <td>
                                    <label style="font-size: 12px;">
                                        Gender :</label>
                                    </td>
                                    <td>
                                    <span id="spngender" style="font-size: 14px;color:red"></span>
                                  </td>
                                  </tr>
                                  <tr>
                                  <td>
                                    <label style="font-size: 12px;">
                                        DOB :</label>
                                    </td>
                                    <td>
                                    <span id="spndob" style="font-size: 14px;color:red"></span>
                                  </td>
                                  <tr>
                                  <td>
                                    <label style="font-size: 12px;">
                                       Marital Status :</label>
                                       </td>
                                    <td>
                                    <span id="spnmrgstatus" style="font-size: 14px;color:red"></span>
                                    </td>
                                     </tr>
                                     <tr>
                                     <td>
                                    <label style="font-size: 12px;">
                                       Nationality	 :</label>
                                    </td>
                                    <td>
                                    <span id="spnnationality" style="font-size: 14px;color:red"></span>
                                    </td>
                                    </tr>
                                    <tr>
                                    <td>
                                    <label style="font-size: 12px;">
                                        Mobile No :</label>
                                    </td>
                                    <td>
                                    <span id="spnmobileno" style="font-size: 14px;color:red"></span>
                                   </td>
                                    </tr>
                                    <tr>
                                   <td>
                                    <label style="font-size: 12px;">
                                        Type :</label>
                                    </td>
                                    <td>
                                    <span id="spntype" style="font-size: 14px;color:red"></span>
                                    </td>
                                     </tr>
                                     <tr>
                                     <td>
                                    <label style="font-size: 12px;">
                                       DOJ :</label>
                                       </td>
                                       <td>
                                    <span id="spndoj" style="font-size: 14px;color:red"></span>
                                  </td>
                                     </tr>
                                     <tr>
                                     <td>
                                    <label style="font-size: 12px;">
                                       Address :</label>
                                    </td>
                                    <td>
                                    <span id="spnaddress" style="font-size: 14px;color:red"></span>
                                   </td>
                                     </tr>
                                     <tr>
                                     <td>
                                    <label style="font-size: 12px;">
                                        Blood group :</label>
                                    </td>
                                    <td>
                                    <span id="spnbloodgrp" style="font-size: 14px;color:red"></span>
                                    </td>
                                    </tr>
                                    <tr>
                                    <td>
                                    <label style="font-size: 12px;">
                                        Licensedetails:DL.NO :</label>
                                    </td>
                                    <td>
                                    <span id="spnlicense" style="font-size: 14px;color:red"></span>
                                   </td>
                                   </tr>
                                   <tr>
                                   <td>
                                    <label style="font-size: 12px;">
                                        License Exp Date:</label>
                                    </td>
                                    <td>
                                    <span id="spnlicenseexp" style="font-size: 14px;color:red"></span>
                                   </td>
                                   </tr>
                                   <tr>
                                 <td>
                                    <label style="font-size: 12px;">
                                       Experience:</label>
                                    </td>
                                    <td>
                                    <span id="spnexp" style="font-size: 14px;color:red"></span>
                                    </td>
                                     </tr>
                                     <tr>
                                     <td>
                                    <label style="font-size: 12px;">
                                       Bank Name:</label>
                                       </td>
                                    <td>
                                    <span id="spnbankname" style="font-size: 14px;color:red"></span>
                                    </td>
                                    </tr>
                                    <tr>
                                    <td>
                                    <label style="font-size: 12px;">
                                      Account Number	:</label>
                                    </td>
                                    <td>
                                    <span id="spnacno" style="font-size: 14px;color:red"></span>
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

