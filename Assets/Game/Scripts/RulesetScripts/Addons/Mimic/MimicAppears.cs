using UnityEngine;

public class MimicAppears : AddOn
{
    public PickUpLoacation[] validLocations;
    PickUpLoacation validLocation;

    public override void StartAddOn()
    {
        validLocation = validLocations[Random.Range(0, validLocations.Length)];
        validLocation.GetComponent<PickUpLoacation>().Local_ActivateMimic(true);
        validLocation.photonView.RPC("RPC_ActivateMimic", PhotonTargets.Others, true);
    }

    public override void EndAddOn()
    {
        validLocation.GetComponent<PickUpLoacation>().Local_ActivateMimic(false);
        validLocation.photonView.RPC("RPC_ActivateMimic", PhotonTargets.Others, false);
    }
}
