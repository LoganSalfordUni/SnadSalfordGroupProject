using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LJBT_GameManager : MonoBehaviour
{
    //felt weird making singletons for every little thing that I wanted to make them for, so this game manager should handle those. 
    //This should handle closing the game, opening the menu, playing sound, putting a text prompt on the screen when the player can interact with something. 

    public static LJBT_GameManager instance;
    private void Awake()
    {
        if (instance != null)
            Debug.Log("There are too many game managers in the scene, my game object is called " + this.gameObject.name);
        else
            instance = this;
    }

    [Tooltip("This is the text field that will appear when the player is near an interactable object or an NPC. Its NOT the text field that dialogue will be displayed too")]
    [SerializeField] GameObject interactionPromptTextParent;
    [SerializeField] Text interactionPromptText;

    [Tooltip("A reference to the player characters body. The NPCs in the scene use this to constantly stare at the player")]
    [SerializeField] Transform playerBody;
    public Transform GetPlayerBody()
    {
        return playerBody;
    }

    private void Start()
    {
        //theres a weird performance drop when the canvas renders for the first time. so im having it render right on load by quickly switching it on and off. 
        interactionPromptTextParent.SetActive(true);
        interactionPromptTextParent.SetActive(false);
    }

    public void DisplayInteractionPromptText(string displayText)
    {
        //This should be called by the NPCDialogue script, and the ObjectPickup Script
        interactionPromptTextParent.SetActive(true);
        interactionPromptText.text = displayText;
    }
    public void HideInteractionPromptText()
    {
        //This should be called by the NPCDialogue script, and the Interaction Detector Script  
        interactionPromptTextParent.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

}
