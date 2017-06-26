
public class ProjectileGun : Gun
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
