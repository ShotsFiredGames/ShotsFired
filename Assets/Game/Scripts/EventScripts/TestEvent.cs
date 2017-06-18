using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEvent : GameEvent
{
    public override void EndEvent()
    {
        Debug.LogError("Event Ended");
    }

    public override void StartEvent()
    {
        Debug.LogError("Event Started");
    }
}
