using UnityEngine;

public class Interactable : MonoBehaviour
{
    protected PlayerWeaponController weaponController;

    protected MeshRenderer mesh;

    protected Material defaultMaterial;
    [SerializeField] private Material highlightMaterial;

    private void Start()
    {
        if(mesh == null) mesh = GetComponentInChildren<MeshRenderer>();

        defaultMaterial = mesh.sharedMaterial;
    }

    protected void UpdateMeshAndMaterial(MeshRenderer newMesh)
    {
        mesh = newMesh;
        defaultMaterial = newMesh.sharedMaterial;
    }

    public virtual void Interaction()
    {
        Debug.Log("INteract with");
    }

    public void HighlightActive(bool active)
    {
        if(active) mesh.material = highlightMaterial;
        else mesh.material = defaultMaterial;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (weaponController == null)
        {
            weaponController = other.GetComponent<PlayerWeaponController>();
        }

        PlayerInteraction playerInteraction = other.GetComponent<PlayerInteraction>();

        if (playerInteraction == null) return;

        playerInteraction.GetInteractables().Add(this);
        playerInteraction.UpdateClosestInteractable();
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        PlayerInteraction playerInteraction = other.GetComponent<PlayerInteraction>();

        if (playerInteraction == null) return;

        playerInteraction.GetInteractables().Remove(this);
        playerInteraction.UpdateClosestInteractable();
    }
}