using UnityEngine;
using Emesefe;

namespace TaskSystem
{
    public class WorkerTransporterTaskAI : MonoBehaviour
    {
        private enum State
        {
            WaitingForNextTask,
            ExecutingTask,
        }
        
        private Worker _worker;
        private State _state;
        private TaskSystem<TransporterTask> _taskSystem;
        
        private float waitingTimer;
        private float waitingTimerMax = .2f;

        public void Setup(Worker worker, TaskSystem<TransporterTask> taskSystem)
        {
            _worker = worker;  
            _state = State.WaitingForNextTask;
            _taskSystem = taskSystem;
        }

        private void Update()
        {
            switch (_state)
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
            EmesefeDebug.TextPopup("RequestNextTask", _worker.GetPosition());
            TransporterTask task = _taskSystem.RequestNextTask();

            if (task == null)
            {
                // No tasks available
                _state = State.WaitingForNextTask;
            }
            else
            {
                _state = State.ExecutingTask;
                switch (task)
                {
                    case TransporterTask.TakeWeaponFromWeaponSlotToPosition takeWeaponFromWeaponSlotToPositionTask:
                        ExecuteTakeWeaponFromWeaponSlotToPositionTask(takeWeaponFromWeaponSlotToPositionTask);
                        break;
                }
               
            }
        }
        
        private void ExecuteTakeWeaponFromWeaponSlotToPositionTask(TransporterTask.TakeWeaponFromWeaponSlotToPosition task)
        {
            EmesefeDebug.TextPopupMouse("ExecuteTakeWeaponFromWeaponSlotToPositionTask");
            _worker.MoveTo(task.weaponSlotPosition,() =>
            {
                task.grabWeapon(this);
                _worker.MoveTo(task.targetPosition, () =>
                {
                    task.dropWeapon();
                    _state = State.WaitingForNextTask;
                });
            });
        }
    }    
}

