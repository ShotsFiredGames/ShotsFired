using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Juggernaut : MonoBehaviour
{
    bool isJuggernaut;
    public GameObject juggernautEffect;
    Coroutine juggernaut;

    // Use this for initialization
    void Start()
    {
        
    }
    
    public void ActivateJuggernaut()
    {
        if (!isJuggernaut)
        {
            juggernaut = StartCoroutine(JuggernautAbility());
        }
    }

    public void CancelJuggernaut()
    {
        if (juggernaut != null)
        {
            StopCoroutine(juggernaut);

            juggernautEffect.SetActive(false);
            isJuggernaut = false;
        }
    }

    IEnumerator JuggernautAbility()
    {
        isJuggernaut = true;
        juggernautEffect.SetActive(true);
        yield return new WaitForSeconds(GameCustomization.abilityDuration);
        juggernautEffect.SetActive(false);
        isJuggernaut = false;
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
                        health.CmdInstantDeath(transform.root.name, location);
                }
            }            
        }
    }
}
