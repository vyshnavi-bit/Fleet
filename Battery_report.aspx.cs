using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class Battery_report : System.Web.UI.Page
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
                    Getbattery();
                }
            }
        }
    }
    void Getbattery()
    {
        try
        {
            lblmsg.Text = "";
            DataTable Report = new DataTable();
            Report.Columns.Add("Sno");
            Report.Columns.Add("VehicleNo");
            Report.Columns.Add("VehicleMake");
            Report.Columns.Add("BatteryNo1");
            Report.Columns.Add("Make1");
            Report.Columns.Add("PuchaseDate1");
            Report.Columns.Add("BatteryNo2");
            Report.Columns.Add("Make2");
            Report.Columns.Add("PuchaseDate2");
           // cmd = new MySqlCommand("SELECT vehicel_master.registration_no, battery_master.bat_sno, DATE_FORMAT(battery_master.purchasedate, '%m/%d/%Y') AS purchasedate, minimasters_2.mm_name, battery_master_1.bat_sno AS Batteryno, DATE_FORMAT(battery_master_1.purchasedate, '%m/%d/%Y') AS Batterydate, minimasters_1.mm_name AS Battrymake, minimasters.mm_name AS VehicleMake FROM  minimasters minimasters_2 INNER JOIN battery_master ON minimasters_2.sno = battery_master.make RIGHT OUTER JOIN minimasters INNER JOIN vehicel_master ON minimasters.sno = vehicel_master.vhmake_refno ON battery_master.sno = vehicel_master.batteryno LEFT OUTER JOIN minimasters minimasters_1 INNER JOIN battery_master battery_master_1 ON minimasters_1.sno = battery_master_1.make ON vehicel_master.batteryno2 = battery_master_1.sno WHERE (vehicel_master.vm_owner = @Owner) AND (vehicel_master.branch_id <> 40) ORDER BY vehicel_master.vhtype_refno");
            cmd = new MySqlCommand("SELECT vehicel_master.registration_no, battery_master.bat_sno, DATE_FORMAT(battery_master.purchasedate, '%m/%d/%Y') AS purchasedate, minimasters_2.mm_name, battery_master_1.bat_sno AS Batteryno, DATE_FORMAT(battery_master_1.purchasedate, '%m/%d/%Y') AS Batterydate, minimasters_1.mm_name AS Battrymake, minimasters.mm_name AS VehicleMake FROM  minimasters minimasters_2 INNER JOIN battery_master ON minimasters_2.sno = battery_master.make RIGHT OUTER JOIN minimasters INNER JOIN vehicel_master ON minimasters.sno = vehicel_master.vhmake_refno ON battery_master.sno = vehicel_master.batteryno LEFT OUTER JOIN minimasters minimasters_1 INNER JOIN battery_master battery_master_1 ON minimasters_1.sno = battery_master_1.make ON vehicel_master.batteryno2 = battery_master_1.sno WHERE (vehicel_master.vm_owner = @Owner) AND (vehicel_master.branch_id <> 40) GROUP BY vehicel_master.registration_no ORDER BY vehicel_master.vhtype_refno");
           cmd.Parameters.Add("@Owner", Session["shortname"].ToString());
            cmd.Parameters.Add("@BranchID", BranchID);
            DataTable dtbattery = vdm.SelectQuery(cmd).Tables[0];
            if (dtbattery.Rows.Count > 0)
            {
                int i = 1;
                foreach (DataRow dr in dtbattery.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["Sno"] = i++.ToString();
                    newrow["VehicleNo"] = dr["registration_no"].ToString();
                    newrow["VehicleMake"] = dr["VehicleMake"].ToString();
                    newrow["BatteryNo1"] = dr["bat_sno"].ToString();
                    newrow["Make1"] = dr["mm_name"].ToString();
                    newrow["PuchaseDate1"] = dr["purchasedate"].ToString();
                    newrow["BatteryNo2"] = dr["Batteryno"].ToString();
                    newrow["Make2"] = dr["Battrymake"].ToString();
                    newrow["PuchaseDate2"] = dr["Batterydate"].ToString();
                    Report.Rows.Add(newrow);
                }
                string title = "Vehicle Battery Report ";
                Session["title"] = title;
                Session["filename"] = "VehicleBattery";
                Session["xportdata"] = Report;
                grdReports.DataSource = Report;
                grdReports.DataBind();
            }
            else
            {
                lblmsg.Text = "No data found";
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
}