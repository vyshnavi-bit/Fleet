using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Drawing;
using System.Configuration;
using MySql.Data.MySqlClient;

public partial class petrocardimport : System.Web.UI.Page
{
    MySqlCommand cmd;
    string BranchID = "";
    VehicleDBMgr vdm;
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMessage.Visible = false;
    }
    protected void btnImport_Click(object sender, EventArgs e)
    {
        try
        {
            string ConStr = "";
            //Extantion of the file upload control saving into ext because   
            //there are two types of extation .xls and .xlsx of Excel   
            string ext = Path.GetExtension(FileUploadToServer.FileName).ToLower();
            //getting the path of the file   
            string path = Server.MapPath("~/Userfiles/" + FileUploadToServer.FileName);
            //saving the file inside the MyFolder of the server  
            FileUploadToServer.SaveAs(path);
            lblMessage.Text = FileUploadToServer.FileName + "\'s Data showing into the GridView";
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
            grvExcelData.DataSource = dt;
            //binding the gridview  
            grvExcelData.DataBind();
            Session["dtImport"] = dt;
            //close the connection  
            conn.Close();
        }

        catch (Exception ex)
        {
            lblMessage.Text = ex.Message.ToString();

        }
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["dtImport"];
            int i = 1;
            foreach (DataRow dr in dt.Rows)
            {
                vdm = new VehicleDBMgr();
                string Cardname = dr["Card name"].ToString();
                string ACCOUNTID = dr["ACCOUNT ID"].ToString();
                string DEALERNAME = dr["DEALER NAME"].ToString();
                string DEALERCITY = dr["DEALER CITY"].ToString();
                string TRANSACTIONDATE = dr["TRANSACTION DATE"].ToString();
                string[] strtrans = TRANSACTIONDATE.Split('-');
                string newdate = strtrans[1] + "/" + strtrans[0] + "/" + strtrans[2];
                DateTime dttransactiondate = Convert.ToDateTime(newdate);
                string ACCOUNTINGDAT = dr["ACCOUNTING DATE"].ToString();
                string[] straccountingdat = ACCOUNTINGDAT.Split('-');
                string newstraccountingdat = straccountingdat[1] + "/" + straccountingdat[0] + "/" + straccountingdat[2];
                DateTime dtaccdate = Convert.ToDateTime(newstraccountingdat);
               
                string TRANSACTIONTYPE = dr["TRANSACTION TYPE"].ToString();
                string CURRENCY = dr["CURRENCY"].ToString();
                string AMOUNT = dr["AMOUNT"].ToString();
                string VOLUMEDOCNO = dr["VOLUME"].ToString();
                string AMOUNTBALANCE = "0";// dr["AMOUNT BALANCE"].ToString();
                string PETROMILESEARNED = dr["PETROMILES EARNED"].ToString();
                string ODOMETERREADING = dr["ODOMETER READING"].ToString();
                string vehicleno = dr["vehicle no"].ToString();
                string Remarks = dr["Remarks"].ToString();
                string vehicletype = dr["vehicle type"].ToString();
                //DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
                cmd = new MySqlCommand("insert into petrocard_details (cardname,accountid,dealername,dealercity,transactiondate,accountingdate,tranactiontype,currency,amount,volume,amountbalance,petromiles_earned,odometer,vehicleno,remarks,vehicletype ) values (@cardname,@accountid,@dealername,@dealercity,@transactiondate,@accountingdate,@tranactiontype,@currency,@amount,@volume,@amountbalance,@petromiles_earned,@odometer,@vehicleno,@remarks,@vehicletype)");
                cmd.Parameters.Add("@cardname", Cardname);
                cmd.Parameters.Add("@accountid", ACCOUNTID);
                cmd.Parameters.Add("@dealername", DEALERNAME);
                cmd.Parameters.Add("@dealercity", DEALERCITY);
                cmd.Parameters.Add("@transactiondate", dttransactiondate);
                cmd.Parameters.Add("@accountingdate", dtaccdate);
                cmd.Parameters.Add("@tranactiontype", TRANSACTIONTYPE);
                cmd.Parameters.Add("@currency", CURRENCY);
                cmd.Parameters.Add("@amount", AMOUNT);
                cmd.Parameters.Add("@volume", VOLUMEDOCNO);
                cmd.Parameters.Add("@amountbalance", AMOUNTBALANCE);
                cmd.Parameters.Add("@petromiles_earned", PETROMILESEARNED);
                cmd.Parameters.Add("@odometer", ODOMETERREADING);
                cmd.Parameters.Add("@vehicleno", vehicleno);
                cmd.Parameters.Add("@remarks", Remarks);
                cmd.Parameters.Add("@vehicletype", vehicletype);
                vdm.insert(cmd);

            }
            lblmsg.Text = "Records inserted successfully";
            lblMessage.Text = "Records inserted successfully";
            DataTable dtempty = new DataTable();
            grvExcelData.DataSource = dtempty;
            grvExcelData.DataBind();
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            lblMessage.Text = ex.Message;
        }
    }
}