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
    public class Test_XuatKho : BaseTestApp
    {
        private static WindowsElement tab_KhoSach;
        private static WindowsElement tab_XuatKho;
        private static WindowsElement button_Them;
        private static WindowsElement panel_KhoSach;
        private static WindowsElement window_Them;

        [TestMethod]
        [DataRow("Ghi chú", 0, 2)]
        public void TTXK_047_XuatKho(object ghichu, object sachIndex, object quantity)
        {
            button_Them.Click();
            Thread.Sleep(100);
            SwitchToPopupWindow();
            window_Them = session.FindElementByXPath("/Window[@ClassName=\"Window\"][@AutomationId=\"_ThongTinChiTiet_XK\"]");
            var textboxes = window_Them.FindElementsByClassName("TextBox");
            textboxes[0].SendKeys(ghichu.ToString());

            OpenWindow_ThemSach();

            Combobox_ChonSach((int)sachIndex);
            Thread.Sleep(100);

            NhapSoLuongSach((int)quantity);
            Thread.Sleep(100);

            session.FindElementByName("Thêm").Click();
            Thread.Sleep(100);

            SwitchToPopupWindow();
            session.FindElementByName("Đồng ý").Click();

            SwitchToPopupWindow();
        }

        [TestMethod]
        [DataRow("Ghi chú", 0, 2)]
        public void TTXK048_XuatKho_KhongChonSach(object ghichu, object sachIndex, object quantity)
        {
            button_Them.Click();
            Thread.Sleep(100);
            SwitchToPopupWindow();

            window_Them = session.FindElementByXPath("/Window[@ClassName=\"Window\"][@AutomationId=\"_ThongTinChiTiet_XK\"]");
            var textboxes = window_Them.FindElementsByClassName("TextBox");
            textboxes[0].SendKeys(ghichu.ToString());

            OpenWindow_ThemSach();
            Thread.Sleep(100);

            NhapSoLuongSach((int)quantity);

            bool button_Them_Disabled = !session.FindElementByName("Thêm").Enabled;

            Assert.IsTrue(button_Them_Disabled);

            CloseWindow_ThemSach();
            CloseWindow_XuatKho();
        }

        [TestMethod]
        [DataRow(0, 2)]
        public void TTXK0049_XuatKho_NhapLai(object sachIndex, object quantity)
        {
            button_Them.Click();
            Thread.Sleep(100);
            SwitchToPopupWindow();

            window_Them = session.FindElementByXPath("/Window[@ClassName=\"Window\"][@AutomationId=\"_ThongTinChiTiet_XK\"]");
            
            OpenWindow_ThemSach();

            Combobox_ChonSach((int)sachIndex);

            NhapSoLuongSach((int)quantity);

            session.FindElementByName("Nhập lại").Click();

            bool isDuLieuDaXoa =
                String.Equals(session.FindElementsByTagName("Edit")[1].Text, "0") &&
                !session.FindElementByName("Thêm").Enabled;

            Assert.IsTrue(isDuLieuDaXoa);

            CloseWindow_ThemSach();
            CloseWindow_XuatKho();
        }

        [TestMethod]
        [DataRow(0)]
        public void TTXK050_NhapKho_Xoa(object index)
        {
            var table = session.FindElementByClassName("DataGrid");
            var rows = table.FindElementsByClassName("DataGridRow");

            int beforeDeleteCount = rows.Count;

            var action = new Actions(session);
            var selectedRow = rows[(int)index];
            action.MoveToElement(selectedRow).Perform();
            action.MoveByOffset(-300, 0).Perform();
            action.ContextClick().Perform();

            SwitchToPopupWindow();
            Thread.Sleep(100);

            var contextMenu = session.FindElementByClassName("ContextMenu");
            contextMenu.FindElementByClassName("MenuItem").Click();

            rows = table.FindElementsByClassName("DataGridRow");

            var deleteSuccess = beforeDeleteCount - rows.Count == 1;

            Assert.IsTrue(deleteSuccess);
        }

        [TestMethod]
        [DataRow(1, 1)]
        public void TTXK052_NhapKho_Huy(object sachIndex, object quantity)
        {
            var table = session.FindElementByClassName("DataGrid");
            var rows = table.FindElementsByClassName("DataGridRow");
            int preCount = rows.Count;
            button_Them.Click();
            Thread.Sleep(100);
            SwitchToPopupWindow();
            window_Them = session.FindElementByXPath("/Window[@ClassName=\"Window\"][@AutomationId=\"_ThongTinChiTiet_XK\"]");

            OpenWindow_ThemSach();

            Combobox_ChonSach((int)sachIndex);

            NhapSoLuongSach((int)quantity);

            session.FindElementByName("Thêm").Click();

            CloseWindow_ThemSach();
            session.FindElementByName("Hủy").Click();
            SwitchToPopupWindow();
            Assert.IsTrue(true);
        }

        private static void CloseWindow_ThemSach()
        {
            var action = new Actions(session);
            action.MoveByOffset(300, 0).Click().Perform();
            SwitchToPopupWindow();
        }

        private static void CloseWindow_XuatKho()
        {
            window_Them.FindElementByName("Hủy").Click();
            SwitchToPopupWindow();
        }

        private static void OpenWindow_ThemSach()
        {
            session.FindElementByXPath("/Window[@AutomationId=\"_ThongTinChiTiet_XK\"]/Custom[@AutomationId=\"_uc\"]/Button[@AutomationId=\"PART_Toggle\"]").Click();
            SwitchToPopupWindow();
        }


        private static void SetUpElements()
        {
            tab_KhoSach = menuBar.FindElementByName("Kho sách") as WindowsElement;
            tab_KhoSach.Click();
            panel_KhoSach = mainAppHeader.FindElementByTagName("Pane") as WindowsElement;
            tab_XuatKho = panel_KhoSach.FindElementByName("Xuất kho") as WindowsElement;
            tab_XuatKho.Click();
            button_Them = panel_KhoSach.FindElementByXPath("/Pane[@ClassName=\"Frame\"]/Pane[@ClassName=\"Frame\"]/Button[@ClassName=\"Button\"]") as WindowsElement;
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
        public void CleanUp()
        {
            TearDown();
        }

        private static void Combobox_ChonSach(int index)
        {
            var combobox = session.FindElementByClassName("ComboBox");
            combobox.Click();
            Thread.Sleep(100);
            var selectedSach = session.FindElementsByClassName("ListBoxItem")[index];
            Thread.Sleep(100);
            selectedSach.Click();
        }

        private static void NhapSoLuongSach(int quantity)
        {
            var textbox_Quantity = session.FindElementsByTagName("Edit")[1];
            textbox_Quantity.Click();
            textbox_Quantity.SendKeys(Keys.Backspace);
            textbox_Quantity.SendKeys(quantity.ToString());
        }

    }
}
