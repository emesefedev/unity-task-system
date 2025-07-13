using System;
using TaskSystem;
using UnityEngine;

public abstract class Task {
    // Worker moves to target position
    public class MoveToPositionTask : Task { public Vector3 targetPosition; }
    
    // Worker plays victory animation
    public class VictoryTask : Task { }

    // Worker moves to stain position and executes clean animation
    public class CleanUpTask : Task
    {
        public Vector3 targetPosition;
        public Action onCleanupAction;
    }
    
    // Worker moves to weapon position, grabs weapon, takes it to weapon slot position and drops weapon
    public class TakeWeaponToWeaponSlot : Task
    {
        public Vector3 weaponPosition;
        public Action<WorkerTaskAI> grabWeapon;
        public Vector3 weaponSlotPosition;
        public Action dropWeapon;
    }
            
}
