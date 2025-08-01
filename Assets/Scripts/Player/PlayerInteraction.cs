using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private List<Interactable> interactables = new List<Interactable>();
    private Interactable closestInteractable;

    private void Start()
    {
        Player player = GetComponent<Player>();
        player.controls.Character.Interaction.performed += context => InteractWithClosest();
    }

    private void InteractWithClosest()
    {
        closestInteractable?.Interaction();
        interactables.Remove(closestInteractable);

        UpdateClosestInteractable();
    }

    public void UpdateClosestInteractable()
    {
        closestInteractable?.HighlightActive(false);
        closestInteractable = null;

        float closeDistance = float.MaxValue;

        foreach(Interactable interactable in interactables)
        {
            float distance = Vector3.Distance(transform.position, interactable.transform.position);

            if(distance < closeDistance)
            {
                closeDistance = distance;
                closestInteractable = interactable;
            }
        }

        closestInteractable?.HighlightActive(true);
    }

    public List<Interactable> GetInteractables() => interactables;
}