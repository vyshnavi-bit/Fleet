using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class Vehiclewise_tyresreport : System.Web.UI.Page
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
                    GetReport();
                    lblTitle.Text = Session["TitleName"].ToString();
                }
            }
        }
    }
    void GetReport()
    {
        try
        {
            lblmsg.Text = "";
            DataTable dtIssueTyre = new DataTable();
            cmd = new MySqlCommand("SELECT vehicel_master.vm_sno as Sno,vehicel_master.registration_no AS VehicleNo, axils_tyres_names.tyrename AS TyreName,new_tyres_sub.Sno as TyreSno, new_tyres_sub.tyre_sno AS TyreNo,new_tyres_sub.SVDSNO, new_tyres_sub.size,new_tyres_sub.current_kms as TotalKms,vehicle_master_sub.Odometer as vehicleKms, new_tyres_sub.cost, minimasters.mm_name AS Brand, new_tyres_sub.tyre_tube, minimasters_1.mm_name AS TyreType FROM vehicel_master INNER JOIN vehicle_master_sub ON vehicel_master.vm_sno = vehicle_master_sub.vehicle_mstr_sno INNER JOIN new_tyres_sub ON vehicle_master_sub.tyre_sno = new_tyres_sub.sno INNER JOIN axils_tyres_names ON vehicle_master_sub.axles_tyres_names_sno = axils_tyres_names.sno INNER JOIN minimasters ON new_tyres_sub.brand = minimasters.sno INNER JOIN minimasters minimasters_1 ON new_tyres_sub.type_of_tyre = minimasters_1.sno WHERE (vehicel_master.branch_id = @BranchID) Order by vehicel_master.registration_no");
            cmd.Parameters.Add("@BranchID", BranchID);
            dtIssueTyre = vdm.SelectQuery(cmd).Tables[0];
            if (dtIssueTyre.Rows.Count > 0)
            {
                string title = "Issue Tyres Report ";
                Session["title"] = title;
                Session["filename"] = "IssueTyresReport ";
                Session["xportdata"] = dtIssueTyre;
                grdReports.DataSource = dtIssueTyre;
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