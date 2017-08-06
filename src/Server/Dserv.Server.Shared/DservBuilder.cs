using System.Collections.Generic;

namespace Dserv.Server
{
    public class DservBuilder
    {
        List<IHandler> _handlers = new List<IHandler>();
        int _port = 8000;

        public Host Build()
        {
            return new Host(_handlers, _port);
        }

        public void AddHandler(IHandler handler)
        {
            _handlers.Add(handler);
        }

        public DservBuilder UsePort(int port)
        {
            _port = port;
            return this;
        }
    }
}