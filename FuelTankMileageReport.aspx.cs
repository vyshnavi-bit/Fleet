using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class FuelTankMileageReport : System.Web.UI.Page
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
    DataTable Report = new DataTable();
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
            lblfromdate.Text = fromdate.ToString("dd/MM/yyyy");
            lbltodate.Text = todate.ToString("dd/MM/yyyy");
            trips = new DataTable();
            Report.Columns.Add("Sno");
            Report.Columns.Add("TripsheetNo");
            Report.Columns.Add("StartDate");
            Report.Columns.Add("EndDate");
            Report.Columns.Add("VehicleNo");
            Report.Columns.Add("Start Odometer");
            Report.Columns.Add("End Odometer");
            Report.Columns.Add("Total").DataType = typeof(Double);
            Report.Columns.Add("LoadType");
            Report.Columns.Add("RouteName");
            Report.Columns.Add("InsideFuel").DataType = typeof(Double);
            Report.Columns.Add("OutsideFuel").DataType = typeof(Double);
            Report.Columns.Add("Qty").DataType = typeof(Double);
            Report.Columns.Add("TodayMileage");
            if (ddlType.SelectedItem.Text == "All Puff" || ddlType.SelectedItem.Text == "All Tanker")
            {
                cmd = new MySqlCommand("SELECT derivedtbl_1.tripsheetno AS TripSheet,DATE_FORMAT( derivedtbl_1.tripdate,'%d-%m-%Y %h:%i %p') AS StartDate,DATE_FORMAT( derivedtbl_1.enddate,'%d-%m-%Y %h:%i %p') AS EndDate,derivedtbl_1.gpskms, derivedtbl_1.registration_no AS VehicleNo,derivedtbl_1.VehicleType,derivedtbl_1.Make,derivedtbl_1.Capacity,derivedtbl_1.TripKMS, derivedtbl_1.loadtype AS LoadType, derivedtbl_1.routeid AS RouteName,derivedtbl_1.vehiclestartreading as StartOdometer ,derivedtbl_1.endodometerreading as EndOdometer,derivedtbl_1.refrigeration_fuel AS InsideFuel, SUM(triplogs.fuel) AS OutsideFuel,derivedtbl_1.Qty,IFNULL(ROUND(derivedtbl_1.TripKMS / (derivedtbl_1.refrigeration_fuel)),0) AS TodayMileage FROM (SELECT        tripdata.tripsheetno, tripdata.tripdate, tripdata.enddate, vehicel_master.registration_no,tripdata.endodometerreading, tripdata.vehiclestartreading,tripdata.endodometerreading -tripdata.vehiclestartreading AS TripKMS,tripdata.gpskms, tripdata.loadtype, tripdata.qty, tripdata.routeid,vehicel_master.Capacity, tripdata.sno, tripdata.refrigeration_fuel,tripdata.endfuelvalue, minimasters.mm_name AS VehicleType, minimasters_1.mm_code AS Make FROM tripdata INNER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno INNER JOIN minimasters ON vehicel_master.vhtype_refno = minimasters.sno INNER JOIN  minimasters minimasters_1 ON vehicel_master.vhmake_refno = minimasters_1.sno WHERE (tripdata.enddate BETWEEN @d1 AND @d2) AND (tripdata.userid = @BranchID) AND (minimasters.mm_name = @VehType) AND (tripdata.status = 'C')) derivedtbl_1 INNER JOIN triplogs ON derivedtbl_1.sno = triplogs.tripsno AND triplogs.fuel_type <> 'OWN' GROUP BY derivedtbl_1.tripsheetno order by Make,VehicleNo, StartDate");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@BranchID", BranchID);
                if (ddlType.SelectedItem.Text == "All Puff")
                {
                    cmd.Parameters.Add("@VehType", "Puff");
                }
                if (ddlType.SelectedItem.Text == "All Tanker")
                {
                    cmd.Parameters.Add("@VehType", "Tanker");
                }
                trips = vdm.SelectQuery(cmd).Tables[0];
            }
            if (ddlType.SelectedItem.Text == "ALL")
            {
                cmd = new MySqlCommand("SELECT derivedtbl_1.tripsheetno AS TripSheet,DATE_FORMAT( derivedtbl_1.tripdate,'%d-%m-%Y %h:%i %p') AS StartDate,DATE_FORMAT( derivedtbl_1.enddate,'%d-%m-%Y %h:%i %p') AS EndDate,derivedtbl_1.gpskms, derivedtbl_1.registration_no AS VehicleNo,derivedtbl_1.VehicleType,derivedtbl_1.Make,derivedtbl_1.Capacity,derivedtbl_1.loadtype AS LoadType, derivedtbl_1.routeid AS RouteName,derivedtbl_1.vehiclestartreading as StartOdometer, derivedtbl_1.endodometerreading as EndOdometer,derivedtbl_1.refrigeration_fuel AS InsideFuel, SUM(triplogs.fuel) AS OutsideFuel,derivedtbl_1.Qty, IFNULL(ROUND(derivedtbl_1.TripKMS / (derivedtbl_1.refrigeration_fuel)),0) AS TodayMileage FROM (SELECT        tripdata.tripsheetno, tripdata.tripdate, tripdata.enddate, vehicel_master.registration_no, tripdata.endodometerreading ,tripdata.vehiclestartreading ,tripdata.endodometerreading -tripdata.vehiclestartreading AS TripKMS,tripdata.gpskms, tripdata.loadtype, tripdata.qty, tripdata.routeid,vehicel_master.Capacity, tripdata.sno, tripdata.endfuelvalue, tripdata.refrigeration_fuel,minimasters.mm_name AS VehicleType, minimasters_1.mm_code AS Make FROM tripdata INNER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno INNER JOIN minimasters ON vehicel_master.vhtype_refno = minimasters.sno INNER JOIN  minimasters minimasters_1 ON vehicel_master.vhmake_refno = minimasters_1.sno WHERE (tripdata.enddate BETWEEN @d1 AND @d2) AND (tripdata.userid = @BranchID) AND (tripdata.status = 'C')) derivedtbl_1 INNER JOIN triplogs ON derivedtbl_1.sno = triplogs.tripsno AND triplogs.fuel_type <> 'OWN' GROUP BY derivedtbl_1.tripsheetno order by VehicleType,Make,Capacity,VehicleNo, StartDate");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@BranchID", BranchID);
                trips = vdm.SelectQuery(cmd).Tables[0];
            }
            if (ddlType.SelectedItem.Text == "Puffs" || ddlType.SelectedItem.Text == "Tankers")
            {
                lblName.Text = ddlvehicles.SelectedItem.Text;
                cmd = new MySqlCommand("SELECT derivedtbl_1.tripsheetno AS TripSheet,DATE_FORMAT( derivedtbl_1.tripdate,'%d-%m-%Y %h:%i %p') AS StartDate,DATE_FORMAT( derivedtbl_1.enddate,'%d-%m-%Y %h:%i %p') AS EndDate,derivedtbl_1.gpskms, derivedtbl_1.registration_no AS VehicleNo,derivedtbl_1.VehicleType,derivedtbl_1.Make,derivedtbl_1.Capacity,derivedtbl_1.loadtype AS LoadType, derivedtbl_1.routeid AS RouteName,derivedtbl_1.vehiclestartreading as StartOdometer,derivedtbl_1.endodometerreading as EndOdometer, derivedtbl_1.refrigeration_fuel AS InsideFuel, SUM(triplogs.fuel) AS OutsideFuel,derivedtbl_1.Qty, IFNULL(ROUND(derivedtbl_1.TripKMS / (derivedtbl_1.refrigeration_fuel)),0) AS TodayMileage FROM (SELECT        tripdata.tripsheetno, tripdata.tripdate, tripdata.enddate, vehicel_master.registration_no, tripdata.endodometerreading,tripdata.vehiclestartreading ,tripdata.endodometerreading -tripdata.vehiclestartreading AS TripKMS,tripdata.gpskms, tripdata.loadtype, tripdata.qty, tripdata.routeid,vehicel_master.Capacity, tripdata.sno, tripdata.endfuelvalue, tripdata.refrigeration_fuel,minimasters.mm_name AS VehicleType, minimasters_1.mm_code AS Make FROM tripdata INNER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno INNER JOIN minimasters ON vehicel_master.vhtype_refno = minimasters.sno INNER JOIN  minimasters minimasters_1 ON vehicel_master.vhmake_refno = minimasters_1.sno WHERE (tripdata.enddate BETWEEN @d1 AND @d2) AND (tripdata.userid = @BranchID) AND (tripdata.vehicleno=@VehicleNo) AND (tripdata.status = 'C')) derivedtbl_1 INNER JOIN triplogs ON derivedtbl_1.sno = triplogs.tripsno AND triplogs.fuel_type <> 'OWN' GROUP BY derivedtbl_1.tripsheetno order by Make,VehicleNo, StartDate");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@BranchID", BranchID);
                cmd.Parameters.Add("@VehicleNo", ddlvehicles.SelectedValue);
                trips = vdm.SelectQuery(cmd).Tables[0];
            }
            if (ddlType.SelectedValue == "Driver Wise")
            {
                lblName.Text = ddlvehicles.SelectedItem.Text;
                cmd = new MySqlCommand("SELECT derivedtbl_1.tripsheetno AS TripSheet,DATE_FORMAT( derivedtbl_1.tripdate,'%d-%m-%Y %h:%i %p') AS StartDate,DATE_FORMAT( derivedtbl_1.enddate,'%d-%m-%Y %h:%i %p') AS EndDate,derivedtbl_1.gpskms, derivedtbl_1.registration_no AS VehicleNo,derivedtbl_1.VehicleType,derivedtbl_1.Make,derivedtbl_1.Capacity,derivedtbl_1.loadtype AS LoadType, derivedtbl_1.routeid AS RouteName,derivedtbl_1.vehiclestartreading as StartOdometer,derivedtbl_1.endodometerreading as EndOdometer, derivedtbl_1.refrigeration_fuel AS InsideFuel, SUM(triplogs.fuel) AS OutsideFuel,derivedtbl_1.Qty, IFNULL(ROUND(derivedtbl_1.TripKMS / (derivedtbl_1.refrigeration_fuel)),0) AS TodayMileage FROM (SELECT        tripdata.tripsheetno, tripdata.tripdate, tripdata.enddate, vehicel_master.registration_no, tripdata.endodometerreading ,tripdata.vehiclestartreading,tripdata.gpskms,tripdata.endodometerreading -tripdata.vehiclestartreading AS TripKMS, tripdata.loadtype, tripdata.qty, tripdata.routeid,vehicel_master.Capacity, tripdata.sno, tripdata.endfuelvalue, tripdata.refrigeration_fuel,minimasters.mm_name AS VehicleType, minimasters_1.mm_code AS Make FROM tripdata INNER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno INNER JOIN minimasters ON vehicel_master.vhtype_refno = minimasters.sno INNER JOIN  minimasters minimasters_1 ON vehicel_master.vhmake_refno = minimasters_1.sno WHERE (tripdata.enddate BETWEEN @d1 AND @d2) AND (tripdata.userid = @BranchID) AND (tripdata.driverid=@driverid) AND (tripdata.status = 'C')) derivedtbl_1 INNER JOIN triplogs ON derivedtbl_1.sno = triplogs.tripsno AND triplogs.fuel_type <> 'OWN' GROUP BY derivedtbl_1.tripsheetno order by Make,VehicleNo, StartDate");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@BranchID", BranchID);
                cmd.Parameters.Add("@driverid", ddlvehicles.SelectedValue);
                trips = vdm.SelectQuery(cmd).Tables[0];
            }
            if (trips.Rows.Count > 0)
            {
                int i = 1;
                foreach (DataRow dr in trips.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["SNo"] = i++.ToString();
                    newrow["TripsheetNo"] = dr["TripSheet"].ToString();
                    newrow["StartDate"] = dr["StartDate"].ToString();
                    newrow["EndDate"] = dr["EndDate"].ToString();
                    newrow["VehicleNo"] = dr["VehicleNo"].ToString() + " /" + dr["VehicleType"].ToString() + " /" + dr["Make"].ToString() + " /" + dr["Capacity"].ToString();
                    newrow["LoadType"] = dr["LoadType"].ToString();
                    newrow["RouteName"] = dr["RouteName"].ToString();
                    newrow["Start Odometer"] = dr["StartOdometer"].ToString();
                    newrow["End Odometer"] = dr["EndOdometer"].ToString();
                    int startreading = 0;
                    int.TryParse(dr["StartOdometer"].ToString(), out startreading);
                    int endingreading = 0;
                    int.TryParse(dr["EndOdometer"].ToString(), out endingreading);
                    int diff = endingreading - startreading;
                    newrow["Total"] = diff.ToString("F2");
                    newrow["InsideFuel"] = dr["InsideFuel"].ToString();
                    newrow["OutsideFuel"] = dr["OutsideFuel"].ToString();
                    newrow["Qty"] = dr["Qty"].ToString();
                    newrow["TodayMileage"] = dr["TodayMileage"].ToString();
                    Report.Rows.Add(newrow);
                }
                string title = "MileageReport Report From: " + fromdate.ToString() + "  To: " + todate.ToString();
                Session["title"] = title;
                Session["filename"] = "MileageReport";
                Session["xportdata"] = Report;
                dataGridView1.DataSource = Report;
                dataGridView1.DataBind();
            }
            else
            {
                lblmsg.Text = "No data found";
                dataGridView1.DataSource = Report;
                dataGridView1.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            dataGridView1.DataSource = Report;
            dataGridView1.DataBind();
        }
    }
    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        vdm = new VehicleDBMgr();

        if (ddlType.SelectedValue == "Puffs")
        {
            hideVehicles.Visible = true;
            cmd = new MySqlCommand("SELECT minimasters.mm_name, vehicel_master.registration_no, vehicel_master.vm_sno FROM vehicel_master INNER JOIN minimasters ON vehicel_master.vhtype_refno = minimasters.sno WHERE(minimasters.mm_name = @Tanker) AND (vehicel_master.branch_id = @BranchID)");
            cmd.Parameters.Add("@BranchID", BranchID);
            cmd.Parameters.Add("@Tanker", "Puff");
            DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
            ddlvehicles.DataSource = dttrips;
            ddlvehicles.DataTextField = "registration_no";
            ddlvehicles.DataValueField = "vm_sno";
            ddlvehicles.DataBind();
        }
        if (ddlType.SelectedValue == "Tankers")
        {
            hideVehicles.Visible = true;
            cmd = new MySqlCommand("SELECT minimasters.mm_name, vehicel_master.registration_no, vehicel_master.vm_sno FROM vehicel_master INNER JOIN minimasters ON vehicel_master.vhtype_refno = minimasters.sno WHERE(minimasters.mm_name = @Tanker) AND (vehicel_master.branch_id = @BranchID)");
            cmd.Parameters.Add("@BranchID", BranchID);
            cmd.Parameters.Add("@Tanker", "Tanker");
            DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
            ddlvehicles.DataSource = dttrips;
            ddlvehicles.DataTextField = "registration_no";
            ddlvehicles.DataValueField = "vm_sno";
            ddlvehicles.DataBind();
        }
        if (ddlType.SelectedValue == "Driver Wise")
        {
            hideVehicles.Visible = true;
            cmd = new MySqlCommand("SELECT emp_sno, employid, employname FROM  employdata WHERE (branch_id = @BranchID) AND (flag <> 0) AND (emp_type='Driver')");
            cmd.Parameters.Add("@BranchID", BranchID);
            DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
            ddlvehicles.DataSource = dttrips;
            ddlvehicles.DataTextField = "employname";
            ddlvehicles.DataValueField = "emp_sno";
            ddlvehicles.DataBind();
        }
        if (ddlType.SelectedValue == "ALL")
        {
            hideVehicles.Visible = false;
            ddlvehicles.Items.Clear();
        }
        if (ddlType.SelectedValue == "All Puff")
        {
            hideVehicles.Visible = false;
            ddlvehicles.Items.Clear();
        }
        if (ddlType.SelectedValue == "All Tanker")
        {
            hideVehicles.Visible = false;
            ddlvehicles.Items.Clear();
        }
    }
}