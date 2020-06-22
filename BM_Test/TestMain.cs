using System;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace BM_Test
{
    [TestClass]
    public class TestMain : BMSession
    {
        private static WindowsElement header;

        [TestInitialize]
        public void TestInitialize()
        {
            Setup(null);
            header = session.FindElementByAccessibilityId("_Login");
            header.FindElementByClassName("TextBox").SendKeys(Keys.LeftControl + "a" + Keys.LeftControl);
            header.FindElementByClassName("TextBox").SendKeys("admin");
            header.FindElementByClassName("PasswordBox").SendKeys(Keys.LeftControl + "a" + Keys.LeftControl);
            header.FindElementByClassName("PasswordBox").SendKeys("admin");
            header.FindElementByXPath("/Window[@AutomationId=\"_Login\"]/Button[@ClassName=\"Button\"][@Name=\"Login\"]").Click();
            Thread.Sleep(100);
            session.SwitchTo().Window(session.WindowHandles.First());
            header = session.FindElementByXPath("/Window[@Name=\"MainWindow\"][@AutomationId=\"_MainWindow\"]");
        }

        [TestMethod]
        public void Test_Main_Can_GoBack()
        {
            var controlBar = header.FindElementByAccessibilityId("uc");
            controlBar.FindElementByName("<").Click();
            Thread.Sleep(500);
            session.SwitchTo().Window(session.WindowHandles.First());
            var returnWindow = session.FindElementByAccessibilityId("_Login");
            Assert.IsNotNull(returnWindow);
        }

        [TestMethod]
        public void Test_Main_Can_Minimize()
        {
            var controlBar = header.FindElementByAccessibilityId("uc");
            controlBar.FindElementByName("_").Click();
            Assert.AreEqual("Minimized", header.GetAttribute("Window.WindowVisualState"));
        }

        [TestMethod]
        public void Test_Main_Can_Maximize()
        {
            var controlBar = header.FindElementByAccessibilityId("uc");
            controlBar.FindElementByAccessibilityId("Maximize").Click();
            Assert.AreEqual("Maximized", header.GetAttribute("Window.WindowVisualState"));
        }

        [TestMethod]
        public void Test_Main_Can_Close()
        {
            var controlBar = header.FindElementByAccessibilityId("uc");
            controlBar.FindElementByName("X").Click();
            Thread.Sleep(100);
            Assert.AreEqual(0, session.WindowHandles.Count);
        }

        [TestMethod]
        public void Test_Main_Can_Logout()
        {
            var controlBar = header.FindElementByAccessibilityId("uc");
            controlBar.FindElementByAccessibilityId("PART_Toggle").Click();
            var popup = header.FindElementByClassName("Popup");
            popup.FindElementByName("Đăng xuất").Click();
            Thread.Sleep(100);
            session.SwitchTo().Window(session.WindowHandles.First());
            var returnWindow = session.FindElementByAccessibilityId("_Login");
            Assert.IsNotNull(returnWindow);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            TearDown();
        }
    }
}
