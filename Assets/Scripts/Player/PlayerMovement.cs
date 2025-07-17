using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private bool shouldFaceMoveDirection = false;
    [SerializeField] private Transform yawTarget;
    private Vector3 velocity;

    private Player player;
    private Animator animator;

    private PlayerControls controls;
    private CharacterController controller;

    [Header("Movement Info")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float turnSpeed;

    private float speed; // 角色当前移动速度
    private float verticalVelocity;

    public Vector2 moveInput {  get; private set; }
    private Vector3 movementDirection;

    private bool isRunning;

    private void Start()
    {
        player = GetComponent<Player>();
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        speed = walkSpeed;

        AssignInputEvents();
    }

    private void Update()
    {
        UpdateMovement();
        //ApplyRotation();
        AnimatorControllers();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        bool isMoving = moveInput != Vector2.zero;
        animator.SetBool("IsWalking", isMoving);

        if (context.performed && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    // 控制角色动画：使用点积
    private void AnimatorControllers()
    {
        float xVelocity = Vector3.Dot(movementDirection.normalized, transform.right);
        float zVelocity = Vector3.Dot(movementDirection.normalized, transform.forward);

        animator.SetFloat("xVelocity", xVelocity, 0.1f, Time.deltaTime);
        animator.SetFloat("zVelocity", zVelocity, 0.1f, Time.deltaTime);

        bool playerRunAnimation = isRunning && movementDirection.magnitude > 0;
        animator.SetBool("IsRunning", playerRunAnimation);
    }

    // 控制角色朝向鼠标方向
    private void ApplyRotation()
    {
        Vector3 lookingDirection = player.aim.GetMouseHitInfo().point - transform.position;
        lookingDirection.y = 0f;
        lookingDirection.Normalize(); // 获取一个没有长度的向量

        // 平滑旋转
        Quaternion desireRotation = Quaternion.LookRotation(lookingDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, desireRotation, turnSpeed * Time.deltaTime);
    }

    #region move and rotation control

    // 更新角色移动
    private void UpdateMovement()
    {
        Vector3 moveDirection = Vector3.zero;

        /* 计算移动方向 */
        if (player.isAiming)
        {
            Vector3 forward = transform.forward;
            Vector3 right = transform.right;

            forward.y = 0;
            right.y = 0;

            forward.Normalize();
            right.Normalize();
            moveDirection = forward * moveInput.y + right * moveInput.x;
        }
        else
        {
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;

            forward.y = 0;
            right.y = 0;

            forward.Normalize();
            right.Normalize();
            moveDirection = forward * moveInput.y + right * moveInput.x;
        }

        controller.Move(moveDirection * speed * Time.deltaTime);

        /* 计算瞄准方向 */
        if (player.isAiming)
        {
            Vector3 lookDirection = yawTarget.forward;
            lookDirection.y = 0;

            if (lookDirection.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }
        else if (shouldFaceMoveDirection && moveDirection.sqrMagnitude > 0.001f)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 10f * Time.deltaTime);
        }
    }

    #endregion

    // 控制角色重力
    private void ApplyGravity()
    {
        if(controller.isGrounded == false)
        {
            verticalVelocity -= 9.81f * Time.deltaTime;
            movementDirection.y = verticalVelocity;
        }
        else
        {
            verticalVelocity = -0.5f;
        }
    }

    // 分配输入系统的各项参数
    private void AssignInputEvents()
    {
        controls = player.controls;

        controls.Character.Movement.performed += context => moveInput = context.ReadValue<Vector2>();
        controls.Character.Movement.canceled += context => moveInput = Vector2.zero;

        controls.Character.Run.performed += context =>
        {
            speed = runSpeed;
            isRunning = true;
        };
        controls.Character.Run.canceled += context =>
        {
            speed = walkSpeed;
            isRunning = false;
        };
    }
}