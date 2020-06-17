using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace BM_Test
{
    [TestClass]
    public class TestLogin : BMSession
    {
        private static WindowsElement header;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Setup(context);
            header = session.FindElementByXPath("/Window[@AutomationId=\"_Login\"]");

        }

        [TestMethod]
        public void TestMethod1()
        {
            header.FindElementByClassName("TextBox").SendKeys(Keys.LeftControl + "a" + Keys.LeftControl);
            header.FindElementByClassName("TextBox").SendKeys("admin");
            header.FindElementByClassName("PasswordBox").SendKeys(Keys.LeftControl + "a" + Keys.LeftControl);
            header.FindElementByClassName("PasswordBox").SendKeys("admin");
            header.FindElementByXPath("/Window[@AutomationId=\"_Login\"]/Button[@ClassName=\"Button\"][@Name=\"Login\"]").Click();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TearDown();
        }
    }
}
