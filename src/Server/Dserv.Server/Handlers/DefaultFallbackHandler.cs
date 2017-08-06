using System;
using System.Net;
using System.Security;
using System.Threading.Tasks;

namespace Dserv.Server
{
    /// <summary>
    /// Default handler, used if no other handlers accept the request
    /// </summary>
    class DefaultFallbackHandler : IHandler
    {
        public string Title => null;

        public bool CanHandle(HttpListenerRequest request)
        {
            return true;
        }

        public Task HandleAsync(HttpListenerContext context)
        {
            context.Response.StatusCode = 404;
            context.Response.Close();

            return Task.CompletedTask;
        }
    }
}