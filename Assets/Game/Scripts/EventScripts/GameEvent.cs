using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Networking;

public abstract class GameEvent : NetworkBehaviour {

    public float duration;
    public string nameEvent;
    public List<AddOn> allAddOns;
    private AddOn _addOn;

    internal List<AddOn> Values
    {
        get { return this.allAddOns; }
    }

    [TypeConverter(typeof(ValueTypeConverter))]
    public AddOn Value
    {
        get { return this._addOn; }
        set { this._addOn = value; }
    }

    public abstract void StartEvent();
    public abstract void EndEvent();

    [HideInInspector]
    public Coroutine gameEventDur;

    /*void StartEvent()
     * // turn on all of what you need for the event
     * gameEventDur = StartCorountine(EventDuration());
     * */

    public void ResetEvent()
    {
        if(gameEventDur != null)
        StopCoroutine(gameEventDur);

        gameEventDur = StartCoroutine(EventDuration());
    }

    public IEnumerator EventDuration()
    {
        yield return new WaitForSeconds(duration);
        EndEvent();
    }

}

class ValueTypeConverter : TypeConverter
{
    public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    {
        List<AddOn> myValues = (context.Instance as GameEvent).Values;
        return new StandardValuesCollection(myValues);
    }

    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    {
        return true;
    }

    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    {
        return true;
    }
}