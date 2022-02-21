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
*
*===============================================================*/
using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace SGLSharp
{
	public static class Utils
	{
		public static Byte[] ToBtyeArray(string StringToConvert)
		{
			return Encoding.ASCII.GetBytes(StringToConvert);
		}
	}
}