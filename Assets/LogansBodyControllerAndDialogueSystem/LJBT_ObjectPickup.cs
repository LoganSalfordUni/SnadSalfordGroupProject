using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LJBT_ObjectPickup : MonoBehaviour, IInteractables
{
    //attatch this to any object in the scene that the player can pick up. itll send a signal to the player progress tracker
    //remember the object MUST have the "interactable" tag. 

    [Tooltip("When the player is looking at this object, what will the prompt say. E.g. Pick up Sphere")]
    [SerializeField] private string promptText = "Pick up ";

    [Tooltip("Whats the name of this object to add to the players inventory. By default all names will be set to lower case")]
    [SerializeField] private string inventoryName;

    public void Interact()
    {
        Debug.Log("You have added me to the inventory");
        LJBT_ProgressTracker.instance.AddToInventory(inventoryName);
        Destroy(this.gameObject);
    }

    public string PromptText()
    {
        return promptText;
    }
}
