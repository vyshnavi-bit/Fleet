using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class VehicleMasterReport : System.Web.UI.Page
{
    MySqlCommand cmd;
    string BranchID = "";
    VehicleDBMgr vdm;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Branch_ID"] == null)
            Response.Redirect("Login.aspx");
        else
        {
            BranchID = Session["Branch_ID"].ToString();
            vdm = new VehicleDBMgr();
            if (!Page.IsPostBack)
            {
                if (!Page.IsCallback)
                {
                    GetReport();
                    lblAddress.Text = Session["Address"].ToString();
                    lblTitle.Text = Session["TitleName"].ToString();
                }
            }
        }
    }
    DataTable Report = new DataTable();
    void GetReport()
    {
        try
        {
            lblmsg.Text = "";
            DataTable dtVehicle = new DataTable();
            Report.Columns.Add("Sno");
            Report.Columns.Add("VehicleSno");
            Report.Columns.Add("VehicleNo");
            Report.Columns.Add("Capacity");
            Report.Columns.Add("Type");
            Report.Columns.Add("Make");
            Report.Columns.Add("Owner");
            Report.Columns.Add("Permitt");
            Report.Columns.Add("PermitExpDate");
            Report.Columns.Add("Pollution");
            Report.Columns.Add("PolExpDate");
            Report.Columns.Add("Insurance");
            Report.Columns.Add("InsExpDate");
            Report.Columns.Add("Fitness");
            Report.Columns.Add("FitExpDate");
            Report.Columns.Add("RoadTax");
            Report.Columns.Add("RoadTaxExpDate");
            Report.Columns.Add("ModelNo");
            Report.Columns.Add("EngineNo");
            Report.Columns.Add("ChasisNo");
            Report.Columns.Add("InsuranceCompany");
            Report.Columns.Add("Image");
            if (BranchID == "1")
            {
                cmd = new MySqlCommand("SELECT vehicel_master.vm_sno,vehicel_master.registration_no AS VehicleNo, vehicel_master.Capacity, minimasters.mm_name AS VehicleType,minimasters_1.mm_name AS Make, vehicel_master.vm_owner AS Owner, vehicel_master.vm_rcno AS Permitt, DATE_FORMAT(vehicel_master.vm_rcexpdate, '%d %b %y') AS PermittExpDate, vehicel_master.vm_pollution AS PollutionNo,DATE_FORMAT(vehicel_master.vm_poll_exp_date, '%d %b %y') AS PolExpdate, vehicel_master.vm_insurance AS InsuranceNo,DATE_FORMAT(vehicel_master.vm_isurence_exp_date, '%d %b %y') AS InsExpDate, DATE_FORMAT(vehicel_master.vm_fit_exp_date, '%d %b %y') AS FitExpDate,vehicel_master.vm_fitness AS Fitness, vehicel_master.vm_roadtax AS RoadTax, DATE_FORMAT(vehicel_master.vm_roadtax_exp_date, '%d %b %y') AS RoadTaxExpDate , vehicel_master.vm_model AS ModelNo, vehicel_master.vm_engine AS EngineNo, vehicel_master.vm_chasiss AS ChasisNo, vendors_info.vendorname AS InsuranceComapanyName FROM vehicel_master INNER JOIN minimasters ON vehicel_master.vhtype_refno = minimasters.sno INNER JOIN minimasters minimasters_1 ON vehicel_master.vhmake_refno = minimasters_1.sno LEFT OUTER JOIN vendors_info ON vehicel_master.insurancesno = vendors_info.sno WHERE (vehicel_master.branch_id = @BranchID) AND (vehicel_master.vm_owner=@Owner) ORDER BY Owner");
                cmd.Parameters.Add("@Owner", Session["shortname"].ToString());
            }
            else
            {
                cmd = new MySqlCommand("SELECT vehicel_master.vm_sno,vehicel_master.registration_no AS VehicleNo, vehicel_master.Capacity, minimasters.mm_name AS VehicleType,minimasters_1.mm_name AS Make, vehicel_master.vm_owner AS Owner, vehicel_master.vm_rcno AS Permitt, DATE_FORMAT(vehicel_master.vm_rcexpdate, '%d %b %y') AS PermittExpDate, vehicel_master.vm_pollution AS PollutionNo,DATE_FORMAT(vehicel_master.vm_poll_exp_date, '%d %b %y') AS PolExpdate, vehicel_master.vm_insurance AS InsuranceNo,DATE_FORMAT(vehicel_master.vm_isurence_exp_date, '%d %b %y') AS InsExpDate, DATE_FORMAT(vehicel_master.vm_fit_exp_date, '%d %b %y') AS FitExpDate,vehicel_master.vm_fitness AS Fitness, vehicel_master.vm_roadtax AS RoadTax, DATE_FORMAT(vehicel_master.vm_roadtax_exp_date, '%d %b %y') AS RoadTaxExpDate , vehicel_master.vm_model AS ModelNo, vehicel_master.vm_engine AS EngineNo, vehicel_master.vm_chasiss AS ChasisNo, vendors_info.vendorname AS InsuranceComapanyName FROM vehicel_master INNER JOIN minimasters ON vehicel_master.vhtype_refno = minimasters.sno INNER JOIN minimasters minimasters_1 ON vehicel_master.vhmake_refno = minimasters_1.sno LEFT OUTER JOIN vendors_info ON vehicel_master.insurancesno = vendors_info.sno WHERE (vehicel_master.branch_id = @BranchID) ORDER BY Owner");
                cmd.Parameters.Add("@Owner", Session["shortname"].ToString());
            }
            cmd.Parameters.Add("@BranchID", BranchID); 
            dtVehicle = vdm.SelectQuery(cmd).Tables[0];
            if (dtVehicle.Rows.Count > 0)
            {
                int i = 1;
                foreach (DataRow dr in dtVehicle.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["Sno"] = i++.ToString();
                    newrow["VehicleSno"] = dr["vm_sno"].ToString();
                    newrow["VehicleNo"] = dr["VehicleNo"].ToString();
                    newrow["Capacity"] = dr["Capacity"].ToString();
                    newrow["Type"] = dr["VehicleType"].ToString();
                    newrow["Make"] = dr["Make"].ToString();
                    newrow["Owner"] = dr["Owner"].ToString();
                    newrow["Permitt"] = dr["Permitt"].ToString();
                    newrow["PermitExpDate"] = dr["PermittExpDate"].ToString();
                    newrow["Pollution"] = dr["PollutionNo"].ToString();
                    newrow["PolExpDate"] = dr["PolExpdate"].ToString();
                    newrow["Insurance"] = dr["InsuranceNo"].ToString();
                    newrow["InsExpDate"] = dr["InsExpDate"].ToString();
                    newrow["Fitness"] = dr["Fitness"].ToString();
                    newrow["FitExpDate"] = dr["FitExpDate"].ToString();
                    newrow["RoadTax"] = dr["RoadTax"].ToString();
                    newrow["RoadTaxExpDate"] = dr["RoadTaxExpDate"].ToString();
                    newrow["ModelNo"] = dr["ModelNo"].ToString();
                    newrow["EngineNo"] = dr["EngineNo"].ToString();
                    newrow["ChasisNo"] = dr["ChasisNo"].ToString();
                    newrow["InsuranceCompany"] = dr["InsuranceComapanyName"].ToString();
                    Report.Rows.Add(newrow);
                }
                Session["title"] = "VehicleReport";
                Session["filename"] = "VehicleReport";
                Session["xportdata"] = Report;
                grdReports.DataSource = Report;
                grdReports.DataBind();
            }
            else
            {
                lblmsg.Text = "No data found";
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
}