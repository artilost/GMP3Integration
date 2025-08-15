using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;

namespace GMP3Integration.API.Filters
{
    public class TransactionHandleScopeFilter : IActionFilter
    {
        private readonly ILogger<TransactionHandleScopeFilter> _logger;
        private const string ScopeKey = "__txn_scope_disposable";
        public TransactionHandleScopeFilter(ILogger<TransactionHandleScopeFilter> logger)
        {
            _logger = logger;
        }

        void IActionFilter.OnActionExecuted(ActionExecutedContext context)
        {
            object scopeObj;
            if (context.HttpContext.Items.TryGetValue(ScopeKey, out scopeObj))
            {
                IDisposable disp = scopeObj as IDisposable;
                if (disp != null) disp.Dispose();
            }
        }

        void IActionFilter.OnActionExecuting(ActionExecutingContext context)
        {
            ulong handle = FindHandle(context.ActionArguments);
            if (handle > 0)
            {
                IDisposable scope = _logger.BeginScope(new Dictionary<string, object>
                {
                    { "transactionHandle", handle }
                });
                context.HttpContext.Items[ScopeKey] = scope;
            }
        }
        private static ulong FindHandle(IDictionary<string, object> args)
        {
            foreach (var kv in args)
            {
                object obj = kv.Value;
                if (obj == null) continue;

                // Tek nesnede TransactionHandle var mı?
                PropertyInfo p = obj.GetType().GetProperty("TransactionHandle");
                if (p != null && p.PropertyType == typeof(ulong))
                {
                    object val = p.GetValue(obj);
                    if (val != null) return (ulong)val;
                }
            }
            return 0UL;
        }
    }
}
