using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimicAppears : AddOn
{
    public PickUpLoacation[] validLocations;
    public GameObject mimic;

    public override void StartAddOn()
    {
        PickUpLoacation validLocation = validLocations[Random.Range(0, validLocations.Length)];
        validLocation.CmdActivateMimic();
    }
}
