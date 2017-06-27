using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimicAppears : AddOn
{
    public PickUpLoacation[] validLocations;
    public GameObject mimic;

    public override void StartAddOn()
    {
        Debug.LogError("Mimic Spawned");
        PickUpLoacation validLocation = validLocations[Random.Range(0, validLocations.Length)];
        validLocation.SpawnSetPickup(mimic);
    }
}
