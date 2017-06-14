using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Juggernaut : MonoBehaviour
{
    float duration;
    bool isJuggernaut;
    public GameObject juggernautEffect;

    // Use this for initialization
    void Start()
    {
        duration = GameCustomization.abilityDuration;
    }
    
    public void ActivateJuggernaut()
    {
        if (!isJuggernaut)
        {
            StartCoroutine(JuggernautAbility());
        }
    }

    IEnumerator JuggernautAbility()
    {
        isJuggernaut = true;
        juggernautEffect.SetActive(true);
        yield return new WaitForSeconds(GameCustomization.abilityDuration);
        juggernautEffect.SetActive(false);
        isJuggernaut = false;
        //Debug.LogError("Ability Done");
    }

    void OnTriggerEnter(Collider other)
    {
        if (isJuggernaut)
        {
            if (other.transform.root != this.transform.root)
            {
                if (other.tag.Equals("Collision"))
                {
                    CollisionDetection.CollisionFlag location = other.GetComponent<CollisionDetection>().collisionLocation;

                    PlayerHealth health = other.transform.root.GetComponent<PlayerHealth>();

                    if (!health.isPlayerDead())
                        health.CmdInstantDeath(location);
                }
            }            
        }
    }
}
