using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Interactions;
using System;
using System.Linq;
using System.Threading;

namespace BM_Test
{
    [TestClass]
    public class Test_NhapKho : BaseTestApp
    {
        private static IWebElement tab_KhoSach;
        private static IWebElement tab_NhapKho;
        private static IWebElement button_Them;
        private static IWebElement panel_KhoSach;
        private static WindowsElement window_Them;

        [TestMethod]
        public void TTNK025_Nhan_ButtonThem()
        {
            button_Them.Click();
            Thread.Sleep(100);
            try
            {
                SwitchToPopupWindow();
                window_Them = session.FindElementByXPath("/Window[@ClassName=\"Window\"][@AutomationId=\"_ThongTinChiTiet_XK\"]");
                CloseWindow_NhapKho();
            }
            catch
            {
                Assert.Fail("Cửa sổ thêm không hiện");
            }
            Assert.IsTrue(true);
        }


        [TestMethod]
        [DataRow("Nhà cung cấp A", "Ghi chú", 0, 2)]
        public void TTNK048_NhapKho(object nccName, object ghichu, object sachIndex, object quantity)
        {
            button_Them.Click();
            Thread.Sleep(100);
            SwitchToPopupWindow();
            window_Them = session.FindElementByXPath("/Window[@ClassName=\"Window\"][@AutomationId=\"_ThongTinChiTiet_XK\"]");
            var textboxes = window_Them.FindElementsByClassName("TextBox");
            textboxes[0].SendKeys(nccName.ToString());
            textboxes[1].SendKeys(ghichu.ToString());

            OpenWindow_ThemSach();

            Combobox_ChonSach((int)sachIndex);

            NhapSoLuongSach((int)quantity);

            session.FindElementByName("Thêm").Click();

            SwitchToPopupWindow();

            session.FindElementByName("Đồng ý").Click();
            SwitchToPopupWindow();
        }

        [TestMethod]
        [DataRow("Nhà cung cấp A", "Ghi chú", 0, 2)]
        public void TTXK049_NhapKho_KhongChonSach(object nccName, object ghichu, object sachIndex, object quantity)
        {
            button_Them.Click();
            Thread.Sleep(100);
            SwitchToPopupWindow();

            window_Them = session.FindElementByXPath("/Window[@ClassName=\"Window\"][@AutomationId=\"_ThongTinChiTiet_XK\"]");
            var textboxes = window_Them.FindElementsByClassName("TextBox");
            textboxes[0].SendKeys(nccName.ToString());
            textboxes[1].SendKeys(ghichu.ToString());

            OpenWindow_ThemSach();
            Thread.Sleep(100);

            NhapSoLuongSach((int)quantity);

            bool button_Them_Disabled = !session.FindElementByName("Thêm").Enabled;

            Assert.IsTrue(button_Them_Disabled);

            CloseWindow_ThemSach();
            CloseWindow_NhapKho();
        }

        [TestMethod]
        [DataRow("Nhà cung cấp A", "Ghi chú", 0, 2)]
        public void TTNK050_NhapKho_NhapLai(object nccName, object ghichu, object sachIndex, object quantity)
        {
            button_Them.Click();
            SwitchToPopupWindow();

            window_Them = session.FindElementByXPath("/Window[@ClassName=\"Window\"][@AutomationId=\"_ThongTinChiTiet_XK\"]");

            OpenWindow_ThemSach();

            NhapSoLuongSach((int)quantity);

            session.FindElementByName("Nhập lại").Click();

            bool isDuLieuDaXoa =
                String.Equals(session.FindElementsByTagName("Edit")[1].Text, "0") &&
                !session.FindElementByName("Thêm").Enabled;

            Assert.IsTrue(isDuLieuDaXoa);
            
            CloseWindow_ThemSach();
            CloseWindow_NhapKho();
        }

        [TestMethod]
        [DataRow(0)]
        public void TTNK051_NhapKho_Xoa(object index)
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

            Assert.IsTrue(true);
        }

        [TestMethod]
        [DataRow(1, 1)]
        public void TTNK053_NhapKho_Huy(object sachIndex, object quantity)
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
            Thread.Sleep(100);

            NhapSoLuongSach((int)quantity);
            Thread.Sleep(100);

            session.FindElementByName("Thêm").Click();
            Thread.Sleep(100);

            CloseWindow_ThemSach();
            CloseWindow_NhapKho();

            Thread.Sleep(100);

            rows = table.FindElementsByClassName("DataGridRow");

            Assert.AreEqual(preCount, rows.Count - 1);
        }

        [TestCleanup]
        public void ClassCleanup()
        {
            TearDown();
        }

        [TestInitialize]
        public void ClassInitialize()
        {
            Setup(null);
            Thread.Sleep(500);
            loginHeader = session.FindElementByXPath("/Window[@AutomationId=\"_Login\"]");
            Thread.Sleep(100);
            AutoLogin();
            NavigateMainWindow();
            Thread.Sleep(100);
            SetUpElements();
        }

        private static void OpenWindow_ThemSach()
        {
            session.FindElementByXPath("/Window[@AutomationId=\"_ThongTinChiTiet_XK\"]/Custom[@AutomationId=\"_uc\"]/Button[@AutomationId=\"PART_Toggle\"]").Click();
            SwitchToPopupWindow();
        }

        private static void CloseWindow_ThemSach()
        {
            var action = new Actions(session);
            action.MoveByOffset(300, 0).Click().Perform();
        }

        private static void CloseWindow_NhapKho()
        {
            window_Them.FindElementByName("Hủy").Click();
            SwitchToPopupWindow();
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

        private static void SetUpElements()
        {
            tab_KhoSach = menuBar.FindElementByName("Kho sách");
            tab_KhoSach.Click();
            panel_KhoSach = mainAppHeader.FindElementsByTagName("Pane")[0];
            tab_NhapKho = panel_KhoSach.FindElement(By.Name("Nhập kho"));
            tab_NhapKho.Click();
            button_Them = panel_KhoSach.FindElements(By.ClassName("Button"))[3];
        }
    }
}
