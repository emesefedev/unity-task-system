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
using CodeMonkey;

namespace CM_TaskSystem {

    public class CM_WorkerTaskAI : MonoBehaviour {

        private enum State {
            WaitingForNextTask,
            ExecutingTask,
        }

        private CM_IWorker worker;
        private CM_TaskSystem taskSystem;
        private State state;
        private float waitingTimer;

        public void Setup(CM_IWorker worker, CM_TaskSystem taskSystem) {
            this.worker = worker;
            this.taskSystem = taskSystem;
            state = State.WaitingForNextTask;
        }

        private void Update() {
            switch (state) {
            case State.WaitingForNextTask:
                // Waiting to request a new task
                waitingTimer -= Time.deltaTime;
                if (waitingTimer <= 0) {
                    float waitingTimerMax = .2f; // 200ms
                    waitingTimer = waitingTimerMax;
                    RequestNextTask();
                }
                break;
            case State.ExecutingTask:
                break;
            }
        }

        private void RequestNextTask() {
            CMDebug.TextPopup("RequestNextTask", worker.GetPosition());
            CM_TaskSystem.Task task = taskSystem.RequestNextTask();
            if (task == null) {
                state = State.WaitingForNextTask;
            } else {
                state = State.ExecutingTask;
                ExecuteTask(task);
            }
        }

        private void ExecuteTask(CM_TaskSystem.Task task) {
            CMDebug.TextPopup("ExecuteTask", worker.GetPosition());
            worker.MoveTo(task.targetPosition, () => {
                state = State.WaitingForNextTask;
            });
        }
    }

}