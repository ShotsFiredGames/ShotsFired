using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPad : MonoBehaviour
{
    PlayerHealth[] healths;


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();

        }
    }

    private void OnTriggerStay(Collider other)
    {
        //if (health != null)
        //{

        //}
    }
}
