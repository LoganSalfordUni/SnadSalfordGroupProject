using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LJBT_DialogueManager : MonoBehaviour
{
    public static LJBT_DialogueManager instance;
    private void Awake()
    {
        instance = this;
    }

    [SerializeField] LJBT_CameraController playerCamera;
    [Tooltip("This is what appears / disappears when dialogue is or isnt active.")]
    [SerializeField] GameObject dialogueSpeechPanel;

    bool dialogueActive;//oaef remember to add a way to disable dialogue active
    float cooldownTimer;//there should be a minimum of 0.1 seconds between each click 

    [SerializeField] TextAsset textFile;

    LJBT_NPCDialogue currentNPC;//the current NPC the player is near. return this to void after the player leaves an NPC 
    
    private void Start()
    {
        LJBT_TextFileReader.instance.LoadFile(textFile);
    }

    private void Update()
    {
        if (dialogueActive)
        {
            if (Input.GetMouseButtonDown(0) && cooldownTimer <= 0f)
            {
                //When the player presses the mouse button, check if text is still being printed to the screen 
                if (LJBT_DialoguePrinter.instance.CheckIfPrintingAndSkip())// 
                    return;
                else
                {
                    //if text isnt being printed to the screen, go to the next line 
                    LJBT_TextFileReader.instance.HandleNextLine();
                    cooldownTimer = 0.05f;//stops the player from accidently double clicking and skipping through the whole game by putting a 0.05 second buffer.  
                }
            }
            else
                cooldownTimer -= Time.deltaTime;
        }
        else
        {
            //whilst dialogue isnt active, check if the player puts the mouse button down, and if theyre currently inside a NPCs trigger volume
            if (Input.GetMouseButtonDown(0))
            {
                if (currentNPC != null)
                {
                    BeginDialogue(currentNPC.startDialogue());
                }
            }
        }
    }

    void BeginDialogue(string sectionName)
    {
        dialogueSpeechPanel.SetActive(true);
        cooldownTimer = 0f;
        dialogueActive = true;
        LJBT_TextFileReader.instance.SectionJump(sectionName);
    }

    public void EndDialogue()
    {
        //call this when the player leaves the NPCs talking area, when a command to end dialogue is reached, or when the text file reader reaches a new section without skipping to it.

        dialogueActive = false;
        playerCamera.DisableQuestion();
        dialogueSpeechPanel.SetActive(false);
    }

    string questionSectionOne;
    string questionSectionTwo;
    public void AskQuestion(string sectionNameOne, string sectionNameTwo)
    {
        //called by the file reader when it reaches a question
        //send a message to the camera controller to start reading if the player is nodding or shaking their head
        playerCamera.EnableQuestion();

        questionSectionOne = sectionNameOne;
        questionSectionTwo = sectionNameTwo;
    }

    public void QuestionNod()
    {
        if (dialogueActive)
        {
            //called by the camera controller when the player nods whilst being asked a question
            //im checking if the dialogue is still active just to make certain the nod / shake is relevant. 
            LJBT_TextFileReader.instance.SectionJump(questionSectionOne);
        }
    }
    public void QuestionShake()
    {
        if (dialogueActive)
        {
            LJBT_TextFileReader.instance.SectionJump(questionSectionTwo);
        }
    }

    public void playerIsNearNPC(LJBT_NPCDialogue npcDialogue)
    {
        //when the player enters the NPCs trigger zone, it should feed this script who they are
        currentNPC = npcDialogue;
    }
    public void playerHasLeftNPC()
    {
        //return the nearby NPC value to null + end dialogue
        //IMPORTANT. when making any changes to this function, make sure to change its overload version too (see below) 
        currentNPC = null;
        EndDialogue();
    }
    public void playerHasLeftNPC(string sectionToJumpTo)
    {
        //return the nearby NPC value to null + end dialogue
        //allow the npc to call a new section name so they can complain about being left IF the dialogue is active
        currentNPC = null;
        EndDialogue();

        if (dialogueActive)
        {
            //if the NPC was talking to the player, jump to the section where they complain about being walked away from
            BeginDialogue(sectionToJumpTo);
        }
    }
}