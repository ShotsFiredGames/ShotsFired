using UnityEngine;

public class MimicAppears : AddOn
{
    public PickUpLoacation[] validLocations;

    public override void StartAddOn()
    {
        PickUpLoacation validLocation = validLocations[Random.Range(0, validLocations.Length)];
        validLocation.photonView.RPC("RPC_ActivateMimic", PhotonTargets.All);
    }
}
