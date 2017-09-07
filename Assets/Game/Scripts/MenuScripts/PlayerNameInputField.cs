using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(InputField))]
public class PlayerNameInputField : MonoBehaviour
{
    static string playerNamePrefKey = "PlayerName";

	void Start ()
    {
        string defaultName = "Player";
        InputField _inputField = GetComponent<InputField>();
        if(_inputField != null)
        {
            if(PlayerPrefs.HasKey(playerNamePrefKey))
            {
                defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                _inputField.text = defaultName.Substring(0, defaultName.Length - 6);
            }
        }

        PhotonNetwork.playerName = defaultName;
	}
	
	public void SetPlayerName(string value)
    {
        string addedNum = "#" + Random.Range(10000, 99999);
        PhotonNetwork.playerName = value + addedNum;
        PlayerPrefs.SetString(playerNamePrefKey, value + addedNum);
    }
}
