using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NerdDinner.Web.Common
{
  public class ErrorLog
  {
    private readonly RequestDelegate _next;

    public ErrorLog(RequestDelegate next)
    {
      _next = next;
    }

    public Task Invoke(HttpContext httpContext)
    {

      if (httpContext.Request.Path.Value.Equals("/api/errorlog", StringComparison.CurrentCultureIgnoreCase))
      {
        return ShowErrorLog(httpContext);
      }

      return _next(httpContext);
    }

    private Task ShowErrorLog(HttpContext context)
    {

      context.Response.Clear();

      context.Response.ContentType = "application/json";
      return context.Response.WriteAsync(JsonConvert.SerializeObject(SimpleWebLoggerProvider.LogMessages));

    }
  }
}
