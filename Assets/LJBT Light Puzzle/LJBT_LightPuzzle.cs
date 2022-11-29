using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LJBT_LightPuzzle : MonoBehaviour
{
    //An instance of this script should exist be for every mirror room puzzle. 
    //This controls the origin point

    [SerializeField] LJBT_LightSpotGoals goal;
    [SerializeField, Tooltip("Where does the beam of light begin")] Transform lightOriginPoint;
    LayerMask lightRayMask;
    [SerializeField] LineRenderer beamRender;

    [Header("Reward")]
    [SerializeField, Tooltip("Where the item drop spawns")] Transform itemDropSpawnLocation;
    [SerializeField, Tooltip("What item drops when this is solved (should be a prefab)")] GameObject itemDrop;

    private void Start()
    {
        lightRayMask = LJBT_GameManager.instance.MirrorPuzzleLightLayerMask;
        goal.SetUpLightPuzzle(this);
    }

    private void Update()
    {
        RaycastHit hit;
        Physics.Raycast(lightOriginPoint.position, lightOriginPoint.transform.forward, out hit, 100f, lightRayMask);//this will get stuck within itself and the player probably. so skip past it somehow...cant use a layer mask because this is the layer mask
        //Debug.DrawRay(lightOriginPoint.position,lightOriginPoint.transform.forward * 100f, Color.green);
        beamRender.SetPosition(0, lightOriginPoint.position);
        beamRender.SetPosition(1, lightOriginPoint.position + (lightOriginPoint.transform.forward * hit.distance));

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.GetComponent<LJBT_LightMirrors>() != null)
            {
                hit.collider.gameObject.GetComponent<LJBT_LightMirrors>().ShineLight();
            }
            else if (hit.collider.gameObject.GetComponent<LJBT_LightSpotGoals>())
            {
                Debug.Log("somehow im already touching the goal...this almost definitely should not be happening");
                hit.collider.gameObject.GetComponent<LJBT_LightSpotGoals>().ShineOnMe();
            }
            else
            {
                Debug.Log(hit.collider.gameObject.name);
            }
        }

    }

    public void CompletePuzzle()
    {
        //called by the light spot goal when it touches light
        //drop or create an item somewhere. 
        Instantiate(itemDrop, itemDropSpawnLocation);

    }
}
