using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BM_Test
{
    [TestClass]
    public class TestBookManager: BMSession
    {
        private static WindowsElement header;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Setup(context);
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
        [DataRow("100000", "Tiếng Việt 1", "Ban Biên Tập", "NXB Giao Duc", "2019", "S", "Sách Tiếng Việt cho học sinh lớp 1", "99000")]
        public void Test_BookManager_Add(string d1, string d2, string d3, string d4, string d5, string d6, string d7, string d8)
        {
            var img = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Images\tieng-viet-1.jpg");
            var menubar = header.FindElementByClassName("MyMenuBar");
            menubar.FindElementByName("Kho sách").Click();
            var frame = header.FindElementByClassName("Frame");
            frame.FindElementByName("Thông tin").Click();
            frame.FindElementByAccessibilityId("AddBtn").Click();
            session.SwitchTo().Window(session.WindowHandles.First());
            header = session.FindElementByClassName("Window");
            var tbs = header.FindElementsByClassName("TextBox");
            tbs[0].SendKeys(d1);
            tbs[1].SendKeys(d2);
            tbs[2].SendKeys(d3);
            tbs[3].SendKeys(d4);
            tbs[4].SendKeys(d5);
            tbs[5].SendKeys(d7);
            tbs[6].SendKeys(d8);
            tbs[7].SendKeys(d6 + Keys.Enter);
            header.FindElementByAccessibilityId("ChooseImageBtn").Click();
            session.SwitchTo().Window(session.WindowHandles.First());
            header = session.FindElementByName("Open");
            var comboBox = header.FindElementByClassName("ComboBox");
            comboBox.FindElementByClassName("Edit").SendKeys(img + Keys.Enter);
            session.SwitchTo().Window(session.WindowHandles.First());
            header = session.FindElementByClassName("Window");
            header.FindElementByName("Đồng ý").Click();
            session.SwitchTo().Window(session.WindowHandles.First());
            header = session.FindElementByClassName("Window");
            Assert.AreEqual("Thêm sách thành công", header.FindElementByClassName("TextBlock").Text);
            header.FindElementByName("Đồng ý").Click();
            session.SwitchTo().Window(session.WindowHandles.First());
            header = session.FindElementByClassName("Window");
            var c = header.FindElementsByClassName("DataGridCell").Where(x => x.Text == d2).Count();
            Assert.AreNotEqual(0, c);
        }

        [TestMethod]
        [DataRow("Tiếng Việt 1", "200000", "100000")]
        public void Test_BookManager_Edit(string d1, string d2, string d3)
        {
            var menubar = header.FindElementByClassName("MyMenuBar");
            menubar.FindElementByName("Kho sách").Click();
            var frame = header.FindElementByClassName("Frame");
            frame.FindElementByName("Thông tin").Click();
            header.FindElementByName("ID").Click();
            header.FindElementByName("ID").Click();
            var item = header.FindElementsByClassName("DataGridCell").Where(x => x.Text == d1).SingleOrDefault();
            item.Click();
            frame.FindElementByName("Sửa").Click();

            session.SwitchTo().Window(session.WindowHandles.First());
            header = session.FindElementByClassName("Window");
            var tbs = header.FindElementsByClassName("TextBox");
            tbs[0].SendKeys(Keys.LeftControl + "a");
            tbs[0].SendKeys(Keys.Backspace + d2);
            tbs[6].SendKeys(Keys.LeftControl + "a");
            tbs[6].SendKeys(Keys.Backspace + d3);
            header.FindElementByName("Đồng ý").Click();
            session.SwitchTo().Window(session.WindowHandles.First());
            header = session.FindElementByClassName("Window");
            Assert.AreEqual("Sửa thành công", header.FindElementByClassName("TextBlock").Text);
            header.FindElementByName("Đồng ý").Click();
            session.SwitchTo().Window(session.WindowHandles.First());
            header = session.FindElementByClassName("Window");
        }

        [TestMethod]
        [DataRow("Tiếng Việt 1")]
        public void Test_BookManager_Delete(string d)
        {
            var menubar = header.FindElementByClassName("MyMenuBar");
            menubar.FindElementByName("Kho sách").Click();
            var frame = header.FindElementByClassName("Frame");
            frame.FindElementByName("Thông tin").Click();
            header.FindElementByName("ID").Click();
            header.FindElementByName("ID").Click();
            var item = header.FindElementsByClassName("DataGridCell").Where(x => x.Text == d).SingleOrDefault();
            item.Click();
            frame.FindElementByName("Xóa").Click();
            session.SwitchTo().Window(session.WindowHandles.First());
            header = session.FindElementByClassName("Window");
            Assert.AreEqual("Xóa thành công", header.FindElementByClassName("TextBlock").Text);
            header.FindElementByName("Đồng ý").Click();
            session.SwitchTo().Window(session.WindowHandles.First());
            header = session.FindElementByClassName("Window");
        }

            /*[ClassCleanup]
            public static void ClassCleanup()
            {
                TearDown();
            }*/
        }
}
