//using System.Collections;
using ExitGames.Client.Photon;
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

    [Header("Event Toggles")]
    public List<TextToggle> events = new List<TextToggle>();

    [Header("AddOn Toggles")]
    public List<TextToggle> addOns = new List<TextToggle>();

    Hashtable roomProps;

    public const string PLAYERHEALTH = "hp";
    public const string RESPAWNTIME = "rt";

    // Use this for initialization
    void Start ()
    {
        roomProps = new Hashtable() { { PLAYERHEALTH, playerHealth.value }, { RESPAWNTIME, respawnTime.value} };
        PhotonNetwork.room.SetCustomProperties(roomProps);
        print(PhotonNetwork.room.CustomProperties + " in start");
	}
	
	public void UpdateValues()
    {
        roomProps[PLAYERHEALTH] = playerHealth.value;
        roomProps[RESPAWNTIME] = respawnTime.value;
        PhotonNetwork.room.SetCustomProperties(roomProps);
        print(PhotonNetwork.room.CustomProperties + " onclick");

        //do a for loop that loops through each of the addons/events
    }
}
