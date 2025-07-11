using UnityEngine;

public enum Enemy_Range_WeaponHoldType { Common, LowHold, HighHold };

public class Enemy_Range_WeaponModel : MonoBehaviour
{
    public Enemy_RangeWeaponType weaponType;
    public Enemy_Range_WeaponHoldType weaponHoldType;

}
