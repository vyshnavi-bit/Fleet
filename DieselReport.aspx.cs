using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MySql.Data.MySqlClient;

public partial class DieselReport : System.Web.UI.Page
{
    MySqlCommand cmd;
    DataTable dtAddress = new DataTable();
    VehicleDBMgr vdm;
    string BranchID;
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
                    txtFromdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                    txtTodate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
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
            lblmsg.Text = "";
            vdm = new VehicleDBMgr();
            pvisible.Visible = true;
            
            DateTime fromdate = DateTime.Now;
            string[] dateFromstrig = txtFromdate.Text.Split(' ');
            if (dateFromstrig.Length > 1)
            {
                if (dateFromstrig[0].Split('-').Length > 0)
                {
                    string[] dates = dateFromstrig[0].Split('-');
                    string[] times = dateFromstrig[1].Split(':');
                    fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            DateTime Todate = DateTime.Now;
            string[] dateTostrig = txtTodate.Text.Split(' ');
            if (dateTostrig.Length > 1)
            {
                if (dateTostrig[0].Split('-').Length > 0)
                {
                    string[] dates = dateTostrig[0].Split('-');
                    string[] times = dateTostrig[1].Split(':');
                    Todate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            if (ddlReportType.SelectedValue == "Daily Diesel")
            {
                lblOppBal.Text = "";
                lblCloBal.Text = "";
                Session["title"] = "Diesel Report";
                DataTable Report = new DataTable();
                Report.Columns.Add("Sno");
                Report.Columns.Add("Date");
                Report.Columns.Add("Time");
                Report.Columns.Add("Route");
                Report.Columns.Add("Vehicleno");
                Report.Columns.Add("VehicleType");
                Report.Columns.Add("Cost Per Ltr");
                Report.Columns.Add("Diesel Filled").DataType = typeof(Double);
                Report.Columns.Add("Diesel Value").DataType = typeof(Double);
                Report.Columns.Add("Driver Name");
                Report.Columns.Add("Load Type");
                Report.Columns.Add("Start Reading");
                Report.Columns.Add("Pump Reading");
                Report.Columns.Add("Token");
                cmd = new MySqlCommand("SELECT sno, fuel, doe, userid, operetedby, costperltr FROM fuel_transaction WHERE (doe BETWEEN @d1 AND @d2) AND (transtype = 2)");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate).AddDays(-1));
                cmd.Parameters.Add("@d2", GetHighDate(Todate).AddDays(-1));
                DataTable dtOpp = vdm.SelectQuery(cmd).Tables[0];
                if (dtOpp.Rows.Count > 0)
                {
                    pnlOpp.Visible = true;
                    lblOppBal.Text = dtOpp.Rows[0]["fuel"].ToString();
                }
                cmd = new MySqlCommand("SELECT sno, fuel, doe, userid, operetedby, costperltr FROM fuel_transaction WHERE (doe BETWEEN @d1 AND @d2) AND (transtype = 2)");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(Todate));
                DataTable dtclo = vdm.SelectQuery(cmd).Tables[0];
                if (dtclo.Rows.Count > 0)
                {
                    pnlClo.Visible = true;
                    lblCloBal.Text = dtclo.Rows[0]["fuel"].ToString();
                }
                cmd = new MySqlCommand("SELECT employdata.employname,tripdata.routeid, tripdata.fueltank,tripdata.pumpreading,tripdata.Tokenno, tripdata.vehiclestartreading,tripdata.refrigeration_fuel, tripdata.gpskms, tripdata.tripsheetno, tripdata.enddate, employdata.Phoneno AS phoneNumber, tripdata.sno, tripdata.loadtype, employdata.emp_licencenum AS LicenseNo, tripdata.routeid AS RouteName, vehicel_master.registration_no AS Vehicleno, vehicel_master.vm_model AS VehicleModel, minimasters.mm_name AS VehicleType, minimasters_1.mm_name AS VehicleMake,tripdata.endfuelvalue FROM tripdata INNER JOIN employdata ON tripdata.driverid = employdata.emp_sno INNER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno INNER JOIN minimasters ON vehicel_master.vhtype_refno = minimasters.sno INNER JOIN minimasters minimasters_1 ON vehicel_master.vhmake_refno = minimasters_1.sno WHERE (tripdata.enddate BETWEEN @d1 AND @d2) AND (tripdata.userid = @BranchID) AND (tripdata.status = 'C') order by tripdata.enddate");
                cmd.Parameters.Add("@BranchID", BranchID);
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(Todate));
                DataTable dtDiesel = vdm.SelectQuery(cmd).Tables[0];
                if (dtDiesel.Rows.Count > 0)
                {
                    int i = 1;
                    foreach (DataRow dr in dtDiesel.Rows)
                    {
                        DataRow newrow = Report.NewRow();
                        newrow["Sno"] = i++.ToString();
                        string assigndate = dr["enddate"].ToString();
                        DateTime dtPlantime = Convert.ToDateTime(assigndate);
                        string date = dtPlantime.ToString("dd/MMM/yyyy");
                        string time = dtPlantime.ToString("dd/MMM/yyyy HH:mm");
                        string strPlantime = dtPlantime.ToString();
                        string[] PlanDateTime = time.Split(' ');
                        newrow["Date"] = date;
                        newrow["Time"] = PlanDateTime[1];
                        newrow["Route"] = dr["routeid"].ToString();
                        newrow["Vehicleno"] = dr["Vehicleno"].ToString();
                        newrow["VehicleType"] = dr["VehicleType"].ToString();
                        double Diesel = 0;
                        double.TryParse(dr["endfuelvalue"].ToString(), out Diesel);
                         double refrigeration_fuel = 0;
                        double.TryParse(dr["refrigeration_fuel"].ToString(), out refrigeration_fuel);
                        double total = 0;
                        total = Diesel + refrigeration_fuel;
                        newrow["Diesel Filled"] = total;
                        newrow["Start Reading"] = dr["vehicleStartReading"].ToString();
                        newrow["Pump Reading"] = dr["pumpreading"].ToString();
                        newrow["Token"] = dr["Tokenno"].ToString();
                        newrow["Driver Name"] = dr["employname"].ToString();
                        newrow["Load Type"] = dr["loadtype"].ToString();
                        Report.Rows.Add(newrow);
                    }
                    DataRow New = Report.NewRow();
                    New["Vehicleno"] = "Total";
                    double valnewCash = 0.0;
                    foreach (DataColumn dc in Report.Columns)
                    {
                        if (dc.DataType == typeof(Double))
                        {
                            valnewCash = 0.0;
                            double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out valnewCash);                            
                            New[dc.ToString()] = valnewCash;
                        }
                    }
                    Report.Rows.Add(New);

                    grdReports.DataSource = Report;
                    grdReports.DataBind();
                    string title = "DieselReport From: " + fromdate.ToString() + "  To: " + Todate.ToString();
                    Session["title"] = title;
                    Session["filename"] = "DieselReport";
                    Session["xportdata"] = Report;
                }
                else
                {
                    pvisible.Visible = false;
                    lblmsg.Text = "No data were found";
                }
            }
            else
            {
                pnlOpp.Visible = false;
                pnlClo.Visible = false;
                DataTable Report = new DataTable();
                Report.Columns.Add("Sno");
                Report.Columns.Add("Date");
                Report.Columns.Add("Opp Bal");
                Report.Columns.Add("Received");
                Report.Columns.Add("Diesel Filled");
                Report.Columns.Add("Clo Bal");
                cmd = new MySqlCommand("SELECT sno, transtype, fuel, doe, userid FROM fuel_transaction WHERE (transtype = @transtype) AND (userid = @BranchID) AND (doe BETWEEN @d1 AND @d2)");
                cmd.Parameters.Add("@BranchID", BranchID);
                cmd.Parameters.Add("@transtype", "1");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(Todate));
                DataTable dtInward = vdm.SelectQuery(cmd).Tables[0];

                cmd = new MySqlCommand("SELECT sno, transtype, fuel, doe, userid FROM fuel_transaction WHERE (transtype = @transtype) AND (userid = @BranchID) AND (doe BETWEEN @d1 AND @d2)");
                cmd.Parameters.Add("@BranchID", BranchID);
                cmd.Parameters.Add("@transtype", "2");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(Todate));
                DataTable dtClosingStock = vdm.SelectQuery(cmd).Tables[0];

                cmd = new MySqlCommand("SELECT SUM(endfuelvalue) AS fuel,SUM(refrigeration_fuel) AS refrigeration_fuel, enddate FROM tripdata WHERE (enddate BETWEEN @d1 AND @d2) AND (userid = @BranchID) AND (status = 'C') Group by DATE(enddate) ORDER BY enddate");
                cmd.Parameters.Add("@BranchID", BranchID);
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(Todate));
                DataTable dtOutward = vdm.SelectQuery(cmd).Tables[0];
                //cmd = new MySqlCommand("SELECT sno, transtype, sum(fuel) as fuel, doe, userid FROM fuel_transaction WHERE (transtype = @transtype) AND (userid = @BranchID) AND (doe BETWEEN @d1 AND @d2) Group by DATE(doe),transtype");
                //cmd.Parameters.Add("@BranchID", BranchID);
                //cmd.Parameters.Add("@transtype", "3");
                //cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                //cmd.Parameters.Add("@d2", GetHighDate(Todate));
                //DataTable dtOutward = vdm.SelectQuery(cmd).Tables[0];
                int i = 1;
                foreach (DataRow dr in dtOutward.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["Sno"] = i++.ToString();
                    string assigndate = dr["enddate"].ToString();
                    DateTime dtPlantime = Convert.ToDateTime(assigndate);
                    string date = dtPlantime.ToString("dd/MMM/yyyy");
                    newrow["Date"] = date;
                    double Diesel = 0;
                    double.TryParse(dr["fuel"].ToString(), out Diesel);
                    double refrigeration_fuel = 0;
                    double.TryParse(dr["refrigeration_fuel"].ToString(), out refrigeration_fuel);
                    double total = 0;
                    total = Diesel + refrigeration_fuel;
                    newrow["Diesel Filled"] = total;
                    Report.Rows.Add(newrow);
                }
                foreach (DataRow dr in Report.Rows)
                {
                    foreach (DataRow drC in dtClosingStock.Rows)
                    {
                        string assigndate = drC["doe"].ToString();
                        DateTime dtPlantime = Convert.ToDateTime(assigndate);
                        string date = dtPlantime.ToString("dd/MMM/yyyy");
                        if (dr["Date"].ToString() == date)
                        {
                            dr["Clo Bal"] = drC["fuel"].ToString();
                        }
                    }
                }
                foreach (DataRow dr in Report.Rows)
                {
                    foreach (DataRow drC in dtClosingStock.Rows)
                    {
                        string assigndate = drC["doe"].ToString();
                        DateTime dtPlantime = Convert.ToDateTime(assigndate).AddDays(1);
                        string date = dtPlantime.ToString("dd/MMM/yyyy");
                        if (dr["Date"].ToString() == date)
                        {
                            dr["Opp Bal"] = drC["fuel"].ToString();
                        }
                    }
                }
                foreach (DataRow dr in Report.Rows)
                {
                    foreach (DataRow drI in dtInward.Rows)
                    {
                        string assigndate = drI["doe"].ToString();
                        DateTime dtPlantime = Convert.ToDateTime(assigndate);
                        string date = dtPlantime.ToString("dd/MMM/yyyy");
                        if (dr["Date"].ToString() == date)
                        {
                            dr["Received"] = drI["fuel"].ToString();
                        }
                    }
                }
                grdReports.DataSource = Report;
                grdReports.DataBind();

                string title = "DieselReport From: " + fromdate.ToString() + "  To: " + Todate.ToString();
                Session["title"] = title;
                Session["filename"] = "DieselReport";
                Session["xportdata"] = Report;

            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
}