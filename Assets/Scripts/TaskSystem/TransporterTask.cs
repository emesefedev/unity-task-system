using System;
using TaskSystem;
using UnityEngine;

namespace TaskSystem
{
    public class TransporterTask : TaskBase {
    
        // Worker moves to weapon position, grabs weapon, takes it to weapon slot position and drops weapon
        public class TakeWeaponFromWeaponSlotToPosition : TransporterTask
        {
            public Vector3 weaponSlotPosition;
            public Vector3 targetPosition;
            public Action<WorkerTransporterTaskAI> grabWeapon;
            public Action dropWeapon;
        }
            
    }
}