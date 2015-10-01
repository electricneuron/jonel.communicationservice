using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Threading;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.Host.HttpListener;
using Newtonsoft.Json;
using Jonel.Logger;

namespace Jonel.Domain
{
    public class Integrator : IIntegrator
    {
        protected static Integrator sharedIntegrator = null;

        private IDisposable signalR;
        protected string serverURI;
        protected ServerStatus serverStatus = ServerStatus.Stopped;
        public event EventHandler OnStateChanged;

        public FileLogger _logger = null;

        public ServerStatus Status
        {
            get
            {
                return serverStatus;
            }
            protected set
            {
                serverStatus = value;
                NotifyOnStateChanged(new IntegratorStateEventArgs() { Status = serverStatus });
            }
        }

        public static Integrator Shared
        {
            get
            {
                if (sharedIntegrator == null)
                {
                    sharedIntegrator = new Integrator();
                }

                return sharedIntegrator;
            }
        }

        protected Integrator()
        {
            //Logger.Initialize(Assembly.GetExecutingAssembly(), "Initializing Integrator.");
            _logger = new FileLogger();
            serverURI = String.Format("http://localhost:{0}", Settings.Shared.Port);
        }

        public void StartListening()
        {
            _logger.Info(">> Starting SignalR server...");
            try
            {
                if (signalR != null)
                {
                    StopListening();
                }
                Task.Run(() => StartServer());
                Status = ServerStatus.Running;
            }
            catch (Exception ex)
            {
                _logger.Error("Error starting SignalR server: ", ex);
                Status = ServerStatus.Stopped;
                return;
            }
            _logger.Info("SignalR server started.");
        }

        public void StopListening()
        {
            _logger.Info("<< Stopping SignalR server...");
            if (signalR != null)
            {
                signalR.Dispose();
            }
            Status = ServerStatus.Stopped;
        }

        private void StartServer()
        {
            try
            {
                _logger.Info(string.Format("Starting SignalR at: {0}", serverURI));
                signalR = WebApp.Start(serverURI);
                Status = ServerStatus.Running;
            }
            catch (Exception ex)
            {
                _logger.Error("StartServer() error: ", ex);
                Status = ServerStatus.Stopped;
            }
        }

        #region Events

        protected void NotifyOnStateChanged(IntegratorStateEventArgs e)
        {
            if (OnStateChanged != null)
            {
                try
                {
                    //Dispatcher.CurrentDispatcher.BeginInvoke(OnStateChanged, this, e);
                }
                catch (Exception ex)
                {
                    _logger.Error("Error notifying subscribers of State Changed. ", ex);
                }
            }
        }

        #endregion
    }

    public enum ServerStatus
    {
        Stopped,
        Running
    }

    public class IntegratorStateEventArgs : EventArgs
    {
        public ServerStatus Status { get; set; }
    }

    public interface IIntegrator
    {
        ServerStatus Status { get; }
        void StartListening();
        void StopListening();
        event EventHandler OnStateChanged;
    }
}
