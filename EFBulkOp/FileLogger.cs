using System;
using System.IO;

namespace EFBulkOp
{
    internal static class FileLogger
    {
        private static readonly FileInfo logFile;

        static FileLogger()
        {
            try
            {
                logFile = new FileInfo("results.log");
                using (var fs = logFile.AppendText())
                {
                    fs.Write($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm}] [INFO] ");
                    fs.WriteLine("Log started _______________\r\n");
                }
            }
            catch (Exception ex)
            {
                // ignored
            }

        }

        public static void Info(string str)
        {
            try
            {
                using (var fs = logFile.AppendText())
                {
                    fs.Write($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm}] [INFO] ");
                    fs.WriteLine(str);
                }
            }
            catch (Exception ex)
            {
                // ignored
            }
        }

        public static void Error(string str)
        {
            try
            {
                using (var fs = logFile.AppendText())
                {
                    fs.Write($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm}] [ERROR] ");
                    fs.WriteLine(str);
                }
            }
            catch (Exception e)
            {
                // ignored
            }
        }
    }
}
