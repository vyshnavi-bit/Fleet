using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

public partial class SAP_tanker_report : System.Web.UI.Page
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
            if (!Page.IsCallback)
            {
                txt_FromDate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                txt_Todate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                lblTitle.Text = Session["TitleName"].ToString();
            }
        }
    }
    void Getroutes()
    {
        try
        {
            vdm = new VehicleDBMgr();
            lblmsg.Text = "";
            DateTime fromdate = DateTime.Now;
            DateTime todate = DateTime.Now;
            string[] datestrig = txt_FromDate.Text.Split(' ');
            if (datestrig.Length > 1)
            {
                if (datestrig[0].Split('-').Length > 0)
                {
                    string[] dates = datestrig[0].Split('-');
                    string[] times = datestrig[1].Split(':');
                    fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            datestrig = txt_Todate.Text.Split(' ');
            if (datestrig.Length > 1)
            {
                if (datestrig[0].Split('-').Length > 0)
                {
                    string[] dates = datestrig[0].Split('-');
                    string[] times = datestrig[1].Split(':');
                    todate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            cmd = new MySqlCommand("SELECT tripdata.routeid FROM tripdata INNER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno WHERE(tripdata.tripdate BETWEEN @d1 AND @d2) AND (vehicel_master.vhtype_refno = 7) AND (tripdata.userid = @BranchID) group by tripdata.routeid");
            cmd.Parameters.Add("@BranchID", Session["Branch_ID"].ToString());
            cmd.Parameters.Add("@d1", GetLowDate(fromdate).AddDays(-5));
            cmd.Parameters.Add("@d2", GetHighDate(todate).AddDays(-5));
            DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
            ddlroutes.DataSource = dttrips;
            ddlroutes.DataTextField = "routeid";
            ddlroutes.DataValueField = "routeid";
            ddlroutes.DataBind();
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        vdm = new VehicleDBMgr();
        if (ddlStatus.SelectedValue == "Own Puff SO")
        {
            hideroutes.Visible = true;
            Getroutes();
        }
        else
        {
            hideroutes.Visible = false;
        }
       
    }
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        GetReport();
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
    void GetReport()
    {
        try
        {
            lblmsg.Text = "";
            pnlHide.Visible = true;
            Session["IDate"] = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
            vdm = new VehicleDBMgr();
            DateTime fromdate = DateTime.Now;
            DateTime todate = DateTime.Now;
            string[] dateFromstrig = txt_FromDate.Text.Split(' ');
            if (dateFromstrig.Length > 1)
            {
                if (dateFromstrig[0].Split('-').Length > 0)
                {
                    string[] dates = dateFromstrig[0].Split('-');
                    string[] times = dateFromstrig[1].Split(':');
                    fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            string[] datestrig = txt_Todate.Text.Split(' ');
            if (datestrig.Length > 1)
            {
                if (datestrig[0].Split('-').Length > 0)
                {
                    string[] dates = datestrig[0].Split('-');
                    string[] times = datestrig[1].Split(':');
                    todate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            string strmonth = fromdate.ToString("MMM/dd/yyyy");

            lbl_selfromdate.Text = fromdate.ToString("dd/MM/yyyy");
            Session["xporttype"] = "TallyTankers";
            Report.Columns.Add("JV No.");
            Report.Columns.Add("JV Date");
            Report.Columns.Add("WH Code");
            Report.Columns.Add("Ledger Code");
            Report.Columns.Add("Ledger Name");
            Report.Columns.Add("Amount");
            Report.Columns.Add("Narration");
            if (ddlStatus.SelectedItem.Text == "Own Tanker")
            {
                Session["filename"] = " Tally OwnTankers" + todate.ToString("dd/MM/yyyy");
                //cmd = new MySqlCommand("SELECT COUNT(vehicel_master.registration_no) AS vehicleworkingdays,vehicel_master.ledgername,vehicel_master.ledger_code, vehicel_master.vm_sno, vehicel_master.registration_no, SUM(triplogs.km) AS kms, SUM(triplogs.km * triplogs.charge) AS value FROM vehicel_master INNER JOIN tripdata ON vehicel_master.vm_sno = tripdata.vehicleno INNER JOIN triplogs ON tripdata.sno = triplogs.tripsno WHERE (vehicel_master.vm_owner = @Owner) AND (vehicel_master.vhtype_refno = 22) AND (triplogs.doe BETWEEN @d1 AND @d2) GROUP BY vehicel_master.registration_no");
                cmd = new MySqlCommand("SELECT COUNT(vehicel_master.registration_no) AS vehicleworkingdays, vehicel_master.ledgername, vehicel_master.ledger_code, vehicel_master.vm_sno, vehicel_master.registration_no, SUM(triplogs.km) AS kms, SUM(triplogs.km * triplogs.charge) AS value, branch_info.whcode FROM  vehicel_master INNER JOIN tripdata ON vehicel_master.vm_sno = tripdata.vehicleno INNER JOIN triplogs ON tripdata.sno = triplogs.tripsno INNER JOIN branch_info ON vehicel_master.branch_id = branch_info.brnch_sno WHERE        (vehicel_master.vm_owner = @Owner) AND (vehicel_master.vhtype_refno = 22) AND (triplogs.doe BETWEEN @d1 AND @d2) GROUP BY vehicel_master.registration_no, branch_info.whcode");
                cmd.Parameters.Add("@Owner", Session["shortname"].ToString());
                cmd.Parameters.Add("@BranchID", 1);
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                DataTable dtble = vdm.SelectQuery(cmd).Tables[0];
                double totamount = 0;
                fromdate = fromdate.AddDays(-1);
                string frmdate = fromdate.ToString("dd-MM-yyyy");
                string[] strjv = frmdate.Split('-');

                foreach (DataRow branch in dtble.Rows)
                {

                    DataRow newrow = Report.NewRow();
                    newrow["JV No."] = "TANKER TRANS OWN- " + strjv[1];
                    newrow["JV Date"] = todate.ToString("dd-MMM-yyyy");
                    newrow["WH Code"] = branch["whcode"].ToString();
                    newrow["Ledger Code"] = branch["ledger_code"].ToString();
                    newrow["Ledger Name"] = branch["ledgername"].ToString();
                    double amount = 0;
                    double.TryParse(branch["value"].ToString(), out amount);
                    totamount += amount;
                    newrow["Amount"] = "-" + amount;
                    newrow["Narration"] = "Being the tanker transportation amount credited  " + branch["registration_no"].ToString() + " for the month of " + todate.ToString("MMM-yyyy") + " Amount " + amount + " Vehicle Number " + branch["registration_no"].ToString() + ",Emp Name  " + Session["employname"].ToString();
                    Report.Rows.Add(newrow);
                    //string ledgercode = "";
                    DataRow newrow1 = Report.NewRow();
                    newrow1["JV No."] = "TANKER TRANS OWN- " + strjv[1];
                    newrow1["JV Date"] = todate.ToString("dd-MMM-yyyy");
                    newrow1["WH Code"] = "SVDSPP02";
                    newrow1["Ledger Code"] = "5123008";
                    newrow1["Ledger Name"] = "Tankers Transportation Own-Pbk";
                    newrow1["Amount"] = amount;
                    newrow1["Narration"] = "Being the tanker transportation amount credited  " + branch["registration_no"].ToString() + " for the month of " + todate.ToString("MMM-yyyy") + " Amount " + amount + " Vehicle Number " + branch["registration_no"].ToString() + ",Emp Name  " + Session["employname"].ToString();
                    Report.Rows.Add(newrow1);
                }
            }
            if (ddlStatus.SelectedItem.Text == "Others Tanker")
            {
                Session["filename"] = " Tally OtherTankers" + todate.ToString("dd/MM/yyyy");
                //cmd = new MySqlCommand("SELECT COUNT(vehicel_master.registration_no) AS vehicleworkingdays,vehicel_master.ledgername,vehicel_master.ledger_code,vehicel_master.vm_sno, vehicel_master.registration_no, SUM(triplogs.km) AS kms, SUM(triplogs.km * triplogs.charge) AS value FROM vehicel_master INNER JOIN tripdata ON vehicel_master.vm_sno = tripdata.vehicleno INNER JOIN triplogs ON tripdata.sno = triplogs.tripsno WHERE (vehicel_master.vm_owner <> @Owner) AND (vehicel_master.vhtype_refno = 22) AND (triplogs.doe BETWEEN @d1 AND @d2) GROUP BY vehicel_master.registration_no");
                cmd = new MySqlCommand("SELECT COUNT(vehicel_master.registration_no) AS vehicleworkingdays,vehicel_master.ledgername,vehicel_master.ledger_code,vehicel_master.vm_sno, vehicel_master.registration_no, SUM(triplogs.km) AS kms, SUM(triplogs.km * triplogs.charge) AS value FROM vehicel_master INNER JOIN tripdata ON vehicel_master.vm_sno = tripdata.vehicleno INNER JOIN triplogs ON tripdata.sno = triplogs.tripsno WHERE (vehicel_master.vm_owner <> @Owner) AND (vehicel_master.vhtype_refno = 22) AND (triplogs.doe BETWEEN @d1 AND @d2) GROUP BY vehicel_master.registration_no");
                cmd.Parameters.Add("@Owner", Session["shortname"].ToString());
                cmd.Parameters.Add("@BranchID", 1);
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                DataTable dtble = vdm.SelectQuery(cmd).Tables[0];
                double totamount = 0;
                fromdate = fromdate.AddDays(-1);
                string frmdate = fromdate.ToString("dd-MM-yyyy");
                string[] strjv = frmdate.Split('-');

                foreach (DataRow branch in dtble.Rows)
                {

                    DataRow newrow = Report.NewRow();
                    newrow["JV No."] = "TANKER TRANS - " + strjv[1];
                    newrow["JV Date"] = todate.ToString("dd-MMM-yyyy");
                    newrow["Ledger Code"] = branch["ledger_code"].ToString();
                    newrow["Ledger Name"] = branch["ledgername"].ToString();
                    double amount = 0;
                    double.TryParse(branch["value"].ToString(), out amount);
                    totamount += amount;
                    if (amount == 0)
                    {
                        newrow["Amount"] = 0;
                    }
                    else
                    {
                        newrow["Amount"] = "-" + amount;
                    }
                    newrow["Narration"] = "Being the tanker transportation amount credited  " + branch["registration_no"].ToString() + " for the month of " + todate.ToString("MMM-yyyy") + " Amount " + amount + " Vehicle Number " + branch["registration_no"].ToString() + ",Emp Name  " + Session["employname"].ToString();
                    Report.Rows.Add(newrow);

                    DataRow newrow1 = Report.NewRow();
                    newrow1["JV No."] = "TANKER TRANS - " + strjv[1];
                    newrow1["JV Date"] = todate.ToString("dd-MMM-yyyy");
                    newrow1["Ledger Code"] = branch["ledger_code"].ToString();
                    newrow1["Ledger Name"] = branch["ledgername"].ToString();
                    newrow1["Amount"] = amount;
                    newrow1["Narration"] = "Being the tanker transportation amount credited  " + branch["registration_no"].ToString() + " for the month of " + todate.ToString("MMM-yyyy") + " Amount " + amount + " Vehicle Number " + branch["registration_no"].ToString() + ",Emp Name  " + Session["employname"].ToString();
                    Report.Rows.Add(newrow1);
                }
            }
            if (ddlStatus.SelectedItem.Text == "Tanker Diesel")
            {
                Session["filename"] = " Tally TankerDiesel" + todate.ToString("dd/MM/yyyy");
                DataTable newReport = new DataTable();
                newReport.Columns.Add("VehicleNo");
                newReport.Columns.Add("WH Code");
                newReport.Columns.Add("Ledger Code");
                newReport.Columns.Add("LedgerName");
                newReport.Columns.Add("Infuel");
                newReport.Columns.Add("outfuel");
                newReport.Columns.Add("Price");
               // cmd = new MySqlCommand("SELECT SUM(triplogs.fuel) AS fuel, tripdata.DieselCost,vehicel_master.ledgername,vehicel_master.ledger_code, vehicel_master.registration_no FROM vehicel_master INNER JOIN tripdata ON vehicel_master.vm_sno = tripdata.vehicleno INNER JOIN triplogs ON triplogs.tripsno = tripdata.sno WHERE (vehicel_master.vm_owner = @Owner) AND (vehicel_master.vhtype_refno = 22) AND (tripdata.tripdate BETWEEN @d1 AND @d2) GROUP BY vehicel_master.registration_no");
                cmd = new MySqlCommand("SELECT SUM(triplogs.fuel) AS fuel, tripdata.DieselCost, vehicel_master.ledgername, vehicel_master.ledger_code, vehicel_master.registration_no, branch_info.whcode FROM  vehicel_master INNER JOIN tripdata ON vehicel_master.vm_sno = tripdata.vehicleno INNER JOIN triplogs ON triplogs.tripsno = tripdata.sno INNER JOIN branch_info ON vehicel_master.branch_id = branch_info.brnch_sno WHERE (vehicel_master.vm_owner = @Owner) AND (vehicel_master.vhtype_refno = 22) AND (tripdata.tripdate BETWEEN @d1 AND @d2) GROUP BY vehicel_master.registration_no, branch_info.whcode");
                cmd.Parameters.Add("@Owner", Session["shortname"].ToString());
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                DataTable dtoutdiesel = vdm.SelectQuery(cmd).Tables[0];
                cmd = new MySqlCommand("SELECT SUM(tripdata.endfuelvalue) AS endfuel, tripdata.DieselCost, vehicel_master.registration_no FROM tripdata INNER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno WHERE  (vehicel_master.vm_owner = @Owner) AND (vehicel_master.vhtype_refno = 22) AND (tripdata.tripdate BETWEEN @d1 AND @d2) GROUP BY vehicel_master.registration_no");
                cmd.Parameters.Add("@Owner", Session["shortname"].ToString());
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                DataTable dtenddiesel = vdm.SelectQuery(cmd).Tables[0];
                double totamount = 0;
                fromdate = fromdate.AddDays(-1);
                string frmdate = fromdate.ToString("dd-MM-yyyy");
                string[] strjv = frmdate.Split('-');
                foreach (DataRow dr in dtoutdiesel.Rows)
                {
                    DataRow newrow = newReport.NewRow();
                    newrow["VehicleNo"] = dr["registration_no"].ToString();
                    newrow["WH Code"] = dr["whcode"].ToString();
                    newrow["Ledger Code"] = dr["ledger_code"].ToString();
                    newrow["LedgerName"] = dr["ledgername"].ToString();
                    newReport.Rows.Add(newrow);
                }
                foreach (DataRow dr in newReport.Rows)
                {
                    foreach (DataRow drm in dtenddiesel.Rows)
                    {
                        if (dr["VehicleNo"].ToString() == drm["registration_no"].ToString())
                        {
                            double Diesel = 0;
                            double.TryParse(drm["endfuel"].ToString(), out Diesel);
                            dr["Infuel"] = Math.Round(Diesel, 2);
                        }
                    }
                }
                foreach (DataRow dr in newReport.Rows)
                {
                    foreach (DataRow drm in dtoutdiesel.Rows)
                    {
                        if (dr["VehicleNo"].ToString() == drm["registration_no"].ToString())
                        {
                            double Diesel = 0;
                            double.TryParse(drm["fuel"].ToString(), out Diesel);
                            dr["outfuel"] = Math.Round(Diesel, 2);
                        }
                    }
                }
                foreach (DataRow branch in newReport.Rows)
                {

                    DataRow newrow = Report.NewRow();
                    newrow["JV No."] = "TANKER DIESEL - " + strjv[1];
                    newrow["JV Date"] = todate.ToString("dd-MMM-yyyy");
                    newrow["WH Code"] = branch["WH Code"].ToString();
                    newrow["Ledger Code"] = branch["Ledger Code"].ToString();
                    newrow["Ledger Name"] = branch["LedgerName"].ToString();
                    double Infuel = 0;
                    double.TryParse(branch["Infuel"].ToString(), out Infuel);
                    double outfuel = 0;
                    double.TryParse(branch["outfuel"].ToString(), out outfuel);
                    double tot_fuel = 0;
                    tot_fuel = Infuel + outfuel;
                    double fuelcost = 57.91;
                    double tot_dieselvalue = 0;
                    tot_dieselvalue = tot_fuel * fuelcost;
                    tot_dieselvalue = Math.Round(tot_dieselvalue, 2);
                    totamount += tot_dieselvalue;
                    newrow["Amount"] = "-" + tot_dieselvalue;
                    newrow["Narration"] = "Being the diesel filling  qty   " + branch["VehicleNo"].ToString() + " for the month of " + todate.ToString("MMM-yyyy") + " Amount " + tot_dieselvalue + " Vehicle Number " + branch["VehicleNo"].ToString() + ",Emp Name  " + Session["employname"].ToString();
                    Report.Rows.Add(newrow);

                    DataRow newrow1 = Report.NewRow();
                    newrow1["JV No."] = "TANKER DIESEL - " + strjv[1];
                    newrow1["JV Date"] = todate.ToString("dd-MMM-yyyy");
                    newrow1["WH Code"] = branch["WH Code"].ToString();
                    newrow1["Ledger Code"] = branch["Ledger Code"].ToString();
                    newrow1["Ledger Name"] = branch["LedgerName"].ToString();
                    newrow1["Amount"] = tot_dieselvalue;
                    newrow1["Narration"] = "Being the diesel filling  qty   " + branch["VehicleNo"].ToString() + " for the month of " + todate.ToString("MMM-yyyy") + " Amount " + tot_dieselvalue + " Vehicle Number " + branch["VehicleNo"].ToString() + ",Emp Name  " + Session["employname"].ToString();
                    Report.Rows.Add(newrow1);
                }

            }
            if (ddlStatus.SelectedItem.Text == "Puff Diesel")
            {
                Session["filename"] = " Tally PuffDiesel" + todate.ToString("dd/MM/yyyy");
                DataTable newReport = new DataTable();
                newReport.Columns.Add("VehicleNo");
                newReport.Columns.Add("WH Code");
                newReport.Columns.Add("LedgerCode");
                newReport.Columns.Add("LedgerName");
                newReport.Columns.Add("Infuel");
                newReport.Columns.Add("outfuel");
                newReport.Columns.Add("Price");
                cmd = new MySqlCommand("SELECT SUM(triplogs.fuel) AS fuel,tripdata.routeid, tripdata.DieselCost,vehicel_master.ledgername,vehicel_master.ledger_code, vehicel_master.registration_no FROM vehicel_master INNER JOIN tripdata ON vehicel_master.vm_sno = tripdata.vehicleno INNER JOIN triplogs ON triplogs.tripsno = tripdata.sno WHERE (vehicel_master.vm_owner = @Owner) AND (vehicel_master.vhtype_refno = 7) AND (tripdata.tripdate BETWEEN @d1 AND @d2) GROUP BY vehicel_master.registration_no");
                //cmd = new MySqlCommand("SELECT SUM(triplogs.fuel) AS fuel, tripdata.routeid, tripdata.DieselCost, vehicel_master.ledgername, vehicel_master.ledger_code, vehicel_master.registration_no, branch_info.whcode FROM  vehicel_master INNER JOIN tripdata ON vehicel_master.vm_sno = tripdata.vehicleno INNER JOIN triplogs ON triplogs.tripsno = tripdata.sno INNER JOIN branch_info ON vehicel_master.branch_id = branch_info.brnch_sno WHERE (vehicel_master.vm_owner = @Owner) AND (vehicel_master.vhtype_refno = 7) AND (tripdata.tripdate BETWEEN @d1 AND @d2) GROUP BY vehicel_master.registration_no, branch_info.whcode");
                cmd.Parameters.Add("@Owner", Session["shortname"].ToString());
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                DataTable dtoutdiesel = vdm.SelectQuery(cmd).Tables[0];
                cmd = new MySqlCommand("SELECT SUM(tripdata.endfuelvalue) AS endfuel, tripdata.DieselCost, vehicel_master.registration_no FROM tripdata INNER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno WHERE  (vehicel_master.vm_owner = @Owner) AND (vehicel_master.vhtype_refno = 7) AND (tripdata.tripdate BETWEEN @d1 AND @d2) GROUP BY vehicel_master.registration_no");
                cmd.Parameters.Add("@Owner", Session["shortname"].ToString());
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                DataTable dtenddiesel = vdm.SelectQuery(cmd).Tables[0];
                double totamount = 0;
                fromdate = fromdate.AddDays(-1);
                string frmdate = fromdate.ToString("dd-MM-yyyy");
                string[] strjv = frmdate.Split('-');
                cmd = new MySqlCommand("SELECT sno, routename, status, branch_id, operatedby, ledgername,ledgercode,whcode FROM routes");
                DataTable dtroutes = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow dr in dtoutdiesel.Rows)
                {
                    DataRow newrow = newReport.NewRow();
                    newrow["VehicleNo"] = dr["registration_no"].ToString();
                    string route = dr["routeid"].ToString();
                    foreach (DataRow drroute in dtroutes.Select("routename='" + route + "'"))
                    {
                        newrow["LedgerCode"] = drroute["ledgercode"].ToString();
                        newrow["LedgerName"] = drroute["ledgername"].ToString();
                        newrow["WH Code"] = drroute["whcode"].ToString();
                    }
                    newReport.Rows.Add(newrow);
                }
                foreach (DataRow dr in newReport.Rows)
                {
                    foreach (DataRow drm in dtenddiesel.Rows)
                    {
                        if (dr["VehicleNo"].ToString() == drm["registration_no"].ToString())
                        {
                            double Diesel = 0;
                            double.TryParse(drm["endfuel"].ToString(), out Diesel);
                            dr["Infuel"] = Math.Round(Diesel, 2);
                        }
                    }
                }
                foreach (DataRow dr in newReport.Rows)
                {
                    foreach (DataRow drm in dtoutdiesel.Rows)
                    {
                        if (dr["VehicleNo"].ToString() == drm["registration_no"].ToString())
                        {
                            double Diesel = 0;
                            double.TryParse(drm["fuel"].ToString(), out Diesel);
                            dr["outfuel"] = Math.Round(Diesel, 2);
                        }
                    }
                }
                foreach (DataRow branch in newReport.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["JV No."] = "PUFF DIESEL - " + strjv[1];
                    newrow["JV Date"] = todate.ToString("dd-MMM-yyyy");
                    newrow["Ledger Code"] = branch["ledgercode"].ToString();
                    newrow["Ledger Name"] = branch["LedgerName"].ToString();
                    newrow["WH Code"] = branch["WH Code"].ToString();
                   
                    double Infuel = 0;
                    double.TryParse(branch["Infuel"].ToString(), out Infuel);
                    double outfuel = 0;
                    double.TryParse(branch["outfuel"].ToString(), out outfuel);
                    double tot_fuel = 0;
                    tot_fuel = Infuel + outfuel;
                    double fuelcost = 57.91;
                    double tot_dieselvalue = 0;
                    tot_dieselvalue = tot_fuel * fuelcost;
                    tot_dieselvalue = Math.Round(tot_dieselvalue, 2);
                    totamount += tot_dieselvalue;
                    newrow["Amount"] = "-" + tot_dieselvalue;
                    newrow["Narration"] = "Being the diesel filling  qty   " + branch["VehicleNo"].ToString() + " for the month of " + todate.ToString("MMM-yyyy") + " Amount " + tot_dieselvalue + " Vehicle Number " + branch["VehicleNo"].ToString() + ",Emp Name  " + Session["employname"].ToString();
                    Report.Rows.Add(newrow);

                    DataRow newrow1 = Report.NewRow();
                    newrow1["JV No."] = "PUFF DIESEL - " + strjv[1];
                    newrow1["JV Date"] = todate.ToString("dd-MMM-yyyy");
                    newrow1["Ledger Code"] = branch["ledgercode"].ToString();
                    newrow1["Ledger Name"] = branch["LedgerName"].ToString();
                    newrow1["WH Code"] = branch["WH Code"].ToString();
                    newrow1["Amount"] = tot_dieselvalue;
                    newrow1["Narration"] = "Being the diesel filling  qty   " + branch["VehicleNo"].ToString() + " for the month of " + todate.ToString("MMM-yyyy") + " Amount " + tot_dieselvalue + " Vehicle Number " + branch["VehicleNo"].ToString() + ",Emp Name  " + Session["employname"].ToString();
                    Report.Rows.Add(newrow1);
                }
            }
            if (ddlStatus.SelectedItem.Text == "Own Puff Plant")
            {
                Session["filename"] = " Tally OwnPuffs" + todate.ToString("dd/MM/yyyy");
               // cmd = new MySqlCommand("SELECT COUNT(vehicel_master.registration_no) AS vehicleworkingdays, vehicel_master.ledgername,vehicel_master.ledger_code,vehicel_master.vm_sno, vehicel_master.registration_no, tripdata.Rent, tripdata.routeid FROM vehicel_master INNER JOIN tripdata ON vehicel_master.vm_sno = tripdata.vehicleno WHERE (vehicel_master.vm_owner = @Owner) AND (vehicel_master.vhtype_refno = 7) AND (vehicel_master.branch_id = 1) AND (tripdata.tripdate BETWEEN @d1 AND @d2)  GROUP BY vehicel_master.registration_no, tripdata.routeid");
                cmd = new MySqlCommand("SELECT  COUNT(vehicel_master.registration_no) AS vehicleworkingdays, vehicel_master.ledgername, vehicel_master.ledger_code, vehicel_master.vm_sno, vehicel_master.registration_no, tripdata.Rent, tripdata.routeid,branch_info.whcode FROM  vehicel_master INNER JOIN tripdata ON vehicel_master.vm_sno = tripdata.vehicleno INNER JOIN branch_info ON vehicel_master.branch_id = branch_info.brnch_sno WHERE (vehicel_master.vm_owner = @Owner) AND (vehicel_master.vhtype_refno = 7) AND (tripdata.tripdate BETWEEN @d1 AND @d2) AND (vehicel_master.branch_id = 1) GROUP BY vehicel_master.registration_no, tripdata.routeid, branch_info.whcode");
                cmd.Parameters.Add("@Owner", Session["shortname"].ToString());
                cmd.Parameters.Add("@BranchID", 1);
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                DataTable dtble = vdm.SelectQuery(cmd).Tables[0];
                double totamount = 0;
                fromdate = fromdate.AddDays(-1);
                string frmdate = fromdate.ToString("dd-MM-yyyy");
                string[] strjv = frmdate.Split('-');

                foreach (DataRow branch in dtble.Rows)
                {

                    DataRow newrow = Report.NewRow();
                    newrow["JV No."] = "PUFF TRANS - " + strjv[1];
                    newrow["JV Date"] = todate.ToString("dd-MMM-yyyy");
                    newrow["WH Code"] = branch["whcode"].ToString();
                    newrow["Ledger Name"] = branch["routeid"].ToString();
                    double vehicleworkingdays = 0;
                    double.TryParse(branch["vehicleworkingdays"].ToString(), out vehicleworkingdays);
                    double Rent = 0;
                    double.TryParse(branch["Rent"].ToString(), out Rent);
                    double amount = 0;
                    amount = vehicleworkingdays * Rent;
                    amount = Math.Round(amount, 2);
                    totamount += amount;
                    newrow["Amount"] = amount;
                    newrow["Narration"] = "Being the puff transportation amount credited  " + branch["registration_no"].ToString() + " Route Name " + branch["routeid"].ToString() + " for the month of " + todate.ToString("MMM-yyyy") + " Per day Rs. " + Rent + " /-* " + vehicleworkingdays + " Amount " + amount + " Vehicle Number " + branch["registration_no"].ToString() + ",Emp Name  " + Session["employname"].ToString();
                    Report.Rows.Add(newrow);

                    DataRow newrow1 = Report.NewRow();
                    newrow1["JV No."] = "PUFF TRANS - " + strjv[1];
                    newrow1["JV Date"] = todate.ToString("dd-MMM-yyyy");
                    newrow1["WH Code"] = branch["whcode"].ToString();
                    newrow1["Ledger Code"] = branch["ledger_code"].ToString();
                    newrow1["Ledger Name"]=branch["ledgername"].ToString();
                   // newrow1["Ledger Name"] = Session["shortname"].ToString() + " PUFF " + branch["registration_no"].ToString();
                    newrow1["Amount"] = "-" + amount;
                    newrow1["Narration"] = "Being the puff transportation amount credited  " + branch["registration_no"].ToString() + " Route Name " + branch["routeid"].ToString() + " for the month of " + todate.ToString("MMM-yyyy") + " Per day Rs. " + Rent + " /-* " + vehicleworkingdays + " Amount " + amount + " Vehicle Number " + branch["registration_no"].ToString() + ",Emp Name  " + Session["employname"].ToString();
                    Report.Rows.Add(newrow1);
                }
            }
            if (ddlStatus.SelectedItem.Text == "Own Puff SO")
            {
                Session["filename"] = " Tally OwnPuffs" + todate.ToString("dd/MM/yyyy");
                //cmd = new MySqlCommand("SELECT COUNT(vehicel_master.registration_no) AS vehicleworkingdays,vehicel_master.ledgername,vehicel_master.ledger_code, vehicel_master.vm_sno, vehicel_master.registration_no, tripdata.Rent, tripdata.routeid FROM vehicel_master INNER JOIN tripdata ON vehicel_master.vm_sno = tripdata.vehicleno WHERE (vehicel_master.vm_owner = @Owner) AND (vehicel_master.vhtype_refno = 7) AND (vehicel_master.branch_id = 1) AND (tripdata.tripdate BETWEEN @d1 AND @d2)  GROUP BY  tripdata.routeid");
                cmd = new MySqlCommand("SELECT COUNT(vehicel_master.registration_no) AS vehicleworkingdays, vehicel_master.ledgername, vehicel_master.ledger_code, vehicel_master.vm_sno, vehicel_master.registration_no, tripdata.Rent, tripdata.routeid, branch_info.whcode FROM  vehicel_master INNER JOIN tripdata ON vehicel_master.vm_sno = tripdata.vehicleno INNER JOIN branch_info ON vehicel_master.branch_id = branch_info.brnch_sno WHERE (vehicel_master.vm_owner = @Owner) AND (vehicel_master.vhtype_refno = 7) AND (tripdata.tripdate BETWEEN @d1 AND @d2) AND (vehicel_master.branch_id = 1) AND (tripdata.routeid=@routename) GROUP BY tripdata.routeid, branch_info.whcode");
                cmd.Parameters.Add("@Owner", Session["shortname"].ToString());
                cmd.Parameters.Add("@BranchID", 1);
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@routename", ddlroutes.SelectedValue);
                DataTable dtble = vdm.SelectQuery(cmd).Tables[0];
                cmd = new MySqlCommand("SELECT sno, routename, status, branch_id, operatedby, ledgername,ledgercode,whcode FROM routes");
                DataTable dtroutes = vdm.SelectQuery(cmd).Tables[0];
                double totamount = 0;
                fromdate = fromdate.AddDays(-1);
                string frmdate = fromdate.ToString("dd-MM-yyyy");
                string[] strjv = frmdate.Split('-');
                string whcode = "";
                foreach (DataRow branch in dtble.Rows)
                {

                    DataRow newrow = Report.NewRow();
                    newrow["JV No."] = "PUFF TRANS - " + strjv[1];
                    newrow["JV Date"] = todate.ToString("dd-MMM-yyyy");
                    foreach (DataRow drroute in dtroutes.Select("routename='" + branch["routeid"].ToString() + "'"))
                    {
                        whcode = drroute["whcode"].ToString();
                    }
                    newrow["WH Code"] = whcode;
                    newrow["Ledger Name"] = "Sales transportation-" + branch["routeid"].ToString();
                    newrow["Ledger Code"] = "5134002";
                    double vehicleworkingdays = 0;
                    double.TryParse(branch["vehicleworkingdays"].ToString(), out vehicleworkingdays);
                    double Rent = 0;
                    double.TryParse(branch["Rent"].ToString(), out Rent);
                    double amount = 0;
                    amount = vehicleworkingdays * Rent;
                    amount = Math.Round(amount, 2);
                    totamount += amount;
                    newrow["Amount"] = amount;
                    newrow["Narration"] = "Being the puff transportation amount credited  " + branch["registration_no"].ToString() + " Route Name " + branch["routeid"].ToString() + " for the month of " + todate.ToString("MMM-yyyy") + " Per day Rs. " + Rent + " /-* " + vehicleworkingdays + " Amount " + amount + " Vehicle Number " + branch["registration_no"].ToString() + ",Emp Name  " + Session["employname"].ToString();
                    Report.Rows.Add(newrow);

                    DataRow newrow1 = Report.NewRow();
                    newrow1["JV No."] = "PUFF TRANS - " + strjv[1];
                    newrow1["JV Date"] = todate.ToString("dd-MMM-yyyy");
                    newrow1["WH Code"] = branch["whcode"].ToString();
                    newrow1["Ledger Code"] = branch["ledger_code"].ToString();
                    newrow1["Ledger Name"] = branch["ledgername"].ToString();
                    newrow1["Amount"] = "-" + amount;
                    newrow1["Narration"] = "Being the puff transportation amount credited  " + branch["registration_no"].ToString() + " Route Name " + branch["routeid"].ToString() + " for the month of " + todate.ToString("MMM-yyyy") + " Per day Rs. " + Rent + " /-* " + vehicleworkingdays + " Amount " + amount + " Vehicle Number " + branch["registration_no"].ToString() + ",Emp Name  " + Session["employname"].ToString();
                    Report.Rows.Add(newrow1);
                }
            }
            grdReports.DataSource = Report;
            grdReports.DataBind();
            Session["xportdata"] = Report;
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            grdReports.DataSource = Report;
            grdReports.DataBind();
        }
    }
    SqlCommand sqlcmd;
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            vdm = new VehicleDBMgr();
            DateTime CreateDate = VehicleDBMgr.GetTime(vdm.conn);
            SAPdbmanger SAPvdm = new SAPdbmanger();
            DateTime fromdate = DateTime.Now;
            DataTable dt = (DataTable)Session["xportdata"];
            string[] dateFromstrig = txt_FromDate.Text.Split(' ');
            if (dateFromstrig.Length > 1)
            {
                if (dateFromstrig[0].Split('-').Length > 0)
                {
                    string[] dates = dateFromstrig[0].Split('-');
                    string[] times = dateFromstrig[1].Split(':');
                    fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            foreach (DataRow dr in dt.Rows)
            {
                string AcctCode = dr["Ledger Code"].ToString();
                string WHcode = dr["WH Code"].ToString();
                if (AcctCode == "" || WHcode == "")
                {
                }
                else
                {
                    sqlcmd = new SqlCommand("Insert into EMROJDT (CreateDate, RefDate, DocDate, TransNo, AcctCode, AcctName, Debit, Credit, B1Upload, Processed,Ref1) values (@CreateDate, @RefDate, @DocDate,@TransNo, @AcctCode, @AcctName, @Debit, @Credit, @B1Upload, @Processed,@Ref1)");
                    sqlcmd.Parameters.Add("@CreateDate", GetLowDate(fromdate));
                    sqlcmd.Parameters.Add("@RefDate", GetLowDate(fromdate));
                    sqlcmd.Parameters.Add("@docdate", GetLowDate(fromdate));
                    sqlcmd.Parameters.Add("@Ref1", dr["JV No"].ToString());
                    int TransNo = 1;
                    sqlcmd.Parameters.Add("@TransNo", TransNo);
                    sqlcmd.Parameters.Add("@AcctCode", dr["Ledger Code"].ToString());
                    sqlcmd.Parameters.Add("@AcctName", dr["Ledger Name"].ToString());
                    double amount = 0;
                    double.TryParse(dr["Amount"].ToString(), out amount);
                    if (amount < 0)
                    {
                        amount = Math.Abs(amount);
                        double Debit = 0;
                        sqlcmd.Parameters.Add("@Debit", Debit);
                        sqlcmd.Parameters.Add("@Credit", amount);
                    }
                    else
                    {
                        amount = Math.Abs(amount);
                        double Credit = 0;
                        sqlcmd.Parameters.Add("@Debit", amount);
                        sqlcmd.Parameters.Add("@Credit", Credit);
                    }
                    string B1Upload = "N";
                    string Processed = "N";
                    sqlcmd.Parameters.Add("@B1Upload", B1Upload);
                    sqlcmd.Parameters.Add("@Processed", Processed);
                    SAPvdm.insert(sqlcmd);
                }
            }
            pnlHide.Visible = false;
            DataTable dtempty = new DataTable();
            grdReports.DataSource = dtempty;
            grdReports.DataBind();
            lblmsg.Text = "Successfully Saved";
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.ToString();
        }
    }
}
