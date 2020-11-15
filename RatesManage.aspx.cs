
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
//using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;
using System.Text;
using ClosedXML.Excel;
using System.Configuration;
using System.Data.SqlClient;

public partial class RatesManage : System.Web.UI.Page
{
    MySqlCommand cmd;
    string BranchID = "";
    VehicleDBMgr vdm;
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["branch"] == null)
        //{
        //    Response.Redirect("Login.aspx");
        //}
        //else
        //{
        //    BranchID = Session["branch"].ToString();
        //}
        //UserName = Session["field1"].ToString();
        //vdm = new VehicleDBMgr();
        if (!this.IsPostBack)
        {
            if (!Page.IsCallback)
            {
            }
        }
    }
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
    }
    string id = "";


    protected void btn_Export_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable("GridView_Data");
            foreach (TableCell cell in grdReports.HeaderRow.Cells)
            {
                if (cell.Text == "Agent Name")
                {
                    dt.Columns.Add(cell.Text);
                }
                else
                {
                    dt.Columns.Add(cell.Text).DataType = typeof(double);
                }
            }
            foreach (GridViewRow row in grdReports.Rows)
            {
                dt.Rows.Add();
                for (int i = 0; i < row.Cells.Count; i++)
                {
                    if (row.Cells[i].Text == "&nbsp;")
                    {
                        row.Cells[i].Text = "0";
                    }
                    dt.Rows[dt.Rows.Count - 1][i] = row.Cells[i].Text;
                }
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                string FileName = Session["filename"].ToString();
                Response.AddHeader("content-disposition", "attachment;filename=" + FileName + ".xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void btn_Import_Click(object sender, EventArgs e)
    {
         string ConStr = "";
            //Extantion of the file upload control saving into ext because   
            //there are two types of extation .xls and .xlsx of Excel   
            string ext = Path.GetExtension(FileUploadToServer.FileName).ToLower();
            //getting the path of the file   
            string path = Server.MapPath("~/Userfiles/" + FileUploadToServer.FileName);
            //saving the file inside the MyFolder of the server  
            FileUploadToServer.SaveAs(path);
            //lblMessage.Text = FileUploadToServer.FileName + "\'s Data showing into the GridView";
            //checking that extantion is .xls or .xlsx  
            if (ext.Trim() == ".xls")
            {
                //connection string for that file which extantion is .xls  
                ConStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
            }
            else if (ext.Trim() == ".xlsx")
            {
                //connection string for that file which extantion is .xlsx  
                ConStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
            }
            //making query  
            OleDbConnection con = null;
            con = new OleDbConnection(ConStr);
            con.Close(); con.Open();
            DataTable dtquery = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            //Get first sheet name
            string getExcelSheetName = dtquery.Rows[0]["Table_Name"].ToString();
            //string query = "SELECT * FROM [Total Deduction$]";
            //Providing connection  
            OleDbConnection conn = new OleDbConnection(ConStr);
            //checking that connection state is closed or not if closed the   
            //open the connection  
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            //create command object  
            OleDbCommand cmd = new OleDbCommand(@"SELECT * FROM [" + getExcelSheetName + @"]", conn);
            // create a data adapter and get the data into dataadapter  
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            //DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            //fill the Excel data to data set  
            da.Fill(dt);
            //set data source of the grid view  
            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                if (dt.Rows[i][1] == DBNull.Value)
                    dt.Rows[i].Delete();
            }
            dt.AcceptChanges();
            grdReports.DataSource = dt;
            //binding the gridview  
            grdReports.DataBind();
            Session["dtImport"] = dt;
            //close the connection  
            conn.Close();
    }
    private void Import_To_Grid(string FilePath, string Extension, string isHDR)
    {
        try
        {
            string conStr = "";
            switch (Extension)
            {
                case ".xls": //Excel 97-03
                    conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                    break;
                case ".xlsx": //Excel 07
                    conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                    break;
            }
            conStr = String.Format(conStr, FilePath, isHDR);
            OleDbConnection connExcel = new OleDbConnection(conStr);
            OleDbCommand cmdExcel = new OleDbCommand();
            OleDbDataAdapter oda = new OleDbDataAdapter();
            DataTable dt = new DataTable();
            cmdExcel.Connection = connExcel;

            //Get the name of First Sheet
            connExcel.Open();
            DataTable dtExcelSchema;
            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
            connExcel.Close();

            //Read Data from First Sheet
            connExcel.Open();
            cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
            oda.SelectCommand = cmdExcel;
            oda.Fill(dt);
            connExcel.Close();

            //Bind Data to GridView
            grdReports.Caption = Path.GetFileName(FilePath);
            grdReports.DataSource = dt;
            grdReports.DataBind();
            Session["dtImport"] = dt;
            BtnSave.Visible = true;
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }

    }
    //protected void PageIndexChanging(object sender, GridViewPageEventArgs e)
    //{
    //    string FolderPath = ConfigurationManager.AppSettings["FolderPath"];
    //    string FileName = GridView1.Caption;
    //    string Extension = Path.GetExtension(FileName);
    //    string FilePath = Server.MapPath(FolderPath + FileName);

    //    Import_To_Grid(FilePath, Extension, rbHDR.SelectedItem.Text);
    //    GridView1.PageIndex = e.NewPageIndex;
    //    GridView1.DataBind();
    //}
    SqlCommand sqlcmd;
    protected void btn_WIDB_Click(object sender, EventArgs e)
    {
        try
        {
            vdm = new VehicleDBMgr();
            ////SAPdbmanger SAPvdm = new SAPdbmanger();
            DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
            DataTable dt = (DataTable)Session["dtImport"];
            foreach (DataRow dr in dt.Rows)
            {
                ////////string ledger_code = dr["BP Code"].ToString();
                ////////string ledgername = dr["BP Name"].ToString();
                ////////cmd = new MySqlCommand("Update  vehicel_master set  ledger_code=@ledger_code where ledgername=@ledgername");
                ////////cmd.Parameters.Add("@ledger_code", ledger_code);
                ////////cmd.Parameters.Add("@ledgername", ledgername);
                ////////vdm.Update(cmd);


                ////////cmd = new MySqlCommand("Update  vehicel_master set  Petroledger_code=@Petroledger_code where petroledgername=@petroledgername");
                ////////cmd.Parameters.Add("@Petroledger_code", ledger_code);
                ////////cmd.Parameters.Add("@petroledgername", ledgername);
                ////////vdm.Update(cmd);


                ////////cmd = new MySqlCommand("Update  termloanentry set  ledger_code=@ledger_code where ledgername=@ledgername");
                ////////cmd.Parameters.Add("@ledger_code", ledger_code);
                ////////cmd.Parameters.Add("@ledgername", ledgername);
                ////////vdm.Update(cmd);

                string sno = dr["sno"].ToString();
                string seriesno = dr["newno"].ToString();
                cmd = new MySqlCommand("Update  new_tyres_sub set  seriesno=@seriesno where sno=@sno");
                cmd.Parameters.Add("@seriesno", seriesno);
                cmd.Parameters.Add("@sno", sno);
                vdm.Update(cmd);
            }
            lblmsg.Text = "Saved";
        }
        catch (Exception ex)
        {
            if (ex.Message == "Object reference not set to an instance of an object.")
            {
                lblmsg.Text = "Session Expired";
                Response.Redirect("Login.aspx");
            }
            else
            {
                lblmsg.Text = ex.Message;

            }
        }
    }
}