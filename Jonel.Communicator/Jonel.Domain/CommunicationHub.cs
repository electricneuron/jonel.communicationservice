using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Jonel.Logger;

namespace Jonel.Domain
{
    public class CommunicationHub : Hub
    {
        private FileLogger _log = null;

        public CommunicationHub ()
	{
            _log=new FileLogger();
	}

        public void Send(string message, string data)
        {
            _log.Info(String.Format("Received from: {0}  Message: {1}  Data: {2}", Context.ConnectionId, message, data));
            Clients.All.addMessage(message, data);
        }

        public override Task OnConnected()
        {
            _log.Info(String.Format(">> Client connected: {0}", Context.ConnectionId));
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            _log.Info(String.Format("<< Client disconnected: {0}", Context.ConnectionId));
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            _log.Info(String.Format(">> Client reconnected: {0}", Context.ConnectionId));
            return base.OnReconnected();
        }
    }
}
