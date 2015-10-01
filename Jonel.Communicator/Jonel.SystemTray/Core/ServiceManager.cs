using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Jonel.Logger;

namespace Jonel.SystemTray.Core
{
    public class ServiceManager
    {
        private FileLogger _log = null;
        private ServiceController _serviceController = null;

        public ServiceManager(string serviceName)
        {
            _log = new FileLogger();

            _serviceController = new ServiceController();
            _serviceController.ServiceName = serviceName;
        }

        public ServiceControllerStatus GetServiceStatus()
        {
            return _serviceController.Status;
        }

        public bool StartService()
        {
            bool success = true;
            try
            {
                _serviceController.Start();

                _log.Info(Constants.JONEL_COMMUNICATOR_HANDLE, "Service started");
            }
            catch (Exception ex)
            {
                success = false;
                _log.Log(Constants.JONEL_COMMUNICATOR_HANDLE, ex);
            }
            return success;
        }

        public bool StopService()
        {
            bool success = true;
            try
            {
                _serviceController.Stop();

                _log.Info(Constants.JONEL_COMMUNICATOR_HANDLE, "Service stopped");
            }
            catch (Exception ex)
            {
                success = false;
                _log.Log(Constants.JONEL_COMMUNICATOR_HANDLE, ex);
            }
            return success;
        }
    }
}
