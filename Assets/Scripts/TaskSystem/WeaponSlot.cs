using UnityEngine;
using Emesefe.Utilities;

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

        FunctionTimer.Create(() =>
        {
            if (weaponTransform != null)
            {
                Object.Destroy(weaponTransform.gameObject);
                SetWeaponTransform(null);
            }
        }, 5f);
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
