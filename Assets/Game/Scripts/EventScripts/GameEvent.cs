using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class GameEvent : NetworkBehaviour {

    public float duration;
    public string nameEvent;

    public abstract void StartEvent();
    public abstract void EndEvent();
}
