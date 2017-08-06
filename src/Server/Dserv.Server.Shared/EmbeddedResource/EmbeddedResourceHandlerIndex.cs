using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Dserv.Server
{
    class EmbeddedResourceHandlerIndex : IHandler, IModule
    {
        readonly EmbeddedResourceHandler _resourceHandler;

        public EmbeddedResourceHandlerIndex(EmbeddedResourceHandler resourceHandler)
        {
            _resourceHandler = resourceHandler;
        }

        public string Title => "Embedded Resources";

        public string Path => "/EmbeddedResources";

        public bool CanHandle(HttpListenerRequest request)
        {
            return request.RawUrl == Path;
        }

        public Task HandleAsync(HttpListenerContext context)
        {
            var sb = new StringBuilder();

            sb.AppendLine("<html><head><title>Embedded Resources</title></head><body>");
            sb.AppendLine("<h1>Embedded Resources</h1><ul>");

            foreach (var item in _resourceHandler.Resources)
            {
                var path = item.Value.UrlPath.Escape();
                sb.AppendLine($"<li><a href='{path}'>{path}</a></li>");
            }

            sb.AppendLine("</ul></body></html>");

            return context.Response.WriteHtmlAsync(sb.ToString());
        }
    }
}
