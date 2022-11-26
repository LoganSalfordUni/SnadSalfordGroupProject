using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LJBT_DialoguePrinter : MonoBehaviour
{
    public static LJBT_DialoguePrinter instance;
    private void Awake()
    {
        instance = this;
    }

    [SerializeField] Text nameField;
    [SerializeField] Text textField;


    [Tooltip(" The lower this value, the faster text renders. Set to 0 to instantly print text.")]
    [Range(0f, 0.25f)]
    [SerializeField] float printSpeed = 0.1f;
    //float timeBetweenCharacters = 0.05f;

    float timeToNextCharacter;
    bool printDialogue;
    string currentText;
    string targetText;
    int completedCharacters;


    public void SetSpeakerName(string name)
    {
        nameField.text = name;
    }

    public void DisplayText(string dialogue)
    {
        printDialogue = false;
        currentText = "";
        textField.text = "";


        if (printSpeed > 0f)
        {
            printDialogue = true;
            targetText = dialogue;
            completedCharacters = 0;

            timeToNextCharacter = printSpeed;
        }
        else
        {
            textField.text = dialogue;
            printDialogue = false;
        }

    }

    private void Update()
    {
        if (printDialogue)
        {
            if (completedCharacters < targetText.Length)
            {
                timeToNextCharacter -= Time.deltaTime;

                while (timeToNextCharacter < 0f && completedCharacters < targetText.Length)
                {
                    currentText = currentText + targetText[completedCharacters];
                    textField.text = currentText;

                    completedCharacters++;
                    timeToNextCharacter += printSpeed;
                }
            }
            else
                printDialogue = false;
        }
    }

    public bool CheckIfPrintingAndSkip()
    {
        //return true if the dialogue is still printing, and then skip printing. 
        //The conductor should call this to check if it can go to the next line of dialogue or not (it can go to the next line of dialogue if it returns falsE)

        if (printDialogue)
        {
            printDialogue = false;
            textField.text = targetText;

            return true;
        }
        else
            return false;
    }

    private void Start()
    {
        //DisplayText("May the bumper cars of fate let us bonk once again");//testing
        printDialogue = false;
    }


}
