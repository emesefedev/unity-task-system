using System;
using System.Collections.Generic;
using Emesefe.Utilities;
using UnityEngine;

namespace TaskSystem {
    
    public abstract class TaskBase { }

    public class TaskSystem<TaskType> where TaskType : TaskBase {

        private List<TaskType> taskList;
        private List<QueuedTask<TaskType>> queuedTaskList;

        public TaskSystem() {
            taskList = new List<TaskType>(); // List of all tasks ready to be executed
            queuedTaskList = new List<QueuedTask<TaskType>>(); // Any queued task must be validated before being dequeued
            FunctionPeriodic.Create(DequeueTasks, .2f); // No need to try dequeue every single frame
        }

        public TaskType RequestNextTask() {
            if (taskList.Count > 0) {
                // Give worker the first task of the list
                TaskType task = taskList[0];
                taskList.RemoveAt(0);
                
                return task;
            } 
            
            // No tasks are available
            return null;
        }

        public void AddTask(TaskType task) {
            taskList.Add(task);
        }

        public void EnqueueTask(QueuedTask<TaskType> queuedTask)
        {
            queuedTaskList.Add(queuedTask);
        }
        
        public void EnqueueTask(Func<TaskType> tryGetTaskFunc)
        {
            QueuedTask<TaskType> queuedTask = new QueuedTask<TaskType>(tryGetTaskFunc);
            EnqueueTask(queuedTask);
        }
        
        private void DequeueTasks() 
        {
            for (int i = 0; i < queuedTaskList.Count; i++)
            {
                QueuedTask<TaskType> queuedTask = queuedTaskList[i];
                TaskType task = queuedTask.TryDequeueTask();
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