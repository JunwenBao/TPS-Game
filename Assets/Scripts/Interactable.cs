using UnityEngine;

public class Interactable : MonoBehaviour
{
    private MeshRenderer mesh;
    private Material defaultMaterial;
    [SerializeField] private Material highlightMaterial;

    private void Start()
    {
        if(mesh == null) mesh = GetComponentInChildren<MeshRenderer>();

        defaultMaterial = mesh.material;
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