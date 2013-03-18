using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
namespace MonitorWeb
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        [WebMethod]
        public static string DisplayData()
        {
            return DateTime.Now.ToString();
        }
        [WebMethod]
        public static List<Test> GetStatus()
        {
            List<Test> testList = new List<Test>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MonitorConnection"].ConnectionString))
            {
                StringBuilder query = new StringBuilder();
                query.Append("SELECT p .Page,AVG(DATEDIFF (S, Start,Finish )) TestTime,Max(p .AverageTime) AverageTime, Max(p.[Order]) [Order], MAX(tt.Finish) LastTime FROM TestTracking tt ");
                query.Append("INNER JOIN Phases p ON tt .IDPhase= p.IDPhase WHERE tt. TestNumber> (SELECT MAX(TestNumber ) FROM TestTracking) -5 ");
                query.Append("GROUP BY p.Page");
                using (SqlCommand cmd = new SqlCommand(query.ToString(), conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if(reader.HasRows)
                            while (reader.Read())
                            {
                                testList.Add(new Test(reader.GetString(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(3),reader.GetDateTime(4)));
                            }
                    }
                }
            };
            return testList;
        }
        [WebMethod]
        public static List<ILPFlow> GetTestFlow(string numberOfRows)
        {
            List<ILPFlow> testList = new List<ILPFlow>();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MonitorConnection"].ConnectionString))
                {
                    if (numberOfRows == null || numberOfRows.Equals(String.Empty))
                        numberOfRows = "1";
                    StringBuilder query = new StringBuilder();
                    query.Append("SELECT TOP " + numberOfRows + " TestNumber,Isnull(LoanNumber,'') LoanNumber,Email,Start,Finish,isnull([1],-1) AS [ApplicationForm],isnull([2],-1) AS [PrivacyPolicy],isnull([3],-1) AS [UpSell],isnull([4],-1) AS [eSign], ");
                    query.Append("	isnull([5],-1) [ThankYou] FROM ( SELECT TestNumber,LoanNumber,(SELECT MIN(Start) FROM TestTracking WHERE TestNumber=tt.TestNumber) AS Start,(SELECT MAX(Finish) FROM TestTracking WHERE TestNumber=tt.TestNumber) AS Finish,Email,IDPhase,DATEDIFF (S, Start,Finish )AS [Time] FROM TestTracking tt) AS P ");
                    query.Append("PIVOT ( MAX([Time]) FOR IDPhase in ([1],[2],[3],[4],[5])) AS Pvt Order by Pvt.TestNumber DESC");
                    DateTime start, finish;
                    start = finish=DateTime.MinValue;
                    using (SqlCommand cmd = new SqlCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                                while (reader.Read())
                                {
                                    if (!reader.IsDBNull(3))
                                        start = reader.GetDateTime(3);
                                    if (!reader.IsDBNull(4))
                                        finish = reader.GetDateTime(4);
                                    testList.Add(new ILPFlow(reader.GetInt64(0), reader.GetString(1), reader.GetString(2),start,finish, reader.GetInt32(5), reader.GetInt32(6), reader.GetInt32(7), reader.GetInt32(8), reader.GetInt32(9)));
                                }
                        }
                    }
                };
            }
            catch (Exception ex)
            { 
            }
            return testList;
        }
    }
//    [Serializable]
//    public class Test
//    {
//        public string Page {set;get;}
//        public int PageTime { set; get; }
//        public int AvgTime { set; get; }
//        public int Order { set; get; }
//        public string LasTime { set; get; }
//        public string LoanNumber { set; get; }
//        public Test(string page,int pageTime, int avgTime, int order, DateTime lastTime)
//        {
//            this.Page = page;
//            this.PageTime = pageTime;
//            this.AvgTime = avgTime;
//            this.Order = order;
//            this.LasTime = lastTime.ToString("MM/dd/yyyy hh:mm:ss");
//        }
//    }
//    [Serializable]
//    public class ILPFlow
//    {
//        public Int64 TestNumber { set; get; }
//        public string LoanNumber { set; get; }
//        public string Email { set; get; }
//        public string Start { set; get; }
//        public string Finish { set; get; }
//        public int ApplicationForm { set; get; }
//        public int PrivacyPolicy { set; get; }
//        public int UpSell { set; get; }
//        public int ESign { set; get; }
//        public int ThankYou { set; get; }

//        public ILPFlow(Int64 testNumber,string loanNumber, string email,DateTime start,DateTime finish, int applicationForm,int privacyPolicy,int upSell,int eSign,int thankYou)
//        {
//            this.TestNumber = testNumber;
//            this.LoanNumber = loanNumber;
//            this.Email = email;
//            this.Start = (start.Equals(DateTime.MinValue))?String.Empty: start.ToString("MM/dd/yyyy hh:mm:ss");
//            this.Finish = (finish.Equals(DateTime.MinValue))?String.Empty:finish.ToString("MM/dd/yyyy hh:mm:ss");
//            this.ApplicationForm = applicationForm;
//            this.PrivacyPolicy = privacyPolicy;
//            this.UpSell = upSell;
//            this.ESign = eSign;
//            this.ThankYou = thankYou;
//        }
//    }
}
