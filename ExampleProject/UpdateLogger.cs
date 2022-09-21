using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using ISerilogger = Serilog.ILogger;


namespace ExampleProject;

internal class UpdateLogger : ILogger
{
    private readonly ISerilogger _logger;
    public UpdateLogger(ISerilogger logger)
    {
        _logger = logger;
        logger.Information("[NewLogger] Initialize new serilogger");
    }
#nullable enable
    private static ILogger? DefaultInstance { get; set; }
#nullable disable

    public static ILogger GetDefault()
    {
        if (DefaultInstance is not null) return DefaultInstance;
        Serilog.Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(theme: AnsiConsoleTheme.Code)
            .CreateLogger();

        UpdateLogger logger = new(Serilog.Log.ForContext<MekUpdater.UpdateBuilder.Update>());
        DefaultInstance = logger;
        return logger;
    }

    public IDisposable BeginScope<TState>(TState state) => default;


    public bool IsEnabled(LogLevel logLevel)
    {
        return _logger.IsEnabled(LogLevelToEventLevel(logLevel));
    }


    public void Log<TState>(
        LogLevel logLevel, EventId eventId,
        TState state, Exception exception,
        Func<TState, Exception, string> formatter)
    {
        if (IsEnabled(logLevel) is false) return;
        _logger.Write(LogLevelToEventLevel(logLevel), exception, state.ToString());
    }

    private static LogEventLevel LogLevelToEventLevel(LogLevel logLevel)
    {
        return logLevel switch
        {
            LogLevel.Debug => LogEventLevel.Debug,
            LogLevel.Information => LogEventLevel.Information,
            LogLevel.Warning => LogEventLevel.Warning,
            LogLevel.Error => LogEventLevel.Error,
            LogLevel.Critical => LogEventLevel.Fatal,
            LogLevel.None => LogEventLevel.Verbose,
            LogLevel.Trace => LogEventLevel.Verbose,
            _ => LogEventLevel.Verbose
        };
    }

}
