using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class TripsheetReport : System.Web.UI.Page
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
                    dtp_FromDate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                    dtp_Todate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
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
    DataTable trips = new DataTable();
    protected void btn_Generate_Click(object sender, EventArgs e)
    {
        try
        {
            lblmsg.Text = "";
            hidePanel.Visible = true;
            DateTime fromdate = DateTime.Now;
            DateTime todate = DateTime.Now;
            string[] datestrig = dtp_FromDate.Text.Split(' ');
            if (datestrig.Length > 1)
            {
                if (datestrig[0].Split('-').Length > 0)
                {
                    string[] dates = datestrig[0].Split('-');
                    string[] times = datestrig[1].Split(':');
                    fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            datestrig = dtp_Todate.Text.Split(' ');
            if (datestrig.Length > 1)
            {
                if (datestrig[0].Split('-').Length > 0)
                {
                    string[] dates = datestrig[0].Split('-');
                    string[] times = datestrig[1].Split(':');
                    todate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            trips = new DataTable();
            if (ddlStatus.SelectedItem.Text == "All")
            {
                cmd = new MySqlCommand("SELECT tripdata.sno as RefNo, tripdata.tripsheetno, DATE_FORMAT(tripdata.tripdate,'%m/%d/%Y %h:%i %p') AS StartDate,DATE_FORMAT(tripdata.enddate,'%m/%d/%Y %h:%i %p') AS EndDate,vehicel_master.vm_owner as Owner, vehicel_master.registration_no AS VehicleNo, (tripdata.endodometerreading - tripdata.vehiclestartreading) AS TripKMS, tripdata.gpskms,ROUND(tripdata.endodometerreading - tripdata.vehiclestartreading - tripdata.gpskms,2) AS Dif, tripdata.endfuelvalue  AS Diesel,ROUND((tripdata.endodometerreading - tripdata.vehiclestartreading)/( tripdata.endfuelvalue),2) as Mileage, tripdata.loadtype, tripdata.qty,tripdata.tripexpences AS TripExpences, tripdata.routeid as RouteName, employdata.employname AS DriverName FROM tripdata INNER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno LEFT OUTER JOIN employdata ON tripdata.driverid = employdata.emp_sno WHERE (tripdata.tripdate BETWEEN @d1 AND @d2) AND (tripdata.userid = @BranchID)");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@BranchID", BranchID);
                trips = vdm.SelectQuery(cmd).Tables[0];
            }
            else if (ddlStatus.SelectedItem.Text == "Vehicles")
            {
                cmd = new MySqlCommand("SELECT tripdata.sno as RefNo,tripdata.tripsheetno, DATE_FORMAT(tripdata.tripdate,'%d/%m/%Y %h:%i %p') AS StartDate,DATE_FORMAT(tripdata.enddate,'%d/%m/%Y %h:%i %p') AS EndDate,vehicel_master.vm_owner as Owner, vehicel_master.registration_no AS VehicleNo, (tripdata.endodometerreading - tripdata.vehiclestartreading) AS TripKMS, tripdata.gpskms, ROUND(tripdata.endodometerreading - tripdata.vehiclestartreading - tripdata.gpskms,2) AS Dif,  tripdata.endfuelvalue AS Diesel,ROUND((tripdata.endodometerreading - tripdata.vehiclestartreading)/( tripdata.endfuelvalue),2) as Mileage, tripdata.loadtype, tripdata.qty,tripdata.tripexpences AS TripExpences, tripdata.routeid as RouteName, employdata.employname AS DriverName FROM tripdata INNER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno LEFT OUTER JOIN employdata ON tripdata.driverid = employdata.emp_sno WHERE (tripdata.tripdate BETWEEN @d1 AND @d2) AND (tripdata.userid = @BranchID) and (tripdata.vehicleno=@vehicleno)");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@BranchID", BranchID);
                cmd.Parameters.Add("@vehicleno", ddlvehicles.SelectedValue);
                trips = vdm.SelectQuery(cmd).Tables[0];
            }
            else if (ddlStatus.SelectedItem.Text == "Drivers")
            {
                cmd = new MySqlCommand("SELECT tripdata.sno as RefNo,tripdata.tripsheetno, DATE_FORMAT(tripdata.tripdate,'%d/%m/%Y %h:%i %p') AS StartDate,DATE_FORMAT(tripdata.enddate,'%d/%m/%Y %h:%i %p') AS EndDate,vehicel_master.vm_owner as Owner, vehicel_master.registration_no AS VehicleNo, (tripdata.endodometerreading - tripdata.vehiclestartreading) AS TripKMS, tripdata.gpskms, tripdata.endodometerreading - tripdata.vehiclestartreading - tripdata.gpskms AS Dif,  tripdata.endfuelvalue  AS Diesel,ROUND((tripdata.endodometerreading - tripdata.vehiclestartreading)/(tripdata.endfuelvalue),2) as Mileage, tripdata.loadtype, tripdata.qty,tripdata.tripexpences AS TripExpences, tripdata.routeid as RouteName, employdata.employname AS DriverName FROM tripdata INNER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno LEFT OUTER JOIN employdata ON tripdata.driverid = employdata.emp_sno WHERE (tripdata.tripdate BETWEEN @d1 AND @d2) AND (tripdata.userid = @BranchID) and (tripdata.DriverID=@DriverID)");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@BranchID", BranchID);
                cmd.Parameters.Add("@DriverID", ddlvehicles.SelectedValue);
                trips = vdm.SelectQuery(cmd).Tables[0];
            }
            if (trips.Rows.Count > 0)
            {
                string title = "Tripsheet Report From: " + fromdate.ToString() + "  To: " + todate.ToString();
                Session["title"] = title;
                Session["filename"] = "TripsheetReport";
                Session["xportdata"] = trips;
                dataGridView1.DataSource = trips;
                dataGridView1.DataBind();
            }
            else
            {
                lblmsg.Text = "No data found";
                dataGridView1.DataSource = trips;
                dataGridView1.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            dataGridView1.DataSource = trips;
            dataGridView1.DataBind();
        }
    }
    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        vdm = new VehicleDBMgr();
        if (ddlStatus.SelectedItem.Text == "All")
        {
            Pnlhide.Visible = false ;
        }
        if (ddlStatus.SelectedItem.Text == "Vehicles")
        {
            Pnlhide.Visible = true;
            cmd = new MySqlCommand("SELECT vm_sno AS VehicleSno, registration_no AS VehicleNumber FROM vehicel_master WHERE (branch_id = @BranchID) GROUP BY registration_no");
            cmd.Parameters.Add("@BranchID", BranchID);
            DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
            ddlvehicles.DataSource = dttrips;
            ddlvehicles.DataTextField = "VehicleNumber";
            ddlvehicles.DataValueField = "VehicleSno";
            ddlvehicles.DataBind();
        }
        else if (ddlStatus.SelectedItem.Text == "Drivers")
        {
            Pnlhide.Visible = true;
            cmd = new MySqlCommand("SELECT emp_sno as Sno, employname FROM employdata WHERE (emp_type = @EmployeeType) AND (branch_id = @BranchID)");
            cmd.Parameters.Add("@BranchID", BranchID);
            cmd.Parameters.Add("@EmployeeType", "Driver");
            DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
            ddlvehicles.DataSource = dttrips;
            ddlvehicles.DataTextField = "employname";
            ddlvehicles.DataValueField = "Sno";
            ddlvehicles.DataBind();
        }
    }
    protected void grdReports_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = Convert.ToInt32(e.CommandArgument);
        GridViewRow row = dataGridView1.Rows[rowIndex];
        string ReceiptNo = row.Cells[1].Text;
        Session["TripSheetNo"] = ReceiptNo;
        Response.Redirect("ViewTripSheet.aspx", false);
    }
}