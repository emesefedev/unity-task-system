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
        
        private static List<FunctionUpdater> _updaterList; // Holds a reference to all active updaters
        private static GameObject _initGameObject; // Global game object used for initializing class, is destroyed on scene change
        
        private readonly GameObject _gameObject;
        private readonly Func<bool> _updateFunc; // Destroy Updater if return true
        private readonly string _functionName;
        private readonly bool _active;
        
        private FunctionUpdater(GameObject gameObject, Func<bool> updateFunc, string functionName, bool active) {
            _gameObject = gameObject;
            _updateFunc = updateFunc;
            _functionName = functionName;
            _active = active;
        }
        
        private static void InitIfNeeded()
        {
            if (_initGameObject != null) return;
            
            _initGameObject = new GameObject("FunctionUpdater Global");
            _updaterList = new List<FunctionUpdater>();
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

            _updaterList.Add(functionUpdater);
            return functionUpdater;
        }
        
        public static void StopUpdaterWithName(string functionName) {
            InitIfNeeded();
            
            for (int i = 0; i < _updaterList.Count; i++)
            {
                if (_updaterList[i]._functionName != functionName) continue;
                
                _updaterList[i].DestroySelf();
                return;
            }
        }

        private static void StopAllUpdatersWithName(string functionName) {
            InitIfNeeded();
            
            for (int i = 0; i < _updaterList.Count; i++)
            {
                if (_updaterList[i]._functionName != functionName) continue;
                
                _updaterList[i].DestroySelf();
                i--;
            }
        }
        
        private static void RemoveUpdater(FunctionUpdater funcUpdater) {
            InitIfNeeded();
            _updaterList.Remove(funcUpdater);
        }
        
        private void Update() {
            if (!_active) return;
            if (_updateFunc()) {
                DestroySelf();
            }
        }
        
        private void DestroySelf() {
            RemoveUpdater(this);
            if (_gameObject != null) {
                UnityEngine.Object.Destroy(_gameObject);
            }
        }
    }
}

