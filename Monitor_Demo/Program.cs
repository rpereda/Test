using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
//Browsers
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;

//Selenium support
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Support.UI;

namespace WebDriverTest
{
    /// <summary>
    /// Demo application for Selenium Test Driver
    /// Author: Raul Pereda
    /// Date: 12/28/2012
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            WIILP wiilp = new WIILP();
            try
            {
                IWebDriver specificBrowser = new ChromeDriver();
                wiilp.SetupTest(specificBrowser);
                wiilp.TheWIILPTest();
                //IWebDriver specificBrowser = new FirefoxDriver();
                //wiilp.SetupTest(specificBrowser);
                //wiilp.TheWIILPTest();
                Console.ReadLine();                
            }
            catch (Exception)
            {
                wiilp.TakeScreenShot();
            }
        }
    }
    public class WIILP
    {
        private IWebDriver driver;
        private StringBuilder verificationErrors;
        private string baseURL;
        private IWait<IWebDriver> wait;
        private double timeout = 90;
        private string imageLogsDirectory = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "ImageLogs");
        //[SetUp]
        public void SetupTest(IWebDriver driverp)
        {
            //driver = new ChromeDriver();
            //TODO: We can iterate drivers
            driver = driverp;
            //TODO: We can read the server from config file or table
            baseURL = "http://ebgdev03:81";
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
            //Opening the application page
            driver.Navigate().GoToUrl(baseURL + "/pdlApplication.aspx");            
            //waiting till page is fully loaded
            wait.Until(d => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
            //picking an specific state, ie. WI wich only have ILP
            //TODO: We can iterate each state from a table or collection
            driver.FindElement(By.CssSelector("option[value=\"WI\"]")).Click();
            Log("driver.FindElement(By.CssSelector(option[value=WI])).Click();");
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
            //waiting to load privacy policy page
            //wait.Until(d => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
            wait.Until(driver1 => driver.FindElement(By.Id("ctl00_PrimaryContent_A1")));
            // Agree to the privacy policy
            driver.FindElement(By.Id("ctl00_PrimaryContent_A1")).Click();
            Log("driver.FindElement(By.Id(ctl00_PrimaryContent_A1)).Click();");
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(timeout));
            //Waiting to load Application approved page
            wait.Until(d => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
            //Acept to credit agreement
            driver.FindElement(By.Id("ctl00_PrimaryContent_btnContinue")).Click();
            Log("driver.FindElement(By.Id(ctl00_PrimaryContent_btnContinue)).Click();");
            //Agree to credit agreement
            wait.Until(d => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
            driver.FindElement(By.Id("ctl00_PrimaryContent_eSignDocWizard1_RadioButtonList1_0")).Click();
            Log("driver.FindElement(By.Id(ctl00_PrimaryContent_eSignDocWizard1_RadioButtonList1_0)).Click();");
            //Next button on approved page
            wait.Until(d => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
            driver.FindElement(By.Id("ctl00_PrimaryContent_eSignDocWizard1_btnContinueDocument")).Click();
            Log("driver.FindElement(By.Id(ctl00_PrimaryContent_eSignDocWizard1_btnContinueDocument)).Click();");
            IWebElement message = driver.FindElement(By.Id("ctl00_PrimaryContent_pnlThankyouNewLoan")).FindElement(By.ClassName("eightcol")).FindElement(By.TagName("h3"));
            if (message != null && message.Text.ToLower().Contains("thank you"))
                Log("Application finished");
            else
                Log("Error");           
            message = driver.FindElement(By.Id("ctl00_PrimaryContent_pnlThankyouNewLoan")).FindElement(By.ClassName("eightcol")).FindElement(By.Id("ctl00_PrimaryContent_pnlDocumentNeeded")).FindElement(By.ClassName("prompt_msg_box"));
            if (message != null)
            {
                string loanNumber = message.Text.Substring(0,message.Text.IndexOf("."));
                loanNumber= loanNumber.ToLower().Replace("your application for installment loan #", String.Empty);
                loanNumber = loanNumber.ToLower().Replace("is incomplete", String.Empty);
                Log(String.Format("Loan: {0}",loanNumber));
                TakeScreenShot();
            }
            driver.FindElement(By.Id("ctl00_MyAccountsDefaultTopNavigation1_lnklogout2")).Click();
            Log("driver.FindElement(By.Id(ctl00_MyAccountsDefaultTopNavigation1_lnklogout2)).Click();");
            driver.Close();
            Log("End");            
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
            Console.WriteLine(DateTime.Now.TimeOfDay.ToString());
            Console.WriteLine(message);
        }
        public void TakeScreenShot()
        {
            Screenshot screenshot = ((ITakesScreenshot) driver).GetScreenshot();
            screenshot.SaveAsFile(System.IO.Path.Combine(imageLogsDirectory,String.Format("{0}{1}.png",DateTime.Now.ToShortDateString().Replace('/','_'),Guid.NewGuid().ToString())), System.Drawing.Imaging.ImageFormat.Png);
        }
    }
}
