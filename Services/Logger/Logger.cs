using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysteriousTools.Logger
{
    public static class Logger
    {
        private static readonly object locker = new object();
        private static string logPath;
        public static string FullLogPath;

        #region Initialization

        public static void Initialize(string logFilePath, string logFileName)
        {
            logPath = logFilePath;
            FullLogPath = logFilePath + logFileName;//Path.Combine(logFilePath, logFileName);
            DateTime NowTime = DateTime.UtcNow;

            if (!Directory.Exists(logFilePath)) { Directory.CreateDirectory(logFilePath); }
            if (!File.Exists(FullLogPath))
            {
                using (StreamWriter sw = File.CreateText(FullLogPath))
                {
                    sw.WriteLine($"{NowTime:dd.MM.yyyy HH:mm:ss.fff}  : *************** Mysterious Log Service ***************");
                }
            }
        }

        #endregion

        #region Logging

        public static async Task LogAsync(string text)
        {
            await Task.Run(() =>
            {
                lock (locker)
                {
                    try
                    {
                        using (StreamWriter streamWriter = new StreamWriter(FullLogPath, true))
                        {
                            DateTime NowTime = DateTime.UtcNow;
                            StringBuilder stringBuilder = new StringBuilder();
                            stringBuilder.Append($"{NowTime:dd.MM.yyyy HH:mm:ss.fff}");
                            stringBuilder.Append("  : ");
                            stringBuilder.Append(text);

                            streamWriter.WriteLine(stringBuilder.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            });
        }
        public static void Log(string text)
        {
            lock (locker)
            {
                try
                {
                    using (StreamWriter streamWriter = new StreamWriter(FullLogPath, true))
                    {
                        DateTime NowTime = DateTime.UtcNow;
                        StringBuilder stringBuilder = new StringBuilder();
                        stringBuilder.Append($"{NowTime:dd.MM.yyyy HH:mm:ss.fff}");
                        stringBuilder.Append("  : ");
                        stringBuilder.Append(text);

                        streamWriter.WriteLine(stringBuilder.ToString());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }
        public static void ForceLog(string text, string fileName)
        {
            lock (locker)
            {
                try
                {
                    using (StreamWriter streamWriter = new StreamWriter(Path.Combine(logPath, fileName), true))
                    {
                        DateTime NowTime = DateTime.UtcNow;
                        StringBuilder stringBuilder = new StringBuilder();
                        stringBuilder.Append($"{NowTime:dd.MM.yyyy HH:mm:ss.fff}");
                        stringBuilder.Append("  : ");
                        stringBuilder.Append(text);
                        streamWriter.WriteLineAsync(stringBuilder.ToString());
                        streamWriter.Close();
                    }
                }
                catch { }
            }
        }

        #endregion
    }
}
