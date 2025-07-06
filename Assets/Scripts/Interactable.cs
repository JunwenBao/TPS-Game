using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private MeshRenderer mesh;
    [SerializeField] private Material highlightMaterial;
    private Material defaultMaterial;

    private void Start()
    {
        if(mesh == null) mesh = GetComponentInChildren<MeshRenderer>();

        defaultMaterial = mesh.material;
    }

    public void Interaction()
    {
        Debug.Log("INteract with");
    }

    public void HighlightActive(bool active)
    {
        if(active) mesh.material = highlightMaterial;
        else mesh.material = defaultMaterial;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerInteraction playerInteraction = other.GetComponent<PlayerInteraction>();

        if (playerInteraction == null) return;

        playerInteraction.interactables.Add(this);
        HighlightActive(true);

        playerInteraction.UpdateClosestInteractable();
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerInteraction playerInteraction = other.GetComponent<PlayerInteraction>();

        if (playerInteraction == null) return;

        playerInteraction.interactables.Remove(this);

        HighlightActive(false);

        playerInteraction.UpdateClosestInteractable();
    }
}