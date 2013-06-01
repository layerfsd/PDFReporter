using System;
using System.Collections.Generic;
using System.Text;

namespace AxiomCoders.PdfReports
{
    internal class Logger
    {
        public const string LogFileName = "pdfreports.log";

        /// <summary>
        /// Log warning
        /// </summary>
        /// <param name="format"></param>
        /// <param name="objs"></param>
        public static void LogWarning(string format, params object[] args)
        {
            string msg = string.Format(format, args);
            System.IO.File.AppendAllText(LogFileName, string.Format("WARNING: {0} - {1}\r\n", DateTime.Now.ToString(), msg));
        }

        /// <summary>
        /// Log error
        /// </summary>
        /// <param name="format"></param>
        /// <param name="objs"></param>
        public static void LogError(string format, params object[] args)
        {
            string msg = string.Format(format, args);
            System.IO.File.AppendAllText(LogFileName, string.Format("ERROR: {0} - {1}\r\n", DateTime.Now.ToString(), msg));
        }

        /// <summary>
        /// Log exception
        /// </summary>
        /// <param name="format"></param>
        /// <param name="objs"></param>
        public static void LogException(Exception ex)
        {
            string msg = string.Format("Exception: {0}, StackTrace: {1}", ex.Message, ex.StackTrace);
            System.IO.File.AppendAllText(LogFileName, string.Format("ERROR: {0} - {1}\r\n", DateTime.Now.ToString(), msg));
        }

        /// <summary>
        /// Log notice
        /// </summary>
        /// <param name="format"></param>
        /// <param name="objs"></param>
        public static void LogNotice(string format, params object[] args)
        {
            string msg = string.Format(format, args);
            System.IO.File.AppendAllText(LogFileName, string.Format("NOTICE: {0} - {1}\r\n", DateTime.Now.ToString(), msg));
        }

    }
}
