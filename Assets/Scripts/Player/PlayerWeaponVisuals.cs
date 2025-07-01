using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerWeaponVisuals : MonoBehaviour
{
    private Player player;
    private Animator animator;

    [SerializeField] private WeaponModel[] weaponModels;
    [SerializeField] private BackupWeaponModel[] backupWeaponModels;

    [Header("Rig")]
    [SerializeField] private float rigWeightIncreaseRate;
    private Rig rig;
    private bool shouldIncrease_RigWeight;

    [Header("Left Hand IK")]
    [SerializeField] private float leftHandIKWeightIncreaseRate;
    [SerializeField] private TwoBoneIKConstraint leftHandIK;
    [SerializeField] private Transform leftHandIK_Target;
    private bool shouldIncrease_LeftHandIKWeight;

    private void Start()
    {
        player = GetComponent<Player>();
        animator = GetComponentInChildren<Animator>();
        rig = GetComponentInChildren<Rig>();
        weaponModels = GetComponentsInChildren<WeaponModel>(true);
        backupWeaponModels = GetComponentsInChildren<BackupWeaponModel>(true);
    }

    private void Update()
    {
        UpdateRigWeight();
        UpdateLeftHandIKWeight();
    }

    // ��ȡ����ģ�ͣ���Ҫ�õ�GunPoint��LeftHandPoint
    public WeaponModel CurrentWeaponModle()
    {
        WeaponModel weaponModel = null;
        WeaponType weaponType = player.weapon.CurrentWeapon().weaponType;

        for(int i = 0; i < weaponModels.Length; i++)
        {
            if (weaponModels[i].weaponType == weaponType)
            {
                weaponModel = weaponModels[i];
            }
        }

        return weaponModel;
    }

    public void PlayerFireAnimation() => animator.SetTrigger("Fire");

    // ����װ������
    public void PlayReloadAnimation()
    {
        float reloadSpeed = player.weapon.CurrentWeapon().reloadSpeed;

        animator.SetFloat("ReloadSpeed", reloadSpeed);
        animator.SetTrigger("Reload");

        ReduceRigWeight();
    }

    // �л�ָ������
    public void SwitchOnCurrentWeaponModel()
    {
        int animationLayerIndex = (int)CurrentWeaponModle().holdType;

        /* ���ȹر��������� */
        SwitchOffWeaponModels();

        /* ��ʾ�ڶ����� */
        SwitchOffBakcupWeaponModel();
        if(player.weapon.HasOnlyOoneWeapon() == false)
        {
            SwichOnBackupWeaponWeaponModel();
        }

        /* ������ȡ�������� */
        SwitchAnimationLayer(animationLayerIndex);
        CurrentWeaponModle().gameObject.SetActive(true);
        AttachLeftHand();
    }

    private void SwitchOffBakcupWeaponModel()
    {
        foreach(BackupWeaponModel backupModel in backupWeaponModels)
        {
            backupModel.gameObject.SetActive(false);
        }
    }

    public void SwichOnBackupWeaponWeaponModel()
    {
        WeaponType weaponType = player.weapon.BackupWeapon().weaponType;

        foreach(BackupWeaponModel backupModel in backupWeaponModels)
        {
            if(backupModel.weaponType == weaponType)
            {
                backupModel.gameObject.SetActive(true);
            }
        }
    }

    // �ر���������
    public void SwitchOffWeaponModels()
    {
        for(int i = 0; i < weaponModels.Length; i++)
        {
            weaponModels[i].gameObject.SetActive(false);
        }
    }

    // �л���ͬ�����Ķ�����
    private void SwitchAnimationLayer(int layerIndex)
    {
        for(int i = 0; i < animator.layerCount; i++)
        {
            animator.SetLayerWeight(i, 0);
        }

        animator.SetLayerWeight(layerIndex, 1);
    }

    // ������ȡ��������
    public void PlayWeaponEquipAnimation()
    {
        EquipType equipType = CurrentWeaponModle().equipAnimationType;

        float equipmentSpeed = player.weapon.CurrentWeapon().equipmentSpeed;

        leftHandIK.weight = 0;
        ReduceRigWeight();
        animator.SetTrigger("EquipWeapon");
        animator.SetFloat("EquipType", (float)equipType);
        animator.SetFloat("EquipSpeed", equipmentSpeed);
    }

    #region Animation Rigging Method

    // �л�����ʱ���������ڲ�ͬ������λ��
    private void AttachLeftHand()
    {
        Transform targetTransform = CurrentWeaponModle().holdPoint;

        leftHandIK_Target.localPosition = targetTransform.localPosition;
        leftHandIK_Target.localRotation = targetTransform.localRotation;
    }

    private void UpdateLeftHandIKWeight()
    {
        if (shouldIncrease_LeftHandIKWeight)
        {
            leftHandIK.weight += leftHandIKWeightIncreaseRate * Time.deltaTime;

            if (leftHandIK.weight >= 1) shouldIncrease_LeftHandIKWeight = false;
        }
    }

    private void UpdateRigWeight()
    {
        if (shouldIncrease_RigWeight)
        {
            rig.weight += rigWeightIncreaseRate * Time.deltaTime;

            if (rig.weight >= 1) shouldIncrease_RigWeight = false;
        }
    }

    private void ReduceRigWeight()
    {
        rig.weight = 0.15f;
    }

    public void MaximizeRigWeight() => shouldIncrease_RigWeight = true;
    public void MaximizeLeftHandWeight() => shouldIncrease_LeftHandIKWeight = true;

    #endregion
}