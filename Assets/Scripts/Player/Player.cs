using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform playerBody;
    public PlayerControls controls { get; private set; }
    public PlayerAim aim { get; private set; }
    public PlayerMovement movement { get; private set; }
    public PlayerWeaponController weapon { get; private set; }
    public PlayerWeaponVisuals weaponVisuals { get; private set; }
    public PlayerInteraction interaction { get; private set; }
    public Player_Health health { get; private set; }
    public Ragdoll ragdoll { get; private set; }

    public Animator animator { get; private set; }

    public bool isAiming;

    private void Awake()
    {
        controls = new PlayerControls();

        animator = GetComponentInChildren<Animator>();
        ragdoll = GetComponent<Ragdoll>();
        health = GetComponent<Player_Health>();
        aim = GetComponent<PlayerAim>();
        movement = GetComponent<PlayerMovement>();
        weapon = GetComponent<PlayerWeaponController>();
        weaponVisuals = GetComponent<PlayerWeaponVisuals>();
        interaction = GetComponent<PlayerInteraction>();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}