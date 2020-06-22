using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BM_Test
{
    [TestClass]
    public class Test_TaiKhoan : BaseTestApp
    {
        private static WindowsElement panel_TaiKhoan;
        private static WindowsElement currentPanel;
        private static WindowsElement button_ThongTin;
        private static WindowsElement button_QuanLiTaiKhoan;
        private static WindowsElement window_CapNhatThongTin;

        [TestMethod]
        public void TTTK014_KiemTraButtonCapNhat()
        {
            try
            { 
                Open_Window_CapNhatThongTin();
                Assert.IsTrue(true);
                Close_Window_CapNhatThongTin();
            } catch
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        [DataRow("Nguyễn Văn B", 1, "0123456789", "Hồ Chí Minh")]
        public void TTTK015_CapNhatThongTin(object tenTK, object gioitinhIndex, object sdt, object diachi)
        {
            Open_Window_CapNhatThongTin();

            var textboxes = window_CapNhatThongTin.FindElementsByClassName("TextBox");

            textboxes[0].SendKeys(Keys.LeftControl + "a");
            textboxes[0].SendKeys((string)tenTK);
            Chon_GioiTinh((int)gioitinhIndex);
            textboxes[3].SendKeys(Keys.LeftControl + "a");
            textboxes[3].SendKeys((string)sdt);
            textboxes[5].SendKeys(Keys.LeftControl + "a");
            textboxes[5].SendKeys((string)diachi);


            window_CapNhatThongTin.FindElementByName("Cập nhật").Click();
            SwitchToPopupWindow();

            Assert.AreEqual(session.FindElementByName((string)tenTK).Text, (string)tenTK);
        }

        [TestMethod]
        [DataRow("Nguyễn Văn B", 1, "0123456789", "Hồ Chí Minh")]
        public void TTTK016_CapNhatThongTin_KhongNhapTen(object tenTK, object gioitinhIndex, object sdt, object diachi)
        {
            Open_Window_CapNhatThongTin();

            var textboxes = window_CapNhatThongTin.FindElementsByClassName("TextBox");

            textboxes[0].SendKeys(Keys.LeftControl + "a" + Keys.Backspace);
            Chon_GioiTinh((int)gioitinhIndex);
            textboxes[3].SendKeys(Keys.LeftControl + "a");
            textboxes[3].SendKeys((string)sdt);
            textboxes[5].SendKeys(Keys.LeftControl + "a");
            textboxes[5].SendKeys((string)diachi);


            window_CapNhatThongTin.FindElementByName("Cập nhật").Click();

            Assert.AreEqual(textboxes[1].Text.ToLower(), "nhập tên người dùng!");
            Close_Window_CapNhatThongTin();
        }

        [TestMethod]
        [DataRow("Nguyễn Văn B", 1, "0123456789", "Hồ Chí Minh")]
        public void TTTK017_CapNhatThongTin_KhongNhapSDT(object tenTK, object gioitinhIndex, object sdt, object diachi)
        {
            Open_Window_CapNhatThongTin();

            var textboxes = window_CapNhatThongTin.FindElementsByClassName("TextBox");

            textboxes[3].SendKeys(Keys.LeftControl + "a" + Keys.Backspace);

            window_CapNhatThongTin.FindElementByName("Cập nhật").Click();

            Assert.AreEqual(textboxes[4].Text.ToLower(), "nhập số điện thoại!");
            Close_Window_CapNhatThongTin();
        }

        [TestMethod]
        [DataRow("Nguyễn Văn B", 1, "0123456789", "Hồ Chí Minh")]
        public void TTTK018_CapNhatThongTin_KhongNhapDiaChi(object tenTK, object gioitinhIndex, object sdt, object diachi)
        {
            Open_Window_CapNhatThongTin();

            var textboxes = window_CapNhatThongTin.FindElementsByClassName("TextBox");

            textboxes[5].SendKeys(Keys.LeftControl + "a" + Keys.Backspace);

            window_CapNhatThongTin.FindElementByName("Cập nhật").Click();

            Assert.AreEqual(textboxes[6].Text.ToLower(), "nhập địa chỉ!");
            Close_Window_CapNhatThongTin();
        }

        [TestMethod]
        [DataRow("Nguyễn Văn B", 1, "0123456789", "Hồ Chí Minh")]
        public void TTTK019_CapNhatThongTin_Huy(object tenTK, object gioitinhIndex, object sdt, object diachi)
        {
            Open_Window_CapNhatThongTin();

            var textboxes = window_CapNhatThongTin.FindElementsByClassName("TextBox");

            var beforeUpdate_TenTK = textboxes[0].Text;

            textboxes[0].SendKeys(Keys.LeftControl + "a");
            textboxes[0].SendKeys((string)tenTK);
            Chon_GioiTinh((int)gioitinhIndex);
            textboxes[3].SendKeys(Keys.LeftControl + "a");
            textboxes[3].SendKeys((string)sdt);
            textboxes[5].SendKeys(Keys.LeftControl + "a");
            textboxes[5].SendKeys((string)diachi);

            Close_Window_CapNhatThongTin();

            Assert.AreEqual(session.FindElementByName((string)tenTK).Text, beforeUpdate_TenTK);
        }

        private static void Chon_GioiTinh(int gioitinhIndex)
        {
            var gioitinhCombobox = window_CapNhatThongTin.FindElementByClassName("ComboBox");
            gioitinhCombobox.Click();
            SwitchToPopupWindow();
            Thread.Sleep(100);
            var selectedGioiTinh = session.FindElementsByClassName("ListBoxItem")[gioitinhIndex];
            selectedGioiTinh.Click();
            SwitchToPopupWindow();
        }

        private void Open_Window_CapNhatThongTin()
        {
            session.FindElementByName("Cập nhật thông tin").Click();
            SwitchToPopupWindow();
            window_CapNhatThongTin = session.FindElementByXPath("/Window[@AutomationId=\"_Sua_TK\"]");
        }

        private void Close_Window_CapNhatThongTin()
        {
            window_CapNhatThongTin.FindElementByName("Hủy").Click();
            SwitchToPopupWindow();
        }

        private static void SetUpElements()
        {
            menuBar.FindElementByName("Tài khoản").Click();
            panel_TaiKhoan = session.FindElementByXPath("/Window[@Name=\"MainWindow\"][@AutomationId=\"_MainWindow\"]/Pane[@ClassName=\"Frame\"]") as WindowsElement;
            var buttons = panel_TaiKhoan.FindElementByClassName("MyMenuBar").FindElementsByClassName("Button");
            button_ThongTin = buttons[0] as WindowsElement;
            button_QuanLiTaiKhoan = buttons[1] as WindowsElement;
        }

        [TestInitialize]
        public void ClassInitialize()
        {
            Setup(null);
            Thread.Sleep(100);
            loginHeader = session.FindElementByXPath("/Window[@AutomationId=\"_Login\"]");
            Thread.Sleep(100);
            AutoLogin();
            NavigateMainWindow();
            Thread.Sleep(100);
            SetUpElements();
        }

        [TestCleanup]
        public void Clean()
        {
            TearDown();
        }
    }
}
