using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameEvent : MonoBehaviour {

    public float duration;
    public string nameEvent;

    public abstract void StartEvent();
    public abstract void EndEvent();
}
