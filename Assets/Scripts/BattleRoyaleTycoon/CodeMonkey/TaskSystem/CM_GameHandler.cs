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
using CodeMonkey.Utils;

namespace TaskSystem {

    public class CM_GameHandler : MonoBehaviour {

        private TaskSystem taskSystem;

        private void Start() {
            taskSystem = new TaskSystem();

            CM_Worker worker = CM_Worker.Create(new Vector3(500, 500));
            CM_WorkerTaskAI workerTaskAI = worker.gameObject.AddComponent<CM_WorkerTaskAI>();
            workerTaskAI.Setup(worker, taskSystem);
            
            worker = CM_Worker.Create(new Vector3(550, 500));
            workerTaskAI = worker.gameObject.AddComponent<CM_WorkerTaskAI>();
            workerTaskAI.Setup(worker, taskSystem);

            /*FunctionTimer.Create(() => {
                CMDebug.TextPopupMouse("Task Added");
                TaskSystem.Task task = new TaskSystem.Task { targetPosition = new Vector3(550, 550) };
                taskSystem.AddTask(task);
            }, 5f);^*/
        }

        private void Update() {
            if (Input.GetMouseButtonDown(0)) {
                TaskSystem.Task task = new TaskSystem.Task { targetPosition = UtilsClass.GetMouseWorldPosition() };
                taskSystem.AddTask(task);
            }
        }

    }

}