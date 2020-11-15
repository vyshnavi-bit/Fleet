using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class TransportPL : System.Web.UI.Page
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
    protected void btn_Generate_Click(object sender, EventArgs e)
    {
        try
        {
            lblmsg.Text = "";
            hidepanel.Visible = true;

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

            lblFromDate.Text = fromdate.ToString("dd/MMM/yyyy");
            lbltodate.Text = todate.ToString("dd/MMM/yyyy");
            Report.Columns.Add("Sno");
            Report.Columns.Add("VehicleNo");
            Report.Columns.Add("Running kms").DataType = typeof(Double);
            Report.Columns.Add("Value").DataType = typeof(Double);
            if (ddlType.SelectedValue == "All Puffs")
            {
            }
            else
            {
                Report.Columns.Add("Diesel Expenses").DataType = typeof(Double);
            }
            Report.Columns.Add("Store Consumption").DataType = typeof(Double);
            Report.Columns.Add("Repair&Maintance").DataType = typeof(Double);
            Report.Columns.Add("Taxes").DataType = typeof(Double);
            Report.Columns.Add("Salaries").DataType = typeof(Double);
            if (ddltermloanType.SelectedValue == "With TL")
            {
                Report.Columns.Add("TermLoan").DataType = typeof(Double);
            }
            else
            {
            }
            Report.Columns.Add("Insurance").DataType = typeof(Double);
            Report.Columns.Add("GPS").DataType = typeof(Double);
            Report.Columns.Add("Total Expenses").DataType = typeof(Double);
            Report.Columns.Add("Net Profit/Loss").DataType = typeof(Double);

            Report.Columns.Add("Per kms");
            if (ddlType.SelectedValue == "All Puffs")
            {
            }
            else
            {
                Report.Columns.Add("Avg Mileage");
            }


            #region "All Puffs"
            
            if (ddlType.SelectedValue == "All Puffs")
            {
                Session["filename"] = "P&L All Puffs";
                    cmd = new MySqlCommand("SELECT COUNT(vehicel_master.registration_no) as vehicleworkingdays, vehicel_master.registration_no, SUM(tripdata.endodometerreading - tripdata.vehiclestartreading) AS TripKMS, SUM(tripdata.Rent) AS Rent FROM vehicel_master INNER JOIN tripdata ON vehicel_master.vm_sno = tripdata.vehicleno WHERE (vehicel_master.vm_owner = @Owner) AND (vehicel_master.vhtype_refno = 7) AND (tripdata.tripdate BETWEEN @d1 AND @d2) GROUP BY vehicel_master.registration_no");
                    cmd.Parameters.Add("@Owner", Session["shortname"].ToString());
                cmd.Parameters.Add("@BranchID", BranchID);
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                DataTable dtVehiclekms = vdm.SelectQuery(cmd).Tables[0];
                int i = 1;
                foreach (DataRow dr in dtVehiclekms.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["Sno"] = i++.ToString();
                    newrow["VehicleNo"] = dr["registration_no"].ToString();
                    double gps = 572.5;
                    newrow["GPS"] = gps.ToString();
                    double TripKMS = 0;
                    double.TryParse(dr["TripKMS"].ToString(), out TripKMS);
                    double Rent = 0;
                    double.TryParse(dr["Rent"].ToString(), out Rent);
                    int vehicleworkingdays = 0;
                    int.TryParse(dr["vehicleworkingdays"].ToString(), out vehicleworkingdays);
                    if (vehicleworkingdays > 30)
                    {
                        vehicleworkingdays = 30;
                    }
                    vehicleworkingdays = vehicleworkingdays + 1;
                    if (TripKMS == 0.0)
                    {
                        Rent = 0;
                    }
                    newrow["Running kms"] = TripKMS;
                    newrow["Value"] = Rent;

                    double sal = 0;
                    string Vehicleno = dr["registration_no"].ToString();
                    if (Vehicleno == "AP26TC3159")
                    {
                        sal = 31 * 613.767;

                    }
                    else
                    {
                        sal = vehicleworkingdays * 613.767;
                        if (Vehicleno == "AP26TB2358" || Vehicleno == "AP26TC5769" || Vehicleno == "AP26TC5949" || Vehicleno == "AP26TC6786")
                        {
                            sal = sal + (vehicleworkingdays * 346);
                        }
                    }
                    newrow["Salaries"] = Math.Round(sal, 2);

                    Report.Rows.Add(newrow);
                }
               
                    cmd = new MySqlCommand("SELECT  SUM(sub_veh_exp.amount) AS Amount, vehicel_master.registration_no FROM veh_exp INNER JOIN sub_veh_exp ON veh_exp.sno = sub_veh_exp.refno RIGHT OUTER JOIN vehicel_master ON veh_exp.vehsno = vehicel_master.vm_sno WHERE (veh_exp.doe BETWEEN @d1 AND @d2) AND (vehicel_master.vm_owner = @Owner) AND (vehicel_master.vhtype_refno = 7) AND (sub_veh_exp.head_sno <> 38) AND  (sub_veh_exp.head_sno <> 9) GROUP BY vehicel_master.registration_no");
                    cmd.Parameters.Add("@Owner", Session["shortname"].ToString());
               
                cmd.Parameters.Add("@BranchID", BranchID);
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                DataTable dtmaintaince = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow dr in Report.Rows)
                {
                    foreach (DataRow drm in dtmaintaince.Rows)
                    {
                        if (dr["VehicleNo"].ToString() == drm["registration_no"].ToString())
                        {
                            double Amount = 0;
                            double.TryParse(drm["Amount"].ToString(), out Amount);
                            dr["Repair&Maintance"] = Math.Round(Amount, 2);
                        }
                    }
                }
                cmd = new MySqlCommand("SELECT SUM(sub_veh_exp.amount) AS Amount, vehicel_master.registration_no FROM veh_exp INNER JOIN sub_veh_exp ON veh_exp.sno = sub_veh_exp.refno INNER JOIN head_master ON sub_veh_exp.head_sno = head_master.sno RIGHT OUTER JOIN vehicel_master ON veh_exp.vehsno = vehicel_master.vm_sno WHERE (veh_exp.doe BETWEEN @d1 AND @d2) AND (vehicel_master.vm_owner = @Owner) AND (vehicel_master.vhtype_refno = 7) AND (head_master.account_type = 'Store Consumption') GROUP BY vehicel_master.registration_no");
                cmd.Parameters.Add("@Owner", Session["shortname"].ToString());
                cmd.Parameters.Add("@BranchID", BranchID);
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                DataTable dtstoreconsumption = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow dr in Report.Rows)
                {
                    foreach (DataRow drm in dtstoreconsumption.Rows)
                    {
                        if (dr["VehicleNo"].ToString() == drm["registration_no"].ToString())
                        {
                            double Amount = 0;
                            double.TryParse(drm["Amount"].ToString(), out Amount);
                            dr["Store Consumption"] = Math.Round(Amount, 2);
                        }
                    }
                }
                //DataTable dtEmi = vdm.SelectQuery(cmd).Tables[0];
                string strmonth = fromdate.ToString("MMM/dd/yyyy");

                cmd = new MySqlCommand("SELECT  vehicel_master.registration_no ,DATE_FORMAT( termloanentry.mfgdate, '%d %b %y') as mfgdate,DATE_FORMAT( termloanentry.termloandate, '%d %b %y') as termloandate,termloanentry.type,termloanentry.sno, termloanentry.termloanno,termloanentry.loanamount, termloanentry.instalamount,DATE_FORMAT(termloanentry.instaldate, '%d %b %y') as instaldate , termloanentry.totalinstall, termloanentry.com_install, termloanentry.bankname, vehicel_master.Capacity, minimasters.mm_name AS VehicleType, minimasters_1.mm_code AS Make FROM termloanentry INNER JOIN vehicel_master ON termloanentry.vehsno = vehicel_master.vm_sno INNER JOIN minimasters ON vehicel_master.vhtype_refno = minimasters.sno INNER JOIN minimasters minimasters_1 ON vehicel_master.vhmake_refno = minimasters_1.sno   where (vehicel_master.vm_owner=@Owner)  Group by vehicel_master.registration_no,termloanentry.Type order by termloanentry.Bankname");
                    cmd.Parameters.Add("@Owner", Session["shortname"].ToString());
                cmd.Parameters.Add("@BranchID", BranchID);
                DataTable dtTermLoans = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow dr in Report.Rows)
                {
                    foreach (DataRow drm in dtTermLoans.Rows)
                    {
                        if (dr["VehicleNo"].ToString() == drm["registration_no"].ToString())
                        {
                            double TermLoanAmount = 0;
                            if (ddltermloanType.SelectedValue == "With TL")
                            {
                                int totalinstall = 0;
                                int.TryParse(drm["totalinstall"].ToString(), out totalinstall);
                                string termloandate = drm["termloandate"].ToString();
                                string instaldate = drm["instaldate"].ToString();
                                DateTime dtinstaldate = Convert.ToDateTime(instaldate);
                                string strdate = dtinstaldate.ToString("dd/MMM/yyyy");
                                string[] strarray = strdate.Split('/');
                                string[] newmonth = strmonth.Split('/');
                                string install = strarray[0] + "/" + newmonth[0] + "/" + newmonth[2];
                                DateTime dtnewdate = Convert.ToDateTime(install);
                                DateTime dtterm = Convert.ToDateTime(termloandate);
                                TimeSpan dateSpan = dtnewdate.Subtract(dtterm);
                                int NoOfdays = dateSpan.Days;
                                int month = NoOfdays / 30;
                                int Remaining = 0;
                                Remaining = totalinstall - month;
                                if (Remaining < 0)
                                {
                                    Remaining = 0;
                                }
                                else
                                {
                                    double.TryParse(drm["instalamount"].ToString(), out TermLoanAmount);
                                    dr["TermLoan"] = TermLoanAmount;
                                }
                            }
                            else
                            {
                                TermLoanAmount = 0;

                            }
                            double Salaries = 0;
                            double.TryParse(dr["Salaries"].ToString(), out Salaries);
                            double totalExpenses = 0;
                            totalExpenses = Salaries + TermLoanAmount;
                            dr["Total Expenses"] = Math.Round(totalExpenses, 2);
                            double value = 0;
                            double.TryParse(dr["Value"].ToString(), out value);
                            double total = 0;
                            total = value - totalExpenses;
                            double gps = 572.5;
                            total = total - gps;
                            dr["Net Profit/Loss"] = Math.Round(total, 2);
                            double Runningkms = 0;
                            double.TryParse(dr["Running kms"].ToString(), out Runningkms);
                            dr["Per kms"] = Math.Round(Runningkms / total, 2);
                        }
                    }
                }
               
                    cmd = new MySqlCommand("SELECT  veh_exp.sno, veh_exp.vehsno,vehicel_master.registration_no, veh_exp.name, veh_exp.incharge, veh_exp.doe, veh_exp.branchid, Sum(sub_veh_exp.amount) as Amount, veh_exp.entry_by, veh_exp.Maintace_id, veh_exp.remarks, sub_veh_exp.head_sno FROM veh_exp INNER JOIN sub_veh_exp ON veh_exp.sno = sub_veh_exp.refno LEFT OUTER JOIN vehicel_master ON veh_exp.vehsno = vehicel_master.vm_sno WHERE (veh_exp.doe BETWEEN @d1 AND @d2) AND (vehicel_master.vm_owner = @Owner) AND (vehicel_master.vhtype_refno = 7) AND (sub_veh_exp.head_sno = 9) GROUP BY vehicel_master.registration_no");
                    cmd.Parameters.Add("@Owner", Session["shortname"].ToString());
                cmd.Parameters.Add("@BranchID", BranchID);
                cmd.Parameters.Add("@d1", GetLowDate(todate).AddMonths(-12));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                DataTable dtTax = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow dr in Report.Rows)
                {
                    foreach (DataRow drm in dtTax.Rows)
                    {
                        if (dr["VehicleNo"].ToString() == drm["registration_no"].ToString())
                        {
                            double Amount = 0;
                            double.TryParse(drm["Amount"].ToString(), out Amount);
                            Amount = Amount / 12;
                            dr["Taxes"] = Math.Round(Amount, 2);
                        }
                    }
                }

                cmd = new MySqlCommand("SELECT  veh_exp.sno, veh_exp.vehsno,vehicel_master.registration_no, veh_exp.name, veh_exp.incharge, veh_exp.doe, veh_exp.branchid, Sum(sub_veh_exp.amount) as Amount, veh_exp.entry_by, veh_exp.Maintace_id, veh_exp.remarks, sub_veh_exp.head_sno FROM veh_exp INNER JOIN sub_veh_exp ON veh_exp.sno = sub_veh_exp.refno LEFT OUTER JOIN vehicel_master ON veh_exp.vehsno = vehicel_master.vm_sno WHERE (veh_exp.doe BETWEEN @d1 AND @d2) AND (vehicel_master.vm_owner = @Owner) AND (vehicel_master.vhtype_refno = 7) AND (sub_veh_exp.head_sno = 38) GROUP BY vehicel_master.registration_no");
                    cmd.Parameters.Add("@Owner", Session["shortname"].ToString());
                cmd.Parameters.Add("@BranchID", BranchID);
                cmd.Parameters.Add("@d1", GetLowDate(todate).AddMonths(-12));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                DataTable dtInsurance = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow dr in Report.Rows)
                {
                    foreach (DataRow drm in dtInsurance.Rows)
                    {
                        if (dr["VehicleNo"].ToString() == drm["registration_no"].ToString())
                        {
                            double Amount = 0;
                            double.TryParse(drm["Amount"].ToString(), out Amount);
                            Amount = Amount / 12;
                            dr["Insurance"] = Math.Round(Amount, 2);
                        }
                    }
                }
                if (ddltermloanType.SelectedValue == "With TL")
                {
                    foreach (DataRow dr in Report.Rows)
                    {
                        if (dr["TermLoan"].ToString() == "")
                        {
                            double TermLoanAmount = 0;
                            dr["TermLoan"] = TermLoanAmount;
                            double Salaries = 0;
                            double.TryParse(dr["Salaries"].ToString(), out Salaries);
                            double repair = 0;
                            double.TryParse(dr["Repair&Maintance"].ToString(), out repair);
                            double storeconsumption = 0;
                            double.TryParse(dr["Store Consumption"].ToString(), out storeconsumption);
                            double Taxes = 0;
                            double.TryParse(dr["Taxes"].ToString(), out Taxes);
                            double Insurance = 0;
                            double.TryParse(dr["Insurance"].ToString(), out Insurance);
                            double totalExpenses = 0;
                            totalExpenses = Salaries + TermLoanAmount + repair + Taxes + Insurance + storeconsumption;
                            dr["Total Expenses"] = Math.Round(totalExpenses, 2);
                            double value = 0;
                            double.TryParse(dr["Value"].ToString(), out value);
                            double total = 0;
                            total = value - totalExpenses;
                            double gps = 572.5;
                            total = total - gps;
                            dr["Net Profit/Loss"] = Math.Round(total, 2);
                            double Runningkms = 0;
                            double.TryParse(dr["Running kms"].ToString(), out Runningkms);
                            dr["Per kms"] = Math.Round(Runningkms / total, 2);
                        }
                    }
                }
                else
                {
                    foreach (DataRow dr in Report.Rows)
                    {
                        double TermLoanAmount = 0;
                        double Salaries = 0;
                        double.TryParse(dr["Salaries"].ToString(), out Salaries);
                        double repair = 0;
                        double.TryParse(dr["Repair&Maintance"].ToString(), out repair);
                        double storeconsumption = 0;
                        double.TryParse(dr["Store Consumption"].ToString(), out storeconsumption);
                        double Taxes = 0;
                        double.TryParse(dr["Taxes"].ToString(), out Taxes);
                        double Insurance = 0;
                        double.TryParse(dr["Insurance"].ToString(), out Insurance);
                        double totalExpenses = 0;
                        totalExpenses = Salaries + TermLoanAmount + repair + Taxes + Insurance + storeconsumption;
                        dr["Total Expenses"] = Math.Round(totalExpenses, 2);
                        double value = 0;
                        double.TryParse(dr["Value"].ToString(), out value);
                        double total = 0;
                        total = value - totalExpenses;
                        double gps = 572.5;
                        total = total - gps;
                        dr["Net Profit/Loss"] = Math.Round(total, 2);
                        double Runningkms = 0;
                        double.TryParse(dr["Running kms"].ToString(), out Runningkms);
                        dr["Per kms"] = Math.Round(Runningkms / total, 2);
                    }
                }
            }

            #endregion "All Puffs"


            #region "All Tankers"
           

            if (ddlType.SelectedValue == "All Tankers")
            {
                Session["filename"] = "P&L All Tankers";

                cmd = new MySqlCommand("SELECT COUNT(vehicel_master.registration_no) AS vehicleworkingdays,vehicel_master.vm_sno, vehicel_master.registration_no, SUM(triplogs.km) AS kms, SUM(triplogs.km * triplogs.charge) AS value FROM vehicel_master INNER JOIN tripdata ON vehicel_master.vm_sno = tripdata.vehicleno INNER JOIN triplogs ON tripdata.sno = triplogs.tripsno WHERE (vehicel_master.vm_owner = @Owner) AND (vehicel_master.vhtype_refno = 22) AND (triplogs.doe BETWEEN @d1 AND @d2) GROUP BY vehicel_master.registration_no");
                    cmd.Parameters.Add("@Owner", Session["shortname"].ToString());
                    cmd.Parameters.Add("@BranchID", BranchID);
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                DataTable dtVehiclekms = vdm.SelectQuery(cmd).Tables[0];
                int i = 1;
                foreach (DataRow dr in dtVehiclekms.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["Sno"] = i++.ToString();
                    newrow["VehicleNo"] = dr["registration_no"].ToString();
                    double gps = 572.5;
                    newrow["GPS"] = gps.ToString();
                    double TripKMS = 0;
                    double.TryParse(dr["kms"].ToString(), out TripKMS);
                    TripKMS = Math.Abs(TripKMS);
                    double value = 0;
                    double.TryParse(dr["Value"].ToString(), out value);
                    int vehicleworkingdays = 0;
                    int.TryParse(dr["vehicleworkingdays"].ToString(), out vehicleworkingdays);
                    if (vehicleworkingdays > 30)
                    {
                        vehicleworkingdays = 30;
                    }
                    vehicleworkingdays = vehicleworkingdays + 1;
                    if (TripKMS == 0.0)
                    {
                        value = 0;
                    }
                    newrow["Running kms"] = TripKMS;
                    newrow["Value"] = value;
                    double sal = 0;
                    string Vehicleno = dr["registration_no"].ToString();
                    if (Vehicleno == "TN02BA9549" || Vehicleno == "TN02BB2070")
                    {
                        sal = 31 * 613.767;

                    }
                    else
                    {
                        sal = vehicleworkingdays * 613.767;
                    }
                    if (Vehicleno == "AP26TB2358" || Vehicleno == "AP26TC5769" || Vehicleno == "AP26TC5949" || Vehicleno == "AP26TC6786")
                    {
                        sal = sal + (vehicleworkingdays * 346);
                    }
                    newrow["Salaries"] = Math.Round(sal, 2);

                    Report.Rows.Add(newrow);
                }
                cmd = new MySqlCommand("SELECT SUM(triplogs.fuel) AS fuel, tripdata.DieselCost, vehicel_master.registration_no FROM vehicel_master INNER JOIN tripdata ON vehicel_master.vm_sno = tripdata.vehicleno INNER JOIN triplogs ON triplogs.tripsno = tripdata.sno WHERE (vehicel_master.vm_owner = @Owner) AND (vehicel_master.vhtype_refno = 22) AND (tripdata.tripdate BETWEEN @d1 AND @d2) GROUP BY vehicel_master.registration_no");
                cmd.Parameters.Add("@Owner", Session["shortname"].ToString());
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                DataTable dtoutdiesel = vdm.SelectQuery(cmd).Tables[0];
                cmd = new MySqlCommand("SELECT SUM(tripdata.endfuelvalue) AS endfuel, tripdata.DieselCost, vehicel_master.registration_no FROM tripdata INNER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno WHERE  (vehicel_master.vm_owner = @Owner) AND (vehicel_master.vhtype_refno = 22) AND (tripdata.tripdate BETWEEN @d1 AND @d2) GROUP BY vehicel_master.registration_no");
                cmd.Parameters.Add("@Owner", Session["shortname"].ToString());
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                DataTable dtenddiesel = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow dr in Report.Rows)
                {
                    foreach (DataRow drm in dtenddiesel.Rows)
                    {
                        if (dr["VehicleNo"].ToString() == drm["registration_no"].ToString())
                        {
                            double Diesel = 0;
                            double.TryParse(drm["endfuel"].ToString(), out Diesel);
                            double Cost = 60.2;
                            //double.TryParse(drm["DieselCost"].ToString(), out Cost);

                            double DieselCost = 0;
                            DieselCost = Diesel * Cost;
                            dr["Diesel Expenses"] = Math.Round(DieselCost, 2);
                        }
                    }
                }
                foreach (DataRow dr in Report.Rows)
                {
                    foreach (DataRow drm in dtoutdiesel.Rows)
                    {
                        if (dr["VehicleNo"].ToString() == drm["registration_no"].ToString())
                        {
                            double Diesel = 0;
                            double.TryParse(drm["fuel"].ToString(), out Diesel);
                            double Cost = 60.2;
                            //double.TryParse(drm["DieselCost"].ToString(), out Cost);
                            double DieselCost = 0;
                            DieselCost = Diesel * Cost;
                            double olddiesel = 0;
                            double.TryParse(dr["Diesel Expenses"].ToString(), out olddiesel);
                            double newdiesel = 0;
                            newdiesel = olddiesel + DieselCost;
                            dr["Diesel Expenses"] = Math.Round(newdiesel, 2);
                            double Mileage = 0;
                            double kms = 0;
                            double.TryParse(dr["Running kms"].ToString(), out kms);
                            double prediesel = 0;
                            double dieselpresent = 0;
                            prediesel = olddiesel / Cost;
                            dieselpresent = prediesel + Diesel;
                            Mileage = kms / dieselpresent;
                            dr["Avg Mileage"] = Math.Round(Mileage, 2);
                        }
                    }
                }

                cmd = new MySqlCommand("SELECT  SUM(sub_veh_exp.amount) AS Amount, vehicel_master.registration_no FROM veh_exp INNER JOIN  sub_veh_exp ON veh_exp.sno = sub_veh_exp.refno RIGHT OUTER JOIN vehicel_master ON veh_exp.vehsno = vehicel_master.vm_sno WHERE (veh_exp.doe BETWEEN @d1 AND @d2) AND (vehicel_master.vm_owner = @Owner) AND (vehicel_master.vhtype_refno = 22) AND (sub_veh_exp.head_sno <> 38) AND  (sub_veh_exp.head_sno <> 9) GROUP BY vehicel_master.registration_no");
                    cmd.Parameters.Add("@Owner", Session["shortname"].ToString());
                    cmd.Parameters.Add("@BranchID", BranchID);
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                DataTable dtmaintaince = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow dr in Report.Rows)
                {
                    foreach (DataRow drm in dtmaintaince.Rows)
                    {
                        if (dr["VehicleNo"].ToString() == drm["registration_no"].ToString())
                        {
                            double Amount = 0;
                            double.TryParse(drm["Amount"].ToString(), out Amount);
                            dr["Repair&Maintance"] = Math.Round(Amount, 2);
                        }
                    }
                }
                cmd = new MySqlCommand("SELECT SUM(sub_veh_exp.amount) AS Amount, vehicel_master.registration_no FROM veh_exp INNER JOIN sub_veh_exp ON veh_exp.sno = sub_veh_exp.refno INNER JOIN head_master ON sub_veh_exp.head_sno = head_master.sno RIGHT OUTER JOIN vehicel_master ON veh_exp.vehsno = vehicel_master.vm_sno WHERE (veh_exp.doe BETWEEN @d1 AND @d2) AND (vehicel_master.vm_owner = @Owner) AND (vehicel_master.vhtype_refno = 22) AND (head_master.account_type = 'Store Consumption') GROUP BY vehicel_master.registration_no");
                cmd.Parameters.Add("@Owner", Session["shortname"].ToString());
                cmd.Parameters.Add("@BranchID", BranchID);
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                DataTable dtstoreconsumption = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow dr in Report.Rows)
                {
                    foreach (DataRow drm in dtstoreconsumption.Rows)
                    {
                        if (dr["VehicleNo"].ToString() == drm["registration_no"].ToString())
                        {
                            double Amount = 0;
                            double.TryParse(drm["Amount"].ToString(), out Amount);
                            dr["Store Consumption"] = Math.Round(Amount, 2);
                        }
                    }
                }
                string strmonth = fromdate.ToString("MMM/dd/yyyy");

                cmd = new MySqlCommand("SELECT  vehicel_master.registration_no,DATE_FORMAT( termloanentry.mfgdate, '%d %b %y') as mfgdate,DATE_FORMAT( termloanentry.termloandate, '%d %b %y') as termloandate,termloanentry.type,termloanentry.sno, termloanentry.termloanno,termloanentry.loanamount, termloanentry.instalamount,DATE_FORMAT(termloanentry.instaldate, '%d %b %y') as instaldate , termloanentry.totalinstall, termloanentry.com_install, termloanentry.bankname, vehicel_master.Capacity, minimasters.mm_name AS VehicleType, minimasters_1.mm_code AS Make FROM termloanentry INNER JOIN vehicel_master ON termloanentry.vehsno = vehicel_master.vm_sno INNER JOIN minimasters ON vehicel_master.vhtype_refno = minimasters.sno INNER JOIN minimasters minimasters_1 ON vehicel_master.vhmake_refno = minimasters_1.sno   where (vehicel_master.vm_owner=@Owner)  Group by vehicel_master.registration_no,termloanentry.Type order by termloanentry.Bankname");
                    cmd.Parameters.Add("@Owner", Session["shortname"].ToString());
                cmd.Parameters.Add("@BranchID", BranchID);
                DataTable dtTermLoans = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow dr in Report.Rows)
                {
                    foreach (DataRow drm in dtTermLoans.Rows)
                    {
                        if (dr["VehicleNo"].ToString() == drm["registration_no"].ToString())
                        {
                            double TermLoanAmount = 0;
                            if (ddltermloanType.SelectedValue == "With TL")
                            {
                                int totalinstall = 0;
                                int.TryParse(drm["totalinstall"].ToString(), out totalinstall);
                                string termloandate = drm["termloandate"].ToString();
                                string instaldate = drm["instaldate"].ToString();
                                DateTime dtinstaldate = Convert.ToDateTime(instaldate);
                                string strdate = dtinstaldate.ToString("dd/MMM/yyyy");
                                string[] strarray = strdate.Split('/');
                                string[] newmonth = strmonth.Split('/');
                                string install = strarray[0] + "/" + newmonth[0] + "/" + newmonth[2];
                                DateTime dtnewdate = Convert.ToDateTime(install);
                                DateTime dtterm = Convert.ToDateTime(termloandate);
                                TimeSpan dateSpan = dtnewdate.Subtract(dtterm);
                                int NoOfdays = dateSpan.Days;
                                int month = NoOfdays / 30;
                                int Remaining = 0;
                                Remaining = totalinstall - month;
                                if (Remaining < 0)
                                {
                                    Remaining = 0;
                                }
                                else
                                {
                                    double.TryParse(drm["instalamount"].ToString(), out TermLoanAmount);
                                    dr["TermLoan"] = TermLoanAmount;
                                }
                            }
                            else
                            {
                                TermLoanAmount = 0;

                            }
                            double Salaries = 0;
                            double.TryParse(dr["Salaries"].ToString(), out Salaries);
                            double DieselExpenses = 0;
                            double.TryParse(dr["Diesel Expenses"].ToString(), out DieselExpenses);
                            double totalExpenses = 0;
                            totalExpenses = Salaries + TermLoanAmount + DieselExpenses;
                            dr["Total Expenses"] = Math.Round(totalExpenses, 2);
                            double value = 0;
                            double.TryParse(dr["Value"].ToString(), out value);
                            double total = 0;
                            total = value - totalExpenses;
                            double gps = 572.5;
                            total = total - gps;
                            dr["Net Profit/Loss"] = Math.Round(total, 2);
                            double Runningkms = 0;
                            double.TryParse(dr["Running kms"].ToString(), out Runningkms);
                            dr["Per kms"] = Math.Round(Runningkms / total, 2);
                        }
                    }
                }

                cmd = new MySqlCommand("SELECT  veh_exp.sno, veh_exp.vehsno,vehicel_master.registration_no, veh_exp.name, veh_exp.incharge, veh_exp.doe, veh_exp.branchid, Sum(sub_veh_exp.amount) as Amount, veh_exp.entry_by, veh_exp.Maintace_id, veh_exp.remarks, sub_veh_exp.head_sno FROM veh_exp INNER JOIN sub_veh_exp ON veh_exp.sno = sub_veh_exp.refno LEFT OUTER JOIN vehicel_master ON veh_exp.vehsno = vehicel_master.vm_sno WHERE (veh_exp.doe BETWEEN @d1 AND @d2) AND (vehicel_master.vm_owner = @Owner) AND (vehicel_master.vhtype_refno = 22) AND (sub_veh_exp.head_sno = 9) GROUP BY vehicel_master.registration_no");
                    cmd.Parameters.Add("@Owner", Session["shortname"].ToString());
                cmd.Parameters.Add("@BranchID", BranchID);
                cmd.Parameters.Add("@d1", GetLowDate(fromdate).AddMonths(-12));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                DataTable dtTax = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow dr in Report.Rows)
                {
                    foreach (DataRow drm in dtTax.Rows)
                    {
                        if (dr["VehicleNo"].ToString() == drm["registration_no"].ToString())
                        {
                            double Amount = 0;
                            double.TryParse(drm["Amount"].ToString(), out Amount);
                            Amount = Amount / 12;
                            dr["Taxes"] = Math.Round(Amount, 2);
                        }
                    }
                }
                cmd = new MySqlCommand("SELECT  veh_exp.sno, veh_exp.vehsno,vehicel_master.registration_no, veh_exp.name, veh_exp.incharge, veh_exp.doe, veh_exp.branchid, Sum(sub_veh_exp.amount) as Amount, veh_exp.entry_by, veh_exp.Maintace_id, veh_exp.remarks, sub_veh_exp.head_sno FROM veh_exp INNER JOIN sub_veh_exp ON veh_exp.sno = sub_veh_exp.refno LEFT OUTER JOIN vehicel_master ON veh_exp.vehsno = vehicel_master.vm_sno WHERE (veh_exp.doe BETWEEN @d1 AND @d2) AND (vehicel_master.vm_owner = @Owner) AND (vehicel_master.vhtype_refno = 22) AND (sub_veh_exp.head_sno = 85) GROUP BY vehicel_master.registration_no");
                cmd.Parameters.Add("@Owner", Session["shortname"].ToString());
                cmd.Parameters.Add("@BranchID", BranchID);
                cmd.Parameters.Add("@d1", GetLowDate(fromdate).AddMonths(-12));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                DataTable dtTNTax = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow dr in Report.Rows)
                {
                    foreach (DataRow drm in dtTNTax.Rows)
                    {
                        if (dr["VehicleNo"].ToString() == drm["registration_no"].ToString())
                        {
                            double Amount = 0;
                            double.TryParse(drm["Amount"].ToString(), out Amount);
                            Amount = Amount / 12;
                            dr["Taxes"] = Math.Round(Amount, 2);
                        }
                    }
                }
                cmd = new MySqlCommand("SELECT  veh_exp.sno, veh_exp.vehsno,vehicel_master.registration_no, veh_exp.name, veh_exp.incharge, veh_exp.doe, veh_exp.branchid, Sum(sub_veh_exp.amount) as Amount, veh_exp.entry_by, veh_exp.Maintace_id, veh_exp.remarks, sub_veh_exp.head_sno FROM veh_exp INNER JOIN sub_veh_exp ON veh_exp.sno = sub_veh_exp.refno LEFT OUTER JOIN vehicel_master ON veh_exp.vehsno = vehicel_master.vm_sno WHERE (veh_exp.doe BETWEEN @d1 AND @d2) AND (vehicel_master.vm_owner = @Owner) AND (vehicel_master.vhtype_refno = 22) AND (sub_veh_exp.head_sno = 38) GROUP BY vehicel_master.registration_no");
                    cmd.Parameters.Add("@Owner", Session["shortname"].ToString());
                cmd.Parameters.Add("@BranchID", BranchID);
                cmd.Parameters.Add("@d1", GetLowDate(fromdate).AddMonths(-12));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                DataTable dtInsurance = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow dr in Report.Rows)
                {
                    foreach (DataRow drm in dtInsurance.Rows)
                    {
                        if (dr["VehicleNo"].ToString() == drm["registration_no"].ToString())
                        {
                            double Amount = 0;
                            double.TryParse(drm["Amount"].ToString(), out Amount);
                            Amount = Amount / 12;
                            dr["Insurance"] = Math.Round(Amount, 2);
                        }
                    }
                }
                if (ddltermloanType.SelectedValue == "With TL")
                {
                    foreach (DataRow dr in Report.Rows)
                    {
                        if (dr["TermLoan"].ToString() == "")
                        {
                            double TermLoanAmount = 0;
                            dr["TermLoan"] = TermLoanAmount;
                            double Salaries = 0;
                            double.TryParse(dr["Salaries"].ToString(), out Salaries);
                            double DieselExpenses = 0;
                            double.TryParse(dr["Diesel Expenses"].ToString(), out DieselExpenses);
                            double repair = 0;
                            double.TryParse(dr["Repair&Maintance"].ToString(), out repair);
                            double storeconsumption = 0;
                            double.TryParse(dr["Store Consumption"].ToString(), out storeconsumption);
                            double totalExpenses = 0;
                            double Taxes = 0;
                            double.TryParse(dr["Taxes"].ToString(), out Taxes);
                            double Insurance = 0;
                            double.TryParse(dr["Insurance"].ToString(), out Insurance);
                            totalExpenses = Salaries + TermLoanAmount + repair + Taxes + Insurance + DieselExpenses + storeconsumption;
                            dr["Total Expenses"] = Math.Round(totalExpenses, 2);
                            double value = 0;
                            double.TryParse(dr["Value"].ToString(), out value);
                            double total = 0;
                            total = value - totalExpenses;
                            double gps = 572.5;
                            total = total - gps;
                            dr["Net Profit/Loss"] = Math.Round(total, 2);
                            double Runningkms = 0;
                            double.TryParse(dr["Running kms"].ToString(), out Runningkms);
                            dr["Per kms"] = Math.Round(Runningkms / total, 2);
                        }
                    }
                }
                else
                {
                    foreach (DataRow dr in Report.Rows)
                    {
                        double TermLoanAmount = 0;
                        double Salaries = 0;
                        double.TryParse(dr["Salaries"].ToString(), out Salaries);
                        double DieselExpenses = 0;
                        double.TryParse(dr["Diesel Expenses"].ToString(), out DieselExpenses);
                        double repair = 0;
                        double.TryParse(dr["Repair&Maintance"].ToString(), out repair);
                        double storeconsumption = 0;
                        double.TryParse(dr["Store Consumption"].ToString(), out storeconsumption);
                        double totalExpenses = 0;
                        double Taxes = 0;
                        double.TryParse(dr["Taxes"].ToString(), out Taxes);
                        double Insurance = 0;
                        double.TryParse(dr["Insurance"].ToString(), out Insurance);
                        totalExpenses = Salaries + TermLoanAmount + repair + Taxes + Insurance + DieselExpenses + storeconsumption;
                        dr["Total Expenses"] = Math.Round(totalExpenses, 2);
                        double value = 0;
                        double.TryParse(dr["Value"].ToString(), out value);
                        double total = 0;
                        total = value - totalExpenses;
                        double gps = 572.5;
                        total = total - gps;
                        dr["Net Profit/Loss"] = Math.Round(total, 2);
                        double Runningkms = 0;
                        double.TryParse(dr["Running kms"].ToString(), out Runningkms);
                        dr["Per kms"] = Math.Round(Runningkms / total, 2);
                    }
                }
            }

            #endregion "All Tankers" 




            DataRow New = Report.NewRow();
            New["VehicleNo"] = "Total";
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
            dataGridView1.DataSource = Report;
            dataGridView1.DataBind();
            string title = "P&L Report From: " + fromdate.ToString() + "  To: " + todate.ToString();
            Session["title"] = title;
            Session["xportdata"] = Report;
        }
        catch
        {
        }

       
    }
}