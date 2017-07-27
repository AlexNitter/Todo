using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AlexNitter.Todo.Lib.Services
{
    public class LoggingService
    {
        private static Object _lock = new Object();

        public static void Log(Exception ex)
        {
            var builder = new StringBuilder();

            builder.AppendLine("Datum / Uhrzeit: " + DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss"));
            builder.AppendLine("Fehlermeldung: " + ex.Message);
            builder.AppendLine("Stacktrace: " + ex.StackTrace);

            if (ex.InnerException != null)
            {
                builder.AppendLine("Inner Exception Fehlermeldung: " + ex.InnerException.Message);
                builder.AppendLine("Inner Exception Stacktrace: " + ex.InnerException.StackTrace);
            }

            createAndWriteFile(builder.ToString());
        }

        private static void createAndWriteFile(String content)
        {
            createFolder(Config.LogFileDirectory);

            var fileName = DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss_") + Guid.NewGuid().ToString() + ".log";

            File.WriteAllText(Path.Combine(Config.LogFileDirectory, fileName), content);
        }

        /// <summary>
        /// Überprüft, ob der Ordner im übergebenen Pfad bereits existiert, und legt ihn ggf. an (threadsafe)
        /// </summary>
        private static void createFolder(String folder)
        {
            if (!Directory.Exists(folder))
            {
                lock (_lock)
                {
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }
                }
            }
        }
    }
}

