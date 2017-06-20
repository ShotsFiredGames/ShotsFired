using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagEndPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Flag"))
        {
            other.GetComponent<Flag>().FlagReturned();
        }
    }
}
