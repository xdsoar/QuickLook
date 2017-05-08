﻿using System.Diagnostics;
using System.Windows.Forms;
using QuickLook.Helpers;
using QuickLook.Properties;
using Application = System.Windows.Application;

namespace QuickLook
{
    public class TrayIcon
    {
        private static TrayIcon _instance;

        private readonly NotifyIcon _icon;

        private readonly MenuItem _itemAutorun =
            new MenuItem("Run at &Startup", (sender, e) =>
            {
                if (AutoStartupHelper.IsAutorun())
                    AutoStartupHelper.RemoveAutorunShortcut();
                else
                    AutoStartupHelper.CreateAutorunShortcut();
            });

        private TrayIcon()
        {
            _icon = new NotifyIcon
            {
                Icon = Resources.app_white,
                Visible = true,
                ContextMenu = new ContextMenu(new[]
                {
                    new MenuItem("Check for &Updates...",
                        (sender, e) => Process.Start(@"http://pooi.moe/QuickLook/")),
                    _itemAutorun,
                    new MenuItem("&Quit", (sender, e) => Application.Current.Shutdown())
                })
            };

            _icon.ContextMenu.Popup += (sender, e) => { _itemAutorun.Checked = AutoStartupHelper.IsAutorun(); };
        }

        public void ShowNotification(string title, string content, bool isError = false)
        {
            _icon.ShowBalloonTip(5000, title, content, isError ? ToolTipIcon.Error : ToolTipIcon.Info);
        }

        internal static TrayIcon GetInstance()
        {
            return _instance ?? (_instance = new TrayIcon());
        }
    }
}