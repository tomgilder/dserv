using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Dserv.Server
{
    class HomeHandler : IHandler
    {
        readonly IEnumerable<IModule> _modules;

        public HomeHandler(IEnumerable<IModule> modules)
        {
            _modules = modules;
        }

        public string Title => null;

        public bool CanHandle(HttpListenerRequest request)
        {
            return request.RawUrl == "/";
        }

        public async Task HandleAsync(HttpListenerContext context)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<html><head></head><body>");
            sb.AppendLine("<h1>Deserve</h1>");
            sb.AppendLine("<ul>");

            foreach (var module in _modules)
            {
                sb.AppendLine($"<li><a href='{module.Path}'>{module.Title}</a></li>");
            }

            sb.AppendLine("</ul>");
            sb.AppendLine("</body></html>");

            await context.Response.WriteHtmlAsync(sb.ToString());
        }
    }
}