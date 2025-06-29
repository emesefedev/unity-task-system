using UnityEngine;

namespace TaskSystem {

    public class GameManager : MonoBehaviour {

        private TaskSystem taskSystem;

        private void Start() {
            taskSystem = new TaskSystem();
            
            Debug.Log(taskSystem.RequestNextTask());
            
            TaskSystem.Task task = new TaskSystem.Task();
            taskSystem.AddTask(task);
            Debug.Log(taskSystem.RequestNextTask());
            
            Debug.Log(taskSystem.RequestNextTask());
        }
    }
}