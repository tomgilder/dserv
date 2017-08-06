using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace Dserv.Server
{
    class EmbeddedResourceHandler : IHandler
    {
        public string Title => null;

        public bool CanHandle(HttpListenerRequest request)
		{
            var path = request.RawUrl.TrimStart('/');
            return _resources.ContainsKey(path);
		}

        Dictionary<string, Resource> _resources;

        public IReadOnlyDictionary<string, Resource> Resources => _resources;

        public EmbeddedResourceHandler(IEnumerable<Assembly> assemblies)
        {
            _resources =
                (
                    from assembly in assemblies.Concat(new[] { Assembly.GetExecutingAssembly() })
                    from resourceName in assembly.GetManifestResourceNames()
                    select new Resource(assembly, resourceName)
                )
                .ToDictionary(x => x.UrlPath);
        }

        public async Task HandleAsync(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;
            var path = request.RawUrl.TrimStart('/');
            
            
            var resource = _resources[path];
            await response.WriteStreamAsync(resource.Stream, resource.ResourceName);
            response.OutputStream.Close();
        }

        
    }
}
