using System;
using System.Collections.Generic;
using UnityEngine;

namespace Emesefe.Utilities
{
    public class FunctionUpdater
    {
        // Class to hook Actions into MonoBehaviour
        private class MonoBehaviourHook : MonoBehaviour {

            public Action OnUpdate;

            private void Update() {
                if (OnUpdate != null) OnUpdate();
            }

        }
        
        private static List<FunctionUpdater> updaterList; // Holds a reference to all active updaters
        private static GameObject initGameObject; // Global game object used for initializing class, is destroyed on scene change
        
        private GameObject gameObject;
        private string functionName;
        private bool active;
        private Func<bool> updateFunc; // Destroy Updater if return true;
        
        private FunctionUpdater(GameObject gameObject, Func<bool> updateFunc, string functionName, bool active) {
            this.gameObject = gameObject;
            this.updateFunc = updateFunc;
            this.functionName = functionName;
            this.active = active;
        }
        
        private static void InitIfNeeded() {
            if (initGameObject == null) {
                initGameObject = new GameObject("FunctionUpdater Global");
                updaterList = new List<FunctionUpdater>();
            }
        }
        
        public static FunctionUpdater Create(Action updateFunc) {
            return Create(() => { updateFunc(); return false; }, "", true, false);
        }

        public static FunctionUpdater Create(Action updateFunc, string functionName) {
            return Create(() => { updateFunc(); return false; }, functionName, true, false);
        }

        public static FunctionUpdater Create(Func<bool> updateFunc) {
            return Create(updateFunc, "", true, false);
        }

        public static FunctionUpdater Create(Func<bool> updateFunc, string functionName) {
            return Create(updateFunc, functionName, true, false);
        }

        public static FunctionUpdater Create(Func<bool> updateFunc, string functionName, bool active) {
            return Create(updateFunc, functionName, active, false);
        }

        private static FunctionUpdater Create(Func<bool> updateFunc, string functionName, bool active, bool stopAllWithSameName) {
            InitIfNeeded();

            if (stopAllWithSameName) {
                StopAllUpdatersWithName(functionName);
            }

            GameObject gameObject = new GameObject("FunctionUpdater Object " + functionName, typeof(MonoBehaviourHook));
            FunctionUpdater functionUpdater = new FunctionUpdater(gameObject, updateFunc, functionName, active);
            gameObject.GetComponent<MonoBehaviourHook>().OnUpdate = functionUpdater.Update;

            updaterList.Add(functionUpdater);
            return functionUpdater;
        }
        
        public static void StopUpdaterWithName(string functionName) {
            InitIfNeeded();
            for (int i = 0; i < updaterList.Count; i++) {
                if (updaterList[i].functionName == functionName) {
                    updaterList[i].DestroySelf();
                    return;
                }
            }
        }

        private static void StopAllUpdatersWithName(string functionName) {
            InitIfNeeded();
            for (int i = 0; i < updaterList.Count; i++)
            {
                if (updaterList[i].functionName != functionName) continue;
                
                updaterList[i].DestroySelf();
                i--;
            }
        }
        
        private static void RemoveUpdater(FunctionUpdater funcUpdater) {
            InitIfNeeded();
            updaterList.Remove(funcUpdater);
        }
        
        private void Update() {
            if (!active) return;
            if (updateFunc()) {
                DestroySelf();
            }
        }
        
        private void DestroySelf() {
            RemoveUpdater(this);
            if (gameObject != null) {
                UnityEngine.Object.Destroy(gameObject);
            }
        }
    }
}

