using System;
using UnityEngine;

namespace TaskSystem {

    public class GameManager : MonoBehaviour {
        public static GameManager Instance { get; private set; }

        private TaskSystem taskSystem;
        public GameObject worker;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError($"There is more than one GameManager in scene");
            }
            Instance = this;
        }

        private void Start() {
            taskSystem = new TaskSystem();

            Worker worker = Worker.Create(Vector3.zero);
            WorkerTaskAI workerTaskAI = worker.gameObject.AddComponent<WorkerTaskAI>();
            workerTaskAI.Setup(worker);
        }

        public GameObject InstantiateWorker(Vector3 position)
        {
            return Instantiate(worker, position, Quaternion.identity);
        }
    }
}