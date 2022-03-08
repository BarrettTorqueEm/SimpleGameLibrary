/*===============================================================
*	Project:    	SimpleGameLibrary
*	Developer:  	Michael Girard (mgirard989@outlook.com)
*	Company:    	Triad
*	Creation Date:	12/29/2020 13:40
*	Version:    	1.0
*	Description:	Creates a log file and methods to write to that file.
*===============================================================*/

/*==============================================================
*	Audit Log:
*       12/20/2022 MWG Updated Logic to compile.
*       12/21/2022 MWG Added ability to toggle time printing in log.
*								Misc changes for formating.
*
*===============================================================*/
using System;
using System.IO;
using UnityEngine;

namespace SGLSharp
{
    public static class LogHandler
    {
        public enum LogLevel
        {
            Debug,
            Info,
            Warning,
            Error,
            Fatal,
        }

        private static StreamWriter sw;
        private static bool logTime; // 12/21 MWG
        private static bool isBlock;

        /// <summary>
        /// Format messages and write them to the log file.
        /// <param name="Level">Log severarity.</param>
        /// <param name="Sender">Object that is sending the log message. Calls ToString() on object passed.</param>
        /// <param name="Message">Message to be printed to the log.</param>
        /// </summary>
        public static void LogMessage(LogLevel Level, object Sender, string Message)
        {
            LogMessage(Level, Sender.ToString(), Message);
        }

        /// <summary>
        /// Format messages and write them to the log file.
        /// <param name="Level">Log severarity.</param>
        /// <param name="Sender">Object that is sending the log message. Calls ToString() on object passed.</param>
        /// <param name="Message">Will create a 'Block' message to be printed to log.</param>
        /// </summary>
        public static void LogMessage(LogLevel Level, object Sender, string[] Message)
        {
            LogMessage(Level, Sender.ToString(), Message);
        }

        /// <summary>
        /// Format messages and write them to the log file.
        /// <param name="Level">Log severarity.</param>
        /// <param name="Sender">Object that is sending the log message.</param>
        /// <param name="Message">Will create a 'Block' message to be printed to log.</param>
        /// </summary>
        public static void LogMessage(LogLevel Level, string Sender, string[] Message)
        {
            foreach (string s in Message)
            {
                LogMessage(Level, Sender.ToString(), s);
                isBlock = true;
            }
			
            isBlock = false;
        }

        /// <summary>
        /// Format messages and write them to the log file.
        /// <param name="Level">Log severarity.</param>
        /// <param name="Sender">Object that is sending the log message.</param>
        /// <param name="Message">Message to be printed to the log.</param>
        /// </summary>
        public static void LogMessage(LogLevel Level, string Sender, string Message)
        {
            string s;
            ///TODO: Make better
            //12/21 MWG
            //If this is a block log remove the first line info (eg time and level) and indent the message
            if (isBlock)
                s = $"\t\t {Message}";
            else
                s = $"[{Level.ToString()}] ({Sender.ToString()}): {Message}";

            if (sw == null)
            {
                Debug.LogError($"No log file created. Please call LogHandler.CreateLogFile().");
            }

            //12/20 MWG)
            switch (Level)
            {
                case LogLevel.Debug:
                    Debug.Log(s);
                    break;
                case LogLevel.Info:
                    Debug.Log(s);
                    break;
                case LogLevel.Warning:
                    Debug.LogWarning(s);
                    break;
                case LogLevel.Error:
                    Debug.LogError(s);
                    break;
                case LogLevel.Fatal:
                    Debug.LogError(s);
                    break;
                default:
                    break;
            }

            //12/21 MWG
            if (logTime)
                s = $"{DateTime.Now.ToString("HH:mm:ss")} {s}";

            if (Level == LogLevel.Debug && Debug.isDebugBuild)
            {
                sw.WriteLine(s);
            }
            else if (Level != LogLevel.Debug)
            {
                sw.WriteLine(s);
            }
        }

        /// <summary>
        /// Checks path for a directory if exists creates file, else create directory and file.
        /// <param name="Path">Path where the log files are to be created (eg. C:/Users/Mike/Desktop/Log/)
        /// </summary>
        public static void CreateLogFile(string Path, string[] header = null, bool includeTime = true)
        {
            //12/21 MWG
            logTime = includeTime;

            bool exists = Directory.Exists(Path);

            if (!exists)
            {
                Directory.CreateDirectory(Path);
            }

            if (sw == null)
                sw = new StreamWriter($"{Path}/{Application.productName}_{DateTime.Now.ToString("MM_dd_yyyy_HH_mm_ss")}.log");

            sw.AutoFlush = true;

            LogMessage(LogLevel.Info, "LogHandler.cs", "Created File");

            if (header != null)
            {
                for (int i = 0; i < header.Length; i++)
                {
                    LogMessage(LogLevel.Info, "LogHandler.cs", header[i]);
                }
            }
        }

        ///<summary>
        ///Closes the log file.
        ///</summary>
        public static void Close(string[] footer = null)
        {
            if (sw == null)
                return;

            LogMessage(LogLevel.Info, "LogHandler.cs", "Closing File");

            if (footer != null)
            {
                for (int i = 0; i < footer.Length; i++)
                {
                    LogMessage(LogLevel.Info, "LogHandler.cs", footer[i]);
                }
            }

            sw.Flush();
            sw.Close();
        }
    }
}