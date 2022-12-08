using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LJBT_LightMirrors : MonoBehaviour, IInteractables
{
    //mirrors can be picked up
    //mirrors should be attatchable to statues. 
    //when hit by a beam of light. They raycast forward. (Note: the hitbox on the mirror should be larger than the players hitbox so the beam of light can reflect of the mirror even if its hitting the players back)
    //this code is rly messy. trying to get the whole mirror puzzle done in one day


    //get a reference to the player bodys position, or maybe signal it to hold this. will need a new function for "holding"
    bool beingHeld;
    [SerializeField, Tooltip("Make sure this is outside the players and mirrors hitbox")] Transform raycastOriginPoint;
    [SerializeField] LineRenderer beamRender;

    private LayerMask lightRayMask;


    //note: found bug where player cant drop mirror. unsure what caused it. itll be that beingHeld = false whilst the player is actually holding it I bet, but im unsure how that happened. 

    void Start()
    {
        beingHeld = false;
        lightRayMask = LJBT_GameManager.instance.MirrorPuzzleLightLayerMask;
    }
    
    public void Interact()
    {
        this.transform.parent = LJBT_GameManager.instance.PlayerPickUpObject();
        this.transform.localPosition = new Vector3(0f, 0f, 0f);
        this.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
        
        StartCoroutine(WaitFrame());//wait a frame so the game wont immediatly drop the item (in update it checks if the mouse is being clicked. so the same frame you pick the item up, you drop it)
    }

    IEnumerator WaitFrame()
    {
        yield return null;
        beingHeld = true;
    }

    public string PromptText()
    {
        return "Hold Mirror";
    }

    void Update()
    {
        if (beingHeld)
        {
            //whilst being held is true, the player can put the mirror down by pressing the mouse button again
            if (Input.GetMouseButtonDown(0))
            {
                //drop the mirror onto the ground or slot it into a statue
                RaycastHit hit;
                if(Physics.Raycast(raycastOriginPoint.position, this.transform.forward, out hit, 1f))
                {
                    if (hit.collider.gameObject.GetComponent<LJBT_MirrorHoldingStatues>() != null)
                    {
                        this.transform.position = hit.collider.gameObject.GetComponent<LJBT_MirrorHoldingStatues>().PlaceMirror();
                    }
                }
                else
                {
                    LJBT_GameManager.instance.PlayerDropObject();
                    this.transform.parent = null;
                }
                beingHeld = false;
            }
        }

    }

    Coroutine lastCoroutine = null;
    bool HaveIAlreadyFiredThisFrame = false;
    public void ShineLight()
    {
        //this can cause an overflow problem if a mirror shoots light at another mirror that shoots light at it...
        //i dont have the time to fix this.  im just scrapping the 2nd puzzle room where this is likely to happen. technically its possible in the first room, but probably wont happen. 
        /*if (HaveIAlreadyFiredThisFrame)
            return;
        
        HaveIAlreadyFiredThisFrame = true;
        FrameEndReset();*///This solution didnt work. the wait for end of frame coroutine doesnt do what i want it to do. 

        //this should happen once every frame, its called ffrom the update loop of LJBT light puzzle (or from other instances of this script)
        //each frame that light is being shot at this, shoot light forward
        //Debug.Log("Light is being shot at me");
        RaycastHit hit;
        Physics.Raycast(raycastOriginPoint.position, this.transform.forward, out hit, 100f, lightRayMask);//this will get stuck within itself and the player probably. so skip past it somehow...cant use a layer mask because this is the layer mask
        //Debug.DrawRay(raycastOriginPoint.position, this.transform.forward * 100f, Color.green);
        beamRender.enabled = true;
        beamRender.SetPosition(0, raycastOriginPoint.position);
        beamRender.SetPosition(1, raycastOriginPoint.position + (this.transform.forward * hit.distance));
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.GetComponent<LJBT_LightMirrors>() != null)
            {
                hit.collider.gameObject.GetComponent<LJBT_LightMirrors>().ShineLight();
            }
            else if (hit.collider.gameObject.GetComponent<LJBT_LightSpotGoals>())
            {
                //Debug.Log("Touching the light spot goal");
                hit.collider.gameObject.GetComponent<LJBT_LightSpotGoals>().ShineOnMe();
            }
        }

        if (lastCoroutine != null)
            StopCoroutine(lastCoroutine);
        //StopAllCoroutines();
        lastCoroutine = StartCoroutine(DeleteBeam());
    }

    /*IEnumerator FrameEndReset()
    {
        yield return new WaitForEndOfFrame();
        HaveIAlreadyFiredThisFrame = false;
    }*/
    IEnumerator DeleteBeam()
    {
        //if the light isnt reflecting from this mirror, delete the beam. To do this, im gonna have this coroutine stopped and then called every frame that the mirror is shooting light. if its not stopped for two frames, delete it
        
        //wait two frames. if this co-routine hasnt been blocked. delete the beam renderer. 
        yield return null;
        yield return null;
        beamRender.enabled = false;
        lastCoroutine = null;
    }
}
