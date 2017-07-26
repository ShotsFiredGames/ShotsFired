using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizationToServer : MonoBehaviour
{
    public Button customizeMatchButton;

    [Header("Stats To Transfer")]
    public Slider playerHealth;
    public Slider respawnTime;
    public Slider playerSpeed;
    public Slider abilityDuration;
    public Slider eventOccurence;
    public Slider gameLength;
    public Slider pointsToWin;
    public Slider pointsPerKill;
    public Toggle unlimitedAmmo;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
