using UnityEngine;
using UnityEngine.SceneManagement;

public class DyslexiaToggle : MonoBehaviour
{
    /* This script enables/Disables Dyslexia Font being used by swapping scenes i did this
    instead of tabs since I think I would have had a struggle using tabs instead of a full scene swap
    it also keeps the scenes nice and clean without adding loads of new objects into one scene and
    adding un-needed clatter! ~ Sam
     */

    public void DyslexiaEnabler()
    {
        SceneManager.LoadScene("DyslexiaON");
        Debug.Log("Successfully Turned on Dyslexia Mode!");
        // When turned on should switch to the same menu but with the Dyslexia Font changed over in the menu
    }

    public void DyslexiaDisabler()
    {
        SceneManager.LoadScene("Menu");
        Debug.Log("Successfully turned off Dyslexia Mode!");
        // When turned off will revert to default setting given when loading in the menu originally!
    }
    
}
