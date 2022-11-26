using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LJBT_DialogueCommandManager : MonoBehaviour
{
    public static LJBT_DialogueCommandManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Hold up, theres too many of me existing at once. kill me pls, my game object is called: " + this.gameObject.name);
        }
        instance = this;
    }

    //when making a command start the line with a £
    //put any variables after a , mark
    //(using commas because theyre probably the most replaceable punctuation mark. item names wont be able to use commas now but thats fine, semi colons and full stops exist. something like a colon is harder to replace in item names though. if for some reason we wanted an item name with a colon in it. also commas are pretty decent at seperating parts of a bit of text so its functional too. the | could be good too but ive chosen a comma now.)

    public void HandleCommand(string commandLine)
    {
        //figure out what command is being issued, and then signal a new function to deal with it. Afterwards, call the next line in the text file reader if appropriate
        //note: each command starts with a £ symbol
        commandLine = commandLine.ToLower();//makes sure that any capitalisation errors wont mess anything up
        
        //Spliting the line by commas here. making sure to trim each part of the argument too, just to get rid of spaces that would be easy to miss and could cause bugs. 
        string[] arguments = commandLine.Split(',');
        for (int x = 0; x < arguments.Length - 1; x++)
            arguments[x] = arguments[x].Trim();



        //Use ".Contains" not "==" for some lines. some command lines can be more than just the specific command. e.g. AddToInventory needs to also know what items are getting added to the inventory. 
        if (commandLine.Contains("%addtoinventory"))
        {
            //string[] arguments = commandLine.Split(',');
            AddToInventory(arguments);
            LJBT_TextFileReader.instance.HandleNextLine();
        }
        else if (commandLine.Contains("%removefrominventory"))
        {

        }
        else if (commandLine.Contains("%checkprogress") || commandLine.Contains("%checkinventory"))
        {
            CheckProgress(arguments);
        }
    }

    void AddToInventory(string[] parts)
    {
        //This command looks like: "£AddToInventory,item1,item2,item3" (you can add as many items as you want. put them after commas.) 
        //the line is split by ',' marks in the handle command function. so it should be in two or more parts. the first saying "£addtoinventory" and every following entry is an item
        //btw the naming convention for items is to put spaces in them. E.g. "Frog Plush" rather than "FrogPlush" (this might not be the smartest considering how commands DONT use spaces. so if this causes issues, go into the progress tracker, and update the check progress function to replace spaces with emptyness. 

        for (int x = 1; x <= parts.Length - 1; x++)//starting at 1 (position 2) because the first item in the array is £addtoinventory
        {
            Debug.Log("Adding the item: " + parts[x] + " to the players inventory");
            LJBT_ProgressTracker.instance.AddToInventory(parts[x]);
        }
    }

    void CheckProgress(string[] parts)
    {
        //This command looks like: "£CheckProgress,thing1,thing2...section1,section2" (you can add as many items as you want. put them after commas. the last two values MUST be section names) 
        //This command checks if the player has reached a certain progress point (it can only check one) and then it skips to a specific section. If all the things are found, it goes to the first section. otherwise it goes to the second. 
        //%CheckInventory also works but its literally the exact same command. there is no command to check exclusivly the inventory. 
        //parts 0 is £checkinventory, and parts 2 should be the specific items that are being checked

        //loop through every item and check if theyre in the players progress tracker. if ANY are missing, then all items are found is false. otherwise its true
        bool areAllTheItemsFound = true;
        for (int x = 1; x <= parts.Length - 3; x++)//the first item in the array is £checkprogress and we want to skip that. ALSO, we skip the final two positions (hence parts.length-3)e.g. if the length is 4 (checkprogress,item,section,section) then we only want position 1 (item). and 4-3 is 1. 
        {
            Debug.Log(parts[x]);
            if (!LJBT_ProgressTracker.instance.CheckProgress(parts[x]))
                areAllTheItemsFound = false;
        }

        if (areAllTheItemsFound)
        {
            string desiredSection = parts[parts.Length - 2];//This should be the second last item in the list of parts 
            if (!desiredSection.Contains('$'))
                desiredSection = "$" + desiredSection;
            LJBT_TextFileReader.instance.SectionJump(desiredSection);
        }
        else
        {
            string desiredSection = parts[parts.Length - 1];//this should be the last item in the list of parts. 
            if (!desiredSection.Contains('$'))
                desiredSection = "$" + desiredSection;
            LJBT_TextFileReader.instance.SectionJump(desiredSection);
        }
    }


}
