using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MySql.Data.MySqlClient;

public partial class PuffTransportationReport : System.Web.UI.Page
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
                    FillRouteName();
                    dtp_FromDate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                    dtp_Todate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                    lblAddress.Text = Session["Address"].ToString();
                    lblTitle.Text = Session["TitleName"].ToString();
                }
            }
        }
    }
    void FillRouteName()
    {
        try
        {
            vdm = new VehicleDBMgr();
            if (Session["BranchType"].ToString() == "Plant")
            {
                DataTable dtBranch = new DataTable();
                dtBranch.Columns.Add("BranchName");
                dtBranch.Columns.Add("sno");
                cmd = new MySqlCommand("SELECT branch_info.branchname,branch_info.brnch_sno FROM branch_info INNER JOIN branchmappingtable ON branch_info.brnch_sno = branchmappingtable.subbranch WHERE (branchmappingtable.superbranch = @SuperBranch)");
                cmd.Parameters.Add("@SuperBranch", Session["Branch_ID"]);
                DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow dr in dtRoutedata.Rows)
                {
                    DataRow newrow = dtBranch.NewRow();
                    newrow["BranchName"] = dr["branchname"].ToString();
                    newrow["sno"] = dr["brnch_sno"].ToString();
                    dtBranch.Rows.Add(newrow);
                }
                cmd = new MySqlCommand("SELECT branchname, brnch_sno FROM branch_info WHERE (brnch_sno = @BranchID)");
                cmd.Parameters.Add("@BranchID", Session["Branch_ID"]);
                DataTable dtPlant = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow dr in dtPlant.Rows)
                {
                    DataRow newrow = dtBranch.NewRow();
                    newrow["BranchName"] = dr["branchname"].ToString();
                    newrow["sno"] = dr["brnch_sno"].ToString();
                    dtBranch.Rows.Add(newrow);
                }
                ddlSalesOffice.DataSource = dtBranch;
                ddlSalesOffice.DataTextField = "BranchName";
                ddlSalesOffice.DataValueField = "sno";
                ddlSalesOffice.DataBind();
            }
            else
            {
                cmd = new MySqlCommand("SELECT branchname, brnch_sno FROM branch_info WHERE (brnch_sno = @BranchID)");
                cmd.Parameters.Add("@BranchID", Session["Branch_ID"]);
                DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
                ddlSalesOffice.DataSource = dtRoutedata;
                ddlSalesOffice.DataTextField = "branchname";
                ddlSalesOffice.DataValueField = "brnch_sno";
                ddlSalesOffice.DataBind();
            }
        }
        catch
        {
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

        if (ddlType.SelectedValue == "Puff")
        {
            hideVehicles.Visible = true;
            cmd = new MySqlCommand("SELECT minimasters.mm_name, vehicel_master.registration_no, vehicel_master.vm_sno FROM vehicel_master INNER JOIN minimasters ON vehicel_master.vhtype_refno = minimasters.sno WHERE(minimasters.mm_name = @Tanker) AND (vehicel_master.branch_id = @BranchID)");
            cmd.Parameters.Add("@BranchID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.Add("@Tanker", "Puff");
            DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
            ddlvehicles.DataSource = dttrips;
            ddlvehicles.DataTextField = "registration_no";
            ddlvehicles.DataValueField = "vm_sno";
            ddlvehicles.DataBind();
        }
        if (ddlType.SelectedValue == "Route")
        {
            hideVehicles.Visible = true;
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
            cmd = new MySqlCommand("SELECT tripdata.routeid FROM tripdata INNER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno WHERE(tripdata.tripdate BETWEEN @d1 AND @d2) AND (vehicel_master.vhtype_refno = 7) AND (tripdata.userid = @BranchID)");
            cmd.Parameters.Add("@BranchID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.Add("@d1", GetLowDate(fromdate).AddDays(-5));
            cmd.Parameters.Add("@d2", GetHighDate(todate).AddDays(-5));
            DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
            ddlvehicles.DataSource = dttrips;
            ddlvehicles.DataTextField = "routeid";
            ddlvehicles.DataValueField = "routeid";
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
            //SalesDBManager SalesDB = new SalesDBManager();
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
            lblFromDate.Text = fromdate.ToString("dd/MM/yyyy");
            lbltodate.Text = todate.ToString("dd/MM/yyyy");
            cmd = new MySqlCommand("SELECT brnch_sno, salesbranchid FROM  branch_info WHERE  (brnch_sno = @BranchID)");
            cmd.Parameters.Add("@BranchID", ddlSalesOffice.SelectedValue);
            DataTable dtBranch= vdm.SelectQuery(cmd).Tables[0];
            string SalesBranchID = "0";
            if (dtBranch.Rows.Count > 0)
            {
                SalesBranchID = dtBranch.Rows[0]["salesbranchid"].ToString();
            }
            if (ddlSalesOffice.SelectedValue == "1" || ddlSalesOffice.SelectedValue == "7")
            {
                string PlantID = ddlSalesOffice.SelectedValue;
                if (ddlType.SelectedValue == "All")
                {
                    Report.Columns.Add("Sno");
                    Report.Columns.Add("VehicleNo");
                    Report.Columns.Add("Date");
                    //Report.Columns.Add("DC No");
                    Report.Columns.Add("Tripsheet No");
                    Report.Columns.Add("Route Name");
                    Report.Columns.Add("Qty").DataType = typeof(Double);
                    Report.Columns.Add("Kms").DataType = typeof(Double);
                    Report.Columns.Add("Mileage");
                    Report.Columns.Add("Act Mileage");
                    Report.Columns.Add("Puff Rent").DataType = typeof(Double);
                    Report.Columns.Add("Diesel Qty").DataType = typeof(Double);
                    Report.Columns.Add("Diesel Cost").DataType = typeof(Double);
                    Report.Columns.Add("Toll Gate").DataType = typeof(Double);
                    Report.Columns.Add("Total Transport").DataType = typeof(Double);
                    Report.Columns.Add("Cost Per Ltr");
                    TimeSpan dateSpan = todate.Subtract(fromdate);
                    int NoOfdays = dateSpan.Days;
                    NoOfdays = NoOfdays + 1;

                    for (int k = 0; k < NoOfdays; k++)
                    {
                        DateTime frmdate = fromdate.AddDays(k);
                        todate = frmdate;
                        //cmd = new MySqlCommand("SELECT derivedtbl_1.tripsheetno AS TripSheet,derivedtbl_1.registration_no AS VehicleNo,derivedtbl_1.TripKMS, derivedtbl_1.loadtype AS LoadType, derivedtbl_1.routeid AS RouteName, derivedtbl_1.DieselCost,  derivedtbl_1.endfuelvalue AS InsideFuel,SUM(triplogs.tollgateamnt) AS tollgateamnt, SUM(triplogs.fuel) AS OutsideFuel,derivedtbl_1.Qty,derivedtbl_1.Rent,(derivedtbl_1.endfuelvalue + SUM(triplogs.fuel)) as Diesel, IFNULL(ROUND(derivedtbl_1.TripKMS / (derivedtbl_1.endfuelvalue + SUM(triplogs.fuel)), 2),0) AS TodayMileage ,derivedtbl_1.tripdate FROM (SELECT tripdata.tripsheetno, tripdata.tripdate, tripdata.enddate, vehicel_master.registration_no,tripdata.endodometerreading - tripdata.vehiclestartreading AS TripKMS, tripdata.loadtype,tripdata.QTy, tripdata.routeid,tripdata.Rent, tripdata.sno, tripdata.endfuelvalue,tripdata.DieselCost  FROM tripdata INNER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno INNER JOIN minimasters ON vehicel_master.vhtype_refno = minimasters.sno WHERE (tripdata.tripdate BETWEEN @d1 AND @d2) AND (tripdata.userid = @BranchID) AND (tripdata.status = 'C') and (vehicel_master.vhtype_refno=7)) derivedtbl_1 INNER JOIN triplogs ON derivedtbl_1.sno = triplogs.tripsno AND triplogs.fuel_type <> 'OWN' GROUP BY derivedtbl_1.tripsheetno");
                        cmd = new MySqlCommand("SELECT        derivedtbl_1.tripsheetno AS TripSheet, derivedtbl_1.registration_no AS VehicleNo,derivedtbl_1.act_mileage, derivedtbl_1.TripKMS, derivedtbl_1.loadtype AS LoadType, derivedtbl_1.routeid AS RouteName, derivedtbl_1.DieselCost, derivedtbl_1.endfuelvalue AS InsideFuel, SUM(triplogs.tollgateamnt) AS tollgateamnt, SUM(triplogs.fuel) AS OutsideFuel, derivedtbl_1.qty, derivedtbl_1.Rent, derivedtbl_1.endfuelvalue + SUM(triplogs.fuel) AS Diesel, IFNULL(ROUND(derivedtbl_1.TripKMS / (derivedtbl_1.endfuelvalue + SUM(triplogs.fuel)), 2), 0) AS TodayMileage, derivedtbl_1.tripdate, derivedtbl_1.VehicleType, derivedtbl_1.Vehiclemake, derivedtbl_1.Capacity FROM (SELECT tripdata.tripsheetno, tripdata.tripdate, tripdata.enddate, vehicel_master.registration_no, tripdata.endodometerreading - tripdata.vehiclestartreading AS TripKMS, tripdata.loadtype, tripdata.qty, tripdata.routeid, tripdata.Rent, tripdata.sno, tripdata.endfuelvalue, tripdata.DieselCost, minimasters.mm_code AS VehicleType, minimasters_1.mm_code AS Vehiclemake, vehicel_master.Capacity,vehicel_master.act_mileage FROM tripdata INNER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno INNER JOIN minimasters ON vehicel_master.vhtype_refno = minimasters.sno INNER JOIN minimasters minimasters_1 ON vehicel_master.vhmake_refno = minimasters_1.sno WHERE (tripdata.tripdate BETWEEN @d1 AND @d2) AND (tripdata.userid = @BranchID) AND (tripdata.status = 'C') AND (vehicel_master.vhtype_refno = 7)) derivedtbl_1 INNER JOIN triplogs ON derivedtbl_1.sno = triplogs.tripsno AND triplogs.fuel_type <> 'OWN' GROUP BY derivedtbl_1.tripsheetno order by Vehiclemake");
                        cmd.Parameters.Add("@d1", GetLowDate(frmdate));
                        cmd.Parameters.Add("@d2", GetHighDate(todate));
                        cmd.Parameters.Add("@BranchID", PlantID);
                        cmd.Parameters.Add("@VehType", ddlType.SelectedItem.Text);
                        DataTable dtTrips = vdm.SelectQuery(cmd).Tables[0];
                        string date = todate.ToString("dd/MMM/yyyy");
                        DataRow[] result = Report.Select("Date='" + date + "'");
                        double TotQty = 0;
                        double Totkms = 0;
                        double TotRent = 0;
                        double Totdieselcost = 0;
                        double TotTransport = 0;
                        int i = 1;
                        foreach (DataRow dr in dtTrips.Rows)
                        {
                            DataRow newrow = Report.NewRow();
                            newrow["Sno"] = i++.ToString();
                            newrow["VehicleNo"] = dr["VehicleNo"].ToString() + " /" + dr["Vehiclemake"].ToString() + " /" + dr["Capacity"].ToString();
                            DateTime Assidate = Convert.ToDateTime(dr["tripdate"].ToString());
                            string Adate = Assidate.ToString("dd/MMM/yyyy HH:MM");
                            newrow["Date"] = Adate;
                            newrow["Route Name"] = dr["RouteName"].ToString();
                            newrow["Qty"] = dr["Qty"].ToString();
                            double Kms = 0;
                            double.TryParse(dr["TripKMS"].ToString(), out Kms);
                            newrow["Kms"] = Kms.ToString();
                            newrow["Tripsheet No"] = dr["TripSheet"].ToString();
                            double Rent = 0;
                            double.TryParse(dr["Rent"].ToString(), out Rent);
                            newrow["Puff Rent"] = Rent.ToString();
                            double TodayMileage = 0;
                            double.TryParse(dr["TodayMileage"].ToString(), out TodayMileage);
                            newrow["Diesel Qty"] = dr["Insidefuel"].ToString();
                            double Cost = 0;
                            double.TryParse(dr["DieselCost"].ToString(), out Cost);
                            double DieselCost = 0;
                            double tollgate = 0;
                            double.TryParse(dr["tollgateamnt"].ToString(), out tollgate);
                            newrow["Toll Gate"] = tollgate.ToString();
                            if (TodayMileage != 0)
                            {
                                DieselCost = (Kms / TodayMileage) * Cost;
                                DieselCost = Math.Round(DieselCost, 2);
                                newrow["Mileage"] = TodayMileage.ToString();
                                newrow["Act Mileage"] = dr["act_mileage"].ToString();
                                newrow["Diesel Cost"] = DieselCost.ToString();
                                double Transport = 0;
                                Transport = Rent + DieselCost;
                                Transport = Transport + tollgate;
                                Transport = Math.Round(Transport, 2);
                                newrow["Total Transport"] = Transport.ToString();
                                double Qty = 0;
                                double.TryParse(dr["Qty"].ToString(), out Qty);
                                TotQty += Qty;
                                double CostPerltr = 0;
                                CostPerltr = Transport / Qty;
                                CostPerltr = Math.Round(CostPerltr, 2);
                                newrow["Cost Per Ltr"] = CostPerltr.ToString();
                            }
                            else
                            {
                                newrow["Mileage"] = "0";
                                newrow["Diesel Cost"] = "0";
                                newrow["Total Transport"] = "0";
                                newrow["Cost Per Ltr"] = "0";
                            }
                            Report.Rows.Add(newrow);
                        }
                        DataRow newvartical = Report.NewRow();
                        newvartical["Route Name"] = "";
                        Report.Rows.Add(newvartical);
                    }
                    DataRow newvartical2 = Report.NewRow();
                    newvartical2["Route Name"] = "Total";
                    double val = 0.0;
                    foreach (DataColumn dc in Report.Columns)
                    {
                        if (dc.DataType == typeof(Double))
                        {
                            val = 0.0;
                            double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
                            newvartical2[dc.ToString()] = val;
                        }
                    }
                    Report.Rows.Add(newvartical2);
                    grdReports.DataSource = Report;
                    grdReports.DataBind();
                    Session["xportdata"] = Report;
                }
                if (ddlType.SelectedValue == "Puff")
                {
                    lblVehicleNo.Text = ddlvehicles.SelectedItem.Text;
                    Report.Columns.Add("Sno");
                    Report.Columns.Add("Date");
                    //Report.Columns.Add("DC No");
                    Report.Columns.Add("Tripsheet No");
                    Report.Columns.Add("Route Name");
                    Report.Columns.Add("Qty").DataType = typeof(Double);
                    Report.Columns.Add("Kms").DataType = typeof(Double);
                    Report.Columns.Add("Mileage");
                    Report.Columns.Add("Act Mileage");
                    Report.Columns.Add("Puff Rent").DataType = typeof(Double);
                    Report.Columns.Add("Diesel Qty").DataType = typeof(Double);
                    Report.Columns.Add("Diesel Cost").DataType = typeof(Double);
                    Report.Columns.Add("Toll Gate").DataType = typeof(Double);
                    Report.Columns.Add("Total Transport").DataType = typeof(Double);
                    Report.Columns.Add("Cost Per Ltr");

                    cmd = new MySqlCommand("SELECT derivedtbl_1.tripsheetno AS TripSheet,derivedtbl_1.registration_no AS VehicleNo,derivedtbl_1.act_mileage,derivedtbl_1.TripKMS,derivedtbl_1.tripdate, derivedtbl_1.loadtype AS LoadType, derivedtbl_1.routeid AS RouteName, derivedtbl_1.DieselCost,  derivedtbl_1.endfuelvalue AS InsideFuel,SUM(triplogs.tollgateamnt) AS tollgateamnt, SUM(triplogs.fuel) AS OutsideFuel,derivedtbl_1.Qty,derivedtbl_1.Rent,(derivedtbl_1.endfuelvalue + SUM(triplogs.fuel)) as Diesel, IFNULL(ROUND(derivedtbl_1.TripKMS / (derivedtbl_1.endfuelvalue + SUM(triplogs.fuel)), 2),0) AS TodayMileage,derivedtbl_1.tripdate FROM (SELECT tripdata.tripsheetno,tripdata.tripdate , tripdata.enddate,vehicel_master.act_mileage, vehicel_master.registration_no,tripdata.endodometerreading - tripdata.vehiclestartreading AS TripKMS, tripdata.loadtype,tripdata.QTy, tripdata.routeid,tripdata.Rent, tripdata.sno, tripdata.endfuelvalue,tripdata.DieselCost  FROM tripdata INNER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno INNER JOIN minimasters ON vehicel_master.vhtype_refno = minimasters.sno WHERE (tripdata.tripdate BETWEEN @d1 AND @d2) AND (tripdata.userid = @BranchID) AND (tripdata.status = 'C') group by tripdata.sno order by tripdata.sno) derivedtbl_1 INNER JOIN triplogs ON derivedtbl_1.sno = triplogs.tripsno AND triplogs.fuel_type <> 'OWN' AND derivedtbl_1.registration_no=@VehicleNo Group by derivedtbl_1.tripsheetno");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@BranchID", PlantID);
                    cmd.Parameters.Add("@VehicleNo", ddlvehicles.SelectedItem.Text); ;
                    DataTable dtTrips = vdm.SelectQuery(cmd).Tables[0];
                    int i = 1;
                    foreach (DataRow dr in dtTrips.Rows)
                    {
                        DataRow newrow = Report.NewRow();
                        newrow["Sno"] = i++.ToString();
                        DateTime Assidate = Convert.ToDateTime(dr["tripdate"].ToString());
                        string Adate = Assidate.ToString("dd/MMM/yyyy");
                        newrow["Date"] = Adate;
                        newrow["Route Name"] = dr["RouteName"].ToString();
                        newrow["Qty"] = dr["Qty"].ToString();
                        double Kms = 0;
                        double.TryParse(dr["TripKMS"].ToString(), out Kms);
                        newrow["Kms"] = Kms.ToString();
                        newrow["Tripsheet No"] = dr["TripSheet"].ToString();
                        double Rent = 0;
                        double.TryParse(dr["Rent"].ToString(), out Rent);
                        newrow["Puff Rent"] = Rent.ToString();
                        double TodayMileage = 0;
                        double.TryParse(dr["TodayMileage"].ToString(), out TodayMileage);
                        newrow["Diesel Qty"] = dr["Insidefuel"].ToString();
                        double Cost = 0;
                        double.TryParse(dr["DieselCost"].ToString(), out Cost);
                        double DieselCost = 0;
                        double tollgate = 0;
                        double.TryParse(dr["tollgateamnt"].ToString(), out tollgate);
                        newrow["Toll Gate"] = tollgate.ToString();
                        if (TodayMileage != 0)
                        {
                            DieselCost = (Kms / TodayMileage) * Cost;
                            DieselCost = Math.Round(DieselCost, 2);
                            newrow["Mileage"] = TodayMileage.ToString();
                            newrow["Act Mileage"] = dr["act_mileage"].ToString();
                            newrow["Diesel Cost"] = DieselCost.ToString();
                            double Transport = 0;
                            Transport = Rent + DieselCost;
                            Transport = Transport + tollgate;
                            Transport = Math.Round(Transport, 2);
                            newrow["Total Transport"] = Transport.ToString();
                            double Qty = 0;
                            double.TryParse(dr["Qty"].ToString(), out Qty);
                            //TotQty += Qty;
                            double CostPerltr = 0;
                            CostPerltr = Transport / Qty;
                            CostPerltr = Math.Round(CostPerltr, 2);
                            newrow["Cost Per Ltr"] = CostPerltr.ToString();
                        }
                        else
                        {
                            newrow["Mileage"] = "0";
                            newrow["Diesel Cost"] = "0";
                            newrow["Total Transport"] = "0";
                            newrow["Cost Per Ltr"] = "0";
                        }
                        Report.Rows.Add(newrow);
                    }
                    DataRow newvartical = Report.NewRow();
                    newvartical["Route Name"] = "Total";
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
                    grdReports.DataSource = Report;
                    grdReports.DataBind();
                    Session["xportdata"] = Report;

                }
                if (ddlType.SelectedValue == "Route")
                {
                    lblVehicleNo.Text = ddlvehicles.SelectedItem.Text;
                    Report.Columns.Add("Sno");
                    Report.Columns.Add("Date");
                    //Report.Columns.Add("DC No");
                    Report.Columns.Add("Tripsheet No");
                    Report.Columns.Add("Vehicle No");
                    Report.Columns.Add("Qty").DataType = typeof(Double);
                    Report.Columns.Add("Kms").DataType = typeof(Double);
                    Report.Columns.Add("Mileage");
                    Report.Columns.Add("Act Mileage");
                    Report.Columns.Add("Puff Rent").DataType = typeof(Double);
                    Report.Columns.Add("Diesel Qty").DataType = typeof(Double);
                    Report.Columns.Add("Diesel Cost").DataType = typeof(Double);
                    Report.Columns.Add("Toll Gate").DataType = typeof(Double);
                    Report.Columns.Add("Total Transport").DataType = typeof(Double);
                    Report.Columns.Add("Cost Per Ltr");
                    cmd = new MySqlCommand("SELECT derivedtbl_1.tripsheetno AS TripSheet,derivedtbl_1.registration_no AS VehicleNo,derivedtbl_1.act_mileage,derivedtbl_1.TripKMS,derivedtbl_1.tripdate, derivedtbl_1.loadtype AS LoadType, derivedtbl_1.routeid AS RouteName, derivedtbl_1.DieselCost,  derivedtbl_1.endfuelvalue AS InsideFuel,SUM(triplogs.tollgateamnt) AS tollgateamnt, SUM(triplogs.fuel) AS OutsideFuel,derivedtbl_1.Qty,derivedtbl_1.Rent,(derivedtbl_1.endfuelvalue + SUM(triplogs.fuel)) as Diesel, IFNULL(ROUND(derivedtbl_1.TripKMS / (derivedtbl_1.endfuelvalue + SUM(triplogs.fuel)), 2),0) AS TodayMileage,derivedtbl_1.tripdate,derivedtbl_1.routeid FROM (SELECT tripdata.tripsheetno,tripdata.tripdate , tripdata.enddate,vehicel_master.act_mileage, vehicel_master.registration_no,tripdata.endodometerreading - tripdata.vehiclestartreading AS TripKMS, tripdata.loadtype,tripdata.QTy, tripdata.routeid,tripdata.Rent, tripdata.sno, tripdata.endfuelvalue,tripdata.DieselCost  FROM tripdata INNER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno INNER JOIN minimasters ON vehicel_master.vhtype_refno = minimasters.sno WHERE (tripdata.tripdate BETWEEN @d1 AND @d2) AND (tripdata.userid = @BranchID) AND (tripdata.status = 'C') group by tripdata.sno order by tripdata.sno) derivedtbl_1 INNER JOIN triplogs ON derivedtbl_1.sno = triplogs.tripsno AND triplogs.fuel_type <> 'OWN' AND derivedtbl_1.routeid=@routeid Group by derivedtbl_1.tripsheetno");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@BranchID", PlantID);
                    cmd.Parameters.Add("@routeid", ddlvehicles.SelectedItem.Text); ;
                    DataTable dtTrips = vdm.SelectQuery(cmd).Tables[0];
                    int i = 1;
                    foreach (DataRow dr in dtTrips.Rows)
                    {
                        DataRow newrow = Report.NewRow();
                        newrow["Sno"] = i++.ToString();
                        DateTime Assidate = Convert.ToDateTime(dr["tripdate"].ToString());
                        string Adate = Assidate.ToString("dd/MMM/yyyy");
                        newrow["Date"] = Adate;
                        newrow["Vehicle No"] = dr["VehicleNo"].ToString();
                        newrow["Qty"] = dr["Qty"].ToString();
                        double Kms = 0;
                        double.TryParse(dr["TripKMS"].ToString(), out Kms);
                        newrow["Kms"] = Kms.ToString();
                        newrow["Tripsheet No"] = dr["TripSheet"].ToString();
                        double Rent = 0;
                        double.TryParse(dr["Rent"].ToString(), out Rent);
                        newrow["Puff Rent"] = Rent.ToString();
                        double TodayMileage = 0;
                        double.TryParse(dr["TodayMileage"].ToString(), out TodayMileage);
                        newrow["Diesel Qty"] = dr["Insidefuel"].ToString();
                        double Cost = 0;
                        double.TryParse(dr["DieselCost"].ToString(), out Cost);
                        double DieselCost = 0;
                        double tollgate = 0;
                        double.TryParse(dr["tollgateamnt"].ToString(), out tollgate);
                        newrow["Toll Gate"] = tollgate.ToString();
                        if (TodayMileage != 0)
                        {
                            DieselCost = (Kms / TodayMileage) * Cost;
                            DieselCost = Math.Round(DieselCost, 2);
                            newrow["Mileage"] = TodayMileage.ToString();
                            newrow["Act Mileage"] = dr["act_mileage"].ToString();
                            newrow["Diesel Cost"] = DieselCost.ToString();
                            double Transport = 0;
                            Transport = Rent + DieselCost;
                            Transport = Transport + tollgate;
                            Transport = Math.Round(Transport, 2);
                            newrow["Total Transport"] = Transport.ToString();
                            double Qty = 0;
                            double.TryParse(dr["Qty"].ToString(), out Qty);
                            double CostPerltr = 0;
                            CostPerltr = Transport / Qty;
                            CostPerltr = Math.Round(CostPerltr, 2);
                            newrow["Cost Per Ltr"] = CostPerltr.ToString();
                        }
                        else
                        {
                            newrow["Mileage"] = "0";
                            newrow["Diesel Cost"] = "0";
                            newrow["Total Transport"] = "0";
                            newrow["Cost Per Ltr"] = "0";
                        }
                        Report.Rows.Add(newrow);
                    }
                    DataRow newvartical = Report.NewRow();
                    newvartical["Vehicle No"] = "Total";
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
                    grdReports.DataSource = Report;
                    grdReports.DataBind();
                    Session["xportdata"] = Report;

                }
                hidepanel.Visible = true;
            }
            else
            {
                if (ddlType.SelectedValue == "All")
                {
                    lblmsg.Text = "";

                    cmd = new MySqlCommand("SELECT vehicel_master.registration_no,vehicel_master.Capacity, tripdata.qty, tripdata.routeid, DATE_FORMAT(tripdata.tripdate, '%d %b %y') AS AssignDate, tripdata.perkmcharge, tripdata.tripsheetno, tripdata.endodometerreading - tripdata.vehiclestartreading AS TripKMS, tripdata.Rent, vehicel_master.branch_id, SUM(triplogs.tollgateamnt) AS tollgate FROM tripdata INNER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno LEFT OUTER JOIN triplogs ON tripdata.sno = triplogs.tripsno WHERE (tripdata.enddate BETWEEN @d1 AND @d2) AND (vehicel_master.branch_id = @BranchID) GROUP BY tripdata.tripsheetno Order by tripdata.routeid,tripdata.tripsno,DATE(tripdata.tripdate)");
                    //cmd = new MySqlCommand("SELECT derivedtbl_1.tripsheetno AS TripSheet,derivedtbl_1.registration_no AS VehicleNo,derivedtbl_1.TripKMS, derivedtbl_1.loadtype AS LoadType, derivedtbl_1.routeid AS RouteName, derivedtbl_1.DieselCost,  derivedtbl_1.endfuelvalue AS InsideFuel, SUM(triplogs.fuel) AS OutsideFuel,derivedtbl_1.Qty,derivedtbl_1.Rent,(derivedtbl_1.endfuelvalue + SUM(triplogs.fuel)) as Diesel, IFNULL(ROUND(derivedtbl_1.TripKMS / (derivedtbl_1.endfuelvalue + SUM(triplogs.fuel)), 2),0) AS TodayMileage FROM (SELECT tripdata.tripsheetno, tripdata.tripdate, tripdata.enddate, vehicel_master.registration_no,tripdata.endodometerreading - tripdata.vehiclestartreading AS TripKMS, tripdata.loadtype,tripdata.QTy, tripdata.routeid,tripdata.Rent, tripdata.sno, tripdata.endfuelvalue,tripdata.DieselCost  FROM tripdata INNER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno INNER JOIN minimasters ON vehicel_master.vhtype_refno = minimasters.sno WHERE (tripdata.tripdate BETWEEN @d1 AND @d2) AND (tripdata.userid = @BranchID) AND (tripdata.status = 'C')) derivedtbl_1 INNER JOIN triplogs ON derivedtbl_1.sno = triplogs.tripsno AND triplogs.fuel_type <> 'OWN' GROUP BY derivedtbl_1.tripsheetno");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@BranchID", ddlSalesOffice.SelectedValue);
                    cmd.Parameters.Add("@VehType", ddlType.SelectedItem.Text);
                    DataTable dtTrips = vdm.SelectQuery(cmd).Tables[0];
                    int i = 1;
                    DataTable trips = new DataView(dtTrips).ToTable(true, "registration_no");
                    if (dtTrips.Rows.Count > 0)
                    {
                        DataTable exp = new DataTable();
                        exp.Columns.Add("Sno");
                        exp.Columns.Add("VehicleNo");
                        exp.Columns.Add("Capacity");
                        exp.Columns.Add("Date");
                        exp.Columns.Add("Tripsheet No");
                        exp.Columns.Add("RouteName");
                        exp.Columns.Add("Qty").DataType = typeof(Double);
                        exp.Columns.Add("Kms").DataType = typeof(Double);
                        exp.Columns.Add("Puff Rent").DataType = typeof(Double);
                        exp.Columns.Add("Extra Charges").DataType = typeof(Double);
                        exp.Columns.Add("Toll Gate").DataType = typeof(Double);
                        exp.Columns.Add("Total Transport").DataType = typeof(Double);
                        exp.Columns.Add("Cost Per Ltr");
                        foreach (DataRow dr2 in trips.Rows)
                        {
                            Report = new DataTable();
                            Report.Columns.Add("Sno");
                            Report.Columns.Add("VehicleNo");
                            Report.Columns.Add("Capacity");
                            Report.Columns.Add("Date");
                            Report.Columns.Add("Tripsheet No");
                            Report.Columns.Add("RouteName");
                            Report.Columns.Add("Qty").DataType = typeof(Double);
                            Report.Columns.Add("Kms").DataType = typeof(Double);
                            Report.Columns.Add("Puff Rent").DataType = typeof(Double);
                            Report.Columns.Add("Extra Charges").DataType = typeof(Double);
                            Report.Columns.Add("Toll Gate").DataType = typeof(Double);
                            Report.Columns.Add("Total Transport").DataType = typeof(Double);
                            Report.Columns.Add("Cost Per Ltr");
                            DataRow[] newdatarow = dtTrips.Select("registration_no='" + dr2["registration_no"].ToString() + "'");
                            foreach (DataRow dr in newdatarow)
                            {
                                DataRow newrow = Report.NewRow();
                                newrow["Sno"] = i++.ToString();
                                newrow["VehicleNo"] = dr["registration_no"].ToString();
                                newrow["Capacity"] = dr["Capacity"].ToString();
                                newrow["Date"] = dr["AssignDate"].ToString();
                                newrow["RouteName"] = dr["routeid"].ToString();
                                newrow["Qty"] = dr["Qty"].ToString();
                                double Kms = 0;
                                double.TryParse(dr["TripKMS"].ToString(), out Kms);
                                newrow["Kms"] = Kms.ToString();
                                newrow["Tripsheet No"] = dr["tripsheetno"].ToString();
                                double Rent = 0;
                                double.TryParse(dr["Rent"].ToString(), out Rent);
                                newrow["Puff Rent"] = Rent.ToString();
                                double extracharge = 0;
                                double perkmcharge = 0;
                                double.TryParse(dr["perkmcharge"].ToString(), out perkmcharge);
                                if (Kms > 80)
                                {
                                    extracharge = Kms - 80;
                                    if (extracharge > 0)
                                    {
                                        extracharge = extracharge * perkmcharge;
                                        extracharge = Math.Round(extracharge, 2);
                                    }
                                }
                                newrow["Extra Charges"] = extracharge.ToString();
                                double TotTransport = 0;
                                TotTransport = Rent + extracharge;
                                double tollgate = 0;
                                double.TryParse(dr["tollgate"].ToString(), out tollgate);
                                newrow["Toll Gate"] = tollgate.ToString();
                                TotTransport = TotTransport + tollgate;
                                TotTransport = Math.Round(TotTransport, 2);
                                newrow["Total Transport"] = TotTransport.ToString();
                                double Qty = 0;
                                double.TryParse(dr["Qty"].ToString(), out Qty);
                                double CostPerltr = 0;
                                CostPerltr = TotTransport / Qty;
                                CostPerltr = Math.Round(CostPerltr, 2);
                                newrow["Cost Per Ltr"] = CostPerltr.ToString();
                                Report.Rows.Add(newrow);

                            }
                            DataRow newvartical = Report.NewRow();
                            newvartical["RouteName"] = "Total";
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
                        DataRow[] ttl = exp.Select("RouteName='Total'");
                        double tolltl = 0.00;
                        double qtytl = 0.00;
                        double kmstl = 0.00;
                        double pufftl = 0.00;
                        double extrachargetl = 0.00;
                        double TotalTransporttl = 0.00;
                        foreach (DataRow item in ttl)
                        {
                            double temp = 0.00;
                            double.TryParse(item["Toll Gate"].ToString(), out temp);
                            tolltl += temp;
                            double Qty = 0.00;
                            double.TryParse(item["Qty"].ToString(), out Qty);
                            qtytl += Qty;
                            double kms = 0.00;
                            double.TryParse(item["Kms"].ToString(), out kms);
                            kmstl += kms;
                            double puffrent = 0.00;
                            double.TryParse(item["Puff Rent"].ToString(), out puffrent);
                            pufftl += puffrent;
                            double ExtraCharges = 0.00;
                            double.TryParse(item["Extra Charges"].ToString(), out ExtraCharges);
                            extrachargetl += ExtraCharges;
                            double TotalTransport = 0.00;
                            double.TryParse(item["Total Transport"].ToString(), out TotalTransport);
                            TotalTransporttl += TotalTransport;
                        }
                        Grandtotal["RouteName"] = "Grand Total";
                        Grandtotal["Toll Gate"] = tolltl;
                        Grandtotal["Qty"] = qtytl;
                        Grandtotal["Kms"] = kmstl;
                        Grandtotal["Puff Rent"] = pufftl;
                        Grandtotal["Extra Charges"] = extrachargetl;
                        Grandtotal["Total Transport"] = TotalTransporttl;
                        exp.Rows.Add(Grandtotal);
                        grdReports.DataSource = exp;
                        grdReports.DataBind();
                        Session["xportdata"] = exp;
                    }
                }
                if (ddlType.SelectedValue == "Puff")
                {
                    lblVehicleNo.Text = ddlvehicles.SelectedItem.Text;
                    lblmsg.Text = "";
                    Report.Columns.Add("Sno");
                    Report.Columns.Add("VehicleNo");
                    Report.Columns.Add("Capacity");
                    Report.Columns.Add("Date");
                    Report.Columns.Add("Tripsheet No");
                    Report.Columns.Add("Route Name");
                    Report.Columns.Add("Qty").DataType = typeof(Double);
                    Report.Columns.Add("Kms").DataType = typeof(Double);
                    Report.Columns.Add("Puff Rent").DataType = typeof(Double);
                    Report.Columns.Add("Extra Charges").DataType = typeof(Double);
                    Report.Columns.Add("Toll Gate");
                    Report.Columns.Add("Total Transport").DataType = typeof(Double);
                    Report.Columns.Add("Cost Per Ltr");
                    cmd = new MySqlCommand("SELECT vehicel_master.registration_no,vehicel_master.Capacity, tripdata.qty, tripdata.routeid, DATE_FORMAT(tripdata.tripdate, '%d %b %y') AS AssignDate, tripdata.perkmcharge, tripdata.tripsheetno, tripdata.endodometerreading - tripdata.vehiclestartreading AS TripKMS, tripdata.Rent, vehicel_master.branch_id, SUM(triplogs.tollgateamnt) AS tollgate FROM tripdata INNER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno LEFT OUTER JOIN triplogs ON tripdata.sno = triplogs.tripsno WHERE (tripdata.enddate BETWEEN @d1 AND @d2) AND (vehicel_master.branch_id = @BranchID) and (vehicel_master.vm_sno=@Vehicleno) GROUP BY tripdata.tripsheetno");
                    //cmd = new MySqlCommand("SELECT derivedtbl_1.tripsheetno AS TripSheet,derivedtbl_1.registration_no AS VehicleNo,derivedtbl_1.TripKMS, derivedtbl_1.loadtype AS LoadType, derivedtbl_1.routeid AS RouteName, derivedtbl_1.DieselCost,  derivedtbl_1.endfuelvalue AS InsideFuel, SUM(triplogs.fuel) AS OutsideFuel,derivedtbl_1.Qty,derivedtbl_1.Rent,(derivedtbl_1.endfuelvalue + SUM(triplogs.fuel)) as Diesel, IFNULL(ROUND(derivedtbl_1.TripKMS / (derivedtbl_1.endfuelvalue + SUM(triplogs.fuel)), 2),0) AS TodayMileage FROM (SELECT tripdata.tripsheetno, tripdata.tripdate, tripdata.enddate, vehicel_master.registration_no,tripdata.endodometerreading - tripdata.vehiclestartreading AS TripKMS, tripdata.loadtype,tripdata.QTy, tripdata.routeid,tripdata.Rent, tripdata.sno, tripdata.endfuelvalue,tripdata.DieselCost  FROM tripdata INNER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno INNER JOIN minimasters ON vehicel_master.vhtype_refno = minimasters.sno WHERE (tripdata.tripdate BETWEEN @d1 AND @d2) AND (tripdata.userid = @BranchID) AND (tripdata.status = 'C')) derivedtbl_1 INNER JOIN triplogs ON derivedtbl_1.sno = triplogs.tripsno AND triplogs.fuel_type <> 'OWN' GROUP BY derivedtbl_1.tripsheetno");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@BranchID", ddlSalesOffice.SelectedValue);
                    cmd.Parameters.Add("@VehType", ddlType.SelectedItem.Text);
                    cmd.Parameters.Add("@Vehicleno", ddlvehicles.SelectedValue);
                    DataTable dtTrips = vdm.SelectQuery(cmd).Tables[0];
                    int i = 1;
                    foreach (DataRow dr in dtTrips.Rows)
                    {

                        DataRow newrow = Report.NewRow();
                        newrow["Sno"] = i++.ToString();
                        newrow["VehicleNo"] = dr["registration_no"].ToString();
                        newrow["Capacity"] = dr["Capacity"].ToString();
                        newrow["Date"] = dr["AssignDate"].ToString();
                        newrow["Route Name"] = dr["routeid"].ToString();
                        newrow["Qty"] = dr["Qty"].ToString();
                        double Kms = 0;
                        double.TryParse(dr["TripKMS"].ToString(), out Kms);
                        newrow["Kms"] = Kms.ToString();
                        newrow["Tripsheet No"] = dr["tripsheetno"].ToString();
                        double Rent = 0;
                        double.TryParse(dr["Rent"].ToString(), out Rent);
                        newrow["Puff Rent"] = Rent.ToString();
                        double extracharge = 0;
                        double perkmcharge = 0;
                        double.TryParse(dr["perkmcharge"].ToString(), out perkmcharge);
                        if (Kms > 80)
                        {
                            extracharge = Kms - 80;
                            if (extracharge > 0)
                            {
                                extracharge = extracharge * perkmcharge;
                                extracharge = Math.Round(extracharge, 2);
                            }
                        }
                        newrow["Extra Charges"] = extracharge.ToString();
                        double TotTransport = 0;
                        TotTransport = Rent + extracharge;
                        double tollgate = 0;
                        double.TryParse(dr["tollgate"].ToString(), out tollgate);
                        newrow["Toll Gate"] = tollgate.ToString();
                        TotTransport = TotTransport + tollgate;
                        TotTransport = Math.Round(TotTransport, 2);
                        newrow["Total Transport"] = TotTransport.ToString();
                        double Qty = 0;
                        double.TryParse(dr["Qty"].ToString(), out Qty);
                        double CostPerltr = 0;
                        CostPerltr = TotTransport / Qty;
                        CostPerltr = Math.Round(CostPerltr, 2);
                        newrow["Cost Per Ltr"] = CostPerltr.ToString();
                        Report.Rows.Add(newrow);
                    }
                    DataRow newvartical = Report.NewRow();
                    newvartical["Route Name"] = "Total";
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
                    grdReports.DataSource = Report;
                    grdReports.DataBind();
                    Session["xportdata"] = Report;
                }
                if (ddlType.SelectedValue == "Route")
                {
                    lblVehicleNo.Text = ddlvehicles.SelectedItem.Text;
                    lblmsg.Text = "";
                    Report.Columns.Add("Sno");
                    Report.Columns.Add("VehicleNo");
                    Report.Columns.Add("Capacity");
                    Report.Columns.Add("Date");
                    Report.Columns.Add("Tripsheet No");
                    Report.Columns.Add("Route Name");
                    Report.Columns.Add("Qty").DataType = typeof(Double);
                    Report.Columns.Add("Kms").DataType = typeof(Double);
                    Report.Columns.Add("Puff Rent").DataType = typeof(Double);
                    Report.Columns.Add("Extra Charges").DataType = typeof(Double);
                    Report.Columns.Add("Toll Gate");
                    Report.Columns.Add("Total Transport").DataType = typeof(Double);
                    Report.Columns.Add("Cost Per Ltr");
                    cmd = new MySqlCommand("SELECT vehicel_master.registration_no,vehicel_master.Capacity, tripdata.qty, tripdata.routeid, DATE_FORMAT(tripdata.tripdate, '%d %b %y') AS AssignDate, tripdata.perkmcharge, tripdata.tripsheetno, tripdata.endodometerreading - tripdata.vehiclestartreading AS TripKMS, tripdata.Rent, vehicel_master.branch_id, SUM(triplogs.tollgateamnt) AS tollgate FROM tripdata INNER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno LEFT OUTER JOIN triplogs ON tripdata.sno = triplogs.tripsno WHERE (tripdata.enddate BETWEEN @d1 AND @d2) AND (vehicel_master.branch_id = @BranchID) and (tripdata.routeid=@routeid) GROUP BY tripdata.tripsheetno");
                    //cmd = new MySqlCommand("SELECT derivedtbl_1.tripsheetno AS TripSheet,derivedtbl_1.registration_no AS VehicleNo,derivedtbl_1.TripKMS, derivedtbl_1.loadtype AS LoadType, derivedtbl_1.routeid AS RouteName, derivedtbl_1.DieselCost,  derivedtbl_1.endfuelvalue AS InsideFuel, SUM(triplogs.fuel) AS OutsideFuel,derivedtbl_1.Qty,derivedtbl_1.Rent,(derivedtbl_1.endfuelvalue + SUM(triplogs.fuel)) as Diesel, IFNULL(ROUND(derivedtbl_1.TripKMS / (derivedtbl_1.endfuelvalue + SUM(triplogs.fuel)), 2),0) AS TodayMileage FROM (SELECT tripdata.tripsheetno, tripdata.tripdate, tripdata.enddate, vehicel_master.registration_no,tripdata.endodometerreading - tripdata.vehiclestartreading AS TripKMS, tripdata.loadtype,tripdata.QTy, tripdata.routeid,tripdata.Rent, tripdata.sno, tripdata.endfuelvalue,tripdata.DieselCost  FROM tripdata INNER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno INNER JOIN minimasters ON vehicel_master.vhtype_refno = minimasters.sno WHERE (tripdata.tripdate BETWEEN @d1 AND @d2) AND (tripdata.userid = @BranchID) AND (tripdata.status = 'C')) derivedtbl_1 INNER JOIN triplogs ON derivedtbl_1.sno = triplogs.tripsno AND triplogs.fuel_type <> 'OWN' GROUP BY derivedtbl_1.tripsheetno");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@BranchID", ddlSalesOffice.SelectedValue);
                    cmd.Parameters.Add("@routeid", ddlvehicles.SelectedValue);
                    DataTable dtTrips = vdm.SelectQuery(cmd).Tables[0];
                    int i = 1;
                    foreach (DataRow dr in dtTrips.Rows)
                    {

                        DataRow newrow = Report.NewRow();
                        newrow["Sno"] = i++.ToString();
                        newrow["VehicleNo"] = dr["registration_no"].ToString();
                        newrow["Capacity"] = dr["Capacity"].ToString();
                        newrow["Date"] = dr["AssignDate"].ToString();
                        newrow["Route Name"] = dr["routeid"].ToString();
                        newrow["Qty"] = dr["Qty"].ToString();
                        double Kms = 0;
                        double.TryParse(dr["TripKMS"].ToString(), out Kms);
                        newrow["Kms"] = Kms.ToString();
                        newrow["Tripsheet No"] = dr["tripsheetno"].ToString();
                        double Rent = 0;
                        double.TryParse(dr["Rent"].ToString(), out Rent);
                        newrow["Puff Rent"] = Rent.ToString();
                        double extracharge = 0;
                        double perkmcharge = 0;
                        double.TryParse(dr["perkmcharge"].ToString(), out perkmcharge);
                        if (Kms > 80)
                        {
                            extracharge = Kms - 80;
                            if (extracharge > 0)
                            {
                                extracharge = extracharge * perkmcharge;
                                extracharge = Math.Round(extracharge, 2);
                            }
                        }
                        newrow["Extra Charges"] = extracharge.ToString();
                        double TotTransport = 0;
                        TotTransport = Rent + extracharge;
                        double tollgate = 0;
                        double.TryParse(dr["tollgate"].ToString(), out tollgate);
                        newrow["Toll Gate"] = tollgate.ToString();
                        TotTransport = TotTransport + tollgate;
                        TotTransport = Math.Round(TotTransport, 2);
                        newrow["Total Transport"] = TotTransport.ToString();
                        double Qty = 0;
                        double.TryParse(dr["Qty"].ToString(), out Qty);
                        double CostPerltr = 0;
                        CostPerltr = TotTransport / Qty;
                        CostPerltr = Math.Round(CostPerltr, 2);
                        newrow["Cost Per Ltr"] = CostPerltr.ToString();
                        Report.Rows.Add(newrow);
                    }
                    DataRow newvartical = Report.NewRow();
                    newvartical["Route Name"] = "Total";
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
                    grdReports.DataSource = Report;
                    grdReports.DataBind();
                    Session["xportdata"] = Report;
                }
                hidepanel.Visible = true;
            }
            string title = "Puffs Report";
            Session["title"] = title;
            Session["filename"] = "PuffsReport";
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
}