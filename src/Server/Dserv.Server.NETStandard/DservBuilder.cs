using System.Collections.Generic;

namespace Dserv.Server
{   
    public class DservBuilder
    {
        List<IHandler> _handlers = new List<IHandler>();

        internal int Port { get; set; } = 8000;

        public Host Build()
        {
            return new Host(_handlers, Port);
        }

        public void AddHandler(IHandler handler)
        {
            _handlers.Add(handler);
        }
    }
}