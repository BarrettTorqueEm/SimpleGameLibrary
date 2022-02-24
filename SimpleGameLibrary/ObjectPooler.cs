/*
/*===============================================================
*	Project:    	SimpleGameLibrary
*	Developer:  	Michael Girard (mgirard989@outlook.com)
*	Company:    	Triad
*	Creation Date:	12/29/2020 13:40
*	Version:    	1.0
*	Description:	Handles pooling and depooling of objects.
*===============================================================*/

/*==============================================================
*	Audit Log:
*
*
*===============================================================*/
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SGLSharp {
	public class ObjectPooler {
		private Dictionary<string, Stack<IPoolable>> poolDictionary = new Dictionary<string, Stack<IPoolable>>();

		///<summary>
		///Trys to get an item from the pool by <paramref name="ID">
		///<param name="ID"> Id of the pool you wish to pull from.</param>
		///<param name="Item"> Item returned from the pool if successful.</param>
		///<returns>Item retreved from the pool.</returns>
		///</summary>
		public bool TryGetFromPool(string ID, out IPoolable Item) {
			if (poolDictionary[ID].Count == 0) {
				Item = null;
				return false;
			}

			if (!poolDictionary.ContainsKey(ID)) {
				LogHandler.LogMessage(LogHandler.LogLevel.Warning, "ObjectPooler.cs", $"Pool with ID {ID} doesn't exist.");
				Item = null;
				return false;
			}

			Item = poolDictionary[ID].Pop();
			return true;
		}

		///<summary>
		///Adds an item to the pool by <paramref name="ID">.
		///If a pool does not exist, creates one.
		///<param name="ID"> Id of the pool you wish to add to.</param>
		///<param name="Item"> Item to be added to the pool.</param>
		///</summary>
		public void AddToPool(string ID, IPoolable Item) {
			if (Item.OnEnpool()) {
				if (!poolDictionary.ContainsKey(ID)) {
					poolDictionary.Add(ID, new Stack<IPoolable>());
				}
				poolDictionary[ID].Push(Item);
			}
		}

		///<summary>
		///<returns> List of all keys in the dictionary.</reutrns>
		///</summary>
		public string[] ListPools() {
			return poolDictionary.Keys.ToArray();
		}

		///<summary>
		///Removes entire pool by <paramref name="ID">
		///<param name="ID"> ID of the pool you with to remove.</param>
		///</summary>
		public void RemovePool(string ID) {
			if (poolDictionary.ContainsKey(ID)) {
				poolDictionary.Remove(ID);
			}
		}
	}
}

