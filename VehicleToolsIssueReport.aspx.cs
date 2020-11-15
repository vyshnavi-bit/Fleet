using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MySql.Data.MySqlClient;

public partial class VehicleToolsIssueReport : System.Web.UI.Page
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
                    txt_FromDate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                    txt_Todate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                    lblAddress.Text = Session["Address"].ToString();
                    lblTitle.Text = Session["TitleName"].ToString();
                }
            }
        }
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
    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        vdm = new VehicleDBMgr();

        if (ddlType.SelectedValue == "Vehicle Wise")
        {
            hideVehicles.Visible = true;
            cmd = new MySqlCommand("SELECT minimasters.mm_name, vehicel_master.registration_no, vehicel_master.vm_sno FROM vehicel_master INNER JOIN minimasters ON vehicel_master.vhtype_refno = minimasters.sno WHERE (vehicel_master.branch_id = @BranchID)");
            cmd.Parameters.Add("@BranchID", BranchID);
            DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
            ddlvehicles.DataSource = dttrips;
            ddlvehicles.DataTextField = "registration_no";
            ddlvehicles.DataValueField = "vm_sno";
            ddlvehicles.DataBind();
        }
        if (ddlType.SelectedValue == "All")
        {
            hideVehicles.Visible = false;
            ddlvehicles.Items.Clear();
        }
        if (ddlType.SelectedValue == "Select Type")
        {
            hideVehicles.Visible = false;
            ddlvehicles.Items.Clear();
        }

    }
    DataTable Report = new DataTable();
    protected void btn_Generate_Click(object sender, EventArgs e)
    {
        try
        {
            lblmsg.Text = "";
            DateTime fromdate = DateTime.Now;
            DateTime todate = DateTime.Now;
            string[] datestrig = txt_FromDate.Text.Split(' ');
            if (datestrig.Length > 1)
            {
                if (datestrig[0].Split('-').Length > 0)
                {
                    string[] dates = datestrig[0].Split('-');
                    string[] times = datestrig[1].Split(':');
                    fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            datestrig = txt_Todate.Text.Split(' ');
            if (datestrig.Length > 1)
            {
                if (datestrig[0].Split('-').Length > 0)
                {
                    string[] dates = datestrig[0].Split('-');
                    string[] times = datestrig[1].Split(':');
                    todate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            lblType.Text = ddlType.SelectedItem.Text;
            lblFromDate.Text = fromdate.ToString("dd/MM/yyyy");
            lbltodate.Text = todate.ToString("dd/MM/yyyy");
            if (ddlType.SelectedValue == "All")
            {
                cmd = new MySqlCommand("SELECT vehicel_master.registration_no AS VehicleNo,  minimasters_1.mm_name AS ToolName, employdata.employname AS DriverName FROM vehicletools_issue_return INNER JOIN minimasters ON vehicletools_issue_return.make = minimasters.sno INNER JOIN minimasters minimasters_1 ON vehicletools_issue_return.tools = minimasters_1.sno INNER JOIN employdata ON vehicletools_issue_return.driverid = employdata.emp_sno INNER JOIN vehicel_master ON vehicletools_issue_return.vehiclesno = vehicel_master.vm_sno WHERE  (vehicletools_issue_return.doe BETWEEN @d1 AND @d2)  AND (vehicletools_issue_return.type = @Issuetype)");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@Issuetype", ddlissuereturntype.SelectedValue);
                DataTable dtExp = vdm.SelectQuery(cmd).Tables[0];
                if (dtExp.Rows.Count > 0)
                {
                    hidepanel.Visible = true;
                    grdReports.DataSource = dtExp;
                    grdReports.DataBind();
                    Session["xportdata"] = dtExp;
                }
                else
                {
                    hidepanel.Visible = false;
                    lblmsg.Text = "No data were found";
                }
            }
            if (ddlType.SelectedValue == "Vehicle Wise")
            {
                lblVehicleNo.Text = ddlvehicles.SelectedItem.Text;
                cmd = new MySqlCommand("SELECT vehicel_master.registration_no AS VehicleNo, minimasters_1.mm_name AS ToolName, employdata.employname AS DriverName FROM vehicletools_issue_return INNER JOIN minimasters ON vehicletools_issue_return.make = minimasters.sno INNER JOIN minimasters minimasters_1 ON vehicletools_issue_return.tools = minimasters_1.sno INNER JOIN employdata ON vehicletools_issue_return.driverid = employdata.emp_sno INNER JOIN vehicel_master ON vehicletools_issue_return.vehiclesno = vehicel_master.vm_sno WHERE  (vehicletools_issue_return.doe BETWEEN @d1 AND @d2) AND (vehicletools_issue_return.vehiclesno = @VehicleNo) AND (vehicletools_issue_return.type = @Issuetype)");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate)); 
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@Issuetype", ddlissuereturntype.SelectedValue);
                cmd.Parameters.Add("@VehicleNo", ddlvehicles.SelectedValue);
                DataTable dtExp = vdm.SelectQuery(cmd).Tables[0];
                if (dtExp.Rows.Count > 0)
                {
                    hidepanel.Visible = true;
                    grdReports.DataSource = dtExp;
                    grdReports.DataBind();
                    Session["xportdata"] = dtExp;
                }
                else
                {
                    hidepanel.Visible = false;
                    lblmsg.Text = "No data were found";
                }
            }
        }
        catch (Exception ex)
        {
            hidepanel.Visible = false;
            lblmsg.Text = ex.Message;
        }
    }
}