using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    public virtual void PlayerEnters(GameObject player)
    { }

    public virtual void PlayerStays(GameObject player)
    { }

    public virtual void PlayerLeaves(GameObject player)
    { }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            PlayerEnters(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            PlayerStays(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            PlayerLeaves(other.gameObject);
        }
    }


}
