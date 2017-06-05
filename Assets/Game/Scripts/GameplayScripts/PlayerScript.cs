using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
    public bool canPass { get; set; }

	// Use this for initialization
	void Start () {
        canPass = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Jumppad1")
        {
            StartCoroutine(Onewayfloor(1.5f));
        }
    }
    IEnumerator Onewayfloor(float _delay)
    {
        canPass = true;
        yield return new WaitForSeconds(_delay);
        canPass = false;
        
    }
}
