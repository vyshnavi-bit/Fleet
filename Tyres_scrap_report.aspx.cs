using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MySql.Data.MySqlClient;

public partial class Tyres_scrap_report : System.Web.UI.Page
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
                    GetTyres();
                }
            }
        }
    }
     void GetTyres()
    {
        try
        {
            lblmsg.Text = "";
            DataTable trips = new DataTable();
            cmd = new MySqlCommand("SELECT DATE_FORMAT(new_tyres.purchase_date, '%d %b %y') AS PurchaseDate, new_tyres.invoiceno AS InvoiceNo, new_tyres_sub.tyre_sno AS TyreNo,new_tyres_sub.svdsno as SVDSNo,minimasters.mm_name AS TyreType, minimasters_1.mm_name AS Brand, new_tyres_sub.current_KMS AS CurKms,  new_tyres_sub.cost as Cost FROM new_tyres INNER JOIN new_tyres_sub ON new_tyres.sno = new_tyres_sub.newtyre_refno INNER JOIN minimasters ON new_tyres_sub.type_of_tyre = minimasters.sno INNER JOIN minimasters minimasters_1 ON new_tyres_sub.brand = minimasters_1.sno WHERE (new_tyres.branch_id = @BranchID)  and (new_tyres_sub.Fitting_Type='SC') order by seriesno");
            cmd.Parameters.Add("@BranchID", BranchID);
            trips = vdm.SelectQuery(cmd).Tables[0];
            if (trips.Rows.Count > 0)
            {
                string title = "Tyres Scrap Report " ;
                Session["title"] = title;
                Session["filename"] = "TyresScrapReport";
                Session["xportdata"] = trips;
                grdReports.DataSource = trips;
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
