using UnityEngine;

public class HitscanGun : Gun
{
    void Start()
    {
        SetAmmo();
    }

    public override void Discard()
    {
        playerManager.CmdDisarm();
    }
}
