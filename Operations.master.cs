using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Operations : System.Web.UI.MasterPage
{
    string UserName = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Employ_Sno"] == "" || Session["Employ_Sno"] == null)
        {
            Response.Redirect("Login.aspx");
        }
        else
        {
            UserName = Session["employname"].ToString();
            if (!this.IsPostBack)
            {
                if (!Page.IsCallback)
                {
                    lblMessage.Text =  UserName;
                    lblName.Text = UserName;
                    lblmyname.Text = UserName;
                    lblRole.Text = Session["Employ_Type"].ToString();
                }
            }
        }
    }
}
