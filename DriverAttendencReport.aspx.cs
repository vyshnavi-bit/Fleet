using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class DriverAttendencReport : System.Web.UI.Page
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
            vdm = new VehicleDBMgr();
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
            TimeSpan dateSpan = todate.Subtract(fromdate);
            int NoOfdays = dateSpan.Days;
            NoOfdays = NoOfdays + 2;
            lblType.Text = ddlType.SelectedItem.Text;
            lblFromDate.Text = fromdate.ToString("dd/MM/yyyy");
            lbltodate.Text = todate.ToString("dd/MM/yyyy");
            if (ddlType.SelectedValue == "Driver")
            {
                lblheader.Text = "Driver Attendence Report";
                cmd = new MySqlCommand("SELECT employdata.employname, tripdata.tripdate, triplogs.doe, tripdata.enddate, vehicel_master.vhtype_refno FROM tripdata INNER JOIN triplogs ON tripdata.sno = triplogs.tripsno LEFT OUTER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno LEFT OUTER JOIN employdata ON tripdata.driverid = employdata.emp_sno WHERE (triplogs.doe BETWEEN @d1 AND @d2) and  (vehicel_master.vhtype_refno=22) AND (employdata.branch_id=@BranchID) group by employdata.employname, DATE(triplogs.doe) order by DATE(triplogs.doe)");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@BranchID", Session["Branch_ID"].ToString());
                
                DataTable dtTankar = vdm.SelectQuery(cmd).Tables[0];
                cmd = new MySqlCommand("SELECT employdata.employname, tripdata.tripdate, triplogs.doe, tripdata.enddate, vehicel_master.vhtype_refno FROM tripdata INNER JOIN triplogs ON tripdata.sno = triplogs.tripsno LEFT OUTER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno LEFT OUTER JOIN employdata ON tripdata.driverid = employdata.emp_sno WHERE (triplogs.doe BETWEEN @d1 AND @d2) and  (vehicel_master.vhtype_refno=7 OR vehicel_master.vhtype_refno=23 ) AND (employdata.branch_id=@BranchID) group by  DATE(tripdata.tripdate)");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@BranchID", Session["Branch_ID"].ToString());
                DataTable dtPuff = vdm.SelectQuery(cmd).Tables[0];
                DataTable sum = new DataTable();
                sum = dtTankar.Copy();
                sum.Merge(dtPuff);
                Report = new DataTable();
                Report.Columns.Add("Sno");
                Report.Columns.Add("Driver Name");
                int count = 0;
                DateTime dtFrm = new DateTime();
                for (int j = 1; j < NoOfdays; j++)
                {
                    if (j == 1)
                    {
                        dtFrm = fromdate;
                    }
                    else
                    {
                        dtFrm = dtFrm.AddDays(1);
                    }
                    string strdate = dtFrm.ToString("dd/MMM");
                    Report.Columns.Add(strdate);
                    count++;
                }
                Report.Columns.Add("Total", typeof(Double)).SetOrdinal(count + 2);
                DataView view = new DataView(sum);
                DataTable DriverData = view.ToTable(true, "employname");
                int i = 1;
                foreach (DataRow branch in DriverData.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["Sno"] = i++.ToString();
                    newrow["Driver Name"] = branch["employname"].ToString();
                    int total = 0;
                    foreach (DataRow drDriver in sum.Rows)
                    {
                        if (branch["employname"].ToString() == drDriver["employname"].ToString())
                        {
                            string employname = drDriver["employname"].ToString();
                            string doe = drDriver["doe"].ToString();
                            if (doe != "")
                            {
                                DateTime dtDoe = Convert.ToDateTime(doe);
                                string strdateTime = dtDoe.ToString("HH");
                                //int intdate = 0;
                                //int.TryParse(strdateTime, out intdate);
                                //if (intdate < 15)
                                //{
                                    string strdate = dtDoe.ToString("dd/MMM");
                                    //string strDate = dtDoe.ToString("dd");
                                    //int intdate = 0;
                                    //int.TryParse(strDate, out intdate);
                                    newrow[strdate] = "P";
                                    total++;
                                //}
                            }
                        }
                    }
                    newrow["Total"] = total;
                    Report.Rows.Add(newrow);
                }
                grdReports.DataSource = Report;
                grdReports.DataBind();
                hidepanel.Visible = true;
            }
            if (ddlType.SelectedValue == "Vehicle")
            {
                lblheader.Text = "Vehicle Attendence Report";
                cmd = new MySqlCommand("SELECT vehicel_master.registration_no AS VehicleNo, triplogs.doe, tripdata.tripdate FROM vehicel_master INNER JOIN tripdata ON vehicel_master.vm_sno = tripdata.vehicleno INNER JOIN triplogs ON tripdata.sno = triplogs.tripsno WHERE (triplogs.doe BETWEEN @d1 AND @d2) group by vehicel_master.registration_no, DATE(triplogs.doe) order by DATE(triplogs.doe)");
                //cmd = new MySqlCommand("SELECT employdata.employname, tripdata.tripdate, triplogs.doe, tripdata.enddate, vehicel_master.vhtype_refno FROM tripdata INNER JOIN triplogs ON tripdata.sno = triplogs.tripsno LEFT OUTER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno LEFT OUTER JOIN employdata ON tripdata.driverid = employdata.emp_sno WHERE (triplogs.doe BETWEEN @d1 AND @d2) and  (vehicel_master.vhtype_refno=22) group by employdata.employname, DATE(triplogs.doe) order by DATE(triplogs.doe)");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                DataTable dtTankar = vdm.SelectQuery(cmd).Tables[0];
                cmd = new MySqlCommand("SELECT vehicel_master.registration_no AS VehicleNo,employdata.employname, tripdata.tripdate, triplogs.doe, tripdata.enddate, vehicel_master.vhtype_refno FROM tripdata INNER JOIN triplogs ON tripdata.sno = triplogs.tripsno LEFT OUTER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno LEFT OUTER JOIN employdata ON tripdata.driverid = employdata.emp_sno WHERE (triplogs.doe BETWEEN @d1 AND @d2) and  (vehicel_master.vhtype_refno=7 OR vehicel_master.vhtype_refno=23 ) group by DATE(tripdata.tripdate)");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                DataTable dtPuff = vdm.SelectQuery(cmd).Tables[0];
                DataTable sum = new DataTable();
                sum = dtTankar.Copy();
                sum.Merge(dtPuff);
                Report = new DataTable();
                Report.Columns.Add("Sno");
                Report.Columns.Add("Vehicle No");
                int count = 0;
                DateTime dtFrm = new DateTime();
                for (int j = 1; j < NoOfdays; j++)
                {
                    if (j == 1)
                    {
                        dtFrm = fromdate;
                    }
                    else
                    {
                        dtFrm = dtFrm.AddDays(1);
                    }
                    string strdate = dtFrm.ToString("dd/MMM");
                    Report.Columns.Add(strdate);
                    count++;
                }
                Report.Columns.Add("Total", typeof(Double)).SetOrdinal(count + 2);
                DataView view = new DataView(sum);
                DataTable DriverData = view.ToTable(true, "VehicleNo");
                int i = 1;
                foreach (DataRow branch in DriverData.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["Sno"] = i++.ToString();
                    newrow["Vehicle No"] = branch["VehicleNo"].ToString();
                    int total = 0;
                    foreach (DataRow drDriver in sum.Rows)
                    {
                        if (branch["VehicleNo"].ToString() == drDriver["VehicleNo"].ToString())
                        {
                            string VehicleNo = drDriver["VehicleNo"].ToString();
                            string doe = drDriver["doe"].ToString();
                            if (doe != "")
                            {
                                DateTime dtDoe = Convert.ToDateTime(doe);
                                string strdateTime = dtDoe.ToString("HH");
                                //int intdate = 0;
                                //int.TryParse(strdateTime, out intdate);
                                //if (intdate < 15)
                                //{
                                string strdate = dtDoe.ToString("dd/MMM");
                                //string strDate = dtDoe.ToString("dd");
                                //int intdate = 0;
                                //int.TryParse(strDate, out intdate);
                                newrow[strdate] = "P";
                                total++;
                                //}
                            }
                        }
                    }
                    newrow["Total"] = total;
                    Report.Rows.Add(newrow);
                }
                grdReports.DataSource = Report;
                grdReports.DataBind();
                hidepanel.Visible = true;
            }
        }
        catch(Exception ex)
        {
            lblmsg.Text = ex.Message;
        }

    }
}