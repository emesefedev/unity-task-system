/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this Code Monkey project
    I hope you find it useful in your own projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */
 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CM_TaskSystem {

    public class CM_TaskSystem {

        public class Task {
            public Vector3 targetPosition;
        }

        private List<Task> taskList;

        public CM_TaskSystem() {
            taskList = new List<Task>();
        }

        public Task RequestNextTask() {
            // Worker requesting a task
            if (taskList.Count > 0) {
                // Give worker the first task
                Task task = taskList[0];
                taskList.RemoveAt(0);
                return task;
            } else {
                // No tasks are available
                return null;
            }
        }

        public void AddTask(Task task) {
            taskList.Add(task);
        }


    }

}