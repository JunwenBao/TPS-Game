public enum WeaponType
{
    Pistol,
    AutoRifle,
    Shotgun,
    Sniper
}

[System.Serializable]
public class Weapon
{
    public WeaponType weaponType;
    public int ammo;
    public int maxAmmo;

    public bool CanShoot()
    {
        return HaveEnoughBullets();
    }

    // ¼ì²éÎäÆ÷ÊÇ·ñÓĞ×ã¹»¶àµÄµ¯Ò©
    private bool HaveEnoughBullets()
    {
        if (ammo > 0)
        {
            ammo--;
            return true;
        }

        return false;
    }
}