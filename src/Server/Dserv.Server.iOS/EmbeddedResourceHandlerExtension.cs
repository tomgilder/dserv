using System.Reflection;

namespace Dserv.Server
{
    public static class EmbeddedResourceHandlerExtension
    {
        public static DservBuilder UseEmbeddedResources(this DservBuilder builder, params Assembly[] assemblies)
        {
            builder.AddHandler(new EmbeddedResourceHandler(assemblies));
            return builder;
        }
    }
}