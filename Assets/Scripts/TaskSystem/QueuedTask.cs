using System;
using UnityEngine;

public class QueuedTask
{

    private Func<Task> _tryGetTaskFunc;
    
    public QueuedTask(Func<Task> tryGetTaskFunc)
    {
        _tryGetTaskFunc = tryGetTaskFunc;
    }

    public Task TryDequeueTask()
    {
        return _tryGetTaskFunc();
    }
}
