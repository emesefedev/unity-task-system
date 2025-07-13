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
                InstantiateStainWithTask(Utils.GetMouseWorldPosition());
            }
            
            if (Input.GetMouseButtonDown(1))
            {
                Task newTask = new Task.MoveToPositionTask { targetPosition = Utils.GetMouseWorldPosition()};
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

        private void InstantiateStainWithTask(Vector3 position)
        {
            GameObject stain = InstantiateStain(position);
            float cleanUpTime = Time.time + 5f; // 5 seconds must pass after instantiation until start cleaning
            //taskSystem.AddTask(newTask);
            taskSystem.EnqueueTask(() =>
            {
                if (Time.time >= cleanUpTime)
                {
                    Task newTask = new Task.CleanUpTask
                    {
                        targetPosition = stain.transform.position,
                    
                        onCleanupAction = () =>
                        {
                            float alpha = 1;
                            FunctionUpdater.Create(() =>
                            {
                                alpha -= Time.deltaTime;
                                foreach (Transform child in stain.transform)
                                {
                                    SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
                                    Color color = spriteRenderer.color;
                                    spriteRenderer.color = new Color(color.r, color.g, color.b, alpha);
                                }

                            });
                        }
                    };
                    return newTask;
                }

                return null;
            });
        }
    }
}