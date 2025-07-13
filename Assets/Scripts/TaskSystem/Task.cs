using System;
using UnityEngine;

public abstract class Task {
    public class MoveToPositionTask : Task { public Vector3 targetPosition; }
    public class VictoryTask : Task { }

    public class CleanUpTask : Task
    {
        public Vector3 targetPosition;
        public Action onCleanupAction;
    }
            
}
