using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Juggernaut : MonoBehaviour
{
    float duration;
    bool isJuggernaut;

    // Use this for initialization
    void Start()
    {
        duration = GameCustomization.abilityDuration;
    }

    public void ActivateJuggernaut()
    {
        if (!isJuggernaut)
        {
            Debug.LogError("Activate Ability");
            StartCoroutine(JuggernautAbility());
        }
    }

    IEnumerator JuggernautAbility()
    {
        isJuggernaut = true;
        yield return new WaitForSeconds(GameCustomization.abilityDuration);
        isJuggernaut = false;
        Debug.LogError("Ability Done");
    }

    void OnTriggerEnter(Collider other)
    {
        if (isJuggernaut)
        {
            if (other.transform.root != this.transform.root)
            {
                if (other.tag.Equals("Collision"))
                {
                    Debug.LogError("You've been juggered!");
                    CollisionDetection.CollisionFlag location = other.GetComponent<CollisionDetection>().collisionLocation;
                    other.transform.root.GetComponent<PlayerHealth>().CmdInstantDeath(location);
                }
            }            
        }
    }
}
