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

namespace BarrettTorqueEm.ObjectPooler {
    public interface IPooledItem {
        bool OnEnpool();
        void OnDestroy();
        System.Type GetType { get; }
    }
    public class ObjectPooler {
        public Dictionary<string, Stack<IPooledItem>> poolDictionary;

        ///<summary>
        ///Trys to get an item from the pool by <paramref name="ID">
        ///<param name="ID"> Id of the pool you wish to pull from.</param>
        ///<param name="Item"> Item returned from the pool if successful.</param>
        ///<returns>Item retreved from the pool.</returns>
        ///</summary>
        public bool TryGetFromPool(string ID, out IPooledItem Item) {
            if (poolDictionary[ID].Count == 0) {
                Item = null;
                return false;
            }

            if (!poolDictionary.ContainsKey(ID)) {
                Debug.LogWarning($"Pool with ID {ID} doesn't exist.");
                Item = null;
                return false;
            }

            Item = poolDictionary[ID].Pop();
            return true;
        }

        ///<summary>
        ///Adds an item to the pool by <paramref name="ID">
        ///<param name="ID"> Id of the pool you wish to add to.</param>
        ///<param name="Item"> Item to be added to the pool.</param>
        ///</summary>
        public void AddToPool(string ID, IPooledItem Item) {
            if (Item.OnEnpool()) {
                if (!poolDictionary.ContainsKey(ID)) {
                    poolDictionary.Add(ID, new Stack<IPooledItem>());
                }
                poolDictionary[ID].Push(Item);
            }
        }

        public string[] ListPools() {
            //for each key in pooldictionary add temp array and return
            throw new System.NotImplementedException();
        }

        public void RemovePool() {
            throw new System.NotImplementedException();
        }
    }
}

