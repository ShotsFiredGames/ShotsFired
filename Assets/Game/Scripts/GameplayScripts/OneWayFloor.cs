using UnityEngine;
using System.Collections;

public class OneWayFloor : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player" && !other.GetComponent<PlayerScript>().canPass)
        {
            this.GetComponent<BoxCollider>().isTrigger = false;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player" && !other.GetComponent<PlayerScript>().canPass)
        {
            this.GetComponent<BoxCollider>().isTrigger = true;
        }

    }
}
