using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LJBT_LightSpotGoals : MonoBehaviour
{
    //When the player shoots a light beam raycast into these glowy spots, they win the puzzle. 

    bool lightShiningOnMe;
    Coroutine lastCoroutine = null;

    bool activatedBefore;
    private float activationTimer;

    LJBT_LightPuzzle lightPuzzle;
    public void SetUpLightPuzzle(LJBT_LightPuzzle thing)
    {
        //hold a reference to the light puzzle so that when im solved I can tell the light puzzle that im touching the light
        activatedBefore = false;
        lightPuzzle = thing;
    }
    public void ShineOnMe()
    {
        lightShiningOnMe = true;

        if (lastCoroutine != null)
            StopCoroutine(lastCoroutine);
        lastCoroutine = StartCoroutine(LostLight());
    }

    IEnumerator LostLight()
    {
        yield return null;
        yield return null;

        lightShiningOnMe = false;
    }

    private void Update()
    {
        if (!activatedBefore)//dont let the puzzle get solved twice. 
        {
            if (lightShiningOnMe)
            {
                activationTimer += Time.deltaTime;
                if (activationTimer >= 0.5f)
                {
                    activatedBefore = true;
                    lightPuzzle.CompletePuzzle();
                }
            }
            else
            {
                activationTimer = 0f;
            }
        }

    }
}
