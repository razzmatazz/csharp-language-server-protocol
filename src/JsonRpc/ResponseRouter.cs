using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace OmniSharp.Extensions.JsonRpc
{
    public interface IPendingResponse
    {
        Type ResponseType { get; }
        void SetResult(object value);
        void SetException(Exception e);
        bool IsVoid { get; }
    }

    class PendingResponse : IPendingResponse
    {
        private readonly TaskCompletionSource<object> _completionSource;

        public PendingResponse(Type responseType, TaskCompletionSource<object> completionSource)
        {
            _completionSource = completionSource;
            ResponseType = responseType;
        }

        public Type ResponseType { get; }
        public void SetResult(object value) => _completionSource.SetResult(value);

        public void SetException(Exception e) => _completionSource.SetException(e);
        public bool IsVoid => ResponseType == typeof(Unit);
    }

    public class ResponseRouter : IResponseRouter
    {
        internal readonly IOutputHandler OutputHandler;
        internal readonly ISerializer Serializer;

        internal readonly ConcurrentDictionary<long, IPendingResponse> Requests =
            new ConcurrentDictionary<long, IPendingResponse>();

        internal static readonly ConcurrentDictionary<Type, string> MethodCache =
            new ConcurrentDictionary<Type, string>();

        public ResponseRouter(IOutputHandler outputHandler, ISerializer serializer)
        {
            OutputHandler = outputHandler;
            Serializer = serializer;
        }

        public void SendNotification(string method)
        {
            OutputHandler.Send(new Client.Notification() {
                Method = method
            });
        }

        public void SendNotification<T>(string method, T @params)
        {
            OutputHandler.Send(new Client.Notification() {
                Method = method,
                Params = @params
            });
        }

        public void SendNotification(IRequest @params)
        {
            SendNotification(GetMethodName(@params.GetType()), @params);
        }

        public Task<TResponse> SendRequest<TResponse>(IRequest<TResponse> @params, CancellationToken cancellationToken)
        {
            return SendRequest(GetMethodName(@params.GetType()), @params).Returning<TResponse>(cancellationToken);
        }

        public IResponseRouterReturns SendRequest(string method)
        {
            return new ResponseRouterReturnsImpl(this, method, null);
        }

        public IResponseRouterReturns SendRequest<T>(string method, T @params)
        {
            return new ResponseRouterReturnsImpl(this, method, @params);
        }

        public IPendingResponse GetRequest(long id)
        {
            Requests.TryGetValue(id, out var source);
            return source;
        }

        private string GetMethodName(Type type)
        {
            if (!MethodCache.TryGetValue(type, out var methodName))
            {
                var attribute = type.GetCustomAttribute<MethodAttribute>(true);
                if (attribute == null)
                {
                    throw new NotSupportedException($"Unable to infer method name for type {type.FullName}");
                }

                methodName = attribute.Method;
                MethodCache.TryAdd(type, methodName);
            }

            return methodName;
        }

        class ResponseRouterReturnsImpl : IResponseRouterReturns
        {
            private readonly ResponseRouter _router;
            private readonly string _method;
            private readonly object _params;

            public ResponseRouterReturnsImpl(ResponseRouter router, string method, object @params)
            {
                _router = router;
                _method = method;
                _params = @params;
            }

            public async Task<TResponse> Returning<TResponse>(CancellationToken cancellationToken)
            {
                var nextId = _router.Serializer.GetNextId();
                var tcs = new TaskCompletionSource<object>();
                var pendingResponse = new PendingResponse(typeof(TResponse), tcs);
                _router.Requests.TryAdd(nextId, pendingResponse);

                _router.OutputHandler.Send(new Client.Request() {
                    Method = _method,
                    Params = _params,
                    Id = nextId
                });

                try
                {
                    var result = await tcs.Task;
                    if (typeof(TResponse) == typeof(Unit))
                    {
                        return (TResponse) (object) Unit.Value;
                    }

                    return (TResponse) result;
                }
                finally
                {
                    _router.Requests.TryRemove(nextId, out var _);
                }
            }

            public async Task ReturningVoid(CancellationToken cancellationToken)
            {
                await Returning<Unit>(cancellationToken);
            }
        }
    }
}
