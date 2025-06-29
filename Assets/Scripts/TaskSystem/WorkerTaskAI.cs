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
        
        private float waitingTimer;
        private float waitingTimerMax = .5f;

        public void Setup(IWorker worker)
        {
            this.worker = worker;  
            this.state = State.WaitingForNextTask;
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
        }
    }    
}

