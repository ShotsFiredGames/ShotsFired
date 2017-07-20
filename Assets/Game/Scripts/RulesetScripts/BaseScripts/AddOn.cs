using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class AddOn : NetworkBehaviour
{
    public string addOnName;

    public abstract void StartAddOn();
}
