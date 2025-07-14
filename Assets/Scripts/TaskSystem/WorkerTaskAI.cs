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
        
        private Worker _worker;
        private State _state;
        private TaskSystem<Task> _taskSystem;
        
        private float waitingTimer;
        private float waitingTimerMax = .2f;

        public void Setup(Worker worker, TaskSystem<Task> taskSystem)
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
            Task task = _taskSystem.RequestNextTask();

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
                    case Task.MoveToPositionTask moveToPositionTask:
                        ExecuteMoveToPositionTask(moveToPositionTask);
                        break;
                    case Task.VictoryTask victoryTask:
                        ExecuteVictoryTask(victoryTask);
                        break;
                    case Task.CleanUpTask cleanUpTask:
                        ExecuteCleanupTask(cleanUpTask);
                        break;
                    case Task.TakeWeaponToWeaponSlot takeWeaponToWeaponSlotTask:
                        ExecuteTakeWeaponToWeaponSlotTask(takeWeaponToWeaponSlotTask);
                        break;
                }
               
            }
        }

        private void ExecuteMoveToPositionTask(Task.MoveToPositionTask task)
        {
            EmesefeDebug.TextPopupMouse("ExecuteMoveToPositionTask");
            _worker.MoveTo(task.targetPosition, () =>
            {
                _state = State.WaitingForNextTask;
            });
        }
        
        private void ExecuteVictoryTask(Task.VictoryTask task)
        {
            EmesefeDebug.TextPopupMouse("ExecuteVictoryTask");
            _worker.PlayVictoryAnimation(() =>
            {
                _state = State.WaitingForNextTask;
            });
        }
        
        private void ExecuteCleanupTask(Task.CleanUpTask task)
        {
            EmesefeDebug.TextPopupMouse("ExecuteCleanupTask");
            _worker.MoveTo(task.targetPosition, () =>
            {
                _worker.PlayCleanUpAnimation(() =>
                {
                    task.onCleanupAction?.Invoke();
                    _state = State.WaitingForNextTask;
                });
            });
        }
        
        private void ExecuteTakeWeaponToWeaponSlotTask(Task.TakeWeaponToWeaponSlot task)
        {
            EmesefeDebug.TextPopupMouse("ExecuteTakeWeaponToWeaponSlotTask");
            _worker.MoveTo(task.weaponPosition,() =>
            {
                task.grabWeapon(this);
                _worker.MoveTo(task.weaponSlotPosition, () =>
                {
                    task.dropWeapon();
                    _state = State.WaitingForNextTask;
                });
            });
        }
    }    
}

