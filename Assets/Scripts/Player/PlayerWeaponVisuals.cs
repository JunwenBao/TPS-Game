using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerWeaponVisuals : MonoBehaviour
{
    private Player player;
    private Animator animator;
    private bool isGrabbingWeapon;

    [SerializeField] private WeaponModel[] weaponModels;

    [Header("Rig")]
    [SerializeField] private float rigWeightIncreaseRate;
    private Rig rig;
    private bool shouldIncrease_RigWeight;

    [Header("Left Hand IK")]
    [SerializeField] private float leftHandIKWeightIncreaseRate;
    [SerializeField] private TwoBoneIKConstraint leftHandIK;
    [SerializeField] private Transform leftHand_Target;
    private bool shouldIncrease_LeftHandWeight;

    private void Start()
    {
        player = GetComponent<Player>();
        animator = GetComponentInChildren<Animator>();
        rig = GetComponentInChildren<Rig>();
        weaponModels = GetComponentsInChildren<WeaponModel>(true);
    }

    private void Update()
    {
        CheckWeaponSwitch();

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

    // ����װ������
    public void PlayReloadAnimation()
    {
        if (isGrabbingWeapon) return;

        animator.SetTrigger("Reload");
        ReduceRigWeight();
    }

    private void UpdateLeftHandIKWeight()
    {
        if (shouldIncrease_LeftHandWeight)
        {
            leftHandIK.weight += leftHandIKWeightIncreaseRate * Time.deltaTime;

            if (leftHandIK.weight >= 1) shouldIncrease_LeftHandWeight = false;
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
    public void MaximizeLeftHandWeight() => shouldIncrease_LeftHandWeight = true;

    // ���������л�����
    private void CheckWeaponSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchOn();
            SwitchAnimationLayer(1);
            PlayWeaponGrabAnimation(GrabType.SideGrab);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchOn();
            SwitchAnimationLayer(1);
            PlayWeaponGrabAnimation(GrabType.BackGrab);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchOn();
            SwitchAnimationLayer(2);
            PlayWeaponGrabAnimation(GrabType.BackGrab);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchOn();
            SwitchAnimationLayer(3);
            PlayWeaponGrabAnimation(GrabType.BackGrab);
        }
    }

    // �л�ָ������
    private void SwitchOn()
    {
        SwitchOffWeaponModels();

        CurrentWeaponModle().gameObject.SetActive(true);

        AttachLeftHand();
    }

    // �ر���������
    private void SwitchOffWeaponModels()
    {
        for(int i = 0; i < weaponModels.Length; i++)
        {
            weaponModels[i].gameObject.SetActive(false);
        }
    }

    // �л�����ʱ���������ڲ�ͬ������λ��
    private void AttachLeftHand()
    {
        Transform targetTransform = CurrentWeaponModle().holdPoint;

        leftHand_Target.localPosition = targetTransform.localPosition;
        leftHand_Target.localRotation = targetTransform.localRotation;
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
    private void PlayWeaponGrabAnimation(GrabType grabType)
    {
        leftHandIK.weight = 0;
        ReduceRigWeight();
        animator.SetFloat("WeaponGrabType", (float)grabType);
        animator.SetTrigger("WeaponGrab");
        SetBusyGrabbingWeaponTo(true);
    }

    public void SetBusyGrabbingWeaponTo(bool busy)
    {
        isGrabbingWeapon = busy;
        animator.SetBool("BusyGrabbingWeapon", isGrabbingWeapon);
    }
}