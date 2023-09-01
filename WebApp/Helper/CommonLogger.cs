namespace WebApp.Helper
{
    public class CommonLogger
    {
        //for Nlog Documenataion,plz visit: https://github.com/NLog/NLog/wiki/Tutorial

        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        // very serious errors!
        public static void LogFatal(string message, Exception exception)
        {
            lock (Log)
            {
                Log.Fatal(exception);
            }
        }
        //error messages - most of the time these are Exceptions
        public static void LogError(string message, Exception exception)
        {
            lock (Log)
            {
                //TODO binay: stack trace is not in log. I have addded this manually for now.
                var msg = (string.IsNullOrEmpty(message) ? exception.Message : message) + Environment.NewLine + "Stack: " + exception.ToString();

                if (exception.InnerException != null)
                {
                    msg += Environment.NewLine + "Inner Exception :" + exception.InnerException.ToString();
                }
                Log.Error(exception, msg);
            }
        }
        // warning messages, typically for non-critical issues, which can be recovered or which are temporary failures
        public static void LogWarn(string message, Exception exception)
        {
            lock (Log)
            {
                Log.Warn(exception, string.IsNullOrEmpty(message) ? exception.Message : message);
            }
        }
        //information messages, which are normally enabled in production environment
        public static void LogInfo(string message)
        {
            lock (Log)
            {
                Log.Info(message);
            }
        }
        public static void LogInfo(string message, Exception exception)
        {
            lock (Log)
            {
                Log.Info(exception, string.IsNullOrEmpty(message) ? exception.Message : message);
            }
        }
        //debugging information, less detailed than trace, typically not enabled in production environment.
        public static void Debug(string message, Exception exception)
        {
            lock (Log)
            {
                Log.Debug(exception, string.IsNullOrEmpty(message) ? exception?.Message ?? string.Empty : message);
            }
        }
        //very detailed logs, which may include high-volume information such as protocol payloads. 
        //This log level is typically only enabled during development
        public static void Trace(string message, Exception exception)
        {
            lock (Log)
            {
                Log.Trace(exception);
            }
        }
    }
}
