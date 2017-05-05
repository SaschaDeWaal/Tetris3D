using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Sascha;

namespace Sascha {
    public class ObjectPool {

        private static ObjectPool _instance;

        private Vector3 hidePosition = new Vector3(-100, -100, -100);
        private Dictionary<IReusableObject, bool> brickList = new Dictionary<IReusableObject, bool>();
        
        public static ObjectPool Instance {
            get {
                if(_instance == null)
                    _instance = new ObjectPool();
                return _instance;
            }
        }

        /// <summary>
        ///  Returns a Script with a GameObject. When there are no more object available, it creates a new one from the Resources folder.
        /// </summary>
        /// <typeparam name="T">Type of the object</typeparam>
        /// <param name="prefabName">Name of the prefab</param>
        /// <returns></returns>
        public IReusableObject GetObject<T>(string prefabName) {

            foreach(KeyValuePair<IReusableObject, bool> reusableObject in brickList) {
                if(!reusableObject.Value) {
                    brickList[reusableObject.Key] = true;
                    return reusableObject.Key;
                }
            }

            IReusableObject newBrick = UnityEngine.Object.Instantiate(Resources.Load(prefabName) as GameObject).GetComponent<IReusableObject>();
            brickList.Add(newBrick, true);

            return newBrick;
        }

        public void ReturnObject(Brick brick) {
            brick.gameObject.transform.position = hidePosition;
            brickList[brick] = false;
        }


        private ObjectPool() {

        }
    }
}