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
        aimLaser.material = new Material(Shader.Find("Sprites/Default")); // 或 Unlit/Color
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

    // 更新射击辅助线
    private void UpdateAimVisuals()
    {
        aimLaser.enabled = player.weapon.WeaponReady();
        if (!aimLaser.enabled) return;

        WeaponModel weaponModel = player.weaponVisuals.CurrentWeaponModle();
        weaponModel.transform.LookAt(aim);
        Transform gunPoint = weaponModel.gunPoint;
        gunPoint.LookAt(aim);

        Vector3 laserDirection = player.weapon.BulletDirection();
        float gunDistance = Mathf.Max(player.weapon.CurrentWeapon().gunDistance, 5f); // 最小距离容错
        float laserTipLength = 0.5f;

        Vector3 endPoint = gunPoint.position + laserDirection * gunDistance;

        // 如果命中目标，则使用命中点作为终点
        if (Physics.Raycast(gunPoint.position, laserDirection, out RaycastHit hit, gunDistance))
        {
            endPoint = hit.point;

            // 防止 laserDirection 太短导致消失
            if (Vector3.Distance(gunPoint.position, endPoint) < 0.1f)
            {
                endPoint = gunPoint.position + laserDirection * 0.5f;
            }

            laserTipLength = 0.2f;
        }

        // 设置 LineRenderer
        aimLaser.positionCount = 3;
        aimLaser.SetPosition(0, gunPoint.position);
        aimLaser.SetPosition(1, endPoint);
        aimLaser.SetPosition(2, endPoint + laserDirection * laserTipLength);
    }

    // 更新物体Aim的位置
    private void UpdateAimPosition()
    {
        /* 辅助瞄准：自动锁定Target中心 */
        Transform target = Target();

        if (target != null && isLockingToTarget)
        {
            if(target.GetComponent<Renderer>() != null)
            {
                aim.position = target.GetComponent<Renderer>().bounds.center;
            }
            else if(target.GetComponent<BoxCollider>() != null)
            {
                Debug.Log("瞄准Collider中心");
                aim.position = target.GetComponent<BoxCollider>().bounds.center;
            }
            else
            {
                aim.position = target.position;
            }
            return;
        }

        aim.position = GetMouseHitInfo().point;

        /* 未开启精准射击：射击位置Y轴的高度将被锁定 */
        if (!isAimingPrecisely)
        {
            aim.position = new Vector3(aim.position.x, transform.position.y + 3f, aim.position.z);
        }
    }

    // 获取被击中对象的Target脚本，用于辅助瞄准
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

    // 更新镜头位置
    private void UpdateCameraPosition()
    {
        /* 添加一个平滑插值，以缓慢平滑地移动相机位置 */
        cameraTarget.position = Vector3.Lerp(cameraTarget.position, DesieredCameraPosition(), cameraSensetivity * Time.deltaTime);
    }

    // 计算相机位置
    private Vector3 DesieredCameraPosition()
    {
        /* 当玩家向后移动，相机拉近；反之亦然 */
        float actualMaxCameraDistance = player.movement.moveInput.y < -.5f ? minCameraDistance : maxCameraDistance;

        Vector3 desiredCameraPosition = GetMouseHitInfo().point;
        Vector3 aimDirection = (desiredCameraPosition - transform.position).normalized;

        float distanceToDesiredPosition = Vector3.Distance(transform.position, desiredCameraPosition);
        float clampedDistance = Mathf.Clamp(distanceToDesiredPosition, minCameraDistance, actualMaxCameraDistance);

        desiredCameraPosition = transform.position + aimDirection * clampedDistance;
        desiredCameraPosition.y = transform.position.y + 3f;

        return desiredCameraPosition;
    }

    // 绑定Input System的按键
    private void AssignInputEvents()
    {
        controls = player.controls;

        /* 鼠标控制瞄准 */
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