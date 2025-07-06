using UnityEngine;

public class Pickup_Weapon : Interactable
{
    private PlayerWeaponController weaponController;
    [SerializeField] private Weapon_Data weaponData;

    [SerializeField] private BackupWeaponModel[] models;

    private void Start()
    {
        UpdateGameobject();
    }

    [ContextMenu("Update Item Model")]
    public void UpdateGameobject()
    {
        gameObject.name = "Pickup_Weapon - " + weaponData.weaponType.ToString();
        UpdateItemModel();
    }

    public void UpdateItemModel()
    {
        foreach(BackupWeaponModel model in models)
        {
            model.gameObject.SetActive(false);

            if(model.weaponType == weaponData.weaponType) 
            {
                model.gameObject.SetActive(true);
                UpdateMeshAndMaterial(model.GetComponentInChildren<MeshRenderer>());
            }
        }
    }

    public override void Interaction()
    {
        Debug.Log("¼ñÆðÎäÆ÷");
        weaponController.PickupWeapon(weaponData);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if(weaponController == null)
        {
            weaponController = other.GetComponent<PlayerWeaponController>();
        }
    }
}