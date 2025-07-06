using UnityEngine;

public class Interactable : MonoBehaviour
{
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
        PlayerInteraction playerInteraction = other.GetComponent<PlayerInteraction>();

        if (playerInteraction == null) return;

        HighlightActive(true);

        playerInteraction.interactables.Add(this);
        playerInteraction.UpdateClosestInteractable();
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        PlayerInteraction playerInteraction = other.GetComponent<PlayerInteraction>();

        if (playerInteraction == null) return;

        HighlightActive(false);

        playerInteraction.interactables.Remove(this);
        playerInteraction.UpdateClosestInteractable();
    }
}