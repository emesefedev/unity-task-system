using UnityEngine;
using Emesefe.Utilities;

namespace TaskSystem {

    public class GameManager : MonoBehaviour {

        private TaskSystem taskSystem;
        
        [SerializeField] private GameObject workerPrefab;
        [SerializeField] private GameObject stainPrefab;

        private void Start() {
            taskSystem = new TaskSystem();

            Worker worker = InstantiateWorker(Vector3.zero);
            WorkerTaskAI workerTaskAI = worker.gameObject.AddComponent<WorkerTaskAI>();
            workerTaskAI.Setup(worker, taskSystem);
            
            worker = InstantiateWorker(5 * Vector3.up);
            workerTaskAI = worker.gameObject.AddComponent<WorkerTaskAI>();
            workerTaskAI.Setup(worker, taskSystem);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameObject stain = InstantiateStain(Utils.GetMouseWorldPosition());
                TaskSystem.Task newTask = new TaskSystem.Task.CleanUpTask
                {
                    targetPosition = stain.transform.position,
                    
                    onCleanupAction = () => Destroy(stain)
                };
                taskSystem.AddTask(newTask);
            }
            
            if (Input.GetMouseButtonDown(1))
            {
                TaskSystem.Task newTask = new TaskSystem.Task.MoveToPositionTask { targetPosition = Utils.GetMouseWorldPosition()};
                taskSystem.AddTask(newTask);
            }

            // if (Input.GetMouseButtonDown(1))
            // {
            //     TaskSystem.Task newTask = new TaskSystem.Task.VictoryTask { };
            //     taskSystem.AddTask(newTask);
            // }
        }

        private Worker InstantiateWorker(Vector3 position)
        {
            return Instantiate(workerPrefab, position, Quaternion.identity).GetComponent<Worker>();
        }
        
        private GameObject InstantiateStain(Vector3 position)
        {
            return Instantiate(stainPrefab, position, Quaternion.identity);
        }
    }
}