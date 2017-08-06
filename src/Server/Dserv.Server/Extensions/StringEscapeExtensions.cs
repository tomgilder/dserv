using System.Security;

namespace Dserv.Server
{
    static class StringEscapeExtensions
    {
        public static string Escape(this string str)
        {
            return SecurityElement.Escape(str);
        }
    }
}