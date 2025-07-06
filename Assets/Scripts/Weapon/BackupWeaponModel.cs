using UnityEngine;

public enum HangType
{
    LowBackhand,
    Backhand,
    SideHang
}

public class BackupWeaponModel : MonoBehaviour
{
    public WeaponType weaponType;

    [SerializeField] private HangType hangeType;

    public void Activate(bool activated) => gameObject.SetActive(activated);
    public bool HangTypeIs(HangType hangType) => this.hangeType == hangType;
}