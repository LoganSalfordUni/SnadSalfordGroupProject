using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LJBT_TextFileReader : MonoBehaviour
{
    public static LJBT_TextFileReader instance;
    private void Awake()
    {
        instance = this;
    }

    List<string> fileLines = new List<string>();

    //[SerializeField] TextAsset textAssetToUse;

    int currentLineNumber;

    string currentLine;
    //make this true whenever a new section is skipped too. Its false whilst the player is being asked a question or a command is happening
    bool allowContinuation;//if true, the handle next line method will work. if false, it wont. 
   

    public void HandleNextLine()
    {
        if (!allowContinuation)
            return;

        currentLineNumber++;

        currentLine = fileLines[currentLineNumber].Trim();
        Debug.Log(currentLine);

        //figure out what type of line it is 
        if (currentLine.StartsWith("$"))
        {
            //end dialogue because we've reached a new section organically 
            LJBT_DialogueManager.instance.EndDialogue();
        }
        else if (currentLine.StartsWith("\""))
        {
            //its a line of dialogue. feed it to the printer. 
            HandleDialogueLine();
        }
        else if (currentLine.StartsWith("!"))
        {
            HandleNameLine();
        }
        else if (currentLine.StartsWith("?"))
        {
            //its a question. send a signal to the dialogue manager. Note: Write questions like: nod,shake  
            HandleQuestionLine();

            //oaef the question line actually needs to have the dialogue with it OR I could, whilst writing a dialogue line. check if a question is next.
        }
        else if (currentLine.StartsWith("%"))
        {
            //its a command. send a signal to the dialogue command manager 
            //Note: Was using a £ symbol, but unity didnt seem to recognise it as a character for whatever reason. Instead going to try %
            Debug.Log("command line");
            LJBT_DialogueCommandManager.instance.HandleCommand(currentLine);
        }
        else if (currentLine.StartsWith(";"))
        {
            //use a line that starts with a semi colon to end the dialogue
            LJBT_DialogueManager.instance.EndDialogue();
        }

    }

    void HandleNameLine()
    {
        //send the name to the printer and then go to the next line automatically
        LJBT_DialoguePrinter.instance.SetSpeakerName(currentLine.Replace("!", ""));
        HandleNextLine();

    }
    void HandleDialogueLine()
    {
        //send the line of dialogue to the printer but DONT go to the next line, as we do that once the player inputs a button
        //check the next line to see if its a question. if so, handle the next line anyway
        LJBT_DialoguePrinter.instance.DisplayText(currentLine);


        //oaef this should really happen AFTER its finished printing though :/
        if (fileLines[currentLineNumber + 1].StartsWith("?"))
            HandleNextLine();
    }
    void HandleQuestionLine()
    {
        //split the line between the , symbols and trim each half. also remove the & symbol from the start. then send those to the dialogue manager
        allowContinuation = false;
        
        string q = currentLine.Replace("?", "");
        q = q.Trim();
        string[] qs = q.Split(',');
        LJBT_DialogueManager.instance.AskQuestion(qs[0].Trim(), qs[1].Trim());
    }


    public void LoadFile(TextAsset newTextAsset)
    {
        fileLines = new List<string>(newTextAsset.ToString().Split('\n'));
        currentLineNumber = 0;
    }
    public void SectionJump(string sectionName)
    {
        //scripts that call this: DialogueCommandManager during the £CheckProgress function. The DialogueManager when dialogue begins, and when the player nods or shakes their head to a question. 

        allowContinuation = true;
        sectionName = sectionName.ToLower().Trim('\n').Trim();//make sure theres no spaces and that its all in lower case 

        if (!sectionName.StartsWith("$"))
            sectionName = "$" + sectionName;

        int x = 0;
        foreach (string line in fileLines)
        {
            //if the line isn't a section, just skip past it.  

            if (line.StartsWith("$") && line.ToLower().Trim('\n').Trim() == sectionName)
            {
                currentLineNumber = x;
                HandleNextLine();
            }
            else
                x++;
        }
    }
}
