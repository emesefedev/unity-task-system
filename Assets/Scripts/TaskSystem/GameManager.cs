using System;
using UnityEngine;
using Emesefe.Utilities;

namespace TaskSystem {

    public class GameManager : MonoBehaviour {
        public static GameManager Instance { get; private set; }

        private TaskSystem taskSystem;
        public GameObject workerPrefab;

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

            Worker worker = InstantiateWorker(Vector3.zero);
            WorkerTaskAI workerTaskAI = worker.gameObject.AddComponent<WorkerTaskAI>();
            workerTaskAI.Setup(worker, taskSystem);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                TaskSystem.Task newTask = new TaskSystem.Task { targetPosition = Utils.GetMouseWorldPosition()};
                taskSystem.AddTask(newTask);
            }
        }

        private Worker InstantiateWorker(Vector3 position)
        {
            return Instantiate(workerPrefab, position, Quaternion.identity).GetComponent<Worker>();
        }
    }
}