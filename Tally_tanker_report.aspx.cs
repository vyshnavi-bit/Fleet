using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class Tally_tanker_report : System.Web.UI.Page
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
        //UserName = Session["field1"].ToString();
        //vdm = new VehicleDBMgr();
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
            cmd = new MySqlCommand("SELECT tripdata.routeid FROM tripdata INNER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno WHERE(tripdata.tripdate BETWEEN @d1 AND @d2) AND (vehicel_master.vhtype_refno = 7) AND (tripdata.userid = @BranchID)");
            cmd.Parameters.Add("@BranchID", Session["Branch_ID"].ToString());
            cmd.Parameters.Add("@d1", GetLowDate(fromdate).AddDays(-5));
            cmd.Parameters.Add("@d2", GetHighDate(todate).AddDays(-5));
            DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
            ddlroutes.DataSource = dttrips;
            ddlroutes.DataTextField = "routeid";
            ddlroutes.DataValueField = "routeid";
            ddlroutes.DataBind();
            ddlroutes.ClearSelection();
            ddlroutes.Items.Insert(0, new ListItem { Value = "ALL", Text = "ALL", Selected = true });
            ddlroutes.SelectedValue = "ALL";
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        vdm = new VehicleDBMgr();
        if (ddlStatus.SelectedValue == "Own Puff Plant")
        {
            hideroutes.Visible = true;
            hidevehicles.Visible = true;
            Getroutes();
            cmd = new MySqlCommand("SELECT minimasters.mm_name, vehicel_master.registration_no, vehicel_master.vm_sno FROM vehicel_master INNER JOIN minimasters ON vehicel_master.vhtype_refno = minimasters.sno WHERE(minimasters.mm_name = @Tanker) AND (vehicel_master.branch_id = @BranchID)");
            cmd.Parameters.Add("@BranchID", Session["Branch_ID"].ToString());
            cmd.Parameters.Add("@Tanker", "Puff");
            DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
            ddlvehicles.DataSource = dttrips;
            ddlvehicles.DataTextField = "registration_no";
            ddlvehicles.DataValueField = "vm_sno";
            ddlvehicles.DataBind();
            ddlvehicles.ClearSelection();
            ddlvehicles.Items.Insert(0, new ListItem { Value = "ALL", Text = "ALL", Selected = true });
            ddlvehicles.SelectedValue = "ALL";
        }
        if (ddlStatus.SelectedValue == "Puff Diesel")
        {
            hideroutes.Visible = true;
            hidevehicles.Visible = false;
            Getroutes();
           
        }
        if (ddlStatus.SelectedValue == "Own Tanker")
        {
            hideroutes.Visible = false;
            hidevehicles.Visible = false;
        }
        if (ddlStatus.SelectedValue == "Others Tanker")
        {
            hideroutes.Visible = false;
            hidevehicles.Visible = false;
        }
        if (ddlStatus.SelectedValue == "Tanker Diesel")
        {
            hideroutes.Visible = false;
            hidevehicles.Visible = false;
        }
        if (ddlStatus.SelectedValue == "Own Puff SO")
        {
            hideroutes.Visible = false;
            hidevehicles.Visible = false;
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
            Report.Columns.Add("Ledger Name");
            Report.Columns.Add("Amount");
            Report.Columns.Add("Narration");
            if (ddlStatus.SelectedItem.Text == "Own Tanker")
            {
                Session["filename"] = " Tally OwnTankers" + todate.ToString("dd/MM/yyyy");
                cmd = new MySqlCommand("SELECT COUNT(vehicel_master.registration_no) AS vehicleworkingdays,vehicel_master.ledgername, vehicel_master.vm_sno, vehicel_master.registration_no, SUM(triplogs.km) AS kms, SUM(triplogs.km * triplogs.charge) AS value FROM vehicel_master INNER JOIN tripdata ON vehicel_master.vm_sno = tripdata.vehicleno INNER JOIN triplogs ON tripdata.sno = triplogs.tripsno WHERE (vehicel_master.vm_owner = @Owner) AND (vehicel_master.vhtype_refno = 22) AND (triplogs.doe BETWEEN @d1 AND @d2) GROUP BY vehicel_master.registration_no");
                cmd.Parameters.Add("@Owner", Session["shortname"].ToString());
                cmd.Parameters.Add("@BranchID", 1);
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                DataTable dtble = vdm.SelectQuery(cmd).Tables[0];
                double totamount = 0;
                fromdate = fromdate.AddDays(-1);
                string frmdate = fromdate.ToString("dd-MMM-yyyy");
                string[] strjv = frmdate.Split('-');

                foreach (DataRow branch in dtble.Rows)
                {
                    string vehicleno = branch["registration_no"].ToString();
                    DataRow newrow = Report.NewRow();
                 //   newrow["Type"] = "Credit";
                    newrow["JV No."] = "TANKER TRANS OWN- " + vehicleno + "-" + strjv[1] + "'" + strjv[2];
                    newrow["JV Date"] = todate.ToString("dd-MMM-yyyy");
                    newrow["Ledger Name"] = branch["ledgername"].ToString();
                    double amount = 0;
                    double.TryParse(branch["value"].ToString(), out amount);
                    totamount += amount;
                    newrow["Amount"] = "-" + amount;
                    newrow["Narration"] = "Being the tanker transportation amount credited  " + branch["registration_no"].ToString() + " for the month of " + todate.ToString("MMM-yyyy") + " Amount " + amount + " Vehicle Number " + branch["registration_no"].ToString() + ",Emp Name  " + Session["employname"].ToString();
                    Report.Rows.Add(newrow);

                    DataRow newrow1 = Report.NewRow();
                  //  newrow1["Type"] = "Debit";
                    newrow1["JV No."] = "TANKER TRANS OWN- " + vehicleno + "-" + strjv[1] + "'" + strjv[2];
                    newrow1["JV Date"] = todate.ToString("dd-MMM-yyyy");
                    newrow1["Ledger Name"] = "Tankers Transportation Own-Pbk";
                    newrow1["Amount"] = amount;
                    newrow1["Narration"] = "Being the tanker transportation amount credited  " + branch["registration_no"].ToString() + " for the month of " + todate.ToString("MMM-yyyy") + " Amount " + amount + " Vehicle Number " + branch["registration_no"].ToString() + ",Emp Name  " + Session["employname"].ToString();
                    Report.Rows.Add(newrow1);
                }
            }
            if (ddlStatus.SelectedItem.Text == "Others Tanker")
            {
                Session["filename"] = " Tally OtherTankers" + todate.ToString("dd/MM/yyyy");
                cmd = new MySqlCommand("SELECT COUNT(vehicel_master.registration_no) AS vehicleworkingdays,vehicel_master.ledgername,vehicel_master.vm_sno, vehicel_master.registration_no, SUM(triplogs.km) AS kms, SUM(triplogs.km * triplogs.charge) AS value FROM vehicel_master INNER JOIN tripdata ON vehicel_master.vm_sno = tripdata.vehicleno INNER JOIN triplogs ON tripdata.sno = triplogs.tripsno WHERE (vehicel_master.vm_owner <> @Owner) AND (vehicel_master.vhtype_refno = 22) AND (triplogs.doe BETWEEN @d1 AND @d2) GROUP BY vehicel_master.registration_no");
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

                    string vehicleno = branch["registration_no"].ToString();
                   // string[] strvehicle = vehicleno.Split(null);
                    DataRow newrow = Report.NewRow();
                  //  newrow["Type"] = "Credit";
                    newrow["JV No."] = "TANKER TRANS - " + "Others " + vehicleno;
                    newrow["JV Date"] = todate.ToString("dd-MMM-yyyy");
                    newrow["Ledger Name"] = branch["ledgername"].ToString();
                    double amount = 0;
                    double.TryParse(branch["value"].ToString(), out amount);
                    totamount += amount;
                    newrow["Amount"] = "-" + amount;
                    newrow["Narration"] = "Being the tanker transportation amount credited  " + branch["registration_no"].ToString() + " for the month of " + todate.ToString("MMM-yyyy") + " Amount " + amount + " Vehicle Number " + branch["registration_no"].ToString() + ",Emp Name  " + Session["employname"].ToString();
                    Report.Rows.Add(newrow);

                    DataRow newrow1 = Report.NewRow();
                    //newrow1["Type"] = "Debit";
                    newrow1["JV No."] = "TANKER TRANS - " + "Others " + vehicleno;
                    newrow1["JV Date"] = todate.ToString("dd-MMM-yyyy");
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
                newReport.Columns.Add("LedgerName");
                newReport.Columns.Add("Infuel");
                newReport.Columns.Add("outfuel");
                newReport.Columns.Add("Price");
                cmd = new MySqlCommand("SELECT SUM(triplogs.fuel) AS fuel, tripdata.DieselCost,vehicel_master.ledgername, vehicel_master.registration_no FROM vehicel_master INNER JOIN tripdata ON vehicel_master.vm_sno = tripdata.vehicleno INNER JOIN triplogs ON triplogs.tripsno = tripdata.sno WHERE (vehicel_master.vm_owner = @Owner) AND (vehicel_master.vhtype_refno = 22) AND (tripdata.tripdate BETWEEN @d1 AND @d2) GROUP BY vehicel_master.registration_no");
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
                string frmdate = fromdate.ToString("dd-MMM-yyyy");
                string[] strjv = frmdate.Split('-');
                foreach (DataRow dr in dtoutdiesel.Rows)
                {
                    DataRow newrow = newReport.NewRow();
                    newrow["VehicleNo"] = dr["registration_no"].ToString();
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
                    string vehicleno = branch["VehicleNo"].ToString();
                    DataRow newrow = Report.NewRow();
                    //newrow["Type"] = "Credit";
                    newrow["JV No."] = "TANKER DIESEL - " + vehicleno+"-"+ strjv[1] + "'" + strjv[2];
                    newrow["JV Date"] = todate.ToString("dd-MMM-yyyy");
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
                    //newrow1["Type"] = "Debit";
                    newrow1["JV No."] = "TANKER DIESEL - " + vehicleno + "-" + strjv[1] + "'" + strjv[2];
                    newrow1["JV Date"] = todate.ToString("dd-MMM-yyyy");
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
                newReport.Columns.Add("LedgerName");
                newReport.Columns.Add("Infuel");
                newReport.Columns.Add("outfuel");
                newReport.Columns.Add("Price");
                if (ddlroutes.SelectedItem.Text == "ALL")
                {
                    cmd = new MySqlCommand("SELECT SUM(triplogs.fuel) AS fuel,tripdata.routeid, tripdata.DieselCost,vehicel_master.ledgername, vehicel_master.registration_no FROM vehicel_master INNER JOIN tripdata ON vehicel_master.vm_sno = tripdata.vehicleno INNER JOIN triplogs ON triplogs.tripsno = tripdata.sno WHERE (vehicel_master.vm_owner = @Owner) AND (vehicel_master.vhtype_refno = 7) AND (tripdata.tripdate BETWEEN @d1 AND @d2) GROUP BY vehicel_master.registration_no");
                }
                else
                {
                    cmd = new MySqlCommand("SELECT SUM(triplogs.fuel) AS fuel,tripdata.routeid, tripdata.DieselCost,vehicel_master.ledgername, vehicel_master.registration_no FROM vehicel_master INNER JOIN tripdata ON vehicel_master.vm_sno = tripdata.vehicleno INNER JOIN triplogs ON triplogs.tripsno = tripdata.sno WHERE (vehicel_master.vm_owner = @Owner) AND (vehicel_master.vhtype_refno = 7) AND (tripdata.tripdate BETWEEN @d1 AND @d2) AND (tripdata.routeid = @route) GROUP BY vehicel_master.registration_no");
                }
                 cmd.Parameters.Add("@Owner", Session["shortname"].ToString());
                cmd.Parameters.Add("@route", ddlroutes.SelectedValue);
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                DataTable dtoutdiesel = vdm.SelectQuery(cmd).Tables[0];
                if (ddlroutes.SelectedItem.Text == "ALL")
                {
                    cmd = new MySqlCommand("SELECT SUM(tripdata.endfuelvalue) AS endfuel, tripdata.DieselCost, vehicel_master.registration_no FROM tripdata INNER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno WHERE  (vehicel_master.vm_owner = @Owner) AND (vehicel_master.vhtype_refno = 7) AND (tripdata.tripdate BETWEEN @d1 AND @d2) GROUP BY vehicel_master.registration_no");
                }
                else
                {
                    cmd = new MySqlCommand("SELECT SUM(tripdata.endfuelvalue) AS endfuel, tripdata.DieselCost, vehicel_master.registration_no FROM tripdata INNER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno WHERE  (vehicel_master.vm_owner = @Owner) AND (vehicel_master.vhtype_refno = 7) AND (tripdata.tripdate BETWEEN @d1 AND @d2) AND (tripdata.routeid = @route) GROUP BY vehicel_master.registration_no");
                }
                cmd.Parameters.Add("@Owner", Session["shortname"].ToString());
                cmd.Parameters.Add("@route", ddlroutes.SelectedValue);
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                DataTable dtenddiesel = vdm.SelectQuery(cmd).Tables[0];
                double totamount = 0;
                fromdate = fromdate.AddDays(-1);
                string frmdate = fromdate.ToString("dd-MMM-yyyy");
                string[] strjv = frmdate.Split('-');
                cmd = new MySqlCommand("SELECT sno, routename, status, branch_id, operatedby, ledgername FROM routes");
                DataTable dtroutes = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow dr in dtoutdiesel.Rows)
                {
                    DataRow newrow = newReport.NewRow();
                    newrow["VehicleNo"] = dr["registration_no"].ToString();
                    string route = dr["routeid"].ToString();
                    foreach (DataRow drroute in dtroutes.Select("routename='" + route + "'"))
                    {
                        newrow["LedgerName"] = drroute["ledgername"].ToString();
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

                    string vehicleno = branch["VehicleNo"].ToString();
                    //  string strvehicle = vehicleno.Substring(5, 8);
                    DataRow newrow = Report.NewRow();
                   // newrow["Type"] = "Credit";
                    newrow["JV No."] = "PUFF DIESEL - " + vehicleno + " -" + strjv[1] + "'" + strjv[2];
                    newrow["JV Date"] = todate.ToString("dd-MMM-yyyy");
                    newrow["Ledger Name"] = "Sales Maintenance-Pbk-Diesel";//branch["LedgerName"].ToString();
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
                  //  newrow1["Type"] = "Debit";
                    newrow1["JV No."] = "PUFF DIESEL - " + vehicleno+"-"+strjv[1] + "'" +strjv[2];
                    newrow1["JV Date"] = todate.ToString("dd-MMM-yyyy");
                    newrow1["Ledger Name"] = branch["LedgerName"].ToString();
                    newrow1["Amount"] = tot_dieselvalue;
                    newrow1["Narration"] = "Being the diesel filling  qty   " + branch["VehicleNo"].ToString() + " for the month of " + todate.ToString("MMM-yyyy") + " Amount " + tot_dieselvalue + " Vehicle Number " + branch["VehicleNo"].ToString() + ",Emp Name  " + Session["employname"].ToString();
                    Report.Rows.Add(newrow1);
                }
            }
            if (ddlStatus.SelectedItem.Text == "Own Puff Plant")
            {
                Session["filename"] = " Tally OwnPuffs" + todate.ToString("dd/MM/yyyy");
                if (ddlroutes.SelectedItem.Text == "ALL" && ddlvehicles.SelectedItem.Text == "ALL")
                {
                    cmd = new MySqlCommand("SELECT COUNT(vehicel_master.registration_no) AS vehicleworkingdays, vehicel_master.vm_sno, vehicel_master.registration_no, tripdata.Rent, tripdata.routeid FROM vehicel_master INNER JOIN tripdata ON vehicel_master.vm_sno = tripdata.vehicleno WHERE (vehicel_master.vm_owner = @Owner) AND (vehicel_master.vhtype_refno = 7) AND (vehicel_master.branch_id = 1) AND (tripdata.tripdate BETWEEN @d1 AND @d2)  GROUP BY vehicel_master.registration_no, tripdata.routeid");
                }
                if (ddlroutes.SelectedItem.Text == "ALL" && ddlvehicles.SelectedItem.Text != "ALL")
                {
                    cmd = new MySqlCommand("SELECT COUNT(vehicel_master.registration_no) AS vehicleworkingdays, vehicel_master.vm_sno, vehicel_master.registration_no, tripdata.Rent, tripdata.routeid FROM vehicel_master INNER JOIN tripdata ON vehicel_master.vm_sno = tripdata.vehicleno WHERE (vehicel_master.vm_owner = @Owner) AND (vehicel_master.vhtype_refno = 7) AND (vehicel_master.branch_id = 1) AND (tripdata.tripdate BETWEEN @d1 AND @d2)  AND (vehicel_master.vm_sno = @vehicle) GROUP BY vehicel_master.registration_no, tripdata.routeid");
                }
                if (ddlvehicles.SelectedItem.Text == "ALL" && ddlroutes.SelectedItem.Text != "ALL")
                {
                    cmd = new MySqlCommand("SELECT COUNT(vehicel_master.registration_no) AS vehicleworkingdays, vehicel_master.vm_sno, vehicel_master.registration_no, tripdata.Rent, tripdata.routeid FROM vehicel_master INNER JOIN tripdata ON vehicel_master.vm_sno = tripdata.vehicleno WHERE (vehicel_master.vm_owner = @Owner) AND (vehicel_master.vhtype_refno = 7) AND (vehicel_master.branch_id = 1) AND (tripdata.tripdate BETWEEN @d1 AND @d2)  AND (tripdata.routeid = @route) GROUP BY vehicel_master.registration_no, tripdata.routeid");
                }
                if(ddlvehicles.SelectedItem.Text != "ALL" && ddlroutes.SelectedItem.Text != "ALL")
                {
                    cmd = new MySqlCommand("SELECT COUNT(vehicel_master.registration_no) AS vehicleworkingdays, vehicel_master.vm_sno, vehicel_master.registration_no, tripdata.Rent, tripdata.routeid FROM vehicel_master INNER JOIN tripdata ON vehicel_master.vm_sno = tripdata.vehicleno WHERE (vehicel_master.vm_owner = @Owner) AND (vehicel_master.vhtype_refno = 7) AND (vehicel_master.branch_id = 1) AND (tripdata.tripdate BETWEEN @d1 AND @d2) AND (vehicel_master.vm_sno = @vehicle) AND (tripdata.routeid = @route) GROUP BY vehicel_master.registration_no, tripdata.routeid");
                }
                cmd.Parameters.Add("@Owner", Session["shortname"].ToString());
                cmd.Parameters.Add("@route", ddlroutes.SelectedValue);
                cmd.Parameters.Add("@vehicle", ddlvehicles.SelectedValue);
                cmd.Parameters.Add("@BranchID", 1);
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                DataTable dtble = vdm.SelectQuery(cmd).Tables[0];
                double totamount = 0;
                fromdate = fromdate.AddDays(-1);
                string frmdate = fromdate.ToString("dd-MMM-yyyy");
                string[] strjv = frmdate.Split('-');

                foreach (DataRow branch in dtble.Rows)
                {
                    string vehicleno = branch["registration_no"].ToString();
                  //  string strvehicle = vehicleno.Substring(5, 8);
                    DataRow newrow = Report.NewRow();
                   // newrow["Type"] = "Debit";
                    newrow["JV No."] = "PUFF TRANS - " + vehicleno +" -"+ strjv[1] +"'"+ strjv[2];
                    newrow["JV Date"] = todate.ToString("dd-MMM-yyyy");
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
                   // newrow1["Type"] = "Credit";
                    newrow1["JV No."] = "PUFF TRANS - " + vehicleno + " -" + strjv[1] + "'" + strjv[2];
                    newrow1["JV Date"] = todate.ToString("dd-MMM-yyyy");
                    newrow1["Ledger Name"] = Session["shortname"].ToString() +" PUFF " + branch["registration_no"].ToString();
                    newrow1["Amount"] = "-" + amount;
                    newrow1["Narration"] = "Being the puff transportation amount credited  " + branch["registration_no"].ToString() + " Route Name " + branch["routeid"].ToString() + " for the month of " + todate.ToString("MMM-yyyy") + " Per day Rs. " + Rent + " /-* " + vehicleworkingdays + " Amount " + amount + " Vehicle Number " + branch["registration_no"].ToString() + ",Emp Name  " + Session["employname"].ToString();
                    Report.Rows.Add(newrow1);
                }
            }
            if (ddlStatus.SelectedItem.Text == "Own Puff SO")
            {
                Session["filename"] = " Tally OwnPuffs" + todate.ToString("dd/MM/yyyy");
                cmd = new MySqlCommand("SELECT COUNT(vehicel_master.registration_no) AS vehicleworkingdays, vehicel_master.vm_sno, vehicel_master.registration_no, tripdata.Rent, tripdata.routeid FROM vehicel_master INNER JOIN tripdata ON vehicel_master.vm_sno = tripdata.vehicleno WHERE (vehicel_master.vm_owner = @Owner) AND (vehicel_master.vhtype_refno = 7) AND (vehicel_master.branch_id = 1) AND (tripdata.tripdate BETWEEN @d1 AND @d2)  GROUP BY  tripdata.routeid");
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
                    string vehicleno = branch["registration_no"].ToString();
                    DataRow newrow = Report.NewRow();
                    newrow["JV No."] = "PUFF TRANS - " + vehicleno;
                    newrow["JV Date"] = todate.ToString("dd-MMM-yyyy");
                    newrow["Ledger Name"] = "Sales transportation-" + branch["routeid"].ToString();
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
                    newrow1["JV No."] = "PUFF TRANS - " + vehicleno;
                    newrow1["JV Date"] = todate.ToString("dd-MMM-yyyy");
                    newrow1["Ledger Name"] = "SVDS. P.LTD. PUNABAKA PLANT";
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
}