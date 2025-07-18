using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Weapon_UI : MonoBehaviour
{
    [SerializeField] private Image weapon_Image;
    [SerializeField] private TextMeshProUGUI weaponBullet_Text;

    [SerializeField] private Sprite[] weaponTypeSprites;

    private int currentBulletNum;
    private int reserveBulletNum;

    // 初始化武器UI
    public void InitialWeaponUI(WeaponType weaponType, int currentBulletNum, int reserveBulletNum)
    {
        weapon_Image.sprite = weaponTypeSprites[(int)weaponType];
        this.currentBulletNum = currentBulletNum;
        this.reserveBulletNum = reserveBulletNum;

        weaponBullet_Text.text = $"{this.currentBulletNum} / {this.reserveBulletNum}";
    }

    // 更新武器的子弹数量UI
    public void UpdateWeaponUI()
    {
        currentBulletNum--;
        weaponBullet_Text.text = $"{this.currentBulletNum} / {this.reserveBulletNum}";
    }
}