using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class Pending_tripsheet_Report : System.Web.UI.Page
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
                    lblAddress.Text = Session["Address"].ToString();
                    lblTitle.Text = Session["TitleName"].ToString();
                    GetPending_trips();
                }
            }
        }
    }
    void GetPending_trips()
    {
        try
        {
            vdm = new VehicleDBMgr();
            string UserID = Session["Branch_ID"].ToString();
            cmd = new MySqlCommand("SELECT DATE_FORMAT(tripdata.tripdate,'%d/%m/%Y') AS TripDate,vehicel_master.registration_no AS vehicleNo, minimasters.mm_name AS VehicleType, minimasters_1.mm_name AS Make, vehicel_master.Capacity, employdata.employname as Driver, tripdata.routeid as Route, tripdata.tripsheetno as TripSheetNo,  tripdata.loadtype as LoadType, employdata.Phoneno FROM tripdata INNER JOIN employdata ON tripdata.driverid = employdata.emp_sno INNER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno INNER JOIN minimasters ON vehicel_master.vhtype_refno = minimasters.sno INNER JOIN minimasters minimasters_1 ON vehicel_master.vhmake_refno = minimasters_1.sno WHERE (tripdata.status = @Status) AND (tripdata.userid = @UserID) ORDER BY tripdata.sno");
            cmd.Parameters.Add("@Status", "A");
            cmd.Parameters.Add("@UserID", UserID);
            DataTable dtPending_trips = vdm.SelectQuery(cmd).Tables[0];
            if (dtPending_trips.Rows.Count > 0)
            {
                hidePanel.Visible = true;
                string title = "Pending Tripsheet Report ";
                Session["title"] = title;
                Session["filename"] = "Pending Trips";
                Session["xportdata"] = dtPending_trips;
                grdReports.DataSource = dtPending_trips;
                grdReports.DataBind();
            }
            else
            {
                hidePanel.Visible = false;
                lblmsg.Text = "No data found";
            }
        }
        catch(Exception ex)
        {
            hidePanel.Visible = false;
            lblmsg.Text = ex.Message;
        }
    }
}