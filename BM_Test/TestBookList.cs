using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BM_Test
{
    [TestClass]
    public class TestBookList: BMSession
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
        public void Test_BookList_Filter()
        {
            var menubar = header.FindElementByClassName("MyMenuBar");
            menubar.FindElementByName("Nhà sách").Click();
            var frame = header.FindElementByClassName("Frame");
            frame.FindElementByAccessibilityId("PART_Toggle").Click();
            var popup = header.FindElementByClassName("Popup");
            popup.FindElementByName("Hài").Click();
            frame.FindElementByAccessibilityId("PART_Toggle").Click();
            var x = header.FindElementByName("Những Mẩu Chuyện Khôi Hài Ý Vị");
            Assert.IsNotNull(x);
            frame.FindElementByAccessibilityId("PART_Toggle").Click();
            popup.FindElementByName("Hài").Click();
            frame.FindElementByAccessibilityId("PART_Toggle").Click();
        }

        [TestMethod]
        public void Test_BookList_Search()
        {
            var menubar = header.FindElementByClassName("MyMenuBar");
            menubar.FindElementByName("Nhà sách").Click();
            var frame = header.FindElementByClassName("Frame");
            frame.FindElementByClassName("TextBox").SendKeys("nhà giả kim");
            var x = header.FindElementByName("Nhà Giả Kim");
            Assert.IsNotNull(x);
            frame.FindElementByClassName("TextBox").SendKeys(Keys.LeftControl + "a");
            frame.FindElementByClassName("TextBox").SendKeys("kim nhà giả");
            var y = header.FindElementByName("Không tìm thấy sách");
            Assert.IsNotNull(y);
        }

        [TestMethod]
        public void Test_BookList_Order_Create()
        {
            var menubar = header.FindElementByClassName("MyMenuBar");
            menubar.FindElementByName("Nhà sách").Click();
            var scrollView = header.FindElementByClassName("ScrollViewer");
            scrollView.FindElementByClassName("Image").Click();
            scrollView.FindElementByName("Thêm vào giỏ hàng").Click();

            session.FindElementsByXPath("/Window[@Name=\"MainWindow\"][@AutomationId=\"_MainWindow\"]/Pane[@ClassName=\"Frame\"]/Button[@AutomationId=\"PART_Toggle\"]").Last().Click();
            var popup = header.FindElementByClassName("Window");
            
            popup.FindElementByName("Thanh toán").Click();

            session.SwitchTo().Window(session.WindowHandles.First());
            header = session.FindElementByClassName("Window");

            header.FindElementsByClassName("ComboBox")[0].SendKeys("Vũ Tuấn Hải");
            header.FindElementsByClassName("ComboBox")[1].SendKeys(Keys.ArrowDown + Keys.ArrowDown);

            header.FindElementByName("Đồng ý").Click();

            session.SwitchTo().Window(session.WindowHandles.First());
            header = session.FindElementByClassName("Window");
            var x = header.FindElementByName("Thêm hóa đơn thành công");

            Assert.IsNotNull(x);
            header.FindElementByName("Đồng ý").Click();

            session.SwitchTo().Window(session.WindowHandles.First());
            header = session.FindElementByClassName("Window");
        }

        [TestMethod]
        public void Test_BookList_Order_Abort()
        {
            var menubar = header.FindElementByClassName("MyMenuBar");
            menubar.FindElementByName("Nhà sách").Click();
            var scrollView = header.FindElementByClassName("ScrollViewer");
            scrollView.FindElementByClassName("Image").Click();
            scrollView.FindElementByName("Thêm vào giỏ hàng").Click();

            session.FindElementsByXPath("/Window[@Name=\"MainWindow\"][@AutomationId=\"_MainWindow\"]/Pane[@ClassName=\"Frame\"]/Button[@AutomationId=\"PART_Toggle\"]").Last().Click();
            var popup = header.FindElementByClassName("Window");

            popup.FindElementByName("Thanh toán").Click();

            session.SwitchTo().Window(session.WindowHandles.First());
            header = session.FindElementByClassName("Window");

            header.FindElementsByClassName("ComboBox")[0].SendKeys("Vũ Tuấn Hải");
            header.FindElementsByClassName("ComboBox")[1].SendKeys(Keys.ArrowDown + Keys.ArrowDown);

            header.FindElementByName("Quay lại").Click();

            session.SwitchTo().Window(session.WindowHandles.First());
            header = session.FindElementByClassName("Window");
            menubar = header.FindElementByClassName("MyMenuBar");
            var x = menubar.FindElementByName("Nhà sách");
            Assert.IsNotNull(x);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TearDown();
        }
    }
}
