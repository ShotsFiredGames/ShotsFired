using UnityEngine;

public class PickUp : MonoBehaviour
{
    public enum PickUpType
    {
        None,
        Gun,
        Ability
    };

    public enum GunType
    {
        None,
        MachineGun, 
        RocketLauncher,
		ShotGun,
        LaserGun,
        MineGun
    };

    public enum AbilityType
    {
        None,
        Juggernaut,
        Overcharged
    };

    [HideInInspector]
    public Animator pickUpLocation;

    PickUpManager pickUpManager;

    [HideInInspector]
    public PickUpType type = PickUpType.None;
    [HideInInspector]
    public GunType gun = GunType.None;
    [HideInInspector]
    public AbilityType ability = AbilityType.None;

    public void SetAnimator(Animator anim)
    {
        pickUpLocation = anim;
    }
}
