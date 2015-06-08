using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Http;
using System.Net;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;
using System.Data;
using System.Web.Security;

using Microsoft.Crm.Sdk.Messages;

// These namespaces are found in the Microsoft.Xrm.Sdk.dll assembly
// located in the SDK\bin folder of the SDK download.
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

// These namespaces are found in the Microsoft.Xrm.Client.dll assembly
// located in the SDK\bin folder of the SDK download.
using Microsoft.Xrm.Client;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Client.Services;

using WebApplication3;

namespace WebApplication3
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (TextBox1.Text == "")
            {
                TextBox1.Text = "TheFa.com";
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            GetTableData();
            GetChartData();
            GetMeetingData();
            LastInteractionDate();
        }

        public void LastInteractionDate()
        {

            string Meetings = DataHugLastInteraction(TextBox1.Text).Result;

            JObject results = JObject.Parse(Meetings);

            Label3.Text = "IMG Colleague " + (string)results["Result"]["Companies"][0]["LastInteraction"]["ContactInfo"]["Name"] + " had the last interaction on " + (DateTime)results["Result"]["Companies"][0]["LastInteraction"]["InteractionDate"];

        }

        public void GetMeetingData(){
            
            string Meetings = DataHugCalendar(TextBox1.Text).Result;
                        
            JObject results = JObject.Parse(Meetings);

            DateTime d;
            DateTime ThreeMonthsAgo = DateTime.Today.AddMonths(-3);
            int count=0;
            foreach (var meeting in results["Result"]["Meetings"])
            {
                  d =(DateTime)meeting["StartTime"];
                  if (d > ThreeMonthsAgo)
                  {
                      count++;
                  }
            }
          Label2.Text = "Meetings in the last 3 Month: "+count.ToString();
    
        }

        private void GetTableData()
        {
            string DHContacts = DataHugConnections(TextBox1.Text).Result;
            string DHColleagues = DataHugColleagues(TextBox1.Text).Result;
            
            if (DHContacts != "test")
            {
                JObject results = JObject.Parse(DHColleagues);
                
                #region Colleagues
                DataTable col = new DataTable();
                col.Columns.Add("HugRank", typeof(string));
                col.Columns.Add("Name", typeof(string));
                col.Columns.Add("Title", typeof(string));
                //col.Columns.Add("Email", typeof(string));
                col.Columns.Add("InternalColleagueConnections", typeof(string));
                foreach (var result in results["Result"]["Connections"])
                {
                    col.Rows.Add((string)result["Hugrank"], (string)result["ContactInfo"]["Name"], (string)result["ContactInfo"]["Title"], (string)result["InternalColleagueConnections"]);//(string)result["ContactInfo"]["Email"],                 
                }
                var outObject = JsonConvert.DeserializeObject<RootObject>(DHColleagues);
                List<ColleagueDetails> list2 = new List<ColleagueDetails>();
                list2 = (from DataRow row in col.Rows

                         select new ColleagueDetails()
                         {
                             Hugrank = row["Hugrank"].ToString(),
                             Name = row["Name"].ToString(),
                             Title = row["Title"].ToString(),
                            // Email = row["Email"].ToString(),
                             InternalColleagueConnections = row["InternalColleagueConnections"].ToString()
                         }).ToList();

                GridView2.DataSource = list2;
                GridView2.DataBind();
                #endregion 

                #region DataHug Contacts
                var CRMData = GetCRMData("Url=https://imgmay.crm4.dynamics.com; Username=al@imgmay.onmicrosoft.com; Password=Liverpool2015;");
                DataTable dt = new DataTable();
                dt.Columns.Add("HugRank", typeof(string));
                dt.Columns.Add("Name", typeof(string));
                dt.Columns.Add("Title", typeof(string));
                //dt.Columns.Add("Email", typeof(string));
                dt.Columns.Add("InternalColleagueConnections", typeof(string));
                dt.Columns.Add("CRM", typeof(string));
                dt.Columns.Add("imagelogo", typeof(HyperLink));
                results = JObject.Parse(DHContacts);
                Label1.Text = "Total Contact: "+(string)results["TotalUnfilteredResults"];
                foreach (var result in results["Result"]["Connections"])
                {
                
                    dt.Rows.Add((string)result["Hugrank"], (string)result["ContactInfo"]["Name"], (string)result["ContactInfo"]["Title"], (string)result["InternalColleagueConnections"],
                    CheckEmailInCRM(CRMData, (string)result["ContactInfo"]["Email"]) ? "In CRM" : "Not in CRM");//, ResolveUrl("http://img.com/App_Themes/IMGWorld/images/logo.jpg"));
                }
                List<ContactDetails> list = new List<ContactDetails>();
                list = (from DataRow row in dt.Rows

                        select new ContactDetails()
                        {
                            Hugrank = row["Hugrank"].ToString(),
                            Name = row["Name"].ToString(),
                            Title = row["Title"].ToString(),
                           // Email = row["Email"].ToString(),
                            InternalColleagueConnections = row["InternalColleagueConnections"].ToString(),
                           // CRM = row["CRM"].ToString(),
                            //Image = Image1.ImageUrl
                        }).ToList();
                #endregion

                GridView1.DataSource = list;
                GridView1.DataBind();
            }
        }

        public static async Task<String> DataHugCalendar(string domain)
        {

            try
            {
                var username = "balpreet.mander@img.com";
                var password = "Liverpool2015";

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(
                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                        Convert.ToBase64String(
                            System.Text.ASCIIEncoding.ASCII.GetBytes(
                                string.Format("{0}:{1}", username, password))));

                    using (HttpResponseMessage response = client.GetAsync(
                                "https://api.datahug.com/Company/" + domain + "/Calendar?PageSize=500").Result)
                    {
                        response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();
                        return responseBody;
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return "test";
        }

        public static async Task<String> DataHugLastInteraction(string domain)
        {

            try
            {
                var username = "balpreet.mander@img.com";
                var password = "Liverpool2015";

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(
                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                        Convert.ToBase64String(
                            System.Text.ASCIIEncoding.ASCII.GetBytes(
                                string.Format("{0}:{1}", username, password))));

                    using (HttpResponseMessage response = client.GetAsync(
                                "https://api.datahug.com/Company/" + domain + "/").Result)
                    {
                        response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();
                        return responseBody;
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return "test";
        }


        public static async Task<String> DataHugConnections(string domain)
        {

            try
            {
                var username = "balpreet.mander@img.com";
                var password = "Liverpool2015";

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(
                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                        Convert.ToBase64String(
                            System.Text.ASCIIEncoding.ASCII.GetBytes(
                                string.Format("{0}:{1}", username, password))));

                    using (HttpResponseMessage response = client.GetAsync(
                                "https://api.datahug.com/Company/" + domain + "/Connections?PageSize=10&Type=1").Result)
                    {
                        response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();
                        return responseBody;
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return "test";
        }

        public static async Task<String> DataHugColleagues(string domain)
        {

            try
            {
                var username = "balpreet.mander@img.com";
                var password = "Liverpool2015";

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(
                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                        Convert.ToBase64String(
                            System.Text.ASCIIEncoding.ASCII.GetBytes(
                                string.Format("{0}:{1}", username, password))));

                    using (HttpResponseMessage response = client.GetAsync(
                                "https://api.datahug.com/Company/" + domain + "/Connections?type=0&PageSize=10").Result)
                    {
                        response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();
                        return responseBody;
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return "test";
        }
        
        public bool CheckEmailInCRM(List<Entity> CRMData,string email)
        {

            foreach (var crmemail in CRMData)
            {
                if ((string)crmemail.Attributes["emailaddress1"] == email)
                {
                    return true;
                }
            }

            return false;
        }

        public static async Task<String> DHChart(string domain)
        {

            try
            {
                var username = "balpreet.mander@img.com";
                var password = "Liverpool2015";

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(
                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                        Convert.ToBase64String(
                            System.Text.ASCIIEncoding.ASCII.GetBytes(
                                string.Format("{0}:{1}", username, password))));

                    using (HttpResponseMessage response = client.GetAsync(
                                "https://api.datahug.com/Company/" + domain + "/Analytics").Result)
                    {
                        response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();
                        return responseBody;
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return "test";
        }

        private void GetChartData()
        {
            string Response = DHChart(TextBox1.Text).Result;
            JObject results = JObject.Parse(Response);
            
            lbl_FromUsTotal.Text =  "Emails Sent: "+(string)results["PulseChart"]["Month3InteractionStats"]["FromUsTotal"];
            lbl_FromThemTotal.Text = "Emails Recieved: " + (string)results["PulseChart"]["Month3InteractionStats"]["FromThemTotal"];
            lbl_TotalEmailCount.Text = "Email Count: " + (string)results["PulseChart"]["Month3InteractionStats"]["TotalEmailCount"];
            lbl_TotalInteractionTime.Text = "Interaction Time: " + (string)results["PulseChart"]["Month3InteractionStats"]["TotalInteractionTime"];
            lbl_interactionratio.Text = "Email Ratio: " + (string)results["PulseChart"]["Month3InteractionStats"]["InteractionRatio"];
     
            List<string> ChartX = new List<string>();
            List<string> ChartY = new List<string>();
            List<string> ChartZ = new List<string>();

            Chart1.Series.Add("Test1");
            Chart1.Series.Add("Test2");
            Chart1.Series[1].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
            foreach (var result in results["PulseChart"]["FromUsSeries"])
            {
                ChartY.Add((string)result);
            }

            foreach (var result in results["PulseChart"]["XAxisCategories"])
            {
                ChartX.Add((string)result);
            }

            foreach (var result in results["PulseChart"]["FromThemSeries"])
            {
                ChartZ.Add((string)result);
            }

            for (int i = 0; i < ChartX.Count; i++)
            {
                Chart1.Series[0].Points.AddXY(ChartX[i], ChartY[i]);
            }

            for (int i = 0; i < ChartZ.Count; i++)
            {
                Chart1.Series[1].Points.AddXY(ChartX[i], System.Math.Abs(Convert.ToInt32(ChartZ[i])));
            }
           
            Chart1.DataBind();

        }

        #region UserDetails  - Gridview 1 CONTACTS
        public class ContactDetails
        {
            public string Hugrank { get; set; }
            public string Name { get; set; }
            public string Title { get; set; }
           // public string Email { get; set; }
            public string InternalColleagueConnections { get; set; }
            public string CRM { get; set; }
            public string Image { get; set; }
        }

        public class ColleagueDetails
        {
            public string Hugrank { get; set; }
            public string Name { get; set; }
            public string Title { get; set; }
            // public string Email { get; set; }
            public string InternalColleagueConnections { get; set; } 
        }
        #endregion

        #region JsonClasses


        public class ContactInfo
        {
            public string ContactId { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Domain { get; set; }
            public string CompanyName { get; set; }
            public string Phone { get; set; }
            public string ImId { get; set; }
            public string Title { get; set; }
            public string Location { get; set; }
            public string EntityType { get; set; }
        }

        public class ContactInfo2
        {
            public string ContactId { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Domain { get; set; }
            public string CompanyName { get; set; }
            public string Phone { get; set; }
            public string ImId { get; set; }
            public string Title { get; set; }
            public string Location { get; set; }
            public string EntityType { get; set; }
        }

        public class LastInteraction
        {
            public string InteractionDate { get; set; }
            public ContactInfo2 ContactInfo { get; set; }
        }

        public class ContactInfo3
        {
            public string ContactId { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Domain { get; set; }
            public string CompanyName { get; set; }
            public string Phone { get; set; }
            public string ImId { get; set; }
            public string Title { get; set; }
            public string Location { get; set; }
            public string EntityType { get; set; }
        }

        public class FirstInteraction
        {
            public string InteractionDate { get; set; }
            public ContactInfo3 ContactInfo { get; set; }
        }

        public class ContactInfo4
        {
            public string ContactId { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Domain { get; set; }
            public string CompanyName { get; set; }
            public string Phone { get; set; }
            public string ImId { get; set; }
            public string Title { get; set; }
            public string Location { get; set; }
            public string EntityType { get; set; }
        }

        public class NextMeeting
        {
            public string InteractionDate { get; set; }
            public ContactInfo4 ContactInfo { get; set; }
        }

        public class Organizer
        {
            public string ContactId { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Domain { get; set; }
            public string CompanyName { get; set; }
            public string Phone { get; set; }
            public string ImId { get; set; }
            public string Title { get; set; }
            public string Location { get; set; }
            public string EntityType { get; set; }
        }

        public class Attendee
        {
            public string ContactId { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Domain { get; set; }
            public string CompanyName { get; set; }
            public string Phone { get; set; }
            public string ImId { get; set; }
            public string Title { get; set; }
            public string Location { get; set; }
            public string EntityType { get; set; }
        }

        public class NextMeetingDetail
        {
            public string MeetingId { get; set; }
            public string StartTime { get; set; }
            public string EndTime { get; set; }
            public Organizer Organizer { get; set; }
            public List<Attendee> Attendees { get; set; }
        }

        public class Connection
        {
            public ContactInfo ContactInfo { get; set; }
            public string Photo { get; set; }
            public string Type { get; set; }
            public int Hugrank { get; set; }
            public bool KnownToUser { get; set; }
            public bool IsPrivate { get; set; }
            public bool IsFollowed { get; set; }
            public int TotalEmails { get; set; }
            public int TotalPhoneCalls { get; set; }
            public int TotalFutureMeetings { get; set; }
            public int TotalPastMeetings { get; set; }
            public int TotalConnections { get; set; }
            public int InternalColleagueConnections { get; set; }
            public int ConnectionsWithTheirOwnDomain { get; set; }
            public int ConnectionsWithExternalDomains { get; set; }
            public LastInteraction LastInteraction { get; set; }
            public FirstInteraction FirstInteraction { get; set; }
            public NextMeeting NextMeeting { get; set; }
            public NextMeetingDetail NextMeetingDetail { get; set; }
        }

        public class Result
        {
            public List<Connection> Connections { get; set; }
        }

        public class RootObject
        {
            public int Page { get; set; }
            public int PageSize { get; set; }
            public bool HasNextPage { get; set; }
            public string NextPage { get; set; }
            public string PreviousPage { get; set; }
            public int TotalUnfilteredResults { get; set; }
            public Result Result { get; set; }
        }

        #endregion
        
        private OrganizationService _orgService;
        public List<Entity> GetCRMData(String connectionString)
        {
                List<Entity> result = new List<Entity>();
            
                //Microsoft.Xrm.Client.CrmConnection connection = CrmConnection.Parse(connectionString);
                //using (_orgService = new OrganizationService(connection))
                //{
                //    OrganizationServiceContext orgSvcContext = new OrganizationServiceContext(_orgService);
                //    var getContact = from c in orgSvcContext.CreateQuery("contact")
                //                     join a in orgSvcContext.CreateQuery("account") on c["parentcustomerid"] equals a["accountid"]
                //                     where ((string)a["websiteurl"]).Contains("thefa")
                //                     select c;

                    
                //    foreach (var c in getContact)
                //    {
                //        TextBox2.Text += c.Attributes["emailaddress1"].ToString();
                //        result.Add(c);
                //    }
                   // TextBox2.Text += " Finish retriving Solution img";
                    return result;
                }
      
#region catch not working
            // Catch any service fault exceptions that Microsoft Dynamics CRM throws.
            //catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault>)
            //{
            //    // You can handle an exception here or pass it back to the calling method.
            //    throw;
            //
        
#endregion  
        

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        


    }
}