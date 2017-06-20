using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    CaptureTheFlag captureTheFlag;
    GameObject carrier;

    Coroutine resetTimer;

    private void Start()
    {
        captureTheFlag = GameObject.Find("CaptureTheFlag").GetComponent<CaptureTheFlag>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Player"))
        {
            if (resetTimer != null)
                StopCoroutine(resetTimer);

            carrier = other.gameObject;
            carrier.GetComponent<PlayerManager>().hasFlag = true;
            gameObject.transform.SetParent(carrier.transform);
            gameObject.transform.position = carrier.transform.position + new Vector3(0, carrier.transform.localScale.y, 0);
        }
    }

    public void FlagReturned()
    {
        transform.SetParent(null);
        captureTheFlag.FlagReturned(carrier.name);
    }

    public void FlagDropped()
    {
        transform.SetParent(null);
        resetTimer = StartCoroutine(captureTheFlag.ResetTimer());
    }
}
