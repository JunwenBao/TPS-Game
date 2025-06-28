using UnityEngine;
using UnityEngine.UIElements;

public class WeaponVisualController : MonoBehaviour
{
    [SerializeField] private Transform[] gunTransforms;

    [SerializeField] private Transform pistol;
    [SerializeField] private Transform autoRifle;
    [SerializeField] private Transform shotgun;
    [SerializeField] private Transform sniper;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchOn(pistol);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchOn(autoRifle);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchOn(shotgun);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchOn(sniper);
        }
    }

    private void SwitchOn(Transform gunTransform)
    {
        SwitchOffGuns();
        gunTransform.gameObject.SetActive(true);
    }

    private void SwitchOffGuns()
    {
        for(int i = 0; i < gunTransforms.Length; i++)
        {
            gunTransforms[i].gameObject.SetActive(false);
        }
    }
}