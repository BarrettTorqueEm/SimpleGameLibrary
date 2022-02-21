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
*
*===============================================================*/
using System;
using System.IO;
using System.Text;
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

		private static FileStream fs;

		/// <summary>
		/// Format messages and write them to the log file.
		/// <param name="Level">Log severarity.</param>
		/// <param name="Sender">Object that is sending the log message.</param>
		/// <param name="Message">Object that is sending the log message.</param>
		/// </summary>
		public static void LogMessage(LogLevel Level, object Sender, string Message)
		{
			LogMessage(Level, Sender.ToString(), Message);
		}

		/// <summary>
		/// Format messages and write them to the log file.
		/// <param name="Level">Log severarity.</param>
		/// <param name="Sender">Object that is sending the log message.</param>
		/// <param name="Message">Object that is sending the log message.</param>
		/// </summary>
		public static void LogMessage(LogLevel Level, string Sender, string Message)
		{
			if (fs == null)
			{
				Debug.LogError($"{DateTime.Now.ToString("HH:mm:ss")} - No log file created. Please call CreateLogFile().");
			}

			//12/20 MWG
			switch (Level)
			{
				case LogLevel.Debug:
					Debug.Log($"{DateTime.Now.ToString("HH:mm:ss")} - [{Level.ToString()}] ({Sender.ToString()}): {Message}\n");
					break;
				case LogLevel.Info:
					Debug.Log($"{DateTime.Now.ToString("HH:mm:ss")} - [{Level.ToString()}] ({Sender.ToString()}): {Message}\n");
					break;
				case LogLevel.Warning:
					Debug.LogWarning($"{DateTime.Now.ToString("HH:mm:ss")} - [{Level.ToString()}] ({Sender.ToString()}): {Message}\n");
					break;
				case LogLevel.Error:
					Debug.LogError($"{DateTime.Now.ToString("HH:mm:ss")} - [{Level.ToString()}] ({Sender.ToString()}): {Message}\n");
					break;
				case LogLevel.Fatal:
					Debug.LogError($"{DateTime.Now.ToString("HH:mm:ss")} - [{Level.ToString()}] ({Sender.ToString()}): {Message}\n");
					break;
				default:
					break;
			}

			if (Level == LogLevel.Debug && Debug.isDebugBuild)
			{
				fs.Write(Utils.ToBtyeArray($"{DateTime.Now.ToString("HH:mm:ss")} - [{Level.ToString()}] ({Sender.ToString()}): {Message}\n"));
			}
			else
			{
				fs.Write(Utils.ToBtyeArray($"{DateTime.Now.ToString("HH:mm:ss")} - [{Level.ToString()}] ({Sender.ToString()}): {Message}\n"));
			}
		}

		/// <summary>
		/// Checks path for a directory if exists creates file, else create directory and file.
		/// <param name="Path">Path where the log files are to be created (eg. C:/Users/Mike/Desktop/Log/)
		/// </summary>
		public static void CreateLogFile(string Path, string[] header = null)
		{
			bool exists = Directory.Exists(Path);

			if (!exists)
			{
				Directory.CreateDirectory(Path);
			}

			fs = File.Create($"{Path}/{Application.productName}_{DateTime.Now.ToString("MM_dd_yyyy_HH_mm_ss")}.log");

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
			if (fs == null)
				return;

			if (footer != null)
			{
				for (int i = 0; i < footer.Length; i++)
				{
					LogMessage(LogLevel.Info, "LogHandler.cs", footer[i]);
				}
			}

			fs.Flush();
			fs.Close();
		}
	}
}