using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class SearchTripsheet : System.Web.UI.Page
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
            trips = new DataTable();
            if (txtTripRefNo.Text != "")
            {
                cmd = new MySqlCommand("SELECT tripdata.sno as RefNo,tripdata.tripsheetno, DATE_FORMAT(tripdata.tripdate,'%d/%m/%Y %h:%i %p') AS StartDate,DATE_FORMAT(tripdata.enddate,'%d/%m/%Y %h:%i %p') AS EndDate, vehicel_master.registration_no AS VehicleNo, (tripdata.endodometerreading - tripdata.vehiclestartreading) AS TripKMS, tripdata.gpskms, tripdata.endodometerreading - tripdata.vehiclestartreading - tripdata.gpskms AS Dif,  tripdata.endfuelvalue  AS Diesel,ROUND((tripdata.endodometerreading - tripdata.vehiclestartreading)/( tripdata.endfuelvalue),2) as Mileage, tripdata.loadtype, tripdata.qty,tripdata.tripexpences AS TripExpences, tripdata.routeid as RouteName, employdata.employname AS DriverName FROM tripdata INNER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno LEFT OUTER JOIN employdata ON tripdata.driverid = employdata.emp_sno WHERE (tripdata.sno=@TripRefNo) AND (tripdata.userid = @BranchID)");
                cmd.Parameters.Add("@BranchID", BranchID);
                cmd.Parameters.Add("@TripRefNo", txtTripRefNo.Text);
                trips = vdm.SelectQuery(cmd).Tables[0];
                if (trips.Rows.Count > 0)
                {
                    Session["filename"] = "TripsheetReport";
                    Session["xportdata"] = trips;
                    dataGridView1.DataSource = trips;
                    dataGridView1.DataBind();
                }
                else
                {
                    lblmsg.Text = "Tripsheet not found";
                    dataGridView1.DataSource = trips;
                    dataGridView1.DataBind();
                }
            }
            else
            {
                lblmsg.Text = "Pease enter tripsheetno";
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            dataGridView1.DataSource = trips;
            dataGridView1.DataBind();
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