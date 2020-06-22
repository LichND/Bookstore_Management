using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BM_Test
{
    public class BaseTestApp : BMSession
    {
        protected static WindowsElement loginHeader;
        protected static WindowsElement mainAppHeader;
        protected static WindowsElement menuBar;

        public static void AutoLogin()
        {
            loginHeader.FindElementByClassName("TextBox").SendKeys(Keys.LeftControl + "a");
            loginHeader.FindElementByClassName("TextBox").SendKeys("admin");
            loginHeader.FindElementByClassName("PasswordBox").SendKeys(Keys.LeftControl + "a");
            loginHeader.FindElementByClassName("PasswordBox").SendKeys("admin");
            loginHeader.FindElementByXPath("/Window[@AutomationId=\"_Login\"]/Button[@ClassName=\"Button\"][@Name=\"Login\"]").Click();
        }

        public static void NavigateMainWindow()
        {
            session.SwitchTo().Window(session.WindowHandles.First());
            mainAppHeader = session.FindElementByXPath("/Window[@Name=\"MainWindow\"][@AutomationId=\"_MainWindow\"]");
            Thread.Sleep(500);
            menuBar = mainAppHeader.FindElementByClassName("MyMenuBar") as WindowsElement;
        }

        protected static void SwitchToPopupWindow()
        {
            session.SwitchTo().Window(session.WindowHandles.First());
        }
    }
}
