using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

namespace Messenger.Core.Utils
{
    public class NotifyIconControl
    {
        private readonly NotifyIcon _notifyIcon = new NotifyIcon();

        readonly ToolStripMenuItem _openWindowItem = new ToolStripMenuItem("Открыть Messenger");
        readonly ToolStripMenuItem _disableNotificationsItem = new ToolStripMenuItem("Выключить уведомления");
        readonly ToolStripMenuItem _closeWindowItem = new ToolStripMenuItem("Выход");
        
        public void InitializeNotifyIcon()
        {
            string workingDirectory = Environment.CurrentDirectory;
            string path = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
            
            _notifyIcon.Icon = new Icon(path + "\\Assets\\Icons\\icon.ico");
            _notifyIcon.Text = @"Messenger";
            _notifyIcon.Visible = true;
            _notifyIcon.ContextMenuStrip = new ContextMenuStrip();
            _notifyIcon.ContextMenuStrip.Items.AddRange(new ToolStripItem[]
                { _openWindowItem, _disableNotificationsItem, _closeWindowItem });

            if ((MainWindow)Application.Current.MainWindow != null)
            {
                _notifyIcon.MouseDown += ((MainWindow)Application.Current.MainWindow).OpenWindow;
                _openWindowItem.MouseUp += ((MainWindow)Application.Current.MainWindow).OpenWindow;
                _disableNotificationsItem.MouseUp += ((MainWindow)Application.Current.MainWindow).DisableNotifications;
                _closeWindowItem.MouseUp += ((MainWindow)Application.Current.MainWindow).CloseApplication;
            }
        }

        public void ShowNotificationWhenWindowClosed()
        {
            _notifyIcon.ShowBalloonTip(5000, "Уведомление", "Вы свернули Messenger.", ToolTipIcon.Info);
        }
        public void DetachIcon()
        {
            _notifyIcon.Dispose();
        }
    }
}