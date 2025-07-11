using System;
using System.Collections.Generic;
using UnityEngine;

namespace TaskSystem {

    public class TaskSystem {

        public abstract class Task {
            public class MoveToPositionTask : Task { public Vector3 targetPosition; }
            public class VictoryTask : Task { }

            public class CleanUpTask : Task
            {
                public Vector3 targetPosition;
                public Action onCleanupAction;
            }
            
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