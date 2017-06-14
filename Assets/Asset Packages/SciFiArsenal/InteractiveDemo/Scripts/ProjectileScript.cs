using UnityEngine;
using UnityEngine.Networking;

public class ProjectileScript : NetworkBehaviour 
{
    public GameObject impactParticle;
    public GameObject projectileParticle;
    public GameObject[] trailParticles;
    [HideInInspector]
    public Vector3 impactNormal; //Used to rotate impactparticle.
	
	void Start () 
	{
        //projectileParticle = Instantiate(projectileParticle, transform.position, transform.rotation) as GameObject;
        //projectileParticle.transform.parent = transform;
	}

	void OnCollisionEnter (Collision hit) {

        if(!hit.transform.gameObject.layer.Equals("Default") && !hit.transform.tag.Equals("Player"))
        {
            print(hit.transform.name);
            impactParticle = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal)) as GameObject;

            if (hit.gameObject.tag == "Destructible") // Projectile will destroy objects tagged as Destructible
            {
                Destroy(hit.gameObject);
            }
            
            foreach (GameObject trail in trailParticles)
            {
                GameObject curTrail = transform.Find(projectileParticle.name + "/" + trail.name).gameObject;
                curTrail.transform.parent = null;
                Destroy(curTrail, 3f);
            }
            Destroy(projectileParticle, 3f);
            Destroy(impactParticle, 5f);
            Destroy(gameObject);
        }
    }
}