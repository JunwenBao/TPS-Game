using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Enemy_MeleeWeaponType { OneHand, Throw }

public class Enemy_Visuals : MonoBehaviour
{
    [Header("Weapon Model")]
    [SerializeField] private Enemy_WeaponModel[] weaponModels;
    [SerializeField] private Enemy_MeleeWeaponType weaponType;
    public GameObject currentWeaponModel {  get; private set; }

    [Header("Color")]
    [SerializeField] private Texture[] colorTextures;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;

    private void Start()
    {
        weaponModels = GetComponentsInChildren<Enemy_WeaponModel>(true);
        InvokeRepeating(nameof(SetupLook), 0, 1.5f);
    }

    // ���ý�ɫ���
    public void SetupLook()
    {
        SetupRandomColor();
        SetupRandomWeapon();
    }

    // ������������
    public void SetupWeaponType(Enemy_MeleeWeaponType type) => weaponType = type;

    // Ϊ��ɫ�����������
    private void SetupRandomWeapon()
    {
        foreach(var weaponModel in weaponModels)
        {
            weaponModel.gameObject.SetActive(false);
        }

        List<Enemy_WeaponModel> filteredWeaponModels = new List<Enemy_WeaponModel>();

        foreach(var weaponModel in weaponModels)
        {
            if(weaponModel.weaponType == weaponType)
            {
                filteredWeaponModels.Add(weaponModel);
            }
        }

        int randomIndex = Random.Range(0, filteredWeaponModels.Count);

        currentWeaponModel = filteredWeaponModels[randomIndex].gameObject;
        currentWeaponModel.SetActive(true);
    }

    // ����ɫ��Texture����Ϊ���Texture
    private void SetupRandomColor()
    {
        int randomIndex = Random.Range(0, colorTextures.Length);

        Material newMat = new Material(skinnedMeshRenderer.material);

        newMat.mainTexture = colorTextures[randomIndex];

        skinnedMeshRenderer.material = newMat;
    }
}