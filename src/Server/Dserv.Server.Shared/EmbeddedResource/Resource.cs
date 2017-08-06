using System.IO;
using System.Reflection;

namespace Dserv.Server
{
    class Resource
    {
        public Assembly Assembly { get; }
        public string ResourceName { get; }
        public string UrlPath { get; }
        public Stream Stream => Assembly.GetManifestResourceStream(ResourceName);

        public Resource(Assembly assembly, string resourceName)
        {
            Assembly = assembly;
            ResourceName = resourceName;
            UrlPath = GetPath(assembly, resourceName);
        }

        static string GetPath(Assembly assembly, string resourceName)
        {
            //var prefix = $"{assembly.GetName().Name}.";

            //if (resourceName.StartsWith(prefix, System.StringComparison.Ordinal))
            //{
            //    resourceName = resourceName.Substring(prefix.Length);
            //}

            var lastDotIndex = resourceName.LastIndexOf('.');

            if (lastDotIndex == -1)
            {
                return resourceName;
            }

            return resourceName.Substring(0, lastDotIndex).Replace('.', '/') + resourceName.Substring(lastDotIndex);
        }
    }
}
