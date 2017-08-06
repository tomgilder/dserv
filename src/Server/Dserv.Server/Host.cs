using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Dserv.Server
{
    public class Host
    {
        readonly IEnumerable<IHandler> _handlers;
        int _port;

        public Host(IEnumerable<IHandler> handlers, int port)
        {
            var resourceHandler = new EmbeddedResourceHandler(handlers.Select(x => x.GetType().Assembly));
            var resourceIndex = new EmbeddedResourceHandlerIndex(resourceHandler);

            var defaultHandlers = new IHandler[]
            {
                resourceHandler,
                resourceIndex,
                new HomeHandler(handlers.OfType<IModule>().Concat(new[] { resourceIndex })),
                new DefaultFallbackHandler()
            };

            _handlers = handlers.Concat(defaultHandlers);
            _port = port;
        }

        public void Run()
        {
            if (!HttpListener.IsSupported)
            {
                throw new NotSupportedException("HttpListener is not supported on this platform");
            }

            var listener = new HttpListener();

            try
            {
                listener.Prefixes.Add($"http://localhost:{_port}/");
                listener.Start();
                Log($"Listener started");
            }
            catch (Exception ex)
            {
                Log($"Failed to start HttpListener:\n{ex}");
                return;
            }

            LoopAsync(listener, new CancellationToken()).ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    Log($"Deserve listener failed: {t.Exception}");
                }
                else
                {
                    Log("Deserve listener exited");
                }
            });
        }

        async Task LoopAsync(HttpListener listener, CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                Log($"Waiting for request...");

				var context = await listener.GetContextAsync();

                Log($"Processing request to {context.Request.RawUrl}");

                foreach (var handler in _handlers)
                {
                    if (handler.CanHandle(context.Request))
                    {
                        HandleRequest(handler, context);
                        break;
                    }
                }
            }
        }

        void HandleRequest(IHandler handler, HttpListenerContext context)
        {
            var request = $"{context.Request.HttpMethod} {context.Request.RawUrl}";
            Log($"'{handler.GetType().Name}' handing {request}...");

			handler.HandleAsync(context)
				   .ContinueWith(t =>
				   {
                       if (t.IsFaulted)
                       {
                           Log($"Handler '{handler.GetType().Name}' failed while processing {request}:\n\n{t.Exception}\n");
                       }
                       else
                       {
                           Log($"{request} completed by {handler.GetType().Name}\n");
                       }
				   });
        }

        void Log(string log)
        {
            Console.WriteLine(log);
        }
    }
}
