using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    private CinemachineCamera virtualCamera;
    private CinemachinePositionComposer transposer;

    [SerializeField] private float distanceChangeRate;
    private float targetCameraDistance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        virtualCamera = GetComponentInChildren<CinemachineCamera>();
        transposer = GetComponentInChildren<CinemachinePositionComposer>();
    }

    private void Update()
    {
        UpdateCameraDistance();
    }

    private void UpdateCameraDistance()
    {
        float currentDistance = transposer.CameraDistance;

        if (Mathf.Abs(targetCameraDistance - currentDistance) < 0.01f) return;

        transposer.CameraDistance = Mathf.Lerp(transposer.CameraDistance, targetCameraDistance, distanceChangeRate * Time.deltaTime);
    }

    public void ChangeCameraDistance(float distance) => targetCameraDistance = distance;
}