using System.Collections.Generic;
using UnityEngine;

namespace TaskSystem {

    public class TaskSystem {

        public class Task {
            public Vector3 targetPosition;
        }

        private List<Task> taskList;

        public TaskSystem() {
            taskList = new List<Task>();
        }

        public Task RequestNextTask() {
            if (taskList.Count > 0) {
                // Give worker the first task of the list
                Task task = taskList[0];
                taskList.RemoveAt(0);
                
                return task;
            } 
            
            // No tasks are available
            return null;
        }

        public void AddTask(Task task) {
            taskList.Add(task);
        }
    }
}