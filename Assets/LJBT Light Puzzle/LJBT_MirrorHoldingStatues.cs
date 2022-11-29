using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LJBT_MirrorHoldingStatues : MonoBehaviour
{
    [SerializeField, Tooltip("This is where the mirror will be placed when placed on teh statue")]
    private Vector3 mirrorPlacementLocation;

    public Vector3 PlaceMirror()
    {
        return mirrorPlacementLocation;
    }
}
