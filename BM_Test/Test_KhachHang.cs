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
    public class Test_KhachHang : BaseTestApp
    {
        private static WindowsElement tab_KhachHang;
        private static WindowsElement table_KhachHang;
        private static WindowsElement button_Them;
        private static WindowsElement panel_KhachHang;
        private static WindowsElement window_Them;

        
        [TestMethod]
        [DataRow("Khách hàng A", 1, "0987654321", "Hồ Chí Minh")]
        public void KH034_KhachHang_Add(object tenKH, object gioitinhIndex, object sdt, object diachi)
        {
            button_Them.Click();
            SwitchToPopupWindow();
            window_Them = session.FindElementByClassName("Popup");

            var textboxes = window_Them.FindElementsByTagName("Edit");

            textboxes[0].SendKeys((string)tenKH);
            Chon_GioiTinh((int)gioitinhIndex);
            Thread.Sleep(100);
            SwitchToPopupWindow();

            textboxes[2].SendKeys((string)sdt);
            textboxes[3].SendKeys((string)diachi);

            window_Them.FindElementByName("Thêm").Click();

            bool isNewKhachHangExists = table_KhachHang.FindElementsByTagName("DataItem").Any(row =>
            {
                var cell_Ten = row.FindElementsByClassName("DataGridCell")[1];
                return cell_Ten.Text.Equals(tenKH);
            });

            Assert.IsTrue(isNewKhachHangExists);
        }

        [TestMethod]
        public void KH035_NhapThieuDuLieu()
        {
            button_Them.Click();
            SwitchToPopupWindow();
            window_Them = session.FindElementByClassName("Popup");

            Thread.Sleep(100);
            SwitchToPopupWindow();

            window_Them.FindElementByName("Thêm").Click();

            bool isNewKhachHangExists = table_KhachHang.FindElementsByTagName("DataItem").Any(row =>
            {
                var cell_Ten = row.FindElementsByClassName("DataGridCell")[1];
                return cell_Ten.Text.Equals("");
            });

            Assert.IsTrue(isNewKhachHangExists);
        }

        [TestMethod]
        [DataRow("Khách hàng A", 1, "0987654321", "Hồ Chí Minh")]
        public void KH036_NhapLaiDuLieu(object tenKH, object gioitinhIndex, object sdt, object diachi)
        {
            button_Them.Click();
            SwitchToPopupWindow();
            window_Them = session.FindElementByClassName("Popup");

            var textboxes = window_Them.FindElementsByTagName("Edit");

            textboxes[0].SendKeys((string)tenKH);
            Chon_GioiTinh((int)gioitinhIndex);
            Thread.Sleep(100);
            SwitchToPopupWindow();

            textboxes[2].SendKeys((string)sdt);
            textboxes[3].SendKeys((string)diachi);

            window_Them.FindElementByName("Nhập lại").Click();

            bool isDuLieuBiXoa =
                String.IsNullOrEmpty(textboxes[0].Text) &&
                String.IsNullOrEmpty(textboxes[2].Text) &&
                String.IsNullOrEmpty(textboxes[3].Text);

            Assert.IsTrue(isDuLieuBiXoa);

            new Actions(session).MoveByOffset(300, 0).Click().Perform();
        }

        [TestMethod]
        [DataRow(0)]
        public void KH038_XoaKhachHang(object rowIndex)
        {
            var rows = table_KhachHang.FindElementsByClassName("DataGridRow");
            var preCount = rows.Count;
            var actions = new Actions(session);
            actions.MoveToElement(rows[(int)rowIndex]).Perform();
            actions.MoveByOffset(-300, 0).Perform();
            actions.Click().Perform();

            session.FindElementByName("Xóa").Click();

            rows = table_KhachHang.FindElementsByClassName("DataGridRow");

            Assert.AreEqual(preCount - 1, rows.Count);
        }

        private static void Chon_GioiTinh(int gioitinhIndex)
        {
            var gioitinhCombobox = window_Them.FindElementByClassName("ComboBox");
            gioitinhCombobox.Click();
            SwitchToPopupWindow();
            Thread.Sleep(100);
            var selectedGioiTinh = session.FindElementsByClassName("ListBoxItem")[gioitinhIndex];
            selectedGioiTinh.Click();
        }

        private static void SetUpElements()
        {
            tab_KhachHang = menuBar.FindElementByName("Khách hàng") as WindowsElement;
            tab_KhachHang.Click();
            panel_KhachHang = mainAppHeader.FindElementByTagName("Pane") as WindowsElement;
            table_KhachHang = panel_KhachHang.FindElementByClassName("DataGrid") as WindowsElement;
            button_Them = panel_KhachHang.FindElementByXPath("/Pane[@ClassName=\"Frame\"]/Custom[@AutomationId=\"_uc\"]/Button[@AutomationId=\"PART_Toggle\"]") as WindowsElement;
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
