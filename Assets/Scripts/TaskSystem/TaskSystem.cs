using System;
using System.Collections.Generic;
using Emesefe.Utilities;
using UnityEngine;

namespace TaskSystem {

    public class TaskSystem {

        private List<Task> taskList;
        private List<QueuedTask> queuedTaskList;

        public TaskSystem() {
            taskList = new List<Task>(); // List of all tasks ready to be executed
            queuedTaskList = new List<QueuedTask>(); // Any queued task must be validated before being dequeued
            FunctionPeriodic.Create(DequeueTasks, .2f); // No need to try dequeue every single frame
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

        public void EnqueueTask(QueuedTask queuedTask)
        {
            queuedTaskList.Add(queuedTask);
        }
        
        public void EnqueueTask(Func<Task> tryGetTaskFunc)
        {
            QueuedTask queuedTask = new QueuedTask(tryGetTaskFunc);
            EnqueueTask(queuedTask);
        }
        
        private void DequeueTasks() 
        {
            for (int i = 0; i < queuedTaskList.Count; i++)
            {
                QueuedTask queuedTask = queuedTaskList[i];
                Task task = queuedTask.TryDequeueTask();
                if (task != null)
                {
                    // Task dequeued. Let's add it to the taskList and remove it from queuedTaskList
                    AddTask(task);
                    queuedTaskList.RemoveAt(i);
                    i--;
                }
                else
                {
                    // Task remains queued
                }
            }
        }
    }
}