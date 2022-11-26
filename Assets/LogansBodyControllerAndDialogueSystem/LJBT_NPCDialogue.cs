using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LJBT_NPCDialogue : MonoBehaviour
{
    //oaef add in code for the NPC to decide what section they should send to teh player

    [Tooltip("This is used to display the prompt text Talk with ...")]
    [SerializeField] private string characterName = "them";

    [System.Serializable]
    private struct SectionSignal
    {
        [Tooltip("The LJBT_ProgressTracker holds a list of strings representing the players progress. Its not caps sensitive")]
        public string[] requirements;
        [Tooltip("The section that is called if all the requirements are met")]
        public string sectionName;
    }
    [Tooltip("What sections this NPC calls to, and the requirements for them. Later entries have priority")]
    [SerializeField] private SectionSignal[] sectionSignals;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            LJBT_GameManager.instance.DisplayInteractionPromptText("Click to talk with " + characterName);
            LJBT_DialogueManager.instance.playerIsNearNPC(this);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            LJBT_GameManager.instance.HideInteractionPromptText();
            LJBT_DialogueManager.instance.playerHasLeftNPC("change this later");
        }
    }

    public string startDialogue()
    {
        //called by the dialogue manager when teh player presses the mouse button whilst naer the NPC but not within 
        //get a string of the section name that this NPC wants to send the player too 

        //return "Hello";
        LJBT_GameManager.instance.HideInteractionPromptText();

        int x = sectionSignals.Length - 1;
        int requirementsRequiredToMeet = 0;
        int requirementsMet = 0;
        while (x >= 0)
        {
            requirementsRequiredToMeet = sectionSignals[x].requirements.Length;
            requirementsMet = 0;
            foreach (string requirement in sectionSignals[x].requirements)
            {
                if (LJBT_ProgressTracker.instance.CheckProgress(requirement))
                {
                    requirementsMet += 1;
                }
            }
            if (requirementsMet == requirementsRequiredToMeet)
            {
                return sectionSignals[x].sectionName;
            }

            x--;
        }

        Debug.Log("ERROR. No sections that the player can access where found. Make sure the NPC has one section without any requirements");
        return null;
    }


    /*private Transform playerBody;
    private void Start()
    {
        playerBody = LJBT_GameManager.instance.GetPlayerBody();
    }

    private void Update()
    {
        //Vector3 targetRotation = new Vector3(0f, 0f, playerBody.rotation.z);
        //this.transform.LookAt(targetRotation);
        //this.transform.LookAt(playerBody);
        //this.transform.eulerAngles = new Vector3(90f, 180f, transform.rotation.eulerAngles.z);
    }*/
}
