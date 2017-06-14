﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PickUp : NetworkBehaviour
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
        RocketLauncher
    };

    public enum AbilityType
    {
        None,
        SuperBoots,
        Juggernaut
    };

    PickUpManager pickUpManager;
    public AudioClip pickupSound;

    [HideInInspector]
    public PickUpType type = PickUpType.None;
    [HideInInspector]
    public GunType gun = GunType.None;
    [HideInInspector]
    public AbilityType ability = AbilityType.None;

    [ClientCallback]
    public void PlaySound(AudioSource source)
    {
        source.PlayOneShot(pickupSound);
    }
}
