using UnityEngine;
using Emesefe.Utilities;
using TaskSystem;

public class WeaponSlot
{
    private Transform _weaponTransform;
    private Transform _weaponSlotTransform;
    private bool _hasWeaponIncoming;
    
    public WeaponSlot(Transform weaponSlotTransform)
    {
        _weaponSlotTransform = weaponSlotTransform;
        SetWeaponTransform(null);
    }

    public bool IsEmpty()
    {
        return _weaponTransform == null && !_hasWeaponIncoming;
    }

    public void SetHasWeaponIncoming(bool hasWeaponIncoming)
    {
        _hasWeaponIncoming = hasWeaponIncoming;
        UpdateSprite();
    }

    public void SetWeaponTransform(Transform weaponTransform)
    {
        _weaponTransform = weaponTransform;
        SetHasWeaponIncoming(false);
        UpdateSprite();

        if (weaponTransform != null)
        {
            TransporterTask.TakeWeaponFromWeaponSlotToPosition newTask =
                new TransporterTask.TakeWeaponFromWeaponSlotToPosition
                {
                    weaponSlotPosition = GetPosition(),
                    targetPosition = GetPosition() + Vector3.right * 10,
                    grabWeapon = (workerTransporterTaskAI) =>
                    {
                        weaponTransform.SetParent(workerTransporterTaskAI.transform);
                        SetWeaponTransform(null);
                    },
                    dropWeapon = () =>
                    {
                        weaponTransform.SetParent(null);
                    }

                };
            GameManager.transporterTaskSystem.AddTask(newTask);
        }
    }

    public Vector3 GetPosition()
    {
        return _weaponSlotTransform.transform.position;
    }

    private void UpdateSprite()
    {
        _weaponSlotTransform.GetComponent<SpriteRenderer>().color = IsEmpty() ? Color.gray : Color.red;
    }
}
