/*===============================================================
*	Project:    	SimpleGameLibrary
*	Developer:  	Michael Girard (mgirard989@outlook.com)
*	Company:    	Triad
*	Creation Date: 02/20/2022 20:17
*	Version:    	1.0
*	Description:	Holds helpful functions to be used by other functions
*===============================================================*/

/*==============================================================
*	Audit Log:
*		
*		02/24/2022 MWG Added Singlton class (Thanks Jordan)
*===============================================================*/
using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace SGLSharp {
	public static class Utils {
		public static Byte[] ToBtyeArray(string StringToConvert) {
			return Encoding.ASCII.GetBytes(StringToConvert);
		}
	}

	//02/24 MWG
	public class Singleton<T> : MonoBehaviour where T : Component {
		private static T instance;

		public static T Current {
			get {
				if (instance == null) {
					var instances = FindObjectsOfType<T>();

					if (instances.Length == 1) {
						instance = instances[0];
					} else if (instances.Length > 1) {
						LogHandler.LogMessage(LogHandler.LogLevel.Error, T, typeof(T) + ": There is more than 1 instance in the scene.");
					} else {
						LogHandler.LogMessage(LogHandler.LogLevel.Error, T, typeof(T) + ": Instance doesn't exist in the scene.");
					}
				}
				return instance;
			}
		}
	}
}