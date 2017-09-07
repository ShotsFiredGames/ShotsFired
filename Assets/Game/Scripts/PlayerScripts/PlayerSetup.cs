using UnityEngine;

public class PlayerSetup : Photon.MonoBehaviour
{
    public Behaviour[] componentsToDisable;
    public GameObject thirdPersonAnimations;
    public GameObject damageIndicators;
    GameObject collisionDetection;

    private void Start()
    {
        if (!photonView.isMine)
        {
            for (int i = 0; i < componentsToDisable.Length; i++)
            {
                componentsToDisable[i].enabled = false;
            }
        }
        else
        {
            thirdPersonAnimations.SetActive(false);
            damageIndicators.SetActive(true);
        }

        PhotonPlayer _photonPlayer = photonView.owner;
        string _playerID = _photonPlayer.NickName;
        PlayerManager _player = GetComponent<PlayerManager>();
        PlayerWrangler.RegisterPlayer(_playerID, _player);

        if (photonView.isMine)
        {
            collisionDetection = transform.Find("CollisionDetection").gameObject;
            foreach (Transform go in collisionDetection.GetComponentsInChildren<Transform>())
            {
                go.gameObject.layer = LayerMask.NameToLayer("Default");
            }
        }
        else
        {
            collisionDetection = transform.Find("CollisionDetection").gameObject;
            foreach (Transform go in collisionDetection.GetComponentsInChildren<Transform>())
                go.gameObject.layer = LayerMask.NameToLayer("Collision");
        }
    }
}