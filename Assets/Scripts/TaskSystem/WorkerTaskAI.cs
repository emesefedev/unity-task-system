using UnityEngine;
using Emesefe;

namespace TaskSystem
{
    public class WorkerTaskAI : MonoBehaviour
    {
        private enum State
        {
            WaitingForNextTask,
            ExecutingTask,
        }
        
        private IWorker worker;
        private State state;
        private TaskSystem taskSystem;
        
        private float waitingTimer;
        private float waitingTimerMax = .2f;

        public void Setup(IWorker worker, TaskSystem taskSystem)
        {
            this.worker = worker;  
            state = State.WaitingForNextTask;
            this.taskSystem = taskSystem;
        }

        private void Update()
        {
            switch (state)
            {
                case State.WaitingForNextTask:
                    waitingTimer -= Time.deltaTime;
                    if (waitingTimer <= 0)
                    {
                        waitingTimer = waitingTimerMax;
                        RequestNextTask();
                    }
                    break;
                
                case State.ExecutingTask:
                    break;
            }
        }

        private void RequestNextTask()
        {
            EmesefeDebug.TextPopupMouse("RequestNextTask");
            TaskSystem.Task task = taskSystem.RequestNextTask();

            if (task == null)
            {
                // No tasks available
                state = State.WaitingForNextTask;
            }
            else
            {
                state = State.ExecutingTask;
                ExecuteTask(task);
            }
        }

        private void ExecuteTask(TaskSystem.Task task)
        {
            EmesefeDebug.TextPopupMouse("ExecuteTask");
            worker.MoveTo(task.targetPosition, () =>
            {
                state = State.WaitingForNextTask;
            });
        }
    }    
}

