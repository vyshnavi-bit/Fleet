using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class PrintTripSheet : System.Web.UI.Page
{
    MySqlCommand cmd;
    string BranchID = "";
    DataTable dtAddress = new DataTable();
    VehicleDBMgr vdm;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Branch_ID"] == null)
            Response.Redirect("Login.aspx");
        else
        {
            BranchID = Session["Branch_ID"].ToString();
            if (!this.IsPostBack)
            {
                if (!Page.IsCallback)
                {
                    if (Session["TripSheetNo"] == null)
                    {
                    }
                    else
                    {
                        txtTripRefNo.Text = Session["TripSheetNo"].ToString();
                    }
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
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlStatus.SelectedValue == "With Logo")
            {
                hidePanel.Visible = true;
            }
            else
            {
                hidePanel.Visible = false;
            }
            vdm = new VehicleDBMgr();
            lblmsg.Text = "";
            cmd = new MySqlCommand("SELECT employdata.employname, tripdata.tripsheetno, tripdata.tripdate, employdata.Phoneno AS phoneNumber, tripdata.sno, tripdata.loadtype, employdata.emp_licencenum AS LicenseNo, tripdata.routeid AS RouteName, vehicel_master.registration_no AS Vehicleno, vehicel_master.vm_model AS VehicleModel, minimasters.mm_name AS VehicleType, minimasters_1.mm_name AS VehicleMake FROM tripdata INNER JOIN employdata ON tripdata.driverid = employdata.emp_sno INNER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno INNER JOIN minimasters ON vehicel_master.vhtype_refno = minimasters.sno INNER JOIN minimasters minimasters_1 ON vehicel_master.vhmake_refno = minimasters_1.sno WHERE (tripdata.sno = @tripID) and (tripdata.userid=@BranchID)");
            cmd.Parameters.Add("@TripID", txtTripRefNo.Text);
            cmd.Parameters.Add("@BranchID", BranchID);
            DataTable dtTripSheet = vdm.SelectQuery(cmd).Tables[0];
            if (dtTripSheet.Rows.Count > 0)
            {
                lblTripRefNo.Text = txtTripRefNo.Text;
                lblTripsheetNo.Text = dtTripSheet.Rows[0]["tripsheetno"].ToString();
                string TripTime = dtTripSheet.Rows[0]["tripdate"].ToString();
                DateTime dtPlantime = Convert.ToDateTime(TripTime);
                string time = dtPlantime.ToString("dd/MMM/yyyy");
                string strPlantime = dtPlantime.ToString();
                string[] DateTime = strPlantime.Split(' ');
                string[] PlanDateTime = strPlantime.Split(' ');
                lblDate.Text = time;
                lblTime.Text = PlanDateTime[1] + " " + PlanDateTime[2];
                lblVehicleNo.Text = dtTripSheet.Rows[0]["Vehicleno"].ToString();
                lblMake.Text = dtTripSheet.Rows[0]["VehicleMake"].ToString();
                //lblModel.Text = dtTripSheet.Rows[0]["VehicleModel"].ToString();
                lblVehicleType.Text = dtTripSheet.Rows[0]["VehicleType"].ToString();

                lblPhoneNo.Text = dtTripSheet.Rows[0]["phoneNumber"].ToString();
                lblDriverName.Text = dtTripSheet.Rows[0]["employname"].ToString();
                lblLicenceNo.Text = dtTripSheet.Rows[0]["LicenseNo"].ToString();
                lblAssignRoute.Text = dtTripSheet.Rows[0]["RouteName"].ToString();
                lblTypeOfLoad.Text = dtTripSheet.Rows[0]["loadtype"].ToString();
                BindEmpty();
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }


    }
    void BindEmpty()
    {
        DataTable EmptyReport = new DataTable();
        EmptyReport.Columns.Add("Sno");
        EmptyReport.Columns.Add("Date");
        EmptyReport.Columns.Add("Time");
        EmptyReport.Columns.Add("Km's");
        EmptyReport.Columns.Add("Place");
        EmptyReport.Columns.Add("Details");
        EmptyReport.Columns.Add("Amount");
        EmptyReport.Columns.Add("Qty");
        EmptyReport.Columns.Add("Diesel Filled");
        int i = 10;
        if (ddlStatus.SelectedValue == "With Logo")
        {
            for (i = 1; i < 11; i++)
            {
                DataRow newr = EmptyReport.NewRow();
                newr["Sno"] = i.ToString();
                EmptyReport.Rows.Add(newr);
            }
        }
        else
        {
            for (i = 1; i < 5; i++)
            {
                DataRow newr = EmptyReport.NewRow();
                newr["Sno"] = i.ToString();
                EmptyReport.Rows.Add(newr);
            }
        }
        grdReports.DataSource = EmptyReport;
        grdReports.DataBind();
    }
}