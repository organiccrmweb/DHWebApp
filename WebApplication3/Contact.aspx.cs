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
namespace WebApplication3
{
    public partial class Contact : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (TextBox1.Text == "")
            {
                TextBox1.Text = "nikki.smith@TheFa.com";
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            
            Analytics();
            Meetings();
            Colleagues();
        }

        public void Colleagues() {

            var Colleague = DataHugData(TextBox1.Text,"0").Result;
            JObject results = JObject.Parse(Colleague);

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

            GridView3.DataSource = list2;
            GridView3.DataBind();


            var Contacts = DataHugData(TextBox1.Text, "1").Result;
            JObject Contactresults = JObject.Parse(Contacts);

            DataTable td = new DataTable();
            td.Columns.Add("HugRank", typeof(string));
            td.Columns.Add("Name", typeof(string));
            td.Columns.Add("Title", typeof(string));
            //col.Columns.Add("Email", typeof(string));
            td.Columns.Add("InternalColleagueConnections", typeof(string));
            foreach (var result in Contactresults["Result"]["Connections"])
            {
                col.Rows.Add((string)result["Hugrank"], (string)result["ContactInfo"]["Name"], (string)result["ContactInfo"]["Title"], (string)result["InternalColleagueConnections"]);//(string)result["ContactInfo"]["Email"],                 
            }

            List<ColleagueDetails> list3 = new List<ColleagueDetails>();
            list3 = (from DataRow row in col.Rows

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


        
        }

        public static async Task<String> DataHugData(string domain,string type)
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
                                "https://api.datahug.com/Contact/" + domain + "/Connections?type="+type+"&PageSize=10").Result)
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
        
        public void Meetings()
        {
            var Meetings = MeetingData(TextBox1.Text).Result;
            JObject results = JObject.Parse(Meetings);
            lbl_totalmeeting.Text = "Total Meetings: " + (string)results["TotalPastMeetings"];            
        }


        public void Analytics()
        {
            var AnalyticsData = DataHugConnections(TextBox1.Text).Result;
            JObject results = JObject.Parse(AnalyticsData);

          string active= "Active: "+ (string)results["RelationshipStatus"]["Active"];
          string fading= "Fading: " + (string)results["RelationshipStatus"]["Fading"];
          string cold = "Cold: " + (string)results["RelationshipStatus"]["Cold"];
          string inactive = "Inactive: " + (string)results["RelationshipStatus"]["Inactive"];
          
          lbl_fromustotal.Text = "Emails Sent: "+(string)results["TotalInteractionStats"]["FromUsTotal"];
          lbl_fromthemtotal.Text = "Emails Received: " + (string)results["TotalInteractionStats"]["FromThemTotal"];
          lbl_totalemailcount.Text = "Total Emails: " + (string)results["TotalInteractionStats"]["TotalEmailCount"];
          lbl_totalinteractiontime.Text = "Interaction Time - Hours: " + ((int)results["TotalInteractionStats"]["TotalInteractionTime"] / 60) + " Minutes: "+((int)results["TotalInteractionStats"]["TotalInteractionTime"] % 60);
          lbl_interactionration.Text = "Interaction Ratio: " + (string)results["TotalInteractionStats"]["InteractionRatio"];
              


          Chart1.Series.Add("Test2");
          Chart1.Series[1].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Pie;


          
          Chart1.Series[1].Points.AddXY(active, (string)results["RelationshipStatus"]["Active"]);
          Chart1.Series[1].Points.AddXY(fading, (string)results["RelationshipStatus"]["Fading"]);
          Chart1.Series[1].Points.AddXY(cold, (string)results["RelationshipStatus"]["Cold"]);
          Chart1.Series[1].Points.AddXY(inactive, (string)results["RelationshipStatus"]["Inactive"]);

          Chart1.DataBind();

            
        }
        public static async Task<String> DataHugConnections(string email)
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
                                "https://api.datahug.com/Contact/" + email + "/Analytics").Result)
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
        public static async Task<String> MeetingData(string email)
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
                                "https://api.datahug.com/Contact/" + email + "/InteractionSummary").Result)
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
    
    }
}