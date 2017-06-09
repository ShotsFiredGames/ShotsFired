using UnityEngine.Networking;
using UnityEngine;
using System.Collections;

public class PlayerSetup : NetworkBehaviour
{
    public Behaviour[] componentsToDisable;
    public GameObject thirdPersonAnimations;
    public GameObject damageIndicators;
    GameObject collisionDetection;

    private void Start()
    {
        if (!isLocalPlayer)
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

        if (isLocalPlayer)
        {
            collisionDetection = transform.Find("CollisionDetection").gameObject;
            foreach (Transform go in collisionDetection.GetComponentsInChildren<Transform>())
                go.gameObject.layer = LayerMask.NameToLayer("Default");
        }
        else
        {
            collisionDetection = transform.Find("CollisionDetection").gameObject;
            foreach (Transform go in collisionDetection.GetComponentsInChildren<Transform>())
                go.gameObject.layer = LayerMask.NameToLayer("Collision");
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        string _netID = GetComponent<NetworkIdentity>().netId.ToString();
        PlayerManager _player = GetComponent<PlayerManager>();
        PlayerWrangler.RegisterPlayer(_netID, _player);
    }
}
