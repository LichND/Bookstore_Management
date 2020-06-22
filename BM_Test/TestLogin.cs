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
    public class TestLogin : BMSession
    {
        private static WindowsElement header;

        [TestInitialize]
        public void TestInitialize()
        {
            TearDown();
            Setup(null);
            header = session.FindElementByClassName("Window");
        }

        [TestMethod]
        [DataRow("admin", "admin")]
        public void Test_Login_Valid(string object1, string object2)
        {
            header.FindElementByClassName("TextBox").SendKeys(Keys.LeftControl + "a");
            header.FindElementByClassName("TextBox").SendKeys(object1);
            header.FindElementByClassName("PasswordBox").SendKeys(Keys.LeftControl + "a");
            header.FindElementByClassName("PasswordBox").SendKeys(object2);
            header.FindElementByXPath("/Window[@AutomationId=\"_Login\"]/Button[@ClassName=\"Button\"][@Name=\"Login\"]").Click();
            var result = header.FindElementByAccessibilityId("Warning").Text;
            Assert.AreNotEqual("Sai tài khoản hoặc mật khẩu", result);
        }

        [TestMethod]
        [DataRow("admin", "admin1")]
        public void Test_Login_Invalid(string object1, string object2)
        {
            header.FindElementByClassName("TextBox").SendKeys(Keys.LeftControl + "a");
            header.FindElementByClassName("TextBox").SendKeys(object1);
            header.FindElementByClassName("PasswordBox").SendKeys(Keys.LeftControl + "a");
            header.FindElementByClassName("PasswordBox").SendKeys(object2);
            header.FindElementByXPath("/Window[@AutomationId=\"_Login\"]/Button[@ClassName=\"Button\"][@Name=\"Login\"]").Click();
            var result = header.FindElementByAccessibilityId("Warning").Text;
            Assert.AreEqual("Sai tài khoản hoặc mật khẩu", result);
        }

        [TestMethod]
        [DataRow("admin", "")]
        public void Test_Login_No_Password(string object1, string object2)
        {
            header.FindElementByClassName("TextBox").SendKeys(Keys.LeftControl + "a");
            header.FindElementByClassName("TextBox").SendKeys(Keys.Backspace);
            header.FindElementByClassName("TextBox").SendKeys(object1);
            header.FindElementByClassName("PasswordBox").SendKeys(Keys.LeftControl + "a");
            header.FindElementByClassName("PasswordBox").SendKeys(Keys.Backspace);
            header.FindElementByClassName("PasswordBox").SendKeys(object2);
            header.FindElementByXPath("/Window[@AutomationId=\"_Login\"]/Button[@ClassName=\"Button\"][@Name=\"Login\"]").Click();
            var result = header.FindElementByAccessibilityId("Warning").Text;
            Assert.AreEqual("Mật khẩu không được để trống!", result);
        }

        [TestMethod]
        [DataRow("", "admin")]
        public void Test_Login_No_Username(string object1, string object2)
        {
            header.FindElementByClassName("TextBox").SendKeys(Keys.LeftControl + "a");
            header.FindElementByClassName("TextBox").SendKeys(Keys.Backspace);
            header.FindElementByClassName("TextBox").SendKeys(object1);
            header.FindElementByClassName("PasswordBox").SendKeys(Keys.LeftControl + "a");
            header.FindElementByClassName("PasswordBox").SendKeys(Keys.Backspace);
            header.FindElementByClassName("PasswordBox").SendKeys(object2);
            header.FindElementByXPath("/Window[@AutomationId=\"_Login\"]/Button[@ClassName=\"Button\"][@Name=\"Login\"]").Click();
            var result = header.FindElementByAccessibilityId("Warning").Text;
            Assert.AreEqual("Tài khoản không được để trống!", result);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            TearDown();
        }
    }
}
