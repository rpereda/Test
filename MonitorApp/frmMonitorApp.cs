using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenQA.Selenium;
//Browsers
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;

//Selenium support
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Support.UI;

// SQL references
using System.Data.SqlClient;
using System.Configuration;

namespace MonitorApp
{
    /// <summary>
    /// Demo application for Selenium Test Driver
    /// Author: Raul Pereda
    /// Date: 12/28/2012
    /// </summary>
    public partial class frmMonitor : Form
    {
        LogEventHandler logEvent;
        WIILP wiilp;
        System.Threading.Thread thread;
        public frmMonitor()
        {
            InitializeComponent();
            
            
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnStart.Text == "Start")
                {
                    btnStart.Text = "Stop";
                    wiilp = new WIILP((rb1.Checked) ? txtSite1.Text : txtSite2.Text);
                    wiilp.logEvent += new LogEventHandler(this.WriteLog);
                    thread = new System.Threading.Thread(new System.Threading.ThreadStart(wiilp.TheWIILPTest));
                    thread.Start();
                    //wiilp.TheWIILPTest();
                }
                else
                {
                    btnStart.Text = "Start";
                    thread.Abort();
                }
            }
            catch
            {
                if (thread != null)
                    thread.Abort();
            }

        }
        public void WriteLog(string message)
        {
            if (txtLog.InvokeRequired)
                txtLog.Invoke(new MethodInvoker(delegate { txtLog.AppendText(message+Environment.NewLine); }));            
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtLog.Clear();
        }
    }

    public class WIILP
    {
        public LogEventHandler logEvent;
        private IWebDriver driver;
        private StringBuilder verificationErrors;
        private string baseURL;
        private IWait<IWebDriver> wait;
        private double timeout = 360;
        private string _site = "http://ebgdev03:81";
        private string imageLogsDirectory = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "ImageLogs");

        public WIILP(string site)
        {
            this._site = site;
        }
        
        //[SetUp]
        public void SetupTest()
        {

                driver = new ChromeDriver();
                //TODO: We can read the server from config file or table
                baseURL = this._site;
                verificationErrors = new StringBuilder();
                wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
            //Logs directories
            if (!System.IO.Directory.Exists(imageLogsDirectory))
                System.IO.Directory.CreateDirectory(imageLogsDirectory);
        }

        //[TearDown]
        public void TeardownTest()
        {
            try
            {
                driver.Quit();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
            //Assert.AreEqual("", verificationErrors.ToString());
        }

        //[Test]
        public void TheWIILPTest()
        {
            while (true)
            {
                try
                {
                    SetupTest();
                    //Opening the application page

                    driver.Navigate().GoToUrl(baseURL + "/pdlApplication.aspx");
                    //waiting till page is fully loaded
                    wait.Until(d => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
                    //picking an specific state, ie. WI wich only have ILP
                    //TODO: We can iterate each state from a table or collection
                    String email = driver.FindElement(By.Id("tbxEmailAddressA")).GetAttribute("value");
                    //We got an ID for the whole test
                    int idTestNumber = StartTracking(ApplicationPhases.ApplicationForm,email);
                    //System.Threading.Thread.Sleep(2000);
                    driver.FindElement(By.CssSelector("option[value=\"WI\"]")).Click();
                    Log("driver.FindElement(By.CssSelector(option[value=WI])).Click();");
                    //Log("driver.FindElement(By.CssSelector(option[value=WI])).Click();");
                    //Since there is an ajax call we need to freeze the script for a bit
                    System.Threading.Thread.Sleep(30000);
                    //Validating the textbox exists
                    wait.Until(d => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
                    wait.Until(d => d.FindElement(By.Id("tbxLoanAmount")));
                    //Setting up an specific amount
                    driver.FindElement(By.Id("tbxLoanAmount")).SendKeys("300");
                    Log("driver.FindElement(By.Id(tbxLoanAmount)).SendKeys(300);");
                    //taking an screenshot before submit the application
                    TakeScreenShot();
                    //Submitting the application 
                    wait.Until(d => d.FindElement(By.Id("btnNextStep")));
                    driver.FindElement(By.Id("btnNextStep")).Click();
                    Log(@"driver.FindElement(By.Id(btnNextStep)).Click();");
                    EndPhase(ApplicationPhases.ApplicationForm, idTestNumber,email);

                    //waiting to load privacy policy page
                    //wait.Until(d => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));

                    StartPhase(ApplicationPhases.PrivacyPolicy, idTestNumber,email);
                    wait.Until(driver1 => driver.FindElement(By.Id("ctl00_PrimaryContent_A1")));
                    // Agree to the privacy policy
                    driver.FindElement(By.Id("ctl00_PrimaryContent_A1")).Click();
                    Log("driver.FindElement(By.Id(ctl00_PrimaryContent_A1)).Click();");
                    EndPhase(ApplicationPhases.PrivacyPolicy, idTestNumber,email);

                    StartPhase(ApplicationPhases.UpSell, idTestNumber,email);
                    driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(timeout));
                    //Waiting to load Application approved page
                    wait.Until(d => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
                    //Acept to credit agreement
                    driver.FindElement(By.Id("ctl00_PrimaryContent_btnContinue")).Click();
                    Log("driver.FindElement(By.Id(ctl00_PrimaryContent_btnContinue)).Click();");
                    EndPhase(ApplicationPhases.UpSell, idTestNumber,email);
                    //Agree to credit agreement
                    StartPhase(ApplicationPhases.Esign, idTestNumber,email);
                    wait.Until(d => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
                    driver.FindElement(By.Id("ctl00_PrimaryContent_eSignDocWizard1_RadioButtonList1_0")).Click();
                    Log("driver.FindElement(By.Id(ctl00_PrimaryContent_eSignDocWizard1_RadioButtonList1_0)).Click();");
                    //Next button on approved page
                    wait.Until(d => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
                    driver.FindElement(By.Id("ctl00_PrimaryContent_eSignDocWizard1_btnContinueDocument")).Click();
                    Log("driver.FindElement(By.Id(ctl00_PrimaryContent_eSignDocWizard1_btnContinueDocument)).Click();");
                    EndPhase(ApplicationPhases.Esign, idTestNumber,email);
                    StartPhase(ApplicationPhases.ThankYou, idTestNumber,email);
                    IWebElement message = driver.FindElement(By.Id("ctl00_PrimaryContent_pnlThankyouNewLoan")).FindElement(By.ClassName("eightcol")).FindElement(By.TagName("h3"));
                    if (message != null && message.Text.ToLower().Contains("thank you"))
                        Log("Application finished");
                    else
                        Log("Error");
                    message = driver.FindElement(By.Id("ctl00_PrimaryContent_pnlThankyouNewLoan")).FindElement(By.ClassName("eightcol")).FindElement(By.Id("ctl00_PrimaryContent_pnlDocumentNeeded")).FindElement(By.ClassName("prompt_msg_box"));
                    string loanNumber=String.Empty;
                    if (message != null)
                    {
                        loanNumber = message.Text.Substring(0, message.Text.IndexOf("."));
                        loanNumber = loanNumber.ToLower().Replace("your application for installment loan #", String.Empty);
                        loanNumber = loanNumber.ToLower().Replace("is incomplete", String.Empty);
                        Log(String.Format("Loan: {0}", loanNumber));
                        TakeScreenShot();
                    }
                    EndTracking(ApplicationPhases.ThankYou, idTestNumber,loanNumber);
                    driver.FindElement(By.Id("ctl00_MyAccountsDefaultTopNavigation1_lnklogout2")).Click();
                    Log("driver.FindElement(By.Id(ctl00_MyAccountsDefaultTopNavigation1_lnklogout2)).Click();");
                }
                catch
                { }
                finally
                {
                    driver.Close();
                    driver.Quit();
                    driver.Dispose();
                    Log("End");
                }
                System.Threading.Thread.Sleep(30000);
            }
        }

        private int StartTracking(ApplicationPhases phase, string email)
        {
            int idTestTracking=0;
            try
            {                
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MonitorConnection"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("DECLARE @TESTNUMBER BIGINT;SELECT @TESTNUMBER=ISNULL(MAX(TestNumber),0)+1 from TestTracking;  INSERT INTO TESTTRACKING(TESTNUMBER,IDPHASE,START,SESSIONID,EMAIL) VALUES (@TESTNUMBER,@IDPHASE,GETDATE(),@SESSIONID,@EMAIL);SELECT @TESTNUMBER;", conn))
                    {

                        System.Collections.ObjectModel.ReadOnlyCollection<Cookie> cookies = driver.Manage().Cookies.AllCookies;
                        cmd.Parameters.Add(new SqlParameter("IDPHASE", (int)phase));
                        cmd.Parameters.Add(new SqlParameter("SESSIONID", cookies[0].Value));
                        cmd.Parameters.Add(new SqlParameter("EMAIL", email));
                        conn.Open();
                        idTestTracking = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch(Exception e)
            {
                Log(String.Format("THERE WAS A DATABASE ERROR:{0}",e.Message));
            }
            return idTestTracking;
        }
        private void StartPhase(ApplicationPhases phase, int idTestNumber,string email)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MonitorConnection"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO TESTTRACKING(TESTNUMBER,IDPHASE,START,SESSIONID,EMAIL) VALUES (@TESTNUMBER,@IDPHASE,GETDATE(),@SESSIONID,@EMAIL);", conn))
                    {

                        System.Collections.ObjectModel.ReadOnlyCollection<Cookie> cookies = driver.Manage().Cookies.AllCookies;
                        cmd.Parameters.Add(new SqlParameter("IDPHASE", (int)phase));
                        cmd.Parameters.Add(new SqlParameter("SESSIONID", cookies[0].Value));
                        cmd.Parameters.Add(new SqlParameter("TESTNUMBER", idTestNumber));
                        cmd.Parameters.Add(new SqlParameter("EMAIL", email));
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                Log(String.Format("THERE WAS A DATABASE ERROR:{0}", e.Message));
            }
        }
        private void EndPhase(ApplicationPhases phase, int idTestNumber,string email)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MonitorConnection"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("UPDATE TESTTRACKING SET FINISH=GETDATE() WHERE TESTNUMBER=@TESTNUMBER AND IDPHASE=@IDPHASE;UPDATE TESTTRACKING SET EMAIL=@EMAIL WHERE TESTNUMBER=@TESTNUMBER;UPDATE TESTTRACKING SET EMAIL=@EMAIL WHERE TESTNUMBER=@TESTNUMBER;", conn))
                    {

                        System.Collections.ObjectModel.ReadOnlyCollection<Cookie> cookies = driver.Manage().Cookies.AllCookies;
                        cmd.Parameters.Add(new SqlParameter("TESTNUMBER", idTestNumber));
                        cmd.Parameters.Add(new SqlParameter("IDPHASE", (int)phase));
                        cmd.Parameters.Add(new SqlParameter("EMAIL", email));
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                };
            }
            catch (Exception e)
            {
                Log(String.Format("THERE WAS A DATABASE ERROR:{0}", e.Message));
            }
        }
        private void EndTracking(ApplicationPhases phase, int idTestNumber,string LoanNumber)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MonitorConnection"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("UPDATE TESTTRACKING SET FINISH=GETDATE() WHERE TESTNUMBER=@TESTNUMBER AND IDPHASE=@IDPHASE;UPDATE TESTTRACKING SET LoanNumber=@LoanNumber WHERE TESTNUMBER=@TESTNUMBER", conn))
                    {

                        System.Collections.ObjectModel.ReadOnlyCollection<Cookie> cookies = driver.Manage().Cookies.AllCookies;
                        cmd.Parameters.Add(new SqlParameter("TESTNUMBER", idTestNumber));
                        cmd.Parameters.Add(new SqlParameter("IDPHASE", (int)phase));
                        cmd.Parameters.Add(new SqlParameter("LoanNumber", LoanNumber));
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                };
            }
            catch (Exception e)
            {
                Log(String.Format("THERE WAS A DATABASE ERROR:{0}", e.Message));
            }
        }
        private bool IsElementPresent(By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
        private void Log(string message)
        {
            if (logEvent != null)
            {
                logEvent(message);
            }
            else
            {
                Console.WriteLine(DateTime.Now.TimeOfDay.ToString());
                Console.WriteLine(message);
            }
        }
        public void TakeScreenShot()
        {
            Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            screenshot.SaveAsFile(System.IO.Path.Combine(imageLogsDirectory, String.Format("{0}{1}.png", DateTime.Now.ToShortDateString().Replace('/', '_'), Guid.NewGuid().ToString())), System.Drawing.Imaging.ImageFormat.Png);
        }
    }
    public delegate void LogEventHandler(string message);
    public enum ApplicationPhases
    {
        ApplicationForm=1,
        PrivacyPolicy=2,
        UpSell=3,
        Esign=4,
        ThankYou = 5
    }
}
