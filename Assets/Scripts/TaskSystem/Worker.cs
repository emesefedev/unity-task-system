using System;
using UnityEngine;

namespace TaskSystem {

    public class Worker : IWorker {

        public GameObject gameObject;

        public static Worker Create(Vector3 position) {
            return new Worker(position);
        }

        private Worker(Vector3 position)
        {
            gameObject = GameManager.Instance.InstantiateWorker(position);
        }

        public void MoveTo(Vector3 position, Action onArrivedAtPosition = null) {
            
        }

        public bool IsMoving()
        {
            return true;
        }

        public Vector3 GetPosition() {
            return gameObject.transform.position;
        }

    }

}