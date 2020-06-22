using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace BM_Test
{
    public class BMSession
    {
        // Note: append /wd/hub to the URL if you're directing the test at Appium
        protected const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";
        private static readonly string AppWorkingDir = @"C:\Users\trunghieu\source\repos\Bookstore_Management\Bookstore Management\bin\Debug\";
        private static readonly string AppPath = Path.Combine(AppWorkingDir, "BookStore Management.exe");

        protected static WindowsDriver<WindowsElement> session;

        public static void Setup(TestContext context)
        {
            if (session == null)
            {

                // Create a new session to bring up an instance of the Calculator application
                // Note: Multiple calculator windows (instances) share the same process Id
                AppiumOptions appCapabilities = new AppiumOptions();
                appCapabilities.AddAdditionalCapability("app", AppPath);
                appCapabilities.AddAdditionalCapability("platformName", "Windows");
                appCapabilities.AddAdditionalCapability("deviceName", "WindowsPC");

                //appCapabilities.SetCapability("appArguments", @"MyTestFile.txt");
                appCapabilities.AddAdditionalCapability("appWorkingDir", AppWorkingDir);
                session = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appCapabilities);


                Assert.IsNotNull(session);

                // Set implicit timeout to 1.5 seconds to make element search to retry every 500 ms for at most three times
                session.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1.5);

            }
        }

        public static void TearDown()
        {
            // Close the application and delete the session
            if (session != null)
            {
                session.Quit();
                session = null;
            }
        }
    }
}
