using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace WebApplication3
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click1(object sender, EventArgs e)
        {
            //var script = "google.load('visualization', '1', { packages: ['corechart'] });";
            //ClientScript.RegisterStartupScript(this.GetType(), "sample", script.ToString(), true);

            //var script = " function drawChart() {var data = google.visualization.arrayToDataTable([['Task', 'Hours per Day'],['Work', 25],['Eat', 25],['Commute', 25],['Watch TV', 25],['Sleep', 7]]);var options = {title: 'My Daily Activities'};var chart = new google.visualization.PieChart(document.getElementById('piechart'));chart.draw(data, options);}";
            //ClientScript.RegisterStartupScript(this.GetType(), "sample", script.ToString(), true);

            var
              script = "google.setOnLoadCallback(drawChart);";
            ClientScript.RegisterStartupScript(this.GetType(), "sample", script.ToString(), true);



             
        }
    }
}