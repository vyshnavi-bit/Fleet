using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.IO;
using System.Data;

public partial class VehicleInvoice_Print : System.Web.UI.Page
{
    MySqlCommand cmd;
    string UserName = "";
    VehicleDBMgr vdm;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Branch_ID"] == null)
        {
            Response.Redirect("Login.aspx");
        }
        if (!this.IsPostBack)
        {
            if (!IsCallback)
            {
                dtp_FromDate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                dtp_Todate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                Load_VehicleType();
                Load_VehicleNOList();
                Load_BillingownersList();
            }
        }
    }
    private void Load_VehicleType()
    {
        try
        {
            vdm = new VehicleDBMgr();
            cmd = new MySqlCommand(" SELECT DISTINCT minimasters.mm_name, minimasters.sno FROM vehicel_master INNER JOIN minimasters ON vehicel_master.vhtype_refno = minimasters.sno INNER JOIN vmsc ON vehicel_master.vm_sno = vmsc.VID WHERE(vmsc.MID = @Mid) AND(minimasters.mm_type = @ddlVehicletype) ");
            cmd.Parameters.Add("@ddlVehicletype", "VehicleType");
            cmd.Parameters.Add("@Mid", Session["Mid"].ToString());
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            if (dt.Rows.Count > 0)
            {
                ddl_VehicleType.Items.Clear();

                ddl_VehicleType.DataSource = dt;
                ddl_VehicleType.DataValueField = "sno";
                ddl_VehicleType.DataTextField = "mm_name";
                ddl_VehicleType.DataBind();
                ddl_VehicleType.ClearSelection();
                ddl_VehicleType.Items.Insert(0, new ListItem { Value = "ALL", Text = "ALL", Selected = true });
                ddl_VehicleType.SelectedIndex = 0;

            }
            else
            {

            }
        }
        catch (Exception ex)
        {
        }
    }

    private void Load_VehicleNOList()
    {
        try
        {

            vdm = new VehicleDBMgr();
            cmd = new MySqlCommand("SELECT vmsc.ID, vmsc.MID, vmsc.VID, vmsc.VehicleRegistrationNo, vmsc.Capacity, vmsc.PerMonth, vmsc.PerDay, vmsc.PerKm, vmsc.PerKg, vmsc.PerTrip, vmsc.PerKmEmpty, vmsc.PresentDefaultMode FROM vehicel_master INNER JOIN minimasters ON vehicel_master.vhtype_refno = minimasters.sno INNER JOIN vmsc ON vehicel_master.vm_sno = vmsc.VID WHERE(minimasters.sno = @ddlVehicletype) AND(vmsc.MID = @Mid)");
            cmd.Parameters.Add("@ddlVehicletype", ddl_VehicleType.SelectedItem.Value);
            cmd.Parameters.Add("@Mid", Session["Mid"].ToString());
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            if (dt.Rows.Count > 0)
            {
                ddl_VehicleNo.Items.Clear();

                ddl_VehicleNo.DataSource = dt;
                ddl_VehicleNo.DataValueField = "VID";
                ddl_VehicleNo.DataTextField = "VehicleRegistrationNo";
                ddl_VehicleNo.DataBind();
                ddl_VehicleNo.ClearSelection();
                ddl_VehicleNo.Items.Insert(0, new ListItem { Value = "ALL", Text = "ALL", Selected = true });
                ddl_VehicleNo.SelectedIndex = 0;
            }
            else
            {

            }
        }
        catch (Exception ex)
        {
        }
    }

    private void Load_BillingownersList()
    {
        try
        {

            vdm = new VehicleDBMgr();
            cmd = new MySqlCommand(" SELECT sno, vendorname FROM vendors_info WHERE  (vendor_type =@OwnerType)");
            cmd.Parameters.Add("@OwnerType", "Dairy");
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            if (dt.Rows.Count > 0)
            {
                ddl_Billingowners.Items.Clear();

                ddl_Billingowners.DataSource = dt;
                ddl_Billingowners.DataValueField = "sno";
                ddl_Billingowners.DataTextField = "vendorname";
                ddl_Billingowners.DataBind();
                ddl_Billingowners.ClearSelection();
                ddl_Billingowners.Items.Insert(0, new ListItem { Value = "ALL", Text = "ALL", Selected = true });
                ddl_Billingowners.SelectedIndex = 0;
            }
            else
            {

            }
        }

        catch (Exception ex)
        {
        }
    }

    private void Load_VehicleTripBasedRouteData()
    {
        try
        {
            lblmsg.Text = "";
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


            vdm = new VehicleDBMgr();

            cmd = new MySqlCommand("SELECT DISTINCT tripdata.routeid FROM vehicel_master INNER JOIN minimasters ON vehicel_master.vhtype_refno = minimasters.sno INNER JOIN vmsc ON vehicel_master.vm_sno = vmsc.VID INNER JOIN tripdata ON vehicel_master.vm_sno = tripdata.vehicleno WHERE(minimasters.sno = @ddlVehicletype) AND(vmsc.MID = @Mid) AND(vmsc.VID = @Vid) AND(tripdata.tripdate BETWEEN @d1 AND @d2) ");
            cmd.Parameters.Add("@ddlVehicletype", ddl_VehicleType.SelectedItem.Value);
            cmd.Parameters.Add("@Mid", Session["Mid"].ToString());
            cmd.Parameters.Add("@d1", fromdate);
            cmd.Parameters.Add("@d2", todate);
            cmd.Parameters.Add("@vid", ddl_VehicleNo.SelectedItem.Value);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            if (dt.Rows.Count > 0)
            {

                ddl_Route.DataSource = dt;
                ddl_Route.DataValueField = "routeid";
                ddl_Route.DataTextField = "routeid";
                ddl_Route.DataBind();
                ddl_Route.ClearSelection();
                ddl_Route.Items.Insert(0, new ListItem { Value = "ALL", Text = "ALL", Selected = true });
                ddl_Route.SelectedIndex = 0;
            }
            else
            {

            }
        }
        catch (Exception ex)
        {
        }
    }

    public void ddl_VehicleType_SelectedIndexChanged(object sender, EventArgs e)
    {      
        try
        {
            Load_VehicleNOList();
             Load_VehicleTripBasedRouteData();
        }
        catch (Exception ex)
        {
        }
    }
    public void ddl_VehicleNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
             Load_VehicleTripBasedRouteData();
        }
        catch (Exception ex)
        {
        }
      

    }
    public void ddl_Billingowners_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
             Load_VehicleTripBasedRouteData();
        }
        catch (Exception ex)
        {
        }
       
    }
    public void ddl_Route_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        {
        }
    }

    public void btn_Generate_Click(object sender, EventArgs e)
    {
        try
        {
            GetReport();
        }
        catch (Exception ex)
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


    string routeid = "";
    string routeitype = "";
    DataTable Report = new DataTable();
    void GetReport()
    {
        try
        {
            lblinvoiceno.Text = "";
            lblmsg.Text = "";
            vdm = new VehicleDBMgr();
            PanelHide.Visible = true;
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
            // Invoice No check (Alter)
            cmd = new MySqlCommand("SELECT sno, fromdate, todate, invoiceno, branchid, entryby, vehicleno, vendorid, invoicedate FROM invoicetable WHERE(fromdate BETWEEN @d1 AND @d2) AND (vehicleno = @vehicleno) ");
            cmd.Parameters.Add("@vehicleno", ddl_VehicleNo.SelectedItem.Value);
            cmd.Parameters.Add("@d1", GetLowDate(fromdate));
            cmd.Parameters.Add("@d2", GetLowDate(todate));
            DataTable dtClose = vdm.SelectQuery(cmd).Tables[0];
            if (dtClose.Rows.Count > 0)
            {
                lblinvoiceno.Text = Session["shortname"].ToString() + "/TRAN/" + dtClose.Rows[0]["invoiceno"].ToString();
            }

            //Pls Alter it Dynamic
            cmd = new MySqlCommand("SELECT vehicel_master.vm_sno, vehicel_master.vhtype_refno, vehicel_master.stateid, statemaster.statename,statemaster.gststatecode, statemaster.statecode FROM vehicel_master INNER JOIN statemaster ON vehicel_master.stateid = statemaster.sno WHERE (vehicel_master.vm_sno = @Vehicleno)");
            cmd.Parameters.Add("@vehicleno", ddl_VehicleNo.SelectedValue);
            DataTable dt_fromVendor = vdm.SelectQuery(cmd).Tables[0];
            string gst_statecode = "";
            string gst_statename = "";
            if (dt_fromVendor.Rows.Count > 0)
            {
                string state_code = dt_fromVendor.Rows[0]["statecode"].ToString();
                gst_statecode = dt_fromVendor.Rows[0]["gststatecode"].ToString();
                gst_statename = dt_fromVendor.Rows[0]["statename"].ToString();
                if (state_code == "AP")
                {
                    Session["TitleName"] = "SRI VYSHNAVI DAIRY SPECIALITIES (P) LTD";
                    Session["Address"] = "R.S.No:381/2,Punabaka village Post,Pellakuru Mandal,Nellore District -524129., ANDRAPRADESH (State).Phone: 9440622077, Fax: 044 – 26177799. , ";
                    Session["Address1"] = "R.S.No:381/2,Punabaka village Post,";
                    Session["Address2"] = "Pellakuru Mandal,Nellore District -524129.";
                    Session["Address3"] = "ANDRAPRADESH (State).Phone: 9440622077, Fax: 044 – 26177799.";
                    Session["GSTno"] = "37AAFCS1152D1ZP";
                }
                if (state_code == "TS")
                {
                    Session["TitleName"] = "SRI VYSHNAVI FOODS (P) LTD";
                    Session["Address"] = " Near Ayyappa Swamy Temple, Shasta Nagar, WYRA-507165,KHAMMAM (District), TELANGANA (State).Phone: 08749 – 251326, Fax: 08749 – 252198.";
                    Session["Address1"] = "Near Ayyappa Swamy Temple,,";
                    Session["Address2"] = "Shasta Nagar, WYRA-507165 ,KHAMMAM (District).";
                    Session["Address3"] = " TELANGANA (State).Phone: 08749 – 251326, Fax: 08749 – 252198.";
                    Session["GSTno"] = "36AAGCS6022F1ZJ";
                }
                if (state_code == "TN")
                {
                    Session["TitleName"] = "SRI VYSHNAVI DAIRY SPECIALITIES (P) LTD";
                    Session["Address"] = " No,67/25, 2nd Street, Vishnavi House, TNHB Colony, Korattur, Chennai, Tamil Nadu 600080.  ";
                    Session["Address1"] = "No,67/25, 2nd Street,";
                    Session["Address2"] = "Vishnavi House, TNHB Colony.";
                    Session["Address3"] = "Korattur, Chennai, Tamil Nadu 600080.";
                    Session["GSTno"] = "33AAFCS1152D1ZX";
                }
            }
            lbl_client.Text = Session["TitleName"].ToString();
            lbl_client_address1.Text = Session["Address1"].ToString();
            lbl_client_address2.Text = Session["Address2"].ToString();
            lbl_client_address3.Text = Session["Address3"].ToString();
            lbl_client_gstno.Text = Session["GSTno"].ToString();
            lbl_client_statename.Text = gst_statename;
            lbl_client_statecode.Text = gst_statecode;
            lbl_peroidFrom.Text = fromdate.ToString("dd/MMM/yyyy");
            lbl_peroidto.Text = todate.ToString("dd/MMM/yyyy");
            lbl_tankerNo.Text = ddl_VehicleNo.SelectedItem.Text;
            lbldate.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            Session["filename"] = ddl_VehicleNo.SelectedItem.Text + " Report";

            cmd = new MySqlCommand("SELECT vendors_info.sno, vendors_info.vendor_code, vendors_info.vendorname, vendors_info.vendor_address, vendors_info.vendor_email, vendors_info.vendor_mob, vendors_info.gstin, vendors_info.vendor_vat,  vendors_info.vendor_cst, vendors_info.vendor_stax, vendors_info.branch_id, vendors_info.operatedby, vendors_info.vendor_status, vendors_info.vendor_type, vendors_info.tinno, vendors_info.panno,  statemaster.statename, statemaster.gststatecode FROM vendors_info INNER JOIN statemaster ON vendors_info.stateid = statemaster.sno WHERE (vendors_info.sno = @VendorID)");
            cmd.Parameters.Add("@VendorID", ddl_Billingowners.SelectedValue);
            DataTable dt_toVendor = vdm.SelectQuery(cmd).Tables[0];
            if (dt_toVendor.Rows.Count > 0)
            {
                lbl_Vendor_tile.Text = dt_toVendor.Rows[0]["vendorname"].ToString();
                lbl_Vendor_address.Text = dt_toVendor.Rows[0]["vendor_address"].ToString();
                lbl_Vendor_gstno.Text = dt_toVendor.Rows[0]["gstin"].ToString();
                lbl_Vendor_statename.Text = dt_toVendor.Rows[0]["statename"].ToString();
                lbl_Vendor_statecode.Text = dt_toVendor.Rows[0]["gststatecode"].ToString();
            }


            #region  Tanker Rpt   


            if (ddl_VehicleType.SelectedItem.Text == "Tanker")
            {

                if (ddl_VehicleNo.SelectedItem.Text == "ALL" && ddl_Route.SelectedItem.Text == "ALL")
                {
                    cmd = new MySqlCommand("SELECT DATE_FORMAT(triplogs.doe, '%d/%b/%y') AS TripDate, triplogs.fuel,vehicel_master.vm_owner,tripdata.DieselCost, locations.Location_name, triplogs.log_rank, triplogs.km, triplogs.charge, triplogs.charge * triplogs.km AS Amount, tripdata.tripsheetno, triplogs.tollgateamnt,triplogs.expamount, triplogs.load_cap, triplogs.unload_cap, tripdata.endfuelvalue, vehicel_master.registration_no AS VehicleNo, employdata.employname AS DriverName, triplogs.odometer, tripdata.vehiclestartreading,tripdata.TripSno, tripdata.endodometerreading  FROM  tripdata INNER JOIN triplogs ON tripdata.sno = triplogs.tripsno INNER JOIN locations ON triplogs.place = locations.sno LEFT OUTER JOIN employdata ON tripdata.driverid = employdata.emp_sno LEFT OUTER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno WHERE (tripdata.enddate BETWEEN @d1 AND @d2)  AND (tripdata.status = 'C') ORDER BY tripdata.tripdate, triplogs.log_rank");
                }

                if (ddl_VehicleNo.SelectedItem.Text != "ALL" && ddl_Route.SelectedItem.Text == "ALL")
                {
                    cmd = new MySqlCommand("SELECT DATE_FORMAT(triplogs.doe, '%d/%b/%y') AS TripDate, triplogs.fuel,vehicel_master.vm_owner,tripdata.DieselCost, locations.Location_name, triplogs.log_rank, triplogs.km, triplogs.charge, triplogs.charge * triplogs.km AS Amount, tripdata.tripsheetno, triplogs.tollgateamnt,triplogs.expamount, triplogs.load_cap, triplogs.unload_cap, tripdata.endfuelvalue, vehicel_master.registration_no AS VehicleNo, employdata.employname AS DriverName, triplogs.odometer, tripdata.vehiclestartreading,tripdata.TripSno, tripdata.endodometerreading  FROM  tripdata INNER JOIN triplogs ON tripdata.sno = triplogs.tripsno INNER JOIN locations ON triplogs.place = locations.sno LEFT OUTER JOIN employdata ON tripdata.driverid = employdata.emp_sno LEFT OUTER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno WHERE (tripdata.enddate BETWEEN @d1 AND @d2)  AND (tripdata.vehicleno = @VehicleNo) AND (tripdata.status = 'C') ORDER BY tripdata.tripdate, triplogs.log_rank");
                }

                if (ddl_VehicleNo.SelectedItem.Text == "ALL" && ddl_Route.SelectedItem.Text != "ALL")
                {
                    cmd = new MySqlCommand("SELECT DATE_FORMAT(triplogs.doe, '%d/%b/%y') AS TripDate, triplogs.fuel,vehicel_master.vm_owner,tripdata.DieselCost, locations.Location_name, triplogs.log_rank, triplogs.km, triplogs.charge, triplogs.charge * triplogs.km AS Amount, tripdata.tripsheetno, triplogs.tollgateamnt,triplogs.expamount, triplogs.load_cap, triplogs.unload_cap, tripdata.endfuelvalue, vehicel_master.registration_no AS VehicleNo, employdata.employname AS DriverName, triplogs.odometer, tripdata.vehiclestartreading,tripdata.TripSno, tripdata.endodometerreading  FROM  tripdata INNER JOIN triplogs ON tripdata.sno = triplogs.tripsno INNER JOIN locations ON triplogs.place = locations.sno LEFT OUTER JOIN employdata ON tripdata.driverid = employdata.emp_sno LEFT OUTER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno WHERE (tripdata.enddate BETWEEN @d1 AND @d2)  AND  (tripdata.routeid = @routeid) AND (tripdata.status = 'C') ORDER BY tripdata.tripdate, triplogs.log_rank");
                }

                if (ddl_VehicleNo.SelectedItem.Text != "ALL" && ddl_Route.SelectedItem.Text != "ALL")
                {
                    cmd = new MySqlCommand("SELECT DATE_FORMAT(triplogs.doe, '%d/%b/%y') AS TripDate, triplogs.fuel,vehicel_master.vm_owner,tripdata.DieselCost, locations.Location_name, triplogs.log_rank, triplogs.km, triplogs.charge, triplogs.charge * triplogs.km AS Amount, tripdata.tripsheetno, triplogs.tollgateamnt,triplogs.expamount, triplogs.load_cap, triplogs.unload_cap, tripdata.endfuelvalue, vehicel_master.registration_no AS VehicleNo, employdata.employname AS DriverName, triplogs.odometer, tripdata.vehiclestartreading,tripdata.TripSno, tripdata.endodometerreading  FROM  tripdata INNER JOIN triplogs ON tripdata.sno = triplogs.tripsno INNER JOIN locations ON triplogs.place = locations.sno LEFT OUTER JOIN employdata ON tripdata.driverid = employdata.emp_sno LEFT OUTER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno WHERE (tripdata.enddate BETWEEN @d1 AND @d2)  AND (tripdata.status = 'C') AND (tripdata.vehicleno = @VehicleNo) AND (tripdata.routeid = @routeid) ORDER BY tripdata.tripdate, triplogs.log_rank");
                }

                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@VehicleNo", ddl_VehicleNo.SelectedValue);
                cmd.Parameters.Add("@routeid", ddl_Route.SelectedValue);

                DataTable dtTrips = vdm.SelectQuery(cmd).Tables[0];
                DataTable trips = new DataView(dtTrips).ToTable(true, "tripsheetno");
                int i = 1;
                if (dtTrips.Rows.Count > 0)
                {
                    DataTable exp = new DataTable();
                    exp.Columns.Add("Vehicle No");
                    exp.Columns.Add("Sno");
                    exp.Columns.Add("TripsheetNo");
                    exp.Columns.Add("Trip Date");
                    exp.Columns.Add("Odometer");
                    exp.Columns.Add("Location");
                    exp.Columns.Add("Kms").DataType = typeof(Double);
                    exp.Columns.Add("Amount");
                    exp.Columns.Add("Total Amount").DataType = typeof(Double);
                    foreach (DataRow dr2 in trips.Rows)
                    {
                        Report = new DataTable();
                        Report = new DataTable();
                        Report.Columns.Add("Vehicle No");
                        Report.Columns.Add("Sno");
                        Report.Columns.Add("TripsheetNo");
                        Report.Columns.Add("Trip Date");
                        Report.Columns.Add("Odometer");
                        Report.Columns.Add("Location");
                        Report.Columns.Add("Kms").DataType = typeof(Double);
                        Report.Columns.Add("Amount");
                        Report.Columns.Add("Total Amount").DataType = typeof(Double);

                        DataRow[] newdatarow = dtTrips.Select("tripsheetno='" + dr2["tripsheetno"].ToString() + "'");
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
                        int flag = 0;
                        int flag1 = 0;
                        int tRank = 1;

                        foreach (DataRow dr in newdatarow)
                        {
                            Rank = dr["log_rank"].ToString();
                            if (flag == 0 && Rank == tRank.ToString())
                            {
                                flag1 = 1;
                                tRank = Convert.ToInt32(Rank);
                                tRank++;

                                if (Rank == "1")
                                {
                                    Location1 = dr["Location_name"].ToString();
                                    date1 = dr["TripDate"].ToString();
                                    odo1 = dr["odometer"].ToString();
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
                                    newrow["Vehicle No"] = dr["vehicleNo"].ToString();
                                    newrow["Odometer"] = odo1 + " - " + dr["odometer"].ToString();
                                    newrow["Kms"] = dr["km"].ToString();
                                    newrow["Amount"] = dr["charge"].ToString();
                                    newrow["Total Amount"] = dr["Amount"].ToString();

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
                                    Report.Rows.Add(newrow);
                                }
                            }
                        }

                        if (flag1 == 1)
                        {
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
                    }

                    DataRow Grandtotal = exp.NewRow();
                    DataRow[] ttl = exp.Select("Location='Total'");
                    double grdttl = 0.00;
                    double km = 0.00;
                    foreach (DataRow item in ttl)
                    {
                        double temp = 0.00;
                        double.TryParse(item["Total Amount"].ToString(), out temp);
                        grdttl += temp;
                        double temp2 = 0.00;
                        double.TryParse(item["Kms"].ToString(), out temp2);
                        km += temp2;
                    }
                    Grandtotal["Vehicle No"] = "";
                    Grandtotal["Location"] = "Sub Total";
                    Grandtotal["Kms"] = km;
                    Grandtotal["Total Amount"] = grdttl;
                    exp.Rows.Add(Grandtotal);


                    //Pls CheckIt Once Tollgate
                    cmd = new MySqlCommand("SELECT SUM(sub_veh_exp.amount) AS Amount, veh_exp.doe FROM veh_exp INNER JOIN sub_veh_exp ON veh_exp.sno = sub_veh_exp.refno WHERE (veh_exp.vehsno = @Vehsno) AND (sub_veh_exp.head_sno = 20) AND (veh_exp.doe BETWEEN @d1 AND @d2) GROUP BY veh_exp.vehsno, sub_veh_exp.head_sno");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@Vehsno", ddl_VehicleNo.SelectedValue);
                    DataTable dttollgate = vdm.SelectQuery(cmd).Tables[0];
                    double tollamount = 0;

                    if (dttollgate.Rows.Count > 0)
                    {
                        DataRow newrow3 = exp.NewRow();
                        newrow3["Location"] = "Tollgates";
                        double.TryParse(dttollgate.Rows[0]["Amount"].ToString(), out tollamount);
                        newrow3["Total Amount"] = tollamount;
                        exp.Rows.Add(newrow3);
                    }
                    double totalamount = tollamount + grdttl;
                    string Amont = totalamount.ToString();

                    DataRow newrow4 = exp.NewRow();
                    newrow4["Location"] = "Grand Total";
                    newrow4["Total Amount"] = totalamount;
                    exp.Rows.Add(newrow4);


                    //Gst
                    string fromstate = lbl_client_statecode.Text;
                    string tostate = lbl_Vendor_statecode.Text;
                    //if (fromstate == tostate)
                    //{
                    double Igstamount = 0;
                    double Igst = 5;
                    // double Igstcon = 100 + Igst;
                    Igstamount = (totalamount) * Igst;
                    Igstamount = Igstamount / 100;
                    Igstamount = Math.Round(Igstamount, 2);
                    if (fromstate == tostate)
                    {
                        Igstamount = Igstamount / 2;
                        lbl_sgst.Text = Igstamount.ToString("F2");
                        lbl_cgst.Text = Igstamount.ToString("F2");
                        lbl_igst.Text = "0";
                    }
                    else
                    {
                        lbl_sgst.Text = "0";
                        lbl_cgst.Text = "0";
                        lbl_igst.Text = Igstamount.ToString("F2");
                    }



                    double grand_total = 0;
                    grand_total = totalamount + Igstamount;
                    grand_total = Math.Round(grand_total, 0);
                    string stramount = grand_total.ToString();
                    lbl_grandtotal.Text = grand_total.ToString();
                    int amount = 0;
                    int.TryParse(grand_total.ToString(), out amount);
                    lblamountinwords.Text = NumToWordBD(amount) + " Rupees Only";


                    string title = "Tanker Report From: " + fromdate.ToString() + "  To: " + todate.ToString();
                    Session["title"] = title;
                    Session["xportdata"] = exp;
                    grdReports.DataSource = exp;
                    grdReports.DataBind();

                }
            }
            #endregion Tanker Rpt

            #region Bus Rpt 

            if (ddl_VehicleType.Text == "Bus")
            {
                if (ddl_VehicleNo.Text == "ALL" && ddl_Route.Text == "ALL")
                {
                    cmd = new MySqlCommand("SELECT tripdata.sno as RefNo,tripdata.tripSno,tripdata.tripsheetno, DATE_FORMAT(tripdata.tripdate,'%d/%m/%Y') AS StartDate,DATE_FORMAT(tripdata.enddate,'%d/%m/%Y') AS EndDate,vehicel_master.vm_owner as Owner, vehicel_master.registration_no AS VehicleNo,tripdata.rent, (tripdata.endodometerreading - tripdata.vehiclestartreading) AS TripKMS, tripdata.gpskms, ROUND(tripdata.endodometerreading - tripdata.vehiclestartreading - tripdata.gpskms,2) AS Dif,  tripdata.endfuelvalue AS Diesel,ROUND((tripdata.endodometerreading - tripdata.vehiclestartreading)/( tripdata.endfuelvalue),2) as Mileage, tripdata.loadtype, tripdata.qty,tripdata.tripexpences AS TripExpences, tripdata.routeid as RouteName, employdata.employname AS DriverName FROM tripdata INNER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno LEFT OUTER JOIN employdata ON tripdata.driverid = employdata.emp_sno WHERE (tripdata.tripdate BETWEEN @d1 AND @d2) AND (tripdata.userid = @BranchID) AND (vehicel_master.vhtype_refno = 140)");
                }
                if (ddl_VehicleNo.Text != "ALL" && ddl_Route.Text == "ALL")
                {
                    cmd = new MySqlCommand("SELECT tripdata.sno as RefNo,tripdata.tripSno,tripdata.tripsheetno, DATE_FORMAT(tripdata.tripdate,'%d/%m/%Y') AS StartDate,DATE_FORMAT(tripdata.enddate,'%d/%m/%Y') AS EndDate,vehicel_master.vm_owner as Owner, vehicel_master.registration_no AS VehicleNo,tripdata.rent, (tripdata.endodometerreading - tripdata.vehiclestartreading) AS TripKMS, tripdata.gpskms, ROUND(tripdata.endodometerreading - tripdata.vehiclestartreading - tripdata.gpskms,2) AS Dif,  tripdata.endfuelvalue AS Diesel,ROUND((tripdata.endodometerreading - tripdata.vehiclestartreading)/( tripdata.endfuelvalue),2) as Mileage, tripdata.loadtype, tripdata.qty,tripdata.tripexpences AS TripExpences, tripdata.routeid as RouteName, employdata.employname AS DriverName FROM tripdata INNER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno LEFT OUTER JOIN employdata ON tripdata.driverid = employdata.emp_sno WHERE (tripdata.tripdate BETWEEN @d1 AND @d2) AND (tripdata.userid = @BranchID) and (tripdata.vehicleno=@vehicleno)");
                }
                if (ddl_VehicleNo.Text != "ALL" && ddl_Route.Text != "ALL")
                {
                    cmd = new MySqlCommand("SELECT tripdata.sno as RefNo,tripdata.tripSno,tripdata.tripsheetno, DATE_FORMAT(tripdata.tripdate,'%d/%m/%Y') AS StartDate,DATE_FORMAT(tripdata.enddate,'%d/%m/%Y') AS EndDate,vehicel_master.vm_owner as Owner, vehicel_master.registration_no AS VehicleNo,tripdata.rent, (tripdata.endodometerreading - tripdata.vehiclestartreading) AS TripKMS, tripdata.gpskms, ROUND(tripdata.endodometerreading - tripdata.vehiclestartreading - tripdata.gpskms,2) AS Dif,  tripdata.endfuelvalue AS Diesel,ROUND((tripdata.endodometerreading - tripdata.vehiclestartreading)/( tripdata.endfuelvalue),2) as Mileage, tripdata.loadtype, tripdata.qty,tripdata.tripexpences AS TripExpences, tripdata.routeid as RouteName, employdata.employname AS DriverName FROM tripdata INNER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno LEFT OUTER JOIN employdata ON tripdata.driverid = employdata.emp_sno WHERE (tripdata.tripdate BETWEEN @d1 AND @d2) AND (tripdata.userid = @BranchID) and (tripdata.vehicleno=@vehicleno) AND (tripdata.routeid = @routeid)");
                }

                if (ddl_VehicleNo.Text == "ALL" && ddl_Route.Text != "ALL")
                {
                    cmd = new MySqlCommand("SELECT tripdata.sno as RefNo,tripdata.tripSno,tripdata.tripsheetno, DATE_FORMAT(tripdata.tripdate,'%d/%m/%Y') AS StartDate,DATE_FORMAT(tripdata.enddate,'%d/%m/%Y') AS EndDate,vehicel_master.vm_owner as Owner, vehicel_master.registration_no AS VehicleNo,tripdata.rent, (tripdata.endodometerreading - tripdata.vehiclestartreading) AS TripKMS, tripdata.gpskms, ROUND(tripdata.endodometerreading - tripdata.vehiclestartreading - tripdata.gpskms,2) AS Dif,  tripdata.endfuelvalue AS Diesel,ROUND((tripdata.endodometerreading - tripdata.vehiclestartreading)/( tripdata.endfuelvalue),2) as Mileage, tripdata.loadtype, tripdata.qty,tripdata.tripexpences AS TripExpences, tripdata.routeid as RouteName, employdata.employname AS DriverName FROM tripdata INNER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno LEFT OUTER JOIN employdata ON tripdata.driverid = employdata.emp_sno WHERE (tripdata.tripdate BETWEEN @d1 AND @d2) AND (tripdata.routeid = @routeid) AND (tripdata.userid = @BranchID) AND (vehicel_master.vhtype_refno = 140)");
                }
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@BranchID", Session["Branch_ID"].ToString());
                cmd.Parameters.Add("@vehicleno", ddl_VehicleNo.SelectedValue);
                cmd.Parameters.Add("@routeid", ddl_Route.SelectedValue);
                DataTable trips = vdm.SelectQuery(cmd).Tables[0];

                Report.Columns.Add("Vehicle No");
                Report.Columns.Add("Sno");
                Report.Columns.Add("TripsheetNo");
                Report.Columns.Add("start Date");
                Report.Columns.Add("RouteName");
                Report.Columns.Add("TripKMS");

                int i = 1;
                foreach (DataRow dr in trips.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["SNo"] = i++.ToString();
                    newrow["TripsheetNo"] = dr["tripsheetno"].ToString();
                    newrow["start Date"] = dr["StartDate"].ToString();
                    newrow["TripKMS"] = dr["TripKMS"].ToString();
                    newrow["RouteName"] = dr["RouteName"].ToString();
                    newrow["Vehicle No"] = dr["VehicleNo"].ToString();
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
                //object ks; TEST
                //ks = Report.Compute("SUM(TripKMS)", "").ToString();
                string Amont = val.ToString();
                int Num = 0;
                int.TryParse(Amont, out Num);
                lblamountinwords.Text = NumToWordBD(Num) + " Rupees Only";
                Report.Rows.Add(newvartical);


                string title = "Bus Report From: " + fromdate.ToString() + "  To: " + todate.ToString();
                Session["title"] = title;
                Session["xportdata"] = Report;
                grdReports.DataSource = Report;
                grdReports.DataBind();
            }
            #endregion Bus Rpt

            #region  Puff&Truck Rpt 

            Report.Columns.Add("Vehicle No");
            Report.Columns.Add("Sno");
            Report.Columns.Add("TripsheetNo");
            Report.Columns.Add("start Date");
            Report.Columns.Add("RouteName");
            Report.Columns.Add("TripKMS");
            Report.Columns.Add("Rent").DataType = typeof(Double);


            if (ddl_VehicleType.SelectedItem.Text == "Puff" || ddl_VehicleType.SelectedItem.Text == "Truck")
            {
                if (ddl_Route.Text == "ALL" && ddl_VehicleNo.Text != "ALL")
                {
                    cmd = new MySqlCommand("SELECT tripdata.sno as RefNo,tripdata.tripSno,tripdata.tripsheetno, DATE_FORMAT(tripdata.tripdate,'%d/%m/%Y') AS StartDate,DATE_FORMAT(tripdata.enddate,'%d/%m/%Y') AS EndDate,vehicel_master.vm_owner as Owner, vehicel_master.registration_no AS VehicleNo,tripdata.rent, (tripdata.endodometerreading - tripdata.vehiclestartreading) AS TripKMS, tripdata.gpskms, ROUND(tripdata.endodometerreading - tripdata.vehiclestartreading - tripdata.gpskms,2) AS Dif,  tripdata.endfuelvalue AS Diesel,ROUND((tripdata.endodometerreading - tripdata.vehiclestartreading)/( tripdata.endfuelvalue),2) as Mileage, tripdata.loadtype, tripdata.qty,tripdata.tripexpences AS TripExpences, tripdata.routeid as RouteName, employdata.employname AS DriverName FROM tripdata INNER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno LEFT OUTER JOIN employdata ON tripdata.driverid = employdata.emp_sno WHERE (tripdata.tripdate BETWEEN @d1 AND @d2) AND (tripdata.userid = @BranchID) and (tripdata.vehicleno=@vehicleno)");
                }
                if (ddl_Route.SelectedItem.Text != "ALL" && ddl_VehicleNo.SelectedItem.Text != "ALL")
                {
                    cmd = new MySqlCommand("SELECT tripdata.sno as RefNo,tripdata.tripSno,tripdata.tripsheetno, DATE_FORMAT(tripdata.tripdate,'%d/%m/%Y') AS StartDate,DATE_FORMAT(tripdata.enddate,'%d/%m/%Y') AS EndDate,vehicel_master.vm_owner as Owner, vehicel_master.registration_no AS VehicleNo,tripdata.rent, (tripdata.endodometerreading - tripdata.vehiclestartreading) AS TripKMS, tripdata.gpskms, ROUND(tripdata.endodometerreading - tripdata.vehiclestartreading - tripdata.gpskms,2) AS Dif,  tripdata.endfuelvalue AS Diesel,ROUND((tripdata.endodometerreading - tripdata.vehiclestartreading)/( tripdata.endfuelvalue),2) as Mileage, tripdata.loadtype, tripdata.qty,tripdata.tripexpences AS TripExpences, tripdata.routeid as RouteName, employdata.employname AS DriverName FROM tripdata INNER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno LEFT OUTER JOIN employdata ON tripdata.driverid = employdata.emp_sno WHERE (tripdata.tripdate BETWEEN @d1 AND @d2) AND (tripdata.userid = @BranchID) and (tripdata.vehicleno=@vehicleno) AND (tripdata.routeid = @routeid)");
                }
                if (ddl_VehicleType.SelectedItem.Text == "Puff" && ddl_Route.SelectedItem.Text == "ALL" && ddl_VehicleNo.SelectedItem.Text == "ALL")
                {
                    cmd = new MySqlCommand("SELECT tripdata.sno as RefNo,tripdata.tripSno,tripdata.tripsheetno, DATE_FORMAT(tripdata.tripdate,'%d/%m/%Y') AS StartDate,DATE_FORMAT(tripdata.enddate,'%d/%m/%Y') AS EndDate,vehicel_master.vm_owner as Owner, vehicel_master.registration_no AS VehicleNo,tripdata.rent, (tripdata.endodometerreading - tripdata.vehiclestartreading) AS TripKMS, tripdata.gpskms, ROUND(tripdata.endodometerreading - tripdata.vehiclestartreading - tripdata.gpskms,2) AS Dif,  tripdata.endfuelvalue AS Diesel,ROUND((tripdata.endodometerreading - tripdata.vehiclestartreading)/( tripdata.endfuelvalue),2) as Mileage, tripdata.loadtype, tripdata.qty,tripdata.tripexpences AS TripExpences, tripdata.routeid as RouteName, employdata.employname AS DriverName FROM tripdata INNER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno LEFT OUTER JOIN employdata ON tripdata.driverid = employdata.emp_sno WHERE (tripdata.tripdate BETWEEN @d1 AND @d2) AND (tripdata.userid = @BranchID)");
                }
                if (ddl_Route.Text != "ALL" && ddl_VehicleNo.Text == "ALL")
                {
                    cmd = new MySqlCommand("SELECT tripdata.sno as RefNo,tripdata.tripSno,tripdata.tripsheetno, DATE_FORMAT(tripdata.tripdate,'%d/%m/%Y') AS StartDate,DATE_FORMAT(tripdata.enddate,'%d/%m/%Y') AS EndDate,vehicel_master.vm_owner as Owner, vehicel_master.registration_no AS VehicleNo,tripdata.rent, (tripdata.endodometerreading - tripdata.vehiclestartreading) AS TripKMS, tripdata.gpskms, ROUND(tripdata.endodometerreading - tripdata.vehiclestartreading - tripdata.gpskms,2) AS Dif,  tripdata.endfuelvalue AS Diesel,ROUND((tripdata.endodometerreading - tripdata.vehiclestartreading)/( tripdata.endfuelvalue),2) as Mileage, tripdata.loadtype, tripdata.qty,tripdata.tripexpences AS TripExpences, tripdata.routeid as RouteName, employdata.employname AS DriverName FROM tripdata INNER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno LEFT OUTER JOIN employdata ON tripdata.driverid = employdata.emp_sno WHERE (tripdata.tripdate BETWEEN @d1 AND @d2) AND (tripdata.routeid = @routeid) AND (tripdata.userid = @BranchID)");
                }
                if (ddl_VehicleType.SelectedItem.Text == "Truck" && ddl_Route.SelectedItem.Text == "ALL" && ddl_VehicleNo.SelectedItem.Text == "ALL")
                {
                    cmd = new MySqlCommand("SELECT tripdata.sno as RefNo,tripdata.tripSno,tripdata.tripsheetno, DATE_FORMAT(tripdata.tripdate,'%d/%m/%Y') AS StartDate,DATE_FORMAT(tripdata.enddate,'%d/%m/%Y') AS EndDate,vehicel_master.vm_owner as Owner, vehicel_master.registration_no AS VehicleNo,tripdata.rent, (tripdata.endodometerreading - tripdata.vehiclestartreading) AS TripKMS, tripdata.gpskms, ROUND(tripdata.endodometerreading - tripdata.vehiclestartreading - tripdata.gpskms,2) AS Dif,  tripdata.endfuelvalue AS Diesel,ROUND((tripdata.endodometerreading - tripdata.vehiclestartreading)/( tripdata.endfuelvalue),2) as Mileage, tripdata.loadtype, tripdata.qty,tripdata.tripexpences AS TripExpences, tripdata.routeid as RouteName, employdata.employname AS DriverName FROM tripdata INNER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno LEFT OUTER JOIN employdata ON tripdata.driverid = employdata.emp_sno WHERE (tripdata.tripdate BETWEEN @d1 AND @d2) AND (tripdata.userid = @BranchID)  AND (vehicel_master.vhtype_refno = 23)");
                }
                
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@BranchID", Session["Branch_ID"].ToString());
                cmd.Parameters.Add("@vehicleno", ddl_VehicleNo.SelectedValue);
                cmd.Parameters.Add("@routeid", ddl_Route.SelectedValue);

                DataTable trips = vdm.SelectQuery(cmd).Tables[0];
                int i = 1;
                foreach (DataRow dr in trips.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["SNo"] = i++.ToString();
                    newrow["TripsheetNo"] = dr["tripsheetno"].ToString();
                    newrow["start Date"] = dr["StartDate"].ToString();
                    newrow["TripKMS"] = dr["TripKMS"].ToString();
                    newrow["RouteName"] = dr["RouteName"].ToString();
                    newrow["Vehicle No"] = dr["VehicleNo"].ToString();
                    newrow["Rent"] = dr["rent"].ToString();
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
                //Tollgate
                cmd = new MySqlCommand("");
                cmd.Parameters.Add("", 0);
                cmd.Parameters.Add("", 0);
                DataTable Tollgate = vdm.SelectQuery(cmd).Tables[0];
                

                //


                string Amont = val.ToString();
                int Num = 0;
                int.TryParse(Amont, out Num);
                lblamountinwords.Text = NumToWordBD(Num) + " Rupees Only";
                Report.Rows.Add(newvartical);


                string title = "Puff Report From: " + fromdate.ToString() + "  To: " + todate.ToString();
                Session["title"] = title;
                Session["xportdata"] = Report;
                grdReports.DataSource = Report;
                grdReports.DataBind();
            }
            else
            {
                lblmsg.Text = "No Data Found";
                grdReports.Visible = false;
            }
            #endregion  Puff&Truck Rpt 

        }
        catch (Exception ex)
        {
        }
    }
    public void btn_Save_Click(object sender, EventArgs e)
    {
        try
        {
            SalesDBManager svdm = new SalesDBManager();
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
            DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
            int currentyear = ServerDateCurrentdate.Year;
            int nextyear = ServerDateCurrentdate.Year + 1;
            DateTime dtapril = new DateTime();
            DateTime dtmarch = new DateTime();
            if (ServerDateCurrentdate.Month > 3)
            {
                string apr = "4/1/" + currentyear;
                dtapril = DateTime.Parse(apr);
                string march = "3/31/" + nextyear;
                dtmarch = DateTime.Parse(march);
            }
            if (ServerDateCurrentdate.Month <= 3)
            {
                string apr = "4/1/" + (currentyear - 1);
                dtapril = DateTime.Parse(apr);
                string march = "3/31/" + (nextyear - 1);
                dtmarch = DateTime.Parse(march);
            }

            string moduleid = "5";
            string companycode = "";
            string statecode = "";
            string statename = "";
            string stateid = "";
            cmd = new MySqlCommand("SELECT statemaster.gststatecode, statemaster.statecode, statemaster.statename,statemaster.sno FROM vehicel_master INNER JOIN statemaster ON vehicel_master.stateid = statemaster.sno WHERE (vehicel_master.vm_sno = @VehicleID)");
            cmd.Parameters.Add("@VehicleID", ddl_VehicleNo.SelectedValue);
            DataTable dtstate = vdm.SelectQuery(cmd).Tables[0];
            vdm = new VehicleDBMgr();
            string BranchID = Session["Branch_ID"].ToString();
            //Alter FrmDate
            //cmd = new MySqlCommand("SELECT sno, fromdate, todate, invoiceno, branchid, entryby, vehicleno, vendorid FROM invoicetable WHERE (fromdate >= @d1) AND (todate >= @d2) AND (vehicleno = @vehicleno) ");
            cmd = new MySqlCommand("SELECT sno, fromdate, todate, invoiceno, branchid, entryby, vehicleno, vendorid FROM invoicetable WHERE  (fromdate BETWEEN @d1 AND @d2) AND (vehicleno = @vehicleno) ");
            cmd.Parameters.Add("@vehicleno", ddl_VehicleNo.SelectedValue);
            cmd.Parameters.Add("@d1", GetLowDate(fromdate));
            cmd.Parameters.Add("@d2", GetHighDate(todate));
            DataTable dtClose = vdm.SelectQuery(cmd).Tables[0];
            if (dtClose.Rows.Count > 0)
            {
                //lblinvoiceno.Text = Session["shortname"].ToString() + "/TRAN/" + dtClose.Rows[0]["invoiceno"].ToString();
                if (dtstate.Rows.Count > 0)
                {
                    statecode = dtstate.Rows[0]["gststatecode"].ToString();
                    statename = dtstate.Rows[0]["statecode"].ToString();
                    stateid = dtstate.Rows[0]["sno"].ToString();
                }
                string DCNO = "0";
                int countdc = 0;
                int.TryParse(dtClose.Rows[0]["invoiceno"].ToString(), out countdc);
                if (countdc <= 10)
                {
                    DCNO = "0000" + countdc;
                }
                if (countdc >= 10 && countdc <= 99)
                {
                    DCNO = "000" + countdc;
                }
                if (countdc >= 99 && countdc <= 999)
                {
                    DCNO = "00" + countdc;
                }
                if (countdc > 999)
                {
                    DCNO = "0" + countdc;
                }
                lblinvoiceno.Text = statename + "/I" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "/" + DCNO;
                lblmsg.Text = "This invoice already saved";
            }
            else
            {
                string Branch_ID = Session["Branch_ID"].ToString();
                if (dtstate.Rows.Count > 0)
                {
                    statecode = dtstate.Rows[0]["gststatecode"].ToString();
                    statename = dtstate.Rows[0]["statecode"].ToString();
                    stateid = dtstate.Rows[0]["sno"].ToString();
                    cmd = new MySqlCommand("SELECT brnch_sno, salesbranchid, shortname, whcode FROM branch_info WHERE (brnch_sno = @branchid)");
                    cmd.Parameters.Add("@branchid", Branch_ID);
                    DataTable dtdetails = vdm.SelectQuery(cmd).Tables[0];
                    string shortname = dtdetails.Rows[0]["shortname"].ToString();
                    if (stateid == "3")
                    {
                        companycode = "1";
                    }
                    if (stateid == "24")
                    {
                        companycode = "1";
                    }
                    if (stateid == "25")
                    {
                        companycode = "3";
                    }
                    if (stateid == "15")
                    {
                        companycode = "1";
                    }
                    //sales code
                    cmd = new MySqlCommand("SELECT IFNULL(MAX(agentdcno), 0) + 1 AS dcno FROM  agentdc WHERE (companycode = @companycode) AND (stateid = @statecode) AND (IndDate BETWEEN @d1 AND @d2)");
                    cmd.Parameters.Add("@companycode", companycode);
                    cmd.Parameters.Add("@statecode", statecode);
                    cmd.Parameters.Add("@d1", GetLowDate(dtapril));
                    cmd.Parameters.Add("@d2", GetHighDate(dtmarch));
                    DataTable dtdcno = svdm.SelectQuery(cmd).Tables[0];
                    string salesinvoiceno = dtdcno.Rows[0]["dcno"].ToString();
                    cmd = new MySqlCommand("insert into  agentdc(BranchID,IndDate,agentdcno,stateid,companycode,moduleid) values(@BranchID,@IndDate,@agentdcno,@stateid,@companycode,@moduleid) ");
                    cmd.Parameters.Add("@BranchID", Branch_ID);  //taken from sesssion
                    cmd.Parameters.Add("@IndDate", ServerDateCurrentdate);   //serverdate
                    cmd.Parameters.Add("@agentdcno", salesinvoiceno);   //sales dcno
                    cmd.Parameters.Add("@stateid", statecode);
                    cmd.Parameters.Add("@companycode", companycode);
                    cmd.Parameters.Add("@moduleid", moduleid);
                    svdm.insert(cmd);

                    string entryby = Session["Employ_Sno"].ToString();

                    cmd = new MySqlCommand("insert into invoicetable(fromdate,todate,invoiceno,branchid,entryby,vehicleno,vendorid,invoicedate) values(@fromdate,@todate,@invoiceno,@branchid,@entryby,@vehicleno,@vendorid,@invoicedate)");
                    cmd.Parameters.Add("@fromdate", fromdate);
                    cmd.Parameters.Add("@todate", todate);
                    cmd.Parameters.Add("@invoiceno", salesinvoiceno);
                    cmd.Parameters.Add("@branchid", BranchID);
                    cmd.Parameters.Add("@entryby", entryby);
                    cmd.Parameters.Add("@vehicleno", ddl_VehicleNo.SelectedValue);
                    cmd.Parameters.Add("@vendorid", ddl_Billingowners.SelectedValue);
                    cmd.Parameters.Add("@invoicedate", ServerDateCurrentdate);
                    vdm.insert(cmd);
                    string DCNO = "0";
                    int countdc = 0;
                    int.TryParse(salesinvoiceno, out countdc);
                    if (countdc <= 10)
                    {
                        DCNO = "0000" + countdc;
                    }
                    if (countdc >= 10 && countdc <= 99)
                    {
                        DCNO = "000" + countdc;
                    }
                    if (countdc >= 99 && countdc <= 999)
                    {
                        DCNO = "00" + countdc;
                    }
                    if (countdc > 999)
                    {
                        DCNO = "0" + countdc;
                    }
                    lblinvoiceno.Text = statename + "/I" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "/" + DCNO;
                    lblmsg.Text = "Invoice saved successfully";
                }
                else
                {
                    lblmsg.Text = "State name not saved contact fleet admin";
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public static string NumToWordBD(Int64 Num)
    {
        string[] Below20 = { "", "One ", "Two ", "Three ", "Four ",
      "Five ", "Six " , "Seven ", "Eight ", "Nine ", "Ten ", "Eleven ",
    "Twelve " , "Thirteen ", "Fourteen ","Fifteen ",
      "Sixteen " , "Seventeen ","Eighteen " , "Nineteen " };
        string[] Below100 = { "", "", "Twenty ", "Thirty ",
      "Forty ", "Fifty ", "Sixty ", "Seventy ", "Eighty ", "Ninety " };
        string InWords = "";
        if (Num >= 1 && Num < 20)
            InWords += Below20[Num];
        if (Num >= 20 && Num <= 99)
            InWords += Below100[Num / 10] + Below20[Num % 10];
        if (Num >= 100 && Num <= 999)
            InWords += NumToWordBD(Num / 100) + " Hundred " + NumToWordBD(Num % 100);
        if (Num >= 1000 && Num <= 99999)
            InWords += NumToWordBD(Num / 1000) + " Thousand " + NumToWordBD(Num % 1000);
        if (Num >= 100000 && Num <= 9999999)
            InWords += NumToWordBD(Num / 100000) + " Lakh " + NumToWordBD(Num % 100000);
        if (Num >= 10000000)
            InWords += NumToWordBD(Num / 10000000) + " Crore " + NumToWordBD(Num % 10000000);
        return InWords;
    }


}