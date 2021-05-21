using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TailspinSpyworks
{
    public partial class Error : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["Err"] != null)
            {
                Label_ErrorFrom.Text = Request["Err"].ToString();
            }
            else
            {
                Label_ErrorFrom.Text = "Undetermined Source.";
            }
            if (Request["InnerErr"] != null)
            {
                Label_ErrorMessage.Text = Request["InnerErr"].ToString();
            }
            else
            {
                Label_ErrorMessage.Text = "Undetermined Detail.";
            }
        }
    }
}