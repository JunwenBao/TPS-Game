using UnityEngine;

public class WorldCrosshairController : MonoBehaviour
{
    [SerializeField] private RectTransform crosshairUI;
    [SerializeField] private Camera aimCamera;
    [SerializeField] private float maxDistance = 20f;
    [SerializeField] private float crosshairOffsetMultiplier = 0.01f;
    [SerializeField] private LayerMask raycastMask = ~0;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // ʼ�ս�׼��λ����������Ļ����
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 10f); // z=10 ��ʾ������� 10 ��
        Vector3 worldPos = aimCamera.ScreenToWorldPoint(screenCenter);
        crosshairUI.position = worldPos;

        // ʼ����׼�ǳ�������������ⷭת��
        crosshairUI.forward = aimCamera.transform.forward;
        /*
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
        Ray ray = aimCamera.ScreenPointToRay(screenCenter);

        Vector3 targetPos;
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, raycastMask))
        {
            targetPos = hit.point + hit.normal * crosshairOffsetMultiplier;
            crosshairUI.rotation = Quaternion.LookRotation(hit.normal);
            Debug.DrawLine(hit.point, hit.point + hit.normal * 2f, Color.green);
        }
        else
        {
            targetPos = ray.GetPoint(maxDistance);
            crosshairUI.forward = aimCamera.transform.forward;
        }

        crosshairUI.position = targetPos;
        */
    }
}