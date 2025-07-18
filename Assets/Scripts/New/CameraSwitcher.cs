using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private CinemachineCamera freelookCam;
    [SerializeField] private CinemachineCamera aimCam;
    [SerializeField] private CinemachineInputAxisController inputAxisController;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Player player;
    [SerializeField] private PlayerControls input;

    [Header("UI")]
    [SerializeField] private GameObject crosshairUI;
    [SerializeField] private GameObject weaponUI;

    private InputAction aimAction;
    private bool isAiming = false;
    private Transform yawTarget;
    private Transform pitchTarget;

    private AimCameraController aimCamController;

    private void Start()
    {
        aimCamController = aimCam.GetComponent<AimCameraController>();
        inputAxisController = freelookCam.GetComponent<CinemachineInputAxisController>();

        /* …Ë÷√Input System */
        input = new PlayerControls();
        input.Enable();
        aimAction = input.Character.Aim;
    }

    private void Update()
    {
        bool aimPressed = aimAction.IsPressed();
        player.isAiming = aimPressed;

        if (aimPressed && !isAiming)
        {
            EnterAimMode();
        }
        else if (!aimPressed && isAiming)
        {
            ExitAimMode();
        }
    }

    private void EnterAimMode()
    {
        crosshairUI.SetActive(true);
        weaponUI.SetActive(true);

        isAiming = true;

        SnapAimCameraToPlayerForward();

        aimCam.Priority = 20;
        freelookCam.Priority = 10;

        inputAxisController.enabled = false;
    }

    private void ExitAimMode()
    {
        crosshairUI.SetActive(false);
        weaponUI.SetActive(false);

        isAiming = false;

        SnapFreeLookBehindPlayer();

        aimCam.Priority = 10;
        freelookCam.Priority = 20;

        inputAxisController.enabled = true;
    }

    private void SnapFreeLookBehindPlayer()
    {
        CinemachineOrbitalFollow orbitalFollow = freelookCam.GetComponent<CinemachineOrbitalFollow>();
        Vector3 forward = aimCam.transform.forward;
        float angle = Mathf.Atan2(forward.x, forward.z) * Mathf.Rad2Deg;
        orbitalFollow.HorizontalAxis.Value = angle;
    }

    private void SnapAimCameraToPlayerForward()
    {
        aimCamController.SetYawPitchFromCameraForward(freelookCam.transform);
    }
}