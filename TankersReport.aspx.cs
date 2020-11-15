using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MySql.Data.MySqlClient;

public partial class TankersReport : System.Web.UI.Page
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
    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        vdm = new VehicleDBMgr();
        if (ddlType.SelectedValue == "All")
        {
            hideVehicles.Visible = false;
            ddlvehicles.Items.Clear();

        }
        if (ddlType.SelectedValue == "Tanker")
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
        if (ddlType.SelectedValue == "Puff")
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
        if (ddlType.SelectedValue == "Truck")
        {
            hideVehicles.Visible = true;
            cmd = new MySqlCommand("SELECT minimasters.mm_name, vehicel_master.registration_no, vehicel_master.vm_sno FROM vehicel_master INNER JOIN minimasters ON vehicel_master.vhtype_refno = minimasters.sno WHERE(minimasters.mm_name = @Tanker) AND (vehicel_master.branch_id = @BranchID)");
            cmd.Parameters.Add("@BranchID", BranchID);
            cmd.Parameters.Add("@Tanker", "Truck");
            DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
            ddlvehicles.DataSource = dttrips;
            ddlvehicles.DataTextField = "registration_no";
            ddlvehicles.DataValueField = "vm_sno";
            ddlvehicles.DataBind();

        }
        if (ddlType.SelectedValue == "Car")
        {
            hideVehicles.Visible = true;
            cmd = new MySqlCommand("SELECT minimasters.mm_name, vehicel_master.registration_no, vehicel_master.vm_sno FROM vehicel_master INNER JOIN minimasters ON vehicel_master.vhtype_refno = minimasters.sno WHERE(minimasters.mm_name = @Car) AND (vehicel_master.branch_id = @BranchID)");
            cmd.Parameters.Add("@BranchID", BranchID);
            cmd.Parameters.Add("@Car", "car");
            DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
            ddlvehicles.DataSource = dttrips;
            ddlvehicles.DataTextField = "registration_no";
            ddlvehicles.DataValueField = "vm_sno";
            ddlvehicles.DataBind();

        }
        if (ddlType.SelectedValue == "Bus")
        {
            hideVehicles.Visible = true;
            cmd = new MySqlCommand("SELECT minimasters.mm_name, vehicel_master.registration_no, vehicel_master.vm_sno FROM vehicel_master INNER JOIN minimasters ON vehicel_master.vhtype_refno = minimasters.sno WHERE(minimasters.mm_name = @Bus) AND (vehicel_master.branch_id = @BranchID)");
            cmd.Parameters.Add("@BranchID", BranchID);
            cmd.Parameters.Add("@Bus", "bus");
            DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
            ddlvehicles.DataSource = dttrips;
            ddlvehicles.DataTextField = "registration_no";
            ddlvehicles.DataValueField = "vm_sno";
            ddlvehicles.DataBind();

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
            hidepanel.Visible = true;
            double tot_load = 0.0;
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

            lblType.Text = ddlType.SelectedItem.Text;
            lblFromDate.Text = fromdate.ToString("dd/MMM/yyyy");
            lbltodate.Text = todate.ToString("dd/MMM/yyyy");
            DataTable dtTrips = new DataTable();
            if (ddlType.SelectedValue == "All Puffs")
            {
                Session["filename"] = "Puffs Report";
                //lblVehicleNo.Text = ddlvehicles.SelectedItem.Text;
                //cmd = new MySqlCommand("SELECT DATE_FORMAT(triplogs.doe, '%d/%b/%y') as TripDate, locations.Location_name,triplogs.log_rank, triplogs.km, triplogs.charge, triplogs.charge * triplogs.km AS Amount, tripdata.tripsheetno, triplogs.tollgateamnt,triplogs.load_cap, triplogs.unload_cap, tripdata.endfuelvalue FROM  tripdata INNER JOIN triplogs ON tripdata.sno = triplogs.tripsno INNER JOIN locations ON triplogs.place = locations.sno WHERE (tripdata.enddate BETWEEN @d1 AND @d2)  AND (tripdata.userid = @BranchID) AND (tripdata.status = 'C') ORDER BY tripdata.tripdate, triplogs.log_rank");
                cmd = new MySqlCommand("SELECT DATE_FORMAT(triplogs.doe, '%d/%b/%y') AS TripDate,triplogs.fuel,vehicel_master.vm_owner,tripdata.DieselCost, locations.Location_name, triplogs.log_rank, triplogs.km, triplogs.charge, triplogs.charge * triplogs.km AS Amount, tripdata.tripsheetno,tripdata.hourreading, triplogs.tollgateamnt,triplogs.expamount, triplogs.load_cap, triplogs.unload_cap, tripdata.endfuelvalue,tripdata.EndHrMeter, vehicel_master.registration_no AS VehicleNo, employdata.employname AS DriverName, triplogs.odometer, tripdata.vehiclestartreading, tripdata.endodometerreading, tripdata.Dsalary FROM  tripdata INNER JOIN triplogs ON tripdata.sno = triplogs.tripsno INNER JOIN locations ON triplogs.place = locations.sno LEFT OUTER JOIN employdata ON tripdata.driverid = employdata.emp_sno LEFT OUTER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno WHERE (tripdata.enddate BETWEEN @d1 AND @d2) AND (tripdata.userid = @BranchID) AND (tripdata.status = 'C') AND (vehicel_master.vhtype_refno=7)  ORDER BY DATE(tripdata.enddate), triplogs.log_rank,vehicel_master.vhtype_refno");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@BranchID", BranchID);
                dtTrips = vdm.SelectQuery(cmd).Tables[0];
            }
            if (ddlType.SelectedValue == "Own Tankers")
            {
                Session["filename"] = "Tankers Report";
                //lblVehicleNo.Text = ddlvehicles.SelectedItem.Text;
                cmd = new MySqlCommand("SELECT tripdata.hourreading,tripdata.EndHrMeter, DATE_FORMAT(triplogs.doe, '%d/%b/%y') AS TripDate,triplogs.fuel,vehicel_master.vm_owner,tripdata.DieselCost, locations.Location_name, triplogs.log_rank, triplogs.km, triplogs.charge, triplogs.charge * triplogs.km AS Amount, tripdata.tripsheetno, triplogs.tollgateamnt,triplogs.expamount, triplogs.load_cap, triplogs.unload_cap, tripdata.endfuelvalue, vehicel_master.registration_no AS VehicleNo, employdata.employname AS DriverName, triplogs.odometer, tripdata.vehiclestartreading, tripdata.endodometerreading, tripdata.Dsalary FROM  tripdata INNER JOIN triplogs ON tripdata.sno = triplogs.tripsno INNER JOIN locations ON triplogs.place = locations.sno LEFT OUTER JOIN employdata ON tripdata.driverid = employdata.emp_sno LEFT OUTER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno WHERE (tripdata.enddate BETWEEN @d1 AND @d2) AND (tripdata.userid = @BranchID) AND (tripdata.status = 'C') AND (vehicel_master.vhtype_refno=22) AND (vehicel_master.vm_owner=@Owner) ORDER BY DATE(tripdata.enddate), triplogs.log_rank,vehicel_master.vhtype_refno");
                    cmd.Parameters.Add("@Owner", Session["shortname"].ToString());
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@BranchID", BranchID);
                dtTrips = vdm.SelectQuery(cmd).Tables[0];
            }
            if (ddlType.SelectedValue == "Private Tankers")
            {
                Session["filename"] = "Tankers Report";
                //lblVehicleNo.Text = ddlvehicles.SelectedItem.Text;
                //cmd = new MySqlCommand("SELECT DATE_FORMAT(triplogs.doe, '%d/%b/%y') as TripDate, locations.Location_name,triplogs.log_rank, triplogs.km, triplogs.charge, triplogs.charge * triplogs.km AS Amount, tripdata.tripsheetno, triplogs.tollgateamnt,triplogs.load_cap, triplogs.unload_cap, tripdata.endfuelvalue FROM  tripdata INNER JOIN triplogs ON tripdata.sno = triplogs.tripsno INNER JOIN locations ON triplogs.place = locations.sno WHERE (tripdata.enddate BETWEEN @d1 AND @d2)  AND (tripdata.userid = @BranchID) AND (tripdata.status = 'C') ORDER BY tripdata.tripdate, triplogs.log_rank");
                cmd = new MySqlCommand("SELECT tripdata.hourreading,tripdata.EndHrMeter, DATE_FORMAT(triplogs.doe, '%d/%b/%y') AS TripDate,triplogs.fuel,vehicel_master.vm_owner,tripdata.DieselCost, locations.Location_name, triplogs.log_rank, triplogs.km, triplogs.charge, triplogs.charge * triplogs.km AS Amount, tripdata.tripsheetno, triplogs.tollgateamnt,triplogs.expamount, triplogs.load_cap, triplogs.unload_cap, tripdata.endfuelvalue, vehicel_master.registration_no AS VehicleNo, employdata.employname AS DriverName, triplogs.odometer, tripdata.vehiclestartreading, tripdata.endodometerreading, tripdata.Dsalary FROM  tripdata INNER JOIN triplogs ON tripdata.sno = triplogs.tripsno INNER JOIN locations ON triplogs.place = locations.sno LEFT OUTER JOIN employdata ON tripdata.driverid = employdata.emp_sno LEFT OUTER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno WHERE (tripdata.enddate BETWEEN @d1 AND @d2) AND (tripdata.userid = @BranchID) AND (tripdata.status = 'C') AND (vehicel_master.vhtype_refno=22) AND (vehicel_master.vm_owner<>@Owner) ORDER BY DATE(tripdata.enddate), triplogs.log_rank,vehicel_master.vhtype_refno");
                cmd.Parameters.Add("@Owner", Session["shortname"].ToString());
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@BranchID", BranchID);
                dtTrips = vdm.SelectQuery(cmd).Tables[0];
            }
            if (ddlType.SelectedValue == "All Tankers")
            {
                Session["filename"] = "Tankers Report";
                //lblVehicleNo.Text = ddlvehicles.SelectedItem.Text;
                //cmd = new MySqlCommand("SELECT DATE_FORMAT(triplogs.doe, '%d/%b/%y') as TripDate, locations.Location_name,triplogs.log_rank, triplogs.km, triplogs.charge, triplogs.charge * triplogs.km AS Amount, tripdata.tripsheetno, triplogs.tollgateamnt,triplogs.load_cap, triplogs.unload_cap, tripdata.endfuelvalue FROM  tripdata INNER JOIN triplogs ON tripdata.sno = triplogs.tripsno INNER JOIN locations ON triplogs.place = locations.sno WHERE (tripdata.enddate BETWEEN @d1 AND @d2)  AND (tripdata.userid = @BranchID) AND (tripdata.status = 'C') ORDER BY tripdata.tripdate, triplogs.log_rank");
                cmd = new MySqlCommand("SELECT tripdata.hourreading,tripdata.EndHrMeter, DATE_FORMAT(triplogs.doe, '%d/%b/%y') AS TripDate,triplogs.fuel,vehicel_master.vm_owner,tripdata.DieselCost, locations.Location_name, triplogs.log_rank, triplogs.km, triplogs.charge, triplogs.charge * triplogs.km AS Amount, tripdata.tripsheetno, triplogs.tollgateamnt,triplogs.expamount, triplogs.load_cap, triplogs.unload_cap, tripdata.endfuelvalue, vehicel_master.registration_no AS VehicleNo, employdata.employname AS DriverName, triplogs.odometer, tripdata.vehiclestartreading, tripdata.endodometerreading FROM, tripdata.Dsalary  tripdata INNER JOIN triplogs ON tripdata.sno = triplogs.tripsno INNER JOIN locations ON triplogs.place = locations.sno LEFT OUTER JOIN employdata ON tripdata.driverid = employdata.emp_sno LEFT OUTER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno WHERE (tripdata.enddate BETWEEN @d1 AND @d2) AND (tripdata.userid = @BranchID) AND (tripdata.status = 'C') AND (vehicel_master.vhtype_refno=22) ORDER BY DATE(tripdata.enddate), triplogs.log_rank,vehicel_master.vhtype_refno");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@BranchID", BranchID);
                dtTrips = vdm.SelectQuery(cmd).Tables[0];
            }
            if (ddlType.SelectedValue == "All")
            {
                Session["filename"] = "Tankers Report";
                //lblVehicleNo.Text = ddlvehicles.SelectedItem.Text;
                //cmd = new MySqlCommand("SELECT DATE_FORMAT(triplogs.doe, '%d/%b/%y') as TripDate, locations.Location_name,triplogs.log_rank, triplogs.km, triplogs.charge, triplogs.charge * triplogs.km AS Amount, tripdata.tripsheetno, triplogs.tollgateamnt,triplogs.load_cap, triplogs.unload_cap, tripdata.endfuelvalue FROM  tripdata INNER JOIN triplogs ON tripdata.sno = triplogs.tripsno INNER JOIN locations ON triplogs.place = locations.sno WHERE (tripdata.enddate BETWEEN @d1 AND @d2)  AND (tripdata.userid = @BranchID) AND (tripdata.status = 'C') ORDER BY tripdata.tripdate, triplogs.log_rank");
                cmd = new MySqlCommand("SELECT tripdata.hourreading,tripdata.EndHrMeter, DATE_FORMAT(triplogs.doe, '%d/%b/%y') AS TripDate,triplogs.fuel,vehicel_master.vm_owner,tripdata.DieselCost, locations.Location_name, triplogs.log_rank, triplogs.km, triplogs.charge, triplogs.charge * triplogs.km AS Amount, tripdata.tripsheetno, triplogs.tollgateamnt,triplogs.expamount, triplogs.load_cap, triplogs.unload_cap, tripdata.endfuelvalue, vehicel_master.registration_no AS VehicleNo, employdata.employname AS DriverName, triplogs.odometer, tripdata.vehiclestartreading, tripdata.endodometerreading, tripdata.Dsalary FROM  tripdata INNER JOIN triplogs ON tripdata.sno = triplogs.tripsno INNER JOIN locations ON triplogs.place = locations.sno LEFT OUTER JOIN employdata ON tripdata.driverid = employdata.emp_sno LEFT OUTER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno WHERE (tripdata.enddate BETWEEN @d1 AND @d2) AND (tripdata.userid = @BranchID) AND (tripdata.status = 'C')  ORDER BY DATE(tripdata.enddate), triplogs.log_rank,vehicel_master.vhtype_refno");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@BranchID", BranchID);
                dtTrips = vdm.SelectQuery(cmd).Tables[0];
            }
            if (ddlType.SelectedValue == "Tanker" || ddlType.SelectedValue == "Truck" || ddlType.SelectedValue == "Puff" || ddlType.SelectedValue == "Car" || ddlType.SelectedValue == "Bus")
            {
                Session["filename"] = ddlvehicles.SelectedItem.Text + " Report";
                lblVehicleNo.Text = ddlvehicles.SelectedItem.Text;
                cmd = new MySqlCommand("SELECT tripdata.hourreading,tripdata.EndHrMeter, DATE_FORMAT(triplogs.doe, '%d/%b/%y') AS TripDate, triplogs.fuel,vehicel_master.vm_owner,tripdata.DieselCost, locations.Location_name, tripdata.starthrmeter,tripdata.EndHrMeter,triplogs.log_rank, triplogs.km, triplogs.charge, triplogs.charge * triplogs.km AS Amount, tripdata.tripsheetno, triplogs.tollgateamnt,triplogs.expamount, triplogs.load_cap, triplogs.unload_cap, tripdata.endfuelvalue, vehicel_master.registration_no AS VehicleNo, employdata.employname AS DriverName, triplogs.odometer, tripdata.vehiclestartreading, tripdata.endodometerreading, tripdata.Dsalary  FROM  tripdata INNER JOIN triplogs ON tripdata.sno = triplogs.tripsno INNER JOIN locations ON triplogs.place = locations.sno LEFT OUTER JOIN employdata ON tripdata.driverid = employdata.emp_sno LEFT OUTER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno WHERE (tripdata.enddate BETWEEN @d1 AND @d2) AND (tripdata.userid = @BranchID) AND (tripdata.vehicleno = @VehicleNo) AND (tripdata.status = 'C') ORDER BY tripdata.tripdate, triplogs.log_rank");
                //cmd = new MySqlCommand("SELECT DATE_FORMAT(triplogs.doe, '%d/%b/%y') as TripDate, locations.Location_name,triplogs.log_rank, triplogs.km, triplogs.charge, triplogs.charge * triplogs.km AS Amount, tripdata.tripsheetno, triplogs.tollgateamnt,triplogs.load_cap, triplogs.unload_cap, tripdata.endfuelvalue, vehicel_master.registration_no AS VehicleNo, employdata.employname AS DriverName, triplogs.odometer FROM  tripdata INNER JOIN triplogs ON tripdata.sno = triplogs.tripsno INNER JOIN locations ON triplogs.place = locations.sno WHERE (tripdata.tripdate BETWEEN @d1 AND @d2) AND (tripdata.vehicleno = @VehicleNo) AND (tripdata.userid = @BranchID) AND (tripdata.status = 'C') ORDER BY tripdata.tripdate, triplogs.log_rank");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@BranchID", BranchID);
                cmd.Parameters.Add("@VehicleNo", ddlvehicles.SelectedValue);
                dtTrips = vdm.SelectQuery(cmd).Tables[0];
            }
            DataTable trips = new DataView(dtTrips).ToTable(true, "tripsheetno");
            int i = 1;
            if (dtTrips.Rows.Count > 0)
            {
                DataTable exp = new DataTable();
                exp.Columns.Add("Sno");
                exp.Columns.Add("TripsheetNo");
                exp.Columns.Add("vehicleNo");
                exp.Columns.Add("Owner");
                exp.Columns.Add("Driver");
                exp.Columns.Add("Trip Date");
                exp.Columns.Add("Odometer");
                exp.Columns.Add("Location");
                exp.Columns.Add("Kms").DataType = typeof(Double);
                exp.Columns.Add("Amount");
                exp.Columns.Add("Total Amount").DataType = typeof(Double);
                exp.Columns.Add("Load").DataType = typeof(Double);
                exp.Columns.Add("UnLoad").DataType = typeof(Double);
                exp.Columns.Add("Diesel").DataType = typeof(Double);
                exp.Columns.Add("Diesel Cost").DataType = typeof(Double);
                exp.Columns.Add("Mileage").DataType = typeof(Double);
                exp.Columns.Add("Tollgate").DataType = typeof(Double);
                exp.Columns.Add("Expenses").DataType = typeof(Double);
                exp.Columns.Add("Salary").DataType = typeof(Double);
                exp.Columns.Add("P & L").DataType = typeof(Double);
                exp.Columns.Add("Reading");
                exp.Columns.Add("HourReading").DataType = typeof(Double);
                //exp.Columns.Add("HourReading");

                foreach (DataRow dr2 in trips.Rows)
                {
                    Report = new DataTable();
                    Report.Columns.Add("Sno");
                    Report.Columns.Add("TripsheetNo");
                    Report.Columns.Add("Trip Date");
                    Report.Columns.Add("vehicleNo");
                    Report.Columns.Add("Owner");
                    Report.Columns.Add("Driver");
                    Report.Columns.Add("Odometer");
                    Report.Columns.Add("Location");
                    Report.Columns.Add("Kms").DataType = typeof(Double);
                    Report.Columns.Add("Amount");
                    Report.Columns.Add("Total Amount").DataType = typeof(Double);
                    Report.Columns.Add("Tollgate").DataType = typeof(Double);
                    Report.Columns.Add("Load").DataType = typeof(Double);
                    Report.Columns.Add("UnLoad").DataType = typeof(Double);
                    Report.Columns.Add("Diesel").DataType = typeof(Double);
                    Report.Columns.Add("Diesel Cost").DataType = typeof(Double);
                    Report.Columns.Add("Mileage");
                    Report.Columns.Add("Expenses").DataType = typeof(Double);
                    Report.Columns.Add("Salary").DataType = typeof(Double);
                    Report.Columns.Add("P & L").DataType = typeof(Double);
                    Report.Columns.Add("Reading");
                    Report.Columns.Add("HourReading").DataType = typeof(Double);
                  //  Report.Columns.Add("HourReading");
                    DataRow[] newdatarow = dtTrips.Select("tripsheetno='" + dr2["tripsheetno"].ToString() + "'");
                    string strTime = "";
                    string Location1 = "";
                    string Location2 = "";
                    string Location3 = "";
                    string Location4 = "";
                    string Location5 = "";
                    string Location6 = "";
                    string Location7 = "";
                    string Location8 = "";
                    string Location9 = "";
                    string Location10 = "";
                    string Location11 = "";
                    string Location12 = "";
                    string Location13 = "";
                    string Location14 = "";
                    string Location15 = "";
                    string Location16 = "";
                    string Location17 = "";
                    string Location18 = "";
                    string Location19 = "";
                    string Location20 = "";

                    string Rank = "";
                    string loc2 = "";
                    string loc3 = "";
                    string loc4 = "";
                    string loc5 = "";
                    string loc6 = "";
                    string loc7 = "";
                    string loc8 = "";
                    string loc9 = "";
                    string loc10 = "";
                    string loc11 = "";
                    string loc12 = "";
                    string loc13 = "";
                    string loc14 = "";
                    string loc15 = "";
                    string loc16 = "";
                    string loc17 = "";
                    string loc18 = "";
                    string loc19 = "";
                    string loc20 = "";

                    string date1 = "";
                    string date2 = "";
                    string date3 = "";
                    string date4 = "";
                    string date5 = "";
                    string date6 = "";
                    string date7 = "";
                    string date8 = "";
                    string date9 = "";
                    string date10 = "";
                    string date11 = "";
                    string date12 = "";
                    string date13 = "";
                    string date14 = "";
                    string date15 = "";
                    string date16 = "";
                    string date17 = "";
                    string date18 = "";
                    string date19 = "";
                    string date20 = "";

                    string d1 = "";
                    string d2 = "";
                    string d3 = "";
                    string d4 = "";
                    string d5 = "";
                    string d6 = "";
                    string d7 = "";
                    string d8 = "";
                    string d9 = "";
                    string d10 = "";
                    string d11 = "";
                    string d12 = "";
                    string d13 = "";
                    string d14 = "";
                    string d15 = "";
                    string d16 = "";
                    string d17 = "";
                    string d18 = "";
                    string d19 = "";
                    string d20 = "";

                    string odo1 = "";
                    string hr1 = "";
                    foreach (DataRow dr in newdatarow)
                    {
                        Rank = dr["log_rank"].ToString();
                        if (Rank == "1")
                        {
                            Location1 = dr["Location_name"].ToString();
                            date1 = dr["TripDate"].ToString();
                            odo1 = dr["odometer"].ToString();
                            hr1 = dr["hourreading"].ToString();
                        }
                        if (Rank == "2")
                        {
                            DataRow newrow = Report.NewRow();
                            newrow["SNo"] = i++.ToString();
                            newrow["TripsheetNo"] = dr["tripsheetno"].ToString();
                            Location2 = Location1 + " - " + dr["Location_name"].ToString();
                            loc2 = dr["Location_name"].ToString();
                            d2 = date1 + "  -  " + dr["TripDate"].ToString();
                            date2 = dr["TripDate"].ToString();
                            newrow["Trip Date"] = d2;
                            newrow["Location"] = Location2;
                            newrow["VehicleNo"] = dr["vehicleNo"].ToString();
                            newrow["Owner"] = dr["vm_owner"].ToString();
                            newrow["Odometer"] = odo1 + " - " + dr["odometer"].ToString();
                            newrow["Driver"] = dr["DriverName"].ToString();
                            newrow["Kms"] = dr["km"].ToString();
                            newrow["Amount"] = dr["charge"].ToString();
                            newrow["Total Amount"] = dr["Amount"].ToString();
                            newrow["Tollgate"] = dr["tollgateamnt"].ToString();
                            newrow["Expenses"] = dr["expamount"].ToString();
                            newrow["Load"] = dr["load_cap"].ToString();
                            newrow["UnLoad"] = dr["unload_cap"].ToString();
                            //int loadval = Convert.ToInt32(dr["load_cap"].ToString());
                            ////double ld = "";
                            //if (loadval > 0)
                            //{
                            //    newrow["Load"] = dr["load_cap"].ToString();
                            //}
                            //else
                            //{
                            //    newrow["Load"] = "";
                            //}
                            //int load = 0;
                            //int.TryParse(loadval.ToString(), out load);
                            //tot_load += load;
                            int startreading = 0;
                            int.TryParse(hr1, out startreading);
                            int endingreading = 0;
                            int.TryParse(dr["EndHrMeter"].ToString(), out endingreading);
                            int diff = endingreading - startreading;
                            newrow["Reading"] = startreading + " - " + endingreading;
                            newrow["HourReading"] = diff.ToString("F2");
                            double fuel = 0;
                            double.TryParse(dr["endfuelvalue"].ToString(), out fuel);
                            double outfuel = 0;

                            foreach (DataRow drdie in newdatarow)
                            {
                                double out_fuel = 0;
                                double.TryParse(drdie["fuel"].ToString(), out out_fuel);
                                outfuel += out_fuel;
                            }
                            fuel = outfuel + fuel;
                            newrow["Diesel"] = fuel.ToString();
                            double DieselCost = 0;
                            double.TryParse(dr["DieselCost"].ToString(), out DieselCost);
                            double totalDieselCost = 0;
                            totalDieselCost = fuel * DieselCost;
                            totalDieselCost = Math.Round(totalDieselCost, 2);
                            newrow["Diesel Cost"] = totalDieselCost.ToString();

                            double vehiclestartreading = 0;
                            double.TryParse(dr["vehiclestartreading"].ToString(), out vehiclestartreading);
                            double endodometerreading = 0;
                            double.TryParse(dr["endodometerreading"].ToString(), out endodometerreading);
                            double tripkms = 0;
                            tripkms = endodometerreading - vehiclestartreading;
                            double Mileage = 0;
                            Mileage = tripkms / fuel;
                            Mileage = Math.Round(Mileage, 2);
                            newrow["Mileage"] = Mileage.ToString();

                            double tollgate = 0;
                            double.TryParse(dr["tollgateamnt"].ToString(), out tollgate);
                            double expamount = 0;
                            double.TryParse(dr["expamount"].ToString(), out expamount);
                            double salary = 0;
                            double.TryParse(dr["Dsalary"].ToString(), out salary);
                            if (salary > 0)
                            {

                            }
                            else
                            {
                                salary = 613.767;
                            }
                            newrow["Salary"] = salary.ToString();
                            double deduction = 0;
                            deduction = expamount + totalDieselCost + tollgate + salary;
                            double Amount = 0;
                            double.TryParse(dr["Amount"].ToString(), out Amount);
                            double pl = 0;
                            pl = Amount - deduction;
                            newrow["P & L"] = pl.ToString();
                            Report.Rows.Add(newrow);
                        }
                        if (Rank == "3")
                        {
                            DataRow newrow = Report.NewRow();
                            Location3 = loc2 + " - " + dr["Location_name"].ToString();
                            loc3 = dr["Location_name"].ToString();
                            d3 = date2 + "  -  " + dr["TripDate"].ToString();
                            date3 = dr["TripDate"].ToString();
                            newrow["Trip Date"] = d3;
                            newrow["Location"] = Location3;
                            newrow["Odometer"] = dr["odometer"].ToString();
                            newrow["Kms"] = dr["km"].ToString();
                            newrow["Amount"] = dr["charge"].ToString();
                            newrow["Total Amount"] = dr["Amount"].ToString();
                            newrow["Tollgate"] = dr["tollgateamnt"].ToString();
                            newrow["Expenses"] = dr["expamount"].ToString();
                            newrow["Load"] = dr["load_cap"].ToString();
                            newrow["UnLoad"] = dr["unload_cap"].ToString();
                            newrow["P & L"] = dr["Amount"].ToString();
                            Report.Rows.Add(newrow);
                        }
                        if (Rank == "4")
                        {
                            DataRow newrow = Report.NewRow();
                            Location4 = loc3 + "  -  " + dr["Location_name"].ToString();
                            loc4 = dr["Location_name"].ToString();
                            d4 = date3 + "  -  " + dr["TripDate"].ToString();
                            date4 = dr["TripDate"].ToString();
                            newrow["Trip Date"] = d4;
                            newrow["Location"] = Location4;
                            newrow["Odometer"] = dr["odometer"].ToString();
                            newrow["Kms"] = dr["km"].ToString();
                            newrow["Amount"] = dr["charge"].ToString();
                            newrow["Total Amount"] = dr["Amount"].ToString();
                            newrow["Tollgate"] = dr["tollgateamnt"].ToString();
                            newrow["Expenses"] = dr["expamount"].ToString();
                            newrow["Load"] = dr["load_cap"].ToString();
                            newrow["UnLoad"] = dr["unload_cap"].ToString();
                            newrow["P & L"] = dr["Amount"].ToString();
                            Report.Rows.Add(newrow);
                        }
                        if (Rank == "5")
                        {
                            DataRow newrow = Report.NewRow();
                            Location5 = loc4 + "  -  " + dr["Location_name"].ToString();
                            loc5 = dr["Location_name"].ToString();
                            d5 = date4 + "  -  " + dr["TripDate"].ToString();
                            date5 = dr["TripDate"].ToString();
                            newrow["Trip Date"] = d5;
                            newrow["Location"] = Location5;
                            newrow["Odometer"] = dr["odometer"].ToString();
                            newrow["Kms"] = dr["km"].ToString();
                            newrow["Amount"] = dr["charge"].ToString();
                            newrow["Total Amount"] = dr["Amount"].ToString();
                            newrow["Tollgate"] = dr["tollgateamnt"].ToString();
                            newrow["Expenses"] = dr["expamount"].ToString();
                            newrow["Load"] = dr["load_cap"].ToString();
                            newrow["UnLoad"] = dr["unload_cap"].ToString();
                            Report.Rows.Add(newrow);
                        }
                        if (Rank == "6")
                        {
                            DataRow newrow = Report.NewRow();
                            Location6 = loc5 + "  -  " + dr["Location_name"].ToString();
                            loc6 = dr["Location_name"].ToString();
                            d6 = date5 + "  -  " + dr["TripDate"].ToString();
                            date6 = dr["TripDate"].ToString();
                            newrow["Trip Date"] = d6;
                            newrow["Location"] = Location6;
                            newrow["Odometer"] = dr["odometer"].ToString();
                            newrow["Kms"] = dr["km"].ToString();
                            newrow["Amount"] = dr["charge"].ToString();
                            newrow["Total Amount"] = dr["Amount"].ToString();
                            newrow["Tollgate"] = dr["tollgateamnt"].ToString();
                            newrow["Expenses"] = dr["expamount"].ToString();
                            newrow["Load"] = dr["load_cap"].ToString();
                            newrow["UnLoad"] = dr["unload_cap"].ToString();
                            Report.Rows.Add(newrow);
                        }
                        if (Rank == "7")
                        {
                            DataRow newrow = Report.NewRow();
                            Location7 = loc6 + "  -  " + dr["Location_name"].ToString();
                            loc7 = dr["Location_name"].ToString();
                            d7 = date6 + "  -  " + dr["TripDate"].ToString();
                            date7 = dr["TripDate"].ToString();
                            newrow["Trip Date"] = d7;
                            newrow["Location"] = Location7;
                            newrow["Odometer"] = dr["odometer"].ToString();
                            newrow["Kms"] = dr["km"].ToString();
                            newrow["Amount"] = dr["charge"].ToString();
                            newrow["Total Amount"] = dr["Amount"].ToString();
                            newrow["Tollgate"] = dr["tollgateamnt"].ToString();
                            newrow["Expenses"] = dr["expamount"].ToString();
                            newrow["Load"] = dr["load_cap"].ToString();
                            newrow["UnLoad"] = dr["unload_cap"].ToString();
                            Report.Rows.Add(newrow);
                        }
                        if (Rank == "8")
                        {
                            DataRow newrow = Report.NewRow();
                            Location8 = loc7 + "  -  " + dr["Location_name"].ToString();
                            loc8 = dr["Location_name"].ToString();
                            d8 = date7 + "  -  " + dr["TripDate"].ToString();
                            date8 = dr["TripDate"].ToString();
                            newrow["Trip Date"] = d8;
                            newrow["Location"] = Location8;
                            newrow["Odometer"] = dr["odometer"].ToString();
                            newrow["Kms"] = dr["km"].ToString();
                            newrow["Amount"] = dr["charge"].ToString();
                            newrow["Total Amount"] = dr["Amount"].ToString();
                            newrow["Tollgate"] = dr["tollgateamnt"].ToString();
                            newrow["Expenses"] = dr["expamount"].ToString();
                            newrow["Load"] = dr["load_cap"].ToString();
                            newrow["UnLoad"] = dr["unload_cap"].ToString();
                            Report.Rows.Add(newrow);
                        }
                        if (Rank == "9")
                        {
                            DataRow newrow = Report.NewRow();
                            Location9 = loc8 + "  -  " + dr["Location_name"].ToString();
                            loc9 = dr["Location_name"].ToString();
                            d9 = date8 + "  -  " + dr["TripDate"].ToString();
                            date9 = dr["TripDate"].ToString();
                            newrow["Trip Date"] = d9;
                            newrow["Location"] = Location9;
                            newrow["Odometer"] = dr["odometer"].ToString();
                            newrow["Kms"] = dr["km"].ToString();
                            newrow["Amount"] = dr["charge"].ToString();
                            newrow["Total Amount"] = dr["Amount"].ToString();
                            newrow["Tollgate"] = dr["tollgateamnt"].ToString();
                            newrow["Expenses"] = dr["expamount"].ToString();
                            newrow["Load"] = dr["load_cap"].ToString();
                            newrow["UnLoad"] = dr["unload_cap"].ToString();
                            Report.Rows.Add(newrow);
                        }
                        if (Rank == "10")
                        {
                            DataRow newrow = Report.NewRow();
                            Location10 = loc9 + "  -  " + dr["Location_name"].ToString();
                            loc10 = dr["Location_name"].ToString();
                            d10 = date9 + "  -  " + dr["TripDate"].ToString();
                            date10 = dr["TripDate"].ToString();
                            newrow["Trip Date"] = d10;
                            newrow["Location"] = Location10;
                            newrow["Odometer"] = dr["odometer"].ToString();
                            newrow["Kms"] = dr["km"].ToString();
                            newrow["Amount"] = dr["charge"].ToString();
                            newrow["Total Amount"] = dr["Amount"].ToString();
                            newrow["Tollgate"] = dr["tollgateamnt"].ToString();
                            newrow["Expenses"] = dr["expamount"].ToString();
                            newrow["Load"] = dr["load_cap"].ToString();
                            newrow["UnLoad"] = dr["unload_cap"].ToString();
                            Report.Rows.Add(newrow);
                        }
                        if (Rank == "11")
                        {
                            DataRow newrow = Report.NewRow();
                            Location11 = loc10 + "  -  " + dr["Location_name"].ToString();
                            loc11 = dr["Location_name"].ToString();
                            d11 = date10 + "  -  " + dr["TripDate"].ToString();
                            date11 = dr["TripDate"].ToString();
                            newrow["Trip Date"] = d11;
                            newrow["Location"] = Location11;
                            newrow["Odometer"] = dr["odometer"].ToString();
                            newrow["Kms"] = dr["km"].ToString();
                            newrow["Amount"] = dr["charge"].ToString();
                            newrow["Total Amount"] = dr["Amount"].ToString();
                            newrow["Tollgate"] = dr["tollgateamnt"].ToString();
                            newrow["Expenses"] = dr["expamount"].ToString();
                            newrow["Load"] = dr["load_cap"].ToString();
                            newrow["UnLoad"] = dr["unload_cap"].ToString();
                            Report.Rows.Add(newrow);
                        }
                        if (Rank == "12")
                        {
                            DataRow newrow = Report.NewRow();
                            Location12 = loc11 + "  -  " + dr["Location_name"].ToString();
                            loc12 = dr["Location_name"].ToString();
                            d12 = date11 + "  -  " + dr["TripDate"].ToString();
                            date12 = dr["TripDate"].ToString();
                            newrow["Trip Date"] = d12;
                            newrow["Location"] = Location12;
                            newrow["Odometer"] = dr["odometer"].ToString();
                            newrow["Kms"] = dr["km"].ToString();
                            newrow["Amount"] = dr["charge"].ToString();
                            newrow["Total Amount"] = dr["Amount"].ToString();
                            newrow["Tollgate"] = dr["tollgateamnt"].ToString();
                            newrow["Expenses"] = dr["expamount"].ToString();
                            newrow["Load"] = dr["load_cap"].ToString();
                            newrow["UnLoad"] = dr["unload_cap"].ToString();
                            Report.Rows.Add(newrow);
                        }
                        if (Rank == "13")
                        {
                            DataRow newrow = Report.NewRow();
                            Location13 = loc12 + "  -  " + dr["Location_name"].ToString();
                            loc13 = dr["Location_name"].ToString();
                            d13 = date12 + "  -  " + dr["TripDate"].ToString();
                            date13 = dr["TripDate"].ToString();
                            newrow["Trip Date"] = d13;
                            newrow["Location"] = Location13;
                            newrow["Odometer"] = dr["odometer"].ToString();
                            newrow["Kms"] = dr["km"].ToString();
                            newrow["Amount"] = dr["charge"].ToString();
                            newrow["Total Amount"] = dr["Amount"].ToString();
                            newrow["Tollgate"] = dr["tollgateamnt"].ToString();
                            newrow["Expenses"] = dr["expamount"].ToString();
                            newrow["Load"] = dr["load_cap"].ToString();
                            newrow["UnLoad"] = dr["unload_cap"].ToString();
                            Report.Rows.Add(newrow);
                        }
                        if (Rank == "14")
                        {
                            DataRow newrow = Report.NewRow();
                            Location14 = loc13 + "  -  " + dr["Location_name"].ToString();
                            loc14 = dr["Location_name"].ToString();
                            d14 = date13 + "  -  " + dr["TripDate"].ToString();
                            date14 = dr["TripDate"].ToString();
                            newrow["Trip Date"] = d14;
                            newrow["Location"] = Location14;
                            newrow["Odometer"] = dr["odometer"].ToString();
                            newrow["Kms"] = dr["km"].ToString();
                            newrow["Amount"] = dr["charge"].ToString();
                            newrow["Total Amount"] = dr["Amount"].ToString();
                            newrow["Tollgate"] = dr["tollgateamnt"].ToString();
                            newrow["Expenses"] = dr["expamount"].ToString();
                            newrow["Load"] = dr["load_cap"].ToString();
                            newrow["UnLoad"] = dr["unload_cap"].ToString();
                            Report.Rows.Add(newrow);
                        }
                        if (Rank == "15")
                        {
                            DataRow newrow = Report.NewRow();
                            Location15 = loc14 + "  -  " + dr["Location_name"].ToString();
                            loc15 = dr["Location_name"].ToString();
                            d15 = date14 + "  -  " + dr["TripDate"].ToString();
                            date15 = dr["TripDate"].ToString();
                            newrow["Trip Date"] = d15;
                            newrow["Location"] = Location15;
                            newrow["Odometer"] = dr["odometer"].ToString();
                            newrow["Kms"] = dr["km"].ToString();
                            newrow["Amount"] = dr["charge"].ToString();
                            newrow["Total Amount"] = dr["Amount"].ToString();
                            newrow["Tollgate"] = dr["tollgateamnt"].ToString();
                            newrow["Expenses"] = dr["expamount"].ToString();
                            newrow["Load"] = dr["load_cap"].ToString();
                            newrow["UnLoad"] = dr["unload_cap"].ToString();
                            Report.Rows.Add(newrow);
                        }
                        if (Rank == "16")
                        {
                            DataRow newrow = Report.NewRow();
                            Location16 = loc15 + "  -  " + dr["Location_name"].ToString();
                            loc16 = dr["Location_name"].ToString();
                            d16 = date15 + "  -  " + dr["TripDate"].ToString();
                            date16 = dr["TripDate"].ToString();
                            newrow["Trip Date"] = d16;
                            newrow["Location"] = Location16;
                            newrow["Odometer"] = dr["odometer"].ToString();
                            newrow["Kms"] = dr["km"].ToString();
                            newrow["Amount"] = dr["charge"].ToString();
                            newrow["Total Amount"] = dr["Amount"].ToString();
                            newrow["Tollgate"] = dr["tollgateamnt"].ToString();
                            newrow["Expenses"] = dr["expamount"].ToString();
                            newrow["Load"] = dr["load_cap"].ToString();
                            newrow["UnLoad"] = dr["unload_cap"].ToString();
                            Report.Rows.Add(newrow);
                        }
                        if (Rank == "17")
                        {
                            DataRow newrow = Report.NewRow();
                            Location17 = loc16 + "  -  " + dr["Location_name"].ToString();
                            loc17 = dr["Location_name"].ToString();
                            d17 = date16 + "  -  " + dr["TripDate"].ToString();
                            date17 = dr["TripDate"].ToString();
                            newrow["Trip Date"] = d17;
                            newrow["Location"] = Location17;
                            newrow["Odometer"] = dr["odometer"].ToString();
                            newrow["Kms"] = dr["km"].ToString();
                            newrow["Amount"] = dr["charge"].ToString();
                            newrow["Total Amount"] = dr["Amount"].ToString();
                            newrow["Tollgate"] = dr["tollgateamnt"].ToString();
                            newrow["Expenses"] = dr["expamount"].ToString();
                            newrow["Load"] = dr["load_cap"].ToString();
                            newrow["UnLoad"] = dr["unload_cap"].ToString();
                            Report.Rows.Add(newrow);
                        }
                        if (Rank == "18")
                        {
                            DataRow newrow = Report.NewRow();
                            Location18 = loc17 + "  -  " + dr["Location_name"].ToString();
                            loc18 = dr["Location_name"].ToString();
                            d18 = date17 + "  -  " + dr["TripDate"].ToString();
                            date18 = dr["TripDate"].ToString();
                            newrow["Trip Date"] = d18;
                            newrow["Location"] = Location18;
                            newrow["Odometer"] = dr["odometer"].ToString();
                            newrow["Kms"] = dr["km"].ToString();
                            newrow["Amount"] = dr["charge"].ToString();
                            newrow["Total Amount"] = dr["Amount"].ToString();
                            newrow["Tollgate"] = dr["tollgateamnt"].ToString();
                            newrow["Load"] = dr["load_cap"].ToString();
                            newrow["UnLoad"] = dr["unload_cap"].ToString();
                            Report.Rows.Add(newrow);
                        }
                        if (Rank == "19")
                        {
                            DataRow newrow = Report.NewRow();
                            Location19 = loc18 + "  -  " + dr["Location_name"].ToString();
                            loc19 = dr["Location_name"].ToString();
                            d19 = date18 + "  -  " + dr["TripDate"].ToString();
                            date19 = dr["TripDate"].ToString();
                            newrow["Trip Date"] = d19;
                            newrow["Location"] = Location19;
                            newrow["Odometer"] = dr["odometer"].ToString();
                            newrow["Kms"] = dr["km"].ToString();
                            newrow["Amount"] = dr["charge"].ToString();
                            newrow["Total Amount"] = dr["Amount"].ToString();
                            newrow["Tollgate"] = dr["tollgateamnt"].ToString();
                            newrow["Expenses"] = dr["expamount"].ToString();
                            newrow["Load"] = dr["load_cap"].ToString();
                            newrow["UnLoad"] = dr["unload_cap"].ToString();
                            Report.Rows.Add(newrow);
                        }
                        if (Rank == "20")
                        {
                            DataRow newrow = Report.NewRow();
                            Location20 = loc19 + "  -  " + dr["Location_name"].ToString();
                            loc20 = dr["Location_name"].ToString();
                            d20 = date19 + "  -  " + dr["TripDate"].ToString();
                            date20 = dr["TripDate"].ToString();
                            newrow["Trip Date"] = d20;
                            newrow["Location"] = Location20;
                            newrow["Odometer"] = dr["odometer"].ToString();
                            newrow["Kms"] = dr["km"].ToString();
                            newrow["Amount"] = dr["charge"].ToString();
                            newrow["Total Amount"] = dr["Amount"].ToString();
                            newrow["Tollgate"] = dr["tollgateamnt"].ToString();
                            newrow["Expenses"] = dr["expamount"].ToString();
                            newrow["Load"] = dr["load_cap"].ToString();
                            newrow["UnLoad"] = dr["unload_cap"].ToString();
                            Report.Rows.Add(newrow);
                        }
                    }
                    DataRow newvartical = Report.NewRow();
                    newvartical["Location"] = "Total";
                    double val = 0.0;
                    foreach (DataColumn dc in Report.Columns)
                    {
                        if (dc.DataType == typeof(Double))
                        {
                            val = 0.0;
                            double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
                            newvartical[dc.ToString()] = val;
                        }
                    }
                    Report.Rows.Add(newvartical);
                    foreach (DataRow item in Report.Rows)
                    {
                        exp.ImportRow(item);
                    }
                }
                DataRow Grandtotal = exp.NewRow();
                DataRow[] ttl = exp.Select("Location='Total'");
                double grdttl = 0.00;
                double km = 0.00;
                double toll = 0.00;
                double tot_diesel = 0.00;
                double tot_exp = 0.00;
                double tot_HourReading = 0.00;
                double tot_dieselval = 0.00;
                double tot_unload = 0.00;
                foreach (DataRow item in ttl)
                {
                    double temp = 0.00;
                    double.TryParse(item["Total Amount"].ToString(), out temp);
                    grdttl += temp;
                    double temp2 = 0.00;
                    double.TryParse(item["Kms"].ToString(), out temp2);
                    km += temp2;
                    double temp3 = 0.00;
                    double.TryParse(item["Tollgate"].ToString(), out temp3);
                    toll += temp3;
                    double dieselval = 0.0;
                    double.TryParse(item["Diesel"].ToString(), out dieselval);
                    tot_dieselval += dieselval;
                    double DieselCost = 0.00;
                    double.TryParse(item["Diesel Cost"].ToString(), out DieselCost);
                    tot_diesel += DieselCost;
                    double expenses = 0.00;
                    double.TryParse(item["Expenses"].ToString(), out expenses);
                    tot_exp += expenses;
                    double HourReading = 0.00;
                    double.TryParse(item["HourReading"].ToString(), out HourReading);
                    tot_HourReading += HourReading;
                    double load = 0.00;
                    double.TryParse(item["Load"].ToString(), out load);
                    tot_load += load;
                    double unload = 0.00;
                    double.TryParse(item["UnLoad"].ToString(), out unload);
                    tot_unload += unload;
                }
                Grandtotal["Location"] = "Grand Total";
                Grandtotal["Kms"] = km;
                Grandtotal["Total Amount"] = grdttl;
                Grandtotal["Tollgate"] = toll;
                Grandtotal["Diesel"] = tot_dieselval;
                Grandtotal["Diesel Cost"] = tot_diesel;
                Grandtotal["Expenses"] = tot_exp;
                Grandtotal["HourReading"] = tot_HourReading ;
                Grandtotal["Load"] = tot_load;
                Grandtotal["UnLoad"] = tot_unload;
                double sal = 0;
                sal = 613.767;
                double totdeduction = 0;
                totdeduction = toll + tot_diesel + tot_exp + sal;
                double totpl = 0;
                totpl = grdttl - totdeduction;
                totpl = Math.Round(totpl, 2);
                Grandtotal["P & L"] = totpl;
                exp.Rows.Add(Grandtotal);

                string title = "Puff & TankerReport Report From: " + fromdate.ToString() + "  To: " + todate.ToString();
                Session["title"] = title;
                //Session["filename"] = ddlvehicles.SelectedItem.Text;
                Session["xportdata"] = exp;
                dataGridView1.DataSource = exp;
                dataGridView1.DataBind();
            }
            else
            {
                lblmsg.Text = "No data were found";
                dataGridView1.DataSource = Report;
                dataGridView1.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.ToString();
            dataGridView1.DataSource = Report;
            dataGridView1.DataBind();
        }
    }
    protected void grdReports_RowDataBound(object sender, GridViewRowEventArgs e)
    {
         if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //If Salary is less than 10000 than set the row Background Color to Cyan  
            string val = e.Row.Cells[11].Text;
            if (e.Row.Cells[11].Text == "0")
            {
                e.Row.Cells[11].ForeColor = System.Drawing.Color.White;
            }
            if (e.Row.Cells[12].Text == "0")
            {
                e.Row.Cells[12].ForeColor = System.Drawing.Color.White;
            }
        }  
    }
 }
