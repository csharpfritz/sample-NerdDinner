using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NerdDinner.Web.Common
{
  public class SimpleWebLoggerProvider : ILoggerProvider
  {

    internal static readonly List<LogMessage> LogMessages = new List<LogMessage>();

    #region ILoggerProvider Implementation

    public SimpleWebLoggerProvider(LogLevel minLogLevel)
    {
      this.LogLevel = minLogLevel;
    }

    public ILogger CreateLogger(string categoryName)
    {
      return new SimpleWebLogger(this.LogLevel, categoryName);
    }

    public void Dispose()
    {
      LogMessages.Clear();
    }

    #endregion

    protected internal LogLevel LogLevel { get; }

  }

  public class SimpleWebLogger : ILogger
  {

    public SimpleWebLogger(LogLevel minLogLevel, string categoryName)
    {
      LogLevel = minLogLevel;
      CategoryName = categoryName;
    }

    protected LogLevel LogLevel { get; }

    public string CategoryName { get; }

    public IDisposable BeginScope<TState>(TState state)
    {
      // This is a simple provider... no scope provided
      return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
      // Check with the existing logger
      return LogLevel != Microsoft.Extensions.Logging.LogLevel.None && LogLevel <= logLevel;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {

      // Check if we are logging at this level
      if (!IsEnabled(logLevel)) return;

      var msg = new LogMessage
      {
        Level = logLevel,
        LevelText = Enum.GetName(typeof(LogLevel), logLevel),
        EventId = eventId,
        Message = formatter != null ? formatter(state, exception) : exception.Message,
        Exception = exception
      };

      SimpleWebLoggerProvider.LogMessages.Add(msg);

    }
  }

  public class LogMessage
  {

    public LogMessage()
    {
      OccurredAt = DateTime.Now;
    }

    public LogLevel Level { get; set; }

    public EventId EventId { get; set; }

    public DateTime OccurredAt { get; }

    public string Message { get; set; }

    public Exception Exception { get; set; }

    public string LevelText { get; internal set; }
  }

  public static class SimpleWebLoggerExtensions
  {

    public static ILoggerFactory AddSimpleWebLogger(this ILoggerFactory factory, LogLevel minLogLevel = LogLevel.Error)
    {

      factory.AddProvider(new SimpleWebLoggerProvider(minLogLevel));

      return factory;

    }

    public static IApplicationBuilder UseSimpleErrorLog(this IApplicationBuilder builder)
    {
      return builder.UseMiddleware<ErrorLog>();
    }


  }

}
