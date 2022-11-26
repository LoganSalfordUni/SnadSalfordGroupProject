using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractables
{
    //Important note. anything with this interface MUST have the tag "Interactable"
    //btw NPCs arnt interactables. they have their own system, but I could convert them to interactables. it might work better that way...
    //Anyway. The Player Interaction detector works by checking if the player is looking at anything that inherits from this interface
    void Interact();
    //This is the text that will be displayed when the player can interact with the object. E.g. "Click to pick up"
    string PromptText();
}
