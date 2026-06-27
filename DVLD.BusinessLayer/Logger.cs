using System;
using System.IO;
using System.Threading;

namespace DVLD.BusinessLayer
{
    public static class Logger
    {
        private static readonly string LogFilePath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "Logs", "DVLD.log");
        private static readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        static Logger()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(LogFilePath));
        }

        public static void LogError(Exception ex, string context = "")
        {
            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [ERROR] {context}: {ex.Message}\n{ex.StackTrace}\n";
            WriteToFile(logEntry);
        }

        public static void LogWarning(string message, string context = "")
        {
            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [WARN] {context}: {message}\n";
            WriteToFile(logEntry);
        }

        public static void LogInfo(string message, string context = "")
        {
            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [INFO] {context}: {message}\n";
            WriteToFile(logEntry);
        }

        private static void WriteToFile(string logEntry)
        {
            _lock.EnterWriteLock();
            try
            {
                File.AppendAllText(LogFilePath, logEntry);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
    }
}
