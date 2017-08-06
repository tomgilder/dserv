using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Dserv.iOS.Server;

namespace Dserv.Server
{
    static class HttpListenerResponseExtensions
    {
        public static async Task WriteHtmlAsync(this HttpListenerResponse response, string str, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            response.AddHeader("Content-Type", "text/html");

            var buffer = encoding.GetBytes(str);
            response.ContentLength64 = buffer.Length;
            await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            response.OutputStream.Close();
        }

        public static Task WriteStreamAsync(this HttpListenerResponse response, Stream stream, string fileName)
		{
            response.AddHeader("Content-Type", MimeTypeMap.GetMimeType(Path.GetExtension(fileName)));
            return stream.CopyToAsync(response.OutputStream);
		}
    }
}
