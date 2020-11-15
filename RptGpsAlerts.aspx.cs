using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class RptGpsAlerts : System.Web.UI.Page
{
    MySqlCommand cmd;
    string BranchID = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Branch_ID"] == null)
            Response.Redirect("Login.aspx");
        else
        {
            BranchID = Session["Branch_ID"].ToString();

            if (!Page.IsPostBack)
            {
                if (!Page.IsCallback)
                {
                    lblAddress.Text = Session["Address"].ToString();
                    lblTitle.Text = Session["TitleName"].ToString();
                    GetGpsAlertsInfo();
                }
            }
        }


    }

    void GetGpsAlertsInfo()
    {
        try
        {
            lblmsg.Text = "";
            DataTable Report = new DataTable();
            Report.Columns.Add("PlantName");
            Report.Columns.Add("Sno");
            Report.Columns.Add("VehicleNo");
            Report.Columns.Add("VehicleType");
            Report.Columns.Add("Logdate");
            Report.Columns.Add("Device");
            Report.Columns.Add("Sim");
            Report.Columns.Add("Wire");
            Report.Columns.Add("Vehicle");
            Report.Columns.Add("EntryBy");
            Report.Columns.Add("EntryDate");

            GpsDBManager gdm = new GpsDBManager();
            //cmd = new MySqlCommand(" SELECT vehicleid,VehicleType,Device,Sim,Wire,Vehicle FROM cabmanagement order by PlantName ");
            cmd = new MySqlCommand(" Select t1.MId, t1.vehicleid, t2.Sno, t2.vehicleid AS Expr1, cabmanagement.VehicleType, t2.Logdate, t2.Device, t2.Sim, t2.Wire, t2.Vehicle, t2.EntryBy, t2.EntryDate, cabmanagement.PlantName from(SELECT Max(sno) As MId, vehicleid FROM GpsDeviceAlertInfo group by vehicleid) AS t1 Inner join (Select Sno, vehicleid, Logdate, Device, Sim, Wire, Vehicle, EntryBy, EntryDate From GpsDeviceAlertInfo) As t2 ON  t1.vehicleid = t2.vehicleid AND t1.MId = t2.Sno INNER JOIN cabmanagement ON t1.vehicleid = cabmanagement.VehicleID Order by cabmanagement.PlantName ");

            DataTable gpsalertinfo = gdm.SelectQuery(cmd).Tables[0];
            if (gpsalertinfo.Rows.Count > 0)
            {
                int i = 1;
                foreach (DataRow dr in gpsalertinfo.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["PlantName"] = dr["PlantName"].ToString();
                    newrow["Sno"] = i++.ToString();
                    newrow["VehicleNo"] = dr["vehicleid"].ToString();
                    newrow["VehicleType"] = dr["VehicleType"].ToString();
                    DateTime logd = Convert.ToDateTime(dr["Logdate"].ToString());
                    newrow["Logdate"] = logd.ToString("dd/MMM/yyyy");
                    newrow["Device"] = dr["Device"].ToString();
                    newrow["Sim"] = dr["Sim"].ToString();
                    newrow["Wire"] = dr["Wire"].ToString();
                    newrow["Vehicle"] = dr["Vehicle"].ToString();
                    newrow["EntryBy"] = dr["EntryBy"].ToString();
                    logd = Convert.ToDateTime(dr["EntryDate"].ToString());
                    newrow["EntryDate"] = logd.ToString("dd/MMM/yyyy");


                    Report.Rows.Add(newrow);
                }
                string title = "vehicleGps Informations Report ";
                Session["title"] = title;
                Session["filename"] = "vehicleGpsInfo";
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
    protected void btnsave_Click(object sender, EventArgs e)
    {
        GetGpsAlertsInfo();
    }

    protected void grdReports_DataBinding(object sender, EventArgs e)
    {
        try
        {
            GridViewGroup First = new GridViewGroup(grdReports, null, "PlantName");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void grdReports_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {           
            TableCell cell1 = e.Row.Cells[0];
            if (cell1.Text.Length > 0)
            {
                cell1.BackColor = System.Drawing.Color.YellowGreen;
            }
            if (e.Row.Cells[1].Text.Length>0)
            {
                e.Row.BackColor = System.Drawing.Color.White;
                e.Row.Font.Bold = true;
            }
        }
    }

}