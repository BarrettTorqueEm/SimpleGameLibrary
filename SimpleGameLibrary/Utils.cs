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
*		03/08/2022:1 DLW Added persistent singleton system
*		03/08/2022:2 DLW Changed instances var to implement as an array 
*		03/08/2022:3 DLW Implemented system to create a gameobject with instance type requested if it is initially null
*		03/08/2022:4 DLW Modified string parsing method to obtain T name and add to message easier
*		03/08/2022:5 DLW Added typeof parameter to object sender in message log functions to remove error
*		03/08/2022:6 DLW Changed if statement to check for null instance to return if null != true. This removes nested if statements
*		03/08/2022:7 DLW Added more useful functions to Utils class
* 		03/08/2022:8 DLW Added a utility to store a reference to main camera so whenever it needs to be retrieved, the 
*			program doesnt need to search the entire scene hierarchy every time.
* 		03/08/2022:9 DLW Added system to cache WaitForSeconds objects to be used for later. Values can be 
*			retrieved globally from any class
*===============================================================*/
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SGLSharp {
	public static class Utils {
		
		public static Byte[] ToBtyeArray(string StringToConvert) {
			return Encoding.ASCII.GetBytes(StringToConvert);
		}

		// 03/08/2022:7
        public static List<T> FindAllInScene<T>() where T : Component
        {
            List<T> results = new List<T>();
            T[] allObjsAry = Resources.FindObjectsOfTypeAll(typeof(T)) as T[];

            if (allObjsAry.Length == 0)
            {
                Debug.LogWarning("Unable to find any of requested objects in scene");
                return null;
            }

            for (var i = 0; i < allObjsAry.Length; ++i)
            {
                results.Add(allObjsAry[i]);
            }

            Debug.Log($"Found {results.Count} items.");
            return results;
        }

        public static T RandomListSelection<T>(this IList<T> list)
        {
            return list[UnityEngine.Random.Range(0, list.Count)];
        }

        // 03/08/2022:8
        private static Camera cam;
        public static Camera Camera
        {
            get
            {
                if (cam == null) cam = Camera.main;
                return cam;
            }
        }

        // 03/08/2022:9
		private static readonly Dictionary<float, WaitForSeconds> WaitingCache = new Dictionary<float, WaitForSeconds>();
		/// <summary>
		/// Retrieve an already allocated WaitForSeconds object. If the value requested does not exist, it will be created
		/// then added to the dicitonary before the value is returned
		/// </summary>
		/// <param name="time">Requested time to pass in for WaitForSeconds</param>
		/// <returns>WaitingCache item containing the WaitForSeconds item with requested duration</returns>
		public static WaitForSeconds GetWait(float time) {
			if (WaitingCache.TryGetValue(time, out var waitItem)) return waitItem;
			
			WaitingCache[time] = new WaitForSeconds(time);
			return WaitingCache[time];
		}
    }

	//02/24 MWG
	public class Singleton<T> : MonoBehaviour where T : Component {
		
		private static T instance;

		public static T Current {
			get {
				// 03/08/2022:6
				if (instance != null) {
					return instance;
				}

				// MWG ORIGINAL: var instances = FindObjectsOfType<T>();
				
				// 03/08/2022:2
				var instances = FindObjectsOfType(typeof(T)) as T[];

				if (instances.Length == 1) {
					instance = instances[0];
				} else if (instances.Length > 1) {
					// 03/08/2022:4
					// 03/08/2022:5
					LogHandler.LogMessage(LogHandler.LogLevel.Error, typeof(T), $"{typeof(T).Name} : There is more than 1 instance in the scene.");
				} else {
					// 03/08/2022:4
					// 03/08/2022:5
					LogHandler.LogMessage(LogHandler.LogLevel.Error, typeof(T), $"{ typeof(T).Name} : Instance doesn't exist in the scene.");
				}

				// 03/08/2022:3
				if (instance == null) {
					GameObject obj = new GameObject(){ hideFlags = HideFlags.HideAndDontSave};			
					instance = obj.AddComponent<T>();
				}
				
				return instance;
			}
		}
	}

	// 03/08/2022:1
	public class SingletonPersis<T> : MonoBehaviour where T : Component {
		
		public static T Instance { get; private set; }

		protected virtual void Awake() {
			if (Instance == null) {
				Instance = this as T;
				DontDestroyOnLoad(this);
			} else {
				Destroy(gameObject);
			}
		}
	}
}