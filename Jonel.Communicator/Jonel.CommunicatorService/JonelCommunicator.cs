using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Jonel.Domain;
using Jonel.Logger;

namespace Jonel.CommunicatorService
{
    public partial class JonelCommunicator : ServiceBase
    {
        private FileLogger _logger = null;

        public const string EVENT_SOURCE = "Jonel Communicator";
        public const string EVENT_LOG = "Jonel Communicator Log";
        public const string SERVICE_NAME_HANDLE = "[Communicator Service]";

        public EventLog eventLog;

        public Timer startTimer;

        public JonelCommunicator()
        {
            InitializeComponent();

            _logger = new FileLogger();

            startTimer = new Timer();
            startTimer.Elapsed += startTimer_Elapsed;
            startTimer.Interval = 10000;

            //if (!System.Diagnostics.EventLog.SourceExists(EVENT_SOURCE))
            //    System.Diagnostics.EventLog.CreateEventSource(EVENT_SOURCE, EVENT_LOG);

            //eventLog.Source = EVENT_SOURCE;
            //eventLog.Log = EVENT_LOG;
        }

        void startTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                //Disable the Timer
                startTimer.Enabled = false;

                Integrator.Shared.StartListening();

                _logger.Info(SERVICE_NAME_HANDLE, "Integrator started");
                //eventLog.WriteEntry(string.Format("{0} : {1}", SERVICE_NAME_HANDLE, "Service Started"));
            }
            catch (Exception ex)
            {
                _logger.Log(SERVICE_NAME_HANDLE, ex);
                //eventLog.WriteEntry(ex.Message, EventLogEntryType.Error);
            }
        }

        protected override void OnStart(string[] args)
        {
            startTimer.Enabled = true;
            startTimer.Start();
            //try
            //{
            //    eventLog.WriteEntry("Starting Jonel Communicator");
            //    //Integrator.Shared.StartListening();

            //    eventLog.WriteEntry("[Jonel Communicator] Integrator Started");
            //}
            //catch (Exception ex)
            //{
            //    eventLog.WriteEntry(ex.Message, EventLogEntryType.Error);
            //}
        }

        protected override void OnStop()
        {
            try
            {
                //Disable the Timer
                startTimer.Enabled = false;

                Integrator.Shared.StopListening();

                _logger.Info(SERVICE_NAME_HANDLE, "Integrator stopped");
                //eventLog.WriteEntry(string.Format("{0} : {1}", SERVICE_NAME_HANDLE, "Service Started"));
            }
            catch (Exception ex)
            {
                _logger.Log(SERVICE_NAME_HANDLE, ex);
                //eventLog.WriteEntry(ex.Message, EventLogEntryType.Error);
            }
        }
    }
}
