using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LJBT_ProgressTracker : MonoBehaviour
{
    public static LJBT_ProgressTracker instance;
    private void Awake()
    {
        instance = this;
    }

    //IMPORTANT NOTE: ITEMS AND PROGRESS MARKERS CAN NOT CONTAIN COMMAS IN THEM. (DUE TO HOW THE DIALOGUE COMMAND MANAGER WORKS)

    private List<string> inventory;
    private List<string> progressMarkers;

    private void Start()
    {
        inventory = new List<string>();
        progressMarkers = new List<string>();
    }

    public void AddToInventory(string item)
    {
        item = item.ToLower();//i set string values to lower case because its really easy to make a typo somewhere that causes problems :< 

        inventory.Add(item);
    }

    public void RemoveFromInventory(string item)
    {
        item = item.ToLower();

        inventory.Remove(item);
    }

    public void AddToProgressMarkers(string occurance)
    {
        occurance = occurance.ToLower();

        progressMarkers.Add(occurance);
    }

    public bool CheckProgress(string lookingFor)
    {
        lookingFor = lookingFor.ToLower();


        //checks the players progress markers and inventory. returns true if either list contains the object
        if (inventory.Contains(lookingFor) || progressMarkers.Contains(lookingFor))
            return true;
        else
            return false;

    }
}
