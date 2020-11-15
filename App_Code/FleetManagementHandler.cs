using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using MySql.Data.MySqlClient;
using System.Data;
using System.Web.Services;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

/// <summary>
/// Summary description for FleetManagementHandler
/// </summary>
public class FleetManagementHandler : IHttpHandler, IRequiresSessionState
{
    MySqlCommand cmd;
    VehicleDBMgr vdm = new VehicleDBMgr();
    public FleetManagementHandler()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public bool IsReusable
    {
        get { return true; }
    }
    private static string GetJson(object obj)
    {
        JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
        return jsonSerializer.Serialize(obj);
    }
    public class routesave
    {
        public string routename { get; set; }
        public string status { get; set; }
        public string btnval { get; set; }
        public string updatesno { get; set; }
        public string ledgername { get; set; }
        public string mobileno { get; set; }
    }
    public class getroutescls
    {
        public string routename { get; set; }
        public string status { get; set; }
        public string sno { get; set; }
        public string ledgername { get; set; }
        public string mobileno { get; set; }
    }
    public class getstarttripperamtrscls
    {
        public List<vehiclescls> vehicles { get; set; }
        public List<driverscls> drivers { get; set; }
        public List<routescls> routes { get; set; }
    }
    public class routescls
    {
        public string routename { get; set; }
        public string routesno { get; set; }
    }
    public class driverscls
    {
        public string drivername { get; set; }
        public string driversno { get; set; }
    }
    public class vehiclescls
    {
        public string vehiclenum { get; set; }
        public string vehiclesno { get; set; }
    }
    public class starttripsave
    {
        public string vehicle { get; set; }
        public string driver { get; set; }
        public string route { get; set; }
        public string date { get; set; }
        public string startodometer { get; set; }
        public string startfuelval { get; set; }
        public string startengineworkinghrs { get; set; }
        public string Status { get; set; }
    }
    public class getassignedtripscls
    {
        public string tripsno { get; set; }
        public string vehicle { get; set; }
        public string driver { get; set; }
        public string route { get; set; }
        public string date { get; set; }
        public string startodometer { get; set; }
        public string startfuelval { get; set; }
        public string startengineworkinghrs { get; set; }
        public string user { get; set; }
    }
    public class save_endtripcls
    {
        public string tripsno { get; set; }
        public string endtripdate { get; set; }
        public string endodometer { get; set; }
        public string intrmdtfuelfllingval { get; set; }
        public string endfuelval { get; set; }
        public string endenginewrknghrs { get; set; }
        public string remarks { get; set; }
        public string Status { get; set; }
    }
    class GetJsonData
    {
        public string op { set; get; }
    }
    //  [WebMethod(Description="Delete Template",BufferResponse=false)]
    public void ProcessRequest(HttpContext context)
    {
        try
        {
            string operation = context.Request["op"];
            switch (operation)
            {

                case "get_state_details":
                    get_state_details(context);
                    break;
                case "getcompanydetails":
                    getcompanydetails(context);
                    break;
                case "for_save_edit_vendor":
                    for_save_edit_vendor(context);
                    break;
                case "get_vendor_details":
                    get_vendor_details(context);
                    break;
                case "get_Edit_triplogs_data":
                    get_Edit_triplogs_data(context);
                    break;
                case "get_Battery_details":
                    get_Battery_details(context);
                    break;
                case "getroutes":
                    getroutes(context);
                    break;
                //case "for_save_edit_CostCenter":
                //    for_save_edit_CostCenter(context);
                //    break;
                //case "get_CC_details":
                //    get_CC_details(context);
                //    break;
                case "for_save_edit_PArtGroup":
                    for_save_edit_PArtGroup(context);
                    break;
                case "get_all_partgroups":
                    get_all_partgroups(context);
                    break;
                case "get_Mini_Master_data":
                    get_Mini_Master_data(context);
                    break;
                case "for_save_edit_MinMaster":
                    for_save_edit_MinMaster(context);
                    break;
                case "retrivestarttripperameters":
                    retrivestarttripperameters(context);
                    break;
                //case "getassignedtrips":
                //    getassignedtrips(context);
                //    break;
                case "save_partname":
                    save_partname(context);
                    break;
                case "get_all_partname_data":
                    get_all_partname_data(context);
                    break;
                case "get_Part_NAme_data":
                    get_Part_NAme_data(context);
                    break;
                case "save_part_number":
                    save_part_number(context);
                    break;
                case "get_all_part_number_data":
                    get_all_part_number_data(context);
                    break;
                case "save_cmpnydetails":
                    save_cmpnydetails(context);
                    break;
                case "save_Branch_data":
                    save_Branch_data(context);
                    break;
                case "get_all_BranchName_data":
                    get_all_BranchName_data(context);
                    break;
                case "get_all_axils":
                    get_all_axils(context);
                    break;
                case "save_vehicle_master":
                    save_vehicle_master(context);
                    break;
                case "get_all_veh_master_data":
                    get_all_veh_master_data(context);
                    break;
                case "get_all_veh_info":
                    get_all_veh_info(context);
                    break;
                case "for_save_edit_axilmaster":
                    for_save_edit_axilmaster(context);
                    break;
                case "get_Part_number_data":
                    get_Part_number_data(context);
                    break;
                case "Maintanance_save_start":
                    List<dis_items> Maintanance_row_detail = new List<dis_items>();
                    context.Session["load_Next_Maintanance"] = Maintanance_row_detail;
                    string loadmsg = "success";
                    string loadresponse = GetJson(loadmsg);
                    context.Response.Write(loadresponse);
                    break;
                case "save_edit_Maintanance":
                    save_edit_Maintanance(context);
                    break;
                case "get_all_Maintenance":
                    get_all_Maintenance(context);
                    break;
                case "for_save_edit_employee":
                    for_save_edit_employee(context);
                    break;
                case "get_all_employee_data":
                    get_all_employee_data(context);
                    break;
                case "get_Recurring_data":
                    get_Recurring_data(context);
                    break;
                case "Recurrence_save_start":
                    List<dis_items> Recurrence_row_detail = new List<dis_items>();
                    context.Session["load_Next_Recurrence"] = Recurrence_row_detail;
                    string loadmsg2 = "success";
                    string loadresponse2 = GetJson(loadmsg2);
                    context.Response.Write(loadresponse2);
                    break;
                case "save_edit_Recurrence":
                    save_edit_Recurrence(context);
                    break;
                case "get_rackansstock_data":
                    get_rackansstock_data(context);
                    break;
                case "get_rackansstockandunitcost_data":
                    get_rackansstockandunitcost_data(context);
                    break;
                case "Inward_save_start":
                    List<Inward_items> Inward_row_detail = new List<Inward_items>();
                    context.Session["load_Next_Inward"] = Inward_row_detail;
                    string loadmsg3 = "success";
                    string loadresponse3 = GetJson(loadmsg3);
                    context.Response.Write(loadresponse3);
                    break;
                case "save_edit_Inward":
                    save_edit_Inward(context);
                    break;
                case "get_inward_no":
                    get_inward_no(context);
                    break;
                case "get_inward_data":
                    get_inward_data(context);
                    break;
                case "get_all_tyre_data":
                    get_all_tyre_data(context);
                    break;
                //case "get_workorder_data":
                //    get_workorder_data(context);
                //    break;
                //case "get_total_workorder_data":
                //    get_total_workorder_data(context);
                //    break;
                case "getalldataforissuetyre":
                    getalldataforissuetyre(context);
                    break;
                case "for_save_edit_removedtyre":
                    for_save_edit_removedtyre(context);
                    break;
                case "get_issued_tyres":
                    get_issued_tyres(context);
                    break;
                //case "get_workorder_data_foroutward":
                //    get_workorder_data_foroutward(context);
                //    break;
                case "Axils_save_start":
                    List<Axils_items> Axils_row_detail = new List<Axils_items>();
                    context.Session["load_Next_Axils"] = Axils_row_detail;
                    string loadmsg4 = "success";
                    string loadresponse4 = GetJson(loadmsg4);
                    context.Response.Write(loadresponse4);
                    break;
                case "save_edit_Axils":
                    save_edit_Axils(context);
                    break;
                case "get_only_axilmaster":
                    get_only_axilmaster(context);
                    break;
                case "get_all_data_Axils":
                    get_all_data_Axils(context);
                    break;
                case "get_allin_no":
                    get_allin_no(context);
                    break;
                case "GetEditTripSheetValues":
                    GetEditTripSheetValues(context);
                    break;
                case "GetStockQty":
                    GetStockQty(context);
                    break;
                case "Outward_save_start":
                    List<Outward_items> Outward_row_detail = new List<Outward_items>();
                    context.Session["load_Next_Outward"] = Outward_row_detail;
                    string loadmsg5 = "success";
                    string loadresponse5 = GetJson(loadmsg5);
                    context.Response.Write(loadresponse5);
                    break;
                //case "save_edit_Outward":
                //    save_edit_Outward(context);
                //    break;
                case "get_only_tyre_data":
                    get_only_tyre_data(context);
                    break;
                case "get_axils_for_vehicles":
                    get_axils_for_vehicles(context);
                    break;
                case "get_tyre_using_position":
                    get_tyre_using_position(context);
                    break;
                case "for_save_edit_fittedtyre":
                    for_save_edit_fittedtyre(context);
                    break;
                case "edit_save_location":
                    edit_save_location(context);
                    break;
                case "retrive_all_location":
                    retrive_all_location(context);
                    break;
                case "generate_locations":
                    generate_locations(context);
                    break;
                case "Distances_save_start":
                    List<Distance_items> Distance_row_detail = new List<Distance_items>();
                    context.Session["load_Next_Distance"] = Distance_row_detail;
                    string loadmsg6 = "success";
                    string loadresponse6 = GetJson(loadmsg6);
                    context.Response.Write(loadresponse6);
                    break;
                case "save_edit_Distances":
                    save_edit_Distances(context);
                    break;
                case "get_kms_from_to":
                    get_kms_from_to(context);
                    break;
                case "get_inward_only":
                    get_inward_only(context);
                    break;
                case "getremaining_veh_data":
                    getremaining_veh_data(context);
                    break;
                case "btnTripSheetSaveClick":
                    btnTripSheetSaveClick(context);
                    break;
                case "btnTripendSaveClick":
                    btnTripendSaveClick(context);
                    break;
                case "TripendSaveClick":
                    TripendSaveClick(context);
                    break;
                case "get_driver_and_helper":
                    get_driver_and_helper(context);
                    break;
                case "fill_tripsheet_no":
                    fill_tripsheet_no(context);
                    break;
                case "gettripalldetails":
                    gettripalldetails(context);
                    break;
                case "gettripjobcards":
                    gettripjobcards(context);
                    break;
                case "insert_Distances_fromtrip":
                    insert_Distances_fromtrip(context);
                    break;
                case "TripLog_save_start":
                    List<TripLog_items> TripLog_row_detail = new List<TripLog_items>();
                    context.Session["load_Next_TripLog"] = TripLog_row_detail;
                    string loadmsg7 = "success";
                    string loadresponse7 = GetJson(loadmsg7);
                    context.Response.Write(loadresponse7);
                    break;
                case "save_edit_TripLog":
                    save_edit_TripLog(context);
                    break;
                case "GetAssignTripSheets":
                    GetAssignTripSheets(context);
                    break;
                case "GetBtnViewJobcardclick":
                    GetBtnViewJobcardclick(context);
                    break;
                case "btnTripSheetPrintClick":
                    btnTripSheetPrintClick(context);
                    break;
                case "UpdateJobcardbtnclick":
                    UpdateJobcardbtnclick(context);
                    break;
                case "btnstockclosing":
                    btnstockclosing(context);
                    break;
                case "btnsaveInwardstock":
                    btnsaveInwardstock(context);
                    break;                    
                case "Tyres_save_start":
                    List<Tyres_items> Tyres_row_detail = new List<Tyres_items>();
                    context.Session["load_Next_Tyres"] = Tyres_row_detail;
                    string loadmsg8 = "success";
                    string loadresponse8 = GetJson(loadmsg8);
                    context.Response.Write(loadresponse8);
                    break;
                case "save_edit_Tyres":
                    save_edit_Tyres(context);
                    break;
                case "get_tyres_new":
                    get_tyres_new(context);
                    break;
                case "save_edit_TyresRethread":
                    save_edit_TyresRethread(context);
                    break;
                case "save_edit_TyresTransfer":
                    save_edit_TyresTransfer(context);
                    break;
                case "get_transfered_tyres_thisbrn":
                    get_transfered_tyres_thisbrn(context);
                    break;
                case "accept_reject_tyre":
                    accept_reject_tyre(context);
                    break;
                case "get_only_vehicles_data":
                    get_only_vehicles_data(context);
                    break;
                case "save_edit_TyresInspection":
                    save_edit_TyresInspection(context);
                    break;
                case "get_all_axil_names":
                    get_all_axil_names(context);
                    break;
                case "get_filled_data_tyres_vehmstr":
                    get_filled_data_tyres_vehmstr(context);
                    break;
                case "get_axil_odometer_using_position":
                    get_axil_odometer_using_position(context);
                    break;
                case "getalldataforissuetyre_tyres":
                    getalldataforissuetyre_tyres(context);
                    break;
                case "keep_sessionalive":
                    keep_sessionalive(context);
                    break;
                case "getallveh_nos_frommoduleConfig":
                    getallveh_nos_frommoduleConfig(context);
                    break;
                case "get_all_veh_master_data_notrips":
                    get_all_veh_master_data_notrips(context);
                    break;
                case "get_tyres_for_vehicle":
                    get_tyres_for_vehicle(context);
                    break;
                case "get_only_tyres_first_data":
                    get_only_tyres_first_data(context);
                    break;
                case "get_remaining_tyres_data":
                    get_remaining_tyres_data(context);
                    break;
                case "check_triplog_save":
                    check_triplog_save(context);
                    break;
                case "get_all_tripsheets":
                    get_all_tripsheets(context);
                    break;
                case "get_tollgates":
                    get_tollgates(context);
                    break;
                case "save_edit_Tyres_new":
                    save_edit_Tyres_new(context);
                    break;
                case "get_diesel_cost":
                    get_diesel_cost(context);
                    break;
                case "get_triplogs_data":
                    get_triplogs_data(context);
                    break;
                case "btnUpdatePuffRentclick":
                    btnUpdatePuffRentclick(context);
                    break;
                case "GetVehiclerents":
                    GetVehiclerents(context);
                    break;
                case "btnSalaryadvanceClick":
                    btnSalaryadvanceClick(context);
                    break;
                case "get_all_vehhilcles":
                    get_all_vehhilcles(context);
                    break;
                case "btnVehicleTermLoanEntrySaveClick":
                    btnVehicleTermLoanEntrySaveClick(context);
                    break;
                case "update_sales_data":
                    update_sales_data(context);
                    break;
                case "get_insurancecompany":
                    get_insurancecompany(context);
                    break;
                case "btnsavebatterymasterClick":
                    btnsavebatterymasterClick(context);
                    break;
                case "get_route_names":
                    get_route_names(context);
                    break;
                case "btnRouteAssignClick":
                    btnRouteAssignClick(context);
                    break;
                case "btnEditTripSheetSaveClick":
                    btnEditTripSheetSaveClick(context);
                    break;
                case "save_Veh_Servicing_Save_click":
                    save_Veh_Servicing_Save_click(context);
                    break;
                case "GetVehicleAlertDeatails":
                    GetVehicleAlertDeatails(context);
                    break;
                case "get_vehicleServicealerts_details":
                    get_vehicleServicealerts_details(context);
                    break;
                case "get_vehicleservice_update_kms_details":
                    get_vehicleservice_update_kms_details(context);
                    break;

                case "get_vehiclemake":
                    get_vehiclemake(context);
                    break;
                case "save_personal_info":
                    save_personal_info(context);
                    break;
                case "get_all_personal_data":
                    get_all_personal_data(context);
                    break;
                case "btnVehicle_tools_issue_return_saveclick":
                    btnVehicle_tools_issue_return_saveclick(context);
                    break;
                case "get_Vehciledocuments_data":
                    get_Vehciledocuments_data(context);
                    break;
                case "head_master_saveclick":
                    head_master_saveclick(context);
                    break;
                case "get_head_master_List":
                    get_head_master_List(context);
                    break;
                case "GetMaintenance_list":
                    GetMaintenance_list(context);
                    break;
                case "btnMaintenancePrintClick":
                    btnMaintenancePrintClick(context);
                    break;
                case "GetSubmaintenses":
                    GetSubmaintenses(context);
                    break;
                case "GetVehicleExpDeatails":
                    GetVehicleExpDeatails(context);
                    break;
                case "GetVehicle_MileageDeatails":
                    GetVehicle_MileageDeatails(context);
                    break;
                case "Getavailable_Vehicle_Deatails":
                    Getavailable_Vehicle_Deatails(context);
                    break;
                case "Getrunning_Vehicle_Deatails":
                    Getrunning_Vehicle_Deatails(context);
                    break;
                case "GetSubPaybleValues":
                    GetSubPaybleValues(context);
                    break;

                //////////.....................Chart Reports.......................////////////////

                case "GetVehicleWisePerformanceclick":
                    GetVehicleWisePerformanceclick(context);
                    break;
                case "GetIdle_Vehicle_Deatails":
                    GetIdle_Vehicle_Deatails(context);
                    break;
                case "testmail":
                    testmail(context);
                    break;
                case "GetVehicle_service_deatails":
                    GetVehicle_service_deatails(context);
                    break;
                case "save_Veh_Servicing_update_kms_click":
                    save_Veh_Servicing_update_kms_click(context);
                    break;
                case "Vehicle_pic_files_upload":
                    Vehicle_pic_files_upload(context);
                    break;
                case "save_Vehicle_Document_Info":
                    save_Vehicle_Document_Info(context);
                    break;
                case "getvehicle_Uploaded_Documents":
                    getvehicle_Uploaded_Documents(context);
                    break;
                case "btnstart_tripsheetclick":
                    btnstart_tripsheetclick(context);
                    break;
                case "getall_TermLoanDetails":
                    getall_TermLoanDetails(context);
                    break;
                case "get_trip_startingodometer":
                    get_trip_startingodometer(context);
                    break;
                case "emp_profile_pic_files_upload":
                    emp_profile_pic_files_upload(context);
                    break;
                case "save_employeeDocument":
                    save_employeeDocument(context);
                    break;
                case "getemployee_Uploaded_Documents":
                    getemployee_Uploaded_Documents(context);
                    break;

                case "get_tyresum_report":
                    get_tyresum_report(context);
                    break;
                case "get_tyre_report":
                    get_tyre_report(context);
                    break;
                case "get_tyres_number":
                    get_tyres_number(context);
                    break;
                case "generate_sales_data":
                    generate_sales_data(context);
                    break;
                case "get_veh_handover_data":
                    get_veh_handover_data(context);
                    break;
                case "save_Vehicle_handover_Info":
                    save_Vehicle_handover_Info(context);
                    break;
                case "vehicle_handover_pics_upload":
                    vehicle_handover_pics_upload(context);
                    break;
                case "get_handover_details":
                    get_handover_details(context);
                    break;
                case "getvehicle_Uploaded_photos":
                    getvehicle_Uploaded_photos(context);
                    break;
                case "get_handover_print_details":
                    get_handover_print_details(context);
                    break;
                case "get_vehicle_tools":
                    get_vehicle_tools(context);
                    break;
                case "get_vehicle_tyres":
                    get_vehicle_tyres(context);
                    break;
                case "get_handover_sno":
                    get_handover_sno(context);
                    break;
                case "get_emp_information":
                    get_emp_information(context);
                    break;
                case "get_termloan_transaction_details":
                    get_termloan_transaction_details(context);
                    break;
                    
                case "termlons_save_start":
                    List<termlonans> termlon_row_detail = new List<termlonans>();
                    context.Session["load_Next_row"] = termlon_row_detail;
                    string loadmsg9 = "success";
                    string loadresponse9 = GetJson(loadmsg9);
                    context.Response.Write(loadresponse9);
                    break;

                case "termloans_save_edit_data":
                    termloans_save_edit_data(context);
                    break;
                case "get_employee_details":
                    get_employee_details(context);
                    break;
                case "btn_getlogininfoemployee_details":
                    btn_getlogininfoemployee_details(context);
                    break;
                case "getDailyinfo":
                    getDailyinfo(context);
                    break;
                case "GetVehicleModuleConfig":
                    GetVehicleModuleConfig(context);
                    break;
                case "btnUpdateVehicleRate":
                    btnUpdateVehicleRate(context);
                    break;
                case "get_BillingOwnersList":
                    get_BillingOwnersList(context);
                    break;
                case "get_gps_plantname":
                    get_gps_plantname(context);
                    break;
                case "GetGPsVehicleInformations":
                    GetGPsVehicleInformations(context);
                    break;
                case "btnUpdateVehicleGpsInformation":
                    btnUpdateVehicleGpsInformation(context);
                    break;






                default:
                    var req = context.Request.Params[1];
                    string[] test = req.Split(',');
                    var js = new JavaScriptSerializer();
                    if (test[0].Contains("save_route"))
                    {
                        routesave obj = js.Deserialize<routesave>(req);
                        save_route(obj, context);
                    }
                    if (test[0].Contains("save_starttrip"))
                    {
                        starttripsave obj = js.Deserialize<starttripsave>(req);
                        save_starttrip(obj, context);
                    }
                    if (test[0].Contains("save_endtrip"))
                    {
                        save_endtripcls obj = js.Deserialize<save_endtripcls>(req);
                        save_endtrip(obj, context);
                    }
                    if (test[0].Contains("cancel_trip"))
                    {
                        save_endtripcls obj = js.Deserialize<save_endtripcls>(req);
                        cancel_trip(obj, context);
                    }
                    if (test[0].Contains("Maintanance_save_RowData"))
                    {
                        dis_rowwise obj = js.Deserialize<dis_rowwise>(req);
                        Maintanance_save_RowData(obj, context);
                    }
                    if (test[0].Contains("Recurrence_save_RowData"))
                    {
                        dis_rowwise obj = js.Deserialize<dis_rowwise>(req);
                        Recurrence_save_RowData(obj, context);
                    }
                    if (test[0].Contains("Inward_save_RowData"))
                    {
                        Inward_rowwise obj = js.Deserialize<Inward_rowwise>(req);
                        Inward_save_RowData(obj, context);
                    }
                    //if (test[0].Contains("save_edit_WorkOrder"))
                    //{
                    //    workorder_save obj = js.Deserialize<workorder_save>(req);
                    //    save_edit_WorkOrder(obj, context);
                    //}
                    if (test[0].Contains("for_save_edit_tyre"))
                    {
                        get_all_tyre_datacls obj = js.Deserialize<get_all_tyre_datacls>(req);
                        for_save_edit_tyre(obj, context);
                    }
                    if (test[0].Contains("Axils_save_RowData"))
                    {
                        Axils_rowwise obj = js.Deserialize<Axils_rowwise>(req);
                        Axils_save_RowData(obj, context);
                    }
                    //if (test[0].Contains("Outward_save_RowData"))
                    //{
                    //    Outward_rowwise obj = js.Deserialize<Outward_rowwise>(req);
                    //    Outward_save_RowData(obj, context);
                    //}
                    if (test[0].Contains("Distances_save_RowData"))
                    {
                        Distance_rowwise obj = js.Deserialize<Distance_rowwise>(req);
                        Distances_save_RowData(obj, context);
                    }
                    if (test[0].Contains("jobcardsaveclick"))
                    {
                        Routes obj = js.Deserialize<Routes>(req);
                        jobcardsaveclick(obj, context);
                    }
                    if (test[0].Contains("TripLog_save_RowData"))
                    {
                        TripLog_rowwise obj = js.Deserialize<TripLog_rowwise>(req);
                        TripLog_save_RowData(obj, context);
                    }
                    if (test[0].Contains("Tyres_save_RowData"))
                    {
                        Tyres_rowwise obj = js.Deserialize<Tyres_rowwise>(req);
                        Tyres_save_RowData(obj, context);
                    }
                    if (test[0].Contains("BtnRaiseVehicleExpendature_saveclick"))
                    {
                        VehicleExpendaturecls obj = js.Deserialize<VehicleExpendaturecls>(req);
                        BtnRaiseVehicleExpendature_saveclick(obj, context);
                    }
                    //if (test[0].Contains("Triplogs_edit_Click"))
                    //{
                    //    TripLog_rowwise obj = js.Deserialize<TripLog_rowwise>(req);
                    //    Triplogs_edit_Click(obj, context);
                    //}
                    if (test[0].Contains("termloans_save_RowData"))
                    {
                        termloans_rowwsie obj = js.Deserialize<termloans_rowwsie>(req);
                        termloans_save_RowData(obj, context);
                    }
                    var jsonString = String.Empty;
                    context.Request.InputStream.Position = 0;
                    using (var inputStream = new StreamReader(context.Request.InputStream))
                    {
                        jsonString = HttpUtility.UrlDecode(inputStream.ReadToEnd());
                    }
                    if (jsonString != "")
                    {
                        var js1 = new JavaScriptSerializer();
                        // var title1 = context.Request.Params[1];
                        GetJsonData obj = js1.Deserialize<GetJsonData>(jsonString);
                        switch (obj.op)
                        {
                            case "Triplogs_edit_Click":
                                Triplogs_edit_Click(jsonString, context);
                                break;
                        }
                    }
                    else
                    {
                        var js1 = new JavaScriptSerializer();
                        var title1 = context.Request.Params[1];
                        GetJsonData obj = js1.Deserialize<GetJsonData>(title1);
                        switch (obj.op)
                        {
                            
                        }
                    }
                    break;
            }
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.ToString());
            context.Response.Write(response);
        }
    }
    public class statemaster
    {
        public string sno { get; set; }
        public string statename { get; set; }
        public string statecode { get; set; }
        public string ecode { get; set; }
        public string gststatecode { get; set; }
    }
    private void get_state_details(HttpContext context)
    {
        try
        {
            vdm = new VehicleDBMgr();
            cmd = new MySqlCommand("SELECT  sno,statename,statecode,ecode,gststatecode FROM  statemaster");
            DataTable dtstates = vdm.SelectQuery(cmd).Tables[0];
            List<statemaster> stateslist = new List<statemaster>();
            foreach (DataRow dr in dtstates.Rows)
            {
                statemaster obj1 = new statemaster();
                obj1.sno = dr["sno"].ToString();
                obj1.statename = dr["statename"].ToString();
                obj1.statecode = dr["statecode"].ToString();
                obj1.ecode = dr["ecode"].ToString();
                obj1.gststatecode = dr["gststatecode"].ToString();
                obj1.sno = dr["sno"].ToString();
                stateslist.Add(obj1);
            }
            string response = GetJson(stateslist);
            context.Response.Write(response);
        }
        catch
        {
        }
    }
    public class termloans_details
    {
        public string vehiclesno { get; set; }
        public string vehiclename { get; set; }
        public string date { get; set; }
        public string ledgercode { get; set; }
        public string ledgername { get; set; }
        public string amount { get; set; }
        public string remarks { get; set; }
        public string dates { get; set; }
        public string whcode { get; set; }
    }
    private void get_termloan_transaction_details(HttpContext context)
    {
        try
        {
            vdm = new VehicleDBMgr();
            string branchid = context.Session["Branch_ID"].ToString();
            string title = context.Session["TitleName"].ToString();
            string companyAddress = context.Session["Address"].ToString();
            string shortname = context.Session["shortname"].ToString();
            string date = context.Request["date"].ToString();
            DateTime fromdate = Convert.ToDateTime(date);
            fromdate = fromdate.AddMonths(-1);
            string strmonth = fromdate.ToString("MMM/dd/yyyy");
            double totamount = 0;
            cmd = new MySqlCommand("SELECT  termloanentry.bankname,vehicel_master.whcode, vehicel_master.registration_no, termloanentry.type,termloanentry.termloandate,vehicel_master.vm_owner, vehicel_master.vm_model,termloanentry.ledger_code, termloanentry.ledgername, termloanentry.interest_per, termloanentry.instalamount, termloanentry.loanamount, termloanentry.totalinstall, termloanentry.com_install, termloanentry.instaldate,vehicel_master.vm_sno FROM termloanentry INNER JOIN vehicel_master ON termloanentry.vehsno = vehicel_master.vm_sno WHERE (vehicel_master.vm_owner = @Owner) ");
            cmd.Parameters.Add("@Owner", shortname);
            DataTable dtdata = vdm.SelectQuery(cmd).Tables[0];
            List<termloans_details> termloans_details = new List<termloans_details>();
            if (dtdata.Rows.Count > 0)
            {
                foreach (DataRow dr in dtdata.Rows)
                {
                    double instalamount = 0;
                    double.TryParse(dr["instalamount"].ToString(), out instalamount);
                    double loanamount = 0;
                    double.TryParse(dr["loanamount"].ToString(), out loanamount);
                    double interest_per = 0;
                    double.TryParse(dr["interest_per"].ToString(), out interest_per);
                    string instaldate = dr["instaldate"].ToString();
                    DateTime dtinstaldate = Convert.ToDateTime(instaldate);
                    string strdate = dtinstaldate.ToString("dd/MMM/yyyy");
                    string[] strarray = strdate.Split('/');
                    string[] newmonth = strmonth.Split('/');
                    string install = strarray[0] + "/" + newmonth[0] + "/" + newmonth[2];
                    int totalinstall = 0;
                    int.TryParse(dr["totalinstall"].ToString(), out totalinstall);
                    string termloandate = dr["termloandate"].ToString();
                    DateTime dtnewdate = Convert.ToDateTime(install);
                    DateTime dtterm = Convert.ToDateTime(termloandate);
                    TimeSpan dateSpan = dtnewdate.Subtract(dtterm);
                    int NoOfdays = dateSpan.Days;
                    int month = NoOfdays / 30;
                    month = month + 1;
                    double tot_loanamountpercentage = 0;
                    double changedloan = 0;
                    double changedloanpercentage = 0;
                    double totalbal = 0;
                    totalbal = loanamount * interest_per;
                    totalbal = totalbal / 100;
                    changedloan = totalbal;
                    double permonthinterset = 0;
                    permonthinterset = totalbal / 12;
                    double tot_loanamount = 0;
                    string branchname = dr["bankname"].ToString();
                    if (branchname == "HDFC BANK" || branchname == "KOTAK MAHINDRA BANK")
                    {
                        tot_loanamount = loanamount;
                    }
                    else
                    {
                        tot_loanamount = loanamount + permonthinterset;
                    }
                    changedloan = tot_loanamount;
                    for (int i = 0; i < month; i++)
                    {

                        tot_loanamountpercentage = changedloan * interest_per;
                        double tot_loanamountpercentagemonth = 0;
                        tot_loanamountpercentagemonth = tot_loanamountpercentage / 100;
                        tot_loanamountpercentagemonth = tot_loanamountpercentagemonth / 12;
                        tot_loanamountpercentagemonth = Math.Round(tot_loanamountpercentagemonth, 2);
                        changedloanpercentage = tot_loanamountpercentagemonth;
                        changedloanpercentage = Math.Round(changedloanpercentage, 2);
                        double remaingamount = 0;
                        remaingamount = instalamount - tot_loanamountpercentagemonth;
                        changedloan = changedloan - remaingamount;
                    }
                    string tammount = "";
                    string tremarks = "";
                    string vehiclesno = "";
                    string ledgername = "";
                    string tdate = "";
                    if (changedloanpercentage > 0)
                    {
                        vehiclesno = dr["vm_sno"].ToString();
                        ledgername = dr["ledgername"].ToString();
                        tdate = fromdate.ToString("yyyy-MM-dd");
                        totamount += changedloanpercentage;
                        tammount = changedloanpercentage.ToString();
                        tremarks = "Being the vehicle interest for the month of " + fromdate.ToString("MMM-yyyy") + " Amount " + changedloanpercentage + " Vehicle Number " + dr["registration_no"].ToString() + " Type " + dr["type"].ToString() + ",Emp Name  " + context.Session["employname"].ToString();
                        termloans_details loans = new termloans_details();
                        loans.date = tdate.ToString();
                        loans.ledgercode = dr["ledger_code"].ToString();
                        loans.ledgername = ledgername.ToString();
                        loans.vehiclename = dr["registration_no"].ToString();
                        loans.vehiclesno = vehiclesno.ToString();
                        loans.amount = tammount.ToString();
                        loans.whcode = dr["whcode"].ToString();
                        loans.remarks = tremarks.ToString();
                        termloans_details.Add(loans);
                    }
                }
                string response = GetJson(termloans_details);
                context.Response.Write(response);
            }
            else
            {

            }

        }
        catch (Exception ex)
        {
            string Response = GetJson(ex.Message);
            context.Response.Write(Response);
        }
    }
    public class termloans_rowwsie
    {
        public string end { get; set; }
        public termlonans row_detail { set; get; }
    }
    public class termlonans
    {
        public string vehiclesno { get; set; }
        public string vehiclename { get; set; }
        public string ledgercode { get; set; }
        public string ledgername { get; set; }
        public string amount { get; set; }
        public string remarks { get; set; }
        public string whcode { get; set; }
        public string dates { get; set; }
        public string btnval { get; set; }
        public string date { get; set; }
        public List<termloans_rowwsie> termloansdetails { get; set; }
    }
    private void termloans_save_RowData(termloans_rowwsie obj, HttpContext context)
    {
        try
        {
            termlonans o = obj.row_detail;
            List<termlonans> Product_list = (List<termlonans>)context.Session["load_Next_row"];
            Product_list.Add(o);
            context.Session["load_Next_row"] = Product_list;
            string msg = obj.end;
            string response = GetJson(msg);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }
    private DateTime GetLowMonthRetrive(DateTime dt)
    {
        double Day, Hour, Min, Sec;
        DateTime DT = dt;
        DT = dt;
        Day = -dt.Day + 1;
        Hour = -dt.Hour;
        Min = -dt.Minute;
        Sec = -dt.Second;
        DT = DT.AddDays(Day);
        DT = DT.AddHours(Hour);
        DT = DT.AddMinutes(Min);
        DT = DT.AddSeconds(Sec);
        return DT;

    }
    private DateTime GetHighMonth(DateTime dt)
    {
        double Day, Hour, Min, Sec;
        DateTime DT = DateTime.Now;
        Day = 31 - dt.Day;
        Hour = 23 - dt.Hour;
        Min = 59 - dt.Minute;
        Sec = 59 - dt.Second;
        DT = dt;
        DT = DT.AddDays(Day);
        DT = DT.AddHours(Hour);
        DT = DT.AddMinutes(Min);
        DT = DT.AddSeconds(Sec);
        if (DT.Day == 3)
        {
            DT = DT.AddDays(-3);
        }
        else if (DT.Day == 2)
        {
            DT = DT.AddDays(-2);
        }
        else if (DT.Day == 1)
        {
            DT = DT.AddDays(-1);
        }
        return DT;
    }
    private void termloans_save_edit_data(HttpContext context)
    {
        try
        {
            vdm = new VehicleDBMgr();
            string branchid = context.Session["Branch_ID"].ToString();
            string btnval = context.Request["btnval"];
            string date = context.Request["date"];
            DateTime fromdate = Convert.ToDateTime(date);
            DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
            List<termlonans> Product_list = (List<termlonans>)context.Session["load_Next_row"];
            cmd = new MySqlCommand("SELECT  sno, vehsno, doe, paymenttype, amount, remarks, branchid FROM   termloantransactions where doe between @d1 and @d2 and branchid=@branchid");
            cmd.Parameters.Add("@d1", GetLowMonthRetrive(fromdate));
            cmd.Parameters.Add("@d2", GetHighMonth(fromdate));
            cmd.Parameters.Add("@branchid", branchid);
            DataTable dtterm = vdm.SelectQuery(cmd).Tables[0];
            if (dtterm.Rows.Count > 0)
            {
                string response = GetJson("This month transaction already saved");
                context.Response.Write(response);
            }
            else
            {
                if (btnval == "Save")
                {
                    foreach (termlonans art in Product_list)
                    {
                        if (art.amount != null || art.amount != "")
                        {
                            cmd = new MySqlCommand("insert into termloantransactions (vehsno,doe,amount,branchid,ledgername,vehicleno,whcode,remarks,ledgercode) values (@vehsno,@doe,@amount,@branchid,@ledgername,@vehicleno,@whcode,@remarks,@ledgercode)");
                            cmd.Parameters.Add("@vehsno", art.vehiclesno);
                            cmd.Parameters.Add("@doe", fromdate);
                            cmd.Parameters.Add("@amount", art.amount);
                            cmd.Parameters.Add("@branchid", branchid);
                            cmd.Parameters.Add("@ledgername", art.ledgername);
                            cmd.Parameters.Add("@vehicleno", art.vehiclename);
                            cmd.Parameters.Add("@whcode", art.whcode);
                            cmd.Parameters.Add("@remarks", art.remarks);
                            cmd.Parameters.Add("@ledgercode", art.ledgercode);
                            vdm.insert(cmd);
                        }
                    }
                    string response = GetJson("Term Loans Saved Successfully");
                    context.Response.Write(response);
                }
            }
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }
    /// <summary>
    /// //SAP Staging DB Query Changed
    /// </summary>
    /// 

    public class salesinvoice
    {
        public string createdate { get; set; }
        public string cardcode { get; set; }
        public string cardname { get; set; }
        public string taxdate { get; set; }
        public string docdate { get; set; }
        public string docduedate { get; set; }
        public string discpercent { get; set; }
        public string refno { get; set; }
        public string itemcode { get; set; }
        public string desc { get; set; }
        public string whscode { get; set; }
        public string qty { get; set; }
        public string price { get; set; }
        public string remarks { get; set; }
        public string freightcode { get; set; }
        public string FreightAmount { get; set; }
        public string b1upload { get; set; }
        public string processed { get; set; }
    }

    public class purchaseinvoice
    {

        public string taxdate { get; set; }
        public string cardcode { get; set; }
        public string cardname { get; set; }
        public string discpercent { get; set; }
        public string refno { get; set; }
        public string itemcode { get; set; }
        public string desc { get; set; }
        public string whscode { get; set; }
        public string qty { get; set; }
        public string price { get; set; }
        public string remarks { get; set; }
        public string b1upload { get; set; }
        public string processed { get; set; }

    }

    public class leaksdata
    {

        public string taxdate { get; set; }
        public string refno { get; set; }
        public string itemcode { get; set; }
        public string desc { get; set; }
        public string whscode { get; set; }
        public string qty { get; set; }
        public string price { get; set; }
        public string remarks { get; set; }
        public string b1upload { get; set; }
        public string processed { get; set; }
    }

    public class journal
    {

        public string taxdate { get; set; }
        public string refno { get; set; }
        public string itemcode { get; set; }
        public string desc { get; set; }
        public string whscode { get; set; }
        public string qty { get; set; }
        public string price { get; set; }
        public string debit { get; set; }
        public string credit { get; set; }
        public string remarks { get; set; }
        public string b1upload { get; set; }
        public string processed { get; set; }
    }

    public class inwarddata
    {

        public string taxdate { get; set; }
        public string refno { get; set; }
        public string itemcode { get; set; }
        public string desc { get; set; }
        public string whscode { get; set; }
        public string qty { get; set; }
        public string price { get; set; }
        public string remarks { get; set; }
        public string b1upload { get; set; }
        public string processed { get; set; }
    }

    public class journalpayments
    {

        public string taxdate { get; set; }
        public string refno { get; set; }
        public string itemcode { get; set; }
        public string desc { get; set; }
        public string whscode { get; set; }
        public string qty { get; set; }
        public string price { get; set; }
        public string debit { get; set; }
        public string credit { get; set; }
        public string b1upload { get; set; }
        public string processed { get; set; }
    }

    public class mrngrn
    {

        public string cardcode { get; set; }
        public string cardname { get; set; }
        public string taxdate { get; set; }
        public string refno { get; set; }
        public string itemcode { get; set; }
        public string desc { get; set; }
        public string whscode { get; set; }
        public string qty { get; set; }
        public string price { get; set; }
        public string remarks { get; set; }
        public string b1upload { get; set; }
        public string processed { get; set; }
    }

    public class receipts
    {

        public string taxdate { get; set; }
        public string refno { get; set; }
        public string itemcode { get; set; }
        public string desc { get; set; }
        public string whscode { get; set; }
        public string qty { get; set; }
        public string price { get; set; }
        public string remarks { get; set; }
        public string b1upload { get; set; }
        public string processed { get; set; }
    }

    public class stocktransfer
    {
        public string taxdate { get; set; }
        public string fromwarehouse { get; set; }
        public string towarehouse { get; set; }
        public string itemcode { get; set; }
        public string desc { get; set; }
        public string qty { get; set; }
        public string price { get; set; }
        public string remarks { get; set; }
        public string refno { get; set; }
        public string b1upload { get; set; }
        public string processed { get; set; }
    }

    public class payments
    {
        public string taxdate { get; set; }
        public string refno { get; set; }
        public string amount { get; set; }
        public string invoiceno { get; set; }
        public string whscode { get; set; }
        public string qty { get; set; }
        public string price { get; set; }
        public string remarks { get; set; }
        public string cardcode { get; set; }
        public string cardname { get; set; }
        public string b1upload { get; set; }
        public string processed { get; set; }
    }

    public class salesorder
    {
        public string taxdate { get; set; }
        public string cardcode { get; set; }
        public string cardname { get; set; }
        public string discpercent { get; set; }
        public string refno { get; set; }
        public string itemcode { get; set; }
        public string desc { get; set; }
        public string whscode { get; set; }
        public string qty { get; set; }
        public string price { get; set; }
        public string remarks { get; set; }
        public string b1upload { get; set; }
        public string processed { get; set; }
    }

    private void generate_sales_data(HttpContext context)
    {
        try
        {
            SAPdbmanger sapdm = new SAPdbmanger();
            SqlCommand sapcmd = new SqlCommand();
            string module = context.Request["module"].ToString();

            //Production Module
            if (module == "EMROIGN")
            {
                sapcmd = new SqlCommand("SELECT docdate, ReferenceNo, ItemCode, ItemName, Quantity, WhsCode, Price, Account,Remarks, B1Upload, Processed FROM  EMROIGN WHERE  (B1Upload = 'N') order by ReferenceNo");
                DataTable dtinward = sapdm.SelectQuery(sapcmd).Tables[0];
                List<inwarddata> inward = new List<inwarddata>();
                foreach (DataRow dr in dtinward.Rows)
                {
                    inwarddata data = new inwarddata();
                    string DocDate = dr["docdate"].ToString();
                    DateTime dtdoc = Convert.ToDateTime(DocDate);
                    data.taxdate = dtdoc.ToString("dd/MMM/yyyy");
                    data.refno = dr["ReferenceNo"].ToString();
                    data.itemcode = dr["ItemCode"].ToString();
                    data.desc = dr["ItemName"].ToString();
                    data.whscode = dr["WhsCode"].ToString();
                    data.qty = dr["Quantity"].ToString();
                    data.price = dr["Price"].ToString();
                    data.remarks = dr["Remarks"].ToString();
                    data.b1upload = dr["b1upload"].ToString();
                    data.processed = dr["processed"].ToString();
                    inward.Add(data);
                }
                string response = GetJson(inward);
                context.Response.Write(response);
            }
            // Sales Order
            if (module == "EMRORDR")
            {
                sapcmd = new SqlCommand("SELECT CardCode,taxdate, ReferenceNo, ItemCode, WhsCode, CardName, Quantity, Price, Dscription,REMARKS,B1Upload,processed FROM   EMRORDR WHERE  (B1Upload = 'N') order by ReferenceNo");
                DataTable dtsalesorder = sapdm.SelectQuery(sapcmd).Tables[0];
                List<salesorder> salesorderlist = new List<salesorder>();
                foreach (DataRow dr in dtsalesorder.Rows)
                {
                    salesorder data = new salesorder();
                    string DocDate = dr["taxdate"].ToString();
                    DateTime dtdoc = Convert.ToDateTime(DocDate);
                    data.taxdate = dtdoc.ToString("dd/MMM/yyyy");
                    data.cardcode = dr["CardCode"].ToString();
                    data.cardname = dr["CardName"].ToString();
                    data.refno = dr["ReferenceNo"].ToString();
                    data.itemcode = dr["ItemCode"].ToString();
                    data.desc = dr["Dscription"].ToString();
                    data.whscode = dr["WhsCode"].ToString();
                    data.qty = dr["Quantity"].ToString();
                    data.price = dr["Price"].ToString();
                    data.remarks = dr["REMARKS"].ToString();
                    data.b1upload = dr["b1upload"].ToString();
                    data.processed = dr["processed"].ToString();
                    salesorderlist.Add(data);
                }
                string response = GetJson(salesorderlist);
                context.Response.Write(response);
            }
            //Stock Transfer
            if (module == "EMROWTR")
            {
                sapcmd = new SqlCommand("SELECT CreateDate, PostingDate, DocDate, ReferenceNo, FromWhsCode, ToWhsCode, ItemCode, ItemName, Quantity, Price, Account, OcrCode, OcrCode2, OcrCode3, OcrCode4, OcrCode5, Remarks, B1Upload,Processed, Series FROM EMROWTR WHERE (B1Upload = 'N') order by ReferenceNo");
                DataTable dtsales = sapdm.SelectQuery(sapcmd).Tables[0];
                List<stocktransfer> salesinv = new List<stocktransfer>();
                foreach (DataRow dr in dtsales.Rows)
                {
                    stocktransfer data = new stocktransfer();
                    string DocDate = dr["DocDate"].ToString();
                    DateTime dtdoc = Convert.ToDateTime(DocDate);
                    data.taxdate = dtdoc.ToString("dd/MMM/yyyy");
                    data.refno = dr["ReferenceNo"].ToString();
                    data.fromwarehouse = dr["FromWhsCode"].ToString();
                    data.towarehouse = dr["ToWhsCode"].ToString();
                    data.itemcode = dr["ItemCode"].ToString();
                    data.desc = dr["ItemName"].ToString();
                    data.qty = dr["Quantity"].ToString();
                    data.price = dr["Price"].ToString();
                    data.remarks = dr["REMARKS"].ToString();
                    data.b1upload = dr["b1upload"].ToString();
                    data.processed = dr["processed"].ToString();
                    salesinv.Add(data);
                }
                string response = GetJson(salesinv);
                context.Response.Write(response);
            }
            //Sales Invoice
            if (module == "EMROINV")
            {
                sapcmd = new SqlCommand("SELECT CreateDate, CardCode, CardName, TaxDate, DocDate, DocDueDate, DiscPercent, ReferenceNo, ItemCode, Dscription, WhsCode, Quantity, Price, VAT_Percent, LineTotal, OcrCode, OcrCode2, OcrCode3, OcrCode4,OcrCode5, TaxCode, TaxAmount, FreightCode, FreightAmount, FreightTaxCode, U_Status, D_Date, UnitQty, UnitCost, PaymentStatus, DeliveryTime, Returnqty, Quantity_Kgs, PERCENTAGEON, SNF, FAT, CLR, HS, ALCOHOL, REMARKS, CHEMIST, QCO, INWARDNO, VEHICLENO, TEMP, DOE, BRANCHID, OPERATEDBY, CELLNO, MILKTYPE, COB1, PHOSPS1, MBRT, ACIDITY, OT, NEUTRALIZERS, PARTYDCNO, ENTRYDATE, SALETYPE, B1Upload, Processed FROM  EMROINV WHERE (B1Upload = 'N') order by ReferenceNo");
                DataTable dtsales = sapdm.SelectQuery(sapcmd).Tables[0];
                List<salesinvoice> salesinv = new List<salesinvoice>();
                foreach (DataRow dr in dtsales.Rows)
                {
                    salesinvoice data = new salesinvoice();
                    string DocDate = dr["TaxDate"].ToString();
                    DateTime dtdoc = Convert.ToDateTime(DocDate);
                    data.taxdate = dtdoc.ToString("dd/MMM/yyyy");
                    data.createdate = dr["CreateDate"].ToString();
                    data.cardcode = dr["CardCode"].ToString();
                    data.cardname = dr["CardName"].ToString();
                    data.refno = dr["ReferenceNo"].ToString();
                    data.itemcode = dr["ItemCode"].ToString();
                    data.desc = dr["Dscription"].ToString();
                    data.whscode = dr["WhsCode"].ToString();
                    data.qty = dr["Quantity"].ToString();
                    data.price = dr["Price"].ToString();
                    data.remarks = dr["REMARKS"].ToString();
                    data.b1upload = dr["b1upload"].ToString();
                    data.processed = dr["processed"].ToString();
                    salesinv.Add(data);
                }
                string response = GetJson(salesinv);
                context.Response.Write(response);
            }
            // Sales Incentives (Credit Memo)
            if (module == "EMRORIN")
            {
                sapcmd = new SqlCommand("SELECT CreateDate, PostingDate, DocDate, CardCode, Cardname, ReferenceNo, Description, AcctCode, Price, TaxCode, LocCode, OcrCode, Remarks, B1Upload, Processed, Series FROM EMRORIN WHERE (B1Upload = 'N') order by ReferenceNo");
                DataTable dtsales = sapdm.SelectQuery(sapcmd).Tables[0];
                List<salesinvoice> salesinv = new List<salesinvoice>();
                foreach (DataRow dr in dtsales.Rows)
                {
                    salesinvoice data = new salesinvoice();
                    string DocDate = dr["DocDate"].ToString();
                    DateTime dtdoc = Convert.ToDateTime(DocDate);
                    data.taxdate = dtdoc.ToString("dd/MMM/yyyy");
                    data.cardcode = dr["CardCode"].ToString();
                    data.cardname = dr["CardName"].ToString();
                    data.refno = dr["ReferenceNo"].ToString();
                    data.desc = dr["Description"].ToString();
                    data.whscode = dr["OcrCode"].ToString();
                    data.price = dr["Price"].ToString();
                    data.b1upload = dr["b1upload"].ToString();
                    data.processed = dr["processed"].ToString();
                    salesinv.Add(data);
                }
                string response = GetJson(salesinv);
                context.Response.Write(response);
            }
            //Goods Issue(Leaks)
            if (module == "EMROIGE")
            {
                sapcmd = new SqlCommand("SELECT DocDate, ReferenceNo, ItemCode, ItemName, Price, Quantity, Account, WhsCode,Remarks, B1Upload, Processed FROM EMROIGE WHERE  (B1Upload = 'N') order by ReferenceNo");
                DataTable dtleaks = sapdm.SelectQuery(sapcmd).Tables[0];
                List<leaksdata> leaks = new List<leaksdata>();
                foreach (DataRow dr in dtleaks.Rows)
                {
                    leaksdata data = new leaksdata();
                    string DocDate = dr["DocDate"].ToString();
                    DateTime dtdoc = Convert.ToDateTime(DocDate);
                    data.taxdate = dtdoc.ToString("dd/MMM/yyyy");
                    data.refno = dr["ReferenceNo"].ToString();
                    data.itemcode = dr["ItemCode"].ToString();
                    data.desc = dr["ItemName"].ToString();
                    data.whscode = dr["WhsCode"].ToString();
                    data.qty = dr["Quantity"].ToString();
                    data.price = dr["Price"].ToString();
                    data.remarks = dr["Remarks"].ToString();
                    data.b1upload = dr["b1upload"].ToString();
                    data.processed = dr["processed"].ToString();
                    leaks.Add(data);
                }
                string response = GetJson(leaks);
                context.Response.Write(response);
            }
            // Journel Voucher
            if (module == "EMROJDT")
            {
                sapcmd = new SqlCommand("SELECT  CreateDate, RefDate, DocDate, Ref1, Ref2, Ref3, TransNo, AcctCode, AcctName, Debit, Credit, OcrCode, OcrCode2, OcrCode3, OcrCode4, OcrCode5, B1Upload, Processed FROM   EMROJDT WHERE  (B1Upload = 'N') order by Ref1");
                DataTable dtjv = sapdm.SelectQuery(sapcmd).Tables[0];
                List<journal> journallist = new List<journal>();
                foreach (DataRow dr in dtjv.Rows)
                {
                    journal data = new journal();
                    string DocDate = dr["DocDate"].ToString();
                    DateTime dtdoc = Convert.ToDateTime(DocDate);
                    data.taxdate = dtdoc.ToString("dd/MMM/yyyy");
                    data.refno = dr["TransNo"].ToString();
                    data.itemcode = dr["AcctCode"].ToString();
                    data.desc = dr["AcctName"].ToString();
                    data.debit = dr["Debit"].ToString();
                    data.credit = dr["Credit"].ToString();
                    data.whscode = dr["OcrCode"].ToString();
                    data.b1upload = dr["b1upload"].ToString();
                    data.processed = dr["processed"].ToString();
                    journallist.Add(data);
                }
                string response = GetJson(journallist);
                context.Response.Write(response);
            }
            // Journel Payments
            if (module == "EMROJDTP")
            {
                sapcmd = new SqlCommand("SELECT CreateDate, RefDate, DocDate, Ref1, Ref2, Ref3, TransNo, TransCode, AcctCode, AcctName, Debit, Credit, Remarks, OcrCode, OcrCode2, OcrCode3, OcrCode4, OcrCode5, B1Upload, Processed FROM    EMROJDTP WHERE  (B1Upload = 'N') order by Ref1");
                DataTable dtJpayments = sapdm.SelectQuery(sapcmd).Tables[0];
                List<journalpayments> paymentlist = new List<journalpayments>();
                foreach (DataRow dr in dtJpayments.Rows)
                {
                    journalpayments data = new journalpayments();
                    string DocDate = dr["DocDate"].ToString();
                    DateTime dtdoc = Convert.ToDateTime(DocDate);
                    data.taxdate = dtdoc.ToString("dd/MMM/yyyy");
                    data.refno = dr["TransNo"].ToString();
                    data.itemcode = dr["AcctCode"].ToString();
                    data.desc = dr["AcctName"].ToString();
                    data.debit = dr["Debit"].ToString();
                    data.credit = dr["Credit"].ToString();
                    data.whscode = dr["OcrCode"].ToString();
                    data.b1upload = dr["b1upload"].ToString();
                    data.processed = dr["processed"].ToString();
                    paymentlist.Add(data);
                }
                string response = GetJson(paymentlist);
                context.Response.Write(response);
            }
            // GRN
            if (module == "EMROPDN")
            {
                sapcmd = new SqlCommand("SELECT CreateDate, CardCode, CardName, TaxDate, DocDate, DocDueDate, DiscPercent, ReferenceNo, ItemCode, Dscription, WhsCode, Quantity, Price, VAT_Percent, LineTotal, OcrCode, OcrCode2, OcrCode3, OcrCode4, OcrCode5, U_Status, D_Date, UnitQty, UnitCost, PaymentStatus, DeliveryTime, Returnqty, Quantity_Kgs, PERCENTAGEON, SNF, FAT, CLR, HS, ALCOHOL, REMARKS, CHEMIST, QCO, INWARDNO, VEHICLENO, TEMP, DOE, BRANCHID, OPERATEDBY, CELLNO, MILKTYPE, COB1, PHOSPS1, MBRT, ACIDITY, OT, NEUTRALIZERS, PARTYDCNO, ENTRYDATE, PURCHASETYPE, SESSION, B1Upload, Processed FROM  EMROPDN WHERE  (B1Upload = 'N') order by ReferenceNo");
                DataTable dtmrn = sapdm.SelectQuery(sapcmd).Tables[0];
                List<mrngrn> mrnlist = new List<mrngrn>();
                foreach (DataRow dr in dtmrn.Rows)
                {
                    mrngrn data = new mrngrn();
                    string DocDate = dr["DocDate"].ToString();
                    DateTime dtdoc = Convert.ToDateTime(DocDate);
                    data.taxdate = dtdoc.ToString("dd/MMM/yyyy");
                    data.refno = dr["ReferenceNo"].ToString();
                    data.cardcode = dr["CardCode"].ToString();
                    data.cardname = dr["CardName"].ToString();
                    data.itemcode = dr["ItemCode"].ToString();
                    data.desc = dr["Dscription"].ToString();
                    data.whscode = dr["WhsCode"].ToString();
                    data.qty = dr["Quantity"].ToString();
                    data.price = dr["Price"].ToString();
                    data.remarks = dr["Remarks"].ToString();
                    data.b1upload = dr["b1upload"].ToString();
                    data.processed = dr["processed"].ToString();
                    mrnlist.Add(data);
                }
                string response = GetJson(mrnlist);
                context.Response.Write(response);
            }
            // A/P Invoice here only showing top 10 percent
            if (module == "EMROPCH")
            {
                sapcmd = new SqlCommand("SELECT  TOP 1500 CreateDate, CardCode, CardName, TaxDate, DocDate, DocDueDate, DiscPercent, ReferenceNo, GRNRefNo, ItemCode, Dscription, WhsCode, Quantity, Price, VAT_Percent, LineTotal, OcrCode, OcrCode2,OcrCode3, OcrCode4, OcrCode5, TaxCode, TaxAmount, OverHeadAmount1, CartageAmount2, SpecialBonus3, Incentive4, Others5, Others6, Others7, U_Status, D_Date, UnitQty, UnitCost, PaymentStatus, DeliveryTime, Returnqty, Quantity_Kgs, PERCENTAGEON, SNF, FAT, CLR, HS, ALCOHOL, REMARKS, CHEMIST, QCO, INWARDNO, VEHICLENO, TEMP, DOE, BRANCHID, OPERATEDBY, CELLNO, MILKTYPE, COB1, PHOSPS1, MBRT, ACIDITY, OT, NEUTRALIZERS, PARTYDCNO, ENTRYDATE, PURCHASETYPE, SESSION, B1Upload, Processed FROM  EMROPCH WHERE (B1Upload = 'N') order by ReferenceNo ");
                DataTable dtpurchase = sapdm.SelectQuery(sapcmd).Tables[0];
                List<purchaseinvoice> purchaseinv = new List<purchaseinvoice>();
                foreach (DataRow dr in dtpurchase.Rows)
                {
                    purchaseinvoice data = new purchaseinvoice();
                    string DocDate = dr["DocDate"].ToString();
                    DateTime dtdoc = Convert.ToDateTime(DocDate);
                    data.taxdate = dtdoc.ToString("dd/MMM/yyyy");
                    data.cardcode = dr["CardCode"].ToString();
                    data.cardname = dr["CardName"].ToString();
                    data.discpercent = dr["DiscPercent"].ToString();
                    data.refno = dr["ReferenceNo"].ToString();
                    data.itemcode = dr["ItemCode"].ToString();
                    data.desc = dr["Dscription"].ToString();
                    data.whscode = dr["WhsCode"].ToString();
                    data.qty = dr["Quantity"].ToString();
                    data.price = dr["Price"].ToString();
                    data.remarks = dr["REMARKS"].ToString();
                    data.b1upload = dr["b1upload"].ToString();
                    data.processed = dr["processed"].ToString();
                    purchaseinv.Add(data);
                }
                string response = GetJson(purchaseinv);
                context.Response.Write(response);
            }
            // A/P Invoice for stores
            if (module == "EMROPCHS")
            {
                sapcmd = new SqlCommand("SELECT  CreateDate, CardCode, CardName, TaxDate, DocDate, DocDueDate, DiscPercent, ReferenceNo, GRNRefNo, ItemCode, Dscription, WhsCode, Quantity, Price, VAT_Percent, LineTotal, OcrCode, OcrCode2,OcrCode3, OcrCode4, OcrCode5, TaxCode, TaxAmount, OverHeadAmount1, CartageAmount2, SpecialBonus3, Incentive4, Others5, Others6, Others7, U_Status, D_Date, UnitQty, UnitCost, PaymentStatus, DeliveryTime, Returnqty, Quantity_Kgs, PERCENTAGEON, SNF, FAT, CLR, HS, ALCOHOL, REMARKS, CHEMIST, QCO, INWARDNO, VEHICLENO, TEMP, DOE, BRANCHID, OPERATEDBY, CELLNO, MILKTYPE, COB1, PHOSPS1, MBRT, ACIDITY, OT, NEUTRALIZERS, PARTYDCNO, ENTRYDATE, PURCHASETYPE, SESSION, B1Upload, Processed FROM  EMROPCHS WHERE (B1Upload = 'N') order by ReferenceNo ");
                DataTable dtpurchase = sapdm.SelectQuery(sapcmd).Tables[0];
                List<purchaseinvoice> purchaseinv = new List<purchaseinvoice>();
                foreach (DataRow dr in dtpurchase.Rows)
                {
                    purchaseinvoice data = new purchaseinvoice();
                    string DocDate = dr["DocDate"].ToString();
                    DateTime dtdoc = Convert.ToDateTime(DocDate);
                    data.taxdate = dtdoc.ToString("dd/MMM/yyyy");
                    data.cardcode = dr["CardCode"].ToString();
                    data.cardname = dr["CardName"].ToString();
                    data.discpercent = dr["DiscPercent"].ToString();
                    data.refno = dr["ReferenceNo"].ToString();
                    data.itemcode = dr["ItemCode"].ToString();
                    data.desc = dr["Dscription"].ToString();
                    data.whscode = dr["WhsCode"].ToString();
                    data.qty = dr["Quantity"].ToString();
                    data.price = dr["Price"].ToString();
                    data.remarks = dr["REMARKS"].ToString();
                    data.b1upload = dr["b1upload"].ToString();
                    data.processed = dr["processed"].ToString();
                    purchaseinv.Add(data);
                }
                string response = GetJson(purchaseinv);
                context.Response.Write(response);
            }
            // RECEIPTS
            if (module == "EMRORCT")
            {
                sapcmd = new SqlCommand("SELECT CreateDate, PaymentDate, DOE, ReferenceNo, CardCode, Remarks, ApprovedBy, CreatedBy, Status, AcctNo, InvoiceNo, PaymentDoc, PaymentMode, PaymentSum, B1Upload, Processed FROM  EMRORCT WHERE (B1Upload = 'N') order by ReferenceNo");
                DataTable dtreceipt = sapdm.SelectQuery(sapcmd).Tables[0];
                List<receipts> receiptlist = new List<receipts>();
                foreach (DataRow dr in dtreceipt.Rows)
                {
                    receipts data = new receipts();
                    string DocDate = dr["PaymentDate"].ToString();
                    DateTime dtdoc = Convert.ToDateTime(DocDate);
                    data.taxdate = dtdoc.ToString("dd/MMM/yyyy");
                    data.refno = dr["ReferenceNo"].ToString();
                    data.itemcode = dr["CardCode"].ToString();
                    //data.desc = dr["Dscription"].ToString();
                    data.price = dr["PaymentSum"].ToString();
                    data.processed = dr["Processed"].ToString();
                    data.b1upload = dr["B1Upload"].ToString();
                    data.remarks = dr["Remarks"].ToString();
                    receiptlist.Add(data);
                }
                string response = GetJson(receiptlist);
                context.Response.Write(response);
            }
            // PAYMENTS
            if (module == "EMROVPM")
            {
                sapcmd = new SqlCommand("SELECT CreateDate, PaymentDate, DOE, ReferenceNo, CardCode, Remarks, ApprovedBy, CreatedBy, Status, AcctNo, InvoiceNo, PaymentDoc, PaymentMode, PaymentSum,B1Upload, Processed FROM  EMROVPM WHERE  (B1Upload = 'N') order by ReferenceNo");
                DataTable dtpayments = sapdm.SelectQuery(sapcmd).Tables[0];
                List<payments> pymntlist = new List<payments>();
                foreach (DataRow dr in dtpayments.Rows)
                {
                    payments data = new payments();
                    string DocDate = dr["PaymentDate"].ToString();
                    DateTime dtdoc = Convert.ToDateTime(DocDate);
                    data.taxdate = dtdoc.ToString("dd/MMM/yyyy");
                    data.refno = dr["ReferenceNo"].ToString();
                    data.amount = dr["PaymentSum"].ToString();
                    data.cardcode = dr["CardCode"].ToString();
                    data.b1upload = dr["b1upload"].ToString();
                    data.processed = dr["processed"].ToString();
                    data.remarks = dr["Remarks"].ToString();
                    pymntlist.Add(data);
                }
                string response = GetJson(pymntlist);
                context.Response.Write(response);
            }
            // PURCHASE ORDER
            if (module == "EMROPOR")
            {
                sapcmd = new SqlCommand("SELECT CreateDate, CardCode, CardName, TaxDate, DocDate, DocDueDate, DiscPercent, ReferenceNo, ItemCode, Dscription, WhsCode, Quantity, Price, PURCHASETYPE, B1Upload, Processed FROM EMROPOR WHERE (B1Upload = 'N') order by ReferenceNo");
                DataTable dtpayments = sapdm.SelectQuery(sapcmd).Tables[0];
                List<mrngrn> pymntlist = new List<mrngrn>();
                foreach (DataRow dr in dtpayments.Rows)
                {
                    mrngrn data = new mrngrn();
                    string DocDate = dr["DocDate"].ToString();
                    DateTime dtdoc = Convert.ToDateTime(DocDate);
                    data.taxdate = dtdoc.ToString("dd/MMM/yyyy");
                    data.refno = dr["ReferenceNo"].ToString();
                    data.cardcode = dr["CardCode"].ToString();
                    data.cardname = dr["CardName"].ToString();
                    data.itemcode = dr["ItemCode"].ToString();
                    data.desc = dr["Dscription"].ToString();
                    data.whscode = dr["WhsCode"].ToString();
                    data.qty = dr["Quantity"].ToString();
                    data.price = dr["Price"].ToString();
                    // data.remarks = dr["Remarks"].ToString();
                    data.b1upload = dr["b1upload"].ToString();
                    data.processed = dr["processed"].ToString();
                    pymntlist.Add(data);
                }
                string response = GetJson(pymntlist);
                context.Response.Write(response);
            }
        }
        
         catch (Exception ex)
        {
            string response = GetJson(ex.ToString());
            context.Response.Write(response);
        }
    }

    private void update_sales_data(HttpContext context)
    {
        try
        {
            SAPdbmanger sapdm = new SAPdbmanger();
            SqlCommand sapcmd = new SqlCommand();
            string module = context.Request["module"].ToString();
            //Production Module
            if (module == "EMROIGN")
            {
                sapcmd = new SqlCommand("Update EMROIGN set processed=@processed  WHERE  (B1Upload = 'N') and (Processed='Y')");
                sapcmd.Parameters.Add("@processed", 'N');
                sapdm.Update(sapcmd);
            }
            // Sales Order
            if (module == "EMRORDR")
            {
                sapcmd = new SqlCommand("Update EMRORDR set processed=@processed  WHERE  (B1Upload = 'N') and (Processed='Y')");
                sapcmd.Parameters.Add("@processed", 'N');
                sapdm.Update(sapcmd);
            }
            //Stock Transfer
            if (module == "EMROWTR")
            {
                sapcmd = new SqlCommand("Update EMROWTR set processed=@processed  WHERE  (B1Upload = 'N') and (Processed='Y')");
                sapcmd.Parameters.Add("@processed", 'N');
                sapdm.Update(sapcmd);
            }
            //Sales Invoice
            if (module == "EMROINV")
            {
                sapcmd = new SqlCommand("Update EMROINV set processed=@processed  WHERE  (B1Upload = 'N') and (Processed='Y')");
                sapcmd.Parameters.Add("@processed", 'N');
                sapdm.Update(sapcmd);
               
            }
            // Sales Incentives (Credit Memo)
            if (module == "EMRORIN")
            {
                sapcmd = new SqlCommand("Update EMRORIN set processed=@processed  WHERE  (B1Upload = 'N') and (Processed='Y')");
                sapcmd.Parameters.Add("@processed", 'N');
                sapdm.Update(sapcmd);
            }
            //Goods Issue(Leaks)
            if (module == "EMROIGE")
            {
                sapcmd = new SqlCommand("Update EMROIGE set processed=@processed  WHERE  (B1Upload = 'N') and (Processed='Y')");
                sapcmd.Parameters.Add("@processed", 'N');
                sapdm.Update(sapcmd);
            }
            // Journel Voucher
            if (module == "EMROJDT")
            {
                sapcmd = new SqlCommand("Update EMROJDT set processed=@processed  WHERE  (B1Upload = 'N') and (Processed='Y')");
                sapcmd.Parameters.Add("@processed", 'N');
                sapdm.Update(sapcmd);
            }
            // Journel Payments
            if (module == "EMROJDTP")
            {
                sapcmd = new SqlCommand("Update EMROJDTP set processed=@processed  WHERE  (B1Upload = 'N') and (Processed='Y')");
                sapcmd.Parameters.Add("@processed", 'N');
                sapdm.Update(sapcmd);
            }
            // GRN
            if (module == "EMROPDN")
            {
                sapcmd = new SqlCommand("Update EMROPDN set processed=@processed  WHERE  (B1Upload = 'N') and (Processed='Y')");
                sapcmd.Parameters.Add("@processed", 'N');
                sapdm.Update(sapcmd);
            }
            // A/P Invoice For Procurement Purchase
            if (module == "EMROPCH")
            {
                sapcmd = new SqlCommand("Update EMROPCH set processed=@processed  WHERE  (B1Upload = 'N') and (Processed='Y')");
                sapcmd.Parameters.Add("@processed", 'N');
                sapdm.Update(sapcmd);
            }
            // A/P Invoice for Stores Purchase
            if (module == "EMROPCHS")
            {
                sapcmd = new SqlCommand("Update EMROPCHS set processed=@processed  WHERE  (B1Upload = 'N') and (Processed='Y')");
                sapcmd.Parameters.Add("@processed", 'N');
                sapdm.Update(sapcmd);
            }
            // RECEIPTS
            if (module == "EMRORCT")
            {
                sapcmd = new SqlCommand("Update EMRORCT set processed=@processed  WHERE  (B1Upload = 'N') and (Processed='Y')");
                sapcmd.Parameters.Add("@processed", 'N');
                sapdm.Update(sapcmd);
            }
            // PAYMENTS
            if (module == "EMROVPM")
            {
                sapcmd = new SqlCommand("Update EMROVPM set processed=@processed  WHERE  (B1Upload = 'N') and (Processed='Y')");
                sapcmd.Parameters.Add("@processed", 'N');
                sapdm.Update(sapcmd);
            }
            // PURCHASE ORDER
            if (module == "EMROPOR")
            {
                sapcmd = new SqlCommand("Update EMROPOR set processed=@processed  WHERE  (B1Upload = 'N') and (Processed='Y')");
                sapcmd.Parameters.Add("@processed", 'N');
                sapdm.Update(sapcmd);
            }
            string msg = "Updated Successfully";
            string response = GetJson(msg);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.ToString());
            context.Response.Write(response);
        }
    }

    public class vehicle_document_details
    {
        public string Vehno { get; set; }
        public string vehicle_name { get; set; }
        public string documentid { get; set; }
        public string documentname { get; set; }
        public string ftplocation { get; set; }
        public string photo { get; set; }
        public string imageid { get; set; }
        

        public string imagename { get; set; }
    }
    public class term_loan_details
    {
        public string vm_sno { get; set; }
        public string VehicleNo { get; set; }
        public string mfgdate { get; set; }
        public string termloandate { get; set; }
        public string type { get; set; }
        public string sno { get; set; }
        public string termloanno { get; set; }
        public string loanamount { get; set; }
        public string instalamount { get; set; }
        public string instaldate { get; set; }
        public string totalinstall { get; set; }
        public string com_install { get; set; }
        public string bankname { get; set; }
        public string Capacity { get; set; }
        public string VehicleType { get; set; }
        public string Make { get; set; }
        public string interest_per { get; set; }
        public string ledgername { get; set; }
        public string ledgercode { get; set; }

    }

    private void getall_TermLoanDetails(HttpContext context)
    {
        try
        {
            vdm = new VehicleDBMgr();
            string BranchID = context.Session["Branch_ID"].ToString();
            cmd = new MySqlCommand("SELECT vehicel_master.vm_sno,  vehicel_master.registration_no AS VehicleNo,termloanentry.ledgername,termloanentry.ledger_code, termloanentry.mfgdate,termloanentry.termloandate,termloanentry.interest_per,termloanentry.type,termloanentry.sno, termloanentry.termloanno,termloanentry.loanamount, termloanentry.instalamount,termloanentry.instaldate , termloanentry.totalinstall, termloanentry.com_install, termloanentry.bankname, vehicel_master.Capacity, minimasters.mm_name AS VehicleType, minimasters_1.mm_code AS Make FROM termloanentry INNER JOIN vehicel_master ON termloanentry.vehsno = vehicel_master.vm_sno INNER JOIN minimasters ON vehicel_master.vhtype_refno = minimasters.sno INNER JOIN minimasters minimasters_1 ON vehicel_master.vhmake_refno = minimasters_1.sno   where (vehicel_master.vm_owner=@Owner)  Group by vehicel_master.registration_no,termloanentry.Type order by termloanentry.Bankname");
            cmd.Parameters.Add("@Owner", context.Session["shortname"].ToString());
            cmd.Parameters.Add("@BranchID", BranchID);
            DataTable dtTermLoans = vdm.SelectQuery(cmd).Tables[0];
            List<term_loan_details> termloanlist = new List<term_loan_details>();
            if (dtTermLoans.Rows.Count > 0)
            {
                foreach (DataRow dr in dtTermLoans.Rows)
                {
                    term_loan_details Gettermloans = new term_loan_details();
                    Gettermloans.vm_sno = dr["vm_sno"].ToString();
                    Gettermloans.VehicleNo = dr["VehicleNo"].ToString();
                    Gettermloans.mfgdate = ((DateTime)dr["mfgdate"]).ToString("yyyy-MM-dd"); //dr["mfgdate"].ToString();
                    Gettermloans.termloandate = ((DateTime)dr["termloandate"]).ToString("yyyy-MM-dd"); //dr["termloandate"].ToString();
                    Gettermloans.type = dr["type"].ToString();
                    Gettermloans.sno = dr["sno"].ToString();
                    Gettermloans.termloanno = dr["termloanno"].ToString();
                    Gettermloans.loanamount = dr["loanamount"].ToString();
                    Gettermloans.instalamount = dr["instalamount"].ToString();
                    Gettermloans.instaldate = ((DateTime)dr["instaldate"]).ToString("yyyy-MM-dd"); //dr["instaldate"].ToString();
                    Gettermloans.totalinstall = dr["totalinstall"].ToString();
                    Gettermloans.com_install = dr["com_install"].ToString();
                    Gettermloans.bankname = dr["bankname"].ToString();
                    Gettermloans.Capacity = dr["Capacity"].ToString();
                    Gettermloans.VehicleType = dr["VehicleType"].ToString();
                    Gettermloans.Make = dr["Make"].ToString();
                    Gettermloans.interest_per = dr["interest_per"].ToString();
                    Gettermloans.ledgername = dr["ledgername"].ToString();
                    Gettermloans.ledgercode = dr["ledger_code"].ToString();
                    termloanlist.Add(Gettermloans);
                }
            }
            string response = GetJson(termloanlist);
            context.Response.Write(response);
        }
        catch
        {
        }
    }
    private void get_trip_startingodometer(HttpContext context)
    {
        try
        {
            vdm = new VehicleDBMgr();
            string Trip_sno = context.Request["tripid"];
            cmd = new MySqlCommand("SELECT sno, tripsheetno, vehiclestartreading FROM tripdata WHERE (sno = @Trip_sno)");
            cmd.Parameters.Add("@Trip_sno", Trip_sno);
            DataTable dtemp = vdm.SelectQuery(cmd).Tables[0];
            string msg = "";
            if (dtemp.Rows.Count > 0)
            {
                foreach (DataRow dr in dtemp.Rows)
                {
                    msg = dr["vehiclestartreading"].ToString();
                }
            }
            string response = GetJson(msg);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string Response = GetJson(ex.Message);
            context.Response.Write(Response);
        }
    }
    private void getvehicle_Uploaded_Documents(HttpContext context)
    {
        try
        {
            vdm = new VehicleDBMgr();
            string Vehiclesno = context.Request["Vehiclesno"];
            cmd = new MySqlCommand("SELECT vehicel_master.registration_no, vehicle_documents.documentpath, vehicle_documents.documentid FROM vehicel_master INNER JOIN vehicle_documents ON vehicel_master.vm_sno = vehicle_documents.vehicleid WHERE (vehicel_master.vm_sno = @Vehiclesno)");
            cmd.Parameters.Add("@Vehiclesno", Vehiclesno);
            DataTable dtemp = vdm.SelectQuery(cmd).Tables[0];
            List<vehicle_document_details> bankMasterlist = new List<vehicle_document_details>();
            if (dtemp.Rows.Count > 0)
            {
                foreach (DataRow dr in dtemp.Rows)
                {
                    vehicle_document_details GetEmployee = new vehicle_document_details();
                    GetEmployee.Vehno = Vehiclesno;
                    GetEmployee.vehicle_name = dr["registration_no"].ToString();
                    GetEmployee.documentid = dr["documentid"].ToString();
                    GetEmployee.ftplocation = "ftp://223.196.32.30:21/FLEET/";
                    GetEmployee.photo = dr["documentpath"].ToString();
                    bankMasterlist.Add(GetEmployee);
                }
            }
            string response = GetJson(bankMasterlist);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string Response = GetJson(ex.Message);
            context.Response.Write(Response);
        }
    }

    private void save_Vehicle_Document_Info(HttpContext context)
    {
        try
        {
            //if (context.Session["branch_id"] != null)
            //{
            if (context.Request.Files.Count > 0)
            {
                string vehiclesno = context.Request["vehiclesno"];
                vehiclesno = vehiclesno.TrimEnd();
                string registration_no = context.Request["registration_no"];
                registration_no = registration_no.TrimEnd();
                string documentname = context.Request["documentname"];
                documentname = documentname.TrimEnd();
                string documentid = context.Request["documentid"];
                documentid = documentid.TrimEnd();
                string entryby = context.Session["Employ_Sno"].ToString();
                HttpFileCollection files = context.Request.Files;
                DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFile file = files[i];
                    string[] extension = file.FileName.Split('.');
                    string upload_filename = registration_no + documentname + documentid + vehiclesno + ".jpeg";// +extension[extension.Length - 1];
                    if (UploadToFTP(file, upload_filename))
                    {
                        MySqlCommand cmd = new MySqlCommand("update  vehicle_documents set documentpath=@documentpath where vehicleid=@vehicleid and documentid=@documentid");
                        cmd.Parameters.Add("@vehicleid", vehiclesno);
                        cmd.Parameters.Add("@documentpath", upload_filename);
                        cmd.Parameters.Add("@documentid", documentid);
                        if (vdm.Update(cmd) == 0)
                        {
                            cmd = new MySqlCommand("insert into vehicle_documents (vehicleid,documentpath,doe,entryby,documentid) values (@vehicleid,@documentpath,@doe,@entryby,@documentid)");
                            cmd.Parameters.Add("@vehicleid", vehiclesno);
                            cmd.Parameters.Add("@documentpath", upload_filename);
                            cmd.Parameters.Add("@documentid", documentid);
                            cmd.Parameters.Add("@doe", ServerDateCurrentdate);
                            cmd.Parameters.Add("@entryby", entryby);
                            vdm.insert(cmd);
                        }
                    }
                }
                context.Response.ContentType = "text/plain";
                context.Response.Write("File Uploaded Successfully!");
            }
            //}

        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }
    private void Vehicle_pic_files_upload(HttpContext context)
    {
        try
        {
            if (context.Request.Files.Count > 0)
            {
                string vehiclesno = context.Request["vehiclesno"];
                string Veh_sno = context.Request["Veh_sno"];
                HttpFileCollection files = context.Request.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFile file = files[i];
                    string[] extension = file.FileName.Split('.');
                    string upload_filename = vehiclesno + "_profilepic_" + Veh_sno + ".jpeg";// +extension[extension.Length - 1];
                    if (UploadToFTP(file, upload_filename))
                    {
                        MySqlCommand cmd = new MySqlCommand("update vehicel_master set imagename=@imagename where  vm_sno=@vm_sno");
                        cmd.Parameters.Add("@vm_sno", Veh_sno);
                        cmd.Parameters.Add("@imagename", upload_filename);
                        vdm.Update(cmd);
                    }
                }
                context.Response.ContentType = "text/plain";
                context.Response.Write("File Uploaded Successfully!");
            }
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }
    private bool UploadToFTP(HttpPostedFile fileToUpload, string filename)
    {
        // Get the object used to communicate with the server.
        string uploadUrl = "ftp://223.196.32.30:21/FLEET/";
        // string fileName = fileToUpload.FileName;
        try
        {
            FtpWebRequest del_request = (FtpWebRequest)WebRequest.Create(uploadUrl + @"/" + filename);
            del_request.Credentials = new NetworkCredential("ftpvys", "Vyshnavi123");
            del_request.Method = WebRequestMethods.Ftp.DeleteFile;
            FtpWebResponse delete_response = (FtpWebResponse)del_request.GetResponse();
            Console.WriteLine("Delete status: {0}", delete_response.StatusDescription);
            delete_response.Close();
        }
        catch
        {
        }
        FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uploadUrl + @"/" + filename);
        request.Credentials = new NetworkCredential("ftpvys", "Vyshnavi123");
        request.Method = WebRequestMethods.Ftp.UploadFile;
        byte[] fileContents = null;
        using (var binaryReader = new BinaryReader(fileToUpload.InputStream))
        {
            fileContents = binaryReader.ReadBytes(fileToUpload.ContentLength);
        }
        request.ContentLength = fileContents.Length;
        Stream requestStream = request.GetRequestStream();
        requestStream.Write(fileContents, 0, fileContents.Length);
        requestStream.Close();
        FtpWebResponse response = (FtpWebResponse)request.GetResponse();
        response.Close();
        return true;
    }

    class Veh_exp_cls
    {
        public string sno { get; set; }
        public string maintace_code { get; set; }
        public string maintace_id { get; set; }
        public string vehicleno { get; set; }
        public string veh_sno { get; set; }
        public string name { get; set; }
        public string incharge { get; set; }
        public string amount { get; set; }
        public string remarks { get; set; }
    }
    class Subpayables
    {
        public string HeadSno { get; set; }
        public string HeadOfAccount { get; set; }
        public string Amount { get; set; }

    }
    class VehicleMileagecls
    {
        public string vehicleno { get; set; }
        public string routename { get; set; }
        public string mileage { get; set; }

    }
    class Availablehiclecls
    {
        public string vehicleno { get; set; }
        public string type { get; set; }
        public string make { get; set; }
        public string route { get; set; }

    }
    public class vehiServicingAlertsclass
    {
        public string vehicleno { get; set; }
        public string vehsno { get; set; }
        public string vehiclemake { get; set; }
        public string vehiclemodel { get; set; }
        public string vehicletype { get; set; }
        public string Routename { get; set; }
        public string Model { get; set; }
        public string Capacity { get; set; }
        public string Make { get; set; }
        public string EOCColor { get; set; }
        public string EOC { get; set; }
        public string GOCColor { get; set; }
        public string GOC { get; set; }
        public string AFCColor { get; set; }
        public string AFC { get; set; }
        public string OFCColor { get; set; }
        public string OFC { get; set; }
        public string brakefluidColor { get; set; }
        public string brakefluid { get; set; }
        public string powersteeringfluidColor { get; set; }
        public string powersteeringfluid { get; set; }
        public string transmissionfluidColor { get; set; }
        public string transmissionfluid { get; set; }
        public string washerfluidColor { get; set; }
        public string washerfluid { get; set; }
        public string checkbrakesColor { get; set; }
        public string checkbrakes { get; set; }
        public string checkleaksColor { get; set; }
        public string checkleaks { get; set; }
        public string allbeltsColor { get; set; }
        public string allbelts { get; set; }
        public string lubricatechassisColor { get; set; }
        public string lubricatechassis { get; set; }
        public string Sno { get; set; }
    }
    private void GetVehicle_service_deatails(HttpContext context)
    {

        try
        {
            vdm = new VehicleDBMgr();
            string Maintace_id = context.Request["Maintenance_sno"];
            string BranchID = "0";
            BranchID = context.Session["Branch_ID"].ToString();
            cmd = new MySqlCommand("SELECT vehicel_master.vm_sno,vehicel_master.registration_no, minimasters.mm_name AS VehicleType, minimasters_1.mm_name AS Make, vehicel_master.Capacity, vehicel_master.fuel_capacity, vehicel_master.odometer, vehi_service_update_kms.eoc, vehi_service_update_kms.goc, vehi_service_update_kms.ofc, vehi_service_update_kms.afc, vehi_service_update_kms.brake_fluid, vehi_service_update_kms.steering_fluid, vehi_service_update_kms.transmission_fluid, vehi_service_update_kms.washer_fluid, vehi_service_update_kms.wheel_bearings, vehi_service_update_kms.checkleaks, vehi_service_update_kms.belts_hoses, vehi_service_update_kms.lubricate_chasis, vehi_service_update_kms.airchecking, vehi_service_update_kms.tyreinterchanging, vehi_service_update_kms.vehsno FROM minimasters INNER JOIN vehicel_master ON minimasters.sno = vehicel_master.vhtype_refno INNER JOIN minimasters minimasters_1 ON vehicel_master.vhmake_refno = minimasters_1.sno INNER JOIN vehi_service_update_kms ON vehicel_master.vm_sno = vehi_service_update_kms.vehsno WHERE (vehicel_master.vm_owner = @Owner)");
            cmd.Parameters.Add("@Owner", context.Session["shortname"].ToString());
            cmd.Parameters.Add("@BranchID", BranchID);
            DataTable dtVehicle_update_kms = vdm.SelectQuery(cmd).Tables[0];
            List<vehiServicingAlertsclass> Vehicleslist = new List<vehiServicingAlertsclass>();
            cmd = new MySqlCommand("SELECT vehi_servicingalerts.sno, vehi_servicingalerts.vehsno, vehi_servicingalerts.eoc, vehi_servicingalerts.goc, vehi_servicingalerts.ofc, vehi_servicingalerts.afc, vehi_servicingalerts.brake_fluid, vehi_servicingalerts.steering_fluid, vehi_servicingalerts.transmission_fluid, vehi_servicingalerts.washer_fluid, vehi_servicingalerts.wheel_bearings, vehi_servicingalerts.checkleaks, vehi_servicingalerts.belts_hoses, vehi_servicingalerts.lubricate_chasis, vehi_servicingalerts.airchecking, vehi_servicingalerts.tyreinterchanging, vehicel_master.registration_no FROM vehi_servicingalerts INNER JOIN vehicel_master ON vehi_servicingalerts.vehsno = vehicel_master.vm_sno WHERE (vehicel_master.vm_owner = @Owner)");
            cmd.Parameters.Add("@Owner", context.Session["shortname"].ToString());
            DataTable dtVehicle_service = vdm.SelectQuery(cmd).Tables[0];
            if (dtVehicle_update_kms.Rows.Count > 0)
            {
                foreach (DataRow dr in dtVehicle_update_kms.Rows)
                {
                    vehiServicingAlertsclass Getvehicle = new vehiServicingAlertsclass();
                    Getvehicle.vehicleno = dr["registration_no"].ToString();
                    Getvehicle.vehiclemake = dr["Make"].ToString();
                    Getvehicle.vehicletype = dr["VehicleType"].ToString();
                    Getvehicle.Capacity = dr["Capacity"].ToString();
                    DataRow[] drr = dtVehicle_service.Select("vehsno=" + dr["vm_sno"].ToString());
                    DataTable dtservice = new DataTable();
                    if (drr.Length > 0)
                    {
                        dtservice = drr.CopyToDataTable();
                    }
                    foreach (DataRow drser in dtservice.Rows)
                    {

                        ////////////////////EOC Change.....................//////////////////////
                        int updateEOC = 0;
                        int.TryParse(dr["EOC"].ToString(), out updateEOC);
                        int serviceEOC = 0;
                        int.TryParse(drser["EOC"].ToString(), out serviceEOC);
                        int eoc = 0;
                        eoc = serviceEOC * 90 / 100;
                        if (updateEOC > eoc)
                        {
                            if (updateEOC > serviceEOC)
                            {
                                Getvehicle.EOCColor = "Red";
                                Getvehicle.EOC = dr["EOC"].ToString();
                            }
                            else
                            {
                                Getvehicle.EOCColor = "orange";
                                Getvehicle.EOC = dr["EOC"].ToString();
                            }
                        }
                        else
                        {
                            Getvehicle.EOCColor = "";
                            Getvehicle.EOC = dr["EOC"].ToString();
                        }

                        ////////////////////GOC Change.....................//////////////////////
                        int updateGOC = 0;
                        int.TryParse(dr["GOC"].ToString(), out updateGOC);
                        int serviceGOC = 0;
                        int.TryParse(drser["GOC"].ToString(), out serviceGOC);
                        int GOC = 0;
                        GOC = serviceGOC * 90 / 100;
                        if (updateGOC > GOC)
                        {
                            if (updateGOC > serviceGOC)
                            {
                                Getvehicle.GOCColor = "Red";
                                Getvehicle.GOC = dr["GOC"].ToString();
                            }
                            else
                            {
                                Getvehicle.GOCColor = "orange";
                                Getvehicle.GOC = dr["GOC"].ToString();
                            }
                        }
                        else
                        {
                            Getvehicle.GOCColor = "";
                            Getvehicle.GOC = dr["GOC"].ToString();
                        }

                        ////////////////////AFC Change.....................//////////////////////
                        int updateAFC = 0;
                        int.TryParse(dr["AFC"].ToString(), out updateAFC);
                        int serviceAFC = 0;
                        int.TryParse(drser["AFC"].ToString(), out serviceAFC);
                        int AFC = 0;
                        AFC = serviceAFC * 90 / 100;
                        if (updateAFC > AFC)
                        {
                            if (updateAFC > serviceAFC)
                            {
                                Getvehicle.AFCColor = "Red";
                                Getvehicle.AFC = dr["AFC"].ToString();
                            }
                            else
                            {
                                Getvehicle.AFCColor = "orange";
                                Getvehicle.AFC = dr["AFC"].ToString();
                            }
                        }
                        else
                        {
                            Getvehicle.AFCColor = "";
                            Getvehicle.AFC = dr["AFC"].ToString();
                        }

                        ////////////////////OFC Change.....................//////////////////////
                        int updateOFC = 0;
                        int.TryParse(dr["OFC"].ToString(), out updateOFC);
                        int serviceOFC = 0;
                        int.TryParse(drser["OFC"].ToString(), out serviceOFC);
                        int OFC = 0;
                        OFC = serviceOFC * 90 / 100;
                        if (updateOFC > OFC)
                        {
                            if (updateOFC > serviceOFC)
                            {
                                Getvehicle.OFCColor = "Red";
                                Getvehicle.OFC = dr["OFC"].ToString();
                            }
                            else
                            {
                                Getvehicle.OFCColor = "orange";
                                Getvehicle.OFC = dr["OFC"].ToString();
                            }
                        }
                        else
                        {
                            Getvehicle.OFCColor = "";
                            Getvehicle.OFC = dr["OFC"].ToString();
                        }

                        ////////////////////brake_fluid Change.....................//////////////////////
                        int updatebrake_fluid = 0;
                        int.TryParse(dr["brake_fluid"].ToString(), out updatebrake_fluid);
                        int servicebrake_fluid = 0;
                        int.TryParse(drser["brake_fluid"].ToString(), out servicebrake_fluid);
                        int brake_fluid = 0;
                        brake_fluid = servicebrake_fluid * 90 / 100;
                        if (updatebrake_fluid > brake_fluid)
                        {
                            if (updatebrake_fluid > servicebrake_fluid)
                            {
                                Getvehicle.brakefluidColor = "Red";
                                Getvehicle.brakefluid = dr["brake_fluid"].ToString();
                            }
                            else
                            {
                                Getvehicle.brakefluidColor = "orange";
                                Getvehicle.brakefluid = dr["brake_fluid"].ToString();
                            }
                        }
                        else
                        {
                            Getvehicle.brakefluidColor = "";
                            Getvehicle.brakefluid = dr["brake_fluid"].ToString();
                        }

                        ////////////////////brake_fluid Change.....................//////////////////////
                        int updatesteering_fluid = 0;
                        int.TryParse(dr["steering_fluid"].ToString(), out updatesteering_fluid);
                        int servicesteering_fluid = 0;
                        int.TryParse(drser["steering_fluid"].ToString(), out servicesteering_fluid);
                        int steering_fluid = 0;
                        steering_fluid = servicesteering_fluid * 90 / 100;
                        if (updatesteering_fluid > steering_fluid)
                        {
                            if (updatesteering_fluid > servicesteering_fluid)
                            {
                                Getvehicle.powersteeringfluidColor = "Red";
                                Getvehicle.powersteeringfluid = dr["steering_fluid"].ToString();
                            }
                            else
                            {
                                Getvehicle.powersteeringfluidColor = "orange";
                                Getvehicle.powersteeringfluid = dr["steering_fluid"].ToString();
                            }
                        }
                        else
                        {
                            Getvehicle.powersteeringfluidColor = "";
                            Getvehicle.powersteeringfluid = dr["steering_fluid"].ToString();
                        }

                        ////////////////////brake_fluid Change.....................//////////////////////
                        int updatetransmission_fluid = 0;
                        int.TryParse(dr["transmission_fluid"].ToString(), out updatetransmission_fluid);
                        int servicetransmission_fluid = 0;
                        int.TryParse(drser["transmission_fluid"].ToString(), out servicetransmission_fluid);
                        int transmission_fluid = 0;
                        transmission_fluid = servicetransmission_fluid * 90 / 100;
                        if (updatetransmission_fluid > transmission_fluid)
                        {
                            if (updatetransmission_fluid > servicetransmission_fluid)
                            {
                                Getvehicle.transmissionfluidColor = "Red";
                                Getvehicle.transmissionfluid = dr["transmission_fluid"].ToString();
                            }
                            else
                            {
                                Getvehicle.transmissionfluidColor = "orange";
                                Getvehicle.transmissionfluid = dr["transmission_fluid"].ToString();
                            }
                        }
                        else
                        {
                            Getvehicle.transmissionfluidColor = "";
                            Getvehicle.transmissionfluid = dr["transmission_fluid"].ToString();
                        }

                        ////////////////////brake_fluid Change.....................//////////////////////
                        int updatewasher_fluid = 0;
                        int.TryParse(dr["washer_fluid"].ToString(), out updatewasher_fluid);
                        int servicewasher_fluid = 0;
                        int.TryParse(drser["washer_fluid"].ToString(), out servicewasher_fluid);
                        int washer_fluid = 0;
                        washer_fluid = servicewasher_fluid * 90 / 100;
                        if (updatewasher_fluid > washer_fluid)
                        {
                            if (updatewasher_fluid > servicewasher_fluid)
                            {
                                Getvehicle.washerfluidColor = "Red";
                                Getvehicle.washerfluid = dr["washer_fluid"].ToString();
                            }
                            else
                            {
                                Getvehicle.washerfluidColor = "orange";
                                Getvehicle.washerfluid = dr["washer_fluid"].ToString();
                            }
                        }
                        else
                        {
                            Getvehicle.washerfluidColor = "";
                            Getvehicle.washerfluid = dr["washer_fluid"].ToString();
                        }

                        ////////////////////brake_fluid Change.....................//////////////////////
                        int updatewheel_bearings = 0;
                        int.TryParse(dr["wheel_bearings"].ToString(), out updatewheel_bearings);
                        int servicewheel_bearings = 0;
                        int.TryParse(drser["wheel_bearings"].ToString(), out servicewheel_bearings);
                        int wheel_bearings = 0;
                        wheel_bearings = servicewheel_bearings * 90 / 100;
                        if (updatewheel_bearings > wheel_bearings)
                        {
                            if (updatewheel_bearings > servicewheel_bearings)
                            {
                                Getvehicle.checkbrakesColor = "Red";
                                Getvehicle.checkbrakes = dr["wheel_bearings"].ToString();
                            }
                            else
                            {
                                Getvehicle.checkbrakesColor = "orange";
                                Getvehicle.checkbrakes = dr["wheel_bearings"].ToString();
                            }
                        }
                        else
                        {
                            Getvehicle.checkbrakesColor = "";
                            Getvehicle.checkbrakes = dr["wheel_bearings"].ToString();
                        }

                        ////////////////////brake_fluid Change.....................//////////////////////
                        int updatecheckleaks = 0;
                        int.TryParse(dr["checkleaks"].ToString(), out updatecheckleaks);
                        int servicecheckleaks = 0;
                        int.TryParse(drser["checkleaks"].ToString(), out servicecheckleaks);
                        int checkleaks = 0;
                        checkleaks = servicecheckleaks * 90 / 100;
                        if (updatecheckleaks > checkleaks)
                        {
                            if (updatecheckleaks > servicecheckleaks)
                            {
                                Getvehicle.checkleaksColor = "Red";
                                Getvehicle.checkleaks = dr["checkleaks"].ToString();
                            }
                            else
                            {
                                Getvehicle.checkleaksColor = "orange";
                                Getvehicle.checkleaks = dr["checkleaks"].ToString();
                            }
                        }
                        else
                        {
                            Getvehicle.checkleaksColor = "";
                            Getvehicle.checkleaks = dr["checkleaks"].ToString();
                        }

                        ////////////////////brake_fluid Change.....................//////////////////////
                        int updatebelts_hoses = 0;
                        int.TryParse(dr["belts_hoses"].ToString(), out updatebelts_hoses);
                        int servicebelts_hoses = 0;
                        int.TryParse(drser["belts_hoses"].ToString(), out servicebelts_hoses);
                        int belts_hoses = 0;
                        belts_hoses = servicebelts_hoses * 90 / 100;
                        if (updatebelts_hoses > belts_hoses)
                        {
                            if (updatebelts_hoses > servicebelts_hoses)
                            {
                                Getvehicle.allbeltsColor = "Red";
                                Getvehicle.allbelts = dr["belts_hoses"].ToString();
                            }
                            else
                            {
                                Getvehicle.allbeltsColor = "orange";
                                Getvehicle.allbelts = dr["belts_hoses"].ToString();
                            }
                        }
                        else
                        {
                            Getvehicle.allbeltsColor = "";
                            Getvehicle.allbelts = dr["belts_hoses"].ToString();
                        }

                        ////////////////////brake_fluid Change.....................//////////////////////
                        int updatelubricate_chasis = 0;
                        int.TryParse(dr["lubricate_chasis"].ToString(), out updatelubricate_chasis);
                        int servicelubricate_chasis = 0;
                        int.TryParse(drser["lubricate_chasis"].ToString(), out servicelubricate_chasis);
                        int lubricate_chasis = 0;
                        lubricate_chasis = servicelubricate_chasis * 90 / 100;
                        if (updatelubricate_chasis > lubricate_chasis)
                        {
                            if (updatelubricate_chasis > servicelubricate_chasis)
                            {
                                Getvehicle.lubricatechassisColor = "Red";
                                Getvehicle.lubricatechassis = dr["lubricate_chasis"].ToString();
                            }
                            else
                            {
                                Getvehicle.lubricatechassisColor = "orange";
                                Getvehicle.lubricatechassis = dr["lubricate_chasis"].ToString();
                            }
                        }
                        else
                        {
                            Getvehicle.lubricatechassisColor = "";
                            Getvehicle.lubricatechassis = dr["lubricate_chasis"].ToString();
                        }
                        Vehicleslist.Add(Getvehicle);
                    }
                }
                string response = GetJson(Vehicleslist);
                context.Response.Write(response);
            }
        }
        catch
        {
        }
    }
    public void testmail(HttpContext context)
    {
        string toAddress = context.Request["mail"];
        string subject = "Trip Details";
        string result = "Success";
        string senderID = "vyshnavidairyinfo@gmail.com";// use sender's email id here..
        const string senderPassword = "vyshnavi123"; // sender password here...
        try
        {
            SmtpClient smtp = new SmtpClient
            {
                Host = "smtp.gmail.com", // smtp server address here...
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new System.Net.NetworkCredential(senderID, senderPassword),
                Timeout = 30000,
            };

            MailMessage message = new MailMessage(senderID, toAddress, subject, "hii");
            message.IsBodyHtml = true;
            smtp.Send(message);
            string msg = "send Mail";
            string response = GetJson(msg);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            result = "Error sending data please try again.!!!";
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
        //SendEmail(toAddress, subject, body);
    }
    private void GetSubPaybleValues(HttpContext context)
    {

        try
        {
            vdm = new VehicleDBMgr();
            string Maintace_id = context.Request["Maintenance_sno"];
            string branchid = "0";
            branchid = context.Session["Branch_ID"].ToString();
            cmd = new MySqlCommand("SELECT sno FROM veh_exp WHERE (Maintace_id = @Maintace_id) AND (BranchID = @BranchID)");
            cmd.Parameters.Add("@Maintace_id", Maintace_id);
            cmd.Parameters.Add("@BranchID", branchid);
            DataTable dtCash = vdm.SelectQuery(cmd).Tables[0];
            string RefNo = dtCash.Rows[0]["sno"].ToString();
            List<Subpayables> SubpayableList = new List<Subpayables>();
            cmd = new MySqlCommand("SELECT sub_veh_exp.amount, head_master.sno, head_master.head_desc FROM sub_veh_exp INNER JOIN head_master ON sub_veh_exp.head_sno = head_master.sno WHERE (sub_veh_exp.refno = @Refno) ");
            cmd.Parameters.Add("@RefNo", RefNo);
            DataTable dtCashSubPayable = vdm.SelectQuery(cmd).Tables[0];
            foreach (DataRow dr in dtCashSubPayable.Rows)
            {
                Subpayables GetSubpayable = new Subpayables();
                GetSubpayable.HeadSno = dr["sno"].ToString();
                GetSubpayable.HeadOfAccount = dr["head_desc"].ToString();
                GetSubpayable.Amount = dr["amount"].ToString();
                SubpayableList.Add(GetSubpayable);
            }
            string response = GetJson(SubpayableList);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }
    private void GetIdle_Vehicle_Deatails(HttpContext context)
    {
        try
        {
            vdm = new VehicleDBMgr();
            string BranchID = context.Session["Branch_ID"].ToString();
            DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
            List<Availablehiclecls> Vehicleslist = new List<Availablehiclecls>();
                cmd = new MySqlCommand("SELECT vehicel_master.vm_sno, vehicel_master.registration_no, vehicel_master.door_no, vehicel_master.status, vehicel_master.branch_id, vehicel_master.operatedby, vehicel_master.Capacity, minimasters.mm_name AS veh_type, minimasters_1.mm_name AS veh_make, vehicel_master.fuel_capacity, vehicel_master.axils_refno, axil_master.axilmaster_name FROM vehicel_master INNER JOIN minimasters ON vehicel_master.vhtype_refno = minimasters.sno INNER JOIN minimasters minimasters_1 ON vehicel_master.vhmake_refno = minimasters_1.sno LEFT OUTER JOIN axil_master ON vehicel_master.axils_refno = axil_master.sno WHERE  (vehicel_master.branch_id = @branch_id) AND (vehicel_master.vm_owner = @Owner) AND (vehicel_master.vm_sno NOT IN (SELECT vehicleno FROM tripdata WHERE (tripdate BETWEEN @d1 AND @d2)))");
                cmd.Parameters.Add("@Owner", context.Session["shortname"].ToString());
            cmd.Parameters.Add("@d1", GetLowDate(ServerDateCurrentdate.AddDays(-15)));
            cmd.Parameters.Add("@d2", GetHighDate(ServerDateCurrentdate));
            cmd.Parameters.Add("@branch_id", BranchID);
            DataTable trips = vdm.SelectQuery(cmd).Tables[0];
            if (trips.Rows.Count > 0)
            {
                foreach (DataRow dr in trips.Rows)
                {
                    Availablehiclecls Getvehicle = new Availablehiclecls();
                    Getvehicle.vehicleno = dr["registration_no"].ToString();
                    string type = dr["veh_type"].ToString();
                    if (type == "Puff" || type == "Tanker")
                    {
                        Getvehicle.type = dr["veh_type"].ToString();
                        Getvehicle.make = dr["veh_make"].ToString();
                        Vehicleslist.Add(Getvehicle);
                    }
                }
                string response = GetJson(Vehicleslist);
                context.Response.Write(response);
            }
        }
        catch
        {
        }
    }
    private void Getrunning_Vehicle_Deatails(HttpContext context)
    {
        try
        {
            vdm = new VehicleDBMgr();
            string BranchID = context.Session["Branch_ID"].ToString();
            DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
            List<Availablehiclecls> Vehicleslist = new List<Availablehiclecls>();
            cmd = new MySqlCommand("SELECT vehicel_master.vm_sno, vehicel_master.registration_no, minimasters.mm_name AS veh_type, minimasters_1.mm_name AS veh_make, vehicel_master.fuel_capacity, vehicel_master.axils_refno, axil_master.axilmaster_name, tripdata.routeid, tripdata.tripdate FROM vehicel_master INNER JOIN minimasters ON vehicel_master.vhtype_refno = minimasters.sno INNER JOIN minimasters minimasters_1 ON vehicel_master.vhmake_refno = minimasters_1.sno INNER JOIN tripdata ON vehicel_master.vm_sno = tripdata.vehicleno LEFT OUTER JOIN  axil_master ON vehicel_master.axils_refno = axil_master.sno WHERE  (vehicel_master.branch_id = @branch_id) AND (tripdata.status = 'A')");
            cmd.Parameters.Add("@branch_id", BranchID);
            DataTable trips = vdm.SelectQuery(cmd).Tables[0];
            if (trips.Rows.Count > 0)
            {
                foreach (DataRow dr in trips.Rows)
                {
                    Availablehiclecls Getvehicle = new Availablehiclecls();
                    Getvehicle.vehicleno = dr["registration_no"].ToString();
                    string type = dr["veh_type"].ToString();
                    if (type == "Puff" || type == "Tanker")
                    {
                        Getvehicle.type = dr["veh_type"].ToString();
                        Getvehicle.route = dr["routeid"].ToString();
                        Vehicleslist.Add(Getvehicle);
                    }
                }
                string response = GetJson(Vehicleslist);
                context.Response.Write(response);
            }
        }
        catch
        {
        }
    }
    private void Getavailable_Vehicle_Deatails(HttpContext context)
    {
        try
        {
            vdm = new VehicleDBMgr();
            string BranchID = context.Session["Branch_ID"].ToString();
            DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
            List<Availablehiclecls> Vehicleslist = new List<Availablehiclecls>();
                cmd = new MySqlCommand("SELECT vehicel_master.vm_sno, vehicel_master.registration_no, vehicel_master.door_no, vehicel_master.status, vehicel_master.branch_id, vehicel_master.operatedby, vehicel_master.Capacity, minimasters.mm_name AS veh_type, minimasters_1.mm_name AS veh_make, vehicel_master.fuel_capacity, vehicel_master.axils_refno, axil_master.axilmaster_name FROM vehicel_master INNER JOIN minimasters ON vehicel_master.vhtype_refno = minimasters.sno INNER JOIN minimasters minimasters_1 ON vehicel_master.vhmake_refno = minimasters_1.sno LEFT OUTER JOIN axil_master ON vehicel_master.axils_refno = axil_master.sno WHERE (vehicel_master.branch_id = @branch_id) AND  (vehicel_master.vm_owner=@Owner) AND (vehicel_master.vm_sno NOT IN (SELECT vehicleno FROM tripdata WHERE (status = 'A')))");
                cmd.Parameters.Add("@Owner", context.Session["shortname"].ToString());
            cmd.Parameters.Add("@branch_id", BranchID);
            DataTable trips = vdm.SelectQuery(cmd).Tables[0];
            if (trips.Rows.Count > 0)
            {
                foreach (DataRow dr in trips.Rows)
                {
                    Availablehiclecls Getvehicle = new Availablehiclecls();
                    Getvehicle.vehicleno = dr["registration_no"].ToString();
                    string type = dr["veh_type"].ToString();
                    if (type == "Puff" || type == "Tanker")
                    {
                        Getvehicle.type = dr["veh_type"].ToString();
                        Getvehicle.make = dr["veh_make"].ToString(); 
                        Vehicleslist.Add(Getvehicle);
                    }
                }
                string response = GetJson(Vehicleslist);
                context.Response.Write(response);
            }
        }
        catch
        {
        }
    }
    private void GetVehicle_MileageDeatails(HttpContext context)
    {
        try
        {
            vdm = new VehicleDBMgr();
            string BranchID = context.Session["Branch_ID"].ToString();
            DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
            List<VehicleMileagecls> Vehicleslist = new List<VehicleMileagecls>();
            cmd = new MySqlCommand("SELECT derivedtbl_1.tripsheetno AS TripSheet,DATE_FORMAT( derivedtbl_1.tripdate,'%d-%m-%Y %h:%i %p') AS StartDate,DATE_FORMAT( derivedtbl_1.enddate,'%d-%m-%Y %h:%i %p') AS EndDate,derivedtbl_1.gpskms, derivedtbl_1.registration_no AS VehicleNo,derivedtbl_1.VehicleType,derivedtbl_1.Make,derivedtbl_1.Capacity,derivedtbl_1.TripKMS, derivedtbl_1.loadtype AS LoadType, derivedtbl_1.routeid AS RouteName, derivedtbl_1.endfuelvalue AS InsideFuel, SUM(triplogs.fuel) AS OutsideFuel,derivedtbl_1.Qty,IFNULL(ROUND(derivedtbl_1.TripKMS / (derivedtbl_1.endfuelvalue + SUM(triplogs.fuel)), 2),0) AS TodayMileage FROM (SELECT        tripdata.tripsheetno, tripdata.tripdate, tripdata.enddate, vehicel_master.registration_no, tripdata.endodometerreading - tripdata.vehiclestartreading AS TripKMS,tripdata.gpskms, tripdata.loadtype, tripdata.qty, tripdata.routeid,vehicel_master.Capacity, tripdata.sno, tripdata.endfuelvalue, minimasters.mm_name AS VehicleType, minimasters_1.mm_code AS Make FROM tripdata INNER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno INNER JOIN minimasters ON vehicel_master.vhtype_refno = minimasters.sno INNER JOIN  minimasters minimasters_1 ON vehicel_master.vhmake_refno = minimasters_1.sno WHERE (tripdata.enddate BETWEEN @d1 AND @d2) AND (tripdata.userid = @BranchID)  AND (tripdata.status = 'C')) derivedtbl_1 INNER JOIN triplogs ON derivedtbl_1.sno = triplogs.tripsno AND triplogs.fuel_type <> 'OWN' GROUP BY derivedtbl_1.tripsheetno");
            cmd.Parameters.Add("@d1", GetLowDate(ServerDateCurrentdate).AddDays(-1));
            cmd.Parameters.Add("@d2", GetHighDate(ServerDateCurrentdate).AddDays(-1));
            cmd.Parameters.Add("@BranchID", BranchID);
            DataTable trips = vdm.SelectQuery(cmd).Tables[0];
            if (trips.Rows.Count > 0)
            {
                foreach (DataRow dr in trips.Rows)
                {
                    VehicleMileagecls Getvehicle = new VehicleMileagecls();
                    Getvehicle.vehicleno = dr["VehicleNo"].ToString();
                    Getvehicle.routename = dr["RouteName"].ToString();
                    double mileage = 0;
                    double.TryParse(dr["TodayMileage"].ToString(), out mileage);
                    if (mileage < 4)
                    {
                        Getvehicle.mileage = mileage.ToString();
                        Vehicleslist.Add(Getvehicle);
                    }
                }
                string response = GetJson(Vehicleslist);
                context.Response.Write(response);
            }
        }
        catch
        {
        }
    }
    private void GetVehicleExpDeatails(HttpContext context)
    {
        try
        {
            vdm = new VehicleDBMgr();
            string BranchID = context.Session["Branch_ID"].ToString();
            DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
                cmd = new MySqlCommand("SELECT minimasters.mm_name AS VehicelType, minimasters_1.mm_name AS Make, vehicel_master.registration_no, vehicel_master.Capacity, vehicel_master.vm_model, vehicel_master.vm_engine, vehicel_master.vm_chasiss, vehicel_master.vm_owner, vehicel_master.vm_rcexpdate, vehicel_master.vm_poll_exp_date, vehicel_master.vm_isurence_exp_date, vehicel_master.vm_fit_exp_date,vehicel_master.vm_roadtax_exp_date, vehicel_master.fuel_capacity FROM vehicel_master INNER JOIN minimasters ON vehicel_master.vhtype_refno = minimasters.sno INNER JOIN minimasters minimasters_1 ON vehicel_master.vhmake_refno = minimasters_1.sno WHERE (vehicel_master.branch_id = @BranchID) and (vehicel_master.vm_owner=@Owner) GROUP BY vehicel_master.registration_no order by DATE(vehicel_master.vm_rcexpdate), DATE(vehicel_master.vm_poll_exp_date), DATE(vehicel_master.vm_isurence_exp_date), DATE(vehicel_master.vm_fit_exp_date),DATE(vehicel_master.vm_roadtax_exp_date)");
                cmd.Parameters.Add("@Owner", context.Session["shortname"].ToString());
            cmd.Parameters.Add("@BranchID", BranchID);
            DataTable dtVehicles = vdm.SelectQuery(cmd).Tables[0];
            List<vehiclesclass> Vehicleslist = new List<vehiclesclass>();
            if (dtVehicles.Rows.Count > 0)
            {
                foreach (DataRow dr in dtVehicles.Rows)
                {
                    vehiclesclass Getvehicle = new vehiclesclass();
                    Getvehicle.vehicleno = dr["registration_no"].ToString();
                    Getvehicle.vehiclemake = dr["Make"].ToString();
                    Getvehicle.vehiclemodel = dr["vm_model"].ToString();
                    Getvehicle.vehicletype = dr["VehicelType"].ToString();
                    Getvehicle.Capacity = dr["Capacity"].ToString();
                    Getvehicle.FuelCapacity = dr["fuel_capacity"].ToString();
                    Getvehicle.EngineNo = dr["vm_engine"].ToString();
                    Getvehicle.ChasisNo = dr["vm_chasiss"].ToString();
                    string InsDate = dr["vm_isurence_exp_date"].ToString();
                    if (InsDate == "")
                    {
                        Getvehicle.InsExpDate = "";
                        Getvehicle.InsExpcolor = "Red";
                    }
                    else
                    {
                        DateTime dtInsDate = Convert.ToDateTime(InsDate);
                        TimeSpan dateSpan = dtInsDate.Subtract(ServerDateCurrentdate);
                        int NoOfdays = dateSpan.Days;
                        if (NoOfdays < 30)
                        {
                            if (NoOfdays < 0)
                            {
                                Getvehicle.InsExpcolor = "Red";
                                string InsExpdate = dtInsDate.ToString("dd/MMM/yyyy");
                                Getvehicle.InsExpDate = InsExpdate;
                                //Vehicleslist.Add(Getvehicle);
                            }
                            else
                            {
                                Getvehicle.InsExpcolor = "orange";
                                string InsExpdate = dtInsDate.ToString("dd/MMM/yyyy");
                                Getvehicle.InsExpDate = InsExpdate;
                                //Vehicleslist.Add(Getvehicle);
                            }
                        }

                    }
                    string FitDate = dr["vm_fit_exp_date"].ToString();
                    if (FitDate == "")
                    {
                        Getvehicle.FitExpDate = "";
                        Getvehicle.FitExpcolor = "Red";
                    }
                    else
                    {
                        DateTime dtFitDate = Convert.ToDateTime(FitDate);
                        TimeSpan dateSpan = dtFitDate.Subtract(ServerDateCurrentdate);
                        int NoOfdays = dateSpan.Days;
                        if (NoOfdays < 30)
                        {
                            if (NoOfdays < 0)
                            {
                                Getvehicle.FitExpcolor = "Red";
                                string FitExpdate = dtFitDate.ToString("dd/MMM/yyyy");
                                Getvehicle.FitExpDate = FitExpdate;
                                //Vehicleslist.Add(Getvehicle);
                            }
                            else
                            {
                                Getvehicle.FitExpcolor = "orange";
                                string FitExpdate = dtFitDate.ToString("dd/MMM/yyyy");
                                Getvehicle.FitExpDate = FitExpdate;
                                //Vehicleslist.Add(Getvehicle);
                            }
                        }

                    }
                    string RoadTaxDate = dr["vm_roadtax_exp_date"].ToString();
                    if (RoadTaxDate == "")
                    {
                        Getvehicle.RoadTaxExpDate = "";
                        Getvehicle.RoadTaxExpcolor = "Red";
                    }
                    else
                    {
                        DateTime dtRoadTaxDate = Convert.ToDateTime(RoadTaxDate);
                        TimeSpan dateSpan = dtRoadTaxDate.Subtract(ServerDateCurrentdate);
                        int NoOfdays = dateSpan.Days;
                        if (NoOfdays < 30)
                        {
                            if (NoOfdays < 0)
                            {
                                Getvehicle.RoadTaxExpcolor = "Red";
                                string RoadTaxExpdate = dtRoadTaxDate.ToString("dd/MMM/yyyy");
                                Getvehicle.RoadTaxExpDate = RoadTaxExpdate;
                                //Vehicleslist.Add(Getvehicle);
                            }
                            else
                            {
                                Getvehicle.RoadTaxExpcolor = "orange";
                                string RoadTaxExpdate = dtRoadTaxDate.ToString("dd/MMM/yyyy");
                                Getvehicle.RoadTaxExpDate = RoadTaxExpdate;
                                //Vehicleslist.Add(Getvehicle);
                            }
                        }
                    }
                    string RCDate = dr["vm_rcexpdate"].ToString();
                    if (RCDate == "")
                    {
                        Getvehicle.RCExpDate = "";
                        Getvehicle.RCExpcolor = "Red";
                    }
                    else
                    {
                        DateTime dtRCDate = Convert.ToDateTime(RCDate);
                        TimeSpan dateSpan = dtRCDate.Subtract(ServerDateCurrentdate);
                        int NoOfdays = dateSpan.Days;
                        if (NoOfdays < 30)
                        {
                            if (NoOfdays < 0)
                            {
                                Getvehicle.RCExpcolor = "Red";
                                string RCExpdate = dtRCDate.ToString("dd/MMM/yyyy");
                                Getvehicle.RCExpDate = RCExpdate;
                                //Vehicleslist.Add(Getvehicle);
                            }
                            else
                            {
                                Getvehicle.RCExpcolor = "orange";
                                string RCExpdate = dtRCDate.ToString("dd/MMM/yyyy");
                                Getvehicle.RCExpDate = RCExpdate;
                                //Vehicleslist.Add(Getvehicle);
                            }
                        }

                    }
                    Vehicleslist.Add(Getvehicle);
                }
                string response = GetJson(Vehicleslist);
                context.Response.Write(response);
            }
        }
        catch
        {
        }
    }
    private void GetSubmaintenses(HttpContext context)
    {
        try
        {
            vdm = new VehicleDBMgr();
            string BranchID = context.Session["Branch_ID"].ToString();
            string MaintenanceID = context.Request["MaintenanceID"];
            List<Subpayables> SubpayableList = new List<Subpayables>();
            cmd = new MySqlCommand("SELECT head_master.head_desc,sub_veh_exp.head_sno, sub_veh_exp.amount FROM head_master INNER JOIN sub_veh_exp ON head_master.sno = sub_veh_exp.head_sno INNER JOIN veh_exp ON sub_veh_exp.refno = veh_exp.sno WHERE (veh_exp.branchid = @BranchID) AND (veh_exp.Maintace_id = @Maintace_id)");
            cmd.Parameters.Add("@BranchID", BranchID);
            cmd.Parameters.Add("@Maintace_id", MaintenanceID);
            DataTable dtble = vdm.SelectQuery(cmd).Tables[0];
            int i = 1;
            foreach (DataRow dr in dtble.Rows)
            {
                Subpayables GetSubpayable = new Subpayables();
                GetSubpayable.HeadSno = i++.ToString();
                GetSubpayable.HeadOfAccount = dr["head_desc"].ToString();
                GetSubpayable.Amount = dr["amount"].ToString();
                SubpayableList.Add(GetSubpayable);
            }
            string response = GetJson(SubpayableList);
            context.Response.Write(response);
        }
        catch
        {
        }
    }
    private void btnMaintenancePrintClick(HttpContext context)
    {
        try
        {
            context.Session["MaintenanceID"] = context.Request["MaintenanceID"];

        }
        catch
        {
        }
    }
    private void GetMaintenance_list(HttpContext context)
    {
        try
        {
            vdm = new VehicleDBMgr();
            string BranchID = context.Session["Branch_ID"].ToString();
            string FDate = context.Request["FromDate"];
            DateTime FromDate = Convert.ToDateTime(FDate);
            string TDate = context.Request["ToDate"];
            DateTime Todate = Convert.ToDateTime(TDate);
            cmd = new MySqlCommand("SELECT vehicel_master.registration_no, veh_exp.name, veh_exp.sno, veh_exp.incharge, veh_exp.amount, veh_exp.remarks,veh_exp.Maintace_id, veh_exp.vehsno FROM veh_exp INNER JOIN vehicel_master ON veh_exp.vehsno = vehicel_master.vm_sno WHERE (veh_exp.branchid = @BranchID) AND (veh_exp.doe BETWEEN @d1 AND @d2)");
            cmd.Parameters.Add("@branchid", BranchID);
            cmd.Parameters.Add("@d1", GetLowDate(FromDate));
            cmd.Parameters.Add("@d2", GetHighDate(Todate));
            DataTable dtble = vdm.SelectQuery(cmd).Tables[0];
            List<Veh_exp_cls> explist = new List<Veh_exp_cls>();
            int i = 1;
            foreach (DataRow dr in dtble.Rows)
            {
                Veh_exp_cls gethead = new Veh_exp_cls();
                gethead.sno = i++.ToString();
                gethead.maintace_code = dr["sno"].ToString();
                gethead.maintace_id = dr["Maintace_id"].ToString();
                gethead.vehicleno = dr["registration_no"].ToString();
                gethead.veh_sno = dr["vehsno"].ToString();
                gethead.name = dr["name"].ToString();
                gethead.incharge = dr["incharge"].ToString();
                gethead.amount = dr["amount"].ToString();
                gethead.remarks = dr["remarks"].ToString();
                explist.Add(gethead);
            }
            string respnceString = GetJson(explist);
            context.Response.Write(respnceString);
        }
        catch
        {
        }
    }
    class Head_mastercls
    {
        public string sno { get; set; }
        public string desc { get; set; }
        public string status { get; set; }
        public string refno { get; set; }
        public string accounttype { get; set; }
    }
    class VehicleExpendaturecls
    {
        public string doe { get; set; }
        public string vehicleno { get; set; }
        public string Amount { get; set; }
        public string name { get; set; }
        public string Incharge { get; set; }
        public string Remarks { get; set; }
        public string btnSave { get; set; }
        public string refno { get; set; }

        public List<Vehicleheadcls> Cashdetails { get; set; }
    }
    class Vehicleheadcls
    {
        public string headsno { get; set; }
        public string Account { get; set; }
        public string amount { get; set; }
    }
    private void BtnRaiseVehicleExpendature_saveclick(VehicleExpendaturecls obj, HttpContext context)
    {
        try
        {
            vdm = new VehicleDBMgr();
            string BranchID = context.Session["Branch_ID"].ToString();
            string UserID = context.Session["Employ_Sno"].ToString();
            string doe = obj.doe;
            DateTime Currentdate = Convert.ToDateTime(doe);
            string vehicleno = obj.vehicleno;
            string Amount = obj.Amount;
            double expamount = 0;
            double.TryParse(Amount, out expamount);
            string name = obj.name;
            string Incharge = obj.Incharge;
            string Remarks = obj.Remarks;
            DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
            //DateTime Currentdate = DateTime.Now;
            string btnSave = obj.btnSave;
            if (btnSave == "Raise")
            {
                cmd = new MySqlCommand("Select IFNULL(MAX(Maintace_id),0)+1 as Sno  from veh_exp where branchid=@branchid");
                cmd.Parameters.Add("@branchid", BranchID);
                DataTable dtTripId = vdm.SelectQuery(cmd).Tables[0];
                string Maintaceid = dtTripId.Rows[0]["Sno"].ToString();
                cmd = new MySqlCommand("insert into veh_exp(vehsno,name,incharge,doe,branchid,amount,entry_by,Maintace_id,remarks,create_date) values(@vehsno,@name,@incharge,@doe,@branchid,@amount,@entry_by,@Maintace_id,@remarks,@create_date)");
                cmd.Parameters.Add("@vehsno", vehicleno);
                cmd.Parameters.Add("@name", name);
                cmd.Parameters.Add("@incharge", Incharge);
                cmd.Parameters.Add("@doe", Currentdate);
                cmd.Parameters.Add("@branchid", BranchID);
                cmd.Parameters.Add("@amount", Amount);
                cmd.Parameters.Add("@entry_by", UserID);
                cmd.Parameters.Add("@Maintace_id", Maintaceid);
                cmd.Parameters.Add("@remarks", Remarks);
                cmd.Parameters.Add("@create_date", ServerDateCurrentdate);
                long refno = vdm.insertScalar(cmd);
                foreach (Vehicleheadcls vds in obj.Cashdetails)
                {
                    double headamount = 0;
                    double.TryParse(vds.amount, out headamount);
                    cmd = new MySqlCommand("insert into sub_veh_exp (head_sno,amount,refno) values (@head_sno,@amount,@refno)");
                    cmd.Parameters.Add("@head_sno", vds.headsno);
                    cmd.Parameters.Add("@amount", headamount);
                    cmd.Parameters.Add("@refno", refno);
                    vdm.insert(cmd);
                }
                string msg = "Transaction successfully saved";
                string response = GetJson(msg);
                context.Response.Write(response);
            }
            else
            {
                string refno = obj.refno;
                cmd = new MySqlCommand("update veh_exp set vehsno=@vehsno,name=@name,incharge=@incharge,amount=@amount,remarks=@remarks where sno=@sno");
                cmd.Parameters.Add("@vehsno", vehicleno);
                cmd.Parameters.Add("@name", name);
                cmd.Parameters.Add("@incharge", Incharge);
                cmd.Parameters.Add("@remarks", Remarks);
                cmd.Parameters.Add("@amount", Amount);
                cmd.Parameters.Add("@sno", refno);
                vdm.Update(cmd);
                cmd = new MySqlCommand("delete from sub_veh_exp where refno=@refno");
                cmd.Parameters.Add("@refno", refno);
                vdm.Delete(cmd);
                foreach (Vehicleheadcls vds in obj.Cashdetails)
                {
                    double headamount = 0;
                    double.TryParse(vds.amount, out headamount);
                    cmd = new MySqlCommand("insert into sub_veh_exp (head_sno,amount,refno) values (@head_sno,@amount,@refno)");
                    cmd.Parameters.Add("@head_sno", vds.headsno);
                    cmd.Parameters.Add("@amount", headamount);
                    cmd.Parameters.Add("@refno", refno);
                    vdm.insert(cmd);
                }
                string msg = "Transaction successfully updated";
                string response = GetJson(msg);
                context.Response.Write(response);

            }
        }
        catch
        {
        }
    }
    private void get_head_master_List(HttpContext context)
    {
        try
        {
            vdm = new VehicleDBMgr();
            string BranchID = context.Session["Branch_ID"].ToString();
            cmd = new MySqlCommand("SELECT sno, head_desc, status,account_type FROM head_master WHERE  (branchid = @branchid)");
            cmd.Parameters.Add("@branchid", BranchID);
            DataTable dtble = vdm.SelectQuery(cmd).Tables[0];
            List<Head_mastercls> headlist = new List<Head_mastercls>();
            int i = 1;
            foreach (DataRow dr in dtble.Rows)
            {
                Head_mastercls gethead = new Head_mastercls();
                gethead.sno = i++.ToString();
                gethead.desc = dr["head_desc"].ToString();
                gethead.status = dr["status"].ToString();
                gethead.refno = dr["sno"].ToString();
                gethead.accounttype = dr["account_type"].ToString();
                headlist.Add(gethead);
            }
            string respnceString = GetJson(headlist);
            context.Response.Write(respnceString);
        }
        catch
        {
        }
    }
    private void head_master_saveclick(HttpContext context)
    {
        try
        {
            vdm = new VehicleDBMgr();
            string BranchID = context.Session["Branch_ID"].ToString();
            string description = context.Request["description"];
            string status = context.Request["status"];
            string operation = context.Request["operation"];
            string accounttype = context.Request["accounttype"];
            DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
            string msg = "";
            if (operation == "SAVE")
            {
                cmd = new MySqlCommand("insert into head_master(head_desc,branchid,status,account_type) values(@head_desc,@branchid,@status,@account_type)");
                cmd.Parameters.Add("@head_desc", description);
                cmd.Parameters.Add("@branchid", BranchID);
                cmd.Parameters.Add("@status", status);
                cmd.Parameters.Add("@account_type", accounttype);
                vdm.insert(cmd);
                msg = "Head created successfully";
            }
            else
            {
                string Headsno = context.Request["Headsno"];
                cmd = new MySqlCommand("update head_master set head_desc=@head_desc,account_type=@account_type, status=@status where branchid=@branchid and sno=@sno");
                cmd.Parameters.Add("@head_desc", description);
                cmd.Parameters.Add("@branchid", BranchID);
                cmd.Parameters.Add("@status", status);
                cmd.Parameters.Add("@sno", Headsno);
                cmd.Parameters.Add("@account_type", accounttype);
                vdm.Update(cmd);
                msg = "Head updated successfully";
            }
            string response = GetJson(msg);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string msg = ex.Message;
            string response = GetJson(msg);
            context.Response.Write(response);
        }
    }
    class Insurancecls
    {
        public string InsSno { get; set; }
        public string InsuranceName { get; set; }
    }
    class Dispatchcls
    {
        public string DispSno { get; set; }
        public string DispName { get; set; }
    }
    class GetEditTripvalues
    {
        public string Sno { get; set; }
        public string VehicleSno { get; set; }
        public string DriverID { get; set; }
        public string HelperID { get; set; }
        public string RouteID { get; set; }
        public string StartFrom { get; set; }
        public string VehStartReading { get; set; }
        public string HrReading { get; set; }
        public string LoadType { get; set; }
        public string Qty { get; set; }
        public string EndOdometerReading { get; set; }
        public string Fuelfilled { get; set; }
        public string PumpReading { get; set; }
        public string Token { get; set; }
        public string Remarks { get; set; }
        public string tripdate { get; set; }
        public string enddate { get; set; }

        public string refrigerationfuel { get; set; }

        public string endhrreading { get; set; }
    }
    public class vehiclesclass
    {
        public string vehicleno { get; set; }
        public string vehiclemake { get; set; }
        public string vehiclemodel { get; set; }
        public string vehicletype { get; set; }
        public string Routename { get; set; }
        public string Model { get; set; }
        public string Make { get; set; }
        public string TripSheetNo { get; set; }
        public string GpsKms { get; set; }
        public string TripDate { get; set; }
        public string Driver { get; set; }
        public string TripSno { get; set; }
        public string Capacity { get; set; }
        public string FuelCapacity { get; set; }
        public string EngineNo { get; set; }
        public string ChasisNo { get; set; }
        public string RCExpDate { get; set; }
        public string RCExpcolor { get; set; }
        public string InsExpDate { get; set; }
        public string InsExpcolor { get; set; }
        public string FitExpDate { get; set; }
        public string FitExpcolor { get; set; }
        public string PolExpDate { get; set; }
        public string PolExpcolor { get; set; }
        public string RoadTaxExpDate { get; set; }
        public string RoadTaxExpcolor { get; set; }

        public string odometer { get; set; }
    }

    public class LineChartValuesclass
    {
        public List<string> Mileageltr { get; set; }
        public List<string> ActMileage { get; set; }
        public List<string> Status { get; set; }
        public string TripDate { get; set; }
        public string AvgLtr { get; set; }
    }
    public class vehciclemakecls
    {
        public string sno { get; set; }
        public string make { get; set; }
    }
    public class personalcls
    {
        public string sno { get; set; }
        public string name { get; set; }
        public string phoneno { get; set; }
        public string emailid { get; set; }
        public string alert_type { get; set; }
        public string designation { get; set; }
    }
    public class Vehicledocuments
    {
        public string sno { get; set; }
        public string vehicleno { get; set; }
        public string type { get; set; }
        public string make { get; set; }
        public string model { get; set; }
        public string capacity { get; set; }
        public string insurance { get; set; }
        public string pollution { get; set; }
        public string fitness { get; set; }
        public string roadtax { get; set; }
        public string permit { get; set; }
        public List<SubVehicle> SubVehiclelist { get; set; }
    }
    //vehicleno, permit_expdate, pol_expdate, ins_expdate, fitness_expdate, roadtax_expdate, state_permit_expdate, state_roadtax_expdate
    public class SubVehicle
    {
        public string sno { get; set; }
        public string permit_expdate { get; set; }
        public string pol_expdate { get; set; }
        public string ins_expdate { get; set; }
        public string fitness_expdate { get; set; }
        public string roadtax_expdate { get; set; }
        public string state_permit_expdate { get; set; }
        public string state_roadtax_expdate { get; set; }
    }
    private void get_Vehciledocuments_data(HttpContext context)
    {
        try
        {
            vdm = new VehicleDBMgr();
            string BranchID = context.Session["Branch_ID"].ToString();
            cmd = new MySqlCommand("SELECT minimasters.mm_name AS VehicelType, minimasters_1.mm_name AS Make,vehicel_master.vm_sno, vehicel_master.registration_no, vehicel_master.Capacity, vehicel_master.vm_model, vehicel_master.vm_engine, vehicel_master.vm_chasiss, vehicel_master.vm_owner, vehicel_master.vm_rcexpdate, vehicel_master.vm_poll_exp_date, vehicel_master.vm_isurence_exp_date, vehicel_master.vm_fit_exp_date,vehicel_master.vm_roadtax_exp_date, vehicel_master.fuel_capacity FROM vehicel_master INNER JOIN minimasters ON vehicel_master.vhtype_refno = minimasters.sno INNER JOIN minimasters minimasters_1 ON vehicel_master.vhmake_refno = minimasters_1.sno WHERE (vehicel_master.branch_id = @BranchID) and (vehicel_master.vm_owner=@Owner) group by vehicel_master.registration_no order by DATE(vehicel_master.vm_rcexpdate), DATE(vehicel_master.vm_poll_exp_date), DATE(vehicel_master.vm_isurence_exp_date), DATE(vehicel_master.vm_fit_exp_date),DATE(vehicel_master.vm_roadtax_exp_date) ");
            cmd.Parameters.Add("@Owner", context.Session["shortname"].ToString());
            cmd.Parameters.Add("@BranchID", BranchID);
            DataTable dtble = vdm.SelectQuery(cmd).Tables[0];
            DataView view1 = new DataView(dtble);
            DataTable distinctValues = view1.ToTable(true, "vm_sno", "VehicelType", "Make", "registration_no", "Capacity", "vm_model", "vm_rcexpdate", "vm_poll_exp_date", "vm_isurence_exp_date", "vm_fit_exp_date", "vm_roadtax_exp_date");
            cmd = new MySqlCommand("SELECT sno, vehicleno, permit_expdate, pol_expdate, ins_expdate, fitness_expdate, roadtax_expdate, state_permit_expdate, state_roadtax_expdate, branch_sno, operated_by FROM vehicle_exp_doc_logs WHERE (branch_sno = @BranchID)");
            cmd.Parameters.Add("@BranchID", BranchID);
            DataTable dtblelogs = vdm.SelectQuery(cmd).Tables[0];
            DataView view2 = new DataView(dtblelogs);
            List<Vehicledocuments> Vehiclelist = new List<Vehicledocuments>();
            DataTable dtsubtable = view2.ToTable(true, "sno", "vehicleno", "permit_expdate", "pol_expdate", "ins_expdate", "fitness_expdate", "roadtax_expdate", "state_permit_expdate", "state_roadtax_expdate");
            foreach (DataRow dr in distinctValues.Rows)
            {
                Vehicledocuments getVehcle = new Vehicledocuments();
                getVehcle.sno = dr["vm_sno"].ToString();
                getVehcle.vehicleno = dr["registration_no"].ToString();
                getVehcle.type = dr["VehicelType"].ToString();
                getVehcle.make = dr["Make"].ToString();
                getVehcle.model = dr["vm_model"].ToString();
                getVehcle.capacity = dr["Capacity"].ToString();
                string vm_isurence_exp_date = dr["vm_isurence_exp_date"].ToString();
                if (vm_isurence_exp_date == "")
                {
                    getVehcle.insurance = "";
                }
                else
                {
                    DateTime dtDate = Convert.ToDateTime(vm_isurence_exp_date);

                    string Expdate = dtDate.ToString("dd/MMM/yyyy");
                    getVehcle.insurance = Expdate;
                }
                string vm_poll_exp_date = dr["vm_poll_exp_date"].ToString();
                if (vm_poll_exp_date == "")
                {
                    getVehcle.pollution = "";
                }
                else
                {
                    DateTime dtDate = Convert.ToDateTime(vm_poll_exp_date);

                    string Expdate = dtDate.ToString("dd/MMM/yyyy");
                    getVehcle.pollution = Expdate;
                }
                string vm_fit_exp_date = dr["vm_fit_exp_date"].ToString();
                if (vm_fit_exp_date == "")
                {
                    getVehcle.fitness = "";
                }
                else
                {
                    DateTime dtDate = Convert.ToDateTime(vm_fit_exp_date);

                    string Expdate = dtDate.ToString("dd/MMM/yyyy");
                    getVehcle.fitness = Expdate;
                }
                string vm_roadtax_exp_date = dr["vm_roadtax_exp_date"].ToString();
                if (vm_roadtax_exp_date == "")
                {
                    getVehcle.roadtax = "";
                }
                else
                {
                    DateTime dtDate = Convert.ToDateTime(vm_roadtax_exp_date);

                    string Expdate = dtDate.ToString("dd/MMM/yyyy");
                    getVehcle.roadtax = Expdate;
                }
                string vm_rcexpdate = dr["vm_rcexpdate"].ToString();
                if (vm_rcexpdate == "")
                {
                    getVehcle.permit = "";
                }
                else
                {
                    DateTime dtDate = Convert.ToDateTime(vm_rcexpdate);

                    string Expdate = dtDate.ToString("dd/MMM/yyyy");
                    getVehcle.permit = Expdate;
                }
                List<SubVehicle> SubVehiclelist = new List<SubVehicle>();
                foreach (DataRow drr in dtsubtable.Select("vehicleno=" + dr["vm_sno"]))
                {
                    SubVehicle GetSubVehicle = new SubVehicle();
                    GetSubVehicle.sno = drr["vehicleno"].ToString();

                    string permit_expdate = drr["permit_expdate"].ToString();
                    if (permit_expdate == "")
                    {
                        GetSubVehicle.permit_expdate = "";
                    }
                    else
                    {
                        DateTime dtPolDate = Convert.ToDateTime(permit_expdate);

                        string PolExpdate = dtPolDate.ToString("dd/MMM/yyyy");
                        GetSubVehicle.permit_expdate = PolExpdate;
                    }
                    string PolDate = drr["pol_expdate"].ToString();
                    if (PolDate == "")
                    {
                        GetSubVehicle.pol_expdate = "";
                    }
                    else
                    {
                        DateTime dtPolDate = Convert.ToDateTime(PolDate);

                        string PolExpdate = dtPolDate.ToString("dd/MMM/yyyy");
                        GetSubVehicle.pol_expdate = PolExpdate;
                    }
                    string ins_expdate = drr["ins_expdate"].ToString();
                    if (ins_expdate == "")
                    {
                        GetSubVehicle.ins_expdate = "";
                    }
                    else
                    {
                        DateTime dtins_expdate = Convert.ToDateTime(ins_expdate);

                        string PolExpdate = dtins_expdate.ToString("dd/MMM/yyyy");
                        GetSubVehicle.ins_expdate = PolExpdate;
                    }
                    string fitness_expdate = drr["fitness_expdate"].ToString();
                    if (fitness_expdate == "")
                    {
                        GetSubVehicle.fitness_expdate = "";
                    }
                    else
                    {
                        DateTime dtfitness_expdate = Convert.ToDateTime(fitness_expdate);

                        string PolExpdate = dtfitness_expdate.ToString("dd/MMM/yyyy");
                        GetSubVehicle.fitness_expdate = PolExpdate;
                    }
                    string roadtax_expdate = drr["roadtax_expdate"].ToString();
                    if (roadtax_expdate == "")
                    {
                        GetSubVehicle.roadtax_expdate = "";
                    }
                    else
                    {
                        DateTime dtroadtax_expdate = Convert.ToDateTime(roadtax_expdate);

                        string PolExpdate = dtroadtax_expdate.ToString("dd/MMM/yyyy");
                        GetSubVehicle.roadtax_expdate = PolExpdate;
                    }
                    string state_permit_expdate = drr["state_permit_expdate"].ToString();
                    if (state_permit_expdate == "")
                    {
                        GetSubVehicle.state_permit_expdate = "";
                    }
                    else
                    {
                        DateTime dtroadtax_expdate = Convert.ToDateTime(state_permit_expdate);

                        string PolExpdate = dtroadtax_expdate.ToString("dd/MMM/yyyy");
                        GetSubVehicle.state_permit_expdate = PolExpdate;
                    }
                    string state_roadtax_expdate = drr["state_roadtax_expdate"].ToString();
                    if (state_roadtax_expdate == "")
                    {
                        GetSubVehicle.state_roadtax_expdate = "";
                    }
                    else
                    {
                        DateTime dtroadtax_expdate = Convert.ToDateTime(state_roadtax_expdate);

                        string PolExpdate = dtroadtax_expdate.ToString("dd/MMM/yyyy");
                        GetSubVehicle.state_roadtax_expdate = PolExpdate;
                    }
                    SubVehiclelist.Add(GetSubVehicle);
                }
                getVehcle.SubVehiclelist = SubVehiclelist;
                Vehiclelist.Add(getVehcle);
            }
            string response = GetJson(Vehiclelist);
            context.Response.Write(response);
        }
        catch
        {
        }
    }
    private void btnVehicle_tools_issue_return_saveclick(HttpContext context)
    {
        try
        {
            vdm = new VehicleDBMgr();
            string BranchID = context.Session["Branch_ID"].ToString();
            string Employ_Sno = context.Session["Employ_Sno"].ToString();
            string vehicleno = context.Request["vehicleno"];
            string tools = context.Request["tools"];
            string make = context.Request["make"];
            string driverid = context.Request["driverid"];
            string capacity = context.Request["capacity"];
            string cost = context.Request["cost"];
            string type = context.Request["type"];
            DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
            cmd = new MySqlCommand("insert into vehicletools_issue_return(vehiclesno,doe,make,tools,driverid,capacity,cost,operated_by,type,branch_sno) values(@vehiclesno,@doe,@make,@tools,@driverid,@capacity,@cost,@operated_by,@type,@branch_sno)");
            cmd.Parameters.Add("@vehiclesno", vehicleno);
            cmd.Parameters.Add("@doe", ServerDateCurrentdate);
            cmd.Parameters.Add("@make", make);
            cmd.Parameters.Add("@tools", tools);
            cmd.Parameters.Add("@driverid", driverid);
            cmd.Parameters.Add("@capacity", capacity);
            cmd.Parameters.Add("@cost", cost);
            cmd.Parameters.Add("@operated_by", Employ_Sno);
            cmd.Parameters.Add("@type", type);
            cmd.Parameters.Add("@branch_sno", BranchID);
            vdm.insert(cmd);
            string msg = "Issued saved successfully";
            string response = GetJson(msg);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string msg = ex.Message;
            string response = GetJson(msg);
            context.Response.Write(response);
        }
    }
    private void get_all_personal_data(HttpContext context)
    {
        try
        {
            vdm = new VehicleDBMgr();
            string BranchID = context.Session["Branch_ID"].ToString();
            cmd = new MySqlCommand("SELECT sno, name, phoneno, emailid, alert_type,designation FROM personal_info WHERE (branch_sno = @branch_sno)");
            cmd.Parameters.Add("@branch_sno", BranchID);
            DataTable dtble = vdm.SelectQuery(cmd).Tables[0];
            List<personalcls> makelist = new List<personalcls>();
            foreach (DataRow dr in dtble.Rows)
            {
                personalcls getmake = new personalcls();
                getmake.sno = dr["sno"].ToString();
                getmake.name = dr["name"].ToString();
                getmake.phoneno = dr["phoneno"].ToString();
                getmake.emailid = dr["emailid"].ToString();
                getmake.alert_type = dr["alert_type"].ToString();
                getmake.designation = dr["designation"].ToString();
                makelist.Add(getmake);
            }
            string respnceString = GetJson(makelist);
            context.Response.Write(respnceString);
        }
        catch
        {
        }
    }
    private void save_personal_info(HttpContext context)
    {
        try
        {
            vdm = new VehicleDBMgr();
            string BranchID = context.Session["Branch_ID"].ToString();
            string name = context.Request["name"];
            string designation = context.Request["designation"];
            string mobileno = context.Request["mobileno"];
            string alerttype = context.Request["alerttype"];
            string email = context.Request["email"];
            string sno = context.Request["sno"];
            string btnval = context.Request["btnval"];
            string msg = "";
            if (btnval == "Save")
            {
                cmd = new MySqlCommand("insert into personal_info(name,phoneno,emailid,alert_type,branch_sno,designation) values(@name,@phoneno,@emailid,@alert_type,@branch_sno,@designation)");
                cmd.Parameters.Add("@name", name);
                cmd.Parameters.Add("@phoneno", mobileno);
                cmd.Parameters.Add("@emailid", email);
                cmd.Parameters.Add("@alert_type", alerttype);
                cmd.Parameters.Add("@designation", designation);
                cmd.Parameters.Add("@branch_sno", BranchID);
                vdm.insert(cmd);
                msg = "Personal information saved successfully";
            }
            else
            {
                cmd = new MySqlCommand("update personal_info set name=@name,phoneno=@phoneno,emailid=@emailid,alert_type=@alert_type,designation=@designation where branch_sno=@branch_sno and sno=@sno");
                cmd.Parameters.Add("@name", name);
                cmd.Parameters.Add("@phoneno", mobileno);
                cmd.Parameters.Add("@emailid", email);
                cmd.Parameters.Add("@alert_type", alerttype);
                cmd.Parameters.Add("@designation", designation);
                cmd.Parameters.Add("@branch_sno", BranchID);
                cmd.Parameters.Add("@sno", sno);
                vdm.Update(cmd);
                msg = "Personal information modified successfully";
            }
            string response = GetJson(msg);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string msg = ex.Message;
            string response = GetJson(msg);
            context.Response.Write(response);
        }
    }
    private void get_vehiclemake(HttpContext context)
    {
        try
        {
            vdm = new VehicleDBMgr();
            string BranchID = context.Session["Branch_ID"].ToString();
            cmd = new MySqlCommand("SELECT minimasters.mm_name, minimasters.sno FROM vehicel_master INNER JOIN minimasters ON vehicel_master.vhmake_refno = minimasters.sno WHERE (vehicel_master.branch_id = @BranchID) GROUP BY minimasters.mm_name");
            cmd.Parameters.Add("@BranchID", BranchID);
            DataTable dtble = vdm.SelectQuery(cmd).Tables[0];
            List<vehciclemakecls> makelist = new List<vehciclemakecls>();
            foreach (DataRow dr in dtble.Rows)
            {
                vehciclemakecls getmake = new vehciclemakecls();
                getmake.sno = dr["sno"].ToString();
                getmake.make = dr["mm_name"].ToString();
                makelist.Add(getmake);
            }
            string respnceString = GetJson(makelist);
            context.Response.Write(respnceString);
        }
        catch
        {
        }
    }
    private void GetVehicleWisePerformanceclick(HttpContext context)
    {
        try
        {
            vdm = new VehicleDBMgr();
            string BranchID = context.Session["Branch_ID"].ToString();
            string VehicleNo = context.Request["vehicleno"];
            string ChartType = context.Request["ChartType"];
            string FormName = context.Request["FormName"];
            string FDate = context.Request["FromDate"];
            DateTime FromDate = Convert.ToDateTime(FDate);
            string TDate = context.Request["Todate"];
            DateTime Todate = Convert.ToDateTime(TDate);
            TimeSpan dateSpan = Todate.Subtract(FromDate);
            int NoOfdays = dateSpan.Days;

            if (ChartType == "Vehicle Wise")
            {
                cmd = new MySqlCommand("SELECT derivedtbl_1.tripsheetno AS TripSheet,derivedtbl_1.employname, DATE_FORMAT(derivedtbl_1.tripdate, '%d-%m-%Y %h:%i %p') AS StartDate, DATE_FORMAT(derivedtbl_1.enddate, '%d-%m-%Y') AS EndDate, derivedtbl_1.gpskms, derivedtbl_1.registration_no AS VehicleNo,  derivedtbl_1.endfuelvalue AS InsideFuel, SUM(triplogs.fuel) AS OutsideFuel, derivedtbl_1.qty, IFNULL(ROUND(derivedtbl_1.TripKMS / (derivedtbl_1.endfuelvalue + SUM(triplogs.fuel)), 2), 0) AS TodayMileage, derivedtbl_1.sno,derivedtbl_1.act_mileage,derivedtbl_1.routeid,derivedtbl_1.employid FROM (SELECT     vehicel_master.act_mileage,   tripdata.tripsheetno, tripdata.tripdate, tripdata.enddate, vehicel_master.registration_no, tripdata.endodometerreading - tripdata.vehiclestartreading AS TripKMS, tripdata.gpskms, tripdata.loadtype, tripdata.qty, tripdata.routeid, vehicel_master.Capacity, tripdata.sno, tripdata.endfuelvalue,employdata.employid, employdata.employname FROM tripdata INNER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno INNER JOIN employdata ON tripdata.driverid = employdata.emp_sno WHERE        (tripdata.enddate BETWEEN @d1 AND @d2) AND (tripdata.userid = @BranchID) AND (tripdata.vehicleno = @VehicleNo) AND (tripdata.status = 'C')) derivedtbl_1 INNER JOIN triplogs ON derivedtbl_1.sno = triplogs.tripsno AND triplogs.fuel_type <> 'OWN'GROUP BY derivedtbl_1.tripsheetno ORDER BY derivedtbl_1.sno");

                cmd.Parameters.Add("@BranchID", BranchID);
                cmd.Parameters.Add("@VehicleNo", VehicleNo);
                cmd.Parameters.Add("@d1", GetLowDate(FromDate));
                cmd.Parameters.Add("@d2", GetHighDate(Todate));
            }
            if (ChartType == "Driver Wise")
            {
                cmd = new MySqlCommand("SELECT derivedtbl_1.tripsheetno AS TripSheet, DATE_FORMAT(derivedtbl_1.tripdate, '%d-%m-%Y %h:%i %p') AS StartDate, DATE_FORMAT(derivedtbl_1.enddate, '%d-%m-%Y') AS EndDate, derivedtbl_1.gpskms, derivedtbl_1.registration_no AS VehicleNo, derivedtbl_1.VehicleType, derivedtbl_1.Make, derivedtbl_1.Capacity, derivedtbl_1.TripKMS, derivedtbl_1.loadtype AS LoadType, derivedtbl_1.routeid AS RouteName, derivedtbl_1.endfuelvalue AS InsideFuel, SUM(triplogs.fuel) AS OutsideFuel, derivedtbl_1.qty, IFNULL(ROUND(derivedtbl_1.TripKMS / (derivedtbl_1.endfuelvalue + SUM(triplogs.fuel)), 2), 0) AS TodayMileage, derivedtbl_1.sno,derivedtbl_1.act_mileage,derivedtbl_1.routeid FROM (SELECT    vehicel_master.act_mileage,   tripdata.tripsheetno, tripdata.tripdate, tripdata.enddate, vehicel_master.registration_no, tripdata.endodometerreading - tripdata.vehiclestartreading AS TripKMS, tripdata.gpskms, tripdata.loadtype, tripdata.qty, tripdata.routeid, vehicel_master.Capacity, tripdata.sno, tripdata.endfuelvalue, minimasters.mm_name AS VehicleType, minimasters_1.mm_code AS Make FROM tripdata INNER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno INNER JOIN minimasters ON vehicel_master.vhtype_refno = minimasters.sno INNER JOIN minimasters minimasters_1 ON vehicel_master.vhmake_refno = minimasters_1.sno WHERE        (tripdata.enddate BETWEEN @d1 AND @d2) AND (tripdata.userid = @BranchID)  AND (tripdata.driverid = @VehicleNo)  AND (tripdata.status = 'C')) derivedtbl_1 INNER JOIN triplogs ON derivedtbl_1.sno = triplogs.tripsno AND triplogs.fuel_type <> 'OWN' GROUP BY derivedtbl_1.tripsheetno order by derivedtbl_1.sno");
                cmd.Parameters.Add("@BranchID", BranchID);
                cmd.Parameters.Add("@VehicleNo", VehicleNo);
                cmd.Parameters.Add("@d1", GetLowDate(FromDate));
                cmd.Parameters.Add("@d2", GetHighDate(Todate));
            }
            if (ChartType == "Vehicle Make Wise")
            {
                cmd = new MySqlCommand("SELECT derivedtbl_1.tripsheetno AS TripSheet, DATE_FORMAT(derivedtbl_1.tripdate, '%d-%m-%Y %h:%i %p') AS StartDate, DATE_FORMAT(derivedtbl_1.enddate, '%d-%m-%Y') AS EndDate, derivedtbl_1.gpskms, derivedtbl_1.registration_no AS VehicleNo, derivedtbl_1.VehicleType, derivedtbl_1.Make, derivedtbl_1.Capacity, derivedtbl_1.TripKMS, derivedtbl_1.loadtype AS LoadType, derivedtbl_1.routeid AS RouteName, derivedtbl_1.endfuelvalue AS InsideFuel, SUM(triplogs.fuel) AS OutsideFuel, derivedtbl_1.qty, IFNULL(ROUND(derivedtbl_1.TripKMS / (derivedtbl_1.endfuelvalue + SUM(triplogs.fuel)), 2), 0) AS TodayMileage, derivedtbl_1.sno, derivedtbl_1.act_mileage, derivedtbl_1.routeid FROM (SELECT        vehicel_master.act_mileage, tripdata.tripsheetno, tripdata.tripdate, tripdata.enddate, vehicel_master.registration_no, tripdata.endodometerreading - tripdata.vehiclestartreading AS TripKMS, tripdata.gpskms, tripdata.loadtype, tripdata.qty, tripdata.routeid, vehicel_master.Capacity, tripdata.sno, tripdata.endfuelvalue, minimasters.mm_name AS VehicleType, minimasters_1.mm_code AS Make FROM tripdata INNER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno INNER JOIN minimasters ON vehicel_master.vhtype_refno = minimasters.sno INNER JOIN minimasters minimasters_1 ON vehicel_master.vhmake_refno = minimasters_1.sno WHERE        (tripdata.enddate BETWEEN @d1 AND @d2) AND (tripdata.userid = @BranchID) AND (vehicel_master.vhmake_refno = @Make) AND (tripdata.status = 'C') GROUP BY vehicel_master.registration_no) derivedtbl_1 INNER JOIN triplogs ON derivedtbl_1.sno = triplogs.tripsno AND triplogs.fuel_type <> 'OWN' GROUP BY derivedtbl_1.tripsheetno ORDER BY derivedtbl_1.sno");
                cmd.Parameters.Add("@BranchID", BranchID);
                cmd.Parameters.Add("@Make", VehicleNo);
                cmd.Parameters.Add("@d1", GetLowDate(FromDate));
                cmd.Parameters.Add("@d2", GetHighDate(Todate));
            }
            DataTable dtble = vdm.SelectQuery(cmd).Tables[0];
            List<LineChartValuesclass> LineChartValuelist = new List<LineChartValuesclass>();
            string Mileage = "";
            //string mouseOver = "";
            string AvgMileage = "";
            string ActMileage = "";
            string Date = "";
            double avgSaleQty = 0;
            int count = 0;
            if (dtble.Rows.Count > 0)
            {
                foreach (DataRow dr in dtble.Rows)
                {

                    string TodayMileage = dr["TodayMileage"].ToString();
                    if (TodayMileage != "0")
                    {
                        Mileage += dr["TodayMileage"].ToString() + ',';
                        ActMileage += dr["act_mileage"].ToString() + ',';
                        double milltr = 0;
                        double.TryParse(dr["TodayMileage"].ToString(), out milltr);
                        avgSaleQty += milltr;
                        //string EndDate = dr["EndDate"].ToString();
                        //DateTime dtDOE1 = Convert.ToDateTime(EndDate);
                        //string TripEndDate = dtDOE1.ToString("dd/MMM/yy");
                        if (FormName == "vehicleperformancechart")
                        {
                            if (ChartType == "Vehicle Wise")
                            {
                                Date += dr["EndDate"].ToString() + "->" + dr["employname"].ToString() + "->" + dr["routeid"].ToString() + ',';
                                count++;
                            }
                            if (ChartType == "Driver Wise")
                            {
                                Date += dr["EndDate"].ToString() + "->" + dr["VehicleNo"].ToString() + "->" + dr["routeid"].ToString() + ',';
                                count++;
                            }
                        }
                        if (FormName == "Routewisechart")
                        {
                            if (ChartType == "Vehicle Wise")
                            {
                                Date += dr["routeid"].ToString() + ',';
                                count++;
                            }
                            if (ChartType == "Driver Wise")
                            {
                                Date += dr["routeid"].ToString() + ',';
                                count++;
                            }
                            if (ChartType == "Vehicle Make Wise")
                            {
                                Date += dr["VehicleNo"].ToString() + ',';
                                count++;
                            }
                        }
                    }
                }
                double avg = 0;
                Mileage = Mileage.Substring(0, Mileage.Length - 1);
                ActMileage = ActMileage.Substring(0, ActMileage.Length - 1);
                avg = (avgSaleQty / count);
                avg = Math.Round(avg, 2);
                Date = Date.Substring(0, Date.Length - 1);
                foreach (DataRow dr in dtble.Rows)
                {
                    string TodayMileage = dr["TodayMileage"].ToString();
                    if (TodayMileage != "0")
                    {
                        AvgMileage += avg.ToString() + ",";
                    }
                }
                AvgMileage = AvgMileage.Substring(0, AvgMileage.Length - 1);
                LineChartValuesclass getLineChart = new LineChartValuesclass();
                List<string> unitlist = new List<string>();
                List<string> Deliverlist = new List<string>();
                List<string> Datelist = new List<string>();
                List<string> Statuslist = new List<string>();
                List<string> ActMillist = new List<string>();

                Deliverlist.Add(Mileage);
                Deliverlist.Add(AvgMileage);
                Deliverlist.Add(ActMileage);
                Statuslist.Add("Mileage Ltr");
                Statuslist.Add("Avg Mileage");
                Statuslist.Add("Act Mileage");
                //ActMillist.Add(ActMileage);
                getLineChart.TripDate = Date;
                getLineChart.Mileageltr = Deliverlist;
                getLineChart.AvgLtr = AvgMileage;
                getLineChart.Status = Statuslist;
                getLineChart.ActMileage = ActMillist;
                LineChartValuelist.Add(getLineChart);
                string respnceString = GetJson(LineChartValuelist);
                context.Response.Write(respnceString);
            }
        }
        catch
        {
        }
    }
    private void get_vehicleservice_update_kms_details(HttpContext context)
    {
        try
        {
            vdm = new VehicleDBMgr();
            string BranchID = context.Session["Branch_ID"].ToString();
            //cmd = new MySqlCommand("SELECT vehicel_master.registration_no, vehi_service_update_kms.vehsno, vehi_service_update_kms.sno, vehi_service_update_kms.eoc, vehi_service_update_kms.goc, vehi_service_update_kms.ofc, vehi_service_update_kms.afc, vehi_service_update_kms.steering_fluid, vehi_service_update_kms.brake_fluid, vehi_service_update_kms.transmission_fluid, vehi_service_update_kms.washer_fluid, vehi_service_update_kms.wheel_bearings, vehi_service_update_kms.checkleaks, vehi_service_update_kms.belts_hoses, vehi_service_update_kms.lubricate_chasis, vehi_service_update_kms.airchecking, vehi_service_update_kms.tyreinterchanging, vehi_service_update_kms.doe FROM vehicel_master INNER JOIN vehi_service_update_kms ON vehicel_master.vm_sno = vehi_service_update_kms.vehsno WHERE (vehicel_master.branch_id = @BranchID) GROUP BY vehicel_master.registration_no");
            //cmd.Parameters.Add("@BranchID", BranchID);
            cmd = new MySqlCommand("SELECT vehicel_master.registration_no,vehi_service_update_kms.vehsno, vehi_service_update_kms.sno, vehi_service_update_kms.eoc, vehi_service_update_kms.goc, vehi_service_update_kms.ofc, vehi_service_update_kms.afc, vehi_service_update_kms.steering_fluid, vehi_service_update_kms.brake_fluid, vehi_service_update_kms.transmission_fluid, vehi_service_update_kms.washer_fluid, vehi_service_update_kms.wheel_bearings, vehi_service_update_kms.checkleaks, vehi_service_update_kms.belts_hoses, vehi_service_update_kms.lubricate_chasis, vehi_service_update_kms.airchecking, vehi_service_update_kms.tyreinterchanging, vehi_service_update_kms.doe FROM  vehi_service_update_kms RIGHT OUTER JOIN vehicel_master ON vehi_service_update_kms.vehsno = vehicel_master.vm_sno");
            DataTable dtVehicleServicing = vdm.SelectQuery(cmd).Tables[0];
            List<vehiServicingAlertsclass> Vehi_Servicing_list = new List<vehiServicingAlertsclass>();
            if (dtVehicleServicing.Rows.Count > 0)
            {
                foreach (DataRow dr in dtVehicleServicing.Rows)
                {
                    vehiServicingAlertsclass GetServicing = new vehiServicingAlertsclass();
                    GetServicing.vehicleno = dr["registration_no"].ToString();
                    GetServicing.vehsno = dr["vehsno"].ToString();
                    GetServicing.EOC = dr["eoc"].ToString();
                    GetServicing.GOC = dr["goc"].ToString();
                    GetServicing.AFC = dr["afc"].ToString();
                    GetServicing.OFC = dr["ofc"].ToString();
                    GetServicing.brakefluid = dr["brake_fluid"].ToString();
                    GetServicing.powersteeringfluid = dr["steering_fluid"].ToString();
                    GetServicing.transmissionfluid = dr["transmission_fluid"].ToString();
                    GetServicing.washerfluid = dr["washer_fluid"].ToString();
                    GetServicing.checkbrakes = dr["wheel_bearings"].ToString();
                    GetServicing.checkleaks = dr["checkleaks"].ToString();
                    GetServicing.allbelts = dr["belts_hoses"].ToString();
                    GetServicing.lubricatechassis = dr["lubricate_chasis"].ToString();
                    GetServicing.Sno = dr["sno"].ToString();
                    Vehi_Servicing_list.Add(GetServicing);
                }
                string response = GetJson(Vehi_Servicing_list);
                context.Response.Write(response);
            }
            else
            {
                string msg = "No data found";
                string response = GetJson(msg);
                context.Response.Write(response);
            }
        }
        catch
        {
        }
    }
    private void get_vehicleServicealerts_details(HttpContext context)
    {
        try
        {
            vdm = new VehicleDBMgr();
            string BranchID = context.Session["Branch_ID"].ToString();
            cmd = new MySqlCommand("SELECT vehicel_master.registration_no,vehi_servicingalerts.vehsno, vehi_servicingalerts.eoc, vehi_servicingalerts.goc, vehi_servicingalerts.ofc, vehi_servicingalerts.afc, vehi_servicingalerts.steering_fluid, vehi_servicingalerts.brake_fluid,vehi_servicingalerts.transmission_fluid, vehi_servicingalerts.washer_fluid, vehi_servicingalerts.wheel_bearings,             vehi_servicingalerts.checkleaks, vehi_servicingalerts.belts_hoses, vehi_servicingalerts.lubricate_chasis, vehi_servicingalerts.sno FROM vehi_servicingalerts INNER JOIN vehicel_master ON vehi_servicingalerts.vehsno = vehicel_master.vm_sno WHERE (vehicel_master.branch_id = @BranchID) GROUP BY vehicel_master.registration_no");
            cmd.Parameters.Add("@BranchID", BranchID);
            DataTable dtVehicleServicing = vdm.SelectQuery(cmd).Tables[0];
            List<vehiServicingAlertsclass> Vehi_Servicing_list = new List<vehiServicingAlertsclass>();
            if (dtVehicleServicing.Rows.Count > 0)
            {
                foreach (DataRow dr in dtVehicleServicing.Rows)
                {
                    vehiServicingAlertsclass GetServicing = new vehiServicingAlertsclass();
                    GetServicing.vehicleno = dr["registration_no"].ToString();
                    GetServicing.vehsno = dr["vehsno"].ToString();
                    GetServicing.EOC = dr["eoc"].ToString();
                    GetServicing.GOC = dr["goc"].ToString();
                    GetServicing.AFC = dr["afc"].ToString();
                    GetServicing.OFC = dr["ofc"].ToString();
                    GetServicing.brakefluid = dr["brake_fluid"].ToString();
                    GetServicing.powersteeringfluid = dr["steering_fluid"].ToString();
                    GetServicing.transmissionfluid = dr["transmission_fluid"].ToString();
                    GetServicing.washerfluid = dr["washer_fluid"].ToString();
                    GetServicing.checkbrakes = dr["wheel_bearings"].ToString();
                    GetServicing.checkleaks = dr["checkleaks"].ToString();
                    GetServicing.allbelts = dr["belts_hoses"].ToString();
                    GetServicing.lubricatechassis = dr["lubricate_chasis"].ToString();
                    GetServicing.Sno = dr["sno"].ToString();
                    Vehi_Servicing_list.Add(GetServicing);
                }
                string response = GetJson(Vehi_Servicing_list);
                context.Response.Write(response);
            }
            else
            {
                string msg = "No data found";
                string response = GetJson(msg);
                context.Response.Write(response);
            }
        }
        catch
        {
        }
    }
    private void GetVehicleAlertDeatails(HttpContext context)
    {
        try
        {
            vdm = new VehicleDBMgr();
            string BranchID = context.Session["Branch_ID"].ToString();
            DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
            cmd = new MySqlCommand("SELECT minimasters.mm_name AS VehicelType, minimasters_1.mm_name AS Make, vehicel_master.registration_no, vehicel_master.Capacity, vehicel_master.vm_model, vehicel_master.vm_engine, vehicel_master.vm_chasiss, vehicel_master.vm_owner, vehicel_master.vm_rcexpdate, vehicel_master.vm_poll_exp_date, vehicel_master.vm_isurence_exp_date, vehicel_master.vm_fit_exp_date,vehicel_master.vm_roadtax_exp_date, vehicel_master.fuel_capacity,vehicel_master.odometer FROM vehicel_master INNER JOIN minimasters ON vehicel_master.vhtype_refno = minimasters.sno INNER JOIN minimasters minimasters_1 ON vehicel_master.vhmake_refno = minimasters_1.sno WHERE (vehicel_master.branch_id = @BranchID) and (vehicel_master.vm_owner=@Owner) order by VehicelType DESC");
                cmd.Parameters.Add("@Owner", context.Session["shortname"].ToString());
            cmd.Parameters.Add("@BranchID", BranchID);
            DataTable dtVehicles = vdm.SelectQuery(cmd).Tables[0];
            List<vehiclesclass> Vehicleslist = new List<vehiclesclass>();
            if (dtVehicles.Rows.Count > 0)
            {
                foreach (DataRow dr in dtVehicles.Rows)
                {
                    vehiclesclass Getvehicle = new vehiclesclass();
                    Getvehicle.vehicleno = dr["registration_no"].ToString();
                    Getvehicle.vehiclemake = dr["Make"].ToString();
                    Getvehicle.vehiclemodel = dr["vm_model"].ToString();
                    Getvehicle.vehicletype = dr["VehicelType"].ToString();
                    Getvehicle.Capacity = dr["Capacity"].ToString();
                    Getvehicle.FuelCapacity = dr["fuel_capacity"].ToString();
                    Getvehicle.EngineNo = dr["vm_engine"].ToString();
                    Getvehicle.ChasisNo = dr["vm_chasiss"].ToString();
                    Getvehicle.odometer = dr["odometer"].ToString();
                    string InsDate = dr["vm_isurence_exp_date"].ToString();
                    if (InsDate == "")
                    {
                        Getvehicle.InsExpDate = "";
                        Getvehicle.InsExpcolor = "Red";
                    }
                    else
                    {
                        DateTime dtInsDate = Convert.ToDateTime(InsDate);
                        TimeSpan dateSpan = dtInsDate.Subtract(ServerDateCurrentdate);
                        int NoOfdays = dateSpan.Days;
                        if (NoOfdays < 30)
                        {
                            if (NoOfdays < 0)
                            {
                                Getvehicle.InsExpcolor = "Red";
                            }
                            else
                            {
                                Getvehicle.InsExpcolor = "orange";
                            }
                        }
                        string InsExpdate = dtInsDate.ToString("dd/MMM/yyyy");
                        Getvehicle.InsExpDate = InsExpdate;
                    }
                    string PolDate = dr["vm_poll_exp_date"].ToString();
                    if (PolDate == "")
                    {
                        Getvehicle.PolExpDate = "";
                        Getvehicle.PolExpcolor = "Red";
                    }
                    else
                    {
                        DateTime dtPolDate = Convert.ToDateTime(PolDate);
                        TimeSpan dateSpan = dtPolDate.Subtract(ServerDateCurrentdate);
                        int NoOfdays = dateSpan.Days;
                        if (NoOfdays < 30)
                        {
                            if (NoOfdays < 0)
                            {
                                Getvehicle.PolExpcolor = "Red";
                            }
                            else
                            {
                                Getvehicle.PolExpcolor = "orange";
                            }
                        }
                        string PolExpdate = dtPolDate.ToString("dd/MMM/yyyy");
                        Getvehicle.PolExpDate = PolExpdate;
                    }
                    string FitDate = dr["vm_fit_exp_date"].ToString();
                    if (FitDate == "")
                    {
                        Getvehicle.FitExpDate = "";
                        Getvehicle.FitExpcolor = "Red";
                    }
                    else
                    {
                        DateTime dtFitDate = Convert.ToDateTime(FitDate);
                        TimeSpan dateSpan = dtFitDate.Subtract(ServerDateCurrentdate);
                        int NoOfdays = dateSpan.Days;
                        if (NoOfdays < 30)
                        {
                            if (NoOfdays < 0)
                            {
                                Getvehicle.FitExpcolor = "Red";
                            }
                            else
                            {
                                Getvehicle.FitExpcolor = "orange";
                            }
                        }
                        string FitExpdate = dtFitDate.ToString("dd/MMM/yyyy");
                        Getvehicle.FitExpDate = FitExpdate;
                    }
                    string RoadTaxDate = dr["vm_roadtax_exp_date"].ToString();
                    if (RoadTaxDate == "")
                    {
                        Getvehicle.RoadTaxExpDate = "";
                        Getvehicle.RoadTaxExpcolor = "Red";
                    }
                    else
                    {
                        DateTime dtRoadTaxDate = Convert.ToDateTime(RoadTaxDate);
                        TimeSpan dateSpan = dtRoadTaxDate.Subtract(ServerDateCurrentdate);
                        int NoOfdays = dateSpan.Days;
                        if (NoOfdays < 30)
                        {
                            if (NoOfdays < 0)
                            {
                                Getvehicle.RoadTaxExpcolor = "Red";
                            }
                            else
                            {
                                Getvehicle.RoadTaxExpcolor = "orange";
                            }
                        }
                        string RoadTaxExpdate = dtRoadTaxDate.ToString("dd/MMM/yyyy");
                        Getvehicle.RoadTaxExpDate = RoadTaxExpdate;
                    }
                    string RCDate = dr["vm_rcexpdate"].ToString();
                    if (RCDate == "")
                    {
                        Getvehicle.RCExpDate = "";
                        Getvehicle.RCExpcolor = "Red";
                    }
                    else
                    {
                        DateTime dtRCDate = Convert.ToDateTime(RCDate);
                        TimeSpan dateSpan = dtRCDate.Subtract(ServerDateCurrentdate);
                        int NoOfdays = dateSpan.Days;
                        if (NoOfdays < 30)
                        {
                            if (NoOfdays < 0)
                            {
                                Getvehicle.RCExpcolor = "Red";
                            }
                            else
                            {
                                Getvehicle.RCExpcolor = "orange";
                            }
                        }
                        string RCExpdate = dtRCDate.ToString("dd/MMM/yyyy");
                        Getvehicle.RCExpDate = RCExpdate;
                    }
                    Vehicleslist.Add(Getvehicle);
                }
                string response = GetJson(Vehicleslist);
                context.Response.Write(response);
            }
            else
            {
                string msg = "No data found";
                string response = GetJson(msg);
                context.Response.Write(response);
            }
        }
        catch
        {
        }
    }
    private void save_Veh_Servicing_update_kms_click(HttpContext context)
    {
        try
        {
            vdm = new VehicleDBMgr();
            string vehicleNo = context.Request["vehsno"];
            string eoc = context.Request["eoc"];
            string goc = context.Request["goc"];
            string ofc = context.Request["ofc"];
            string afc = context.Request["afc"];
            string brakefluid = context.Request["brakefluid"];
            string powersteeringfluid = context.Request["powersteeringfluid"];
            string transmissionfluid = context.Request["transmissionfluid"];
            string washerfluid = context.Request["washerfluid"];
            string checkbrakes = context.Request["checkbrakes"];
            string checkleaks = context.Request["checkleaks"];
            string allbelts = context.Request["allbelts"];
            string lubricatechassis = context.Request["lubricatechassis"];
            string airchecking = context.Request["airchecking"];
            string tyreinterchanging = context.Request["tyreinterchanging"];
            string btnval = context.Request["btnval"];
            string sno = context.Request["sno"];
            string msg = "";
            DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
            if (btnval == "Save")
            {
                cmd = new MySqlCommand("insert into vehi_service_update_kms (vehsno, eoc, goc, ofc, afc, brake_fluid, steering_fluid, transmission_fluid, washer_fluid, wheel_bearings, checkleaks, belts_hoses, lubricate_chasis,airchecking,tyreinterchanging,doe) values (@vehsno, @eoc, @goc, @ofc, @afc, @brake_fluid, @steering_fluid, @transmission_fluid, @washer_fluid, @wheel_bearings, @checkleaks, @belts_hoses, @lubricate_chasis,@airchecking,@tyreinterchanging,@doe)");
                cmd.Parameters.Add("@vehsno", vehicleNo);
                cmd.Parameters.Add("@eoc", eoc);
                cmd.Parameters.Add("@goc", goc);
                cmd.Parameters.Add("@ofc", ofc);
                cmd.Parameters.Add("@afc", afc);
                cmd.Parameters.Add("@brake_fluid", brakefluid);
                cmd.Parameters.Add("@steering_fluid", powersteeringfluid);
                cmd.Parameters.Add("@transmission_fluid", transmissionfluid);
                cmd.Parameters.Add("@washer_fluid", washerfluid);
                cmd.Parameters.Add("@wheel_bearings", checkbrakes);
                cmd.Parameters.Add("@checkleaks", checkleaks);
                cmd.Parameters.Add("@belts_hoses", allbelts);
                cmd.Parameters.Add("@lubricate_chasis", lubricatechassis);
                cmd.Parameters.Add("@airchecking", airchecking);
                cmd.Parameters.Add("@tyreinterchanging", tyreinterchanging);
                cmd.Parameters.Add("@doe", ServerDateCurrentdate);
                vdm.insert(cmd);
                msg = "Kms saved successfully";
            }
            else
            {
                cmd = new MySqlCommand("update   vehi_service_update_kms set doe=@doe,  airchecking=@airchecking,tyreinterchanging=@tyreinterchanging,eoc=@eoc, goc=@goc, ofc=@ofc, afc=@afc, brake_fluid=@brake_fluid, steering_fluid=@steering_fluid, transmission_fluid=@transmission_fluid, washer_fluid=@washer_fluid, wheel_bearings=@wheel_bearings, checkleaks=@checkleaks, belts_hoses=@belts_hoses, lubricate_chasis=@lubricate_chasis where vehsno=@vehsno and sno=@sno");
                cmd.Parameters.Add("@doe", ServerDateCurrentdate);
                cmd.Parameters.Add("@vehsno", vehicleNo);
                cmd.Parameters.Add("@eoc", eoc);
                cmd.Parameters.Add("@goc", goc);
                cmd.Parameters.Add("@ofc", ofc);
                cmd.Parameters.Add("@afc", afc);
                cmd.Parameters.Add("@brake_fluid", brakefluid);
                cmd.Parameters.Add("@steering_fluid", powersteeringfluid);
                cmd.Parameters.Add("@transmission_fluid", transmissionfluid);
                cmd.Parameters.Add("@washer_fluid", washerfluid);
                cmd.Parameters.Add("@wheel_bearings", checkbrakes);
                cmd.Parameters.Add("@checkleaks", checkleaks);
                cmd.Parameters.Add("@belts_hoses", allbelts);
                cmd.Parameters.Add("@lubricate_chasis", lubricatechassis);
                cmd.Parameters.Add("@sno", sno);
                cmd.Parameters.Add("@airchecking", airchecking);
                cmd.Parameters.Add("@tyreinterchanging", tyreinterchanging);
                vdm.Update(cmd);
                msg = "Kms updated successfully";
            }
            string response = GetJson(msg);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string msg = ex.Message;
            string response = GetJson(msg);
            context.Response.Write(response);
        }
    }
    private void save_Veh_Servicing_Save_click(HttpContext context)
    {
        try
        {
            vdm = new VehicleDBMgr();
            string vehicleNo = context.Request["vehsno"];
            string eoc = context.Request["eoc"];
            string goc = context.Request["goc"];
            string ofc = context.Request["ofc"];
            string afc = context.Request["afc"];
            string brakefluid = context.Request["brakefluid"];
            string powersteeringfluid = context.Request["powersteeringfluid"];
            string transmissionfluid = context.Request["transmissionfluid"];
            string washerfluid = context.Request["washerfluid"];
            string checkbrakes = context.Request["checkbrakes"];
            string checkleaks = context.Request["checkleaks"];
            string allbelts = context.Request["allbelts"];
            string lubricatechassis = context.Request["lubricatechassis"];
            string airchecking = context.Request["airchecking"];
            string tyreinterchanging = context.Request["tyreinterchanging"];
            string btnval = context.Request["btnval"];
            string sno = context.Request["sno"];
            string msg = "";
            if (btnval == "Save")
            {
                cmd = new MySqlCommand("insert into vehi_servicingalerts (vehsno, eoc, goc, ofc, afc, brake_fluid, steering_fluid, transmission_fluid, washer_fluid, wheel_bearings, checkleaks, belts_hoses, lubricate_chasis,airchecking,tyreinterchanging) values (@vehsno, @eoc, @goc, @ofc, @afc, @brake_fluid, @steering_fluid, @transmission_fluid, @washer_fluid, @wheel_bearings, @checkleaks, @belts_hoses, @lubricate_chasis,@airchecking,@tyreinterchanging)");
                cmd.Parameters.Add("@vehsno", vehicleNo);
                cmd.Parameters.Add("@eoc", eoc);
                cmd.Parameters.Add("@goc", goc);
                cmd.Parameters.Add("@ofc", ofc);
                cmd.Parameters.Add("@afc", afc);
                cmd.Parameters.Add("@brake_fluid", brakefluid);
                cmd.Parameters.Add("@steering_fluid", powersteeringfluid);
                cmd.Parameters.Add("@transmission_fluid", transmissionfluid);
                cmd.Parameters.Add("@washer_fluid", washerfluid);
                cmd.Parameters.Add("@wheel_bearings", checkbrakes);
                cmd.Parameters.Add("@checkleaks", checkleaks);
                cmd.Parameters.Add("@belts_hoses", allbelts);
                cmd.Parameters.Add("@lubricate_chasis", lubricatechassis);
                cmd.Parameters.Add("@airchecking", airchecking);
                cmd.Parameters.Add("@tyreinterchanging", tyreinterchanging);
                vdm.insert(cmd);
                msg = "Kms saved successfully";
            }
            else
            {
                cmd = new MySqlCommand("update  vehi_servicingalerts set  airchecking=@airchecking,tyreinterchanging=@tyreinterchanging,eoc=@eoc, goc=@goc, ofc=@ofc, afc=@afc, brake_fluid=@brake_fluid, steering_fluid=@steering_fluid, transmission_fluid=@transmission_fluid, washer_fluid=@washer_fluid, wheel_bearings=@wheel_bearings, checkleaks=@checkleaks, belts_hoses=@belts_hoses, lubricate_chasis=@lubricate_chasis where vehsno=@vehsno and sno=@sno");
                cmd.Parameters.Add("@vehsno", vehicleNo);
                cmd.Parameters.Add("@eoc", eoc);
                cmd.Parameters.Add("@goc", goc);
                cmd.Parameters.Add("@ofc", ofc);
                cmd.Parameters.Add("@afc", afc);
                cmd.Parameters.Add("@brake_fluid", brakefluid);
                cmd.Parameters.Add("@steering_fluid", powersteeringfluid);
                cmd.Parameters.Add("@transmission_fluid", transmissionfluid);
                cmd.Parameters.Add("@washer_fluid", washerfluid);
                cmd.Parameters.Add("@wheel_bearings", checkbrakes);
                cmd.Parameters.Add("@checkleaks", checkleaks);
                cmd.Parameters.Add("@belts_hoses", allbelts);
                cmd.Parameters.Add("@lubricate_chasis", lubricatechassis);
                cmd.Parameters.Add("@sno", sno);
                cmd.Parameters.Add("@airchecking", airchecking);
                cmd.Parameters.Add("@tyreinterchanging", tyreinterchanging);
                vdm.Update(cmd);
                msg = "Kms updated successfully";
            }
            string response = GetJson(msg);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string msg = ex.Message;
            string response = GetJson(msg);
            context.Response.Write(response);
        }
    }
    private void btnEditTripSheetSaveClick(HttpContext context)
    {
        try
        {
            vdm = new VehicleDBMgr();
            string vehicleNo = context.Request["vehicleNo"];
            string RouteID = context.Request["RouteID"];
            string driver = context.Request["driver"];
            string helper = context.Request["helper"];
            string VehicleStartReading = context.Request["VehicleStartReading"];
            string HrReading = context.Request["HrReading"];
            string load = context.Request["load"];
            string LoadQty = context.Request["Qty"];
            double Qty = 0;
            double.TryParse(LoadQty, out Qty);
            string txtendodometerrdng = context.Request["txtendodometerrdng"];
            string endfuelrdng = context.Request["txtendfuelrdng"];
            double txtendfuelrdng = 0;
            double.TryParse(endfuelrdng, out txtendfuelrdng);
            string txtpumpreading = context.Request["txtpumpreading"];
            string txttoken = context.Request["txttoken"];
            string TripRefNo = context.Request["txtTripid"];
            string BranchID = context.Session["Branch_ID"].ToString();
            string startdate = context.Request["startdate"];
            DateTime dtstartdate = Convert.ToDateTime(startdate);
            string enddate = context.Request["enddate"];
            DateTime dtenddate = Convert.ToDateTime(enddate);
            string refrigerationfuel = context.Request["refrigerationfuel"];
            string Endhrreading = context.Request["Endhrreading"];

            cmd = new MySqlCommand("update tripdata set tripdate=@tripdate,enddate=@enddate, Vehicleno=@Vehicleno, vehiclestartreading=@vehiclestartreading, HourReading=@HourReading,DriverID=@DriverID, HelperID=@HelperID, LoadType=@LoadType, Qty=@Qty,  RouteID=@RouteID,endodometerreading=@endodometerreading,tokenno=@tokenno,pumpreading=@pumpreading,endfuelvalue=@endfuelvalue,EndHrmeter=@Endhrreading,refrigeration_fuel=@refrigerationfuel where UserID=@BranchID and sno=@TripRefNo");
            cmd.Parameters.Add("@tripdate", dtstartdate);
            cmd.Parameters.Add("@enddate", dtenddate);
            cmd.Parameters.Add("@Vehicleno", vehicleNo);
            cmd.Parameters.Add("@vehiclestartreading", VehicleStartReading);
            cmd.Parameters.Add("@HourReading", HrReading);
            cmd.Parameters.Add("@DriverID", driver);
            int helperid = 0;
            int.TryParse(helper, out helperid);
            cmd.Parameters.Add("@HelperID", helperid);
            cmd.Parameters.Add("@LoadType", load);
            cmd.Parameters.Add("@Qty", Qty);
            cmd.Parameters.Add("@RouteID", RouteID);
            cmd.Parameters.Add("@BranchID", BranchID);
            cmd.Parameters.Add("@TripRefNo", TripRefNo);
            cmd.Parameters.Add("@endodometerreading", txtendodometerrdng);
            cmd.Parameters.Add("@tokenno", txttoken);
            cmd.Parameters.Add("@pumpreading", txtpumpreading);
            cmd.Parameters.Add("@endfuelvalue", txtendfuelrdng);
            cmd.Parameters.Add("@refrigerationfuel", refrigerationfuel);
            cmd.Parameters.Add("@Endhrreading", Endhrreading);
            vdm.Update(cmd);
            string msg = "Tripsheet updated successfully";
            string response = GetJson(msg);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string msg = ex.Message;
            string response = GetJson(msg);
            context.Response.Write(response);
        }
    }
    private void GetEditTripSheetValues(HttpContext context)
    {
        try
        {
            vdm = new VehicleDBMgr();
            string TripRefno = context.Request["TripRefno"];
            string BranchID = context.Session["Branch_ID"].ToString();
            cmd = new MySqlCommand("SELECT sno, tripsheetno, tripdate, vehicleno, gpskms, vehiclestartreading, hourreading, driverid, helperid, fueltank,loadtype, EndHrMeter, qty, dieselmeter, routeid, perhrdiesel, starthrmeter,EndHrmeter, status, userid, refrigeration_fuel,operatedby,jobcards, endodometerreading, endfuelvalue,enddate, tokenno, pumpreading, mileage, tripexpences, Rent, DieselCost, perkmcharge, TripSno FROM tripdata WHERE (sno = @TripRefno) AND (userid = @BranchID)");
            cmd.Parameters.Add("@TripRefno", TripRefno);
            cmd.Parameters.Add("@BranchID", BranchID);
            DataTable dtTripsheet = vdm.SelectQuery(cmd).Tables[0];
            List<GetEditTripvalues> Tripsheetlist = new List<GetEditTripvalues>();
            if (dtTripsheet.Rows.Count > 0)
            {
                foreach (DataRow dr in dtTripsheet.Rows)
                {
                    GetEditTripvalues GetTrip = new GetEditTripvalues();
                    GetTrip.Sno = dr["sno"].ToString();
                    GetTrip.VehicleSno = dr["vehicleno"].ToString();
                    GetTrip.DriverID = dr["driverid"].ToString();
                    GetTrip.HelperID = dr["helperid"].ToString();
                    GetTrip.RouteID = dr["routeid"].ToString();
                    GetTrip.VehStartReading = dr["vehiclestartreading"].ToString();
                    GetTrip.HrReading = dr["hourreading"].ToString();
                    GetTrip.LoadType = dr["loadtype"].ToString();
                    GetTrip.Qty = dr["qty"].ToString();
                    GetTrip.EndOdometerReading = dr["endodometerreading"].ToString();
                    GetTrip.Fuelfilled = dr["endfuelvalue"].ToString();
                    GetTrip.PumpReading = dr["pumpreading"].ToString();
                    GetTrip.refrigerationfuel = dr["refrigeration_fuel"].ToString();
                    GetTrip.Token = dr["tokenno"].ToString();
                    GetTrip.endhrreading = dr["EndHrmeter"].ToString();
                    GetTrip.tripdate = ((DateTime)dr["tripdate"]).ToString("yyyy-MM-dd");//dr["tripdate"].ToString();
                    GetTrip.enddate = ((DateTime)dr["enddate"]).ToString("yyyy-MM-dd"); //dr["enddate"].ToString();
                    Tripsheetlist.Add(GetTrip);
                }
                string response = GetJson(Tripsheetlist);
                context.Response.Write(response);
            }
            else
            {
                string msg = "No Tripsheet found";
                string response = GetJson(msg);
                context.Response.Write(response);
            }
        }
        catch (Exception ex)
        {
            string msg = ex.Message;
            string response = GetJson(msg);
            context.Response.Write(response);
        }
    }
    private void btnRouteAssignClick(HttpContext context)
    {
        try
        {
            string vehiclesno = context.Request["ddlvehicle"];
            string despsno = context.Request["ddlroute"];
            cmd = new MySqlCommand("insert into routeassign (despsno,vehicleno) values (@despsno,@vehicleno)");
            cmd.Parameters.Add("@despsno", despsno);
            cmd.Parameters.Add("@vehicleno", vehiclesno);
            vdm.insert(cmd);
            string msg = "Route assign successfully";
            string response = GetJson(msg);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string msg = ex.Message;
            string response = GetJson(msg);
            context.Response.Write(response);
        }
    }
    private void get_route_names(HttpContext context)
    {
        try
        {
            List<Dispatchcls> DispatchplanList = new List<Dispatchcls>();
            //SalesDBManager SDB = new SalesDBManager();
            cmd = new MySqlCommand("SELECT brnch_sno, salesbranchid FROM  branch_info WHERE  (brnch_sno = @BranchID)");
            cmd.Parameters.Add("@BranchID", context.Session["Branch_ID"]);
            DataTable dtBranch = vdm.SelectQuery(cmd).Tables[0];
            string SalesBranchID = "0";
            if (dtBranch.Rows.Count > 0)
            {
                SalesBranchID = dtBranch.Rows[0]["salesbranchid"].ToString();
            }
            cmd = new MySqlCommand("SELECT dispatch.sno, dispatch.DispName FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno WHERE  (dispatch.Branch_Id = @BranchID) AND (dispatch.flag <> 0) OR (branchdata.SalesOfficeID = @BranchID) ORDER BY dispatch.sno");
            cmd.Parameters.Add("@BranchID", SalesBranchID);
            //DataTable dt = SDB.SelectQuery(cmd).Tables[0];
            int i = 1;
            //foreach (DataRow dr in dt.Rows)
            //{
            //    Dispatchcls Getdesp = new Dispatchcls();
            //    Getdesp.DispSno = dr["sno"].ToString();
            //    Getdesp.DispName = dr["DispName"].ToString();
            //    DispatchplanList.Add(Getdesp);
            //}
            string response = GetJson(DispatchplanList);
            context.Response.Write(response);
        }
        catch
        {
        }
    }
    class Batterycls
    {
        public string Sno { get; set; }
        public string BatteryMake { get; set; }
        public string MakeSno { get; set; }
        public string purchasedate { get; set; }
        public string bat_sno { get; set; }
    }
    private void get_Battery_details(HttpContext context)
    {
        try
        {
            string BranchID = context.Session["Branch_ID"].ToString();
            cmd = new MySqlCommand("SELECT minimasters.mm_name,battery_master.Make, battery_master.sno, battery_master.bat_sno, battery_master.purchasedate FROM  battery_master INNER JOIN minimasters ON battery_master.make = minimasters.sno WHERE (battery_master.branch_id = @BranchID)");
            cmd.Parameters.Add("@BranchID", BranchID);
            DataTable dtBattery = vdm.SelectQuery(cmd).Tables[0];
            List<Batterycls> Batterylist = new List<Batterycls>();
            int i = 1;
            foreach (DataRow dr in dtBattery.Rows)
            {
                Batterycls GetBattery = new Batterycls();
                GetBattery.Sno = dr["sno"].ToString();
                GetBattery.BatteryMake = dr["mm_name"].ToString();
                GetBattery.MakeSno = dr["Make"].ToString();
                GetBattery.purchasedate = ((DateTime)dr["purchasedate"]).ToString("yyyy-MM-dd");//dr["purchasedate"].ToString();
                GetBattery.bat_sno = dr["bat_sno"].ToString();
                Batterylist.Add(GetBattery);
            }
            string response = GetJson(Batterylist);
            context.Response.Write(response);
        }
        catch
        {
        }
    }
    private void btnsavebatterymasterClick(HttpContext context)
    {
        try
        {
            string batterymake = context.Request["batterymake"];
            string batterynumber = context.Request["batterynumber"];
            string purchasedate = context.Request["purchasedate"];
            string btnval = context.Request["btnval"];
            string sno = context.Request["sno"];
            DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
            string msg = "";
            if (btnval == "Save")
            {
                cmd = new MySqlCommand("insert into battery_master (make,bat_sno,purchasedate,branch_id,doe) values (@make,@bat_sno,@purchasedate,@branch_id,@doe)");
                cmd.Parameters.Add("@make", batterymake);
                cmd.Parameters.Add("@bat_sno", batterynumber);
                cmd.Parameters.Add("@purchasedate", purchasedate);
                cmd.Parameters.Add("@doe", ServerDateCurrentdate);
                cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
                vdm.insert(cmd);
                msg = "Battery Saved successfully";
            }
            else
            {
                cmd = new MySqlCommand("update  battery_master set make= @make,bat_sno=@bat_sno,purchasedate=@purchasedate where branch_id=@branch_id and sno=@sno");
                cmd.Parameters.Add("@sno", sno);
                cmd.Parameters.Add("@make", batterymake);
                cmd.Parameters.Add("@bat_sno", batterynumber);
                cmd.Parameters.Add("@purchasedate", purchasedate);
                cmd.Parameters.Add("@doe", ServerDateCurrentdate);
                cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
                vdm.Update(cmd);
                msg = "Battery Master successfully updated";
            }
            string response = GetJson(msg);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string msg = ex.Message;
            string response = GetJson(msg);
            context.Response.Write(response);
        }
    }
    private void get_insurancecompany(HttpContext context)
    {
        try
        {
            cmd = new MySqlCommand("SELECT sno, vendorname FROM vendors_info WHERE (vendor_type = @vendortype) AND (branch_id = @BranchID)");
            cmd.Parameters.Add("@vendortype", "Insurance");
            cmd.Parameters.Add("@BranchID", context.Session["Branch_ID"]);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            List<Insurancecls> Insurancelist = new List<Insurancecls>();
            int i = 1;
            foreach (DataRow dr in dt.Rows)
            {
                Insurancecls GetInsurance = new Insurancecls();
                GetInsurance.InsSno = dr["sno"].ToString();
                GetInsurance.InsuranceName = dr["vendorname"].ToString();
                Insurancelist.Add(GetInsurance);
            }
            string response = GetJson(Insurancelist);
            context.Response.Write(response);
        }
        catch
        {
        }
    }
    //private void btnVehicleEMISaveClick(HttpContext context)
    //{
    //    try
    //    {
    //        string vehicleno = context.Request["vehicleno"];
    //        string Amount = context.Request["Amount"];
    //        string Paymenttype = context.Request["Paymenttype"];
    //        string remarks = context.Request["remarks"];
    //        string type = context.Request["type"];
    //        DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
    //        cmd = new MySqlCommand("insert into termloantransactions (vehsno,doe,paymenttype,amount,remarks,branchid) values (@vehsno,@doe,@paymenttype,@amount,@remarks,@branchid)");
    //        cmd.Parameters.Add("@vehsno", vehicleno);
    //        cmd.Parameters.Add("@doe", ServerDateCurrentdate);
    //        cmd.Parameters.Add("@paymenttype", Paymenttype);
    //        cmd.Parameters.Add("@amount", Amount);
    //        cmd.Parameters.Add("@remarks", remarks);
    //        cmd.Parameters.Add("@branchid", context.Session["Branch_ID"]);
    //        vdm.insert(cmd);
    //        int count = 1;
    //        cmd = new MySqlCommand("update termloanentry set com_install=com_install+@com_install  where branchid= @branchid and vehsno=@vehsno and type=@type");
    //        cmd.Parameters.Add("@vehsno", vehicleno);
    //        cmd.Parameters.Add("@com_install", count);
    //        cmd.Parameters.Add("@type", type);
    //        cmd.Parameters.Add("@branchid", context.Session["Branch_ID"]);
    //        vdm.Update(cmd);
    //        string msg = "Term loan EMI successfully updated";
    //        string response = GetJson(msg);
    //        context.Response.Write(response);
    //    }
    //    catch (Exception ex)
    //    {
    //        string msg = ex.Message;
    //        string response = GetJson(msg);
    //        context.Response.Write(response);
    //    }
    //}
    private void get_all_vehhilcles(HttpContext context)
    {
        try
        {

            cmd = new MySqlCommand("SELECT  vehicel_master.vm_sno, vehicel_master.registration_no, vehicel_master.Capacity, minimasters.mm_name AS VehType, vehicel_master.fuel_capacity, minimasters_1.mm_name AS make FROM vehicel_master INNER JOIN minimasters ON vehicel_master.vhtype_refno = minimasters.sno INNER JOIN minimasters minimasters_1 ON vehicel_master.vhmake_refno = minimasters_1.sno WHERE (vehicel_master.branch_id = @BranchID)");
            // cmd = new MySqlCommand("SELECT vehicel_master.vm_sno, vehicel_master.registration_no, vehicel_master.door_no, vehicel_master.status, vehicel_master.branch_id, vehicel_master.operatedby, vehicel_master.Capacity, minimasters.mm_name AS veh_type, minimasters_1.mm_name AS veh_make, vehicel_master.fuel_capacity, vehicel_master.axils_refno, axil_master.axilmaster_name FROM vehicel_master INNER JOIN minimasters ON vehicel_master.vhtype_refno = minimasters.sno INNER JOIN minimasters minimasters_1 ON vehicel_master.vhmake_refno = minimasters_1.sno LEFT OUTER JOIN axil_master ON vehicel_master.axils_refno = axil_master.sno WHERE (vehicel_master.branch_id = @branch_id) AND (vehicel_master.vm_sno NOT IN (SELECT vehicleno FROM tripdata WHERE (status = 'A')))");
            cmd.Parameters.Add("@BranchID", context.Session["Branch_ID"]);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            List<get_veh_master_data> vendor = new List<get_veh_master_data>();

            foreach (DataRow dr in dt.Rows)
            {
                get_veh_master_data data = new get_veh_master_data();
                data.vm_sno = dr["vm_sno"].ToString();
                data.registration_no = dr["registration_no"].ToString();
                data.Capacity = dr["Capacity"].ToString();
                data.VehType = dr["VehType"].ToString();
                data.VehMake = dr["make"].ToString();
                data.v_ty_fuel_capacity = dr["fuel_capacity"].ToString();
                vendor.Add(data);
            }
            string response = GetJson(vendor);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }
    public class all_triplogs_data
    {
        public string sno { get; set; }
        public string doe { get; set; }
        public string km { get; set; }
        public string place { get; set; }
        public string details { get; set; }
        public string expamount { get; set; }
        public string fuel { get; set; }
        public string fuel_type { get; set; }
        public string tripsno { get; set; }
        public string load_cap { get; set; }
        public string load_cap_kgs { get; set; }
        public string unload_cap { get; set; }
        public string unload_cap_kgs { get; set; }
        public string charge { get; set; }
        public string log_rank { get; set; }
        public string tollgateamnt { get; set; }
        public string odometer { get; set; }
        public string remarks { get; set; }
        public string acfuel { get; set; }

    }
    private void btnVehicleTermLoanEntrySaveClick(HttpContext context)
    {
        try
        {
            string vehicleno = context.Request["vehicleno"];
            string mfgdate = context.Request["mfgdate"];
            string termloanno = context.Request["termloanno"];
            string loanamount = context.Request["termloanamount"];
            string monthlyinstal = context.Request["monthlyinstal"];
            string instalmentdate = context.Request["instalmentdate"];
            string termLoandate = context.Request["termLoandate"];
            string totalinstal = context.Request["totalinstal"];
            string paidinstal = context.Request["paidinstal"];
            string bankname = context.Request["bankname"];
            string remarks = context.Request["remarks"];
            string BodyType = context.Request["Type"];
            string interest_per = context.Request["Interestper"];
            string ledgername = context.Request["ledgername"];
            string ledgercode = context.Request["ledgercode"];
            DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
            string btn_save = context.Request["btn_save"];
            if (btn_save == "Save")
            {
                cmd = new MySqlCommand(" insert into termloanentry (vehsno,doe,mfgdate,termloanno,loanamount,instalamount,instaldate,totalinstall,com_install,remarks,bankname,branchid,type,interest_per,ledgername,termloandate,ledger_code) values (@vehsno,@doe,@mfgdate,@termloanno,@loanamount,@instalamount,@instaldate,@totalinstall,@com_install,@remarks,@bankname,@branchid,@type,@interest_per,@ledgername,@termloandate,@ledger_code)");
                cmd.Parameters.Add("@vehsno", vehicleno);
                cmd.Parameters.Add("@doe", ServerDateCurrentdate);
                cmd.Parameters.Add("@mfgdate", mfgdate);
                cmd.Parameters.Add("@termloanno", termloanno);
                cmd.Parameters.Add("@loanamount", loanamount);
                cmd.Parameters.Add("@instalamount", monthlyinstal);
                cmd.Parameters.Add("@instaldate", instalmentdate);
                cmd.Parameters.Add("@totalinstall", totalinstal);
                cmd.Parameters.Add("@com_install", paidinstal);
                cmd.Parameters.Add("@termloandate", termLoandate);
                cmd.Parameters.Add("@remarks", remarks);
                cmd.Parameters.Add("@type", BodyType);
                cmd.Parameters.Add("@bankname", bankname);
                cmd.Parameters.Add("@interest_per", interest_per);
                cmd.Parameters.Add("@ledgername", ledgername);
                cmd.Parameters.Add("@ledger_code", ledgercode);
                cmd.Parameters.Add("@branchid", context.Session["Branch_ID"]);
                vdm.insert(cmd);
            }
            else
            {
                string refno = context.Request["refno"];
                cmd = new MySqlCommand("update termloanentry set ledger_code=@ledger_code, interest_per=@interest_per,ledgername=@ledgername,termloandate=@termloandate,type=@type,loanamount=@loanamount,instalamount=@instalamount,totalinstall=@totalinstall,instaldate=@instaldate where sno=@sno");
                cmd.Parameters.Add("@interest_per", interest_per);
                cmd.Parameters.Add("@ledgername", ledgername);
                cmd.Parameters.Add("@ledger_code", ledgercode);
                cmd.Parameters.Add("@termloandate", termLoandate);
                cmd.Parameters.Add("@type", BodyType);
                cmd.Parameters.Add("@loanamount", loanamount);
                cmd.Parameters.Add("@instalamount", monthlyinstal);
                cmd.Parameters.Add("@totalinstall", totalinstal);
                cmd.Parameters.Add("@instaldate", instalmentdate);
                cmd.Parameters.Add("@sno", refno);
                vdm.Update(cmd);
            }
            string msg = "Term loan entry saved successfully";
            string response = GetJson(msg);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string msg = ex.Message;
            string response = GetJson(msg);
            context.Response.Write(response);
        }
    }
    private void btnSalaryadvanceClick(HttpContext context)
    {
        try
        {
            string EmpSno = context.Request["EmpSno"];
            string Amount = context.Request["Amount"];
            string Paymenttype = context.Request["Paymenttype"];
            string Remarks = context.Request["Remarks"];
            string Paidby = context.Request["Paidby"];
            string Cost = context.Request["Cost"];
            DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
            cmd = new MySqlCommand(" insert into salaryadvance (empsno,advanceamount,doe,remarks,paymenttype,paidby) values (@empsno,@advanceamount,@doe,@remarks,@paymenttype,@paidby)");
            cmd.Parameters.Add("@empsno", EmpSno);
            cmd.Parameters.Add("@advanceamount", Amount);
            cmd.Parameters.Add("@doe", ServerDateCurrentdate);
            cmd.Parameters.Add("@remarks", Remarks);
            cmd.Parameters.Add("@paymenttype", Paymenttype);
            cmd.Parameters.Add("@paidby", Paidby);
            vdm.insert(cmd);
            string msg = "Salary advance saved successfully";
            string response = GetJson(msg);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string msg = ex.Message;
            string response = GetJson(msg);
            context.Response.Write(response);
        }
    }
    private void btnUpdatePuffRentclick(HttpContext context)
    {
        try
        {
            string VehilceSno = context.Request["VehilceSno"];
            string Cost = context.Request["Cost"];
            string CostPerKm = context.Request["CostPerKm"];
            double perkmcharge = 0;
            double.TryParse(CostPerKm, out  perkmcharge);
            cmd = new MySqlCommand(" update vehicel_master set cost=@cost,perkmcharge=@perkmcharge where  vm_sno=@VehicleSno ");
            cmd.Parameters.Add("@cost", Cost);
            cmd.Parameters.Add("@perkmcharge", perkmcharge);
            cmd.Parameters.Add("@VehicleSno", VehilceSno);
            vdm.Update(cmd);
            string msg = "Rates Updated Successfully";
            string response = GetJson(msg);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string msg = ex.Message;
            string response = GetJson(msg);
            context.Response.Write(response);
        }
    }
    public class Vehicleclass
    {
        public string VehicleName { get; set; }
        public string VehicleNo { get; set; }
        public string cost { get; set; }
        public string costPerKm { get; set; }
        public string sno { get; set; }
    }
    private void GetVehiclerents(HttpContext context)
    {
        try
        {
            string ddlVehicletype = context.Request["ddlVehicletype"];
            cmd = new MySqlCommand(" SELECT registration_no, vm_sno, Cost,perkmcharge FROM vehicel_master WHERE (vhtype_refno = @ddlVehicletype) AND (vm_owner=@Owner)   AND (branch_id=@BranchID)");
            cmd.Parameters.Add("@Owner", context.Session["shortname"].ToString());
            cmd.Parameters.Add("@ddlVehicletype", ddlVehicletype);
            cmd.Parameters.Add("@BranchID", context.Session["Branch_ID"]);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            List<Vehicleclass> Vehiclelist = new List<Vehicleclass>();
            int i = 1;
            foreach (DataRow dr in dt.Rows)
            {
                Vehicleclass GetVehicle = new Vehicleclass();
                GetVehicle.VehicleName = dr["registration_no"].ToString();
                GetVehicle.VehicleNo = dr["vm_sno"].ToString();
                GetVehicle.cost = dr["Cost"].ToString();
                GetVehicle.costPerKm = dr["perkmcharge"].ToString();
                GetVehicle.sno = i++.ToString();
                Vehiclelist.Add(GetVehicle);
            }
            string response = GetJson(Vehiclelist);
            context.Response.Write(response);
        }
        catch
        {
        }
    }

    private void get_Edit_triplogs_data(HttpContext context)
    {
        try
        {
            string tripid = context.Request["tripid"];
            string UserID = context.Session["Branch_ID"].ToString();
            if (tripid != "0")
            {
                cmd = new MySqlCommand("SELECT sno, doe, km, place, details, expamount, fuel, fuel_type, tripsno, load_cap, load_cap_kgs, unload_cap, unload_cap_kgs, charge, log_rank, tollgateamnt, odometer,remarks,acfuel FROM triplogs where tripsno=@tripsno ORDER BY log_rank");
                cmd.Parameters.Add("@tripsno", tripid);
                DataTable dt = vdm.SelectQuery(cmd).Tables[0];
                List<all_triplogs_data> diesel = new List<all_triplogs_data>();
                foreach (DataRow dr in dt.Rows)
                {
                    all_triplogs_data mont = new all_triplogs_data();
                    mont.sno = dr["sno"].ToString();
                    mont.doe = ((DateTime)dr["doe"]).ToString("yyyy-MM-ddThh:mm");
                    mont.km = dr["km"].ToString();
                    mont.place = dr["place"].ToString();
                    mont.details = dr["details"].ToString();
                    mont.expamount = dr["expamount"].ToString();
                    mont.fuel = dr["fuel"].ToString();
                    mont.fuel_type = dr["fuel_type"].ToString();
                    mont.tripsno = dr["tripsno"].ToString();
                    mont.load_cap = dr["load_cap"].ToString();
                    mont.load_cap_kgs = dr["load_cap_kgs"].ToString();
                    mont.unload_cap = dr["unload_cap"].ToString();
                    mont.unload_cap_kgs = dr["unload_cap_kgs"].ToString();
                    mont.charge = dr["charge"].ToString();
                    mont.log_rank = dr["log_rank"].ToString();
                    mont.tollgateamnt = dr["tollgateamnt"].ToString();
                    mont.odometer = dr["odometer"].ToString();
                    mont.remarks = dr["remarks"].ToString();
                    mont.acfuel = dr["acfuel"].ToString();
                    diesel.Add(mont);
                }
                string response = GetJson(diesel);
                context.Response.Write(response);
            }
            else
            {
                string msg = "No tripsheet found";
                string response = GetJson(msg);
                context.Response.Write(response);

            }
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }

    private void get_triplogs_data(HttpContext context)
    {
        try
        {
            string Tripsno = context.Request["tripid"];
            string UserID = context.Session["Branch_ID"].ToString();
            cmd = new MySqlCommand("SELECT sno, doe, km, place, details, expamount, fuel, fuel_type, tripsno, load_cap, load_cap_kgs, unload_cap, unload_cap_kgs, charge, log_rank, tollgateamnt, odometer,remarks FROM triplogs where tripsno=@tripsno ORDER BY log_rank");
            cmd.Parameters.Add("@tripsno", Tripsno);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            List<all_triplogs_data> diesel = new List<all_triplogs_data>();
            foreach (DataRow dr in dt.Rows)
            {
                all_triplogs_data mont = new all_triplogs_data();
                mont.sno = dr["sno"].ToString();
                mont.doe = ((DateTime)dr["doe"]).ToString("yyyy-MM-ddThh:mm");
                mont.km = dr["km"].ToString();
                mont.place = dr["place"].ToString();
                mont.details = dr["details"].ToString();
                mont.expamount = dr["expamount"].ToString();
                mont.fuel = dr["fuel"].ToString();
                mont.fuel_type = dr["fuel_type"].ToString();
                mont.tripsno = dr["tripsno"].ToString();
                mont.load_cap = dr["load_cap"].ToString();
                mont.load_cap_kgs = dr["load_cap_kgs"].ToString();
                mont.unload_cap = dr["unload_cap"].ToString();
                mont.unload_cap_kgs = dr["unload_cap_kgs"].ToString();
                mont.charge = dr["charge"].ToString();
                mont.log_rank = dr["log_rank"].ToString();
                mont.tollgateamnt = dr["tollgateamnt"].ToString();
                mont.odometer = dr["odometer"].ToString();
                mont.remarks = dr["remarks"].ToString();
                diesel.Add(mont);
            }
            string response = GetJson(diesel);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }

    public class get_fuel_monitor
    {
        public string sno { get; set; }
        public string fuel { get; set; }
        public string costperltr { get; set; }
    }
    private void get_diesel_cost(HttpContext context)
    {
        try
        {
            cmd = new MySqlCommand("SELECT sno, fuel, userid, operatedby, costperltr FROM fuel_monitor WHERE (userid = @userid)");
            cmd.Parameters.Add("@userid", context.Session["Branch_ID"]);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            List<get_fuel_monitor> diesel = new List<get_fuel_monitor>();
            foreach (DataRow dr in dt.Rows)
            {
                get_fuel_monitor mont = new get_fuel_monitor();
                mont.costperltr = dr["costperltr"].ToString();
                mont.sno = dr["sno"].ToString();
                mont.fuel = dr["fuel"].ToString();
                diesel.Add(mont);
            }
            string response = GetJson(diesel);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }

    }

    private void save_edit_Tyres_new(HttpContext context)
    {
        try
        {
            string SVDSNO = context.Request["SVDSNO"];
            string min_grove = context.Request["min_grove"];
            string max_grove = context.Request["max_grove"];
            string Fitting_type = context.Request["grove"];
            string sno = context.Request["sno"];
            string currentkms = context.Request["currentkms"];
            string tyre_sno = context.Request["tyreno"];
            string flag = context.Request["flag"];
            cmd = new MySqlCommand("update new_tyres_sub set tyre_sno=@tyre_sno,svdsno=@svdsno,min_grove=@min_grove,max_grove=@max_grove,Fitting_type=@Fitting_type,current_kms=@current_kms ,flag=@flag where sno=@sno");
            cmd.Parameters.Add("@svdsno", SVDSNO);
            double min_grove_ = 0.00;
            double.TryParse(min_grove, out min_grove_);
            double max_grove_ = 0.00;
            double.TryParse(max_grove, out max_grove_);
            cmd.Parameters.Add("@min_grove", min_grove_);
            cmd.Parameters.Add("@max_grove", max_grove_);
            cmd.Parameters.Add("@Fitting_type", Fitting_type);
            cmd.Parameters.Add("@sno", sno);
            cmd.Parameters.Add("@current_kms", currentkms);
            cmd.Parameters.Add("@tyre_sno", tyre_sno);
            if (flag == "0")
            {
                cmd.Parameters.Add("@flag", false);
            }
            else
            {
                cmd.Parameters.Add("@flag", true);
            }
            vdm.Update(cmd);
            string response = GetJson("Tyres Edited Successfully");
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }
    private void get_tollgates(HttpContext context)
    {
        try
        {
            string tripid = context.Request["tripid"];
            cmd = new MySqlCommand("SELECT sno, tripsheetno, tripdate, vehicleno, gpskms, vehiclestartreading, hourreading, driverid, helperid, fueltank, loadtype, EndHrMeter,qty, dieselmeter, routeid,perhrdiesel, starthrmeter, status, userid, operatedby, jobcards, endodometerreading, endfuelvalue,enddate, tokenno, pumpreading, mileage FROM tripdata");
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            List<al_tripdata_items> vendor = new List<al_tripdata_items>();
            foreach (DataRow dr in dt.Rows)
            {
                al_tripdata_items data = new al_tripdata_items();
                data.sno = dr["sno"].ToString();
                data.tripsheetno = dr["tripsheetno"].ToString();
                vendor.Add(data);
            }
            string response = GetJson(vendor);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }
    public class al_tripdata_items
    {
        public string sno { get; set; }
        public string tripsheetno { get; set; }
        public string tripdate { get; set; }
        public string vehicleno { get; set; }
        public string gpskms { get; set; }
        public string vehiclestartreading { get; set; }
        public string hourreading { get; set; }
        public string driverid { get; set; }
        public string helperid { get; set; }
        public string fueltank { get; set; }
        public string loadtype { get; set; }
        public string EndHrMeter { get; set; }
        public string qty { get; set; }
        public string dieselmeter { get; set; }
        public string routeid { get; set; }
        public string perhrdiesel { get; set; }
        public string starthrmeter { get; set; }
        public string status { get; set; }
        public string userid { get; set; }
        public string jobcards { get; set; }
        public string endodometerreading { get; set; }
        public string endfuelvalue { get; set; }
        public string enddate { get; set; }
        public string tokenno { get; set; }
        public string pumpreading { get; set; }
        public string mileage { get; set; }
    }
    private void get_all_tripsheets(HttpContext context)
    {
        try
        {
            cmd = new MySqlCommand("SELECT sno, tripsheetno, tripdate, vehicleno, gpskms, vehiclestartreading, hourreading, driverid, helperid, fueltank, loadtype, EndHrMeter,qty, dieselmeter, routeid,perhrdiesel, starthrmeter, status, userid, operatedby, jobcards, endodometerreading, endfuelvalue,enddate, tokenno, pumpreading, mileage FROM tripdata WHERE (status = 'C')");
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            List<al_tripdata_items> vendor = new List<al_tripdata_items>();
            foreach (DataRow dr in dt.Rows)
            {
                al_tripdata_items data = new al_tripdata_items();
                data.sno = dr["sno"].ToString();
                data.tripsheetno = dr["tripsheetno"].ToString();
                vendor.Add(data);
            }
            string response = GetJson(vendor);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }
    private void check_triplog_save(HttpContext context)
    {
        try
        {
            string tripid = context.Request["tripid"];
            cmd = new MySqlCommand("SELECT sno, doe, km, place, details, expamount, fuel, fuel_type, tripsno, load_cap, load_cap_kgs, unload_cap, unload_cap_kgs, charge, log_rank, tollgateamnt, odometer,remarks FROM triplogs triplogs_1 WHERE (tripsno = @tripsno)");
            cmd.Parameters.Add("@tripsno", tripid);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            if (dt.Rows.Count > 0)
            {
                string response = GetJson("YES");
                context.Response.Write(response);
            }
            else
            {
                string response = GetJson("NO");
                context.Response.Write(response);
            }
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }

    private void get_remaining_tyres_data(HttpContext context)
    {
        try
        {
            string vehicleno = context.Request["vehicleno"];
            string fittingtype = context.Request["fittingtype"];
            if (fittingtype == "Fitted")
            {
                cmd = new MySqlCommand("SELECT new_tyres_sub.flag, new_tyres_sub.tyre_sno, new_tyres_sub.tyre_tube, new_tyres_sub.Fitting_Type, new_tyres_sub.cost, new_tyres_sub.status, new_tyres_sub.current_KMS, new_tyres_sub.current_hrs,new_tyres_sub.Fitting_Type, new_tyres_sub.frontrback, new_tyres_sub.svdsno, new_tyres_sub.grove, new_tyres_sub.min_grove, new_tyres_sub.max_grove,new_tyres_sub.sno AS tyresub_sno, new_tyres_sub.newtyre_refno, minimasters_1.mm_name AS size, minimasters.mm_name AS type_of_tyre,minimasters_2.mm_name AS brand, vehicle_master_sub.vehicle_mstr_sno FROM new_tyres_sub INNER JOIN minimasters minimasters_1 ON new_tyres_sub.size = minimasters_1.sno INNER JOIN minimasters ON new_tyres_sub.type_of_tyre = minimasters.sno INNER JOIN minimasters minimasters_2 ON new_tyres_sub.brand = minimasters_2.sno INNER JOIN vehicle_master_sub ON new_tyres_sub.sno = vehicle_master_sub.tyre_sno WHERE (vehicle_master_sub.vehicle_mstr_sno = @vehicle_mstr_sno) order by new_tyres_sub.svdsno");
                cmd.Parameters.Add("@vehicle_mstr_sno", vehicleno);
                DataTable dt = vdm.SelectQuery(cmd).Tables[0];
                List<Tyres_items> vendor = new List<Tyres_items>();
                foreach (DataRow dr in dt.Rows)
                {
                    Tyres_items data = new Tyres_items();
                    data.sno = dr["tyresub_sno"].ToString();
                    data.tyresno = dr["tyre_sno"].ToString();
                    data.Size = dr["size"].ToString();
                    data.Type_of_Tyre = dr["type_of_tyre"].ToString();
                    data.Tube_Type = dr["tyre_tube"].ToString();
                    data.Cost = dr["cost"].ToString();
                    data.Brand = dr["brand"].ToString();
                    data.SVDSNO = dr["svdsno"].ToString();
                    data.grove = dr["Fitting_Type"].ToString();
                    data.min_grove = dr["min_grove"].ToString();
                    data.max_grove = dr["max_grove"].ToString();
                    data.currentkms = dr["current_KMS"].ToString();
                    data.flag = dr["flag"].ToString();
                    vendor.Add(data);
                }
                string response = GetJson(vendor);
                context.Response.Write(response);
            }
            else
            {
                cmd = new MySqlCommand("SELECT new_tyres_sub.flag, new_tyres_sub.tyre_sno, new_tyres_sub.tyre_tube, new_tyres_sub.Fitting_Type, new_tyres_sub.cost, new_tyres_sub.status, new_tyres_sub.current_KMS, new_tyres_sub.current_hrs,new_tyres_sub.Fitting_Type, new_tyres_sub.frontrback, new_tyres_sub.svdsno, new_tyres_sub.grove, new_tyres_sub.min_grove, new_tyres_sub.max_grove,new_tyres_sub.sno AS tyresub_sno, new_tyres_sub.newtyre_refno, minimasters_1.mm_name AS size, minimasters.mm_name AS type_of_tyre, minimasters_2.mm_name AS brand FROM new_tyres_sub INNER JOIN minimasters minimasters_1 ON new_tyres_sub.size = minimasters_1.sno INNER JOIN minimasters ON new_tyres_sub.type_of_tyre = minimasters.sno INNER JOIN minimasters minimasters_2 ON new_tyres_sub.brand = minimasters_2.sno");
                //cmd = new MySqlCommand("SELECT new_tyres_sub.tyre_sno, new_tyres_sub.tyre_tube, new_tyres_sub.cost, new_tyres_sub.status, new_tyres_sub.current_KMS, new_tyres_sub.current_hrs,new_tyres_sub.Fitting_Type, new_tyres_sub.frontrback, new_tyres_sub.svdsno, new_tyres_sub.grove, new_tyres_sub.min_grove, new_tyres_sub.max_grove,new_tyres_sub.sno AS tyresub_sno, new_tyres_sub.newtyre_refno, minimasters_1.mm_name AS size, minimasters.mm_name AS type_of_tyre, minimasters_2.mm_name AS brand FROM new_tyres_sub INNER JOIN minimasters minimasters_1 ON new_tyres_sub.size = minimasters_1.sno INNER JOIN minimasters ON new_tyres_sub.type_of_tyre = minimasters.sno INNER JOIN minimasters minimasters_2 ON new_tyres_sub.brand = minimasters_2.sno WHERE (new_tyres_sub.Fitting_Type = 'NF')");
                //cmd = new MySqlCommand("SELECT new_tyres_sub.tyre_sno, new_tyres_sub.tyre_tube, new_tyres_sub.cost, new_tyres_sub.status, new_tyres_sub.current_KMS, new_tyres_sub.current_hrs,new_tyres_sub.Fitting_Type, new_tyres_sub.frontrback, new_tyres_sub.svdsno, new_tyres_sub.grove, new_tyres_sub.min_grove, new_tyres_sub.max_grove,new_tyres_sub.sno AS tyresub_sno, new_tyres_sub.newtyre_refno, minimasters_1.mm_name AS size, minimasters.mm_name AS type_of_tyre,minimasters_2.mm_name AS brand, vehicle_master_sub.vehicle_mstr_sno FROM new_tyres_sub INNER JOIN minimasters minimasters_1 ON new_tyres_sub.size = minimasters_1.sno INNER JOIN minimasters ON new_tyres_sub.type_of_tyre = minimasters.sno INNER JOIN minimasters minimasters_2 ON new_tyres_sub.brand = minimasters_2.sno INNER JOIN vehicle_master_sub ON new_tyres_sub.sno = vehicle_master_sub.tyre_sno WHERE (vehicle_master_sub.vehicle_mstr_sno = @vehicle_mstr_sno)");
                cmd.Parameters.Add("@vehicle_mstr_sno", vehicleno);
                DataTable dt = vdm.SelectQuery(cmd).Tables[0];
                List<Tyres_items> vendor = new List<Tyres_items>();
                foreach (DataRow dr in dt.Rows)
                {
                    Tyres_items data = new Tyres_items();
                    data.sno = dr["tyresub_sno"].ToString();
                    data.tyresno = dr["tyre_sno"].ToString();
                    data.Size = dr["size"].ToString();
                    data.Type_of_Tyre = dr["type_of_tyre"].ToString();
                    data.Tube_Type = dr["tyre_tube"].ToString();
                    data.Cost = dr["cost"].ToString();
                    data.Brand = dr["brand"].ToString();
                    data.SVDSNO = dr["svdsno"].ToString();
                    data.grove = dr["Fitting_Type"].ToString();
                    data.min_grove = dr["min_grove"].ToString();
                    data.max_grove = dr["max_grove"].ToString();
                    data.currentkms = dr["current_KMS"].ToString();
                    data.flag = dr["flag"].ToString();
                    vendor.Add(data);
                }
                string response = GetJson(vendor);
                context.Response.Write(response);
            }
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }

    public class get_first_data_tyre
    {
        public string sno { get; set; }
        public string purchase_date { get; set; }
        public string entry_date { get; set; }
        public string vendor_refno { get; set; }
        public string invoiceno { get; set; }
        public string payment_type { get; set; }
        public string total_cost { get; set; }
        public string tax { get; set; }
        public string other_expences { get; set; }
        public string discount { get; set; }
        public string grand_total { get; set; }
        public string payable_cost { get; set; }
    }

    private void get_only_tyres_first_data(HttpContext context)
    {
        try
        {
            cmd = new MySqlCommand("SELECT sno, purchase_date, entry_date, vendor_refno, invoiceno, payment_type, total_cost, tax, other_expences, discount, grand_total, payable_cost, branch_id,user_id FROM new_tyres WHERE (branch_id = @branch_id)");
            cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            List<get_first_data_tyre> vendor = new List<get_first_data_tyre>();
            foreach (DataRow dr in dt.Rows)
            {
                get_first_data_tyre data = new get_first_data_tyre();
                data.sno = dr["sno"].ToString();
                if (dr["purchase_date"].ToString() != null && dr["purchase_date"].ToString() != "")
                {
                    data.purchase_date = ((DateTime)dr["purchase_date"]).ToString("yyyy-MM-dd");
                }
                else
                {
                    data.purchase_date = dr["purchase_date"].ToString();
                }
                data.entry_date = dr["entry_date"].ToString();
                data.vendor_refno = dr["vendor_refno"].ToString();
                data.invoiceno = dr["invoiceno"].ToString();
                data.payment_type = dr["payment_type"].ToString();
                data.total_cost = dr["total_cost"].ToString();
                data.tax = dr["tax"].ToString();
                data.other_expences = dr["other_expences"].ToString();
                data.discount = dr["discount"].ToString();
                data.grand_total = dr["grand_total"].ToString();
                data.payable_cost = dr["payable_cost"].ToString();
                vendor.Add(data);
            }
            string response = GetJson(vendor);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }

    public class get_tyres_for_vehicle_cls
    {
        public string sno { get; set; }
        public string tyre_sno { get; set; }
        public string brand { get; set; }
        public string tyretype { get; set; }
        public string tubetyre { get; set; }
        public string size { get; set; }
        public string current_KMS { get; set; }
        public string TyreName { get; set; }
    }

    private void get_tyres_for_vehicle(HttpContext context)
    {
        try
        {
            string vehicle_sno = context.Request["vehicle_sno"];
            cmd = new MySqlCommand("SELECT vehicel_master.registration_no AS VehicleNo, axils_tyres_names.tyrename AS TyreName, new_tyres_sub.tyre_sno AS TyreNo, new_tyres_sub.cost, minimasters.mm_name AS Brand, new_tyres_sub.tyre_tube,minimasters_1.mm_name AS TyreType, new_tyres_sub.sno, minimasters_2.mm_name AS size,new_tyres_sub.current_KMS FROM vehicel_master INNER JOIN vehicle_master_sub ON vehicel_master.vm_sno = vehicle_master_sub.vehicle_mstr_sno INNER JOIN new_tyres_sub ON vehicle_master_sub.tyre_sno = new_tyres_sub.sno INNER JOIN axils_tyres_names ON vehicle_master_sub.axles_tyres_names_sno = axils_tyres_names.sno INNER JOIN minimasters ON new_tyres_sub.brand = minimasters.sno INNER JOIN minimasters minimasters_1 ON new_tyres_sub.type_of_tyre = minimasters_1.sno INNER JOIN minimasters minimasters_2 ON new_tyres_sub.size = minimasters_2.sno WHERE (vehicel_master.branch_id = @BranchID) AND (vehicel_master.vm_sno = @vm_sno)");
            cmd.Parameters.Add("@BranchID", context.Session["Branch_ID"]);
            cmd.Parameters.Add("@vm_sno", vehicle_sno);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            List<get_tyres_for_vehicle_cls> vendor = new List<get_tyres_for_vehicle_cls>();
            foreach (DataRow dr in dt.Rows)
            {
                get_tyres_for_vehicle_cls data = new get_tyres_for_vehicle_cls();
                data.sno = dr["sno"].ToString();
                data.tyre_sno = dr["TyreNo"].ToString();
                data.brand = dr["Brand"].ToString();
                data.tyretype = dr["TyreType"].ToString();
                data.tubetyre = dr["tyre_tube"].ToString();
                data.size = dr["size"].ToString();
                data.current_KMS = dr["current_KMS"].ToString();
                data.TyreName = dr["TyreName"].ToString();
                vendor.Add(data);
            }
            string response = GetJson(vendor);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }


    private void getallveh_nos_frommoduleConfig(HttpContext context)
    {
        try
        {            
            cmd = new MySqlCommand("  ");
            cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            List<get_veh_master_data> vendor = new List<get_veh_master_data>();
            foreach (DataRow dr in dt.Rows)
            {
                get_veh_master_data data = new get_veh_master_data();
                data.vm_sno = dr["vm_sno"].ToString();
                data.VehType = dr["veh_type"].ToString();
                data.VehMake = dr["veh_make"].ToString();
                data.registration_no = dr["registration_no"].ToString();
                data.door_no = dr["door_no"].ToString();
                data.Capacity = dr["Capacity"].ToString();

                data.v_ty_fuel_capacity = dr["fuel_capacity"].ToString();
                data.status = dr["status"].ToString();
                data.axils_refno = dr["axils_refno"].ToString();
                data.axilmaster_name = dr["axilmaster_name"].ToString();
                vendor.Add(data);
            }
            string response = GetJson(vendor);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }
    private void get_all_veh_master_data_notrips(HttpContext context)
    {
        try
        {

            //cmd = new MySqlCommand("SELECT minimasters.mm_name AS VehMake, axils_master.axil_name, vehicel_master.vm_sno, vehicel_master.registration_no, vehicel_master.door_no, vehicel_master.fuel_capacity, vehicel_master.status,minimasters_2.mm_name AS Category, vehicle_types.v_ty_name AS VehType, vehicel_master.branch_id, vehicel_master.operatedby FROM vehicel_master INNER JOIN minimasters ON vehicel_master.vhmake_refno = minimasters.sno INNER JOIN axils_master ON vehicel_master.axils_refno = axils_master.sno INNER JOIN minimasters minimasters_2 ON vehicel_master.category = minimasters_2.sno INNER JOIN vehicle_types ON vehicel_master.vhtype_refno = vehicle_types.sno WHERE (vehicel_master.branch_id = @branch_id) AND (vehicel_master.operatedby = @operatedby)");
            cmd = new MySqlCommand("SELECT vehicel_master.vm_sno,vehicel_master.imagename, vehicel_master.registration_no, vehicel_master.door_no, vehicel_master.status, vehicel_master.branch_id, vehicel_master.operatedby, vehicel_master.Capacity, minimasters.mm_name AS veh_type, minimasters_1.mm_name AS veh_make, vehicel_master.fuel_capacity, vehicel_master.axils_refno, axil_master.axilmaster_name FROM vehicel_master INNER JOIN minimasters ON vehicel_master.vhtype_refno = minimasters.sno INNER JOIN minimasters minimasters_1 ON vehicel_master.vhmake_refno = minimasters_1.sno LEFT OUTER JOIN axil_master ON vehicel_master.axils_refno = axil_master.sno WHERE (vehicel_master.branch_id = @branch_id) AND (vehicel_master.vm_sno NOT IN (SELECT vehicleno FROM tripdata WHERE  (status = 'A'))) ");
            cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            List<get_veh_master_data> vendor = new List<get_veh_master_data>();
            foreach (DataRow dr in dt.Rows)
            {
                get_veh_master_data data = new get_veh_master_data();
                data.vm_sno = dr["vm_sno"].ToString();                
                data.registration_no = dr["registration_no"].ToString();
                data.VehType = dr["veh_type"].ToString();
                data.Capacity = dr["Capacity"].ToString();
                data.v_ty_fuel_capacity = dr["fuel_capacity"].ToString();              


                data.VehMake = dr["veh_make"].ToString();
                data.door_no = dr["door_no"].ToString();            
                data.status = dr["status"].ToString();
                data.axils_refno = dr["axils_refno"].ToString();
                data.axilmaster_name = dr["axilmaster_name"].ToString();
                vendor.Add(data);
            }
            string response = GetJson(vendor);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }
    private void keep_sessionalive(HttpContext context)
    {
        string response = GetJson("I Am Alive");
        context.Response.Write(response);
    }

    private void get_axil_odometer_using_position(HttpContext context)
    {
        try
        {
            string tyreposition = context.Request["tyreposition"];
            string vehiclenumber = context.Request["vehiclenumber"];
            string axlenumber = context.Request["axlenumber"];
            cmd = new MySqlCommand("SELECT axils_tyres_names.axilnms_refno, vehicle_master_sub.Odometer, vehicle_master_sub.axles_tyres_names_sno, vehicle_master_sub.vehicle_mstr_sno, vehicle_master_sub.tyre_sno FROM vehicle_master_sub INNER JOIN axils_tyres_names ON vehicle_master_sub.axles_tyres_names_sno = axils_tyres_names.sno WHERE (vehicle_master_sub.vehicle_mstr_sno = @vehicle_mstr_sno) AND (vehicle_master_sub.axles_tyres_names_sno = @axles_tyres_names_sno) AND (vehicle_master_sub.tyre_sno IS NULL)");
            cmd.Parameters.Add("@axles_tyres_names_sno", tyreposition);
            cmd.Parameters.Add("@vehicle_mstr_sno", vehiclenumber);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            List<get_tyre_using_position_cls> vendor = new List<get_tyre_using_position_cls>();
            foreach (DataRow dr in dt.Rows)
            {
                get_tyre_using_position_cls data = new get_tyre_using_position_cls();
                data.Odometer = dr["Odometer"].ToString();
                vendor.Add(data);
            }
            string response = GetJson(vendor);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }

    private void get_filled_data_tyres_vehmstr(HttpContext context)
    {
        try
        {
            string sno = context.Request["sno"];
            string vehmstr_sno = context.Request["vehmstr_sno"];
            //cmd = new MySqlCommand("SELECT axlmstr_sub_axilnames.axlename, axlmstr_sub_axilnames.nooftyresperaxle, axlmstr_sub_axilnames.ranking AS rank, axils_tyres_names.sno AS tyre_position_sno, axils_tyres_names.axilnms_refno,axils_tyres_names.axleside, axils_tyres_names.tyrename, axils_tyres_names.tyre_size_sno, axlmstr_sub_axilnames.axil_mstr_refno, minimasters.mm_name AS tyresize, vehicle_master_sub.tyre_sno,new_tyres_sub.tyre_sno AS Tyre_Name,new_tyres_sub.SVDSNo, new_tyres_sub.Fitting_Type FROM axlmstr_sub_axilnames INNER JOIN axils_tyres_names ON axlmstr_sub_axilnames.sno = axils_tyres_names.axilnms_refno INNER JOIN minimasters ON axils_tyres_names.tyre_size_sno = minimasters.sno INNER JOIN vehicle_master_sub ON axils_tyres_names.sno = vehicle_master_sub.axles_tyres_names_sno LEFT OUTER JOIN new_tyres_sub ON vehicle_master_sub.tyre_sno = new_tyres_sub.sno WHERE (axlmstr_sub_axilnames.axil_mstr_refno = @sno) AND (vehicle_master_sub.vehicle_mstr_sno = @vehicle_mstr_sno) ORDER BY rank");
            cmd = new MySqlCommand("SELECT new_tyres_sub.current_KMS,axlmstr_sub_axilnames.axlename, axlmstr_sub_axilnames.nooftyresperaxle, axlmstr_sub_axilnames.ranking AS rank, axils_tyres_names.sno AS tyre_position_sno, axils_tyres_names.axilnms_refno,axils_tyres_names.axleside, axils_tyres_names.tyrename, axils_tyres_names.tyre_size_sno, axlmstr_sub_axilnames.axil_mstr_refno, minimasters.mm_name AS tyresize, vehicle_master_sub.tyre_sno,new_tyres_sub.tyre_sno AS Tyre_Name, new_tyres_sub.svdsno,new_tyres_sub.grove, new_tyres_sub.Fitting_Type, minimasters_1.mm_name AS Brand FROM minimasters minimasters_1 INNER JOIN new_tyres_sub ON minimasters_1.sno = new_tyres_sub.brand RIGHT OUTER JOIN axlmstr_sub_axilnames INNER JOIN axils_tyres_names ON axlmstr_sub_axilnames.sno = axils_tyres_names.axilnms_refno INNER JOIN minimasters ON axils_tyres_names.tyre_size_sno = minimasters.sno INNER JOIN vehicle_master_sub ON axils_tyres_names.sno = vehicle_master_sub.axles_tyres_names_sno ON new_tyres_sub.sno = vehicle_master_sub.tyre_sno WHERE (axlmstr_sub_axilnames.axil_mstr_refno = @sno) AND (vehicle_master_sub.vehicle_mstr_sno = @vehicle_mstr_sno) ORDER BY rank");
            cmd.Parameters.Add("@sno", sno);
            cmd.Parameters.Add("@vehicle_mstr_sno", vehmstr_sno);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            DataView view = new DataView(dt);
            DataTable Axil_Tbl = view.ToTable(true, "axilnms_refno", "axlename", "nooftyresperaxle");
            List<axil_data> veh_typ_data = new List<axil_data>();
            foreach (DataRow dr in Axil_Tbl.Rows)
            {
                axil_data data = new axil_data();
                data.AxileName = dr["axlename"].ToString();
                data.nooftyresperaxle = dr["nooftyresperaxle"].ToString();
                data.veh_typ_axel_sno = dr["axilnms_refno"].ToString();
                DataRow[] tyredata = dt.Select("axilnms_refno='" + dr["axilnms_refno"].ToString() + "'");

                foreach (DataRow tyre in tyredata)
                {
                    tyre_data tydata = new tyre_data();
                    tydata.side = tyre["axleside"].ToString();
                    tydata.tyre_name = tyre["tyrename"].ToString();
                    tydata.tyre_size = tyre["tyre_size_sno"].ToString();
                    tydata.tyresize = tyre["tyresize"].ToString();
                    tydata.SVDSNo = tyre["SVDSNo"].ToString();
                    tydata.grove = tyre["grove"].ToString();
                    tydata.Brand = tyre["Brand"].ToString();
                    tydata.currentkms = tyre["current_KMS"].ToString();
                    tydata.tyre_position_sno = tyre["tyre_position_sno"].ToString();
                    if (tyre["Fitting_Type"].ToString() == "" || tyre["Fitting_Type"].ToString() == null)
                    {
                        tydata.Tyre_Name = "Tyre Removed";
                    }
                    else
                    {
                        tydata.Tyre_Name = tyre["Tyre_Name"].ToString();
                    }
                    data.tyredata.Add(tydata);
                }
                veh_typ_data.Add(data);
            }
            string response = GetJson(veh_typ_data);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }

    public class get_only_axils_names
    {
        public string axilmaster_name { get; set; }
        public string no_of_axles { get; set; }
        public string sno { get; set; }
    }
    private void get_all_axil_names(HttpContext context)
    {
        try
        {
            cmd = new MySqlCommand("SELECT sno, axilmaster_name, vhmake, vhtype, no_of_axles, v_ty_status, branch_id, operated_by FROM axil_master  ");
            DataTable dtVehicleDetails = vdm.SelectQuery(cmd).Tables[0];
            List<get_only_axils_names> JobCardlist = new List<get_only_axils_names>();
            foreach (DataRow dr in dtVehicleDetails.Rows)
            {
                get_only_axils_names GetCards = new get_only_axils_names();
                GetCards.sno = dr["sno"].ToString();
                GetCards.axilmaster_name = dr["axilmaster_name"].ToString();
                GetCards.no_of_axles = dr["no_of_axles"].ToString();
                JobCardlist.Add(GetCards);
            }
            string response = GetJson(JobCardlist);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }
    private void save_edit_TyresInspection(HttpContext context)
    {
        try
        {
            double refno = 0.0;
            vdm = new VehicleDBMgr();
            string vehicleno = context.Request["vehicleno"];
            DateTime inspectiondate = VehicleDBMgr.GetTime(vdm.conn);
            string inspectedby = context.Request["inspectedby"];
            string kmreading = context.Request["kmreading"];
            string achrmeter = context.Request["achrmeter"];
            string bodycondition = context.Request["bodycondition"];
            string recordscheckup = context.Request["recordscheckup"];
            string remarks = context.Request["remarks"];
            string checkedvehicleTools = context.Request["checkedvehicleTools"];
            string HandOver_driver_name = context.Request["HandOver_driver_name"];
            string Receiver_driver_name = context.Request["Receiver_driver_name"];
            List<Tyres_items> Product_list = (List<Tyres_items>)context.Session["load_Next_Tyres"];
            cmd = new MySqlCommand("insert into tyre_inspection (vehicleno, inspectedby, Inspecteddate, userid, branch_id,kmreading,achrmeter,bodycondition,recordscheck,remarks,handover_driverid,receiver_driverid) values ( @vehicleno, @inspectedby, @Inspecteddate, @userid, @branch_id,@kmreading,@achrmeter,@bodycondition,@recordscheckup,@remarks,@handover_driverid,@receiver_driverid)");
            cmd.Parameters.Add("@vehicleno", vehicleno);
            cmd.Parameters.Add("@inspectedby", inspectedby);
            cmd.Parameters.Add("@Inspecteddate", inspectiondate);
            cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
            cmd.Parameters.Add("@userid", context.Session["Employ_Sno"]);
            cmd.Parameters.Add("@kmreading", kmreading);
            cmd.Parameters.Add("@achrmeter", achrmeter);
            cmd.Parameters.Add("@bodycondition", bodycondition);
            cmd.Parameters.Add("@recordscheckup", recordscheckup);
            cmd.Parameters.Add("@remarks", remarks);
            cmd.Parameters.Add("@handover_driverid", HandOver_driver_name);
            cmd.Parameters.Add("@receiver_driverid", Receiver_driver_name);
            refno = vdm.insertScalar(cmd);
            foreach (Tyres_items art in Product_list)
            {
                cmd = new MySqlCommand("insert into tyre_inspection_sub (tyre_insp_ref, tyre_sno, kms,grove) values (@tyre_insp_ref, @tyre_sno, @kms,@grove)");
                cmd.Parameters.Add("@tyre_insp_ref", refno);
                cmd.Parameters.Add("@tyre_sno", art.tyresno);
                double kms = 0.00;
                double.TryParse(art.KMS, out kms);
                cmd.Parameters.Add("@kms", kms);
                cmd.Parameters.Add("@remarks", art.Remarks);
                int grove = 0;
                int.TryParse(art.grove, out grove);
                cmd.Parameters.Add("@grove", grove);
                vdm.insert(cmd);
            }
            foreach (string str in checkedvehicleTools.Split(','))
            {
                cmd = new MySqlCommand("insert into handover_subtable (toolname, handoversno) values (@toolname, @handoversno)");
                cmd.Parameters.Add("@toolname", str);
                cmd.Parameters.Add("@handoversno", refno);
                if (str != "")
                {
                    vdm.insert(cmd);
                }
            }
            string response = GetJson("handOver Successfully Saved");
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }
    public class get_tyre_vehicles
    {
        public string vm_sno { get; set; }
        public string vhtype_refno { get; set; }
        public string axils_refno { get; set; }
        public string registration_no { get; set; }
    }
    private void get_only_vehicles_data(HttpContext context)
    {
        try
        {
            cmd = new MySqlCommand("SELECT vm_sno, vhtype_refno, axils_refno, registration_no, branch_id, status FROM vehicel_master WHERE (branch_id = @BranchID) AND (status = '1')");
            cmd.Parameters.Add("@BranchID", context.Session["Branch_ID"]);
            DataTable dtVehicleDetails = vdm.SelectQuery(cmd).Tables[0];
            List<get_tyre_vehicles> JobCardlist = new List<get_tyre_vehicles>();
            foreach (DataRow dr in dtVehicleDetails.Rows)
            {
                get_tyre_vehicles GetCards = new get_tyre_vehicles();
                GetCards.vm_sno = dr["vm_sno"].ToString();
                GetCards.vhtype_refno = dr["vhtype_refno"].ToString();
                GetCards.axils_refno = dr["axils_refno"].ToString();
                GetCards.registration_no = dr["registration_no"].ToString();
                JobCardlist.Add(GetCards);
            }
            string response = GetJson(JobCardlist);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }

    private void accept_reject_tyre(HttpContext context)
    {
        try
        {
            string sno = context.Request["sno"];
            string status = context.Request["status"];
            cmd = new MySqlCommand("update tyre_transfer_sub set status=@status where sno=@sno");
            cmd.Parameters.Add("@sno", sno);
            cmd.Parameters.Add("@status", status);
            vdm.Update(cmd);
            if (status == "A")
            {
                string response = GetJson("Tyre Accepted");
                context.Response.Write(response);
            }
            else
            {
                string response = GetJson("Tyre Rejected");
                context.Response.Write(response);
            }
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }


    public class transfer_get
    {
        public string sno { get; set; }
        public string transfer_sno { get; set; }
        public string tyre_sno { get; set; }
        public string brand { get; set; }
        public string tyretype { get; set; }
        public string tubetyre { get; set; }
        public string size { get; set; }
        public string remarks { get; set; }
        public string BranchID { get; set; }
        public string branch_id { get; set; }
        public string branchname { get; set; }
    }

    private void get_transfered_tyres_thisbrn(HttpContext context)
    {
        try
        {

            cmd = new MySqlCommand("SELECT tyre_transfer_sub.sno, tyre_transfer_sub.transfer_sno, tyre_transfer_sub.tyre_sno, tyre_transfer_sub.remarks, tyre_transfer_sub.status, tyre_transfer.BranchID, tyre_transfer.branch_id,minimasters.mm_name AS brand, minimasters_1.mm_name AS tyretype, minimasters_2.mm_name AS size, branch_info.branchname, tyre_transfer_sub.tubetyre FROM tyre_transfer_sub INNER JOIN tyre_transfer ON tyre_transfer_sub.transfer_sno = tyre_transfer.sno INNER JOIN minimasters ON tyre_transfer_sub.brand = minimasters.sno INNER JOIN minimasters minimasters_1 ON tyre_transfer_sub.tyretype = minimasters_1.sno INNER JOIN minimasters minimasters_2 ON tyre_transfer_sub.size = minimasters_2.sno INNER JOIN branch_info ON tyre_transfer.branch_id = branch_info.brnch_sno WHERE (tyre_transfer_sub.status = 'T') AND (tyre_transfer.BranchID = @BranchID)");
            cmd.Parameters.Add("@BranchID", context.Session["Branch_ID"]);
            DataTable dtVehicleDetails = vdm.SelectQuery(cmd).Tables[0];
            List<transfer_get> JobCardlist = new List<transfer_get>();
            foreach (DataRow dr in dtVehicleDetails.Rows)
            {
                transfer_get GetCards = new transfer_get();
                GetCards.sno = dr["sno"].ToString();
                GetCards.transfer_sno = dr["transfer_sno"].ToString();
                GetCards.tyre_sno = dr["tyre_sno"].ToString();
                GetCards.brand = dr["brand"].ToString();
                GetCards.tyretype = dr["tyretype"].ToString();
                GetCards.tubetyre = dr["tubetyre"].ToString();
                GetCards.size = dr["size"].ToString();
                GetCards.remarks = dr["remarks"].ToString();
                GetCards.BranchID = dr["BranchID"].ToString();
                GetCards.branch_id = dr["branch_id"].ToString();
                GetCards.branchname = dr["branchname"].ToString();
                JobCardlist.Add(GetCards);
            }
            string response = GetJson(JobCardlist);
            context.Response.Write(response);

        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }

    private void save_edit_TyresTransfer(HttpContext context)
    {
        try
        {
            double refno = 0.0;
            string branchname = context.Request["branchname"];
            string date = context.Request["date"];
            string remarks = context.Request["remarks"];
            string sendby = context.Request["sendby"];
            List<Tyres_items> Product_list = (List<Tyres_items>)context.Session["load_Next_Tyres"];
            cmd = new MySqlCommand("insert into tyre_transfer ( BranchID, entrydate, remarks, sentby, userid, branch_id) values (@BranchID, @entrydate, @remarks, @sentby, @userid, @branch_id)");
            cmd.Parameters.Add("@BranchID", branchname);
            cmd.Parameters.Add("@entrydate", date);
            cmd.Parameters.Add("@remarks", remarks);
            cmd.Parameters.Add("@sentby", sendby);
            cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
            cmd.Parameters.Add("@userid", context.Session["Employ_Sno"]);
            refno = vdm.insertScalar(cmd);
            foreach (Tyres_items art in Product_list)
            {
                cmd = new MySqlCommand("insert into tyre_transfer_sub (transfer_sno, tyre_sno, brand, tyretype, tubetyre, size, remarks, status) values (@transfer_sno, @tyre_sno, @brand, @tyretype, @tubetyre, @size, @remarks, @status)");
                cmd.Parameters.Add("@transfer_sno", refno);
                cmd.Parameters.Add("@tyre_sno", art.tyresno);
                cmd.Parameters.Add("@brand", art.Brand);
                cmd.Parameters.Add("@tyretype", art.Type_of_Tyre);
                cmd.Parameters.Add("@tubetyre", art.Tube_Type);
                cmd.Parameters.Add("@size", art.Size);
                cmd.Parameters.Add("@remarks", art.Remarks);
                cmd.Parameters.Add("@status", 'T');
                vdm.insert(cmd);
            }
            string response = GetJson("Tyres Transfered Successfully");
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }

    private void save_edit_TyresRethread(HttpContext context)
    {
        try
        {
            double refno = 0.0;
            string senddate = context.Request["senddate"];
            string servicingat = context.Request["servicingat"];
            string excepteddate = context.Request["excepteddate"];
            string remarks = context.Request["remarks"];
            string sendby = context.Request["sendby"];
            string nooftyres = context.Request["nooftyres"];
            List<Tyres_items> Product_list = (List<Tyres_items>)context.Session["load_Next_Tyres"];
            cmd = new MySqlCommand("insert into tyre_rethread ( sentdate, sentby, servicingat, expecteddate, remarks, userid, branch_id,no_of_tyres) values (@sentdate, @sentby, @servicingat, @expecteddate, @remarks, @userid, @branch_id,@no_of_tyres)");
            cmd.Parameters.Add("@sentdate", senddate);
            cmd.Parameters.Add("@sentby", sendby);
            cmd.Parameters.Add("@servicingat", servicingat);
            cmd.Parameters.Add("@expecteddate", excepteddate);
            cmd.Parameters.Add("@remarks", remarks);
            cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
            cmd.Parameters.Add("@userid", context.Session["Employ_Sno"]);
            cmd.Parameters.Add("@no_of_tyres", nooftyres);
            refno = vdm.insertScalar(cmd);
            foreach (Tyres_items art in Product_list)
            {
                cmd = new MySqlCommand("insert into tyre_rethread_sub (rethread_sno, tyre_sno, Make, kmsrun) values (@rethread_sno, @tyre_sno, @Make, @kmsrun)");

                cmd.Parameters.Add("@rethread_sno", refno);
                cmd.Parameters.Add("@tyre_sno", art.tyresno);
                cmd.Parameters.Add("@Make", art.Brand);
                cmd.Parameters.Add("@kmsrun", art.KMS);
                vdm.insert(cmd);
            }
            string response = GetJson("Tyres Rethread Saved Successfully");
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }

    public class get_tyre
    {
        public string tyre_sno { get; set; }
        public string size { get; set; }
        public string type_of_tyre { get; set; }
        public string tyre_tube { get; set; }
        public string cost { get; set; }
        public string brand { get; set; }
        public string veh_make { get; set; }
        public string veh_type { get; set; }
        public string newtyre_refno { get; set; }
        public string current_KMS { get; set; }
        public string sno { get; set; }
    }

    private void get_tyres_new(HttpContext context)
    {
        try
        {
            cmd = new MySqlCommand("SELECT new_tyres_sub.tyre_sno, new_tyres_sub.size, new_tyres_sub.type_of_tyre, new_tyres_sub.tyre_tube, new_tyres_sub.cost, new_tyres_sub.brand, new_tyres_sub.newtyre_refno, new_tyres.branch_id, new_tyres_sub.current_KMS, new_tyres_sub.sno FROM new_tyres_sub INNER JOIN new_tyres ON new_tyres_sub.newtyre_refno = new_tyres.sno WHERE (new_tyres.branch_id = @branch_id) AND (NOT (new_tyres_sub.Fitting_Type = 'F'))");
            cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
            DataTable dtVehicleDetails = vdm.SelectQuery(cmd).Tables[0];
            List<get_tyre> JobCardlist = new List<get_tyre>();
            foreach (DataRow dr in dtVehicleDetails.Rows)
            {
                get_tyre GetCards = new get_tyre();
                GetCards.tyre_sno = dr["tyre_sno"].ToString();
                GetCards.size = dr["size"].ToString();
                GetCards.type_of_tyre = dr["type_of_tyre"].ToString();
                GetCards.tyre_tube = dr["tyre_tube"].ToString();
                GetCards.cost = dr["cost"].ToString();
                GetCards.brand = dr["brand"].ToString();
                GetCards.newtyre_refno = dr["newtyre_refno"].ToString();
                GetCards.current_KMS = dr["current_KMS"].ToString();
                GetCards.sno = dr["sno"].ToString();
                JobCardlist.Add(GetCards);
            }
            string response = GetJson(JobCardlist);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }

    private void save_edit_Tyres(HttpContext context)
    {
        try
        {
            double refno = 0.0;
            string purchasedate = context.Request["purchasedate"];
            string invoiceno = context.Request["invoiceno"];
            string payment = context.Request["payment"];
            string vendor = context.Request["vendor"];
            string totalcost = context.Request["totalcost"];
            string tax = context.Request["tax"];
            string discount = context.Request["discount"];
            string grandtotal = context.Request["grandtotal"];
            string payablecost = context.Request["payablecost"];
            string noof_tyres = context.Request["noof_tyres"];
            string btnval = context.Request["btnval"];
            string othrexpences = context.Request["othrexpences"];
            string main_sno = context.Request["main_sno"];
            if (purchasedate == "")
            {
                purchasedate = null;
            }
            List<Tyres_items> Product_list = (List<Tyres_items>)context.Session["load_Next_Tyres"];
            if (btnval == "Save")
            {
                cmd = new MySqlCommand("insert into new_tyres ( purchase_date, entry_date, vendor_refno, invoiceno, payment_type, total_cost, tax, other_expences, discount, grand_total, payable_cost, branch_id, user_id) values ( @purchase_date, @entry_date, @vendor_refno, @invoiceno, @payment_type, @total_cost, @tax, @other_expences, @discount, @grand_total, @payable_cost, @branch_id, @user_id)");
                cmd.Parameters.Add("@purchase_date", purchasedate);
                cmd.Parameters.Add("@entry_date", DateTime.Now);
                cmd.Parameters.Add("@vendor_refno", vendor);
                cmd.Parameters.Add("@invoiceno", invoiceno);
                cmd.Parameters.Add("@payment_type", payment);
                cmd.Parameters.Add("@total_cost", totalcost);
                cmd.Parameters.Add("@tax", tax);
                cmd.Parameters.Add("@other_expences", othrexpences);
                cmd.Parameters.Add("@discount", discount);
                cmd.Parameters.Add("@grand_total", grandtotal);
                cmd.Parameters.Add("@payable_cost", payablecost);
                cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
                cmd.Parameters.Add("@user_id", context.Session["Employ_Sno"]);
                refno = vdm.insertScalar(cmd);
                string tysno = "";
                foreach (Tyres_items art in Product_list)
                {
                    if (art.tyresno != "" && art.tyresno != null)
                    {
                        cmd = new MySqlCommand("insert into new_tyres_sub (newtyre_refno, tyre_sno, size, type_of_tyre, tyre_tube, cost, brand,frontrback,svdsno, grove,min_grove, max_grove) values (@newtyre_refno, @tyre_sno, @size, @type_of_tyre, @tyre_tube, @cost, @brand,@frontrback,@svdsno, @grove,@min_grove, @max_grove)");
                        cmd.Parameters.Add("@newtyre_refno", refno);
                        cmd.Parameters.Add("@tyre_sno", art.tyresno);
                        tysno = art.tyresno;
                        cmd.Parameters.Add("@size", art.Size);
                        cmd.Parameters.Add("@type_of_tyre", art.Type_of_Tyre);
                        cmd.Parameters.Add("@tyre_tube", art.Tube_Type);
                        cmd.Parameters.Add("@cost", art.Cost);
                        cmd.Parameters.Add("@brand", art.Brand);
                        cmd.Parameters.Add("@frontrback", art.FrntrRear);
                        cmd.Parameters.Add("@svdsno", art.SVDSNO);
                        double min_grove = 0.00;
                        double.TryParse(art.min_grove, out min_grove);
                        double max_grove = 0.00;
                        double.TryParse(art.max_grove, out max_grove);
                        double grove = 0.00;
                        double.TryParse(art.grove, out grove);
                        cmd.Parameters.Add("@min_grove", min_grove);
                        cmd.Parameters.Add("@max_grove", max_grove);
                        cmd.Parameters.Add("@grove", grove);
                        vdm.insert(cmd);
                    }
                }
                string response = GetJson("Tyres Saved Successfully");
                context.Response.Write(response);
            }
            else
            {
                cmd = new MySqlCommand("update new_tyres set purchase_date=@purchase_date, entry_date=@entry_date, vendor_refno=@vendor_refno, invoiceno=@invoiceno, payment_type=@payment_type, total_cost=@total_cost, tax=@tax, other_expences=@other_expences, discount=@discount, grand_total=@grand_total, payable_cost=@payable_cost where sno=@sno");
                cmd.Parameters.Add("@purchase_date", purchasedate);
                cmd.Parameters.Add("@entry_date", DateTime.Now);
                cmd.Parameters.Add("@vendor_refno", vendor);
                cmd.Parameters.Add("@invoiceno", invoiceno);
                cmd.Parameters.Add("@payment_type", payment);
                cmd.Parameters.Add("@total_cost", totalcost);
                cmd.Parameters.Add("@tax", tax);
                cmd.Parameters.Add("@other_expences", othrexpences);
                cmd.Parameters.Add("@discount", discount);
                cmd.Parameters.Add("@grand_total", grandtotal);
                cmd.Parameters.Add("@payable_cost", payablecost);
                cmd.Parameters.Add("@sno", main_sno);
                vdm.Update(cmd);
                foreach (Tyres_items art in Product_list)
                {

                    cmd = new MySqlCommand("update new_tyres_sub set svdsno=@svdsno,min_grove=@min_grove,max_grove=@max_grove,grove=@grove where sno=@sno");
                    cmd.Parameters.Add("@svdsno", art.SVDSNO);
                    double min_grove = 0.00;
                    double.TryParse(art.min_grove, out min_grove);
                    double max_grove = 0.00;
                    double.TryParse(art.max_grove, out max_grove);
                    double grove = 0.00;
                    double.TryParse(art.grove, out grove);
                    cmd.Parameters.Add("@min_grove", min_grove);
                    cmd.Parameters.Add("@max_grove", max_grove);
                    cmd.Parameters.Add("@grove", grove);
                    cmd.Parameters.Add("@sno", art.sno);
                    vdm.Update(cmd);
                }
                string response = GetJson("Tyres Edited Successfully");
                context.Response.Write(response);
            }
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }

    private void Tyres_save_RowData(Tyres_rowwise obj, HttpContext context)
    {
        try
        {
            Tyres_items o = obj.row_detail;
            List<Tyres_items> Product_list = (List<Tyres_items>)context.Session["load_Next_Tyres"];
            Product_list.Add(o);
            context.Session["load_Next_Tyres"] = Product_list;
            string msg = obj.end;
            string response = GetJson(msg);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }
    public class Tyres_rowwise
    {
        public string end { get; set; }
        public Tyres_items row_detail { set; get; }
    }
    public class Tyres_items
    {

        public string tyresno { get; set; }
        public string Brand { get; set; }
        public string Type_of_Tyre { get; set; }
        public string Tube_Type { get; set; }
        public string Size { get; set; }
        public string Cost { get; set; }
        public string Veh_Make { get; set; }
        public string KMS { get; set; }
        public string Remarks { get; set; }
        public string Veh_Type { get; set; }
        public string FrntrRear { get; set; }
        public string SVDSNO { get; set; }
        public string grove { get; set; }
        public string max_grove { get; set; }
        public string min_grove { get; set; }
        public string sno { get; set; }
        public string currentkms { get; set; }
        public string flag { get; set; }
    }


    private DateTime GetLowDate(DateTime dt)
    {
        double Hour, Min, Sec;
        DateTime DT = DateTime.Now;
        DT = dt;
        Hour = -dt.Hour;
        Min = -dt.Minute;
        Sec = -dt.Second;
        DT = DT.AddHours(Hour);
        DT = DT.AddMinutes(Min);
        DT = DT.AddSeconds(Sec);
        return DT;
    }

    private DateTime GetHighDate(DateTime dt)
    {
        double Hour, Min, Sec;
        DateTime DT = DateTime.Now;
        Hour = 23 - dt.Hour;
        Min = 59 - dt.Minute;
        Sec = 59 - dt.Second;
        DT = dt;
        DT = DT.AddHours(Hour);
        DT = DT.AddMinutes(Min);
        DT = DT.AddSeconds(Sec);
        return DT;
    }
    private void btnsaveInwardstock(HttpContext context)
    {
        try
        {
            vdm = new VehicleDBMgr();
            string Userid = context.Session["Branch_ID"].ToString();
            string diesel = context.Request["Qty"];
            string txtOppQty = context.Request["txtOppQty"];
            string litercost = context.Request["litercost"];
            string supplierId = context.Request["supplierid"];
            double Fuel = 0;
            double.TryParse(diesel, out Fuel);
            double OppQty = 0;
            double.TryParse(txtOppQty, out OppQty);
            double totalqty = Fuel + OppQty;
            DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
            cmd = new MySqlCommand("update fuel_monitor set  fuel=@fuel where  UserID= @UserName ");
            cmd.Parameters.Add("@UserName", Userid);
            cmd.Parameters.Add("@fuel", totalqty);
            if (vdm.Update(cmd) == 0)
            {
                cmd = new MySqlCommand("insert into fuel_monitor (fuel,UserID,costperltr) values(@fuel2,@UserID,@costperltr) ");
                cmd.Parameters.Add("@UserID", Userid);
                cmd.Parameters.Add("@fuel2", Fuel);
                cmd.Parameters.Add("@costperltr", litercost);
                
                vdm.insert(cmd);
            }
            cmd = new MySqlCommand("insert into fuel_transaction (transtype,fuel,doe,UserID,costperltr,VendorId) values(@transtype,@fuel,@doe,@UserID,@costperltr,@supplierId)");
            cmd.Parameters.Add("@UserID", Userid);
            cmd.Parameters.Add("@fuel", Fuel);
            cmd.Parameters.Add("@transtype", "1");
            cmd.Parameters.Add("@doe", ServerDateCurrentdate);
            cmd.Parameters.Add("@costperltr", litercost);
            cmd.Parameters.Add("@supplierId", supplierId);
            vdm.insert(cmd);
            string msg = "Updated successfully";
            string response = GetJson(msg);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }
    private void btnstockclosing(HttpContext context)
    {
        try
        {
            vdm = new VehicleDBMgr();
            string Userid = context.Session["Branch_ID"].ToString();
            string diesel = context.Request["Qty"];
            string date = context.Request["date"];
            DateTime ServerDateCurrentdate = Convert.ToDateTime(date);
            double Fuel = 0;
            double.TryParse(diesel, out Fuel);

            cmd = new MySqlCommand("update fuel_transaction SET fuel=@fuel where doe between @d1 and @d2 and transtype=@transtype");
            cmd.Parameters.Add("@fuel", Fuel);
            cmd.Parameters.Add("@d1", GetLowDate(ServerDateCurrentdate));
            cmd.Parameters.Add("@d2", GetHighDate(ServerDateCurrentdate));
            cmd.Parameters.Add("@transtype", "2");
            if (vdm.Update(cmd) == 0)
            {
                cmd = new MySqlCommand("insert into fuel_transaction (transtype,fuel,doe,UserID) values(@transtype,@fuel,@doe,@UserID)");
                cmd.Parameters.Add("@UserID", Userid);
                cmd.Parameters.Add("@fuel", Fuel);
                cmd.Parameters.Add("@transtype", "2");
                cmd.Parameters.Add("@doe", ServerDateCurrentdate);
                vdm.insert(cmd);
            }
            string msg = "Updated successfully";
            string response = GetJson(msg);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }

    public class getstock
    {
        public string fuel { get; set; }
        public string costperltr { get; set; }
    }
    private void GetStockQty(HttpContext context)
    {
        try
        {
            vdm = new VehicleDBMgr();
            string Userid = context.Session["Branch_ID"].ToString();
            cmd = new MySqlCommand("Select  sno, fuel, userid, operatedby, costperltr from fuel_monitor where  UserID= @UserName ");
            cmd.Parameters.Add("@UserName", Userid);
            DataTable dtFuel = vdm.SelectQuery(cmd).Tables[0];
            string FuelValue = "0";
            string costperltr = "0";
            getstock stk = new getstock();
            if (dtFuel.Rows.Count > 0)
            {
                FuelValue = dtFuel.Rows[0]["fuel"].ToString();
                costperltr = dtFuel.Rows[0]["costperltr"].ToString();
            }
            stk.fuel = FuelValue;
            stk.costperltr = costperltr;
            string response = GetJson(stk);
            context.Response.Write(response);
        }
        catch
        {

        }
    }
    private void UpdateJobcardbtnclick(HttpContext context)
    {
        vdm = new VehicleDBMgr();
        try
        {
            string Username = context.Session["Branch_ID"].ToString();
            string tripsno = context.Request["TripSheetNo"];
            string checkedjobcards = context.Request["JobCardName"];
            string[] jobcards = checkedjobcards.Split(',');
            string UserID = context.Session["Branch_ID"].ToString();
            foreach (string jbcrd in jobcards)
            {
                cmd = new MySqlCommand("update jobcards set status=@status where userid=@userid and tripsheetsno=@tripsheetsno and jobcardname=@jobcardname");
                cmd.Parameters.Add("@tripsheetsno", tripsno);
                cmd.Parameters.Add("@jobcardname", jbcrd);
                cmd.Parameters.Add("@status", "C");
                cmd.Parameters.Add("@userid", Username);
                vdm.insert(cmd);
                string msg = "Jobcards successfully Updated";
                string response = GetJson(msg);
                context.Response.Write(response);
            }
        }
        catch (Exception ex)
        {
            string response = GetJson("Error");
            context.Response.Write(response);
        }
    }
    private void btnTripSheetPrintClick(HttpContext context)
    {
        vdm = new VehicleDBMgr();
        try
        {
            context.Session["TripSheetNo"] = context.Request["txtTripDataSno"];
            string msg = "Success";
            string response = GetJson(msg);
            context.Response.Write(response);
        }
        catch
        {
        }
    }
    private void GetBtnViewJobcardclick(HttpContext context)
    {
        try
        {
            vdm = new VehicleDBMgr();
            string txtTripDataSno = context.Request["txtTripDataSno"];
            string UserID = context.Session["Branch_ID"].ToString();
            cmd = new MySqlCommand("SELECT sno, tripsheetsno, jobcarddate, jobcardname,jobcarddetails, status FROM jobcards WHERE (tripsheetsno = @tripsheetsno)");
            cmd.Parameters.Add("@tripsheetsno", txtTripDataSno);
            DataTable dtVehicleDetails = vdm.SelectQuery(cmd).Tables[0];
            List<JobCardDetails> JobCardlist = new List<JobCardDetails>();
            foreach (DataRow dr in dtVehicleDetails.Rows)
            {
                JobCardDetails GetCards = new JobCardDetails();
                GetCards.jobcardname = dr["jobcardname"].ToString();
                GetCards.jobcarddetails = dr["jobcarddetails"].ToString();
                string status = dr["status"].ToString();
                string JobStatus = "";
                if (status == "A")
                {
                    JobStatus = "Pending";
                }
                if (status == "C")
                {
                    JobStatus = "Verified";
                }
                GetCards.status = JobStatus;
                JobCardlist.Add(GetCards);
            }
            string response = GetJson(JobCardlist);
            context.Response.Write(response);
        }
        catch
        {
        }
    }
    private void GetAssignTripSheets(HttpContext context)
    {
        try
        {
            vdm = new VehicleDBMgr();
            string UserID = context.Session["Branch_ID"].ToString();
            cmd = new MySqlCommand("SELECT employdata.employname, tripdata.routeid, tripdata.tripsheetno,tripdata.Tripsno, tripdata.tripdate, tripdata.sno, tripdata.loadtype, employdata.Phoneno, vehicel_master.registration_no AS vehicleno FROM tripdata INNER JOIN employdata ON tripdata.driverid = employdata.emp_sno INNER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno WHERE (tripdata.status <> 'C') AND (tripdata.userid = @UserID)");
            cmd.Parameters.Add("@Status", "A");
            cmd.Parameters.Add("@UserID", UserID);
            DataTable dtVehicleDetails = vdm.SelectQuery(cmd).Tables[0];
            List<vehiclesclass> vehicleslist = new List<vehiclesclass>();
            foreach (DataRow dr in dtVehicleDetails.Rows)
            {
                vehiclesclass vehicles = new vehiclesclass();
                vehicles.TripSno = dr["sno"].ToString();
                vehicles.TripSheetNo = dr["tripsheetno"].ToString();
                vehicles.TripDate = dr["Tripdate"].ToString();
                vehicles.vehicleno = dr["Vehicleno"].ToString();
                vehicles.Driver = dr["employname"].ToString();
                vehicles.Routename = dr["routeid"].ToString();
                vehicleslist.Add(vehicles);
            }
            string response = GetJson(vehicleslist);
            context.Response.Write(response);
        }
        catch
        {
        }
    }
    //private void Triplogs_edit_Click(TripLog_rowwise obj, HttpContext context)
    //{
    //    try
    //    {
    //        string TripRefsno = obj.tripid;
    //        string UserID = context.Session["Branch_ID"].ToString();
    //        string tripbtnval = obj.tripbtnval;


    //        if (tripbtnval == "Save Trip Logs")
    //        {
    //            foreach (TripLog_items art in obj.triplogs_array)
    //            {
    //                if (art.log_datetime != "" && art.log_datetime != null)
    //                {
    //                    double kms = 0.00;
    //                    double fuel = 0.00;
    //                    double log_capload = 0.00;
    //                    double log_capload_kgs = 0.00;
    //                    double log_capunload = 0.00;
    //                    double log_capunload_kgs = 0.00;
    //                    double log_charge = 0.00;
    //                    double log_tollgate = 0.00;
    //                    double log_expences = 0.00;
    //                    double log_acfuel = 0.00;
    //                    double.TryParse(art.log_kms, out kms);
    //                    double.TryParse(art.log_fuel, out fuel);
    //                    double.TryParse(art.log_capload, out log_capload);
    //                    double.TryParse(art.log_capload_kgs, out log_capload_kgs);
    //                    double.TryParse(art.log_capunload, out log_capunload);
    //                    double.TryParse(art.log_capunload_kgs, out log_capunload_kgs);
    //                    double.TryParse(art.log_charge, out log_charge);
    //                    double.TryParse(art.log_tollgate, out log_tollgate);
    //                    double.TryParse(art.log_expences, out log_expences);
    //                    double.TryParse(art.log_acfuel, out log_acfuel);
    //                    cmd = new MySqlCommand("insert into triplogs (doe, km, place,  expamount, fuel, tripsno, load_cap ,load_cap_kgs, unload_cap, unload_cap_kgs, charge,log_rank,tollgateamnt,fuel_type,odometer,remarks,acfuel) values (@doe, @km, @place, @expamount, @fuel, @tripsno, @load_cap, @load_cap_kgs,@unload_cap, @unload_cap_kgs,@charge,@log_rank,@tollgateamnt,@fuel_type,@odometer,@remarks,@acfuel)");
    //                    cmd.Parameters.Add("@doe", art.log_datetime);
    //                    cmd.Parameters.Add("@km", kms);
    //                    cmd.Parameters.Add("@place", art.log_fromlocation);
    //                    cmd.Parameters.Add("@expamount", log_expences);
    //                    cmd.Parameters.Add("@fuel", fuel);
    //                    cmd.Parameters.Add("@tripsno", TripRefsno);
    //                    cmd.Parameters.Add("@load_cap", log_capload);
    //                    cmd.Parameters.Add("@unload_cap", log_capunload);
    //                    cmd.Parameters.Add("@charge", log_charge);
    //                    cmd.Parameters.Add("@log_rank", art.rank);
    //                    cmd.Parameters.Add("@tollgateamnt", log_tollgate);
    //                    cmd.Parameters.Add("@fuel_type", art.log_fueltype);
    //                    cmd.Parameters.Add("@odometer", art.log_OdoMeter);
    //                    cmd.Parameters.Add("@remarks", art.log_remarks);
    //                    cmd.Parameters.Add("@load_cap_kgs", log_capload_kgs);
    //                    cmd.Parameters.Add("@unload_cap_kgs", log_capunload_kgs);
    //                    cmd.Parameters.Add("@acfuel", log_acfuel);
    //                    vdm.insert(cmd);
    //                }
    //            }
    //            string response = GetJson("Trip Logs Saved Successfully");
    //            context.Response.Write(response);
    //        }
    //        else
    //        {
    //            cmd = new MySqlCommand("DELETE FROM triplogs WHERE (tripsno = @tripsno)");
    //            cmd.Parameters.Add("@tripsno", TripRefsno);
    //            vdm.Delete(cmd);
    //            foreach (TripLog_items art in obj.triplogs_array)
    //            {
    //                if (art.log_datetime != "" && art.log_datetime != null)
    //                {
    //                    double kms = 0.00;
    //                    double fuel = 0.00;
    //                    double log_capload = 0.00;
    //                    double log_capload_kgs = 0.00;
    //                    double log_capunload = 0.00;
    //                    double log_capunload_kgs = 0.00;
    //                    double log_charge = 0.00;
    //                    double log_tollgate = 0.00;
    //                    double log_expences = 0.00;
    //                    double log_acfuel = 0.00;
    //                    double.TryParse(art.log_kms, out kms);
    //                    double.TryParse(art.log_fuel, out fuel);
    //                    double.TryParse(art.log_capload, out log_capload);
    //                    double.TryParse(art.log_capload_kgs, out log_capload_kgs);
    //                    double.TryParse(art.log_capunload, out log_capunload);
    //                    double.TryParse(art.log_capunload_kgs, out log_capunload_kgs);
    //                    double.TryParse(art.log_charge, out log_charge);
    //                    double.TryParse(art.log_tollgate, out log_tollgate);
    //                    double.TryParse(art.log_expences, out log_expences);
    //                    double.TryParse(art.log_acfuel, out log_acfuel);
    //                    cmd = new MySqlCommand("insert into triplogs (doe, km, place,  expamount, fuel, tripsno, load_cap ,load_cap_kgs, unload_cap, unload_cap_kgs, charge,log_rank,tollgateamnt,fuel_type,odometer,remarks,acfuel) values (@doe, @km, @place, @expamount, @fuel, @tripsno, @load_cap, @load_cap_kgs,@unload_cap, @unload_cap_kgs,@charge,@log_rank,@tollgateamnt,@fuel_type,@odometer,@remarks,@acfuel)");
    //                    cmd.Parameters.Add("@doe", art.log_datetime);
    //                    cmd.Parameters.Add("@km", kms);
    //                    cmd.Parameters.Add("@place", art.log_fromlocation);
    //                    cmd.Parameters.Add("@expamount", log_expences);
    //                    cmd.Parameters.Add("@fuel", fuel);
    //                    cmd.Parameters.Add("@tripsno", TripRefsno);
    //                    cmd.Parameters.Add("@load_cap", log_capload);
    //                    cmd.Parameters.Add("@unload_cap", log_capunload);
    //                    cmd.Parameters.Add("@charge", log_charge);
    //                    cmd.Parameters.Add("@log_rank", art.rank);
    //                    cmd.Parameters.Add("@tollgateamnt", log_tollgate);
    //                    cmd.Parameters.Add("@fuel_type", art.log_fueltype);
    //                    cmd.Parameters.Add("@odometer", art.log_OdoMeter);
    //                    cmd.Parameters.Add("@remarks", art.log_remarks);
    //                    cmd.Parameters.Add("@load_cap_kgs", log_capload_kgs);
    //                    cmd.Parameters.Add("@unload_cap_kgs", log_capunload_kgs);
    //                    cmd.Parameters.Add("@acfuel", log_acfuel);
    //                    vdm.insert(cmd);
    //                }
    //            }
    //            string response = GetJson("Trip Logs Edited Successfully");
    //            context.Response.Write(response);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        string response = GetJson(ex.Message);
    //        context.Response.Write(response);
    //    }
    //}
    public class tropdet
    {
        public string tripid { get; set; }
        public string tripbtnval { get; set; }
        public List<tripcls> triplogs_array { get; set; }
    }
    public class tripcls
    {
        public string log_datetime { get; set; }
        public string log_acfuel { get; set; }
        public string log_tolocation { get; set; }
        public string log_fromlocation { get; set; }
        public string log_kms { get; set; }
        public string log_charge { get; set; }
        public string log_capload { get; set; }
        public string log_capunload { get; set; }
        public string log_fuel { get; set; }
        public string log_expences { get; set; }
        public string rank { get; set; }
        public string log_tollgate { get; set; }
        public string log_fueltype { get; set; }
        public string log_OdoMeter { get; set; }
        public string log_capload_kgs { get; set; }
        public string log_capunload_kgs { get; set; }
        public string log_remarks { get; set; }
    }
    private void Triplogs_edit_Click(string jsonString, HttpContext context)
    {
        try
        {
            var js = new JavaScriptSerializer();
            var title1 = context.Request.Params[1];
            tropdet obj = js.Deserialize<tropdet>(jsonString);
            string TripRefsno = obj.tripid;
            string UserID = context.Session["Branch_ID"].ToString();
            string tripbtnval = obj.tripbtnval;


            if (tripbtnval == "Save Trip Logs")
            {
                foreach (tripcls art in obj.triplogs_array)
                {
                    if (art.log_datetime != "" && art.log_datetime != null)
                    {
                        double kms = 0.00;
                        double fuel = 0.00;
                        double log_capload = 0.00;
                        double log_capload_kgs = 0.00;
                        double log_capunload = 0.00;
                        double log_capunload_kgs = 0.00;
                        double log_charge = 0.00;
                        double log_tollgate = 0.00;
                        double log_expences = 0.00;
                        double log_acfuel = 0.00;
                        double.TryParse(art.log_kms, out kms);
                        double.TryParse(art.log_fuel, out fuel);
                        double.TryParse(art.log_capload, out log_capload);
                        double.TryParse(art.log_capload_kgs, out log_capload_kgs);
                        double.TryParse(art.log_capunload, out log_capunload);
                        double.TryParse(art.log_capunload_kgs, out log_capunload_kgs);
                        double.TryParse(art.log_charge, out log_charge);
                        double.TryParse(art.log_tollgate, out log_tollgate);
                        double.TryParse(art.log_expences, out log_expences);
                        double.TryParse(art.log_acfuel, out log_acfuel);
                        cmd = new MySqlCommand("insert into triplogs (doe, km, place,  expamount, fuel, tripsno, load_cap ,load_cap_kgs, unload_cap, unload_cap_kgs, charge,log_rank,tollgateamnt,fuel_type,odometer,remarks,acfuel) values (@doe, @km, @place, @expamount, @fuel, @tripsno, @load_cap, @load_cap_kgs,@unload_cap, @unload_cap_kgs,@charge,@log_rank,@tollgateamnt,@fuel_type,@odometer,@remarks,@acfuel)");
                        cmd.Parameters.Add("@doe", art.log_datetime);
                        cmd.Parameters.Add("@km", kms);
                        cmd.Parameters.Add("@place", art.log_fromlocation);
                        cmd.Parameters.Add("@expamount", log_expences);
                        cmd.Parameters.Add("@fuel", fuel);
                        cmd.Parameters.Add("@tripsno", TripRefsno);
                        cmd.Parameters.Add("@load_cap", log_capload);
                        cmd.Parameters.Add("@unload_cap", log_capunload);
                        cmd.Parameters.Add("@charge", log_charge);
                        cmd.Parameters.Add("@log_rank", art.rank);
                        cmd.Parameters.Add("@tollgateamnt", log_tollgate);
                        cmd.Parameters.Add("@fuel_type", art.log_fueltype);
                        cmd.Parameters.Add("@odometer", art.log_OdoMeter);
                        cmd.Parameters.Add("@remarks", art.log_remarks);
                        cmd.Parameters.Add("@load_cap_kgs", log_capload_kgs);
                        cmd.Parameters.Add("@unload_cap_kgs", log_capunload_kgs);
                        cmd.Parameters.Add("@acfuel", log_acfuel);
                        vdm.insert(cmd);
                    }
                }
                string response = GetJson("Trip Logs Saved Successfully");
                context.Response.Write(response);
            }
            else
            {
                cmd = new MySqlCommand("DELETE FROM triplogs WHERE (tripsno = @tripsno)");
                cmd.Parameters.Add("@tripsno", TripRefsno);
                vdm.Delete(cmd);
                foreach (tripcls art in obj.triplogs_array)
                {
                    if (art.log_datetime != "" && art.log_datetime != null)
                    {
                        double kms = 0.00;
                        double fuel = 0.00;
                        double log_capload = 0.00;
                        double log_capload_kgs = 0.00;
                        double log_capunload = 0.00;
                        double log_capunload_kgs = 0.00;
                        double log_charge = 0.00;
                        double log_tollgate = 0.00;
                        double log_expences = 0.00;
                        double log_acfuel = 0.00;
                        double.TryParse(art.log_kms, out kms);
                        double.TryParse(art.log_fuel, out fuel);
                        double.TryParse(art.log_capload, out log_capload);
                        double.TryParse(art.log_capload_kgs, out log_capload_kgs);
                        double.TryParse(art.log_capunload, out log_capunload);
                        double.TryParse(art.log_capunload_kgs, out log_capunload_kgs);
                        double.TryParse(art.log_charge, out log_charge);
                        double.TryParse(art.log_tollgate, out log_tollgate);
                        double.TryParse(art.log_expences, out log_expences);
                        double.TryParse(art.log_acfuel, out log_acfuel);
                        cmd = new MySqlCommand("insert into triplogs (doe, km, place,  expamount, fuel, tripsno, load_cap ,load_cap_kgs, unload_cap, unload_cap_kgs, charge,log_rank,tollgateamnt,fuel_type,odometer,remarks,acfuel) values (@doe, @km, @place, @expamount, @fuel, @tripsno, @load_cap, @load_cap_kgs,@unload_cap, @unload_cap_kgs,@charge,@log_rank,@tollgateamnt,@fuel_type,@odometer,@remarks,@acfuel)");
                        cmd.Parameters.Add("@doe", art.log_datetime);
                        cmd.Parameters.Add("@km", kms);
                        cmd.Parameters.Add("@place", art.log_fromlocation);
                        cmd.Parameters.Add("@expamount", log_expences);
                        cmd.Parameters.Add("@fuel", fuel);
                        cmd.Parameters.Add("@tripsno", TripRefsno);
                        cmd.Parameters.Add("@load_cap", log_capload);
                        cmd.Parameters.Add("@unload_cap", log_capunload);
                        cmd.Parameters.Add("@charge", log_charge);
                        cmd.Parameters.Add("@log_rank", art.rank);
                        cmd.Parameters.Add("@tollgateamnt", log_tollgate);
                        cmd.Parameters.Add("@fuel_type", art.log_fueltype);
                        cmd.Parameters.Add("@odometer", art.log_OdoMeter);
                        cmd.Parameters.Add("@remarks", art.log_remarks);
                        cmd.Parameters.Add("@load_cap_kgs", log_capload_kgs);
                        cmd.Parameters.Add("@unload_cap_kgs", log_capunload_kgs);
                        cmd.Parameters.Add("@acfuel", log_acfuel);
                        vdm.insert(cmd);
                    }
                }
                string response = GetJson("Trip Logs Edited Successfully");
                context.Response.Write(response);
            }
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }
    private void save_edit_TripLog(HttpContext context)
    {
        try
        {
            string Tripsno = context.Request["tripid"];
            //string UserID = context.Session["Branch_ID"].ToString();
            cmd = new MySqlCommand("SELECT tripdata.sno, vehicel_master.perkmcharge, tripdata.vehiclestartreading FROM tripdata INNER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno WHERE (tripdata.sno = @Tripsno)");
            cmd.Parameters.Add("@Tripsno", Tripsno);
            DataTable dtTrips = vdm.SelectQuery(cmd).Tables[0];
            double kmcost = 0;
            int start_odometer = 0;
            if (dtTrips.Rows.Count > 0)
            {
                string perkmcharge = dtTrips.Rows[0]["perkmcharge"].ToString();
                if (perkmcharge == "")
                {
                }
                else
                {
                    double.TryParse(perkmcharge, out kmcost);
                }
                string vehiclestartreading = dtTrips.Rows[0]["vehiclestartreading"].ToString();
                if (vehiclestartreading == "")
                {
                }
                else
                {
                    int.TryParse(vehiclestartreading, out start_odometer);
                }
            }

            string tripbtnval = context.Request["tripbtnval"];

            List<TripLog_items> Product_list = (List<TripLog_items>)context.Session["load_Next_TripLog"];

            if (tripbtnval == "Save Trip Logs")
            {
                int i = 1;
                foreach (TripLog_items art in Product_list)
                {
                    if (art.log_datetime != "" && art.log_datetime != null)
                    {
                        double kms = 0.00;
                        double fuel = 0.00;
                        double log_capload = 0.00;
                        double log_capload_kgs = 0.00;
                        double log_capunload = 0.00;
                        double log_capunload_kgs = 0.00;
                        double log_charge = 0.00;
                        double log_tollgate = 0.00;
                        double log_expences = 0.00;
                        double log_acfuel = 0.00;
                        double.TryParse(art.log_kms, out kms);
                        double.TryParse(art.log_fuel, out fuel);
                        double.TryParse(art.log_capload, out log_capload);
                        double.TryParse(art.log_capload_kgs, out log_capload_kgs);
                        double.TryParse(art.log_capunload, out log_capunload);
                        double.TryParse(art.log_capunload_kgs, out log_capunload_kgs);
                        double.TryParse(art.log_charge, out log_charge);
                        double.TryParse(art.log_tollgate, out log_tollgate);
                        double.TryParse(art.log_expences, out log_expences);
                        double.TryParse(art.log_acfuel, out log_acfuel);
                        int OdoMeter = 0;
                        int.TryParse(art.log_OdoMeter, out OdoMeter);
                        if (i == 1)
                        {
                            if (start_odometer == OdoMeter)
                            {
                                cmd = new MySqlCommand("insert into triplogs (doe, km, place,  expamount, fuel, tripsno, load_cap ,load_cap_kgs, unload_cap, unload_cap_kgs, charge,log_rank,tollgateamnt,fuel_type,odometer,remarks,acfuel) values (@doe, @km, @place, @expamount, @fuel, @tripsno, @load_cap, @load_cap_kgs,@unload_cap, @unload_cap_kgs,@charge,@log_rank,@tollgateamnt,@fuel_type,@odometer,@remarks,@acfuel)");
                                cmd.Parameters.Add("@doe", art.log_datetime);
                                cmd.Parameters.Add("@km", kms);
                                cmd.Parameters.Add("@place", art.log_fromlocation);
                                cmd.Parameters.Add("@expamount", log_expences);
                                cmd.Parameters.Add("@fuel", fuel);
                                cmd.Parameters.Add("@tripsno", Tripsno);
                                cmd.Parameters.Add("@load_cap", log_capload);
                                cmd.Parameters.Add("@unload_cap", log_capunload);
                                cmd.Parameters.Add("@charge", kmcost);
                                cmd.Parameters.Add("@log_rank", art.rank);
                                cmd.Parameters.Add("@tollgateamnt", log_tollgate);
                                cmd.Parameters.Add("@fuel_type", art.log_fueltype);
                                cmd.Parameters.Add("@odometer", art.log_OdoMeter);
                                cmd.Parameters.Add("@remarks", art.log_remarks);
                                cmd.Parameters.Add("@load_cap_kgs", log_capload_kgs);
                                cmd.Parameters.Add("@unload_cap_kgs", log_capunload_kgs);
                                cmd.Parameters.Add("@acfuel", log_acfuel);
                                vdm.insert(cmd);
                                i++;
                            }
                            else
                            {
                                string msg = GetJson("Starting odometer not match with present odometer.");
                                context.Response.Write(msg);
                            }
                        }
                        else
                        {
                            cmd = new MySqlCommand("insert into triplogs (doe, km, place,  expamount, fuel, tripsno, load_cap ,load_cap_kgs, unload_cap, unload_cap_kgs, charge,log_rank,tollgateamnt,fuel_type,odometer,remarks,acfuel) values (@doe, @km, @place, @expamount, @fuel, @tripsno, @load_cap, @load_cap_kgs,@unload_cap, @unload_cap_kgs,@charge,@log_rank,@tollgateamnt,@fuel_type,@odometer,@remarks,@acfuel)");
                            cmd.Parameters.Add("@doe", art.log_datetime);
                            cmd.Parameters.Add("@km", kms);
                            cmd.Parameters.Add("@place", art.log_fromlocation);
                            cmd.Parameters.Add("@expamount", log_expences);
                            cmd.Parameters.Add("@fuel", fuel);
                            cmd.Parameters.Add("@tripsno", Tripsno);
                            cmd.Parameters.Add("@load_cap", log_capload);
                            cmd.Parameters.Add("@unload_cap", log_capunload);
                            cmd.Parameters.Add("@charge", kmcost);
                            cmd.Parameters.Add("@log_rank", art.rank);
                            cmd.Parameters.Add("@tollgateamnt", log_tollgate);
                            cmd.Parameters.Add("@fuel_type", art.log_fueltype);
                            cmd.Parameters.Add("@odometer", art.log_OdoMeter);
                            cmd.Parameters.Add("@remarks", art.log_remarks);
                            cmd.Parameters.Add("@load_cap_kgs", log_capload_kgs);
                            cmd.Parameters.Add("@unload_cap_kgs", log_capunload_kgs);
                            cmd.Parameters.Add("@acfuel", log_acfuel);
                            vdm.insert(cmd);
                        }
                    }
                }
                string response = GetJson("Trip Logs Saved Successfully");
                context.Response.Write(response);
            }
            else
            {
                cmd = new MySqlCommand("DELETE FROM triplogs WHERE (tripsno = @tripsno)");
                cmd.Parameters.Add("@tripsno", Tripsno);
                vdm.Delete(cmd);
                foreach (TripLog_items art in Product_list)
                {
                    if (art.log_datetime != "" && art.log_datetime != null)
                    {
                        double kms = 0.00;
                        double fuel = 0.00;
                        double log_capload = 0.00;
                        double log_capload_kgs = 0.00;
                        double log_capunload = 0.00;
                        double log_capunload_kgs = 0.00;
                        double log_charge = 0.00;
                        double log_tollgate = 0.00;
                        double log_expences = 0.00;
                        double log_acfuel = 0.00;
                        double.TryParse(art.log_kms, out kms);
                        double.TryParse(art.log_fuel, out fuel);
                        double.TryParse(art.log_capload, out log_capload);
                        double.TryParse(art.log_capload_kgs, out log_capload_kgs);
                        double.TryParse(art.log_capunload, out log_capunload);
                        double.TryParse(art.log_capunload_kgs, out log_capunload_kgs);
                        double.TryParse(art.log_charge, out log_charge);
                        double.TryParse(art.log_tollgate, out log_tollgate);
                        double.TryParse(art.log_expences, out log_expences);
                        double.TryParse(art.log_acfuel, out log_acfuel);
                        cmd = new MySqlCommand("insert into triplogs (doe, km, place,  expamount, fuel, tripsno, load_cap ,load_cap_kgs, unload_cap, unload_cap_kgs, charge,log_rank,tollgateamnt,fuel_type,odometer,remarks,acfuel) values (@doe, @km, @place, @expamount, @fuel, @tripsno, @load_cap, @load_cap_kgs,@unload_cap, @unload_cap_kgs,@charge,@log_rank,@tollgateamnt,@fuel_type,@odometer,@remarks,@acfuel)");
                        cmd.Parameters.Add("@doe", art.log_datetime);
                        cmd.Parameters.Add("@km", kms);
                        cmd.Parameters.Add("@place", art.log_fromlocation);
                        cmd.Parameters.Add("@expamount", log_expences);
                        cmd.Parameters.Add("@fuel", fuel);
                        cmd.Parameters.Add("@tripsno", Tripsno);
                        cmd.Parameters.Add("@load_cap", log_capload);
                        cmd.Parameters.Add("@unload_cap", log_capunload);
                        cmd.Parameters.Add("@charge", log_charge);
                        cmd.Parameters.Add("@log_rank", art.rank);
                        cmd.Parameters.Add("@tollgateamnt", log_tollgate);
                        cmd.Parameters.Add("@fuel_type", art.log_fueltype);
                        cmd.Parameters.Add("@odometer", art.log_OdoMeter);
                        cmd.Parameters.Add("@remarks", art.log_remarks);
                        cmd.Parameters.Add("@load_cap_kgs", log_capload_kgs);
                        cmd.Parameters.Add("@unload_cap_kgs", log_capunload_kgs);
                        cmd.Parameters.Add("@acfuel", log_acfuel);
                        vdm.insert(cmd);
                    }
                }
                string response = GetJson("Trip Logs Edited Successfully");
                context.Response.Write(response);
            }
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }

    private void TripLog_save_RowData(TripLog_rowwise obj, HttpContext context)
    {
        try
        {
            TripLog_items o = obj.row_detail;
            List<TripLog_items> Product_list = (List<TripLog_items>)context.Session["load_Next_TripLog"];
            Product_list.Add(o);
            context.Session["load_Next_TripLog"] = Product_list;
            string msg = obj.end;
            string response = GetJson(msg);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }

    public class TripLog_rowwise
    {
        public string tripbtnval { get; set; }
        public string tripid { get; set; }
        public string end { get; set; }
        public TripLog_items row_detail { set; get; }
        public List<TripLog_items> triplogs_array { set; get; }
    }
    public class TripLog_items
    {

        public string log_datetime { get; set; }
        public string log_tolocation { get; set; }
        public string log_fromlocation { get; set; }
        public string log_kms { get; set; }
        public string log_charge { get; set; }
        public string log_capload { get; set; }
        public string log_capunload { get; set; }
        public string log_fuel { get; set; }
        public string log_expences { get; set; }
        public string rank { get; set; }
        public string log_tollgate { get; set; }
        public string log_fueltype { get; set; }
        public string log_OdoMeter { get; set; }
        public string log_capload_kgs { get; set; }
        public string log_capunload_kgs { get; set; }
        public string log_remarks { get; set; }
        public string log_acfuel { get; set; }
    }

    private void insert_Distances_fromtrip(HttpContext context)
    {
        try
        {
            string fromloc = context.Request["fromloc"];
            string toloc = context.Request["toloc"];
            string distance = context.Request["distance"];
            cmd = new MySqlCommand("insert into location_distances (From_location, To_location, Distance, branch_id) values (@From_location, @To_location, @Distance, @branch_id)");
            cmd.Parameters.Add("@From_location", toloc);
            cmd.Parameters.Add("@To_location", fromloc);
            cmd.Parameters.Add("@Distance", distance);
            cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
            vdm.insert(cmd);
            string response = GetJson("Distance Successfully Inserted");
            context.Response.Write(response);
        }

        catch (Exception ex)
        {
            string msg = ex.Message;
            string response = GetJson(msg);
            context.Response.Write(response);
        }
    }

    private void fill_tripsheet_no(HttpContext context)
    {
        try
        {
            cmd = new MySqlCommand("SELECT tripdata.sno, tripdata.tripsheetno,  tripdata.status,tripdata.tripdate, tripdata.vehicleno, vehicel_master.registration_no FROM tripdata INNER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno WHERE (tripdata.userid = @userid) AND (tripdata.status <> 'C')");
            cmd.Parameters.Add("@userid", context.Session["Branch_ID"]);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            List<he_dr_data> driver = new List<he_dr_data>();
            foreach (DataRow dr in dt.Rows)
            {
                he_dr_data drive = new he_dr_data();
                drive.sno = dr["sno"].ToString();
                drive.tripsheetno = dr["tripsheetno"].ToString();
                drive.tripdate = dr["tripdate"].ToString();
                drive.registration_no = dr["registration_no"].ToString();
                driver.Add(drive);
            }
            string response = GetJson(driver);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string msg = ex.Message;
            string response = GetJson(msg);
            context.Response.Write(response);
        }
    }

    public class he_dr_data
    {
        public string emp_sno { set; get; }
        public string employid { set; get; }
        public string employname { set; get; }
        public string emp_status { set; get; }
        public string Phoneno { set; get; }
        public string emp_type { set; get; }

        public string sno { set; get; }
        public string tripsheetno { set; get; }
        public string tripdate { set; get; }
        public string registration_no { set; get; }
    }

    private void get_driver_and_helper(HttpContext context)
    {
        try
        {
            string BranchID = context.Session["Branch_ID"].ToString();
            cmd = new MySqlCommand("SELECT emp_sno, employid, employname, emp_status, operatedby, Phoneno, branch_id,emp_type FROM employdata WHERE  (emp_status = '1') and (branch_id=@BranchID)");
            cmd.Parameters.Add("@BranchID", BranchID);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            List<he_dr_data> driver = new List<he_dr_data>();
            foreach (DataRow dr in dt.Rows)
            {
                he_dr_data drive = new he_dr_data();
                drive.emp_sno = dr["emp_sno"].ToString();
                drive.employid = dr["employid"].ToString();
                drive.employname = dr["employname"].ToString();
                drive.Phoneno = dr["Phoneno"].ToString();
                drive.emp_type = dr["emp_type"].ToString();
                driver.Add(drive);
            }
            string response = GetJson(driver);
            context.Response.Write(response);

        }
        catch (Exception ex)
        {
            string msg = ex.Message;
            string response = GetJson(msg);
            context.Response.Write(response);
        }
    }
    public class jobtypescls
    {
        public string jobtype { get; set; }
        public string jobdetails { get; set; }
    }
    class Routes
    {
        public string op { set; get; }
        public string RouteName { set; get; }
        public string PlantSno { set; get; }
        public List<string> data { set; get; }
        public string btnSave { set; get; }
        public string refno { set; get; }
        public string tripsno { set; get; }
        public List<jobtypescls> checkedjobcards { set; get; }
        public DateTime jobcarddate { set; get; }
    }
    public class tripdetailscls
    {
        public List<JobCardDetails> jobcards { get; set; }
        public string Tripdate { get; set; }
        public string Vehicleno { get; set; }
        public string Drivername { get; set; }
        public string RouteName { get; set; }
        public string StrartReading { get; set; }
        public string StrartFuel { get; set; }
        public string gpskms { get; set; }
        public string logfuel { get; set; }
        public string logexpences { get; set; }
    }
    public class JobCardDetails
    {
        public string status { get; set; }
        public string jobcardname { get; set; }
        public string jobcarddetails { get; set; }
    }


    private void btnTripendSaveClick(HttpContext context)
    {
        vdm = new VehicleDBMgr();
        try
        {
            string Username = context.Session["Branch_ID"].ToString();
            string tripsno = context.Request["tripsno"];
            string endodordng = context.Request["endodordng"];
            string endfuelrdng = context.Request["endfuelrdng"];
            string endhourmtrrdng = context.Request["endhourmtrrdng"];
            string gpskms = context.Request["gpskms"];
            string mileage = context.Request["mileage"];
            string txtpumpreading = context.Request["txtpumpreading"];
            int pumpreading = 0;
            int.TryParse(txtpumpreading, out pumpreading);
            string txttoken = context.Request["txttoken"];
            int token = 0;
            int.TryParse(txttoken, out token);
            string total_expences = context.Request["total_expences"];
            double ttl_exp = 0;
            double.TryParse(total_expences, out ttl_exp);
            string perlitercost = context.Request["fuelprice"];
            double litercost = 0;
            double.TryParse(perlitercost, out litercost);

            string refrigeration = context.Request["txtrefrigeration"];
            double refrigeration_fuel = 0;
            double.TryParse(refrigeration, out refrigeration_fuel);
            string ddlfuelstatus = context.Request["ddlfuelstatus"];
            double endodometer = 0;
            double.TryParse(endodordng, out endodometer);
            DateTime TripDate = VehicleDBMgr.GetTime(vdm.conn);
            cmd = new MySqlCommand("SELECT sno, tripsheetsno, jobcarddate, jobcardname, status, userid, operatedby FROM jobcards WHERE (status = @status) AND (tripsheetsno = @tripsheetsno)");
            cmd.Parameters.Add("@tripsheetsno", tripsno);
            cmd.Parameters.Add("@status", "A");
            DataTable dtJobCards = vdm.SelectQuery(cmd).Tables[0];
            if (dtJobCards.Rows.Count > 0)
            {
                string msg = "Please Complete Job cards";
                string response = GetJson(msg);
                context.Response.Write(response);
            }
            else
            {
                var Status = "C";
                cmd = new MySqlCommand("SELECT MAX(triplogs.odometer) AS Vehicle_endOdometer, tripdata.status FROM triplogs INNER JOIN tripdata ON triplogs.tripsno = tripdata.sno WHERE (triplogs.tripsno = @Tripsno)");
                cmd.Parameters.Add("@Tripsno", tripsno);
                DataTable dtEndodometer = vdm.SelectQuery(cmd).Tables[0];
                if (dtEndodometer.Rows.Count > 0)
                {
                    string Vehicle_endOdometer = dtEndodometer.Rows[0]["Vehicle_endOdometer"].ToString();
                    string tripstatus = dtEndodometer.Rows[0]["status"].ToString();
                    if (Vehicle_endOdometer == endodordng)
                    {
                        if (tripstatus == "P")
                        {
                            cmd = new MySqlCommand("UPDATE tripdata SET dieselcost=@dieselcost, Status=@Status,enddate=@EndDate,  EndFuelValue = @EndFuelValue,refrigeration_fuel=@refrigeration_fuel, EndHrMeter = @EndHrMeter,mileage=@mileage,pumpreading=@pumpreading,tokenno=@tokenno,tripexpences=@tripexpences WHERE (Sno = @tripsno)");
                            cmd.Parameters.Add("@Status", Status);
                            cmd.Parameters.Add("@EndDate", TripDate);
                            cmd.Parameters.Add("@EndFuelValue", endfuelrdng);
                            cmd.Parameters.Add("@EndHrMeter", endhourmtrrdng);
                            cmd.Parameters.Add("@tripsno", tripsno);
                            cmd.Parameters.Add("@mileage", mileage);
                            cmd.Parameters.Add("@pumpreading", pumpreading);
                            cmd.Parameters.Add("@tokenno", token);
                            cmd.Parameters.Add("@tripexpences", ttl_exp);
                            cmd.Parameters.Add("@refrigeration_fuel", refrigeration_fuel);
                            cmd.Parameters.Add("@dieselcost", perlitercost);
                            vdm.Update(cmd);
                            //cmd = new MySqlCommand("SELECT tripdata.tripsheetno as TripsheetSno,employdata.employname AS DriverName, vehicel_master.registration_no AS VehicleNo, DATE_FORMAT(tripdata.tripdate, '%m/%d/%Y %h:%i %p') AS StartDate, DATE_FORMAT(tripdata.enddate, '%m/%d/%Y %h:%i %p') AS EndDate, vehicel_master.vm_owner AS Owner,tripdata.endodometerreading - tripdata.vehiclestartreading AS TripKMS, tripdata.gpskms as GpsKms, ROUND(tripdata.endodometerreading - tripdata.vehiclestartreading - tripdata.gpskms, 2) AS DifferenceKMS, tripdata.endfuelvalue AS Diesel,ROUND((tripdata.endodometerreading - tripdata.vehiclestartreading) / tripdata.endfuelvalue, 2) AS TodayMileage, tripdata.loadtype as LoadType,tripdata.routeid, tripdata.qty as Qty, tripdata.tripexpences as Expenses FROM employdata INNER JOIN  tripdata ON employdata.emp_sno = tripdata.driverid INNER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno WHERE (tripdata.sno = @TripSno)");
                            cmd = new MySqlCommand("SELECT tripdata.tripsheetno AS TripsheetSno, employdata.employname AS DriverName,minimasters.mm_code as VehCode, minimasters_1.mm_code AS MakeCode, vehicel_master.registration_no AS VehicleNo, DATE_FORMAT(tripdata.tripdate, '%m/%d/%Y %h:%i %p') AS StartDate, DATE_FORMAT(tripdata.enddate, '%m/%d/%Y %h:%i %p') AS EndDate, vehicel_master.vm_owner AS Owner, tripdata.endodometerreading - tripdata.vehiclestartreading AS TripKMS, tripdata.gpskms AS GpsKms, ROUND(tripdata.endodometerreading - tripdata.vehiclestartreading - tripdata.gpskms, 2) AS DifferenceKMS, tripdata.endfuelvalue AS Diesel, ROUND((tripdata.endodometerreading - tripdata.vehiclestartreading) / tripdata.endfuelvalue, 2) AS TodayMileage, tripdata.loadtype AS LoadType, tripdata.routeid, tripdata.qty AS Qty, tripdata.tripexpences AS Expenses, minimasters.mm_name AS VehicleType, minimasters_1.mm_name AS Make, vehicel_master.Capacity FROM employdata INNER JOIN tripdata ON employdata.emp_sno = tripdata.driverid INNER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno INNER JOIN minimasters ON vehicel_master.vhtype_refno = minimasters.sno INNER JOIN minimasters minimasters_1 ON vehicel_master.vhmake_refno = minimasters_1.sno WHERE (tripdata.sno = @TripSno)");
                            cmd.Parameters.Add("@TripSno", tripsno);
                            DataTable dttrip = vdm.SelectQuery(cmd).Tables[0];
                            if (dttrip.Rows.Count > 0)
                            {
                                WebClient client = new WebClient();
                                string TripsheetSno = " T No:" + dttrip.Rows[0]["TripsheetSno"].ToString() + "\r\n";
                                string DriverName = " D Name:" + dttrip.Rows[0]["DriverName"].ToString() + "\r\n";
                                string VehicleNo = " VehicleNo:" + dttrip.Rows[0]["VehicleNo"].ToString() + "\r\n";
                                string Owner = " Owner:" + dttrip.Rows[0]["Owner"].ToString() + "\r\n";
                                string TripKMS = " TripKMS:" + dttrip.Rows[0]["TripKMS"].ToString() + "\r\n";
                                string GpsKms = " GpsKms:" + dttrip.Rows[0]["GpsKms"].ToString() + "\r\n";
                                string Diesel = " Diesel:" + dttrip.Rows[0]["Diesel"].ToString() + "\r\n";
                                string TodayMileage = " Mileage:" + dttrip.Rows[0]["TodayMileage"].ToString() + "\r\n";
                                string routeid = " Route:" + dttrip.Rows[0]["routeid"].ToString() + "\r\n";
                                string Make = " Make:" + dttrip.Rows[0]["MakeCode"].ToString() + "/" + dttrip.Rows[0]["VehCode"].ToString() + "/" + dttrip.Rows[0]["Capacity"].ToString() + "\r\n";
                                string trips = "Trip Details For " + "\r\n";
                                //string Phone = "9550522331,9382525919";
                                cmd = new MySqlCommand("SELECT sno, name, phoneno, emailid, alert_type, designation FROM personal_info WHERE (branch_sno = @BranchID)");
                                cmd.Parameters.Add("@BranchID", context.Session["Branch_ID"]);
                                DataTable dtphone = vdm.SelectQuery(cmd).Tables[0];
                                if (dttrip.Rows[0]["Owner"].ToString() == "SVDS")
                                {
                                    string tripDetails = trips + VehicleNo + routeid + Make + DriverName + TripKMS + GpsKms + Diesel + TodayMileage;
                                    foreach (DataRow dr in dtphone.Rows)
                                    {
                                        string str = dr["phoneno"].ToString();
                                        string emailid = dr["emailid"].ToString();
                                        string strdiesel = dttrip.Rows[0]["Diesel"].ToString();
                                        if (str.Length == 10)
                                        {
                                           // str = "9944349060";
                                            try
                                            {
                                                if (strdiesel == "" || strdiesel == "0")
                                                {
                                                }
                                                else
                                                {
                                                    string baseurl = "http://www.smsstriker.com/API/sms.php?username=vaishnavidairy&password=vyshnavi@123&from=VYSNVI&to=" + str + "&msg=" + tripDetails + "&type=1";
                                                    // string baseurl = "http://123.63.33.43/blank/sms/user/urlsms.php?username=vyshnavidairy&pass=vyshnavi@123&senderid=VFLEET&dest_mobileno=" + str + "&message=" + tripDetails + "&response=Y";
                                                    //string baseurl = "http://103.16.101.52:8080/sendsms/bulksms?username=kapd-vyshnavi&password=vysavi&type=0&dlr=1&destination=" + str + "&source=VFLEET&message=%20" + tripDetails + "";
                                                    Stream data = client.OpenRead(baseurl);
                                                    StreamReader reader = new StreamReader(data);
                                                    string ResponseID = reader.ReadToEnd();
                                                    data.Close();
                                                    reader.Close();
                                                }
                                            }
                                            catch
                                            {
                                            }
                                        }
                                    }
                                }
                            }
                            string msg = "Trip ended successfully";
                            string response = GetJson(msg);
                            context.Response.Write(response);
                        }
                        else
                        {
                            cmd = new MySqlCommand("UPDATE tripdata SET dieselcost=@dieselcost, Status=@Status,GpsKms = @GpsKms, EndDate = @EndDate, EndFuelValue = @EndFuelValue,refrigeration_fuel=@refrigeration_fuel, EndOdometerReading = @EndOdometerReading, EndHrMeter = @EndHrMeter,mileage=@mileage,pumpreading=@pumpreading,tokenno=@tokenno,tripexpences=@tripexpences WHERE (Sno = @tripsno)");
                            cmd.Parameters.Add("@Status", Status);
                            cmd.Parameters.Add("@GpsKms", gpskms);
                            cmd.Parameters.Add("@EndDate", TripDate);
                            cmd.Parameters.Add("@EndFuelValue", endfuelrdng);
                            cmd.Parameters.Add("@EndOdometerReading", endodordng);
                            cmd.Parameters.Add("@EndHrMeter", endhourmtrrdng);
                            cmd.Parameters.Add("@tripsno", tripsno);
                            cmd.Parameters.Add("@mileage", mileage);
                            cmd.Parameters.Add("@pumpreading", pumpreading);
                            cmd.Parameters.Add("@tokenno", token);
                            cmd.Parameters.Add("@tripexpences", ttl_exp);
                            cmd.Parameters.Add("@refrigeration_fuel", refrigeration_fuel);
                            cmd.Parameters.Add("@dieselcost", perlitercost);
                            vdm.Update(cmd);
                            cmd = new MySqlCommand("SELECT Vehicleno, ROUND(endodometerreading - vehiclestartreading, 2) AS tripKms FROM tripdata WHERE (sno = @TripSno)");
                            cmd.Parameters.Add("@TripSno", tripsno);
                            DataTable dtTrip = vdm.SelectQuery(cmd).Tables[0];
                            if (dtTrip.Rows.Count > 0)
                            {

                                cmd = new MySqlCommand("UPDATE vehicel_master set odometer=odometer+@odometer where vm_sno=@Vehicleno");
                                cmd.Parameters.Add("@odometer", dtTrip.Rows[0]["tripKms"].ToString());
                                cmd.Parameters.Add("@Vehicleno", dtTrip.Rows[0]["Vehicleno"].ToString());
                                vdm.Update(cmd);
                                cmd = new MySqlCommand("SELECT Sno, vehicle_mstr_sno, tyre_sno, Odometer FROM vehicle_master_sub WHERE (vehicle_mstr_sno = @VehicleSno)");
                                cmd.Parameters.Add("@VehicleSno", dtTrip.Rows[0]["Vehicleno"].ToString());
                                DataTable dtTyres = vdm.SelectQuery(cmd).Tables[0];
                                if (dtTyres.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dtTyres.Rows)
                                    {
                                        cmd = new MySqlCommand("update vehicle_master_sub set Odometer=Odometer+@Odometer where tyre_sno=@tyre_sno");
                                        cmd.Parameters.Add("@Odometer", dtTrip.Rows[0]["tripKms"].ToString());
                                        cmd.Parameters.Add("@tyre_sno", dr["tyre_sno"].ToString());
                                        vdm.Update(cmd);
                                        cmd = new MySqlCommand("update new_tyres_sub set current_KMS=current_KMS+@current_KMS where sno=@tyre_sno");
                                        cmd.Parameters.Add("@current_KMS", dtTrip.Rows[0]["tripKms"].ToString());
                                        cmd.Parameters.Add("@tyre_sno", dr["tyre_sno"].ToString());
                                        vdm.Update(cmd);
                                    }
                                }
                                cmd = new MySqlCommand("update  vehi_service_update_kms set doe=@doe,  airchecking=airchecking+@airchecking,tyreinterchanging=tyreinterchanging+@tyreinterchanging,eoc=eoc+@eoc, goc=goc+@goc, ofc=ofc+@ofc, afc=afc+@afc, brake_fluid=brake_fluid+@brake_fluid, steering_fluid=steering_fluid+@steering_fluid, transmission_fluid=transmission_fluid+@transmission_fluid, washer_fluid=washer_fluid+@washer_fluid, wheel_bearings=wheel_bearings+@wheel_bearings, checkleaks=checkleaks+@checkleaks, belts_hoses=belts_hoses+@belts_hoses, lubricate_chasis=lubricate_chasis+@lubricate_chasis where vehsno=@vehsno");
                                cmd.Parameters.Add("@doe", TripDate);
                                cmd.Parameters.Add("@airchecking", dtTrip.Rows[0]["tripKms"].ToString());
                                cmd.Parameters.Add("@tyreinterchanging", dtTrip.Rows[0]["tripKms"].ToString());
                                cmd.Parameters.Add("@eoc", dtTrip.Rows[0]["tripKms"].ToString());
                                cmd.Parameters.Add("@goc", dtTrip.Rows[0]["tripKms"].ToString());
                                cmd.Parameters.Add("@ofc", dtTrip.Rows[0]["tripKms"].ToString());
                                cmd.Parameters.Add("@afc", dtTrip.Rows[0]["tripKms"].ToString());
                                cmd.Parameters.Add("@brake_fluid", dtTrip.Rows[0]["tripKms"].ToString());
                                cmd.Parameters.Add("@steering_fluid", dtTrip.Rows[0]["tripKms"].ToString());
                                cmd.Parameters.Add("@transmission_fluid", dtTrip.Rows[0]["tripKms"].ToString());
                                cmd.Parameters.Add("@washer_fluid", dtTrip.Rows[0]["tripKms"].ToString());
                                cmd.Parameters.Add("@wheel_bearings", dtTrip.Rows[0]["tripKms"].ToString());
                                cmd.Parameters.Add("@checkleaks", dtTrip.Rows[0]["tripKms"].ToString());
                                cmd.Parameters.Add("@belts_hoses", dtTrip.Rows[0]["tripKms"].ToString());
                                cmd.Parameters.Add("@lubricate_chasis", dtTrip.Rows[0]["tripKms"].ToString());
                                cmd.Parameters.Add("@vehsno", dtTrip.Rows[0]["Vehicleno"].ToString());
                                vdm.Update(cmd);
                                //}
                            }
                            cmd = new MySqlCommand("SELECT sno, fuel, userid FROM fuel_monitor where userid=@userid");
                            cmd.Parameters.Add("@userid", context.Session["Branch_ID"]);
                            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
                            if (dt.Rows.Count > 0)
                            {
                                double Fuel = 0;
                                double.TryParse(endfuelrdng, out Fuel);
                                double total = 0;
                                total = Fuel + refrigeration_fuel;
                                double monitor_fuel = 0.00;
                                double.TryParse(dt.Rows[0]["fuel"].ToString(), out monitor_fuel);
                                double final_fule = monitor_fuel - total;
                                cmd = new MySqlCommand("update fuel_monitor set fuel=@fuel where UserID= @UserName ");
                                cmd.Parameters.Add("@UserName", context.Session["Branch_ID"]);
                                cmd.Parameters.Add("@fuel", final_fule);
                                vdm.Update(cmd);
                                cmd = new MySqlCommand("insert into fuel_transaction (transtype,fuel,doe,UserID,costperltr) values(@transtype,@fuel3,@doe,@UserID,@costperltr) ");
                                cmd.Parameters.Add("@UserID", context.Session["Branch_ID"]);
                                cmd.Parameters.Add("@fuel3", total);
                                cmd.Parameters.Add("@transtype", "3");
                                cmd.Parameters.Add("@doe", DateTime.Now);
                                cmd.Parameters.Add("@costperltr", litercost);
                                vdm.insert(cmd);
                            }
                            //cmd = new MySqlCommand("SELECT tripdata.tripsheetno as TripsheetSno,employdata.employname AS DriverName, vehicel_master.registration_no AS VehicleNo, DATE_FORMAT(tripdata.tripdate, '%m/%d/%Y %h:%i %p') AS StartDate, DATE_FORMAT(tripdata.enddate, '%m/%d/%Y %h:%i %p') AS EndDate, vehicel_master.vm_owner AS Owner,tripdata.endodometerreading - tripdata.vehiclestartreading AS TripKMS, tripdata.gpskms as GpsKms, ROUND(tripdata.endodometerreading - tripdata.vehiclestartreading - tripdata.gpskms, 2) AS DifferenceKMS, tripdata.endfuelvalue AS Diesel,ROUND((tripdata.endodometerreading - tripdata.vehiclestartreading) / tripdata.endfuelvalue, 2) AS TodayMileage, tripdata.loadtype as LoadType,tripdata.routeid, tripdata.qty as Qty, tripdata.tripexpences as Expenses FROM employdata INNER JOIN  tripdata ON employdata.emp_sno = tripdata.driverid INNER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno WHERE (tripdata.sno = @TripSno)");
                            cmd = new MySqlCommand("SELECT tripdata.tripsheetno AS TripsheetSno, employdata.employname AS DriverName,minimasters.mm_code as VehCode, minimasters_1.mm_code AS MakeCode, vehicel_master.registration_no AS VehicleNo, DATE_FORMAT(tripdata.tripdate, '%m/%d/%Y %h:%i %p') AS StartDate, DATE_FORMAT(tripdata.enddate, '%m/%d/%Y %h:%i %p') AS EndDate, vehicel_master.vm_owner AS Owner, tripdata.endodometerreading - tripdata.vehiclestartreading AS TripKMS, tripdata.gpskms AS GpsKms, ROUND(tripdata.endodometerreading - tripdata.vehiclestartreading - tripdata.gpskms, 2) AS DifferenceKMS, tripdata.endfuelvalue AS Diesel, ROUND((tripdata.endodometerreading - tripdata.vehiclestartreading) / tripdata.endfuelvalue, 2) AS TodayMileage, tripdata.loadtype AS LoadType, tripdata.routeid, tripdata.qty AS Qty, tripdata.tripexpences AS Expenses, minimasters.mm_name AS VehicleType, minimasters_1.mm_name AS Make, vehicel_master.Capacity FROM employdata INNER JOIN tripdata ON employdata.emp_sno = tripdata.driverid INNER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno INNER JOIN minimasters ON vehicel_master.vhtype_refno = minimasters.sno INNER JOIN minimasters minimasters_1 ON vehicel_master.vhmake_refno = minimasters_1.sno WHERE (tripdata.sno = @TripSno)");
                            cmd.Parameters.Add("@TripSno", tripsno);
                            DataTable dttrip = vdm.SelectQuery(cmd).Tables[0];
                            if (dttrip.Rows.Count > 0)
                            {
                                WebClient client = new WebClient();
                                string TripsheetSno = " T No:" + dttrip.Rows[0]["TripsheetSno"].ToString() + "\r\n";
                                string DriverName = " D Name:" + dttrip.Rows[0]["DriverName"].ToString() + "\r\n";
                                string VehicleNo = " VehicleNo:" + dttrip.Rows[0]["VehicleNo"].ToString() + "\r\n";
                                string Owner = " Owner:" + dttrip.Rows[0]["Owner"].ToString() + "\r\n";
                                string TripKMS = " TripKMS:" + dttrip.Rows[0]["TripKMS"].ToString() + "\r\n";
                                string GpsKms = " GpsKms:" + dttrip.Rows[0]["GpsKms"].ToString() + "\r\n";
                                string Diesel = " Diesel:" + dttrip.Rows[0]["Diesel"].ToString() + "\r\n";
                                string TodayMileage = " Mileage:" + dttrip.Rows[0]["TodayMileage"].ToString() + "\r\n";
                                string routeid = " Route:" + dttrip.Rows[0]["routeid"].ToString() + "\r\n";
                                string Make = " Make:" + dttrip.Rows[0]["MakeCode"].ToString() + "/" + dttrip.Rows[0]["VehCode"].ToString() + "/" + dttrip.Rows[0]["Capacity"].ToString() + "\r\n";
                                string trips = "Trip Details For " + "\r\n";
                                //string Phone = "9550522331,9382525919";
                                cmd = new MySqlCommand("SELECT sno, name, phoneno, emailid, alert_type, designation FROM personal_info WHERE (branch_sno = @BranchID)");
                                cmd.Parameters.Add("@BranchID", context.Session["Branch_ID"]);
                                DataTable dtphone = vdm.SelectQuery(cmd).Tables[0];
                                if (dttrip.Rows[0]["Owner"].ToString() == "SVDS")
                                {
                                    string tripDetails = trips + VehicleNo + routeid + Make + DriverName + TripKMS + GpsKms + Diesel + TodayMileage;
                                    foreach (DataRow dr in dtphone.Rows)
                                    {
                                        string str = dr["phoneno"].ToString();
                                        string emailid = dr["emailid"].ToString();
                                        string strdiesel = dttrip.Rows[0]["Diesel"].ToString();
                                        if (str.Length == 10)
                                        {
                                            try
                                            {
                                                if (strdiesel == "" || strdiesel == "0")
                                                {
                                                }
                                                else
                                                {
                                                    string baseurl = "http://www.smsstriker.com/API/sms.php?username=vaishnavidairy&password=vyshnavi@123&from=VYSNVI&to=" + str + "&msg=" + tripDetails + "&type=1";
                                                  //  string baseurl = "http://123.63.33.43/blank/sms/user/urlsms.php?username=vyshnavidairy&pass=vyshnavi@123&senderid=VFLEET&dest_mobileno=" + str + "&message=" + tripDetails + "&response=Y";
                                                 // string baseurl = "http://103.16.101.52:8080/sendsms/bulksms?username=kapd-vyshnavi&password=vysavi&type=0&dlr=1&destination=" + str + "&source=VFLEET&message=%20" + tripDetails + "";
                                                    Stream data = client.OpenRead(baseurl);
                                                    StreamReader reader = new StreamReader(data);
                                                    string ResponseID = reader.ReadToEnd();
                                                    data.Close();
                                                    reader.Close();
                                                }
                                            }
                                            catch
                                            {
                                            }
                                        }

                                       // SendHTMLMail(dttrip, emailid);
                                    }
                                }
                            }
                            GpsDBManager GpsDB = new GpsDBManager();
                            string routecode = "Plant";
                            if (dttrip.Rows.Count > 0)
                            {
                                string VehicleType = dttrip.Rows[0]["VehicleType"].ToString();
                                if (VehicleType == "Puff" || VehicleType == "Tanker")
                                {
                                    cmd = new MySqlCommand("update cabmanagement set routecode=@routecode,odometer=@odometer,odometer_time=@odometer_time,Smscount=0,SmsMobileNo=123 where VehicleID=@VehicleID");
                                    cmd.Parameters.Add("@routecode", routecode);
                                    cmd.Parameters.Add("@odometer", endodometer);
                                    cmd.Parameters.Add("@odometer_time", TripDate);
                                    cmd.Parameters.Add("@VehicleID", dttrip.Rows[0]["VehicleNo"].ToString());
                                    GpsDB.Update(cmd);
                                }
                            }
                            string msg = "Trip ended successfully";
                            string response = GetJson(msg);
                            context.Response.Write(response);
                        }
                    }
                    else
                    {
                        string msg = "Trip Logs Ending odometer not matchecd";
                        string response = GetJson(msg);
                        context.Response.Write(response);
                    }
                }
                else
                {
                    string msg = "No Trip Logs.Enter trip logs";
                    string response = GetJson(msg);
                    context.Response.Write(response);
                }
            }
        }
        catch (Exception ex)
        {
            string msg = ex.Message;
            string response = GetJson(msg);
            context.Response.Write(response);
        }
    }
    public void SendHTMLMail(DataTable dt, string Email)
    {
        string toAddress = Email;
        string subject = "Trip Details";
        string result = "Success";
        string senderID = "vyshnavidairyinfo@gmail.com";// use sender's email id here..
        const string senderPassword = "vyshnavi123"; // sender password here...
        try
        {
            SmtpClient smtp = new SmtpClient
            {
                Host = "smtp.gmail.com", // smtp server address here...
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new System.Net.NetworkCredential(senderID, senderPassword),
                Timeout = 30000,
            };

            MailMessage message = new MailMessage(senderID, toAddress, subject, GetdatatableData(dt));
            message.IsBodyHtml = true;
            smtp.Send(message);
        }
        catch (Exception ex)
        {
            result = "Error sending data please try again.!!!";
        }
        //SendEmail(toAddress, subject, body);
    }

    public string GetdatatableData(DataTable dtinv)
    {
        StringBuilder strBuilder = new StringBuilder();
        StringWriter strWriter = new StringWriter(strBuilder);
        HtmlTextWriter htw = new HtmlTextWriter(strWriter);
        DataGrid dg = new DataGrid();
        dg.DataSource = dtinv;
        dg.DataBind();
        dg.RenderControl(htw);
        //dtinv
        return strBuilder.ToString();
    }
    public class getjobcardscls
    {
        public string jobcard { get; set; }
        public string jobcarddetails { get; set; }
        public string jobcardstatus { get; set; }
    }
    private void gettripjobcards(HttpContext context)
    {
        vdm = new VehicleDBMgr();
        try
        {
            string Username = context.Session["Branch_ID"].ToString();
            string tripsno = context.Request["tripsno"];
            List<getjobcardscls> getjobcardsclslst = new List<getjobcardscls>();
            cmd = new MySqlCommand("SELECT sno, tripsheetsno, jobcarddate, jobcardname,jobcarddetails, status, userid, operatedby FROM jobcards where userid=@userid");
            cmd.Parameters.Add("@userid", Username);
            DataTable jobcarddata = vdm.SelectQuery(cmd).Tables[0];
            foreach (DataRow dr in jobcarddata.Rows)
            {
                getjobcardscls getjobcardss = new getjobcardscls();
                getjobcardss.jobcard = dr["jobcardname"].ToString();
                getjobcardss.jobcarddetails = dr["jobcarddetails"].ToString();
                getjobcardss.jobcardstatus = dr["status"].ToString();
                getjobcardsclslst.Add(getjobcardss);
            }
            string response = GetJson(getjobcardsclslst);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson("Error");
            context.Response.Write(response);
        }
    }
    GpsDBManager GpsDB = new GpsDBManager();
    private void gettripalldetails(HttpContext context)
    {
        vdm = new VehicleDBMgr();
        GpsDB = new GpsDBManager();
        try
        {
            string Username = context.Session["Branch_ID"].ToString();
            string mainuser = "Vyshnavi";
            string tripsno = context.Request["tripsno"];
            DateTime TripDate = DateTime.Now;

            cmd = new MySqlCommand("SELECT employdata.employname, routes.routename, tripdata.sno, tripdata.tripsheetno, tripdata.tripdate, tripdata.vehicleno, tripdata.vehiclestartreading, tripdata.routeid,tripdata.fueltank, tripdata.loadtype, jobcards.jobcarddate, jobcards.jobcardname, jobcards.status, jobcards.jobcarddetails, employdata.emp_licencenum,vehicel_master.registration_no, vehicel_master.fuel_capacity FROM tripdata INNER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno LEFT OUTER JOIN jobcards ON tripdata.userid = jobcards.userid AND tripdata.sno = jobcards.tripsheetsno LEFT OUTER JOIN employdata ON tripdata.driverid = employdata.emp_sno LEFT OUTER JOIN routes ON tripdata.routeid = routes.sno WHERE (tripdata.sno = @tripsheetsno)");
            //cmd = new MySqlCommand("SELECT employdata.employname, routes.routename, tripdata.sno, tripdata.tripsheetno, tripdata.tripdate, tripdata.vehicleno, tripdata.vehiclestartreading, tripdata.routeid, tripdata.fueltank, tripdata.loadtype,jobcards.jobcarddate, jobcards.jobcardname, jobcards.status, jobcards.jobcarddetails, employdata.emp_licencenum, vehicel_master.fuel_capacity, vehicel_master.registration_no, triplogs.fuel AS Fuel FROM tripdata INNER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno LEFT OUTER JOIN triplogs ON tripdata.sno = triplogs.tripsno LEFT OUTER JOIN jobcards ON tripdata.userid = jobcards.userid AND tripdata.sno = jobcards.tripsheetsno LEFT OUTER JOIN employdata ON tripdata.driverid = employdata.emp_sno LEFT OUTER JOIN routes ON tripdata.routeid = routes.sno WHERE (tripdata.sno = @tripsheetsno)"); 
            cmd.Parameters.Add("@tripsheetsno", tripsno);
            DataTable dttripsdata = vdm.SelectQuery(cmd).Tables[0];
            //DataView view = new DataView(dttripsdata);
            //DataTable jobcars = view.ToTable(true, "jobcardname", "status", "jobcarddetails");
            //DataTable fueldt = view.ToTable(true, "Fuel", "status", "jobcarddetails");
            if (dttripsdata.Rows.Count > 0)
            {
                tripdetailscls tripdetails = new tripdetailscls();
                tripdetails.Tripdate = dttripsdata.Rows[0]["tripdate"].ToString();
                tripdetails.Vehicleno = dttripsdata.Rows[0]["registration_no"].ToString();
                tripdetails.Drivername = dttripsdata.Rows[0]["employname"].ToString();
                tripdetails.RouteName = dttripsdata.Rows[0]["routeid"].ToString();
                tripdetails.StrartReading = dttripsdata.Rows[0]["vehiclestartreading"].ToString();
                //tripdetails.StrartFuel = dttripsdata.Rows[0]["FuelTank"].ToString();
                tripdetails.StrartFuel = dttripsdata.Rows[0]["fuel_capacity"].ToString();

                #region codefor selected vehicles
                DataTable logs = new DataTable();
                DataTable tottable = new DataTable();
                List<string> logstbls = new List<string>();
                logstbls.Add("GpsTrackVehicleLogs");
                logstbls.Add("GpsTrackVehicleLogs1");
                logstbls.Add("GpsTrackVehicleLogs2");
                logstbls.Add("GpsTrackVehicleLogs3");
                //DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
                DateTime fromdate = DateTime.Now;
                fromdate = DateTime.Parse(tripdetails.Tripdate);
                foreach (string tbname in logstbls)
                {
                    cmd = new MySqlCommand("SELECT " + tbname + ".VehicleID, " + tbname + ".Speed, " + tbname + ".DateTime, " + tbname + ".Diesel, " + tbname + ".Latitiude, " + tbname + ".Longitude, " + tbname + ".TimeInterval, " + tbname + ".inp4, " + tbname + ".Status, " + tbname + ".Odometer, " + tbname + ".Direction, " + tbname + ".Direction AS Expr1, vehiclemaster.VendorNo, vehiclemaster.VendorName, vehiclemaster.VehicleTypeName, vehiclemaster.MaintenancePlantName FROM " + tbname + " LEFT OUTER JOIN vehiclemaster ON " + tbname + ".VehicleID = vehiclemaster.VehicleID AND " + tbname + ".UserID = vehiclemaster.UserName WHERE (" + tbname + ".DateTime >= @starttime) AND (" + tbname + ".DateTime <= @endtime) AND (" + tbname + ".VehicleID = '" + tripdetails.Vehicleno + "') and (" + tbname + ".UserID='" + mainuser + "') ORDER BY " + tbname + ".DateTime");
                    cmd.Parameters.Add(new MySqlParameter("@starttime", fromdate));
                    cmd.Parameters.Add(new MySqlParameter("@endtime", TripDate));
                    logs = GpsDB.SelectQuery(cmd).Tables[0];
                    if (tottable.Rows.Count == 0)
                    {
                        tottable = logs.Clone();
                    }
                    foreach (DataRow dr in logs.Rows)
                    {
                        tottable.ImportRow(dr);
                    }
                }
                DataView dv = tottable.DefaultView;
                dv.Sort = "DateTime ASC";
                DataTable table = dv.ToTable();
                DataRow firstrow = null;
                DataRow lastrow = null;
                double TotalDistance = 0;
                if (table.Rows.Count > 1)
                {
                    firstrow = table.Rows[0];
                    lastrow = table.Rows[table.Rows.Count - 1];
                    if (firstrow != null && lastrow != null)
                    {
                        double firstval = 0;
                        double.TryParse(firstrow["Odometer"].ToString(), out firstval);
                        double lastval = 0;
                        double.TryParse(lastrow["Odometer"].ToString(), out lastval);
                        if (lastval > 0 && firstval > 0)
                            TotalDistance = lastval - firstval;
                    }
                }

                #endregion

                tripdetails.gpskms = TotalDistance.ToString();
                ////tripdetails.gpskms = "0";
                List<JobCardDetails> jobcards = new List<JobCardDetails>();
                foreach (DataRow dr in dttripsdata.Rows)
                {
                    JobCardDetails JobCard = new JobCardDetails();
                    JobCard.jobcardname = dr["jobcardname"].ToString();
                    JobCard.jobcarddetails = dr["jobcarddetails"].ToString();
                    JobCard.status = dr["status"].ToString();
                    jobcards.Add(JobCard);
                }
                cmd = new MySqlCommand("SELECT SUM(fuel) AS Fuel, tripsno,SUM(expamount) AS logexp FROM triplogs WHERE (tripsno = @tripsno)");
                cmd.Parameters.Add("@tripsno", tripsno);
                DataTable dtfuel = vdm.SelectQuery(cmd).Tables[0];
                if (dtfuel.Rows.Count > 0)
                {
                    tripdetails.logfuel = dtfuel.Rows[0]["Fuel"].ToString();
                    tripdetails.logexpences = dtfuel.Rows[0]["logexp"].ToString();
                }
                tripdetails.jobcards = jobcards;
                string response = GetJson(tripdetails);
                context.Response.Write(response);
            }
        }
        catch (Exception ex)
        {
            string msg = ex.Message;
            string response = GetJson(msg);
            context.Response.Write(response);
        }
    }
    private void jobcardsaveclick(Routes obj, HttpContext context)
    {
        vdm = new VehicleDBMgr();
        try
        {
            string Username = context.Session["Branch_ID"].ToString();
            //var js = new JavaScriptSerializer();
            //var title1 = context.Request.Params[1];
            //Routes obj = js.Deserialize<Routes>(title1);
            string tripsno = obj.tripsno;
            DateTime Currentdate = DateTime.Now;
            DateTime jobcarddate = obj.jobcarddate;
            foreach (jobtypescls jbcrd in obj.checkedjobcards)
            {
                cmd = new MySqlCommand("insert into jobcards (tripsheetsno, jobcarddate,jobcardname,jobcarddetails,status, userid, operatedby) values (@tripsheetsno, @jobcarddate,@jobcardname,@jobcarddetails,@status, @userid, @operatedby)");
                cmd.Parameters.Add("@tripsheetsno", tripsno);
                cmd.Parameters.Add("@jobcarddate", Currentdate);
                cmd.Parameters.Add("@jobcardname", jbcrd.jobtype);
                cmd.Parameters.Add("@jobcarddetails", jbcrd.jobdetails);
                cmd.Parameters.Add("@status", "A");
                cmd.Parameters.Add("@userid", Username);
                cmd.Parameters.Add("@operatedby", Username);
                vdm.insert(cmd);
            }

            cmd = new MySqlCommand("UPDATE tripdata SET Jobcards = 'Y' WHERE (Sno = @Sno)");
            cmd.Parameters.Add("@Sno", tripsno);
            vdm.Update(cmd);
            string msg = "Jobcards successfully added";
            string response = GetJson(msg);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }
    private void btnstart_tripsheetclick(HttpContext context)
    {
        vdm = new VehicleDBMgr();
        WebClient client = new WebClient();
        try
        {
            string BranchID = context.Session["Branch_ID"].ToString();
            string vehicleNo = context.Request["vehicle_no"];
            string driver = context.Request["driver"];
            string RouteID = context.Request["RouteID"];
            string qty = context.Request["qty"];
            string VehicleStartReading = context.Request["VehicleStartReading"];
            string hourreading = context.Request["hourreading"];
            string BillingOwnerID = context.Request["BillingOwnerID"];
            string Status = "A";
            DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
            string UserName = context.Session["Employ_Sno"].ToString();
            int currentyear = ServerDateCurrentdate.Year;
            int nextyear = ServerDateCurrentdate.Year + 1;
            DateTime dtapril = new DateTime();
            DateTime dtmarch = new DateTime();
            if (ServerDateCurrentdate.Month > 3)
            {
                string apr = "4/1/" + currentyear;
                dtapril = DateTime.Parse(apr);
                string march = "3/31/" + nextyear;
                dtmarch = DateTime.Parse(march);
            }
            if (ServerDateCurrentdate.Month <= 3)
            {
                string apr = "4/1/" + (currentyear - 1);
                dtapril = DateTime.Parse(apr);
                string march = "3/31/" + (nextyear - 1);
                dtmarch = DateTime.Parse(march);
            }
            cmd = new MySqlCommand("Select IFNULL(MAX(TripSno),0)+1 as TripSno from tripdata where UserID=@UserID and tripdate between @d1 and @d2");
            cmd.Parameters.Add("@UserID", BranchID);
            cmd.Parameters.Add("@d1", GetLowDate(dtapril));
            cmd.Parameters.Add("@d2", GetHighDate(dtmarch));
            DataTable dtReceipt = vdm.SelectQuery(cmd).Tables[0];
            string TripNo = "T" + dtReceipt.Rows[0]["TripSno"].ToString();
            string TSno = dtReceipt.Rows[0]["TripSno"].ToString();

            cmd = new MySqlCommand("Select Cost,perkmcharge,registration_no,Capacity from vehicel_master where vm_sno=@vehicleSno");
            cmd.Parameters.Add("@vehicleSno", vehicleNo);
            DataTable dtCost = vdm.SelectQuery(cmd).Tables[0];
            string registrationno = dtCost.Rows[0]["registration_no"].ToString();
            string capacity = dtCost.Rows[0]["Capacity"].ToString();
            double Cost = 0;
            double perkmcharge = 0;
            if (dtCost.Rows.Count > 0)
            {
                string Rate = dtCost.Rows[0]["Cost"].ToString();
                string KmCharge = dtCost.Rows[0]["perkmcharge"].ToString();
                double.TryParse(Rate, out Cost);
                double.TryParse(KmCharge, out perkmcharge);
            }
            cmd = new MySqlCommand("Select costperltr from  fuel_monitor ");
            cmd.Parameters.Add("@vehicleSno", vehicleNo);
            DataTable dtDieselCost = vdm.SelectQuery(cmd).Tables[0];
            double DieselCost = 53;
            if (dtDieselCost.Rows.Count > 0)
            {
                string Rate = dtDieselCost.Rows[0]["costperltr"].ToString();
                double.TryParse(Rate, out DieselCost);
            }
            cmd = new MySqlCommand("insert into tripdata (tripsheetno,Tripdate, Vehicleno, vehiclestartreading,  DriverID, RouteID,Status,UserID,OperatedBy,Rent,DieselCost,perkmcharge,TripSno,doe,qty,HourReading,BillingOwnerID) values (@tripsheetno,@Tripdate, @Vehicleno, @StrartReading, @DriverID, @RouteID,@Status,@UserID,@OperatedBy,@Rent,@DieselCost,@perkmcharge,@TripSno,@doe,@qty,@HourReading,@BillingOwnerID)");
            cmd.Parameters.Add("@tripsheetno", TripNo);
            cmd.Parameters.Add("@Tripdate", ServerDateCurrentdate);
            cmd.Parameters.Add("@Vehicleno", vehicleNo);
            cmd.Parameters.Add("@StrartReading", VehicleStartReading);
            cmd.Parameters.Add("@DriverID", driver);
            cmd.Parameters.Add("@RouteID", RouteID);
            cmd.Parameters.Add("@Status", Status);
            cmd.Parameters.Add("@UserID", BranchID);
            cmd.Parameters.Add("@OperatedBy", UserName);
            cmd.Parameters.Add("@Rent", Cost);
            cmd.Parameters.Add("@DieselCost", DieselCost);
            cmd.Parameters.Add("@perkmcharge", perkmcharge);
            cmd.Parameters.Add("@TripSno", TSno);
            cmd.Parameters.Add("@doe", ServerDateCurrentdate);
            cmd.Parameters.Add("@qty", qty);
            cmd.Parameters.Add("@HourReading", hourreading);
            cmd.Parameters.Add("@BillingOwnerID", BillingOwnerID);
            vdm.insert(cmd);
            cmd = new MySqlCommand(" Select vehicel_master.registration_no, employdata.employname, employdata.Phoneno FROM vehicel_master INNER JOIN tripdata ON vehicel_master.vm_sno = tripdata.vehicleno INNER JOIN employdata ON tripdata.driverid = employdata.emp_sno WHERE (vehicel_master.vm_sno = @VehSno) AND (tripdata.tripdate BETWEEN @d1 AND @d2)");
            cmd.Parameters.Add("@VehSno", vehicleNo);
            cmd.Parameters.Add("@d1", GetLowDate(ServerDateCurrentdate));
            cmd.Parameters.Add("@d2", GetHighDate(ServerDateCurrentdate));
            DataTable dtvehicleNo = vdm.SelectQuery(cmd).Tables[0];
            string vehicle_no = "";
            string Drivername = "";
            string phoneno = "";
            if (dtvehicleNo.Rows.Count > 0)
            {
                vehicle_no = dtvehicleNo.Rows[0]["registration_no"].ToString();
                Drivername = dtvehicleNo.Rows[0]["employname"].ToString();
                phoneno = dtvehicleNo.Rows[0]["Phoneno"].ToString();
            }

            //
            string routemobno = "";
            cmd = new MySqlCommand("SELECT    routename, mobileno FROM   routes WHERE  (routename = @RouteID)");
            cmd.Parameters.Add("@RouteID", RouteID);
            DataTable dtmobileno = vdm.SelectQuery(cmd).Tables[0];
            routemobno = dtmobileno.Rows[0]["mobileno"].ToString();
            string serdate = Convert.ToDateTime(ServerDateCurrentdate).ToString("dd-MM-yyyy h:mm tt");
            //
            GpsDBManager GpsDB = new GpsDBManager();
            cmd = new MySqlCommand("update cabmanagement set routecode=@routecode ,DriverName=@DriverName,MobileNo=@MobileNo,Smscount=0,SmsMobileNo=@routemobno where VehicleID=@VehicleID");
            cmd.Parameters.Add("@routecode", RouteID);
            cmd.Parameters.Add("@DriverName", Drivername);
            cmd.Parameters.Add("@MobileNo", phoneno);
            cmd.Parameters.Add("@VehicleID", vehicle_no);
            cmd.Parameters.Add("@routemobno", routemobno);
            GpsDB.Update(cmd);

            //send messages
            cmd = new MySqlCommand(" SELECT  employid, employname, emp_status, operatedby, Phoneno, emp_type FROM   employdata WHERE  (emp_status = '1') AND (emp_sno = @empsno)");
            cmd.Parameters.Add("@empsno", driver);
            DataTable dtdriver = vdm.SelectQuery(cmd).Tables[0];
            string drivernames = dtdriver.Rows[0]["employname"].ToString();
            string driverphoneno = dtdriver.Rows[0]["Phoneno"].ToString();

            string getlocation = "http://182.18.162.51/VYS/LiveTracking.aspx?Id=" + registrationno.Trim();           
            //Sent Message to Route
            if (routemobno.Length == 10)
            {
                string sendmsg = "Vehicle No " + registrationno + " Capacity " + capacity + " assigned to Route " + RouteID + " Date " + serdate + " . Driver Name " + drivernames + " Phone Number " + driverphoneno + "Track Link" + getlocation;
                string baseurl = "http://www.smsstriker.com/API/sms.php?username=vaishnavidairy&password=vyshnavi@123&from=VYSNVI&to=" + routemobno + "&msg=" + sendmsg + "&type=1";
               // string baseurl = "http://123.63.33.43/blank/sms/user/urlsms.php?username=vyshnavidairy&pass=vyshnavi@123&senderid=VFLEET&dest_mobileno=" + routemobno + "&message=" + sendmsg + "&response=Y";
              //string baseurl = "http://103.16.101.52:8080/sendsms/bulksms?username=kapd-vyshnavi&password=vysavi&type=0&dlr=1&destination=" + routemobno + "&message=%20" + sendmsg + "&source=VFLEET&message";
                Stream data = client.OpenRead(baseurl);
                StreamReader reader = new StreamReader(data);
                string ResponseID = reader.ReadToEnd();
                data.Close();
                reader.Close();
            }
            //send message
            //string PhoneNos = "9848982383";
            //if (PhoneNos.Length == 10)
            //{
            //    string sendmsg = "Vehicle No " + registrationno + " Capacity " + capacity + " assigned to Route " + RouteID + " . Driver Name " + drivernames + " Phone Number " + driverphoneno + "";
            //    string baseurl = "http://103.16.101.52:8080/sendsms/bulksms?username=kapd-vyshnavi&password=vysavi&type=0&dlr=1&destination=" + PhoneNos + "&message=%20" + sendmsg + "&source=VYSAKG&message";
            //    Stream data = client.OpenRead(baseurl);
            //    StreamReader reader = new StreamReader(data);
            //    string ResponseID = reader.ReadToEnd();
            //    data.Close();
            //    reader.Close();
            //}
            string msg = "Trip Sheet Saved Successfully";
            string response = GetJson(msg);
            context.Response.Write(response);
        }
        catch (Exception EX)
        {
            string response = GetJson(EX.Message);
            context.Response.Write(response);
        }
    }
    private void btnTripSheetSaveClick(HttpContext context)
    {
        try
        {
            vdm = new VehicleDBMgr();
            WebClient client = new WebClient();
            string TripDate = context.Request["TripDate"];
            string vehicleNo = context.Request["vehicleNo"];
            string RouteID = context.Request["RouteID"];
            string driver = context.Request["driver"];
            string helper = context.Request["helper"];
            string VehicleStartReading = context.Request["VehicleStartReading"];
            string HrReading = context.Request["HrReading"];
            string FuelTank = context.Request["FuelTank"];
            string load = context.Request["load"];
            string Qty = context.Request["Qty"];
            string tripstartfrom = context.Request["tripstartfrom"];
            string BillingOwnerID = context.Request["BillingOwnerID"];
            string Dsalary = context.Request["Dsalary"];
            //string txtpumpreading = context.Request["txtpumpreading"];
            //int pumpreading = 0;
            //int.TryParse(txtpumpreading, out pumpreading);
            //string txttoken = context.Request["txttoken"];
            //int token = 0;
            //int.TryParse(txttoken, out token);
            string Status = "A";
            DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
            string BranchID = context.Session["Branch_ID"].ToString();
            string UserName = context.Session["Employ_Sno"].ToString();
            int currentyear = ServerDateCurrentdate.Year;
            int nextyear = ServerDateCurrentdate.Year + 1;
            DateTime dtapril = new DateTime();
            DateTime dtmarch = new DateTime();
            if (ServerDateCurrentdate.Month > 3)
            {
                string apr = "4/1/" + currentyear;
                dtapril = DateTime.Parse(apr);
                string march = "3/31/" + nextyear;
                dtmarch = DateTime.Parse(march);
            }
            if (ServerDateCurrentdate.Month <= 3)
            {
                string apr = "4/1/" + (currentyear - 1);
                dtapril = DateTime.Parse(apr);
                string march = "3/31/" + (nextyear - 1);
                dtmarch = DateTime.Parse(march);
            }
            cmd = new MySqlCommand("Select IFNULL(MAX(TripSno),0)+1 as TripSno from tripdata where UserID=@UserID and tripdate between @d1 and @d2");
            cmd.Parameters.Add("@UserID", BranchID);
            cmd.Parameters.Add("@d1", GetLowDate(dtapril));
            cmd.Parameters.Add("@d2", GetHighDate(dtmarch));
            DataTable dtReceipt = vdm.SelectQuery(cmd).Tables[0];
            string TripNo = "T" + dtReceipt.Rows[0]["TripSno"].ToString();
            string TSno = dtReceipt.Rows[0]["TripSno"].ToString();

            //Trip Paymentmode update one field (Paymentmode)

            //
            cmd = new MySqlCommand("Select Cost,perkmcharge,registration_no,Capacity from vehicel_master where vm_sno=@vehicleSno");
            cmd.Parameters.Add("@vehicleSno", vehicleNo);
            DataTable dtCost = vdm.SelectQuery(cmd).Tables[0];
            string registrationno = dtCost.Rows[0]["registration_no"].ToString();
            string capacity = dtCost.Rows[0]["Capacity"].ToString();
            double Cost = 0;
            double perkmcharge = 0;
            if (dtCost.Rows.Count > 0)
            {
                string Rate = dtCost.Rows[0]["Cost"].ToString();
                string KmCharge = dtCost.Rows[0]["perkmcharge"].ToString();
                double.TryParse(Rate, out Cost);
                double.TryParse(KmCharge, out perkmcharge);
            }
            cmd = new MySqlCommand("Select costperltr from  fuel_monitor ");
            cmd.Parameters.Add("@vehicleSno", vehicleNo);
            DataTable dtDieselCost = vdm.SelectQuery(cmd).Tables[0];
            double DieselCost = 53;
            if (dtDieselCost.Rows.Count > 0)
            {
                string Rate = dtDieselCost.Rows[0]["costperltr"].ToString();
                double.TryParse(Rate, out DieselCost);
            }
            cmd = new MySqlCommand("insert into tripdata (tripsheetno,Tripdate, Vehicleno, vehiclestartreading, HourReading, DriverID, HelperID, LoadType, Qty,  RouteID,Status,UserID,OperatedBy,Rent,DieselCost,perkmcharge,TripSno,doe,BillingOwnerID,Dsalary) values (@tripsheetno,@Tripdate, @Vehicleno, @StrartReading, @HourReading, @DriverID, @HelperID, @LoadType, @Qty, @RouteID,@Status,@UserID,@OperatedBy,@Rent,@DieselCost,@perkmcharge,@TripSno,@doe,@BillingOwnerID,@Dsalary)");
            cmd.Parameters.Add("@tripsheetno", TripNo);
            cmd.Parameters.Add("@perkmcharge", perkmcharge);
            cmd.Parameters.Add("@Tripdate", ServerDateCurrentdate);
            cmd.Parameters.Add("@Vehicleno", vehicleNo);
            cmd.Parameters.Add("@StrartReading", VehicleStartReading);
            cmd.Parameters.Add("@HourReading", HrReading);
            cmd.Parameters.Add("@DriverID", driver);
            cmd.Parameters.Add("@HelperID", helper);
            cmd.Parameters.Add("@TripSno", TSno);
            cmd.Parameters.Add("@LoadType", load);
            cmd.Parameters.Add("@Qty", Qty);
            cmd.Parameters.Add("@RouteID", RouteID);
            cmd.Parameters.Add("@Status", Status);
            cmd.Parameters.Add("@UserID", BranchID);
            cmd.Parameters.Add("@OperatedBy", UserName);
            cmd.Parameters.Add("@Rent", Cost);
            cmd.Parameters.Add("@DieselCost", DieselCost);
            cmd.Parameters.Add("@doe", ServerDateCurrentdate);
            cmd.Parameters.Add("@BillingOwnerID", BillingOwnerID);
            cmd.Parameters.Add("@Dsalary", Dsalary);

            vdm.insert(cmd);

            cmd = new MySqlCommand(" Select vehicel_master.registration_no, employdata.employname, employdata.Phoneno FROM vehicel_master INNER JOIN tripdata ON vehicel_master.vm_sno = tripdata.vehicleno INNER JOIN employdata ON tripdata.driverid = employdata.emp_sno WHERE (vehicel_master.vm_sno = @VehSno) AND (tripdata.tripdate BETWEEN @d1 AND @d2)");
            cmd.Parameters.Add("@VehSno", vehicleNo);
            cmd.Parameters.Add("@d1", GetLowDate(ServerDateCurrentdate));
            cmd.Parameters.Add("@d2", GetHighDate(ServerDateCurrentdate));
            DataTable dtvehicleNo = vdm.SelectQuery(cmd).Tables[0];
            string vehicle_no = "";
            string Drivername = "";
            string phoneno = "";
            if (dtvehicleNo.Rows.Count > 0)
            {
                vehicle_no = dtvehicleNo.Rows[0]["registration_no"].ToString();
                Drivername = dtvehicleNo.Rows[0]["employname"].ToString();
                phoneno = dtvehicleNo.Rows[0]["Phoneno"].ToString();
            }
            //
            string routemobno = "";
            cmd = new MySqlCommand("SELECT  routename, mobileno FROM   routes WHERE  (routename = @RouteID)");
            cmd.Parameters.Add("@RouteID", RouteID);
            DataTable dtmobileno = vdm.SelectQuery(cmd).Tables[0];
            routemobno = dtmobileno.Rows[0]["mobileno"].ToString();
           // routemobno = "9944349060";
            //
            GpsDBManager GpsDB = new GpsDBManager();
            cmd = new MySqlCommand("update cabmanagement set routecode=@routecode ,DriverName=@DriverName,MobileNo=@MobileNo,Smscount=0,SmsMobileNo=@routemobno where VehicleID=@VehicleID");
            cmd.Parameters.Add("@routecode", RouteID);
            cmd.Parameters.Add("@DriverName", Drivername);
            cmd.Parameters.Add("@MobileNo", phoneno);
            cmd.Parameters.Add("@VehicleID", vehicle_no);
            cmd.Parameters.Add("@routemobno", routemobno);
            GpsDB.Update(cmd);

            //send message 
            cmd = new MySqlCommand(" SELECT  employid, employname, emp_status, operatedby, Phoneno, emp_type FROM   employdata WHERE  (emp_status = '1') AND (emp_sno = @empsno)");
            cmd.Parameters.Add("@empsno", driver);
            DataTable dtdriver = vdm.SelectQuery(cmd).Tables[0];
            string drivernames = dtdriver.Rows[0]["employname"].ToString();
            string driverphoneno = dtdriver.Rows[0]["Phoneno"].ToString();

            

            string serdate = Convert.ToDateTime(ServerDateCurrentdate).ToString("dd-MM-yyyy h:mm tt");

            //cmd = new MySqlCommand("SELECT  UserName, VehicleID, Lat, Longi, Speed,Timestamp, Direction, Diesel, Odometer, Ignation, AC, Status, Geofense, In1, In2, In3, In4, In5, Op1, Op2, Op3, Op4, Op5, GSMSignal, GPSSignal, SatilitesAvail, EP, BP, Altitude, StoppedTime, DayOdometer, Tempsensor1, Tempsensor2, Tempsensor3, Tempsensor4, BV, ES, EL, HAN, HBN FROM   onlinetable WHERE  (VehicleID = @vehicleid)");
            //cmd.Parameters.Add("@vehicleid", registrationno);
            //DataTable dtonlinetbl = GpsDB.SelectQuery(cmd).Tables[0];
            //string latitude = dtonlinetbl.Rows[0]["Lat"].ToString();
            //string longitude = dtonlinetbl.Rows[0]["Longi"].ToString();


            string getlocation = "http://182.18.162.51/VYS/LiveTracking.aspx?Id=" + registrationno.Trim();
            //Sent Message to Route
            if (routemobno.Length == 10)
            {
                string sendmsg = "Vehicle No " + registrationno + " Capacity " + capacity + " assigned to Route " + RouteID + " Date " + serdate + " . Driver Name " + drivernames + " Phone Number " + driverphoneno + "Track Link" + getlocation;
                string baseurl = "http://www.smsstriker.com/API/sms.php?username=vaishnavidairy&password=vyshnavi@123&from=VYSNVI&to=" + routemobno + "&msg=" + sendmsg + "&type=1";
               // string baseurl1 = "http://123.63.33.43/blank/sms/user/urlsms.php?username=vyshnavidairy&pass=vyshnavi@123&senderid=VFLEET&dest_mobileno=" + routemobno + "&message=" + sendmsg + "&response=Y";
               // string baseurl = "http://103.16.101.52:8080/sendsms/bulksms?username=kapd-vyshnavi&password=vysavi&type=0&dlr=1&destination=" + routemobno + "&message=%20" + sendmsg + "&source=VFLEET&message";
                Stream data = client.OpenRead(baseurl);
                StreamReader reader = new StreamReader(data);
                string ResponseID = reader.ReadToEnd();
                data.Close();
                reader.Close();
            }

            string msg = "Trip Sheet Saved Successfully";
            string response = GetJson(msg);
            context.Response.Write(response);
        }
        catch (Exception EX)
        {
            string response = GetJson(EX.Message);
            context.Response.Write(response);
        }
    }
    private void getremaining_veh_data(HttpContext context)
    {
        try
        {
            string vm_sno = context.Request["vm_sno"];
            cmd = new MySqlCommand("SELECT registration_no, door_no, status, branch_id, operatedby, Capacity, vm_model, vm_engine, vm_chasiss, vm_owner, vm_owneraddr, vm_rcno, vm_rcexpdate, vm_pollution, vm_poll_exp_date, vm_insurance,vm_isurence_exp_date, vm_fitness, vm_fit_exp_date, vm_roadtax, vm_roadtax_exp_date, vm_sno, vhtype_refno, axils_refno FROM vehicel_master WHERE (vm_sno = @vm_sno)");
            cmd.Parameters.Add("@vm_sno", vm_sno);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            List<get_veh_master_data> vendor = new List<get_veh_master_data>();
            foreach (DataRow dr in dt.Rows)
            {

                get_veh_master_data data = new get_veh_master_data();
                data.vm_model = dr["vm_model"].ToString();
                data.vm_engine = dr["vm_engine"].ToString();
                data.vm_chasiss = dr["vm_chasiss"].ToString();
                data.vm_owner = dr["vm_owner"].ToString();
                data.vm_owneraddr = dr["vm_owneraddr"].ToString();
                data.vm_rcno = dr["vm_rcno"].ToString();
                if (dr["vm_rcexpdate"].ToString() != "" && dr["vm_rcexpdate"].ToString() != null)
                {
                    data.vm_rcexpdate = ((DateTime)dr["vm_rcexpdate"]).ToString("yyyy-MM-dd");
                }
                else
                {
                    data.vm_poll_exp_date = dr["vm_rcexpdate"].ToString();
                }
                data.vm_pollution = dr["vm_pollution"].ToString();
                if (dr["vm_poll_exp_date"].ToString() != "" && dr["vm_poll_exp_date"].ToString() != null)
                {
                    data.vm_poll_exp_date = ((DateTime)dr["vm_poll_exp_date"]).ToString("yyyy-MM-dd");
                }
                else
                {
                    data.vm_poll_exp_date = dr["vm_poll_exp_date"].ToString();
                }
                data.vm_insurance = dr["vm_insurance"].ToString();
                if (dr["vm_isurence_exp_date"].ToString() != "" && dr["vm_isurence_exp_date"].ToString() != null)
                {
                    data.vm_isurence_exp_date = ((DateTime)dr["vm_isurence_exp_date"]).ToString("yyyy-MM-dd");
                }
                else
                {
                    data.vm_poll_exp_date = dr["vm_isurence_exp_date"].ToString();
                }
                data.vm_fitness = dr["vm_fitness"].ToString();
                if (dr["vm_fit_exp_date"].ToString() != "" && dr["vm_fit_exp_date"].ToString() != null)
                {
                    data.vm_fit_exp_date = ((DateTime)dr["vm_fit_exp_date"]).ToString("yyyy-MM-dd");
                }
                else
                {
                    data.vm_poll_exp_date = dr["vm_fit_exp_date"].ToString();
                }
                data.vm_roadtax = dr["vm_roadtax"].ToString();
                if (dr["vm_roadtax_exp_date"].ToString() != "" && dr["vm_roadtax_exp_date"].ToString() != null)
                {
                    data.vm_roadtax_exp_date = ((DateTime)dr["vm_roadtax_exp_date"]).ToString("yyyy-MM-dd");
                }
                else
                {
                    data.vm_poll_exp_date = dr["vm_roadtax_exp_date"].ToString();
                }

                vendor.Add(data);
            }
            string response = GetJson(vendor);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }

    public class getkms_from_to
    {
        public string sno { get; set; }
        public string Distance { get; set; }
    }

    private void get_kms_from_to(HttpContext context)
    {
        try
        {
            string fromlocation_val = context.Request["fromlocation_val"];
            string val_tolocation = context.Request["val_tolocation"];
            cmd = new MySqlCommand("SELECT sno, From_location, To_location, Distance, branch_id FROM location_distances WHERE (From_location = @From_location) AND (To_location = @To_location)");
            cmd.Parameters.Add("@From_location", fromlocation_val);
            cmd.Parameters.Add("@To_location", val_tolocation);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            List<getkms_from_to> vendor = new List<getkms_from_to>();
            foreach (DataRow dr in dt.Rows)
            {

                getkms_from_to data = new getkms_from_to();
                data.sno = dr["sno"].ToString();
                data.Distance = dr["Distance"].ToString();
                vendor.Add(data);
            }
            string response = GetJson(vendor);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }

    private void save_edit_Distances(HttpContext context)
    {
        try
        {
            string location_val = context.Request["location_val"];
            string btn_value = context.Request["btn_value"];
            List<Distance_items> Product_list = (List<Distance_items>)context.Session["load_Next_Distance"];
            if (btn_value == "Save Distances")
            {
                foreach (Distance_items art in Product_list)
                {
                    cmd = new MySqlCommand("insert into location_distances (From_location, To_location, Distance, branch_id) values (@From_location, @To_location, @Distance, @branch_id)");
                    cmd.Parameters.Add("@From_location", location_val);
                    cmd.Parameters.Add("@To_location", art.To_location);
                    cmd.Parameters.Add("@Distance", art.Distance);
                    cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
                    vdm.insert(cmd);
                }
                string response = GetJson("Distances Saved Successfully");
                context.Response.Write(response);
            }

        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }

    private void Distances_save_RowData(Distance_rowwise obj, HttpContext context)
    {
        try
        {

            Distance_items o = obj.row_detail;
            List<Distance_items> Product_list = (List<Distance_items>)context.Session["load_Next_Distance"];
            Product_list.Add(o);
            context.Session["load_Next_Distance"] = Product_list;
            string msg = obj.end;
            string response = GetJson(msg);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }
    public class Distance_rowwise
    {
        public string end { get; set; }
        public Distance_items row_detail { set; get; }
    }
    public class Distance_items
    {
        public string To_location { get; set; }
        public string Distance { get; set; }
    }
    private void generate_locations(HttpContext context)
    {
        try
        {
            string location_val = context.Request["location_val"];
            //cmd = new MySqlCommand("SELECT location_distances.sno AS Distance_Sno, location_distances.From_location, location_distances.To_location, location_distances.Distance, locations.sno AS Location_Sno, locations.Location_name,locations.branch_id FROM location_distances RIGHT OUTER JOIN locations ON location_distances.From_location = locations.sno WHERE (locations.sno <> @sno) AND (location_distances.Distance IS NULL)");
            cmd = new MySqlCommand("SELECT sno, Location_name, branch_id FROM locations WHERE (sno NOT IN (SELECT To_location FROM location_distances WHERE (Distance > 0) AND (From_location = @fromlocation))) AND (sno <> @fromlocation)");
            cmd.Parameters.Add("@fromlocation", location_val);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            List<all_locations> vendor = new List<all_locations>();
            foreach (DataRow dr in dt.Rows)
            {

                all_locations data = new all_locations();
                data.sno = dr["sno"].ToString();
                data.Location_name = dr["Location_name"].ToString();
                data.branch_id = dr["branch_id"].ToString();
                vendor.Add(data);

            }
            string response = GetJson(vendor);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            if (ex.ToString().Contains("PRIMARY"))
            {
                string response = GetJson("This Location Already Existed");
                context.Response.Write(response);
            }
            else
            {
                string response = GetJson(ex.ToString());
                context.Response.Write(response);
            }
        }
    }

    public class all_locations
    {
        public string sno { get; set; }
        public string Location_name { get; set; }
        public string branch_id { get; set; }
    }

    private void retrive_all_location(HttpContext context)
    {
        try
        {
            cmd = new MySqlCommand("SELECT sno, Location_name, branch_id FROM locations");
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            List<all_locations> vendor = new List<all_locations>();
            foreach (DataRow dr in dt.Rows)
            {
                all_locations data = new all_locations();
                data.sno = dr["sno"].ToString();
                data.Location_name = dr["Location_name"].ToString();
                data.branch_id = dr["branch_id"].ToString();
                vendor.Add(data);
            }
            string response = GetJson(vendor);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            if (ex.ToString().Contains("PRIMARY"))
            {
                string response = GetJson("This Location Already Existed");
                context.Response.Write(response);
            }
            else
            {
                string response = GetJson(ex.ToString());
                context.Response.Write(response);
            }
        }
    }

    private void edit_save_location(HttpContext context)
    {
        try
        {
            string locationname = context.Request["locationname"];
            string btnval = context.Request["btnval"];
            string sno = context.Request["sno"];
            if (btnval == "Save")
            {
                cmd = new MySqlCommand("insert into locations (Location_name,branch_id) values (@Location_name,@branch_id)");
                cmd.Parameters.Add("@Location_name", locationname);
                cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
                vdm.insert(cmd);
                string response = GetJson("Location Successfully Added");
                context.Response.Write(response);
            }
            else
            {
                cmd = new MySqlCommand("update locations set Location_name=@Location_name where sno=@sno");
                cmd.Parameters.Add("@Location_name", locationname);
                cmd.Parameters.Add("@sno", sno);
                vdm.Update(cmd);
                string response = GetJson("Location Successfully Modified");
                context.Response.Write(response);
            }

        }
        catch (Exception ex)
        {
            if (ex.ToString().Contains("PRIMARY"))
            {
                string response = GetJson("This Location Already Existed");
                context.Response.Write(response);
            }
            else
            {
                string response = GetJson(ex.ToString());
                context.Response.Write(response);
            }
        }
    }

    public class get_tyre_using_position_cls
    {
        public string Sno { get; set; }
        public string tyre_sno { get; set; }
        public string vehicle_mstr_sno { get; set; }
        public string v_ty_axles_names_sno { get; set; }
        public string Odometer { get; set; }
        public string current_KMS { get; set; }
    }

    private void get_tyre_using_position(HttpContext context)
    {
        try
        {
            string tyreposition = context.Request["tyreposition"];
            string vehiclenumber = context.Request["vehiclenumber"];
            string axlenumber = context.Request["axlenumber"];
            cmd = new MySqlCommand("SELECT vehicle_master_sub.Sno, vehicle_master_sub.vehicle_mstr_sno, vehicle_master_sub.Odometer, new_tyres_sub.tyre_sno AS TyreNumber, new_tyres_sub.current_KMS, new_tyres_sub.Fitting_Type,new_tyres_sub.sno AS tyre_sno FROM vehicle_master_sub INNER JOIN new_tyres_sub ON vehicle_master_sub.tyre_sno = new_tyres_sub.sno WHERE (vehicle_master_sub.vehicle_mstr_sno = @vehicle_mstr_sno) AND (vehicle_master_sub.axles_tyres_names_sno = @axles_tyres_names_sno) AND  (new_tyres_sub.Fitting_Type = 'F' OR new_tyres_sub.Fitting_Type = 'S')");
            cmd.Parameters.Add("@axles_tyres_names_sno", tyreposition);
            cmd.Parameters.Add("@vehicle_mstr_sno", vehiclenumber);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            List<get_tyre_using_position_cls> vendor = new List<get_tyre_using_position_cls>();
            foreach (DataRow dr in dt.Rows)
            {
                get_tyre_using_position_cls data = new get_tyre_using_position_cls();
                data.Sno = dr["tyre_sno"].ToString();
                data.tyre_sno = dr["TyreNumber"].ToString();
                data.vehicle_mstr_sno = dr["vehicle_mstr_sno"].ToString();
                data.Odometer = dr["Odometer"].ToString();
                data.current_KMS = dr["current_KMS"].ToString();
                vendor.Add(data);
            }
            string response = GetJson(vendor);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }

    public class axil_from_tyre
    {
        public string sno { get; set; }
        public string axlename { get; set; }
    }

    private void get_axils_for_vehicles(HttpContext context)
    {
        try
        {
            string vehicle_sno = context.Request["vehicle_sno"];
            cmd = new MySqlCommand("SELECT axlmstr_sub_axilnames.axlename, vehicel_master.vm_sno, vehicel_master.branch_id,axlmstr_sub_axilnames.axil_mstr_refno,axlmstr_sub_axilnames.sno FROM axlmstr_sub_axilnames INNER JOIN vehicel_master ON axlmstr_sub_axilnames.axil_mstr_refno = vehicel_master.axils_refno WHERE (vehicel_master.vm_sno = @vm_sno) AND (vehicel_master.branch_id = @branch_id)");
            cmd.Parameters.Add("@vm_sno", vehicle_sno);
            cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            DataView view = new DataView(dt);
            DataTable distinctValues = view.ToTable(true, "sno", "axlename");
            List<axil_from_tyre> vendor = new List<axil_from_tyre>();
            foreach (DataRow dr in distinctValues.Rows)
            {
                axil_from_tyre data = new axil_from_tyre();
                data.sno = dr["sno"].ToString();
                data.axlename = dr["axlename"].ToString();
                vendor.Add(data);
            }
            string response = GetJson(vendor);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }

    public class get_only_tyre
    {
        public string sno { get; set; }
        public string tyre_sno { get; set; }
        public string status { get; set; }
    }

    private void get_only_tyre_data(HttpContext context)
    {
        try
        {

            cmd = new MySqlCommand("SELECT new_tyres_sub.tyre_sno, new_tyres_sub.sno, new_tyres_sub.status, new_tyres_sub.Fitting_Type FROM new_tyres INNER JOIN new_tyres_sub ON new_tyres.sno = new_tyres_sub.newtyre_refno WHERE (new_tyres.branch_id = @branch_id) AND (NOT (new_tyres_sub.Fitting_Type = 'F'))");
            cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            List<get_only_tyre> vendor = new List<get_only_tyre>();
            foreach (DataRow dr in dt.Rows)
            {
                get_only_tyre data = new get_only_tyre();
                data.sno = dr["sno"].ToString();
                data.tyre_sno = dr["tyre_sno"].ToString();
                data.status = dr["status"].ToString();
                vendor.Add(data);
            }
            string response = GetJson(vendor);
            context.Response.Write(response);

        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }

    private void get_rackansstockandunitcost_data(HttpContext context)
    {
        try
        {
            string partno_sno = context.Request["partno_sno"];
            cmd = new MySqlCommand("SELECT minimasters.mm_name AS Rake, partnum.availble_qty, inwarddata_sub.per_unit_cost FROM partnum INNER JOIN minimasters ON partnum.RakeNum = minimasters.sno INNER JOIN inwarddata_sub ON partnum.sno = inwarddata_sub.partnum_sno WHERE (partnum.sno = @sno)");
            cmd.Parameters.Add("@sno", partno_sno);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            List<get_rackandstock> vendor = new List<get_rackandstock>();
            foreach (DataRow dr in dt.Rows)
            {
                get_rackandstock data = new get_rackandstock();
                data.rack = dr["Rake"].ToString();
                data.availble_qty = dr["availble_qty"].ToString();
                data.perunitcost = dr["per_unit_cost"].ToString();
                vendor.Add(data);
            }
            string response = GetJson(vendor);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }

    //private void save_edit_Outward(HttpContext context)
    //{
    //    try
    //    {
    //        List<Outward_items> Product_list = (List<Outward_items>)context.Session["load_Next_Outward"];
    //        double refno = 0.0;
    //        string outwardno = context.Request["outwardno"];
    //        string workorderno = context.Request["workorderno"];
    //        string workordersno = context.Request["workordersno"];
    //        string vehicle_sno = context.Request["vehicle_sno"];
    //        string assign_date = context.Request["assign_date"];
    //        if (assign_date != "" && assign_date != null)
    //        {
    //            DateTime ass_date = DateTime.ParseExact(assign_date, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
    //        }
    //        else
    //        {
    //            DateTime ass_date = DateTime.Parse("01/01/1990 12:00:00 AM", System.Globalization.CultureInfo.InvariantCulture);
    //        }
    //        string assignedto = context.Request["assignedto"];
    //        string V_E_Name = context.Request["V_E_Name"];
    //        string Status = context.Request["Status"];
    //        string vehiclename_sno = context.Request["vehiclename_sno"];
    //        string branch_sno = context.Request["branch_sno"];
    //        string type = context.Request["type"];
    //        string btnval = context.Request["btnval"];
    //        string mainremarks = context.Request["mainremarks"];
    //        string vend_r_emp = context.Request["vend_r_emp"];
    //        if (btnval == "Save")
    //        {
    //            cmd = new MySqlCommand("insert into outward (workorder_sno, vehicle_sno, outward_dt, outward_type, authorized_person, Remarks, outward_serial, transfer_branch_id, branch_id, operatedby) values (@workorder_sno, @vehicle_sno, @outward_dt, @outward_type, @authorized_person, @Remarks, @outward_serial, @transfer_branch_id, @branch_id, @operatedby)");
    //            cmd.Parameters.Add("@workorder_sno", workordersno);
    //            cmd.Parameters.Add("@vehicle_sno", vehicle_sno);
    //            cmd.Parameters.Add("@outward_dt", DateTime.Now);
    //            cmd.Parameters.Add("@outward_type", type);
    //            cmd.Parameters.Add("@authorized_person", vend_r_emp);
    //            cmd.Parameters.Add("@Remarks", mainremarks);
    //            cmd.Parameters.Add("@outward_serial", outwardno);
    //            cmd.Parameters.Add("@transfer_branch_id", branch_sno);
    //            cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
    //            cmd.Parameters.Add("@operatedby", context.Session["Employ_Sno"]);
    //            refno = vdm.insertScalar(cmd);
    //            foreach (Outward_items art in Product_list)
    //            {
    //                cmd = new MySqlCommand("insert into outward_subtable (ow_sno, partnum_sno, wo_partnum_sno, outward_qty, outward_cost, outward_total, outward_pr_qty, outward_pr_status) values (@ow_sno, @partnum_sno, @wo_partnum_sno, @outward_qty, @outward_cost, @outward_total, @outward_pr_qty, @outward_pr_status)");
    //                cmd.Parameters.Add("@ow_sno", refno);
    //                cmd.Parameters.Add("@partnum_sno", art.partnumno_sno);
    //                cmd.Parameters.Add("@wo_partnum_sno", art.wo_part_sno);
    //                cmd.Parameters.Add("@outward_qty", art.issued_qty);
    //                cmd.Parameters.Add("@outward_cost", art.per_unit_cost);
    //                double total = 0.0;
    //                total = art.issued_qty * art.per_unit_cost;
    //                cmd.Parameters.Add("@outward_total", total);
    //                cmd.Parameters.Add("@outward_pr_qty", art.return_qty);
    //                cmd.Parameters.Add("@outward_pr_status", art.return_status);
    //                vdm.insert(cmd);
    //            }
    //            int outward = 0;
    //            int.TryParse(outwardno, out outward);
    //            outward = outward + 1;
    //            cmd = new MySqlCommand("update branch_info set brnch_outward_start=@brnch_outward_start where brnch_sno=@brnch_sno");
    //            cmd.Parameters.Add("@brnch_outward_start", outward);
    //            cmd.Parameters.Add("@brnch_sno", context.Session["Branch_ID"]);
    //            vdm.Update(cmd);
    //        }
    //        string response = GetJson("Outward Done Successfully");
    //        context.Response.Write(response);
    //    }
    //    catch (Exception ex)
    //    {
    //        string response = GetJson(ex.Message);
    //        context.Response.Write(response);
    //    }
    //}

    public class Outward_rowwise
    {
        public string end { get; set; }
        public Outward_items row_detail { set; get; }
    }
    public class Outward_items
    {
        public string partnumno_sno { get; set; }
        public string rake { get; set; }
        public string wrkorder_qty { get; set; }
        public string avail_qty { get; set; }
        public int issued_qty { get; set; }
        public string return_qty { get; set; }
        public string return_status { get; set; }
        public string remarks { get; set; }
        public double per_unit_cost { get; set; }
        public string wo_part_sno { get; set; }

    }
    //private void Outward_save_RowData(Outward_rowwise obj, HttpContext context)
    //{
    //    try
    //    {
    //        Outward_items o = obj.row_detail;
    //        List<Outward_items> Product_list = (List<Outward_items>)context.Session["load_Next_Outward"];
    //        Product_list.Add(o);
    //        context.Session["load_Next_Outward"] = Product_list;
    //        string msg = obj.end;
    //        string response = GetJson(msg);
    //        context.Response.Write(response);
    //    }
    //    catch (Exception ex)
    //    {
    //        string response = GetJson(ex.Message);
    //        context.Response.Write(response);
    //    }
    //}

    public class get_all_nos
    {
        public string brnch_inward_start { get; set; }
        public string brnch_outward_start { get; set; }
        public string brnch_workorder_start { get; set; }
    }


    private void get_allin_no(HttpContext context)
    {
        try
        {
            cmd = new MySqlCommand("SELECT brnch_sno, user_id, branchname, brnch_address, brnch_mobno, brnch_email, brnch_status, brnch_inward_start, brnch_outward_start, brnch_workorder_start FROM branch_info WHERE (brnch_sno = @brnch_sno)");
            cmd.Parameters.Add("@brnch_sno", context.Session["Branch_ID"]);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            List<get_all_nos> vehtype = new List<get_all_nos>();
            foreach (DataRow dr in dt.Rows)
            {
                get_all_nos getvehtype = new get_all_nos();
                getvehtype.brnch_inward_start = dr["brnch_inward_start"].ToString();
                getvehtype.brnch_outward_start = dr["brnch_outward_start"].ToString();
                getvehtype.brnch_workorder_start = dr["brnch_workorder_start"].ToString();
                vehtype.Add(getvehtype);
            }
            string response = GetJson(vehtype);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.ToString());
            context.Response.Write(response);
        }
    }

    public class axil_data
    {
        public string AxileName { get; set; }
        public string nooftyresperaxle { get; set; }
        public string veh_typ_axel_sno { get; set; }
        public List<tyre_data> tyredata = new List<tyre_data>();
    }
    public class tyre_data
    {
        public string side { get; set; }
        public string tyre_name { get; set; }
        public string tyre_size { get; set; }
        public string tyresize { get; set; }
        public string tyre_position_sno { get; set; }
        public string Tyre_Name { get; set; }
        public string Brand { get; set; }
        public string SVDSNo { get; set; }
        public string grove { get; set; }


        public string currentkms { get; set; }
    }

    private void get_all_data_Axils(HttpContext context)
    {
        try
        {
            string sno = context.Request["sno"];
            cmd = new MySqlCommand("SELECT axlmstr_sub_axilnames.axlename, axlmstr_sub_axilnames.nooftyresperaxle, axlmstr_sub_axilnames.ranking, axils_tyres_names.sno AS tyre_position_sno, axils_tyres_names.axilnms_refno,axils_tyres_names.axleside, axils_tyres_names.tyrename, axils_tyres_names.tyre_size_sno, axlmstr_sub_axilnames.axil_mstr_refno, minimasters.mm_name AS tyresize FROM axlmstr_sub_axilnames INNER JOIN axils_tyres_names ON axlmstr_sub_axilnames.sno = axils_tyres_names.axilnms_refno INNER JOIN minimasters ON axils_tyres_names.tyre_size_sno = minimasters.sno WHERE (axlmstr_sub_axilnames.axil_mstr_refno = @sno)");
            cmd.Parameters.Add("@sno", sno);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            DataView view = new DataView(dt);
            DataTable Axil_Tbl = view.ToTable(true, "axilnms_refno", "axlename", "nooftyresperaxle");
            List<axil_data> veh_typ_data = new List<axil_data>();
            foreach (DataRow dr in Axil_Tbl.Rows)
            {
                axil_data data = new axil_data();
                data.AxileName = dr["axlename"].ToString();
                data.nooftyresperaxle = dr["nooftyresperaxle"].ToString();
                data.veh_typ_axel_sno = dr["axilnms_refno"].ToString();
                DataRow[] tyredata = dt.Select("axilnms_refno='" + dr["axilnms_refno"].ToString() + "'");

                foreach (DataRow tyre in tyredata)
                {
                    tyre_data tydata = new tyre_data();
                    tydata.side = tyre["axleside"].ToString();
                    tydata.tyre_name = tyre["tyrename"].ToString();
                    tydata.tyre_size = tyre["tyre_size_sno"].ToString();
                    tydata.tyresize = tyre["tyresize"].ToString();
                    tydata.tyre_position_sno = tyre["tyre_position_sno"].ToString();
                    data.tyredata.Add(tydata);
                }
                veh_typ_data.Add(data);
            }
            string response = GetJson(veh_typ_data);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }

    public class get_only_vehtype
    {
        public string sno { get; set; }
        public string v_ty_name { get; set; }
        public string v_ty_make { get; set; }
        public string no_of_axles { get; set; }
        public string v_ty_status { get; set; }
        public string axilmaster_name { get; set; }
    }


    private void get_only_axilmaster(HttpContext context)
    {
        try
        {
            cmd = new MySqlCommand("SELECT axil_master.sno, axil_master.axilmaster_name, axil_master.vhmake, axil_master.vhtype, axil_master.no_of_axles, axil_master.v_ty_status, axil_master.branch_id, axil_master.operated_by,minimasters.mm_name AS Veh_Type, minimasters_1.mm_name AS Veh_Make FROM axil_master INNER JOIN minimasters ON axil_master.vhtype = minimasters.sno INNER JOIN minimasters minimasters_1 ON axil_master.vhmake = minimasters_1.sno ");
            //cmd.Parameters.Add("@operated_by", context.Session["Employ_Sno"]);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            List<get_only_vehtype> vehtype = new List<get_only_vehtype>();
            foreach (DataRow dr in dt.Rows)
            {
                get_only_vehtype getvehtype = new get_only_vehtype();
                getvehtype.sno = dr["sno"].ToString();
                getvehtype.v_ty_name = dr["Veh_Type"].ToString();
                getvehtype.v_ty_make = dr["Veh_Make"].ToString();
                getvehtype.no_of_axles = dr["no_of_axles"].ToString();
                getvehtype.v_ty_status = dr["v_ty_status"].ToString();
                getvehtype.axilmaster_name = dr["axilmaster_name"].ToString();
                vehtype.Add(getvehtype);
            }
            string response = GetJson(vehtype);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }

    private void save_edit_Axils(HttpContext context)
    {
        try
        {
            double refno = 0.0;
            string vehtype_name = context.Request["vehtype_name"];
            string vehmake_sno = context.Request["vehmake_sno"];
            string fuel_capacity = context.Request["fuel_capacity"];
            string no_axils = context.Request["no_axils"];
            string btnval = context.Request["btnval"];
            string sno = context.Request["sno"];
            string status = context.Request["status"];
            string axil_mstr_name = context.Request["axil_mstr_name"];
            List<Axils_items> Product_list = (List<Axils_items>)context.Session["load_Next_Axils"];
            if (btnval == "Save")
            {
                cmd = new MySqlCommand("insert into axil_master (axilmaster_name, vhmake, vhtype, no_of_axles, v_ty_status, branch_id, operated_by) values (@axilmaster_name, @vhmake, @vhtype, @no_of_axles, @v_ty_status, @branch_id, @operated_by)");
                cmd.Parameters.Add("@axilmaster_name", axil_mstr_name);
                cmd.Parameters.Add("@vhmake", vehmake_sno);
                cmd.Parameters.Add("@vhtype", vehtype_name);
                cmd.Parameters.Add("@no_of_axles", no_axils);
                cmd.Parameters.Add("@v_ty_status", status);
                cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
                cmd.Parameters.Add("@operated_by", context.Session["Employ_Sno"]);
                refno = vdm.insertScalar(cmd);
                foreach (Axils_items art in Product_list)
                {
                    cmd = new MySqlCommand("insert into axlmstr_sub_axilnames (axil_mstr_refno, axlename, nooftyresperaxle, ranking) values (@axil_mstr_refno, @axlename, @nooftyresperaxle, @ranking)");
                    cmd.Parameters.Add("@axil_mstr_refno", refno);
                    cmd.Parameters.Add("@axlename", art.axilname);
                    cmd.Parameters.Add("@ranking", art.ranking);
                    cmd.Parameters.Add("@nooftyresperaxle", art.noof_tyres);
                    double sub_refno = vdm.insertScalar(cmd);
                    foreach (Axils_sub_items obj in art.innertable_array)
                    {
                        cmd = new MySqlCommand("insert into axils_tyres_names (axilnms_refno, axleside, tyrename, tyre_size_sno) values (@axilnms_refno, @axleside, @tyrename, @tyre_size_sno) ");
                        cmd.Parameters.Add("@axilnms_refno", sub_refno);
                        cmd.Parameters.Add("@axleside", obj.Side);
                        cmd.Parameters.Add("@tyrename", obj.tyre_name);
                        cmd.Parameters.Add("@tyre_size_sno", obj.tyre_size_sno);
                        vdm.insert(cmd);
                    }
                }
                string response = GetJson("New Axil Master Succesfully Added");
                context.Response.Write(response);
            }
            else
            {
                cmd = new MySqlCommand("update axil_master set axilmaster_name=@axilmaster_name, vhmake=@vhmake, vhtype=@vhtype, no_of_axles=@no_of_axles, v_ty_status=@v_ty_status where sno=@sno");
                cmd.Parameters.Add("@axilmaster_name", axil_mstr_name);
                cmd.Parameters.Add("@vhmake", vehmake_sno);
                cmd.Parameters.Add("@vhtype", vehtype_name);
                cmd.Parameters.Add("@no_of_axles", no_axils);
                cmd.Parameters.Add("@v_ty_status", status);
                cmd.Parameters.Add("@sno", sno);
                vdm.Update(cmd);
                foreach (Axils_items art in Product_list)
                {
                    cmd = new MySqlCommand("update axlmstr_sub_axilnames set axlename=@axlename, nooftyresperaxle=@nooftyresperaxle where sno=@sno and axil_mstr_refno=@axil_mstr_refno");
                    cmd.Parameters.Add("@axil_mstr_refno", sno);
                    cmd.Parameters.Add("@axlename", art.axilname);
                    cmd.Parameters.Add("@nooftyresperaxle", art.noof_tyres);
                    cmd.Parameters.Add("@sno", art.axlesno);
                    vdm.Update(cmd);
                    foreach (Axils_sub_items obj in art.innertable_array)
                    {
                        cmd = new MySqlCommand("update axils_tyres_names set axilnms_refno=@axilnms_refno, axleside=@axleside, tyrename=@tyrename, tyre_size_sno=@tyre_size_sno where sno=@sno");
                        cmd.Parameters.Add("@axilnms_refno", art.axlesno);
                        cmd.Parameters.Add("@axleside", obj.Side);
                        cmd.Parameters.Add("@tyrename", obj.tyre_name);
                        cmd.Parameters.Add("@tyre_size_sno", obj.tyre_size_sno);
                        cmd.Parameters.Add("@sno", obj.tyre_position_sno);
                        vdm.Update(cmd);
                    }
                }
                string response = GetJson("Axil Master Name Succesfully Modified");
                context.Response.Write(response);
            }
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }

    private void Axils_save_RowData(Axils_rowwise obj, HttpContext context)
    {
        try
        {
            Axils_items o = obj.row_detail;
            List<Axils_items> Product_list = (List<Axils_items>)context.Session["load_Next_Axils"];
            Product_list.Add(o);
            context.Session["load_Next_Axils"] = Product_list;
            string msg = obj.end;
            string response = GetJson(msg);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }
    public class Axils_items
    {
        public string axilname { get; set; }
        public string noof_tyres { get; set; }
        public string axlesno { get; set; }
        public string ranking { get; set; }

        public List<Axils_sub_items> innertable_array = new List<Axils_sub_items>();
    }
    public class Axils_sub_items
    {
        public string tyre_name { get; set; }
        public string tyre_size_sno { get; set; }
        public string Side { get; set; }
        public string tyre_sno { get; set; }
        public string tyre_position_sno { get; set; }
    }
    public class Axils_rowwise
    {
        public string end { get; set; }
        public Axils_items row_detail { set; get; }
    }

    class get_workorder
    {
        public string workorder_num { get; set; }
        public string workorder_dt { get; set; }
        public string vehicle_id { get; set; }
        public string registration_no { get; set; }
        public string workorder_odometer { get; set; }
        public string workorder_enginehrs { get; set; }
        public string workorder_startdt { get; set; }
        public string workorder_sno { get; set; }
        public string employname { get; set; }
        public string vendorname { get; set; }
        public string workordr_dealedby { get; set; }
        public string workordr_vendor { get; set; }
        public string workorder_emp { get; set; }
        public List<get_workorder_parts> parts = new List<get_workorder_parts>();
        public string workorder_status { get; set; }
    }

    class get_workorder_parts
    {
        public string pnum_Name { get; set; }
        public string pn_name { get; set; }
        public string PartGroup { get; set; }
        public string availble_qty { get; set; }
        public string pn_sno { get; set; }
        public string wo_parts_part_sno { get; set; }
        public string RakeNum { get; set; }
        public string wo_parts_qty { get; set; }
        public string workorder_parts_sno { get; set; }
        public string per_unit_cost { get; set; }
    }
    //private void get_workorder_data_foroutward(HttpContext context)
    //{
    //    try
    //    {
    //        string wrk_order_id = context.Request["wrk_order_id"];
    //        //cmd = new MySqlCommand("SELECT     workorder_parts.wo_parts_qty, workorder.workorder_status,  workorder_parts.workorder_sno, workorder.workorder_num, workorder.workorder_dt, workorder.vehicle_id, vehicel_master.registration_no, workorder.workorder_odometer, workorder.workorder_enginehrs, workorder.workorder_startdt, partnum.pnum_Name, partname.pn_name, minimasters.mm_name AS PartGroup, employdata.employname, vendors_info.vendorname, workorder.workordr_dealedby, partnum.RakeNum, partnum.availble_qty, partnum.pn_sno, workorder_parts.wo_parts_part_sno, workorder.workordr_vendor, workorder.workorder_emp,workorder_parts.sno as workorder_parts_sno FROM workorder INNER JOIN workorder_parts ON workorder.sno = workorder_parts.workorder_sno INNER JOIN vehicel_master ON workorder.vehicle_id = vehicel_master.vm_sno INNER JOIN partnum ON workorder_parts.wo_parts_part_sno = partnum.sno INNER JOIN partname ON partnum.pn_sno = partname.sno INNER JOIN minimasters ON partnum.pg_sno = minimasters.sno LEFT OUTER JOIN vendors_info ON workorder.workordr_vendor = vendors_info.sno LEFT OUTER JOIN employdata ON workorder.workorder_emp = employdata.emp_sno WHERE (workorder.branch_id = @branch_id) AND (workorder.workorder_num = @workorder_num)");
    //        cmd = new MySqlCommand("SELECT workorder_parts.wo_parts_qty, workorder.workorder_status, workorder_parts.workorder_sno, workorder.workorder_num, workorder.workorder_dt, workorder.vehicle_id, vehicel_master.registration_no, workorder.workorder_odometer, workorder.workorder_enginehrs, workorder.workorder_startdt, partnum.pnum_Name, partname.pn_name, minimasters.mm_name AS PartGroup, employdata.employname, vendors_info.vendorname, workorder.workordr_dealedby, partnum.RakeNum, partnum.availble_qty, partnum.pn_sno, workorder_parts.wo_parts_part_sno, workorder.workordr_vendor, workorder.workorder_emp, workorder_parts.sno AS workorder_parts_sno, inwarddata_sub.per_unit_cost FROM workorder INNER JOIN workorder_parts ON workorder.sno = workorder_parts.workorder_sno INNER JOIN vehicel_master ON workorder.vehicle_id = vehicel_master.vm_sno INNER JOIN partnum ON workorder_parts.wo_parts_part_sno = partnum.sno INNER JOIN partname ON partnum.pn_sno = partname.sno INNER JOIN minimasters ON partnum.pg_sno = minimasters.sno INNER JOIN inwarddata_sub ON partnum.sno = inwarddata_sub.partnum_sno LEFT OUTER JOIN vendors_info ON workorder.workordr_vendor = vendors_info.sno LEFT OUTER JOIN employdata ON workorder.workorder_emp = employdata.emp_sno WHERE (workorder.branch_id = @branch_id) AND (workorder.workorder_num = @workorder_num)");
    //        cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
    //        cmd.Parameters.Add("@workorder_num", wrk_order_id);
    //        DataTable dt = vdm.SelectQuery(cmd).Tables[0];
    //        DataTable workorders = dt.DefaultView.ToTable(true, new string[] { "workorder_sno", "workordr_vendor", "workorder_emp", "workordr_dealedby", "workorder_num", "workorder_dt", "vehicle_id", "registration_no", "workorder_odometer", "workorder_enginehrs", "workorder_startdt", "vendorname", "employname", "workorder_status" });
    //        DataTable workorders_parts = dt.DefaultView.ToTable(true, new string[] { "workorder_sno", "pnum_Name", "pn_name", "PartGroup", "RakeNum", "availble_qty", "pn_sno", "wo_parts_part_sno", "wo_parts_qty", "workorder_parts_sno", "per_unit_cost" });
    //        List<get_workorder> vendor = new List<get_workorder>();
    //        get_workorder data = new get_workorder();
    //        foreach (DataRow dr in workorders.Rows)
    //        {
    //            data.workorder_sno = dr["workorder_sno"].ToString();
    //            data.workordr_vendor = dr["workordr_vendor"].ToString();
    //            data.workorder_emp = dr["workorder_emp"].ToString();
    //            data.workorder_num = dr["workorder_num"].ToString();
    //            data.workorder_dt = ((DateTime)dr["workorder_dt"]).ToString("dd/MM/yyyy");
    //            data.vehicle_id = dr["vehicle_id"].ToString();
    //            data.workorder_odometer = dr["workorder_odometer"].ToString();
    //            data.workorder_enginehrs = dr["workorder_enginehrs"].ToString();
    //            data.workorder_startdt = dr["workorder_startdt"].ToString();
    //            data.workordr_dealedby = dr["workordr_dealedby"].ToString();
    //            data.registration_no = dr["registration_no"].ToString();
    //            //employname, vendors_info.vendorname, workorder.workordr_dealedby,
    //            //partnum.RakeNum, partnum.availble_qty, partnum.pn_sno, workorder_parts.wo_parts_part_sno,
    //            //workorder.workordr_vendor, workorder.workorder_emp 
    //            data.employname = dr["employname"].ToString();
    //            data.vendorname = dr["vendorname"].ToString();
    //            data.workorder_status = dr["workorder_status"].ToString();
    //            DataRow[] workorderparts_wo = workorders_parts.Select("workorder_sno=" + dr["workorder_sno"].ToString());
    //            foreach (DataRow drr in workorderparts_wo)
    //            {
    //                get_workorder_parts gwp = new get_workorder_parts();
    //                //  PartGroup, pn_name, pnum_Name, RakeNum, availble_qty, pn_sno, wo_parts_part_sno 
    //                gwp.PartGroup = drr["PartGroup"].ToString();
    //                gwp.pnum_Name = drr["pnum_Name"].ToString();
    //                gwp.pn_name = drr["pn_name"].ToString();
    //                gwp.pn_sno = drr["pn_sno"].ToString();
    //                gwp.RakeNum = drr["RakeNum"].ToString();
    //                gwp.availble_qty = drr["availble_qty"].ToString();
    //                gwp.wo_parts_part_sno = drr["workorder_sno"].ToString();
    //                gwp.wo_parts_qty = drr["wo_parts_qty"].ToString();
    //                gwp.workorder_parts_sno = drr["workorder_parts_sno"].ToString();
    //                gwp.per_unit_cost = drr["per_unit_cost"].ToString();
    //                data.parts.Add(gwp);
    //            }
    //        }
    //        string response = GetJson(data);
    //        context.Response.Write(response);
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    public class get_issued_tyrescls
    {
        public string tyrename { get; set; }
        public string sno { get; set; }
    }
    private void get_issued_tyres(HttpContext context)
    {
        try
        {
            string vehiclenumber = context.Request["vehiclenumber"];
            string axlenumber = context.Request["axlenumber"];
            cmd = new MySqlCommand("SELECT axleside, tyrename, axilnms_refno, sno FROM axils_tyres_names WHERE (axilnms_refno = @axilnms_refno)");
            cmd.Parameters.Add("@axilnms_refno", axlenumber);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            List<get_issued_tyrescls> issued_tyrelst = new List<get_issued_tyrescls>();
            foreach (DataRow dr in dt.Rows)
            {
                get_issued_tyrescls issued_tyre = new get_issued_tyrescls();
                issued_tyre.tyrename = dr["tyrename"].ToString() + "  (" + dr["axleside"].ToString() + ")";
                issued_tyre.sno = dr["sno"].ToString();
                issued_tyrelst.Add(issued_tyre);
            }
            string response = GetJson(issued_tyrelst);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.ToString());
            context.Response.Write(response);
        }
    }

    public class workorder_data_cls
    {
        public string sno { get; set; }
        public string workorder_num { get; set; }
        public string workorder_dt { get; set; }
        public string vehicle_id { get; set; }
        public string workorder_odometer { get; set; }
        public string workorder_enginehrs { get; set; }
        public string workorder_startdt { get; set; }
        public string workorder_closed_date { get; set; }
        public string workorder_status { get; set; }
        public string workordr_vendor { get; set; }
        public string workorder_emp { get; set; }
        public List<workorder_maintanace> Maintanance = new List<workorder_maintanace>();
        public List<workorder_replaces> Replaces = new List<workorder_replaces>();
        public List<workorder_repairs> Repairs = new List<workorder_repairs>();
        public List<workorder_Inspecton> Inspections = new List<workorder_Inspecton>();
    }
    public class workorder_maintanace
    {
        public string RecurrenceName { get; set; }
        public string recurenttask_sno { get; set; }
        public string wo_mnt_desc { get; set; }
    }
    public class workorder_replaces
    {
        public string PartNumbSno { get; set; }
        public string PartNumber { get; set; }
        public string PartName { get; set; }
        public string PartGroup { get; set; }
        public string Quantity { get; set; }
        public string RepType { get; set; }
        public string UnitCost { get; set; }
        public string PrtsTotal { get; set; }
    }
    public class workorder_repairs
    {
        public string RepairName { get; set; }
        public string RepairSno { get; set; }
        public string RepairDesc { get; set; }
    }
    public class workorder_Inspecton
    {
        public string CheckupName { get; set; }
        public string InspectionSno { get; set; }
        public string InspectionDescription { get; set; }
    }

    //private void get_total_workorder_data(HttpContext context)
    //{
    //    try
    //    {
    //        string workorder_sno = context.Request["workorder_sno"];
    //        string vehicle_sno = context.Request["vehicle_sno"];
    //        cmd = new MySqlCommand("SELECT workorder.branch_id, workorder.operatedby, workorder.sno, workorder.workorder_num, workorder.workorder_dt, workorder.vehicle_id, workorder.workorder_odometer, workorder.workorder_enginehrs,workorder.workorder_startdt, workorder.workorder_closed_date, workorder.workorder_status, workorder.workordr_vendor, workorder.workorder_emp, workorder_maintainance.recurenttask_sno,workorder_maintainance.wo_mnt_desc, workorder_checkups.wo_checkupcheckup_desc AS InspectionDescription, workorder_repairs.repairs_sno AS RepairSno,workorder_repairs.wo_repair_remarks AS RepairDesc, workorder_parts.wo_parts_part_sno AS PartNumbSno, partnum.pnum_Name AS PartNumber, partname.pn_name AS PartName,minimasters.mm_name AS PartGroup, workorder_parts.wo_parts_qty AS Quantity, workorder_parts.wo_type AS RepType, workorder_parts.wo_parts_cost AS UnitCost, workorder_parts.wo_parts_total AS PrtsTotal,recurrent_group.recurrent_name AS RecurrenceName, minimasters_1.mm_name AS RepairName, workorder_checkups.checkups_sno as InspectionSno, minimasters_2.mm_name AS CheckupName FROM workorder_maintainance INNER JOIN workorder ON workorder_maintainance.workorder_sno = workorder.sno INNER JOIN workorder_checkups ON workorder.sno = workorder_checkups.workorder_sno INNER JOIN workorder_repairs ON workorder.sno = workorder_repairs.workorder_sno INNER JOIN workorder_parts ON workorder.sno = workorder_parts.workorder_sno INNER JOIN partnum ON workorder_parts.wo_parts_part_sno = partnum.sno INNER JOIN partname ON partnum.pn_sno = partname.sno INNER JOIN minimasters ON partnum.pg_sno = minimasters.sno INNER JOIN minimasters minimasters_1 ON workorder_repairs.repairs_sno = minimasters_1.sno INNER JOIN minimasters minimasters_2 ON workorder_checkups.checkups_sno = minimasters_2.sno INNER JOIN  recurrent_group ON workorder_maintainance.recurenttask_sno = recurrent_group.sno WHERE (workorder.branch_id = @branch_id) AND (workorder.operatedby = @operatedby) AND (workorder.sno = @sno)");
    //        cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
    //        cmd.Parameters.Add("@operatedby", context.Session["Employ_Sno"]);
    //        cmd.Parameters.Add("@sno", workorder_sno);
    //        DataTable dt = vdm.SelectQuery(cmd).Tables[0];
    //        List<workorder_data_cls> inward_items = new List<workorder_data_cls>();
    //        DataView view = new DataView(dt);
    //        DataTable distinctValues = view.ToTable(true, "sno", "workorder_num", "workorder_dt", "vehicle_id", "workorder_odometer", "workorder_enginehrs", "workorder_startdt", "workorder_closed_date", "workorder_status", "workordr_vendor", "workorder_emp");
    //        DataTable maintanace_dt = view.ToTable(true, "RecurrenceName", "recurenttask_sno", "wo_mnt_desc");
    //        DataTable replaces_dt = view.ToTable(true, "PartNumbSno", "PartNumber", "PartName", "PartGroup", "Quantity", "RepType", "UnitCost", "PrtsTotal");
    //        DataTable repairs_dt = view.ToTable(true, "RepairName", "RepairSno", "RepairDesc");
    //        DataTable inspection_dt = view.ToTable(true, "CheckupName", "InspectionSno", "InspectionDescription");
    //        workorder_data_cls data = new workorder_data_cls();
    //        foreach (DataRow dr in distinctValues.Rows)
    //        {
    //            //DataRow[] workorderdata = dt.Select("sno='" + dr["sno"].ToString() + "'");
    //            data.sno = dr["sno"].ToString();
    //            data.workorder_num = dr["workorder_num"].ToString();
    //            data.workorder_dt = ((DateTime)dr["workorder_dt"]).ToString("dd/MM/yyyy");
    //            data.vehicle_id = dr["vehicle_id"].ToString();
    //            data.workorder_odometer = dr["workorder_odometer"].ToString();
    //            data.workorder_enginehrs = dr["workorder_enginehrs"].ToString();
    //            data.workorder_startdt = ((DateTime)dr["workorder_startdt"]).ToString("dd/MM/yyyy");
    //            data.workorder_closed_date = ((DateTime)dr["workorder_closed_date"]).ToString("dd/MM/yyyy");
    //            data.workorder_status = dr["workorder_status"].ToString();
    //            data.workordr_vendor = dr["workordr_vendor"].ToString();
    //            data.workorder_emp = dr["workorder_emp"].ToString();

    //        }
    //        foreach (DataRow dr in maintanace_dt.Rows)
    //        {
    //            workorder_maintanace maintanace = new workorder_maintanace();
    //            maintanace.RecurrenceName = dr["RecurrenceName"].ToString();
    //            maintanace.recurenttask_sno = dr["recurenttask_sno"].ToString();
    //            maintanace.wo_mnt_desc = dr["wo_mnt_desc"].ToString();
    //            data.Maintanance.Add(maintanace);
    //        }
    //        foreach (DataRow dr in replaces_dt.Rows)
    //        {
    //            workorder_replaces replaces = new workorder_replaces();
    //            replaces.PartNumbSno = dr["PartNumbSno"].ToString();
    //            replaces.PartNumber = dr["PartNumber"].ToString();
    //            replaces.PartName = dr["PartName"].ToString();
    //            replaces.PartGroup = dr["PartGroup"].ToString();
    //            replaces.Quantity = dr["Quantity"].ToString();
    //            replaces.RepType = dr["RepType"].ToString();
    //            replaces.UnitCost = dr["UnitCost"].ToString();
    //            replaces.PrtsTotal = dr["PrtsTotal"].ToString();
    //            data.Replaces.Add(replaces);
    //        }
    //        foreach (DataRow dr in repairs_dt.Rows)
    //        {
    //            workorder_repairs repairs = new workorder_repairs();
    //            repairs.RepairName = dr["RepairName"].ToString();
    //            repairs.RepairSno = dr["RepairSno"].ToString();
    //            repairs.RepairDesc = dr["RepairDesc"].ToString();
    //            data.Repairs.Add(repairs);
    //        }
    //        foreach (DataRow dr in inspection_dt.Rows)
    //        {
    //            workorder_Inspecton inspection = new workorder_Inspecton();
    //            inspection.CheckupName = dr["CheckupName"].ToString();
    //            inspection.InspectionSno = dr["InspectionSno"].ToString();
    //            inspection.InspectionDescription = dr["InspectionDescription"].ToString();
    //            data.Inspections.Add(inspection);
    //        }
    //        inward_items.Add(data);
    //        string response = GetJson(inward_items);
    //        context.Response.Write(response);
    //    }
    //    catch (Exception ex)
    //    {
    //        string response = GetJson(ex.ToString());
    //        context.Response.Write(response);
    //    }
    //}
    public class forward_inward_data
    {
        public string sno { get; set; }
        public string workorder_num { get; set; }
        public string workorder_dt { get; set; }
        public string vehicle_id { get; set; }
        public string registration_no { get; set; }
        public string workorder_status { get; set; }
    }

    //private void get_workorder_data(HttpContext context)
    //{
    //    try
    //    {
    //        cmd = new MySqlCommand("SELECT workorder.sno, workorder.workorder_num, workorder.workorder_dt, workorder.vehicle_id, vehicel_master.registration_no, workorder.branch_id, workorder.operatedby, workorder.workorder_status FROM workorder INNER JOIN vehicel_master ON workorder.vehicle_id = vehicel_master.vm_sno WHERE (workorder.branch_id = @branch_id) AND (workorder.operatedby = @operatedby)");
    //        cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
    //        cmd.Parameters.Add("@operatedby", context.Session["Employ_Sno"]);
    //        DataTable dt = vdm.SelectQuery(cmd).Tables[0];
    //        List<forward_inward_data> inward = new List<forward_inward_data>();
    //        foreach (DataRow dr in dt.Rows)
    //        {
    //            forward_inward_data data = new forward_inward_data();
    //            data.sno = dr["sno"].ToString();
    //            data.workorder_num = dr["workorder_num"].ToString();
    //            data.workorder_dt = ((DateTime)dr["workorder_dt"]).ToString("yyyy-MM-dd");
    //            data.vehicle_id = dr["vehicle_id"].ToString();
    //            data.registration_no = dr["registration_no"].ToString();
    //            data.workorder_status = dr["workorder_status"].ToString();
    //            inward.Add(data);
    //        }
    //        string response = GetJson(inward);
    //        context.Response.Write(response);
    //    }
    //    catch (Exception ex)
    //    {
    //        string response = GetJson(ex.ToString());
    //        context.Response.Write(response);
    //    }

    //}
    public class getalldataforissuetyrecls
    {
        public List<vehiclescls> vehicles { get; set; }
        public List<get_all_tyre_datacls> tyres { get; set; }
    }

    private void getalldataforissuetyre(HttpContext context)
    {
        try
        {
            getalldataforissuetyrecls getalldataforissuetyreclslst = new getalldataforissuetyrecls();
            List<vehiclescls> getvehicleslst = new List<vehiclescls>();
            cmd = new MySqlCommand("SELECT vm_sno, registration_no FROM vehicel_master WHERE (status = true) and (branch_id = @branch_id) AND (axils_refno IS NOT NULL) AND (NOT (axils_refno = ''))");
            cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
            DataTable vehicles = vdm.SelectQuery(cmd).Tables[0];
            foreach (DataRow dr in vehicles.Rows)
            {
                vehiclescls vehicle = new vehiclescls();
                vehicle.vehiclenum = dr["registration_no"].ToString();
                vehicle.vehiclesno = dr["vm_sno"].ToString();
                getvehicleslst.Add(vehicle);
            }
            getalldataforissuetyreclslst.vehicles = getvehicleslst;
            string response = GetJson(getalldataforissuetyreclslst);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }

    private void getalldataforissuetyre_tyres(HttpContext context)
    {
        try
        {
            getalldataforissuetyrecls getalldataforissuetyreclslst = new getalldataforissuetyrecls();
            List<get_all_tyre_datacls> get_all_tyre_dataclslst = new List<get_all_tyre_datacls>();
            cmd = new MySqlCommand("SELECT new_tyres_sub.tyre_sno, new_tyres_sub.sno, new_tyres_sub.current_KMS, new_tyres_sub.Fitting_Type FROM new_tyres_sub INNER JOIN new_tyres ON new_tyres_sub.newtyre_refno = new_tyres.sno WHERE (new_tyres.branch_id = @branch_id) AND (new_tyres_sub.Fitting_Type = 'R') OR (new_tyres_sub.Fitting_Type = 'NF')");
            //cmd = new MySqlCommand("SELECT new_tyres_sub.tyre_sno, new_tyres_sub.sno, new_tyres_sub.current_KMS, new_tyres_sub.Fitting_Type, vehicle_master_sub.Odometer FROM new_tyres_sub INNER JOIN new_tyres ON new_tyres_sub.newtyre_refno = new_tyres.sno INNER JOIN vehicle_master_sub ON new_tyres_sub.sno = vehicle_master_sub.prev_fit_sno WHERE (new_tyres.branch_id = @branch_id) AND (new_tyres_sub.Fitting_Type = 'R')");
            cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
            DataTable tyrestable = vdm.SelectQuery(cmd).Tables[0];
            foreach (DataRow dr in tyrestable.Rows)
            {
                get_all_tyre_datacls get_all_tyredata = new get_all_tyre_datacls();
                get_all_tyredata.sno = dr["sno"].ToString();
                get_all_tyredata.tyre_sno = dr["tyre_sno"].ToString();
                get_all_tyredata.current_KMS = dr["current_KMS"].ToString();
                get_all_tyre_dataclslst.Add(get_all_tyredata);
            }
            getalldataforissuetyreclslst.tyres = get_all_tyre_dataclslst;
            string response = GetJson(getalldataforissuetyreclslst);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }

    public class get_all_tyre_datacls
    {
        public string sno { get; set; }
        public string tyre_sno { get; set; }
        public string brand { get; set; }
        public string model { get; set; }
        public string size { get; set; }
        public string load_value { get; set; }
        public string type { get; set; }
        public string status { get; set; }
        public string vendor { get; set; }
        public string invoice { get; set; }
        public string price { get; set; }
        public string purchase_date { get; set; }
        public string Total_kms_hrs { get; set; }
        public string totalservicecost { get; set; }
        public string costper_km_hrs { get; set; }
        public string undrwaranty { get; set; }
        public string days { get; set; }
        public string kms { get; set; }
        public string current_KMS { get; set; }
        public string branch_id { get; set; }
        public string btnval { get; set; }
        public string Odometer { get; set; }
    }

    private void for_save_edit_fittedtyre(HttpContext context)
    {
        try
        {
            string fitteddate = context.Request["fitteddate"];
            string issuedtyresno = context.Request["issuedtyresno"];
            string issuedtyretype = context.Request["issuedtyretype"];
            string fittedodometer = context.Request["fittedodometer"];
            string axlenumber = context.Request["axlenumber"];
            string vehiclenumber = context.Request["vehiclenumber"];
            string tyreposition = context.Request["tyreposition"];
            string fitremarks = context.Request["fitremarks"];
            string fitkms = context.Request["fitkms"];
            string axilname = context.Request["axilname"];
            if (fitkms == "")
            {
                fitkms = "0";
            }
            DateTime fitted_date = DateTime.Now;// ParseExact(fitteddate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            cmd = new MySqlCommand("insert into tyre_changes (vehicle_sno, tyre_master_sno, tyre_type, transaction_date, veh_odometer, tyre_axleno, kms, remarks, branch_id, operatedby, Fitting_Type,tyre_position) values (@vehicle_sno, @tyre_master_sno, @tyre_type, @transaction_date, @veh_odometer, @tyre_axleno, @kms, @remarks, @branch_id, @operatedby, @Fitting_Type,@tyre_position)");
            cmd.Parameters.Add("@vehicle_sno", vehiclenumber);
            cmd.Parameters.Add("@tyre_master_sno", issuedtyresno);
            cmd.Parameters.Add("@tyre_type", issuedtyretype);
            cmd.Parameters.Add("@transaction_date", fitted_date);
            cmd.Parameters.Add("@veh_odometer", fittedodometer);
            cmd.Parameters.Add("@tyre_axleno", axlenumber);
            cmd.Parameters.Add("@tyre_position", tyreposition);
            cmd.Parameters.Add("@kms", fitkms);
            cmd.Parameters.Add("@remarks", fitremarks);
            cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
            cmd.Parameters.Add("@operatedby", context.Session["Employ_Sno"]);
            if (axilname == "Stephanie")
            {
                cmd.Parameters.Add("@Fitting_Type", "S");
            }
            else
            {
                cmd.Parameters.Add("@Fitting_Type", "F");
            }
            vdm.insert(cmd);
            if (axilname != "Stephanie")
            {
                cmd = new MySqlCommand("update new_tyres_sub set Fitting_Type='F' where sno=@sno");
                cmd.Parameters.Add("@sno", issuedtyresno);
                vdm.Update(cmd);
            }
            else
            {
                cmd = new MySqlCommand("update new_tyres_sub set Fitting_Type='S' where sno=@sno");
                cmd.Parameters.Add("@sno", issuedtyresno);
                vdm.Update(cmd);
            }
            cmd = new MySqlCommand("update vehicle_master_sub set Odometer=@Odometer,tyre_sno=@tyre_sno where vehicle_mstr_sno=@vehicle_mstr_sno and axles_tyres_names_sno=@axles_tyres_names_sno");
            cmd.Parameters.Add("@Odometer", fittedodometer);
            cmd.Parameters.Add("@vehicle_mstr_sno", vehiclenumber);
            cmd.Parameters.Add("@axles_tyres_names_sno", tyreposition);
            cmd.Parameters.Add("@tyre_sno", issuedtyresno);
            vdm.Update(cmd);
            string response = GetJson("OK");
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }

    private void for_save_edit_removedtyre(HttpContext context)
    {
        try
        {
            //string issueddate = DateTime.Now;
            string removedtyresno = context.Request["removedtyresno"];
            string removedtyretype = context.Request["removedtyretype"];
            string Odometerreading = context.Request["Odometerreading"];
            string axlenumber = context.Request["axlenumber"];
            string vehiclenumber = context.Request["vehiclenumber"];
            string tyreposition = context.Request["tyreposition"];
            string remarks = context.Request["remarks"];
            string kms = context.Request["kms"];
            string totalkms = context.Request["totalkms"];
            string axilname = context.Request["axilname"];
            if (kms == "")
            {
                kms = "0";
            }
            if (totalkms == "")
            {
                totalkms = "0";
            }
            DateTime issued_date = DateTime.Now;
            cmd = new MySqlCommand("insert into tyre_changes (vehicle_sno, tyre_master_sno, tyre_type, transaction_date, veh_odometer, tyre_axleno, kms, remarks, branch_id, operatedby, Fitting_Type,tyre_position) values (@vehicle_sno, @tyre_master_sno, @tyre_type, @transaction_date, @veh_odometer, @tyre_axleno, @kms, @remarks, @branch_id, @operatedby, @Fitting_Type,@tyre_position)");
            cmd.Parameters.Add("@vehicle_sno", vehiclenumber);
            cmd.Parameters.Add("@tyre_master_sno", removedtyresno);
            cmd.Parameters.Add("@tyre_type", removedtyretype);
            cmd.Parameters.Add("@transaction_date", issued_date);
            cmd.Parameters.Add("@veh_odometer", Odometerreading);
            cmd.Parameters.Add("@tyre_axleno", axlenumber);
            cmd.Parameters.Add("@tyre_position", tyreposition);
            cmd.Parameters.Add("@kms", kms);
            cmd.Parameters.Add("@remarks", remarks);
            cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
            cmd.Parameters.Add("@operatedby", context.Session["Employ_Sno"]);
            cmd.Parameters.Add("@Fitting_Type", "R");
            vdm.insert(cmd);
            cmd = new MySqlCommand("update new_tyres_sub set Fitting_Type='R' where sno=@sno");
            cmd.Parameters.Add("@sno", removedtyresno);
            vdm.Update(cmd);
            cmd = new MySqlCommand("update vehicle_master_sub set Odometer=@Odometer,tyre_sno=NULL where vehicle_mstr_sno=@vehicle_mstr_sno and axles_tyres_names_sno=@axles_tyres_names_sno and tyre_sno=@tyre_sno");
            cmd.Parameters.Add("@Odometer", Odometerreading);
            cmd.Parameters.Add("@vehicle_mstr_sno", vehiclenumber);
            cmd.Parameters.Add("@axles_tyres_names_sno", tyreposition);
            cmd.Parameters.Add("@tyre_sno", removedtyresno);
            vdm.Update(cmd);
            string response = GetJson("OK");
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }

    private void for_save_edit_tyre(get_all_tyre_datacls obj, HttpContext context)
    {
        try
        {
            if (obj.btnval == "Save")
            {
                DateTime purchase_date = DateTime.ParseExact(obj.purchase_date, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                cmd = new MySqlCommand("insert into tyres_master (tyre_sno, brand, model, size, load_value, type, status, vendor, invoice, price, purchase_date, Total_kms_hrs, totalservicecost, costper_km_hrs, undrwaranty, days, kms, branch_id, operatedby,Fitting_Type) values (@tyre_sno, @brand, @model, @size, @load_value, @type, @status, @vendor, @invoice, @price, @purchase_date, @Total_kms_hrs, @totalservicecost, @costper_km_hrs, @undrwaranty, @days, @kms, @branch_id, @operatedby,'R')");
                cmd.Parameters.Add("@tyre_sno", obj.tyre_sno);
                cmd.Parameters.Add("@brand", obj.brand);
                cmd.Parameters.Add("@model", obj.model);
                cmd.Parameters.Add("@size", obj.size);
                cmd.Parameters.Add("@load_value", obj.load_value);
                cmd.Parameters.Add("@type", obj.type);
                cmd.Parameters.Add("@status", obj.status);
                cmd.Parameters.Add("@vendor", obj.vendor);
                cmd.Parameters.Add("@invoice", obj.invoice);
                cmd.Parameters.Add("@price", obj.price);
                cmd.Parameters.Add("@purchase_date", purchase_date);
                cmd.Parameters.Add("@Total_kms_hrs", obj.Total_kms_hrs);
                cmd.Parameters.Add("@totalservicecost", obj.totalservicecost);
                cmd.Parameters.Add("@costper_km_hrs", obj.costper_km_hrs);
                cmd.Parameters.Add("@undrwaranty", obj.undrwaranty);
                cmd.Parameters.Add("@days", obj.days);
                cmd.Parameters.Add("@kms", obj.kms);
                cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
                cmd.Parameters.Add("@operatedby", context.Session["Employ_Sno"]);
                vdm.insert(cmd);
                string response = GetJson("OK");
                context.Response.Write(response);
            }
            else
            {
                DateTime purchase_date = DateTime.ParseExact(obj.purchase_date, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                cmd = new MySqlCommand("update tyres_master set tyre_sno=@tyre_sno, brand=@brand, model=@model, size=@size, load_value=@load_value, type=@type, status=@status, vendor=@vendor, invoice=@invoice, price=@price, purchase_date=@purchase_date, Total_kms_hrs=@Total_kms_hrs, totalservicecost=@totalservicecost, costper_km_hrs=@costper_km_hrs, undrwaranty=@undrwaranty, days=@days, kms=@kms where sno=@sno and branch_id=@branch_id");
                cmd.Parameters.Add("@tyre_sno", obj.tyre_sno);
                cmd.Parameters.Add("@brand", obj.brand);
                cmd.Parameters.Add("@model", obj.model);
                cmd.Parameters.Add("@size", obj.size);
                cmd.Parameters.Add("@load_value", obj.load_value);
                cmd.Parameters.Add("@type", obj.type);
                cmd.Parameters.Add("@status", obj.status);
                cmd.Parameters.Add("@vendor", obj.vendor);
                cmd.Parameters.Add("@invoice", obj.invoice);
                cmd.Parameters.Add("@price", obj.price);
                cmd.Parameters.Add("@purchase_date", purchase_date);
                cmd.Parameters.Add("@Total_kms_hrs", obj.Total_kms_hrs);
                cmd.Parameters.Add("@totalservicecost", obj.totalservicecost);
                cmd.Parameters.Add("@costper_km_hrs", obj.costper_km_hrs);
                cmd.Parameters.Add("@undrwaranty", obj.undrwaranty);
                cmd.Parameters.Add("@days", obj.days);
                cmd.Parameters.Add("@kms", obj.kms);
                cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
                cmd.Parameters.Add("@sno", obj.sno);
                vdm.Update(cmd);
                string response = GetJson("UPDATE");
                context.Response.Write(response);
            }
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }

    private void get_all_tyre_data(HttpContext context)
    {
        try
        {
            //cmd = new MySqlCommand("SELECT sno, tyre_sno, brand, model, size, load_value, type, status, vendor, invoice, price, purchase_date, Total_kms_hrs, totalservicecost, costper_km_hrs, undrwaranty, days, kms, branch_id, operatedby FROM tyres_master WHERE (branch_id = @branch_id)");
            cmd = new MySqlCommand("SELECT tyres_master.sno, tyres_master.tyre_sno, tyres_master.load_value, tyres_master.status, tyres_master.invoice, tyres_master.price, tyres_master.purchase_date, tyres_master.Total_kms_hrs,tyres_master.totalservicecost, tyres_master.costper_km_hrs, tyres_master.undrwaranty, tyres_master.days, tyres_master.kms, tyres_master.branch_id, tyres_master.operatedby,minimasters_2.mm_name AS brand, minimasters.mm_name AS model, minimasters_1.mm_name AS size, minimasters_3.mm_name AS type, vendors_info.vendorname AS vendor FROM tyres_master INNER JOIN minimasters minimasters_2 ON tyres_master.brand = minimasters_2.sno INNER JOIN minimasters ON tyres_master.model = minimasters.sno INNER JOIN minimasters minimasters_1 ON tyres_master.size = minimasters_1.sno INNER JOIN minimasters minimasters_3 ON tyres_master.type = minimasters_3.sno INNER JOIN vendors_info ON tyres_master.vendor = vendors_info.sno WHERE (tyres_master.branch_id = @branch_id)");
            cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
            DataTable tyrestable = vdm.SelectQuery(cmd).Tables[0];
            List<get_all_tyre_datacls> get_all_tyre_dataclslst = new List<get_all_tyre_datacls>();
            foreach (DataRow dr in tyrestable.Rows)
            {
                get_all_tyre_datacls get_all_tyredata = new get_all_tyre_datacls();
                get_all_tyredata.sno = dr["sno"].ToString();
                get_all_tyredata.tyre_sno = dr["tyre_sno"].ToString();
                get_all_tyredata.brand = dr["brand"].ToString();
                get_all_tyredata.model = dr["model"].ToString();
                get_all_tyredata.size = dr["size"].ToString();
                get_all_tyredata.load_value = dr["load_value"].ToString();
                get_all_tyredata.type = dr["type"].ToString();
                get_all_tyredata.status = dr["status"].ToString();
                get_all_tyredata.vendor = dr["vendor"].ToString();
                get_all_tyredata.invoice = dr["invoice"].ToString();
                get_all_tyredata.price = dr["price"].ToString();
                get_all_tyredata.purchase_date = ((DateTime)dr["purchase_date"]).ToString("dd/MM/yyyy");
                get_all_tyredata.Total_kms_hrs = dr["Total_kms_hrs"].ToString();
                get_all_tyredata.totalservicecost = dr["totalservicecost"].ToString();
                get_all_tyredata.costper_km_hrs = dr["costper_km_hrs"].ToString();
                get_all_tyredata.undrwaranty = dr["undrwaranty"].ToString();
                get_all_tyredata.days = dr["days"].ToString();
                get_all_tyredata.kms = dr["kms"].ToString();
                get_all_tyredata.branch_id = dr["branch_id"].ToString();
                get_all_tyre_dataclslst.Add(get_all_tyredata);
            }
            string response = GetJson(get_all_tyre_dataclslst);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }

    public class inward_data_cls
    {
        public string sno { get; set; }
        public string inward_id { get; set; }
        public string inward_dt { get; set; }
        public string inward_type { get; set; }
        public string invoice_number { get; set; }
        public string invoice_date { get; set; }
        public string dc_number { get; set; }
        public string lr_number { get; set; }
        public string vendor_sno { get; set; }
        public string po_date { get; set; }
        public string po_number { get; set; }
        public string discount { get; set; }
        public string discountper { get; set; }
        public string discount_type { get; set; }
        public string tax { get; set; }
        public string tax_per { get; set; }
        public string tax_type { get; set; }
        public string ed { get; set; }
        public string ed_per { get; set; }
        public string net_rate { get; set; }
        public string others { get; set; }
        public string total_amount { get; set; }
        public string remarks { get; set; }
        public string status { get; set; }
        public string branch_id { get; set; }
        public List<Inward_Get_items> inwarditems = new List<Inward_Get_items>();
    }

    public class Inward_Get_items
    {

        public string partnum_sno { get; set; }
        public string quantity { get; set; }
        public string actual_per_unit { get; set; }
        public string per_unit_cost { get; set; }
        public string totalcost { get; set; }
        public string PGSno { get; set; }
        public string PGName { get; set; }
        public string PNSno { get; set; }
        public string PNName { get; set; }
        public string PrtNumber { get; set; }
        public string RekeSno { get; set; }
        public string RakeName { get; set; }
        public string minimum_stock { get; set; }
        public string availble_qty { get; set; }
    }

    private void get_inward_data(HttpContext context)
    {
        try
        {
            string inward_sno = context.Request["inward_sno"];
            cmd = new MySqlCommand("SELECT inwarddata.sno, inwarddata.inward_id, inwarddata.inward_dt, inwarddata.inward_type, inwarddata.invoice_number, inwarddata.invoice_date, inwarddata.dc_number, inwarddata.lr_number,inwarddata.vendor_sno, inwarddata.po_date, inwarddata.po_number, inwarddata.discount, inwarddata.discountper, inwarddata.discount_type, inwarddata.tax, inwarddata.tax_per, inwarddata.tax_type,inwarddata.ed, inwarddata.ed_per, inwarddata.net_rate, inwarddata.others, inwarddata.total_amount, inwarddata.remarks, inwarddata.status, inwarddata_sub.partnum_sno, inwarddata_sub.quantity,inwarddata_sub.actual_per_unit, inwarddata_sub.per_unit_cost, inwarddata_sub.totalcost, partnum.pg_sno AS PGSno, minimasters.mm_name AS PGName, partnum.pn_sno AS PNSno, minimasters_1.mm_name AS PNName, partnum.pnum_Name AS PrtNumber, partnum.RakeNum AS RekeSno, minimasters_2.mm_name AS RakeName, partnum.minimum_stock, partnum.availble_qty, inwarddata.branch_id FROM inwarddata INNER JOIN inwarddata_sub ON inwarddata.sno = inwarddata_sub.inwarddata_sno INNER JOIN partnum ON inwarddata_sub.partnum_sno = partnum.sno INNER JOIN minimasters ON partnum.pg_sno = minimasters.sno INNER JOIN minimasters minimasters_1 ON partnum.pn_sno = minimasters_1.sno INNER JOIN minimasters minimasters_2 ON partnum.RakeNum = minimasters_2.sno WHERE (inwarddata.branch_id = @branch_id) AND (inwarddata.sno = @sno)");
            cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
            cmd.Parameters.Add("@sno", inward_sno);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            List<inward_data_cls> vendor = new List<inward_data_cls>();
            DataView view = new DataView(dt);
            DataTable distinctValues = view.ToTable(true, "sno", "inward_id", "inward_dt", "inward_type", "invoice_number", "invoice_date", "dc_number", "lr_number", "vendor_sno", "po_date", "po_number", "discount", "discountper", "discount_type", "tax", "tax_per", "tax_type", "ed", "ed_per", "net_rate", "others", "total_amount", "remarks", "status", "branch_id");
            foreach (DataRow dr in distinctValues.Rows)
            {
                DataRow[] partsdata = dt.Select("sno='" + dr["sno"].ToString() + "'");
                inward_data_cls data = new inward_data_cls();
                data.sno = dr["sno"].ToString();
                data.inward_id = dr["inward_id"].ToString();
                data.inward_dt = ((DateTime)dr["inward_dt"]).ToString("yyyy-MM-dd");
                data.inward_type = dr["inward_type"].ToString();
                data.invoice_number = dr["invoice_number"].ToString();
                data.invoice_date = ((DateTime)dr["invoice_date"]).ToString("yyyy-MM-dd");
                data.dc_number = dr["dc_number"].ToString();
                data.lr_number = dr["lr_number"].ToString();
                data.vendor_sno = dr["vendor_sno"].ToString();
                data.po_date = ((DateTime)dr["po_date"]).ToString("yyyy-MM-dd");
                data.po_number = dr["po_number"].ToString();
                data.discount = dr["discount"].ToString();
                data.discountper = dr["discountper"].ToString();
                data.discount_type = dr["discount_type"].ToString();
                data.tax = dr["tax"].ToString();
                data.tax_per = dr["tax_per"].ToString();
                data.tax_type = dr["tax_type"].ToString();
                data.ed = dr["ed"].ToString();
                data.ed_per = dr["ed_per"].ToString();
                data.net_rate = dr["net_rate"].ToString();
                data.others = dr["others"].ToString();
                data.total_amount = dr["total_amount"].ToString();
                data.remarks = dr["remarks"].ToString();
                data.status = dr["status"].ToString();
                data.branch_id = dr["branch_id"].ToString();
                foreach (DataRow inner in partsdata)
                {
                    Inward_Get_items prtdata = new Inward_Get_items();
                    prtdata.partnum_sno = inner["partnum_sno"].ToString();
                    prtdata.quantity = inner["quantity"].ToString();
                    prtdata.actual_per_unit = inner["actual_per_unit"].ToString();
                    prtdata.per_unit_cost = inner["per_unit_cost"].ToString();
                    prtdata.totalcost = inner["totalcost"].ToString();
                    prtdata.PGSno = inner["PGSno"].ToString();
                    prtdata.PGName = inner["PGName"].ToString();
                    prtdata.PNSno = inner["PNSno"].ToString();
                    prtdata.PNName = inner["PNName"].ToString();
                    prtdata.PrtNumber = inner["PrtNumber"].ToString();
                    prtdata.RekeSno = inner["RekeSno"].ToString();
                    prtdata.RakeName = inner["RakeName"].ToString();
                    prtdata.minimum_stock = inner["minimum_stock"].ToString();
                    prtdata.availble_qty = inner["availble_qty"].ToString();
                    data.inwarditems.Add(prtdata);
                }
                vendor.Add(data);
            }
            string response = GetJson(vendor);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.ToString());
            context.Response.Write(response);
        }
    }

    private void get_inward_only(HttpContext context)
    {
        try
        {
            cmd = new MySqlCommand("SELECT sno, inward_id, invoice_number, inward_dt FROM inwarddata WHERE (branch_id = @branch_id)");
            cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            List<inward_data_cls> vendor = new List<inward_data_cls>();
            foreach (DataRow dr in dt.Rows)
            {
                inward_data_cls data = new inward_data_cls();
                data.sno = dr["sno"].ToString();
                data.inward_id = dr["inward_id"].ToString();
                data.invoice_number = dr["invoice_number"].ToString();
                data.inward_dt = ((DateTime)dr["inward_dt"]).ToString("yyyy-MM-dd");
                vendor.Add(data);
            }
            string response = GetJson(vendor);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.ToString());
            context.Response.Write(response);
        }
    }

    //private void save_edit_WorkOrder(workorder_save obj, HttpContext context)
    //{
    //    double refno = 0;
    //    try
    //    {
    //        DateTime date_issued = DateTime.ParseExact(obj.date_issued, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
    //        DateTime date_started = DateTime.ParseExact(obj.date_started, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
    //        DateTime date_complete = DateTime.ParseExact(obj.date_complete, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
    //        if (obj.btnval == "Save" && obj.status == "Open")
    //        {
    //            cmd = new MySqlCommand("insert into workorder (workorder_num, workorder_dt, vehicle_id, workorder_odometer, workorder_enginehrs, workorder_startdt, workorder_closed_date, workorder_status, branch_id, operatedby, workordr_vendor, workorder_emp,workordr_dealedby) values (@workorder_num, @workorder_dt, @vehicle_id, @workorder_odometer, @workorder_enginehrs, @workorder_startdt, @workorder_closed_date, @workorder_status, @branch_id, @operatedby, @workordr_vendor, @workorder_emp,@workordr_dealedby)");
    //            cmd.Parameters.Add("@workorder_num", obj.workorderno);
    //            cmd.Parameters.Add("@workorder_dt", date_issued);
    //            cmd.Parameters.Add("@vehicle_id", obj.vehicle_val);
    //            cmd.Parameters.Add("@workorder_odometer", obj.odometer);
    //            cmd.Parameters.Add("@workorder_enginehrs", obj.wrkinghrs);
    //            cmd.Parameters.Add("@workorder_startdt", date_started);
    //            cmd.Parameters.Add("@workorder_closed_date", date_complete);
    //            cmd.Parameters.Add("@workorder_status", obj.status);
    //            cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
    //            cmd.Parameters.Add("@operatedby", context.Session["Employ_Sno"]);
    //            if (obj.vendoe_r_employee == "vendor")
    //            {
    //                cmd.Parameters.Add("@workordr_vendor", obj.vend_r_emp_sno);
    //                cmd.Parameters.Add("@workorder_emp", null);
    //            }
    //            else
    //            {
    //                cmd.Parameters.Add("@workordr_vendor", null);
    //                cmd.Parameters.Add("@workorder_emp", obj.vend_r_emp_sno);
    //            }
    //            cmd.Parameters.Add("@workordr_dealedby", obj.vendoe_r_employee);
    //            refno = vdm.insertScalar(cmd);
    //            foreach (maintanace_ lbj in obj.maintanace_Ary)
    //            {
    //                cmd = new MySqlCommand("insert into workorder_maintainance (workorder_sno, recurenttask_sno, wo_mnt_desc) values (@workorder_sno, @recurenttask_sno, @wo_mnt_desc)");
    //                cmd.Parameters.Add("@workorder_sno", refno);
    //                cmd.Parameters.Add("@recurenttask_sno", lbj.Rec_Task_Sno);
    //                cmd.Parameters.Add("@wo_mnt_desc", lbj.Description);
    //                vdm.insert(cmd);
    //            }
    //            foreach (replaces_ lbj in obj.replaces_Ary)
    //            {
    //                cmd = new MySqlCommand("insert into workorder_parts (workorder_sno, wo_parts_part_sno, wo_parts_cost, wo_parts_qty, wo_parts_total, wo_type) values (@workorder_sno, @wo_parts_part_sno, @wo_parts_cost, @wo_parts_qty, @wo_parts_total, @wo_type)");
    //                cmd.Parameters.Add("@workorder_sno", refno);
    //                cmd.Parameters.Add("@wo_parts_part_sno", lbj.Part_Number_Sno);
    //                cmd.Parameters.Add("@wo_parts_cost", lbj.Unitcost);
    //                cmd.Parameters.Add("@wo_parts_qty", lbj.Quantity);
    //                float total = lbj.Unitcost * lbj.Quantity;
    //                cmd.Parameters.Add("@wo_parts_total", total);
    //                cmd.Parameters.Add("@wo_type", lbj.Type);
    //                vdm.insert(cmd);
    //            }
    //            foreach (repairs_ lbj in obj.repairs_Ary)
    //            {
    //                cmd = new MySqlCommand("insert into workorder_repairs (repairs_sno, workorder_sno) values (@repairs_sno, @workorder_sno)");
    //                cmd.Parameters.Add("@workorder_sno", refno);
    //                cmd.Parameters.Add("@repairs_sno", lbj.Repair_Sno);
    //                vdm.insert(cmd);
    //            }
    //            foreach (inspections_ lbj in obj.inspections_Ary)
    //            {
    //                cmd = new MySqlCommand("insert into workorder_checkups (workorder_sno, checkups_sno, wo_checkupcheckup_desc) values (@workorder_sno, @checkups_sno, @wo_checkupcheckup_desc)");
    //                cmd.Parameters.Add("@workorder_sno", refno);
    //                cmd.Parameters.Add("@checkups_sno", lbj.Inspection_Sno);
    //                cmd.Parameters.Add("@wo_checkupcheckup_desc", lbj.Description);
    //                vdm.insert(cmd);
    //            }
    //            int workorder = 0;
    //            int.TryParse(obj.workorderno, out workorder);
    //            workorder = workorder + 1;
    //            cmd = new MySqlCommand("update branch_info set brnch_workorder_start=@brnch_workorder_start where brnch_sno=@brnch_sno");
    //            cmd.Parameters.Add("@brnch_workorder_start", workorder);
    //            cmd.Parameters.Add("@brnch_sno", context.Session["Branch_ID"]);
    //            vdm.Update(cmd);
    //            string response = GetJson("WorkOrder Raised Successuflly");
    //            context.Response.Write(response);
    //        }
    //        else if (obj.btnval == "Modify" && obj.status == "Open")
    //        {
    //            cmd = new MySqlCommand("update workorder set workorder_num=@workorder_num, workorder_dt=@workorder_dt, vehicle_id=@vehicle_id, workorder_odometer=@workorder_odometer, workorder_enginehrs=@workorder_enginehrs, workorder_startdt=@workorder_startdt, workorder_closed_date=@workorder_closed_date, workorder_status=@workorder_status, workordr_vendor=@workordr_vendor, workorder_emp=@workorder_emp,workordr_dealedby=@workordr_dealedby where sno=@sno");
    //            cmd.Parameters.Add("@workorder_num", obj.workorderno);
    //            cmd.Parameters.Add("@workorder_dt", date_issued);
    //            cmd.Parameters.Add("@vehicle_id", obj.vehicle_val);
    //            cmd.Parameters.Add("@workorder_odometer", obj.odometer);
    //            cmd.Parameters.Add("@workorder_enginehrs", obj.wrkinghrs);
    //            cmd.Parameters.Add("@workorder_startdt", date_started);
    //            cmd.Parameters.Add("@workorder_closed_date", date_complete);
    //            cmd.Parameters.Add("@workorder_status", obj.status);
    //            if (obj.vendoe_r_employee == "vendor")
    //            {
    //                cmd.Parameters.Add("@workordr_vendor", obj.vend_r_emp_sno);
    //                cmd.Parameters.Add("@workorder_emp", null);
    //            }
    //            else
    //            {
    //                cmd.Parameters.Add("@workordr_vendor", null);
    //                cmd.Parameters.Add("@workorder_emp", obj.vend_r_emp_sno);
    //            }
    //            cmd.Parameters.Add("@workordr_dealedby", obj.vendoe_r_employee);
    //            cmd.Parameters.Add("@sno", obj.sno);
    //            vdm.Update(cmd);
    //            cmd = new MySqlCommand("delete from workorder_maintainance where workorder_sno=@workorder_sno;delete from workorder_parts where workorder_sno=@workorder_sno;delete from workorder_repairs where workorder_sno=@workorder_sno;delete from workorder_checkups where workorder_sno=@workorder_sno;");
    //            cmd.Parameters.Add("@workorder_sno", obj.sno);
    //            vdm.Delete(cmd);
    //            foreach (maintanace_ lbj in obj.maintanace_Ary)
    //            {
    //                cmd = new MySqlCommand("insert into workorder_maintainance (workorder_sno, recurenttask_sno, wo_mnt_desc) values (@workorder_sno, @recurenttask_sno, @wo_mnt_desc)");
    //                cmd.Parameters.Add("@workorder_sno", obj.sno);
    //                cmd.Parameters.Add("@recurenttask_sno", lbj.Rec_Task_Sno);
    //                cmd.Parameters.Add("@wo_mnt_desc", lbj.Description);
    //                vdm.insert(cmd);
    //            }
    //            foreach (replaces_ lbj in obj.replaces_Ary)
    //            {
    //                cmd = new MySqlCommand("insert into workorder_parts (workorder_sno, wo_parts_part_sno, wo_parts_cost, wo_parts_qty, wo_parts_total, wo_type) values (@workorder_sno, @wo_parts_part_sno, @wo_parts_cost, @wo_parts_qty, @wo_parts_total, @wo_type)");
    //                cmd.Parameters.Add("@workorder_sno", obj.sno);
    //                cmd.Parameters.Add("@wo_parts_part_sno", lbj.Part_Number_Sno);
    //                cmd.Parameters.Add("@wo_parts_cost", lbj.Unitcost);
    //                cmd.Parameters.Add("@wo_parts_qty", lbj.Quantity);
    //                float total = lbj.Unitcost * lbj.Quantity;
    //                cmd.Parameters.Add("@wo_parts_total", total);
    //                cmd.Parameters.Add("@wo_type", lbj.Type);
    //                vdm.insert(cmd);
    //            }
    //            foreach (repairs_ lbj in obj.repairs_Ary)
    //            {
    //                cmd = new MySqlCommand("insert into workorder_repairs (repairs_sno, workorder_sno) values (@repairs_sno, @workorder_sno)");
    //                cmd.Parameters.Add("@workorder_sno", obj.sno);
    //                cmd.Parameters.Add("@repairs_sno", lbj.Repair_Sno);
    //                vdm.insert(cmd);
    //            }
    //            foreach (inspections_ lbj in obj.inspections_Ary)
    //            {
    //                cmd = new MySqlCommand("insert into workorder_checkups (workorder_sno, checkups_sno, wo_checkupcheckup_desc) values (@workorder_sno, @checkups_sno, @wo_checkupcheckup_desc)");
    //                cmd.Parameters.Add("@workorder_sno", obj.sno);
    //                cmd.Parameters.Add("@checkups_sno", lbj.Inspection_Sno);
    //                cmd.Parameters.Add("@wo_checkupcheckup_desc", lbj.Description);
    //                vdm.insert(cmd);
    //            }
    //            string response = GetJson("WorkOrder Successuflly Modified");
    //            context.Response.Write(response);
    //        }
    //        else if (obj.btnval == "Modify" && obj.status == "Pending")
    //        {
    //            cmd = new MySqlCommand("update workorder set workorder_status=@workorder_status where sno=@sno");
    //            cmd.Parameters.Add("@workorder_status", obj.status);
    //            cmd.Parameters.Add("@sno", obj.sno);
    //            vdm.Update(cmd);
    //            string response = GetJson("WorkOrder Status Successuflly Modified to Pending");
    //            context.Response.Write(response);
    //        }
    //        else if (obj.btnval == "Modify" && obj.status == "Closed")
    //        {
    //            cmd = new MySqlCommand("update workorder set workorder_status=@workorder_status where sno=@sno");
    //            cmd.Parameters.Add("@workorder_status", obj.status);
    //            cmd.Parameters.Add("@sno", obj.sno);
    //            vdm.Update(cmd);
    //            string response = GetJson("WorkOrder Status Successuflly Modified to Closed");
    //            context.Response.Write(response);
    //        }
    //        else if (obj.btnval == "Modify" && obj.status == "Waiting For Parts")
    //        {
    //            cmd = new MySqlCommand("update workorder set workorder_status=@workorder_status where sno=@sno");
    //            cmd.Parameters.Add("@workorder_status", obj.status);
    //            cmd.Parameters.Add("@sno", obj.sno);
    //            vdm.Update(cmd);
    //            string response = GetJson("WorkOrder Status Successuflly Modified to Waiting For Parts");
    //            context.Response.Write(response);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        cmd = new MySqlCommand("delete from workorder where sno=@sno;delete from workorder_maintainance where workorder_sno=@sno;delete from workorder_parts where workorder_sno=@sno;delete from workorder_repairs where workorder_sno=@sno;delete from workorder_checkups where workorder_sno=@sno;");
    //        cmd.Parameters.Add("@workorder_sno", refno);
    //        vdm.Delete(cmd);
    //        string response = GetJson(ex.Message);
    //        context.Response.Write(response);
    //    }
    //}
    public class workorder_save
    {
        public string workorderno { get; set; }
        public string vehicle_val { get; set; }
        public string date_issued { get; set; }
        public string date_started { get; set; }
        public string date_complete { get; set; }
        public string odometer { get; set; }
        public string vendoe_r_employee { get; set; }
        public string vend_r_emp_sno { get; set; }
        public string status { get; set; }
        public string wrkinghrs { get; set; }
        public string btnval { get; set; }
        public string sno { get; set; }
        public List<maintanace_> maintanace_Ary = new List<maintanace_>();
        public List<replaces_> replaces_Ary = new List<replaces_>();
        public List<repairs_> repairs_Ary = new List<repairs_>();
        public List<inspections_> inspections_Ary = new List<inspections_>();
    }
    public class maintanace_
    {
        public string Rec_Task { get; set; }
        public string Description { get; set; }
        public string Rec_Task_Sno { get; set; }
    }
    public class replaces_
    {
        public string Part_Number { get; set; }
        public string Part_Number_Sno { get; set; }
        public int Quantity { get; set; }
        public string Type { get; set; }
        public float Unitcost { get; set; }
    }
    public class repairs_
    {
        public string Repair_Name { get; set; }
        public string Description { get; set; }
        public string Repair_Sno { get; set; }
    }
    public class inspections_
    {
        public string Inspection_Name { get; set; }
        public string Description { get; set; }
        public string Inspection_Sno { get; set; }
    }
    private void get_inward_no(HttpContext context)
    {
        try
        {
            cmd = new MySqlCommand("SELECT brnch_sno, brnch_mobno, brnch_email, brnch_status, brnch_inward_start, brnch_outward_start, brnch_workorder_start, branchname, brnch_address, user_id FROM branch_info where");
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }

    private void save_edit_Inward(HttpContext context)
    {
        try
        {
            string inwarddate = context.Request["inwarddate"];
            string inwardno = context.Request["inwardno"];
            string invoiceno = context.Request["invoiceno"];
            string invoicedate = context.Request["invoicedate"];
            string dcno = context.Request["dcno"];
            string partno_sno = context.Request["partno_sno"];
            string lrno = context.Request["lrno"];
            string vendor_sno = context.Request["vendor_sno"];
            string inwardtype = context.Request["inwardtype"];
            string podate = context.Request["podate"];
            string doorno = context.Request["doorno"];
            // string transport = context.Request["transport"];
            string remarks = context.Request["remarks"];
            string totalcost = context.Request["totalcost"];
            string pono = context.Request["pono"];
            string btnval = context.Request["btnval"];
            string sno = context.Request["sno"];
            List<Inward_items> Product_list = (List<Inward_items>)context.Session["load_Next_Inward"];
            if (btnval == "Save")
            {
                cmd = new MySqlCommand("insert into inwarddata (inward_id, inward_dt, inward_type, invoice_number, invoice_date, dc_number, lr_number, vendor_sno, po_date, po_number,total_amount, remarks, branch_id, operated_by) values (@inward_id, @inward_dt, @inward_type, @invoice_number, @invoice_date, @dc_number, @lr_number, @vendor_sno, @po_date, @po_number,@total_amount, @remarks, @branch_id, @operated_by)");
                cmd.Parameters.Add("@inward_id", inwardno);
                cmd.Parameters.Add("@inward_dt", inwarddate);
                cmd.Parameters.Add("@inward_type", inwardtype);
                cmd.Parameters.Add("@invoice_number", invoiceno);
                cmd.Parameters.Add("@invoice_date", invoicedate);
                cmd.Parameters.Add("@dc_number", dcno);
                cmd.Parameters.Add("@lr_number", lrno);
                cmd.Parameters.Add("@vendor_sno", vendor_sno);
                cmd.Parameters.Add("@po_date", podate);
                cmd.Parameters.Add("@po_number", pono);
                cmd.Parameters.Add("@total_amount", totalcost);
                cmd.Parameters.Add("@remarks", remarks);
                cmd.Parameters.Add("@operated_by", context.Session["Employ_Sno"]);
                cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
                double refno = vdm.insertScalar(cmd);
                foreach (Inward_items art in Product_list)
                {
                    cmd = new MySqlCommand("insert into inwarddata_sub (inwarddata_sno, partnum_sno, quantity, per_unit_cost, totalcost) values (@inwarddata_sno, @partnum_sno, @quantity, @per_unit_cost, @totalcost)");
                    cmd.Parameters.Add("@inwarddata_sno", refno);
                    cmd.Parameters.Add("@partnum_sno", art.partnumno_sno);
                    cmd.Parameters.Add("@quantity", art.quantity);
                    cmd.Parameters.Add("@per_unit_cost", art.perunit);
                    cmd.Parameters.Add("@totalcost", art.totalcost);
                    vdm.insert(cmd);
                    cmd = new MySqlCommand("update partnum set availble_qty=@availble_qty where sno=@sno");
                    cmd.Parameters.Add("@sno", art.partnumno_sno);
                    cmd.Parameters.Add("@availble_qty", art.availstock);
                    vdm.Update(cmd);
                }

                int inward = 0;
                int.TryParse(inwardno, out inward);
                inward = inward + 1;
                cmd = new MySqlCommand("update branch_info set brnch_inward_start=@brnch_inward_start where brnch_sno=@brnch_sno");
                cmd.Parameters.Add("@brnch_inward_start", inward);
                cmd.Parameters.Add("@brnch_sno", context.Session["Branch_ID"]);
                vdm.Update(cmd);
                string response = GetJson("Inward Successfull...");
                context.Response.Write(response);
            }
            else
            {
                cmd = new MySqlCommand("update inwarddata set inward_id=@inward_id, inward_dt=@inward_dt, inward_type=@inward_type, invoice_number=@invoice_number, invoice_date=@invoice_date, dc_number=@dc_number, lr_number=@lr_number, vendor_sno=@vendor_sno, po_date=@po_date, po_number=@po_number,total_amount=@total_amount, remarks=@remarks where sno=@sno");
                cmd.Parameters.Add("@inward_id", inwardno);
                cmd.Parameters.Add("@inward_dt", inwarddate);
                cmd.Parameters.Add("@inward_type", inwardtype);
                cmd.Parameters.Add("@invoice_number", invoiceno);
                cmd.Parameters.Add("@invoice_date", invoicedate);
                cmd.Parameters.Add("@dc_number", dcno);
                cmd.Parameters.Add("@lr_number", lrno);
                cmd.Parameters.Add("@vendor_sno", vendor_sno);
                cmd.Parameters.Add("@po_date", podate);
                cmd.Parameters.Add("@po_number", pono);
                cmd.Parameters.Add("@total_amount", totalcost);
                cmd.Parameters.Add("@remarks", remarks);
                cmd.Parameters.Add("@sno", sno);
                vdm.Update(cmd);
                cmd = new MySqlCommand("delete from inwarddata_sub where inwarddata_sno=@inwarddata_sno");
                cmd.Parameters.Add("@inwarddata_sno", sno);
                vdm.Delete(cmd);
                foreach (Inward_items art in Product_list)
                {
                    cmd = new MySqlCommand("insert into inwarddata_sub (inwarddata_sno, partnum_sno, quantity, per_unit_cost, totalcost) values (@inwarddata_sno, @partnum_sno, @quantity, @per_unit_cost, @totalcost)");
                    cmd.Parameters.Add("@inwarddata_sno", sno);
                    cmd.Parameters.Add("@partnum_sno", art.partnumno_sno);
                    cmd.Parameters.Add("@quantity", art.quantity);
                    cmd.Parameters.Add("@per_unit_cost", art.perunit);
                    cmd.Parameters.Add("@totalcost", art.totalcost);
                    vdm.insert(cmd);
                    cmd = new MySqlCommand("update partnum set availble_qty=@availble_qty where sno=@sno");
                    cmd.Parameters.Add("@sno", art.partnumno_sno);
                    cmd.Parameters.Add("@availble_qty", art.availstock);
                    vdm.Update(cmd);
                }
                string response = GetJson("Inward Successfully Updated...");
                context.Response.Write(response);
            }
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }

    public class Inward_items
    {
        public string partnumno_sno { get; set; }
        public string quantity { get; set; }
        public string perunit { get; set; }
        public string totalcost { get; set; }
        public string availstock { get; set; }
    }
    public class Inward_rowwise
    {
        public string end { get; set; }
        public Inward_items row_detail { set; get; }
    }
    private void Inward_save_RowData(Inward_rowwise obj, HttpContext context)
    {
        try
        {
            Inward_items o = obj.row_detail;
            List<Inward_items> Product_list = (List<Inward_items>)context.Session["load_Next_Inward"];
            Product_list.Add(o);
            context.Session["load_Next_Inward"] = Product_list;
            string msg = obj.end;
            string response = GetJson(msg);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }

    public class get_rackandstock
    {
        public string rack { get; set; }
        public string availble_qty { get; set; }
        public string perunitcost { get; set; }
    }

    private void get_rackansstock_data(HttpContext context)
    {
        try
        {
            string partno_sno = context.Request["partno_sno"];
            cmd = new MySqlCommand("SELECT minimasters.mm_name AS Rake, partnum.sno, partnum.sno, partnum.availble_qty FROM partnum INNER JOIN minimasters ON partnum.RakeNum = minimasters.sno WHERE (partnum.sno = @sno)");
            cmd.Parameters.Add("@sno", partno_sno);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            List<get_rackandstock> vendor = new List<get_rackandstock>();
            foreach (DataRow dr in dt.Rows)
            {
                get_rackandstock data = new get_rackandstock();
                data.rack = dr["Rake"].ToString();
                data.availble_qty = dr["availble_qty"].ToString();
                vendor.Add(data);
            }
            string response = GetJson(vendor);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }

    private void save_edit_Recurrence(HttpContext context)
    {
        try
        {
            string taskname = context.Request["taskname"];
            string DueInDays = context.Request["DueInDays"];
            string InWorkingHours = context.Request["InWorkingHours"];
            string DueInKMs = context.Request["DueInKMs"];
            string btnval = context.Request["btnval"];
            string sno = context.Request["sno"];
            string status = context.Request["status"];
            string category_sno = context.Request["category_sno"];
            List<dis_items> Product_list = (List<dis_items>)context.Session["load_Next_Recurrence"];
            if (taskname != "")
            {
                if (btnval == "Save")
                {
                    cmd = new MySqlCommand("insert into recurrent_group ( recurrent_name, due_in_day, due_in_working_hrs, due_in_kms, status, branch_id, operatedby, category_sno) values (@recurrent_name, @due_in_day, @due_in_working_hrs, @due_in_kms, @status, @branch_id, @operatedby, @category_sno)");
                    cmd.Parameters.Add("@recurrent_name", taskname);
                    cmd.Parameters.Add("@due_in_day", DueInDays);
                    cmd.Parameters.Add("@due_in_working_hrs", InWorkingHours);
                    cmd.Parameters.Add("@due_in_kms", DueInKMs);
                    cmd.Parameters.Add("@category_sno", category_sno);
                    cmd.Parameters.Add("@operatedby", context.Session["Employ_Sno"]);
                    cmd.Parameters.Add("@status", status);
                    cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
                    double refno = vdm.insertScalar(cmd);
                    foreach (dis_items art in Product_list)
                    {
                        cmd = new MySqlCommand("insert into recurrent_group_subtable (recurrent_Sno, partnumber_sno,qty, parttype) values (@recurrent_Sno, @partnumber_sno,qty, parttype)");
                        cmd.Parameters.Add("@recurrent_Sno", refno);
                        cmd.Parameters.Add("@partnumber_sno", art.partnumber);
                        cmd.Parameters.Add("@qty", art.qty);
                        cmd.Parameters.Add("@parttype", art.rectype);
                        vdm.insert(cmd);
                    }
                    string response = GetJson("New Recurrence task Successfully Created");
                    context.Response.Write(response);
                }
                else
                {
                    cmd = new MySqlCommand("delete from recurrent_group_subtable where recurrent_Sno=@recurrent_Sno");
                    cmd.Parameters.Add("@recurrent_Sno", sno);
                    vdm.Delete(cmd);
                    cmd = new MySqlCommand("update recurrent_group set recurrent_name=@recurrent_name, due_in_day=@due_in_day, due_in_working_hrs=@due_in_working_hrs, due_in_kms=@due_in_kms, status=@status where sno=@sno");
                    cmd.Parameters.Add("@recurrent_name", taskname);
                    cmd.Parameters.Add("@due_in_day", DueInDays);
                    cmd.Parameters.Add("@due_in_working_hrs", InWorkingHours);
                    cmd.Parameters.Add("@due_in_kms", DueInKMs);
                    cmd.Parameters.Add("@category_sno", category_sno);
                    cmd.Parameters.Add("@status", status);
                    cmd.Parameters.Add("@sno", sno);
                    vdm.Update(cmd);
                    foreach (dis_items art in Product_list)
                    {
                        cmd = new MySqlCommand("insert into recurrent_group_subtable (recurrent_Sno, partnumber_sno,qty, parttype) values (@recurrent_Sno, @partnumber_sno,qty, parttype)");
                        cmd.Parameters.Add("@recurrent_Sno", sno);
                        cmd.Parameters.Add("@partnumber_sno", art.partnumber);
                        cmd.Parameters.Add("@qty", art.qty);
                        cmd.Parameters.Add("@parttype", art.rectype);
                        vdm.insert(cmd);
                    }
                    string response = GetJson("Recurrence task Successfully Modified");
                    context.Response.Write(response);
                }
            }
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }

    private void Recurrence_save_RowData(dis_rowwise obj, HttpContext context)
    {
        try
        {
            dis_items o = obj.row_detail;
            List<dis_items> Product_list = (List<dis_items>)context.Session["load_Next_Recurrence"];
            Product_list.Add(o);
            context.Session["load_Next_Recurrence"] = Product_list;
            string msg = obj.end;
            string response = GetJson(msg);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }

    public class get_recurring
    {
        public string recurrent_name { get; set; }
        public string sno { get; set; }
        public string status { get; set; }
        public string due_in_day { get; set; }
        public string due_in_working_hrs { get; set; }
        public string due_in_kms { get; set; }
        public List<partsdata> partdat = new List<partsdata>();
    }
    public class partsdata
    {
        public string partgroup { get; set; }
        public string partname { get; set; }
        public string partnumber { get; set; }
        public string partnumber_sno { get; set; }
        public string qty { get; set; }
        public string rectype { get; set; }
        public string unitcost { get; set; }
    }

    private void get_Recurring_data(HttpContext context)
    {
        try
        {
            string category_sno = context.Request["category_sno"];
            cmd = new MySqlCommand("SELECT recurrent_group.sno,partnum.pnum_Name AS PartNumber, partname.pn_name AS PartName, minimasters.mm_name AS PartGroup, recurrent_group.recurrent_name, recurrent_group.due_in_day,recurrent_group.due_in_working_hrs, recurrent_group.due_in_kms, recurrent_group.status, recurrent_group.category_sno, recurrent_group.branch_id,recurrent_group_subtable.partnumber_sno,recurrent_group_subtable.qty, recurrent_group_subtable.parttype,partnum.unitcost FROM recurrent_group_subtable INNER JOIN partnum ON recurrent_group_subtable.partnumber_sno = partnum.sno INNER JOIN partname ON partnum.pn_sno = partname.sno INNER JOIN minimasters ON partnum.pg_sno = minimasters.sno INNER JOIN recurrent_group ON recurrent_group_subtable.recurrent_Sno = recurrent_group.sno WHERE (recurrent_group.branch_id = @branch_id) AND (recurrent_group.category_sno = @category_sno)");
            cmd.Parameters.Add("@category_sno", category_sno);
            cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            List<get_recurring> vendor = new List<get_recurring>();
            DataView view = new DataView(dt);
            DataTable distinctValues = view.ToTable(true, "recurrent_name", "sno", "status", "due_in_day", "due_in_working_hrs", "due_in_kms");
            foreach (DataRow mnt in distinctValues.Rows)
            {
                get_recurring data = new get_recurring();
                data.recurrent_name = mnt["recurrent_name"].ToString();
                data.sno = mnt["sno"].ToString();
                data.status = mnt["status"].ToString();
                data.due_in_day = mnt["due_in_day"].ToString();
                data.due_in_working_hrs = mnt["due_in_working_hrs"].ToString();
                data.due_in_kms = mnt["due_in_kms"].ToString();
                DataRow[] mntdata = dt.Select("recurrent_name='" + mnt["recurrent_name"].ToString() + "'");
                foreach (DataRow inner in mntdata)
                {
                    partsdata prtdata = new partsdata();
                    prtdata.partgroup = inner["PartGroup"].ToString();
                    prtdata.partname = inner["PartName"].ToString();
                    prtdata.partnumber = inner["PartNumber"].ToString();
                    prtdata.partnumber_sno = inner["partnumber_sno"].ToString();
                    prtdata.qty = inner["qty"].ToString();
                    prtdata.rectype = inner["parttype"].ToString();
                    prtdata.unitcost = inner["unitcost"].ToString();

                    data.partdat.Add(prtdata);
                }
                vendor.Add(data);
            }
            string response = GetJson(vendor);
            context.Response.Write(response);

        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }

    public class get_employee
    {
        public string emp_sno { get; set; }
        public string employid { get; set; }
        public string employname { get; set; }
        public string emp_dob { get; set; }
        public string emp_licencenum { get; set; }
        public string emp_licenceexpire { get; set; }
        public string emp_status { get; set; }
        public string emp_type { get; set; }
        public string branch_id { get; set; }
        public string emp_login_id { get; set; }
        public string emp_pwd { get; set; }
        public string dept_id { get; set; }
        public string emp_bloodgrp { get; set; }
        public string emp_address { get; set; }
        public string emp_doj { get; set; }
        public string emp_experience { get; set; }
        public string Phoneno { get; set; }
        public string gender { get; set; }
        public string fathernme { get; set; }
        public string eduqual { get; set; }
        public string techqual { get; set; }
        public string bankac { get; set; }
        public string marital { get; set; }
        public string nationality { get; set; }
        public string imagename { get; set; }
        public string ftplocation { get; set; }
        public string imagepath { get; set; }
    }

    private void get_all_employee_data(HttpContext context)
    {
        try
        {
            string location = "ftp://223.196.32.30/FLEET/";
            cmd = new MySqlCommand("SELECT employdata.imagename,employdata.emp_sno, employdata.employid,employdata.imagepath, employdata.employname, employdata.emp_dob, employdata.emp_licencenum, employdata.emp_licenceexpire, employdata.emp_status, employdata.emp_type,employdata.emp_login_id, employdata.emp_pwd, employdata.operatedby, minimasters.mm_name, branch_info.branchname,employdata.emp_bloodgrp, employdata.emp_address, employdata.emp_doj, employdata.emp_experience,employdata.Phoneno,employdata.gender, employdata.fathernme, employdata.eduqual, employdata.techqual, employdata.bankac, employdata.marital, employdata.nationality FROM employdata INNER JOIN minimasters ON employdata.dept_id = minimasters.sno INNER JOIN branch_info ON employdata.branch_id = branch_info.brnch_sno WHERE (employdata.branch_id = @branch_id)");
            cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            List<get_employee> vendor = new List<get_employee>();

            foreach (DataRow dr in dt.Rows)
            {
                get_employee data = new get_employee();
                data.emp_sno = dr["emp_sno"].ToString();
                data.employid = dr["employid"].ToString();
                data.imagename = dr["imagename"].ToString();
                data.employname = dr["employname"].ToString();
                if (dr["emp_dob"].ToString() != null && dr["emp_dob"].ToString() != "")
                {
                    data.emp_dob = ((DateTime)dr["emp_dob"]).ToString("yyyy-MM-dd");
                }
                else
                {
                    data.emp_dob = dr["emp_dob"].ToString();
                }
                data.emp_licencenum = dr["emp_licencenum"].ToString();
                if (dr["emp_licenceexpire"].ToString() != null && dr["emp_licenceexpire"].ToString() != "")
                {
                    data.emp_licenceexpire = ((DateTime)dr["emp_licenceexpire"]).ToString("yyyy-MM-dd");
                }
                else
                {
                    data.emp_licenceexpire = dr["emp_licenceexpire"].ToString();
                }
                data.emp_status = dr["emp_status"].ToString();
                data.emp_type = dr["emp_type"].ToString();
                data.branch_id = dr["branchname"].ToString();
                data.emp_login_id = dr["emp_login_id"].ToString();
                data.emp_pwd = dr["emp_pwd"].ToString();
                data.dept_id = dr["mm_name"].ToString();
                data.emp_bloodgrp = dr["emp_bloodgrp"].ToString();
                data.emp_address = dr["emp_address"].ToString();
                if (dr["emp_doj"].ToString() != null && dr["emp_doj"].ToString() != "")
                {
                    data.emp_doj = ((DateTime)dr["emp_doj"]).ToString("yyyy-MM-dd");
                }
                else
                {
                    data.emp_address = dr["emp_doj"].ToString();
                }
                data.emp_experience = dr["emp_experience"].ToString();
                data.Phoneno = dr["Phoneno"].ToString();
                data.gender = dr["gender"].ToString();
                data.fathernme = dr["fathernme"].ToString();
                data.eduqual = dr["eduqual"].ToString();
                data.techqual = dr["techqual"].ToString();
                data.bankac = dr["bankac"].ToString();
                data.marital = dr["marital"].ToString();
                data.nationality = dr["nationality"].ToString();
                data.imagepath = dr["imagepath"].ToString();
                data.ftplocation = location;
                vendor.Add(data);
            }
            string response = GetJson(vendor);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }

    private void for_save_edit_employee(HttpContext context)
    {
        try
        {
            string emp_name = context.Request["emp_name"];
            string emp_id = context.Request["emp_id"];
            string emp_dep = context.Request["emp_department"];
            string emp_dob = context.Request["emp_dob"];
            string emp_licence = context.Request["emp_licence"];
            string licenceexp = context.Request["licenceexp"];
            string emp_type = context.Request["emp_type"];
            string emp_login = context.Request["emp_login"];
            string emp_pass = context.Request["emp_pass"];
            string sno = context.Request["sno"];
            string btnval = context.Request["btnval"];
            string status = context.Request["status"];
            string bloodgroup = context.Request["bloodgroup"];
            string address = context.Request["address"];
            string doj = context.Request["doj"];
            string experience = context.Request["experience"];
            string gender = context.Request["gender"];
            string fathersnme = context.Request["fathersnme"];
            string eduqquali = context.Request["eduqquali"];
            string techquali = context.Request["techquali"];
            string banckac = context.Request["banckac"];
            string maritial = context.Request["maritial"];
            string nationality = context.Request["nationality"];
            if (experience == "")
            {
                experience = "0.0";
            }
            string mobileno = context.Request["mobileno"];

            if (emp_dob == "")
            {
                emp_dob = null;
            }
            if (licenceexp == "")
            {
                licenceexp = null;
            }
            if (doj == "")
            {
                doj = null;
            }
            if (btnval == "Save")
            {
                cmd = new MySqlCommand("insert into employdata (employid, employname, emp_dob, emp_licencenum, emp_licenceexpire, emp_status, emp_type, branch_id, emp_login_id, emp_pwd, operatedby, dept_id,emp_bloodgrp, emp_address, emp_doj, emp_experience,Phoneno,gender, fathernme, eduqual, techqual, bankac, marital, nationality) values (@employid, @employname, @emp_dob, @emp_licencenum, @emp_licenceexpire, @emp_status, @emp_type, @branch_id, @emp_login_id, @emp_pwd, @operatedby, @dept_id, @emp_bloodgrp, @emp_address, @emp_doj, @emp_experience,@Phoneno,@gender, @fathernme, @eduqual, @techqual, @bankac, @marital, @nationality)");
                cmd.Parameters.Add("@employid", emp_id);
                cmd.Parameters.Add("@employname", emp_name);
                cmd.Parameters.Add("@emp_dob", emp_dob);
                cmd.Parameters.Add("@emp_licencenum", emp_licence);
                cmd.Parameters.Add("@emp_licenceexpire", licenceexp);
                cmd.Parameters.Add("@emp_status", status);
                cmd.Parameters.Add("@emp_type", emp_type);
                cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
                cmd.Parameters.Add("@emp_login_id", emp_login);
                cmd.Parameters.Add("@emp_pwd", emp_pass);
                cmd.Parameters.Add("@operatedby", context.Session["Employ_Sno"]);
                cmd.Parameters.Add("@dept_id", emp_dep);
                cmd.Parameters.Add("@emp_bloodgrp", bloodgroup);
                cmd.Parameters.Add("@emp_address", address);
                cmd.Parameters.Add("@emp_doj", doj);
                cmd.Parameters.Add("@emp_experience", experience);
                cmd.Parameters.Add("@Phoneno", mobileno);
                cmd.Parameters.Add("@gender", gender);
                cmd.Parameters.Add("@fathernme", fathersnme);
                cmd.Parameters.Add("@eduqual", eduqquali);
                cmd.Parameters.Add("@techqual", techquali);
                cmd.Parameters.Add("@bankac", banckac);
                cmd.Parameters.Add("@marital", maritial);
                cmd.Parameters.Add("@nationality", nationality);
                vdm.insert(cmd);
                string response = GetJson("OK");
                context.Response.Write(response);
            }
            else
            {
                cmd = new MySqlCommand("update employdata set   employid=@employid, employname=@employname, emp_dob=@emp_dob, emp_licencenum=@emp_licencenum, emp_licenceexpire=@emp_licenceexpire, emp_status=@emp_status, emp_type=@emp_type, emp_login_id=@emp_login_id, emp_pwd=@emp_pwd,dept_id=@dept_id,emp_bloodgrp=@emp_bloodgrp, emp_address=@emp_address, emp_doj=@emp_doj, emp_experience=@emp_experience,Phoneno=@Phoneno,gender=@gender, fathernme=@fathernme, eduqual=@eduqual, techqual=@techqual, bankac=@bankac, marital=@marital, nationality=@nationality where emp_sno=@emp_sno");
                cmd.Parameters.Add("@employid", emp_id);
                cmd.Parameters.Add("@employname", emp_name);
                cmd.Parameters.Add("@emp_dob", emp_dob);
                cmd.Parameters.Add("@emp_licencenum", emp_licence);
                cmd.Parameters.Add("@emp_licenceexpire", licenceexp);
                cmd.Parameters.Add("@emp_status", status);
                cmd.Parameters.Add("@emp_type", emp_type);
                cmd.Parameters.Add("@emp_login_id", emp_login);
                cmd.Parameters.Add("@emp_pwd", emp_pass);
                cmd.Parameters.Add("@dept_id", emp_dep);
                cmd.Parameters.Add("@emp_bloodgrp", bloodgroup);
                cmd.Parameters.Add("@emp_address", address);
                cmd.Parameters.Add("@emp_doj", doj);
                cmd.Parameters.Add("@emp_experience", experience);
                cmd.Parameters.Add("@emp_sno", sno);
                cmd.Parameters.Add("@Phoneno", mobileno);
                cmd.Parameters.Add("@gender", gender);
                cmd.Parameters.Add("@fathernme", fathersnme);
                cmd.Parameters.Add("@eduqual", eduqquali);
                cmd.Parameters.Add("@techqual", techquali);
                cmd.Parameters.Add("@bankac", banckac);
                cmd.Parameters.Add("@marital", maritial);
                cmd.Parameters.Add("@nationality", nationality);
                vdm.Update(cmd);
                string response = GetJson("UPDATE");
                context.Response.Write(response);
            }
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }

    public class get_maintenance
    {
        public string maintenance_name { get; set; }
        public string sno { get; set; }
        public string status { get; set; }
        public List<partdata> partdat = new List<partdata>();
    }
    public class partdata
    {
        public string partgroup { get; set; }
        public string partname { get; set; }
        public string partnumber { get; set; }
        public string partnumber_sno { get; set; }
    }

    private void get_all_Maintenance(HttpContext context)
    {
        try
        {
            cmd = new MySqlCommand("SELECT maintenance_master.sno, maintenance_master.maintenance_name, partnum.pnum_Name AS PartNum, partname.pn_name AS PartName, minimasters.mm_name AS PartGroup, maintenance_sub.partnum_sno,maintenance_master.status FROM maintenance_master INNER JOIN maintenance_sub ON maintenance_master.sno = maintenance_sub.maintenance_sno INNER JOIN partnum ON maintenance_sub.partnum_sno = partnum.sno INNER JOIN partname ON partnum.pn_sno = partname.sno INNER JOIN minimasters ON partname.MM_sno = minimasters.sno WHERE (maintenance_master.branch_id = @branch_id)");
            cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            List<get_maintenance> vendor = new List<get_maintenance>();
            DataView view = new DataView(dt);
            DataTable distinctValues = view.ToTable(true, "maintenance_name", "sno", "status");
            foreach (DataRow mnt in distinctValues.Rows)
            {
                get_maintenance data = new get_maintenance();
                data.maintenance_name = mnt["maintenance_name"].ToString();
                data.sno = mnt["sno"].ToString();
                data.status = mnt["status"].ToString();
                DataRow[] mntdata = dt.Select("maintenance_name='" + mnt["maintenance_name"].ToString() + "'");
                foreach (DataRow inner in mntdata)
                {
                    partdata prtdata = new partdata();
                    prtdata.partgroup = inner.ItemArray[4].ToString();
                    prtdata.partname = inner.ItemArray[3].ToString();
                    prtdata.partnumber = inner.ItemArray[2].ToString();
                    prtdata.partnumber_sno = inner.ItemArray[5].ToString();
                    data.partdat.Add(prtdata);
                }
                vendor.Add(data);
            }
            string response = GetJson(vendor);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }

    private void save_edit_Maintanance(HttpContext context)
    {
        try
        {
            string maintanancename = context.Request["maintanancename"];
            string btnval = context.Request["btnval"];
            string sno = context.Request["sno"];
            string status = context.Request["status"];
            List<dis_items> Product_list = (List<dis_items>)context.Session["load_Next_Maintanance"];
            if (maintanancename != "")
            {
                if (btnval == "Save")
                {
                    cmd = new MySqlCommand("insert into maintenance_master (maintenance_name,branch_id,status) values (@maintenance_name,@branch_id,@status)");
                    cmd.Parameters.Add("@maintenance_name", maintanancename);
                    cmd.Parameters.Add("@status", status);
                    cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
                    double refno = vdm.insertScalar(cmd);
                    foreach (dis_items art in Product_list)
                    {
                        cmd = new MySqlCommand("insert into maintenance_sub (maintenance_sno, partnum_sno) values (@maintenance_sno, @partnum_sno)");
                        cmd.Parameters.Add("@maintenance_sno", refno);
                        cmd.Parameters.Add("@partnum_sno", art.partnumber);
                        vdm.insert(cmd);
                    }
                    string response = GetJson("New Maintenance Successfully Created");
                    context.Response.Write(response);
                }
                else
                {
                    cmd = new MySqlCommand("delete from maintenance_sub where maintenance_sno=@maintenance_sno");
                    cmd.Parameters.Add("@maintenance_sno", sno);
                    vdm.Delete(cmd);
                    cmd = new MySqlCommand("update maintenance_master set maintenance_name=@maintenance_name,status=@status where sno=@sno");
                    cmd.Parameters.Add("@maintenance_name", maintanancename);
                    cmd.Parameters.Add("@status", status);
                    cmd.Parameters.Add("@sno", sno);
                    vdm.Update(cmd);
                    foreach (dis_items art in Product_list)
                    {
                        cmd = new MySqlCommand("insert into maintenance_sub (maintenance_sno, partnum_sno) values (@maintenance_sno, @partnum_sno)");
                        cmd.Parameters.Add("@maintenance_sno", sno);
                        cmd.Parameters.Add("@partnum_sno", art.partnumber);
                        vdm.insert(cmd);
                    }
                    string response = GetJson("Maintenance Successfully Modified");
                    context.Response.Write(response);
                }
            }
        }
        catch (Exception ex)
        {
            string response = GetJson("Error..!Please try again");
            context.Response.Write(response);
        }
    }

    private void Maintanance_save_RowData(dis_rowwise obj, HttpContext context)
    {
        try
        {

            dis_items o = obj.row_detail;
            List<dis_items> Product_list = (List<dis_items>)context.Session["load_Next_Maintanance"];
            Product_list.Add(o);
            context.Session["load_Next_Maintanance"] = Product_list;
            string msg = obj.end;
            string response = GetJson(msg);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }
    public class dis_rowwise
    {
        public string end { get; set; }
        public dis_items row_detail { set; get; }
    }
    public class dis_items
    {
        public string partgroup { get; set; }
        public string partname { get; set; }
        public string partnumber { get; set; }

        public string qty { get; set; }
        public string rectype { get; set; }
    }
    public class getting_partnum
    {
        public string sno { set; get; }
        public string pnum_Name { set; get; }
        public string RakeNum { set; get; }
        public string RakeName { set; get; }
        public string status { set; get; }
        public string unitcost { set; get; }
    }

    private void get_Part_number_data(HttpContext context)
    {
        try
        {
            string pg_sno = context.Request["partgroup_sno"];
            string pn_sno = context.Request["partname_sno"];
            cmd = new MySqlCommand("SELECT partnum.sno, partnum.pg_sno,partnum.unitcost, partnum.RakeNum,partnum.pn_sno, partnum.pnum_Name, partnum.pnum_desc, partnum.minimum_stock, partnum.operatedby, partnum.branch_id, partnum.status, minimasters.mm_name FROM partnum INNER JOIN minimasters ON partnum.RakeNum = minimasters.sno WHERE (partnum.pg_sno = @pg_sno) AND (partnum.pn_sno = @pn_sno) AND (partnum.branch_id = @branch_id)");
            cmd.Parameters.Add("@pg_sno", pg_sno);
            cmd.Parameters.Add("@pn_sno", pn_sno);
            cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            List<getting_partnum> vendor = new List<getting_partnum>();

            foreach (DataRow dr in dt.Rows)
            {
                getting_partnum data = new getting_partnum();
                data.sno = dr["sno"].ToString();
                data.pnum_Name = dr["pnum_Name"].ToString();
                data.RakeNum = dr["RakeNum"].ToString();
                data.RakeName = dr["mm_name"].ToString();
                data.status = dr["status"].ToString();
                data.unitcost = dr["unitcost"].ToString();
                vendor.Add(data);
            }

            string response = GetJson(vendor);
            context.Response.Write(response);

        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }

    private void for_save_edit_axilmaster(HttpContext context)
    {
        try
        {
            string axilname = context.Request["axilname"];
            string sno = context.Request["sno"];
            string btnval = context.Request["btnval"];
            string status = context.Request["status"];
            if (btnval == "Save")
            {
                cmd = new MySqlCommand("insert into axils_master (axil_name,status, branch_id) values (@axil_name,@status, @branch_id)");
                cmd.Parameters.Add("@axil_name", axilname);
                cmd.Parameters.Add("@status", status);
                cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
                vdm.insert(cmd);
                string response = GetJson("OK");
                context.Response.Write(response);
            }
            else
            {
                cmd = new MySqlCommand("update axils_master set axil_name=@axil_name,status=@status where sno=@sno");
                cmd.Parameters.Add("@axil_name", axilname);
                cmd.Parameters.Add("@status", status);
                cmd.Parameters.Add("@sno", sno);
                vdm.insert(cmd);
                string response = GetJson("UPDATE");
                context.Response.Write(response);
            }
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }
    public class get_veh_master_data
    {
        public string vm_sno { set; get; }
        public string VehType { set; get; }
        public string VehMake { set; get; }
        public string axils_refno { set; get; }
        public string axilmaster_name { set; get; }
        public string registration_no { set; get; }
        public string door_no { set; get; }
        public string fuel_capacity { set; get; }
        public string Category { set; get; }
        public string status { set; get; }
        public string Capacity { set; get; }
        public string v_ty_fuel_capacity { set; get; }
        public string vm_model { set; get; }
        public string vm_engine { set; get; }
        public string vm_chasiss { set; get; }
        public string vm_owner { set; get; }
        public string vm_owneraddr { set; get; }
        public string vm_rcno { set; get; }
        public string vm_rcexpdate { set; get; }
        public string vm_pollution { set; get; }
        public string vm_poll_exp_date { set; get; }
        public string vm_insurance { set; get; }
        public string vm_isurence_exp_date { set; get; }
        public string vm_fitness { set; get; }
        public string vm_fit_exp_date { set; get; }
        public string vm_roadtax { set; get; }
        public string vm_roadtax_exp_date { set; get; }
        public string act_mileage { set; get; }
        public string insurancesno { set; get; }
        public string batteryno { set; get; }
        public string batteryno2 { set; get; }
        public string ftplocation { get; set; }
        public string imagename { get; set; }
        public string ledgername { get; set; }
        public string petroledgername { get; set; }
        public string ledgercode { get; set; }
        public string petro_ledgercode { get; set; }
        public string whscode { get; set; }

        public string permitstatename { set; get; }
        public string permit_state_expdate { set; get; }
        public string stateroadtax { set; get; }
        public string stateroadtaxexp_date { set; get; }
    }
    private void get_all_veh_master_data(HttpContext context)
    {
        try
        {
            //cmd = new MySqlCommand("SELECT minimasters.mm_name AS VehMake, axils_master.axil_name, vehicel_master.vm_sno, vehicel_master.registration_no, vehicel_master.door_no, vehicel_master.fuel_capacity, vehicel_master.status,minimasters_2.mm_name AS Category, vehicle_types.v_ty_name AS VehType, vehicel_master.branch_id, vehicel_master.operatedby FROM vehicel_master INNER JOIN minimasters ON vehicel_master.vhmake_refno = minimasters.sno INNER JOIN axils_master ON vehicel_master.axils_refno = axils_master.sno INNER JOIN minimasters minimasters_2 ON vehicel_master.category = minimasters_2.sno INNER JOIN vehicle_types ON vehicel_master.vhtype_refno = vehicle_types.sno WHERE (vehicel_master.branch_id = @branch_id) AND (vehicel_master.operatedby = @operatedby)");
            cmd = new MySqlCommand("SELECT vehicel_master.whcode, vehicel_master.ledger_code,vehicel_master.Petroledger_code, vehicel_master.petroledgername, vehicel_master.ledgername, vehicel_master.batteryno2,vehicel_master.statename,vehicel_master.imagename,vehicel_master.batteryno, vehicel_master.state_permit_exp_date, vehicel_master.state_roadtax, vehicel_master.state_roadtax_exp_date, vehicel_master.vm_sno, vehicel_master.registration_no,vehicel_master.act_mileage,vehicel_master.insurancesno, vehicel_master.door_no, vehicel_master.status, vehicel_master.branch_id, vehicel_master.operatedby, vehicel_master.Capacity, minimasters.mm_name AS veh_type, minimasters_1.mm_name AS veh_make, vehicel_master.fuel_capacity, vehicel_master.axils_refno, axil_master.axilmaster_name FROM vehicel_master INNER JOIN minimasters ON vehicel_master.vhtype_refno = minimasters.sno INNER JOIN minimasters minimasters_1 ON vehicel_master.vhmake_refno = minimasters_1.sno LEFT OUTER JOIN axil_master ON vehicel_master.axils_refno = axil_master.sno WHERE (vehicel_master.branch_id = @branch_id) order by minimasters.mm_name desc");
            cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            List<get_veh_master_data> vendor = new List<get_veh_master_data>();
            foreach (DataRow dr in dt.Rows)
            {
                get_veh_master_data data = new get_veh_master_data();
                data.vm_sno = dr["vm_sno"].ToString();
                data.VehType = dr["veh_type"].ToString();
                data.VehMake = dr["veh_make"].ToString();
                data.registration_no = dr["registration_no"].ToString();
                data.door_no = dr["door_no"].ToString();
                data.Capacity = dr["Capacity"].ToString();
                data.v_ty_fuel_capacity = dr["fuel_capacity"].ToString();
                data.status = dr["status"].ToString();
                data.axils_refno = dr["axils_refno"].ToString();
                data.axilmaster_name = dr["axilmaster_name"].ToString();
                data.act_mileage = dr["act_mileage"].ToString();
                data.insurancesno = dr["insurancesno"].ToString();
                data.permitstatename = dr["statename"].ToString();
                data.batteryno = dr["batteryno"].ToString();
                data.batteryno2 = dr["batteryno2"].ToString();
                data.ledgername = dr["ledgername"].ToString();
                data.petroledgername = dr["petroledgername"].ToString();
                data.ledgercode = dr["ledger_code"].ToString();
                data.petro_ledgercode = dr["Petroledger_code"].ToString();
                data.whscode = dr["whcode"].ToString();
                data.ftplocation = "ftp://223.196.32.30:21/FLEET/";
                string imagename = dr["imagename"].ToString();
                data.imagename = dr["imagename"].ToString();
                //retrive_Request(context, imagename);
                if (dr["state_permit_exp_date"].ToString() != "" && dr["state_permit_exp_date"].ToString() != null)
                {
                    data.permit_state_expdate = ((DateTime)dr["state_permit_exp_date"]).ToString("yyyy-MM-dd");
                }
                if (dr["state_roadtax_exp_date"].ToString() != "" && dr["state_roadtax_exp_date"].ToString() != null)
                {
                    data.stateroadtaxexp_date = ((DateTime)dr["state_roadtax_exp_date"]).ToString("yyyy-MM-dd");
                }

                data.stateroadtax = dr["state_roadtax"].ToString();
                vendor.Add(data);
            }
            string response = GetJson(vendor);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }
    public void retrive_Request(HttpContext context, string imagename)
    {
        // for ftp connenction 
        string FileSaveUri = "ftp://223.196.32.30:21/FLEET/";
        string ftpUser = "ftpvys";
        string ftpPassWord = "Vyshnavi123";

        FtpWebRequest displayRequest = (FtpWebRequest)WebRequest.Create(new Uri(FileSaveUri + imagename));
        displayRequest.Method = WebRequestMethods.Ftp.DownloadFile;
        displayRequest.Credentials = new NetworkCredential(ftpUser, ftpPassWord);

        try
        {
            FtpWebResponse response = (FtpWebResponse)displayRequest.GetResponse();
            Stream Stream = response.GetResponseStream();
            byte[] bytes = new byte[10240];
            int i = 0;
            MemoryStream mStream = new MemoryStream();
            do
            {
                i = Stream.Read(bytes, 0, bytes.Length);
                mStream.Write(bytes, 0, i);
            } while (i != 0);
            context.Response.Clear();
            context.Response.ClearHeaders();
            context.Response.ClearContent();
            context.Response.ContentType = "image/jpeg";
            context.Response.BinaryWrite(mStream.GetBuffer());
        }
        catch (WebException wex)
        {
        }
        catch (Exception ex)
        {
            throw new Exception("An Error Occurred" + ex);

        }

    }
    private void get_all_veh_info(HttpContext context)
    {
        try
        {
            cmd = new MySqlCommand("SELECT vm_sno, registration_no, door_no, status, vhtype_refno, axils_refno FROM vehicel_master WHERE (branch_id = @b) AND (status = 1)");
            cmd.Parameters.Add("@b", context.Session["Branch_ID"]);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            List<get_veh_master_data> vendor = new List<get_veh_master_data>();
            foreach (DataRow dr in dt.Rows)
            {
                get_veh_master_data data = new get_veh_master_data();
                data.vm_sno = dr["vm_sno"].ToString();
                //  data.VehType = dr["VehType"].ToString();
                //  data.VehMake = dr["VehMake"].ToString();
                //  data.axil_name = dr["axil_name"].ToString();
                data.registration_no = dr["registration_no"].ToString();
                data.door_no = dr["door_no"].ToString();
                data.status = dr["status"].ToString();
                data.Category = dr["vhtype_refno"].ToString();
                vendor.Add(data);
            }

            string response = GetJson(vendor);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }

    private void save_vehicle_master(HttpContext context)
    {
        try
        {
            string regno = context.Request["regno"];
            string vehtype = context.Request["vehtype"];
            string sno = context.Request["sno"];
            string btnval = context.Request["btnval"];
            string doorno = context.Request["doorno"];
            string status = context.Request["status"];
            string Capacity = context.Request["Capacity"];
            string model = context.Request["model"];
            string engineno = context.Request["engineno"];
            string chasiss = context.Request["chasiss"];
            string owner = context.Request["owner"];
            string address = context.Request["address"];
            string rcno = context.Request["rcno"];
            string rcexp_date = context.Request["rcexp_date"];
            string pollution = context.Request["pollution"];
            string pollexp_date = context.Request["pollexp_date"];
            string insurence = context.Request["insurence"];
            string insexp_date = context.Request["insexp_date"];
            string fitness = context.Request["fitness"];
            string fitexp_date = context.Request["fitexp_date"];
            string roadtax = context.Request["roadtax"];
            string roadtaxexp_date = context.Request["roadtaxexp_date"];
            string vehicle_make = context.Request["vehicle_make"];
            string fuel_capacity = context.Request["fuel_capacity"];
            string axilmaster = context.Request["axilmaster"];
            string Odometer = context.Request["Odometer"];
            string axil_name_check = context.Request["axil_name_check"];
            string InsuranceSno = context.Request["InsuranceSno"];
            string Actualmileage = context.Request["Actualmileage"];
            string Batteryno = context.Request["Batteryno"];
            string strBatteryno2 = context.Request["Batteryno2"];
            string ledgername = context.Request["ledgername"];
            string ledgercode = context.Request["ledgercode"];
            string petro_ledgername = context.Request["petro_ledgername"];
            string petro_ledgercode = context.Request["petro_ledgercode"];
            string whscode = context.Request["whscode"];
            
            int Batteryno2 = 0;
            int.TryParse(strBatteryno2, out Batteryno2);

            string permitstatename = context.Request["permitstatename"];
            string permit_state_expdate = context.Request["permit_state_expdate"];
            string stateroadtax = context.Request["stateroadtax"];
            string stateroadtaxexp_date = context.Request["stateroadtaxexp_date"];
            if (rcexp_date == "")
            {
                rcexp_date = null;
            }
            if (pollexp_date == "")
            {
                pollexp_date = null;
            }
            if (insexp_date == "")
            {
                insexp_date = null;
            }
            if (fitexp_date == "")
            {
                fitexp_date = null;
            }
            if (roadtaxexp_date == "")
            {
                roadtaxexp_date = null;
            }
            if (permit_state_expdate == "")
            {
                permit_state_expdate = null;
            }
            if (stateroadtaxexp_date == "")
            {
                stateroadtaxexp_date = null;
            }
            if (status == "True")
            {
                status = "1";
            }
            else
            {
                status = "0";
            }
            double refno = 0.0;
            List<Axils_items> Product_list = (List<Axils_items>)context.Session["load_Next_Axils"];
            if (btnval == "Save")
            {
                cmd = new MySqlCommand("insert into vehicel_master (vhtype_refno,vhmake_refno,axils_refno,registration_no, door_no,branch_id, operatedby,Capacity, vm_model, vm_engine, vm_chasiss, vm_owner, vm_owneraddr, vm_rcno, vm_rcexpdate, vm_pollution,vm_poll_exp_date, vm_insurance, vm_isurence_exp_date, vm_fitness, vm_fit_exp_date, vm_roadtax, vm_roadtax_exp_date,fuel_capacity,insurancesno,act_mileage,statename,state_permit_exp_date,state_roadtax,state_roadtax_exp_date,batteryno,batteryno2,ledgername,petroledgername, ledger_code,Petroledger_code,whcode) values (@vhtype_refno,@vhmake_refno,@axils_refno ,@registration_no, @door_no,@branch_id, @operatedby,@Capacity, @vm_model, @vm_engine, @vm_chasiss, @vm_owner, @vm_owneraddr, @vm_rcno, @vm_rcexpdate, @vm_pollution,@vm_poll_exp_date, @vm_insurance, @vm_isurence_exp_date, @vm_fitness, @vm_fit_exp_date, @vm_roadtax, @vm_roadtax_exp_date,@fuel_capacity,@insurancesno,@act_mileage,@statename,@state_permit_exp_date,@state_roadtax,@state_roadtax_exp_date,@batteryno,@batteryno2,@ledgername,@petroledgername,@ledger_code,@Petroledger_code,@whcode)");
                cmd.Parameters.Add("@vhtype_refno", vehtype);
                cmd.Parameters.Add("@registration_no", regno);
                cmd.Parameters.Add("@door_no", doorno);
                cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
                cmd.Parameters.Add("@operatedby", context.Session["Employ_Sno"]);
                cmd.Parameters.Add("@Capacity", Capacity);
                cmd.Parameters.Add("@vm_model", model);
                cmd.Parameters.Add("@vm_engine", engineno);
                cmd.Parameters.Add("@vm_chasiss", chasiss);
                cmd.Parameters.Add("@vm_owner", owner);
                cmd.Parameters.Add("@vm_owneraddr", address);
                cmd.Parameters.Add("@vm_rcno", rcno);
                cmd.Parameters.Add("@vm_rcexpdate", rcexp_date);
                cmd.Parameters.Add("@vm_pollution", pollution);
                cmd.Parameters.Add("@vm_poll_exp_date", pollexp_date);
                cmd.Parameters.Add("@vm_insurance", insurence);
                cmd.Parameters.Add("@vm_isurence_exp_date", insexp_date);
                cmd.Parameters.Add("@vm_fitness", fitness);
                cmd.Parameters.Add("@vm_fit_exp_date", fitexp_date);
                cmd.Parameters.Add("@vm_roadtax", roadtax);
                cmd.Parameters.Add("@vm_roadtax_exp_date", roadtaxexp_date);
                cmd.Parameters.Add("@vhmake_refno", vehicle_make);
                cmd.Parameters.Add("@fuel_capacity", fuel_capacity);
                cmd.Parameters.Add("@insurancesno", InsuranceSno);
                cmd.Parameters.Add("@act_mileage", Actualmileage);
                cmd.Parameters.Add("@statename", permitstatename);
                cmd.Parameters.Add("@state_permit_exp_date", permit_state_expdate);
                cmd.Parameters.Add("@state_roadtax", stateroadtax);
                cmd.Parameters.Add("@state_roadtax_exp_date", stateroadtaxexp_date);
                cmd.Parameters.Add("@batteryno", Batteryno);
                cmd.Parameters.Add("@batteryno2", Batteryno2);
                cmd.Parameters.Add("@ledgername", ledgername);
                cmd.Parameters.Add("@petroledgername", petro_ledgername);
                cmd.Parameters.Add("@ledger_code", ledgercode);
                cmd.Parameters.Add("@Petroledger_code", petro_ledgercode);
                cmd.Parameters.Add("@whcode", whscode);
                
                if (axilmaster != "" && axilmaster != null)
                {
                    cmd.Parameters.Add("@axils_refno", axilmaster);
                }
                else
                {
                    cmd.Parameters.Add("@axils_refno", null);
                }
                refno = vdm.insertScalar(cmd);
                if (Odometer != "" && axilmaster != "" && axilmaster != null)
                {
                    foreach (Axils_items art in Product_list)
                    {
                        foreach (Axils_sub_items obj in art.innertable_array)
                        {
                            cmd = new MySqlCommand("insert into vehicle_master_sub (vehicle_mstr_sno, axles_tyres_names_sno, tyre_sno,Odometer) values (@vehicle_mstr_sno, @axles_tyres_names_sno, @tyre_sno,@Odometer)");
                            cmd.Parameters.Add("@vehicle_mstr_sno", refno);
                            cmd.Parameters.Add("@axles_tyres_names_sno", obj.tyre_position_sno);
                            cmd.Parameters.Add("@tyre_sno", obj.tyre_sno);
                            cmd.Parameters.Add("@Odometer", Odometer);
                            vdm.insert(cmd);
                            if (art.axilname != "Stephanie")
                            {
                                cmd = new MySqlCommand("update new_tyres_sub set Fitting_Type='F' where sno=@sno");
                                cmd.Parameters.Add("@sno", obj.tyre_sno);
                                vdm.Update(cmd);
                            }
                            else
                            {
                                cmd = new MySqlCommand("update new_tyres_sub set Fitting_Type='S' where sno=@sno");
                                cmd.Parameters.Add("@sno", obj.tyre_sno);
                                vdm.Update(cmd);
                            }
                        }
                    }
                }
                string response = GetJson("OK");
                context.Response.Write(response);
            }
            else
            {
                cmd = new MySqlCommand("SELECT vm_sno, registration_no, branch_id, vm_rcexpdate, vm_poll_exp_date, vm_isurence_exp_date, vm_fit_exp_date, vm_roadtax_exp_date,state_permit_exp_date, state_roadtax_exp_date FROM vehicel_master WHERE (branch_id = @BranchID) AND (vm_sno = @vehclesno)");
                cmd.Parameters.Add("@BranchID", context.Session["Branch_ID"]);
                cmd.Parameters.Add("@vehclesno", sno);
                DataTable dtvehicledetails = vdm.SelectQuery(cmd).Tables[0];
                if (dtvehicledetails.Rows.Count > 0)
                {
                    string permit_exp_date = dtvehicledetails.Rows[0]["vm_rcexpdate"].ToString();
                    string vm_isurence_exp_date = dtvehicledetails.Rows[0]["vm_isurence_exp_date"].ToString();
                    string vm_poll_exp_date = dtvehicledetails.Rows[0]["vm_poll_exp_date"].ToString();
                    string vm_fit_exp_date = dtvehicledetails.Rows[0]["vm_fit_exp_date"].ToString();
                    string vm_roadtax_exp_date = dtvehicledetails.Rows[0]["vm_roadtax_exp_date"].ToString();
                    string state_permit_exp_date = dtvehicledetails.Rows[0]["state_permit_exp_date"].ToString();
                    string state_roadtax_exp_date = dtvehicledetails.Rows[0]["state_roadtax_exp_date"].ToString();
                    if (permit_exp_date == "")
                    {
                        permit_exp_date = null;
                    }
                    else
                    {
                        permit_exp_date = ((DateTime)dtvehicledetails.Rows[0]["vm_rcexpdate"]).ToString("yyyy-MM-dd");
                    }
                    if (vm_isurence_exp_date == "")
                    {
                        vm_isurence_exp_date = null;
                    }
                    else
                    {
                        vm_isurence_exp_date = ((DateTime)dtvehicledetails.Rows[0]["vm_isurence_exp_date"]).ToString("yyyy-MM-dd");
                    }
                    if (vm_poll_exp_date == "")
                    {
                        vm_poll_exp_date = null;
                    }
                    else
                    {
                        vm_poll_exp_date = ((DateTime)dtvehicledetails.Rows[0]["vm_poll_exp_date"]).ToString("yyyy-MM-dd");
                    }
                    if (vm_fit_exp_date == "")
                    {
                        vm_fit_exp_date = null;
                    }
                    else
                    {
                        vm_fit_exp_date = ((DateTime)dtvehicledetails.Rows[0]["vm_fit_exp_date"]).ToString("yyyy-MM-dd");
                    }
                    if (vm_roadtax_exp_date == "")
                    {
                        vm_roadtax_exp_date = null;
                    }
                    else
                    {
                        vm_roadtax_exp_date = ((DateTime)dtvehicledetails.Rows[0]["vm_roadtax_exp_date"]).ToString("yyyy-MM-dd");
                    }
                    if (state_permit_exp_date == "")
                    {
                        state_permit_exp_date = null;
                    }
                    else
                    {
                        state_permit_exp_date = ((DateTime)dtvehicledetails.Rows[0]["state_permit_exp_date"]).ToString("yyyy-MM-dd");
                    }
                    if (state_roadtax_exp_date == "")
                    {
                        state_roadtax_exp_date = null;
                    }
                    else
                    {
                        state_roadtax_exp_date = ((DateTime)dtvehicledetails.Rows[0]["state_roadtax_exp_date"]).ToString("yyyy-MM-dd");
                    }
                    cmd = new MySqlCommand("insert into vehicle_exp_doc_logs (vehicleno,permit_expdate,pol_expdate,ins_expdate,fitness_expdate,roadtax_expdate,state_permit_expdate,state_roadtax_expdate,branch_sno,operated_by) values(@vehicleno,@permit_expdate,@pol_expdate,@ins_expdate,@fitness_expdate,@roadtax_expdate,@state_permit_expdate,@state_roadtax_expdate,@branch_sno,@operated_by)");
                    cmd.Parameters.Add("@vehicleno", sno);
                    cmd.Parameters.Add("@permit_expdate", permit_exp_date);
                    cmd.Parameters.Add("@pol_expdate", vm_poll_exp_date);
                    cmd.Parameters.Add("@ins_expdate", vm_isurence_exp_date);
                    cmd.Parameters.Add("@fitness_expdate", vm_fit_exp_date);
                    cmd.Parameters.Add("@roadtax_expdate", vm_roadtax_exp_date);
                    cmd.Parameters.Add("@state_permit_expdate", state_permit_exp_date);
                    cmd.Parameters.Add("@state_roadtax_expdate", state_roadtax_exp_date);
                    cmd.Parameters.Add("@branch_sno", context.Session["Branch_ID"]);
                    cmd.Parameters.Add("@operated_by", context.Session["Employ_Sno"]);
                    vdm.insert(cmd);
                }
                if (axilmaster != "" && axilmaster != null)
                {
                    cmd = new MySqlCommand("update vehicel_master set whcode=@whcode, Petroledger_code=@Petroledger_code,ledger_code=@ledger_code, petroledgername=@petroledgername, ledgername=@ledgername, batteryno=@batteryno,batteryno2=@batteryno2, statename=@statename,state_permit_exp_date=@state_permit_exp_date,state_roadtax=@state_roadtax,state_roadtax_exp_date=@state_roadtax_exp_date, act_mileage=@act_mileage, registration_no=@registration_no,axils_refno=@axils_refno,vhtype_refno=@vhtype_refno, vhmake_refno=@vhmake_refno,door_no=@door_no,Capacity=@Capacity,status=@status,insurancesno=@insurancesno,vm_model=@vm_model, vm_engine=@vm_engine, vm_chasiss=@vm_chasiss, vm_owner=@vm_owner, vm_owneraddr=@vm_owneraddr, vm_rcno=@vm_rcno, vm_rcexpdate=@vm_rcexpdate, vm_pollution=@vm_pollution,vm_poll_exp_date=@vm_poll_exp_date, vm_insurance=@vm_insurance, vm_isurence_exp_date=@vm_isurence_exp_date, vm_fitness=@vm_fitness, vm_fit_exp_date=@vm_fit_exp_date, vm_roadtax=@vm_roadtax, vm_roadtax_exp_date=@vm_roadtax_exp_date,fuel_capacity=@fuel_capacity where vm_sno=@vm_sno");
                }
                else
                {
                    cmd = new MySqlCommand("update vehicel_master set whcode=@whcode, Petroledger_code=@Petroledger_code,ledger_code=@ledger_code, petroledgername=@petroledgername, ledgername=@ledgername, batteryno=@batteryno,batteryno2=@batteryno2, statename=@statename,state_permit_exp_date=@state_permit_exp_date,state_roadtax=@state_roadtax,state_roadtax_exp_date=@state_roadtax_exp_date, act_mileage=@act_mileage, registration_no=@registration_no,vhtype_refno=@vhtype_refno, vhmake_refno=@vhmake_refno,door_no=@door_no,Capacity=@Capacity,status=@status,insurancesno=@insurancesno,vm_model=@vm_model, vm_engine=@vm_engine, vm_chasiss=@vm_chasiss, vm_owner=@vm_owner, vm_owneraddr=@vm_owneraddr, vm_rcno=@vm_rcno, vm_rcexpdate=@vm_rcexpdate, vm_pollution=@vm_pollution,vm_poll_exp_date=@vm_poll_exp_date, vm_insurance=@vm_insurance, vm_isurence_exp_date=@vm_isurence_exp_date, vm_fitness=@vm_fitness, vm_fit_exp_date=@vm_fit_exp_date, vm_roadtax=@vm_roadtax, vm_roadtax_exp_date=@vm_roadtax_exp_date,fuel_capacity=@fuel_capacity where vm_sno=@vm_sno");
                }
                cmd.Parameters.Add("@whcode", whscode);
                cmd.Parameters.Add("@vhtype_refno", vehtype);
                cmd.Parameters.Add("@registration_no", regno);
                cmd.Parameters.Add("@door_no", doorno);
                cmd.Parameters.Add("@status", status);
                cmd.Parameters.Add("@Capacity", Capacity);
                cmd.Parameters.Add("@vm_sno", sno);
                cmd.Parameters.Add("@vm_model", model);
                cmd.Parameters.Add("@vm_engine", engineno);
                cmd.Parameters.Add("@vm_chasiss", chasiss);
                cmd.Parameters.Add("@vm_owner", owner);
                cmd.Parameters.Add("@vm_owneraddr", address);
                cmd.Parameters.Add("@vm_rcno", rcno);
                cmd.Parameters.Add("@vm_rcexpdate", rcexp_date);
                cmd.Parameters.Add("@vm_pollution", pollution);
                cmd.Parameters.Add("@vm_poll_exp_date", pollexp_date);
                cmd.Parameters.Add("@vm_insurance", insurence);
                cmd.Parameters.Add("@vm_isurence_exp_date", insexp_date);
                cmd.Parameters.Add("@vm_fitness", fitness);
                cmd.Parameters.Add("@vm_fit_exp_date", fitexp_date);
                cmd.Parameters.Add("@vm_roadtax", roadtax);
                cmd.Parameters.Add("@vm_roadtax_exp_date", roadtaxexp_date);
                cmd.Parameters.Add("@vhmake_refno", vehicle_make);
                cmd.Parameters.Add("@fuel_capacity", fuel_capacity);
                cmd.Parameters.Add("@axils_refno", axilmaster);
                cmd.Parameters.Add("@act_mileage", Actualmileage);
                cmd.Parameters.Add("@insurancesno", InsuranceSno);
                cmd.Parameters.Add("@statename", permitstatename);
                cmd.Parameters.Add("@state_permit_exp_date", permit_state_expdate);
                cmd.Parameters.Add("@state_roadtax", stateroadtax);
                cmd.Parameters.Add("@state_roadtax_exp_date", stateroadtaxexp_date);
                cmd.Parameters.Add("@batteryno", Batteryno);
                cmd.Parameters.Add("@batteryno2", Batteryno2);
                cmd.Parameters.Add("@ledgername", ledgername);
                cmd.Parameters.Add("@petroledgername", petro_ledgername);
                cmd.Parameters.Add("@ledger_code", ledgercode);
                cmd.Parameters.Add("@Petroledger_code", petro_ledgercode);
                vdm.Update(cmd);
                if (Odometer != "" && axilmaster != "" && axil_name_check == "NotDone")
                {
                    foreach (Axils_items art in Product_list)
                    {
                        foreach (Axils_sub_items obj in art.innertable_array)
                        {
                            cmd = new MySqlCommand("insert into vehicle_master_sub (vehicle_mstr_sno, axles_tyres_names_sno, tyre_sno,Odometer) values (@vehicle_mstr_sno, @axles_tyres_names_sno, @tyre_sno,@Odometer)");
                            cmd.Parameters.Add("@vehicle_mstr_sno", sno);
                            cmd.Parameters.Add("@axles_tyres_names_sno", obj.tyre_position_sno);
                            cmd.Parameters.Add("@tyre_sno", obj.tyre_sno);
                            cmd.Parameters.Add("@Odometer", Odometer);
                            vdm.insert(cmd);
                            if (art.axilname != "Stephanie")
                            {
                                cmd = new MySqlCommand("update new_tyres_sub set Fitting_Type='F' where sno=@sno");
                                cmd.Parameters.Add("@sno", obj.tyre_sno);
                                vdm.Update(cmd);
                            }
                        }
                    }
                }
                string response = GetJson("UPDATE");
                context.Response.Write(response);
            }
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }
    public class get_axil_data
    {
        public string sno { set; get; }
        public string axil_name { set; get; }
        public string status { set; get; }
    }
    private void get_all_axils(HttpContext context)
    {
        try
        {
            cmd = new MySqlCommand("SELECT sno, axil_name,status, branch_id FROM axils_master");
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            List<get_axil_data> vendor = new List<get_axil_data>();
            foreach (DataRow dr in dt.Rows)
            {
                get_axil_data data = new get_axil_data();
                data.sno = dr["sno"].ToString();
                data.axil_name = dr["axil_name"].ToString();
                data.status = dr["status"].ToString();
                vendor.Add(data);
            }
            string response = GetJson(vendor);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }

    public class get_branch_data
    {
        public string brnch_sno { set; get; }
        public string branchname { set; get; }
        public string brnch_address { set; get; }
        public string brnch_mobno { set; get; }
        public string brnch_email { set; get; }
        public string brnch_status { set; get; }
        public string brnch_inward_start { set; get; }
        public string brnch_outward_start { set; get; }
        public string brnch_workorder_start { set; get; }
    }

    private void get_all_BranchName_data(HttpContext context)
    {
        try
        {
            // cmd = new MySqlCommand("SELECT brnch_sno, user_id, branchname, brnch_address, brnch_mobno, brnch_email, brnch_status,brnch_inward_start, brnch_outward_start, brnch_workorder_start FROM branch_info WHERE (user_id = @user_id)");
            // cmd.Parameters.Add("@user_id", context.Session["Employ_Sno"]);
            cmd = new MySqlCommand("SELECT brnch_sno, user_id, branchname, brnch_address, brnch_mobno, brnch_email, brnch_status,brnch_inward_start, brnch_outward_start, brnch_workorder_start FROM branch_info order by brnch_sno");
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            List<get_branch_data> vendor = new List<get_branch_data>();
            foreach (DataRow dr in dt.Rows)
            {
                get_branch_data data = new get_branch_data();
                data.brnch_sno = dr["brnch_sno"].ToString();
                data.branchname = dr["branchname"].ToString();
                data.brnch_address = dr["brnch_address"].ToString();
                data.brnch_mobno = dr["brnch_mobno"].ToString();
                data.brnch_email = dr["brnch_email"].ToString();
                data.brnch_status = dr["brnch_status"].ToString();
                data.brnch_inward_start = dr["brnch_inward_start"].ToString();
                data.brnch_outward_start = dr["brnch_outward_start"].ToString();
                data.brnch_workorder_start = dr["brnch_workorder_start"].ToString();
                vendor.Add(data);
            }
            string response = GetJson(vendor);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }

    private void save_Branch_data(HttpContext context)
    {
        try
        {
            string branchname = context.Request["branchname"];
            string address = context.Request["address"];
            string mobile = context.Request["mobile"];
            string desc = context.Request["desc"];
            string email = context.Request["email"];
            string status = context.Request["status"];
            string sno = context.Request["sno"];
            string btnval = context.Request["btnval"];
            string inwardno = context.Request["inwardno"];
            string outwardno = context.Request["outwardno"];
            string workorderno = context.Request["workorderno"];
            if (btnval == "Save")
            {
                cmd = new MySqlCommand("insert into branch_info (user_id, branchname, brnch_address, brnch_mobno, brnch_email,brnch_inward_start, brnch_outward_start, brnch_workorder_start) values (@user_id, @branchname, @brnch_address,@brnch_mobno, @brnch_email,@brnch_inward_start, @brnch_outward_start, @brnch_workorder_start)");
                cmd.Parameters.Add("@branchname", branchname);
                cmd.Parameters.Add("@brnch_address", address);
                cmd.Parameters.Add("@brnch_mobno", mobile);
                cmd.Parameters.Add("@brnch_email", email);
                cmd.Parameters.Add("@brnch_inward_start", inwardno);
                cmd.Parameters.Add("@brnch_outward_start", outwardno);
                cmd.Parameters.Add("@brnch_workorder_start", workorderno);
                cmd.Parameters.Add("@user_id", context.Session["Branch_ID"]);
                vdm.insert(cmd);
                string response = GetJson("OK");
                context.Response.Write(response);
            }
            else
            {
                cmd = new MySqlCommand("update branch_info set branchname=@branchname, brnch_address=@brnch_address, brnch_mobno=@brnch_mobno, brnch_email=@brnch_email, brnch_status=@status,brnch_inward_start=@brnch_inward_start, brnch_outward_start=@brnch_outward_start, brnch_workorder_start=@brnch_workorder_start where  brnch_sno=@sno");
                cmd.Parameters.Add("@branchname", branchname);
                cmd.Parameters.Add("@brnch_address", address);
                cmd.Parameters.Add("@brnch_mobno", mobile);
                cmd.Parameters.Add("@brnch_email", email);
                cmd.Parameters.Add("@status", status);
                cmd.Parameters.Add("@brnch_inward_start", inwardno);
                cmd.Parameters.Add("@brnch_outward_start", outwardno);
                cmd.Parameters.Add("@brnch_workorder_start", workorderno);
                cmd.Parameters.Add("@sno", sno);
                vdm.insert(cmd);
                string response = GetJson("UPDATE");
                context.Response.Write(response);
            }
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }

    private void save_cmpnydetails(HttpContext context)
    {
        try
        {
            string cmpny = context.Request["cmpny"];
            string mobileno = context.Request["mobileno"];
            string mail = context.Request["mail"];
            string addr = context.Request["addr"];
            string sno = context.Request["sno"];
            string btnval = context.Request["btnval"];
            cmd = new MySqlCommand("Update user_accounts set CompanyName=@CompanyName, Address=@Address, Email=@Email, PhoneNumber=@PhoneNumber where sno=@sno");
            cmd.Parameters.Add("@CompanyName", cmpny);
            cmd.Parameters.Add("@Address", addr);
            cmd.Parameters.Add("@Email", mail);
            cmd.Parameters.Add("@PhoneNumber", mobileno);
            cmd.Parameters.Add("@sno", context.Session["User_Sno"]);
            vdm.insert(cmd);
            string response = GetJson("OK");
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }

    public class all_part_num_data
    {
        public string sno { set; get; }
        public string partgroup { set; get; }
        public string pn_name { set; get; }
        public string pnum_Name { set; get; }
        public string pnum_desc { set; get; }
        public string RakeNum { set; get; }
        public string minimum_stock { set; get; }
        public string status { set; get; }
        public string rake { set; get; }
        public string availble_qty { set; get; }
        public string unitcost { set; get; }
    }

    private void get_all_part_number_data(HttpContext context)
    {
        try
        {
            string partgroup_sno = context.Request["partgroup_sno"];
            cmd = new MySqlCommand("SELECT partnum.sno, partnum.pnum_Name,partnum.unitcost, partnum.pnum_desc, partnum.minimum_stock, partnum.operatedby, partnum.branch_id, partnum.status, minimasters.mm_name AS PartGroup, partname.pn_name,minimasters_1.mm_name AS Rake, partnum.availble_qty FROM partnum INNER JOIN minimasters ON partnum.pg_sno = minimasters.sno INNER JOIN partname ON partnum.pn_sno = partname.sno INNER JOIN minimasters minimasters_1 ON partnum.RakeNum = minimasters_1.sno WHERE (partnum.branch_id = @branch_id)");
            cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            List<all_part_num_data> vendor = new List<all_part_num_data>();
            foreach (DataRow dr in dt.Rows)
            {
                all_part_num_data data = new all_part_num_data();
                data.sno = dr["sno"].ToString();
                data.partgroup = dr["PartGroup"].ToString();
                data.pn_name = dr["pn_name"].ToString();
                data.pnum_Name = dr["pnum_Name"].ToString();
                data.pnum_desc = dr["pnum_desc"].ToString();
                data.rake = dr["Rake"].ToString();
                data.minimum_stock = dr["minimum_stock"].ToString();
                data.status = dr["status"].ToString();
                data.availble_qty = dr["availble_qty"].ToString();
                data.unitcost = dr["unitcost"].ToString();
                vendor.Add(data);
            }

            string response = GetJson(vendor);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }

    private void save_part_number(HttpContext context)
    {
        try
        {
            string partgroup_sno = context.Request["partgroup_sno"];
            string partname_sno = context.Request["partname_sno"];
            string partnumbername = context.Request["partnumbername"];
            string desc = context.Request["desc"];
            string rake = context.Request["rake"];
            string minstock = context.Request["minstock"];
            string status = context.Request["status"];
            string sno = context.Request["sno"];
            string btnval = context.Request["btnval"];
            string availstock = context.Request["availstock"];
            string unitcost = context.Request["unitcost"];
            if (btnval == "Save")
            {
                cmd = new MySqlCommand("insert into partnum (pg_sno, pn_sno, pnum_Name, pnum_desc, RakeNum, minimum_stock, operatedby, branch_id,availble_qty,unitcost) values (@pg_sno, @pn_sno, @pnum_Name, @pnum_desc, @RakeNum, @minimum_stock, @operatedby, @branch_id,@availble_qty,@unitcost)");
                cmd.Parameters.Add("@pg_sno", partgroup_sno);
                cmd.Parameters.Add("@pn_sno", partname_sno);
                cmd.Parameters.Add("@pnum_Name", partnumbername);
                cmd.Parameters.Add("@pnum_desc", desc);
                cmd.Parameters.Add("@RakeNum", rake);
                cmd.Parameters.Add("@minimum_stock", minstock);
                cmd.Parameters.Add("@operatedby", context.Session["Employ_Sno"]);
                cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
                cmd.Parameters.Add("@availble_qty", availstock);
                cmd.Parameters.Add("@unitcost", unitcost);
                vdm.insert(cmd);
                string response = GetJson("OK");
                context.Response.Write(response);
            }
            else
            {
                cmd = new MySqlCommand("update partnum set pg_sno=@pg_sno, pn_sno=@pn_sno, pnum_Name=@pnum_Name, pnum_desc=@pnum_desc, RakeNum=@RakeNum, minimum_stock=@minimum_stock, status=@status,availble_qty=@availble_qty,unitcost=@unitcost where sno=@sno");
                cmd.Parameters.Add("@pg_sno", partgroup_sno);
                cmd.Parameters.Add("@pn_sno", partname_sno);
                cmd.Parameters.Add("@pnum_Name", partnumbername);
                cmd.Parameters.Add("@pnum_desc", desc);
                cmd.Parameters.Add("@RakeNum", rake);
                cmd.Parameters.Add("@minimum_stock", minstock);
                cmd.Parameters.Add("@status", status);
                cmd.Parameters.Add("@availble_qty", availstock);
                cmd.Parameters.Add("@unitcost", unitcost);
                cmd.Parameters.Add("@sno", sno);
                vdm.insert(cmd);
                string response = GetJson("UPDATE");
                context.Response.Write(response);
            }
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }

    public class all_partnames
    {
        public string sno { set; get; }
        public string pn_name { set; get; }
        public string MM_sno { set; get; }
        public string Maintainance { set; get; }
        public string timeperiod { set; get; }
        public string distance { set; get; }
        public string pn_desc { set; get; }
        public string pn_shorname { set; get; }
        public string units { set; get; }
        public string branch_id { set; get; }
    }

    private void get_Part_NAme_data(HttpContext context)
    {
        try
        {
            string partgroup_sno = context.Request["partgroup_sno"];
            cmd = new MySqlCommand("SELECT sno, pn_name, MM_sno, Maintainance, timeperiod, distance, pn_desc, pn_shorname, operatedby, branch_id FROM partname WHERE (branch_id = @branch_id) AND (MM_sno = @MM_sno)");
            cmd.Parameters.Add("@MM_sno", partgroup_sno);
            cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            List<all_partnames> vendor = new List<all_partnames>();
            foreach (DataRow dr in dt.Rows)
            {
                all_partnames data = new all_partnames();
                data.sno = dr["sno"].ToString();
                data.pn_name = dr["pn_name"].ToString();
                data.MM_sno = dr["MM_sno"].ToString();
                data.Maintainance = dr["Maintainance"].ToString();
                data.timeperiod = dr["timeperiod"].ToString();
                data.distance = dr["distance"].ToString();
                data.pn_desc = dr["pn_desc"].ToString();
                data.pn_shorname = dr["pn_shorname"].ToString();
                vendor.Add(data);
            }
            string response = GetJson(vendor);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }
    public class all_MM_DATA
    {
        public string sno { get; set; }
        public string pn_name { get; set; }
        public string mm_name { get; set; }
        public string pn_desc { get; set; }
    }

    private void get_all_partname_data(HttpContext context)
    {
        try
        {
            cmd = new MySqlCommand("SELECT partname.sno, partname.pn_name, partname.Maintainance, partname.timeperiod, partname.distance, partname.pn_desc, partname.pn_shorname, partname.operatedby, partname.units, partname.branch_id,  minimasters.mm_name FROM partname INNER JOIN minimasters ON partname.MM_sno = minimasters.sno WHERE (partname.branch_id = @branch_id)");
            cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            List<all_MM_DATA> vendor = new List<all_MM_DATA>();
            foreach (DataRow dr in dt.Rows)
            {
                all_MM_DATA data = new all_MM_DATA();
                data.sno = dr["sno"].ToString();
                data.pn_name = dr["pn_name"].ToString();
                data.mm_name = dr["mm_name"].ToString();
                data.pn_desc = dr["pn_desc"].ToString();
                vendor.Add(data);
            }
            string response = GetJson(vendor);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }

    private void save_partname(HttpContext context)
    {
        try
        {
            string partgroup_sno = context.Request["partgroup_sno"];
            string partname = context.Request["partname"];
            string partdesc = context.Request["partdesc"];
            string sno = context.Request["sno"];
            string btnval = context.Request["btnval"];
            if (btnval == "Save")
            {
                cmd = new MySqlCommand("insert into partname (pn_name, MM_sno,pn_desc,operatedby,branch_id) values (@pn_name, @MM_sno,@pn_desc,@operatedby,@branch_id)");
                cmd.Parameters.Add("@pn_name", partname);
                cmd.Parameters.Add("@MM_sno", partgroup_sno);
                cmd.Parameters.Add("@pn_desc", partdesc);
                cmd.Parameters.Add("@operatedby", context.Session["Employ_Sno"]);
                cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
                vdm.insert(cmd);
                string response = GetJson("OK");
                context.Response.Write(response);
            }
            else
            {
                cmd = new MySqlCommand("update partname set pn_name=@pn_name, MM_sno=@MM_sno,pn_desc=@pn_desc where sno=@sno");
                cmd.Parameters.Add("@pn_name", partname);
                cmd.Parameters.Add("@MM_sno", partgroup_sno);
                cmd.Parameters.Add("@pn_desc", partdesc);
                cmd.Parameters.Add("@sno", sno);
                vdm.insert(cmd);
                string response = GetJson("UPDATE");
                context.Response.Write(response);
            }
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }

    private void for_save_edit_MinMaster(HttpContext context)
    {
        try
        {
            string master_type = context.Request["master_type"];
            string master_name = context.Request["master_name"];
            string status = context.Request["status"];
            string sno = context.Request["sno"];
            string btnval = context.Request["btnval"];
            if (btnval == "Save")
            {
                cmd = new MySqlCommand("insert into minimasters (mm_type, mm_name, mm_status, branch_id) values (@mm_type, @mm_name, @mm_status, @branch_id)");
                cmd.Parameters.Add("@mm_type", master_type);
                cmd.Parameters.Add("@mm_name", master_name);
                cmd.Parameters.Add("@mm_status", status);
                cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
                vdm.insert(cmd);
                string response = GetJson("OK");
                context.Response.Write(response);
            }
            else
            {
                cmd = new MySqlCommand("update minimasters set mm_type=@mm_type, mm_name=@mm_name, mm_status=@mm_status where sno=@sno");
                cmd.Parameters.Add("@mm_type", master_type);
                cmd.Parameters.Add("@mm_name", master_name);
                cmd.Parameters.Add("@mm_status", status);
                cmd.Parameters.Add("@sno", sno);
                vdm.Update(cmd);
                string response = GetJson("UPDATE");
                context.Response.Write(response);
            }
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }
    public class get_Minimaster_details
    {
        public string sno { get; set; }
        public string mm_type { get; set; }
        public string mm_name { get; set; }
        public string mm_status { get; set; }
    }
    private void get_Mini_Master_data(HttpContext context)
    {
        try
        {
            string minimaster = context.Request["minimaster"];
            string queryminimast = "";
            string[] splitminimaster = minimaster.Split(',');
            foreach (string mini_req in splitminimaster)
            {
                queryminimast += "'" + mini_req + "',";
            }
            queryminimast = queryminimast.Substring(0, queryminimast.LastIndexOf(','));
            cmd = new MySqlCommand("SELECT sno, mm_type, mm_name, mm_status, branch_id FROM minimasters WHERE mm_type in (" + queryminimast + ") ");
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            List<get_Minimaster_details> vendor = new List<get_Minimaster_details>();
            foreach (DataRow dr in dt.Rows)
            {
                get_Minimaster_details data = new get_Minimaster_details();
                data.sno = dr["sno"].ToString();
                data.mm_type = dr["mm_type"].ToString();
                data.mm_name = dr["mm_name"].ToString();
                data.mm_status = dr["mm_status"].ToString();
                vendor.Add(data);
            }
            string response = GetJson(vendor);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson("Error Occured :(");
            context.Response.Write(response);
        }
    }
    public class get_Partgroup_details
    {
        public string sno { get; set; }
        public string pg_name { get; set; }
        public string pg_desc { get; set; }
        public string status { get; set; }
        public string short_name { get; set; }
    }
    private void get_all_partgroups(HttpContext context)
    {
        try
        {
            cmd = new MySqlCommand("SELECT sno, pg_name, pg_desc, status, short_name, branch_id FROM partgroup");
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            List<get_Partgroup_details> vendor = new List<get_Partgroup_details>();
            foreach (DataRow dr in dt.Rows)
            {
                get_Partgroup_details data = new get_Partgroup_details();
                data.sno = dr["sno"].ToString();
                data.pg_name = dr["pg_name"].ToString();
                data.pg_desc = dr["pg_desc"].ToString();
                data.status = dr["status"].ToString();
                data.short_name = dr["short_name"].ToString();
                vendor.Add(data);
            }
            string response = GetJson(vendor);
            context.Response.Write(response);

        }
        catch (Exception ex)
        {
            string response = GetJson("Error Occured :(");
            context.Response.Write(response);
        }
    }
    private void for_save_edit_PArtGroup(HttpContext context)
    {
        try
        {
            string partgroup = context.Request["partgroup"];
            string partgrpdesc = context.Request["partgrpdesc"];
            string status = context.Request["status"];
            string sno = context.Request["sno"];
            string btnval = context.Request["btnval"];
            if (btnval == "Save")
            {
                cmd = new MySqlCommand("insert into partgroup (pg_name, pg_desc,branch_id) values (@pg_name, @pg_desc,@branch_id)");
                cmd.Parameters.Add("@pg_name", partgroup);
                cmd.Parameters.Add("@pg_desc", partgrpdesc);
                cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
                vdm.insert(cmd);
                string response = GetJson("OK");
                context.Response.Write(response);
            }
            else
            {
                cmd = new MySqlCommand("update partgroup set pg_name=@pg_name,pg_desc=@pg_desc,status=@status where sno=@sno");
                cmd.Parameters.Add("@pg_name", partgroup);
                cmd.Parameters.Add("@pg_desc", partgrpdesc);
                cmd.Parameters.Add("@status", status);
                cmd.Parameters.Add("@sno", sno);
                vdm.Update(cmd);
                string response = GetJson("UPDATE");
                context.Response.Write(response);
            }
        }
        catch (Exception ex)
        {
            string response = GetJson("Error Occured :(");
            context.Response.Write(response);
        }
    }


    public class get_cc_details
    {
        public string sno { get; set; }
        public string cct_sno { get; set; }
        public string cc_name { get; set; }
        public string cc_desc { get; set; }
        public string cc_type { get; set; }
        public string chasissName { get; set; }
        public string dop { get; set; }
        public string cc_status { get; set; }
        public string fuel_capacity { get; set; }
    }
    //private void get_CC_details(HttpContext context)
    //{
    //    try
    //    {
    //        cmd = new MySqlCommand("SELECT * FROM costcenter");
    //        DataTable dt = vdm.SelectQuery(cmd).Tables[0];
    //        List<get_cc_details> vendor = new List<get_cc_details>();
    //        foreach (DataRow dr in dt.Rows)
    //        {
    //            get_cc_details data = new get_cc_details();
    //            data.sno = dr["sno"].ToString();
    //            data.cct_sno = dr["cct_sno"].ToString();
    //            data.cc_name = dr["cc_name"].ToString();
    //            data.cc_desc = dr["cc_desc"].ToString();
    //            data.cc_type = dr["cc_type"].ToString();
    //            data.chasissName = dr["chasissName"].ToString();
    //            data.dop = ((DateTime)dr["dop"]).ToString("dd/MM/yyyy");
    //            data.cc_status = dr["cc_status"].ToString();
    //            data.fuel_capacity = dr["cc_fuel_capacity"].ToString();
    //            vendor.Add(data);
    //        }
    //        string response = GetJson(vendor);
    //        context.Response.Write(response);

    //    }
    //    catch (Exception ex)
    //    {
    //        string response = GetJson(ex.Message);
    //        context.Response.Write(response);
    //    }
    //}

    //private void for_save_edit_CostCenter(HttpContext context)
    //{
    //    try
    //    {

    //        //'for_save_edit_CostCenter', 'ccname': ccname, 'cc_desc': ccdesc, 'cctype': cctype, 'cc_chsis': cc_chsNum, 'dop': cc_dop  };
    //        //string ccname = context.Request["ccname"];
    //        string ccname = context.Request["ccname"];
    //        string ccdesc = context.Request["cc_desc"];
    //        string cctype = context.Request["cctype"];
    //        string cc_dop = context.Request["dop"];
    //        string cc_chsis = context.Request["cc_chsis"];
    //        string btnval = context.Request["btnval"];
    //        string status = context.Request["status"];
    //        string cc_fuel_capacity = context.Request["capacity"];
    //        string sno = context.Request["sno"];
    //        DateTime dop = DateTime.Now;
    //        dop = DateTime.ParseExact(cc_dop, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
    //        if (btnval == "Save")
    //        {
    //            cmd = new MySqlCommand("insert into costcenter (cct_sno,cc_name, cc_desc, cc_type, chasissName, dop,cc_fuel_capacity, branch_id,operatedby) values (1, @cc_name, @cc_desc, @cc_type, @chasissName, @dop,@cc_fuel_capacity, @branch_id,@operatedby)");
    //            cmd.Parameters.Add("@cct_sno", cctype);
    //            cmd.Parameters.Add("@cc_name", ccname);
    //            cmd.Parameters.Add("@cc_desc", ccdesc);
    //            cmd.Parameters.Add("@cc_type", cctype);
    //            cmd.Parameters.Add("@chasissName", cc_chsis);
    //            cmd.Parameters.Add("@cc_fuel_capacity", cc_fuel_capacity);
    //            cmd.Parameters.Add("@dop", dop);
    //            cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
    //            cmd.Parameters.Add("@operatedby", context.Session["Employ_Sno"]);
    //            vdm.insert(cmd);
    //            string response = GetJson("OK");
    //            context.Response.Write(response);
    //        }
    //        else
    //        {
    //            cmd = new MySqlCommand("update costcenter set cct_sno=@cct_sno,cc_name=@cc_name, cc_desc=@cc_desc, cc_type=@cc_type, chasissName=@chasissName, dop=@dop,cc_fuel_capacity=@cc_fuel_capacity,cc_status=@cc_status where sno=@sno");
    //            cmd.Parameters.Add("@cct_sno", 1);
    //            cmd.Parameters.Add("@cc_name", ccname);
    //            cmd.Parameters.Add("@cc_desc", ccdesc);
    //            cmd.Parameters.Add("@cc_type", cctype);
    //            cmd.Parameters.Add("@chasissName", cc_chsis);
    //            cmd.Parameters.Add("@cc_fuel_capacity", cc_fuel_capacity);
    //            cmd.Parameters.Add("@dop", dop);
    //            cmd.Parameters.Add("@cc_status", status);
    //            cmd.Parameters.Add("@sno", sno);
    //            vdm.Update(cmd);
    //            string response = GetJson("UPDATE");
    //            context.Response.Write(response);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        string response = GetJson("Error Occured :(");
    //        context.Response.Write(response);
    //    }
    //}
    public class vendor_data
    {
        public string sno { get; set; }
        public string vendor_code { get; set; }
        public string vendorname { get; set; }
        public string vendor_address { get; set; }
        public string vendor_email { get; set; }
        public string vendor_mob { get; set; }
        public string vendor_tin { get; set; }
        public string vendor_vat { get; set; }
        public string vendor_cst { get; set; }
        public string vendor_stax { get; set; }
        public string vendor_type { get; set; }
        public string stateid { get; set; }
        public string gstin { get; set; }
    }

    private void get_vendor_details(HttpContext context)
    {
        try
        {
            string FormName = context.Request["FormName"];
            if (FormName == "Tyres")
            {
                cmd = new MySqlCommand("SELECT stateid,gstin, sno, vendor_code, vendorname, vendor_address, vendor_email, vendor_mob, vendor_tin, vendor_vat, vendor_cst, vendor_stax, branch_id, operatedby, vendor_status,vendor_type FROM vendors_info WHERE  (branch_id = @branch_id) AND (vendor_type<>'Insurance')");
            }
            if (FormName == "VendorNames")
            {
                cmd = new MySqlCommand("SELECT stateid,gstin, sno, vendor_code, vendorname, vendor_address, vendor_email, vendor_mob, vendor_tin, vendor_vat, vendor_cst, vendor_stax, branch_id, operatedby, vendor_status,vendor_type FROM vendors_info WHERE  (branch_id = @branch_id) ");
            }
            cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            List<vendor_data> vendor = new List<vendor_data>();
            foreach (DataRow dr in dt.Rows)
            {
                vendor_data data = new vendor_data();
                data.sno = dr["sno"].ToString();
                data.vendor_code = dr["vendor_code"].ToString();
                data.vendorname = dr["vendorname"].ToString();
                data.vendor_address = dr["vendor_address"].ToString();
                data.vendor_email = dr["vendor_email"].ToString();
                data.vendor_mob = dr["vendor_mob"].ToString();
                data.vendor_tin = dr["vendor_tin"].ToString();
                data.vendor_vat = dr["vendor_vat"].ToString();
                data.vendor_cst = dr["vendor_cst"].ToString();
                data.vendor_stax = dr["vendor_stax"].ToString();
                data.vendor_type = dr["vendor_type"].ToString();
                data.stateid = dr["stateid"].ToString();
                data.gstin = dr["gstin"].ToString();
                vendor.Add(data);
            }
            string response = GetJson(vendor);
            context.Response.Write(response);

        }
        catch (Exception ex)
        {
            string response = GetJson(ex.ToString());
            context.Response.Write(response);
        }
    }

    private void for_save_edit_vendor(HttpContext context)
    {
        try
        {
            string vendorname = context.Request["vendorname"];
            string vendorcode = context.Request["vendorcode"];
            string addr = context.Request["addr"];
            string email = context.Request["email"];
            string phoneno = context.Request["phoneno"];
            string tan = context.Request["tan"];
            string vat = context.Request["vat"];
            string cst = context.Request["cst"];
            string btnval = context.Request["btnval"];
            string servtax = context.Request["servtax"];
            string sno = context.Request["sno"];
            string vendor_type = context.Request["vendor_type"];
            string tinno = context.Request["tinno"];
            string panno = context.Request["panno"];
            string gst = context.Request["gst"];
            string state = context.Request["state"];
            if (btnval == "Save")
            {
                cmd = new MySqlCommand("insert into vendors_info (vendor_code, vendorname, vendor_address, vendor_email, vendor_mob, vendor_tin, vendor_vat, vendor_cst, vendor_stax, branch_id, operatedby,vendor_type,tinno,panno,gstin,stateid) values (@vendor_code, @vendorname, @vendor_address, @vendor_email, @vendor_mob, @vendor_tin, @vendor_vat, @vendor_cst, @vendor_stax, @branch_id, @operatedby,@vendor_type,@tinno,@panno,@gstin,@stateid)");
                cmd.Parameters.Add("@vendorname", vendorname);
                cmd.Parameters.Add("@vendor_code", vendorcode);
                cmd.Parameters.Add("@vendor_address", addr);
                cmd.Parameters.Add("@vendor_email", email);
                cmd.Parameters.Add("@vendor_mob", phoneno);
                cmd.Parameters.Add("@vendor_tin", tan);
                cmd.Parameters.Add("@vendor_vat", vat);
                cmd.Parameters.Add("@vendor_cst", cst);
                cmd.Parameters.Add("@vendor_stax", servtax);
                cmd.Parameters.Add("@vendor_type", vendor_type);
                cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
                cmd.Parameters.Add("@operatedby", context.Session["Employ_Sno"]);
                cmd.Parameters.Add("@tinno", tinno);
                cmd.Parameters.Add("@panno", panno);
                cmd.Parameters.Add("@gstin", gst);
                cmd.Parameters.Add("@stateid", state);
                vdm.insert(cmd);
                string response = GetJson("OK");
                context.Response.Write(response);
            }
            else
            {
                cmd = new MySqlCommand("update vendors_info set gstin=@gstin,stateid=@stateid, vendor_code=@vendor_code,panno=@panno,tinno=@tinno, vendorname=@vendorname, vendor_address=@vendor_address, vendor_email=@vendor_email, vendor_mob=@vendor_mob, vendor_tin=@vendor_tin, vendor_vat=@vendor_vat, vendor_cst=@vendor_cst, vendor_stax=@vendor_stax,vendor_type=@vendor_type where sno=@sno");
                cmd.Parameters.Add("@vendorname", vendorname);
                cmd.Parameters.Add("@vendor_code", vendorcode);
                cmd.Parameters.Add("@vendor_address", addr);
                cmd.Parameters.Add("@vendor_email", email);
                cmd.Parameters.Add("@vendor_mob", phoneno);
                cmd.Parameters.Add("@vendor_tin", tan);
                cmd.Parameters.Add("@vendor_vat", vat);
                cmd.Parameters.Add("@vendor_cst", cst);
                cmd.Parameters.Add("@vendor_stax", servtax);
                cmd.Parameters.Add("@vendor_type", vendor_type);
                cmd.Parameters.Add("@sno", sno);
                cmd.Parameters.Add("@tinno", tinno);
                cmd.Parameters.Add("@panno", panno);
                cmd.Parameters.Add("@gstin", gst);
                cmd.Parameters.Add("@stateid", state);
                vdm.Update(cmd);
                string response = GetJson("UPDATE");
                context.Response.Write(response);
            }
        }
        catch (Exception ex)
        {
            string response = GetJson("Error Occured :(");
            context.Response.Write(response);
        }
    }

    public class get_useraccounts
    {
        public string sno { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Status { get; set; }
    }

    private void getcompanydetails(HttpContext context)
    {
        try
        {
            cmd = new MySqlCommand("SELECT sno, UserName, Password, CompanyName, Address, Email, PhoneNumber, Status FROM user_accounts where Status='1' and sno=@sno");
            cmd.Parameters.Add("@sno", context.Session["User_Sno"]);
            DataTable routes = vdm.SelectQuery(cmd).Tables[0];
            List<get_useraccounts> getroutesclslst = new List<get_useraccounts>();
            foreach (DataRow dr in routes.Rows)
            {
                get_useraccounts getroutes = new get_useraccounts();
                getroutes.sno = dr["sno"].ToString();
                getroutes.UserName = dr["UserName"].ToString();
                getroutes.Password = dr["Password"].ToString();
                getroutes.CompanyName = dr["CompanyName"].ToString();
                getroutes.Address = dr["Address"].ToString();
                getroutes.Email = dr["Email"].ToString();
                getroutes.PhoneNumber = dr["PhoneNumber"].ToString();
                getroutes.Status = dr["Status"].ToString();
                getroutesclslst.Add(getroutes);
            }
            string response = GetJson(getroutesclslst);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.ToString());
            context.Response.Write(response);
        }
    }
    private void getroutes(HttpContext context)
    {
        try
        {
            cmd = new MySqlCommand("select sno,routename,status,branch_id,operatedby,ledgername,mobileno from routes where branch_id=@branch_id");
            cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
            DataTable routes = vdm.SelectQuery(cmd).Tables[0];
            List<getroutescls> getroutesclslst = new List<getroutescls>();
            foreach (DataRow dr in routes.Rows)
            {
                getroutescls getroutes = new getroutescls();
                getroutes.routename = dr["routename"].ToString();
                getroutes.status = dr["status"].ToString();
                getroutes.ledgername = dr["ledgername"].ToString();
                getroutes.sno = dr["sno"].ToString();
                getroutes.mobileno = dr["mobileno"].ToString();
                getroutesclslst.Add(getroutes);
            }
            string response = GetJson(getroutesclslst);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.ToString());
            context.Response.Write(response);
        }
    }
    private void retrivestarttripperameters(HttpContext context)
    {
        try
        {
            getstarttripperamtrscls getstarttripprmtrscls = new getstarttripperamtrscls();
            List<routescls> getrouteslst = new List<routescls>();
            List<driverscls> getdriverslst = new List<driverscls>();
            List<vehiclescls> getvehicleslst = new List<vehiclescls>();
            cmd = new MySqlCommand("SELECT sno, routename, branch_id FROM routes WHERE (branch_id = @branch_id) and (status='1')");
            cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
            DataTable routes = vdm.SelectQuery(cmd).Tables[0];
            foreach (DataRow dr in routes.Rows)
            {
                routescls getroutes = new routescls();
                getroutes.routename = dr["routename"].ToString();
                getroutes.routesno = dr["sno"].ToString();
                getrouteslst.Add(getroutes);
            }
            getstarttripprmtrscls.routes = getrouteslst;
            cmd = new MySqlCommand("SELECT emp_sno, employname FROM employdata WHERE (emp_type = @emp_type) AND (emp_status = '1') and (branch_id = @branch_id)");
            cmd.Parameters.Add("@emp_type", "Driver");
            cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
            DataTable drivers = vdm.SelectQuery(cmd).Tables[0];
            foreach (DataRow dr in drivers.Rows)
            {
                driverscls getdrvrs = new driverscls();
                getdrvrs.drivername = dr["employname"].ToString();
                getdrvrs.driversno = dr["emp_sno"].ToString();
                getdriverslst.Add(getdrvrs);
            }
            getstarttripprmtrscls.drivers = getdriverslst;

            //cmd = new MySqlCommand("SELECT sno, cc_name FROM costcenter WHERE (cc_type = 1) AND (cc_status = 1) and (branch_id = @branch_id)");
            //cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
            //DataTable vehicles = vdm.SelectQuery(cmd).Tables[0];
            //foreach (DataRow dr in vehicles.Rows)
            //{
            //    vehiclescls vehicle = new vehiclescls();
            //    vehicle.vehiclenum = dr["cc_name"].ToString();
            //    vehicle.vehiclesno = dr["sno"].ToString();
            //    getvehicleslst.Add(vehicle);
            //}
            //getstarttripprmtrscls.vehicles = getvehicleslst;

            string response = GetJson(getstarttripprmtrscls);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.ToString());
            context.Response.Write(response);
        }
    }
    private void save_route(routesave obj, HttpContext context)
    {
        try
        {
            if (obj.btnval == "Save")
            {
                cmd = new MySqlCommand("insert into routes (routename,status,branch_id,operatedby,ledgername,mobileno) values (@routename,@status,@branch_id,@operatedby,@ledgername,@mobileno)");
                cmd.Parameters.Add("@routename", obj.routename);
                cmd.Parameters.Add("@status", obj.status);
                cmd.Parameters.Add("@ledgername", obj.ledgername);
                cmd.Parameters.Add("@mobileno", obj.mobileno);
                cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
                cmd.Parameters.Add("@operatedby", context.Session["Employ_Sno"]);
                vdm.insert(cmd);
                string response = GetJson("Route saved successfully");
                context.Response.Write(response);
            }
            else if (obj.btnval == "Modify")
            {
                cmd = new MySqlCommand("update routes set routename=@routename,status=@status,operatedby=@operatedby,ledgername=@ledgername,mobileno=@mobileno where sno=@sno");
                cmd.Parameters.Add("@routename", obj.routename);
                cmd.Parameters.Add("@status", obj.status);
                cmd.Parameters.Add("@ledgername", obj.ledgername);
                cmd.Parameters.Add("@mobileno", obj.mobileno);
                cmd.Parameters.Add("@sno", obj.updatesno);
                cmd.Parameters.Add("@operatedby", context.Session["Employ_Sno"]);
                vdm.Update(cmd);
                string response = GetJson("Route Modified successfully");
                context.Response.Write(response);
            }
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.ToString());
            context.Response.Write(response);
        }
    }
    private void save_starttrip(starttripsave obj, HttpContext context)
    {
        try
        {
            cmd = new MySqlCommand("insert into tripinfo (vehicle_id, driver_id, trip_date, route_id, start_odometer, start_fuel_value, start_eng_hrs, branch_id, createdby, status) values (@vehicle_id, @driver_id, @trip_date, @route_id, @start_odometer, @start_fuel_value, @start_eng_hrs, @branch_id, @createdby, @status)");
            cmd.Parameters.Add("@vehicle_id", obj.vehicle);
            cmd.Parameters.Add("@driver_id", obj.driver);
            cmd.Parameters.Add("@route_id", obj.route);
            DateTime trip_date = DateTime.ParseExact(obj.date, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            cmd.Parameters.Add("@trip_date", trip_date);
            cmd.Parameters.Add("@start_odometer", obj.startodometer);
            cmd.Parameters.Add("@start_fuel_value", obj.startfuelval);
            cmd.Parameters.Add("@start_eng_hrs", obj.startengineworkinghrs);
            cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
            cmd.Parameters.Add("@createdby", context.Session["Employ_Sno"]);
            cmd.Parameters.Add("@status", obj.Status);
            vdm.insert(cmd);
            string response = GetJson("Trip Assigned successfully");
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.ToString());
            context.Response.Write(response);
        }
    }
    //private void getassignedtrips(HttpContext context)
    //{
    //    try
    //    {
    //        cmd = new MySqlCommand("SELECT tripinfo.trip_sno, costcenter.cc_name AS vehicle, employdata.employname AS driver, routes.routename, tripinfo.trip_date, tripinfo.start_odometer, tripinfo.start_fuel_value, tripinfo.start_eng_hrs, tripinfo.createdby FROM tripinfo INNER JOIN routes ON tripinfo.route_id = routes.sno INNER JOIN employdata ON tripinfo.driver_id = employdata.emp_sno INNER JOIN costcenter ON tripinfo.vehicle_id = costcenter.sno WHERE (tripinfo.status = 'A') AND (tripinfo.branch_id = @branch_id)");
    //        cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
    //        DataTable trips = vdm.SelectQuery(cmd).Tables[0];
    //        List<getassignedtripscls> gettripsclslst = new List<getassignedtripscls>();
    //        foreach (DataRow dr in trips.Rows)
    //        {
    //            getassignedtripscls gettrips = new getassignedtripscls();
    //            gettrips.tripsno = dr["trip_sno"].ToString();
    //            gettrips.vehicle = dr["vehicle"].ToString();
    //            gettrips.driver = dr["driver"].ToString();
    //            gettrips.route = dr["routename"].ToString();
    //            gettrips.date = ((DateTime)dr["trip_date"]).ToString("dd/MM/yyyy");
    //            gettrips.startodometer = dr["start_odometer"].ToString();
    //            gettrips.startfuelval = dr["start_fuel_value"].ToString();
    //            gettrips.startengineworkinghrs = dr["start_eng_hrs"].ToString();
    //            gettrips.user = dr["createdby"].ToString();
    //            gettripsclslst.Add(gettrips);
    //        }
    //        string response = GetJson(gettripsclslst);
    //        context.Response.Write(response);
    //    }
    //    catch (Exception ex)
    //    {
    //        string response = GetJson(ex.ToString());
    //        context.Response.Write(response);
    //    }
    //}
    private void save_endtrip(save_endtripcls obj, HttpContext context)
    {
        try
        {
            cmd = new MySqlCommand("UPDATE tripinfo SET end_odometer = @end_odometer,  inter_fuelfill_value =@inter_fuelfill_value,  end_fuel_value = @end_fuel_value, end_eng_hrs = @end_eng_hrs,enddate =@enddate, remarks = @remarks, endedby = @endedby, status = @status WHERE (trip_sno = @trip_sno) AND (branch_id = @branch_id)");
            DateTime trip_enddate = DateTime.ParseExact(obj.endtripdate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            cmd.Parameters.Add("@enddate", trip_enddate);
            cmd.Parameters.Add("@end_odometer", obj.endodometer);
            cmd.Parameters.Add("@inter_fuelfill_value", obj.intrmdtfuelfllingval);
            cmd.Parameters.Add("@end_fuel_value", obj.endfuelval);
            cmd.Parameters.Add("@end_eng_hrs", obj.endenginewrknghrs);
            cmd.Parameters.Add("@remarks", obj.remarks);
            cmd.Parameters.Add("@endedby", context.Session["Employ_Sno"]);
            cmd.Parameters.Add("@status", obj.Status);
            cmd.Parameters.Add("@trip_sno", obj.tripsno);
            cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
            vdm.Update(cmd);
            string response = GetJson("Trip Completed successfully");
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.ToString());
            context.Response.Write(response);
        }
    }
    private void cancel_trip(save_endtripcls obj, HttpContext context)
    {
        try
        {
            cmd = new MySqlCommand("UPDATE tripinfo SET remarks = @remarks, endedby = @endedby, status = @status WHERE (trip_sno = @trip_sno) AND (branch_id = @branch_id)");
            cmd.Parameters.Add("@remarks", obj.remarks);
            cmd.Parameters.Add("@endedby", context.Session["Employ_Sno"]);
            cmd.Parameters.Add("@status", obj.Status);
            cmd.Parameters.Add("@trip_sno", obj.tripsno);
            cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
            vdm.Update(cmd);
            string response = GetJson("Trip Cancelled successfully");
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.ToString());
            context.Response.Write(response);
        }
    }


    private void emp_profile_pic_files_upload(HttpContext context)
    {
        try
        {

            if (context.Request.Files.Count > 0)
            {
                string emp_sno = context.Request["emp_sno"];
                string employid = context.Request["employid"];

                HttpFileCollection files = context.Request.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFile file = files[i];
                    string[] extension = file.FileName.Split('.');
                    string imagename = employid + "_profilepic_" + emp_sno + ".jpeg";
                    if (UploadToFTP(file, imagename))
                    {
                        cmd = new MySqlCommand("update employdata set imagepath=@imagepath where emp_sno=@emp_sno");
                        cmd.Parameters.Add("@emp_sno", emp_sno);
                        cmd.Parameters.Add("@imagepath", imagename);
                        vdm.Update(cmd);
                    }
                }
                context.Response.ContentType = "text/plain";
                context.Response.Write("File Uploaded Successfully!");
            }


        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }

    private bool uploadToFTP(HttpPostedFile fileToUpload, string filename)
    {
        string uploadUrl = "ftp://223.196.32.30/FLEET/";
        try
        {
            FtpWebRequest del_request = (FtpWebRequest)WebRequest.Create(uploadUrl + @"/" + filename);
            del_request.Credentials = new NetworkCredential("naveen", "Vyshnavi123");
            del_request.Method = WebRequestMethods.Ftp.DeleteFile;
            FtpWebResponse delete_response = (FtpWebResponse)del_request.GetResponse();
            Console.WriteLine("Delete status: {0}", delete_response.StatusDescription);
            delete_response.Close();
        }
        catch
        {
        }

        FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uploadUrl + @"/" + filename);
        request.Credentials = new NetworkCredential("ftpvys", "Vyshnavi123");
        request.Method = WebRequestMethods.Ftp.UploadFile;
        byte[] fileContents = null;
        using (var binaryReader = new BinaryReader(fileToUpload.InputStream))
        {
            fileContents = binaryReader.ReadBytes(fileToUpload.ContentLength);
        }
        request.ContentLength = fileContents.Length;
        Stream requestStream = request.GetRequestStream();
        requestStream.Write(fileContents, 0, fileContents.Length);
        requestStream.Close();
        FtpWebResponse response = (FtpWebResponse)request.GetResponse();
        response.Close();
        return true;
    }

    private void save_employeeDocument(HttpContext context)
    {
        try
        {
            //if (context.Session["branch_id"] != null)
            //{
            if (context.Request.Files.Count > 0)
            {
                string sno = context.Request["emp_sno"];
                sno = sno.TrimEnd();
                string empcode = context.Request["employid"];
                empcode = empcode.TrimEnd();
                string documentname = context.Request["documentname"];
                documentname = documentname.TrimEnd();
                string documentid = context.Request["documentid"];
                documentid = documentid.TrimEnd();
                string entryby = context.Session["Employ_Sno"].ToString();
                HttpFileCollection files = context.Request.Files;
                DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFile file = files[i];
                    string[] extension = file.FileName.Split('.');
                    string upload_filename = empcode + documentname + documentid + sno + ".doc";// +extension[extension.Length - 1];
                    if (UploadToFTP(file, upload_filename))
                    {
                        cmd = new MySqlCommand("update  employee_documents set documentpath=@documentpath where empid=@empid and documentid=@documentid");
                        cmd.Parameters.Add("@empid", sno);
                        cmd.Parameters.Add("@documentpath", upload_filename);
                        cmd.Parameters.Add("@documentid", documentid);
                        if (vdm.Update(cmd) == 0)
                        {
                            cmd = new MySqlCommand("insert into employee_documents (empid,documentpath,doe,entryby,documentid) values (@empid,@documentpath,@doe,@entryby,@documentid)");
                            cmd.Parameters.Add("@empid", sno);
                            cmd.Parameters.Add("@documentpath", upload_filename);
                            cmd.Parameters.Add("@documentid", documentid);
                            cmd.Parameters.Add("@doe", ServerDateCurrentdate);
                            cmd.Parameters.Add("@entryby", entryby);
                            vdm.insert(cmd);
                        }
                    }
                }
                context.Response.ContentType = "text/plain";
                context.Response.Write("File Uploaded Successfully!");
            }
            //}

        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }
    public class Employee_document_details
    {
        public string empsno { get; set; }
        public string employee_name { get; set; }
        public string documentid { get; set; }
        public string documentname { get; set; }
        public string ftplocation { get; set; }
        public string photo { get; set; }
    }

    private void getemployee_Uploaded_Documents(HttpContext context)
    {
        try
        {
            vdm = new VehicleDBMgr();
            string empid = context.Request["emp_sno"];
            cmd = new MySqlCommand("SELECT employdata.employid, employee_documents.documentpath, employee_documents.documentid FROM  employee_documents INNER JOIN employdata ON employee_documents.empid = employdata.emp_sno WHERE (employdata.emp_sno = @emp_sno)");
            cmd.Parameters.Add("@emp_sno", empid);
            DataTable dtemp = vdm.SelectQuery(cmd).Tables[0];
            List<Employee_document_details> bankMasterlist = new List<Employee_document_details>();
            if (dtemp.Rows.Count > 0)
            {
                foreach (DataRow dr in dtemp.Rows)
                {
                    Employee_document_details GetEmployee = new Employee_document_details();
                    GetEmployee.empsno = dr["employid"].ToString();
                    //GetEmployee.employee_name = dr["fullname"].ToString();
                    GetEmployee.documentid = dr["documentid"].ToString();
                    //GetEmployee.documentname = dr["idproof"].ToString();
                    GetEmployee.ftplocation = "ftp://223.196.32.30/FLEET/";
                    GetEmployee.photo = dr["documentpath"].ToString();
                    bankMasterlist.Add(GetEmployee);
                }
            }
            string response = GetJson(bankMasterlist);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string Response = GetJson(ex.Message);
            context.Response.Write(Response);
        }
    }


    private void TripendSaveClick(HttpContext context)
    {
        vdm = new VehicleDBMgr();
        try
        {
            string Username = context.Session["Branch_ID"].ToString();
            string tripsno = context.Request["tripsno"];
            string endodordng = context.Request["endodordng"];
            string gpskms = context.Request["gpskms"];
            string endhourmtrrdng = context.Request["endhourmtrrdng"];
            string fuelprice = context.Request["fuelprice"];
            double endodometer = 0;
            double.TryParse(endodordng, out endodometer);
            double endhourmeter = 0;
            double.TryParse(endhourmtrrdng, out endhourmeter);
            DateTime TripDate = VehicleDBMgr.GetTime(vdm.conn);
            var Status = "P";
            cmd = new MySqlCommand("UPDATE tripdata SET  Status=@Status,GpsKms = @GpsKms, EndDate = @EndDate,  EndHrMeter = @EndHrMeter, EndOdometerReading = @EndOdometerReading WHERE (Sno = @tripsno)");
            cmd.Parameters.Add("@Status", Status);
            cmd.Parameters.Add("@GpsKms", gpskms);
            cmd.Parameters.Add("@EndDate", TripDate);
            cmd.Parameters.Add("@EndOdometerReading", endodordng);
            cmd.Parameters.Add("@EndHrMeter", endhourmtrrdng);
            cmd.Parameters.Add("@tripsno", tripsno);
            vdm.Update(cmd);
            cmd = new MySqlCommand("SELECT Vehicleno, ROUND(endodometerreading - vehiclestartreading, 2) AS tripKms FROM tripdata WHERE (sno = @TripSno)");
            cmd.Parameters.Add("@TripSno", tripsno);
            DataTable dtTrip = vdm.SelectQuery(cmd).Tables[0];
            if (dtTrip.Rows.Count > 0)
            {
                cmd = new MySqlCommand("UPDATE vehicel_master set odometer=odometer+@odometer where vm_sno=@Vehicleno");
                cmd.Parameters.Add("@odometer", dtTrip.Rows[0]["tripKms"].ToString());
                cmd.Parameters.Add("@Vehicleno", dtTrip.Rows[0]["Vehicleno"].ToString());
                vdm.Update(cmd);
                cmd = new MySqlCommand("SELECT Sno, vehicle_mstr_sno, tyre_sno, Odometer FROM vehicle_master_sub WHERE (vehicle_mstr_sno = @VehicleSno)");
                cmd.Parameters.Add("@VehicleSno", dtTrip.Rows[0]["Vehicleno"].ToString());
                DataTable dtTyres = vdm.SelectQuery(cmd).Tables[0];
                if (dtTyres.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtTyres.Rows)
                    {
                        cmd = new MySqlCommand("update vehicle_master_sub set Odometer=Odometer+@Odometer where tyre_sno=@tyre_sno");
                        cmd.Parameters.Add("@Odometer", dtTrip.Rows[0]["tripKms"].ToString());
                        cmd.Parameters.Add("@tyre_sno", dr["tyre_sno"].ToString());
                        vdm.Update(cmd);
                        cmd = new MySqlCommand("update new_tyres_sub set current_KMS=current_KMS+@current_KMS where sno=@tyre_sno");
                        cmd.Parameters.Add("@current_KMS", dtTrip.Rows[0]["tripKms"].ToString());
                        cmd.Parameters.Add("@tyre_sno", dr["tyre_sno"].ToString());
                        vdm.Update(cmd);
                    }
                }
                cmd = new MySqlCommand("update  vehi_service_update_kms set doe=@doe,  airchecking=airchecking+@airchecking,tyreinterchanging=tyreinterchanging+@tyreinterchanging,eoc=eoc+@eoc, goc=goc+@goc, ofc=ofc+@ofc, afc=afc+@afc, brake_fluid=brake_fluid+@brake_fluid, steering_fluid=steering_fluid+@steering_fluid, transmission_fluid=transmission_fluid+@transmission_fluid, washer_fluid=washer_fluid+@washer_fluid, wheel_bearings=wheel_bearings+@wheel_bearings, checkleaks=checkleaks+@checkleaks, belts_hoses=belts_hoses+@belts_hoses, lubricate_chasis=lubricate_chasis+@lubricate_chasis where vehsno=@vehsno");
                cmd.Parameters.Add("@doe", TripDate);
                cmd.Parameters.Add("@airchecking", dtTrip.Rows[0]["tripKms"].ToString());
                cmd.Parameters.Add("@tyreinterchanging", dtTrip.Rows[0]["tripKms"].ToString());
                cmd.Parameters.Add("@eoc", dtTrip.Rows[0]["tripKms"].ToString());
                cmd.Parameters.Add("@goc", dtTrip.Rows[0]["tripKms"].ToString());
                cmd.Parameters.Add("@ofc", dtTrip.Rows[0]["tripKms"].ToString());
                cmd.Parameters.Add("@afc", dtTrip.Rows[0]["tripKms"].ToString());
                cmd.Parameters.Add("@brake_fluid", dtTrip.Rows[0]["tripKms"].ToString());
                cmd.Parameters.Add("@steering_fluid", dtTrip.Rows[0]["tripKms"].ToString());
                cmd.Parameters.Add("@transmission_fluid", dtTrip.Rows[0]["tripKms"].ToString());
                cmd.Parameters.Add("@washer_fluid", dtTrip.Rows[0]["tripKms"].ToString());
                cmd.Parameters.Add("@wheel_bearings", dtTrip.Rows[0]["tripKms"].ToString());
                cmd.Parameters.Add("@checkleaks", dtTrip.Rows[0]["tripKms"].ToString());
                cmd.Parameters.Add("@belts_hoses", dtTrip.Rows[0]["tripKms"].ToString());
                cmd.Parameters.Add("@lubricate_chasis", dtTrip.Rows[0]["tripKms"].ToString());
                cmd.Parameters.Add("@vehsno", dtTrip.Rows[0]["Vehicleno"].ToString());
                vdm.Update(cmd);
            }
            cmd = new MySqlCommand("SELECT tripdata.tripsheetno AS TripsheetSno, employdata.employname AS DriverName,minimasters.mm_code as VehCode, minimasters_1.mm_code AS MakeCode, vehicel_master.registration_no AS VehicleNo, DATE_FORMAT(tripdata.tripdate, '%m/%d/%Y %h:%i %p') AS StartDate, DATE_FORMAT(tripdata.enddate, '%m/%d/%Y %h:%i %p') AS EndDate, vehicel_master.vm_owner AS Owner, tripdata.endodometerreading - tripdata.vehiclestartreading AS TripKMS, tripdata.gpskms AS GpsKms, ROUND(tripdata.endodometerreading - tripdata.vehiclestartreading - tripdata.gpskms, 2) AS DifferenceKMS, tripdata.endfuelvalue AS Diesel, ROUND((tripdata.endodometerreading - tripdata.vehiclestartreading) / tripdata.endfuelvalue, 2) AS TodayMileage, tripdata.loadtype AS LoadType, tripdata.routeid, tripdata.qty AS Qty, tripdata.tripexpences AS Expenses, minimasters.mm_name AS VehicleType, minimasters_1.mm_name AS Make, vehicel_master.Capacity FROM employdata INNER JOIN tripdata ON employdata.emp_sno = tripdata.driverid INNER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno INNER JOIN minimasters ON vehicel_master.vhtype_refno = minimasters.sno INNER JOIN minimasters minimasters_1 ON vehicel_master.vhmake_refno = minimasters_1.sno WHERE (tripdata.sno = @TripSno)");
            cmd.Parameters.Add("@TripSno", tripsno);
            DataTable dttrip = vdm.SelectQuery(cmd).Tables[0];
            GpsDBManager GpsDB = new GpsDBManager();
            string routecode = "Plant";
            if (dttrip.Rows.Count > 0)
            {
                string VehicleType = dttrip.Rows[0]["VehicleType"].ToString();
                if (VehicleType == "Puff" || VehicleType == "Tanker")
                {
                    cmd = new MySqlCommand("update cabmanagement set routecode=@routecode,odometer=@odometer,odometer_time=@odometer_time,Smscount=0,SmsMobileNo=123 where VehicleID=@VehicleID");
                    cmd.Parameters.Add("@routecode", routecode);
                    cmd.Parameters.Add("@odometer", endodometer);
                    cmd.Parameters.Add("@odometer_time", TripDate);
                    cmd.Parameters.Add("@VehicleID", dttrip.Rows[0]["VehicleNo"].ToString());
                    GpsDB.Update(cmd);
                }
            }
            string msg = "Trip ended successfully";
            string response = GetJson(msg);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string msg = ex.Message;
            string response = GetJson(msg);
            context.Response.Write(response);
        }
    }

    public class tyre_no
    {
        public string svdsno { get; set; }
        public string TyreNo { get; set; }
        public string transdate { get; set; }
        public string regno { get; set; }
        public string vehicletype { get; set; }
        public string totalkms { get; set; }
        public string kms { get; set; }
        public string tyreposition { get; set; }
        public string currentkms { get; set; }
        public string odometer { get; set; }
        public string tyreno { get; set; }
        public string brand { get; set; }
        public string size { get; set; }
        public string cost { get; set; }
        public string fittingtype { get; set; }
    }

    private void get_tyresum_report(HttpContext context)
    {
        try
        {
           cmd = new MySqlCommand("SELECT new_tyres_sub.tyre_sno, new_tyres_sub.size, new_tyres_sub.cost, new_tyres_sub.Fitting_Type, new_tyres_sub.brand, new_tyres_sub.current_KMS, new_tyres_sub.svdsno, minimasters.mm_name AS brandname, vehicel_master.registration_no, axils_tyres_names.tyrename AS TyrePosition FROM new_tyres INNER JOIN new_tyres_sub ON new_tyres.sno = new_tyres_sub.newtyre_refno INNER JOIN vehicle_master_sub ON new_tyres_sub.sno = vehicle_master_sub.tyre_sno INNER JOIN vehicel_master ON vehicle_master_sub.vehicle_mstr_sno = vehicel_master.vm_sno INNER JOIN  axils_tyres_names ON vehicle_master_sub.axles_tyres_names_sno = axils_tyres_names.sno INNER JOIN minimasters ON new_tyres_sub.brand = minimasters.sno ORDER BY new_tyres_sub.seriesno");
          //  cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
            //cmd = new MySqlCommand("SELECT new_tyres_sub.tyre_sno, new_tyres_sub.size, new_tyres_sub.cost, new_tyres_sub.Fitting_Type, new_tyres_sub.brand, new_tyres_sub.current_KMS, new_tyres_sub.svdsno,minimasters.mm_name AS brandname, vehicel_master.registration_no, tyre_changes.tyre_position, axils_tyres_names.tyrename AS TyrePosition FROM new_tyres_sub INNER JOIN new_tyres ON new_tyres_sub.newtyre_refno = new_tyres.sno INNER JOIN minimasters ON new_tyres_sub.brand = minimasters.sno INNER JOIN tyre_changes ON new_tyres_sub.sno = tyre_changes.sno INNER JOIN vehicel_master ON tyre_changes.vehicle_sno = vehicel_master.vm_sno INNER JOIN axils_tyres_names ON tyre_changes.tyre_position = axils_tyres_names.sno ORDER BY new_tyres_sub.seriesno ASC");
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            List<tyre_no> tyres = new List<tyre_no>();
            foreach (DataRow dr in dt.Rows)
            {
                tyre_no data = new tyre_no();
                data.tyreno = dr["tyre_sno"].ToString();
                data.svdsno = dr["svdsno"].ToString();
                data.regno = dr["registration_no"].ToString();
                data.size = dr["size"].ToString();
                data.brand = dr["brandname"].ToString();
                data.kms = dr["current_KMS"].ToString();
                data.cost = dr["cost"].ToString();
                data.tyreposition = dr["TyrePosition"].ToString();
                string ftype = dr["Fitting_Type"].ToString();
                if (ftype == "F")
                {
                    data.fittingtype = "Fitted";
                }
                if (ftype == "SC")
                {
                    data.fittingtype = "Scrap";
                }
                if (ftype == "R")
                {
                    data.fittingtype = "Rebutton";
                }
                if (ftype == "NF")
                {
                    data.fittingtype = "Not Fitted";
                }
                if (ftype == "S")
                {
                    data.fittingtype = "Spare";
                }

                tyres.Add(data);
            }
            string response = GetJson(tyres);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.ToString());
            context.Response.Write(response);
        }
    }
    private void get_tyre_report(HttpContext context)
    {
        try
        {
            string tyresno = context.Request["vytyreno"].ToString();
            cmd = new MySqlCommand("SELECT tyre_changes.transaction_date, new_tyres_sub.svdsno,vehicel_master.registration_no AS VehicleNo, minimasters_2.mm_name AS VehicleType, minimasters_3.mm_name AS VehicleMake, vehicel_master.Capacity, tyre_changes.veh_odometer AS Odometer, new_tyres_sub.tyre_sno AS TyreNo, new_tyres_sub.svdsno, minimasters.mm_name AS TyreType, minimasters_1.mm_name AS Brand, new_tyres_sub.cost, axils_tyres_names.tyrename AS TyrePosition, new_tyres_sub.current_KMS AS TotalKMs, tyre_changes.kms, tyre_changes.Fitting_Type AS FitType, axils_tyres_names.axleside, tyre_changes.remarks FROM tyre_changes INNER JOIN new_tyres_sub ON tyre_changes.tyre_master_sno = new_tyres_sub.sno INNER JOIN minimasters ON new_tyres_sub.type_of_tyre = minimasters.sno INNER JOIN minimasters minimasters_1 ON new_tyres_sub.brand = minimasters_1.sno INNER JOIN axils_tyres_names ON tyre_changes.tyre_position = axils_tyres_names.sno INNER JOIN vehicel_master ON tyre_changes.vehicle_sno = vehicel_master.vm_sno INNER JOIN minimasters minimasters_2 ON vehicel_master.vhtype_refno = minimasters_2.sno INNER JOIN minimasters minimasters_3 ON vehicel_master.vhmake_refno = minimasters_3.sno WHERE (tyre_changes.branch_id = @branch_id) AND (new_tyres_sub.svdsno = @subtyre) ORDER BY (tyre_changes.transaction_date)");
            cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
            cmd.Parameters.Add("@subtyre", tyresno);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            List<tyre_no> tyres = new List<tyre_no>();
            foreach (DataRow dr in dt.Rows)
            {
                tyre_no data = new tyre_no();
                data.transdate = dr["transaction_date"].ToString();
                data.regno = dr["VehicleNo"].ToString();
                data.tyreposition = dr["TyrePosition"].ToString();
                data.odometer = dr["Odometer"].ToString();
                data.tyreno = dr["svdsno"].ToString();
                data.currentkms = dr["TotalKMs"].ToString();
                data.kms = dr["kms"].ToString();
                tyres.Add(data);
            }
            string response = GetJson(tyres);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.ToString());
            context.Response.Write(response);
        }
    }
    private void get_tyres_number(HttpContext context)
    {
        try
        {
            cmd = new MySqlCommand("SELECT tyre_sno AS TyreNo, svdsno, current_KMS, Fitting_Type, cost FROM new_tyres_sub");
            cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            List<tyre_no> tyres = new List<tyre_no>();
            foreach (DataRow dr in dt.Rows)
            {
                tyre_no data = new tyre_no();
                data.svdsno = dr["svdsno"].ToString();
                data.TyreNo = dr["TyreNo"].ToString();
                tyres.Add(data);
            }
            string response = GetJson(tyres);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.ToString());
            context.Response.Write(response);
        }
    }
    
    private void vehicle_handover_pics_upload(HttpContext context)
    {
        try
        {
            if (context.Request.Files.Count > 0)
            {
                string vehiclesno = context.Request["vehiclesno"];
                string Veh_sno = context.Request["Veh_sno"];
                HttpFileCollection files = context.Request.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFile file = files[i];
                    string[] extension = file.FileName.Split('.');
                    string upload_filename = vehiclesno + "_profilepic_" + Veh_sno + ".jpeg";// +extension[extension.Length - 1];
                    if (UploadpicToFTP(file, upload_filename))
                    {
                        MySqlCommand cmd = new MySqlCommand("update handoverlogs set imagepath=@imagepath where  handoversno=@handoversno");
                        cmd.Parameters.Add("@handoversno", Veh_sno);
                        cmd.Parameters.Add("@imagepath", upload_filename);
                        vdm.Update(cmd);
                    }
                }
                context.Response.ContentType = "text/plain";
                context.Response.Write("File Uploaded Successfully!");
            }
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }
    private bool UploadpicToFTP(HttpPostedFile fileToUpload, string filename)
    {
        // Get the object used to communicate with the server.
        string uploadUrl = "ftp://223.196.32.30:21/fleetphotos/";
        // string fileName = fileToUpload.FileName;
        try
        {
            FtpWebRequest del_request = (FtpWebRequest)WebRequest.Create(uploadUrl + @"/" + filename);
            del_request.Credentials = new NetworkCredential("ftpvys", "Vyshnavi123");
            del_request.Method = WebRequestMethods.Ftp.DeleteFile;
            FtpWebResponse delete_response = (FtpWebResponse)del_request.GetResponse();
            Console.WriteLine("Delete status: {0}", delete_response.StatusDescription);
            delete_response.Close();
        }
        catch
        {
        }
        FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uploadUrl + @"/" + filename);
        request.Credentials = new NetworkCredential("ftpvys", "Vyshnavi123");
        request.Method = WebRequestMethods.Ftp.UploadFile;
        byte[] fileContents = null;
        using (var binaryReader = new BinaryReader(fileToUpload.InputStream))
        {
            fileContents = binaryReader.ReadBytes(fileToUpload.ContentLength);
        }
        request.ContentLength = fileContents.Length;
        Stream requestStream = request.GetRequestStream();
        requestStream.Write(fileContents, 0, fileContents.Length);
        requestStream.Close();
        FtpWebResponse response = (FtpWebResponse)request.GetResponse();
        response.Close();
        return true;
    }

    private void save_Vehicle_handover_Info(HttpContext context)
    {
        try
        {
            if (context.Request.Files.Count > 0)
            {
                string vehiclesno = context.Request["vehiclesno"];
                vehiclesno = vehiclesno.TrimEnd();
                string registration_no = context.Request["registration_no"];
                registration_no = registration_no.TrimEnd();
                string photoname = context.Request["photoname"];
                photoname = photoname.TrimEnd();
                string photoid = context.Request["photoid"];
                photoid = photoid.TrimEnd();
                string entryby = context.Session["Employ_Sno"].ToString();
                HttpFileCollection files = context.Request.Files;
                DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFile file = files[i];
                    string[] extension = file.FileName.Split('.');
                    string upload_filename = registration_no + photoname + photoid + vehiclesno + ".jpeg";// +extension[extension.Length - 1];
                    if (UploadpicToFTP(file, upload_filename))
                    {
                        MySqlCommand cmd = new MySqlCommand("update  handoverlogs set imagename=@imagename,imagepath=@imagepath where handoversno=@handoversno and imagename=@imagename");
                        cmd.Parameters.Add("@handoversno", vehiclesno);
                        cmd.Parameters.Add("@imagepath", upload_filename);
                        cmd.Parameters.Add("@imagename", photoid);
                        if (vdm.Update(cmd) == 0)
                        {
                            cmd = new MySqlCommand("insert into handoverlogs (handoversno,imagepath,imagename) values (@handoversno,@imagepath,@imagename)");
                            cmd.Parameters.Add("@handoversno", vehiclesno);
                            cmd.Parameters.Add("@imagepath", upload_filename);
                            cmd.Parameters.Add("@imagename", photoid);
                            vdm.insert(cmd);
                        }
                    }
                }
                context.Response.ContentType = "text/plain";
                context.Response.Write("File Uploaded Successfully!");
            }
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }
    public class vehihandover
    {
        public string vm_sno { get; set; }
        public string kmreading { get; set; }
        public string vehno { get; set; }
        public string receiverid { get; set; }
        public string receiverdrier { get; set; }
        public string handoverid { get; set; }
        public string handoverdriver { get; set; }
        public string inspectedby { get; set; }
        public string inspecteddate { get; set; }
        public string acmeter { get; set; }
        public string bodycondition { get; set; }
        public string recordscheck { get; set; }
        public string remarks { get; set; }

        public string ftplocation { get; set; }

        public string imagename { get; set; }
    }
    private void get_veh_handover_data(HttpContext context)
    {
        try
        {
            //cmd = new MySqlCommand("SELECT tyre_inspection.sno, tyre_inspection.vehicleno, tyre_inspection.inspectedby, tyre_inspection.Inspecteddate, tyre_inspection.userid, tyre_inspection.branch_id, tyre_inspection.kmreading, tyre_inspection.achrmeter, tyre_inspection.bodycondition, tyre_inspection.recordscheck, tyre_inspection.remarks, tyre_inspection.receiver_driverid, employdata.employname AS handoverdriver, employdata_1.employname AS receiverdriver, vehicel_master.registration_no, tyre_inspection.handover_driverid, tyre_inspection_sub.tyre_insp_ref FROM   tyre_inspection INNER JOIN employdata ON tyre_inspection.handover_driverid = employdata.emp_sno INNER JOIN employdata employdata_1 ON tyre_inspection.receiver_driverid = employdata_1.emp_sno INNER JOIN vehicel_master ON tyre_inspection.vehicleno = vehicel_master.vm_sno INNER JOIN tyre_inspection_sub ON tyre_inspection.sno = tyre_inspection_sub.sno WHERE (tyre_inspection.branch_id = @branch_id) ");
            cmd = new MySqlCommand("SELECT tyre_inspection.sno, tyre_inspection.vehicleno, tyre_inspection.inspectedby, tyre_inspection.Inspecteddate, tyre_inspection.userid, tyre_inspection.branch_id, tyre_inspection.kmreading, tyre_inspection.achrmeter, tyre_inspection.bodycondition, tyre_inspection.recordscheck, tyre_inspection.remarks, tyre_inspection.receiver_driverid, employdata.employname AS handoverdriver, employdata_1.employname AS receiverdriver, vehicel_master.registration_no, tyre_inspection.handover_driverid, tyre_inspection_sub.tyre_insp_ref, handoverlogs.handoversno, handoverlogs.imagename, handoverlogs.imagepath FROM  tyre_inspection INNER JOIN employdata ON tyre_inspection.handover_driverid = employdata.emp_sno INNER JOIN employdata employdata_1 ON tyre_inspection.receiver_driverid = employdata_1.emp_sno INNER JOIN vehicel_master ON tyre_inspection.vehicleno = vehicel_master.vm_sno INNER JOIN tyre_inspection_sub ON tyre_inspection.sno = tyre_inspection_sub.sno INNER JOIN handoverlogs ON vehicel_master.vm_sno = handoverlogs.sno WHERE (tyre_inspection.branch_id = @branch_id)");
            cmd.Parameters.Add("@branch_id", context.Session["Branch_ID"]);
            DataTable dthandover = vdm.SelectQuery(cmd).Tables[0];
            List<vehihandover> handover = new List<vehihandover>();
            foreach (DataRow dr in dthandover.Rows)
            {
                vehihandover data = new vehihandover();
                data.vm_sno = dr["vehicleno"].ToString();
                data.vehno = dr["registration_no"].ToString();
                data.kmreading = dr["kmreading"].ToString();
                data.receiverid = dr["receiver_driverid"].ToString();
                data.receiverdrier = dr["receiverdriver"].ToString();
                data.handoverid = dr["handover_driverid"].ToString();
                data.handoverdriver = dr["handoverdriver"].ToString();
                data.inspectedby = dr["inspectedby"].ToString();
                data.inspecteddate = dr["Inspecteddate"].ToString();
                data.acmeter = dr["achrmeter"].ToString();
                data.bodycondition = dr["bodycondition"].ToString();
                data.recordscheck = dr["recordscheck"].ToString();
                data.remarks = dr["remarks"].ToString();
                data.ftplocation = "ftp://223.196.32.30:21/fleetphotos/";
                data.imagename = dr["imagename"].ToString();
                handover.Add(data);
            }
            string response = GetJson(handover);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }
    public class hand_over
    {
        public string remarks;
        public string sno { get; set; }
        public string vehno { get; set; }
        public string kmreading { get; set; }
        public string inspecteddate { get; set; }

        public string achrmeter { get; set; }

        public string inspectedby { get; set; }

        public string handoversno { get; set; }

        public string vehmake { get; set; }

        public string capacity { get; set; }

        public string vehtype { get; set; }

        public string phoneno { get; set; }

        public string empname { get; set; }

        public string licenseno { get; set; }

        public string bodycondition { get; set; }

        public string recordscheck { get; set; }

        public string date { get; set; }

        public string time { get; set; }

        public string toolname { get; set; }

        public string svdsno { get; set; }

        public string tyretype { get; set; }

        public string brand { get; set; }

        public string grove { get; set; }

        public string tyresno { get; set; }

        public string address { get; set; }
    }

    DataTable trips = new DataTable();
    private void get_handover_details(HttpContext context)
    {
        try
        {
            string type = context.Request["type"];
            string fromdate = context.Request["fromdate"];
            string todate = context.Request["todate"];
            string branchid = context.Session["Branch_ID"].ToString();
            trips = new DataTable();
            if (type == "Handover")
            {
                cmd = new MySqlCommand("SELECT tyre_inspection.sno, vehicel_master.registration_no AS VehicleNo, tyre_inspection.Inspecteddate, tyre_inspection.kmreading, tyre_inspection.achrmeter, tyre_inspection.inspectedby FROM tyre_inspection INNER JOIN vehicel_master ON tyre_inspection.vehicleno = vehicel_master.vm_sno LEFT OUTER JOIN employdata ON tyre_inspection.handover_driverid = employdata.employid WHERE (tyre_inspection.branch_id = @BranchID) AND (tyre_inspection.Inspecteddate BETWEEN @d1 AND @d2)");
                cmd.Parameters.Add("@d1", fromdate);
                cmd.Parameters.Add("@d2", todate);
                cmd.Parameters.Add("@BranchID", branchid);
                trips = vdm.SelectQuery(cmd).Tables[0];
                List<hand_over> handover = new List<hand_over>();
                if (trips.Rows.Count > 0)
                {
                    foreach (DataRow dr in trips.Rows)
                    {
                        hand_over data = new hand_over();
                        data.sno = dr["sno"].ToString();
                        data.vehno = dr["VehicleNo"].ToString();
                        data.inspecteddate = dr["Inspecteddate"].ToString();
                        data.kmreading = dr["kmreading"].ToString();
                        data.achrmeter = dr["achrmeter"].ToString();
                        data.inspectedby = dr["inspectedby"].ToString();
                        handover.Add(data);
                    }
                    string response = GetJson(handover);
                    context.Response.Write(response);
                }
            }
                else if (type == "Receiver")
                {
                    cmd = new MySqlCommand("SELECT tyre_inspection.sno, vehicel_master.registration_no AS VehicleNo, tyre_inspection.Inspecteddate, tyre_inspection.kmreading, tyre_inspection.achrmeter, tyre_inspection.inspectedby FROM tyre_inspection INNER JOIN vehicel_master ON tyre_inspection.vehicleno = vehicel_master.vm_sno LEFT OUTER JOIN employdata ON tyre_inspection.receiver_driverid = employdata.emp_sno WHERE (tyre_inspection.branch_id = @BranchID) AND (tyre_inspection.Inspecteddate BETWEEN @d1 AND @d2)");
                    cmd.Parameters.Add("@d1", fromdate);
                    cmd.Parameters.Add("@d2", todate);
                    cmd.Parameters.Add("@BranchID", branchid);
                    trips = vdm.SelectQuery(cmd).Tables[0];
                    List<hand_over> receiver = new List<hand_over>();
                    if (trips.Rows.Count > 0)
                    {

                        foreach (DataRow dr in trips.Rows)
                        {
                            hand_over data = new hand_over();
                            data.sno = dr["sno"].ToString();
                            data.vehno = dr["VehicleNo"].ToString();
                            data.inspecteddate = dr["Inspecteddate"].ToString();
                            data.kmreading = dr["kmreading"].ToString();
                            data.achrmeter = dr["achrmeter"].ToString();
                            data.inspectedby = dr["inspectedby"].ToString();
                            receiver.Add(data);
                        }
                        string response = GetJson(receiver);
                        context.Response.Write(response);
                    }
                }
                else
                {
                    string response = "No Data Found";
                    context.Response.Write(response);
                }
        }
        catch (Exception ex)
        {
            string response = GetJson(ex.Message);
            context.Response.Write(response);
        }
    }
    private void getvehicle_Uploaded_photos(HttpContext context)
    {
        try
        {
            vdm = new VehicleDBMgr();
            string Vehiclesno = context.Request["Vehiclesno"];
            cmd = new MySqlCommand("SELECT        handoverlogs.sno, handoverlogs.handoversno, handoverlogs.imagename, handoverlogs.branch_id, handoverlogs.IMAGE, handoverlogs.imagepath, vehicel_master.registration_no FROM handoverlogs INNER JOIN vehicel_master ON handoverlogs.handoversno = vehicel_master.vm_sno WHERE (vehicel_master.vm_sno = @Vehiclesno)");
            cmd.Parameters.Add("@Vehiclesno", Vehiclesno);
            DataTable dtemp = vdm.SelectQuery(cmd).Tables[0];
            List<vehicle_document_details> bankMasterlist = new List<vehicle_document_details>();
            if (dtemp.Rows.Count > 0)
            {
                foreach (DataRow dr in dtemp.Rows)
                {
                    vehicle_document_details GetEmployee = new vehicle_document_details();
                    GetEmployee.Vehno = Vehiclesno;
                    GetEmployee.vehicle_name = dr["registration_no"].ToString();
                    GetEmployee.imageid = dr["imagename"].ToString();
                    GetEmployee.ftplocation = "ftp://223.196.32.30:21/FLEETphotos/";
                    GetEmployee.photo = dr["imagepath"].ToString();
                    bankMasterlist.Add(GetEmployee);
                }
            }
            string response = GetJson(bankMasterlist);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string Response = GetJson(ex.Message);
            context.Response.Write(Response);
        }
    }

    private void get_handover_print_details(HttpContext context)
    {
        try
        {
            vdm = new VehicleDBMgr();
            string type = context.Request["type"];
            string handoversno = context.Request["handoversno"];
            string address = context.Session["Address"].ToString();
            if (type == "Handover")
            {
                //lblHeader.Text = "Vehicle Handover Report";
                cmd = new MySqlCommand("SELECT vehicel_master.registration_no AS VehicleNo,employdata.emp_licencenum as LicenseNo, tyre_inspection.Inspecteddate, tyre_inspection.kmreading, employdata.Phoneno, tyre_inspection.achrmeter, tyre_inspection.bodycondition, tyre_inspection.recordscheck, tyre_inspection.remarks, employdata.employname, minimasters.mm_name AS VehicleType, minimasters_1.mm_name AS VehicleMake, vehicel_master.Capacity, tyre_inspection.handover_driverid FROM tyre_inspection INNER JOIN vehicel_master ON tyre_inspection.vehicleno = vehicel_master.vm_sno INNER JOIN minimasters ON vehicel_master.vhtype_refno = minimasters.sno INNER JOIN minimasters minimasters_1 ON vehicel_master.vhmake_refno = minimasters_1.sno LEFT OUTER JOIN employdata ON tyre_inspection.handover_driverid = employdata.emp_sno WHERE (tyre_inspection.sno = @HandOverSno)");
            }
            if (type == "Receiver")
            {
                //lblHeader.Text = "Vehicle Receiving Report";
                cmd = new MySqlCommand("SELECT vehicel_master.registration_no AS VehicleNo,employdata.emp_licencenum as LicenseNo, tyre_inspection.Inspecteddate, tyre_inspection.kmreading, employdata.Phoneno, tyre_inspection.achrmeter, tyre_inspection.bodycondition, tyre_inspection.recordscheck, tyre_inspection.remarks, employdata.employname, minimasters.mm_name AS VehicleType, minimasters_1.mm_name AS VehicleMake, vehicel_master.Capacity, tyre_inspection.handover_driverid, employdata.emp_licencenum FROM tyre_inspection INNER JOIN vehicel_master ON tyre_inspection.vehicleno = vehicel_master.vm_sno INNER JOIN minimasters ON vehicel_master.vhtype_refno = minimasters.sno INNER JOIN minimasters minimasters_1 ON vehicel_master.vhmake_refno = minimasters_1.sno LEFT OUTER JOIN employdata ON tyre_inspection.receiver_driverid = employdata.emp_sno WHERE (tyre_inspection.sno = @HandOverSno)");
            }
            cmd.Parameters.Add("@HandOverSno", handoversno);
            DataTable dtTripSheet = vdm.SelectQuery(cmd).Tables[0];
            List<hand_over> tripsheet = new List<hand_over>();
            if (dtTripSheet.Rows.Count > 0)
            {

                foreach (DataRow dr in dtTripSheet.Rows)
                {
                    hand_over data = new hand_over();
                    data.handoversno = handoversno;
                    data.address = address;
                    string TripTime = dtTripSheet.Rows[0]["Inspecteddate"].ToString();
                    DateTime dtPlantime = Convert.ToDateTime(TripTime);
                    string time = dtPlantime.ToString("dd/MMM/yyyy");
                    string strPlantime = dtPlantime.ToString();
                    string[] DateTime = strPlantime.Split(' ');
                    string[] PlanDateTime = strPlantime.Split(' ');
                    data.date = time;
                    data.time = PlanDateTime[1];
                    data.vehno = dtTripSheet.Rows[0]["VehicleNo"].ToString();
                    data.vehmake = dtTripSheet.Rows[0]["VehicleMake"].ToString();
                    data.capacity = dtTripSheet.Rows[0]["Capacity"].ToString();
                    data.vehtype = dtTripSheet.Rows[0]["VehicleType"].ToString();
                    data.phoneno = dtTripSheet.Rows[0]["Phoneno"].ToString();
                    data.empname = dtTripSheet.Rows[0]["employname"].ToString();
                    data.licenseno = dtTripSheet.Rows[0]["LicenseNo"].ToString();
                    data.kmreading = dtTripSheet.Rows[0]["kmreading"].ToString();
                    data.achrmeter= dtTripSheet.Rows[0]["achrmeter"].ToString();
                    data.bodycondition = dtTripSheet.Rows[0]["bodycondition"].ToString();
                    data.recordscheck = dtTripSheet.Rows[0]["recordscheck"].ToString();
                    data.remarks = dtTripSheet.Rows[0]["remarks"].ToString();
                    tripsheet.Add(data);
                }

            }
            string response = GetJson(tripsheet);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string Response = GetJson(ex.Message);
            context.Response.Write(Response);
        }

    }
    private void get_vehicle_tools(HttpContext context)
    {
        try
        {
            string handoversno = context.Request["handoversno"];
            cmd = new MySqlCommand("SELECT toolname as Tools FROM  handover_subtable WHERE (handoversno = @handoversno)");
            cmd.Parameters.Add("@handoversno", handoversno);
            DataTable toolsReport = vdm.SelectQuery(cmd).Tables[0];
            List<hand_over> vehicletoools = new List<hand_over>();
            foreach (DataRow dr in toolsReport.Rows)
            {
                hand_over data = new hand_over();
                data.toolname = dr["Tools"].ToString();
                vehicletoools.Add(data);
            }
            string response = GetJson(vehicletoools);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string Response = GetJson(ex.Message);
            context.Response.Write(Response);
        }
}
    private void get_vehicle_tyres(HttpContext context)
    {
        try
        {
            string handoversno = context.Request["handoversno"];
            cmd = new MySqlCommand("SELECT new_tyres_sub.tyre_sno as TyreSno, new_tyres_sub.svdsno as SVDSNo, minimasters.mm_name AS TyreType, minimasters_1.mm_name AS Brand, tyre_inspection_sub.grove FROM minimasters INNER JOIN  new_tyres_sub ON minimasters.sno = new_tyres_sub.type_of_tyre INNER JOIN minimasters minimasters_1 ON new_tyres_sub.brand = minimasters_1.sno RIGHT OUTER JOIN tyre_inspection_sub ON new_tyres_sub.sno = tyre_inspection_sub.tyre_sno WHERE (tyre_inspection_sub.tyre_insp_ref = @HandOverSno)");
            cmd.Parameters.Add("@handoversno", handoversno);
            DataTable toolsReport = vdm.SelectQuery(cmd).Tables[0];
            List<hand_over> vehicletoools = new List<hand_over>();
            foreach (DataRow dr in toolsReport.Rows)
            {
                hand_over data = new hand_over();
                data.tyresno = dr["TyreSno"].ToString();
                data.svdsno = dr["SVDSNo"].ToString();
                data.tyretype = dr["TyreType"].ToString();
                data.brand = dr["Brand"].ToString();
                data.grove = dr["grove"].ToString();
                vehicletoools.Add(data);
            }
            string response = GetJson(vehicletoools);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            string Response = GetJson(ex.Message);
            context.Response.Write(Response);
        }
    }

    private void get_handover_sno(HttpContext context)
    {
        try
        {
            vdm = new VehicleDBMgr();
            List<hand_over> handoverlist = new List<hand_over>();
            hand_over obj = new hand_over();
            context.Session["handoversno"] = context.Request["handoversno"].ToString();
            obj.handoversno = context.Request["handoversno"].ToString();
            handoverlist.Add(obj);
            string response = GetJson(handoverlist);
            context.Response.Write(response);
        }
        catch
        {
        }
    }
    public class empdata
    {
        public string DriverName { get; set; }

        public string EmpID { get; set; }

        public string MobNo { get; set; }

        public string Doj { get; set; }

        public string DOB { get; set; }

        public string emptype { get; set; }

        public string FathersName { get; set; }

        public string Gender { get; set; }

        public string Qualification { get; set; }

        public string Status { get; set; }

        public string Nationality { get; set; }

        public string Address { get; set; }

        public string bloodgroup { get; set; }

        public string LicenceNo { get; set; }

        public string LicenceExpdate { get; set; }

        public string Experience { get; set; }

        public string BankName { get; set; }

        public string AccountNo { get; set; }

        public string title { get; set; }

        public string addr { get; set; }
    }
    private void get_emp_information(HttpContext context)
    {
        try
        {
            string empname = context.Request["empname"];
            string branchid = context.Session["Branch_ID"].ToString();
            string title = context.Session["TitleName"].ToString();
            string companyAddress = context.Session["Address"].ToString();
            cmd = new MySqlCommand("SELECT employdata.emp_sno, employdata.employid, employdata.employname, employdata.emp_dob, employdata.emp_licencenum,DATE_FORMAT(employdata.emp_licenceexpire, '%d %b %y') as LicenceExpDate ,DATE_FORMAT(employdata.emp_dob, '%d %b %y') as DOB, employdata.emp_status, employdata.emp_type,employdata.emp_login_id, employdata.emp_pwd, employdata.operatedby, minimasters.mm_name, branch_info.branchname,employdata.emp_bloodgrp, employdata.emp_address,DATE_FORMAT(employdata.emp_doj, '%d %b %y') as DOJ, employdata.emp_experience,employdata.Phoneno,employdata.gender, employdata.fathernme,employdata.eduqual, employdata.techqual, employdata.bankac, employdata.marital, employdata.nationality FROM employdata INNER JOIN minimasters ON employdata.dept_id = minimasters.sno INNER JOIN branch_info ON employdata.branch_id = branch_info.brnch_sno WHERE (employdata.branch_id = @BranchID)  and (employdata.emp_sno=@EmpSno)");
            cmd.Parameters.Add("@EmpSno", empname);
            cmd.Parameters.Add("@BranchID", branchid);
            DataTable dtDriver = vdm.SelectQuery(cmd).Tables[0];
            List<empdata> driversinfo = new List<empdata>();
            if (dtDriver.Rows.Count > 0)
            {
               
                empdata data = new empdata(); 
                data.DriverName = dtDriver.Rows[0]["employname"].ToString();
                data.EmpID= dtDriver.Rows[0]["employid"].ToString();
                data.MobNo = dtDriver.Rows[0]["Phoneno"].ToString();
                data.Doj = dtDriver.Rows[0]["DOJ"].ToString();
                data.DOB = dtDriver.Rows[0]["DOB"].ToString();
                data.emptype = dtDriver.Rows[0]["emp_type"].ToString();
                data.FathersName= dtDriver.Rows[0]["fathernme"].ToString();
                data.Gender = dtDriver.Rows[0]["gender"].ToString();
                data.Qualification = dtDriver.Rows[0]["eduqual"].ToString();
                data.Status = dtDriver.Rows[0]["marital"].ToString();
                data.Nationality= dtDriver.Rows[0]["nationality"].ToString();
                data.Address = dtDriver.Rows[0]["emp_address"].ToString();
                data.bloodgroup = dtDriver.Rows[0]["emp_bloodgrp"].ToString();
                data.LicenceNo = dtDriver.Rows[0]["emp_licencenum"].ToString();
                data.LicenceExpdate = dtDriver.Rows[0]["LicenceExpDate"].ToString();
                data.Experience = dtDriver.Rows[0]["emp_experience"].ToString();
                data.BankName = dtDriver.Rows[0]["bankac"].ToString();
                data.AccountNo = dtDriver.Rows[0]["bankac"].ToString();
                data.title = title;
                data.addr = companyAddress;
                driversinfo.Add(data);
                string response = GetJson(driversinfo);
                context.Response.Write(response);
            }
            else
            {
                string response1 = "No Information Found";
                context.Response.Write(response1);
            }
            
        }
        catch (Exception ex)
        {
            string Response = GetJson(ex.Message);
            context.Response.Write(Response);
        }
    }
    public class employeedetails
    {
        public string sno { get; set; }
        public string employeename { get; set; }
        public string logintime { get; set; }
        public string logouttime { get; set; }
        public string ipaddress { get; set; }
        public string devicetype { get; set; }
        public string leveltype { get; set; }
        public string loginstatus { get; set; }
        public string sessiontimeout { get; set; }

        public string indate { get; set; }
        public string intime { get; set; }
        public string outdate { get; set; }
        public string outtime { get; set; }
        public string timeinterval { get; set; }
    }
    private void get_employee_details(HttpContext context)
    {
        string BranchID = context.Session["Branch_ID"].ToString();
        cmd = new MySqlCommand("SELECT emp_sno, employid, employname, branch_id, emp_login_id, emp_pwd,emp_type,loginstatus FROM employdata  WHERE  (branch_id = @branchid)");
        cmd.Parameters.Add("@branchid", BranchID);
        DataTable dtemployee = vdm.SelectQuery(cmd).Tables[0];
        List<employeedetails> emloyeeedetalis = new List<employeedetails>();
        if (dtemployee.Rows.Count > 0)
        {
            foreach (DataRow dr in dtemployee.Rows)
            {
                employeedetails details = new employeedetails();
                details.sno = dr["emp_sno"].ToString();
                details.employeename = dr["employname"].ToString();
                details.leveltype = dr["emp_type"].ToString();
                string status = dr["loginstatus"].ToString();
                if (status == "1")
                {
                    status = "Active";
                }
                else
                {
                    status = "InActive";
                }
               
                details.loginstatus = status;
                emloyeeedetalis.Add(details);
            }
            string response = GetJson(emloyeeedetalis);
            context.Response.Write(response);
        }
    }
    private void btn_getlogininfoemployee_details(HttpContext context)
    {
        string BranchID = context.Session["Branch_ID"].ToString();
        string employeeid = context.Request["employeeid"];
        string fromdate = context.Request["fromdate"];
        string todate = context.Request["todate"];
        string date = context.Request["date"];
        DateTime dtfromdate = Convert.ToDateTime(fromdate);
        DateTime dttodate = Convert.ToDateTime(todate);
        DateTime dtdate = Convert.ToDateTime(date);
        if (employeeid == "" || employeeid == null )
        {
            cmd = new MySqlCommand("SELECT employdata.emp_sno, employdata.employid, employdata.employname, employdata.branch_id, employdata.emp_login_id, employdata.emp_pwd, employdata.emp_type, login_info.empid, login_info.logintime, login_info.logouttime, login_info.devicetype, login_info.ipaddress FROM employdata INNER JOIN login_info ON employdata.emp_sno = login_info.empid WHERE  (employdata.branch_id = @branchid) AND (login_info.logintime BETWEEN @d1 AND @d2)");
            cmd.Parameters.Add("@d1", GetLowDate(dtdate));
            cmd.Parameters.Add("@d2", GetHighDate(dtdate));
            cmd.Parameters.Add("@branchid", BranchID);
        }
        else
        {
            cmd = new MySqlCommand("SELECT  employdata.emp_sno, employdata.employid, employdata.employname, employdata.branch_id, employdata.emp_login_id, employdata.emp_pwd, employdata.emp_type, login_info.empid, login_info.logintime, login_info.logouttime, login_info.devicetype,  login_info.ipaddress FROM   employdata INNER JOIN login_info ON employdata.emp_sno = login_info.empid WHERE (employdata.branch_id = @branchid) AND (login_info.logintime BETWEEN @d1 AND @d2) AND (employdata.emp_sno = @sno)");
            cmd.Parameters.Add("@sno", employeeid);
            cmd.Parameters.Add("@d1", GetLowDate(dtfromdate));
            cmd.Parameters.Add("@d2", GetHighDate(dttodate));
            cmd.Parameters.Add("@branchid", BranchID);
        }
        DataTable dtloginfo = vdm.SelectQuery(cmd).Tables[0];
        List<employeedetails> emloyeeedetalis = new List<employeedetails>();
        if (dtloginfo.Rows.Count > 0)
        {
            foreach (DataRow dr in dtloginfo.Rows)
            {
                string LogoutTimes = dr["LogoutTime"].ToString();
                if (LogoutTimes != "")
                {
                    employeedetails details = new employeedetails();
                    details.sno = dr["empid"].ToString();
                    details.employeename = dr["employname"].ToString();
                    details.logintime = dr["logintime"].ToString();
                    details.logouttime = dr["logouttime"].ToString();
                    //details.sessiontimeout = dr["SessionPeriod"].ToString();
                    details.ipaddress = dr["ipaddress"].ToString();
                    details.devicetype = dr["devicetype"].ToString();
                    emloyeeedetalis.Add(details);
                }
            }
            string response = GetJson(emloyeeedetalis);
            context.Response.Write(response);
        }
    }

    //KS 

    public class Perioddates
    {
        public string Betweendate { get; set; }
    }

    public class Periodfuel
    {
        public string periodvhileno { get; set; }
        public string periodBetweendate { get; set; }
        public string periodfuel { get; set; }
        public string periodkm { get; set; }
    }

    public class DailyList
    {
        public List<Perioddates> ListBetweendate { get; set; }
        public List<Periodfuel> Listperiodfuel { get; set; }
    }


    private void getDailyinfo(HttpContext context)
    {
        List<Perioddates> Alldateslists = new List<Perioddates>();
        List<Periodfuel> AllPeriodfuel = new List<Periodfuel>();
        List<DailyList> DailyList = new List<DailyList>();

        string BranchID = context.Session["Branch_ID"].ToString();
        string fromdate = context.Request["fromdate"];
        string todate = context.Request["todate"];
        string Type = context.Request["Type"];
        DateTime FrmDate = Convert.ToDateTime(fromdate);
        DateTime Tdate = Convert.ToDateTime(todate);
        TimeSpan dateSpan = Tdate.Subtract(FrmDate);
        int NoOfdays = dateSpan.Days;
        NoOfdays = NoOfdays + 2;

        int count = 0;
        DateTime dtFrm = new DateTime();
        for (int j = 1; j < NoOfdays; j++)
        {
            if (j == 1)
            {
                dtFrm = FrmDate;
            }
            else
            {
                dtFrm = dtFrm.AddDays(1);
            }
            string strdate = dtFrm.ToString("dd");
            Perioddates obj1 = new Perioddates();
            obj1.Betweendate = strdate;
            Alldateslists.Add(obj1);
            count++;
        }
       

        //
        if (Type == "ALL")
        {
            cmd = new MySqlCommand(" SELECT derivedtbl_1.sno, derivedtbl_1.tripdate, derivedtbl_1.Tkms AS TripKMS, derivedtbl_1.gpskms, derivedtbl_1.vehicleno, derivedtbl_1.endfuelvalue AS Insidefuel, derivedtbl_1.act_mileage, derivedtbl_1.branch_id,  " +
                            "   derivedtbl_1.registration_no, tl.doe, tl.tripsno, derivedtbl_1.endfuelvalue AS Insidefuel, tl.fuel AS Outsidefuel, derivedtbl_1.Tkms / (derivedtbl_1.endfuelvalue + tl.fuel) AS TripMileage,  " +
                            "  derivedtbl_1.endfuelvalue + tl.fuel AS TripFuel, tl.km" +
                            " FROM  (SELECT  tripdata.sno, tripdata.endodometerreading - tripdata.vehiclestartreading AS Tkms, tripdata.gpskms, tripdata.status, tripdata.vehicleno, tripdata.endfuelvalue, DATE_FORMAT(tripdata.tripdate, '%Y-%m-%d') AS tripdate,  " +
                            " vehicel_master.act_mileage, vehicel_master.registration_no, minimasters.branch_id " +
                            " FROM  tripdata INNER JOIN " +
                            " vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno INNER JOIN " +
                            "  minimasters ON vehicel_master.vhtype_refno = minimasters.sno  " +
                            " WHERE (tripdata.tripdate BETWEEN @d1 AND @d2) AND tripdata.userid=@branchid  GROUP BY DATE_FORMAT(tripdata.tripdate, '%Y-%m-%d'), tripdata.vehicleno) derivedtbl_1 INNER JOIN  " +
                            " (SELECT  DATE_FORMAT(doe, '%Y-%m-%d') AS doe, tripsno, SUM(fuel) AS fuel, SUM(km) AS km " +
                            "  FROM   triplogs " +
                            " GROUP BY DATE_FORMAT(doe, '%Y-%m-%d'), tripsno) tl ON derivedtbl_1.sno = tl.tripsno AND derivedtbl_1.tripdate = tl.doe " +
                            " ORDER BY derivedtbl_1.vehicleno,derivedtbl_1.sno, tl.doe");
            cmd.Parameters.Add("@Tanker", "All");
        }
        else if (Type == "All Puffs")
        {
            cmd = new MySqlCommand(" SELECT derivedtbl_1.sno, derivedtbl_1.tripdate, derivedtbl_1.Tkms AS TripKMS, derivedtbl_1.gpskms, derivedtbl_1.vehicleno, derivedtbl_1.endfuelvalue AS Insidefuel, derivedtbl_1.act_mileage, derivedtbl_1.branch_id,  " +
                            "   derivedtbl_1.registration_no, tl.doe, tl.tripsno, derivedtbl_1.endfuelvalue AS Insidefuel, tl.fuel AS Outsidefuel, derivedtbl_1.Tkms / (derivedtbl_1.endfuelvalue + tl.fuel) AS TripMileage,  " +
                            "  derivedtbl_1.endfuelvalue + tl.fuel AS TripFuel, tl.km" +
                            " FROM  (SELECT  tripdata.sno, tripdata.endodometerreading - tripdata.vehiclestartreading AS Tkms, tripdata.gpskms, tripdata.status, tripdata.vehicleno, tripdata.endfuelvalue, DATE_FORMAT(tripdata.tripdate, '%Y-%m-%d') AS tripdate,  " +
                            " vehicel_master.act_mileage, vehicel_master.registration_no, minimasters.branch_id " +
                            " FROM  tripdata INNER JOIN " +
                            " vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno INNER JOIN " +
                            "  minimasters ON vehicel_master.vhtype_refno = minimasters.sno  " +
                            " WHERE (tripdata.tripdate BETWEEN @d1 AND @d2) AND minimasters.mm_name =@Tanker AND tripdata.userid=@branchid   GROUP BY DATE_FORMAT(tripdata.tripdate, '%Y-%m-%d'), tripdata.vehicleno) derivedtbl_1 INNER JOIN  " +
                            " (SELECT  DATE_FORMAT(doe, '%Y-%m-%d') AS doe, tripsno, SUM(fuel) AS fuel, SUM(km) AS km " +
                            "  FROM   triplogs " +
                            " GROUP BY DATE_FORMAT(doe, '%Y-%m-%d'), tripsno) tl ON derivedtbl_1.sno = tl.tripsno AND derivedtbl_1.tripdate = tl.doe " +
                            " ORDER BY derivedtbl_1.vehicleno,derivedtbl_1.sno, tl.doe");
            cmd.Parameters.Add("@Tanker", "Puff");
        }
        else if (Type == "All Tankers")
        {
            cmd = new MySqlCommand(" SELECT derivedtbl_1.sno, derivedtbl_1.tripdate, derivedtbl_1.Tkms AS TripKMS, derivedtbl_1.gpskms, derivedtbl_1.vehicleno, derivedtbl_1.endfuelvalue AS Insidefuel, derivedtbl_1.act_mileage, derivedtbl_1.branch_id,  " +
                            "   derivedtbl_1.registration_no, tl.doe, tl.tripsno, derivedtbl_1.endfuelvalue AS Insidefuel, tl.fuel AS Outsidefuel, derivedtbl_1.Tkms / (derivedtbl_1.endfuelvalue + tl.fuel) AS TripMileage,  " +
                            "  derivedtbl_1.endfuelvalue + tl.fuel AS TripFuel, tl.km" +
                            " FROM  (SELECT  tripdata.sno, tripdata.endodometerreading - tripdata.vehiclestartreading AS Tkms, tripdata.gpskms, tripdata.status, tripdata.vehicleno, tripdata.endfuelvalue, DATE_FORMAT(tripdata.tripdate, '%Y-%m-%d') AS tripdate,  " +
                            " vehicel_master.act_mileage, vehicel_master.registration_no, minimasters.branch_id " +
                            " FROM  tripdata INNER JOIN " +
                            " vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno INNER JOIN " +
                            "  minimasters ON vehicel_master.vhtype_refno = minimasters.sno  " +
                            " WHERE (tripdata.tripdate BETWEEN @d1 AND @d2) AND minimasters.mm_name =@Tanker AND tripdata.userid=@branchid GROUP BY DATE_FORMAT(tripdata.tripdate, '%Y-%m-%d'), tripdata.vehicleno) derivedtbl_1 INNER JOIN  " +
                            " (SELECT  DATE_FORMAT(doe, '%Y-%m-%d') AS doe, tripsno, SUM(fuel) AS fuel, SUM(km) AS km " +
                            "  FROM   triplogs " +
                            " GROUP BY DATE_FORMAT(doe, '%Y-%m-%d'), tripsno) tl ON derivedtbl_1.sno = tl.tripsno AND derivedtbl_1.tripdate = tl.doe " +
                            " ORDER BY derivedtbl_1.vehicleno,derivedtbl_1.sno, tl.doe");
            cmd.Parameters.Add("@Tanker", "Tanker");
            
        }
        else
        {
            cmd = new MySqlCommand(" SELECT derivedtbl_1.sno, derivedtbl_1.tripdate, derivedtbl_1.Tkms AS TripKMS, derivedtbl_1.gpskms, derivedtbl_1.vehicleno, derivedtbl_1.endfuelvalue AS Insidefuel, derivedtbl_1.act_mileage, derivedtbl_1.branch_id,  " +
                             "   derivedtbl_1.registration_no, tl.doe, tl.tripsno, derivedtbl_1.endfuelvalue AS Insidefuel, tl.fuel AS Outsidefuel, derivedtbl_1.Tkms / (derivedtbl_1.endfuelvalue + tl.fuel) AS TripMileage,  " +
                             "  derivedtbl_1.endfuelvalue + tl.fuel AS TripFuel, tl.km" +
                             " FROM  (SELECT  tripdata.sno, tripdata.endodometerreading - tripdata.vehiclestartreading AS Tkms, tripdata.gpskms, tripdata.status, tripdata.vehicleno, tripdata.endfuelvalue, DATE_FORMAT(tripdata.tripdate, '%Y-%m-%d') AS tripdate,  " +
                             " vehicel_master.act_mileage, vehicel_master.registration_no, minimasters.branch_id " +
                             " FROM  tripdata INNER JOIN " +
                             " vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno INNER JOIN " +
                             "  minimasters ON vehicel_master.vhtype_refno = minimasters.sno  " +
                             " WHERE (tripdata.tripdate BETWEEN @d1 AND @d2) AND tripdata.userid=@branchid  GROUP BY DATE_FORMAT(tripdata.tripdate, '%Y-%m-%d'), tripdata.vehicleno) derivedtbl_1 INNER JOIN  " +
                             " (SELECT  DATE_FORMAT(doe, '%Y-%m-%d') AS doe, tripsno, SUM(fuel) AS fuel, SUM(km) AS km " +
                             "  FROM   triplogs " +
                             " GROUP BY DATE_FORMAT(doe, '%Y-%m-%d'), tripsno) tl ON derivedtbl_1.sno = tl.tripsno AND derivedtbl_1.tripdate = tl.doe " +
                             " ORDER BY derivedtbl_1.vehicleno,derivedtbl_1.sno, tl.doe");
            cmd.Parameters.Add("@Tanker", "All");
        }

        cmd.Parameters.Add("@d1", GetLowDate(FrmDate));
        cmd.Parameters.Add("@d2", GetHighDate(Tdate));
        cmd.Parameters.Add("@branchid", BranchID);
        
        DataTable dailyinfo = vdm.SelectQuery(cmd).Tables[0];
        if (dailyinfo.Rows.Count > 0)
        {
            foreach (DataRow dr in dailyinfo.Rows)
            {
                Periodfuel obj2 = new Periodfuel();
                obj2.periodvhileno = dr["registration_no"].ToString();
                obj2.periodBetweendate = Convert.ToDateTime(dr["doe"].ToString()).ToString("dd");
                obj2.periodfuel = dr["TripFuel"].ToString();
                obj2.periodkm = dr["km"].ToString();
                AllPeriodfuel.Add(obj2);
            }
        }
        //
        DailyList obj = new DailyList();
        obj.ListBetweendate = Alldateslists;
        obj.Listperiodfuel = AllPeriodfuel;

        DailyList.Add(obj);
        string response = GetJson(DailyList);
        context.Response.Write(response);
    }

    private class VehicleRateMaster
    {
        public string Sno { get; set; }
        public string ID { get; set; }
        public string MID { get; set; }
        public string VID { get; set; }
        public string VehicleRegistrationNo { get; set; }
        public string Capacity { get; set; }
        public string PerMonth { get; set; }
        public string PerDay { get; set; }
        public string PerKm { get; set; }
        public string PerKg { get; set; }
        public string PerTrip { get; set; }
        public string PerKmEmpty { get; set; }
        public string PresentDefaultMode { get; set; }

        //
        public string DeviceId { get; set; }
        public string Phoneno { get; set; }
        public string Logdate { get; set; }
        public string Device { get; set; }
        public string Sim { get; set; }
        public string Wire { get; set; }
        public string Vehicle { get; set; }
        public string Status { get; set; }

    }
    private void GetVehicleModuleConfig(HttpContext context)
    {
        try
        {
            string Mid = context.Session["MID"].ToString();
            string ddlVehicletype = context.Request["ddlVehicletype"];
            //  cmd = new MySqlCommand(" SELECT ID, MID, VID, VehicleRegistrationNo, Capacity, PerMonth, PerDay, PerKm, PerKg, PerTrip, PerKmEmpty, PresentDefaultMode FROM vmsc WHERE(MID = @Mid) ORDER BY Capacity ");            
            cmd = new MySqlCommand(" SELECT vmsc.ID, vmsc.MID, vmsc.VID, vmsc.VehicleRegistrationNo, vmsc.Capacity, vmsc.PerMonth, vmsc.PerDay, vmsc.PerKm, vmsc.PerKg, vmsc.PerTrip, vmsc.PerKmEmpty, vmsc.PresentDefaultMode FROM vehicel_master INNER JOIN minimasters ON vehicel_master.vhtype_refno = minimasters.sno INNER JOIN vmsc ON vehicel_master.vm_sno = vmsc.VID WHERE(minimasters.sno = @ddlVehicletype) AND(vmsc.MID = @Mid) ");
            cmd.Parameters.Add("@ddlVehicletype", ddlVehicletype);
            cmd.Parameters.Add("@Mid", Mid);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            List<VehicleRateMaster> VehicleRateMasterList = new List<VehicleRateMaster>();
            int i = 1;
            foreach (DataRow dr in dt.Rows)
            {
                VehicleRateMaster GetVehicleRateMaster = new VehicleRateMaster();

                GetVehicleRateMaster.Sno = i++.ToString();
                GetVehicleRateMaster.ID = dr["ID"].ToString();
                GetVehicleRateMaster.MID = dr["MID"].ToString();
                GetVehicleRateMaster.VID = dr["VID"].ToString();
                GetVehicleRateMaster.VehicleRegistrationNo = dr["VehicleRegistrationNo"].ToString();
                GetVehicleRateMaster.Capacity = dr["Capacity"].ToString();
                GetVehicleRateMaster.PerMonth = dr["PerMonth"].ToString();
                GetVehicleRateMaster.PerDay = dr["PerDay"].ToString();
                GetVehicleRateMaster.PerKm = dr["PerKm"].ToString();
                GetVehicleRateMaster.PerKg = dr["PerKg"].ToString();
                GetVehicleRateMaster.PerTrip = dr["PerTrip"].ToString();
                GetVehicleRateMaster.PerKmEmpty = dr["PerKmEmpty"].ToString();
                GetVehicleRateMaster.PresentDefaultMode = dr["PresentDefaultMode"].ToString();                
                VehicleRateMasterList.Add(GetVehicleRateMaster);
            }

            string response = GetJson(VehicleRateMasterList);
            context.Response.Write(response);
        }
        catch
        {
        }
    }    
    private void btnUpdateVehicleRate(HttpContext context)
    {
        try
        {
            string ID = context.Request["ID"];
            string PerMonth = context.Request["PerMonth"];
            string PerDay = context.Request["PerDay"];
            string PerKm = context.Request["PerKm"];
            string PerKg = context.Request["PerKg"];
            string PerTrip = context.Request["PerTrip"];
            string PerKmEmpty = context.Request["PerKmEmpty"];
            string PresentDefaultMode = context.Request["PresentDefaultMode"];
            // cmd = new MySqlCommand("Update vmsc set PerMonth=@PerMonth, PerDay=@PerDay, PerKm=@PerKm, PerKg=@PerKg, PerTrip=@PerTrip, PerKmEmpty=@PerKmEmpt, PresentDefaultMode=@PresentDefaultMode Where ID=@ID");
            cmd = new MySqlCommand("Update vmsc set PerMonth=@PerMonth, PerDay=@PerDay, PerKm=@PerKm, PerKg=@PerKg, PerTrip=@PerTrip, PerKmEmpty=@PerKmEmpty, PresentDefaultMode=@PresentDefaultMode Where ID=@ID");
            cmd.Parameters.Add("@ID", ID);
            cmd.Parameters.Add("@PerMonth", PerMonth);
            cmd.Parameters.Add("@PerDay", PerDay);
            cmd.Parameters.Add("@PerKm", PerKm);
            cmd.Parameters.Add("@PerKg", PerKg);
            cmd.Parameters.Add("@PerTrip", PerTrip);
            cmd.Parameters.Add("@PerKmEmpty", PerKmEmpty);
            cmd.Parameters.Add("@PresentDefaultMode", PresentDefaultMode);
            vdm.Update(cmd);
            string msg = "Data Updated Successfully...";
            string response = GetJson(msg);
            context.Response.Write(response);
        }
        catch(Exception ex)
        {
            string response = GetJson(ex.ToString());
            context.Response.Write(response);
        }
    }
    private void get_BillingOwnersList(HttpContext context)
    {
        try
        {
            string OwnerType = context.Request["OwnerType"];
            cmd = new MySqlCommand("SELECT sno, vendorname FROM vendors_info WHERE  (vendor_type =@OwnerType)");//(branch_id = @BranchID) and
            cmd.Parameters.Add("@OwnerType", OwnerType);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            List<VehicleRateMaster> BillingOwnersList = new List<VehicleRateMaster>();
            if (dt.Rows.Count > 0)
            {
                foreach(DataRow dr in dt.Rows)
                {
                    VehicleRateMaster BillingOwners = new VehicleRateMaster();
                    BillingOwners.Sno = dr["sno"].ToString();
                    BillingOwners.ID = dr["vendorname"].ToString();
                    BillingOwnersList.Add(BillingOwners);
                }
            }
            string response = GetJson(BillingOwnersList);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {

        }
    }

    private void get_gps_plantname(HttpContext context)
    {
        try
        {
            GpsDBManager GpsDB = new GpsDBManager();
            string ddlPlantname = context.Request["ddlPlantname"];

            cmd = new MySqlCommand(" SELECT DISTINCT(cabmanagement.PlantName) AS PlantName FROM cabmanagement Order by PlantName ");
            cmd.Parameters.Add("@ddlPlantname", ddlPlantname);

            DataTable dt = GpsDB.SelectQuery(cmd).Tables[0];
            List<VehicleRateMaster> VehicleRateMasterList = new List<VehicleRateMaster>();
            int i = 1;
            foreach (DataRow dr in dt.Rows)
            {
                VehicleRateMaster GetVehicleRateMaster = new VehicleRateMaster();

                GetVehicleRateMaster.Sno = dr["PlantName"].ToString();// i++.ToString();                
                GetVehicleRateMaster.VID = dr["PlantName"].ToString();
               
                VehicleRateMasterList.Add(GetVehicleRateMaster);
            }

            string response = GetJson(VehicleRateMasterList);
            context.Response.Write(response);
        }
        catch
        {
        }
    }

    
    private void GetGPsVehicleInformations(HttpContext context)
    {
        try
        {
            GpsDBManager GpsDB = new GpsDBManager();
            
            string ddlPlantname = context.Request["ddlPlantname"];
            if (ddlPlantname == "All")
            {
                cmd = new MySqlCommand(" SELECT paireddata.VehicleNumber, paireddata.GprsDevID, paireddata.PhoneNumber, onlinetable.`Timestamp`, cabmanagement.PlantName, STR_TO_DATE(onlinetable.`Timestamp`, '%d/%m/%Y') AS D,cabmanagement.VehicleType FROM  paireddata INNER JOIN onlinetable ON paireddata.VehicleNumber = onlinetable.VehicleID INNER JOIN cabmanagement ON onlinetable.VehicleID = cabmanagement.VehicleID WHERE(LENGTH(paireddata.PhoneNumber) >= 10) AND(LENGTH(paireddata.GprsDevID) > 10)  ORDER BY D DESC; ");
               // cmd.Parameters.Add("@ddlPlantname", ddlPlantname);
            }
            else
            {
                cmd = new MySqlCommand(" SELECT paireddata.VehicleNumber, paireddata.GprsDevID, paireddata.PhoneNumber, onlinetable.`Timestamp`, cabmanagement.PlantName, STR_TO_DATE(onlinetable.`Timestamp`, '%d/%m/%Y') AS D,cabmanagement.VehicleType FROM  paireddata INNER JOIN onlinetable ON paireddata.VehicleNumber = onlinetable.VehicleID INNER JOIN cabmanagement ON onlinetable.VehicleID = cabmanagement.VehicleID WHERE(LENGTH(paireddata.PhoneNumber) >= 10) AND(LENGTH(paireddata.GprsDevID) > 10) AND cabmanagement.PlantName=@ddlPlantname ORDER BY D DESC; ");
                cmd.Parameters.Add("@ddlPlantname", ddlPlantname);
            }
           
            DataTable dt = GpsDB.SelectQuery(cmd).Tables[0];
            List<VehicleRateMaster> VehicleRateMasterList = new List<VehicleRateMaster>();
            DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
            int i = 1;
            foreach (DataRow dr in dt.Rows)
            {
                VehicleRateMaster GetVehicleRateMaster = new VehicleRateMaster();
                GetVehicleRateMaster.Sno = i++.ToString();                
                GetVehicleRateMaster.VehicleRegistrationNo = dr["VehicleNumber"].ToString();
                GetVehicleRateMaster.Logdate = dr["D"].ToString();
                GetVehicleRateMaster.Capacity = dr["VehicleType"].ToString();
                

                GetVehicleRateMaster.DeviceId = dr["GprsDevID"].ToString();
                GetVehicleRateMaster.Phoneno = dr["PhoneNumber"].ToString();
                //
                DateTime Logdate1 = Convert.ToDateTime(GetVehicleRateMaster.Logdate);
                TimeSpan dateSpan = Logdate1.Subtract(ServerDateCurrentdate);
                int NoOfdays = dateSpan.Days;
               
                    if (NoOfdays < 0)
                    {
                        GetVehicleRateMaster.Status = "Red";
                    }
                    else
                    {
                        GetVehicleRateMaster.Status = "orange";
                    }
              
                string InsExpdate = Logdate1.ToString("dd/MMM/yyyy");
                GetVehicleRateMaster.Logdate = InsExpdate;
                //

                VehicleRateMasterList.Add(GetVehicleRateMaster);
            }
            string response = GetJson(VehicleRateMasterList);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
        }
    }

    private void btnUpdateVehicleGpsInformation(HttpContext context)
    {
        try
        {
            GpsDBManager GpsDB = new GpsDBManager();
            string LogDate = context.Request["LogDate"];
            string Device = context.Request["Device"];
            string Sim = context.Request["Sim"];
            string Wire = context.Request["Wire"];
            string Vehicle = context.Request["Vehicle"];
            string username = context.Session["Employ_Sno"].ToString();

            string VehicleRegistrationNo = context.Request["VehicleRegistrationNo"];

            DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
            DateTime Logdate1 = Convert.ToDateTime(LogDate);

            cmd = new MySqlCommand(" Update cabmanagement SET Device=@Device,Sim=@Sim,Wire=@Wire,Vehicle=@Vehicle Where cabmanagement.VehicleID=@VehicleRegistrationNo ");
            cmd.Parameters.Add("@VehicleRegistrationNo", VehicleRegistrationNo);
            cmd.Parameters.Add("@Device", Device);
            cmd.Parameters.Add("@Sim", Sim);
            cmd.Parameters.Add("@Wire", Wire);
            cmd.Parameters.Add("@Vehicle", Vehicle);
                        
            GpsDB.Update(cmd);
            string msg = "Data Update Successfully...";
            //Insert
            cmd = new MySqlCommand(" Insert into GpsDeviceAlertInfo(vehicleid,Logdate,Device,Sim,Wire,Vehicle,EntryBy,EntryDate) Values(@VehicleRegistrationNo,@LogDate,@Device,@Sim,@Wire,@Vehicle,@EntryBy,@EntryDate) ");
            cmd.Parameters.Add("@VehicleRegistrationNo", VehicleRegistrationNo);
            cmd.Parameters.Add("@LogDate", Logdate1.ToString("yyyy/MM/dd"));
            cmd.Parameters.Add("@Device", Device);
            cmd.Parameters.Add("@Sim", Sim);
            cmd.Parameters.Add("@Wire", Wire);
            cmd.Parameters.Add("@Vehicle", Vehicle);
            cmd.Parameters.Add("@EntryBy", username);
            cmd.Parameters.Add("@EntryDate", ServerDateCurrentdate);

            GpsDB.insert(cmd);
            //

            string response = GetJson(msg);
            context.Response.Write(response);
        }
        catch (Exception ex)
        {
            context.Response.Write(ex.ToString());
        }
    }


}