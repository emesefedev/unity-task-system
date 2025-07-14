using System;
using UnityEngine;

public class QueuedTask<TaskType> where TaskType : TaskSystem.TaskBase
{

    private Func<TaskType> _tryGetTaskFunc;
    
    public QueuedTask(Func<TaskType> tryGetTaskFunc)
    {
        _tryGetTaskFunc = tryGetTaskFunc;
    }

    public TaskType TryDequeueTask()
    {
        return _tryGetTaskFunc();
    }
}
