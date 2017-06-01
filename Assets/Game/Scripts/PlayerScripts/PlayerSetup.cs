using UnityEngine.Networking;
using UnityEngine;

public class PlayerSetup : NetworkBehaviour
{
    public Behaviour[] componentsToDisable;
    public GameObject thirdPersonAnimations;

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
            thirdPersonAnimations.SetActive(false);

    }
}
