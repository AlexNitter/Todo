using System;
using System.Collections.Generic;
using System.Text;

namespace AlexNitter.Todo.Lib
{
    public class Config
    {
        public const String DATE_FORMAT = "dd.MM.yyyy";
        public const Int32 SALT_BIT_SIZE = 256;

        public static String Connectionstring { get; private set; }
        public static String LogFileDirectory { get; private set; }
        
        public static void InitializeConfig(String connectionstring, String logFileDirectory)
        {
            Connectionstring = connectionstring;
            LogFileDirectory = logFileDirectory;
        }
    }
}
