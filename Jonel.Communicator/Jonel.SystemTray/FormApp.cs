using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Jonel.Logger;
using Jonel.SystemTray.Core;

namespace Jonel.SystemTray
{
    public partial class FormApp : Form
    {
        private FileLogger _logger = null;
        private ServiceManager _serviceManager = null;
        public ContextMenuStrip trayContextMenu = null;

        private ServiceControllerStatus serviceStatus = ServiceControllerStatus.Stopped;

        private ToolStripMenuItem _stopServiceItem = null;
        private ToolStripMenuItem _startServiceItem = null;

        public FormApp()
        {
            InitializeComponent();

            //Initialize the service
            _serviceManager = new ServiceManager("Jonel Communicator");

            //Initialize logger
            _logger = new FileLogger();

            InitializeWindowPropertisAndEvents();

            InitializeTrayIconAndProperties();
        }

        private void InitializeWindowPropertisAndEvents()
        {
            //Set Properties
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;

            this.FormClosing += FormApp_FormClosing;
        }

        void FormApp_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        public void InitializeTrayIconAndProperties()
        {
            serviceStatus = _serviceManager.GetServiceStatus();

            //Set the Tray icon
            if (serviceStatus == ServiceControllerStatus.Running)
                notifyTrayIcon.Icon = Properties.Resources.TrayIconRunning;
            else if (serviceStatus == ServiceControllerStatus.Stopped)
                notifyTrayIcon.Icon = Properties.Resources.TrayIconStopped;
            else
                notifyTrayIcon.Icon = Properties.Resources.TrayIconActive;

            //Setup context menu options
            trayContextMenu = new ContextMenuStrip();
            trayContextMenu.Items.Add(new ToolStripMenuItem() { Name = ActionConstants.REFRESH, Text = "Refresh Status" });
            trayContextMenu.Items.Add("-");

            _startServiceItem = new ToolStripMenuItem() { Enabled = ServiceControllerStatus.Stopped.Equals(serviceStatus), Name = ActionConstants.START_SERVICE, Text = "Start Service" };
            trayContextMenu.Items.Add(_startServiceItem);

            _stopServiceItem = new ToolStripMenuItem() { Enabled = ServiceControllerStatus.Running.Equals(serviceStatus), Name = ActionConstants.STOP_SERVICE, Text = "Stop Service" };
            trayContextMenu.Items.Add(_stopServiceItem);

            trayContextMenu.Items.Add("-");
            trayContextMenu.Items.Add(new ToolStripMenuItem() { Name = ActionConstants.SHOW_LOGS, Text = "Show Logs" });
            trayContextMenu.Items.Add("-");

            trayContextMenu.Items.Add(new ToolStripMenuItem() { Name = "actionExit", Text = "Exit" });

            trayContextMenu.ItemClicked += trayContextMenu_ItemClicked;

            //Initialize the tray icon here
            this.notifyTrayIcon.ContextMenuStrip = trayContextMenu;
        }

        void trayContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Name)
            {
                case ActionConstants.EXIT:
                    //hide the context menu strip
                    (sender as ContextMenuStrip).Hide();

                    if (MessageBox.Show("Are you sure you want to exit the application?", "Jonel Communicator Control Centre", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Application.Exit();
                    }
                    break;
                case ActionConstants.START_SERVICE:
                    //Disable the clicked item
                    e.ClickedItem.Enabled = false;
                    if (_serviceManager.StartService())
                    {
                        notifyTrayIcon.Icon = Properties.Resources.TrayIconRunning;
                        _stopServiceItem.Enabled = true;
                    }
                    break;
                case ActionConstants.STOP_SERVICE:
                    //Disable the clicked item
                    e.ClickedItem.Enabled = false;
                    if (_serviceManager.StopService())
                    {
                        notifyTrayIcon.Icon = Properties.Resources.TrayIconStopped;
                        _startServiceItem.Enabled = true;
                    }
                    break;
                case ActionConstants.SHOW_LOGS:
                    System.Diagnostics.Process.Start(_logger.GetLogFilePath());
                    break;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.Hide();
        }
    }
}
