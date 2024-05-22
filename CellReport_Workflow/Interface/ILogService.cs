namespace CellReport_Workflow.Interface
{
    public interface ILogService
    {
       
        public void TXTLog(String logMsg);

        //IDisposable BeginScope<TState>(TState state);
        ////
        //// 摘要:
        ////     Checks if the given logLevel is enabled.
        ////
        //// 參數:
        ////   logLevel:
        ////     Level to be checked.
        ////
        //// 傳回:
        ////     true if enabled.
        //bool IsEnabled(LogLevel logLevel);
        ////
        //// 摘要:
        ////     Writes a log entry.
        ////
        //// 參數:
        ////   logLevel:
        ////     Entry will be written on this level.
        ////
        ////   eventId:
        ////     Id of the event.
        ////
        ////   state:
        ////     The entry to be written. Can be also an object.
        ////
        ////   exception:
        ////     The exception related to this entry.
        ////
        ////   formatter:
        ////     Function to create a System.String message of the state and exception.
        ////
        //// 類型參數:
        ////   TState:
        ////     The type of the object to be written.
        //void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter);
    }
}
