using UnityEngine;

public class MimicAppears : AddOn
{
    public PickUpLoacation[] validLocations;
    PickUpLoacation validLocation;

    public override void StartAddOn()
    {
        validLocation = validLocations[Random.Range(0, validLocations.Length)];
        validLocation.photonView.RPC("RPC_ActivateMimic", PhotonTargets.All, true);
    }

    public override void EndAddOn()
    {
        validLocation.photonView.RPC("RPC_ActivateMimic", PhotonTargets.All, false);
    }
}
