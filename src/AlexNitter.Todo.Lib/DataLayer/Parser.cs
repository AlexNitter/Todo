using System;
using System.Globalization;

namespace AlexNitter.Todo.Lib.DataLayer
{
    public class Parser
    {
        public static DateTime StringToDateTime(String value)
        {
            return DateTime.ParseExact(value, Config.DATE_FORMAT, new DateTimeFormatInfo());
        }

        public static String DateTimeToString(DateTime value)
        {
            return value.ToString(Config.DATE_FORMAT);
        }

        /// <summary>
        /// Liefert true, wenn value = 1 ist, sonst false
        /// </summary>
        public static Boolean IntToBoolean(Int32 value)
        {
            return (value == 1 ? true : false);
        }

        /// <summary>
        /// Liefert 1, wenn value = true ist, sonst 0
        /// </summary>
        public static Int32 BooleanToInt(Boolean value)
        {
            return (value ? 1 : 0);
        }
    }
}
