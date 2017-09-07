using UnityEngine;
using UnityEngine.UI;

public class PlayerListing : MonoBehaviour
{
    public PhotonPlayer PhotonPlayer { get; private set; }

    [SerializeField]
    Text _playerName;

    public void ApplyPhotonPlayer(PhotonPlayer photonPlayer)
    {
        PhotonPlayer = photonPlayer;
        _playerName.text = photonPlayer.NickName.Substring(0, photonPlayer.NickName.Length - 6);
    }
}
