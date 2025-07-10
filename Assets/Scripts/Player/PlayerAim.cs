using System;
using TMPro;
using UnityEditor.Search;
using UnityEditor.ShaderGraph;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    private Player player;
    private PlayerControls controls;

    [Header("Aim Visual Laser")]
    [SerializeField] private LineRenderer aimLaser;

    [Header("Aim Control")]
    [SerializeField] private Transform aim;

    [SerializeField] private bool isAimingPrecisely;
    [SerializeField] private bool isLockingToTarget;

    [Header("Camera Control")]
    [SerializeField] private Transform cameraTarget;
    [Range(0.5f, 1f)]
    [SerializeField] private float minCameraDistance = 1.5f;
    [Range(1f, 10f)]
    [SerializeField] private float maxCameraDistance = 10f;
    [Range(3f, 5f)]
    [SerializeField] private float cameraSensetivity = 5f;

    [SerializeField] private LayerMask aimLayerMask;

    private Vector2 mouseInput;
    private RaycastHit lastKnownMouseHit;

    private void Start()
    {
        player = GetComponent<Player>();
        AssignInputEvents();

        aimLaser.positionCount = 3;
        aimLaser.startWidth = 0.05f;
        aimLaser.endWidth = 0.05f;
        aimLaser.material = new Material(Shader.Find("Sprites/Default")); // �� Unlit/Color
        aimLaser.useWorldSpace = true;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            isAimingPrecisely = !isAimingPrecisely;
        }

        if(Input.GetKeyDown(KeyCode.L))
        {
            isLockingToTarget = !isLockingToTarget;
        }

        UpdateAimVisuals();
        UpdateAimPosition();
        UpdateCameraPosition();
    }

    // �������������
    private void UpdateAimVisuals()
    {
        aimLaser.enabled = player.weapon.WeaponReady();
        if (!aimLaser.enabled) return;

        WeaponModel weaponModel = player.weaponVisuals.CurrentWeaponModle();
        weaponModel.transform.LookAt(aim);
        Transform gunPoint = weaponModel.gunPoint;
        gunPoint.LookAt(aim);

        Vector3 laserDirection = player.weapon.BulletDirection();
        float gunDistance = Mathf.Max(player.weapon.CurrentWeapon().gunDistance, 5f); // ��С�����ݴ�
        float laserTipLength = 0.5f;

        Vector3 endPoint = gunPoint.position + laserDirection * gunDistance;

        // �������Ŀ�꣬��ʹ�����е���Ϊ�յ�
        if (Physics.Raycast(gunPoint.position, laserDirection, out RaycastHit hit, gunDistance))
        {
            endPoint = hit.point;

            // ��ֹ laserDirection ̫�̵�����ʧ
            if (Vector3.Distance(gunPoint.position, endPoint) < 0.1f)
            {
                endPoint = gunPoint.position + laserDirection * 0.5f;
            }

            laserTipLength = 0.2f;
        }

        // ���� LineRenderer
        aimLaser.positionCount = 3;
        aimLaser.SetPosition(0, gunPoint.position);
        aimLaser.SetPosition(1, endPoint);
        aimLaser.SetPosition(2, endPoint + laserDirection * laserTipLength);
    }

    // ��������Aim��λ��
    private void UpdateAimPosition()
    {
        /* ������׼���Զ�����Target���� */
        Transform target = Target();

        if (target != null && isLockingToTarget)
        {
            if(target.GetComponent<Renderer>() != null)
            {
                aim.position = target.GetComponent<Renderer>().bounds.center;
            }
            else if(target.GetComponent<BoxCollider>() != null)
            {
                Debug.Log("��׼Collider����");
                aim.position = target.GetComponent<BoxCollider>().bounds.center;
            }
            else
            {
                aim.position = target.position;
            }
            return;
        }

        aim.position = GetMouseHitInfo().point;

        /* δ������׼��������λ��Y��ĸ߶Ƚ������� */
        if (!isAimingPrecisely)
        {
            aim.position = new Vector3(aim.position.x, transform.position.y + 3f, aim.position.z);
        }
    }

    // ��ȡ�����ж����Target�ű������ڸ�����׼
    public Transform Target()
    {
        Transform target = null;

        RaycastHit hit = GetMouseHitInfo();
        if (hit.collider != null && hit.transform.GetComponent<Target>() != null)
        {
            target = hit.transform;
        }

        return target;
    }

    // ���¾�ͷλ��
    private void UpdateCameraPosition()
    {
        /* ���һ��ƽ����ֵ���Ի���ƽ�����ƶ����λ�� */
        cameraTarget.position = Vector3.Lerp(cameraTarget.position, DesieredCameraPosition(), cameraSensetivity * Time.deltaTime);
    }

    // �������λ��
    private Vector3 DesieredCameraPosition()
    {
        /* ���������ƶ��������������֮��Ȼ */
        float actualMaxCameraDistance = player.movement.moveInput.y < -.5f ? minCameraDistance : maxCameraDistance;

        Vector3 desiredCameraPosition = GetMouseHitInfo().point;
        Vector3 aimDirection = (desiredCameraPosition - transform.position).normalized;

        float distanceToDesiredPosition = Vector3.Distance(transform.position, desiredCameraPosition);
        float clampedDistance = Mathf.Clamp(distanceToDesiredPosition, minCameraDistance, actualMaxCameraDistance);

        desiredCameraPosition = transform.position + aimDirection * clampedDistance;
        desiredCameraPosition.y = transform.position.y + 3f;

        return desiredCameraPosition;
    }

    // ��Input System�İ���
    private void AssignInputEvents()
    {
        controls = player.controls;

        /* ��������׼ */
        controls.Character.Aim.performed += context => mouseInput = context.ReadValue<Vector2>();
        controls.Character.Aim.canceled += context => mouseInput = Vector2.zero;
    }

    public Transform Aim() => aim;
    public bool CanAimPrecisly() => isAimingPrecisely;

    public RaycastHit GetMouseHitInfo()
    {
        Ray ray = Camera.main.ScreenPointToRay(mouseInput);

        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, aimLayerMask))
        {
            lastKnownMouseHit = hitInfo;
            return hitInfo;
        }

        return lastKnownMouseHit;
    }
}